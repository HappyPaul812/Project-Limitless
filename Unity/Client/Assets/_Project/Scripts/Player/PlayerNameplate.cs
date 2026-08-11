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
        private Text nameText;

        /// <summary>Player가 만들어질 때 현재 세션 이름으로 World Space 이름표를 준비합니다.</summary>
        private void Awake()
        {
            EnsureNameplate();
        }

        /// <summary>비활성화 후 다시 켜졌을 때 이름표가 사라진 예외 상황을 복구합니다.</summary>
        private void OnEnable()
        {
            EnsureNameplate();
            RefreshName();
        }

        /// <summary>
        /// 다른 Component의 Awake가 모두 끝난 다음 세션 이름을 다시 읽습니다.
        /// Scene 전환 직후에도 Character Creation에서 확정한 이름이 최종 표시되도록 하는 안전장치입니다.
        /// </summary>
        private void Start()
        {
            EnsureNameplate();
            RefreshName();
        }

        /// <summary>GameSessionData의 이름을 읽고, 비어 있으면 안전한 기본 이름을 반환합니다.</summary>
        public static string GetDisplayName()
        {
            return string.IsNullOrWhiteSpace(GameSessionData.PlayerName)
                ? FallbackPlayerName
                : GameSessionData.PlayerName;
        }

        /// <summary>Player의 공통 자식으로 배경과 글자가 들어 있는 World Space Canvas를 만듭니다.</summary>
        private void EnsureNameplate()
        {
            if (nameplateRoot == null)
            {
                Transform existingNameplate = transform.Find("PlayerNameplate");
                nameplateRoot = existingNameplate != null ? existingNameplate.gameObject : null;
            }

            if (nameplateRoot != null)
            {
                nameplateRoot.SetActive(true);
                nameText = nameplateRoot.GetComponentInChildren<Text>(true);
                return;
            }

            nameplateRoot = new GameObject("PlayerNameplate", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler));
            nameplateRoot.transform.SetParent(transform, false);
            nameplateRoot.transform.localPosition = NameplatePosition;
            nameplateRoot.layer = gameObject.layer;
            // 기존 0.005 배율은 실제 Game View에서 글자가 지나치게 작았습니다. NPC 안내와 같은 0.01로 키워 읽을 수 있게 합니다.
            nameplateRoot.transform.localScale = Vector3.one * 0.01f;

            Canvas canvas = nameplateRoot.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            // 독립 World Space Canvas의 정렬을 강제로 사용해 Sprite와 환경 오브젝트보다 항상 앞에 표시합니다.
            canvas.overrideSorting = true;
            canvas.sortingLayerName = "Default";
            canvas.sortingOrder = 1000;
            canvas.enabled = true;

            CanvasScaler scaler = nameplateRoot.GetComponent<CanvasScaler>();
            scaler.dynamicPixelsPerUnit = 100f;

            RectTransform canvasRect = nameplateRoot.GetComponent<RectTransform>();
            canvasRect.sizeDelta = new Vector2(220f, 38f);

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
            nameText = textObject.GetComponent<Text>();
            // Unity 6에서 Arial.ttf 대신 프로젝트의 기존 한글 대응 기본 폰트를 사용합니다.
            nameText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            nameText.fontSize = 24;
            nameText.resizeTextForBestFit = true;
            nameText.resizeTextMinSize = 16;
            nameText.resizeTextMaxSize = 24;
            nameText.color = new Color(0.85f, 0.95f, 1f, 1f);
            nameText.alignment = TextAnchor.MiddleCenter;
            Shadow shadow = textObject.GetComponent<Shadow>();
            shadow.effectColor = new Color(0f, 0f, 0f, 0.9f);
            shadow.effectDistance = new Vector2(2f, -2f);
            Stretch(textObject.GetComponent<RectTransform>(), new Vector2(10f, 3f), new Vector2(-10f, -3f));
            RefreshName();
        }

        /// <summary>현재 GameSessionData.PlayerName을 Text에 넣고, 이름이 비었을 때만 기본값을 사용합니다.</summary>
        private void RefreshName()
        {
            if (nameText != null)
            {
                nameText.text = GetDisplayName();
                nameText.enabled = true;
            }
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
