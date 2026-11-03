using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

/// <summary>제품 시스템 없이 실제 PlayMode Animator를 관찰하고 카메라를 캡처하는 #84 전용 계측기입니다.</summary>
[DefaultExecutionOrder(10000)]
public sealed class Issue84ClipProbe : MonoBehaviour
{
    public Camera stageCamera;
    public SpriteRenderer owl, spider;
    public TextMesh caption;
    public Sprite[] owlFrames, spiderFrames;
    public Sprite idleFrame;
    public AnimationClip attackClip, walkClip;
    public bool finished;
    public string failure;
    const float Dt = 1f / 80;
    readonly float[] speeds = { 0, .65f, 1.3f };
    readonly List<string> checks = new List<string>(), errors = new List<string>();
    readonly HashSet<string> nativeChecked = new HashSet<string>();
    readonly List<Row> rows = new List<Row>();
    readonly List<int> sequence = new List<int>();
    Animator owlAnimator, spiderAnimator;
    Camera spriteCamera;
    int phase, step, cycle, speedIndex, captureIndex, stableSamples;
    bool initialized;
    Sprite lastSprite;
    string Output => Path.GetFullPath(Path.Combine(Application.dataPath, "../../UnityEvidenceVerified"));
    [Serializable] public class Row
    {
        public int unityFrame, step, sourceFrame, expectedFrame;
        public string actor, state, sprite;
        public float normalizedTime, simulationTime, rootX, rootY, speedWUPerSecond;
    }
    [Serializable] class Trace { public Row[] samples; }
    void Start()
    {
        try
        {
            if (!Application.isBatchMode || !Application.dataPath.Replace('\\', '/').EndsWith("/Temp/Issue84OwlSpiderUnityDelta20261103/QA/Client/Assets"))
                throw new InvalidOperationException("#84 fixture PlayMode 전용입니다.");
            Directory.CreateDirectory(Output); Directory.CreateDirectory(Path.Combine(Output, "OwlStage"));
            foreach (int i in new[] { 0, 1, 2 }) Directory.CreateDirectory(Path.Combine(Output, "SpiderStage" + i));
            Directory.CreateDirectory(Path.Combine(Output, "Native40")); Directory.CreateDirectory(Path.Combine(Output, "Native128"));
            Application.runInBackground = true; Application.targetFrameRate = 120; QualitySettings.vSyncCount = 0;
            Time.captureDeltaTime = Dt; Time.timeScale = 1; AudioListener.volume = 0;
            Application.logMessageReceived += Log;
            Check(Application.isPlaying && Application.unityVersion == "6000.6.5f1", "Runtime.RealPlayMode6000_6_5");
            Check(attackClip.frameRate == 8 && Mathf.Abs(attackClip.length - .5f) < .001f, "Owl.RealClip8FPS_Length0_5");
            Check(walkClip.frameRate == 10 && Mathf.Abs(walkClip.length - .8f) < .001f && walkClip.isLooping, "Spider.RealClip10FPS_Loop0_8");
            Check(Array.TrueForAll(owlFrames, s => s.pixelsPerUnit == 512 && s.pivot == new Vector2(256, 256)), "Owl.ImportPPU512CentralPivot");
            Check(Array.TrueForAll(spiderFrames, s => s.pixelsPerUnit == 256 && s.pivot == new Vector2(256, 256)), "Spider.ImportPPU256CentralPivot");
            Check(owl.transform.localScale == Vector3.one && spider.transform.localScale == Vector3.one * .7f, "Actor.Scale1_And0_7");
            Check(!spider.flipX && spider.transform.lossyScale.x > 0, "Spider.FacingRightContract_NoFlip_PositiveX");
            Check(ImportedEdgesClear(owlFrames) && ImportedEdgesClear(spiderFrames), "ImportedSprites.All12CanvasEdgesAlphaZero");
            owlAnimator = owl.GetComponent<Animator>(); spiderAnimator = spider.GetComponent<Animator>();
            Check(owlAnimator.runtimeAnimatorController != null && spiderAnimator.runtimeAnimatorController != null, "Runtime.RealAnimatorControllers");
            spriteCamera = new GameObject("NativeSize_ActualCamera").AddComponent<Camera>(); spriteCamera.enabled = false;
            spriteCamera.orthographic = true; spriteCamera.clearFlags = CameraClearFlags.SolidColor;
            spriteCamera.backgroundColor = Color.clear; spriteCamera.cullingMask = 1 << 8;
            BeginOwl(); initialized = true;
        }
        catch (Exception e) { Fail(e); }
    }
    void Log(string text, string stack, LogType type)
    {
        if (type != LogType.Error && type != LogType.Exception && type != LogType.Assert) return;
        // 이미 알려진 Editor 검색 인덱스 예외만 별도 보존합니다. 그 밖의 오류는 계속 실패시킵니다.
        if (text.Contains("ArgumentOutOfRangeException") && stack.Contains("UnityEditor.Search.SearchDatabase"))
            File.AppendAllText(Path.Combine(Output, "editor_environment_errors.txt"), text + "\n" + stack + "\n");
        else errors.Add(text + "\n" + stack);
    }
    void BeginOwl()
    {
        owl.gameObject.SetActive(true); spider.gameObject.SetActive(false); owl.transform.position = Vector3.zero;
        stageCamera.orthographicSize = 4.5f; caption.transform.position = new Vector3(-7.8f, 4.1f, 0);
        caption.text = "OWL REAL ANIMATIONCLIP | 8FPS | PPU512 | SCALE1\n40px canvas at 640x360 | TEST STAGE / NOT PRODUCT";
        ResetActor(owlAnimator, "Attack"); lastSprite = null; sequence.Clear(); step = 0;
    }
    void BeginSpider()
    {
        phase = 1; step = 0; captureIndex = 0; lastSprite = null; sequence.Clear();
        owl.gameObject.SetActive(false); spider.gameObject.SetActive(true); spider.transform.position = new Vector3(-1, 0, 0);
        stageCamera.orthographicSize = 6.3f; caption.transform.position = new Vector3(-11, 5.8f, 0);
        caption.text = "SPIDER REAL ANIMATIONCLIP | 10FPS | PPU256 | SCALE0.7\n+X speed " + speeds[speedIndex].ToString("F2", CultureInfo.InvariantCulture)
            + " WU/s | 1WU grid | FOOT CONTACT NOT VERIFIED";
        ResetActor(spiderAnimator, "Walk");
    }
    static void ResetActor(Animator animator, string state)
    {
        animator.Rebind(); animator.Play(state, 0, 0); animator.Update(0);
    }
    // Animator 평가가 끝난 LateUpdate에서 실제 Sprite를 읽습니다. PNG를 시간표로 직접 바꾸는 재생기가 아닙니다.
    void LateUpdate()
    {
        if (!initialized || finished) return;
        try
        {
            var view = phase == 0 ? owl : spider;
            var animator = phase == 0 ? owlAnimator : spiderAnimator;
            if (phase == 1) spider.transform.position = new Vector3(-1 + speeds[speedIndex] * step * Dt, 0, 0);
            var info = animator.GetCurrentAnimatorStateInfo(0);
            bool idle = phase == 0 && info.IsName("Idle");
            int index = idle ? 4 : Array.IndexOf(phase == 0 ? owlFrames : spiderFrames, view.sprite);
            if (index < 0) throw new Exception("Animator가 입력 목록 밖 Sprite를 재생했습니다: " + view.sprite);
            if (sequence.Count == 0 || sequence[sequence.Count - 1] != index) sequence.Add(index);
            int expected = idle ? 4 : Mathf.Clamp(Mathf.FloorToInt((phase == 0 ? info.normalizedTime : Mathf.Repeat(info.normalizedTime, 1)) * (phase == 0 ? 4 : 8)), 0, phase == 0 ? 3 : 7);
            float fraction = Mathf.Repeat(info.normalizedTime * (phase == 0 ? 4 : 8), 1);
            // 부동소수점 경계 한 샘플에 결과를 의존시키지 않고 프레임 중간 유지 구간을 비교합니다.
            if (fraction > .12f && fraction < .88f && !idle)
            {
                if (expected != index) throw new Exception("Clip 시간/실제 Sprite 불일치 " + expected + "/" + index);
                stableSamples++;
            }
            rows.Add(new Row { unityFrame = Time.frameCount, step = step, actor = phase == 0 ? "Owl" : "Spider",
                state = idle ? "Idle" : phase == 0 ? "Attack" : "Walk", sprite = view.sprite.name,
                sourceFrame = index, expectedFrame = expected, normalizedTime = info.normalizedTime,
                simulationTime = step * Dt, rootX = view.transform.position.x, rootY = view.transform.position.y,
                speedWUPerSecond = phase == 0 ? 0 : speeds[speedIndex] });
            string tag = phase == 0 ? "Owl_Cycle" + cycle : "Spider_Speed" + speedIndex;
            if (view.sprite != lastSprite)
            {
                NativeCapture(view, 40, tag + "_Step" + step.ToString("000") + "_" + view.sprite.name);
                NativeCapture(view, 128, tag + "_Step" + step.ToString("000") + "_" + view.sprite.name);
                lastSprite = view.sprite;
            }
            if (step % 4 == 0)
            {
                string folder = phase == 0 ? "OwlStage" : "SpiderStage" + speedIndex;
                Capture(stageCamera, 640, 360, Path.Combine(Output, folder, (captureIndex++).ToString("0000") + ".png"));
            }
            if (phase == 0 && ++step >= 60)
            {
                Check(string.Join(",", sequence) == "0,1,2,3,4", "Owl.RuntimeOrder_AttackToIdle_Cycle" + cycle);
                Check(idle && view.sprite == idleFrame, "Owl.ActualAnimatorTransitionToIdle" + cycle);
                if (cycle++ == 0) BeginOwl();
                else { SortingProbe(); BeginSpider(); }
            }
            else if (phase == 1)
            {
                if (step == 64) Check(Mathf.Abs(spider.transform.position.x - (-1 + speeds[speedIndex] * .8f)) < .001f, "Spider.LoopTravelDistance_Speed" + speedIndex);
                if (++step >= 128)
                {
                    Check(sequence.Count >= 16 && sequence.GetRange(0, 16).TrueForAll(i => i >= 0 && i < 8), "Spider.TwoActualLoops_Speed" + speedIndex);
                    Check(Array.TrueForAll(new[] { 0, 1, 2, 3, 4, 5, 6, 7 }, i => sequence.Contains(i)), "Spider.All8FramesRendered_Speed" + speedIndex);
                    if (++speedIndex < speeds.Length) BeginSpider(); else Complete();
                }
            }
        }
        catch (Exception e) { Fail(e); }
    }
    bool ImportedEdgesClear(Sprite[] sprites)
    {
        foreach (var sprite in sprites)
        {
            var p = sprite.texture.GetPixels32(); int w = sprite.texture.width, h = sprite.texture.height;
            for (int x = 0; x < w; x++) if (p[x].a != 0 || p[(h - 1) * w + x].a != 0) return false;
            for (int y = 0; y < h; y++) if (p[y * w].a != 0 || p[y * w + w - 1].a != 0) return false;
        }
        return true;
    }
    void NativeCapture(SpriteRenderer actor, int size, string name)
    {
        float canvasWU = actor.sprite.rect.height / actor.sprite.pixelsPerUnit * actor.transform.lossyScale.y;
        spriteCamera.transform.position = actor.transform.position + new Vector3(0, 0, -10);
        // 40/128px 셀 바깥에 2px 캡처 여백을 남겨 영상 경계 자체가 발끝을 자르지 않게 합니다.
        spriteCamera.orthographicSize = canvasWU * .5f * (size + 4f) / size;
        var pixels = Capture(spriteCamera, size + 4, size + 4, Path.Combine(Output, "Native" + size, name + ".png"));
        if (nativeChecked.Add(actor.name + size))
        {
            var a = spriteCamera.WorldToViewportPoint(actor.bounds.min); var b = spriteCamera.WorldToViewportPoint(actor.bounds.max);
            Check(Mathf.Abs((b.y - a.y) * (size + 4) - size) < .01f && Mathf.Abs((b.x - a.x) * (size + 4) - size) < .01f,
                actor.name + ".ActualProjectedCanvasPixels." + size);
            Check(Array.Exists(pixels, c => c.a > 0), actor.name + ".NativeCaptureNotBlank." + size);
            bool clear = true; int width = size + 4;
            for (int i = 0; i < width; i++) clear &= pixels[i].a == 0 && pixels[(width - 1) * width + i].a == 0
                && pixels[i * width].a == 0 && pixels[i * width + width - 1].a == 0;
            Check(clear, actor.name + ".ActualCaptureOuterBorderClear." + size);
        }
    }
    void SortingProbe()
    {
        var pixel = new Texture2D(1, 1); pixel.SetPixel(0, 0, Color.white); pixel.Apply();
        var proxy = new GameObject("SORTING_TEST_ONLY_PROXY").AddComponent<SpriteRenderer>(); proxy.gameObject.layer = 8;
        proxy.sprite = Sprite.Create(pixel, new Rect(0, 0, 1, 1), new Vector2(.5f, .5f), 1);
        proxy.transform.localScale = Vector3.one * .4f; proxy.color = Color.magenta;
        proxy.sortingOrder = 1; var behind = Capture(stageCamera, 640, 360, Path.Combine(Output, "OwlSorting_Behind.png"));
        proxy.sortingOrder = 3; var front = Capture(stageCamera, 640, 360, Path.Combine(Output, "OwlSorting_Front.png"));
        int different = 0; for (int i = 0; i < front.Length; i++) if (!front[i].Equals(behind[i])) different++;
        Check(different > 5, "Owl.ActualSortingChangesRenderedPixels"); proxy.gameObject.SetActive(false);
    }
    static Color32[] Capture(Camera camera, int width, int height, string path)
    {
        var rt = RenderTexture.GetTemporary(width, height, 24); var old = RenderTexture.active;
        camera.targetTexture = rt; camera.aspect = (float)width / height; camera.Render(); RenderTexture.active = rt;
        var image = new Texture2D(width, height, TextureFormat.RGBA32, false);
        image.ReadPixels(new Rect(0, 0, width, height), 0, 0); image.Apply(); var pixels = image.GetPixels32();
        File.WriteAllBytes(path, image.EncodeToPNG()); Destroy(image); camera.targetTexture = null;
        RenderTexture.active = old; RenderTexture.ReleaseTemporary(rt); return pixels;
    }
    void Check(bool ok, string label)
    {
        checks.Add((ok ? "PASS|" : "FAIL|") + label); File.WriteAllLines(Path.Combine(Output, "checks.txt"), checks);
        if (!ok) throw new Exception(label);
    }
    void Complete()
    {
        Check(stableSamples > 100, "Runtime.StableFrameTimeMatchesSpriteOver100Samples");
        Check(errors.Count == 0, "Runtime.NoNewErrors");
        File.WriteAllText(Path.Combine(Output, "Trace.json"), JsonUtility.ToJson(new Trace { samples = rows.ToArray() }, true));
        File.WriteAllText(Path.Combine(Output, "status.txt"), "OWL_CLIP_DELTA_PASS / SPIDER_WALK_CLIP_DELTA_PASS / ACTUAL_GROUND_LOCK_NOT_VERIFIED");
        finished = true; Time.captureDeltaTime = 0;
    }
    void Fail(Exception e)
    {
        failure = e.ToString(); Directory.CreateDirectory(Output);
        File.WriteAllText(Path.Combine(Output, "failure.txt"), failure); File.WriteAllLines(Path.Combine(Output, "runtime_errors.txt"), errors);
        File.WriteAllText(Path.Combine(Output, "Trace.json"), JsonUtility.ToJson(new Trace { samples = rows.ToArray() }, true));
        File.WriteAllText(Path.Combine(Output, "status.txt"), "FAIL"); finished = true;
    }
}
