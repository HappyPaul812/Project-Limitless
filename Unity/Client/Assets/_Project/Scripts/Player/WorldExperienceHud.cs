using System.Collections.Generic;
using ProjectLimitless.Core;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.Player
{
    /// <summary>
    /// 마을과 필드 화면 왼쪽 위에 현재 캐릭터의 레벨과 경험치 진행을 표시합니다.
    /// 머리 위 이름표와 달리 화면에 고정되어 이동 중에도 성장 정보를 쉽게 확인할 수 있습니다.
    /// </summary>
    public sealed class WorldExperienceHud : MonoBehaviour
    {
        private const string ObjectName = "WorldExperienceHud";
        private const float LeftSafeMargin = 24f;
        private const float TopSafeMargin = 24f;
        private static readonly Color PanelColor = new Color(0.055f, 0.065f, 0.09f, 0.94f);
        private static readonly Color BorderColor = new Color(0.82f, 0.66f, 0.25f, 1f);
        private static readonly Color FillColor = new Color(0.9f, 0.7f, 0.22f, 1f);
        private static readonly HashSet<Object> InteractionUiOwners = new HashSet<Object>();

        private CanvasGroup canvasGroup;
        private Text identityText;
        private Text experienceText;
        private Image pathSymbolImage;
        private Image fillImage;
        private Sprite fillSprite;
        private string displayedName = string.Empty;
        private string displayedPathId = string.Empty;
        private int displayedLevel = -1;
        private int displayedExperience = -1;

        /// <summary>
        /// PlayerNameplate가 사용하는 Overlay Canvas 하나에 HUD 하나만 둡니다.
        /// Scene 전환으로 Player와 Canvas가 정리되므로 이전 HUD가 다음 지도에 겹쳐 남지 않습니다.
        /// </summary>
        public static WorldExperienceHud EnsureOn(GameObject overlayCanvasObject)
        {
            Transform existing = overlayCanvasObject.transform.Find(ObjectName);
            if (existing != null)
            {
                WorldExperienceHud existingHud = existing.GetComponent<WorldExperienceHud>();
                return existingHud != null ? existingHud : existing.gameObject.AddComponent<WorldExperienceHud>();
            }

            GameObject hudObject = new GameObject(ObjectName, typeof(RectTransform), typeof(WorldExperienceHud));
            hudObject.transform.SetParent(overlayCanvasObject.transform, false);
            return hudObject.GetComponent<WorldExperienceHud>();
        }

        /// <summary>
        /// 대화·상점·은행처럼 월드 HUD보다 우선하는 UI의 열림 상태를 소유자별로 등록합니다.
        /// 같은 소유자의 중복 호출은 한 번으로 처리하고, 모든 UI가 닫힌 뒤에만 HUD를 복귀시킵니다.
        /// </summary>
        public static void SetInteractionUiOpen(Object owner, bool isOpen)
        {
            if (owner == null)
            {
                return;
            }

            if (isOpen)
            {
                InteractionUiOwners.Add(owner);
            }
            else
            {
                InteractionUiOwners.Remove(owner);
            }

            ApplyVisibilityToInstances();
        }

        /// <summary>HUD의 렌더링과 입력 차단을 함께 제어합니다.</summary>
        public void SetVisible(bool visible)
        {
            BuildIfNeeded();
            canvasGroup.alpha = visible ? 1f : 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticState()
        {
            InteractionUiOwners.Clear();
        }

        private void Awake()
        {
            BuildIfNeeded();
            Refresh();
            ApplyInteractionVisibility();
        }

        private void OnEnable()
        {
            BuildIfNeeded();
            Refresh();
            ApplyInteractionVisibility();
        }

        private void Update()
        {
            // 세션의 숫자만 비교하므로 비용이 작고, 값이 그대로인 프레임에는 UI 문자열이나 오브젝트를 만들지 않습니다.
            if (displayedName != PlayerNameplate.GetDisplayName()
                || displayedPathId != GameSessionData.SelectedPlayerPathId
                || displayedLevel != GameSessionData.Level
                || displayedExperience != GameSessionData.CurrentExperience)
            {
                Refresh();
            }
        }

        private void OnDestroy()
        {
            if (fillSprite != null)
            {
                Destroy(fillSprite);
            }
        }

        /// <summary>현재 슬롯에서 복원된 세션 값으로 숫자와 Bar를 함께 갱신합니다.</summary>
        public void Refresh()
        {
            BuildIfNeeded();
            displayedName = PlayerNameplate.GetDisplayName();
            displayedPathId = GameSessionData.SelectedPlayerPathId;
            displayedLevel = GameSessionData.Level;
            displayedExperience = GameSessionData.CurrentExperience;

            RefreshPathSymbol();
            identityText.text = $"Lv.{displayedLevel}  {displayedName}";
            if (displayedLevel >= CharacterGrowthCalculator.MaxLevel)
            {
                // 만렙에는 다음 성장 구간이 없어 존재하지 않는 분모를 보여주지 않습니다.
                experienceText.text = "MAX LEVEL";
                fillImage.fillAmount = 1f;
                return;
            }

            // 성장 공식을 UI에 복사하지 않고 중앙 ExperienceProgression을 사용해야 전투 보상과 표시가 어긋나지 않습니다.
            int requiredExperience = ExperienceProgression.RequiredExp(displayedLevel);
            experienceText.text = $"EXP {displayedExperience} / {requiredExperience}";
            fillImage.fillAmount = CalculateProgress(displayedLevel, displayedExperience);
        }

        /// <summary>UI와 검증이 함께 사용하는 0~1 경험치 진행률입니다.</summary>
        public static float CalculateProgress(int level, int currentExperience)
        {
            if (level >= CharacterGrowthCalculator.MaxLevel)
            {
                return 1f;
            }

            int requiredExperience = ExperienceProgression.RequiredExp(level);
            return requiredExperience <= 0 ? 0f : Mathf.Clamp01((float)Mathf.Max(0, currentExperience) / requiredExperience);
        }

        private void BuildIfNeeded()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }

            if (identityText != null && experienceText != null && pathSymbolImage != null && fillImage != null)
            {
                return;
            }

            RectTransform root = GetComponent<RectTransform>();
            // Canvas의 좌상단 Anchor와 같은 Pivot을 사용해 화면 크기가 바뀌어도
            // 왼쪽·위쪽 여백을 일정하게 유지합니다. 중앙 Quest Toast와는 가로 영역을 분리합니다.
            // 향후 모서리 위치 프리셋은 이 Anchor/Pivot/여백 조합을 바꾸면 됩니다.
            root.anchorMin = new Vector2(0f, 1f);
            root.anchorMax = new Vector2(0f, 1f);
            root.pivot = new Vector2(0f, 1f);
            root.anchoredPosition = new Vector2(LeftSafeMargin, -TopSafeMargin);
            root.sizeDelta = new Vector2(320f, 72f);

            Image panel = GetOrAddImage(gameObject);
            panel.color = PanelColor;
            panel.raycastTarget = false;
            Outline panelOutline = GetOrAddOutline(gameObject);
            panelOutline.effectColor = BorderColor;
            panelOutline.effectDistance = new Vector2(2f, -2f);

            GameObject pathSymbol = CreateRect("PathSymbol", new Vector2(12f, 34f), new Vector2(32f, 32f));
            pathSymbolImage = GetOrAddImage(pathSymbol);
            pathSymbolImage.preserveAspect = true;
            pathSymbolImage.color = Color.white;
            // EXP HUD는 전투 상태가 아니라 플레이어의 고정 정체성을 보여 주므로 TraitIcon 대신
            // 선택한 길 자체의 공식 문장인 PathSymbol만 이름 왼쪽에 표시합니다.
            identityText = CreateText("Identity", new Vector2(50f, 37f), new Vector2(150f, 26f), TextAnchor.MiddleLeft, 16);
            experienceText = CreateText("Experience", new Vector2(202f, 37f), new Vector2(106f, 26f), TextAnchor.MiddleRight, 14);
            // 좁아진 첫 줄에서도 이름과 EXP가 각자의 칸 안에서 글자 크기를 조절해 겹치지 않게 합니다.
            identityText.resizeTextForBestFit = true;
            identityText.resizeTextMinSize = 10;
            identityText.resizeTextMaxSize = 16;
            experienceText.resizeTextForBestFit = true;
            experienceText.resizeTextMinSize = 11;
            experienceText.resizeTextMaxSize = 14;
            identityText.horizontalOverflow = HorizontalWrapMode.Wrap;
            experienceText.horizontalOverflow = HorizontalWrapMode.Wrap;

            GameObject barBackground = CreateRect("BarBackground", new Vector2(20f, 11f), new Vector2(280f, 9f));
            GetOrAddImage(barBackground).color = new Color(0.13f, 0.14f, 0.18f, 1f);

            GameObject fill = CreateRect("BarFill", Vector2.zero, Vector2.zero, barBackground.transform);
            RectTransform fillRect = fill.GetComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.offsetMin = new Vector2(1f, 1f);
            fillRect.offsetMax = new Vector2(-1f, -1f);
            fillImage = GetOrAddImage(fill);
            fillImage.color = FillColor;
            // Sprite가 없는 uGUI Image는 단순 사각형 메시로 그려져 Filled 타입과 fillAmount를 무시합니다.
            // 흰색 1픽셀 Sprite를 Source로 제공해 실제 메시가 왼쪽부터 진행률만큼 잘리게 합니다.
            fillSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
            fillSprite.name = "WorldExperienceFillSprite";
            fillImage.sprite = fillSprite;
            fillImage.type = Image.Type.Filled;
            fillImage.fillMethod = Image.FillMethod.Horizontal;
            fillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
            fillImage.raycastTarget = false;
        }

        private void ApplyInteractionVisibility()
        {
            SetVisible(InteractionUiOwners.Count == 0);
        }

        private static void ApplyVisibilityToInstances()
        {
            bool visible = InteractionUiOwners.Count == 0;
            WorldExperienceHud[] huds = FindObjectsByType<WorldExperienceHud>(FindObjectsInactive.Include);
            foreach (WorldExperienceHud hud in huds)
            {
                if (hud != null)
                {
                    hud.SetVisible(visible);
                }
            }
        }

        private void RefreshPathSymbol()
        {
            // 저장 파일에는 바뀔 수 있는 Sprite 자체가 아니라 안정적인 PathId만 보관합니다.
            // 새 게임과 이어하기 모두 그 ID를 세션에 복원하므로 공통 Resolver가 언제나 같은 공식 심볼을 찾습니다.
            PlayerPathDefinition selectedPath = PathPresentationResolver.Find(displayedPathId);
            Sprite symbol = selectedPath?.PathSymbol;
            pathSymbolImage.sprite = symbol;
            // 개발용 세션이나 오래된 저장에 길 정보가 없어도 흰 네모나 예외 대신 기존 레벨·이름만 남깁니다.
            pathSymbolImage.enabled = symbol != null;
        }

        private Text CreateText(string objectName, Vector2 position, Vector2 size, TextAnchor alignment, int fontSize)
        {
            GameObject textObject = CreateRect(objectName, position, size);
            textObject.AddComponent<CanvasRenderer>();
            Text text = textObject.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = fontSize;
            text.color = Color.white;
            text.alignment = alignment;
            text.raycastTarget = false;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            Outline outline = textObject.AddComponent<Outline>();
            outline.effectColor = new Color(0f, 0f, 0f, 0.9f);
            outline.effectDistance = new Vector2(1f, -1f);
            return text;
        }

        private GameObject CreateRect(string objectName, Vector2 position, Vector2 size, Transform parent = null)
        {
            GameObject child = new GameObject(objectName, typeof(RectTransform));
            child.transform.SetParent(parent != null ? parent : transform, false);
            RectTransform rect = child.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.zero;
            rect.pivot = Vector2.zero;
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
            return child;
        }

        private static Image GetOrAddImage(GameObject target)
        {
            Image image = target.GetComponent<Image>();
            if (image == null)
            {
                if (target.GetComponent<CanvasRenderer>() == null) target.AddComponent<CanvasRenderer>();
                image = target.AddComponent<Image>();
            }
            image.raycastTarget = false;
            return image;
        }

        private static Outline GetOrAddOutline(GameObject target)
        {
            Outline outline = target.GetComponent<Outline>();
            return outline != null ? outline : target.AddComponent<Outline>();
        }
    }
}
