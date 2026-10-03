using System;
using System.IO;
using System.Linq;
using ProjectLimitless.Player;
using UnityEditor;
using UnityEngine;

namespace ProjectLimitless.EditorTools
{
    /// <summary>검수 Inventory의 합격 여부와 방향 Clip만 Catalog에 반영합니다. PNG·기존 Animator·Scene은 건드리지 않습니다.</summary>
    public static class PlayerAppearanceSelectionBuild
    {
        public static string Build()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling)
                throw new InvalidOperationException("컴파일 완료 Edit Mode에서 실행하세요.");
            const string path = "Assets/_Project/Resources/PlayerAppearances/External50.asset";
            var catalog = AssetDatabase.LoadAssetAtPath<PlayerAppearanceCatalog>(path);
            var inventory = JsonUtility.FromJson<PlayerAppearanceCatalog.Inventory>(File.ReadAllText(
                "Assets/_Project/Resources/PlayerAppearances/Validated50.json"));
            var existingClips = AssetDatabase.LoadAllAssetsAtPath(path).OfType<AnimationClip>().ToArray();
            string[] directions = { "Down", "Left", "Right", "Up" };
            foreach (var entry in catalog.Entries)
            {
                var review = inventory.entries.Single(e => e.appearanceId == entry.appearanceId);
                entry.readyForSelection = review.readyForSelection;
                entry.reviewReason = review.reviewReason;
                if (!entry.readyForSelection)
                {
                    // 이 도구가 만든 Catalog subasset만 정리합니다. 기존 Player Clip Asset은 수정하지 않습니다.
                    foreach (var clip in existingClips.Where(c => c != null && c.name.StartsWith(entry.appearanceId + "_", StringComparison.Ordinal)))
                        UnityEngine.Object.DestroyImmediate(clip, true);
                    entry.clips = Array.Empty<AnimationClip>(); continue;
                }
                if (entry.frames == null || entry.frames.Length != 16 || entry.frames.Any(s => s == null))
                    throw new InvalidOperationException("16 Sprite 누락: " + entry.appearanceId);
                entry.clips = new AnimationClip[8];
                for (int direction = 0; direction < 4; direction++)
                    for (int motion = 0; motion < 2; motion++)
                    {
                        string name = entry.appearanceId + "_" + (motion == 0 ? "Idle_" : "Walk_") + directions[direction];
                        var clip = existingClips.FirstOrDefault(c => c.name == name);
                        if (clip == null) { clip = new AnimationClip { name = name }; AssetDatabase.AddObjectToAsset(clip, catalog); }
                        clip.frameRate = 6;
                        // 행 첫 프레임은 Idle이며 Walk4장을 6fps로 순환합니다. 끝 키는 첫 장으로 연결합니다.
                        var keys = new ObjectReferenceKeyframe[motion == 0 ? 2 : 5];
                        for (int frame = 0; frame < keys.Length; frame++)
                            keys[frame] = new ObjectReferenceKeyframe { time = frame / 6f,
                                value = entry.frames[direction * 4 + (motion == 0 ? 0 : frame % 4)] };
                        AnimationUtility.SetObjectReferenceCurve(clip,
                            EditorCurveBinding.PPtrCurve("", typeof(SpriteRenderer), "m_Sprite"), keys);
                        var settings = AnimationUtility.GetAnimationClipSettings(clip);
                        settings.loopTime = true;
                        AnimationUtility.SetAnimationClipSettings(clip, settings);
                        EditorUtility.SetDirty(clip);
                        entry.clips[direction * 2 + motion] = clip;
                    }
            }
            EditorUtility.SetDirty(catalog);
            AssetDatabase.SaveAssetIfDirty(catalog);
            return "Ready " + catalog.Entries.Count(e => e.IsUsable) + " / blocked " + catalog.Entries.Count(e => !e.readyForSelection);
        }
    }
}
