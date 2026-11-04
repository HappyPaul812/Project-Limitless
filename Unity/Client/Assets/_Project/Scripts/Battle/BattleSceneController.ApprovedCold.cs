using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ProjectLimitless.Battle
{
    /// <summary>승인된 냉기 아이콘과 정지 VFX만 찾습니다. 피해·턴·해제 규칙을 만들거나 변경하지 않습니다.</summary>
    public static class BattleApprovedColdAssets
    {
        static readonly Dictionary<string, Sprite> icons = new Dictionary<string, Sprite>();
        public static bool IsCold(string id) => id == "cold.chill" || id == "cold.frostbite" || id == "cold.necrosis";
        static string Name(string id) => id == "cold.chill" ? "Chill" : id == "cold.frostbite" ? "Frostbite" : "Necrosis";
        public static Sprite Icon(string id)
        {
            if (!IsCold(id)) return null;
            if (!icons.TryGetValue(id, out Sprite sprite)) icons[id] = sprite = Resources.Load<Sprite>("Battle/Cold/Icons/" + Name(id));
            return sprite;
        }
        public static Sprite Vfx(string id) => IsCold(id) ? Resources.Load<Sprite>("Battle/Cold/VFX/" + Name(id)) : null;
        /// <summary>표시 호출자가 선택한 정지 VFX 하나에만 B안을 적용합니다. 상태 수명/동시 우선순위는 결정하지 않습니다.</summary>
        public static void PlaceVfx(Image body, Image effect, int screenPixels, Camera camera)
        {
            Canvas.ForceUpdateCanvases();
            float scale = body.canvas.scaleFactor;
            effect.raycastTarget = false; effect.preserveAspect = true;
            var rect = effect.rectTransform; rect.anchorMin = rect.anchorMax = body.rectTransform.anchorMin;
            rect.pivot = Vector2.one * .5f; rect.sizeDelta = Vector2.one * screenPixels / scale;
            rect.anchoredPosition = body.rectTransform.anchoredPosition + new Vector2(0, -38);
            if (screenPixels == 128) { rect.anchoredPosition -= new Vector2(0, 24 / scale); effect.transform.SetSiblingIndex(body.transform.GetSiblingIndex()); }
            else
            {
                // 투명 캔버스 가장자리가 아니라 실제 가시 알파 사이를 화면 8px로 분리합니다.
                Rect b = Visible(body, camera), v = Visible(effect, camera);
                rect.anchoredPosition += new Vector2(0, (b.yMin - 8 - v.yMax) / scale);
            }
        }
        public static Rect Visible(Image image, Camera camera)
        {
            Sprite sprite = image.sprite; Texture2D t = sprite.texture; Rect s = sprite.rect;
            var tmp = RenderTexture.GetTemporary(t.width, t.height, 0); var previous = RenderTexture.active;
            Graphics.Blit(t, tmp); RenderTexture.active = tmp;
            var copy = new Texture2D(t.width, t.height, TextureFormat.RGBA32, false); copy.ReadPixels(new Rect(0, 0, t.width, t.height), 0, 0); copy.Apply();
            Color32[] px = copy.GetPixels32(); int lx = (int)s.xMax, ly = (int)s.yMax, hx = (int)s.x, hy = (int)s.y;
            for (int y = (int)s.y; y < s.yMax; y++) for (int x = (int)s.x; x < s.xMax; x++) if (px[y * t.width + x].a > 8) { lx = Math.Min(lx, x); ly = Math.Min(ly, y); hx = Math.Max(hx, x); hy = Math.Max(hy, y); }
            RenderTexture.active = previous; RenderTexture.ReleaseTemporary(tmp); UnityEngine.Object.Destroy(copy);
            Rect r = image.GetPixelAdjustedRect(); float factor = image.preserveAspect ? Mathf.Min(r.width / s.width, r.height / s.height) : r.width / s.width;
            Vector2 min = r.center - s.size * factor * .5f; Vector3 p0 = image.transform.TransformPoint(min + new Vector2(lx - s.x, ly - s.y) * factor); Vector3 p1 = image.transform.TransformPoint(min + new Vector2(hx + 1 - s.x, hy + 1 - s.y) * factor);
            Vector2 a = RectTransformUtility.WorldToScreenPoint(camera, p0), b = RectTransformUtility.WorldToScreenPoint(camera, p1); return Rect.MinMaxRect(a.x, a.y, b.x, b.y);
        }
    }
    public sealed partial class BattleSceneController
    {
        bool approvedColdHudExpanded;
        /// <summary>이미 전달된 냉기 표식이 있을 때 표시만 넓힙니다. 신규 상태를 부여하지 않습니다.</summary>
        private void ApplyApprovedColdRow(CombatantHpRow row)
        {
            RectTransform hud = hpHudTransform.GetComponent<RectTransform>();
            if (!approvedColdHudExpanded)
            {
                var positions = new Dictionary<Transform, Vector3>(); foreach (Transform child in hud) positions[child] = child.position;
                hud.sizeDelta += new Vector2(0, 58); hud.anchoredPosition -= new Vector2(0, 29); Canvas.ForceUpdateCanvases();
                foreach (var pair in positions) pair.Key.position = pair.Value;
                approvedColdHudExpanded = true;
            }
            var background = row.Background.rectTransform;
            // 같은 행을 갱신해도 위치가 매번 내려가지 않도록 크기를 기준으로 한 번만 확장합니다.
            if (background.sizeDelta.y < 80)
            {
                background.sizeDelta = new Vector2(background.sizeDelta.x, 82); background.anchoredPosition -= new Vector2(0, 25);
                foreach (var rect in new[] { row.Name.rectTransform, row.HpFill.transform.parent.GetComponent<RectTransform>() }) { rect.anchorMin = new Vector2(rect.anchorMin.x, 1); rect.anchorMax = new Vector2(rect.anchorMax.x, 1); rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -10.24f); }
            }
            for (int i = 0; i < row.StatusBadges.Count; i++)
            {
                var badge = row.StatusBadges[i]; var root = badge.Root.GetComponent<RectTransform>();
                // 배지는 해당 인물의 행을 부모로 삼아 다른 인물의 HP 아래로 이동하지 않게 합니다.
                root.SetParent(background, false);
                float width = (background.rect.width - 8) / (i < 3 ? 3 : 2);
                root.anchorMin = root.anchorMax = new Vector2(0, 1); root.pivot = Vector2.one * .5f; root.sizeDelta = new Vector2(width, 22); root.anchoredPosition = new Vector2(4 + width * .5f + (i < 3 ? i : i - 3) * width, i < 3 ? -34 : -61);
                var icon = badge.Icon.rectTransform; icon.anchorMin = icon.anchorMax = new Vector2(0, .5f); icon.pivot = Vector2.one * .5f; icon.anchoredPosition = new Vector2(8, 0); icon.sizeDelta = new Vector2(14, 14);
                Text label = badge.Label; label.fontSize = 14; label.resizeTextForBestFit = false; label.horizontalOverflow = HorizontalWrapMode.Overflow; var text = label.rectTransform; text.anchorMin = text.anchorMax = new Vector2(0, .5f); text.pivot = new Vector2(0, .5f); text.anchoredPosition = new Vector2(19, 0); text.sizeDelta = new Vector2(width - 21, 22);
            }
            EnsureApprovedHudFieldGap();
        }
        private void EnsureApprovedHudFieldGap()
        {
            if (battlefieldTransform == null || hpHudTransform == null) return; Canvas.ForceUpdateCanvases();
            var field = battlefieldTransform.GetComponent<RectTransform>(); var hud = hpHudTransform.GetComponent<RectTransform>();
            var a = new Vector3[4]; var b = new Vector3[4]; field.GetWorldCorners(a); hud.GetWorldCorners(b);
            Canvas canvas = field.GetComponentInParent<Canvas>(); Camera camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            float top = RectTransformUtility.WorldToScreenPoint(camera, a[2]).y, bottom = RectTransformUtility.WorldToScreenPoint(camera, b[0]).y;
            float trim = Mathf.Max(0, top - (bottom - 8)) / canvas.scaleFactor;
            field.sizeDelta -= new Vector2(0, trim); field.anchoredPosition -= new Vector2(0, trim * .5f);
        }
    }
}
