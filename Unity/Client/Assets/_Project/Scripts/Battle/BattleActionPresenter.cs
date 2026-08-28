using System;
using System.Collections;
using System.Collections.Generic;
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
        /// Run 애니메이션 Coroutine과 실제 이동 Coroutine이 공유하는 작은 종료 신호입니다.
        /// Transform이 목표에 도착하면 IsRunning을 끄고, Sprite 교체도 같은 시점에 멈춥니다.
        /// </summary>
        private sealed class BeastRunPlayback
        {
            public bool IsRunning = true;
        }

        /// <summary>
        /// 사수는 움직이지 않고 야수만 사수 근처에서 나타나 대상 바로 앞까지 달립니다.
        /// Run 프레임 교체는 제자리에서 다리가 달리는 모습을 만들고, RectTransform 이동은 실제 전장 위치를
        /// 바꿉니다. 둘을 함께 사용하되 독립 값으로 두어 Bear/Fox의 보폭과 이동 속도도 데이터로 조절할 수 있습니다.
        /// </summary>
        public IEnumerator PlayBeastCompanionAssault(RectTransform actor, RectTransform target, Image targetSprite,
            Font damageFont, BeastCompanionDefinition beast, Func<int> applyImpact, Action<int> onImpact, Action onComplete)
        {
            if (actor == null || target == null || targetSprite == null || beast == null)
            {
                int fallbackDamage = applyImpact == null ? 0 : applyImpact();
                onImpact?.Invoke(fallbackDamage);
                onComplete?.Invoke();
                yield break;
            }

            Texture2D runSheet = Resources.Load<Texture2D>(beast.RunResourcePath);
            Sprite[] runFrames = SliceBeastRunSheet(runSheet, beast);
            if (runFrames.Length == 0)
            {
                int fallbackDamage = applyImpact == null ? 0 : applyImpact();
                onImpact?.Invoke(fallbackDamage);
                onComplete?.Invoke();
                yield break;
            }

            GameObject beastObject = new GameObject($"BeastCompanion_{beast.Id}", typeof(Image));
            beastObject.transform.SetParent(actor.parent, false);
            Image beastImage = beastObject.GetComponent<Image>();
            beastImage.sprite = runFrames[0];
            beastImage.preserveAspect = true;
            beastImage.raycastTarget = false;
            RectTransform beastRect = beastImage.rectTransform;
            beastRect.anchorMin = beastRect.anchorMax = Vector2.one * .5f;
            beastRect.pivot = new Vector2(.5f, 0f);
            beastRect.sizeDelta = new Vector2(beast.FrameWidth * beast.DisplayScale,
                runSheet.height * beast.DisplayScale);

            // 출발점은 고정 화면 좌표가 아닙니다. 매 사용 시 현재 행동 중인 사수 ActionRoot의 실제 위치와
            // 표시 크기를 읽어 그 옆 지면을 계산하므로, Formation이나 전투 배치가 바뀌어도 사수를 따라갑니다.
            float moveDirection = target.localPosition.x < actor.localPosition.x ? -1f : 1f;
            float startSideGap = actor.rect.width * .5f + beastRect.rect.width * .1f;
            float startGroundOffset = actor.rect.height * .5f - 2f;
            Vector3 start = actor.localPosition + new Vector3(moveDirection * startSideGap, -startGroundOffset, 0f);

            // 정지점도 대상 ActionRoot와 Wolf 표시 폭에서 계산합니다. 서로의 반 너비를 고려해 Wolf가 대상
            // 몸 안으로 들어가지 않고 진행 방향 쪽 바로 앞에 멈춥니다.
            float contactSideGap = target.rect.width * .5f + beastRect.rect.width * .45f;
            float contactGroundOffset = target.rect.height * .5f - 2f;
            Vector3 contact = target.localPosition - new Vector3(moveDirection * contactSideGap, contactGroundOffset, 0f);
            beastRect.localPosition = start;
            // 원본 Wolf는 왼쪽을 향합니다. 향후 반대편 사수가 같은 Presenter를 사용하면 원본 PNG를
            // 수정하지 않고 Transform만 뒤집어 실제 이동 방향을 바라보게 합니다.
            beastRect.localScale = moveDirection < 0f ? Vector3.one : new Vector3(-1f, 1f, 1f);
            Text callout = CreateSkillCallout(actor, damageFont, "동료의 습격!");

            // Sprite 애니메이션과 Transform 이동은 서로 다른 일입니다. 전자는 12FPS로 다리 그림을 바꾸고,
            // 후자는 매 렌더 프레임 570 UI 단위/초로 위치를 바꿉니다. 두 Coroutine을 동시에 실행해야
            // 제자리 달리기 그림이 아니라 실제로 다리를 움직이며 전장을 달리는 모습이 됩니다.
            BeastRunPlayback runPlayback = new BeastRunPlayback();
            Coroutine runAnimation = StartCoroutine(PlayBeastRunAnimation(
                beastImage, runFrames, beast.FramesPerSecond, runPlayback));
            yield return MoveBeastTransform(beastRect, start, contact, beast.TravelSpeed);
            runPlayback.IsRunning = false;
            if (runAnimation != null) StopCoroutine(runAnimation);

            // 접촉 이전에는 HP를 건드리지 않습니다. 이 한 지점에서만 Executor를 호출해 Run 프레임 수와
            // 무관하게 180% 피해가 정확히 한 번 발생하고 기존 방어 판정도 같은 순간 적용됩니다.
            Vector3 targetOrigin = target.localPosition;
            Color targetOriginalColor = targetSprite.color;
            int damage = applyImpact == null ? 0 : applyImpact();
            onImpact?.Invoke(damage);
            if (damage > 0) StartCoroutine(ShowDamageNumber(target, damageFont, damage));

            // Wolf는 접촉 위치를 관통하지 않고 그대로 멈춥니다. 피격 반응 동안 자리를 유지하고 아주 짧은
            // 여운 뒤 제거한 다음 완료를 알리므로, 제거 전에 다음 턴 입력이 열리지 않습니다.
            yield return PlayHitReaction(target, targetSprite, targetOrigin, targetOriginalColor);
            yield return new WaitForSeconds(.12f);

            target.localPosition = targetOrigin;
            targetSprite.color = targetOriginalColor;
            if (callout != null) Destroy(callout.gameObject);
            Destroy(beastObject);
            foreach (Sprite frame in runFrames) Destroy(frame);
            onComplete?.Invoke();
        }

        /// <summary>
        /// 384×40 한 줄 SpriteSheet를 64×40 여섯 장으로 나눈 Sprite 배열을 0→1→2→3→4→5→0 순서로
        /// 반복합니다. 여기서는 위치를 바꾸지 않고 Image.sprite만 교체하므로 이동 속도와 독립된 12FPS를 유지합니다.
        /// UI 아이콘은 별도로 고른 index 4 한 장만 사용하지만, 전투 Run은 이 여섯 장 전체를 사용합니다.
        /// </summary>
        private static IEnumerator PlayBeastRunAnimation(Image beastImage, Sprite[] frames, float framesPerSecond,
            BeastRunPlayback playback)
        {
            if (beastImage == null || frames == null || frames.Length == 0 || playback == null) yield break;

            float frameInterval = 1f / Mathf.Max(1f, framesPerSecond);
            int frameIndex = 0;
            beastImage.sprite = frames[frameIndex];
            while (playback.IsRunning)
            {
                yield return new WaitForSeconds(frameInterval);
                if (!playback.IsRunning) yield break;
                frameIndex = (frameIndex + 1) % frames.Length;
                // 배열의 서로 다른 Sprite를 실제 전장 Image에 대입해야 화면에서 다리 모양이 바뀝니다.
                beastImage.sprite = frames[frameIndex];
            }
        }

        /// <summary>Sprite 프레임은 건드리지 않고 Wolf UI Transform의 위치만 지정 속도로 이동합니다.</summary>
        private static IEnumerator MoveBeastTransform(RectTransform beastRect, Vector3 from, Vector3 to, float speed)
        {
            float duration = Vector3.Distance(from, to) / Mathf.Max(1f, speed);
            float moveElapsed = 0f;
            while (moveElapsed < duration)
            {
                moveElapsed += Time.deltaTime;
                beastRect.localPosition = Vector3.Lerp(from, to, Mathf.Clamp01(moveElapsed / duration));
                yield return null;
            }
            beastRect.localPosition = to;
        }

        private static Sprite[] SliceBeastRunSheet(Texture2D sheet, BeastCompanionDefinition beast)
        {
            if (sheet == null || sheet.width % beast.FrameWidth != 0) return Array.Empty<Sprite>();
            int count = sheet.width / beast.FrameWidth;
            Sprite[] frames = new Sprite[count];
            for (int index = 0; index < count; index++)
            {
                // 1행 6열 Wolf 시트는 전체 384×40이고 각 칸은 64×40입니다. 원본 파일을 수정하지 않고
                // x=0,64,128,192,256,320 영역을 각각 별도 Sprite로 만들어 Run Coroutine에 전달합니다.
                // 아래 중앙 Pivot은 여섯 프레임 모두 발이 닿는 기준을 일정하게 유지합니다.
                frames[index] = Sprite.Create(sheet,
                    new Rect(index * beast.FrameWidth, 0f, beast.FrameWidth, sheet.height),
                    new Vector2(.5f, 0f), 1f, 0, SpriteMeshType.FullRect);
                frames[index].name = $"{beast.DisplayName}_Run_{index:00}";
            }
            return frames;
        }

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
        /// 회오리 베기는 대상 선택이나 Projectile 없이 제자리에서 짧게 회전하고 모든 적을 거의 동시에
        /// 타격합니다. 실제 HP/기세 계산은 applyImpacts에 남겨 Presenter가 80% 공식이나 전투 자원을
        /// 알지 않게 하고, 여기서는 타격 시점·피해 숫자·피격 반응이 끝나는 순서만 책임집니다.
        /// </summary>
        public IEnumerator PlayWhirlwindAttack(RectTransform attacker, Image attackerSprite,
            IReadOnlyList<RectTransform> targets, IReadOnlyList<Image> targetSprites, Font damageFont,
            Func<IReadOnlyList<int>> applyImpacts, Action<IReadOnlyList<int>> onImpact, Action onComplete)
        {
            if (attacker == null || attackerSprite == null || targets == null || targetSprites == null)
            {
                IReadOnlyList<int> fallback = applyImpacts == null ? Array.Empty<int>() : applyImpacts();
                onImpact?.Invoke(fallback);
                onComplete?.Invoke();
                yield break;
            }

            Color attackerOriginalColor = attackerSprite.color;
            Text callout = CreateSkillCallout(attacker, damageFont, "회오리 베기!");
            GameObject slashObject = new GameObject("WhirlwindSlash", typeof(Image));
            slashObject.transform.SetParent(attacker, false);
            Image slashImage = slashObject.GetComponent<Image>();
            slashImage.sprite = GetOrbSprite();
            slashImage.color = new Color(1f, .7f, .16f, .75f);
            slashImage.raycastTarget = false;
            RectTransform slash = slashImage.rectTransform;
            slash.anchorMin = slash.anchorMax = slash.pivot = Vector2.one * .5f;
            slash.sizeDelta = new Vector2(155f, 18f);

            // 준비 시간은 반응 속도를 요구하는 입력 구간이 아니라 짧은 시각적 예고입니다.
            const float preparationDuration = .1f;
            float elapsed = 0f;
            while (elapsed < preparationDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / preparationDuration);
                attackerSprite.color = Color.Lerp(attackerOriginalColor,
                    new Color(1f, .86f, .4f, attackerOriginalColor.a), t);
                slash.localRotation = Quaternion.Euler(0f, 0f, 120f * t);
                slash.localScale = Vector3.one * (.5f + .5f * t);
                yield return null;
            }

            IReadOnlyList<int> damages = applyImpacts == null ? Array.Empty<int>() : applyImpacts();
            onImpact?.Invoke(damages);

            int count = Mathf.Min(targets.Count, targetSprites.Count);
            Vector3[] origins = new Vector3[count];
            Color[] originalColors = new Color[count];
            for (int index = 0; index < count; index++)
            {
                if (targets[index] == null || targetSprites[index] == null) continue;
                origins[index] = targets[index].localPosition;
                originalColors[index] = targetSprites[index].color;
                if (index < damages.Count && damages[index] > 0)
                    StartCoroutine(ShowDamageNumber(targets[index], damageFont, damages[index]));
            }

            // 모든 대상의 흔들림을 한 루프에서 갱신해 순차 공격처럼 보이지 않게 합니다. 이 루프가 끝난 뒤에만
            // onComplete를 호출하므로 Controller는 피격 반응과 기세 HUD 갱신이 끝나기 전에 다음 턴을 열지 않습니다.
            const float impactDuration = .2f;
            elapsed = 0f;
            while (elapsed < impactDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / impactDuration);
                float shake = Mathf.Sin(t * Mathf.PI * 6f) * (1f - t) * 10f;
                for (int index = 0; index < count; index++)
                {
                    if (targets[index] == null || targetSprites[index] == null) continue;
                    targets[index].localPosition = origins[index] + Vector3.right * shake;
                    Color flash = new Color(originalColors[index].r, originalColors[index].g,
                        originalColors[index].b, .35f);
                    targetSprites[index].color = t < .55f
                        ? flash : Color.Lerp(flash, originalColors[index], (t - .55f) / .45f);
                }
                slash.localRotation = Quaternion.Euler(0f, 0f, 120f + 720f * t);
                slash.sizeDelta = Vector2.Lerp(new Vector2(155f, 18f), new Vector2(230f, 8f), t);
                slashImage.color = new Color(1f, Mathf.Lerp(.7f, 1f, t), .2f, 1f - t);
                yield return null;
            }

            for (int index = 0; index < count; index++)
            {
                if (targets[index] == null || targetSprites[index] == null) continue;
                targets[index].localPosition = origins[index];
                targetSprites[index].color = originalColors[index];
            }
            attackerSprite.color = attackerOriginalColor;
            if (callout != null) Destroy(callout.gameObject);
            Destroy(slashObject);
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

        /// <summary>
        /// 파이어 볼의 충전·큰 Projectile·명중 폭발을 한 행동으로 묶습니다. 실제 피해 함수는 Warm Explosion의
        /// index 4에서만 호출하므로 화염탄이 날아가는 동안 HP가 먼저 줄지 않습니다.
        /// </summary>
        public IEnumerator PlayFireballSkill(RectTransform attacker, RectTransform target, Image targetSprite, Font damageFont,
            Sprite[] projectileFrames, Sprite[] chargeFrames, Sprite[] explosionFrames,
            Func<int> applyImpact, Action<int> onImpact, Action onComplete)
        {
            if (attacker == null || target == null || targetSprite == null)
            {
                int fallbackDamage = applyImpact == null ? 0 : applyImpact();
                onImpact?.Invoke(fallbackDamage);
                onComplete?.Invoke();
                yield break;
            }

            Text callout = CreateSkillCallout(attacker, damageFont, "파이어 볼!");
            Image charge = CreateEffectImage(attacker, "FireballCharge", chargeFrames,
                BattleFireballVisuals.ChargeSize, new Vector2(0f, -2f));
            // Solar Shrapnel의 초기 Charge 두 장만 20FPS로 세 번 반복해, 마도사가 제자리에서 힘을
            // 모으는 단계가 짧게 지나가 버리지 않으면서도 원본 프레임 속도를 유지합니다.
            for (int index = 0; index < 6; index++)
            {
                if (charge != null && chargeFrames != null && chargeFrames.Length > 0)
                    charge.sprite = chargeFrames[index % chargeFrames.Length];
                yield return new WaitForSeconds(BattleFireballVisuals.FrameDuration);
            }
            if (charge != null) Destroy(charge.gameObject);
            if (callout != null) Destroy(callout.gameObject);

            Vector3 start = attacker.localPosition;
            Vector3 destination = target.localPosition;
            GameObject projectileObject = new GameObject("SkillFireballProjectile", typeof(Image));
            projectileObject.transform.SetParent(attacker.parent, false);
            Image projectileImage = projectileObject.GetComponent<Image>();
            projectileImage.sprite = projectileFrames != null && projectileFrames.Length > 0 ? projectileFrames[0] : GetOrbSprite();
            projectileImage.preserveAspect = true;
            projectileImage.raycastTarget = false;
            RectTransform projectile = projectileImage.rectTransform;
            projectile.anchorMin = projectile.anchorMax = projectile.pivot = Vector2.one * .5f;
            projectile.sizeDelta = BattleFireballVisuals.ProjectileSize;
            projectile.localPosition = start;
            projectile.localScale = destination.x < start.x ? new Vector3(-1f, 1f, 1f) : Vector3.one;
            yield return MoveProjectile(projectile, projectileImage, projectileFrames, .06f, .42f, start, destination);
            Destroy(projectileObject);

            Image explosion = CreateEffectImage(target, "WarmExplosion", explosionFrames,
                BattleFireballVisuals.ExplosionSize, Vector2.zero);
            Color targetOriginalColor = targetSprite.color;
            bool impactApplied = false;
            int frameCount = explosionFrames?.Length ?? 0;
            for (int frameIndex = 0; frameIndex < frameCount; frameIndex++)
            {
                if (explosion != null) explosion.sprite = explosionFrames[frameIndex];
                if (!impactApplied && frameIndex >= BattleFireballVisuals.ExplosionPeakFrame)
                {
                    impactApplied = true;
                    // 폭발 그림이 가장 커지는 한 프레임에서만 170% 피해와 화상 부여를 확정합니다.
                    int damage = applyImpact == null ? 0 : applyImpact();
                    onImpact?.Invoke(damage);
                    if (damage > 0)
                    {
                        StartCoroutine(ShowDamageNumber(target, damageFont, damage));
                        StartCoroutine(PlayHitReaction(target, targetSprite, destination, targetOriginalColor));
                    }
                }
                yield return new WaitForSeconds(BattleFireballVisuals.FrameDuration);
            }
            if (!impactApplied)
            {
                int damage = applyImpact == null ? 0 : applyImpact();
                onImpact?.Invoke(damage);
            }
            if (explosion != null) Destroy(explosion.gameObject);
            targetSprite.color = targetOriginalColor;
            onComplete?.Invoke();
        }

        /// <summary>
        /// 화상 틱은 새 화염탄이나 큰 폭발을 만들지 않고 기존 작은 fireball 프레임을 대상 위에서만 재생합니다.
        /// 작은 불꽃의 중간 시점에 저장된 화상 피해를 적용한 뒤 피격 반응까지 보여 줍니다.
        /// </summary>
        public IEnumerator PlayBurnTick(RectTransform target, Image targetSprite, Font damageFont, Sprite[] flameFrames,
            Func<int> applyTick, Action<int> onImpact, Action onComplete)
        {
            if (target == null || targetSprite == null)
            {
                int fallbackDamage = applyTick == null ? 0 : applyTick();
                onImpact?.Invoke(fallbackDamage);
                onComplete?.Invoke();
                yield break;
            }

            Image flame = CreateEffectImage(target, "BurnTickFlame", flameFrames,
                BattleFireballVisuals.BurnTickSize, new Vector2(0f, 12f));
            Color originalColor = targetSprite.color;
            bool applied = false;
            int count = flameFrames?.Length ?? 0;
            for (int index = 0; index < count; index++)
            {
                if (flame != null) flame.sprite = flameFrames[index];
                if (!applied && index >= 3)
                {
                    applied = true;
                    int damage = applyTick == null ? 0 : applyTick();
                    onImpact?.Invoke(damage);
                    if (damage > 0)
                    {
                        StartCoroutine(ShowDamageNumber(target, damageFont, damage));
                        StartCoroutine(PlayHitReaction(target, targetSprite, target.localPosition, originalColor));
                    }
                }
                yield return new WaitForSeconds(.06f);
            }
            if (!applied)
            {
                int damage = applyTick == null ? 0 : applyTick();
                onImpact?.Invoke(damage);
            }
            if (flame != null) Destroy(flame.gameObject);
            targetSprite.color = originalColor;
            onComplete?.Invoke();
        }

        /// <summary>
        /// 짧은 청백색 예고 뒤 모든 대상에서 electric-impact를 같은 프레임 번호로 재생합니다. 대상마다
        /// 코루틴을 따로 시작하면 프레임 시간에 따라 타격 순서가 벌어질 수 있으므로, 하나의 반복문이 모든
        /// Image를 함께 갱신합니다. Peak index 1에서 계산 콜백도 한 번만 호출해 화면의 가장 강한 섬광과
        /// 실제 HP 감소가 일치하도록 합니다.
        /// </summary>
        public IEnumerator PlayThunderboltAttack(RectTransform attacker, IReadOnlyList<RectTransform> targets,
            IReadOnlyList<Image> targetSprites, Font damageFont, Sprite[] impactFrames,
            Func<IReadOnlyList<int>> applyImpacts, Action<IReadOnlyList<int>> onImpact, Action onComplete)
        {
            if (targets == null || targetSprites == null || targets.Count == 0)
            {
                IReadOnlyList<int> fallback = applyImpacts == null ? Array.Empty<int>() : applyImpacts();
                onImpact?.Invoke(fallback);
                onComplete?.Invoke();
                yield break;
            }

            Text callout = CreateSkillCallout(attacker, damageFont, "썬더볼트!");
            List<Image> telegraphs = new List<Image>();
            Sprite[] telegraphFrame = { GetOrbSprite() };
            for (int index = 0; index < targets.Count; index++)
            {
                Image flash = CreateEffectImage(targets[index], "ThunderboltTelegraph", telegraphFrame,
                    BattleThunderboltVisuals.TelegraphSize, new Vector2(0f, 18f));
                if (flash != null)
                {
                    flash.color = new Color(.58f, .88f, 1f, .82f);
                    telegraphs.Add(flash);
                }
            }
            yield return new WaitForSeconds(.14f);
            foreach (Image flash in telegraphs) if (flash != null) Destroy(flash.gameObject);
            if (callout != null) Destroy(callout.gameObject);

            List<Image> impacts = new List<Image>();
            Color[] originalColors = new Color[targetSprites.Count];
            for (int index = 0; index < targets.Count; index++)
            {
                impacts.Add(CreateEffectImage(targets[index], "ElectricImpact", impactFrames,
                    BattleThunderboltVisuals.ImpactSize, new Vector2(0f, 18f)));
                originalColors[index] = targetSprites[index] == null ? Color.white : targetSprites[index].color;
            }

            bool applied = false;
            int frameCount = impactFrames?.Length ?? 0;
            for (int frameIndex = 0; frameIndex < frameCount; frameIndex++)
            {
                foreach (Image impact in impacts) if (impact != null) impact.sprite = impactFrames[frameIndex];
                if (!applied && frameIndex >= BattleThunderboltVisuals.PeakFrame)
                {
                    applied = true;
                    // 한 번 받은 피해 배열을 같은 순서의 대상에 배분하여 3~6명의 HP가 거의 동시에 줄고,
                    // 각 피격 반응도 같은 Peak 프레임에서 시작합니다.
                    IReadOnlyList<int> damages = applyImpacts == null ? Array.Empty<int>() : applyImpacts();
                    onImpact?.Invoke(damages);
                    for (int index = 0; index < targets.Count; index++)
                    {
                        int damage = index < damages.Count ? damages[index] : 0;
                        if (damage <= 0 || targetSprites[index] == null) continue;
                        StartCoroutine(ShowDamageNumber(targets[index], damageFont, damage));
                        StartCoroutine(PlayHitReaction(targets[index], targetSprites[index],
                            targets[index].localPosition, originalColors[index]));
                    }
                }
                yield return new WaitForSeconds(BattleThunderboltVisuals.FrameDuration);
            }
            if (!applied)
            {
                IReadOnlyList<int> damages = applyImpacts == null ? Array.Empty<int>() : applyImpacts();
                onImpact?.Invoke(damages);
            }

            foreach (Image impact in impacts) if (impact != null) Destroy(impact.gameObject);
            for (int index = 0; index < targetSprites.Count; index++)
                if (targetSprites[index] != null) targetSprites[index].color = originalColors[index];
            // 14프레임 재생과 피격 반응이 모두 끝난 뒤에만 Controller가 다음 턴으로 넘어갑니다.
            yield return new WaitForSeconds(.04f);
            onComplete?.Invoke();
        }

        /// <summary>
        /// 가이아 웰은 Projectile 없이 사용자 위치에서 arcane-parry를 한 번 재생합니다. VFX는 상태가
        /// 적용되었다는 시각 안내만 담당하며, 실제 60% 감소와 2회 수명은 applyEffect가 연결한 전투
        /// 상태 저장소가 계산합니다. 이렇게 나누면 프레임 속도나 크기를 바꿔도 전투 수치가 변하지 않습니다.
        /// </summary>
        public IEnumerator PlayGaiaWall(RectTransform actor, Font font, Sprite[] frames,
            Action applyEffect, Action onComplete)
        {
            if (actor == null || frames == null || frames.Length == 0)
            {
                applyEffect?.Invoke();
                onComplete?.Invoke();
                yield break;
            }

            Text callout = CreateSkillCallout(actor, font, "가이아 웰!");
            Image barrier = CreateEffectImage(actor, "ArcaneParry", frames,
                BattleGaiaWallVisuals.EffectSize, new Vector2(0f, 12f));
            bool applied = false;
            for (int frameIndex = 0; frameIndex < frames.Length; frameIndex++)
            {
                if (barrier != null) barrier.sprite = frames[frameIndex];
                if (!applied && frameIndex >= BattleGaiaWallVisuals.PeakFrame)
                {
                    applied = true;
                    // 보호막이 가장 분명한 peak index 8과 상태 적용 시점을 맞춰 사용자가 효과 발생을
                    // 눈으로 확인할 수 있게 합니다. 단, 수치 계산 자체는 Presenter가 알지 않습니다.
                    applyEffect?.Invoke();
                }
                yield return new WaitForSeconds(BattleGaiaWallVisuals.FrameDuration);
            }
            if (!applied) applyEffect?.Invoke();
            if (barrier != null) Destroy(barrier.gameObject);
            if (callout != null) Destroy(callout.gameObject);
            onComplete?.Invoke();
        }

        private static Image CreateEffectImage(RectTransform parent, string objectName, Sprite[] frames,
            Vector2 size, Vector2 anchoredPosition)
        {
            if (parent == null || frames == null || frames.Length == 0) return null;
            GameObject obj = new GameObject(objectName, typeof(Image));
            obj.transform.SetParent(parent, false);
            Image image = obj.GetComponent<Image>();
            image.sprite = frames[0];
            image.preserveAspect = true;
            image.raycastTarget = false;
            RectTransform rect = image.rectTransform;
            rect.anchorMin = rect.anchorMax = rect.pivot = Vector2.one * .5f;
            rect.sizeDelta = size;
            rect.anchoredPosition = anchoredPosition;
            return image;
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
        /// 상승 화살과 여러 대상 위의 낙하 화살을 하나의 연출로 관리합니다. 화면의 화살들은 모두 장식이고
        /// 실제 피해는 낙하가 끝난 뒤 applyImpacts를 딱 한 번 호출해 대상별 한 번만 계산합니다. 여러 개의
        /// 단일 Projectile 코루틴을 따로 실행하면 각 코루틴이 피해·완료·다음 턴을 반복할 수 있으므로,
        /// 광역 행동 전체가 끝나는 시점을 이 메서드 하나가 책임집니다.
        /// </summary>
        public IEnumerator PlayProjectileVolleyAttack(RectTransform attacker, Image attackerSprite,
            IReadOnlyList<RectTransform> targets, IReadOnlyList<Image> targetSprites, Font damageFont,
            Sprite[] projectileFrames, float frameDuration, Func<IReadOnlyList<int>> applyImpacts,
            Action<IReadOnlyList<int>> onImpact, Action onComplete)
        {
            if (attacker == null || attackerSprite == null || targets == null || targetSprites == null)
            {
                IReadOnlyList<int> fallback = applyImpacts == null ? Array.Empty<int>() : applyImpacts();
                onImpact?.Invoke(fallback);
                onComplete?.Invoke();
                yield break;
            }

            Color attackerOriginalColor = attackerSprite.color;
            Text callout = CreateSkillCallout(attacker, damageFont, "화살비!");

            // 조준은 입력을 요구하는 구간이 아니라 짧은 시각적 예고입니다. 이 시간에도 Controller의
            // actionPlaying이 유지되어 플레이어가 같은 행동을 중복 입력할 수 없습니다.
            const float aimDuration = .1f;
            float elapsed = 0f;
            while (elapsed < aimDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / aimDuration);
                attackerSprite.color = Color.Lerp(attackerOriginalColor,
                    new Color(1f, .88f, .42f, attackerOriginalColor.a), Mathf.Sin(t * Mathf.PI));
                yield return null;
            }

            RectTransform projectileParent = attacker.parent as RectTransform;
            List<Image> risingArrows = new List<Image>();
            float[] risingOffsets = { -20f, 0f, 20f };
            for (int index = 0; index < risingOffsets.Length; index++)
            {
                Vector3 start = attacker.localPosition + new Vector3(risingOffsets[index], 18f, 0f);
                risingArrows.Add(CreateVolleyProjectile(projectileParent, projectileFrames, start, 90f));
            }

            const float riseDuration = .13f;
            elapsed = 0f;
            while (elapsed < riseDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / riseDuration);
                for (int index = 0; index < risingArrows.Count; index++)
                {
                    Image arrow = risingArrows[index];
                    if (arrow == null) continue;
                    Vector3 start = attacker.localPosition + new Vector3(risingOffsets[index], 18f, 0f);
                    arrow.rectTransform.localPosition = Vector3.Lerp(start, start + Vector3.up * 220f, t);
                    SetProjectileFrame(arrow, projectileFrames, elapsed, frameDuration);
                }
                yield return null;
            }
            foreach (Image arrow in risingArrows) if (arrow != null) Destroy(arrow.gameObject);

            yield return new WaitForSeconds(.16f);

            // 대상마다 세 발을 만들되 X 위치와 시작 시점을 조금씩 다르게 합니다. 화살 수는 순수 연출이며
            // 아래의 applyImpacts 호출 횟수나 BattleSkillExecutor의 피해량에는 관여하지 않습니다.
            List<Image> fallingArrows = new List<Image>();
            List<Vector3> fallStarts = new List<Vector3>();
            List<Vector3> fallEnds = new List<Vector3>();
            List<float> fallDelays = new List<float>();
            float[] fallOffsets = { -22f, 0f, 22f };
            int targetCount = Mathf.Min(targets.Count, targetSprites.Count);
            for (int targetIndex = 0; targetIndex < targetCount; targetIndex++)
            {
                if (targets[targetIndex] == null || targetSprites[targetIndex] == null) continue;
                for (int arrowIndex = 0; arrowIndex < fallOffsets.Length; arrowIndex++)
                {
                    Vector3 end = targets[targetIndex].localPosition + new Vector3(fallOffsets[arrowIndex], 20f, 0f);
                    Vector3 start = end + Vector3.up * (175f + arrowIndex * 8f);
                    Image arrow = CreateVolleyProjectile(projectileParent, projectileFrames, start, -90f);
                    if (arrow != null) arrow.gameObject.SetActive(arrowIndex == 0);
                    fallingArrows.Add(arrow);
                    fallStarts.Add(start);
                    fallEnds.Add(end);
                    fallDelays.Add(arrowIndex * .03f);
                }
            }

            const float fallDuration = .18f;
            elapsed = 0f;
            while (elapsed < fallDuration)
            {
                elapsed += Time.deltaTime;
                for (int index = 0; index < fallingArrows.Count; index++)
                {
                    Image arrow = fallingArrows[index];
                    if (arrow == null || elapsed < fallDelays[index]) continue;
                    if (!arrow.gameObject.activeSelf) arrow.gameObject.SetActive(true);
                    float availableDuration = Mathf.Max(.01f, fallDuration - fallDelays[index]);
                    float t = Mathf.Clamp01((elapsed - fallDelays[index]) / availableDuration);
                    arrow.rectTransform.localPosition = Vector3.Lerp(fallStarts[index], fallEnds[index],
                        1f - Mathf.Pow(1f - t, 3f));
                    SetProjectileFrame(arrow, projectileFrames, elapsed - fallDelays[index], frameDuration);
                }
                yield return null;
            }
            foreach (Image arrow in fallingArrows) if (arrow != null) Destroy(arrow.gameObject);

            // 모든 낙하가 끝난 공유 타격 시점에 계산을 한 번 실행합니다. 반환된 배열의 한 원소가 한 대상의
            // 120% 피해이며, 이 시점에 HP HUD도 한 번 갱신됩니다.
            IReadOnlyList<int> damages = applyImpacts == null ? Array.Empty<int>() : applyImpacts();
            onImpact?.Invoke(damages);

            Vector3[] targetOrigins = new Vector3[targetCount];
            Color[] targetOriginalColors = new Color[targetCount];
            for (int index = 0; index < targetCount; index++)
            {
                if (targets[index] == null || targetSprites[index] == null) continue;
                targetOrigins[index] = targets[index].localPosition;
                targetOriginalColors[index] = targetSprites[index].color;
                if (index < damages.Count && damages[index] > 0)
                    StartCoroutine(ShowDamageNumber(targets[index], damageFont, damages[index]));
            }

            const float reactionDuration = .18f;
            elapsed = 0f;
            while (elapsed < reactionDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / reactionDuration);
                float shake = Mathf.Sin(t * Mathf.PI * 6f) * (1f - t) * 10f;
                for (int index = 0; index < targetCount; index++)
                {
                    if (targets[index] == null || targetSprites[index] == null) continue;
                    targets[index].localPosition = targetOrigins[index] + Vector3.right * shake;
                    Color flash = new Color(targetOriginalColors[index].r, targetOriginalColors[index].g,
                        targetOriginalColors[index].b, .35f);
                    targetSprites[index].color = t < .55f
                        ? flash : Color.Lerp(flash, targetOriginalColors[index], (t - .55f) / .45f);
                }
                yield return null;
            }

            for (int index = 0; index < targetCount; index++)
            {
                if (targets[index] == null || targetSprites[index] == null) continue;
                targets[index].localPosition = targetOrigins[index];
                targetSprites[index].color = targetOriginalColors[index];
            }
            attackerSprite.color = attackerOriginalColor;
            if (callout != null) Destroy(callout.gameObject);
            // 상승·대기·낙하·모든 피격 반응이 끝난 뒤 완료를 한 번만 알리므로 다음 턴도 한 번만 진행됩니다.
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

        /// <summary>원본 golden_arrow Sprite는 그대로 두고 개별 UI 인스턴스의 회전만 바꿉니다.</summary>
        private static Image CreateVolleyProjectile(RectTransform parent, Sprite[] frames, Vector3 position, float angle)
        {
            if (parent == null) return null;
            GameObject projectileObject = new GameObject("BattleVolleyProjectile", typeof(Image));
            projectileObject.transform.SetParent(parent, false);
            Image image = projectileObject.GetComponent<Image>();
            image.sprite = frames != null && frames.Length > 0 ? frames[0] : null;
            image.preserveAspect = true;
            image.raycastTarget = false;
            RectTransform rect = image.rectTransform;
            rect.anchorMin = rect.anchorMax = rect.pivot = Vector2.one * .5f;
            rect.sizeDelta = new Vector2(52f, 52f);
            rect.localPosition = position;
            rect.localRotation = Quaternion.Euler(0f, 0f, angle);
            return image;
        }

        private static void SetProjectileFrame(Image image, Sprite[] frames, float elapsed, float frameDuration)
        {
            if (image == null || frames == null || frames.Length == 0 || frameDuration <= 0f) return;
            image.sprite = frames[Mathf.FloorToInt(Mathf.Max(0f, elapsed) / frameDuration) % frames.Length];
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
