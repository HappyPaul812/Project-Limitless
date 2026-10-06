using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Audio;
using ProjectLimitless.Battle;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.Player;
using ProjectLimitless.UI;
using ProjectLimitless.World;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object=UnityEngine.Object;

namespace ProjectLimitless.EditorTools
{
    public static partial class SecondRegressionAudit
    {
        static string CaptureRoot=>Path.Combine(Root,"Temp/SecondRegression20261006/UI");
        static uint originalWidth,originalHeight;
        static IEnumerator WaitScene(string scene)
        {
            double end=EditorApplication.timeSinceStartup+30;
            while(SceneManager.GetActiveScene().name!=scene&&EditorApplication.timeSinceStartup<end)yield return null;
            Check(SceneManager.GetActiveScene().name==scene,"Scene",scene,"실제 Scene 도착");yield return Wait(.4);
        }
        static void Hit(BattleSceneController c,Combatant target)
        {
            var views=(IDictionary)Get<object>(c,"combatantViews");var view=views[target];
            ((Button)view.GetType().GetField("HitArea").GetValue(view)).onClick.Invoke();
        }
        static IEnumerator Views(string name)
        {
            Directory.CreateDirectory(CaptureRoot);
            foreach(int width in new[]{1920,1600,1280})
            {
                PlayModeWindow.SetCustomRenderingResolution((uint)width,(uint)(width*9/16),"Background QA");yield return Wait(.35);
                Canvas.ForceUpdateCanvases();
                Check(Screen.width==width&&Screen.height==width*9/16,"UI",name+"/resolution"+width,"포커스 전환 없는 실제 Runtime rendering resolution");
                ScreenCapture.CaptureScreenshot(Path.Combine(CaptureRoot,name+"_"+width+".png"));yield return Wait(.4);
            }
            var es=EventSystem.current;var module=es==null?null:es.GetComponent<InputSystemUIInputModule>();
            Check(es!=null&&module!=null&&module.move.action!=null&&module.submit.action!=null&&module.cancel.action!=null,"Input",name+"/actions","EventSystem/InputSystemUIInputModule Move/Submit/Cancel 연결");
            var selected=es.currentSelectedGameObject;
            Record("Input",name+"/focus",selected!=null?"PASS":"NOT_VERIFIED",selected!=null?selected.name:"이 상태에서는 선택 포커스 없음");
        }
        static void Audio(string label,bool battle=false)
        {
            var service=BgmPlaybackService.Instance;var catalog=Resources.Load<BgmSceneCatalog>("Audio/Music/BgmSceneCatalog");
            var expected=battle?catalog.FindBattle(BattleEncounterContext.FieldSceneName,BattleEncounterContext.StableEncounterId,false):catalog.Find(SceneManager.GetActiveScene().name);
            Check(service!=null&&service.CurrentClip==expected&&Object.FindObjectsByType<BgmPlaybackService>(FindObjectsSortMode.None).Length==1,"Audio",label,"Scene/Battle/Return 정본 Clip 및 BGM Service 단일");
            var source=Get<AudioSource>(service,"source");
            Check(source.outputAudioMixerGroup!=null&&source.loop&&source.volume==1,"Audio",label+"/mixer","BGM Mixer routing/loop/source 기본 음량, 청취 평가 제외");
        }
        static IEnumerator Creation()
        {
            var catalog=PlayerAppearanceCatalog.Load();
            Check(catalog.Entries.Count==50&&catalog.Entries.All(e=>e.RuntimeReady&&e.frames.Length==16&&e.frames.All(f=>f!=null)),"Appearance","50/800","50 READY/800 Sprite null0");
            Check(catalog.Entries.Select(e=>e.GenderStableId+e.PathStableId+e.JobStableId).Distinct().Count()==50,"Appearance","mapping-unique","성별/길/직업 조합 중복0");
            string[] paths={"path.vision","path.hearing","path.mobility","path.intellectual","path.emotional-scar"};
            string[] jobs={"guardian","sharpshooter","mage","fighter","healer"};
            for(int i=0;i<5;i++)
            {
                var gender=i==0||i==2?PlayerVisualType.Male:PlayerVisualType.Female;
                GameSessionData.Reset();GameSaveService.SelectSlot(1);yield return Load("CharacterCreation");
                ButtonNamed(gender==PlayerVisualType.Male?"MaleButton":"FemaleButton").onClick.Invoke();
                var creation=Object.FindAnyObjectByType<CharacterCreationController>();
                Get<InputField>(creation,"nameInputField").text="생성감사"+i;
                Check(GameObject.Find("NextAppearance")==null&&GameObject.Find("PreviousAppearance")==null,"Creation",i+"/auto-only","수동 외형 선택 UI 없음");
                if(i==0)yield return Views("Creation");
                ButtonNamed("StartButton").onClick.Invoke();yield return WaitScene("PathSelection");
                var path=Object.FindAnyObjectByType<PathSelectionController>();
                Call(path,"SelectPath",Array.FindIndex(Get<PlayerPathDefinition[]>(path,"pathDefinitions"),p=>p.Id==paths[i]));
                if(i==0)yield return Views("Path");
                Get<Button>(path,"chooseButton").onClick.Invoke();yield return WaitScene("JobSelection");
                var job=Object.FindAnyObjectByType<JobSelectionController>();
                Call(job,"SelectJob",Resources.LoadAll<JobDefinition>("JobDefinitions").Single(j=>j.JobId==jobs[i]));
                var entry=catalog.FindCombination(gender.ToString(),paths[i],jobs[i]);
                Check(Get<Image>(job,"characterPreview").sprite==entry.frames[0]&&GameSessionData.SelectedAppearanceId==entry.appearanceId,"Creation",i+"/preview","Job 실제 자동 Preview "+entry.appearanceId);
                if(i==0)yield return Views("Job");
                Get<Button>(job,"nextButton").onClick.Invoke();yield return WaitScene("FinalConfirmation");
                Check(GameObject.Find("CharacterImage").GetComponent<Image>().sprite==entry.frames[0],"Creation",i+"/confirm","정식 Confirm Preview");
                if(i==0)yield return Views("Confirm");
                ButtonNamed("StartButton").onClick.Invoke();yield return WaitScene("World_StarterVillage");
                Check(Object.FindAnyObjectByType<PlayerVisualController>().ActiveDefaultSprite==entry.frames[0],"Creation",i+"/world","World Sprite");
                Check(GameSaveService.TryLoadSlot(1,out var saved)&&saved.AppearanceId==entry.appearanceId,"Creation",i+"/save","격리 실제 생성 자동 저장");
                Vector2 at=Object.FindAnyObjectByType<PlayerController>().transform.position;
                Check(GameSaveService.SaveCurrentWorldPosition(at,"World_StarterVillage"),"Creation",i+"/position-save","실제 월드 위치 Save");
                yield return Load("Bootstrap");
                GameObject.Find("StartMenuCanvas/Slot01/Action").GetComponent<Button>().onClick.Invoke();yield return WaitScene("World_StarterVillage");
                Check(GameSessionData.SelectedAppearanceId==entry.appearanceId&&GameSessionData.SelectedPlayerVisual==gender&&GameSessionData.SelectedPlayerPathId==paths[i]&&GameSessionData.SelectedJobId==jobs[i]&&Object.FindAnyObjectByType<PlayerVisualController>().ActiveDefaultSprite==entry.frames[0],"Creation",i+"/continue","Bootstrap 실제 Continue/성별·길·직업·외형");
                var visual=Object.FindAnyObjectByType<PlayerVisualController>();var animator=Get<Animator>(visual,"visualAnimator");
                BattleEncounterContext.Set(null,null,animator.runtimeAnimatorController,visual.ActiveDefaultSprite,at,Vector2.zero);yield return Load("Battle");
                Check(BattleEncounterContext.PlayerBattleSprite==entry.frames[4],"Creation",i+"/battle-left","실제 Battle 전달/Left Idle frame4");
            }
        }
        /// <summary>적 HP/턴/승리 주입 없이 실제 명령과 타깃 버튼으로 승리한다. 테스트 플레이어 레벨만 입장 전에 준비한다.</summary>
        static IEnumerator NormalWin(string label)
        {
            var c=Object.FindAnyObjectByType<BattleSceneController>();var foes=Get<Formation>(c,"enemies");int attacks=0;
            double end=EditorApplication.timeSinceStartup+180;
            while(!Get<bool>(c,"battleEnded")&&EditorApplication.timeSinceStartup<end)
            {
                var actor=Get<Combatant>(c,"currentActor");var attack=Get<Button>(c,"attackButton");
                if(!Get<bool>(c,"actionPlaying")&&actor!=null&&actor.IsPlayerControlled&&attack.interactable)
                {
                    attack.onClick.Invoke();var targets=Get<IReadOnlyList<Combatant>>(c,"selectableTargets");
                    if(Get<bool>(c,"choosingTarget")&&targets.Count>0){Hit(c,targets[0]);attacks++;}
                }
                yield return null;
            }
            Check(Get<bool>(c,"battleEnded")&&foes.IsDefeated&&attacks>0,"Dungeon",label+"/victory","실제 normal Attack→animation→turn→Victory; attacks="+attacks);
        }
        static void PastQuests(string before)
        {
            QuestService.ImportSaveData(new QuestProgressSaveData{CompletedQuestIds=QuestCatalog.All.Where(q=>q.QuestId.StartsWith("main_")&&string.CompareOrdinal(q.QuestId.Substring(0,7),before)<0).Select(q=>q.QuestId).ToArray()});
        }
        static IEnumerator ContinueAt(string scene,string label)
        {
            var player=Object.FindAnyObjectByType<PlayerController>();var position=(Vector2)player.transform.position;
            Check(GameSaveService.SaveCurrentWorldPosition(position,scene),"Save",label+"/write","격리 Validation Save");
            GameSaveService.TryLoadSlot(1,out var saved);
            string quest=JsonUtility.ToJson(saved.QuestProgress),roster=JsonUtility.ToJson(saved.CompanionRoster),inventory=JsonUtility.ToJson(new InventoryBox{entries=saved.Inventory}),resources=JsonUtility.ToJson(new ResourceBox{entries=saved.PartyResources}),beast=JsonUtility.ToJson(saved.BeastCompanions);
            yield return Load("Bootstrap");GameSessionData.Reset();
            GameObject.Find("StartMenuCanvas/Slot01/Action").GetComponent<Button>().onClick.Invoke();yield return WaitScene(scene);
            Check(GameSessionData.SelectedPlayerPathId==saved.PathId&&GameSessionData.SelectedJobId==saved.JobId&&GameSessionData.SelectedAppearanceId==saved.AppearanceId&&GameSessionData.Level==saved.Level&&GameSessionData.CurrentExperience==saved.CurrentExperience&&EconomyService.GetCurrency()==saved.Currency,"Save",label+"/identity-growth","Gender/Path/Job/Appearance/EXP/Talent 저장 복원");
            Check(Vector2.Distance(Object.FindAnyObjectByType<PlayerController>().transform.position,position)<.1f&&GameSessionData.LastSafeZoneSceneId==saved.LastSafeZoneSceneId,"Save",label+"/position-safe","Dungeon/World 실제 좌표 및 최근 안전지대 복원");
            Check(JsonUtility.ToJson(QuestService.ExportSaveData())==quest&&JsonUtility.ToJson(CompanionRosterService.ExportSaveData())==roster,"Save",label+"/quest-party","Quest/Party/Formation 정확 복원; before="+quest+"/"+roster+" after="+JsonUtility.ToJson(QuestService.ExportSaveData())+"/"+JsonUtility.ToJson(CompanionRosterService.ExportSaveData()));
            Check(JsonUtility.ToJson(new InventoryBox{entries=InventoryService.ExportSaveData()})==inventory&&JsonUtility.ToJson(new ResourceBox{entries=PartyResourceService.ExportSaveData()})==resources&&JsonUtility.ToJson(BeastCompanionService.ExportSaveData())==beast,"Save",label+"/items-hp-beast","Inventory/HP/MP/Beast 현재 구조 정확 복원");
        }
        [Serializable] sealed class InventoryBox{public InventoryEntry[] entries;}
        [Serializable] sealed class ResourceBox{public PartyMemberResourceSaveData[] entries;}
        static IEnumerator Dungeons()
        {
            var spawns=Resources.LoadAll<FieldMonsterSpawnDefinition>("MonsterSpawns").Where(s=>(s.SceneName=="Dungeon_01"||s.SceneName=="Dungeon_01_B2")&&!s.NonRespawningBoss).OrderBy(s=>s.SpawnId).ToArray();
            Check(spawns.Length==8,"Dungeon","8-spawns","B1/B2 각4 일반 조우 정본");
            foreach(var spawn in spawns)
            {
                Seed("mage",12);PastQuests("main_10");CompanionRosterService.UnlockPaul("mage");
                GameSessionData.ActivateSafeZone("starter","World_StarterVillage","");
                GameSessionData.RecordLocation(spawn.SceneName,"");
                yield return Load(spawn.SceneName);yield return Wait(2);
                Check(Object.FindObjectsByType<MonsterFieldController>(FindObjectsSortMode.None).Any(m=>m.SpawnDefinition==spawn),"Dungeon",spawn.SpawnId+"/spawn","정식 Field Spawn 존재");
                int exp=GameSessionData.CurrentExperience,level=GameSessionData.Level,currency=EconomyService.GetCurrency();
                Check(GameSaveService.SaveCurrentWorldPosition(Object.FindAnyObjectByType<PlayerController>().transform.position,spawn.SceneName),"Dungeon",spawn.SpawnId+"/pre-battle-save","정식 월드 위치를 기록한 유효 Validation Save");
                // 일반 접촉 서비스 경로를 호출한다. 물리 접촉/사용자 입력 검증과 구분한다.
                typeof(MonsterEncounterService).GetField("suppressedUntil",Static).SetValue(null,0f);
                Check(MonsterEncounterService.TryRaise(spawn.Monster,spawn),"Dungeon",spawn.SpawnId+"/encounter","정식 EncounterService→BattleSceneFlow");
                yield return WaitScene("Battle");Audio(spawn.SpawnId+"/battle",true);
                yield return NormalWin(spawn.SpawnId);
                Check((GameSessionData.Level>level||GameSessionData.CurrentExperience>exp)&&EconomyService.GetCurrency()>currency,"Dungeon",spawn.SpawnId+"/reward","정상 승리 EXP/Talent 증가");
                ButtonNamed("VictoryReturn").onClick.Invoke();yield return WaitScene(spawn.SceneName);Audio(spawn.SpawnId+"/return");
                Check(!MonsterEncounterService.IsSpawnAvailable(spawn)&&!Object.FindObjectsByType<MonsterFieldController>(FindObjectsSortMode.None).Any(m=>m.SpawnDefinition==spawn),"Dungeon",spawn.SpawnId+"/removed","승리한 Field Spawn 제거 및 Dungeon return");
                if(spawn==spawns[0]){yield return Views("Dungeon");yield return ContinueAt(spawn.SceneName,"Dungeon");}
            }
        }
        static IEnumerator WorldPartyUi()
        {
            Seed("sharpshooter",12);PastQuests("main_10");CompanionRosterService.UnlockPaul("sharpshooter");
            EconomyService.Import(123);InventoryService.TryAddItem("item.hp_potion",3);
            yield return Load("World_StarterVillage");Audio("World");yield return Views("WorldHUD");
            var manager=Object.FindObjectsByType<ProjectLimitless.NPC.VillageNpcRole>(FindObjectsSortMode.None).First(n=>n.Role==ProjectLimitless.NPC.VillageNpcRoleType.PartyManager);
            manager.Interact(manager.GetComponent<ProjectLimitless.NPC.NpcController>());
            var ui=PartyManagementPresenter.Instance;Check(ui.IsOpen,"Party","manager","정식 NPC→Party Manager UI");
            foreach(var id in ui.DraftIds.ToArray())ui.ToggleCompanion(id);
            ui.ToggleCompanion(CompanionRosterService.MielId);ui.ToggleCompanion(CompanionRosterService.PaulId);ui.ToggleCompanion(CompanionRosterService.TaeonId);
            Check(ui.DraftIds.Count==2&&!ui.DraftIds.Contains(CompanionRosterService.TaeonId),"Party","limit","Player 고정/동료 최대2 제한");
            ui.ToggleRow("player");ui.ToggleRow(CompanionRosterService.PaulId);yield return Views("Party");
            ui.Confirm();if(ui.IsWarningVisible)ui.Confirm();
            Check(!ui.IsOpen&&CompanionRosterService.GetRow("player")==FormationRow.Rear,"Party","formation","UI Front/Rear 적용·Save");
            GameSessionData.ActivateSafeZone("starter","World_StarterVillage","");
            yield return ContinueAt("World_StarterVillage","PartyWorld");
            var d=DialoguePresenter.Instance;
            d.ShowSequence(new[]{new DialogueLine("companion_miel","미엘","저도 볼 겁니다. 이번에는 순서대로요.")},null);
            yield return Views("Dialogue");d.Hide();
            BattleEncounterContext.Set(null,null,null,null,Vector2.zero,Vector2.zero);yield return Load("Battle");
            var c=Object.FindAnyObjectByType<BattleSceneController>();var actor=Get<Formation>(c,"allies").Members.First();ActorCommand(c,actor);
            yield return Views("Battle");OpenSkills(c);yield return Views("Skill");Get<Button>(c,"cancelButton").onClick.Invoke();
            foreach(var item in ItemCatalog.All.Where(i=>i.UseType==ItemUseType.Battle||i.UseType==ItemUseType.WorldAndBattle))InventoryService.TryAddItem(item.ItemId,1);
            Get<Button>(c,"itemButton").onClick.Invoke();yield return Views("Item");Get<Button>(c,"cancelButton").onClick.Invoke();
            yield return Load("Bootstrap");yield return Views("Title");
            yield return Load("Field_07_AshenReach");Audio("Field07");
        }
        // 기존 성공 묶음은 반복하지 않고, 입장 연출에 가려졌던 화면만 다시 캡처합니다.
        static IEnumerator RemainingViews()
        {
            PlayModeWindow.GetRenderingResolution(out originalWidth,out originalHeight);
            try
            {
                Seed("sharpshooter",12);yield return Load("Field_01");
                var source=new GameObject("AuditPartySource");
                Check(!PartyManagementPresenter.OpenAt(source.transform),"Party","Field/block","일반 Field에서 편성 진입 거절");
                yield return Load("Battle");source=new GameObject("AuditPartySource");
                Check(!PartyManagementPresenter.OpenAt(source.transform),"Party","Battle/block","Battle에서 편성 진입 거절");
                var c=Object.FindAnyObjectByType<BattleSceneController>();ActorCommand(c,Get<Formation>(c,"allies").Members.First());
                Check(GameObject.Find("TieBreakOverlay")==null,"UI","Battle/opening-ended","정상 입장 연출 종료 후 캡처");
                yield return Views("Battle");yield return Views("Timeline");
                var es=EventSystem.current;var attack=Get<Button>(c,"attackButton");
                es.SetSelectedGameObject(attack.gameObject);
                var next=attack.FindSelectableOnRight();if(next==null)next=attack.FindSelectableOnDown();
                var direction=attack.FindSelectableOnRight()!=null?MoveDirection.Right:MoveDirection.Down;
                ExecuteEvents.Execute(attack.gameObject,new AxisEventData(es){moveDir=direction},ExecuteEvents.moveHandler);
                Check(next!=null&&es.currentSelectedGameObject==next.gameObject,"Input","Battle/navigation","EventSystem 방향 이벤트로 실제 선택 이동; 물리 입력 아님");
                OpenSkills(c);yield return Views("Skill");Get<Button>(c,"cancelButton").onClick.Invoke();
                foreach(var item in ItemCatalog.All.Where(i=>i.UseType==ItemUseType.Battle||i.UseType==ItemUseType.WorldAndBattle))InventoryService.TryAddItem(item.ItemId,1);
                Get<Button>(c,"itemButton").onClick.Invoke();yield return Views("Item");Get<Button>(c,"cancelButton").onClick.Invoke();
            }
            finally{PlayModeWindow.SetCustomRenderingResolution(originalWidth,originalHeight,"Restored");}
        }
        static IEnumerator Supplement()
        {
            PlayModeWindow.GetRenderingResolution(out originalWidth,out originalHeight);
            try{if(!results.Any(r=>r.category=="Creation"&&r.id=="4/battle-left"))yield return Creation();yield return Dungeons();if(!results.Any(r=>r.category=="Audio"&&r.id=="Field07"))yield return WorldPartyUi();}
            finally{PlayModeWindow.SetCustomRenderingResolution(originalWidth,originalHeight,"Restored");}
        }
    }
}
