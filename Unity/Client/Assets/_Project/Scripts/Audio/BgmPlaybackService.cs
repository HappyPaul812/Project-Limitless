using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.Audio
{
    /// <summary>Scene을 넘어 단 하나의 BGM Source를 유지합니다. Voice와 독립적이며 공용 음량은 Mixer에서만 적용합니다.</summary>
    public sealed class BgmPlaybackService : MonoBehaviour
    {
        public static BgmPlaybackService Instance { get; private set; }
        private AudioSource source;
        private BgmSceneCatalog catalog;
        private AudioListener sceneListener;
        private bool westernIntroductionPlaying;
        private Coroutine transition;
        private bool battleMusicActive;
        public AudioClip CurrentClip => source != null ? source.clip : null;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetRuntime() => Instance = null;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            if (Instance == null) new GameObject("BgmPlaybackSystem").AddComponent<BgmPlaybackService>();
        }

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            catalog = Resources.Load<BgmSceneCatalog>("Audio/Music/BgmSceneCatalog");
            source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = true;
            source.spatialBlend = 0f;
            source.volume = 1f;
            AudioSettingsService.Route(source, GameAudioChannel.BGM);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void Start() => ApplyScene(SceneManager.GetActiveScene());

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (mode == LoadSceneMode.Single) ApplyScene(scene);
        }

        private void ApplyScene(Scene scene)
        {
            // 소개 음악의 수명은 해당 Scene 안입니다. 전투·이동·패배 복귀에 이전 소개가 남지 않습니다.
            westernIntroductionPlaying = false;
            // Battle는 참가자를 만든 Controller의 Start가 배정합니다. 서비스 Start 순서에 따라 덮어쓰지 않습니다.
            if (scene.name != "Battle")
            {
                AudioClip next = catalog != null ? catalog.Find(scene.name) : null;
                if (battleMusicActive)
                {
                    battleMusicActive = false;
                    Transition(next, .75f, .9f);
                }
                else Play(next);
            }
            // Intro는 자신의 Camera/Listener를 Start에서 생성합니다. 먼저 Listener를 만들면 두 출력이 겹칩니다.
            if (scene.name == "OpeningIntro") { sceneListener = null; return; }
            EnsureListener(scene);
        }

        private void LateUpdate()
        {
            // sceneLoaded 시점에는 이전 Scene의 Listener가 아직 검색될 수 있습니다. 제거된 뒤에도 출력이 끊기지 않게 보충합니다.
            if (source != null && source.clip != null && sceneListener == null)
                EnsureListener(SceneManager.GetActiveScene());
        }

        private void EnsureListener(Scene scene)
        {
            // 기존 Scene에 Listener가 없으면 해당 Scene의 카메라에만 보충합니다. 지속 Listener를 만들지 않아 Intro와 겹치지 않습니다.
            sceneListener = FindAnyObjectByType<AudioListener>();
            if (sceneListener != null) return;
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                Camera camera = root.GetComponentInChildren<Camera>();
                if (camera != null) { sceneListener = camera.gameObject.AddComponent<AudioListener>(); return; }
            }
            // 타이틀처럼 카메라 없는 Overlay UI Scene에서도 Scene 수명에 속하는 출력 Listener 하나만 만듭니다.
            var listenerObject = new GameObject("SceneMusicListener", typeof(AudioListener));
            SceneManager.MoveGameObjectToScene(listenerObject, scene);
            sceneListener = listenerObject.GetComponent<AudioListener>();
        }

        /// <summary>이미 진행 중인 같은 곡은 재시작하지 않습니다. null은 음악 미정 공간으로 진입했을 때의 정리입니다.</summary>
        public void Play(AudioClip clip)
        {
            if (source == null || source.clip == clip && (clip == null || source.isPlaying)) return;
            CancelTransition();
            source.volume = 1f;
            source.Stop();
            source.loop = true;
            source.clip = clip;
            if (clip != null) source.Play();
        }

        // 한 Source의 음량만 순차 변경합니다. Mixer의 사용자 음량과 독립적이며 두 곡이 겹치지 않습니다.
        private void CancelTransition()
        {
            if (transition != null) StopCoroutine(transition);
            transition = null;
        }
        private void Transition(AudioClip clip, float fadeOut, float fadeIn)
        {
            CancelTransition();
            transition = StartCoroutine(Fade(clip, fadeOut, fadeIn));
        }
        private System.Collections.IEnumerator Fade(AudioClip clip, float fadeOut, float fadeIn)
        {
            float initial = source.volume;
            for (float elapsed = 0; elapsed < fadeOut; elapsed += Time.unscaledDeltaTime)
            {
                source.volume = initial * (1f - elapsed / fadeOut);
                yield return null;
            }
            source.Stop(); source.clip = clip; source.loop = true; source.volume = 0f;
            if (clip != null) source.Play();
            for (float elapsed = 0; elapsed < fadeIn; elapsed += Time.unscaledDeltaTime)
            {
                source.volume = elapsed / fadeIn;
                yield return null;
            }
            source.volume = 1f; transition = null;
        }

        /// <summary>Field03의 기존 안전 반경 판정만 음악에 반영하며, 거점/퀘스트 저장 조건은 바꾸지 않습니다.</summary>
        public void SetSafeZoneMusic(bool inside)
        {
            if (SceneManager.GetActiveScene().name != "Field_03") return;
            Play(catalog != null ? inside ? catalog.SafeZoneClip : catalog.Find("Field_03") : null);
        }

        private void OnDisable() { CancelTransition(); battleMusicActive = false; if (source != null) { source.Stop(); source.clip = null; } }
        /// <summary>실제 참가자가 준비된 Battle Start에서 호출합니다. 기존 Source/Mixer와 복귀 Scene 정책을 재사용합니다.</summary>
        public void PlayBattleMusic(bool isBoss)
        {
            if (SceneManager.GetActiveScene().name != "Battle") return;
            battleMusicActive = true;
            Transition(catalog != null ? catalog.FindBattle(
                ProjectLimitless.Battle.BattleEncounterContext.FieldSceneName ?? string.Empty,
                ProjectLimitless.Battle.BattleEncounterContext.StableEncounterId, isBoss) : null, 0f, .20f);
        }

        /// <summary>미래의 첫 서부 소개 시작 Hook입니다. 임의 타이머나 Save 플래그를 추가하지 않습니다.</summary>
        public bool BeginWesternIntroduction()
        {
            if (SceneManager.GetActiveScene().name != "Field_04_WesternBorder" || catalog == null ||
                catalog.WesternIntroductionClip == null) return false;
            westernIntroductionPlaying = true;
            Play(catalog.WesternIntroductionClip);
            source.loop = false;
            return true;
        }

        /// <summary>소개 종료·취소 양쪽에서 호출해 현재 Scene의 기본 탐색곡으로 복구합니다.</summary>
        public void EndWesternIntroduction()
        {
            if (!westernIntroductionPlaying) return;
            westernIntroductionPlaying = false;
            Play(catalog != null ? catalog.Find(SceneManager.GetActiveScene().name) : null);
        }
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            if (Instance == this) Instance = null;
        }
    }
}
