# Main18 세린10 Staged 사전 안전·재현성 감사 — 2026-10-13

**STATIC_AUDIT_COMPLETE / EXECUTION_HOLD / NOT_EXECUTED. 현재 파일은 실행 후보로 승인할 수 없습니다.** 대상10개의 ID·원문·기존 해시는 정본과 일치하지만, 허용 Voice가 Erinome/Kore여서 **Schedar 유지** 계약과 충돌합니다. quota·승인 파일 검증·실패 재개도 보완이 필요합니다. 수정 제안만 작성했으며 원본 코드를 변경하지 않았습니다.

## 조사 기준과 보호 범위

- GitHub main 조회: `eca6f1e5ceb42e9d6c88b53368bc109280a547b8` (`Docs: prepare Oct 13 role-specific prompts and staged Serin TTS safety review`). 로컬 시작 HEAD `509886f85b6b50deb3f2b69ad7d1f1cc85945157`, 브랜치 main. 원격 ahead3/behind0이며 차이는 문서9파일입니다. pull/merge/fetch/push 없이 연결 도구로 문서를 읽었습니다. 일반 Git 원격 조회는 연결 실패했고 GitHub 연결 도구로 확인했습니다.
- 먼저 AGENTS, PROJECT_CONTEXT, CURRENT_STATUS, CODEX_WORKFLOW, 팀 START_HERE/COMMON_RULES/담당 CODEX/CODEX_LATEST를 읽었습니다. 원격 main의 AGENTS·WORKFLOW·CURRENT_STATUS·CODEX_LATEST와 최신 업무지시도 확인했습니다. 오래된 인계의 승인 대기 표기는 위쪽 후속 승인/통합 기록으로 대체됩니다.
- 대조 정본: [Schedar 재제작 인계](Main18_Serin10_Schedar_Regen_Handoff_20261012.csv), [음성 정책](TTS_음성_제작_정책.md), [quota 운영 기록](TTS_FreeTier_Quota_운영_기록.md), [Main18·19 선택 통합 QA](Main18_19_TTS_Selective_Integration_QA_20261012.md), Main18 Manifest/DialogueCatalog 및 Main17 최종 QA.
- 사용자 미커밋 **337 status 항목**, 빈 staged index, CURRENT_STATUS의 기존 사용자 추가8줄을 보호합니다. 이번은 기능 작업이 아닌 조사이므로 CURRENT_STATUS와 기존 CODEX_LATEST는 수정하지 않았습니다. 이 보고서와 Field04 조사 자료가 별도 후임 자료입니다.
- 대상 Python의 import/실행/prepare/report/sample/approve/remaining 모두0. 키 파일·환경변수 값 읽기0, TTS 외부 네트워크/API0, WAV 생성·청취·가공0, 설치/다운로드0, Unity 적용0. 문서 작성과 GitHub 정본 읽기는 승인된 조사 범위입니다. 서비스 현재 제공 여부·SDK 호환·실제 quota/결제 상태는 조회하지 않았습니다.

## 파일 식별

| 항목 | 실제 관측 |
|---|---|
| 경로 | `F:\Downloads\M18_Serin10_Staged.py` |
| 파일 존재/크기 | 존재 / 27,128 bytes |
| SHA256 | `e358511d852a014f0c2196754debe523cd55336bda8a11d18717a3278276b10f` |
| 인코딩 | UTF-8, BOM 없음; encoding cookie UTF-8; strict decode 성공 |
| 구문 | `ast.parse` 성공 / AST 노드4,920개; 정상 실행이나 SDK 호환을 의미하지 않음 |
| 비밀 literal 검사 | 일반 Google/토큰 패턴 발견0; 키값 출력/보관0. 완전한 비밀정보 부재를 보증하는 검사는 아님 |
| 기본 작업 폴더 | `F:\Downloads\Limitless_TTS_Regen_Main18_Serin10` / 조사 시 존재하지 않음 |

텍스트를 표준 라이브러리 검사 코드로 읽고 AST의 literal만 추출했습니다. 대상 함수·모듈을 import/eval/exec/호출하지 않았습니다. Python의 `-B`로 검사 코드의 bytecode 기록도 억제했습니다. 최초 소스 표시 도중 터미널 cp949 출력 제한으로 중단된 부분은 UTF-8 출력으로 다시 읽었습니다. 이는 대상 스크립트 실행 오류가 아닙니다.

## 서비스·설정·호출 수

