# LIMITLESS / Field09 새 미술 3종 샘플 제작 규격 및 Unity 검수 계약 — 2026-10-20

- 기획 브랜치: `docs/planning`; 아트 담당: `docs/art-renewal`; Unity 검증: Codex `codex/*`.
- 바울 대표 Field09 환경 미술 재제작 전환 승인 이후 Codex 기술 보고를 인수한다. 사용자 스크린샷: **"샘플3 기술 인계서 정리"**, Codex 로컬 문서 커밋 `0dcc13ab`(로컬 보고·Push 안 함). 현재 기획 세션에서 해당 로컬 커밋을 직접 읽었거나 실제 게임 화면 검증을 수행한 것이 아니다.
- **상태**: `ART_REDESIGN_APPROVED / TECH_HANDOFF_REPORTED / SAMPLE3_ART_PRODUCTION_AUTHORIZED / SAMPLE3_REAL_PNG_NOT_DELIVERED / SAMPLE3_UNITY_VISUAL_QA_PENDING / ORIGINAL_FIELD09_NOT_APPLIED`.
- 과거 Field09 v1/v2 부분 보완 미술 실패: PNG 반복 경계의 Tiled 반복이 회색띠, 불투명 지면 간 접합이 눈에 띄는 경계, 소품 6종 흰 가이드 잔여픽셀이 화면에 남음. Codex는 소품 가이드의 실제 Scene GameObject 대응도 기록했다고 보고. ZIP의 정상 여부·기술/기능 QA 통과를 미술 합격으로 오해하지 않는다.

## 1. 공통 원화/Importer 계약

- **512×512 PNG, RGBA, PPU128, Pivot 중앙**. 신규 오리지널 이미지로 처음부터 제작. v1/v2 구형 Pixel 재색칠, 기존 결함 픽셀 자동경계 보정, nearest-neighbor 배율 확장, 흰선 위 덮개처리 반복 금지.
- 새 이미지의 실제 Ground/Overlay/Slab **world size는 Unity Renderer Tiled/Simple 설정과 Transform·스케일/중앙 Pivot이 결정**한다. 원화 512×512 자체가 Tiled 표시 크기 21×15 또는 9×15 픽셀을 뜻하는 것은 아니다. Codex가 제공한 다음 Scene 현행 위치·표시 크기는 유지하되 기존 Sprite GUID/meta 파일을 덮어쓰지 않고 신규 격리 Asset으로 Import한다.
- 새로운 Asset/Codex Runner에서 실제 Unity가 사용하는 SpriteMode/TextureType, compression/FilterMode/Tint/Sorting, Overlay Material·Alpha 결합·Collider 관계는 **기존 기술인계서를 정본**으로 확인한다. 그림만 바꾸면 반드시 해결된다고 단정하지 않으며 Renderer·Material 설정 원인은 Codex가 조사·국소 기술 수정한다.
- Field08 붉은 균열→Field09 검은 현무암과 균열·잔열로 연결되는 Chapter2 정체성, 작은 화면 가독성·휠체어 이동/진행 마커 보호. 흰 임시 가이드선·규칙적인 회색 띠·과도한 불투명 정사각형/화면 점멸 금지.

## 2. 먼저 만들 실제 PNG 3종

| 역할 | 실제 Unity 위치 (x,y) | 현재 표시 형태 | 요구 |
|---|---|---|---|
| 신규 Base Ground 1 | (0,0) | Tiled 21×15 | x/y 양축 seamless, 반복 6지점 포함 Tile 경계 회색밴드0 |
| 신규 Cracked Ground/Overlay 1 | (-6,0) | Tiled 9×15 | 타일 자체 seamless + Base와 다른 Material/Alpha의 교차 seam0. Cracked 확산이 자연스럽게 끝나되 기존 이동 방향/Quest 지표를 덮지 않음 |
| 신규 대표 현무암 Slab Prop 1 | (5.2,1) | 약 2×2 world units | 원본과 동일 기능/배치 역할의 독창적 형태, 흰 가이드선0, Alpha foot/접지·Sort와 원래 Collider 영향0 |

**3종 각각 완성 원화 PNG를 실제 전달해야 한다.** 한 장의 합성/콘셉트 보드만으로 완료 처리 불가. 각 PNG의 SHA256/Alpha/Canvas와 가정한 Importer 설정, 세 가지 미술 역할/기존 source mapping을 제공할 것.

### Tiled 원화 QA의 핵심
- PNG 파일의 맞닿는 가장자리 RGBA만 확인하는 `edge-match=0`으로 실제 Scene 시각 통과라고 주장 금지. 최소 **21×15, 9×15 타일링을 모사한 무봉합 확대 화면과 두 재질이 겹치는 합성 모식도**를 별도 생성하되 실 Unity 캡처와 명백히 구분한다.
- Slab도 흰 픽셀 색상만 제거하는 기존 방법 대신 **새로 설계한 소품 실루엣을 투명 바탕에 완성**하고, Alpha 바깥 부유픽셀/굵은 네모 테두리/접지·장애물 표시 오류를 확인한다.

## 3. 단계별 검증 및 마감

1. **리뉴얼 담당 우선:** `docs/art-renewal`에서 신규 원화 PNG3 제작→`Limitless_Field09_Redesign_Sample3_v1.zip` 표준 zipfile/CRC/testzip+Manifest/원본 대응표/Preview/가정된 Importer 사양/오프라인 QA와 SHA 납품. 가능하면 GitHub 담당 브랜치에 Art_Deliveries 파일을 넣어 Codex가 직접 내려받도록 하되 사용자가 F: 수동 복사할 의무 없음. 완료 보고 Branch/Commit/ZIP SHA/실제 PNG3 경로와 대표 승인 필요 여부.
2. **Codex 검수:** 기존 Field09 v1/v2의 기술40PASS·Main18 기능92PASS(기존 이력)를 재검사하지 않고 **샘플 3종의 격리 Unity 동일 Camera/Zoom/3비율 GameView**, Tiled 반복6·이종 Overlay seam·소품 흰선·원래 Collider/Quest marker 및 휠체어 경계만 최소 Delta QA. 새 원화를 샘플로만 격리 Import하고 원본 Scene·기존 official PNG10/meta/GUID·UserData/Save·활성 원본 Editor 보존.
3. 하나라도 실제 Visual FAIL이면 **나머지 5~7개 환경 자산 제작 중단**, 오류 화면/Renderer/Material/좌표/bbox 증거와 미술/기술 원인 분리 후 샘플만 수정. **3종 샘플의 미술·Unity 시각 검수 및 바울 대표 미술 승인이 모두 통과한 뒤** 전체 Field09 신규 환경 제작과 통합 QA를 별도 승인 게이트로 진행.
4. Git main 직접 commit/Push/병합 금지. Field08 공식 환경 및 Main18 13목표/과열/세린·몬스터/Save·Continue/Field09 Spawn·Collider의 새 기능구현 금지. `ART_SAMPLE3_READY` / `OFFLINE_QA_PASS` / `UNITY_VISUAL_PASS` / `USER_VISUAL_APPROVED` / `ORIGINAL_SCENE_APPLIED`은 각각 별도 상태로 기록.

**기획 결론:** Codex 기술 인계 수령. **다음 실행 담당은 리뉴얼 아트이며, 우선 실제 PNG3를 제작하는 단계**. Codex에게 다시 전수 기술 조사를 시키거나 사용자에게 ZIP 업로드를 요구하지 않는다.
