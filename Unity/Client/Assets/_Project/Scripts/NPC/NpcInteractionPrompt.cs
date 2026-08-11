using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.NPC
{
    /// <summary>
    /// NPC GameObject에 붙어 머리 위에 “[E] 대화하기” 안내를 표시합니다.
    /// World Space UI는 게임 세계의 위치를 따라다니는 UI이며, InteractionSystem이 가장 가까운 NPC의 안내만 켭니다.
    /// </summary>
    [RequireComponent(typeof(NpcController))]
    public sealed class NpcInteractionPrompt : MonoBehaviour
    {
        // 안내 문구에 사용할 글꼴입니다. 비어 있으면 Unity 기본 글꼴을 사용합니다.
        [SerializeField] private Font promptFont;

        private GameObject promptRoot;

        /// <summary>NPC에 안내 컴포넌트가 있으면 가져오고, 없으면 자동으로 추가합니다.</summary>
        public static NpcInteractionPrompt GetOrAdd(NpcController npc)
        {
            NpcInteractionPrompt prompt = npc.GetComponent<NpcInteractionPrompt>();
            return prompt != null ? prompt : npc.gameObject.AddComponent<NpcInteractionPrompt>();
        }

        /// <summary>처음 만들어질 때 안내 UI를 준비하되 기본 상태는 숨깁니다.</summary>
        private void Awake()
        {
            CreatePrompt();
            SetVisible(false);
        }

        /// <summary>다시 활성화될 때 UI가 없다면 복구하고 숨긴 상태로 시작합니다.</summary>
        private void OnEnable()
        {
            if (promptRoot == null)
            {
                CreatePrompt();
                promptRoot.SetActive(false);
            }
        }

        /// <summary>이 NPC가 현재 상호작용 대상인지에 따라 안내를 보이거나 숨깁니다.</summary>
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

        /// <summary>NPC의 자식으로 배경과 글자가 포함된 World Space Canvas를 만듭니다.</summary>
        private void CreatePrompt()
        {
            if (promptRoot != null)
            {
                return;
            }

            promptRoot = new GameObject("InteractionPrompt", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            promptRoot.transform.SetParent(transform, false);
            promptRoot.transform.localPosition = new Vector3(0f, 1.1f, 0f);
            // UI 픽셀 크기를 게임 월드 단위에 맞게 작게 줄입니다.
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

        /// <summary>UI 요소가 부모 사각형을 빈틈없이 채우도록 네 방향 기준점과 여백을 설정합니다.</summary>
        private static void Stretch(RectTransform rectTransform, Vector2 offsetMin, Vector2 offsetMax)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = offsetMin;
            rectTransform.offsetMax = offsetMax;
        }
    }
}