| 항목 | 소스 근거 / 판정 |
|---|---|
| 서비스/SDK | Google Gemini API, `google.genai` / 지연 import (L383–392) |
| 모델 literal | `gemini-3.8-flash-tts` (L28); 현재 실서비스 지원 여부 UNKNOWN |
| 기존 Voice 기록 | `OLD_REQUEST_VOICE='Schedar'` (L30). 과거 매핑 설명용이고 실제 요청에 강제되지 않음 |
| 실제 후보 | `CANDIDATE_VOICES=('Erinome','Kore')` (L31), argparse와 sample/approve/remaining 모두 제한. **Schedar 인자를 거부함** |
| style | 침착하고 판단력 있는 젊은 여성 동료, 명확하고 자연스러운 관찰 전달; 과장/뉴스 낭독/기계적 말투 회피; 단일 한국어 대사 정확 발화, 앞뒤 대사/추임새 금지 (L395–399) |
| 요청 | 단일 user text + `speech_metadata.style`, AUDIO + `speech_config.voice_config.voice` (L423–430) |
| 옵션 | temperature/seed 별도 지정 없음. SDK 버전 고정/요청 직렬화 검증 없음 |
| prepare / report / approve | TTS API0 예상. **모두 쓰기 부작용 있음**. approve는 WAV byte 복사도 수행 |
| sample | 신규 Stable ID1개 요청1회 의도. `main18_ground_serin_01` |
| remaining | 승인 샘플 제외9개, StoryOrder 정렬; 한 성공 뒤22초 간격; 호출 실패 시 현재 invocation 즉시 종료 |
| 최대 제작 의도 | 샘플1 + 잔여9 = 대상10 / 앱 요청10회. SDK `HttpRetryOptions(attempts=1)` 의도이나 실제 네트워크 시도 수는 미검증 |
| 현재 승인/실적 | 호출 승인0 / 실제 호출0 / 유료 청구 발생 없음(호출하지 않음). 향후 무료 보장 여부 UNKNOWN |

## Stable ID·원문·원본 해시 대조

[개별 정적 대조표](Serin10_Static_Target_Audit_20261013.csv)에 대상10개를 기록했습니다. TARGETS 집합은 인계10개와 정확히 일치하고 중복0, 화자 companion_serin·원문·기존 WAV SHA256 10/10 일치합니다. Main18DialogueCatalog의 literal 존재/StoryOrder 순서도 독립 텍스트 대조로 일치했습니다.

| 스크립트 목록 순서 | Stable ID | StoryOrder |
|---:|---|---:|
| 1 / 샘플 | main18_ground_serin_01 | 6 |
| 2 | main18_heat_vent_serin_01 | 7 |
| 3 | main18_leon_serin_01 | 2 |
| 4 | main18_overheat_serin_01 | 8 |
| 5 | main18_overheat_serin_02 | 9 |
| 6 | main18_overheat_serin_03 | 10 |
| 7 | main18_overheat_serin_04 | 11 |
| 8 | main18_vibration_serin_01 | 12 |
| 9 | main18_watcher_witness_serin_01 | 13 |
| 10 | main18_watcher_witness_serin_02 | 14 |

샘플은 요청된 ground를 먼저 쓰고, 후속9개는 **leon → heat_vent → overheat01~04 → vibration → watcher01~02** 순서입니다. 샘플이 전체 Story 첫 대사라는 뜻은 아닙니다.

보호 정본은 Main18 정상 NPC12 + Main19 정상 NPC13 = **25**(이 중 세린 Main18 정상5 + Main19 정상7), 기존 **Main17 승인 세린11**입니다. Main17 전체 승인 음성은14개이고 그 중 세린이11개임을 구분합니다. 이 밖의 기존 승인 음성도 보호합니다. 현재 파일의 보호 CSV는 세린5+7만 직접 다루며 정상 NPC25 전체나 Main17 승인11의 해시·Catalog 불변을 강제하는 장치는 없습니다. 기존 전체 Unity Assets/TTS 출력 해시 보호 검사로 이번 무변경을 확인하되, 미래 도구에도 허용10/보호 목록과 hash 검증이 필요합니다.

## 경로·쓰기·충돌 부작용

기본 work_root는 스크립트 옆 위 폴더입니다(L83–86). `LIMITLESS_SERIN10_WORKDIR`로 임의 절대 경로를 받을 수 있고, 저장소는 `LIMITLESS_REPO_ROOT` 또는 F: 고정 경로/cwd 상위에서 찾습니다. **환경변수 값은 읽지 않았으므로 실제 override 존재는 UNKNOWN**입니다.

