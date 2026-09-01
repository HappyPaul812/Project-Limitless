#if UNITY_EDITOR
using System.IO;
using ProjectLimitless.Core;
using UnityEditor;
using UnityEngine;

namespace ProjectLimitless.EditorTools
{
    /// <summary>실제 게임 UI에 개발 버튼을 노출하지 않고 Editor 메뉴에서만 테스트 저장을 지웁니다.</summary>
    public static class GameSaveEditorTools
    {
        [MenuItem("Project Limitless/Test/Delete Local Save")]
        private static void DeleteLocalSave()
        {
            string path = GameSaveService.SaveFilePath;
            if (!File.Exists(path))
            {
                Debug.Log($"삭제할 로컬 저장 파일이 없습니다: {path}");
                return;
            }
            File.Delete(path);
            Debug.Log($"테스트용 로컬 저장 파일을 삭제했습니다: {path}");
        }
    }
}
#endif
