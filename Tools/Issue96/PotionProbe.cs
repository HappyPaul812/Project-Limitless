#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using ProjectLimitless.World;
using ProjectLimitless.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using ProjectLimitless.Battle;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

// #96 물약 전용 격리 검증기입니다. 원본 프로젝트와 과거 전체 QA를 실행하지 않습니다.
[InitializeOnLoad]
public static class PotionBatch
{
    const string Key = "Limitless96PotionArmed";
    static double deadline;
    static PotionBatch()
    {
        EditorApplication.playModeStateChanged += state => {
            if (state != PlayModeStateChange.EnteredPlayMode || !SessionState.GetBool(Key, false)) return;
            EditorApplication.LockReloadAssemblies();
            deadline = EditorApplication.timeSinceStartup + 180;
            EditorApplication.update += Poll;
        };
    }
    public static void Run()
    {
        if (!Application.isBatchMode || Application.unityVersion != "6000.6.5f1" ||
            !Application.dataPath.Replace('\\','/').EndsWith("/Temp/Issue97/QA/Client/Assets"))
            throw new Exception("#96 물약 격리 batch 프로젝트에서만 허용합니다.");
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        new GameObject("Potion96_DeltaOnly").AddComponent<PotionProbe>();
        SessionState.SetBool(Key,true); EditorApplication.isPlaying = true;
    }
    public static void After() { SessionState.SetBool("Potion96Before",false); SessionState.SetBool("Potion96Post",false);Run(); }
    public static void Baseline() { SessionState.SetBool("Potion96Before",true);Run(); }
    public static void PostApply() { SessionState.SetBool("Potion96Post",true);Run(); }
    static void Poll()
    {
        var p = Object.FindAnyObjectByType<PotionProbe>();
        if (p != null && p.Done) {
            SessionState.SetBool(Key,false); EditorApplication.update -= Poll;
            int exit = p.Failed ? 1 : 0; EditorApplication.UnlockReloadAssemblies(); EditorApplication.isPlaying = false;
            EditorApplication.delayCall += () => EditorApplication.Exit(exit);
        } else if (EditorApplication.timeSinceStartup > deadline) {
            EditorApplication.UnlockReloadAssemblies();
            File.WriteAllText(PotionProbe.Output + "/timeout.txt","timeout"); EditorApplication.Exit(2);
        }
    }
}

[DefaultExecutionOrder(-10000)]
public sealed class PotionProbe : MonoBehaviour
{
    public static bool Before => SessionState.GetBool("Potion96Before",false);
    public static bool Post => SessionState.GetBool("Potion96Post",false);
    public static string Output => Path.GetFullPath(Path.Combine(Application.dataPath,"../../../../Issue96Original/QA/"+(Before?"Before":Post?"PostApply":"After")));
    public bool Done, Failed;
    readonly List<string> checks = new List<string>(), errors = new List<string>(), trace = new List<string>();
    IEnumerator routine;
    EditorWindow gameView;

