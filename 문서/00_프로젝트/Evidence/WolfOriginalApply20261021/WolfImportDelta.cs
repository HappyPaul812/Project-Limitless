using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using ProjectLimitless.Battle;

// 원본에서 readback한 여섯 파일의 동일 복사본만 Import합니다. 기존106 PlayMode 테스트를 재실행하지 않습니다.
public static class WolfImportDelta
{
 public static void Run()
 {
  if(!Application.isBatchMode||!Application.dataPath.Replace('\\','/').Contains("/Temp/WolfRunV5Apply20261021/UnityQA/Assets"))throw new Exception("격리 경로만 허용합니다.");
  const string path="Assets/_Project/Resources/CompanionAssaultValidation/Wolf_Run6_v5.png";
  AssetDatabase.ImportAsset(path,ImportAssetOptions.ForceUpdate);
  var t=Resources.Load<Texture2D>(BeastCompanionCatalog.Get("wolf").RunResourcePath);var imp=AssetImporter.GetAtPath(path) as TextureImporter;
  bool ok=t!=null&&t.width==3072&&t.height==384&&imp!=null&&imp.maxTextureSize>=3072&&t.isReadable;
  if(!ok)throw new Exception("Wolf Import 계약 불일치");
  string output=Path.GetFullPath(Path.Combine(Application.dataPath,"../../../WolfOriginalApply20261021/ImportDelta.json"));
  File.WriteAllText(output,"{\"import\":\"PASS\",\"width\":3072,\"height\":384,\"readable\":true,\"compile\":\"PASS\",\"playmode_repeated\":false,\"original_editor_import\":\"NOT_VERIFIED\"}");
  Debug.Log("WOLF_IMPORT_DELTA_COMPLETE");EditorApplication.Exit(0);
 }
}
