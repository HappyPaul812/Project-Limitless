# LIMITLESS — Field09 v2 시각검수 재실패 후 환경 미술 재제작 전환안 (2026-10-20)

- 작성: 기획, 브랜치 `docs/planning`.
- 사용자 질문: **“이럴 바엔 다 엎고 다시 하는게 더 안낫니?”** (Codex v2 Unity 검수 보고 직후)
- **판단:** 기존 Field09 v1/v2 후보를 다시 픽셀 자동 필터링·가이드선 추적·지면 외곽 블렌딩하는 방식의 **추가 반복은 중지할 것을 권고**한다. 작품의 핵심 미술을 재설계하는 것이 시간/QA 면에서 유리하다. 그러나 기존 Scene·퀘스트·과열·스폰·Save·전투·기존 공식 자산 원본은 폐기하지 않는다.
- **상태:** `ART_REDESIGN_RECOMMENDED / NEW_ART_PRODUCTION_AWAITING_USER_DIRECTION / ORIGINAL_UNITY_UNCHANGED / MAIN_V2_NOT_APPLIED`. 질문을 곧바로 추가 제작/Unity 병합 최종 승인으로 간주하지 않는다.

## 현재 증거·근본 문제

Codex의 v2 보고: GitHub ZIP SHA/CRC/Manifest/PNG8 및 격리 Unity 변경분 **40 PASS/0 FAIL**, 기존 Main18 기능 **92 PASS**, 하지만 **지면 회색 반복띠6 + 소품6종 흰 가이드선 잔존** 때문에 시각 QA 불합격. 원본 적용0, 원본 보호 5,346+·UserData23이라고 보고. ZIP 문제를 다시 고쳐도 해결되지 않으며 v1·v2 모두 서로 다른 방법으로 그림 픽셀을 수정했음에도 동일한 사용자 화면 결함이 남아 있다.

`docs/art-renewal` 실물 v2 납품 SHA `30f102c223aa127a7b6a290ef9fa503354cb9e0104faac03e6c395c2b247b8b8`은 `Art_Deliveries/Limitless_Field09_ObsidianScar_Minimal_Fix_v2.zip`에 증거/기록으로만 보존. 만약 흰 선/반복띠가 PNG 자체가 아닌 Unity Tiled Renderer/Material/UV/Overlay, 중복 Sprite 또는 씬 오브젝트에 기인한다면 **그 원인을 구별하지 않고 새 그림만 계속 만들어도 실패할 수 있음**. 따라서 기술원인 좁힌 테스트와 본격 미술 새 제작을 분리한다.

## 전환 기획

**아트 전면 새 제작 = 화면을 구성하는 잘못된 Field09 환경 텍스처/소품을 새 원화에서 재디자인**. 두 개의 보호 PNG `ObsidianScar_AshPatch_01.png`, `ObsidianScar_Ground_Cracked_02.png`는 무조건 다시 만들지 않고 실제 Runtime 사용/역할과 새 아트와의 색감을 보고 재사용 여부를 먼저 결정. 새 환경 렌더러/퀘스트/맵 레이아웃/동선/월드 분기·기존 Sprite GUID/meta를 초기화하거나 기존 Unity 씬 전체 삭제하지 않는다.

**기준 스타일:** Chapter2의 Field07 회재·Field08 붉은 균열 이후 이어지는 진한 현무암 균열 지대, 색·실루엣·다양한 질감과 가독성·접근성 유지. Field09는 Main18 과열/불타는 지반의 감각을 주지만 Field10 이상의 표현을 미리 훔치지 않는다. 풀 화면을 같은 흑/회색으로 단순 타일 반복하거나 흰 보조선으로 geometry를 표시하지 않는다.

**제작 순서 (단계 게이트):**
1. **원인 1회 비교**: Codex가 현재 v1/v2 실패 화면의 `SpriteRenderer`/Tiled 반복 UV/Material/Sorting/Overlay, 씬 표시 오브젝트, 실제 PNG를 짧게 측정하여 결함이 Asset 픽셀·소품 내 가이드인지, 렌더링/Material인지 분리. 기존 QA40+92 재실행 금지. 그림의 Source 정상 여부를 구분하고 미술 담당에게 짧은 1페이지로 전달. 원본 Editor/Save 비접촉.
2. **3종 샘플 우선**: 리뉴얼 아트 담당이 완전히 새로 설계한 **Ground Base 1**, **Cracked Overlay/균열 Ground 1**, **가이드선이 없어야 할 대표 소품 1**의 **실사용 PNG**를 제작. 픽셀 보정/Nearest 업스케일/기존 v2 위 덧그리기 반복 금지. 기존 충돌·씬 배치·PUU/Pivot/Tiled 사용 방식은 실제 Source Mapper를 따라간다.
3. **Unity 단발 시각 게이트**: 샘플3을 별도 격리 Scene에 잠시 연결해 동일 Camera/Zoom/3화면비에서 (a) 타일 반복경계0 (b) Ground/Overlay 이종 이음0 (c) 흰 가이드선0 (d) 퀘스트 접근/과열 VFX 가독성 (e) 타 필드(07/08) 미술 일관성을 실제 화면으로 확인. **시안 PNG만 보고 전체 아트 제작 시작 금지**. 동작/UV 문제가 기술적이면 Codex 최소 runtime fix를 제안, 리뉴얼 PNG를 더 양산하지 않는다.
4. **나머지 필요 환경 PNG 제작**: 샘플 미술+Unity QA가 합격하면 나머지 소품 및 필요한 배경 자산만 새로 제작, 동일 원화·색감/PPU/접지/애니메이션/라이트 기준을 유지. 기존 공인10 PNG/meta는 별도 폴더로 바이트 보존하고 **독립 새 Asset+new GUID**로 참조 변경하는 방법 우선. 전체 새 ZIP는 표준 `zipfile.testzip()`+CRC/SHA+파일별 alpha/행 프레임/Changed source mapping과 비교 프리뷰를 포함.
5. **최종 Unity 검증/승인**: 리뉴얼 art 납품→바울 대표 미술 승인→Codex 별도 `codex/*` QA (Main18 13목표·과열·세린·몬스터/Save·Continue·휠체어·Field08↔09 진입/출구/3비율 최소 delta)→안전한 원본 디스크 적용 조건부; GitHub main 직접 commit/Push 불허.

**장애물:** 원본 GitHub `main`에 공식 PNG10/.meta10 존재 확인 완료. 자료가 없어서 새 프로젝트부터 재구축한다는 말은 사실이 아니다. 필요 시 Codex가 PNG/Scene 캡처만 읽기 전용 전달, 사용자 재업로드 요구하지 말 것.

**기획 결론:** v1/v2를 추가 수리하는 방식은 폐기. Field09 **미술 중심 교체(기존 Scene/기능 보존)**가 합리적. 다만 대표가 이 전환을 최종 지시하기 전에는 신규 PNG 전체 양산·Unity Scene 확정적 변경을 착수하지 않는다.
