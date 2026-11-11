using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectLimitless.Battle
{
    // 전투 메모리와 표시를 위한 종류이며 Save/몬스터/상품의 정식 ID를 추가하지 않습니다.
    public enum ColdStatusKind { Chill, Frostbite, FrozenNecrosis }
    public enum ColdStatusChange { Applied, OpportunitySpent, Damage, Removed, KnockedOut }
    public enum ColdReapplicationRule { RejectWhileActive, RefreshToTwo }

    /// <summary>
    /// 미정 규칙을 공식 기본값으로 만들지 않기 위한 격리 후보 정책입니다. 명시적으로 주입하지 않으면
    /// 냉기 부여는 HOLD입니다. 후보는 차단된 기회 끝에 틱→남은 횟수 감소→만료 순서로 처리하고,
    /// 기존 처리 경계는 Controller가 정책별로 분리합니다. 보호 옵션은 명시적 Stage2 후보에만 켭니다.
    /// 동상의 HP 피해량 null은 피해 규칙 HOLD, 숫자가 있으면 그 값만 사용합니다.
    /// </summary>
    public sealed class ColdStatusCandidateRules
    {
        public ColdStatusCandidateRules(ColdReapplicationRule reapplication, int? frostbiteDamage, bool frostbiteUsesDefending, bool protectNaturalChillRecovery = false)
        {
            if(!Enum.IsDefined(typeof(ColdReapplicationRule),reapplication))throw new ArgumentOutOfRangeException(nameof(reapplication));
            if(frostbiteDamage.HasValue&&frostbiteDamage.Value<=0)throw new ArgumentOutOfRangeException(nameof(frostbiteDamage));
            Reapplication=reapplication;FrostbiteDamage=frostbiteDamage;FrostbiteUsesDefending=frostbiteUsesDefending;ProtectNaturalChillRecovery=protectNaturalChillRecovery;
        }
        public ColdReapplicationRule Reapplication { get; }
        public int? FrostbiteDamage { get; }
        public bool FrostbiteUsesDefending { get; }
        public bool ProtectNaturalChillRecovery { get; }
    }

    /// <summary>
    /// 기존 상태 저장소를 옮기지 않고 냉기 세 종류만 분리해 확장합니다. 객체 참조 기준이므로 같은
    /// 몬스터 ID 두 개도 상태가 섞이지 않습니다. 정화Ⅰ의 공용 HarmfulStatus 목록에는 넣지 않습니다.
    /// </summary>
    public sealed partial class BattleStatusEffectRuntime
    {
        readonly Dictionary<Combatant,int> chills=new Dictionary<Combatant,int>(CombatantReferenceComparer.Instance);
        readonly Dictionary<Combatant,int> frostbites=new Dictionary<Combatant,int>(CombatantReferenceComparer.Instance);
        readonly Dictionary<Combatant,int> necroses=new Dictionary<Combatant,int>(CombatantReferenceComparer.Instance);
        readonly HashSet<Combatant> blockedColdOpportunities=new HashSet<Combatant>(CombatantReferenceComparer.Instance);
        readonly HashSet<Combatant> completingColdOpportunities=new HashSet<Combatant>(CombatantReferenceComparer.Instance);
        // 자연 만료 뒤 정상 행동을 마칠 때까지 한기 재부여만 막습니다. 피해/다른 상태 면역이 아닙니다.
        readonly HashSet<Combatant> chillRecoveryProtection=new HashSet<Combatant>(CombatantReferenceComparer.Instance);
        bool clearingColdStatuses;
        ColdStatusCandidateRules coldRules;
        public bool HasChillRecoveryProtection(Combatant actor)=>actor!=null&&actor.IsAlive&&chillRecoveryProtection.Contains(actor);
        public void NotifyNormalActionCompleted(Combatant actor){if(actor!=null)chillRecoveryProtection.Remove(actor);}
        // 기존 무인자 생성 계약도 유지해 기존 호출자/리플렉션 생성에 영향을 주지 않습니다.
        public BattleStatusEffectRuntime() { }
        public BattleStatusEffectRuntime(ColdStatusCandidateRules candidateRules){coldRules=candidateRules;}
        public bool HasColdCandidateRules=>coldRules!=null;
        // 마지막 값은 Applied/OpportunitySpent에는 남은 횟수·중첩, Damage에는 실제 HP 감소량입니다.
        // 표현 계층은 이 이벤트를 구독할 수 있지만 이번에는 미승인 VFX를 생성하지 않습니다.
        public event Action<Combatant,ColdStatusKind,ColdStatusChange,int> ColdStatusChanged;

#if UNITY_EDITOR
        /// <summary>실제 Battle Controller의 격리 시험만 정책을 넣습니다. 진행 중 정책 교체는 거절합니다.</summary>
        public bool ConfigureColdForAudit(ColdStatusCandidateRules rules)
        {
            if(clearingColdStatuses||chills.Count+frostbites.Count+necroses.Count+blockedColdOpportunities.Count+completingColdOpportunities.Count+chillRecoveryProtection.Count!=0)return false;
            coldRules=rules;return true;
        }
#endif
        static int Remaining(Dictionary<Combatant,int> states,Combatant target)=>target!=null&&target.IsAlive&&states.TryGetValue(target,out int n)?n:0;
        public int GetChillRemaining(Combatant target)=>Remaining(chills,target);
        public int GetFrostbiteRemaining(Combatant target)=>Remaining(frostbites,target);
        public int GetFrozenNecrosisStacks(Combatant target)=>Remaining(necroses,target);
        public bool IsColdActionBlocked(Combatant actor)=>GetChillRemaining(actor)>0||GetFrostbiteRemaining(actor)>0;
        public bool IsFrostbiteDamageOnHold=>coldRules?.FrostbiteDamage==null;

        public bool TryApplyChill(Combatant target)=>ApplyColdDuration(chills,target,ColdStatusKind.Chill);
        public bool TryApplyFrostbite(Combatant target)=>ApplyColdDuration(frostbites,target,ColdStatusKind.Frostbite);
        bool ApplyColdDuration(Dictionary<Combatant,int> states,Combatant target,ColdStatusKind kind)
        {
            if(clearingColdStatuses||coldRules==null||target==null||!target.IsAlive)return false;
            if(kind==ColdStatusKind.Chill&&HasChillRecoveryProtection(target))return false;
            // 이미 처리 중인 기회에 새 지속시간을 섞는 규칙은 아직 미정이므로 이 후보에서는 거절합니다.
            if(blockedColdOpportunities.Contains(target))return false;
            if(states.ContainsKey(target)&&coldRules.Reapplication==ColdReapplicationRule.RejectWhileActive)return false;
            states[target]=2;ColdStatusChanged?.Invoke(target,kind,ColdStatusChange.Applied,2);return true;
        }

        /// <summary>실제 큐가 선택한 자기 기회에서만 시작합니다. 메뉴/취소나 중복 완료는 횟수를 줄이지 않습니다.</summary>
        public bool TryBeginColdBlockedOpportunity(Combatant actor)=>IsColdActionBlocked(actor)&&blockedColdOpportunities.Add(actor);
        public bool CompleteColdBlockedOpportunity(Combatant actor)
        {
            if(actor==null||!blockedColdOpportunities.Contains(actor)||!completingColdOpportunities.Add(actor))return false;
            try
            {
                // HP/표시 알림 콜백이 처리 중 재부여/완료를 요청해도 한 기회의 틱·횟수는 한 번만 적용합니다.
                if(actor.IsAlive&&GetFrostbiteRemaining(actor)>0&&coldRules?.FrostbiteDamage!=null)
                    ApplyFrostbiteDamage(actor,coldRules.FrostbiteDamage.Value,coldRules.FrostbiteUsesDefending);
                SpendColdOpportunity(chills,actor,ColdStatusKind.Chill);
                SpendColdOpportunity(frostbites,actor,ColdStatusKind.Frostbite);
                RemoveInvalidColdStates();return true;
            }
            finally {blockedColdOpportunities.Remove(actor);completingColdOpportunities.Remove(actor);}
        }
        void SpendColdOpportunity(Dictionary<Combatant,int> states,Combatant actor,ColdStatusKind kind)
        {
            if(!actor.IsAlive||!states.TryGetValue(actor,out int n))return;
            n--;if(n==0){states.Remove(actor);if(kind==ColdStatusKind.Chill&&coldRules.ProtectNaturalChillRecovery)chillRecoveryProtection.Add(actor);}else states[actor]=n;
            ColdStatusChanged?.Invoke(actor,kind,ColdStatusChange.OpportunitySpent,n);
            if(n==0)ColdStatusChanged?.Invoke(actor,kind,ColdStatusChange.Removed,0);
        }

        /// <summary>
        /// HP 감소는 행동불가 소비와 별도 경계입니다. 피해량/방어 적용은 호출자가 명시해야 하며,
        /// 독·화상 틱/직접공격의 엄호 처리를 복사하지 않습니다. 양과 시점·경감은 후보 가정입니다.
        /// </summary>
        public int ApplyFrostbiteDamage(Combatant target,int damage,bool applyDefending)
        {
            if(GetFrostbiteRemaining(target)==0||damage<=0)return 0;
            int before=target.CurrentHp;target.TakeDamage(damage,applyDefending);int lost=before-target.CurrentHp;
            ColdStatusChanged?.Invoke(target,ColdStatusKind.Frostbite,ColdStatusChange.Damage,lost);
            RemoveInvalidColdStates();return lost;
        }

        /// <summary>
        /// 동결괴사는 살아 있는 적 보스만 아군에게 부여합니다. 세 번째는 방어/엄호/과열 폭발을
        /// 거치지 않고 기존 TakeDamage·HpChanged·IsAlive 경계로 즉시 KO를 만듭니다. 3/3은 저장하지 않습니다.
        /// </summary>
        public int ApplyFrozenNecrosis(Combatant source,Combatant target)
        {
            if(coldRules==null||source==null||!source.IsAlive||!source.IsBoss||source.Side!=BattleSide.Enemies||target==null||!target.IsAlive||target.Side!=BattleSide.Allies)return 0;
            int next=GetFrozenNecrosisStacks(target)+1;
            if(next<3){necroses[target]=next;ColdStatusChanged?.Invoke(target,ColdStatusKind.FrozenNecrosis,ColdStatusChange.Applied,next);return next;}
            target.TakeDamage(target.CurrentHp,false);
            ColdStatusChanged?.Invoke(target,ColdStatusKind.FrozenNecrosis,ColdStatusChange.KnockedOut,3);
            RemoveInvalidColdStates();return 3;
        }
        public bool RemoveChill(Combatant target)=>RemoveColdState(chills,target,ColdStatusKind.Chill);
        public bool RemoveFrostbite(Combatant target)=>RemoveColdState(frostbites,target,ColdStatusKind.Frostbite);
        public bool RemoveFrozenNecrosis(Combatant target)=>RemoveColdState(necroses,target,ColdStatusKind.FrozenNecrosis);
        bool RemoveColdState(Dictionary<Combatant,int> states,Combatant target,ColdStatusKind kind)
        {
            if(target==null||!states.Remove(target))return false;
            ColdStatusChanged?.Invoke(target,kind,ColdStatusChange.Removed,0);return true;
        }
        public void RemoveInvalidColdStates()
        {
            foreach(var target in chills.Keys.Concat(frostbites.Keys).Concat(necroses.Keys).Concat(blockedColdOpportunities).Concat(chillRecoveryProtection).Distinct(CombatantReferenceComparer.Instance).Where(x=>!x.IsAlive).ToArray())
            {RemoveChill(target);RemoveFrostbite(target);RemoveFrozenNecrosis(target);blockedColdOpportunities.Remove(target);chillRecoveryProtection.Remove(target);}
        }
        /// <summary>KOとは別に戦闘終了/Scene破棄で全員分を片づけ、次の戦闘やSaveへ持ち越しません。</summary>
        public void ClearColdStatuses()
        {
            if(clearingColdStatuses)return;
            clearingColdStatuses=true;
            try
            {
            foreach(var target in chills.Keys.Concat(frostbites.Keys).Concat(necroses.Keys).Distinct(CombatantReferenceComparer.Instance).ToArray())
            {RemoveChill(target);RemoveFrostbite(target);RemoveFrozenNecrosis(target);}
            blockedColdOpportunities.Clear();
            completingColdOpportunities.Clear();
            chillRecoveryProtection.Clear();
            }
            finally {clearingColdStatuses=false;}
        }
    }
}
