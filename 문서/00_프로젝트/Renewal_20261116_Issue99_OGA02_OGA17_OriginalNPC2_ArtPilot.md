# LIMITLESS / Chapter1 Renewal Art / Issue #99 — OGA02 훈련장 안내인·OGA17 장비상인 오리지널 2종 아트 파일럿 납품

**작업일:** 2026-11-16 (KST) · **담당 브랜치:** `docs/art-renewal`  
**상태:** `OGA02_OGA17_NEW_512_RGBA_2_DELIVERED / FILE_TECH_PASS / USER_ART_REVIEW_PENDING / NOT_UNITY_APPLIED / NOT_REMOTE_BINARY_PUSHED`

## 1. 최신 정본·기존 승인 보호
- [Issue #99 신규 발주](https://github.com/HappyPaul812/Project-Limitless/issues/99), [기획 공식 병렬 계약](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/20261116_Parallel_Art_MouseCloseout_NPC02_17.md), [Issue #57 NPC03/09](https://github.com/HappyPaul812/Project-Limitless/issues/57#issuecomment-6791260745), [Issue #95 NPC06/16](https://github.com/HappyPaul812/Project-Limitless/issues/95#issuecomment-6786869689), `COMMON_RULES.md`, `WORLD_ART_RENEWAL_LATEST.md` 확인.
- **기존 승인 완료 KEEP:** OGA03·OGA09·OGA06·OGA16 네 종 `USER_ART_APPROVED / LOCAL_ORIGINAL_APPLIED / USER_REALPLAY_ACCEPTED / REMOTE_NOT_PUSHED`. 위 상태의 실제 소스는 #57/#95 최신 적용·사용자 실게임 수용 댓글이며, 과거 미술 대기 문서의 날짜가 더 오래됨. **이번에 이 4명 재제작/편집/제품 재적용·재QA 0**.
- OGA20 태온 유사성 `HOLD` 유지. #99/기존 라이브러리·#57/#95 파일 검색 범위에서는 OGA02·OGA17 **기승인 오리지널 원화 납품 근거를 찾지 못함**; 본 작업은 새 요청에 의해 새 원화 **정확히 2종**을 제작. 사용자 PC `F:\Downloads` 전체 라이브러리 직접 감사 또는 모든 과거 자료의 부재 증명은 **NOT_DONE**.
- 기존 게임 경로 `starter-village-training-guide` → `VillageNpcSprites/Eldiran/OGA02_TrainingGuide`, `starter-village-equipment-shop` → `VillageNpcSprites/Eldiran/OGA17_EquipmentShop`(기존 `VillageNpcAppearanceCatalog.cs`). 원본 Stable ID/기능/Resources 바이트 미변경.

## 2. 이번 신규 아트 실물: 512² PNG 정확히 두 장

| NPC·Stable ID | 새 마스터 투명 PNG | SHA256 (계산 실값) | 역할 구분 |
|---|---|---|---|
| **OGA02 훈련장 안내인** / `starter-village-training-guide` | `Candidates_512/OGA02_TrainingGuide_Original_512_RGBA.png` (**132,616 bytes**) | **`07bdb6368dcab01c85f551b71bd79ba391a3c3f621355664fc6453cb5da98cdf`** | 크림색 머리띠·갈색 헤어·훈련용 호루라기·손목 보호대·목검/나무 연습봉, 친근하고 전문적인 생활형 훈련 담당 |
| **OGA17 장비상인** / `starter-village-equipment-shop` | `Candidates_512/OGA17_EquipmentShop_Original_512_RGBA.png` (**137,471 bytes**) | **`1282d90a7f89924e3efaad40679cbc272193bb9e05397765671549d9de646359`** | 머리 위 황동 고글·진녹색 상의·금속 견갑·대장장이 앞치마·공구, OGA03 일반 잡화상인의 가방/앞치마와 다른 장비 제작·정비 직업 실루엣 |

- **각 512×512 RGBA / 투명 배경, 독립 정지 1포즈.** 두 생성 원화 1254×1254 RGBA 자체를 `OriginalGeneratedSource_1254/`에 byte KEEP; 출처 SHA256 OGA02 `a0b4005da52845df5d672441b12d352c8a155b6dfb0db6d6252d45c1964d1d5a`, OGA17 `3b71ca9cd6558e5ccd98d5057abff3292ce5ac96fccdfbee67c2dd0c2e229d4e`.
- 과도한 생성 알파 잔상(alpha 1..7)만 제거하고 알파≥8 가시 인물 범위에서 **비율 유지**해 512 캔버스 안에 가운데 배치. 두 후보 보이는 실루엣 bbox: OGA02 `(144,60,368,469)` (409px), OGA17 `(141,60,370,470)` (410px). 캔버스 **네 변 최외곽 Alpha=0**, 발끝 아래쪽 여백 ≈42px로 정렬. 실제 Unity 발접지/Pivot/PPU PASS의 대체 근거가 아님.
- 기승인 네 원화 **바이트 그대로** `References_BYTE_KEEP/`에 저장하여 512 캔버스 동규격 비교. OGA03 SHA `09b2d97643a7aaefd51930eb374c48bf358020f015cc4cce9e7ef4903853151d`, OGA09 `ca0336ab6bbc7aaeb16169037e73ee39ced1b674f48324baeb75b1debfa29fcf`, OGA06 `67aef89677e9b9f174f30b2c2a9e854eaee4e61814774402bd94a18691bd401f`, OGA16 `1199c57d3c279b51af83452c8b580814d6456417ea8e3f3b83ea9fdc6c5f8dcd`. 승인 마스터를 신규 NPC용 소재로 트레이싱·재색칠하지 않음.

## 3. 미술 검토·가독성·제약

- `Review_OFFLINE/Issue99_OGA02_OGA17_vs_Approved4_Style_Comparison.png`: **승인 NPC 4명 + 신규 NPC 2명**, 총6 실제 512 소스 기준 굵은 윤곽선·평면 음영·따뜻한 피부/갈색/남색·녹색 톤 비교. 가시 인물 크기·머리/손·발 바닥 인식. 실제 Unity GameView가 아닌 **오프라인 비교판**.
- `Review_OFFLINE/Issue99_OGA02_OGA17_128_40_RealSize_Comparison.png`: 원본 **전체 512 투명 캔버스**를 각각 128px·40px로 **실제 비율대로 축소**해 밝은 바닥/어두운 배경의 실제 크기로 비교. 별도 40px×5 최근접 확대에는 픽셀 정보가 늘지 않음.
- OGA02 **128px** 머리띠/호루라기/연습봉·팔 보호대 확인, **40px** 머리띠·갈색 옷/봉 형태 남으나 휘슬 작은 디테일은 자연스럽게 감소. OGA17 **128px** 고글·금속 견갑·공구·앞치마, **40px** 큰 고글·진녹색 실루엣과 황동/공구색 구별. OGA03 일반 잡화상의 가방·갈색 앞치마와 구분. 40px에서 코/눈/도구 조각의 독립 형태는 모두 또렷하다고 단정하지 않음. 
- **미술 상태:** 기존 승인 화풍에 대체로 적합하고 역할 구별 가능하므로 **파일럿 2종 사용자 시각 검토 요청**. 현재 **`USER_ART_REVIEW_PENDING`**, 최종 아트 승인/게임 적용/실플레이 PASS 주장 금지.
- 기존 32px/PPU28 NPC에 512 마스터를 **그대로 임포트하지 말 것**. 기존 원본의 월드 보이는 높이·PPU/Pivot·Collider/Nameplate 위치와 맞춘 별도 스케일 계약은 승인 후 Codex의 Unity **6000.6.5f1** 최소 격리 QA 대상.

## 4. 독립 출처/검증/ZIP

- `Evidence/SourceDeclaration.md`: 신규 생성 1254 RGBA 원본(바이트 KEEP) → 가시 알파 정리·균일 비율 축소/512 마스터 변환 과정. 외부 Kenney/기존 OGA 리컬러·트레이스 아님, 제3자 IP 완전 보증 주장은 없음.
- `Evidence/QA_Results.json`의 후보 2장 actual SHA·가시 BBox·알파경계·출처 SHA·참조 원화 4장 SHA·상태, `Manifest.json`, `SHA256SUMS.txt` 포함.
- **납품 ZIP** `LIMITLESS_ISSUE99_OGA02_OGA17_NPC_ORIGINAL_ART_PILOT_20261116.zip` **1,903,948 bytes / SHA256 `f10cb1f417f6bc03c64a05028f9549eacc23276e148c81136d5d922b2b4f1bdd` / 19 entries / CRC PASS / Manifest 17/17 bytes+SHA256 PASS**.
- ZIP 내 후보 PNG2·1254 소스2·128/40 실축소 PNG4·기승인 참고 PNG4 BYTE_KEEP·비교판2·README/SourceDeclaration/QA/Manifest/SHA SUMS.
- **ChatGPT Library 실제 업로드:** `/LIMITLESS_Art_Delivery/Issue99_OGA02_OGA17_NPC_20261116/` ZIP+마스터PNG2+비교판2+128/40 개별4+QA/출처2 총 **11개 항목**. 이 작업은 사용자 `F:\Downloads` 복사, 원격 GitHub ZIP/PNG Push가 아님.

## 5. 절대 보호 / 다음 게이트

- 기존 승인 OGA03/06/09/16, OGA20 유사성 HOLD, 기존 다른 NPC, Quest/Shop·가격/StableID/E·패드·마우스/Collider/Sorting/Nameplate/Save/UserData/기존 GUID/meta/Scene/Prefab, Unity 6000.6.5f1/Discord·원격/main **변경0**. 새 NPC Portrait/Idle/Walk/Attack/방향별 시트 제작0, 다른 직업 세트0. 큰 격리 QA 복제0.
- **담당 GitHub 문서 브랜치 `docs/art-renewal` 만 보고 변경**. `main` 직접 커밋/Push/Merge0, 사용자 PC 로컬 Unity 원본 적용0.
- **바울 대표 시각 승인 후에만** 실제 원본 NPC 32px/PPU28 크기 대비 새 캐릭터의 월드 높이·Pivot/Scale·접지·역할/상호작용·Nameplate 최소 Codex 격리 DeltaQA, 이후 별도 적용 승인 필요. 

**정확한 결과:** `OGA02_OGA17_2_ORIGINAL_STILL_NPC_MASTERS_DELIVERED / OFFLINE_ART_REVIEW_PREPARED / HASH_CRC_PASS / USER_ART_REVIEW_PENDING / UNITY_NOT_APPLIED`.
