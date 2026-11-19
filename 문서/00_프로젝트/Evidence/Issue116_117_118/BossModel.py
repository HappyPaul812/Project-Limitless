"""승인 폭군과 기존 공격 계수를 사용하는 결정적 수치 모형입니다."""
import csv
from dataclasses import dataclass
from itertools import product
from pathlib import Path

root = Path(__file__).parent
stats = {(int(r["level"]),r["job"]):{k:int(r[k]) for k in ("hp","attack","defense","mp","mpRecovery","agility")} for r in csv.DictReader((root/"Stats.csv").open())}
jobs = {"Taeon":"guardian","Miel":"healer","Paul":"mage","Serin":"sharpshooter"}
base = {"Taeon":(132,10,9),"Miel":(104,8,12),"Paul":(96,14,10),"Serin":(108,13,14)}
parties = [("guardian","Miel","Serin"),("healer","Taeon","Paul"),("fighter","Taeon","Serin"),("mage","Taeon","Miel"),("sharpshooter","Taeon","Miel")]
def rnd(x): return int(x + .5)

@dataclass
class Actor:
    job: str
    hp: int
    attack: int
    defense: int
    mp: int
    recovery: int
    agility: int
    def __post_init__(self):
        self.max_hp, self.max_mp = self.hp, self.mp
        self.stacks = self.chill = self.cd = self.momentum = 0

def actor(level, name):
    job = jobs.get(name,name)
    s = stats[level,job]
    if name not in jobs:
        return Actor(job,*s.values())
    hp, atk, agi = base[name]
    gained = level-1
    if job == "guardian":
        return Actor(job,hp+7*gained,rnd(atk+(12+gained)*.5),rnd((12+gained)*1.5),0,0,agi+gained)
    if job == "healer":
        return Actor(job,hp+5*gained,rnd(atk+(12+gained)*.5),0,20+gained+2*(12+gained),2+(12+gained)//10,agi+gained)
    growth_hp = 4 if job=="mage" else 5
    return Actor(job,hp+growth_hp*gained,s["attack"]+atk-12,0,0,0,agi+gained)

def simulate(level, party, pet, focus):
    units = [actor(level,n) for n in party]
    pet_user = units[0] if pet else None
    pet_resistance = pet
    timeline = sorted(range(4),key=lambda index: -(units[index].agility if index<3 else 11))
    hp=1800
    phase=step=burn=burn_damage=furnace=skips=deferred=enemy_actions=ally_actions=heals=chill_attempts=0
    broth=2
    target=None
    saw_response=False
    outcome="timeout"
    for cycle in range(1,81):
        for i in timeline:
            alive = [a for a in units if a.hp>0]
            if not alive: outcome="defeat"; break
            if hp<=0: outcome="victory"; break
            if i<3:
                a=units[i]
                if a.hp<=0: continue
                a.cd=max(0,a.cd-1)
                if a.chill:
                    a.chill-=1
                    skips+=1
                    continue
                ally_actions+=1
                stacked=next((m for m in alive if m.stacks==2),None)
                chilled=next((m for m in alive if m.chill),None)
                injured=min(alive,key=lambda m:m.hp/m.max_hp)
                if stacked:
                    saw_response=True
                    stacked.stacks=0
                    furnace+=1
                elif chilled and broth:
                    chilled.chill=0
                    broth-=1
                elif a.job=="healer" and a.mp>=6 and injured.hp<=injured.max_hp*.68:
                    injured.hp=min(injured.max_hp,injured.hp+rnd(injured.max_hp*.35))
                    a.mp-=6
                    heals+=1
                else:
                    percent=100
                    if a.job=="sharpshooter" and a.cd==0:
                        percent,a.cd=(180,3) if a is pet_user else (160,2)
                    elif a.job=="mage" and a.cd==0:
                        percent,a.cd,burn,burn_damage=170,3,2,rnd(a.attack*.30)
                    elif a.job=="fighter":
                        if a.momentum>=2: percent,a.momentum=160,0
                        else: percent,a.momentum=150,a.momentum+1
                    hp-=rnd(a.attack*percent/100)
                a.mp=min(a.max_mp,a.mp+a.recovery)
                continue
            enemy_actions+=1
            if hp<=900 and phase==0 and step!=1: phase,step=1,0
            priority=alive if focus=="front" else alive[::-1]
            if step==0:
                target=max(priority,key=lambda a:a.stacks)
                saw_response=False
            elif step==1:
                if target.hp>0:
                    if target.stacks==2 and not saw_response:
                        deferred+=1
                        continue
                    target.stacks+=1
                    if target.stacks==3: target.hp=0
                if hp<=900 and phase==0:
                    phase,step=1,-1
            elif step==2:
                a=priority[0]
                a.hp=max(0,a.hp-max(1,rnd(42*(1.4 if phase else 1.2))-a.defense))
            elif phase==0:
                for a in alive: a.hp=max(0,a.hp-27)
            else:
                a=priority[0]
                a.hp=max(0,a.hp-34)
                if a.hp and len(alive)>1 and not any(m.stacks or m.chill for m in alive):
                    chill_attempts+=1
                    if a is pet_user and pet_resistance: pet_resistance=False
                    else: a.chill=2
            step=(step+1)%4
            if burn: hp-=burn_damage; burn-=1
        if outcome!="timeout": break
    # 마지막 큐 슬롯에서 끝난 전투도 결과에 포함합니다.
    if hp<=0: outcome="victory"
    elif not any(a.hp>0 for a in units): outcome="defeat"
    return [level,"+".join(party),int(pet),focus,outcome,cycle,ally_actions,enemy_actions,furnace,skips,2-broth,heals,chill_attempts,deferred,max(0,hp),"/".join(str(a.hp) for a in units),"/".join(str(a.mp) for a in units)]

with (root/"BossModel.csv").open("w",newline="") as output:
    writer=csv.writer(output)
    writer.writerow("level party petModeled targetPriority outcome cycles allyActions bossActions furnace coldSkips soup heals coldAttempts deferred bossHp partyHp partyMp".split())
    for level,party,focus in product(range(19,22),parties,("front","rear")):
        writer.writerow(simulate(level,party,False,focus))
        if party[0]=="sharpshooter": writer.writerow(simulate(level,party,True,focus))
