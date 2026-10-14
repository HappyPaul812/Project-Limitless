# LIMITLESS / 아르벨 v3 사용자 미술 검토 승인과 남은 화면 가림 후속

날짜: 2026-10-14 · 담당: 기획 · 브랜치: docs/planning

**상태: USER_VISUAL_DIRECTION_APPROVED / ISOLATED_TECH_PASS / VISUAL_PARTIAL / OCCLUSION_FIX_PENDING / ORIGINAL_UNITY_NOT_APPLIED / PUSH_NOT_AUTHORIZED.**

## 사용자 확인 및 승인 경계

바울 대표가 아르벨 v3 **격리 Unity 실측 화면·비교 이미지**를 직접 보고 **“봤어. 좋아”**라고 답했다. 따라서 **전반적인 v3 환경 미술의 색감·분위기·디자인 방향은 사용자 승인**으로 기록한다. 이 발언만으로 아래의 가림 문제를 해결·승인하거나 선택 장식11종 배치 및 원본 Unity 정식 적용/브랜치 Push/main 병합을 승인한 것으로 확대 해석하지 않는다.

사용자가 전달한 Codex 격리 QA 결과:
- 실제 격리 Unity 스크린샷 **39장** 제작, **기술 PASS / 시각 PARTIAL·HOLD**.
- 직접 교체 후보 **23종**, Scene Renderer **284/284**, 선택 장식 **0개**, 23 BuildScene 참조 **3,654개** 정상, Missing Sprite/Script **0**.
- **가시 결함:** 북서 건물 지붕이 **세린**을 가림. **양쪽 건물 벽이 출입문을 가림**. 이 두 범주는 정식 적용 전 보완·재검수 필요.
- **HOLD:** 소품 실제 본체 접지, 두 지붕 가림, Portal↔나무 기존 충돌. 휠체어는 6/8 지점 도달 보고, 나머지 2곳은 기존 Collider 영향으로 보고되었으나 접근성 PASS로 처리하지 않는다.
- 원본 **5,223파일 변경0**, 원본 Editor/Play/포커스 조작0, 공식 적용0/Push0. ZIP 검증 결과 **Downloads 현재 ZIP의 SHA가 기대 SHA와 달라**, Codex는 기대 SHA와 일치하는 이전 검증본으로 시험했다고 보고함. PNG 픽셀 차이0, 문서11개 차이. **정식 적용 후보는 정확한 ZIP 경로·SHA 및 선택 기준을 다시 확정**해야 한다.
- 전체 퀘스트·전투 회귀와 정본 Unity 버전은 이번 격리 시험 미완료; 기술 미술 QA=전체 런타임 QA 완료가 아님.

## 후속 작업 지시 원칙

1. **Codex는 기존 승인된 PNG 픽셀과 284 Renderer의 원본 Scene 배치/Collider를 변경하지 않은 격리 환경**에서 먼저 가림 원인(투명 영역·Sorting Layer/Order·Sprite Pivot·Bounds/Renderer 대상·런타임 NPC 위치)을 진단하고 최소안 비교 캡처.
2. 북서 지붕↔세린, 좌우 벽↔실제 문이 **기능적으로도 보이고 접근 가능하도록** 수정안을 제시. 실험적 Sorting/Layer가 다른 지붕·NPC·건물·광장 가림을 만들지 않게 여러 카메라/상호작용 상태 검사. 오리지널 PNG 자체 픽셀 편집이 필수라면 미술 담당에게 **개별 픽셀 수정 요구 및 사용자 추가 미술 확인**. 기존 v3 전체 다시 그리지 않음.
3. 소품9종은 단순 Alpha 최하단·그림자 접촉과 물리 본체 접지를 구분하여 실제 Unity/Collider/이동을 재검증. Optional11 미배치. Portal(-7,-4)와 TreeSouthWest(-7,-4.2) 기존 겹침은 분리된 기술 OPEN 유지, 새 장식으로 가리지 않는다.
4. 화면 수정 비교를 바울 대표에게 제공하고, 완료 후 **원본 디스크 공식 적용 및 작업 브랜치 Push의 별도 실행 승인**을 받을 것. 승인 없이 자동 적용·Push·main Merge 금지.
5. 항상 사용자 원본 Unity Editor/Play/화면 포커스, 미커밋, Save, CurrentStatus, Story, Main08 음성·Field04 승인 아트·복구 Sprite/GUID/meta를 보호. 별도 worktree와 Background batch/RenderTexture 사용.

