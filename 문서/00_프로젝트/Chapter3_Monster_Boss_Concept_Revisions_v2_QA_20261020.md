# LIMITLESS Chapter3 — 서리들쥐·설빙의 폭군 콘셉트 재시안 v2 납품/QA (2026-10-20)

**담당 브랜치:** `docs/art-new` · 연계 Issue [#30](https://github.com/HappyPaul812/Project-Limitless/issues/30)

**판정:** `4_CONCEPTS_DELIVERED / USER_ART_REVIEW_PENDING / NOT_PIXEL_ART / NOT_UNITY_READY`

## 1. 미술 반려 및 재시안 범위

- 바울 대표는 1차 `NorthF1_FrostFieldMouse_KeyArt_Concept.png`와 `FrostTyrant_Silhouette_Concept.png`를 **REJECTED** 판정: 마스코트 같은 귀여움, 보스의 위압감 부족, Chapter2 공식 아트 대비 낮은 세계관 일치도.
- **서리들쥐 2안, 설빙의 폭군 2안**만 신규 시안을 제작. F1·서릿길마을 파일럿은 수정하지 않고, 공식 하렌·세린·태온·폴·미엘·플레이어, Chapter1·2 아트도 보존.
- 서리들쥐에는 야생 해충·생존·거친 체모, 폭군에는 웬디고 모티브/재앙급 규모/실루엣 압박을 적용. 원본 PNG들은 파일명만 영문으로 복사·포장했으며 색·형태·픽셀 재가공 없음.

## 2. 실제 납품 ZIP

**파일명:** `Limitless_Chapter3_Monster_Boss_Concept_Revisions_v2.zip`

**SHA256:** `7b6cc00e896ffa1ddcf4e4aa6fff4b05cbf160d587ef209db5f5e72c53e97561`

**실제 다운로드 위치:** 이번 ChatGPT 대화의 첨부 ZIP (자동으로 GitHub/사용자 PC 로컬에 업로드되는 것은 아님).

총 **8항목**: `Concepts/` 원본 PNG 4장, `Preview/` 비교 PNG 1장, `Manifest.json`, `Manifest.md`, `QA/Offline_QA.json`.

| 원본 콘셉트 | 파일명 | 실제 규격·모드 | SHA-256 |
|---|---|---|---|
| 서리들쥐 A: 서리 낀 털·노출된 앞니·발톱 | `Concepts/FrostFieldMouse_Concept_A.png` | 1254×1254 RGB | `7e26e5ccf30dfd49659007c444f7b3d24cb748ea58b3c1af0c9b3cfd53f15c54` |
| 서리들쥐 B: 낮게 웅크린 야생 들쥐·긴 꼬리 | `Concepts/FrostFieldMouse_Concept_B.png` | 1254×1254 RGB | `772af434edb5e0382636799fadd0b81430674323f2b82aaa155cfae2cbd86943` |
| 설빙의 폭군 A: 큰 흉곽·가지형 머리 구조·장신 | `Concepts/FrostTyrant_Concept_A.png` | 1122×1402 RGB | `dbce450e6e73f94d8b8f0bb8702c8d59ccd2daab36f28044f882bf979da86b7e` |
| 설빙의 폭군 B: 두개골형 안면·무거운 발톱·포식자 공포 | `Concepts/FrostTyrant_Concept_B.png` | 1122×1402 RGB | `b9316a10c94ff5e041354500c72e5e6464f725fd01c1303285584d838c8c55fe` |

## 3. 오프라인 QA / 기술·미술 구분

- 확인: 네 PNG 디코딩 PASS, 실제 해상도/모드/SHA 기록, 원본 파일 바이트 변경 없이 영문 복사, ZIP CRC PASS, Manifest와 내부 8항목 일치.
- **비통과/미적용:** 투명 RGBA 캐릭터 스프라이트가 아님. 현재 시안은 배경 포함 회화적/준실사풍 **무드·실루엣 콘셉트**이며 LIMITLESS 2D 픽셀아트/투명 Game-ready 자산 아님. 고해상도 원화를 단순 512px 축소해 Sprite라고 주장하지 않는다.
- Unity 프레임/방향/PPU/Pivot/Collider, 실제 캐릭터 크기·배치, 게임 Scene, 보스 TurnOrder 등 검사·설정하지 않음. 대표 미술 검토를 우선하고 디자인 선택 후 정식 2D 픽셀아트 1포즈만 분리 제작한다.
- 이미지 배경 속 유적·행인 등은 게임의 공식 지형/퀘스트/몬스터/캐릭터 신규 설정을 의미하지 않음.
- 이전 반려 키아트 및 F1/마을 10종 파일럿, 기존 승인 정식 아트·Unity·Save는 모두 변경하지 않음. **`main` 직접 커밋/Push 없음**.

## 4. 사용자 승인 요청/후속 작업

1. **서리들쥐 A/B, 설빙의 폭군 A/B 중 채택안 혹은 혼합 방향 결정** (사용자 선택 전 정식 미술 승인 아님).
2. 선택 디자인에서 기존 Chapter2 공식 픽셀아트 수준의 **투명 1포즈 시안**으로 재해석·제작. 고해상도 무드 콘셉트 그대로 게임에 삽입 금지.
3. Codex Issue #35의 PPU·Pivot·Collider 등 기술 계약과 기획 Production Lock 이후 애니메이션 시트 규격을 협의. Codex Unity 통합·Runtime QA는 별도 허가.
4. **GitHub에는 본 문서의 QA·SHA만 기록.** ZIP/PNG 바이너리는 GitHub에 업로드하지 않음. `docs/art-new` 커밋만 남기며 `main` 미변경.
