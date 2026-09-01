using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectLimitless.Core
{
    /// <summary>
    /// 게임 시작용 Bootstrap에서 저장 유무에 따라 새 게임과 이어하기를 선택하게 합니다.
    /// </summary>
    public sealed class BootstrapLoader : MonoBehaviour
    {
        // 시작 직후 불러올 Scene 이름입니다. private이지만 SerializeField 덕분에 Inspector에서 바꿀 수 있습니다.
        [SerializeField] private string worldSceneName = "CharacterCreation";
        private GameSaveData continueData;

        /// <summary>Editor 생성 도구가 게임 시작 시 불러올 Scene을 설정합니다.</summary>
        public void ConfigureStartScene(string sceneName)
        {
            worldSceneName = sceneName;
        }

        /// <summary>기존 Scene Asset을 다시 저장하지 않고 최소 시작 UI를 실행 중에 만듭니다.</summary>
        private void Start()
        {
            GameSessionData.Reset();
            GameSaveService.TryLoad(out continueData);
            CreateStartInterface();
        }

        private void StartNewGame()
        {
            // 새 게임 선택만으로 기존 파일을 지우지 않습니다. 최종 확인에서 새 캐릭터를 확정했을 때만
            // 사용자의 명시적인 새 진행으로 판단해 정상 저장 API가 덮어씁니다.
            GameSessionData.Reset();
            SceneManager.LoadSceneAsync(worldSceneName, LoadSceneMode.Single);
        }

        private void ContinueGame()
        {
            if (continueData == null) return;
            try
            {
                // 저장된 문자열 ID를 세션에 복원하면 기존 Player Visual/Path/Job Resolver가 평소와 똑같이
                // 해당 ScriptableObject와 Sprite를 찾아 적용하므로 Unity Object를 파일에 넣을 필요가 없습니다.
                GameSaveService.RestoreSession(continueData);
                SceneManager.LoadSceneAsync(continueData.CurrentSceneId, LoadSceneMode.Single);
            }
            catch (System.Exception exception)
            {
                Debug.LogWarning($"이어하기 준비에 실패하여 새 게임 화면을 유지합니다: {exception.Message}");
            }
        }

        private void CreateStartInterface()
        {
            if (Camera.main == null)
            {
                GameObject cameraObject = new GameObject("Main Camera");
                cameraObject.tag = "MainCamera";
                cameraObject.transform.position = new Vector3(0f, 0f, -10f);
                Camera camera = cameraObject.AddComponent<Camera>();
                camera.orthographic = true;
                camera.backgroundColor = new Color(.018f, .03f, .06f, 1f);
            }
            if (EventSystem.current == null)
            {
                InputSystemUIInputModule module = new GameObject("EventSystem", typeof(EventSystem))
                    .AddComponent<InputSystemUIInputModule>();
                module.AssignDefaultActions();
            }

            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            GameObject canvasObject = new GameObject("StartMenuCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280f, 720f);

            Image background = MakeImage(canvasObject.transform, "Background", new Color(.018f, .03f, .06f, 1f));
            Stretch(background.rectTransform);
            Text title = MakeText(canvasObject.transform, "Title", "PROJECT LIMITLESS", font, 40,
                new Vector2(.5f, .68f), new Vector2(720f, 60f));
            title.color = new Color(1f, .82f, .4f, 1f);
            title.fontStyle = FontStyle.Bold;

            Button continueButton = null;
            if (continueData != null)
            {
                continueButton = MakeButton(canvasObject.transform, "ContinueButton", "이어하기", font, new Vector2(.5f, .48f));
                continueButton.onClick.AddListener(ContinueGame);
            }
            Button newGameButton = MakeButton(canvasObject.transform, "NewGameButton", "새 게임", font,
                new Vector2(.5f, continueData == null ? .46f : .36f));
            newGameButton.onClick.AddListener(StartNewGame);
            EventSystem.current.SetSelectedGameObject((continueButton ?? newGameButton).gameObject);
            MakeText(canvasObject.transform, "SaveHint",
                continueData == null ? "저장된 캐릭터가 없습니다." : $"{continueData.PlayerName} · Lv.{continueData.Level} · {continueData.CurrentSceneId}",
                font, 17, new Vector2(.5f, .24f), new Vector2(720f, 32f));
        }

        private static Button MakeButton(Transform parent, string name, string label, Font font, Vector2 anchor)
        {
            GameObject obj = new GameObject(name, typeof(Image), typeof(Button), typeof(Outline));
            obj.transform.SetParent(parent, false);
            SetRect(obj.GetComponent<RectTransform>(), anchor, new Vector2(280f, 58f));
            Image image = obj.GetComponent<Image>();
            image.color = new Color(.12f, .32f, .5f, 1f);
            Button button = obj.GetComponent<Button>();
            button.targetGraphic = image;
            Outline outline = obj.GetComponent<Outline>();
            outline.effectColor = new Color(.88f, .7f, .32f, 1f);
            outline.effectDistance = new Vector2(2f, -2f);
            Text text = MakeText(obj.transform, "Label", $"[ {label} ]", font, 22, Vector2.one * .5f, new Vector2(260f, 50f));
            text.fontStyle = FontStyle.Bold;
            return button;
        }

        private static Image MakeImage(Transform parent, string name, Color color)
        {
            GameObject obj = new GameObject(name, typeof(Image));
            obj.transform.SetParent(parent, false);
            Image image = obj.GetComponent<Image>();
            image.color = color;
            image.raycastTarget = false;
            return image;
        }

        private static Text MakeText(Transform parent, string name, string value, Font font, int size, Vector2 anchor, Vector2 dimensions)
        {
            GameObject obj = new GameObject(name, typeof(Text));
            obj.transform.SetParent(parent, false);
            Text text = obj.GetComponent<Text>();
            text.font = font;
            text.fontSize = size;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            text.text = value;
            text.raycastTarget = false;
            SetRect(text.rectTransform, anchor, dimensions);
            return text;
        }

        private static void SetRect(RectTransform rect, Vector2 anchor, Vector2 size)
        {
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = Vector2.one * .5f;
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = size;
        }

        private static void Stretch(RectTransform rect) { rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one; rect.offsetMin = Vector2.zero; rect.offsetMax = Vector2.zero; }
    }
}
