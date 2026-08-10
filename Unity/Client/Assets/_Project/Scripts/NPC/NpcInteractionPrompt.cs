using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.NPC
{
    /// <summary>NPC 위에 상호작용 가능 여부를 UI Text로 표시한다.</summary>
    [RequireComponent(typeof(NpcController))]
    public sealed class NpcInteractionPrompt : MonoBehaviour
    {
        [SerializeField] private Font promptFont;

        private GameObject promptRoot;

        public static NpcInteractionPrompt GetOrAdd(NpcController npc)
        {
            NpcInteractionPrompt prompt = npc.GetComponent<NpcInteractionPrompt>();
            return prompt != null ? prompt : npc.gameObject.AddComponent<NpcInteractionPrompt>();
        }

        private void Awake()
        {
            CreatePrompt();
            SetVisible(false);
        }

        private void OnEnable()
        {
            if (promptRoot == null)
            {
                CreatePrompt();
                promptRoot.SetActive(false);
            }
        }

        public void SetVisible(bool isVisible)
        {
            if (promptRoot == null)
            {
                CreatePrompt();
            }

            if (promptRoot != null)
            {
                promptRoot.SetActive(isVisible);
            }
        }

        private void CreatePrompt()
        {
            if (promptRoot != null)
            {
                return;
            }

            promptRoot = new GameObject("InteractionPrompt", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            promptRoot.transform.SetParent(transform, false);
            promptRoot.transform.localPosition = new Vector3(0f, 1.1f, 0f);
            promptRoot.transform.localScale = Vector3.one * 0.01f;

            Canvas canvas = promptRoot.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.sortingOrder = 20;

            RectTransform canvasRect = promptRoot.GetComponent<RectTransform>();
            canvasRect.sizeDelta = new Vector2(280f, 54f);

            GameObject backgroundObject = new GameObject("Background", typeof(Image));
            backgroundObject.transform.SetParent(promptRoot.transform, false);
            Image background = backgroundObject.GetComponent<Image>();
            background.color = new Color(0.03f, 0.05f, 0.09f, 0.9f);
            Stretch(backgroundObject.GetComponent<RectTransform>(), Vector2.zero, Vector2.zero);

            GameObject textObject = new GameObject("PromptText", typeof(Text));
            textObject.transform.SetParent(backgroundObject.transform, false);
            Text promptText = textObject.GetComponent<Text>();
            promptText.font = promptFont != null
                ? promptFont
                : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            promptText.fontSize = 28;
            promptText.color = Color.white;
            promptText.alignment = TextAnchor.MiddleCenter;
            promptText.text = "[E] 대화하기";
            Stretch(textObject.GetComponent<RectTransform>(), new Vector2(10f, 4f), new Vector2(-10f, -4f));
        }

        private static void Stretch(RectTransform rectTransform, Vector2 offsetMin, Vector2 offsetMax)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = offsetMin;
            rectTransform.offsetMax = offsetMax;
        }
    }
}
