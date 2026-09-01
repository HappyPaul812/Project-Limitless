using System;
using System.IO;
using System.Linq;
using System.Text;
using ProjectLimitless.Player;
using UnityEngine;

namespace ProjectLimitless.Core
{
    /// <summary>
    /// 디스크에 기록하는 한 슬롯짜리 JSON 자료입니다. JSON은 사람이 읽을 수 있는 글자 형식으로
    /// 객체의 값만 저장하는 방식입니다. Sprite나 ScriptableObject 같은 Unity Object는 실행마다 참조가
    /// 달라질 수 있으므로 저장하지 않고, 다시 데이터 Asset을 찾을 수 있는 안정적인 ID만 기록합니다.
    /// </summary>
    [Serializable]
    public sealed class GameSaveData
    {
        public int Version = GameSaveService.CurrentVersion;
        public string PlayerName = string.Empty;
        public string PlayerVisualId = string.Empty;
        public string PathId = string.Empty;
        public string JobId = string.Empty;
        public int Level = 1;
        public int CurrentExperience;
        public string CurrentSceneId = string.Empty;
        public string SpawnPointId = string.Empty;
    }

    /// <summary>
    /// GameSessionData는 현재 실행 중 빠르게 쓰는 메모리이고 GameSaveData는 게임을 꺼도 남는 디스크 기록입니다.
    /// 이 서비스가 두 자료 사이를 변환하므로 기존 Scene 코드는 GameSessionData를 계속 사용할 수 있습니다.
    /// </summary>
    public static class GameSaveService
    {
        public const int CurrentVersion = 1;
        public const string SaveFileName = "project_limitless_save.json";
        public static string SaveFilePath => Path.Combine(Application.persistentDataPath, SaveFileName);

        public static bool HasSaveFile => File.Exists(SaveFilePath);

        /// <summary>향후 레벨업에서도 호출할 수 있는 공용 저장 API입니다.</summary>
        public static bool SaveCurrentSession(string sceneId = null, string spawnPointId = null)
        {
            string resolvedScene = sceneId ?? GameSessionData.CurrentSceneId;
            string resolvedSpawn = spawnPointId ?? GameSessionData.LastSpawnPointId;
            GameSessionData.RecordLocation(resolvedScene, resolvedSpawn);
            GameSaveData data = new GameSaveData
            {
                PlayerName = GameSessionData.PlayerName,
                PlayerVisualId = GameSessionData.SelectedPlayerVisual.ToString(),
                PathId = GameSessionData.SelectedPlayerPathId,
                JobId = GameSessionData.SelectedJobId,
                Level = GameSessionData.Level,
                CurrentExperience = GameSessionData.CurrentExperience,
                CurrentSceneId = resolvedScene,
                SpawnPointId = resolvedSpawn
            };

            if (!Validate(data, out string validationError))
            {
                Debug.LogError($"저장하지 못했습니다: {validationError}");
                return false;
            }

            try
            {
                string directory = Path.GetDirectoryName(SaveFilePath);
                if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);
                string temporaryPath = SaveFilePath + ".tmp";
                File.WriteAllText(temporaryPath, JsonUtility.ToJson(data, true), new UTF8Encoding(false));
                File.Copy(temporaryPath, SaveFilePath, true);
                File.Delete(temporaryPath);
                Debug.Log($"게임 자동 저장 완료: {SaveFilePath}");
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError($"저장 파일을 쓰지 못했습니다. 기존 파일은 삭제하지 않습니다: {exception.Message}");
                return false;
            }
        }

        public static bool TryLoad(out GameSaveData data)
        {
            data = null;
            if (!HasSaveFile) return false;
            try
            {
                string json = File.ReadAllText(SaveFilePath, Encoding.UTF8);
                data = JsonUtility.FromJson<GameSaveData>(json);
                if (!Validate(data, out string error))
                {
                    Debug.LogWarning($"저장 파일을 불러올 수 없어 새 게임 화면으로 돌아갑니다: {error}");
                    data = null;
                    return false;
                }
                return true;
            }
            catch (Exception exception)
            {
                // 손상 파일을 자동 삭제하거나 덮어쓰지 않아 사용자가 원본을 조사·복구할 수 있게 둡니다.
                Debug.LogWarning($"저장 JSON이 손상되었거나 읽을 수 없습니다. 새 게임을 선택할 수 있습니다: {exception.Message}");
                data = null;
                return false;
            }
        }

        public static void RestoreSession(GameSaveData data)
        {
            if (!Validate(data, out string error)) throw new InvalidOperationException(error);
            Enum.TryParse(data.PlayerVisualId, true, out PlayerVisualType visual);
            GameSessionData.Reset();
            GameSessionData.ConfigurePlayer(visual, data.PlayerName);
            GameSessionData.SelectPlayerPath(data.PathId);
            GameSessionData.SelectJob(data.JobId);
            GameSessionData.ConfigureProgress(data.Level, data.CurrentExperience);
            GameSessionData.RecordLocation(data.CurrentSceneId, data.SpawnPointId);
            GameSessionData.SetPendingSpawnPoint(data.SpawnPointId);
        }

        private static bool Validate(GameSaveData data, out string error)
        {
            if (data == null) { error = "저장 내용이 비어 있습니다."; return false; }
            if (data.Version != CurrentVersion) { error = $"지원하지 않는 저장 버전입니다. 파일 {data.Version}, 현재 {CurrentVersion}"; return false; }
            if (string.IsNullOrWhiteSpace(data.PlayerName)) { error = "캐릭터 이름이 없습니다."; return false; }
            if (!Enum.TryParse(data.PlayerVisualId, true, out PlayerVisualType visual) || !Enum.IsDefined(typeof(PlayerVisualType), visual))
            { error = $"알 수 없는 Player Visual ID입니다: {data.PlayerVisualId}"; return false; }
            if (!Resources.LoadAll<PlayerPathDefinition>("PathDefinitions").Any(item => item.Id == data.PathId))
            { error = $"알 수 없는 Path ID입니다: {data.PathId}"; return false; }
            if (!Resources.LoadAll<JobDefinition>("JobDefinitions").Any(item => item.JobId == data.JobId))
            { error = $"알 수 없는 Job ID입니다: {data.JobId}"; return false; }
            if (string.IsNullOrWhiteSpace(data.CurrentSceneId) || IsNonWorldSaveScene(data.CurrentSceneId) ||
                !Application.CanStreamedLevelBeLoaded(data.CurrentSceneId))
            { error = $"이어갈 수 없는 Scene ID입니다: {data.CurrentSceneId}"; return false; }
            if (data.Level < 1 || data.CurrentExperience < 0) { error = "레벨 또는 경험치 값이 올바르지 않습니다."; return false; }
            if (data.SpawnPointId != null && (data.SpawnPointId.Length > 128 || data.SpawnPointId.Contains("/") || data.SpawnPointId.Contains("\\")))
            { error = "SpawnPoint ID 형식이 올바르지 않습니다."; return false; }
            error = string.Empty;
            return true;
        }

        private static bool IsNonWorldSaveScene(string sceneId)
        {
            // 캐릭터 생성 중간 Scene이나 Battle을 이어하기 위치로 인정하면 미완성 선택 또는 전투 중간
            // 상태를 복원해야 합니다. 1차 저장은 정상 World/Field/Dungeon 도착만 허용합니다.
            return sceneId == "Bootstrap" || sceneId == "CharacterCreation" || sceneId == "PathSelection" ||
                sceneId == "JobSelection" || sceneId == "FinalConfirmation" || sceneId == "Battle" ||
                sceneId == "SampleScene";
        }
    }
}
