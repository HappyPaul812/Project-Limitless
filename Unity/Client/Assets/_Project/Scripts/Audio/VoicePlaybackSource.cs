using UnityEngine;

namespace ProjectLimitless.Audio
{
    /// <summary>
    /// 대사 전용 AudioSource의 재생 상태를 관리합니다. 향후 Voice AudioMixer가 추가되면 이
    /// 컴포넌트의 AudioSource 출력만 연결해 전체·음악·효과음과 독립된 음성 음량을 적용할 수 있습니다.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class VoicePlaybackSource : MonoBehaviour
    {
        private AudioSource source;

        public float ClipLength => source != null && source.clip != null ? source.clip.length : 0f;

        private void Awake()
        {
            source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = false;
            source.spatialBlend = 0f;
        }

        public void Play(AudioClip clip)
        {
            // 다음 대사 전에 이전 음성을 멈춰 두 문장이 겹치지 않게 합니다.
            Stop();
            source.clip = clip;
            if (clip != null) source.Play();
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
    }
}
