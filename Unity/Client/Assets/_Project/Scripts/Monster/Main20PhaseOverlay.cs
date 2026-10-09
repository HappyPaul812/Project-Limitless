using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.Monster
{
    /// <summary>기본 거신 애니메이터가 실제 선택한 프레임 번호를 그대로 따라갑니다. 독립 타이머/Animator는 없습니다.</summary>
    public sealed class Main20PhaseOverlay : MonoBehaviour
    {
        private MonsterSpriteSheetAnimation source;
        private Image baseImage, overlayImage;
        private SpriteRenderer baseRenderer, overlayRenderer;
        private Sprite[] frames;
        public bool PhaseTwo { get; set; }
        public int FrameIndex { get; private set; } = -1;
        public void Configure(MonsterSpriteSheetAnimation animation, Image image)
        {
            source = animation; baseImage = image; Load();
            var child = new GameObject("Phase2Overlay", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            child.transform.SetParent(image.transform, false); overlayImage = child.GetComponent<Image>();
            overlayImage.raycastTarget = false; overlayImage.preserveAspect = image.preserveAspect;
            var rect = child.GetComponent<RectTransform>(); rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
        }
        public void Configure(MonsterSpriteSheetAnimation animation, SpriteRenderer renderer)
        {
            source = animation; baseRenderer = renderer; Load();
            var child = new GameObject("Phase2Overlay", typeof(SpriteRenderer));child.transform.SetParent(renderer.transform,false);
            overlayRenderer = child.GetComponent<SpriteRenderer>();
        }
        private void Load()
        {
            frames = Resources.LoadAll<Sprite>("Main20/Boss/Veinfire_Colossus_Phase2_Overlay");
            Array.Sort(frames, (a,b) => string.CompareOrdinal(a.name,b.name));
        }
        private void LateUpdate()
        {
            Sprite current = source == null ? null : source.CurrentFrame;
            FrameIndex = current == null ? -1 : ParseIndex(current.name);
            bool visible = PhaseTwo && FrameIndex >= 0 && FrameIndex < frames.Length;
            if (overlayImage != null) { overlayImage.enabled = visible; overlayImage.color=baseImage.color; if(visible)overlayImage.sprite=frames[FrameIndex]; }
            if (overlayRenderer != null)
            {
                overlayRenderer.enabled = visible;
                if(visible)overlayRenderer.sprite=frames[FrameIndex];
                overlayRenderer.sortingLayerID=baseRenderer.sortingLayerID;
                overlayRenderer.sortingOrder=baseRenderer.sortingOrder+1;
                overlayRenderer.flipX=baseRenderer.flipX;overlayRenderer.flipY=baseRenderer.flipY;
                overlayRenderer.color=baseRenderer.color;
            }
        }
        private static int ParseIndex(string name)
        { int index; return int.TryParse(name.Substring(name.LastIndexOf('_')+1),out index)?index:-1; }
    }
}
