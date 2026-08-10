using UnityEngine;

namespace ProjectLimitless.Core
{
    /// <summary>실제 스프라이트가 준비되기 전까지 사용할 단색 SpriteRenderer를 만든다.</summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class PlaceholderVisual : MonoBehaviour
    {
        [SerializeField] private Color placeholderColor = Color.white;
        [SerializeField] private Vector2 size = Vector2.one;
        [SerializeField] private string label;
        [SerializeField] private int sortingOrder;

        public void Configure(Color color, Vector2 visualSize, string visualLabel, int order = 0)
        {
            placeholderColor = color;
            size = visualSize;
            label = visualLabel;
            sortingOrder = order;
        }

        private void Awake()
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer.sprite == null)
            {
                spriteRenderer.sprite = CreateSquareSprite();
            }

            spriteRenderer.color = placeholderColor;
            spriteRenderer.sortingOrder = sortingOrder;
            transform.localScale = new Vector3(size.x, size.y, 1f);

            if (!string.IsNullOrWhiteSpace(label))
            {
                CreateLabel(spriteRenderer.sortingOrder + 1);
            }
        }

        private static Sprite CreateSquareSprite()
        {
            Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            return Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
        }

        private void CreateLabel(int order)
        {
            GameObject labelObject = new GameObject("Label");
            labelObject.transform.SetParent(transform, false);
            labelObject.transform.localScale = Vector3.one;
            labelObject.transform.localPosition = new Vector3(0f, 0.65f, 0f);

            TextMesh textMesh = labelObject.AddComponent<TextMesh>();
            textMesh.text = label;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            textMesh.characterSize = 0.12f;
            textMesh.fontSize = 48;
            textMesh.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            textMesh.color = Color.white;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = order;
        }
    }
}
