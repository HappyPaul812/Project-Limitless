using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ProjectLimitless.EditorTools
{
    /// <summary>기존 Main18/19 Audit을 격리 Batch에서 재사용합니다. 결과 기록 및 Editor 종료만 추가하며 테스트 기능을 바꾸지 않습니다.</summary>
    public static class Main20RegressionBatch
    {
        private static bool main19;
        private static double deadline;
        public static void RunMain19(){main19=true;Begin();Main19RuntimeAudit.Launch();}
        public static void RunMain18(){main19=false;Begin();Main18RuntimeAudit.Launch();}
        private static void Begin()
        {
            if(!Application.isBatchMode||!Application.dataPath.Replace('\\','/').Contains("/Temp/Main20QA/Client/Assets"))throw new InvalidOperationException("격리 Main20QA만 허용합니다.");
            Directory.CreateDirectory(Path.GetFullPath(Path.Combine(Application.dataPath,"../../../문서/00_프로젝트")));
            EditorSceneManager.OpenScene("Assets/_Project/Scenes/Bootstrap.unity",OpenSceneMode.Single);
            deadline=EditorApplication.timeSinceStartup+540;EditorApplication.update+=Tick;
        }
        private static void Tick()
        {
            string status=main19?Main19RuntimeAudit.Status:Main18RuntimeAudit.Status;
            if(status=="PASS"){EditorApplication.update-=Tick;EditorApplication.Exit(0);}
            else if(status=="FAIL"||EditorApplication.timeSinceStartup>deadline){Debug.LogError("회귀 QA 실패/시간 초과: "+status);EditorApplication.Exit(1);}
        }
    }
}