    PlayerController Player => Object.FindAnyObjectByType<PlayerController>();
    const BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
    void Awake()
    {
        DontDestroyOnLoad(gameObject); Directory.CreateDirectory(Output);
        GameSaveService.AuditSaveDirectory = Path.GetFullPath(Path.Combine(Application.dataPath,"../../../../Issue96Original/QA/SyntheticSaves_"+(Before?"Before":Post?"Post":"After")));
        UserSettingsService.BeginAudit(Path.GetFullPath(Path.Combine(Application.dataPath,"../../../../Issue96Original/QA/SyntheticSettings_"+(Before?"Before":Post?"Post":"After"))));
        UserSettingsService.SetMuteAll(true); AudioListener.volume = 0;
        Application.runInBackground = true; QualitySettings.vSyncCount = 0; Application.targetFrameRate = 60;
        Application.logMessageReceived += (text,stack,type) => {
            if (type != LogType.Error && type != LogType.Exception && type != LogType.Assert) return;
            if (stack.Contains("UnityEditor.Search.SearchDatabase")) File.AppendAllText(Output+"/EnvironmentErrors.txt",text+"\n"+stack);
            else errors.Add(text+"\n"+stack);
        };
    }
    void Start() { routine = Flatten(Run()); }
    void Update()
    {
        if (Done || routine == null) return;
        try { if (!routine.MoveNext()) Finish(); }
        catch (Exception e) { errors.Add(e.ToString()); Check(false,"HarnessException"); Finish(); }
    }
    void Finish()
    {
        Done = true; Failed = checks.Any(x=>x.StartsWith("FAIL")) || errors.Count > 0;
        File.WriteAllLines(Output+"/checks.txt",checks); File.WriteAllLines(Output+"/Errors.txt",errors);
        File.WriteAllLines(Output+"/input-trace.csv",trace);
        File.WriteAllText(Output+"/status.txt",Failed?"FAIL":"PASS");
    }
    void Check(bool pass,string name) { checks.Add((pass?"PASS ":"FAIL ")+name); File.WriteAllLines(Output+"/checks.txt",checks); }
    static IEnumerator Flatten(IEnumerator e)
    {
        var stack = new Stack<IEnumerator>(); stack.Push(e);
        while (stack.Count>0) { var top=stack.Peek(); if(!top.MoveNext()){stack.Pop();continue;} if(top.Current is IEnumerator n)stack.Push(n);else yield return top.Current; }
    }
    static IEnumerator Wait(float seconds) { float end=Time.realtimeSinceStartup+seconds;while(Time.realtimeSinceStartup<end)yield return null; }
    // GameView 자체의 PlayModeView 렌더 경로를 사용합니다. Camera.Render나 오프라인 UI 합성이 아닙니다.
    void SetSize(int width,int height=720)
    {
        var a=typeof(EditorWindow).Assembly;var t=a.GetType("UnityEditor.GameView");
        if(gameView==null)gameView=EditorWindow.GetWindow(t,false,"VenomPotion96 QA",false);
        var sizesType=a.GetType("UnityEditor.GameViewSizes");
        var singleton=typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
        var sizes=singleton.GetProperty("instance",Flags).GetValue(null,null);
        var groupType=a.GetType("UnityEditor.GameViewSizeGroupType");
        var group=sizesType.GetMethod("GetGroup",Flags).Invoke(sizes,new[]{Enum.Parse(groupType,"Standalone")});
        int count=(int)group.GetType().GetMethod("GetTotalCount",Flags).Invoke(group,null);
        var sizeType=a.GetType("UnityEditor.GameViewSize");var kind=a.GetType("UnityEditor.GameViewSizeType");
        var size=Activator.CreateInstance(sizeType,Flags,null,new object[]{Enum.Parse(kind,"FixedResolution"),width,height,"Potion96_"+width},null);
        group.GetType().GetMethod("AddCustomSize",Flags).Invoke(group,new[]{size});
        t.GetProperty("selectedSizeIndex",Flags).SetValue(gameView,count,null);
        t.GetProperty("targetSize",Flags).SetValue(gameView,new Vector2(width,height),null);
        gameView.position=new Rect(0,0,width,height+30);gameView.Repaint();
        Camera.main.aspect=width/(float)height;
    }
    void Capture(string variant,int width,int height=720)
    {

        Canvas.ForceUpdateCanvases();
        var rt=(RenderTexture)gameView.GetType().GetMethod("RenderView",Flags).Invoke(gameView,new object[]{Vector2.zero,false});
        if(rt==null)throw new Exception("실제 GameView RenderView가 RenderTexture를 반환하지 않았습니다.");
        var old=RenderTexture.active;RenderTexture.active=rt;var tex=new Texture2D(rt.width,rt.height,TextureFormat.RGBA32,false);
        tex.ReadPixels(new Rect(0,0,rt.width,rt.height),0,0);tex.Apply();
        // D3D의 GameView RT 저장 방향만 정규화합니다. 게임 내용/UI를 합성하거나 수정하지 않습니다.
        if(SystemInfo.graphicsUVStartsAtTop) {
            var pixels=tex.GetPixels32();int stride=tex.width;
            for(int y=0;y<tex.height/2;y++)for(int x=0;x<stride;x++) {
                int a=y*stride+x,b=(tex.height-1-y)*stride+x;var value=pixels[a];pixels[a]=pixels[b];pixels[b]=value;
            }
            tex.SetPixels32(pixels);tex.Apply();
        }
        File.WriteAllBytes(Output+"/"+variant+"_GameView_"+width+"x"+height+".png",tex.EncodeToPNG());RenderTexture.active=old;Destroy(tex);
        Check(rt.width==width&&rt.height==height,variant+".ActualGameViewResolution."+width);
        Check(Camera.main.targetTexture==null,variant+".NoCameraRTSubstitution."+width);
        File.AppendAllText(Output+"/capture-metadata.txt",variant+"|"+width+"x"+height+"|UnityEditor.GameView.RenderView|batch="+Application.isBatchMode+"|camera="+Camera.main.transform.position+"|ortho="+Camera.main.orthographicSize+"|screen="+Screen.width+"x"+Screen.height+"|overlayCanvases="+Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None).Count(c=>c.renderMode==RenderMode.ScreenSpaceOverlay)+"\n");
    }


    ShopPresenter Shop => ShopPresenter.Instance;
    Keyboard keyboard; Mouse mouse;
    object Get(object o,string n) => o.GetType().GetField(n,Flags).GetValue(o);
    T Field<T>(string n) => (T)Get(Shop,n);
    Button Row(string id) => Field<List<Button>>("itemButtons").First(x=>x.name=="Item_"+id);
    Image Icon(string id) => Row(id).transform.Find("Icon").GetComponent<Image>();
    void Call(string n,params object[] values) => typeof(ShopPresenter).GetMethod(n,Flags).Invoke(Shop,values);
    const string Hp="item_healing_potion_small", Mp="item_mana_potion_small";
    IEnumerator KeyPress(Key key,float seconds=.12f) {
        InputSystem.QueueStateEvent(keyboard,new KeyboardState(key));yield return Wait(seconds);
        InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return Wait(.08f);
    }
    IEnumerator Click(Button button) {
        Canvas.ForceUpdateCanvases();Vector2 point=RectTransformUtility.WorldToScreenPoint(null,button.GetComponent<RectTransform>().TransformPoint(button.GetComponent<RectTransform>().rect.center));
        InputSystem.QueueStateEvent(mouse,new MouseState{position=point});yield return Wait(.1f);
        InputSystem.QueueStateEvent(mouse,new MouseState{position=point}.WithButton(MouseButton.Left));yield return Wait(.1f);
        InputSystem.QueueStateEvent(mouse,new MouseState{position=point});yield return Wait(.12f);
        trace.Add("mouse,"+button.name+","+point+",selected="+(EventSystem.current.currentSelectedGameObject==null?"null":EventSystem.current.currentSelectedGameObject.name));
    }
    IEnumerator Shots(string phase) {
        foreach(int width in new[]{960,1280}) {
            SetSize(width);yield return Wait(.5f);gameView.GetType().GetMethod("RenderView",Flags).Invoke(gameView,new object[]{Vector2.zero,true});yield return Wait(.5f);
            Capture(phase,width);Check(Icon(Hp).rectTransform.sizeDelta==new Vector2(64,64)&&Icon(Mp).rectTransform.sizeDelta==new Vector2(64,64),phase+".Logical64x64."+width);
        }
    }
    void CheckIcons(bool replacement,string label) {
        foreach(var id in new[]{Hp,Mp}) {
            var icon=Icon(id);var item=ItemCatalog.All.First(x=>x.ItemId==id);string path=AssetDatabase.GetAssetPath(icon.sprite);
            Check(replacement?path.Contains("StarterVillageShop_"):icon.sprite==item.Icon,label+".Sprite."+id);
            Check(icon.color==(replacement?Color.white:item.IconTint)&&icon.preserveAspect,label+".TintAndAspect."+id);
        }
    }
    IEnumerator Run() {
        InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
        InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
        Check(Application.unityVersion=="6000.6.5f1","Unity6000.6.5f1");
        GameSessionData.Reset();GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"Potion96 QA");GameSessionData.SelectPlayerPath("path.mobility");GameSessionData.SelectJob("guardian");GameSessionData.ConfigureProgress(5,0);GameSaveService.SelectSlot(1);
        var loading=SceneManager.LoadSceneAsync("World_StarterVillage");while(!loading.isDone)yield return null;yield return Wait(.5f);
        EconomyService.Import(1000);InventoryService.Reset();InventoryService.TryAddItem(Hp,5);InventoryService.TryAddItem(Mp,3);
        var other=ItemCatalog.All.First(x=>x.ItemId!=Hp&&x.ItemId!=Mp&&ShopService.IsSellable(x));InventoryService.TryAddItem(other.ItemId,2);Sprite otherOriginal=other.Icon;
        var owner=GameObject.Find("Npc_GeneralShop").transform;
        Player.transform.position=owner.position+new Vector3(0,-1,0);Player.GetComponent<Rigidbody2D>().position=Player.transform.position;
        keyboard=InputSystem.AddDevice<Keyboard>();mouse=InputSystem.AddDevice<Mouse>();keyboard.MakeCurrent();mouse.MakeCurrent();
        ShopPresenter.OpenStarterGeneralShop(owner);yield return Wait(.25f);
        Check(Shop.IsOpen&&PlayerController.IsMovementLocked,"OpenAndMovementLock");CheckIcons(!Before,"StarterBuy");
        if(Before){yield return Shots("Before");Shop.Close();Check(errors.Count==0,"BaselineRuntimeErrorsZero");yield break;}
        var hp=ItemCatalog.All.First(x=>x.ItemId==Hp);var mp=ItemCatalog.All.First(x=>x.ItemId==Mp);
        Check(hp.EffectAmount==30&&hp.BuyPrice==20&&hp.SellPrice==10&&mp.EffectAmount==12&&mp.BuyPrice==30&&mp.SellPrice==15,"EffectsBuySellPricesUnchanged");
        string hpIcon=AssetDatabase.GetAssetPath(hp.Icon),mpIcon=AssetDatabase.GetAssetPath(mp.Icon);Check(hpIcon.EndsWith("HealingPotionSmall.png")&&mpIcon.EndsWith("ManaPotionSmall.png"),"SharedItemDefinitionsStillKenney");
        yield return Shots(Post?"PostApply":"After");
        Vector3 start=Player.transform.position;yield return KeyPress(Key.D,.25f);Check(Vector3.Distance(start,Player.transform.position)<.03f,"KeyboardMoveBlockedWhileShopOpen");
        yield return Click(Row(Mp));Check(Field<ItemDefinition>("selectedItem").ItemId==Mp,"MouseSelectMP");
        Check(Field<Text>("detailsLabel").text.Contains("MP +12")&&Field<Text>("detailsLabel").text.Contains("30"),"MPDescriptionEffectPrice");
        var selected=EventSystem.current.currentSelectedGameObject;yield return KeyPress(Key.Tab);Check(EventSystem.current.currentSelectedGameObject!=selected,"KeyboardTabMovesFocus");
        Row(Hp).Select();yield return KeyPress(Key.Enter);Check(Field<ItemDefinition>("selectedItem").ItemId==Hp,"KeyboardSubmitSelectHP");
        Check(Field<Text>("detailsLabel").text.Contains("HP +30")&&Field<Text>("detailsLabel").text.Contains(hp.Description),"HPTooltipDescriptionAndEffect");
        yield return Click(Field<Button>("increaseButton"));Check(Shop.TradeQuantity==2,"MouseQuantityIncrease");
        yield return Click(Field<Button>("buyButton"));Check(InventoryService.GetItemCount(Hp)==7&&EconomyService.GetCurrency()==960,"BuyHP2At20Each");
        yield return Click(Row(Mp));Shop.SetTradeQuantity(2);yield return Click(Field<Button>("buyButton"));Check(InventoryService.GetItemCount(Mp)==5&&EconomyService.GetCurrency()==900,"BuyMP2At30Each");
        Check(Field<Text>("detailsLabel").text.Contains("5")&&ShopService.GetMaximumBuyQuantity(Field<ShopDefinition>("shop"),Mp)==30,"QuantityAndAffordableStockRefresh");
        yield return Click(Field<Button>("sellModeButton"));CheckIcons(true,"StarterSell");Check(Row(other.ItemId).transform.Find("Icon").GetComponent<Image>().sprite==otherOriginal,"OtherItemIconUnchanged");
        yield return Click(Row(Hp));Shop.SetTradeQuantity(2);yield return Click(Field<Button>("sellButton"));Check(InventoryService.GetItemCount(Hp)==5&&EconomyService.GetCurrency()==920,"SellHP2At10Each");
        yield return Click(Row(Mp));Shop.SetTradeQuantity(2);yield return Click(Field<Button>("sellButton"));Check(InventoryService.GetItemCount(Mp)==3&&EconomyService.GetCurrency()==950,"SellMP2At15Each");
        Check(GameSaveService.TryLoadSlot(1,out GameSaveData saved)&&saved.Currency==950&&saved.Inventory.First(x=>x.ItemId==Hp).Count==5&&saved.Inventory.First(x=>x.ItemId==Mp).Count==3,"SyntheticSaveKeepsTransactionAndItemIds");
        Check(File.ReadAllText(GameSaveService.GetSaveFilePath(1)).IndexOf("StarterVillageShop_",StringComparison.Ordinal)<0,"SaveContainsNoIconOverrideData");
        yield return KeyPress(Key.Escape);Check(!Shop.IsOpen&&!PlayerController.IsMovementLocked,"KeyboardEscapeClosesAndUnlocks");
        start=Player.transform.position;yield return KeyPress(Key.D,.3f);Check(Vector3.Distance(start,Player.transform.position)>.05f,"KeyboardMovementRestoredAfterClose");
        ShopPresenter.OpenStarterGeneralShop(owner);yield return Wait(.15f);yield return Click(Field<Button>("closeButton"));Check(!Shop.IsOpen&&!PlayerController.IsMovementLocked,"MouseCloseAndUnlock");
        ShopPresenter.OpenCatalog("ShopDefinitions/CatacombEntranceSupply",owner);yield return Wait(.2f);CheckIcons(false,"OtherShopInVillage");Shop.Close();
        loading=SceneManager.LoadSceneAsync("Field_01");while(!loading.isDone)yield return null;yield return Wait(.3f);
        owner=new GameObject("96ScopeOnlyOwner").transform;ShopPresenter.OpenStarterGeneralShop(owner);yield return Wait(.2f);CheckIcons(false,"StarterShopOutsideVillage");Shop.Close();
        Check(hp.Icon!=null&&mp.Icon!=null&&AssetDatabase.GetAssetPath(hp.Icon)==hpIcon&&AssetDatabase.GetAssetPath(mp.Icon)==mpIcon,"InventoryAndBattleSharedIconsUnchanged");
        InputSystem.RemoveDevice(keyboard);InputSystem.RemoveDevice(mouse);
        Check(errors.Count==0,"NewRuntimeErrorsZero");
    }
}
#endif
