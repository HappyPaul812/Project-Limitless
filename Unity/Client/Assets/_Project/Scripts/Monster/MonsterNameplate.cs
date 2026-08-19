using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.Monster
{
    /// <summary>
    /// 필드 몬스터의 이름을 머리 위에 표시하는 재사용 가능한 Screen Space Overlay 이름표입니다.
    /// `FieldMonsterInstaller`가 만든 몬스터에 붙으며, 몬스터가 이동해도 같은 대상을 화면 좌표로 계속 추적합니다.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class MonsterNameplate : MonoBehaviour
    {
        private const string FallbackMonsterName = "몬스터";
        private const string OverlayCanvasName = "MonsterNameOverlayCanvas";
        private const int MonsterFontSize = 21;

        // 현재 슬라임 Sprite의 아래쪽 Pivot을 기준으로 머리 위까지 올리는 월드 좌표 거리입니다.
        private static readonly Vector3 NameWorldOffset = new Vector3(0f, 1.05f, 0f);

        private RectTransform nameTextRect;
        private Text nameText;
        private Camera worldCamera;
        private string displayName = FallbackMonsterName;

        /// <summary>몬스터가 생성될 때 이름표가 사용할 공용 Overlay Canvas와 전용 Text를 준비합니다.</summary>
        private void Awake()
        {
            EnsureOverlayNameplate();
        }

        /// <summary>비활성화 후 다시 켜졌을 때 이름표도 함께 복구하고 현재 이름을 다시 표시합니다.</summary>
        private void OnEnable()
        {
            EnsureOverlayNameplate();
            RefreshName();
        }

        /// <summary>몬스터가 비활성화된 동안 화면에 이름만 남지 않도록 Text도 숨깁니다.</summary>
        private void OnDisable()
        {
            if (nameText != null) nameText.gameObject.SetActive(false);
        }

        /// <summary>
        /// 설치기가 MonsterDefinition을 읽은 직후 호출하여 표시 이름을 전달합니다.
        /// 같은 컴포넌트를 다른 몬스터 종류에 재사용하면 해당 데이터의 DisplayName이 그대로 표시됩니다.
        /// </summary>
        public void Configure(string monsterDisplayName)
        {
            string trimmedName = monsterDisplayName?.Trim();
            displayName = string.IsNullOrEmpty(trimmedName) ? FallbackMonsterName : trimmedName;
            RefreshName();
        }

        /// <summary>
        /// 몬스터와 Camera의 이동이 끝난 뒤 머리 위 월드 좌표를 화면 좌표로 바꿉니다.
        /// Overlay UI이므로 Camera 확대·축소와 관계없이 글자 크기와 외곽선 두께가 일정하게 유지됩니다.
        /// </summary>
        private void LateUpdate()
        {
            if (nameTextRect == null) EnsureOverlayNameplate();
            if (worldCamera == null) worldCamera = Camera.main;
            if (worldCamera == null || nameTextRect == null) return;

            Vector3 screenPosition = worldCamera.WorldToScreenPoint(transform.position + NameWorldOffset);
            bool isInFrontOfCamera = screenPosition.z > 0f;
            nameText.gameObject.SetActive(isInFrontOfCamera);
            if (isInFrontOfCamera) nameTextRect.position = screenPosition;
        }

        /// <summary>몬스터가 제거될 때 공용 Canvas는 유지하고 이 몬스터가 소유한 Text만 정리합니다.</summary>
        private void OnDestroy()
        {
            if (nameText != null) Destroy(nameText.gameObject);
        }

        /// <summary>
        /// Scene의 모든 몬스터가 함께 쓰는 Overlay Canvas를 찾거나 만들고, 이 몬스터만의 Text를 생성합니다.
        /// 공용 Canvas 아래에 Text를 각각 두므로 몬스터가 여러 마리여도 이름이 서로 덮어쓰이지 않습니다.
        /// </summary>
        private void EnsureOverlayNameplate()
        {
            GameObject canvasObject = GameObject.Find(OverlayCanvasName);
            if (canvasObject == null)
                canvasObject = new GameObject(OverlayCanvasName, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler));

            Canvas canvas = canvasObject.GetComponent<Canvas>();
            if (canvas == null) canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 5;
            canvas.enabled = true;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            if (scaler == null) scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            scaler.scaleFactor = 1f;

            if (nameText == null)
            {
                GameObject textObject = new GameObject(
                    $"NameText_{GetInstanceID()}",
                    typeof(RectTransform),
                    typeof(CanvasRenderer),
                    typeof(Text),
                    typeof(Outline));
                textObject.transform.SetParent(canvasObject.transform, false);
                nameTextRect = textObject.GetComponent<RectTransform>();
                nameText = textObject.GetComponent<Text>();
            }

            // PlayerNameplate와 같은 Unity 기본 한글 대응 폰트를 사용해 "초원 슬라임"이 깨지지 않게 합니다.
            nameText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            nameText.fontSize = MonsterFontSize;
            nameText.resizeTextForBestFit = false;
            nameText.horizontalOverflow = HorizontalWrapMode.Overflow;
            nameText.verticalOverflow = VerticalWrapMode.Overflow;
            nameText.color = Color.white;
            nameText.alignment = TextAnchor.MiddleCenter;
            nameText.raycastTarget = false;

            Outline outline = nameText.GetComponent<Outline>();
            outline.effectColor = new Color(0f, 0f, 0f, .95f);
            outline.effectDistance = new Vector2(1f, -1f);

            nameTextRect.anchorMin = new Vector2(.5f, .5f);
            nameTextRect.anchorMax = new Vector2(.5f, .5f);
            nameTextRect.pivot = new Vector2(.5f, .5f);
            nameTextRect.sizeDelta = new Vector2(220f, 32f);
            nameTextRect.localScale = Vector3.one;
            RefreshName();
        }

        /// <summary>현재 MonsterDefinition에서 받은 이름을 Text에 반영합니다.</summary>
        private void RefreshName()
        {
            if (nameText == null) return;

            nameText.text = displayName;
            nameText.enabled = true;
            nameText.gameObject.SetActive(isActiveAndEnabled);
            nameText.SetAllDirty();
        }
    }
}
