using UnityEngine;

namespace ProjectLimitless.Battle
{
    /// <summary>
    /// 필드 Animator의 현재 상태와 무관하게 전투에서 사용할 고정 Idle Sprite를 선택합니다.
    /// 향후 전투 전용 Sprite나 Animator가 생기면 이 해석기 입력만 전투 전용 데이터로 교체할 수 있습니다.
    /// </summary>
    public static class BattleVisualResolver
    {
        public const string AllyIdleState = "Idle_Left";
        public const string EnemyIdleState = "Idle_Right";

        /// <summary>
        /// 임시 Animator에서 지정한 Idle 상태를 직접 평가해 첫 Sprite를 얻습니다.
        /// 상태 평가가 불가능해도 필드의 현재 프레임이 아닌 외형 데이터의 안정적인 기본 Sprite를 사용합니다.
        /// </summary>
        public static Sprite ResolveIdleSprite(RuntimeAnimatorController controller, string idleStateName, Sprite fallbackSprite)
        {
            if (controller == null) return fallbackSprite;

            GameObject sampleObject = new GameObject("BattleIdleSpriteSample", typeof(SpriteRenderer), typeof(Animator));
            sampleObject.hideFlags = HideFlags.HideAndDontSave;
            SpriteRenderer renderer = sampleObject.GetComponent<SpriteRenderer>();
            Animator animator = sampleObject.GetComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            animator.Rebind();
            animator.Play(idleStateName, 0, 0f);
            animator.Update(0f);

            Sprite idleSprite = renderer.sprite;
            UnityEngine.Object.Destroy(sampleObject);
            return idleSprite != null ? idleSprite : fallbackSprite;
        }
    }
}
