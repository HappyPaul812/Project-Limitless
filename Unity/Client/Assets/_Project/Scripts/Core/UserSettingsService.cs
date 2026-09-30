using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace ProjectLimitless.Core
{
    [Serializable]
    public sealed class UserSettingsData
    {
        public int Version = 2;
        public bool SkipOpeningIntro;
        public bool MuteAll;
        public int VoiceVolume = 100;
        public int SfxVolume = 100;
        public int BgmVolume = 80;
    }

    /// <summary>
    /// 모든 캐릭터 슬롯이 공유하는 사용자 설정을 별도 JSON으로 관리합니다. 시작 이야기 건너뛰기와
    /// Audio 음량은 캐릭터 성장 정보가 아니므로 슬롯 변경·삭제와 무관하게 유지합니다.
    /// </summary>
    public static class UserSettingsService
    {
        private const int CurrentVersion = 2;
        private const string FileName = "user_settings.json";
        private static UserSettingsData cached;
        public static event Action Changed;

#if UNITY_EDITOR
        // Runtime QA가 실제 사용자 설정을 덮어쓰지 않게 별도 폴더를 사용할 수 있습니다.
        public static string AuditSettingsDirectory { get; private set; }
        public static void BeginAudit(string directory) { AuditSettingsDirectory = directory; cached = null; }
        public static void FinishAudit() { AuditSettingsDirectory = null; Reload(); }
#endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetRuntimeCache() { cached = null; Changed = null; }

        public static string SettingsDirectoryPath
        {
            get
            {
#if UNITY_EDITOR
                if (!string.IsNullOrEmpty(AuditSettingsDirectory)) return AuditSettingsDirectory;
                string clientRoot = Directory.GetParent(Application.dataPath)?.FullName ?? Application.dataPath;
                return Path.Combine(clientRoot, "UserData", "Settings");
#else
                return Path.Combine(Application.persistentDataPath, "Settings");
#endif
            }
        }

        public static string SettingsFilePath => Path.Combine(SettingsDirectoryPath, FileName);
        public static bool SkipOpeningIntro => Load().SkipOpeningIntro;
        public static bool MuteAll => Load().MuteAll;
        public static int VoiceVolume => Load().VoiceVolume;
        public static int SfxVolume => Load().SfxVolume;
        public static int BgmVolume => Load().BgmVolume;

        public static void SetMuteAll(bool value)
        {
            UserSettingsData settings = Load();
            if (settings.MuteAll == value) return;
            // 음소거는 채널값을 덮어쓰지 않습니다. 해제하면 변경 전/음소거 중 설정한 값을 사용합니다.
            settings.MuteAll = value;
            Save(settings);
            Changed?.Invoke();
        }

        public static void SetAudioVolumes(int voice, int sfx, int bgm)
        {
            UserSettingsData settings = Load();
            voice = Mathf.Clamp(voice, 0, 100); sfx = Mathf.Clamp(sfx, 0, 100); bgm = Mathf.Clamp(bgm, 0, 100);
            if (settings.VoiceVolume == voice && settings.SfxVolume == sfx && settings.BgmVolume == bgm) return;
            settings.VoiceVolume = voice; settings.SfxVolume = sfx; settings.BgmVolume = bgm;
            Save(settings);
            Changed?.Invoke();
        }

        /// <summary>파일의 설정을 다시 읽습니다. Scene 이동 때는 시작 시 읽은 캐시를 공유합니다.</summary>
        public static UserSettingsData Reload()
        {
            cached = null;
            UserSettingsData settings = Load();
            Changed?.Invoke();
            return settings;
        }

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
                // 기본값을 가진 객체에 덮어써 구버전 JSON에 없는 새 항목이 0으로 초기화되지 않게 합니다.
                UserSettingsData loaded = new UserSettingsData();
                JsonUtility.FromJsonOverwrite(File.ReadAllText(SettingsFilePath, Encoding.UTF8), loaded);
                if (loaded.Version == 1 || loaded.Version == CurrentVersion)
                {
                    if (loaded.Version == 1)
                    {
                        // Version1의 Skip 설정은 보존하고 새 Audio 항목만 정식 기본값으로 이행합니다.
                        loaded.MuteAll = false; loaded.VoiceVolume = 100; loaded.SfxVolume = 100; loaded.BgmVolume = 80;
                    }
                    loaded.Version = CurrentVersion;
                    loaded.VoiceVolume = Mathf.Clamp(loaded.VoiceVolume, 0, 100);
                    loaded.SfxVolume = Mathf.Clamp(loaded.SfxVolume, 0, 100);
                    loaded.BgmVolume = Mathf.Clamp(loaded.BgmVolume, 0, 100);
                    cached = loaded;
                }
                else Debug.LogWarning("사용자 설정 형식을 읽을 수 없어 안전한 기본값을 사용합니다.");
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"사용자 설정이 손상되어 안전한 기본값을 사용합니다: {exception.Message}");
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