| 하위 경로 | 예상 동작 |
|---|---|
| backup_original/TTSOutput/<ID>.wav | 원본10 복사, `xb` 신규 전용. 동일 SHA 기존 백업 유지; 다른 SHA면 중단 |
| backup_original/Unity/<Assets 상대경로> | 동일 대상명 Unity WAV 발견 시 백업. 공식 제외10은 현재 미연결 정본 |
| sample_candidate/main18_ground_serin_01.wav | 샘플1 신규 전용 `xb`; 존재 시 요청 전 중단 |
| new_approved/<ID>.wav | 샘플 승인 시 byte 복사, 잔여9 생성. 이름은 기존 원본과 동일하나 폴더가 별도 |
| _STATE.json / _STATE.tmp | tmp `write_text` 후 `os.replace`; 기존 상태/임시 파일 덮어씀 |
| Repair_Status / Backup_Index / Protected / STORY_ORDER_LISTENING CSV·STATUS_REPORT.txt | `w` 또는 write_text로 기존 보고서 덮어씀. `report`도 읽기 전용이 아님 |

명시적인 원본 삭제·WAV 덮어쓰기·Unity 연결·git·shell·패키지 설치·다운로드 API는 소스에서 발견하지 않았습니다. 다만 work_root의 **금지 경로 containment/symlink/junction 검사 없음**으로 override가 Unity/기존 출력/Save 아래면 “Unity를 쓰지 않는다”는 설명을 강제할 수 없습니다. 백업 인덱스도 현재 경로 문자열을 신뢰합니다. 별도 폴더라 해도 공유 임시 파일과 상태에 lock이 없어서 동시 프로세스는 상태를 덮고 중복 요청할 수 있습니다. `xb`는 WAV 덮어쓰기는 막지만 두 프로세스의 유료/무료 호출 중복까지 막지 않습니다.

## 위험·수정 건의 (수정/실행 승인 아님)

| 우선순위 | 코드 근거 / 위험 | 건의 |
|---|---|---|
| 필수 | L31/467–468/488/508: Schedar 허용 안 됨 | Schedar 고정 계약으로 검토된 별도 버전 납품. 기존 원본을 자동 수정하거나 Voice 변경하지 않음 |
| 필수 | L377–380/438–439/526: 로컬 날짜·KeySlot별10회. 다른 slot/작업폴더/다른 도구 사용량·Project·Model budget 미합산 | Project+Model 실제 quota/당일 전체 시도 이력과 승인 budget을 사용. Key2로 quota가 늘거나 한국 날짜가 reset이라는 가정 금지 |
| 필수 | L370–374: confirm-free-tier는 사용자 flag뿐; Billing/Project/실제 무료 잔량 검증 기능 없음 | 실제 계정에서 무료 범위/과금 연결/현재 quota 확인 후 승인1회만 배정. 미확인 시 호출0 유지 |
| 필수 | L433–462: IN_PROGRESS 사전 저장은 중복 방지에 도움. 실패/불확실도 영구 attempts에 남음 | 모델·SDK·style·원문 hash·요청/응답 MIME·WAV hash·승인 budget 기록, 실패/불확실은 자동 재호출 없이 조정 절차 마련 |
| 필수 | L518–525: 후속 invocation에서 이전 실패 ID를 건너뛰고 뒤 ID 호출 가능. 실패를 전체 quota 중단 lock으로 보존하지 않음 | 첫 quota 실패 후 미호출까지 전역 HOLD. 실제 상태 재확인과 별도 수동 승인 없이는 다음 ID/Key 교체 금지 |
| 필수 | L434–437: 같은 ID는 날짜와 관계없이 attempts가 있으면 거부. IN_PROGRESS crash/실패를 안전하게 수동 재개하는 기능 없음 | 성공 원본 보존, 실패·불확실만 이력 유지한 명시적 재개 계약. 상태 삭제/새 workdir로 우회 금지 |
| 필수 | L482–499: 현재 샘플을 state SAVED hash와 비교하지 않고 승인; L510/520은 존재만 검사 | 승인 직전 state↔sample hash 일치, 잔여 호출 전 approved sample hash 일치, 기존 신규 WAV도 hash 검증. 수동 교체/손상은 중단 |
| 중요 | L451–454: `wav_info` REVIEW도 result SAVED. approve도 FAIL만 거절 | quiet/clipping/duration/rate REVIEW를 성공/음색 승인으로 승격하지 말고 명시적 검수 보류 |
| 중요 | L402–420: RIFF 그대로 반환, raw PCM은 rate만 보고 mono16bit 가정·MIME 폐기 | 응답 MIME/채널/bit-depth/실제 bytes 기록·형식 검증. 원본 rate가 없거나 불일치면 중단. 오디오 보정은 별도 승인 |
| 중요 | L344–349: tmp→replace 방식, 프로세스 lock/schema validation/설정 fingerprint/fsync 없음 | 버전·대상·Voice·model·style·text hash를 검증하고 단일 writer lock. crash/불완전 파일을 성공으로 취급 금지 |
| 중요 | L83–86/221–229: 임의 workdir·경로 문자열·복사 | Unity/Save/기존 출력/ThirdParty와 겹치지 않는 독립 staging 및 link/containment 검증 |
| 보완 | L383–392/457–460: 키는 env/getpass, raw 예외를 기록하지 않고 error_type만 저장. stderr/unhandled SDK 초기화 예외는 별도 통제 없음 | 정상 소스상 키 literal/의도적 키 로그는 없음. 키·전체 env·SDK debug/raw exception/payload secret 로그 금지 유지, client 생성 실패도 비밀 제거 처리 |
| 보완 | L299–315/575–577: 정상5 청취 pending으로 표시, 향후 다른 Main Voice까지 확대하는 설명 | 기존 승인 상태 유지, 정상25/Main17 세린11 전체 보호표 추가. 다른 Main의 새 Voice 정책을 이 파일로 확정하지 않음 |

