using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.UI
{
    /// <summary>
    /// DialogueSystem GameObject에 붙어 화자 이름과 대사를 화면 아래쪽 패널에 표시합니다.
    /// Scene 어디서든 하나의 Instance를 통해 같은 대화 UI를 사용하도록 관리합니다.
    /// </summary>
    public sealed class DialoguePresenter : MonoBehaviour
    {
        public static DialoguePresenter Instance { get; private set; }

        // 대화에 사용할 글꼴입니다. 비어 있으면 Unity 기본 글꼴을 사용합니다.
        [SerializeField] private Font dialogueFont;
        private GameObject panel;
        private Text dialogueText;

        /// <summary>중복 대화 UI를 제거하고, 이 객체를 공용 Instance로 등록한 뒤 패널을 만듭니다.</summary>
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            CreatePanel();
        }

        /// <summary>현재 공용 Instance가 제거되는 객체라면 참조를 비웁니다.</summary>
        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        /// <summary>화자와 대사를 넣고 대화 패널을 화면에 표시합니다.</summary>
        public void Show(string speaker, string message)
        {
            dialogueText.text = $"{speaker}\n{message}\n\n[Esc 또는 게임패드 B: 닫기]";
            panel.SetActive(true);
        }

        /// <summary>대화 내용을 유지한 채 패널을 화면에서 숨깁니다.</summary>
        public void Hide() => panel.SetActive(false);

        /// <summary>해상도에 맞춰 크기가 조절되는 Canvas와 대화 배경·글자를 코드로 구성합니다.</summary>
        private void CreatePanel()
        {
            GameObject canvasObject = new GameObject("DialogueCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);
            // Screen Space Overlay는 카메라 위치와 관계없이 UI를 화면 위에 고정해 표시합니다.
            canvasObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasObject.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1280, 720);

            panel = new GameObject("DialoguePanel", typeof(Image));
            panel.transform.SetParent(canvasObject.transform, false);
            Image panelImage = panel.GetComponent<Image>();
            panelImage.color = new Color(0.04f, 0.06f, 0.1f, 0.92f);
            RectTransform panelRect = panel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.08f, 0.05f);
            panelRect.anchorMax = new Vector2(0.92f, 0.28f);
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;

            GameObject textObject = new GameObject("DialogueText", typeof(Text));
            textObject.transform.SetParent(panel.transform, false);
            dialogueText = textObject.GetComponent<Text>();
            dialogueText.font = dialogueFont != null
                ? dialogueFont
                : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            dialogueText.fontSize = 28;
            dialogueText.color = Color.white;
            dialogueText.alignment = TextAnchor.MiddleLeft;
            dialogueText.horizontalOverflow = HorizontalWrapMode.Wrap;
            dialogueText.verticalOverflow = VerticalWrapMode.Overflow;
            RectTransform textRect = textObject.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(28f, 20f);
            textRect.offsetMax = new Vector2(-28f, -20f);

            panel.SetActive(false);
        }
    }
}
