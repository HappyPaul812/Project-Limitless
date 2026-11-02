"""Main24 격리 후보의 변경 범위·ID·보호 계약을 읽기 전용으로 확인합니다."""
import hashlib
import json
import re
import subprocess
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
BASE = "ab47b931fec92976170f9f50aabde1822d78bb90"
UNITY = ROOT / "Unity/Client"
EVIDENCE = ROOT / "문서/00_프로젝트/Evidence/Main24FirstPlayable20261103"
results = []

def check(name, condition):
    results.append({"name": name, "pass": bool(condition)})

def git(*args):
    return subprocess.check_output(["git", *args], cwd=ROOT)

allowed = {
    "Unity/Client/Assets/_Project/Scripts/World/Chapter3Main23Flow.cs",
    "Unity/Client/Assets/_Project/Scripts/World/Main23GuidanceHud.cs",
    "Unity/Client/Assets/_Project/Scripts/World/Chapter3Main24Flow.cs",
    "Unity/Client/Assets/_Project/Scripts/World/Chapter3Main24Flow.cs.meta",
    "Unity/Client/Assets/_Project/Scripts/Editor/Main24FirstPlayableQA.cs",
    "Unity/Client/Assets/_Project/Scripts/Editor/Main24FirstPlayableQA.cs.meta",
    "Unity/Client/Assets/_Project/Resources/QuestDefinitions/Main24_FirstSnowWarning.asset",
    "Unity/Client/Assets/_Project/Resources/QuestDefinitions/Main24_FirstSnowWarning.asset.meta",
}
changed = set(git("diff", BASE, "--name-only", "--", "Unity").decode().splitlines())
changed.update(git("ls-files", "--others", "--exclude-standard", "Unity").decode().splitlines())
check("Unity changes limited to approved Main24 source and quest", changed <= allowed)
for label, path in {
    "Save writer": "Assets/_Project/Scripts/Core/GameSaveService.cs",
    "Six slot persistence": "Assets/_Project/Scripts/Battle/SkillLoadoutService.cs",
    "Skill UI": "Assets/_Project/Scripts/UI/SkillManagementPresenter.cs",
    "Main23 quest": "Assets/_Project/Resources/QuestDefinitions/Main23_WarmLand.asset",
    "ProjectVersion": "ProjectSettings/ProjectVersion.txt",
    "Package manifest": "Packages/manifest.json",
    "Package lock": "Packages/packages-lock.json",
    "BuildSettings": "ProjectSettings/EditorBuildSettings.asset",
}.items():
    relative = "Unity/Client/" + path
    # Git의 checkout 줄바꿈 필터를 적용한 blob으로 비교하여 CRLF/LF만 다른 경우를 오검출하지 않습니다.
    check(label + " unchanged", git("hash-object", "--path=" + relative, relative).strip() == git("rev-parse", BASE + ":" + relative).strip())
quest = (UNITY / "Assets/_Project/Resources/QuestDefinitions/Main24_FirstSnowWarning.asset").read_text(encoding="utf-8")
flow = (UNITY / "Assets/_Project/Scripts/World/Chapter3Main24Flow.cs").read_text(encoding="utf-8")
ids = [re.search(r"^  questId: (.+)$", f.read_text(encoding="utf-8-sig"), re.M).group(1)
       for f in (UNITY / "Assets/_Project/Resources/QuestDefinitions").glob("*.asset")]
check("Quest IDs unique", len(ids) == len(set(ids)))
check("Prerequisite and zero reward", "- main_23_southbound_traces" in quest and "Experience: 0" in quest and "Currency: 0" in quest and "Items: []" in quest)
check("F2 remains explicit unissued waiting signal", "targetId: frostway_main24_f2_ready" in quest and "frostway_main24_f2_ready" not in flow and "NotifySignal" not in flow)
check("No payment item grant or F2 load APIs", not any(x in flow for x in ["TryPurchase", "SpendCurrency", "AddCurrency", "TryAddItem", "SceneTransitionService.TryLoad", "NotifyEncounterWon"]))
check("Runtime source is identical to tested QA copy", all(
    (ROOT.parent / "QA/Client" / path.removeprefix("Unity/Client/")).read_bytes() == (ROOT / path).read_bytes()
    for path in allowed if path.endswith(".cs") or path.endswith(".asset")))
EVIDENCE.mkdir(parents=True, exist_ok=True)
result = {"base": BASE, "checks": results, "pass": sum(x["pass"] for x in results), "fail": sum(not x["pass"] for x in results)}
(EVIDENCE / "StaticScope.json").write_text(json.dumps(result, ensure_ascii=False, indent=2), encoding="utf-8")
print(json.dumps(result, ensure_ascii=False, indent=2))
raise SystemExit(bool(result["fail"]))
