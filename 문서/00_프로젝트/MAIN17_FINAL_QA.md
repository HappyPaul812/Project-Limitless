## 2026-10-08 Main17 승인 TTS14 적용·Runtime 검증 완료

사용자 직접 청취 **USER_LISTENED_PASS14/14**. 원본 byte 복사14·Catalog236→250(prefix보호)·SHA/PCM/Unity전sample14/14오차0. Default/Hearing 실제12목표·조건부Paul/Miel/Taeon 유무·자연종료14 후page유지·Next/Scene/Battle잔류0·정상승리/afterVoice·실제Continue5지점·Mixer100/40/0/Mute/복원 PASS. 기술고유646PASS/FAIL0, CompileError0/최종ConsoleError·Warning0/새Warning0/MissingScript0. **Main17 TTS_PENDING14→0, USER_LISTENED_PASS14·UNITY_APPLIED14·RUNTIME_VERIFIED14**. Main18 TTS_PENDING22·생성/적용/변경0, API0·가공0·기존Voice예상밖변경0.

기존3253파일 중3252byte보호·의도한Catalog1수정·삭제0, 사용자 미커밋 변경 보호. 추가 Main17 Voice 청취 요청0; 기존 음악/실물입력 및 Main18의 별도 미확인 항목 유지. 다음권장: 사용자 지정 후속 확인 또는 별도 승인 Main18 음성 작업. 마지막 관련 구현 commit `f0f44b3`, QA는 이 항목 포함 최신Docs commit. [최종TTSQA](Main17_TTS_Final_QA.md) · [적용14표](Main17_TTS_Applied_Matrix.csv). cleanBootstrap EditMode·비포커스·격리저장/설정/Listener복구. GitHub Push0.

---

## 최신 최종 판정 — 실제 Art / Audio 적용

FUNCTION_COMPLETE / ART_COMPLETE / BGM_COMPLETE / SAVE_COMPLETE / PATH_COMPLETE. 정확한 English.zip 원본9PNG 등록·7실제적용(지면변형2보관), Preview미사용. PNG/ZIP/MP3 원본 수정0. Blade and Gambit는 Chapter2 서부 일반/Story에 적용, 기존 단일Source/Mixer Fade0.20/0.75/0.9·LoopON, Field07/08 탐색곡 복귀·Boss예외 유지. Quest12/보상60·50/Serin·Fox/Party/5Path 및 기존44승리·KO0 보호.

이번 고유 PASS270 / FAIL0 / 자동 NOT_VERIFIED0 / 진행 Blocker0. 기존309/180PASS 별도 재사용. 실제 통합 Story 승리/완료Continue 및 Field07/08 일반 BGM·Fade·3해상도 HUD 캡처 PASS. Transform425/Collider15 예상 밖 변경0, 원본3075 누락0·PNG/WAV 보호. Compile완료/Error0, 기존 CS0618 Warning16·이번 클래스 Warning0, MissingScript0. Bootstrap 비포커스 EditMode·격리Save/설정 해제.

남은 사용자/외부: TTS_PENDING14, 실물 Keyboard/Gamepad/Dungeon USER_INPUT_REQUIRED3, Field08/Battle 음악 USER_LISTENING_REQUIRED2. 기존 장비DF1/Standalone DR1 분리. 다음 작업은 청취·실물/Release 검증이며 TTS/Main18은 별도 요청 전 작업하지 않는다. 마지막 구현 commit Art3b4820a / Audio53ef42c, 상세 [최종 QA](Main17_ArtAudio_QA.md). GitHub Push하지 않음.

---

# 2026-10-06 Main17 TTS 제외 최종 마감 — 최신 정본

아래 Phase9 보고는 c182a04 당시 이력이다. 이후 사용자 승인으로 **Main17 완료 Reward EXP60·탈렌트50**을 확정했다. 환경 ART_WAITING_EXTERNAL, Chapter2 Battle BGM AUDIO_WAITING_EXTERNAL, TTS_PENDING14 유지. 신규 Elite는 Main17 미완성이 아니라 Main18 후보이며 이번에 만들지 않는다.

이번 마감 근거: Main17_Reward_Audit.md, Main17_Field08_Art_Handoff.md, Main17_Audio_Handoff.md, Main17_Balance_QA.md, Main17_Closure_Runtime.txt. 기존309PASS는 재사용하며 새 검증에 합산하지 않는다. 새 밸런스는5Job×Lv4/9/12/16×Story/General40회 + 치유 스킬4회, 모두 실제 Controller에서 승리/복귀·KO0이다. 몬스터 수치 변경0. 보상 변경은 마지막 실제 대화·완료·중복방지·완료Save/Continue 및 레벨업 경계만 재검증한다. 기존 Save5지점은 반복하지 않는다.

