using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.Battle
{
    /// <summary>stable Character ID에 연결된 전투 전용 Animator를 uGUI Image에서 재생합니다.</summary>
    public sealed class BattleCharacterSpriteAnimator : MonoBehaviour
    {
        public const string TaeonId = "companion_taeon";
        private const string TaeonControllerPath = "BattleCharacters/Taeon/Animations/Taeon_Battle";
        private const string TaeonIdleSpritePath = "BattleCharacters/Taeon/Taeon_Battle_Final";

        private static readonly int AttackTrigger = Animator.StringToHash("Attack");
        private static readonly int GuardTrigger = Animator.StringToHash("Guard");
        private static readonly int SkillTrigger = Animator.StringToHash("Skill");
        private static readonly int HitTrigger = Animator.StringToHash("Hit");
        private static readonly int DefeatTrigger = Animator.StringToHash("Defeat");

        private Animator animator;
        private Image image;
        private Sprite idleSprite;
        private bool defeated;

        public bool IsConfigured => animator != null && animator.runtimeAnimatorController != null && image != null;
        public bool IsDefeated => defeated;

        public static bool TryAttach(string characterId, Image targetImage,
            out BattleCharacterSpriteAnimator playback, out Sprite firstIdleFrame)
        {
            playback = null;
            firstIdleFrame = null;
            if (characterId != TaeonId || targetImage == null) return false;

            RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>(TaeonControllerPath);
            Sprite[] sprites = Resources.LoadAll<Sprite>(TaeonIdleSpritePath);
            foreach (Sprite sprite in sprites)
            {
                if (sprite != null && sprite.name == "Taeon_Idle_00")
                {
                    firstIdleFrame = sprite;
                    break;
                }
            }

            if (controller == null || firstIdleFrame == null) return false;
            Animator targetAnimator = targetImage.GetComponent<Animator>();
            if (targetAnimator == null) targetAnimator = targetImage.gameObject.AddComponent<Animator>();
            targetAnimator.runtimeAnimatorController = controller;
            targetAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;

            playback = targetImage.GetComponent<BattleCharacterSpriteAnimator>();
            if (playback == null) playback = targetImage.gameObject.AddComponent<BattleCharacterSpriteAnimator>();
            playback.animator = targetAnimator;
            playback.image = targetImage;
            playback.idleSprite = firstIdleFrame;
            playback.PlayIdle();
            return true;
        }

        public void PlayIdle()
        {
            if (!IsConfigured || defeated) return;
            animator.ResetTrigger(AttackTrigger);
            animator.ResetTrigger(GuardTrigger);
            animator.ResetTrigger(SkillTrigger);
            animator.ResetTrigger(HitTrigger);
            animator.Play("Idle", 0, 0f);
            animator.Update(0f);
            if (image.sprite == null) image.sprite = idleSprite;
        }

        public void PlayAttack() => SetTrigger(AttackTrigger);
        public void PlayGuard() => SetTrigger(GuardTrigger);
        public void PlaySkill() => SetTrigger(SkillTrigger);
        public void PlayHit() => SetTrigger(HitTrigger);

        public void PlayDefeat()
        {
            if (!IsConfigured || defeated) return;
            defeated = true;
            animator.SetTrigger(DefeatTrigger);
        }

        private void SetTrigger(int trigger)
        {
            if (!IsConfigured || defeated) return;
            animator.SetTrigger(trigger);
        }
    }
}
