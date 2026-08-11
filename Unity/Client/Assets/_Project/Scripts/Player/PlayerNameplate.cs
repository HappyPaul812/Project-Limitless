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

        // 128x128, PPU 128인 현재 캐릭터의 머리 바로 위쪽인 공통 위치입니다.
        private static readonly Vector3 NameplatePosition = new Vector3(0f, 1.16f, 0f);

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
            string playerName = GameSessionData.PlayerName?.Trim();
            return string.IsNullOrEmpty(playerName) ? FallbackPlayerName : playerName;
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
            // RectTransform과 함께 조정한 배율입니다. 긴 이름 공간은 확보하면서 캐릭터보다 과도하게 커지지 않습니다.
            nameplateRoot.transform.localScale = Vector3.one * 0.0065f;

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
            // Canvas도 고정된 중앙 기준 크기를 가져야 자식 Text의 좌표 기준이 0이 되지 않습니다.
            canvasRect.anchorMin = new Vector2(0.5f, 0.5f);
            canvasRect.anchorMax = new Vector2(0.5f, 0.5f);
            canvasRect.pivot = new Vector2(0.5f, 0.5f);
            canvasRect.sizeDelta = new Vector2(280f, 44f);

            // 배경 Image가 글자 렌더링을 방해할 가능성을 없애기 위해 이름은 Canvas의 직접 자식으로 둡니다.
            GameObject textObject = new GameObject("NameText", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text), typeof(Outline));
            textObject.transform.SetParent(nameplateRoot.transform, false);
            nameText = textObject.GetComponent<Text>();
            // Unity 6에서 Arial.ttf 대신 프로젝트의 기존 한글 대응 기본 폰트를 사용합니다.
            nameText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            nameText.fontSize = 28;
            nameText.resizeTextForBestFit = false;
            nameText.horizontalOverflow = HorizontalWrapMode.Overflow;
            nameText.verticalOverflow = VerticalWrapMode.Overflow;
            nameText.color = Color.white;
            nameText.alignment = TextAnchor.MiddleCenter;
            nameText.raycastTarget = false;
            Outline outline = textObject.GetComponent<Outline>();
            outline.effectColor = new Color(0f, 0f, 0f, 0.95f);
            outline.effectDistance = new Vector2(2f, -2f);
            RectTransform textRect = textObject.GetComponent<RectTransform>();
            // Stretch를 사용하면 sizeDelta가 Left/Right/Top/Bottom 여백으로 해석됩니다.
            // 고정 중앙 Anchor를 먼저 지정한 뒤 실제 폭과 높이를 명시해 Inspector에서도 280×44로 보이게 합니다.
            textRect.anchorMin = new Vector2(0.5f, 0.5f);
            textRect.anchorMax = new Vector2(0.5f, 0.5f);
            textRect.pivot = new Vector2(0.5f, 0.5f);
            textRect.anchoredPosition = Vector2.zero;
            textRect.sizeDelta = new Vector2(280f, 44f);
            textRect.localScale = Vector3.one;
            // UI는 뒤에 생성된 sibling이 위에 그려집니다. 이름을 마지막에 두어 다른 요소가 덮지 못하게 합니다.
            textRect.SetAsLastSibling();
            RefreshName();
        }

        /// <summary>현재 GameSessionData.PlayerName을 Text에 넣고, 이름이 비었을 때만 기본값을 사용합니다.</summary>
        private void RefreshName()
        {
            if (nameText != null)
            {
                nameText.text = GetDisplayName();
                nameText.enabled = true;
                nameText.gameObject.SetActive(true);
                nameText.SetAllDirty();
                Canvas.ForceUpdateCanvases();
            }
        }

    }
}
