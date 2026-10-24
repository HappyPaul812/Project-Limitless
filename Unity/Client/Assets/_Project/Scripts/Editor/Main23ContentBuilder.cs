using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using ProjectLimitless.World;
using Object=UnityEngine.Object;
namespace ProjectLimitless.EditorTools
{
/// <summary>지정 격리 worktree에서만 실행하는 제작기입니다. 기존 남부를 옮기지 않고 북부 확장과 새 마을만 저장합니다.</summary>
public static class Main23ContentBuilder
{
 const string Scenes="Assets/_Project/Scenes/";
 /// <summary>최종 저장 Scene을 다시 읽어 Missing Script·승인 타일9·BuildSettings 연결만 검증합니다. Scene은 저장하지 않습니다.</summary>
 public static void Validate(){
  if(!Application.isBatchMode||!Application.dataPath.Replace('\\','/').Contains("/Temp/Main23FirstPlayable20261023/Worktree/Unity/Client/Assets"))throw new Exception("격리 검증만 허용합니다.");
  foreach(string name in new[]{Chapter3Main23Flow.Field,Chapter3Main23Flow.Village}){var scene=EditorSceneManager.OpenScene(Scenes+name+".unity",OpenSceneMode.Single);if(scene.GetRootGameObjects().SelectMany(r=>r.GetComponentsInChildren<Transform>(true)).Any(t=>GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject)>0))throw new Exception("Missing Script: "+name);if(Object.FindObjectsByType<PlayerController>(FindObjectsSortMode.None).Length!=1||Object.FindAnyObjectByType<WorldBounds2D>()==null)throw new Exception("Player/Bounds: "+name);if(EditorBuildSettings.scenes.Count(s=>s.enabled&&s.path==Scenes+name+".unity")!=1)throw new Exception("BuildSettings: "+name);}
  var map=Object.FindAnyObjectByType<Tilemap>();if(map.GetUsedTilesCount()!=9||map.GetUsedTiles().Any(t=>!AssetDatabase.GetAssetPath(t).Contains("FrostwayVillageV10/Tiles/")))throw new Exception("승인 Tile9 참조 오류");
  Debug.Log("MAIN23_FINAL_SCENE_VALIDATION_PASS");
 }
 public static void Build(){
  if(!Application.dataPath.Replace('\\','/').Contains("/Temp/Main23FirstPlayable20261023/Worktree/Unity/Client/Assets"))throw new Exception("Main23 격리 경로만 허용합니다.");
  string path="Assets/_Project/Resources/QuestDefinitions/Main23_WarmLand.asset";
  var quest=AssetDatabase.LoadAssetAtPath<QuestDefinition>(path);if(quest==null){quest=ScriptableObject.CreateInstance<QuestDefinition>();AssetDatabase.CreateAsset(quest,path);}
  var ids=new[]{"reach_deep_north_f1","inspect_two_southbound_traces","arrive_frostway_south_gate","hear_frostway_testimony","register_frostway_checkpoint"};
  var texts=new[]{"F1 심부에 도착하세요.","동물과 장작·짐수레의 남하 흔적을 각각 조사하세요.","서릿길 마을 남문으로 들어가세요.","주민 증언과 태온·하렌의 판단을 끝까지 들으세요.","중앙 안전거점을 등록하고 저장하세요."};
  var targets=new[]{Chapter3Main23Flow.Deep,Chapter3Main23Flow.Traces,Chapter3Main23Flow.Arrival,Chapter3Main23Flow.Guide,Chapter3Main23Flow.Checkpoint};
  var types=new[]{QuestObjectiveType.ReachLocation,QuestObjectiveType.Interact,QuestObjectiveType.ReachLocation,QuestObjectiveType.TalkToNpc,QuestObjectiveType.Interact};
  quest.ConfigureForAudit(Chapter3Main23Flow.QuestId,"따뜻한 땅을 떠난 자들",QuestType.Main,Enumerable.Range(0,5).Select(i=>{var s=new QuestObjectiveDefinition();s.ConfigureForAudit(ids[i],texts[i],types[i],targets[i],i==1?2:1);return s;}).ToArray(),new RewardBundle(),new[]{Chapter3Main22Flow.QuestId});
  quest.ConfigureDescription("첫 플레이 임시 대사·미확정 보상 무지급. 정식 수치와 Voice ID는 별도 승인.");EditorUtility.SetDirty(quest);
  var field=EditorSceneManager.OpenScene(Scenes+Chapter3Main23Flow.Field+".unity",OpenSceneMode.Single);
  var bounds=Object.FindAnyObjectByType<WorldBounds2D>();bounds.Configure(new Vector2(0,6),new Vector2(32,36));
  // 북벽만 제거·연장하고 기존 남부의 Collider와 좌표를 유지합니다. 명시 선택 출구에는 물리 stop을 유지합니다.
  foreach(var c in Object.FindObjectsByType<BoxCollider2D>(FindObjectsSortMode.None).Where(c=>c.name.StartsWith("Boundary_Top_")).ToArray())Object.DestroyImmediate(c.gameObject);
  Box("Main23_NorthManualStop",new Vector2(0,24.2f),new Vector2(32,.4f));Box("Main23_LeftExtension",new Vector2(-16.2f,18),new Vector2(.4f,12));Box("Main23_RightExtension",new Vector2(16.2f,18),new Vector2(.4f,12));
  var ground=Object.FindObjectsByType<Tilemap>(FindObjectsSortMode.None).OrderByDescending(m=>m.GetUsedTilesCount()).First();var tiles=ground.GetUsedTiles();
  var ext=new GameObject("Main23_NorthernGround",typeof(Grid));ext.GetComponent<Grid>().cellSize=new Vector3(2,2,1);var map=new GameObject("Ground",typeof(Tilemap),typeof(TilemapRenderer)).GetComponent<Tilemap>();map.transform.SetParent(ext.transform,false);map.GetComponent<TilemapRenderer>().sortingOrder=-10;
  for(int x=-8;x<8;x++)for(int y=6;y<12;y++)map.SetTile(new Vector3Int(x,y,0),tiles[0]);
  Spawn(Chapter3Main23Flow.ReturnSpawn,new Vector2(0,20));EditorSceneManager.SaveScene(field);
  string villagePath=Scenes+Chapter3Main23Flow.Village+".unity";
  if(!File.Exists(villagePath)&&!AssetDatabase.CopyAsset(Scenes+Chapter3Main23Flow.Field+".unity",villagePath))throw new Exception("마을 Scene 복제 실패");
  var village=EditorSceneManager.OpenScene(villagePath,OpenSceneMode.Single);
  // 카메라와 Player 구성만 재사용합니다. F1 전투·환경·Spawn은 새 마을에 남기지 않습니다.
  foreach(var root in village.GetRootGameObjects())if(root.GetComponentInChildren<PlayerController>()==null&&root.GetComponentInChildren<Camera>()==null)Object.DestroyImmediate(root);
  var b=new GameObject("WorldBounds",typeof(WorldBounds2D)).GetComponent<WorldBounds2D>();b.Configure(Vector2.zero,new Vector2(40,28));WorldBounds2D.CreateBoundaryColliders(b.transform,b.Bounds,.4f);
  var grid=new GameObject("ApprovedV10_Tile2WU",typeof(Grid));grid.GetComponent<Grid>().cellSize=new Vector3(2,2,1);
  var tilemap=new GameObject("ApprovedV10_Ground",typeof(Tilemap),typeof(TilemapRenderer)).GetComponent<Tilemap>();tilemap.transform.SetParent(grid.transform,false);tilemap.GetComponent<TilemapRenderer>().sortingOrder=-10;
  var approved=Resources.LoadAll<Tile>("FrostwayVillageV10/Tiles").OrderBy(t=>t.name).ToArray();if(approved.Length!=9)throw new Exception("승인 Tile9 필요");
  for(int x=-10;x<10;x++)for(int y=-7;y<7;y++){int index=Math.Abs(x*13+y*7)%3;bool road=Math.Abs(x)<=1||Math.Abs(y)<=1;tilemap.SetTile(new Vector3Int(x,y,0),approved[road?3+index:((x+y)%4==0?6:0)+index]);}
  Spawn(Chapter3Main23Flow.ArrivalSpawn,new Vector2(0,-8));Spawn(Chapter3Main23Flow.SafeSpawn,new Vector2(0,2));
  var player=Object.FindAnyObjectByType<PlayerController>();player.transform.position=new Vector2(0,-8);player.GetComponent<Rigidbody2D>().position=player.transform.position;
  // 서비스 건물은 최종 미술이 아니므로 텍스트가 붙는 단순 영역만 예약합니다. 남북 주 통로는 4WU 이상 유지합니다.
  Box("VISUAL_PLACEHOLDER_Inn",new Vector2(-9,3),new Vector2(3,3));Box("VISUAL_PLACEHOLDER_Party",new Vector2(9,3),new Vector2(3,3));
  EditorSceneManager.SaveScene(village);var scenes=EditorBuildSettings.scenes.ToList();if(!scenes.Any(s=>s.path==villagePath))scenes.Add(new EditorBuildSettingsScene(villagePath,true));EditorBuildSettings.scenes=scenes.ToArray();AssetDatabase.SaveAssets();QuestCatalog.ReloadForAudit();Debug.Log("MAIN23_CONTENT_BUILD_PASS");
 }
 static void Spawn(string id,Vector2 at){var g=new GameObject(id,typeof(SceneSpawnPoint));g.transform.position=at;g.GetComponent<SceneSpawnPoint>().Configure(id);}
 static void Box(string name,Vector2 at,Vector2 size){var g=new GameObject(name,typeof(BoxCollider2D));g.transform.position=at;g.GetComponent<BoxCollider2D>().size=size;}
}
}
