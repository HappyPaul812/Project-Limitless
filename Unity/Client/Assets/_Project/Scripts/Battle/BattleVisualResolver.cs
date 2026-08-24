using System;
using System.Linq;
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

        /// <summary>Animator Controller에서 지정한 Idle Clip의 첫 프레임을 얻습니다.</summary>
        public static Sprite ResolveIdleSprite(RuntimeAnimatorController controller, string idleStateName, Sprite fallbackSprite)
        {
            AnimationClip clip = controller?.animationClips.FirstOrDefault(item =>
                item != null && (string.Equals(item.name, idleStateName, StringComparison.OrdinalIgnoreCase)
                    || item.name.EndsWith("_" + idleStateName, StringComparison.OrdinalIgnoreCase)));
            if (clip == null)
            {
                Debug.LogWarning($"Battle Visual: '{idleStateName}' Clip을 찾지 못해 데이터의 기본 Sprite를 사용합니다.");
                return fallbackSprite;
            }

            GameObject sampleObject = new GameObject("BattleIdleSpriteSample", typeof(SpriteRenderer));
            sampleObject.hideFlags = HideFlags.HideAndDontSave;
            SpriteRenderer renderer = sampleObject.GetComponent<SpriteRenderer>();
            clip.SampleAnimation(sampleObject, 0f);
            Sprite idleSprite = renderer.sprite;
            UnityEngine.Object.Destroy(sampleObject);

            if (idleSprite != null) return idleSprite;
            Debug.LogWarning($"Battle Visual: '{clip.name}'에서 Sprite를 읽지 못해 데이터의 기본 Sprite를 사용합니다.");
            return fallbackSprite;
        }
    }
}
