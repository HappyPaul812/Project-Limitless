using UnityEngine;
using ProjectLimitless.Core;

namespace ProjectLimitless.Player
{
    /// <summary>현재 단계에서 선택할 수 있는 플레이어 외형 종류입니다.</summary>
    public enum PlayerVisualType
    {
        Male,
        Female,
    }

    /// <summary>
    /// 플레이어의 이동·충돌·상호작용은 부모 GameObject에 그대로 두고, 화면에 보이는 외형만 선택합니다.
    /// Male 또는 Female을 고르면 Visual 자식의 첫 Sprite와 Animator Controller만 함께 교체합니다.
    /// 따라서 외형이 늘어나도 PlayerController에 성별별 이동 코드를 추가할 필요가 없습니다.
    /// </summary>
    public sealed class PlayerVisualController : MonoBehaviour
    {
        // 현재 기본값은 기존 동작을 보존하도록 Male입니다. Inspector에서 Female로 바꾸어 시험할 수 있습니다.
        [SerializeField] private PlayerVisualType visualType = PlayerVisualType.Male;
        // 실제 그림과 애니메이션은 부모의 물리 Component와 분리된 Visual 자식에 있습니다.
        [SerializeField] private SpriteRenderer visualRenderer;
        [SerializeField] private Animator visualAnimator;
        [SerializeField] private RuntimeAnimatorController maleAnimatorController;
        [SerializeField] private RuntimeAnimatorController femaleAnimatorController;
        [SerializeField] private Sprite maleDefaultSprite;
        [SerializeField] private Sprite femaleDefaultSprite;

        /// <summary>현재 선택된 외형입니다. 향후 캐릭터 생성 화면이나 저장 시스템에서 읽을 수 있습니다.</summary>
        public PlayerVisualType VisualType => visualType;

        /// <summary>게임이 시작될 때 Character Creation에서 세션에 저장한 외형을 읽어 적용합니다.</summary>
        private void Awake()
        {
            SetVisual(GameSessionData.SelectedPlayerVisual);
            if (GetComponent<PlayerPathVisualController>() == null)
            {
                gameObject.AddComponent<PlayerPathVisualController>();
            }
        }

        /// <summary>Inspector에서 값을 바꾸면 Play Mode 전에도 선택 결과를 미리 볼 수 있게 합니다.</summary>
        private void OnValidate()
        {
            ApplyVisual();
        }

        /// <summary>향후 캐릭터 생성 화면 등이 Male 또는 Female 외형을 선택할 때 호출합니다.</summary>
        public void SetVisual(PlayerVisualType newVisualType)
        {
            visualType = newVisualType;
            ApplyVisual();
        }

        /// <summary>Editor 생성 도구가 Visual 참조와 두 외형의 Asset을 안전하게 연결합니다.</summary>
        public void Configure(
            SpriteRenderer renderer,
            Animator animator,
            RuntimeAnimatorController maleController,
            RuntimeAnimatorController femaleController,
            Sprite maleSprite,
            Sprite femaleSprite)
        {
            visualRenderer = renderer;
            visualAnimator = animator;
            maleAnimatorController = maleController;
            femaleAnimatorController = femaleController;
            maleDefaultSprite = maleSprite;
            femaleDefaultSprite = femaleSprite;
            visualType = PlayerVisualType.Male;
            ApplyVisual();
        }

        /// <summary>선택값에 맞는 Sprite와 Animator Controller를 Visual 자식에 동시에 적용합니다.</summary>
        private void ApplyVisual()
        {
            if (visualRenderer == null || visualAnimator == null)
            {
                return;
            }

            bool useFemale = visualType == PlayerVisualType.Female;
            visualRenderer.sprite = useFemale ? femaleDefaultSprite : maleDefaultSprite;
            visualAnimator.runtimeAnimatorController = useFemale ? femaleAnimatorController : maleAnimatorController;

            // Controller가 바뀌면 같은 상태 이름이라도 새 외형에서 다시 재생해야 합니다.
            visualAnimator.GetComponent<PlayerSpriteAnimator>()?.RefreshVisual();
            GetComponent<PlayerPathVisualController>()?.Apply(GameSessionData.SelectedPlayerPathId);
        }
    }
}