## 역할

- **리뉴얼 담당**: 사용자 v3 미술 방향 승인 보고를 읽고 docs/art-renewal 최신 인계에 기재. 필요시 **가림을 피하는 해당 지붕/벽 그림만** 최소 수정 제안. Chapter3 신규 아트 영역 침범 금지.
- **Codex**: 격리 원인 진단·오클루전 수정 실험·시각/기능 QA 및 실제 캡처. 승인까지 원본 프로젝트 영구변경0.
- **기획**: 승인 경계·기술 HOLD·디자인 변경/부서 승인 관리. 수정본 직접 확인 후 다음 정식 반영 단계 승인 요청.


## 2026-10-14 후속 Codex 격리 보완 보고 — 기술 3245 PASS / 시각 PARTIAL

바울 대표가 새 Codex 격리 Unity 보완·캡처 보고를 전달. 사용자 로컬 화면 보고를 기록한 것이며 이 기획 세션이 화면55장을 직접 다운로드·정밀 비교하거나 로컬 Unity를 실행해 별도 판단한 것은 아니다.

- **변경 형태**: 원본 PNG 픽셀 수정0; 격리 Scene에서 SpriteRenderer Sorting 계층을 최소 보완. 기존 23개 직접 교체후보, Scene Renderer284/284, Optional 장식0.
- **보고 QA**: 3,245 PASS / 0 FAIL, Missing Sprite/Script0. 실제 Unity 캡처 **55장**.
- **개선 PASS**: 북서 지붕의 세린 완전 가림에서 세린 시각 표시 가능으로 변경; 양쪽 건물 벽이 가리던 문이 이제 보임. 소품은 그림자 Alpha 영역과 불투명 본체 근사 접지선을 나누어 검사했고, Collider가 있는 **9개 배치는 높이 범위 검사 PASS**.
- **접근성**: 기존 Barrel·파티 안내판·Chest Collider가 일부 직선 경로를 차단. 보고된 주변 정규 동선에서는 도보·휠체어 모두 **5/5 지점 우회 도달 가능**. 특정 6/8·2/8 이전 보류를 무조건 모든 입구/실기 조작 완전 PASS로 치환하지 않는다. 우회 거리·인지부하·원본 수동 입력은 별도 점검 권고. 해당 Collider를 임의 제거/이동하지 않음.
- **OPEN/HOLD**: 세린의 지붕 앞뒤 **깊이감** 표현은 추가 시각 확인 필요(지붕 뒤에 있어야 하는 위치에서 앞에 나타나는지, 문 출입 시 겹침 문제가 재발하는지). 기존 Portal-나무 겹침은 별도 OPEN 유지. 전체 퀘스트·전투 회귀와 정본 Unity 버전 검증은 이번 격리 보완으로 완료됐다고 간주하지 않는다.
- **적용 자산 출처**: 기대 SHA256 `84e9707df7f0409e0238781f0860499dd4344b79ec88b49964a7ce7d8e10633b`의 **이전 보존 검증 ZIP**으로 보완 작업을 수행했으며 현재 `Downloads` ZIP은 `58fb22...`로 불일치(보고상 문서11개 차이, PNG43 동일). 정식 적용 전에 정확한 후보ZIP 출처·SHA/Manifest를 다시 확정해 두 버전을 임의 혼용하지 않는다.
- **원본 보호**: 원본 5,223파일 변경0, 사용자 Editor/Play/포커스 조작0, 공식 Unity 디스크 반영0, Git Commit·Push·main Merge0.

**기획 판정:** `ISOLATED_TECH_QA_PASS / DOORS_AND_PROP_QA_PASS / SERIN_DEPTH_VISUAL_HOLD / PORTAL_TREE_EXISTING_OPEN / USER_APPROVED_ART_DIRECTION_ONLY / OFFICIAL_APPLY_NOT_AUTHORIZED`.

**다음 최소 조치**: Codex가 격리 환경에서 세린/지붕의 앞·뒤 경계와 캐릭터 이동 시 표시·출입문 가시성(여러 위치/화면비·실제 카메라) 확인 후 보완 Sorting 명세 + Before/After 비교를 제출. 필요하면 art renewal 담당의 지붕 부분 픽셀 조정 제안으로 분리. 기존 Portal 겹침은 새 아트의 문제로 섞지 말고 기존 기술 이슈로 별도 기록. 바울 대표가 실제 비교를 다시 본 뒤에만 정식 적용 및 feature-branch Push 범위를 승인한다.
