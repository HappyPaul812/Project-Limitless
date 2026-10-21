# LIMITLESS Chapter3 Main23 — Frostway Village v2 영문 ZIP 재패키징 납품 (2026-10-21)

> **ENGLISH_PACKAGING_DELIVERED / SOURCE_CONCEPTS_PRESERVED / USER_ART_APPROVAL_PENDING / GAME_READY_NOT_VERIFIED / NOT_UNITY_INTEGRATED**
>
> 담당 브랜치 `docs/art-new`, 연결 Issue [#45](https://github.com/HappyPaul812/Project-Limitless/issues/45).
> 기획 정본 `docs/planning/문서/00_프로젝트/20261021_Chapter3_Main23_Frostway_Village_Art_And_Tech_Parallel_Kickoff.md` 참조.

## 사용자 요청과 실제 결과

- 사용자는 이전 응답의 이미지들에 대해 **A: 영문 파일명으로 ZIP 패키징**을 선택. 기존 이미지들을 이름만 영문으로 바이트 동일 복사하여, 별도 화면비 미리보기·Manifest·SHA256 목록·오프라인 QA와 함께 압축했다.
- 실제 납품: **`Limitless_Chapter3_FrostwayVillage_Environment_ArtPilot_v2.zip`** (현재 ChatGPT 대화 다운로드 첨부)
- ZIP SHA-256: **`2142fe641c64a2ead8ad37309258d49727f35e5aaea74e429cbd6bae546154ad`**
- 파일 수 **19개**: 원본 이미지 **11개**, 신규 화면비/전체 비교 미리보기 **4개**, Manifest.md / Manifest.json / QA/Offline_QA.json / SHA256SUMS.txt **4개**.
- 원본11: `Tiles/` 5 PNG, `Buildings/` 합성시트1, `Props/` 합성시트1, `Layouts/` 전체 마을 시안 및 분위기4. **모두 원본 바이트 동일**로 소스 SHA-256 재검증.
- Preview4: `Frostway_LayoutPreview_16x9.png`(1280×720), `Frostway_LayoutPreview_4x3.png`(1280×960), `Frostway_LayoutPreview_21x9.png`(1680×720), `Frostway_Asset_ContactSheet.png`(1040×700). 레이아웃 축소는 contain/letterbox로 화면 전체를 유지; 새로운 Scene 확정 아님.
- 모든 원본 및 미리보기 디코딩 정상, ZIP `testzip()==None`, 내부 해시 일치. 개별 파일 SHA·네이티브 해상도·모드는 ZIP `Manifest.json`, `QA/Offline_QA.json`, `SHA256SUMS.txt` 참조.

## 중요 품질·기술 한계

- 이번 결과는 사용자가 선택한 **기존 생성 이미지 재패키징**이며, 공식 게임용 256px 타일·개별 건물 스프라이트·개별 소품 투명 PNG 완성 **아님**. 원본 PNG 다수는 **1254×1254** (RGB/RGBA), 건물·소품 합성 시트는 **1448×1086 RGBA**로, 최종 Unity PPU/TileGrid와는 다른 미승인 콘셉트 파일. 수동 셀 분해, 경계 2×2 Seam, 실제 알파 사용성은 미검증.
- RGB 지면 원화는 반복 가능한 Seamless 타일이라고 주장하지 않음. RGBA 합성시트의 부분 알파/테두리도 사용 전 아트 QA 필요.
- 16:9/4:3/21:9 미리보기는 오프라인 Presentation Preview이며 **Unity GameView 아님**. 남북 통행로 3 WU, 이동보조기 통행, 서릿길 마을 서비스 위치, 동쪽 비작동 포탈 돌기단은 정본의 배치 의도이며 해당 이미지로 실제 Collider/Accessibility PASS를 주장하지 않음.
- 건물 간판/포탈 돌기단의 세부 오브젝트 확정·개별화, F1↔마을 Unity 공용 PPU 및 Chapter2 공식 미술 동배율 비교 QA, 실제 Main23 Scene 작업은 **미완료**. 승인된 F1 6 PNG, 기존 공식 캐릭터·몬스터·Chapter1/2·Unity Scene/Save/Guid를 수정하지 않음.
- 원본 11장과 ZIP 자체는 ChatGPT 첨부에 존재하며 **GitHub에는 이 QA 문서만 등록**. 아트 바이너리를 GitHub/바울 대표 Windows F:에 배치했다고 주장하지 않음.
- **다음 승인 게이트**: 대표 미술 검토 → Codex Main23 기초 기술 계약/타일·문·길 접근성 검사 → 개별 게임 실사용 자산 분해/픽셀 마감 작업 → Codex 별도 Unity 적용 QA. 사용자 승인 전 main 직접 커밋·Push·병합 금지.

