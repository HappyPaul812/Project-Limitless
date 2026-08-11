using ProjectLimitless.Core;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.Player
{
    /// <summary>
    /// Character Creation에서 정한 이름을 플레이어 머리 위에 표시합니다.
    /// 이 컴포넌트는 Male/Female Visual과 분리된 Player 부모에 있으므로 외형이 바뀌어도 같은 이름표를 사용합니다.
    /// </summary>
    public sealed class PlayerNameplate : MonoBehaviour
    {
        private const string FallbackPlayerName = "플레이어";

        // 128x128, PPU 128인 현재 캐릭터의 머리보다 조금 위쪽인 공통 위치입니다.
        private static readonly Vector3 NameplatePosition = new Vector3(0f, 1.2f, 0f);

        private GameObject nameplateRoot;

        /// <summary>Player가 만들어질 때 현재 세션 이름으로 World Space 이름표를 준비합니다.</summary>
        private void Awake()
        {
            CreateNameplate();
        }

        /// <summary>비활성화 후 다시 켜졌을 때 이름표가 사라진 예외 상황을 복구합니다.</summary>
        private void OnEnable()
        {
            if (nameplateRoot == null)
            {
                CreateNameplate();
            }
        }

        /// <summary>GameSessionData의 이름을 읽고, 비어 있으면 안전한 기본 이름을 반환합니다.</summary>
        public static string GetDisplayName()
        {
            return string.IsNullOrWhiteSpace(GameSessionData.PlayerName)
                ? FallbackPlayerName
                : GameSessionData.PlayerName;
        }

        /// <summary>Player의 공통 자식으로 배경과 글자가 들어 있는 World Space Canvas를 만듭니다.</summary>
        private void CreateNameplate()
        {
            if (nameplateRoot != null)
            {
                return;
            }

            nameplateRoot = new GameObject("PlayerNameplate", typeof(Canvas), typeof(CanvasScaler));
            nameplateRoot.transform.SetParent(transform, false);
            nameplateRoot.transform.localPosition = NameplatePosition;
            // UI의 픽셀 단위를 작은 게임 월드 단위로 바꾸어 캐릭터보다 과도하게 커지지 않게 합니다.
            nameplateRoot.transform.localScale = Vector3.one * 0.005f;

            Canvas canvas = nameplateRoot.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.sortingOrder = 25;

            CanvasScaler scaler = nameplateRoot.GetComponent<CanvasScaler>();
            scaler.dynamicPixelsPerUnit = 100f;

            RectTransform canvasRect = nameplateRoot.GetComponent<RectTransform>();
            canvasRect.sizeDelta = new Vector2(300f, 48f);

            GameObject backgroundObject = new GameObject("Background", typeof(Image), typeof(Outline));
            backgroundObject.transform.SetParent(nameplateRoot.transform, false);
            Image background = backgroundObject.GetComponent<Image>();
            background.color = new Color(0.035f, 0.07f, 0.12f, 0.88f);
            Outline border = backgroundObject.GetComponent<Outline>();
            border.effectColor = new Color(0.35f, 0.75f, 1f, 0.95f);
            border.effectDistance = new Vector2(2f, -2f);
            Stretch(backgroundObject.GetComponent<RectTransform>(), Vector2.zero, Vector2.zero);

            GameObject textObject = new GameObject("NameText", typeof(Text), typeof(Shadow));
            textObject.transform.SetParent(backgroundObject.transform, false);
            Text nameText = textObject.GetComponent<Text>();
            // Unity 6에서 Arial.ttf 대신 프로젝트의 기존 한글 대응 기본 폰트를 사용합니다.
            nameText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            nameText.fontSize = 24;
            nameText.resizeTextForBestFit = true;
            nameText.resizeTextMinSize = 16;
            nameText.resizeTextMaxSize = 24;
            nameText.color = new Color(0.85f, 0.95f, 1f, 1f);
            nameText.alignment = TextAnchor.MiddleCenter;
            nameText.text = GetDisplayName();
            Shadow shadow = textObject.GetComponent<Shadow>();
            shadow.effectColor = new Color(0f, 0f, 0f, 0.9f);
            shadow.effectDistance = new Vector2(2f, -2f);
            Stretch(textObject.GetComponent<RectTransform>(), new Vector2(10f, 3f), new Vector2(-10f, -3f));
        }

        /// <summary>UI 요소가 부모 사각형을 채우도록 기준점과 안쪽 여백을 설정합니다.</summary>
        private static void Stretch(RectTransform rectTransform, Vector2 offsetMin, Vector2 offsetMax)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = offsetMin;
            rectTransform.offsetMax = offsetMax;
        }
    }
}
