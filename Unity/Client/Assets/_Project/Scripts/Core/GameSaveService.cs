using System;
using System.IO;
using System.Linq;
using System.Text;
using ProjectLimitless.Player;
using UnityEngine;

namespace ProjectLimitless.Core
{
    /// <summary>JSON은 값만 저장합니다. Unity Object 대신 기존 데이터 Asset을 다시 찾을 안정적인 ID를 기록합니다.</summary>
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
        // Version 1 파일에 이 필드가 없어도 JsonUtility는 false/0으로 채우므로 기존 저장은 SpawnPoint fallback을 사용합니다.
        public bool HasSavedWorldPosition;
        public float SavedPositionX;
        public float SavedPositionY;
    }

    public enum SaveSlotState { Empty, Valid, Invalid }

    public sealed class SaveSlotInfo
    {
        public int SlotIndex { get; }
        public SaveSlotState State { get; }
        public GameSaveData Data { get; }
        public string Error { get; }
        public SaveSlotInfo(int slotIndex, SaveSlotState state, GameSaveData data = null, string error = "")
        { SlotIndex = slotIndex; State = state; Data = data; Error = error; }
    }

    /// <summary>
    /// GameSessionData는 지금 실행 중인 메모리이고 GameSaveData는 게임을 꺼도 남는 디스크 기록입니다.
    /// 둘을 분리하면 기존 Scene은 Session을 그대로 쓰면서 저장 형식만 안전하게 버전 관리할 수 있습니다.
    /// </summary>
    public static class GameSaveService
    {
        public const int CurrentVersion = 1;
        public const int DefaultSaveSlotCount = 5;
        public const string LegacySaveFileName = "project_limitless_save.json";
        private const string SlotFilePattern = "save_slot_{0:D2}.json";

        /// <summary>0은 아직 Bootstrap에서 슬롯을 고르지 않았다는 뜻입니다.</summary>
        public static int CurrentSlotIndex { get; private set; }

        public static string SaveDirectoryPath
        {
            get
            {
#if UNITY_EDITOR
                // dataPath는 .../Unity/Client/Assets입니다. 부모에서 계산하므로 PC의 드라이브 경로를 하드코딩하지 않습니다.
                string clientRoot = Directory.GetParent(Application.dataPath)?.FullName ?? Application.dataPath;
                return Path.Combine(clientRoot, "UserData", "Saves");
#else
                // Standalone 최종 정책은 미확정이며, 현재 빌드는 OS가 보장하는 사용자 저장 위치를 사용합니다.
                return Path.Combine(Application.persistentDataPath, "Saves");
#endif
            }
        }

        public static string LegacySaveFilePath => Path.Combine(Application.persistentDataPath, LegacySaveFileName);

        public static bool SelectSlot(int slotIndex)
        {
            if (!IsValidSlotIndex(slotIndex)) { Debug.LogWarning($"존재하지 않는 저장 슬롯입니다: {slotIndex}"); return false; }
            CurrentSlotIndex = slotIndex;
            return true;
        }

        public static string GetSaveFilePath(int slotIndex)
        {
            if (!IsValidSlotIndex(slotIndex)) throw new ArgumentOutOfRangeException(nameof(slotIndex));
            return Path.Combine(SaveDirectoryPath, string.Format(SlotFilePattern, slotIndex));
        }

        public static SaveSlotInfo InspectSlot(int slotIndex)
        {
            string path = GetSaveFilePath(slotIndex);
            if (!File.Exists(path)) return new SaveSlotInfo(slotIndex, SaveSlotState.Empty);
            return TryRead(path, out GameSaveData data, out string error)
                ? new SaveSlotInfo(slotIndex, SaveSlotState.Valid, data)
                : new SaveSlotInfo(slotIndex, SaveSlotState.Invalid, null, error);
        }

        /// <summary>향후 레벨업도 호출할 공용 API이며 현재 선택 슬롯 하나만 갱신합니다.</summary>
        public static bool SaveCurrentSession(string sceneId = null, string spawnPointId = null)
        {
            if (!IsValidSlotIndex(CurrentSlotIndex))
            {
                Debug.LogWarning("선택된 저장 슬롯이 없어 자동 저장을 건너뜁니다. Bootstrap 슬롯에서 게임을 시작해주세요.");
                return false;
            }
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
                SpawnPointId = resolvedSpawn,
                HasSavedWorldPosition = GameSessionData.HasSavedWorldPosition,
                SavedPositionX = GameSessionData.SavedPositionX,
                SavedPositionY = GameSessionData.SavedPositionY
            };
            if (!Validate(data, out string validationError)) { Debug.LogError($"슬롯 {CurrentSlotIndex}을 저장하지 못했습니다: {validationError}"); return false; }

            string path = GetSaveFilePath(CurrentSlotIndex);
            try
            {
                Directory.CreateDirectory(SaveDirectoryPath);
                string temporaryPath = path + ".tmp";
                File.WriteAllText(temporaryPath, JsonUtility.ToJson(data, true), new UTF8Encoding(false));
                File.Copy(temporaryPath, path, true);
                File.Delete(temporaryPath);
                Debug.Log($"슬롯 {CurrentSlotIndex} 자동 저장 완료: {path}");
                return true;
            }
            catch (Exception exception)
            { Debug.LogError($"슬롯 {CurrentSlotIndex}을 쓰지 못했습니다. 기존 파일은 삭제하지 않습니다: {exception.Message}"); return false; }
        }

        public static bool TryLoadSlot(int slotIndex, out GameSaveData data)
        {
            SaveSlotInfo info = InspectSlot(slotIndex);
            data = info.Data;
            if (info.State == SaveSlotState.Invalid) Debug.LogWarning($"슬롯 {slotIndex}은 불러올 수 없습니다. 다른 슬롯은 계속 사용할 수 있습니다: {info.Error}");
            return info.State == SaveSlotState.Valid;
        }

        public static void RestoreSession(int slotIndex, GameSaveData data)
        {
            if (!Validate(data, out string error)) throw new InvalidOperationException(error);
            if (!SelectSlot(slotIndex)) throw new InvalidOperationException($"존재하지 않는 저장 슬롯입니다: {slotIndex}");
            Enum.TryParse(data.PlayerVisualId, true, out PlayerVisualType visual);
            GameSessionData.Reset();
            GameSessionData.ConfigurePlayer(visual, data.PlayerName);
            GameSessionData.SelectPlayerPath(data.PathId);
            GameSessionData.SelectJob(data.JobId);
            GameSessionData.ConfigureProgress(data.Level, data.CurrentExperience);
            GameSessionData.RecordLocation(data.CurrentSceneId, data.SpawnPointId);
            // 캐릭터 본체가 유효하면 좌표 하나가 손상됐다는 이유로 슬롯 전체를 막지 않습니다.
            // 좌표만 무효화하면 다음 Scene에서 기존 SpawnPoint가 안전 fallback으로 동작합니다.
            if (data.HasSavedWorldPosition && IsFinite(data.SavedPositionX) && IsFinite(data.SavedPositionY))
                GameSessionData.RecordWorldPosition(data.SavedPositionX, data.SavedPositionY);
            else GameSessionData.ClearWorldPosition();
            GameSessionData.SetPendingSpawnPoint(data.SpawnPointId);
        }

        /// <summary>현재 월드 좌표를 Session에 기록한 뒤 현재 슬롯에 저장합니다.</summary>
        public static bool SaveCurrentWorldPosition(Vector2 position, string sceneId, string spawnPointId = null)
        {
            if (!IsFinite(position.x) || !IsFinite(position.y)) { Debug.LogWarning("NaN 또는 Infinity 월드 좌표는 저장하지 않습니다."); return false; }
            GameSessionData.RecordWorldPosition(position.x, position.y);
            return SaveCurrentSession(sceneId, spawnPointId);
        }

        /// <summary>
        /// 캐릭터 삭제는 선택한 JSON 하나만 지웁니다. 다른 슬롯까지 함께 지우면 서로 독립적인 캐릭터 진행이 훼손됩니다.
        /// </summary>
        public static bool DeleteSlot(int slotIndex, out string error)
        {
            error = string.Empty;
            try
            {
                string path = GetSaveFilePath(slotIndex);
                if (File.Exists(path)) File.Delete(path);
                if (CurrentSlotIndex == slotIndex) CurrentSlotIndex = 0;
                return true;
            }
            catch (Exception exception)
            {
                error = exception.Message;
                Debug.LogError($"슬롯 {slotIndex} 저장을 삭제하지 못했습니다: {exception.Message}");
                return false;
            }
        }

        /// <summary>
        /// 옛 단일 저장이 유효하고 슬롯 1이 비어 있을 때만 복사합니다. 원본을 삭제하지 않고 기존 슬롯도
        /// 덮어쓰지 않아 마이그레이션 실패가 사용자 진행을 훼손하지 않게 합니다.
        /// </summary>
        public static void TryMigrateLegacySaveToSlotOne()
        {
            string slotOnePath = GetSaveFilePath(1);
            if (File.Exists(slotOnePath) || !File.Exists(LegacySaveFilePath)) return;
            if (!TryRead(LegacySaveFilePath, out _, out string error)) { Debug.LogWarning($"기존 단일 저장을 슬롯 1로 옮기지 않았습니다: {error}"); return; }
            try
            {
                Directory.CreateDirectory(SaveDirectoryPath);
                File.Copy(LegacySaveFilePath, slotOnePath, false);
                Debug.Log($"기존 단일 저장을 슬롯 1로 복사했습니다. 원본은 유지합니다: {slotOnePath}");
            }
            catch (Exception exception) { Debug.LogWarning($"기존 단일 저장 마이그레이션에 실패했습니다. 원본은 유지합니다: {exception.Message}"); }
        }

        private static bool TryRead(string path, out GameSaveData data, out string error)
        {
            data = null; error = string.Empty;
            try
            {
                data = JsonUtility.FromJson<GameSaveData>(File.ReadAllText(path, Encoding.UTF8));
                if (Validate(data, out error)) return true;
            }
            catch (Exception exception) { error = $"JSON이 손상되었거나 읽을 수 없습니다: {exception.Message}"; }
            data = null; return false;
        }

        private static bool IsValidSlotIndex(int slotIndex) => slotIndex >= 1 && slotIndex <= DefaultSaveSlotCount;

        private static bool Validate(GameSaveData data, out string error)
        {
            if (data == null) { error = "저장 내용이 비어 있습니다."; return false; }
            // Version은 옛 JSON을 잘못 해석하지 않고 향후 명시적인 형식 변환을 할 기준입니다.
            if (data.Version != CurrentVersion) { error = $"지원하지 않는 저장 버전입니다. 파일 {data.Version}, 현재 {CurrentVersion}"; return false; }
            if (string.IsNullOrWhiteSpace(data.PlayerName)) { error = "캐릭터 이름이 없습니다."; return false; }
            if (!Enum.TryParse(data.PlayerVisualId, true, out PlayerVisualType visual) || !Enum.IsDefined(typeof(PlayerVisualType), visual)) { error = $"알 수 없는 Player Visual ID입니다: {data.PlayerVisualId}"; return false; }
            if (!Resources.LoadAll<PlayerPathDefinition>("PathDefinitions").Any(item => item.Id == data.PathId)) { error = $"알 수 없는 Path ID입니다: {data.PathId}"; return false; }
            if (!Resources.LoadAll<JobDefinition>("JobDefinitions").Any(item => item.JobId == data.JobId)) { error = $"알 수 없는 Job ID입니다: {data.JobId}"; return false; }
            if (string.IsNullOrWhiteSpace(data.CurrentSceneId) || IsNonWorldSaveScene(data.CurrentSceneId) || !Application.CanStreamedLevelBeLoaded(data.CurrentSceneId)) { error = $"이어갈 수 없는 Scene ID입니다: {data.CurrentSceneId}"; return false; }
            if (data.Level < 1 || data.Level > CharacterGrowthCalculator.MaxLevel || data.CurrentExperience < 0)
            { error = $"레벨은 1~{CharacterGrowthCalculator.MaxLevel} 범위이고 경험치는 0 이상이어야 합니다."; return false; }
            if (data.SpawnPointId != null && (data.SpawnPointId.Length > 128 || data.SpawnPointId.Contains("/") || data.SpawnPointId.Contains("\\"))) { error = "SpawnPoint ID 형식이 올바르지 않습니다."; return false; }
            error = string.Empty; return true;
        }

        private static bool IsFinite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);

        private static bool IsNonWorldSaveScene(string sceneId)
        {
            // 생성 중간이나 Battle을 저장하면 미완성 선택·전투 상태까지 복원해야 하므로 자동 저장 시점을 제한합니다.
            return sceneId == "Bootstrap" || sceneId == "OpeningIntro" || sceneId == "CharacterCreation" || sceneId == "PathSelection" || sceneId == "JobSelection" || sceneId == "FinalConfirmation" || sceneId == "Battle" || sceneId == "SampleScene";
        }
    }
}
