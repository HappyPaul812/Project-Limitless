using System;
using System.Collections.Generic;
using UnityEngine;
namespace ProjectLimitless.World
{
    /// <summary>기존 환경의 좌표·충돌체를 보존하고 표시만 교체합니다. 반복 타일은 크기를 늘리지 않고 Tiled로 그립니다.</summary>
    [DisallowMultipleComponent]
    public sealed class DungeonEnvironmentRenderer : MonoBehaviour
    {
        [SerializeField] private DungeonEnvironmentDefinition definition;
        private readonly List<SpriteRenderer> originals = new List<SpriteRenderer>();
        private readonly List<bool> originalEnabled = new List<bool>();
        private GameObject visuals;
        public bool Initialized { get; private set; }
        public int GeneratedRendererCount => visuals == null ? 0 : visuals.GetComponentsInChildren<SpriteRenderer>(true).Length;
        private void OnEnable()
        {
            if (!Initialized) Build();
            if (!Initialized) return;
            visuals.SetActive(true);
            foreach (var renderer in originals) if (renderer != null) renderer.enabled = false;
        }
        private void OnDisable()
        {
            // 다시 활성화할 때 같은 렌더러를 재사용합니다. Save/Continue의 Scene 재생성은 새 인스턴스를 만들므로 중복되지 않습니다.
            if (visuals != null) visuals.SetActive(false);
            for (int i = 0; i < originals.Count; i++) if (originals[i] != null) originals[i].enabled = originalEnabled[i];
        }
        private void Build()
        {
            if (definition == null || definition.sceneName != gameObject.scene.name || definition.placements == null) return;
            var anchors = GetComponentsInChildren<SpriteRenderer>(true);
            var resolved = new List<SpriteRenderer>();
            var used = new HashSet<SpriteRenderer>();
            // 모든 대응을 먼저 검사합니다. 불완전한 데이터로 기존 환경을 숨겨 검은 맵이 되는 일을 막습니다.
            foreach (var entry in definition.placements)
            {
                SpriteRenderer match = null;
                foreach (var anchor in anchors)
                    if (anchor.name == entry.anchorName && Vector3.Distance(anchor.transform.localPosition, entry.localPosition) < .001f)
                    { if (match != null) { Debug.LogWarning("환경 이름·좌표가 중복됩니다.", this); return; } match = anchor; }
                bool needsSprite = entry.mode == DungeonEnvironmentDefinition.RenderMode.Tiled || entry.mode == DungeonEnvironmentDefinition.RenderMode.NativeSprite;
                if (match == null || !used.Add(match) || (needsSprite && entry.sprite == null)) { Debug.LogWarning("환경 아트 대응이 불완전하여 기존 표시를 유지합니다.", this); return; }
                resolved.Add(match);
            }
            if (resolved.Count != anchors.Length) { Debug.LogWarning("환경 오브젝트 수가 달라 기존 표시를 유지합니다.", this); return; }
            visuals = new GameObject("__DungeonRenewalVisuals");
            visuals.transform.SetParent(transform, false);
            for (int i = 0; i < resolved.Count; i++)
            {
                var entry = definition.placements[i]; var anchor = resolved[i];
                if (entry.mode == DungeonEnvironmentDefinition.RenderMode.RetainLegacy) continue;
                originals.Add(anchor); originalEnabled.Add(anchor.enabled);
                if (entry.mode == DungeonEnvironmentDefinition.RenderMode.HideDetail) continue;
                var child = new GameObject("Renewal_" + i + "_" + anchor.name, typeof(SpriteRenderer));
                child.layer = anchor.gameObject.layer; child.transform.SetParent(visuals.transform, false);
                child.transform.position = anchor.transform.position + (Vector3)entry.visualOffset;
                child.transform.rotation = anchor.transform.rotation;
                var scale = visuals.transform.lossyScale;
                child.transform.localScale = new Vector3(1 / scale.x, 1 / scale.y, 1 / scale.z);
                var renderer = child.GetComponent<SpriteRenderer>(); renderer.sprite = entry.sprite;
                renderer.sortingLayerID = anchor.sortingLayerID; renderer.sortingOrder = anchor.sortingOrder;
                renderer.sharedMaterial = anchor.sharedMaterial; renderer.flipX = anchor.flipX; renderer.flipY = anchor.flipY; renderer.color = entry.color;
                if (entry.mode == DungeonEnvironmentDefinition.RenderMode.Tiled) { renderer.drawMode = SpriteDrawMode.Tiled; renderer.size = entry.worldSize; }
                // 소품은 PPU의 원래 크기를 사용합니다. 투명 여백 때문에 작아지는 비율 맞춤은 하지 않으며 새 Collider도 만들지 않습니다.
            }
            Initialized = true;
        }
    }
}
