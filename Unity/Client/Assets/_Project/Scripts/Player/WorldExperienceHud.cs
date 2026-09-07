using ProjectLimitless.Core;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.Player
{
    /// <summary>
    /// 마을과 필드 화면 아래에 현재 캐릭터의 레벨과 경험치 진행을 표시합니다.
    /// 머리 위 이름표와 달리 화면에 고정되어 이동 중에도 성장 정보를 쉽게 확인할 수 있습니다.
    /// </summary>
    public sealed class WorldExperienceHud : MonoBehaviour
    {
        private const string ObjectName = "WorldExperienceHud";
        private static readonly Color PanelColor = new Color(0.055f, 0.065f, 0.09f, 0.94f);
        private static readonly Color BorderColor = new Color(0.82f, 0.66f, 0.25f, 1f);
        private static readonly Color FillColor = new Color(0.9f, 0.7f, 0.22f, 1f);

        private Text identityText;
        private Text experienceText;
        private Image fillImage;
        private Sprite fillSprite;
        private string displayedName = string.Empty;
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

        private void Awake()
        {
            BuildIfNeeded();
            Refresh();
        }

        private void OnEnable()
        {
            BuildIfNeeded();
            Refresh();
        }

        private void Update()
        {
            // 세션의 숫자만 비교하므로 비용이 작고, 값이 그대로인 프레임에는 UI 문자열이나 오브젝트를 만들지 않습니다.
            if (displayedName != PlayerNameplate.GetDisplayName()
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
            displayedLevel = GameSessionData.Level;
            displayedExperience = GameSessionData.CurrentExperience;

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
            if (identityText != null && experienceText != null && fillImage != null)
            {
                return;
            }

            RectTransform root = GetComponent<RectTransform>();
            root.anchorMin = new Vector2(0.5f, 0f);
            root.anchorMax = new Vector2(0.5f, 0f);
            root.pivot = new Vector2(0.5f, 0f);
            root.anchoredPosition = new Vector2(0f, 22f);
            root.sizeDelta = new Vector2(420f, 76f);

            Image panel = GetOrAddImage(gameObject);
            panel.color = PanelColor;
            panel.raycastTarget = false;
            Outline panelOutline = GetOrAddOutline(gameObject);
            panelOutline.effectColor = BorderColor;
            panelOutline.effectDistance = new Vector2(2f, -2f);

            identityText = CreateText("Identity", new Vector2(16f, 40f), new Vector2(388f, 26f), TextAnchor.MiddleLeft, 20);
            experienceText = CreateText("Experience", new Vector2(16f, 16f), new Vector2(388f, 22f), TextAnchor.MiddleRight, 17);

            GameObject barBackground = CreateRect("BarBackground", new Vector2(16f, 8f), new Vector2(388f, 8f));
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
