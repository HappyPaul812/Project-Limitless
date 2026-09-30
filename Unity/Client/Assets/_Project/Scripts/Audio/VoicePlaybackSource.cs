using UnityEngine;

namespace ProjectLimitless.Audio
{
    /// <summary>
    /// 대사 전용 AudioSource의 재생과 정리를 담당합니다. 공용 음성 음량은 Voice Mixer에서만
    /// 적용하므로 Source 음량을 다시 곱하지 않고 Scene마다 같은 사용자 설정을 따릅니다.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class VoicePlaybackSource : MonoBehaviour
    {
        private AudioSource source;

        public float ClipLength => source != null && source.clip != null ? source.clip.length : 0f;
        public AudioClip Clip => source != null ? source.clip : null;
        public bool IsPlaying => source != null && source.isPlaying;
        public bool IsAudible => source != null && source.enabled && source.gameObject.activeInHierarchy
            && !source.mute && source.volume > 0f && Mathf.Approximately(source.spatialBlend, 0f)
            && !ProjectLimitless.Core.UserSettingsService.MuteAll && ProjectLimitless.Core.UserSettingsService.VoiceVolume > 0;

        private void Awake()
        {
            source = gameObject.AddComponent<AudioSource>();
            source.enabled = true;
            source.playOnAwake = false;
            source.loop = false;
            source.volume = 1f;
            source.mute = false;
            source.spatialBlend = 0f;
            AudioSettingsService.Route(source, GameAudioChannel.Voice);
        }

        public bool Play(AudioClip clip)
        {
            // 다음 대사 전에 이전 음성을 멈춰 두 문장이 겹치지 않게 합니다.
            Stop();
            source.clip = clip;
            if (clip == null) return false;

            source.Play();
            return source.isPlaying;
        }

        public void Pause()
        {
            // Pause는 재생 위치를 보존하므로 P로 다시 시작할 때 처음부터 읽지 않습니다.
            if (source != null && source.isPlaying) source.Pause();
        }

        public void Resume()
        {
            if (source != null && source.clip != null) source.UnPause();
        }

        public void Stop()
        {
            // Stop은 문장 전환·인트로 종료용이며 이전 재생 위치와 Clip 참조를 함께 지웁니다.
            if (source == null) return;
            source.Stop();
            source.clip = null;
        }

        private void OnDisable() => Stop();
    }
}
