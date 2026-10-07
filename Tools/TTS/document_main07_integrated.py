# -*- coding: utf-8 -*-
"""검증 결과만 최신 정본에 기록한다. 과거 QA 기록과 청취 미확인 상태는 유지한다."""
import csv,json,shutil
from pathlib import Path
ROOT=Path(__file__).resolve().parents[2];TEMP=ROOT/'Temp/Main07Integrated';DOC=ROOT/'문서/00_프로젝트'
assert not (DOC/'Main07_Miel_Recovery_Matrix.csv').exists(),'One-shot documentation: preserve the recorded result; edit follow-up findings directly'
summary=json.loads((DOC/'Main07_Integrated_Verification.json').read_text(encoding='utf-8'))
summary.update(compile_errors=0,console_errors=0,console_warnings=16,runtime_console_warnings=0,compile_warnings_existing=16,compile_warnings_new=0,missing_script_runtime=0,missing_script_scenes=9,final_scene='Bootstrap',final_editor_playing=False,final_editor_focused=False,audit_save_released=True,input_settings_restored=True)
(DOC/'Main07_Integrated_Verification.json').write_text(json.dumps(summary,ensure_ascii=False,indent=2),encoding='utf-8')
shutil.copyfile(TEMP/'scenes-missing-script.csv',DOC/'Main07_Integrated_MissingScript.csv')
growth=list(csv.DictReader((DOC/'Companion_Growth_Runtime_Matrix.csv').open(encoding='utf-8-sig')))
table='|동료|Lv|HP/Attack/Agility|동직업 Player HP/Attack/Agility|\n|---|---:|---|---|\n'
names={'companion_taeon':'태온','companion_miel':'미엘','companion_paul':'폴','companion_serin':'세린'}
for r in growth:
    if r['level'] in ['1','3','10']:table+=f"|{names[r['character']]}|{r['level']}|{r['max_hp']}/{r['attack']}/{r['agility']}|{r['player_hp']}/{r['player_attack']}/{r['player_agility']}|\n"
