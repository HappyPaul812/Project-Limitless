using System.Collections;
using System.Linq;
using ProjectLimitless.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectLimitless.UI
{
    /// <summary>
    /// 시작 이야기를 자동 재생하면서 클릭·Enter·Space 진행, P 일시정지, Esc 건너뛰기를 제공합니다.
    /// 새 캐릭터 모드는 선택된 슬롯을 그대로 둔 채 CharacterCreation으로 가고, 다시 보기는 Bootstrap으로 돌아갑니다.
    /// </summary>
    public sealed class OpeningIntroController : MonoBehaviour
    {
        private const float FadeSeconds = .8f;
        private const float CaptionAnchorY = .31f;
        private const float CaptionMaxTextWidth = 820f;
        private const float CaptionHorizontalPadding = 42f;
        private const float CaptionVerticalPadding = 20f;
        private CanvasGroup content;
        private Text narration;
        private Text pauseLabel;
        private Image glow;
        private Image glowCore;
        private Image captionShade;
        private readonly Image[] pathSymbols = new Image[5];
        private static Sprite radialGlowSprite;
        private int slideIndex = -1;
        private bool paused;
        private bool transitioning;
        private bool advanceRequested;
        private Coroutine sequence;

        private void Start()
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorPrefs.GetBool("ProjectLimitless.PlayOpeningIntroAsReplay", false))
            {
                UnityEditor.EditorPrefs.SetBool("ProjectLimitless.PlayOpeningIntroAsReplay", false);
                OpeningIntroLaunchContext.BeginReplay();
            }
#endif
            CreateInterface();
            sequence = StartCoroutine(PlaySequence());
        }

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null || transitioning) return;
            if (keyboard.escapeKey.wasPressedThisFrame) { Finish(); return; }
            if (keyboard.pKey.wasPressedThisFrame) TogglePause();
            if (!paused && (keyboard.enterKey.wasPressedThisFrame || keyboard.spaceKey.wasPressedThisFrame)) RequestAdvance();
        }

        private IEnumerator PlaySequence()
        {
            for (slideIndex = 0; slideIndex < OpeningIntroSequence.Slides.Length; slideIndex++)
            {
                OpeningIntroSlide slide = OpeningIntroSequence.Slides[slideIndex];
                advanceRequested = false;
                ApplySlide(slide);
                yield return Fade(0f, 1f);
                float elapsed = 0f;
                while (elapsed < slide.Duration && !advanceRequested)
                {
                    if (!paused)
                    {
                        elapsed += Time.unscaledDeltaTime;
                        AnimateVisual(slide.Visual, elapsed / Mathf.Max(.01f, slide.Duration));
                    }
                    yield return null;
                }
                yield return Fade(1f, 0f);
            }
            Finish();
        }

        private void ApplySlide(OpeningIntroSlide slide)
        {
            narration.text = slide.Text;
            narration.fontSize = slide.Visual == OpeningIntroVisual.Title ? 66 : slide.Visual == OpeningIntroVisual.Limit ? 52 : 30;
            narration.fontStyle = slide.Visual == OpeningIntroVisual.Title || slide.Visual == OpeningIntroVisual.Limit ? FontStyle.Bold : FontStyle.Normal;
            UpdateCaptionLayout();
            bool showGlow = slide.Visual == OpeningIntroVisual.Light || slide.Visual == OpeningIntroVisual.Gift || slide.Visual == OpeningIntroVisual.Limit;
            glow.gameObject.SetActive(showGlow);
            glowCore.gameObject.SetActive(showGlow);
            bool showPaths = slide.Visual == OpeningIntroVisual.Paths;
            for (int i = 0; i < pathSymbols.Length; i++) pathSymbols[i].gameObject.SetActive(showPaths);
        }

        private void AnimateVisual(OpeningIntroVisual visual, float progress)
        {
            if (glow.gameObject.activeSelf)
            {
                float pulse = Mathf.Sin(Mathf.Clamp01(progress) * Mathf.PI);
                glow.rectTransform.localScale = Vector3.one * (Mathf.Lerp(.72f, 1.16f, progress) + pulse * .025f);
                glowCore.rectTransform.localScale = Vector3.one * (Mathf.Lerp(.7f, 1.04f, progress) + pulse * .02f);
                glow.color = new Color(1f, .72f, .27f, .1f + pulse * .07f);
                glowCore.color = new Color(1f, .9f, .62f, .16f + pulse * .08f);
            }
            if (visual == OpeningIntroVisual.Paths)
            {
                int visibleCount = Mathf.Clamp(Mathf.CeilToInt(progress * pathSymbols.Length), 1, pathSymbols.Length);
                for (int i = 0; i < pathSymbols.Length; i++)
                    pathSymbols[i].color = i < visibleCount && pathSymbols[i].sprite != null ? Color.white : Color.clear;
            }
            content.transform.localScale = Vector3.one * Mathf.Lerp(1f, 1.025f, progress);
        }

        private IEnumerator Fade(float from, float to)
        {
            float elapsed = 0f;
            while (elapsed < FadeSeconds)
            {
                if (!paused) elapsed += Time.unscaledDeltaTime;
                content.alpha = Mathf.Lerp(from, to, elapsed / FadeSeconds);
                yield return null;
            }
            content.alpha = to;
        }

        private void RequestAdvance()
        {
            if (!transitioning && !paused) advanceRequested = true;
        }

        private void TogglePause()
        {
            paused = !paused;
            pauseLabel.text = paused ? "일시정지됨 · P로 계속" : string.Empty;
        }

        private void Finish()
        {
            if (transitioning) return;
            transitioning = true;
            if (sequence != null) StopCoroutine(sequence);
            // Esc/버튼은 이번 재생만 끝냅니다. 체크 설정은 Toggle callback에서만 저장합니다.
            SceneManager.LoadSceneAsync(OpeningIntroLaunchContext.IsReplay ? "Bootstrap" : "CharacterCreation", LoadSceneMode.Single);
        }

        private void CreateInterface()
        {
            if (Camera.main == null) { GameObject cameraObject = new GameObject("Main Camera"); cameraObject.tag = "MainCamera"; cameraObject.AddComponent<Camera>().backgroundColor = Color.black; }
            if (EventSystem.current == null) { InputSystemUIInputModule module = new GameObject("EventSystem", typeof(EventSystem)).AddComponent<InputSystemUIInputModule>(); module.AssignDefaultActions(); }
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            GameObject canvasObject = new GameObject("OpeningIntroCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; scaler.referenceResolution = new Vector2(1280, 720);
            Image background = MakeImage(canvasObject.transform, "Background", new Color(.005f, .009f, .02f, 1f)); Stretch(background.rectTransform);
            Button advance = new GameObject("AdvanceArea", typeof(Image), typeof(Button)).GetComponent<Button>(); advance.transform.SetParent(canvasObject.transform, false); Stretch(advance.GetComponent<RectTransform>()); advance.GetComponent<Image>().color = Color.clear; advance.onClick.AddListener(RequestAdvance);
            GameObject contentObject = new GameObject("SlideContent", typeof(RectTransform), typeof(CanvasGroup)); contentObject.transform.SetParent(canvasObject.transform, false); Stretch(contentObject.GetComponent<RectTransform>()); content = contentObject.GetComponent<CanvasGroup>();
            Sprite glowSprite = GetRadialGlowSprite();
            glow = MakeImage(content.transform, "AbstractLight", new Color(1f, .72f, .27f, .1f)); glow.sprite = glowSprite; glow.preserveAspect = true; SetRect(glow.rectTransform, new Vector2(.5f, .59f), new Vector2(500, 500));
            glowCore = MakeImage(content.transform, "AbstractLightCore", new Color(1f, .9f, .62f, .16f)); glowCore.sprite = glowSprite; glowCore.preserveAspect = true; SetRect(glowCore.rectTransform, new Vector2(.5f, .59f), new Vector2(220, 220));
            captionShade = MakeImage(content.transform, "CaptionShade", new Color(.01f, .02f, .04f, .68f)); captionShade.sprite = glowSprite; SetRect(captionShade.rectTransform, new Vector2(.5f, CaptionAnchorY), new Vector2(300, 80));
            narration = MakeText(content.transform, "Narration", string.Empty, font, 30, new Vector2(.5f, CaptionAnchorY), new Vector2(CaptionMaxTextWidth, 150)); narration.lineSpacing = 1.25f; narration.verticalOverflow = VerticalWrapMode.Overflow;
            string[] order = { "path.emotional-scar", "path.hearing", "path.vision", "path.mobility", "path.intellectual" };
            for (int i = 0; i < order.Length; i++) { pathSymbols[i] = MakeImage(content.transform, $"PathSymbol{i + 1}", Color.clear); pathSymbols[i].sprite = PathPresentationResolver.Find(order[i])?.PathSymbol; pathSymbols[i].preserveAspect = true; SetRect(pathSymbols[i].rectTransform, new Vector2(.27f + i * .115f, .69f), new Vector2(112, 112)); }
            Button skip = MakeButton(canvasObject.transform, "Skip", "건너뛰기", font, new Vector2(.9f, .06f), Finish);
            Toggle toggle = new GameObject("SkipNextTime", typeof(Toggle)).GetComponent<Toggle>(); toggle.transform.SetParent(canvasObject.transform, false); SetRect(toggle.GetComponent<RectTransform>(), new Vector2(.27f, .06f), new Vector2(440, 32));
            Image toggleBackground = MakeImage(toggle.transform, "Background", new Color(.12f, .16f, .22f, 1f)); SetRect(toggleBackground.rectTransform, new Vector2(.03f, .5f), new Vector2(24, 24));
            Image check = MakeImage(toggleBackground.transform, "Checkmark", new Color(1f, .78f, .3f, 1f)); Stretch(check.rectTransform); toggle.targetGraphic = toggleBackground; toggle.graphic = check; toggle.isOn = UserSettingsService.SkipOpeningIntro; toggle.onValueChanged.AddListener(UserSettingsService.SetSkipOpeningIntro);
            MakeText(toggle.transform, "Label", "다음부터 시작 이야기를 자동으로 건너뜁니다.", font, 16, new Vector2(.56f, .5f), new Vector2(385, 30)).alignment = TextAnchor.MiddleLeft;
            pauseLabel = MakeText(canvasObject.transform, "PauseState", string.Empty, font, 18, new Vector2(.5f, .92f), new Vector2(500, 34)); pauseLabel.color = new Color(1f, .82f, .4f, 1f);
            MakeText(canvasObject.transform, "Controls", "클릭 / Enter / Space: 다음   P: 일시정지   Esc: 건너뛰기", font, 14, new Vector2(.5f, .02f), new Vector2(620, 24)).color = new Color(.72f, .78f, .88f, 1f);
            EventSystem.current.SetSelectedGameObject(skip.gameObject);
        }

        private void UpdateCaptionLayout()
        {
            float textWidth = Mathf.Clamp(Mathf.Ceil(narration.preferredWidth), 140f, CaptionMaxTextWidth);
            narration.rectTransform.sizeDelta = new Vector2(textWidth, 150f);
            float textHeight = Mathf.Max(narration.fontSize * 1.25f, Mathf.Ceil(narration.preferredHeight));
            narration.rectTransform.sizeDelta = new Vector2(textWidth, textHeight);
            captionShade.rectTransform.sizeDelta = new Vector2(
                textWidth + CaptionHorizontalPadding * 2f,
                textHeight + CaptionVerticalPadding * 2f);
        }

        private static Button MakeButton(Transform parent, string name, string label, Font font, Vector2 anchor, UnityEngine.Events.UnityAction action) { GameObject obj = new GameObject(name, typeof(Image), typeof(Button), typeof(Outline)); obj.transform.SetParent(parent, false); SetRect(obj.GetComponent<RectTransform>(), anchor, new Vector2(170, 44)); obj.GetComponent<Image>().color = new Color(.12f, .32f, .5f, 1f); Button button = obj.GetComponent<Button>(); button.onClick.AddListener(action); MakeText(obj.transform, "Label", $"[ {label} ]", font, 17, Vector2.one * .5f, new Vector2(160, 38)); return button; }
        private static Image MakeImage(Transform parent, string name, Color color) { GameObject obj = new GameObject(name, typeof(Image)); obj.transform.SetParent(parent, false); Image image = obj.GetComponent<Image>(); image.color = color; image.raycastTarget = false; return image; }
        private static Sprite GetRadialGlowSprite()
        {
            if (radialGlowSprite != null) return radialGlowSprite;

            const int size = 128;
            Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false)
            {
                name = "OpeningIntroRadialGlow",
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp,
                hideFlags = HideFlags.HideAndDontSave
            };
            Color32[] pixels = new Color32[size * size];
            for (int y = 0; y < size; y++)
            {
                float normalizedY = ((y + .5f) / size) * 2f - 1f;
                for (int x = 0; x < size; x++)
                {
                    float normalizedX = ((x + .5f) / size) * 2f - 1f;
                    float inward = Mathf.Clamp01(1f - Mathf.Sqrt(normalizedX * normalizedX + normalizedY * normalizedY));
                    float alpha = inward * inward * (3f - 2f * inward);
                    pixels[y * size + x] = new Color32(255, 255, 255, (byte)Mathf.RoundToInt(alpha * 255f));
                }
            }
            texture.SetPixels32(pixels);
            texture.Apply(false, true);
            radialGlowSprite = Sprite.Create(texture, new Rect(0f, 0f, size, size), Vector2.one * .5f, 100f, 0, SpriteMeshType.FullRect);
            radialGlowSprite.name = "OpeningIntroRadialGlow";
            radialGlowSprite.hideFlags = HideFlags.HideAndDontSave;
            return radialGlowSprite;
        }
        private static Text MakeText(Transform parent, string name, string value, Font font, int size, Vector2 anchor, Vector2 dimensions) { GameObject obj = new GameObject(name, typeof(Text)); obj.transform.SetParent(parent, false); Text text = obj.GetComponent<Text>(); text.font = font; text.fontSize = size; text.color = Color.white; text.alignment = TextAnchor.MiddleCenter; text.text = value; text.raycastTarget = false; SetRect(text.rectTransform, anchor, dimensions); return text; }
        private static void SetRect(RectTransform rect, Vector2 anchor, Vector2 size) { rect.anchorMin = anchor; rect.anchorMax = anchor; rect.pivot = Vector2.one * .5f; rect.anchoredPosition = Vector2.zero; rect.sizeDelta = size; }
        private static void Stretch(RectTransform rect) { rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one; rect.offsetMin = Vector2.zero; rect.offsetMax = Vector2.zero; }
    }
}
