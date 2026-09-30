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
            Play(catalog != null ? catalog.Find(scene.name) : null);
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
            source.Stop();
            source.clip = clip;
            if (clip != null) source.Play();
        }

        /// <summary>Field03의 기존 안전 반경 판정만 음악에 반영하며, 거점/퀘스트 저장 조건은 바꾸지 않습니다.</summary>
        public void SetSafeZoneMusic(bool inside)
        {
            if (SceneManager.GetActiveScene().name != "Field_03") return;
            Play(catalog != null ? inside ? catalog.SafeZoneClip : catalog.Find("Field_03") : null);
        }

        private void OnDisable() { if (source != null) { source.Stop(); source.clip = null; } }
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            if (Instance == this) Instance = null;
        }
    }
}
