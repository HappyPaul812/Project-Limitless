using System;
using ProjectLimitless.Audio;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace ProjectLimitless.UI
{
    /// <summary>연속 대화 한 쪽의 화자 ID·표시 이름·본문을 함께 보관합니다.</summary>
    public sealed class DialogueLine
    {
        public const string PlayerSpeakerId = "player";
        public const string DirectionPrefix = "<지문>";

        public DialogueLine(string speakerId, string speakerName, string message, string dialogueId = null)
        {
            speakerId = speakerId ?? string.Empty;
            speakerName = speakerName ?? string.Empty;
            message = message ?? string.Empty;
            // Main03의 구형 페이지는 화자를 본문 첫 줄에 넣고 모든 ID를 태온으로 전달했습니다.
            // 이 호환 처리는 한 곳에서 실제 화자를 복원하며 발화 본문이나 페이지 순서는 바꾸지 않습니다.
            if (speakerName == "대화" && message.StartsWith("플레이어\n", StringComparison.Ordinal))
            {
                speakerId = PlayerSpeakerId;
                speakerName = "플레이어";
                message = message.Substring("플레이어\n".Length);
            }
            else if (speakerName == "대화" && message.StartsWith("태온\n", StringComparison.Ordinal))
            {
                speakerId = DialoguePortraitCatalog.TaeonId;
                speakerName = "태온";
                message = message.Substring("태온\n".Length);
            }
            // 명시적인 지문은 Character 발화가 아닙니다. 기본 NPC 대화나 연속 페이지에서
            // 화자 ID가 잘못 전달되어도 같은 규칙으로 이름·초상화·음성 연결을 차단합니다.
            if (message.StartsWith(DirectionPrefix, StringComparison.Ordinal))
            {
                speakerId = string.Empty;
                speakerName = string.Empty;
            }
            // 이름이 바뀌는 기존 무음 Player 페이지도 같은 stable 화자 규칙을 사용합니다.
            // 정식 NPC ID가 있으면 Player가 NPC와 같은 이름을 골랐더라도 NPC 화자는 보존합니다.
            bool playerName = speakerName == "플레이어" ||
                (!string.IsNullOrWhiteSpace(GameSessionData.PlayerName) && speakerName == GameSessionData.PlayerName);
            if (speakerId == PlayerSpeakerId || (string.IsNullOrWhiteSpace(speakerId) && playerName))
            {
                speakerId = PlayerSpeakerId;
                speakerName = string.IsNullOrWhiteSpace(GameSessionData.PlayerName) ? "플레이어" : GameSessionData.PlayerName;
            }
            SpeakerId = speakerId;
            SpeakerName = speakerName;
            Message = message;
            DialogueId = dialogueId ?? string.Empty;
        }

        public string SpeakerId { get; }
        public string SpeakerName { get; }
        public string Message { get; }
        public bool IsPlayer => SpeakerId == PlayerSpeakerId;
        public bool IsDirection => Message.StartsWith(DirectionPrefix, StringComparison.Ordinal);
        // 제작 manifest의 안정 ID입니다. 음성이 없는 기존 대사는 빈 값이며 Save에는 기록하지 않습니다.
        public string DialogueId { get; }
    }

    /// <summary>
    /// DialogueSystem GameObject에 붙어 화자 이름과 대사를 화면 아래쪽 패널에 표시합니다.
    /// Scene 어디서든 하나의 Instance를 통해 같은 대화 UI를 사용하도록 관리합니다.
    /// </summary>
    public sealed class DialoguePresenter : MonoBehaviour
    {
        public static DialoguePresenter Instance { get; private set; }

        // 대화에 사용할 글꼴입니다. 비어 있으면 Unity 기본 글꼴을 사용합니다.
        [SerializeField] private Font dialogueFont;
        private GameObject panel;
        private Text dialogueText;
        private RectTransform dialogueTextRect;
        private RectTransform bodyViewport;
        private UnityEngine.UI.ScrollRect bodyScroll;
        private UnityEngine.UI.Text speakerText;
        private UnityEngine.UI.Text footerText;
        private RectTransform panelRect;
        private RectTransform canvasRect;
        private bool layoutDirty;
        private Vector2 lastCanvasSize;
        private GameObject portraitRoot;
        private Image portraitImage;
        private GameObject choiceRow;
        private Button confirmButton;
        private Button cancelButton;
        private Transform dialogueViewer;
        private Transform dialogueOwner;
        private float dialogueBreakDistance;
        private bool isDistanceTracked;
        private InputAction advanceAction;
        private DialogueLine[] sequenceLines = Array.Empty<DialogueLine>();
        private int sequencePageIndex;
        private Action onSequenceCompleted;
        private VoicePlaybackSource voicePlayback;
        private VoiceClipCatalog voiceCatalog;
        public bool IsOpen => panel != null && panel.activeSelf;

        /// <summary>중복 대화 UI를 제거하고, 이 객체를 공용 Instance로 등록한 뒤 패널을 만듭니다.</summary>
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            voicePlayback = gameObject.AddComponent<VoicePlaybackSource>();
            voiceCatalog = Resources.Load<VoiceClipCatalog>("Audio/Voice/Story/StoryVoiceCatalog");
            CreatePanel();
            advanceAction = new InputAction("AdvanceDialogue", InputActionType.Button);
            advanceAction.AddBinding("<Keyboard>/enter");
            advanceAction.AddBinding("<Keyboard>/space");
            advanceAction.performed += _ => Advance();
        }

        /// <summary>현재 공용 Instance가 제거되는 객체라면 참조를 비웁니다.</summary>
        private void OnDestroy()
        {
            voicePlayback?.Stop();
            WorldExperienceHud.SetInteractionUiOpen(this, false);
            WorldModalState.Release(this);
            advanceAction?.Dispose();
            if (Instance == this)
            {
                Instance = null;
            }
        }

        /// <summary>화자와 대사를 넣고 대화 패널을 화면에 표시합니다.</summary>
        public void Show(string speaker, string message)
            => Show(string.Empty, speaker, message);

        /// <summary>stable ID에 연결된 초상화와 함께 대사를 표시합니다. Sprite가 없으면 기존 글자 전용 배치를 사용합니다.</summary>
        public void Show(string speakerId, string speaker, string message)
        {
            ClearSequence();
            if (!WorldModalState.TryAcquire(this)) return;
            var line = new DialogueLine(speakerId, speaker, message);
            ApplyPortrait(line.SpeakerId);
            SetContent(line, "[Esc 또는 게임패드 B: 닫기]");
            choiceRow.SetActive(false);
            panel.SetActive(true);
            panel.transform.SetAsLastSibling();
            WorldExperienceHud.SetInteractionUiOpen(this, true);
        }

        /// <summary>긴 대사를 읽기 쉬운 여러 쪽으로 보여 주며, 마지막 쪽을 넘겼을 때만 완료 사건을 알립니다.</summary>
        public void ShowSequence(string speaker, string[] pages, Action onCompleted)
            => ShowSequence(string.Empty, speaker, pages, onCompleted);

        public void ShowSequence(string speakerId, string speaker, string[] pages, Action onCompleted)
        {
            if (pages == null || pages.Length == 0) return;
            DialogueLine[] lines = new DialogueLine[pages.Length];
            for (int i = 0; i < pages.Length; i++) lines[i] = new DialogueLine(speakerId, speaker, pages[i]);
            ShowSequence(lines, onCompleted);
        }

        /// <summary>페이지마다 화자와 초상화를 바꿀 수 있는 범용 연속 대화입니다.</summary>
        public void ShowSequence(DialogueLine[] lines, Action onCompleted)
        {
            if (lines == null || lines.Length == 0 || !WorldModalState.TryAcquire(this)) return;
            sequenceLines = lines;
            sequencePageIndex = 0;
            onSequenceCompleted = onCompleted;
            choiceRow.SetActive(false);
            panel.SetActive(true);
            panel.transform.SetAsLastSibling();
            WorldExperienceHud.SetInteractionUiOpen(this, true);
            RefreshSequenceText();
        }

        public void Advance()
        {
            if (!IsOpen || sequenceLines.Length == 0) return;
            if (sequencePageIndex + 1 < sequenceLines.Length)
            {
                sequencePageIndex++;
                RefreshSequenceText();
                return;
            }

            Action completed = onSequenceCompleted;
            Hide();
            completed?.Invoke();
        }

        /// <summary>시간 제한 없이 마우스·키보드·게임패드로 고를 수 있는 두 선택지를 표시합니다.</summary>
        public void ShowConfirmation(
            string speaker,
            string message,
            string confirmText,
            string cancelText,
            Action onConfirmed)
            => ShowConfirmation(string.Empty, speaker, message, confirmText, cancelText, onConfirmed);

        public void ShowConfirmation(
            string speakerId,
            string speaker,
            string message,
            string confirmText,
            string cancelText,
            Action onConfirmed)
        {
            if (!WorldModalState.TryAcquire(this)) return;
            ClearSequence();
            var line = new DialogueLine(speakerId, speaker, message);
            ApplyPortrait(line.SpeakerId);
            SetContent(line, string.Empty);
            choiceRow.SetActive(true);
            ConfigureButton(confirmButton, confirmText, () =>
            {
                choiceRow.SetActive(false);
                onConfirmed?.Invoke();
            });
            ConfigureButton(cancelButton, cancelText, Hide);
            panel.SetActive(true);
            panel.transform.SetAsLastSibling();
            WorldExperienceHud.SetInteractionUiOpen(this, true);
            confirmButton.Select();
        }

        /// <summary>대화 내용을 유지한 채 패널을 화면에서 숨깁니다.</summary>
        public void Hide()
        {
            panel.SetActive(false);
            ApplyPortrait(string.Empty);
            ClearDistanceTracking();
            ClearSequence();
            WorldExperienceHud.SetInteractionUiOpen(this, false);
            WorldModalState.Release(this);
        }

        private void RefreshSequenceText()
        {
            DialogueLine line = sequenceLines[sequencePageIndex];
            // 무음/Player도 Play(null)로 이전 Clip을 즉시 정리합니다. 잘못 등록된 Player ID나
            // 직전 NPC의 음성을 fallback으로 쓰지 않으며 자막은 기존 수동 Next로 진행합니다.
            voicePlayback.Play(!line.IsPlayer && !line.IsDirection && voiceCatalog != null ? voiceCatalog.Find(line.DialogueId, line.SpeakerId) : null);
            ApplyPortrait(line.SpeakerId);
            SetContent(line, "[E/F/Enter/Space 또는 A: 계속] [Esc/B: 닫기] [↑↓/패드/휠: 장문 읽기]");
        }

        /// <summary>본문은 Voice/Story 원문 그대로 표시하며 화자·조작 안내와 별도 영역으로 나눕니다.</summary>
        private void SetContent(DialogueLine line, string hint)
        {
            speakerText.text = line.SpeakerName;
            // 지문 prefix는 서식 태그가 아닌 본문입니다. 다음 Character 페이지는 기존 Rich Text 정책으로 복원합니다.
            dialogueText.supportRichText = !line.IsDirection;
            dialogueText.text = line.Message;
            footerText.text = hint;
            bodyScroll.verticalNormalizedPosition = 1f;
            layoutDirty = true;
        }

        private void LateUpdate()
        {
            if (!IsOpen) return;
            if (layoutDirty || canvasRect.rect.size != lastCanvasSize) RefreshLayout();
            // 최대 패널 높이를 넘는 본문은 잘라 버리지 않고 키보드/패드/마우스로 끝까지 읽습니다.
            if (bodyScroll.enabled)
            {
                float direction = 0f;
                if (Keyboard.current != null)
                {
                    if (Keyboard.current.upArrowKey.isPressed) direction += 1f;
                    if (Keyboard.current.downArrowKey.isPressed) direction -= 1f;
                }
                if (Gamepad.current != null) direction += Gamepad.current.dpad.ReadValue().y;
                float range = Mathf.Max(1f, dialogueTextRect.rect.height - bodyViewport.rect.height);
                bodyScroll.verticalNormalizedPosition = Mathf.Clamp01(bodyScroll.verticalNormalizedPosition + direction * 160f * Time.unscaledDeltaTime / range);
            }
        }

        private static float PreferredHeight(UnityEngine.UI.Text text, float width)
        {
            return text.cachedTextGeneratorForLayout.GetPreferredHeight(text.text,
                text.GetGenerationSettings(new Vector2(width, 0f))) / text.pixelsPerUnit;
        }

        /// <summary>실제 줄바꿈 높이를 측정해 Portrait·본문·Footer를 분리하고 화면 높이40% 안에 배치합니다.</summary>
        private void RefreshLayout()
        {
            lastCanvasSize = canvasRect.rect.size;
            if (lastCanvasSize.x <= 0f || lastCanvasSize.y <= 0f) return;
            const float padding = 16f, gap = 8f;
            float panelWidth = lastCanvasSize.x * .84f;
            float left = portraitRoot.activeSelf ? 190f : 28f;
            float textWidth = Mathf.Max(1f, panelWidth - left - 28f);
            float footerHeight = choiceRow.activeSelf ? 52f : PreferredHeight(footerText, panelWidth - 56f);
            float speakerHeight = PreferredHeight(speakerText, textWidth);
            float fixedHeight = padding * 2f + gap * 2f + speakerHeight + footerHeight;
            float maxHeight = lastCanvasSize.y * .4f;
            dialogueText.fontSize = 28;
            float bodyHeight = PreferredHeight(dialogueText, textWidth);
            // 제한 없는 AutoSize 대신 작은 범위만 사용해 장문에서도 읽을 수 있는 기본 크기를 보호합니다.
            while (bodyHeight + fixedHeight > maxHeight && dialogueText.fontSize > 24)
            {
                dialogueText.fontSize--;
                bodyHeight = PreferredHeight(dialogueText, textWidth);
            }
            float minimum = lastCanvasSize.y * .23f;
            if (portraitRoot.activeSelf)
                minimum = Mathf.Max(minimum, padding * 2f + 150f + gap + footerHeight);
            float height = Mathf.Clamp(bodyHeight + fixedHeight, minimum, maxHeight);
            panelRect.sizeDelta = new Vector2(0f, height);
            PlaceText(speakerText.rectTransform, left, padding, speakerHeight);
            float availableBody = height - fixedHeight;
            PlaceText(bodyViewport, left, padding + speakerHeight + gap, availableBody);
            dialogueTextRect.anchorMin = new Vector2(0f, 1f);
            dialogueTextRect.anchorMax = Vector2.one;
            dialogueTextRect.pivot = new Vector2(.5f, 1f);
            dialogueTextRect.offsetMin = new Vector2(0f, -Mathf.Max(bodyHeight, availableBody));
            dialogueTextRect.offsetMax = Vector2.zero;
            bodyScroll.enabled = bodyHeight > availableBody + .5f;
            PlaceText(footerText.rectTransform, 28f, height - padding - footerHeight, footerHeight);
            footerText.gameObject.SetActive(!choiceRow.activeSelf);
            layoutDirty = false;
        }

        private static void PlaceText(RectTransform rect, float left, float top, float height)
        {
            rect.anchorMin = new Vector2(0f, 1f); rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(.5f, 1f);
            rect.offsetMin = new Vector2(left, -top - height);
            rect.offsetMax = new Vector2(-28f, -top);
        }

        private void ClearSequence()
        {
            voicePlayback?.Stop();
            sequenceLines = Array.Empty<DialogueLine>();
            sequencePageIndex = 0;
            onSequenceCompleted = null;
        }

        /// <summary>현재 대화의 실제 참여자 참조와 공통 종료 거리를 등록합니다.</summary>
        public void TrackDistance(Transform viewer, Transform owner, float breakDistance)
        {
            dialogueViewer = viewer;
            dialogueOwner = owner;
            dialogueBreakDistance = Mathf.Max(0.1f, breakDistance);
            isDistanceTracked = true;
        }

        /// <summary>대화 중 상대가 사라지거나 종료 거리를 벗어나면 공통 종료 경로로 닫습니다.</summary>
        private void Update()
        {
            if (panel == null || !panel.activeSelf || !isDistanceTracked)
            {
                return;
            }

            if (dialogueOwner == null || dialogueViewer == null)
            {
                Hide();
                return;
            }

            if (!dialogueOwner.gameObject.activeInHierarchy
                || !dialogueViewer.gameObject.activeInHierarchy
                || (dialogueOwner.position - dialogueViewer.position).sqrMagnitude
                    > dialogueBreakDistance * dialogueBreakDistance)
            {
                Hide();
            }
        }

        private void ClearDistanceTracking()
        {
            dialogueViewer = null;
            dialogueOwner = null;
            dialogueBreakDistance = 0f;
            isDistanceTracked = false;
        }

        /// <summary>Scene 전환이나 객체 비활성화 중에도 HUD 억제 상태가 남지 않게 해제합니다.</summary>
        private void OnDisable()
        {
            advanceAction?.Disable();
            ClearSequence();
            WorldExperienceHud.SetInteractionUiOpen(this, false);
            WorldModalState.Release(this);
        }

        private void OnEnable() => advanceAction?.Enable();

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus) advanceAction?.Enable();
        }

        /// <summary>해상도에 맞춰 크기가 조절되는 Canvas와 대화 배경·글자를 코드로 구성합니다.</summary>
        private void CreatePanel()
        {
            if (EventSystem.current == null)
            {
                InputSystemUIInputModule inputModule = new GameObject("EventSystem", typeof(EventSystem))
                    .AddComponent<InputSystemUIInputModule>();
                inputModule.AssignDefaultActions();
            }

            GameObject canvasObject = new GameObject("DialogueCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);
            canvasRect = canvasObject.GetComponent<RectTransform>();
            // Screen Space Overlay는 카메라 위치와 관계없이 UI를 화면 위에 고정해 표시합니다.
            Canvas dialogueCanvas = canvasObject.GetComponent<Canvas>();
            dialogueCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            // World HUD(5)보다 높게 두어 표시 억제가 실패해도 상호작용 UI가 앞에 남습니다.
            dialogueCanvas.sortingOrder = 10;
            canvasObject.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasObject.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1280, 720);

            panel = new GameObject("DialoguePanel", typeof(Image));
            panel.transform.SetParent(canvasObject.transform, false);
            Image panelImage = panel.GetComponent<Image>();
            panelImage.color = new Color(0.04f, 0.06f, 0.1f, 0.92f);
            panelRect = panel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.08f, 0.05f);
            panelRect.anchorMax = new Vector2(0.92f, 0.05f);
            panelRect.pivot = new Vector2(.5f, 0f);
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;

            // 단일 본문은 측정한 높이를 직접 적용하여 ContentSizeFitter와 수동 배치의 충돌을 피합니다.
            var scrollObject = new GameObject("BodyScroll", typeof(RectTransform), typeof(UnityEngine.UI.ScrollRect));
            scrollObject.transform.SetParent(panel.transform, false);
            var viewportObject = new GameObject("BodyViewport", typeof(RectTransform), typeof(UnityEngine.UI.Image), typeof(UnityEngine.UI.Mask));
            viewportObject.transform.SetParent(scrollObject.transform, false);
            var viewportRect = viewportObject.GetComponent<RectTransform>();
            viewportRect.anchorMin = Vector2.zero; viewportRect.anchorMax = Vector2.one;
            viewportRect.offsetMin = Vector2.zero; viewportRect.offsetMax = Vector2.zero;
            viewportObject.GetComponent<UnityEngine.UI.Mask>().showMaskGraphic = false;
            bodyViewport = scrollObject.GetComponent<RectTransform>();
            bodyScroll = scrollObject.GetComponent<UnityEngine.UI.ScrollRect>();
            bodyScroll.viewport = viewportRect;
            bodyScroll.horizontal = false;
            bodyScroll.vertical = true;
            bodyScroll.movementType = UnityEngine.UI.ScrollRect.MovementType.Clamped;
            bodyScroll.inertia = false;
            bodyScroll.scrollSensitivity = 30f;
            GameObject textObject = new GameObject("DialogueText", typeof(Text));
            textObject.transform.SetParent(viewportObject.transform, false);
            dialogueText = textObject.GetComponent<Text>();
            dialogueText.font = dialogueFont != null
                ? dialogueFont
                : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            dialogueText.fontSize = 28;
            dialogueText.color = Color.white;
            dialogueText.alignment = TextAnchor.UpperLeft;
            dialogueText.horizontalOverflow = HorizontalWrapMode.Wrap;
            dialogueText.verticalOverflow = VerticalWrapMode.Truncate;
            dialogueTextRect = textObject.GetComponent<RectTransform>();
            bodyScroll.content = dialogueTextRect;
            dialogueTextRect.anchorMin = Vector2.zero;
            dialogueTextRect.anchorMax = Vector2.one;
            dialogueTextRect.offsetMin = new Vector2(28f, 20f);
            dialogueTextRect.offsetMax = new Vector2(-28f, -62f);

            speakerText = CreateTextRegion("SpeakerName", 28);
            footerText = CreateTextRegion("ContinueHint", 20);

            CreatePortraitSlot();

            choiceRow = new GameObject("ChoiceRow", typeof(RectTransform));
            choiceRow.transform.SetParent(panel.transform, false);
            RectTransform choiceRect = choiceRow.GetComponent<RectTransform>();
            choiceRect.anchorMin = new Vector2(1f, 0f);
            choiceRect.anchorMax = new Vector2(1f, 0f);
            choiceRect.pivot = new Vector2(1f, 0f);
            choiceRect.anchoredPosition = new Vector2(-28f, 16f);
            choiceRect.sizeDelta = new Vector2(520f, 52f);

            confirmButton = CreateChoiceButton(choiceRow.transform, "ConfirmButton", new Vector2(-270f, 0f));
            cancelButton = CreateChoiceButton(choiceRow.transform, "CancelButton", Vector2.zero);

            panel.SetActive(false);
        }

        private UnityEngine.UI.Text CreateTextRegion(string name, int size)
        {
            var text = new GameObject(name, typeof(UnityEngine.UI.Text)).GetComponent<UnityEngine.UI.Text>();
            text.transform.SetParent(panel.transform, false);
            text.font = dialogueText.font; text.fontSize = size; text.color = Color.white;
            text.alignment = TextAnchor.UpperLeft; text.raycastTarget = false;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Truncate;
            return text;
        }

        /// <summary>초상화 영역은 Sprite가 있을 때만 활성화하며, 없을 때에는 빈 여백도 남기지 않습니다.</summary>
        private void CreatePortraitSlot()
        {
            portraitRoot = new GameObject("PortraitSlot", typeof(Image));
            portraitRoot.transform.SetParent(panel.transform, false);
            Image frame = portraitRoot.GetComponent<Image>();
            frame.color = new Color(0.14f, 0.19f, 0.28f, 1f);
            frame.raycastTarget = false;
            RectTransform frameRect = portraitRoot.GetComponent<RectTransform>();
            frameRect.anchorMin = new Vector2(0f, 1f);
            frameRect.anchorMax = new Vector2(0f, 1f);
            frameRect.pivot = new Vector2(0f, 1f);
            frameRect.anchoredPosition = new Vector2(20f, -16f);
            frameRect.sizeDelta = new Vector2(150f, 150f);

            GameObject imageObject = new GameObject("PortraitImage", typeof(Image));
            imageObject.transform.SetParent(portraitRoot.transform, false);
            portraitImage = imageObject.GetComponent<Image>();
            portraitImage.preserveAspect = true;
            portraitImage.raycastTarget = false;
            RectTransform imageRect = imageObject.GetComponent<RectTransform>();
            imageRect.anchorMin = Vector2.zero;
            imageRect.anchorMax = Vector2.one;
            imageRect.offsetMin = new Vector2(6f, 6f);
            imageRect.offsetMax = new Vector2(-6f, -6f);
            portraitRoot.SetActive(false);
        }

        private void ApplyPortrait(string speakerId)
        {
            Sprite portrait = DialoguePortraitCatalog.GetPortrait(speakerId);
            bool hasPortrait = portrait != null;
            portraitImage.sprite = portrait;
            portraitRoot.SetActive(hasPortrait);
            bodyViewport.offsetMin = new Vector2(hasPortrait ? 190f : 28f, 20f);
            layoutDirty = true;
        }

        /// <summary>선택지 버튼의 공통 크기와 읽기 쉬운 글자 스타일을 구성합니다.</summary>
        private Button CreateChoiceButton(Transform parent, string objectName, Vector2 position)
        {
            GameObject buttonObject = new GameObject(objectName, typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(parent, false);
            RectTransform rect = buttonObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(1f, 0f);
            rect.anchorMax = new Vector2(1f, 0f);
            rect.pivot = new Vector2(1f, 0f);
            rect.anchoredPosition = position;
            rect.sizeDelta = new Vector2(250f, 48f);
            buttonObject.GetComponent<Image>().color = new Color(0.18f, 0.24f, 0.34f, 1f);

            GameObject labelObject = new GameObject("Label", typeof(Text));
            labelObject.transform.SetParent(buttonObject.transform, false);
            Text label = labelObject.GetComponent<Text>();
            label.font = dialogueFont != null ? dialogueFont : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            label.fontSize = 22;
            label.color = Color.white;
            label.alignment = TextAnchor.MiddleCenter;
            RectTransform labelRect = labelObject.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
            return buttonObject.GetComponent<Button>();
        }

        /// <summary>이전 상호작용의 Listener가 다음 선택에서 다시 실행되지 않도록 교체합니다.</summary>
        private static void ConfigureButton(Button button, string label, UnityEngine.Events.UnityAction action)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
            button.GetComponentInChildren<Text>().text = label;
        }
    }
}
