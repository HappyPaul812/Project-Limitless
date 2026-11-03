using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>#84 전용 독립 프로젝트에서만 AnimationClip/Animator를 생성합니다. 제품 코드나 저장기를 불러오지 않습니다.</summary>
[InitializeOnLoad]
public static class Issue84UnityAnimationDelta
{
    const string Key = "Issue84ClipDelta";
    static double deadline;
    static string Output => Path.GetFullPath(Path.Combine(Application.dataPath, "../../UnityEvidenceVerified"));
    static Issue84ClipProbe probe;
    static Issue84UnityAnimationDelta()
    {
        EditorApplication.playModeStateChanged += state => {
            if (state != PlayModeStateChange.EnteredPlayMode || !SessionState.GetBool(Key, false)) return;
            deadline = EditorApplication.timeSinceStartup + 180;
            EditorApplication.update += Poll;
        };
    }
    public static void Run()
    {
        if (!Application.isBatchMode || !Application.dataPath.Replace('\\', '/').EndsWith("/Temp/Issue84OwlSpiderUnityDelta20261103/QA/Client/Assets"))
            throw new InvalidOperationException("#84 독립 fixture만 허용합니다.");
        Directory.CreateDirectory(Output);
        Directory.CreateDirectory("Assets/GeneratedVerified");
        foreach (string rawPath in Directory.GetFiles("Assets/Input", "*.png", SearchOption.AllDirectories))
        {
            string path = rawPath.Replace('\\', '/');
            var importer = (TextureImporter)AssetImporter.GetAtPath(path);
            importer.textureType = TextureImporterType.Sprite; importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePixelsPerUnit = path.Contains("/Owl/") ? 512 : 256;
            importer.alphaIsTransparency = true; importer.mipmapEnabled = false; importer.isReadable = true;
            importer.filterMode = FilterMode.Point; importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.npotScale = TextureImporterNPOTScale.None;
            var settings = new TextureImporterSettings(); importer.ReadTextureSettings(settings);
            settings.spriteAlignment = (int)SpriteAlignment.Custom; settings.spritePivot = new Vector2(.5f, .5f);
            settings.spriteMeshType = SpriteMeshType.FullRect; importer.SetTextureSettings(settings); importer.SaveAndReimport();
        }
        Sprite[] owl = Enumerable.Range(0, 4).Select(i => SpriteAt($"Owl/Owl_{i:00}.png")).ToArray();
        Sprite[] spider = Enumerable.Range(0, 8).Select(i => SpriteAt($"Spider/Spider_{i:00}.png")).ToArray();
        var idle = SpriteAt("Owl/Idle00.png");
        var attackClip = Clip("Owl_Attack4_8FPS", owl, 8, false);
        var idleClip = Clip("Owl_Idle00_Return", new[] { idle }, 8, true);
        var walkClip = Clip("Spider_Walk8_10FPS", spider, 10, true);
        var owlController = AnimatorController.CreateAnimatorControllerAtPath("Assets/GeneratedVerified/Owl.controller");
        var sm = owlController.layers[0].stateMachine;
        var attackState = sm.AddState("Attack"); attackState.motion = attackClip; sm.defaultState = attackState;
        var idleState = sm.AddState("Idle"); idleState.motion = idleClip;
        var transition = attackState.AddTransition(idleState); transition.hasExitTime = true; transition.exitTime = 1;
        transition.duration = 0; transition.hasFixedDuration = true;
        var spiderController = AnimatorController.CreateAnimatorControllerAtPath("Assets/GeneratedVerified/Spider.controller");
        var walk = spiderController.layers[0].stateMachine.AddState("Walk"); walk.motion = walkClip;
        spiderController.layers[0].stateMachine.defaultState = walk;
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var camera = new GameObject("Real_Unity_Camera").AddComponent<Camera>();
        camera.transform.position = new Vector3(0, 0, -10); camera.orthographic = true;
        camera.clearFlags = CameraClearFlags.SolidColor; camera.backgroundColor = new Color(.18f, .23f, .23f);
        var owlView = Actor("OWL_ACTUAL_CLIP", owl[0], owlController, 1);
        var spiderView = Actor("SPIDER_ACTUAL_CLIP", spider[0], spiderController, .7f);
        // 배경은 검증용 지면 격자입니다. 발 Collider가 없으므로 접촉 검증과 구별합니다.
        for (int x = -12; x <= 12; x++) Line(new Vector3(x, -7), new Vector3(x, 7));
        for (int y = -7; y <= 7; y++) Line(new Vector3(-12, y), new Vector3(12, y));
        var caption = new GameObject("TEST_ONLY_Label").AddComponent<TextMesh>();
        caption.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); caption.fontSize = 32;
        caption.characterSize = .14f; caption.anchor = TextAnchor.UpperLeft; caption.color = Color.white;
        caption.GetComponent<MeshRenderer>().sharedMaterial = caption.font.material;
        caption.GetComponent<MeshRenderer>().sortingOrder = 50;
        probe = new GameObject("Issue84_PlayMode_Probe").AddComponent<Issue84ClipProbe>();
        probe.stageCamera = camera; probe.owl = owlView; probe.spider = spiderView; probe.caption = caption;
        probe.owlFrames = owl; probe.spiderFrames = spider; probe.idleFrame = idle;
        probe.attackClip = attackClip; probe.walkClip = walkClip;
        EditorSceneManager.SaveScene(scene, "Assets/GeneratedVerified/Issue84_Animation_TestStage.unity");
        AssetDatabase.SaveAssets();
        File.WriteAllText(Path.Combine(Output, "EditorSpec.json"), JsonUtility.ToJson(new Spec {
            editor = Application.unityVersion, owlFPS = attackClip.frameRate, owlLength = attackClip.length,
            spiderFPS = walkClip.frameRate, spiderLength = walkClip.length, spiderLoop = walkClip.isLooping,
            owlKeyCount = AnimationUtility.GetObjectReferenceCurve(attackClip, AnimationUtility.GetObjectReferenceCurveBindings(attackClip)[0]).Length,
            spiderKeyCount = AnimationUtility.GetObjectReferenceCurve(walkClip, AnimationUtility.GetObjectReferenceCurveBindings(walkClip)[0]).Length
        }, true));
        SessionState.SetBool(Key, true); EditorApplication.EnterPlaymode();
    }
    [Serializable] class Spec { public string editor; public float owlFPS, owlLength, spiderFPS, spiderLength; public bool spiderLoop; public int owlKeyCount, spiderKeyCount; }
    static Sprite SpriteAt(string path) => AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Input/" + path);
    static AnimationClip Clip(string name, Sprite[] frames, float fps, bool loop)
    {
        var clip = new AnimationClip { name = name, frameRate = fps };
        var binding = new EditorCurveBinding { path = "", type = typeof(SpriteRenderer), propertyName = "m_Sprite" };
        var keys = new ObjectReferenceKeyframe[frames.Length];
        for (int i = 0; i < frames.Length; i++) keys[i] = new ObjectReferenceKeyframe { time = i / fps, value = frames[i] };
        // Unity가 마지막 Sprite 키의 한 프레임 유지 시간을 포함하므로 중복 끝 키를 넣지 않습니다.
        AnimationUtility.SetObjectReferenceCurve(clip, binding, keys);
        var settings = AnimationUtility.GetAnimationClipSettings(clip); settings.loopTime = loop;
        AnimationUtility.SetAnimationClipSettings(clip, settings);
        AssetDatabase.CreateAsset(clip, "Assets/GeneratedVerified/" + name + ".anim");
        return clip;
    }
    static SpriteRenderer Actor(string name, Sprite sprite, RuntimeAnimatorController controller, float scale)
    {
        var g = new GameObject(name, typeof(SpriteRenderer), typeof(Animator)); g.layer = 8;
        g.transform.localScale = Vector3.one * scale;
        var view = g.GetComponent<SpriteRenderer>(); view.sprite = sprite; view.sortingOrder = 2; view.flipX = false;
        var animator = g.GetComponent<Animator>(); animator.runtimeAnimatorController = controller;
        animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        return view;
    }
    static void Line(Vector3 a, Vector3 b)
    {
        var line = new GameObject("TEST_GROUND_GRID_1WU").AddComponent<LineRenderer>();
        line.positionCount = 2; line.SetPositions(new[] { a, b }); line.startWidth = line.endWidth = .018f;
        line.sharedMaterial = new Material(Shader.Find("Sprites/Default"));
        line.startColor = line.endColor = new Color(.34f, .42f, .38f); line.sortingOrder = -10;
    }
    static void Poll()
    {
        probe = UnityEngine.Object.FindAnyObjectByType<Issue84ClipProbe>();
        if (probe != null && probe.finished)
        {
            SessionState.SetBool(Key, false); EditorApplication.update -= Poll;
            EditorApplication.Exit(string.IsNullOrEmpty(probe.failure) ? 0 : 1);
        }
        else if (EditorApplication.timeSinceStartup > deadline)
        {
            File.WriteAllText(Path.Combine(Output, "failure.txt"), "PlayMode timeout"); EditorApplication.Exit(2);
        }
    }
}
