using ProjectLimitless.Core;
using UnityEngine;
using UnityEngine.Audio;

namespace ProjectLimitless.Audio
{
    public enum GameAudioChannel { Voice, SFX, BGM }

    /// <summary>
    /// 사용자 환경 설정을 정식 Mixer에 적용합니다. 슬롯 저장이나 Scene 이동과 독립적으로
    /// 같은 Mixer Asset을 공유하며, AudioSource마다 공용 음량을 다시 곱하지 않습니다.
    /// </summary>
    public static class AudioSettingsService
    {
        private const string ResourcePath = "Audio/LimitlessAudioMixer";
        private static AudioMixer mixer;
        private static bool initialized;
        private static readonly AudioMixerGroup[] groups = new AudioMixerGroup[3];
        public static AudioMixer Mixer { get { Initialize(); return mixer; } }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetRuntime()
        {
            UserSettingsService.Changed -= Apply;
            initialized = false; mixer = null;
            for (int i = 0; i < groups.Length; i++) groups[i] = null;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void StartRuntime()
        {
            Initialize();
            new GameObject("AudioSettingsSystem").AddComponent<ProjectLimitless.UI.AudioSettingsPresenter>();
        }

        private static void Initialize()
        {
            if (initialized) return;
            initialized = true;
            UserSettingsService.Load();
            mixer = Resources.Load<AudioMixer>(ResourcePath);
            if (mixer == null) { Debug.LogError("정식 AudioMixer를 찾지 못했습니다: " + ResourcePath); return; }
            for (int i = 0; i < groups.Length; i++)
            {
                string name = ((GameAudioChannel)i).ToString();
                AudioMixerGroup[] matches = mixer.FindMatchingGroups(name);
                foreach (AudioMixerGroup group in matches) if (group.name == name) { groups[i] = group; break; }
                if (groups[i] == null) Debug.LogError("AudioMixer Group을 찾지 못했습니다: " + name);
            }
            UserSettingsService.Changed += Apply;
        }

        /// <summary>새 음원은 용도를 명시해 이 공통 진입점으로 Group에 연결합니다.</summary>
        public static bool Route(AudioSource source, GameAudioChannel channel)
        {
            Initialize();
            int index = (int)channel;
            if (source == null || index < 0 || index >= groups.Length || groups[index] == null) return false;
            source.outputAudioMixerGroup = groups[index];
            return true;
        }

        public static float ToDecibels(int value)
        {
            return value <= 0 ? -80f : 20f * Mathf.Log10(Mathf.Clamp(value, 1, 100) / 100f);
        }

        /// <summary>Start 이후 적용하며 음소거는 Master만 변경하고 채널값은 독립적으로 유지합니다.</summary>
        public static void Apply()
        {
            Initialize();
            if (mixer == null) return;
            SetParameter("MasterVolume", UserSettingsService.MuteAll ? -80f : 0f);
            SetParameter("VoiceVolume", ToDecibels(UserSettingsService.VoiceVolume));
            SetParameter("SFXVolume", ToDecibels(UserSettingsService.SfxVolume));
            SetParameter("BGMVolume", ToDecibels(UserSettingsService.BgmVolume));
        }

        private static void SetParameter(string name, float value)
        {
            if (!mixer.SetFloat(name, value)) Debug.LogError("AudioMixer 노출 파라미터를 찾지 못했습니다: " + name);
        }
    }
}
