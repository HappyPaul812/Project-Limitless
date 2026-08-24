using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.Battle
{
    /// <summary>
    /// 전투 계산과 분리된 공용 행동 연출기입니다.
    /// 플레이어, NPC, 몬스터의 UI 위치와 Sprite 표시만 전달받아 같은 연출을 재사용합니다.
    /// </summary>
    public sealed class BattleActionPresenter : MonoBehaviour
    {
        private const float ApproachDuration = .16f;
        private const float ImpactPause = .09f;
        private const float ReturnDuration = .18f;
        private const float TargetGap = 145f;
        private const float AimDuration = .2f;
        private const float ProjectileDuration = .24f;

        /// <summary>
        /// 공격자를 대상 앞까지 이동시키고 타격 순간에 기존 피해 처리를 호출한 뒤 원위치로 복귀합니다.
        /// 완료 콜백은 모든 표시 상태가 복구된 뒤 호출되므로 다음 턴 진행 시점으로 사용할 수 있습니다.
        /// </summary>
        public IEnumerator PlayMeleeAttack(RectTransform attacker, RectTransform target, Image targetSprite, Font damageFont,
            Func<int> applyImpact, Action<int> onImpact, Action onComplete)
        {
            if (attacker == null || target == null || targetSprite == null)
            {
                int fallbackDamage = applyImpact == null ? 0 : applyImpact();
                onImpact?.Invoke(fallbackDamage);
                onComplete?.Invoke();
                yield break;
            }

            Vector3 attackerOrigin = attacker.localPosition;
            Vector3 targetOrigin = target.localPosition;
            Color targetOriginalColor = targetSprite.color;
            Vector2 direction = targetOrigin.x >= attackerOrigin.x ? Vector2.right : Vector2.left;
            Vector3 destination = targetOrigin - (Vector3)(direction * TargetGap);

            yield return Move(attacker, attackerOrigin, destination, ApproachDuration);
            yield return new WaitForSeconds(ImpactPause);

            int damage = applyImpact == null ? 0 : applyImpact();
            onImpact?.Invoke(damage);
            if (damage > 0) StartCoroutine(ShowDamageNumber(target, damageFont, damage));
            yield return PlayHitReaction(target, targetSprite, targetOrigin, targetOriginalColor);
            yield return Move(attacker, attacker.localPosition, attackerOrigin, ReturnDuration);

            attacker.localPosition = attackerOrigin;
            target.localPosition = targetOrigin;
            targetSprite.color = targetOriginalColor;
            onComplete?.Invoke();
        }

        /// <summary>
        /// 공격자는 제자리에 둔 채 조준 후 Projectile을 목표까지 이동시킵니다.
        /// projectileSprite가 없으면 임시 Graphic을 만들며, 이후 화살이나 마법탄 Sprite를 같은 인자로 교체할 수 있습니다.
        /// </summary>
        public IEnumerator PlayProjectileAttack(RectTransform attacker, RectTransform target, Image targetSprite, Font damageFont,
            Sprite projectileSprite, Color projectileColor, Func<int> applyImpact, Action<int> onImpact, Action onComplete)
        {
            if (attacker == null || target == null || targetSprite == null)
            {
                int fallbackDamage = applyImpact == null ? 0 : applyImpact();
                onImpact?.Invoke(fallbackDamage);
                onComplete?.Invoke();
                yield break;
            }

            yield return new WaitForSeconds(AimDuration);

            Vector3 start = attacker.localPosition;
            Vector3 destination = target.localPosition;
            Vector3 direction = destination - start;
            GameObject projectileObject = new GameObject("BattleProjectile", typeof(Image));
            projectileObject.transform.SetParent(attacker.parent, false);
            Image projectileImage = projectileObject.GetComponent<Image>();
            projectileImage.sprite = projectileSprite;
            projectileImage.color = projectileColor;
            projectileImage.raycastTarget = false;
            RectTransform projectile = projectileImage.rectTransform;
            projectile.anchorMin = Vector2.one * .5f;
            projectile.anchorMax = Vector2.one * .5f;
            projectile.pivot = Vector2.one * .5f;
            projectile.sizeDelta = projectileSprite == null ? new Vector2(46f, 8f) : new Vector2(52f, 20f);
            projectile.localPosition = start;
            projectile.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

            yield return Move(projectile, start, destination, ProjectileDuration);
            Destroy(projectileObject);

            Color targetOriginalColor = targetSprite.color;
            int damage = applyImpact == null ? 0 : applyImpact();
            onImpact?.Invoke(damage);
            if (damage > 0) StartCoroutine(ShowDamageNumber(target, damageFont, damage));
            yield return PlayHitReaction(target, targetSprite, destination, targetOriginalColor);
            onComplete?.Invoke();
        }
        private static IEnumerator Move(RectTransform subject, Vector3 from, Vector3 to, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                subject.localPosition = Vector3.LerpUnclamped(from, to, 1f - Mathf.Pow(1f - t, 3f));
                yield return null;
            }
            subject.localPosition = to;
        }

        private static IEnumerator PlayHitReaction(RectTransform target, Image sprite, Vector3 origin, Color originalColor)
        {
            const float duration = .18f;
            float elapsed = 0f;
            Color flashColor = new Color(originalColor.r, originalColor.g, originalColor.b, .35f);
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float shake = Mathf.Sin(t * Mathf.PI * 6f) * (1f - t) * 10f;
                target.localPosition = origin + Vector3.right * shake;
                sprite.color = t < .55f ? flashColor : Color.Lerp(flashColor, originalColor, (t - .55f) / .45f);
                yield return null;
            }
            target.localPosition = origin;
            sprite.color = originalColor;
        }

        private static IEnumerator ShowDamageNumber(RectTransform target, Font font, int damage)
        {
            GameObject numberObject = new GameObject("DamageNumber", typeof(Text));
            numberObject.transform.SetParent(target.parent, false);
            Text text = numberObject.GetComponent<Text>();
            text.font = font;
            text.fontSize = 25;
            text.fontStyle = FontStyle.Bold;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = new Color(1f, .82f, .38f, 1f);
            text.text = $"-{damage}";
            text.raycastTarget = false;
            RectTransform rect = text.rectTransform;
            rect.anchorMin = target.anchorMin;
            rect.anchorMax = target.anchorMax;
            rect.pivot = Vector2.one * .5f;
            rect.sizeDelta = new Vector2(100f, 40f);
            Vector2 start = target.anchoredPosition + new Vector2(0f, 70f);

            const float duration = .55f;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                rect.anchoredPosition = start + Vector2.up * (42f * t);
                Color color = text.color;
                color.a = 1f - Mathf.Clamp01((t - .55f) / .45f);
                text.color = color;
                yield return null;
            }
            Destroy(numberObject);
        }
    }
}
