using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
// 빌드 실행은 게임 플레이 전수검사가 아니라 승인자산 포함과 PilotBee 배제의 산출물 근거입니다.
public static class EodApprovedBuild {
 [Serializable]public class Result {public string unity,result;public int errors,warnings;public long bytes;public string[] sourceAssets;public string[] legacyFound;}
 public static void Run(){
  if(!Application.isBatchMode||Application.unityVersion!="6000.6.5f1"||!Application.dataPath.Replace('\\','/').EndsWith("/Temp/EODApproved20261104/QA/Client/Assets"))throw new Exception("격리경로/버전실패");
  string output="F:/study/codex/Project-Limitless/Temp/EODApproved20261104";string source="F:/study/codex/Project-Limitless/Temp/EODApproved20261104/Integration/Unity/Client/ProjectSettings/EditorBuildSettings.asset";
  // 통합제품의 실제 enabled Scene목록을 읽되 ProjectSettings 원본을 수정하지 않습니다.
  var lines=File.ReadAllLines(source);var scenes=lines.Where(x=>x.TrimStart().StartsWith("path: ")).Select(x=>x.Trim().Substring(6)).ToArray();if(scenes.Length!=26)throw new Exception("기존26Scene계약차이:"+scenes.Length);
  var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=scenes,locationPathName=output+"/PlayerBuild/LIMITLESS_QA.exe",target=BuildTarget.StandaloneWindows64,options=BuildOptions.Development|BuildOptions.DetailedBuildReport|BuildOptions.CleanBuildCache});
  string[] assets=report.packedAssets.SelectMany(p=>p.contents).Select(p=>p.sourceAssetPath).Distinct().OrderBy(x=>x).ToArray();string[] bad=assets.Where(x=>!x.EndsWith(".cs",StringComparison.OrdinalIgnoreCase)).Where(x=>x.IndexOf("PilotBee",StringComparison.OrdinalIgnoreCase)>=0||x.IndexOf("pilot_bee",StringComparison.OrdinalIgnoreCase)>=0).ToArray();
  var result=new Result{unity=Application.unityVersion,result=report.summary.result.ToString(),errors=(int)report.summary.totalErrors,warnings=(int)report.summary.totalWarnings,bytes=(long)report.summary.totalSize,sourceAssets=assets,legacyFound=bad};File.WriteAllText(output+"/Evidence/BuildReportAssets.json",JsonUtility.ToJson(result,true));File.WriteAllLines(output+"/Evidence/BuildScenes.txt",scenes);EditorApplication.Exit(report.summary.result==BuildResult.Succeeded&&bad.Length==0?0:1);
 }
}