남은 수동 확인: 실물 Keyboard/Gamepad/Dungeon접촉 USER_INPUT_REQUIRED3, 탐색 BGM 음악적 적합성 USER_LISTENING_REQUIRED1. 일반장비 DF1/Standalone DR1은 기존 별도 범위다. TTS/외부환경/외부전투Audio 제외 Main17 기능·Story·Battle·Save·Path 진행 Blocker를 닫으며 실제 출시 전 수동/Release 검증을 완료로 취급하지 않는다.

이번 마감 고유 검증 **PASS180 / FAIL0 / NOT_VERIFIED0 / 진행·Save·전투·Path Blocker0**. 원시236체크는 중복 제거해 Main17_Closure_Cases.csv에180개를 기록했다. 추가 치유4회는 같은 검증 ID를 재사용하므로 고유 합계에 다시 넣지 않는다. 기존309PASS 재사용은 이번180개와 별도 집계다. 마지막 실제 대화/지역명/QuestHUD/조사Prompt/Portrait/Voice없음 및 정상60·50완료와 Lv12→13/EXP30 경계, 중복지급0·완료Continue/BGM/Party/Beast PASS. Compile 완료·Console Error0/Warning0·Missing Script0·Bootstrap EditMode/격리Save해제/slot0/비포커스 복귀. 보호3075파일 누락0, 시작전103상태 보존. 실제 출시는 외부 납품·청취·Release 검증 후 판단한다.

---
# Main17 최종 통합 QA — 2026-10-06

## RESUME와 범위

LOCAL 체크포인트의 Phase1~8 완료 산출물을 재사용하고 Phase9를 마무리했다. 원래 작업 중 Phase5에서 전달된 RESUME를 반영했으며, 현재 재개는 미커밋 통합 QA 후반부터 진행했다. 기존 817건/812 PASS와 전투 하위 메뉴 484건/482 PASS는 재사용 근거이며 이번 수치에 합산하지 않는다.

## 구현 계약

Main17 `main_17_red_rift` / 「붉은 균열」, Field_08_RedRift / 붉은 균열 협곡. Main16 완료가 선행 조건이다. 12개 순차 목표는 entry/crack/vibration/tracks/serin/compare/resonance/witness/threat/after/route/withdraw. 완료 RewardBundle은 비어 있고 전투의 기존 보상만 적용한다. 전투 뒤에도 진동이 남고 더 깊은 길은 위험하므로 준비 후 조사한다는 Main18 Hook까지 구현한다. Main18 Quest·보스·Overheat는 구현하지 않는다.

Field08은 21×15 Bounds, 동쪽 Field07 왕복 출구와 분리된 Spawn을 사용한다. 서쪽은 막혀 있다. 깊은 틈 중앙 Collider와 북/남 우회로, 붉은 지면·미약한 빛·재·어두운 암석을 기본 도형으로 표현한다. 새 PNG 생성이나 기존 사용자 PNG 수정은 없다. 환경은 전용 완성 아트가 아닌 Placeholder다.

LOCAL에서 세린은 Main15 정식 해금되어 있다. 이를 취소하거나 Party를 강제 덮어쓰지 않는다. Main17 현장 조사 협력 Actor는 별도로 만들고, 전투는 저장된 최대3인 편성을 사용한다. Hearing / Sharpshooter / Fox 및 Aim·Arrow Rain·Companion Assault의 기존 구조를 재사용한다. 공식 세린 Sprite/Animator/Portrait는 READY다.

Hearing은 Player가 반복 패턴을 먼저 확인하고 세린이 보완한다. Default는 세린이 먼저 안내하며 나머지4개 Path의 관찰 대사도 존재한다. 모든 Path의 목표와 결과는 동일하다. 지문은 화자·초상화·Voice가 없다. 37개 Stable ID 중 NPC14개 TTS_PENDING, 나머지23개 NOT_EXPECTED. 다른 Voice를 임시 연결하지 않았으며 TTS 생성0이다.

필수 Story Encounter는 기존 도마뱀 1체, 선택 일반 Encounter는 기존 딱정벌레다. 기존 몬스터 원본 수치는 변경하지 않았다. 실제 Fighter20/저장3인 편성의 정상 일반공격으로 승리·복귀를 검증했다. 모든 직업/저레벨 밸런스의 포괄 검증은 아니다. 기존 불씨망령만 반복하지 않으며 신규 Elite Art는 준비되지 않았다.

