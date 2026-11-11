using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.World;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.Battle
{
    /// <summary>미승인 감소/재적용/AI 정책을 제품 기본값으로 만들지 않는 Main24 격리 연결입니다.</summary>
    public sealed partial class BattleSceneController
    {
        bool owlChillPilotArmed, owlChillPilotApplied;
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
            if (!Main24FrostOwlCandidate.IsEncounter || owlChillPilotArmed) return false;
            // Stage2 B 격리 기준: 기회 경과와 실제 행동 완료 분리, 자연 만료 뒤 정상 행동 보호.
            if (!statusEffects.ConfigureColdForAudit(new ColdStatusCandidateRules(ColdReapplicationRule.RejectWhileActive, null, false, true))) return false;
            owlChillPilotArmed = true;
            var title = GameObject.Find("BattleCanvas").transform.Find("Title");
            if (title != null) title.GetComponent<Text>().text = "서리올빼미 · 한기 시험";
            return true;
        }
#endif
        void ApplyOwlFirstChillAtImpact(Combatant actor, Combatant target, int damage)
        {
            // 확률/반복 주기/추가 피해를 만들지 않고 첫 기존 기본공격의 유효 타격에만 연결합니다.
            if (!owlChillPilotArmed || owlChillPilotApplied || !Main24FrostOwlCandidate.IsEncounter || actor.Side != BattleSide.Enemies || damage <= 0 || !target.IsAlive) return;
            if (statusEffects.TryApplyChill(target)) owlChillPilotApplied = true;
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
        void LateUpdate(){if(owlChillPilotArmed)SyncOwlChillVfx();}
        void SyncOwlChillVfx()
        {
            if (!owlChillPilotArmed || combatantViews == null) return;
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
