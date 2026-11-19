import csv
from itertools import product
from pathlib import Path
root = Path(__file__).parent
routes = list(csv.DictReader((root / "Routes.csv").open()))
with (root / "Economy.csv").open("w", newline="") as file:
    out = csv.writer(file)
    out.writerow("perField side startTalent beforeGross afterBossGross endGross reserveSpend beforePreserve afterBossPreserve endPreserve beforeSellExpected afterBossSellExpected endSellExpected".split())
    for route, wallet in product(routes, (0, 100, 200)):
        if route["startLevel"] != "16" or route["startExp"] != "0" or int(route["perField"]) not in (5,10,15):
            continue
        gross = [int(route[k])+wallet for k in ("beforeTalent","afterBossTalent","endTalent")]
        # 직업 3인 스킬 패키지 상한 + 펫 한 마리 + 무료분 소진 뒤 추가 약 2개씩
        spend = 540 + 100 + 2*20 + 2*40
        keep = [value-spend for value in gross]
        loot = int(route["lootTenths"])/10 - 22 - 65*int(route["side"])
        out.writerow([route["perField"],route["side"],wallet,*gross,spend,*keep,*[round(value+loot,1) for value in keep]])
with (root / "BossQueue.csv").open("w", newline="") as file:
    out = csv.writer(file)
    out.writerow(["opportunities","counterplay","withoutGate","withGate","targetStacksAfterFurnace","talentCost"])
    for states in product(("ready","chill","frostbite","ko"), repeat=3):
        can_respond = "ready" in states
        ended = all(state == "ko" for state in states)
        result = "battle ended" if ended else "allow if declined" if can_respond else "defer"
        out.writerow(["/".join(states),int(can_respond),"battle ended" if ended else "KO",result,0 if can_respond else "",0])
