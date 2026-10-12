# Codex 조사 전후 보호·검증 기록 — 2026-10-13

**문서 조사만 완료 / Python 원본·Unity 게임 파일 무변경 / TTS 실행0 / Push0.** A(세린 정적 감사)와 B(Field04 역할 조사)의 공통 증거입니다.

## 시작 기준·원격 확인

- 로컬 main HEAD `509886f85b6b50deb3f2b69ad7d1f1cc85945157`.
- GitHub 연결 도구로 확인한 최신 main `eca6f1e5ceb42e9d6c88b53368bc109280a547b8`. compare 결과 원격 ahead3/behind0, merge-base=로컬 HEAD; 차이는 문서9파일이며 Unity 게임 변경은 없습니다. 로컬 checkout/index에 원격을 병합하지 않았습니다.
- 요청한 원격 AGENTS/CODEX_WORKFLOW/CURRENT_STATUS/CODEX_LATEST 확인. 최신 업무지시·Chapter2 아트 로드맵·리뉴얼 담당 지침도 읽었습니다. 다른 담당에게 메시지를 보내지 않았습니다.
- 시작 git status --porcelain=v1 -z: 사용자337항목. staged index 비어 있음. CURRENT_STATUS에는 기존 사용자8줄 추가가 있습니다. 이 파일과 기존 인계 모두 byte 그대로 보호합니다. 기능 구현 없이 조사만 했으므로 CURRENT_STATUS를 덧쓰거나 사용자 변경을 커밋하지 않았습니다.

## 파일 보호 증거

문서 생성 전과 생성 후 아래 범위를 각각 읽어 SHA256을 계산했습니다. 디렉터리 digest는 **전체 파일을 절대 경로 정렬한 후 상대경로 UTF-8 + NUL + 파일 SHA256 raw bytes**를 연결한 SHA256입니다. count와 digest가 모든 그룹에서 일치했습니다. 단일 파일은 파일 byte SHA256입니다. 단순 git status 일치만으로 파일 무변경을 주장하지 않습니다.

| 보호 범위 | 파일 수 | 전후 동일 SHA256 |
|---|---:|---|
| Unity/Client/Assets | 3820 | `ddb1c108f6ebfda34a8ffdecede39fba5d5d45f6c657cc68318760882cc5aa21` |
| Unity/Client/Packages | 2 | `89480fd5fb05c85d5e6431aa475da6669d125c4e4f8116533a16031c2f70ceb5` |
| Unity/Client/ProjectSettings | 29 | `c79357acbc8734d6b3d298be7795ec3a5dab1e1bb817de93f825da7dace671d9` |
| Unity/Client/UserData | 23 | `6aaadcc302010dde2ca0b015fe51d4eb831e7c3b3bc410783c26ae3f610c4e9d` |
| Unity/PathVisualsOriginalBackup | 8 | `e2488b2c98a02ab88760734df05b95b11eb18b8d02a46bff126d0fd48cbce8b8` |
| Limitless_TTS_Output_Main18 | 26 | `73df77622952277e82bfc28048e939d9d82f2c0edc000cd2b3d24e014ad4bdc1` |
| Limitless_TTS_Output_Main19 | 15 | `592a540384996d6366ce405478cff032a5ad7da0fb394c179b26191eb6e81117` |
| F:/Downloads/M18_Serin10_Staged.py | 1 | `e358511d852a014f0c2196754debe523cd55336bda8a11d18717a3278276b10f` |
| AGENTS.md | 1 | `593a7ddaa7d3816ca97269dd445e27304cc401460567dc21c5d7d42c14aeb9f7` |
| 문서/00_프로젝트/CURRENT_STATUS.md | 1 | `a3ccd498f40589fa05f14d20fd9f69c247117eff24aee490d7c6e7af4752f273` |
| 문서/00_프로젝트/팀운영/인계/CODEX_LATEST.md | 1 | `5f35e25d8859c1ff774d162321582bd34f45c7b473ec551ec572b08e7da323d4` |
| _TTS_RESUME_2026-10-12.json | 1 | `e8b9b9bd62046e3344f5439ed7fa15a91a7deb3dbca427756ec47dee29ef597d` |
| TTS_PROGRESS_2026-10-12.txt | 1 | `d6ed88d081cb9cd8c25729044322145d3e00ff9f844ffbec336adcef5034dfa7` |

