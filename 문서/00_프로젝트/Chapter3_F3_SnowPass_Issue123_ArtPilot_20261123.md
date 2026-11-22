# LIMITLESS / Chapter 3 F3 눈 덮인 고갯길 환경 아트 소량 파일럿 — 2026-11-23

**Issue:** [#123](https://github.com/HappyPaul812/Project-Limitless/issues/123)  
**담당 브랜치:** `docs/art-new` (문서 기록만)  
**상태:** `F3_PILOT_3_ART_CANDIDATES_DELIVERED / OFFLINE_FILE_QA_PASS / USER_ART_REVIEW_PENDING / PPU_TILEGRID_COLLIDER_SPEC_PENDING / UNITY_NOT_APPLIED`.

## 1. 2026-11-23 최신 발주 확인 및 사전 중복 검사

[#123 지시](https://github.com/HappyPaul812/Project-Limitless/issues/123)에는 F3=눈 덮인 고갯길, F2 첫눈 숲→고갯길 지형 단계 전환, 최대3종만 후보 제작, 휠체어/보행 경로 가독성, F1·F2/F2 B352/BattleBG v2 및 M1~M5 KEEP, 원본 Unity/Collider/Quest/Spawn/Save 변경 금지가 명시됨. [2026-11-19 안전 체크포인트](https://github.com/HappyPaul812/Project-Limitless/tree/backup/2026-11-19-safe-checkpoint)에서도 `Unity/Client/Assets/_Project/Scenes/World_NorthF2_CANDIDATE.unity` 실물 원격 문서가 존재함을 확인했고 해당 Scene은 읽기만 하였음.

**실제 비교 자료 4개 ZIP (읽기 전용, 원본 바이트 불변):**
- `Limitless_F2_MinimalFollowup_20261103.zip`: F2 표지판 후보288×384, 낮은 나무 울타리 후보256×192, 그루터기 후보320×384, FULLREDRAW 대안3. 표지판·울타리 대안은 최종 승인 아님.
- `Limitless_F2_Stump_Natural_Issue81_20261103.zip`: 승인 자연형 그루터기 v4 320×384.
- `F2_P2_Targeted_Sign_Stump_20261029.zip`: 표지판/그루터기 과거 후보.
- `LIMITLESS_Issue75_F2_BendSnow_ArtCandidate_20261110.zip`: F2 눈길 굽음·눈경계 1774×887 ArtCandidate. 11/10 신규 SnowEdge C는 불필요/미채택, B는 FirstSnow 기존 Sprite 재배치 방향; 새 F3 그림과 중복 생성 금지.

중복 회피 결과: F2 목재 표지판·낮은 목재 울타리·그루터기·기존 FirstSnow·길 테두리 그리기 금지. F3에서는 **암석 중심**의 지면 전환 패치 1, 길 바깥 저상 암반 1, **바닥에 밀착된** 얕은 돌·얼음 시각 경계 1로 차별화. 신규 몬스터/타일 대량생성/전체 F3 Scene 제작 없음.

## 2. 실제 신규 투명 PNG 3종

| 역할 | 파일명 (`Art/`) | 실제 게임 후보 규격 | SHA256 |
|---|---|---|---|
| 눈·암석 지면 접합 데칼 | `F3_SnowRock_GroundTransition_256x256_RGBA_CANDIDATE.png` | 256×256 투명 RGBA | `d63c8d5ae60921dc7007caf839b720d68dda8078f273b86f1404f6121475a4be` |
| 길 옆 낮은 빙설 암반 | `F3_LowFrostRock_Outcrop_320x192_RGBA_CANDIDATE.png` | 320×192 투명 RGBA | `ddbba749c8f60f3b207769c35883aa844e80e782e8dc512077608d2568f07bb1` |
| 바닥형 경계 표식 | `F3_FlushStone_EdgeMarker_256x128_RGBA_CANDIDATE.png` | 256×128 투명 RGBA | `ccb99fa51dec1c33d07eaca3fd0fede994b65f0fd5776c98cda690b197ffedc0` |

모두 신규 원화 생성 후 투명 알파 독립 영역3개를 추출하여 개별 캔버스에 비율 유지 LANCZOS 정규화. 원본에서 3가지 서로 다른 지형·소품만 선택, 타사·기존 승인 PNG를 재색칠하거나 재가공하지 않음. 실제 알파 bbox는 `QA/asset_specs.json` 기록. PNG 3/3 개별 디코드·정확 RGBA·비투명 바깥 외곽0, SHA 확인. 원본 ImageGen 출력은 4프레임 GIF 형태의 가로 파노라마이지만 **애니메이션 납품이 아님**. 그 4프레임을 공간상으로 이어붙였을 때 3개 독립 자산이 되고, 이를 각각 떼어 최종 독립 정지 PNG 3개로 납품함.

**스케일 계약:** 이전 [F1 승인 지면6종](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-6307798816)은 256×256, F2 승인/후보 소품은 256×192·320×384 등 실물별 규격이 상이함. F2 FirstSnow는 [Issue #75 기록](https://github.com/HappyPaul812/Project-Limitless/issues/75#issuecomment-6783671968)상 PPU128/Pivot(0.5,0.5) 실측 이력이 있으나 **F3 신규 3종의 최종 PPU·Pivot·TileGrid·Sorting·Collider 규격은 아직 미확정 `SPEC_PENDING`**. 이 파일 3종을 '무조건 512'로 강제하지 않았고, 128/40px 축소 판독은 각 최종 PNG에서 실제 수행함.

**원본/가독성 한계:** 지면 접합은 **한 번 배치하는 가장자리 오버레이 후보**이며 실제 기존 F2 바닥과 4면 seamless tileability는 `NOT_VERIFIED`(반복 Base 타일로서 PASS 아님). 320×192 암반은 길 바깥쪽만 배치, 콜라이더 미작성. 경계 소품은 낮은 돌판+얇은 얼음으로 구성한 **시각 표식**이며 표지판/울타리/벽이나 이동 차단 구조물이 아님. 인게임 휠체어 통행 주요 길폭 4.5WU는 이전 F2 시각 디자인 참고이지만, 이번 F3의 실제 Collision/Entry/Path 물리 통행은 `NOT_RUN`. F3 실측 길폭/배치 좌표를 만들어 주장하지 않음.

## 3. 비교판 및 ZIP

- `Preview/F3_3Asset_ActualScale_128_40_Review_OFFLINE.png`: 실제 개별 후보 PNG 원크기/128/40px 축소 및 투명 Checkerboard.
- `Preview/F2_ApprovedAndCandidate_vs_F3_OfflineStyleComparison.png`: 실제 기존 F2 승인 그루터기와 대안 울타리·길 후보 이미지 그리고 F3 3종의 동일 보드. 표기된 F2 대안은 사용자 최종 미술 승인으로 오인하지 않음.
- `Preview/F3_PassPath_Accessibility_PlacementConcept_OFFLINE.png`: 중앙 주요 통로를 비워 둔 **오프라인 배치 개념도**; 실제 Unity GameView/Collider QA 또는 4.5WU 물리 폭 증명이 아님.
- `Docs/SourceDeclaration_and_ArtLimits.md`, `QA/asset_specs.json`, Manifest, SHA256SUMS 포함.
- ZIP **`LIMITLESS_Issue123_F3_SnowPass_3Asset_ArtPilot_20261123.zip`**, **1,047,761 bytes / 10 entries**, SHA256 **`5704d754355d7a4d353be8324c0c442e0b8672e623fe7492849e40c49c4e8be4`**.
- 완성 ZIP 재개봉 확인: CRC `testzip() is None` **PASS**, Manifest 내용물 **8/8 크기+SHA 전체커버 PASS**, SHA256SUMS 내용물8 + Manifest1 = **9/9 PASS**, Manifest 자기참조/SUMS 순환 참조0.
- 실물 ZIP 및 직접 PNG/비교판은 ChatGPT 대화에서 다운로드 제공. Windows PC `F:\Downloads` 자동 복사·GitHub 원격 바이너리 PNG/ZIP 업로드·Unity 재빌드 **하지 않음**.

## 4. 승인 게이트 / 절대 보호

- **바울 대표 신규 F3 아트 3종 미술 승인 필요: YES.** 중복회피/프리뷰/파일규격 QA PASS는 실제 게임 채택 승인과 다름.
- 이전 11/19 안전체크포인트와 기존 승인 F1/F2/F2 B352/BattleBGv2/M1~M5 모든 아트 원본 및 F2 Ground9/WornTrail/FirstSnow·F2 소품 **바이트 수정0**.
- Unity **6000.6.5f1**, Scene/Prefab/GUID/meta/Collider/Quest/Spawn/NPC/Doors/Save/UserData/Discord/#85 HOLD/main 변경0. 실게임 임포트·타일양산·F4/F5 진행·대형 QA 복제0.
- 추후 사용자 미술 승인 시 **최종 PPU/Pivot/Grid/정확 배치/F2-F3 접속과 휠체어 동선**을 Codex 별도 국소 검증. 현 단계에서 Unity 적용 허가로 간주 금지.

**결론:** `F3_THREE_ASSETS_READY_FOR_ART_REVIEW / SPEC_PENDING / UNITY_NOT_APPLIED`.
