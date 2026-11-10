# LIMITLESS / Issue #43 / VenomSnake v3 최종 표적 검수 (2026-11-10)

**브랜치:** docs/art-renewal  
**판정:** VENOMSNAKE_V3_KEEP / NO_CHANGE_NEEDED / USER_ART_REVIEW_PENDING / NOT_UNITY_APPLIED

## 1. 기준과 기존 자산 보호

- 최신 [총괄 지시](https://github.com/HappyPaul812/Project-Limitless/issues/43#issuecomment-6757398584), [Codex 실물 검수](https://github.com/HappyPaul812/Project-Limitless/issues/43#issuecomment-6757168292) 확인.
- 기준 파일 VenomSnake_KeyArt_v3.png, 1254×1254 RGBA, 2,285,408 bytes, SHA256: 7001d199b279cb8e541fc603bd62351ab460d3b44da91bc91a2d65dc099f8fd0. 실제 라이브러리 원본과 이전 SHA 일치.
- v1 거절, v2 HOLD, v3 Codex 단일포즈 미술 PASS 권고. 단, 사용자 최종 승인 별도.
- v3 원본 바이트 수정 0. 새로운 v4 원화/보정 PNG/애니메이션 생성 0.

## 2. 왼쪽 경계와 40px 판단

- 원본 x=0에서 alpha>0 픽셀은 25개. 해당 25개 모두 alpha **정확히 1/255**. alpha>1 픽셀 **0개**, alpha>8 **0개**. 유의미한 가시 잘림이나 회백 매트가 아니라서 원본을 고쳐도 화면 이득이 거의 없음.
- 1254×1254 전체 캔버스를 동일 비율로 512·128·40px 축소하여 비교. 40px에서 우향 머리·목의 S자 곡선·벌어진 입 실루엣은 유지, 개별 송곳니와 비늘은 축소 한계상 결합. 디자인 변경에 따른 위험보다 개선 이익이 작아 원본 KEEP.
- 512·128·40 OFFLINE 비교판의 양쪽은 **동일 v3 원본**을 사용하며 오른쪽은 수정후 결과물이 아닌 'NO CORRECTION NEEDED' 표시다. 신규 후보가 만들어졌다고 허위 주장하지 않는다.
- Unity 실제 PPU/Scale/Pivot, GameView 판정, 실제 전투 슬롯 읽힘은 미검증.

## 3. 실제 납품 및 무결성

- ZIP: LIMITLESS_VENOMSNAKE_ISSUE43_KEEP_NOCHANGE_20261110.zip
- ZIP **3,318,345 bytes**, SHA256 **8c6871ec0cdaa452dd9000b95ac2dfce95c52b798b0925a7e683aa273c47ca80**, **13 entries**, CRC PASS, Manifest **11/11 SHA·bytes PASS**.
- Source_KEEP/VenomSnake_KeyArt_v3_ORIGINAL_BYTE_KEEP.png : 원본 PNG 바이트 정확한 복사.
- Preview/VenomSnake_v3_512px_RGBA_OFFLINE.png, Preview/VenomSnake_v3_128px_RGBA_OFFLINE.png, Preview/VenomSnake_v3_40px_RGBA_OFFLINE.png
- Review/Original_KEEP_vs_NoCorrection_512px_OFFLINE.png, Review/Original_KEEP_vs_NoCorrection_128px_OFFLINE.png, Review/Original_KEEP_vs_NoCorrection_40px_OFFLINE.png
- Review/40px_ThreeBackgrounds_Zoom_OFFLINE.png, Review/LeftEdge_Alpha1_Diagnostic_OFFLINE.png
- Evidence/QA_Results.json, DesignNotes_KO.md, Manifest.json, SHA256SUMS.txt 포함.
- ChatGPT Library /LIMITLESS_Art_Delivery/VenomSnake_Issue43_FinalKeep_20261110/ 에 ZIP·원본·512/128/40 비교판·알파진단·40px 배경판 7개 저장 확인. 사용자 F: 자동복사/원격 GitHub 바이너리 Push 없음.

## 4. 변경 금지 및 다음 게이트

- 기존 v3, VenomBee, ForestSpider, Ch2 공식 몬스터·보스·펫·배경, Unity 6000.6.5f1 원본, Scene, Prefab, GUID/meta, Quest, Spawn, AI, 독/드롭, Save/UserData, main 변경 0. 전체 재작화/Idle·Attack 양산/Unity 적용 금지.
- 바울 대표의 **v3 원본 KEEP 최종 미술 승인 필요**. Codex PASS 권고를 USER_APPROVED로 승격하지 않는다.
- Issue #95 OGA06 치유사·OGA16 은행 안내인 신규 2종은 **맹독뱀 사용자 최종 승인 후 다음 턴**에 시작. 이번 NPC 신규 0.
- Issue #96 HP/MP 소형 물약 2종은 Codex 로컬 탐색에서 적합한 내부 오리지널이 **NOT_FOUND로 확정된 경우에만** 리뉴얼 소량 제작 인계. 기존 원본 탐색 중이므로 이번 제작 0.

**최종 상태: VENOM_SNAKE_ART_CANDIDATE_FINALIZED / NO_CHANGE_NEEDED / USER_ART_REVIEW_PENDING / NOT_UNITY_APPLIED**