`attempts=1`과 현재 오류 중단은 좋은 방어 요소지만 정상 작동/무과금/안전 재개가 입증됐다는 뜻은 아닙니다. 과거 오류10의 실제 Voice/요청 payload는 UNKNOWN이라는 정본을 유지합니다. style 여성 명시는 조건 차이이며 원인 확정이나 해결 보장이 아닙니다.

## 미래 한 개 샘플 순서 — 실행하지 않은 절차

1. 위 필수 사항을 반영한 **Schedar 유지 별도 검토본**을 TTS 담당이 제출합니다. 새 SHA/AST/대상10/원문/경로를 다시 읽기 감사합니다. 현재 파일에 실행 명령을 권하지 않습니다.
2. 실제 Project+Model/SDK 버전, 무료 사용 범위·Billing 연결·남은 quota·당일 다른 도구 attempts를 확인합니다. 비밀키 값은 기록하지 않습니다. 미확인이면 중단합니다.
3. 정상25·세린5+7·Main17 세린11·오류 원본10·체크포인트·Catalog·Save 해시를 확보하고 금지 경로와 분리한 staging을 확인합니다. 쓰기/백업도 이 후속 작업의 승인 범위여야 합니다.
4. **사용자의 실제 생성1회 명시적 승인** 후에만 `main18_ground_serin_01` / companion_serin / Schedar / 인계 ExactText 한 대사를 **1 call → 1 WAV**, retry0으로 생성합니다. 호출 전 IN_PROGRESS 기록, 호출 후 결과·MIME·SDK/model/style/원문 hash·WAV hash를 비밀 없이 기록합니다.
5. 실패/quota/불확실이면 즉시 중단하고 성공·실패·미호출/총 시도를 기록합니다. Key 교체/뒤 대사 호출/자동 retry/파일 삭제 금지입니다.
6. 샘플 기계검사와 **사용자 실제 음색·원문 청취 승인**을 각각 기록합니다. 승인 전 잔여9 호출0. 불만족이면 원본을 유지하고 새 요청 승인을 별도로 받습니다.
7. 샘플 승인 후에도 잔여9는 **별도 생성 승인과 현재 quota 재확인**이 필요합니다. 전체 사용자 검수 뒤 Unity 연결은 별도 Codex 통합 작업입니다. 이번 조사에서 어느 단계도 실행하지 않았습니다.

## 검증 경계·후임

- PASS(정적): 존재/해시/UTF-8/AST,10 Stable ID·원문·원본 SHA/StoryOrder 대조. 문서 검사 및 보호 결과는 [공통 조사 보호 기록](Codex_ReadOnly_Audit_Protection_20261013.md)에 기록합니다.
- NOT_VERIFIED: SDK 설치/현재 모델·Voice 지원/실제 payload/응답/청구·quota/reset/실제 성공·실패 복구/음색·청취/Unity 새 Import·Runtime. 시험 호출로 확인하지 않았습니다.
- 다음 작업: TTS 담당이 필수 보완을 검토하고 Schedar 계약의 후보를 별도로 제출. 사용자의 새 실행 승인 전 HOLD를 유지합니다.
- 후임 시작 문구: “Main18 세린10 정적 감사 보고서와 CSV를 먼저 읽으세요. Staged 원본 SHA e358511d…는 Erinome/Kore 후보라 실행 보류입니다. Schedar 유지, 정상25/세린5+7/Main17 세린11 보호, 승인 전 수정·API·Unity 적용 금지입니다.”
- Commit SHA는 이 문서가 포함된 로컬 Git 이력과 최종 보고로 확인합니다. GitHub 반영/Push는 하지 않았습니다.
