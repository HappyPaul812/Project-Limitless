using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace ProjectLimitless.Core
{
    [Serializable]
    public sealed class UserSettingsData
    {
        public int Version = 1;
        public bool SkipOpeningIntro;
    }

    /// <summary>
    /// 모든 캐릭터 슬롯이 공유하는 사용자 설정을 별도 JSON으로 관리합니다. 시작 이야기 건너뛰기는
    /// 캐릭터의 성장 정보가 아니므로 슬롯을 바꾸거나 삭제해도 유지되어야 합니다.
    /// </summary>
    public static class UserSettingsService
    {
        private const int CurrentVersion = 1;
        private const string FileName = "user_settings.json";
        private static UserSettingsData cached;

        public static string SettingsDirectoryPath
        {
            get
            {
#if UNITY_EDITOR
                string clientRoot = Directory.GetParent(Application.dataPath)?.FullName ?? Application.dataPath;
                return Path.Combine(clientRoot, "UserData", "Settings");
#else
                return Path.Combine(Application.persistentDataPath, "Settings");
#endif
            }
        }

        public static string SettingsFilePath => Path.Combine(SettingsDirectoryPath, FileName);
        public static bool SkipOpeningIntro => Load().SkipOpeningIntro;

        public static void SetSkipOpeningIntro(bool value)
        {
            UserSettingsData settings = Load();
            if (settings.SkipOpeningIntro == value) return;
            settings.SkipOpeningIntro = value;
            Save(settings);
        }

        public static UserSettingsData Load()
        {
            if (cached != null) return cached;
            cached = new UserSettingsData();
            if (!File.Exists(SettingsFilePath)) return cached;
            try
            {
                UserSettingsData loaded = JsonUtility.FromJson<UserSettingsData>(File.ReadAllText(SettingsFilePath, Encoding.UTF8));
                if (loaded != null && loaded.Version == CurrentVersion) cached = loaded;
                else Debug.LogWarning("사용자 설정 형식을 읽을 수 없어 안전한 기본값을 사용합니다.");
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"사용자 설정이 없거나 손상되어 시작 이야기를 표시합니다: {exception.Message}");
            }
            return cached;
        }

        private static void Save(UserSettingsData settings)
        {
            try
            {
                Directory.CreateDirectory(SettingsDirectoryPath);
                string temporaryPath = SettingsFilePath + ".tmp";
                File.WriteAllText(temporaryPath, JsonUtility.ToJson(settings, true), new UTF8Encoding(false));
                File.Copy(temporaryPath, SettingsFilePath, true);
                File.Delete(temporaryPath);
            }
            catch (Exception exception)
            {
                // 설정 저장 실패가 새 게임 시작 자체를 막지 않도록 메모리 값은 유지하고 오류만 알립니다.
                Debug.LogWarning($"사용자 설정을 저장하지 못했습니다: {exception.Message}");
            }
        }
    }
}
