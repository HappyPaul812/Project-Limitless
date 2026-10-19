# LIMITLESS — Main10 복구 음성 18개 청취 승인 · Field08 신규 제작 생략 기획 결정

날짜 2026-10-19 / 담당 기획 / 브랜치 docs/planning.
상태: MAIN10_USER_LISTENING_APPROVED_18_OF_18 / MAIN10_QUEST_FIX_ISOLATED_QA_PASS / MAIN10_VOICE_UNITY_APPLY_PENDING / FIELD08_PLANNING_NO_NEW_ART / FIELD07_FIELD08_BRIDGE_CHECK_CONDITIONAL.
본 문서는 기획 결정 기록이며 Unity 원본 적용, Push, main 병합을 실행했다는 뜻이 아니다. Field08 NO NEW ART는 기획 담당이 리뉴얼 담당 감사 권고를 수용한 판단이며 사용자에게 실제 제작을 새로 요구하지 않는다.

## 1. Main10 청취 승인 18/18

바울 대표 Codex 보고: codex/main10-voice-quest-interaction-fix-20261019, 기능 5c36a6531f23550c535737a96e1cd18648229f6b, QA인계 b9516bc8408ec91b13dd50f9be7d222f6aaa05fc.
현재 조사 목표가 인근 치유사 NPC 입력보다 우선하도록 수정, 치유사 서비스 유지. 격리 Runtime QA 132 PASS/0 FAIL: 키보드 E/F·게임패드 A, Quest 7목표, 40EXP/40탈렌트, Continue, 던전 입장 복귀 및 3화면비. 원본 Unity 실제 사용자 Editor QA는 별개.
기존 WAV 내부 다른 문장 중첩/분할 문제를 발견하여 기존 PCM에서 후보 18개 복원. 새로운 TTS·API 호출0, 기존 WAV overwrite0, Main09 승인27/UserData23 및 보호대상 4,235개 원본 SHA 불변 보고. ASR 일치 11개, 우선 청취 7개 중 마지막 '음...' 후보 포함.
바울 대표가 '다 들었고 다 맞고'라고 확인했으므로 복구 후보 18개 모두 USER_LISTENING_APPROVED. 개별 WAV SHA와 원문/화자/ID 매핑은 Codex 보고의 최종 파일을 정본으로 하고 기획이 새 SHA를 추정하지 않는다.
다음 Codex 계약: 승인된 18개만 별도 복구 WAV 경로와 StoryVoiceCatalog에 정확히 연결하고 원본 WAV·meta/GUID·기존 Main09 승인본은 보호. ID↔화자↔자막↔PCM/샘플길이↔GUID/meta↔SHA와 다음/빠른 Next·Continue를 변경분만 delta QA. 132 기존 QA 전체 반복 금지. 원본 dirty/UserData/Editor 포커스 및 Scene 보호·충돌없음·백업 확인 후 비파괴 원본 디스크 적용 가능 여부 별도 보고. TTS API0. PR/main 병합은 별도 승인. 사용자 청취 승인은 Unity 원본 적용 완료와 다르다.

## 2. Field08 RedRift 환경 신규 제작 생략 — 기획 승인

정본: docs/art-renewal/문서/00_프로젝트/Field08_RedRift_ExistingOfficialArt_Audit_20261019.md. 감사된 Field_08_RedRift Scene 저장 GameObject428, SpriteRenderer375(현재 표시7), MeshRenderer45(현재 표시30), 활성표시 Renderer37은 기존 공식 Main17 아트 사용.
공식 환경 PNG9종 중 7종 실제 사용, CrackedGround2종 의도적 미사용. 원본1254px/png, Material, GUID/meta, Quest/Collider/Save/세린/몬스터/BGM/TTS 모두 보호. 기존 Main17 QA270 PASS는 역사 결과이며 새 검증으로 중복 계수하지 않는다.
신규 제작 필요 근거0: Field08 새로운 PNG0, ZIP0, 공식 자산 재제작0. 현재 아트 그대로 유지. 미래 조건부 검토만: 승인된 Field07 v1이 격리/원본 Unity에 실제 적용된 뒤 Field07 서쪽(-8,0)↔Field08 동쪽(+7,0) 실제 카메라/색상/타일 연결 확인. 명백한 결함 때만 최소 경계 Decal 또는 기존 Material Tint를 별도 승인받아 처리. 확정 결함 전 추가 미술발주 금지.

## 3. 다음 실행 순서

Codex는 Main10 승인 18음성 정식 연결 및 delta QA 우선, 이후 Field06 v1.1과 Field07 v1 통합 및 Field07/08 경계 검수. 기존 v1 QA 재실행 남발 금지. 리뉴얼 담당은 Field08 신규 작업 SKIP 결정을 docs/art-renewal에 기록하고 다음 Field09_ObsidianScar의 기존 공식/임시 환경 자산 읽기 전용 감사만 진행. Field09 새 PNG·ZIP 제작은 별도 승인 전 금지. 기존 Chapter3 신규 아트는 별도 담당.

GitHub docs/planning에 문서만 기록. Codex/리뉴얼 세션 자동 알림·Unity 원본 편집·main Push/병합은 실행하지 않았다.
