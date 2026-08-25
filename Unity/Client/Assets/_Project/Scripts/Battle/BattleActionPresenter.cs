using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.Battle
{
    /// <summary>실제 Projectile Sprite가 없을 때 사용할 임시 Graphic 형태입니다.</summary>
    public enum BattleProjectileStyle { Arrow, Orb }
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
        private static Sprite orbSprite;

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
        /// Sprite 프레임이 없으면 임시 Graphic을 만들며, 실제 에셋도 같은 이동/도착 처리를 재사용합니다.
        /// </summary>
        public IEnumerator PlayProjectileAttack(RectTransform attacker, RectTransform target, Image targetSprite, Font damageFont,
            Sprite[] projectileFrames, float frameDuration, float travelDuration, Vector2 projectileSize, bool directional,
            Color projectileColor, BattleProjectileStyle projectileStyle, float preparationDuration,
            Func<int> applyImpact, Action<int> onImpact, Action onComplete)
        {
            if (attacker == null || target == null || targetSprite == null)
            {
                int fallbackDamage = applyImpact == null ? 0 : applyImpact();
                onImpact?.Invoke(fallbackDamage);
                onComplete?.Invoke();
                yield break;
            }

            yield return new WaitForSeconds(Mathf.Max(0f, preparationDuration));

            Vector3 start = attacker.localPosition;
            Vector3 destination = target.localPosition;
            Vector3 direction = destination - start;
            Sprite firstFrame = projectileFrames != null && projectileFrames.Length > 0 ? projectileFrames[0] : null;
            GameObject projectileObject = new GameObject("BattleProjectile", typeof(Image));
            projectileObject.transform.SetParent(attacker.parent, false);
            Image projectileImage = projectileObject.GetComponent<Image>();
            projectileImage.sprite = firstFrame != null ? firstFrame : projectileStyle == BattleProjectileStyle.Orb ? GetOrbSprite() : null;
            projectileImage.color = projectileColor;
            projectileImage.raycastTarget = false;
            projectileImage.preserveAspect = true;
            RectTransform projectile = projectileImage.rectTransform;
            projectile.anchorMin = Vector2.one * .5f;
            projectile.anchorMax = Vector2.one * .5f;
            projectile.pivot = Vector2.one * .5f;
            projectile.sizeDelta = firstFrame != null ? projectileSize : projectileStyle == BattleProjectileStyle.Orb ? new Vector2(30f, 30f) : new Vector2(46f, 8f);
            projectile.localPosition = start;
            projectile.localScale = directional && destination.x < start.x ? new Vector3(-1f, 1f, 1f) : Vector3.one;
            projectile.localRotation = projectileStyle == BattleProjectileStyle.Arrow
                ? Quaternion.Euler(0f, 0f, GetDirectionalAngle(direction))
                : Quaternion.identity;

            yield return MoveProjectile(projectile, projectileImage, projectileFrames, frameDuration, travelDuration, start, destination);
            Destroy(projectileObject);

            Color targetOriginalColor = targetSprite.color;
            int damage = applyImpact == null ? 0 : applyImpact();
            onImpact?.Invoke(damage);
            if (damage > 0) StartCoroutine(ShowDamageNumber(target, damageFont, damage));
            yield return PlayHitReaction(target, targetSprite, destination, targetOriginalColor);
            onComplete?.Invoke();
        }

        /// <summary>스킬 사용자를 제자리에서 짧게 밝히고 텍스트를 표시한 뒤 지정 시점에 효과를 적용합니다.</summary>
        public IEnumerator PlaySkillEmphasis(RectTransform actor, Image actorSprite, Font font, string callout,
            Action applyEffect, Action onComplete)
        {
            if (actor == null || actorSprite == null)
            {
                applyEffect?.Invoke();
                onComplete?.Invoke();
                yield break;
            }

            Color originalColor = actorSprite.color;
            Text calloutText = CreateSkillCallout(actor, font, callout);
            const float duration = .34f;
            const float effectTime = .14f;
            float elapsed = 0f;
            bool effectApplied = false;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float pulse = Mathf.Sin(t * Mathf.PI);
                actorSprite.color = Color.Lerp(originalColor, new Color(1f, .9f, .48f, originalColor.a), pulse);
                if (calloutText != null)
                {
                    calloutText.rectTransform.anchoredPosition = new Vector2(0f, 82f + 18f * t);
                    Color color = calloutText.color;
                    color.a = 1f - Mathf.Clamp01((t - .65f) / .35f);
                    calloutText.color = color;
                }
                if (!effectApplied && elapsed >= effectTime)
                {
                    effectApplied = true;
                    applyEffect?.Invoke();
                }
                yield return null;
            }

            if (!effectApplied) applyEffect?.Invoke();
            actorSprite.color = originalColor;
            if (calloutText != null) Destroy(calloutText.gameObject);
            onComplete?.Invoke();
        }

        /// <summary>
        /// 선택한 아군 위치에서 회복 Sprite 프레임을 재생합니다. manifest의 peak 프레임에 도달했을 때
        /// applyHealing을 한 번만 호출하여 가장 밝게 피어나는 순간과 실제 HP 증가 시점을 맞춥니다.
        /// Presenter는 회복 공식을 알지 못하고 전달받은 함수를 호출하므로 전투 계산과 연출이 분리됩니다.
        /// </summary>
        public IEnumerator PlayHealingEffect(RectTransform target, Font font, Sprite[] frames, float frameDuration,
            int peakFrame, Vector2 effectSize, Func<int> applyHealing, Action<int> onImpact, Action onComplete)
        {
            if (target == null || frames == null || frames.Length == 0)
            {
                int fallbackHealing = applyHealing == null ? 0 : applyHealing();
                onImpact?.Invoke(fallbackHealing);
                if (fallbackHealing > 0) StartCoroutine(ShowHealingNumber(target, font, fallbackHealing));
                onComplete?.Invoke();
                yield break;
            }

            GameObject effectObject = new GameObject("RadiantHealEffect", typeof(Image));
            effectObject.transform.SetParent(target, false);
            Image effectImage = effectObject.GetComponent<Image>();
            effectImage.sprite = frames[0];
            effectImage.preserveAspect = true;
            effectImage.raycastTarget = false;
            RectTransform effectRect = effectImage.rectTransform;
            effectRect.anchorMin = Vector2.one * .5f;
            effectRect.anchorMax = Vector2.one * .5f;
            effectRect.pivot = new Vector2(.5f, 29f / 96f);
            effectRect.anchoredPosition = new Vector2(0f, -42f);
            effectRect.sizeDelta = effectSize;

            float safeFrameDuration = Mathf.Max(.01f, frameDuration);
            float totalDuration = frames.Length * safeFrameDuration;
            float elapsed = 0f;
            bool healingApplied = false;
            while (elapsed < totalDuration)
            {
                int frameIndex = Mathf.Min(frames.Length - 1, Mathf.FloorToInt(elapsed / safeFrameDuration));
                effectImage.sprite = frames[frameIndex];
                if (!healingApplied && frameIndex >= Mathf.Clamp(peakFrame, 0, frames.Length - 1))
                {
                    healingApplied = true;
                    int recoveredHp = applyHealing == null ? 0 : applyHealing();
                    onImpact?.Invoke(recoveredHp);
                    if (recoveredHp > 0) StartCoroutine(ShowHealingNumber(target, font, recoveredHp));
                }
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (!healingApplied)
            {
                int recoveredHp = applyHealing == null ? 0 : applyHealing();
                onImpact?.Invoke(recoveredHp);
                if (recoveredHp > 0) StartCoroutine(ShowHealingNumber(target, font, recoveredHp));
            }
            Destroy(effectObject);
            onComplete?.Invoke();
        }

        private static Text CreateSkillCallout(RectTransform actor, Font font, string callout)
        {
            if (font == null || string.IsNullOrEmpty(callout)) return null;
            GameObject obj = new GameObject("SkillCallout", typeof(Text));
            obj.transform.SetParent(actor, false);
            Text text = obj.GetComponent<Text>();
            text.font = font;
            text.fontSize = 24;
            text.fontStyle = FontStyle.Bold;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = new Color(1f, .84f, .36f, 1f);
            text.text = callout;
            text.raycastTarget = false;
            text.rectTransform.anchorMin = Vector2.one * .5f;
            text.rectTransform.anchorMax = Vector2.one * .5f;
            text.rectTransform.pivot = Vector2.one * .5f;
            text.rectTransform.sizeDelta = new Vector2(150f, 38f);
            text.rectTransform.anchoredPosition = new Vector2(0f, 82f);
            return text;
        }
        /// <summary>원본이 오른쪽을 향한다는 전제에서 수평 반전 뒤에도 이동 기울기가 유지되도록 각도를 계산합니다.</summary>
        private static float GetDirectionalAngle(Vector3 direction)
        {
            float horizontal = Mathf.Abs(direction.x);
            float angle = Mathf.Atan2(direction.y, horizontal) * Mathf.Rad2Deg;
            return direction.x < 0f ? -angle : angle;
        }

        private static IEnumerator MoveProjectile(RectTransform subject, Image image, Sprite[] frames, float frameDuration,
            float travelDuration, Vector3 from, Vector3 to)
        {
            travelDuration = Mathf.Max(.01f, travelDuration);
            float elapsed = 0f;
            while (elapsed < travelDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / travelDuration);
                subject.localPosition = Vector3.LerpUnclamped(from, to, 1f - Mathf.Pow(1f - t, 3f));
                if (frames != null && frames.Length > 0 && frameDuration > 0f)
                    image.sprite = frames[Mathf.FloorToInt(elapsed / frameDuration) % frames.Length];
                yield return null;
            }
            subject.localPosition = to;
        }
        /// <summary>파일을 만들지 않고 임시 빛 구체용 원형 Sprite를 한 번만 생성합니다.</summary>
        private static Sprite GetOrbSprite()
        {
            if (orbSprite != null) return orbSprite;

            const int size = 32;
            Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false)
            {
                name = "BattleProjectileOrbTexture",
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp,
                hideFlags = HideFlags.HideAndDontSave,
            };
            Color[] pixels = new Color[size * size];
            Vector2 center = Vector2.one * (size - 1) * .5f;
            float radius = size * .5f;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), center) / radius;
                    float alpha = Mathf.Clamp01(1f - distance);
                    pixels[y * size + x] = new Color(1f, 1f, 1f, alpha * alpha);
                }
            }
            texture.SetPixels(pixels);
            texture.Apply(false, true);
            orbSprite = Sprite.Create(texture, new Rect(0f, 0f, size, size), Vector2.one * .5f, size);
            orbSprite.name = "BattleProjectileOrbSprite";
            orbSprite.hideFlags = HideFlags.HideAndDontSave;
            return orbSprite;
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

        /// <summary>
        /// 회복량은 색상뿐 아니라 반드시 + 기호를 붙여 피해 숫자와 구분합니다.
        /// target이 없는 예외 경로에서는 숫자 오브젝트를 만들지 않아 NullReference를 피합니다.
        /// </summary>
        private static IEnumerator ShowHealingNumber(RectTransform target, Font font, int recoveredHp)
        {
            if (target == null || font == null || recoveredHp <= 0) yield break;
            GameObject numberObject = new GameObject("HealingNumber", typeof(Text));
            numberObject.transform.SetParent(target.parent, false);
            Text text = numberObject.GetComponent<Text>();
            text.font = font;
            text.fontSize = 25;
            text.fontStyle = FontStyle.Bold;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = new Color(.45f, 1f, .62f, 1f);
            text.text = $"+{recoveredHp}";
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
