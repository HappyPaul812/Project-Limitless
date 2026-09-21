using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.Battle
{
    /// <summary>stable Character ID에 연결된 전투 전용 Animator를 uGUI Image에서 재생합니다.</summary>
    public sealed class BattleCharacterSpriteAnimator : MonoBehaviour
    {
        private readonly struct CharacterVisualDefinition
        {
            public CharacterVisualDefinition(string characterId, string controllerPath, string sheetPath, string spritePrefix)
            {
                CharacterId = characterId;
                ControllerPath = controllerPath;
                SheetPath = sheetPath;
                SpritePrefix = spritePrefix;
            }

            public string CharacterId { get; }
            public string ControllerPath { get; }
            public string SheetPath { get; }
            public string SpritePrefix { get; }
        }

        // 캐릭터별 차이는 Resources 경로와 Sprite 접두사 데이터로만 관리합니다.
        private static readonly CharacterVisualDefinition[] CharacterVisuals =
        {
            new CharacterVisualDefinition("companion_taeon", "BattleCharacters/Taeon/Animations/Taeon_Battle",
                "BattleCharacters/Taeon/Taeon_Battle_Final", "Taeon"),
            new CharacterVisualDefinition("companion_miel", "BattleCharacters/Miel/Animations/Miel_Battle",
                "BattleCharacters/Miel/Miel_Battle_Final", "Miel")
        };

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
            if (targetImage == null) return false;

            CharacterVisualDefinition? matchedDefinition = null;
            foreach (CharacterVisualDefinition definition in CharacterVisuals)
            {
                if (!string.Equals(characterId, definition.CharacterId, StringComparison.Ordinal)) continue;
                matchedDefinition = definition;
                break;
            }
            if (!matchedDefinition.HasValue) return false;
            CharacterVisualDefinition visual = matchedDefinition.Value;

            RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>(visual.ControllerPath);
            Sprite[] sprites = Resources.LoadAll<Sprite>(visual.SheetPath);
            foreach (Sprite sprite in sprites)
            {
                if (sprite != null && sprite.name == $"{visual.SpritePrefix}_Idle_00")
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
