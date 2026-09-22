using ProjectLimitless.Core;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectLimitless.World
{
    /// <summary>Field 02의 회피 흔적과 Field 03의 조사·푸른 빛을 stable ID 기반으로 연결합니다.</summary>
    public static class MainQuest08FieldFlow
    {
        public const string QuestId = "main_08_what_they_avoid";
        public const string Trace01 = "field02_main08_avoid_trace_01";
        public const string Trace02 = "field02_main08_avoid_trace_02";
        public const string Trace03 = "field02_main08_avoid_trace_03";
        public const string WheelTracks = "field02_main08_paul_wheel_tracks";
        public const string Field03Entry = "field03_main08_entry";
        public const string Investigation = "field03_main08_investigation_trace";
        public const string DeepZone = "field03_main08_deep_zone";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize() { SceneManager.sceneLoaded -= Install; SceneManager.sceneLoaded += Install; }

        private static void Install(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "Field_02" && scene.name != "Field_03") return;
            if (QuestService.GetState(QuestId) == QuestState.Available) QuestService.TryStart(QuestId);
            if (DialoguePresenter.Instance == null) new GameObject("DialogueSystem").AddComponent<DialoguePresenter>();
            GameObject root = new GameObject("MainQuest08FieldFlow"); SceneManager.MoveGameObjectToScene(root, scene);
            if (scene.name == "Field_02")
            {
                Site(root.transform, "AvoidTrace01", Trace01, new Vector2(4.7f,-2.8f), "◇ 옆으로 꺾인 거미 흔적");
                Site(root.transform, "AvoidTrace02", Trace02, new Vector2(6.1f,-3.7f), "◇ 되돌아간 뱀의 흔적");
                Site(root.transform, "AvoidTrace03", Trace03, new Vector2(7.4f,-4.6f), "◇ 비어 있는 둥근 구역");
                Site(root.transform, "PaulWheelTracks", WheelTracks, new Vector2(8.3f,-5.25f), "◇ 일정한 두 줄의 바퀴 자국");
                DrawTracks(root.transform, new Vector2(8.3f,-5.25f));
            }
            else
            {
                QuestRuntimeState active = QuestService.ActiveMainQuest;
                if (active?.Definition.QuestId == QuestId && active.CurrentObjective?.TargetId == Field03Entry)
                    QuestService.NotifyLocationReached(Field03Entry);
                Site(root.transform, "InvestigationTrace", Investigation, new Vector2(0f,-1.5f), "◇ 가지와 돌이 놓인 흔적");
                Location(root.transform, "DeepQuietZone", DeepZone, new Vector2(0f,-5.2f));
            }
            root.AddComponent<MainQuest08Visibility>();
        }

        private static void Site(Transform parent,string name,string id,Vector2 at,string label)
        { var go=new GameObject(name,typeof(CircleCollider2D),typeof(MainQuest08Interactable)); go.transform.SetParent(parent,false); go.transform.position=at;
          go.GetComponent<CircleCollider2D>().isTrigger=true; go.GetComponent<CircleCollider2D>().radius=1.15f; go.GetComponent<MainQuest08Interactable>().Configure(id,label); QuestNavigationTarget.Attach(go,id,label.TrimStart('◆','◇',' '),new Vector3(0,1.45f,0)); }
        private static void Location(Transform parent,string name,string id,Vector2 at)
        { var go=new GameObject(name,typeof(CircleCollider2D),typeof(MainQuest08Location)); go.transform.SetParent(parent,false); go.transform.position=at;
          go.GetComponent<CircleCollider2D>().isTrigger=true; go.GetComponent<CircleCollider2D>().radius=1.4f; go.GetComponent<MainQuest08Location>().Configure(id); QuestNavigationTarget.Attach(go,id,"깊은 구역"); }
        private static void DrawTracks(Transform parent,Vector2 at)
        { foreach(float x in new[]{-.28f,.28f}) { var q=GameObject.CreatePrimitive(PrimitiveType.Quad); q.name="StableWheelTrack"; q.transform.SetParent(parent,false); q.transform.position=at+new Vector2(x,0);
            q.transform.localScale=new Vector3(.12f,1.5f,1); Object.Destroy(q.GetComponent<MeshCollider>()); var r=q.GetComponent<Renderer>(); r.material=new Material(Shader.Find("Sprites/Default")); r.material.color=new Color(.13f,.09f,.05f,.55f); r.sortingOrder=4; } }

        internal static DialogueLine[] Lines(string id)
        {
            string player=string.IsNullOrWhiteSpace(GameSessionData.PlayerName)?"플레이어":GameSessionData.PlayerName;
            DialogueLine L(string who,string name,string text)=>new DialogueLine(who,name,text);
            if(id==Trace01) return new[]{L(CompanionRosterService.TaeonId,"태온","잠깐만요."),L(CompanionRosterService.TaeonId,"태온","여기부터 흔적이 거의 없습니다."),L(CompanionRosterService.MielId,"미엘","몬스터가 적다는 건 좋은 일 아닌가요?")};
            if(id==Trace02) return new[]{L(CompanionRosterService.TaeonId,"태온","평소라면 그렇겠죠."),L(CompanionRosterService.TaeonId,"태온","그런데 조금 전까지 이쪽으로 이어지던 흔적들이 전부 방향을 바꾸고 있습니다.")};
            if(id==Trace03) return new[]{L(CompanionRosterService.MielId,"미엘","없어진 게 아니네요."),L(CompanionRosterService.MielId,"미엘","다들 이쪽을 피해서 지나가고 있어요."),L(CompanionRosterService.TaeonId,"태온","초원에서는 무언가를 피해 밀려오는 것처럼 보였습니다."),L(CompanionRosterService.TaeonId,"태온","여기서는 그 방향이 조금 더 분명합니다."),L("",player,"그러면 우리가 찾던 방향은 맞는 것 같습니다.")};
            if(id==WheelTracks) return new[]{L(CompanionRosterService.MielId,"미엘","이 흔적… 폴 씨 것 아닐까요?"),L(CompanionRosterService.TaeonId,"태온","폭이 같습니다. 아마 맞을 겁니다.")};
            if(id==Investigation) return new[]{L(CompanionRosterService.TaeonId,"태온","누군가 이곳을 조사한 흔적입니다."),L(CompanionRosterService.MielId,"미엘","폴 씨일까요?"),L("",player,"아마 먼저 안쪽으로 간 것 같습니다.")};
            return new[]{L(CompanionRosterService.MielId,"미엘","조용하네요."),L(CompanionRosterService.TaeonId,"태온","너무 조용합니다."),L(CompanionRosterService.MielId,"미엘","몬스터들이 여기를 피하는 이유가 있는 거겠죠?"),L(CompanionRosterService.TaeonId,"태온","그 이유가 무엇인지는 아직 모르겠습니다."),L("",player,"안쪽을 확인해보죠."),L(CompanionRosterService.MielId,"미엘","…저쪽에서 빛이 났어요."),L(CompanionRosterService.TaeonId,"태온","마법 같습니다."),L("",player,"가보죠.")};
        }
    }

    public sealed class MainQuest08Visibility:MonoBehaviour
    { private void OnEnable(){QuestService.Changed+=Refresh;Refresh();} private void OnDisable()=>QuestService.Changed-=Refresh;
      private void Refresh(){bool active=QuestService.ActiveMainQuest?.Definition.QuestId==MainQuest08FieldFlow.QuestId; foreach(Transform c in transform)c.gameObject.SetActive(active);} }

    public sealed class MainQuest08Interactable:MonoBehaviour
    { string id,label; PlayerController nearby; InputAction action; GameObject marker;
      public void Configure(string stableId,string text){id=stableId;label=text;}
      private void Start(){marker=MakeLabel(label+"\n[E/F] 확인"); action=new InputAction("Main08Interact",InputActionType.Button); action.AddBinding("<Keyboard>/e");action.AddBinding("<Keyboard>/f");action.AddBinding("<Gamepad>/buttonSouth");action.performed+=_=>Interact();action.Enable();QuestService.Changed+=Refresh;Refresh();}
      private void OnDestroy(){QuestService.Changed-=Refresh;action?.Dispose();} private void OnTriggerEnter2D(Collider2D c){nearby=c.GetComponent<PlayerController>()??nearby;Refresh();} private void OnTriggerExit2D(Collider2D c){if(c.GetComponent<PlayerController>()==nearby)nearby=null;Refresh();}
      bool Current()=>QuestService.ActiveMainQuest?.Definition.QuestId==MainQuest08FieldFlow.QuestId&&QuestService.ActiveMainQuest.CurrentObjective?.TargetId==id;
      void Refresh(){if(marker!=null)marker.SetActive(Current());} void Interact(){if(nearby==null||!Current()||WorldModalState.IsOpen)return;DialoguePresenter.Instance.ShowSequence(MainQuest08FieldFlow.Lines(id),()=>{QuestService.NotifyInteraction(id);if(GameSaveService.CurrentSlotIndex>0)GameSaveService.SaveCurrentSession();});DialoguePresenter.Instance.TrackDistance(nearby.transform,transform,3f);}
      GameObject MakeLabel(string text){var root=new GameObject("QuestMarker",typeof(Canvas),typeof(CanvasScaler));root.transform.SetParent(transform,false);root.transform.localPosition=new Vector3(0,1.5f,0);root.transform.localScale=Vector3.one*.01f;var canvas=root.GetComponent<Canvas>();canvas.renderMode=RenderMode.WorldSpace;canvas.sortingOrder=22;root.GetComponent<RectTransform>().sizeDelta=new Vector2(340,70);var t=new GameObject("Text",typeof(Text)).GetComponent<Text>();t.transform.SetParent(root.transform,false);t.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");t.fontSize=23;t.alignment=TextAnchor.MiddleCenter;t.color=new Color(.4f,.82f,1f);t.text=text;t.rectTransform.anchorMin=Vector2.zero;t.rectTransform.anchorMax=Vector2.one;t.rectTransform.offsetMin=Vector2.zero;t.rectTransform.offsetMax=Vector2.zero;return root;}
    }

    public sealed class MainQuest08Location:MonoBehaviour
    { string id; public void Configure(string value)=>id=value; private void OnTriggerEnter2D(Collider2D c){if(c.GetComponent<PlayerController>()==null)return;var q=QuestService.ActiveMainQuest;if(q?.Definition.QuestId!=MainQuest08FieldFlow.QuestId||q.CurrentObjective?.TargetId!=id)return;
        var lines=MainQuest08FieldFlow.Lines(id); var before=new DialogueLine[5]; System.Array.Copy(lines,before,5);
        DialoguePresenter.Instance.ShowSequence(before,()=>{ShowBlueLight(); var after=new DialogueLine[3];System.Array.Copy(lines,5,after,0,3);DialoguePresenter.Instance.ShowSequence(after,()=>{QuestService.NotifyLocationReached(id);if(GameSaveService.CurrentSlotIndex>0)GameSaveService.SaveCurrentSession();});});}
      void ShowBlueLight(){var go=GameObject.CreatePrimitive(PrimitiveType.Quad);go.name="Main08BlueMagicLight";go.transform.position=transform.position+new Vector3(2.8f,1.2f,0);go.transform.localScale=Vector3.one*.65f;Object.Destroy(go.GetComponent<MeshCollider>());var r=go.GetComponent<Renderer>();r.material=new Material(Shader.Find("Sprites/Default"));r.material.color=new Color(.15f,.55f,1f,.85f);r.sortingOrder=20;Object.Destroy(go,1.2f);}}
}
