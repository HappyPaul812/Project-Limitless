using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ProjectLimitless.Validation
{
    /// <summary>
    /// 실제 Field와 전투를 건드리지 않고 Pilot Bee와 현재 초원 슬라임만 나란히 비교하는 검증 화면입니다.
    /// 벌을 채택하기 전 크기·색감·픽셀 밀도와 Idle/Attack 움직임을 눈으로 판단하는 용도이며,
    /// 독 판정이나 몬스터 데이터는 전혀 만들지 않습니다.
    /// </summary>
    public sealed class PilotBeeComparisonValidation : MonoBehaviour
    {
        private const float FramesPerSecond = 10f;
        private const float RecommendedBeeScale = .85f;
        private const float SlimeDisplaySize = 136f;
        private const float CommonPixelsPerUnit = 314f;

        private Sprite[] beeIdle = Array.Empty<Sprite>();
        private Sprite[] beeAttack = Array.Empty<Sprite>();
        private Sprite[] slimeIdle = Array.Empty<Sprite>();
        private Image beeImage;
        private Image slimeImage;
        private Text stateText;
        private float elapsed;
        private float beeScale = RecommendedBeeScale;
        private bool attackPlaying;
        private bool paused;

        private void Start()
        {
            Texture2D idleSheet = Resources.Load<Texture2D>("MonsterValidation/PilotBee/pilot_bee_idle");
            Texture2D attackSheet = Resources.Load<Texture2D>("MonsterValidation/PilotBee/pilot_bee_attack");
            Texture2D slimeSheet = Resources.Load<Texture2D>("MonsterValidation/PilotBee/grass_slime_comparison");
            beeIdle = SliceGrid(idleSheet, 238, 215, 8, 10, "PilotBee_Idle");
            beeAttack = SliceGrid(attackSheet, 315, 253, 10, 10, "PilotBee_Attack");
            // 슬라임 시트의 첫째 줄 4장은 현재 정면 Idle/움직임 비교에 충분하며 원본을 수정하지 않습니다.
            slimeIdle = SliceGrid(slimeSheet, 314, 314, 4, 4, "GrassSlime_Comparison");
            BuildScreen();
            RefreshStateText();
        }

        private void Update()
        {
            // 이 프로젝트는 새 Input System만 사용하므로 legacy UnityEngine.Input을 호출하면
            // Player 설정과 충돌해 매 프레임 InvalidOperationException이 발생합니다.
            // 새 Input System은 현재 연결된 키보드를 Keyboard.current로 읽으며, 키보드가 없는
            // 환경도 있을 수 있으므로 null일 때는 입력만 건너뛰고 애니메이션은 계속 갱신합니다.
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null)
            {
                if (keyboard.spaceKey.wasPressedThisFrame) paused = !paused;
                if (keyboard.aKey.wasPressedThisFrame)
                {
                    attackPlaying = !attackPlaying;
                    elapsed = 0f;
                }

                // 메인 키보드의 +/-는 각각 Equals/Minus 키로 들어오며,
                // 숫자 키패드가 있는 키보드는 별도의 NumpadPlus/NumpadMinus 키로 처리합니다.
                if (keyboard.equalsKey.wasPressedThisFrame || keyboard.numpadPlusKey.wasPressedThisFrame)
                    beeScale = Mathf.Min(1.5f, beeScale + .05f);
                if (keyboard.minusKey.wasPressedThisFrame || keyboard.numpadMinusKey.wasPressedThisFrame)
                    beeScale = Mathf.Max(.3f, beeScale - .05f);
            }

            if (!paused) elapsed += Time.unscaledDeltaTime;
            int frameIndex = Mathf.FloorToInt(elapsed * FramesPerSecond);
            Sprite[] currentBee = attackPlaying ? beeAttack : beeIdle;
            if (beeImage != null && currentBee.Length > 0)
                beeImage.sprite = currentBee[frameIndex % currentBee.Length];
            if (slimeImage != null && slimeIdle.Length > 0)
                slimeImage.sprite = slimeIdle[frameIndex % slimeIdle.Length];
            ApplyBeeDisplaySize();
            RefreshStateText();
        }

        private void BuildScreen()
        {
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            Canvas canvas = new GameObject("PilotBeeValidationCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster)).GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280f, 720f);

            Image background = MakeImage(canvas.transform, "Background", new Color(.035f, .055f, .11f, 1f));
            Stretch(background.rectTransform);
            MakeText(canvas.transform, "Title", "Pilot Bee · 초원 슬라임 비교 검증 (Field/Battle 미연결)", font, 27,
                new Vector2(.5f, .93f), new Vector2(1100f, 50f));
            MakeText(canvas.transform, "Guide", "Space: 일시정지  |  A: Idle / Attack  |  +/-: 벌 Scale 조절",
                font, 17, new Vector2(.5f, .07f), new Vector2(1000f, 36f));
            stateText = MakeText(canvas.transform, "State", string.Empty, font, 17,
                new Vector2(.5f, .84f), new Vector2(1000f, 42f));

            CreateCandidatePanel(canvas.transform, font, "현재 초원 슬라임", new Vector2(.31f, .48f), out slimeImage);
            CreateCandidatePanel(canvas.transform, font, "Pilot Bee", new Vector2(.69f, .48f), out beeImage);
            if (slimeIdle.Length > 0) slimeImage.sprite = slimeIdle[0];
            if (beeIdle.Length > 0) beeImage.sprite = beeIdle[0];
            slimeImage.rectTransform.sizeDelta = Vector2.one * SlimeDisplaySize;
            ApplyBeeDisplaySize();
        }

        private static void CreateCandidatePanel(Transform parent, Font font, string title, Vector2 anchor, out Image candidate)
        {
            Image panel = MakeImage(parent, title + "Panel", new Color(.07f, .105f, .18f, .96f));
            SetRect(panel.rectTransform, anchor, new Vector2(410f, 410f));
            MakeText(panel.transform, "Label", title, font, 24, new Vector2(.5f, .88f), new Vector2(360f, 42f));
            candidate = MakeImage(panel.transform, "Candidate", Color.white);
            candidate.preserveAspect = true;
            candidate.rectTransform.anchorMin = candidate.rectTransform.anchorMax = new Vector2(.5f, .45f);
            candidate.rectTransform.pivot = Vector2.one * .5f;
            candidate.rectTransform.anchoredPosition = Vector2.zero;
        }

        private void ApplyBeeDisplaySize()
        {
            if (beeImage == null) return;
            int cellWidth = attackPlaying ? 315 : 238;
            int cellHeight = attackPlaying ? 253 : 215;
            // 슬라임의 314px 셀을 136 UI px로 표시하는 비율을 공통 기준으로 삼습니다. 여기에 Scale을
            // 곱하면 원본 해상도가 다른 두 그림을 임의의 같은 폭으로 왜곡하지 않고 비교할 수 있습니다.
            float pixelToUi = SlimeDisplaySize / CommonPixelsPerUnit;
            beeImage.rectTransform.sizeDelta = new Vector2(cellWidth * pixelToUi * beeScale,
                cellHeight * pixelToUi * beeScale);
        }

        private void RefreshStateText()
        {
            if (stateText == null) return;
            stateText.text = $"벌 상태: {(attackPlaying ? "Attack" : "Idle")}  |  Scale: {beeScale:0.00} (권장 시작 {RecommendedBeeScale:0.00})  |  {(paused ? "일시정지" : "재생 중")}";
        }

        private static Sprite[] SliceGrid(Texture2D sheet, int cellWidth, int cellHeight, int columns,
            int frameCount, string prefix)
        {
            if (sheet == null || cellWidth <= 0 || cellHeight <= 0 || columns <= 0) return Array.Empty<Sprite>();
            Sprite[] frames = new Sprite[frameCount];
            for (int index = 0; index < frameCount; index++)
            {
                int column = index % columns;
                int rowFromTop = index / columns;
                float y = sheet.height - ((rowFromTop + 1) * cellHeight);
                frames[index] = Sprite.Create(sheet,
                    new Rect(column * cellWidth, y, cellWidth, cellHeight), new Vector2(.5f, .5f),
                    1f, 0, SpriteMeshType.FullRect);
                frames[index].name = $"{prefix}_{index:00}";
            }
            return frames;
        }

        private static Image MakeImage(Transform parent, string objectName, Color color)
        {
            GameObject obj = new GameObject(objectName, typeof(Image));
            obj.transform.SetParent(parent, false);
            Image image = obj.GetComponent<Image>();
            image.color = color;
            image.raycastTarget = false;
            return image;
        }

        private static Text MakeText(Transform parent, string objectName, string value, Font font, int fontSize,
            Vector2 anchor, Vector2 size)
        {
            GameObject obj = new GameObject(objectName, typeof(Text));
            obj.transform.SetParent(parent, false);
            Text text = obj.GetComponent<Text>();
            text.font = font;
            text.fontSize = fontSize;
            text.color = new Color(.94f, .96f, 1f, 1f);
            text.alignment = TextAnchor.MiddleCenter;
            text.text = value;
            text.raycastTarget = false;
            SetRect(text.rectTransform, anchor, size);
            return text;
        }

        private static void SetRect(RectTransform rect, Vector2 anchor, Vector2 size)
        {
            rect.anchorMin = rect.anchorMax = anchor;
            rect.pivot = Vector2.one * .5f;
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = size;
        }

        private static void Stretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
    }
}
