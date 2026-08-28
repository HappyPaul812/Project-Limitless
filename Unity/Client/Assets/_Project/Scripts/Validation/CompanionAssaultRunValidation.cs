using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.Validation
{
    /// <summary>
    /// 실제 전투에 연결하지 않고 Wolf/Fox/Bear의 달리기만 같은 조건에서 비교하는 임시 검증 화면입니다.
    /// 최종 동물을 선택한 뒤 Scripts/Validation과 Resources/CompanionAssaultValidation을 함께 제거할 수 있습니다.
    /// </summary>
    public sealed class CompanionAssaultRunValidation : MonoBehaviour
    {
        private const int FrameWidth = 64;
        private const float DefaultFramesPerSecond = 12f;
        private const float DisplayScale = 2f;
        private const float TrackHalfWidth = 500f;
        private const float TravelSpeed = 280f;

        private readonly List<CandidateLane> lanes = new List<CandidateLane>();
        private float framesPerSecond = DefaultFramesPerSecond;
        private float elapsed;
        private bool paused;
        private bool movingLeft = true;
        private Text helpText;

        private sealed class CandidateLane
        {
            public string Name;
            public Sprite[] Frames;
            public RectTransform Animal;
            public Image Image;
            public float PositionX;
        }

        private void Start()
        {
            BuildComparisonScreen();
        }

        private void Update()
        {
            HandleKeyboard();
            if (paused || lanes.Count == 0) return;

            elapsed += Time.unscaledDeltaTime;
            int frameIndex = Mathf.FloorToInt(elapsed * framesPerSecond);
            float direction = movingLeft ? -1f : 1f;

            foreach (CandidateLane lane in lanes)
            {
                if (lane.Frames.Length == 0) continue;

                lane.Image.sprite = lane.Frames[frameIndex % lane.Frames.Length];

                // Sprite 프레임 교체는 제자리에서 다리를 움직이는 그림만 담당하고,
                // Transform 이동은 동물이 전장을 가로지르는 위치만 담당합니다.
                // 둘을 분리해야 나중에 달리기 FPS와 돌진 속도를 서로 독립적으로 조절할 수 있습니다.
                lane.PositionX += direction * TravelSpeed * Time.unscaledDeltaTime;
                if (movingLeft && lane.PositionX < -TrackHalfWidth) lane.PositionX = TrackHalfWidth;
                if (!movingLeft && lane.PositionX > TrackHalfWidth) lane.PositionX = -TrackHalfWidth;
                lane.Animal.anchoredPosition = new Vector2(lane.PositionX, 0f);
            }
        }

        private void HandleKeyboard()
        {
            if (Input.GetKeyDown(KeyCode.Space)) paused = !paused;
            if (Input.GetKeyDown(KeyCode.F))
            {
                movingLeft = !movingLeft;
                foreach (CandidateLane lane in lanes)
                {
                    // 원본은 왼쪽을 바라봅니다. 같은 프레임을 오른쪽으로 재사용할 때는
                    // 이미지를 다시 만들지 않고 X축 Scale만 반대로 뒤집습니다.
                    lane.Animal.localScale = new Vector3(movingLeft ? 1f : -1f, 1f, 1f);
                }
            }

            if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus))
                framesPerSecond = Mathf.Min(24f, framesPerSecond + 1f);
            if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
                framesPerSecond = Mathf.Max(1f, framesPerSecond - 1f);

            if (helpText != null)
                helpText.text = $"Space: 일시정지  |  F: 방향 Flip  |  +/-: Run FPS ({framesPerSecond:0})  |  이동: {(movingLeft ? "오른쪽 → 왼쪽" : "왼쪽 → 오른쪽")}";
        }

        private void BuildComparisonScreen()
        {
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            Canvas canvas = new GameObject("ValidationCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster)).GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280f, 720f);

            Image background = MakeImage(canvas.transform, "Background", new Color(.035f, .055f, .11f, 1f));
            Stretch(background.rectTransform);
            MakeText(canvas.transform, "Title", "동료의 습격 · Run 후보 비교 (전투 미연결)", font, 27, new Vector2(.5f, .94f), new Vector2(900f, 48f));
            helpText = MakeText(canvas.transform, "Help", string.Empty, font, 17, new Vector2(.5f, .055f), new Vector2(1000f, 36f));

            CreateLane(canvas.transform, font, "Wolf", "CompanionAssaultValidation/Wolf_Run", .74f);
            CreateLane(canvas.transform, font, "Fox", "CompanionAssaultValidation/Fox_Run", .50f);
            CreateLane(canvas.transform, font, "Bear", "CompanionAssaultValidation/Bear_Run", .26f);
            HandleKeyboard();
        }

        private void CreateLane(Transform canvas, Font font, string displayName, string resourcePath, float anchorY)
        {
            Image track = MakeImage(canvas, $"{displayName}Track", new Color(.07f, .105f, .18f, .96f));
            SetRect(track.rectTransform, new Vector2(.56f, anchorY), new Vector2(1060f, 132f));
            MakeText(track.transform, "Label", displayName, font, 22, new Vector2(.055f, .72f), new Vector2(110f, 38f));
            MakeText(track.transform, "Scale", "원본 2배\nBattle 표시폭 참고: 아군 112 / 적 136px", font, 13, new Vector2(.10f, .29f), new Vector2(220f, 48f));

            Texture2D sheet = Resources.Load<Texture2D>(resourcePath);
            Sprite[] frames = SliceRunSheet(sheet, displayName);
            if (frames.Length == 0)
            {
                MakeText(track.transform, "Missing", "Run SpriteSheet를 불러오지 못했습니다.", font, 18, new Vector2(.58f, .5f), new Vector2(500f, 40f));
                return;
            }

            Image animal = MakeImage(track.transform, "Animal", Color.white);
            animal.preserveAspect = true;
            animal.sprite = frames[0];
            animal.rectTransform.anchorMin = animal.rectTransform.anchorMax = new Vector2(.5f, .22f);
            animal.rectTransform.pivot = new Vector2(.5f, 0f);
            animal.rectTransform.sizeDelta = new Vector2(FrameWidth * DisplayScale, sheet.height * DisplayScale);
            animal.rectTransform.anchoredPosition = new Vector2(TrackHalfWidth, 0f);

            lanes.Add(new CandidateLane
            {
                Name = displayName,
                Frames = frames,
                Animal = animal.rectTransform,
                Image = animal,
                PositionX = TrackHalfWidth
            });
        }

        private static Sprite[] SliceRunSheet(Texture2D sheet, string candidateName)
        {
            if (sheet == null || sheet.width % FrameWidth != 0) return Array.Empty<Sprite>();

            int frameCount = sheet.width / FrameWidth;
            Sprite[] frames = new Sprite[frameCount];
            for (int index = 0; index < frameCount; index++)
            {
                // 한 장의 SpriteSheet는 여러 동작 순간을 가로로 이어 둔 그림입니다.
                // 64px씩 잘라 순서대로 바꾸면 원본을 수정하지 않고 달리기 애니메이션이 됩니다.
                Rect rect = new Rect(index * FrameWidth, 0f, FrameWidth, sheet.height);

                // 모든 프레임의 Pivot을 아래 중앙으로 통일하면 몸의 폭과 높이가 조금 달라져도
                // 발이 닿는 기준점이 유지되어 애니메이션 전체가 불필요하게 흔들리지 않습니다.
                frames[index] = Sprite.Create(sheet, rect, new Vector2(.5f, 0f), 1f, 0, SpriteMeshType.FullRect);
                frames[index].name = $"{candidateName}_Run_{index:00}";
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

        private static Text MakeText(Transform parent, string objectName, string value, Font font, int fontSize, Vector2 anchor, Vector2 size)
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
