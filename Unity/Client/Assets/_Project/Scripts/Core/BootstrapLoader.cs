using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectLimitless.Core
{
    /// <summary>Bootstrap에서 다섯 캐릭터 슬롯을 독립적으로 조사하고 새 게임 또는 이어하기를 시작합니다.</summary>
    public sealed class BootstrapLoader : MonoBehaviour
    {
        [SerializeField] private string worldSceneName = "CharacterCreation";
        private readonly List<Button> slotButtons = new List<Button>();

        public void ConfigureStartScene(string sceneName) { worldSceneName = sceneName; }

        private void Start()
        {
            GameSessionData.Reset();
            GameSaveService.TryMigrateLegacySaveToSlotOne();
            CreateStartInterface();
        }

        private void StartNewGame(int slotIndex)
        {
            // 빈 슬롯을 먼저 선택해 두면 FinalConfirmation과 이후 자동 저장이 해당 파일만 갱신합니다.
            if (!GameSaveService.SelectSlot(slotIndex)) return;
            GameSessionData.Reset();
            SceneManager.LoadSceneAsync(worldSceneName, LoadSceneMode.Single);
        }

        private void ContinueGame(int slotIndex)
        {
            if (!GameSaveService.TryLoadSlot(slotIndex, out GameSaveData data)) return;
            try
            {
                // 문자열 ID를 Session에 복원하면 기존 Resolver가 실제 ScriptableObject와 Sprite를 다시 찾습니다.
                GameSaveService.RestoreSession(slotIndex, data);
                SceneManager.LoadSceneAsync(data.CurrentSceneId, LoadSceneMode.Single);
            }
            catch (System.Exception exception) { Debug.LogWarning($"슬롯 {slotIndex} 이어하기 준비에 실패했습니다: {exception.Message}"); }
        }

        private void CreateStartInterface()
        {
            CreateCameraAndEventSystem();
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            GameObject canvasObject = new GameObject("StartMenuCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; scaler.referenceResolution = new Vector2(1280, 720);
            Stretch(MakeImage(canvasObject.transform, "Background", new Color(.018f, .03f, .06f, 1)).rectTransform);
            Text title = MakeText(canvasObject.transform, "Title", "PROJECT LIMITLESS", font, 36, new Vector2(.5f, .93f), new Vector2(720, 50)); title.color = new Color(1, .82f, .4f, 1); title.fontStyle = FontStyle.Bold;
            MakeText(canvasObject.transform, "Subtitle", "캐릭터 저장 슬롯", font, 20, new Vector2(.5f, .87f), new Vector2(500, 34));
            for (int slot = 1; slot <= GameSaveService.DefaultSaveSlotCount; slot++) CreateSlotRow(canvasObject.transform, font, GameSaveService.InspectSlot(slot), .75f - (slot - 1) * .135f);
            LinkVerticalNavigation();
            if (slotButtons.Count > 0) EventSystem.current.SetSelectedGameObject(slotButtons[0].gameObject);
            MakeText(canvasObject.transform, "Help", "방향키: 슬롯 이동   Enter / Space: 선택", font, 15, new Vector2(.5f, .035f), new Vector2(760, 26)).color = new Color(.7f, .77f, .86f, 1);
        }

        private void CreateSlotRow(Transform parent, Font font, SaveSlotInfo info, float anchorY)
        {
            Image panel = MakeImage(parent, $"Slot{info.SlotIndex:00}", new Color(.055f, .08f, .13f, .97f)); SetRect(panel.rectTransform, new Vector2(.5f, anchorY), new Vector2(880, 82));
            Outline outline = panel.gameObject.AddComponent<Outline>(); outline.effectColor = new Color(.3f, .39f, .52f, 1);
            string details; string action; bool interactable = info.State != SaveSlotState.Invalid;
            if (info.State == SaveSlotState.Valid) { details = $"{info.Data.PlayerName}  ·  {GetJobName(info.Data.JobId)}  ·  Lv.{info.Data.Level}  ·  {GetSceneName(info.Data.CurrentSceneId)}"; action = "이어하기"; }
            else if (info.State == SaveSlotState.Empty) { details = "빈 슬롯"; action = "새 캐릭터"; }
            else { details = "저장 파일을 읽을 수 없습니다 (Console 확인)"; action = "사용 불가"; Debug.LogWarning($"슬롯 {info.SlotIndex} 오류: {info.Error}"); }
            Text number = MakeText(panel.transform, "Number", $"슬롯 {info.SlotIndex}", font, 21, new Vector2(.1f, .5f), new Vector2(140, 50)); number.color = new Color(1, .82f, .4f, 1); number.fontStyle = FontStyle.Bold;
            MakeText(panel.transform, "Summary", details, font, 18, new Vector2(.48f, .5f), new Vector2(510, 50)).alignment = TextAnchor.MiddleLeft;
            Button button = MakeButton(panel.transform, "Action", action, font, new Vector2(.86f, .5f)); button.interactable = interactable;
            int selectedSlot = info.SlotIndex;
            if (info.State == SaveSlotState.Valid) button.onClick.AddListener(() => ContinueGame(selectedSlot));
            else if (info.State == SaveSlotState.Empty) button.onClick.AddListener(() => StartNewGame(selectedSlot));
            if (interactable) slotButtons.Add(button);
        }

        private void LinkVerticalNavigation()
        {
            for (int i = 0; i < slotButtons.Count; i++) { int previous = (i - 1 + slotButtons.Count) % slotButtons.Count; int next = (i + 1) % slotButtons.Count; slotButtons[i].navigation = new Navigation { mode = Navigation.Mode.Explicit, selectOnUp = slotButtons[previous], selectOnDown = slotButtons[next] }; }
        }

        private static string GetJobName(string id) { JobDefinition item = Resources.LoadAll<JobDefinition>("JobDefinitions").FirstOrDefault(value => value.JobId == id); return item == null ? id : item.DisplayName; }
        private static string GetSceneName(string id) { if (id == "World_StarterVillage") return "시작 마을"; if (id == "Field_01") return "초원"; if (id == "Field_02") return "그늘진 숲길"; return id; }
        private static void CreateCameraAndEventSystem() { if (Camera.main == null) { GameObject obj = new GameObject("Main Camera"); obj.tag = "MainCamera"; obj.transform.position = new Vector3(0, 0, -10); Camera camera = obj.AddComponent<Camera>(); camera.orthographic = true; camera.backgroundColor = new Color(.018f, .03f, .06f, 1); } if (EventSystem.current == null) { InputSystemUIInputModule module = new GameObject("EventSystem", typeof(EventSystem)).AddComponent<InputSystemUIInputModule>(); module.AssignDefaultActions(); } }
        private static Button MakeButton(Transform parent, string name, string label, Font font, Vector2 anchor) { GameObject obj = new GameObject(name, typeof(Image), typeof(Button), typeof(Outline)); obj.transform.SetParent(parent, false); SetRect(obj.GetComponent<RectTransform>(), anchor, new Vector2(190, 50)); Image image = obj.GetComponent<Image>(); image.color = new Color(.12f, .32f, .5f, 1); Button button = obj.GetComponent<Button>(); button.targetGraphic = image; Outline outline = obj.GetComponent<Outline>(); outline.effectColor = new Color(.88f, .7f, .32f, 1); outline.effectDistance = new Vector2(2, -2); MakeText(obj.transform, "Label", $"[ {label} ]", font, 19, Vector2.one * .5f, new Vector2(180, 44)).fontStyle = FontStyle.Bold; return button; }
        private static Image MakeImage(Transform parent, string name, Color color) { GameObject obj = new GameObject(name, typeof(Image)); obj.transform.SetParent(parent, false); Image image = obj.GetComponent<Image>(); image.color = color; image.raycastTarget = false; return image; }
        private static Text MakeText(Transform parent, string name, string value, Font font, int fontSize, Vector2 anchor, Vector2 dimensions) { GameObject obj = new GameObject(name, typeof(Text)); obj.transform.SetParent(parent, false); Text text = obj.GetComponent<Text>(); text.font = font; text.fontSize = fontSize; text.color = Color.white; text.alignment = TextAnchor.MiddleCenter; text.text = value; text.raycastTarget = false; SetRect(text.rectTransform, anchor, dimensions); return text; }
        private static void SetRect(RectTransform rect, Vector2 anchor, Vector2 size) { rect.anchorMin = anchor; rect.anchorMax = anchor; rect.pivot = Vector2.one * .5f; rect.anchoredPosition = Vector2.zero; rect.sizeDelta = size; }
        private static void Stretch(RectTransform rect) { rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one; rect.offsetMin = Vector2.zero; rect.offsetMax = Vector2.zero; }
    }
}
