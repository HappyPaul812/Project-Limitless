using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.Monster
{
    /// <summary>
    /// MonsterDefinition에 저장된 원본 SpriteSheet를 실행 중에 프레임으로 나누어 반복 재생합니다.
    /// 원본 PNG를 잘라 새 이미지로 만들지 않으며, Field의 SpriteRenderer와 Battle의 UI Image가
    /// 같은 프레임 구조를 공유할 수 있게 합니다.
    /// </summary>
    public sealed class MonsterSpriteSheetAnimation : MonoBehaviour
    {
        private MonsterDefinition definition;
        private SpriteRenderer fieldRenderer;
        private Image battleImage;
        private Sprite[] idleFrames = Array.Empty<Sprite>();
        private Sprite[] attackFrames = Array.Empty<Sprite>();
        private float elapsed;
        private bool attacking;

        public void Configure(SpriteRenderer target, MonsterDefinition monster)
        {
            fieldRenderer = target;
            Configure(monster);
        }

        public void Configure(Image target, MonsterDefinition monster)
        {
            battleImage = target;
            Configure(monster);
        }

        private void Configure(MonsterDefinition monster)
        {
            definition = monster;
            idleFrames = CreateFrames(monster?.IdleSpriteSheet, monster?.IdleFrameSize ?? Vector2Int.zero,
                monster?.IdleColumns ?? 1, monster?.IdleFrameCount ?? 0, $"{monster?.MonsterId}_Idle");
            attackFrames = CreateFrames(monster?.AttackSpriteSheet, monster?.AttackFrameSize ?? Vector2Int.zero,
                monster?.AttackColumns ?? 1, monster?.AttackFrameCount ?? 0, $"{monster?.MonsterId}_Attack");
            ShowFrame(idleFrames, 0);
        }

        /// <summary>
        /// Battle 기본 공격이 시작될 때 Attack 프레임으로 전환합니다. 공격 연출이 끝나면
        /// StopAttackAndReturnToIdle이 호출되어 같은 Image가 다시 Idle 반복으로 돌아갑니다.
        /// </summary>
        public void PlayAttack()
        {
            if (attackFrames.Length == 0) return;
            attacking = true;
            elapsed = 0f;
            ShowFrame(attackFrames, 0);
        }

        public void StopAttackAndReturnToIdle()
        {
            attacking = false;
            elapsed = 0f;
            ShowFrame(idleFrames, 0);
        }

        private void Update()
        {
            if (definition == null) return;
            Sprite[] frames = attacking ? attackFrames : idleFrames;
            if (frames.Length == 0) return;

            elapsed += Time.unscaledDeltaTime;
            int frameIndex = Mathf.FloorToInt(elapsed * definition.AnimationFramesPerSecond);
            if (attacking && frameIndex >= frames.Length)
            {
                StopAttackAndReturnToIdle();
                return;
            }
            ShowFrame(frames, frameIndex % frames.Length);
        }

        private void ShowFrame(Sprite[] frames, int index)
        {
            if (frames == null || frames.Length == 0) return;
            if (fieldRenderer != null) fieldRenderer.sprite = frames[index];
            if (battleImage != null) battleImage.sprite = frames[index];
        }

        private static Sprite[] CreateFrames(Texture2D sheet, Vector2Int frameSize, int columns,
            int frameCount, string prefix)
        {
            if (sheet == null || frameSize.x <= 0 || frameSize.y <= 0 || columns <= 0 || frameCount <= 0)
                return Array.Empty<Sprite>();

            Sprite[] frames = new Sprite[frameCount];
            for (int index = 0; index < frameCount; index++)
            {
                int column = index % columns;
                int rowFromTop = index / columns;
                float y = sheet.height - ((rowFromTop + 1) * frameSize.y);
                // 셀 너비를 PPU로 사용하면 Field에서 한 프레임 폭이 1월드 단위가 됩니다.
                // 여기에 Definition의 0.85 Scale을 적용해 검증 Scene에서 확정한 상대 크기를 유지합니다.
                frames[index] = Sprite.Create(sheet,
                    new Rect(column * frameSize.x, y, frameSize.x, frameSize.y), new Vector2(.5f, .5f),
                    frameSize.x, 0, SpriteMeshType.FullRect);
                frames[index].name = $"{prefix}_{index:00}";
            }
            return frames;
        }
    }
}
