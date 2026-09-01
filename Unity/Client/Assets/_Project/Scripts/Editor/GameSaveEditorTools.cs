#if UNITY_EDITOR
using System.IO;
using ProjectLimitless.Core;
using UnityEditor;
using UnityEngine;

namespace ProjectLimitless.EditorTools
{
    /// <summary>실제 게임 UI에 개발 버튼을 남기지 않고 슬롯별 저장을 확인·삭제하는 Editor 전용 창입니다.</summary>
    public sealed class GameSaveEditorTools : EditorWindow
    {
        [MenuItem("Project Limitless/Test/Manage Local Saves")]
        private static void Open() => GetWindow<GameSaveEditorTools>("Local Saves");

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Project-Limitless 로컬 저장", EditorStyles.boldLabel);
            EditorGUILayout.SelectableLabel(GameSaveService.SaveDirectoryPath, EditorStyles.textField, GUILayout.Height(38));
            for (int slot = 1; slot <= GameSaveService.DefaultSaveSlotCount; slot++)
            {
                string path = GameSaveService.GetSaveFilePath(slot);
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField($"슬롯 {slot}: {(File.Exists(path) ? Path.GetFileName(path) : "비어 있음")}");
                    using (new EditorGUI.DisabledScope(!File.Exists(path)))
                        if (GUILayout.Button("삭제", GUILayout.Width(70)) && EditorUtility.DisplayDialog("저장 삭제", $"슬롯 {slot}을 삭제할까요?\n{path}", "삭제", "취소")) Delete(path);
                }
            }
            EditorGUILayout.Space();
            if (GUILayout.Button("모든 슬롯 삭제") && EditorUtility.DisplayDialog("모든 저장 삭제", "모든 캐릭터 슬롯의 저장 파일을 삭제할까요?", "모두 삭제", "취소"))
                for (int slot = 1; slot <= GameSaveService.DefaultSaveSlotCount; slot++) Delete(GameSaveService.GetSaveFilePath(slot));
            EditorGUILayout.HelpBox("기존 단일 저장 원본은 마이그레이션 확인을 위해 이 창에서 삭제하지 않습니다.", MessageType.Info);
        }

        private static void Delete(string path) { if (!File.Exists(path)) return; File.Delete(path); Debug.Log($"테스트용 로컬 저장 파일을 삭제했습니다: {path}"); }
    }
}
#endif
