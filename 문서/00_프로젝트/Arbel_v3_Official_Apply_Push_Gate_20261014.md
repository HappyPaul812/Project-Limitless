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

## 2026-10-14 Codex 공식 적용 게이트 실패: 세린 지붕 깊이감 (최신)

바울 대표가 Codex 격리 재검증 결과를 직접 전달함. **필수 시각 게이트 FAIL, 공식 적용·Commit·Push 중단**은 올바른 조치. 이전 조건부 적용/Push 허가는 이 게이트가 PASS하기 전에는 **실행 불가**이며 다시 임의로 사용하지 않는다.

### 실패의 성격과 성공한 범위
- 격리에서 후보 직접 교체23개·원본 Renderer284/284·MissingSprite/Script0, 기술검사 **905 PASS**, 실제 Unity 캡처17장, PNG 재제작0.
- **원인:** 지붕 뒤 Sorting으로 두면 세린 대부분이 가려지고, 앞으로 올리면 세린이 지붕의 윗면에 서 있는 것처럼 보임. 두 Order30 실험값은 정상 시각 해법 아님. 새 기능 회귀 검증에 착수하지 않음.
- 양쪽 출입문, 바닥·도로·소품 접지는 기술 후보를 유지. 기존 Portal-나무 겹침은 별도 OPEN.
- 사용자 원본5,223파일 SHA/HEAD/Git dirty, UnityEditor·Play·포커스·Save 보호, 정식 적용0/Commit0/Push0/main변경0 보고.

### 후속 기획 판단: 깊이 표현 구조를 해결

1. **Codex 담당 우선 (읽기/실험 전용):** 격리 Unity에서 세린의 실제 **발 위치, 이동 경로, 지붕 Sprite의 투명/불투명 범위, 지붕/벽의 각 오브젝트 SortingLayer/Order, 자산 PPU·Pivot·Renderer bounds, 캐릭터 앵커/SortingGroup/시각 깊이**를 분석. 세린이 위치별로 지붕 앞·뒤 중 어느 편에 있어야 하는지 실제 카메라 좌표로 표시. 임의 NPC Transform/Collider 이동 금지.
2. 구현 대안 중 **(a) 자연스러운 앞/뒤 깊이 순서의 발 앵커 기준 Y-Sort 및 SortingGroup**, **(b) 지붕/처마 레이어 분리(배경 지붕과 앞쪽 가림), (c) 진입 때 지붕 투명 전환/occluder mask**를 비교해 최소 범위를 정한다. 모든 장면/카메라에서 일관적이어야 하고, 단순 Order 숫자 올림/내림만 반복하지 않는다. 다른 NPC·주인공(도보/휠체어)와의 상호작용/실내외 경계를 포함한다. 런타임 오브젝트를 가리는 방식은 가독성·광과민 접근성도 평가.
3. **PNG 픽셀 분할/전경 레이어 아트 신규 제작이 필수이면 리뉴얼 아트 담당 `docs/art-renewal`에 해당 지붕 **최소 필요한 층/투명 마스크·구분 영역/정확 크기**의 수정 지시와 검수이미지 생성 요청**. 기존 PNG 43종 전체 리뉴얼 재제작, 기존 승인 작품·GUID 덮어쓰기, 선택 장식11 임의 설치 금지. Codex가 미술을 임의 제작하고 아트 승인했다고 선언하지 않는다.
4. 격리 Unity에서 비교 화면을 생성해 사람의 눈으로 판정 가능하도록 **세린이 지붕 뒤로 가는 위치/건물 옆/문 앞/지붕 앞으로 나오는 위치/실제 상호작용 위치**를 최소 4장 이상의 고정 카메라 비교로 제시. 세린이 지붕 위에 선 것처럼 보이는 화면0, 건물 벽/문 가림 재발0, NPC 기능/접근성·기존 우회경로 유지.
5. 원본 프로젝트 공식 적용과 전용 작업 브랜치 Commit·Push, PR/main Merge는 **필수 시각 게이트 PASS 및 바울 대표 화면 재확인 후** 조건부 허가를 재판정한다. 905 PASS 같은 기술검사만으로 PASS 승격 금지. 원본 Editor/Save/Scene 등 계속 보호.
6. 이 단계에서 Codex는 **진단과 격리 수정후보/아트 담당용 필요자료 인계까지만** 수행한다. 실제 PNG 새 파일 제작은 담당 승인·지시 후. TTS Main08 태온005·006 분리 작업은 이 리뉴얼 범위와 별개.

**최신 상태: `OFFICIAL_APPLY_BLOCKED / SERIN_DEPTH_VISUAL_FAIL / TECH_QA_905_PASS / ORIGINAL_UNCHANGED / ART_FIX_FEASIBILITY_TO_CHECK`.**

