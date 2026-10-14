using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using ProjectLimitless.Player;
using ProjectLimitless.NPC;

namespace ProjectLimitless.World
{
    /// <summary>아르벨 두 건물의 발 기준 깊이와 국소 지붕 가림만 담당합니다. 이동·상호작용·저장은 변경하지 않습니다.</summary>
    public sealed class ArbelBuildingDepth : MonoBehaviour
    {
        [Serializable] public sealed class Building
        {
            public SpriteRenderer roof, wallLeft, wallRight, door;
            public SortingGroup group;
            public float footY;
            // 승인된 처마·접합 범위입니다. 지붕 위치나 원본 PNG 대신 셰이더 표시만 보완합니다.
            public float protectedY = 4.09375f;
            public Vector4 seam;
        }
        [SerializeField] private Building[] buildings;
        [SerializeField] private Shader roofShader;
        private sealed class Actor
        {
            public SpriteRenderer view;
            public Transform foot;
            public SortingGroup group;
            public int previousOrder, previousGroupOrder;
            public SpriteSortPoint previousPoint;
            public bool addedGroup;
        }
        private readonly List<Actor> actors = new List<Actor>();
        private Material[] materials, originalMaterials;
        private float nextActorScan;

        public Building[] Buildings => buildings;
        public void Configure(Building[] value, Shader shader) { buildings = value; roofShader = shader; }

        private void OnEnable()
        {
            if (!Application.isPlaying) return;
            if (buildings == null || roofShader == null) { enabled = false; return; }
            materials = new Material[buildings.Length]; originalMaterials = new Material[buildings.Length];
            for (int n = 0; n < buildings.Length; n++)
            {
                var b = buildings[n]; originalMaterials[n] = b.roof.sharedMaterial;
                materials[n] = new Material(roofShader) { name = "Arbel Local Roof (Runtime)" };
                materials[n].SetFloat("_ProtectedY", b.protectedY);
                materials[n].SetVector("_Badge", new Vector4(b.roof.transform.position.x, 4.125f, .24f, 0));
                materials[n].SetVector("_Seam", b.seam); materials[n].SetFloat("_JoinEnabled", 1);
                b.roof.sharedMaterial = materials[n];
            }
            ScanActors();
        }

        // 외형 교체와 애니메이션의 현재 Sprite를 매 프레임 사용합니다. 플레이어 생성·교체만 저빈도로 재검색합니다.
        private void ScanActors()
        {
            foreach (var view in FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None))
            {
                if (view.gameObject.scene != gameObject.scene || actors.Exists(a => a.view == view)) continue;
                var npc = view.GetComponentInParent<NpcController>(); var player = view.GetComponentInParent<PlayerController>();
                if (npc == null && player == null) continue;
                var group = view.GetComponent<SortingGroup>(); bool added = group == null;
                if (added) group = view.gameObject.AddComponent<SortingGroup>();
                actors.Add(new Actor { view = view, foot = player != null ? player.transform : npc.transform,
                    group = group, addedGroup = added, previousOrder = view.sortingOrder,
                    previousPoint = view.spriteSortPoint, previousGroupOrder = group.sortingOrder });
                group.sortingLayerID = view.sortingLayerID; view.sortingOrder = 0; view.spriteSortPoint = SpriteSortPoint.Pivot;
            }
            nextActorScan = Time.unscaledTime + .5f;
        }

        private static bool Behind(Actor actor, Building b) => actor.foot.position.y > b.footY + .01f &&
            actor.view.bounds.max.x > b.roof.bounds.min.x && actor.view.bounds.min.x < b.roof.bounds.max.x;

        private void LateUpdate()
        {
            if (Time.unscaledTime >= nextActorScan) ScanActors();
            foreach (var actor in actors)
            {
                if (actor.view == null || actor.group == null) continue;
                bool behind = false; foreach (var b in buildings) behind |= Behind(actor, b);
                // 기존 소품 순서를 유지하는 건물 주변 뒤1/앞5 비교입니다. 모든 월드에 적용하는 전역 Y-Sort가 아닙니다.
                actor.group.sortingOrder = behind ? 1 : 5;
            }
            for (int n = 0; n < buildings.Length; n++)
            {
                var b = buildings[n]; var mat = materials[n]; int slot = 0;
                foreach (var actor in actors)
                {
                    var view = actor.view;
                    if (view == null || !view.enabled || !view.gameObject.activeInHierarchy || view.sprite == null || !Behind(actor, b) || !view.bounds.Intersects(b.roof.bounds)) continue;
                    if (slot >= 4) break;
                    var sprite = view.sprite; string suffix = slot.ToString();
                    mat.SetTexture("_ActorTex" + suffix, sprite.texture);
                    mat.SetMatrix("_ActorWorldToLocal" + suffix, view.transform.worldToLocalMatrix);
                    mat.SetVector("_ActorUV" + suffix, new Vector4(sprite.rect.x / sprite.texture.width, sprite.rect.y / sprite.texture.height,
                        sprite.rect.width / sprite.texture.width, sprite.rect.height / sprite.texture.height));
                    mat.SetVector("_ActorPivotPPU" + suffix, new Vector4(sprite.pivot.x, sprite.pivot.y, sprite.pixelsPerUnit, 1));
                    mat.SetVector("_ActorSizeFlip" + suffix, new Vector4(sprite.rect.width, sprite.rect.height, view.flipX ? -1 : 1, view.flipY ? -1 : 1));
                    slot++;
                }
                // 이전 프레임의 NPC나 교체 전 외형이 남지 않도록 사용하지 않는 마스크 슬롯을 반드시 지웁니다.
                while (slot < 4) { mat.SetVector("_ActorPivotPPU" + slot, Vector4.zero); slot++; }
            }
        }

        private void OnDisable()
        {
            for (int n = 0; materials != null && n < materials.Length; n++)
            {
                if (buildings[n].roof != null) buildings[n].roof.sharedMaterial = originalMaterials[n];
                if (materials[n] != null) Destroy(materials[n]);
            }
            foreach (var actor in actors)
            {
                if (actor.view != null) { actor.view.sortingOrder = actor.previousOrder; actor.view.spriteSortPoint = actor.previousPoint; }
                if (actor.group != null) { if (actor.addedGroup) Destroy(actor.group); else actor.group.sortingOrder = actor.previousGroupOrder; }
            }
            actors.Clear();
        }
    }
}
