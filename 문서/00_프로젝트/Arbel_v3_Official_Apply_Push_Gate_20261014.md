# LIMITLESS — 아르벨 v3 정식 적용·기능 QA·작업브랜치 Push 게이트

- 작성일: 2026-10-14
- 책임: 기획
- 브랜치: `docs/planning`
- 상태: `USER_AUTHORIZED_CONDITIONAL_OFFICIAL_APPLY_AND_FEATURE_PUSH / NOT_EXECUTED / MAIN_MERGE_NOT_AUTHORIZED`

## 요청 범위

바울 대표가 아르벨 v3 격리 시험 3,245 PASS와 세린 깊이감 등 잔여 OPEN 보고를 받은 뒤 **“넘어가자.”**고 지시했다. 따라서 기존 아트 디자인 방향 승인과 결합하여 **남은 필수 게이트를 통과한 경우에만** Arbel v3 직접 교체후보23종의 사용자 원본 Unity 디스크 정식 적용, QA, **Codex 전용 feature 브랜치 Push**까지 진행하도록 허가한다. `main` 직접 commit/Push, PR 병합, 선택 장식11종 배치, 다른 지역·게임플레이 임의 변경은 승인하지 않는다.

## 근거와 아직 끝나지 않은 게이트

- Codex 제공 격리 결과: 3,245 PASS/0 FAIL, Scene Renderer284/284, Sprite/Script Missing0, PNG 변경0, 23개 교체 후보, 선택 장식0, 실제 Unity 캡처55장. Sorting 계층만 바꾸어 양쪽 건물 벽·문 가림 해결, 소품9 실제 Alpha·Collider 높이 검사, 주변 우회 도보/휠체어 5/5.
- **세린과 북서 건물 지붕의 앞/뒤 깊이감**이 모든 관련 NPC 위치·상호작용에서 자연스럽고 문이 가려지지 않는지 확인 필요. 단순 '화면에 보인다'만으로 PASS 금지. Sorting 해결이 안 될 경우 범위 내 최소 보완 후 캡처 및 재검사, PNG 재작업 필요하면 아트 담당에 분리하고 **정식 적용은 중단**.
- **Portal(-7,-4)·TreeSouthWest(-7,-4.2)** 기존 겹침은 이 리뉴얼 신규 결함 아님. Exit/상호작용·휠체어 실제 우회 동선 장애를 유발하지 않는다는 검사에 한해서 별도 OPEN 유지 가능. 핵심 접근이 불가능하면 공식 적용 차단. 원본 Collider나 나무 임의 삭제 금지.
- **ZIP 해시 불일치 중요**: 검증된 v3 소스는 SHA256 `84e9707df7f0409e0238781f0860499dd4344b79ec88b49964a7ce7d8e10633b`의 보존본이며 현재 Downloads 경로 파일은 다른 해시(보고상 `58fb22...`)이다. 원본 대상 자산 선택 전, 실제 사용되는 ZIP/Extracted Manifest/PNG43 SHA/문서 11개 차이 정확 확인. **정확한 검증본에 기반한 적용만 허용**하고 버전 혼합·임의 overwrite 금지. 적합한 검증본을 못 찾으면 멈추고 보고.
- 원본 Unity 디스크 적용 전 원본 5,223파일의 SHA, HEAD, Git dirty, Scene/Asset/Save/Settings/Voice/BGM/기존복구 Sprite 원본 백업/롤백 경로 확립. 원본 Unity Editor/Play/OS 포커스 변경 금지.
- 변경 내용은 `Arbel` Scene의 후보23 Sprite 연동 + 검증된 최소 Sorting 계층 및 필요한 신규 _Project 아트 자산/GUID/meta로 제한; **기존 Story/Quest/Portal/Collider/Camera/NPC Transform/Save·Main08 승인6개·현재 미해결 태온005/006·Field04 Road42/Renderer375·Chapter3 기획** 보호.
- 격리 Unity 사용해 후보를 재현 검증 후 원본 디스크 적용. 실제 원본 Editor 화면을 강제 Refresh/Play/Scene 전환하지 않는다. 즉시 반영 화면이 아닌 파일 디스크 적용과 실시간 에디터 메모리 갱신은 별도 보고.
- 최종 게이트: Arbel 284/284, 기존 23 BuildScene 참조/Script Missing0, Arbel 카메라/Sorting/문·NPC·소품 시각; 도보·휠체어 접근/Portal 기능/상점·치유·펫·퀘스트/Save·Continue; Field04·Main08·타 Scene 주요 회귀; Preview 아닌 진짜 Unity Run 캡처, `git diff --check`; 기존 Unity Editor 버전/검색 예외/수동 조작 미검증은 PARTIAL로 분리. 새 regression0, 기존 OPEN 상호작용 장애0 필수.
- QA 통과 시 `codex/arbel-v3-official-apply-20261014`같은 **Codex 작업 전용 브랜치**에만 정확 변경을 Commit·원격 Push. `main` 병합은 하지 않고 PR/브랜치/커밋/아트 적용 목록·실제 QA·보호 결과 보고. QA 실패 시 안전 상태에서 중단하며 `main` 불변.
- 사용자 승인 범위는 v3 기본 후보23종의 조건부 정식 적용, 선택 장식11종은 추가 사용자 승인까지 **0배치**이다.

## 후속

아르벨 기술 적용 보고와 사용자 실제 화면 확인 후 기획 담당이 최종 승인/미해결 목록을 문서화한다. TTS 오류 추적은 별도 담당 업무이며 이번 Scene 적용과 혼합하지 않는다.