실제 로컬 Beneath_The_Cracked_Earth.mp3를 Field08 BGM으로 연결했다. Chapter2 Battle BGM은 기존 LOCAL 계약의 null/TBD를 유지한다. 승리·Continue 후 Field08 음악 복귀, 단일 Source·Loop·Mixer를 확인했다. 사람의 음악 청취는 별도 항목이다.

## 검증 근거와 한계

Main17_Runtime_Results.txt는 최종 통합 실행, Main17_Phase6_Paths.txt는 5개 Path 실행 근거다. Main17_QA_Cases.csv는 이 두 파일의 PASS/FAIL 문자열 ID를 중복 제거한 목록이다. 원시 체크 수와 고유 체크 수를 합산하지 않는다. Quest 정적17개/대사142개 비교/37개 Voice 미연결 확인은 추가 근거이며 CSV 수치에 합산하지 않는다.

실행한 범위: Main16 마지막 조사/서쪽 잠금, Main17 시작, 양방향 Scene전환, Spawn이격·grace·Bounds·카메라, 조사 순서·대화 취소·같은 프레임 중복 입력 차단, Actor/목표 중복 방지, 실제 Story Battle·승리·복귀·후속 조사·완료, 5지점 Save/실제 시작메뉴 Continue, Party/Formation/Serin/Fox/Path 복원, Arbel/Field06 서쪽 내비게이션, Skill/Item 뒤 기본5버튼 숨김·입력 차단·포커스·취소 복원, Portrait와 새 Voice 없음, BGM 복귀.

일반 몬스터 respawn은 기존 메모리 기반35초 정책의 제어된 상태 fixture/동일 프로세스 Continue를 확인했다. OS 재시작의 respawn 타이머 저장은 검증하거나 새로 구현하지 않았다. 비승리 복귀는 공통 API로 실행했으며 실물 도망 입력 검증이 아니다. 기존 실제 전멸/도망 QA는 재사용한다.

발견/수정: 인접 조사에 게임패드 A의 마지막 Next가 같은 프레임 전달되는 문제를 1프레임 보호로 차단했다. Field08 재 Mesh에 런타임 전용 Main16AshPulse를 Scene 직렬화하여 384개 누락 참조 경고가 발생했다. Scene에는 Mesh만 저장하고 PlayMode에 기존 연출 컴포넌트를 추가하도록 수정한 뒤 Scene 재생성·재로드·통합 실행을 다시 수행했다. 최종 Console 상태와 정확한 수치는 아래 완료 기록을 따른다.

보호 파일3075개 해시 검사: 누락0. 변경7개는 BgmSceneCatalog, BattlePrototypeEncounter, BattleSceneController, 이전 QA의 PartyManagementPresenter, EditorBuildSettings, Chapter2 Main16 후속 설명/서부 방향 문서다. 기존 Sprite50/Voice·Main03/Main04 WAV/사용자 Save는 변경하지 않았다.

## 남은 항목

- USER_INPUT_REQUIRED 4: 실물 Keyboard, Gamepad, Dungeon 물리 접촉, Field08 BGM 실제 청취.
- DEFERRED_FEATURE 1: 일반 Equipment 장착/Save.
- DEFERRED_RELEASE_VALIDATION 1: Standalone Build/OS 재시작.
- TTS_PENDING 14: Main17 신규 NPC 대사 제작·원본/Runtime 청취.
- ART_PENDING: 전용 협곡 환경 및 신규 Elite Art. 현재 기본 도형 환경과 기존 공식 몬스터로 동작한다.
- 포괄 직업/저레벨 전투 밸런스는 이번 대표 검증 범위 밖이다.

## Git 정책

Phase별 관련 파일만 Stage/Commit한다. 기존103개 작업 상태는 보존한다. 이번 변경의 diff --check는 통과해야 한다. 전체 작업트리 diff --check는 시작 전부터 남아 있던 unrelated whitespace를 별도 기록하며 수정하지 않는다. GitHub Push는 하지 않는다.

## 최종 완료 기록

통합 원시246 PASS/FAIL0, Path 원시120 PASS/FAIL0. 두 근거의 고유 CSV: PASS 309 / FAIL 0 / NOT_VERIFIED 0 / USER_INPUT_REQUIRED 4. 이외 DF1/DR1/TTS14/환경·Elite Art 대기는 위 분류로 별도 관리한다. 최종 재로드 Missing Script0, 컴파일 완료·Console Error0/Warning0(수정 후 재실행 관찰 구간), 이전 CS0618 Warning16 이력은 삭제된 결함으로 해석하지 않는다. Bootstrap EditMode/비포커스/slot0/runInBackground=false 복귀 확인.
