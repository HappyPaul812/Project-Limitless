# LIMITLESS / Arbel 환경 리뉴얼 v1 납품물 QA 후속 — v2 최소 보완 요청 (2026-10-14)

**상태: NEEDS_MINOR_FIX / ART_FIX_REQUESTED / USER_ART_FINAL_APPROVAL_PENDING / UNITY_NOT_APPLIED / SOURCE_ZIP_PROTECTED.**

바울 대표가 Codex 로컬 읽기전용 ZIP 검수 결과를 기획 담당에게 전달했다. 이 문서는 **사용자 제공 Codex QA 보고의 접수 및 리뉴얼 담당으로의 작업 분리**이며, 기획 세션이 로컬 F: ZIP을 직접 열거나 수정·실제 Unity Play를 실행한 기록이 아니다.

## 검수 근거

- ZIP: `F:/Downloads/Limitless_Arbel_Renewal_Art_v1.zip`
- v1 ZIP SHA256: `ec707452169aa99ec297a7ac23a14928d4a0123b4b7b051d17ddcb282953d626`.
- 파일 47개/PNG40개 = 교체후보23 + 선택 장식11 + Preview5 + QA 이미지1 (Codex 보고 기준).
- 현행 `main`의 `Arbel` Scene `SpriteRenderer` **284/284 매칭**, 좌표·GUID·local fileID·PPU·Pivot·Sorting 대응 정상 보고.
- Manifest/README/대응표/인계서 모두 존재. Single Sprite 구성으로 별도 Sprite Sheet가 없는 것은 결함 아님.
- PNG CRC/decode/Manifest SHA/바닥 6종 Seam 대부분 통과, 도로2개 경계 및 장식 기능 가독성 보완.
- Preview/Contact Sheet 존재하지만 **합성 이미지**, 실제 Unity RenderTexture/플레이 화면이 아님. 리뉴얼 담당의 미술 평가와 사용자의 실제 미술 승인/실제 런타임 QA는 별개.
- 원본 ZIP 및 Unity·Scene·Asset·Save 변경0, Codex QA 브랜치/Push/커밋 없음 보고. 원본 5221파일 SHA/HEAD/dirty 상태 보호 보고.

## 수정이 필요한 세 항목 — 정확하게 최소 범위

1. **도로 PNG 2장 경계**: 좌우 인접 RGB 경계 9px 불일치, 상하 Alpha 경계 소수(각 1·4px로 보고) 불일치. `QA/Offline_Validation_v1.json`과 원본 이미지에서 불일치 위치/두 파일명을 확인하고 올바른 타일 반복 시점에서 경계를 Seamless로 수정. Tile 크기, PPU, Pivot, alpha 투명띠 정상영역을 전역으로 뒤집거나 다른 도로/잔디 PNG를 훼손하지 않음. 정확한 파일 이름·불일치 좌표를 QA 보고에서 먼저 재확인. 수정 전후 반복 배치 그림 비교 필요.
2. **소품 9개 크기·접지 정보**: 기존 Scene의 실측 원본 크기와 신규 Prop Alpha 하단 위치를 비교한 크기·접지 문서 정보의 결함. 보고된 9개 소품의 파일/Renderer 역할을 대응표에서 확인해 원본 크기란과 그림의 Alpha 접지 기준 및 문서상의 기대 적용 크기를 정확히 수정하고, 그림 수정이 필요한 소품만 선택적으로 보완. **9개 모두 PNG에 결함이 있다고 단정하지 말 것.** 임의 Pivot/Transform 변경이나 Scene 수정으로 덮지 않고, 수정 전후 접지·크기 대비 표시.
3. **북서 건물 지붕의 치유 십자 마크**: 북서 건물의 장식 십자가 실제 치유사 위치와 어긋나 기능·출입구를 오인할 위험. 기존 Arbel Scene의 실제 치유시설 위치와 건물 구분을 근거로 표시 위치/귀속 디자인만 교정. 다른 건물의 지붕, 출입구·Portal·NPC 위치/역할을 바꾸지 않음. 필요하면 지붕 원본과 장식 레이어를 분리하는 안을 제시해 바울 대표 미술 승인을 받음.

**독립 HOLD:** 건물 지붕 확대와 건물/카메라/NPC 겹침은 Unity 런타임 교차 확인 전 대체 승인되지 않음. 선택 장식11개는 별도 승인까지 미배치; 기존 Portal 겹침 장식은 **HOLD** 유지. 284/284 대응은 v2에도 그대로 유지되어야 한다.

## 아트 담당 납품·QA 목표

- **리뉴얼 담당**(`docs/art-renewal`): 위 3개만 보정하여 `Limitless_Arbel_Renewal_Art_v2.zip` 새 아카이브를 납품. 원본 v1 ZIP·출처·기존 정상 PNG 변경 없음; 의도된 수정 PNG와 변경 전/후 SHA 목록 제공.
- v2 ZIP에 기존 필수 문서·매핑284행·PNG/Preview/Contact Sheet/Manifest SHA를 동기화하고 `QA/Offline_Validation_v2.json`과 변경부 확대 비교 이미지를 넣을 것. 교체후보23·선택11 수량 및 원본 GUID, Renderer 좌표·Pivot·PPU 그대로 보호.
- 도로2종 RGB/Alpha 상하좌우 반복 seam=PASS, 소품9종 원본크기·접지 정보 QA=PASS, 치유 지붕의 실제 시설 위치/표시 정합성=PASS (수동 승인 별도). 284/284 재매핑 PASS, 전체 ZIP CRC/PNGDecode/SHA PASS. Source v1 ZIP SHA 보존.
- **Codex**: v2 납품 후 읽기전용 기술 재검사 및 원본 Scene/Importer 비교, 바울 대표 미술 승인 후에만 Unity 원본 대상 최소 적용을 수행. 현재 단계에서 전체 Arbel 교체·GUID 변경·Unity Editor 조작·PR/main Push 금지.
- **바울 대표**: v2 Preview/변경부 확인 후 교체23종과 선택 장식11종의 승인 범위를 구분해 판단. 원본 지붕 확대·기존 Tree/Portal 충돌 HOLD는 별도로 검증.

## 부서 공통

Chapter1·2 리뉴얼 담당만 Arbel 제작 책임. Chapter3 신규 아트 담당의 `설빙의 폭군` / 북부 몬스터 및 화로 기획과 혼합하지 않는다. 최신 main `bddca540d2fdd9e93f642f8d0f387b34730bd2d8`에서 Sprite 참조 복구/PR10 병합된 상태 보존. 문서 담당 브랜치 `docs/planning`, Codex·리뉴얼 담당은 각자 브랜치 사용. **본 문서는 사용자 미술 최종 승인도 새 구현 승인도 아님**.
