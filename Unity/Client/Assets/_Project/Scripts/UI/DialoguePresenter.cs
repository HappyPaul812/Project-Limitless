using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.UI
{
    /// <summary>Milestone 01용 최소 대화 패널을 표시한다.</summary>
    public sealed class DialoguePresenter : MonoBehaviour
    {
        public static DialoguePresenter Instance { get; private set; }

        private GameObject panel;
        private Text dialogueText;

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

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void Show(string speaker, string message)
        {
            dialogueText.text = $"{speaker}\n{message}\n\n[Esc 또는 게임패드 B: 닫기]";
            panel.SetActive(true);
        }

        public void Hide() => panel.SetActive(false);

        private void CreatePanel()
        {
            GameObject canvasObject = new GameObject("DialogueCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);
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
            dialogueText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
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
