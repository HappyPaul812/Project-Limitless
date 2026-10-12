# Codex 최신 인계 — 2026-10-12

역할: LIMITLESS Unity 구현·검증 담당. 대화 맥락 장기화에 대비한 로컬 인계다. GitHub Push0이며 사용자가 직접 수행한다. 새 세션은 AGENTS/팀운영 START_HERE·COMMON_RULES·담당 CODEX/PROJECT_CONTEXT/CURRENT_STATUS를 먼저 읽는다.

## 현재 완료와 승인

- Field01 복구, 하렌/세린 공식 아트, Main18 정상12·Main19 정상13 TTS 통합, Chapter2 종합QA/Chapter3 설계는 개별 보고서·CURRENT_STATUS를 따른다. 오류 Main18 세린10개는 REGEN_REQUIRED/Unity무연결·무음/Schedar 유지. 재통합·TTS API 호출 금지.
- Field02 USER_ART_APPROVED·정식Sprite12/Scene363+Theme10 연결 완료. 통합 `7b75d00`, 상태 `e31d8e3`; 사용자가 Push 완료, 이번 시작 ls-remote와 로컬HEAD e31d8e3 일치. Field02 재작업 금지.
- Field03 최신 검수 [정본](../../Field03_Renewal_Art_v1_Inspection_20261012.md): **USER_ART_REVIEW_PENDING / QA_PARTIAL / 영구적용0**. ZIP SHA333ffb32…1209d81, PNG29(실사용20/직접12+보조8). 실제Scene363+Theme13=376. 원본/후보Runtime·376CSV·Overlay·단계별확대 확보.

## Field03 핵심 인계

- Rock B 상단 분리14alpha픽셀, 본체 bbox(12,69)-(116,125)의 중앙아래 배치/접지 보완 권장. 나무 밝은 외곽·기존Tint 암부·반복감·Field02 연속성 검토 필요. PNG 자동 수정/재생성 금지. 보조8종 미배치, 입구 항상표시 금지. 출처/권리 독립감사 미완료는 Manifest에 기재됨.
- Main08 Field03 조사/깊은고요, Main09 6목표·폴해금, Main10 7목표·입구0~3숨김/4이후표시, BootstrapContinue3회, DungeonB1진입/Field03귀환, Field02왕복 PASS.
- 기존Field03 몬스터6(거미1/뱀1/갑충2/박쥐2)의 SpawnDefinition은 **전부35초**다. 사용자요청30초와 다르므로 값을 변경하지 않았다. 6개 정상UI전투승리/30초미생성/35초생성·중복0 확인. 이 불일치를 다음 통합에서 무단으로30초로 바꾸지 않는다.
- 실제Player Circle반경0.5+margin0.03·NPC Collider 포함11지점 물리 접근, 실제16:9 Camera4방향 Bounds PASS. QA의 잘못된30초 기대값·반경0.4/NPC제외 탐색 실패는 원시로그를 보존하고 검사만 보정했다. 목표중심이 막히면 상호작용범위1.6 이내(검사0.9 이내) 접근점을 사용한다.
- 기존 Unity Search 내부 예외2이슈 유지. 전체 PARTIAL. OS직접입력·사람미술/가독성·전체레벨/직업·연속사냥밸런스 미검증. 정상전투는 실제 UI버튼/대상HitArea/TurnQueue 경로이며 결과주입과 구별한다.

## 로컬 보호와 검증 경로

저장소 `F:/study/codex/Project-Limitless`. 원본 Unity PID31832를 조작하지 않는다. 미커밋337항목 유지(CURRENT_STATUS 사용자8줄 포함). Field03 시작 보호게임3712파일 전부byte 동일. Save·meta/GUID·승인아트·TTS/BGM·ThirdParty 원본 보호.

기존 격리 `Temp/Field01RenewalQA/Client` 재사용. Field03 결과 `Field03Inspection_20261012`, 접근/Camera 결과 `Field03Viewport_20261012`. 스피커0/별도 AuditSaveDirectory·Settings/최초쓰기 경로 검증. QA C#는 격리복사본에만 있다. 원시로그·스냅샷·재실행 스크립트 `Temp/Field03Inspection20261012/` 보존. 검수 증거는 정본의Evidence 링크.

이전 Git 통합worktree `Temp/GitIntegration20261012/main-merge`는55ac89f에 남아 있으며 무단삭제 금지. 원본main은 승인된 안전FF 후 Field02/검수 Commit을 쌓는다. reset/clean/add-A/Push 금지, 선택적 Commit만.

## 다음 순서

1. Field03 미술보완/사용자승인을 확인한다. 지금은 승인 대기이며 실제게임Asset에 추가하면 안 된다.
2. 승인 범위가 정해지면376CSV로 Sprite-only 별도 통합, 원래좌표/Collider/Tint/스토리·35초 타이머 보호. 보조8종은 별도 승인 없으면 추가하지 않는다.
3. 기존Search 예외는 독립 작업으로 조사한다. 다른 완료 기능을 재작업하지 않는다.

새 세션 복붙: **“너는 LIMITLESS Codex 담당이다. AGENTS와 팀운영/인계/CODEX_LATEST.md 및 CURRENT_STATUS를 읽고, Field03 미술 승인 상태를 확인한 뒤 허용된 범위만 이어 진행해 주세요. 원본Editor·Save·337개 사용자 변경 보호, Push/TTS API 금지.”**

마지막 검수 Commit은 CURRENT_STATUS 및 본 파일의 후속 SHA 기록을 확인한다. 이번 문서는 로컬 저장이며 원격 반영됐다고 주장하지 않는다.
