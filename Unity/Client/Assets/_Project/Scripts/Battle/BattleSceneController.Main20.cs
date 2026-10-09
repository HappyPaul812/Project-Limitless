using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.Battle
{
    /// <summary>Main20 행동을 기존 직접 피해/과열/화상 경로에 연결합니다. 파수꾼 행동은 별도 원본 분기를 유지합니다.</summary>
    public sealed partial class BattleSceneController
    {
        private string main20PhaseDirection;
        private IEnumerator PlayMain20Action(Combatant actor, string action)
        {
            actionPlaying = true; SetCommandButtons(false);
            var view = combatantViews[actor];
            bool tell = action == "condensation" || action == "resonance";
            bool area = action == "core_wave" || action == "eruption" || action == "wave";
            bool heat = action == "injection" || action == "eruption";
            string label = action == "injection" ? "열압 주입" : action == "molten" ? "용융 강타"
                : action == "condensation" ? "열핵 응축" : action == "core_wave" ? "열핵 파동"
                : action == "resonance" ? "열핵 공명" : action == "eruption" ? "열핵 분출" : "열파";
            if(action == "molten")view.MonsterAnimation?.PlayAttack();else view.MonsterAnimation?.PlaySkill();
            messageText.text = tell ? label + "! " + main20Boss.Telegraph : "열맥 거신의 " + label + "!";
            RefreshCombatantViews(null);
            if (tell)
            {
                yield return Main20Vfx(view.ActionRoot, action);
                view.MonsterAnimation?.StopAttackAndReturnToIdle(); actionPlaying = false; FinishCurrentAction(); yield break;
            }
            Combatant[] targets;
            if(area)targets=allies.LivingMembers.ToArray();
            else
            {
                var choices=TargetResolver.ResolveHostileTargets(actor,allies,
                    action=="molten"?TargetRangeType.MeleePhysical:TargetRangeType.Magic);
                if(action=="injection"&&choices.Count>0)
                { int high=choices.Max(statusEffects.GetOverheatStacks);choices=choices.Where(t=>statusEffects.GetOverheatStacks(t)==high).ToArray(); }
                var selected=ChooseEnemyTarget(actor,choices);targets=selected==null?Array.Empty<Combatant>():new[]{selected};
            }
            int percent=action=="injection"?90:action=="molten"?125:action=="core_wave"?80:action=="eruption"?60:55;
            int raw=statusEffects.ModifyOutgoingDamage(actor,(int)Math.Max(1L,((long)actor.Attack*percent+99)/100));
            foreach(var target in targets)
            {
                if(!target.IsAlive)continue;
                yield return Main20Vfx(combatantViews[target].ActionRoot,action);
                int damage=pathTraits.ApplyDirectDamage(statusEffects,actor,target,raw,areaAttack:area);
                if(target.IsAlive&&action=="molten")statusEffects.ApplyOrRefreshBurn(target,
                    (int)Math.Max(1L,((long)actor.Attack*30+99)/100),2);
                if(target.IsAlive&&heat)statusEffects.ApplyOverheat(target);
                messageText.text=label+": "+target.DisplayName+"에게 "+damage+" 피해"+(heat?", 생존 대상 과열 +1":"");
                RefreshCombatantViews(null);
            }
            view.MonsterAnimation?.StopAttackAndReturnToIdle();actionPlaying=false;FinishCurrentAction();
        }
        // 정적 512px PNG 하나를 부드럽게 표시합니다. 분할/점멸/진동을 추가하지 않고 HUD 예고 텍스트를 병행합니다.
        private IEnumerator Main20Vfx(RectTransform parent,string action)
        {
            string name=action=="injection"?"Veinfire_HeatPressureInjection_VFX":action=="molten"?"Veinfire_MoltenStrike_VFX"
                :action=="condensation"?"Veinfire_CoreCondensation_Telegraph":action=="core_wave"?"Veinfire_CoreWave_VFX"
                :action=="resonance"?"Veinfire_CoreResonance_Telegraph":action=="eruption"?"Veinfire_CoreEruption_VFX":"Veinfire_HeatWave_VFX";
            var sprite=Resources.Load<Sprite>("Main20/VFX/"+name);
            if(sprite==null){Debug.LogError("Main20 VFX 누락: "+name);yield break;}
            var go=new GameObject(name,typeof(RectTransform),typeof(CanvasRenderer),typeof(Image));go.transform.SetParent(parent,false);
            var image=go.GetComponent<Image>();image.sprite=sprite;image.preserveAspect=true;image.raycastTarget=false;
            go.GetComponent<RectTransform>().sizeDelta=new Vector2(230,230);
            float time=0;while(time<.5f){time+=Time.unscaledDeltaTime;image.color=new Color(1,1,1,.65f*Mathf.Sin(Mathf.Clamp01(time/.5f)*Mathf.PI));yield return null;}
            Destroy(go);
        }
    }
}
