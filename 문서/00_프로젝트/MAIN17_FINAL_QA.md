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