전체 그룹 합계 **3,929파일**(경로 중복 없음). Unity Assets에 사용자 승인 PNG·meta·Scene·Prefab·C#·TTS/BGM·ThirdParty가 포함됩니다. UserData의 Save/설정23파일과 외형 원본 Backup8도 포함합니다. 로컬 TTS 출력·체크포인트·진행 기록 및 다운로드 Python은 별도로 보호했습니다. **키 파일/Google TTS 디렉터리/환경변수 값은 읽지 않았습니다.** Library/Temp 전체의 무변경을 보증하는 검사는 아니며, 조사 명령은 그곳에 출력 파일을 생성하지 않았습니다.

추가 개별 대조:

- 오류10 원본 WAV 해시 = Schedar 인계 CSV SHA **10/10**, 원문·StableID10/10 일치.
- 정상 Main18 NPC12+Main19 NPC13 **25/25**: 실제 원본 WAV와 Unity WAV의 SHA가 기존 적용표와 일치. 정상 세린5+7 포함.
- Main17 승인 세린/Schedar **11/11**: 실제 Unity WAV SHA가 Main17 적용표와 일치. Main17 전체 승인14와 구분.
- Field04 Scene file SHA256 `fe0434fac5fba396ca24b12dbf6bd6686face743cdd58ff08b98a094f005ee55`; 원격/로컬 Git blob `b9f7219864b8380219e7326fdb78cfab1595e223` 일치.

## 포커스·실행 경계

Unity MCP **읽기 resource**만 사용했습니다. 시작/문서 생성 후 모두 연결1개, Unity6000.6.5f1/Bootstrap/Edit Mode/is_focused=false/is_compiling=false/is_updating=false. 상태 조회가 Field04 실행 검증은 아닙니다. 원본 Scene 로드/Import/Refresh/Play/선택/종료·Game View·스크린샷·OS 입력·foreground0입니다.

Python은 신뢰한 별도 검사 코드에서 대상 .py를 text/AST로 읽었을 뿐입니다. 대상 module import/function 호출/CLI phase 실행0, 키 읽기0, TTS API·네트워크0, WAV 생성/재생/변경0, 다운로드·설치0, 청구 발생시킬 요청0입니다. 실제 Project의 잔량/다른 작업의 청구 여부는 UNKNOWN입니다.

## 산출물 검증과 커밋 범위

- A: [안전 감사](Serin10_Staged_Safety_Audit_20261013.md), [대상10 대조 CSV](Serin10_Static_Target_Audit_20261013.csv).
- B: [Field04 조사](Field04_ReadOnly_Renewal_Mapping_20261013.md), [저장 Scene374행 CSV](Field04_Serialized_Object_Mapping_20261013.csv), [Runtime 보호18행 CSV](Field04_Runtime_Protection_Mapping_20261013.csv).
- 공통: 본 기록. **신규 문서6개만 선별 커밋**, A와 B는 구분된 Docs 커밋으로 저장합니다. 기존 사용자337항목의 status·보호 digest를 커밋 뒤 다시 대조합니다. git add -A/clean/reset --hard/push는 사용하지 않습니다.
- 문서 CSV 재읽기,행 수·중복 fileID/대상 ID·음성 대조 boolean·363 Renderer/21 Collider·Sprite GUID+signed fileID 해석,문서 링크·비밀 패턴·공백 검사 PASS. A 선택 staged diff --check PASS. SDK/게임 코드 테스트는 수정이 없고 실행 미승인 범위를 지키기 위해 실행하지 않았습니다.
- 미검증: 실제 TTS 호출·응답·결제/quota/SDK 호환·음색/청취·Field04 실시간 Hierarchy/Importer/Runtime/물리/Camera/Save 회귀/새 아트 가독성. 북쪽 경계 틈은 정적 우려이며 후속 격리 QA 항목입니다.
- A 로컬 Commit: `3a37aa2e9a972b1ff9de7c051c6c744b057c918f` (`Docs: 세린10 Staged 정적 안전 감사와 실행 보류 기록`), 문서2개. B/공통 문서4개 Commit은 본 파일을 포함하는 로컬 Git 이력 및 최종 보고에서 확인합니다. 원격 main을 병합하지 않은 상태에 로컬 문서 커밋만 추가합니다. 향후 동기화는 사용자 기존 변경을 보호하는 별도 작업입니다.
