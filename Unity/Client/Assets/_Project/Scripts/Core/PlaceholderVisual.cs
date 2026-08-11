using UnityEngine;

namespace ProjectLimitless.Core
{
    /// <summary>
    /// 실제 그림이 없는 GameObject에 임시 단색 Sprite와 글자 이름표를 만들어 줍니다.
    /// SpriteRenderer(2D 그림 표시 컴포넌트)가 있는 플레이어·NPC 등의 개발용 임시 모습에 붙입니다.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class PlaceholderVisual : MonoBehaviour
    {
        // 아래 값들은 private이지만 SerializeField로 Inspector에서 확인하고 조절할 수 있습니다.
        [SerializeField] private Color placeholderColor = Color.white;
        [SerializeField] private Vector2 size = Vector2.one;
        [SerializeField] private string label;
        [SerializeField] private int sortingOrder;

        /// <summary>Scene 생성 도구가 임시 모습의 색, 크기, 이름표, 앞뒤 순서를 설정할 때 사용합니다.</summary>
        public void Configure(Color color, Vector2 visualSize, string visualLabel, int order = 0)
        {
            placeholderColor = color;
            size = visualSize;
            label = visualLabel;
            sortingOrder = order;
        }

        /// <summary>그림이 없으면 흰 사각형을 만들고 설정된 색·크기·이름표를 적용합니다.</summary>
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

        /// <summary>색을 입혀 사용할 1×1 픽셀 흰색 Sprite를 메모리에서 만듭니다.</summary>
        private static Sprite CreateSquareSprite()
        {
            Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            return Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
        }

        /// <summary>색상만으로 대상을 구분하지 않아도 되도록 오브젝트 위에 글자 이름표를 만듭니다.</summary>
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
            textMesh.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            textMesh.color = Color.white;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = order;
        }
    }
}
