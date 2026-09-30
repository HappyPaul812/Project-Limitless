using System.Collections.Generic;
using ProjectLimitless.Audio;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.UI
{
    /// <summary>
    /// 모든 Scene이 공유하는 환경 설정 화면입니다. 사용자 채널값과 실제 Mixer 적용은 서비스에
    /// 맡기고, 화면은 정수 표시·접근 가능한 조작·모달 소유권과 선택 포커스만 관리합니다.
    /// </summary>
    // 첫 음성의 Start보다 먼저 저장된 Mixer 음량을 적용해 시작 순간의 음소거도 보호합니다.
    [DefaultExecutionOrder(-1000)]
    public sealed class AudioSettingsPresenter : MonoBehaviour
    {
        public static AudioSettingsPresenter Instance { get; private set; }
        private static int inputClosedFrame = -1;
        public static bool BlocksSceneInput => Instance != null && (Instance.IsOpen || inputClosedFrame == Time.frameCount);
        public bool IsOpen => overlay != null && overlay.activeSelf;
        private GameObject overlay;
        private GameObject previousSelection;
        private UnityEngine.UI.Button entryButton;
        private UnityEngine.UI.Toggle muteToggle;
        private UnityEngine.UI.Text muteValue;
        private readonly UnityEngine.UI.Slider[] sliders = new UnityEngine.UI.Slider[3];
        private readonly UnityEngine.UI.Text[] values = new UnityEngine.UI.Text[3];
        private readonly List<UnityEngine.UI.Selectable> controls = new List<UnityEngine.UI.Selectable>();
        private Font font;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetRuntime() { Instance = null; inputClosedFrame = -1; }

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void Start()
        {
            CreateUi();
            UserSettingsService.Changed += Refresh;
            // Mixer.SetFloat는 Awake/OnEnable 대신 Start 이후 호출해야 런타임 값이 덮이지 않습니다.
            AudioSettingsService.Apply();
            Refresh();
        }

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            Gamepad pad = Gamepad.current;
            if ((keyboard != null && keyboard.f10Key.wasPressedThisFrame)
                || (pad != null && pad.startButton.wasPressedThisFrame))
            {
                if (IsOpen) Close(); else Open();
                return;
            }
            if (!IsOpen) return;
            if ((keyboard != null && keyboard.escapeKey.wasPressedThisFrame)
                || (pad != null && pad.buttonEast.wasPressedThisFrame)) { Close(); return; }
            if (keyboard != null && keyboard.tabKey.wasPressedThisFrame)
            {
                GameObject selected = EventSystem.current != null ? EventSystem.current.currentSelectedGameObject : null;
                int index = controls.FindIndex(item => item.gameObject == selected);
                int direction = keyboard.shiftKey.isPressed ? -1 : 1;
                Select(controls[(index + direction + controls.Count) % controls.Count].gameObject);
            }
        }

        public void Open()
        {
            if (overlay == null || IsOpen || !WorldModalState.TryAcquire(this)) return;
            EnsureEventSystem();
            previousSelection = EventSystem.current.currentSelectedGameObject;
            Refresh();
            overlay.SetActive(true);
            PlayerController.SetMovementLocked(this, true);
            Select(muteToggle.gameObject);
        }

        public void Close()
        {
            if (!IsOpen) return;
            overlay.SetActive(false);
            WorldModalState.Release(this);
            PlayerController.SetMovementLocked(this, false);
            // 같은 프레임의 Esc가 Intro Skip에도 쓰이지 않게 닫은 프레임까지 입력을 보호합니다.
            inputClosedFrame = Time.frameCount;
            Select(previousSelection != null && previousSelection.activeInHierarchy ? previousSelection : entryButton.gameObject);
            previousSelection = null;
        }

        private void SceneLoaded(Scene scene, LoadSceneMode mode) => Close();
        private void OnDisable() => Close();
        private void OnDestroy()
        {
            WorldModalState.Release(this);
            PlayerController.SetMovementLocked(this, false);
            UserSettingsService.Changed -= Refresh;
            SceneManager.sceneLoaded -= SceneLoaded;
            if (Instance == this) Instance = null;
        }

        private void Refresh()
        {
            if (muteToggle == null) return;
            muteToggle.SetIsOnWithoutNotify(UserSettingsService.MuteAll);
            muteValue.text = UserSettingsService.MuteAll ? "ON" : "OFF";
            int[] current = { UserSettingsService.VoiceVolume, UserSettingsService.SfxVolume, UserSettingsService.BgmVolume };
            for (int i = 0; i < sliders.Length; i++)
            {
                // 화면 동기화는 저장 callback을 호출하지 않고 실제 사용자 조작만 저장합니다.
                sliders[i].SetValueWithoutNotify(current[i]);
                values[i].text = current[i].ToString();
            }
        }

        private void ChangeVolume(int index, float value)
        {
            int voice = UserSettingsService.VoiceVolume, sfx = UserSettingsService.SfxVolume, bgm = UserSettingsService.BgmVolume;
            int number = Mathf.RoundToInt(value);
            if (index == 0) voice = number; else if (index == 1) sfx = number; else bgm = number;
            UserSettingsService.SetAudioVolumes(voice, sfx, bgm);
        }

        private void CreateUi()
        {
            font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            GameObject canvasObject = new GameObject("AudioSettingsCanvas", typeof(Canvas), typeof(UnityEngine.UI.CanvasScaler), typeof(UnityEngine.UI.GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);
            Canvas canvas = canvasObject.GetComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay; canvas.sortingOrder = 1000;
            UnityEngine.UI.CanvasScaler scaler = canvasObject.GetComponent<UnityEngine.UI.CanvasScaler>();
            scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize; scaler.referenceResolution = new Vector2(1280, 720);
            entryButton = Button(canvasObject.transform, "Settings", "설정 · F10", new Vector2(1110, 660), new Vector2(150, 42));
            RectTransform entryRect = entryButton.GetComponent<RectTransform>();
            entryRect.anchorMin = entryRect.anchorMax = Vector2.one; entryRect.anchoredPosition = new Vector2(-90, -30);
            entryButton.onClick.AddListener(Open);
            UnityEngine.UI.Image shade = Image(canvasObject.transform, "SettingsOverlay", new Color(0, 0, 0, .82f));
            overlay = shade.gameObject; Stretch(shade.rectTransform); shade.raycastTarget = true;
            UnityEngine.UI.Image panel = Image(overlay.transform, "AudioPanel", new Color(.06f, .08f, .13f, 1));
            Rect(panel.rectTransform, new Vector2(640, 360), new Vector2(620, 480)); panel.raycastTarget = true;
            panel.rectTransform.anchorMin = panel.rectTransform.anchorMax = Vector2.one * .5f;
            panel.rectTransform.anchoredPosition = Vector2.zero;
            Text(panel.transform, "Title", "설정 · 오디오", new Vector2(310, 426), new Vector2(560, 40), 28);
            GameObject toggleObject = new GameObject("MuteAll", typeof(UnityEngine.UI.Toggle)); toggleObject.transform.SetParent(panel.transform, false);
            muteToggle = toggleObject.GetComponent<UnityEngine.UI.Toggle>(); Rect(muteToggle.GetComponent<RectTransform>(), new Vector2(310, 354), new Vector2(530, 46));
            UnityEngine.UI.Image toggleBackground = Image(toggleObject.transform, "Background", new Color(.12f, .24f, .36f));
            Rect(toggleBackground.rectTransform, new Vector2(18, 23), new Vector2(32, 32)); toggleBackground.raycastTarget = true;
            UnityEngine.UI.Image checkmark = Image(toggleBackground.transform, "Checkmark", new Color(1f, .78f, .3f));
            Rect(checkmark.rectTransform, new Vector2(16, 16), new Vector2(20, 20));
            muteToggle.targetGraphic = toggleBackground; muteToggle.graphic = checkmark;
            Text(toggleObject.transform, "Label", "전체 음소거", new Vector2(154, 23), new Vector2(220, 40), 21);
            muteValue = Text(toggleObject.transform, "Value", "OFF", new Vector2(490, 23), new Vector2(70, 40), 21);
            muteToggle.onValueChanged.AddListener(UserSettingsService.SetMuteAll); controls.Add(muteToggle);
            string[] labels = { "음성", "효과음", "배경음" };
            for (int i = 0; i < sliders.Length; i++)
            {
                float y = 274 - i * 72;
                Text(panel.transform, "Channel" + i, labels[i], new Vector2(94, y), new Vector2(100, 42), 21);
                GameObject sliderObject = new GameObject("Volume" + i, typeof(UnityEngine.UI.Slider)); sliderObject.transform.SetParent(panel.transform, false);
                UnityEngine.UI.Slider slider = sliderObject.GetComponent<UnityEngine.UI.Slider>(); sliders[i] = slider;
                Rect(slider.GetComponent<RectTransform>(), new Vector2(320, y), new Vector2(300, 42));
                UnityEngine.UI.Image track = Image(slider.transform, "Track", new Color(.14f, .2f, .28f));
                Rect(track.rectTransform, new Vector2(150, 21), new Vector2(300, 10)); track.raycastTarget = true;
                UnityEngine.UI.Image fill = Image(track.transform, "Fill", new Color(.35f, .64f, .82f)); Stretch(fill.rectTransform);
                GameObject handleArea = new GameObject("HandleArea", typeof(RectTransform)); handleArea.transform.SetParent(slider.transform, false); Stretch(handleArea.GetComponent<RectTransform>());
                UnityEngine.UI.Image handle = Image(handleArea.transform, "Handle", new Color(1f, .78f, .3f));
                Rect(handle.rectTransform, Vector2.zero, new Vector2(22, 36)); handle.raycastTarget = true;
                handle.rectTransform.anchorMin = handle.rectTransform.anchorMax = new Vector2(0, .5f);
                slider.fillRect = fill.rectTransform; slider.handleRect = handle.rectTransform; slider.targetGraphic = handle;
                slider.direction = UnityEngine.UI.Slider.Direction.LeftToRight; slider.minValue = 0; slider.maxValue = 100; slider.wholeNumbers = true;
                int index = i; slider.onValueChanged.AddListener(value => ChangeVolume(index, value)); controls.Add(slider);
                values[i] = Text(panel.transform, "VolumeValue" + i, "100", new Vector2(536, y), new Vector2(70, 42), 21);
            }
            UnityEngine.UI.Button close = Button(panel.transform, "Close", "닫기", new Vector2(310, 54), new Vector2(160, 42));
            close.onClick.AddListener(Close); controls.Add(close);
            Text(panel.transform, "Controls", "Tab / 위·아래: 이동   좌·우: 음량   Enter: 선택   Esc / 패드 B: 닫기", new Vector2(310, 16), new Vector2(590, 24), 14);
            for (int i = 0; i < controls.Count; i++)
                controls[i].navigation = new UnityEngine.UI.Navigation { mode = UnityEngine.UI.Navigation.Mode.Explicit,
                    selectOnUp = controls[(i + controls.Count - 1) % controls.Count], selectOnDown = controls[(i + 1) % controls.Count] };
            overlay.SetActive(false);
        }

        private static void EnsureEventSystem()
        {
            if (EventSystem.current != null) return;
            InputSystemUIInputModule module = new GameObject("EventSystem", typeof(EventSystem)).AddComponent<InputSystemUIInputModule>();
            module.AssignDefaultActions();
        }
        private static void Select(GameObject target) { if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(target); }
        private UnityEngine.UI.Button Button(Transform parent, string name, string text, Vector2 center, Vector2 size)
        {
            UnityEngine.UI.Image image = Image(parent, name, new Color(.12f, .32f, .5f)); image.raycastTarget = true;
            Rect(image.rectTransform, center, size);
            UnityEngine.UI.Button button = image.gameObject.AddComponent<UnityEngine.UI.Button>(); button.targetGraphic = image;
            Text(image.transform, "Label", "[ " + text + " ]", size * .5f, size - new Vector2(8, 4), 19);
            return button;
        }
        private UnityEngine.UI.Text Text(Transform parent, string name, string value, Vector2 center, Vector2 size, int fontSize)
        {
            GameObject obj = new GameObject(name, typeof(UnityEngine.UI.Text)); obj.transform.SetParent(parent, false);
            UnityEngine.UI.Text text = obj.GetComponent<UnityEngine.UI.Text>(); text.font = font; text.fontSize = fontSize;
            text.text = value; text.alignment = TextAnchor.MiddleCenter; text.color = Color.white; text.raycastTarget = false;
            Rect(text.rectTransform, center, size); return text;
        }
        private static UnityEngine.UI.Image Image(Transform parent, string name, Color color)
        {
            GameObject obj = new GameObject(name, typeof(UnityEngine.UI.Image)); obj.transform.SetParent(parent, false);
            UnityEngine.UI.Image image = obj.GetComponent<UnityEngine.UI.Image>(); image.color = color; image.raycastTarget = false; return image;
        }
        // 패널 내부는 왼쪽 아래를 기준으로 배치하고 CanvasScaler로 화면 크기에 맞춥니다.
        private static void Rect(RectTransform rect, Vector2 center, Vector2 size)
        { rect.anchorMin = rect.anchorMax = Vector2.zero; rect.pivot = Vector2.one * .5f; rect.anchoredPosition = center; rect.sizeDelta = size; }
        private static void Stretch(RectTransform rect)
        { rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one; rect.offsetMin = rect.offsetMax = Vector2.zero; }
    }
}