audit=json.loads((DOC/'Main07_Miel_Taeon_Original_PCM_Audit.json').read_text(encoding='utf-8'));old={r['stable_id']:r for r in audit};proof=json.loads((DOC/'Main07_Miel_PCM_Recovery_Proof.json').read_text(encoding='utf-8'))
rows=[]
for r in proof:rows.append(dict(stable_id=r['stable_id'],expected_text=r['expected_text'],old_actual_asr=old[r['stable_id']]['actual_asr_text'],old_duration=old[r['stable_id']]['duration'],final_duration=r['final']['duration'],source_parts=json.dumps(r['parts'],ensure_ascii=False),method=r['method'],final_wav=f"Unity/Client/Assets/_Project/Audio/Voice/Story/Main07/{r['stable_id']}.wav",sha256=r['final']['sha256'],pcm_sha256=r['final']['pcm_sha256'],runtime=r['runtime'],user_listening='PENDING'))
with (DOC/'Main07_Miel_Recovery_Matrix.csv').open('w',encoding='utf-8-sig',newline='') as f:w=csv.DictWriter(f,fieldnames=rows[0]);w.writeheader();w.writerows(rows)
p=DOC/'Main07_Integrated_Retry_Growth_Voice_QA.md';s=p.read_text(encoding='utf-8');s=s.replace('진행 중. 실제 실행 결과가 나오기 전 PASS로 기록하지 않는다.',f'''최종 **고유 검사 label {summary['runtime_pass']} PASS / FAIL0**, 반복·재개를 포함한 PASS 기록{summary['runtime_pass_records']}개. 단일 Play에서 전수 검사를 모두 완료했다고 주장하지 않는다. 초기 실패는 아래 검사기 보완 기록과 분리한다. [실행 결과](Main07_Integrated_Runtime_Results.txt), [검증 JSON](Main07_Integrated_Verification.json).

### Story Battle Retry

QuestCatalog 전체 Main01~17과 YAML inline/일반 형식 모두 확인: DefeatEncounter10개. [전수10 Matrix](Story_Battle_Retry_Matrix.csv). Main07만 FAIL_FIXED, 다른9개 기존 진입/재도전 구조 변경0. Main11만 보스 도망 금지 NOT_APPLICABLE_BOSS_FLEE이며 패배 후 재도전·승리는 PASS. 모든 도망/패배 Count0, 실제 재진입/승리 Count1/중복 종료·승리알림의 진행·EXP 중복0 및 Save/Continue PASS. 퀘스트 완료 보상은 기존 QuestService의 완료 제거/완료ID 정책을 보존한다.

Main07 최초 대화부터 실제 Main06완료→Field02재진입→Early7→PaulFirst→도망→F 재도전→패배→안전지대/거점 복귀→Continue→A 재도전→정상 명령 승리→미엘귀환/MielMeeting→마지막 대화→Main07완료→Continue를 통과했다. 별도 E 입력의 도망→Continue→재도전→정상 승리도 통과했다. 재도전은 첫 대화0, “폴 주변 몬스터 재도전” marker, 승리 뒤 Current=false를 확인했다. 다른 전수 전투는 실제 Scene·기존 Interact/MonsterEncounter 이벤트와 HP 승패 fixture를 사용했다. Collision 접촉·실물 키보드/패드는 이번 자동 입력·이벤트 검증 범위와 구분한다.

### Companion Growth

원인 COMPANION_LEVEL_SCALING_MISSING: 일반 정의와 Main03/04/07 임시 Factory 모두 고정 HP/Attack/Agility였음. 이제 같은 CompanionDefinition과 현재 Player Level을 생성 직전에 읽으며 레벨 Save0. Player 성장식 변경0. 수호자/치유사 동료 공통+1/레벨은 사용자 승인 정책이다. [공식과 근거](../10_전투/동료_파티_편성.md), [전체4동료×6레벨24행](Companion_Growth_Runtime_Matrix.csv).

{table}
Lv1/3/5/10/20/50 모두 역전0/범위 정상/최소HP·Attack·민첩1이상. 기존 HP/민첩 Lv1 값을 유지하고 공격만 baseAttack+기존직업보너스를 적용하므로 중복 성장0. 실제 EXP 레벨업3→4 이후 생성/SaveRestore와 실제 Bootstrap Continue 뒤4동료 EffectiveLv5 동일. 세린+미엘 수동행, Player여우/세린곰, 현재HP45/치유사MP7 등을 포함한 저장 보존 PASS.

Lv3 폴 Before HP96/Attack14/Agility10 → After104/44/12, 같은직업 Player108/42/12. 동일 방어0·충분한HP fixture에서 일반피해14→44, Fireball24→75(Player72): [실제 SkillExecutor 비교](Main07_Paul_Damage_Comparison.csv). [실제 Main07 명령 피해](Main07_Paul_Damage_Runtime.csv)는 남은HP 상한/기존 길 특성·전투 상태에 따라 다르며 원시 공격식과 구분한다. Fireball·기본공격의 실제 기여와 정상 승리 확인.

### Miel / Taeon Voice

미엘001/002/003의 원본 분할·번호 밀림 확인, 기존 rawPCM으로3개 복구. 최종 길이6.94/3.63/4.72초. Catalog/ID/본문/GUID/meta 변경0. [본문·기존 실제·원본·방법·최종WAV Matrix3](Main07_Miel_Recovery_Matrix.csv), [상세 Voice QA](Main07_Miel_Taeon_Voice_QA.md).

실제 MielMeeting 페이지8 미엘001 전체→Paul017/018→페이지11 미엘002→Paul019→페이지13 미엘003→Paul020, Next 수동/단일 Source/자연종료/이전 Clip 정리 및 PCM 일치. Early7/Paul20/Miel3/Taeon3 **실제33PCM 전sample 오차0**: [페이지 기록](Main07_Integrated_Voice_Pages.csv). 태온3은 해당문장과 대응하여 WAV 변경0. ASR 단어/문장부호 오차를 의미 오류나 사람청취 PASS로 자동 확정하지 않는다. 복구 미엘3의 사용자 청취는PENDING. 신규TTS API0/재생성필요0/남은 생성 여유 소비0.

### 통합 검증 / 보호

CompileError0, RuntimeConsoleError/Warning0, 최종컴파일 ConsoleError0/기존CS0618Warning16/이번변경새Warning0, 실제Runtime MissingScript0 + 관련9Scene 원본MissingScript0. [Scene 검사](Main07_Integrated_MissingScript.csv). 최종 cleanBootstrap EditMode/is_focused=false/격리Save·Settings해제/InputSettings 원값복원. 창foreground/OS입력0.

Baseline3274개 중 기존변경은 Main07 retry C#1/동료성장 C#2/MielWAV3=6, 나머지3268byte동일. Early7/Paul20/Main03~06Voice/Main17Voice·TTS/244전체목록/원본/사용자Save/PlayerGrowth/QuestReward/Party·Beast코드·Assets/meta/Catalog 보호. 검증 helper와 도구·문서만 추가했다.

사용자 직접 확인: 복구 미엘3 발화 청취, 기존의 실물 입력/물리Collision 별도 검증 범위 유지. 모든 Voice 사람청취 완료라고 보고하지 않는다. 다음 권장: 보호된 기존 미청취 목록과 별도 요청된 후속 범위 진행. GitHub push0.''')
s+='\n\n### 후반 fixture 상태 보완\n\n초기 Main11 SaveRestore 비교는 완료Main09 기록만 넣고 정식 폴 해금을 반영하지 않은 fixture였다. 기존 구버전 복원용 해금 경로와 같은 조건을 fixture에도 반영한 후 Main11 및 후반6전투의 동일 비교가 PASS했다. Save/Quest/CompanionRoster 게임 코드는 변경하지 않았다.\n'
p.write_text(s,encoding='utf-8')
status=DOC/'CURRENT_STATUS.md';s=status.read_text(encoding='utf-8');s=f'''## 2026-10-07 Main07 재도전·동료 성장·미엘 PCM 복구 완료

Main07 전투목표에서도 폴 위치의 E/F/A 재도전을 허용, 첫대화 반복0·승리후차단. 필수DefeatEncounter10 전수 재도전/단일승리/Continue PASS(Main11 보스도망금지). 동료 EffectiveLevel=PlayerLevel, 고유값+기존직업식/수호자·치유사 동료만 승인된 공통+1 적용, 일반·임시Story공유·동료레벨Save0·Player성장보존. 미엘기존3 rawPCM복구(6.94/3.63/4.72초), 태온3변경0·신규TTS0.

Main06→07 Early7/PaulFirst→도망·패배재도전→정상승리→MielMeeting→완료Continue, 별도도망재도전정상승리 PASS. 고유label{summary['runtime_pass']}PASS/FAIL0(반복기록{summary['runtime_pass_records']}), 실제33PCM오차0, CompileError0/RuntimeConsoleError·Warning0/기존컴파일Warning16/새Warning0/MissingScript0·관련9Scene0. Lv1/3/5/10/20/50×4, EXP레벨업/실제BootstrapContinue/수동Party·Formation·Beast·HP/MP보존 PASS. 기존3274중6변경/3268byte보호, Early7/Paul20/Main03~06/Main17/244/원본/사용자Save보호. cleanBootstrap EditMode·비포커스·격리Save/Settings/Input원복.

사용자 확인: 복구미엘3 청취PENDING(자동의미청취PASS로승격0), 기존실물입력/물리Collision 별도. 다음권장: 보호된미청취목록·별도후속요청. [통합QA](Main07_Integrated_Retry_Growth_Voice_QA.md) · [전수Retry10](Story_Battle_Retry_Matrix.csv) · [동료24행](Companion_Growth_Runtime_Matrix.csv) · [Voice3](Main07_Miel_Recovery_Matrix.csv). 마지막 관련구현commit은 아래최종QA Git항목/최신local log. GitHub Push0.

---

'''+s;status.write_text(s,encoding='utf-8')
for name in ['Main07_Early7_Paul_Recovery_QA.md','Story_Dialogue_Consistency_QA.md','Story_Voice_Main01_12_QA.md']:
    p=DOC/name;s=p.read_text(encoding='utf-8');p.write_text('> 2026-10-07 최신 Main07 미엘기존3 PCM복구/태온3보존·재도전·동료성장은 [종합QA](Main07_Integrated_Retry_Growth_Voice_QA.md)를 참조. 이 문서의 이전 기록과 기존244 청취 목록은 유지한다. Early7/Paul20 원본과 결과는 보호했다.\n\n'+s,encoding='utf-8')
print('Documented verified final results; Miel human listening PENDING; no push')
