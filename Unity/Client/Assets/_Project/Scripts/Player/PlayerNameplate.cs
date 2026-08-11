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
        private const string OverlayCanvasName = "PlayerNameOverlayCanvas";

        // 128x128, PPU 128인 현재 캐릭터의 머리 바로 위를 가리키는 월드 좌표 오프셋입니다.
        private static readonly Vector3 NameWorldOffset = new Vector3(0f, 1.15f, 0f);

        private GameObject overlayCanvasObject;
        private RectTransform nameTextRect;
        private Text nameText;
        private Camera worldCamera;
        private bool ownsOverlayCanvas;

        /// <summary>Player가 만들어질 때 월드 크기와 무관한 Screen Space Overlay 이름표를 준비합니다.</summary>
        private void Awake()
        {
            RemoveLegacyWorldSpaceNameplate();
            EnsureOverlayNameplate();
        }

        /// <summary>비활성화 후 다시 켜졌을 때 이름표가 사라진 예외 상황을 복구합니다.</summary>
        private void OnEnable()
        {
            EnsureOverlayNameplate();
            RefreshName();
        }

        /// <summary>Player가 비활성화된 동안 화면에 이름만 남지 않도록 Text도 함께 숨깁니다.</summary>
        private void OnDisable()
        {
            if (nameText != null)
            {
                nameText.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 다른 Component의 Awake가 모두 끝난 다음 세션 이름을 다시 읽습니다.
        /// Scene 전환 직후에도 Character Creation에서 확정한 이름이 최종 표시되도록 하는 안전장치입니다.
        /// </summary>
        private void Start()
        {
            EnsureOverlayNameplate();
            RefreshName();
        }

        /// <summary>
        /// Player와 Camera가 모두 이동한 뒤 머리 위 월드 좌표를 화면 좌표로 바꿉니다.
        /// Overlay UI이므로 Camera 확대·축소가 바뀌어도 글자 자체의 픽셀 크기는 변하지 않습니다.
        /// </summary>
        private void LateUpdate()
        {
            if (nameTextRect == null)
            {
                EnsureOverlayNameplate();
            }

            if (worldCamera == null)
            {
                worldCamera = Camera.main;
            }

            if (worldCamera == null || nameTextRect == null)
            {
                return;
            }

            Vector3 screenPosition = worldCamera.WorldToScreenPoint(transform.position + NameWorldOffset);
            bool isInFrontOfCamera = screenPosition.z > 0f;
            nameText.gameObject.SetActive(isInFrontOfCamera);
            if (isInFrontOfCamera)
            {
                nameTextRect.position = screenPosition;
            }
        }

        /// <summary>이 Player가 만든 전용 Overlay Canvas를 Scene 종료 시 함께 정리합니다.</summary>
        private void OnDestroy()
        {
            if (ownsOverlayCanvas && overlayCanvasObject != null)
            {
                Destroy(overlayCanvasObject);
            }
        }

        /// <summary>GameSessionData의 이름을 읽고, 비어 있으면 안전한 기본 이름을 반환합니다.</summary>
        public static string GetDisplayName()
        {
            string playerName = GameSessionData.PlayerName?.Trim();
            return string.IsNullOrEmpty(playerName) ? FallbackPlayerName : playerName;
        }

        /// <summary>Scene 루트에 이름표 전용 Screen Space Overlay Canvas와 uGUI Text를 만듭니다.</summary>
        private void EnsureOverlayNameplate()
        {
            if (overlayCanvasObject == null)
            {
                overlayCanvasObject = GameObject.Find(OverlayCanvasName);
            }

            if (overlayCanvasObject == null)
            {
                overlayCanvasObject = new GameObject(OverlayCanvasName, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler));
                ownsOverlayCanvas = true;
            }

            overlayCanvasObject.SetActive(true);
            Canvas canvas = overlayCanvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 5;
            canvas.enabled = true;

            CanvasScaler scaler = overlayCanvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            scaler.scaleFactor = 1f;

            Transform existingText = overlayCanvasObject.transform.Find("NameText");
            if (existingText == null)
            {
                GameObject textObject = new GameObject("NameText", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text), typeof(Outline));
                textObject.transform.SetParent(overlayCanvasObject.transform, false);
                existingText = textObject.transform;
            }

            nameTextRect = existingText.GetComponent<RectTransform>();
            nameText = existingText.GetComponent<Text>();
            // Unity 6에서 Arial.ttf 대신 프로젝트의 기존 한글 대응 기본 폰트를 사용합니다.
            nameText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            nameText.fontSize = 24;
            nameText.resizeTextForBestFit = false;
            nameText.horizontalOverflow = HorizontalWrapMode.Overflow;
            nameText.verticalOverflow = VerticalWrapMode.Overflow;
            nameText.color = Color.white;
            nameText.alignment = TextAnchor.MiddleCenter;
            nameText.raycastTarget = false;
            Outline outline = existingText.GetComponent<Outline>();
            outline.effectColor = new Color(0f, 0f, 0f, 0.95f);
            outline.effectDistance = new Vector2(1f, -1f);
            nameTextRect.anchorMin = new Vector2(0.5f, 0.5f);
            nameTextRect.anchorMax = new Vector2(0.5f, 0.5f);
            nameTextRect.pivot = new Vector2(0.5f, 0.5f);
            nameTextRect.sizeDelta = new Vector2(240f, 36f);
            nameTextRect.localScale = Vector3.one;
            nameTextRect.SetAsLastSibling();
            RefreshName();
        }

        /// <summary>이전 버전이 Player 자식으로 만들던 World Space Canvas가 남아 있으면 중복 표시 전에 제거합니다.</summary>
        private void RemoveLegacyWorldSpaceNameplate()
        {
            Transform legacyNameplate = transform.Find("PlayerNameplate");
            if (legacyNameplate != null)
            {
                Destroy(legacyNameplate.gameObject);
            }
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
