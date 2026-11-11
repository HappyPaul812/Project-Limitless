using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.World;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.Battle
{
    /// <summary>승인된 F2 Owl 1회 한기 패턴과 기존 Editor 감사 경로를 분리하고 B 기회 경과를 연결합니다.</summary>
    public sealed partial class BattleSceneController
    {
        bool owlChillPilotArmed, owlChillPilotApplied;
        // 사용 기록은 전투 인스턴스에만 둡니다. 명중 실패나 부여 거절도 시도 1회로 끝냅니다.
        bool owlChillProductionEnabled, owlChillAttempted, owlChillImpactHandled;
        Combatant owlChillAttacker, owlChillTarget;
        // 기회 번호로 정상/차단 종료를 한 번만 처리합니다. 다른 인물의 완료가 보호를 소비하지 않습니다.
        bool opportunityOpen, activeOpportunityBlocked;
        int activeOpportunityId, endingOpportunityId = -1, completedOpportunityId = -1;
        int burnOpportunityId = -1, poisonOpportunityId = -1;
        Vector2 owlVfxScreenSize;
        readonly Dictionary<Combatant, Image> owlChillEffects = new Dictionary<Combatant, Image>();
#if UNITY_EDITOR
        /// <summary>격리 검사기가 실제 F2 후보 전투에서만 명시적으로 켭니다. 원본/일반 전투는 기본 OFF입니다.</summary>
        public bool ConfigureOwlFirstChillPilot()
        {
            if (owlChillProductionEnabled || !Main24FrostOwlCandidate.IsEncounter || owlChillPilotArmed) return false;
            // Stage2 B 격리 기준: 기회 경과와 실제 행동 완료 분리, 자연 만료 뒤 정상 행동 보호.
            if (!statusEffects.ConfigureColdForAudit(new ColdStatusCandidateRules(ColdReapplicationRule.RejectWhileActive, null, false, true))) return false;
            owlChillPilotArmed = true;
            var title = GameObject.Find("BattleCanvas").transform.Find("Title");
            if (title != null) title.GetComponent<Text>().text = "서리올빼미 · 한기 시험";
            return true;
        }
#endif
        bool CanUseOwlFirstChill(Combatant actor)
        {
            return owlChillProductionEnabled && !owlChillAttempted && actor != null && actor.IsAlive
                && participantSetups.TryGetValue(actor, out var setup) && Main24FrostOwlCandidate.IsDefinition(setup.MonsterDefinition);
        }
        IEnumerator PlayOwlFirstChillAttack(Combatant actor, Combatant target)
        {
            // 기존 EnemyAction이 고른 대상과 기존 기본공격 피해 경로를 그대로 사용합니다.
            // 예고는 시간 제한 없는 기존 선택 UI라 빠른 반응이나 회피 입력을 요구하지 않습니다.
            owlChillAttempted = true; owlChillAttacker = actor; owlChillTarget = target; owlChillImpactHandled = false;
            actionPlaying = true; SetCommandButtons(false); SetCancelButtonVisible(false);
            messageText.text = $"서리올빼미: 한기 공격 예고 · {target.DisplayName} 적중 시 행동 2회 불가";
            while (WorldModalState.IsOpen && !battleEnded) yield return null;
            if (battleEnded) yield break;
            bool acknowledged = false;
            Main21ChoicePanel.Show($"한기 공격 예고\n{target.DisplayName}: 맞으면 자기 행동 2회 동안 행동할 수 없습니다.",
                new[] { "확인하고 계속" }, _ => acknowledged = true);
            while (!acknowledged && !battleEnded) yield return null;
            if (battleEnded) yield break;
            if (!actor.IsAlive || !target.IsAlive) { actionPlaying = false; FinishCurrentAction(); yield break; }
            PlayBasicAttack(actor, target); // 명중/피해/방어/보호자 판정을 새로 만들지 않습니다.
        }
        void ApplyOwlFirstChillAtImpact(Combatant actor, Combatant target, int damage)
        {
            if (owlChillProductionEnabled)
            {
                if (!owlChillAttempted || owlChillImpactHandled || actor != owlChillAttacker || target != owlChillTarget) return;
                owlChillImpactHandled = true; // 피해 0/KO/재부여 보호에도 후속 일반공격에서 다시 시도하지 않습니다.
                if (damage > 0 && target.IsAlive) statusEffects.TryApplyChill(target);
                return;
            }
            // 기존 Editor 감사 arm은 정식 패턴과 분리하고 자동 활성화하지 않습니다.
            if (!owlChillPilotArmed || owlChillPilotApplied || !Main24FrostOwlCandidate.IsEncounter || actor.Side != BattleSide.Enemies || damage <= 0 || !target.IsAlive) return;
            if (statusEffects.TryApplyChill(target)) owlChillPilotApplied = true;
        }
        void ResetOwlFirstChillPattern()
        {
            owlChillProductionEnabled = false; owlChillAttempted = false; owlChillImpactHandled = false;
            owlChillAttacker = null; owlChillTarget = null; owlChillPilotArmed = false; owlChillPilotApplied = false;
            foreach (var effect in owlChillEffects.Values) if (effect != null) effect.gameObject.SetActive(false);
        }
        bool TrySkipOwlChillOpportunity()
        {
            if (!statusEffects.TryBeginColdBlockedOpportunity(currentActor)) return false;
            activeOpportunityBlocked = true;
            actionPlaying = true; SetCommandButtons(false); SetCancelButtonVisible(false);
            messageText.text = $"{currentActor.DisplayName}: 한기 {statusEffects.GetChillRemaining(currentActor)}회 · 행동불가";
            UpdateTimeline(); RefreshCombatantViews(null);
            StartCoroutine(CompleteOwlChillOpportunity(currentActor)); return true;
        }
        IEnumerator CompleteOwlChillOpportunity(Combatant actor)
        {
            // 입력 경쟁 없이 안내 연출 후 자동으로 건너뜁니다. 1초는 상태 수명이 아닌 격리 표시 시간입니다.
            // 남은 횟수는 다음 인물의 시간 제한 없는 선택 중에도 HUD에 남습니다.
            yield return new WaitForSecondsRealtime(1f);
            if (battleEnded) yield break;
            if (!TryStartOpportunityEnd(actor)) yield break;
            int token = activeOpportunityId;
            statusEffects.CompleteOpportunityDurations(actor, false);
            PlayOpportunityEndEffects(actor, true, token);
        }
        bool TryStartOpportunityEnd(Combatant actor)
        {
            if (battleEnded || !opportunityOpen || actor != currentActor || endingOpportunityId == activeOpportunityId) return false;
            endingOpportunityId = activeOpportunityId; return true;
        }
        void CompleteOpportunityEnd(Combatant actor, bool blocked, int token)
        {
            if (battleEnded || token != activeOpportunityId || completedOpportunityId == token) return;
            completedOpportunityId = token;
            if (blocked) statusEffects.CompleteColdBlockedOpportunity(actor);
            else statusEffects.NotifyNormalActionCompleted(actor);
            statusEffects.RemoveInvalidPersistentEffects(AllCombatants);statusEffects.RemoveInvalidColdStates();
            opportunityOpen = false; actionPlaying = false; RefreshCombatantViews(null);
            StartCoroutine(AdvanceAfterDelay());
        }
        // 해상도 변경에만 가시 알파 간격을 다시 계산합니다. PNG·캐릭터 진형/위치는 바꾸지 않습니다.
        void LateUpdate(){if(owlChillPilotArmed || owlChillProductionEnabled)SyncOwlChillVfx();}
        void SyncOwlChillVfx()
        {
            if ((!owlChillPilotArmed && !owlChillProductionEnabled) || combatantViews == null) return;
            var size = new Vector2(Screen.width, Screen.height);bool resized = size != owlVfxScreenSize;owlVfxScreenSize = size;
            foreach (var actor in allies.Members)
            {
                bool active = statusEffects.GetChillRemaining(actor) > 0;
                if (!owlChillEffects.TryGetValue(actor, out var effect) && active)
                {
                    var body = combatantViews[actor].SpriteImage;
                    effect = MakeImage(body.transform.parent, "PilotChillVfx", Color.white);
                    effect.sprite = BattleApprovedColdAssets.Vfx("cold.chill");
                    BattleApprovedColdAssets.PlaceVfx(body, effect, 128, null);
                    PlaceStage2ChillEffect(body, effect, actor);
                    owlChillEffects.Add(actor, effect);
                }
                if (effect != null) {effect.gameObject.SetActive(active);if(active&&resized)PlaceStage2ChillEffect(combatantViews[actor].SpriteImage,effect,actor);}
            }
        }
        void PlaceStage2ChillEffect(Image body, Image effect, Combatant owner)
        {
            BattleApprovedColdAssets.PlaceVfx(body,effect,128,null);
            effect.transform.SetAsLastSibling(); // 해당 ActionRoot 내부 전경만, HUD/다른 캐릭터 전역 정렬은 유지합니다.
            Rect b=BattleApprovedColdAssets.Visible(body,null),v=BattleApprovedColdAssets.Visible(effect,null);
            // 한쪽으로 계속 밀어 화면 밖으로 나가지 않도록 좌/우 후보 중 가장 가까운 빈 곳을 고릅니다.
            Rect desired=v;desired.y=b.yMin-8-v.height;
            var obstacles=new List<Rect>();
            foreach(var pair in combatantViews.Where(x=>x.Key!=owner&&x.Key.IsAlive))
            {
                Rect other=BattleApprovedColdAssets.Visible(pair.Value.SpriteImage,null);
                obstacles.Add(Rect.MinMaxRect(other.xMin-8,other.yMin-8,other.xMax+8,other.yMax+8));
            }
            var candidates=new List<Rect>{desired};
            foreach(var obstacle in obstacles)
            {var left=desired;left.x=obstacle.xMin-left.width;candidates.Add(left);var right=desired;right.x=obstacle.xMax;candidates.Add(right);}
            var corners=new Vector3[4];battlefieldTransform.GetComponent<RectTransform>().GetWorldCorners(corners);
            Rect best=desired;float distance=float.PositiveInfinity;
            foreach(var candidate in candidates)
            {
                if(candidate.xMin<8||candidate.xMax>Screen.width-8||candidate.yMin<corners[0].y+8||candidate.yMax>corners[2].y-8||obstacles.Any(x=>candidate.Overlaps(x)))continue;
                float d=(candidate.position-v.position).sqrMagnitude;if(d<distance){distance=d;best=candidate;}
            }
            Vector2 move=best.position-v.position;
            effect.rectTransform.anchoredPosition+=move/body.canvas.scaleFactor;
        }
    }
}
