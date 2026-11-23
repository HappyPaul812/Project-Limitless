# LIMITLESS / Chapter 3 F3 최종 미술 검토 자료 + F4 백색 침엽림 환경 전환 기획 — 2026-11-24

**Issue:** [#123](https://github.com/HappyPaul812/Project-Limitless/issues/123)  
**최신 발주:** [2026-11-24 기획 병렬 부서 지시](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7115938974)  
**기획 정본:** [20261124 병렬 부서 발주](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/20261124_Parallel_Department_Orders_While_Issue85_Repair.md)  
**담당 브랜치:** `docs/art-new`  
**상태:** `F3_ART_REVIEW_MEDIA_READY / F3_3_OF_3_ISOLATED_IMPORT_PASS_PREVIOUSLY / USER_F3_ART_FINAL_APPROVAL_PENDING / F4_CONCEPT_ONLY / UNITY_NOT_APPLIED`

## 1. 이미 완료된 F3 파일럿 3종 보호 및 근거

[기존 11/23 제작 보고](Chapter3_F3_SnowPass_Issue123_ArtPilot_20261123.md)와 [Codex 격리 Unity 기술 검수](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7087543816) 확인. 기존 F3 3종의 **오프라인 파일 QA 3/3 PASS, 실제 Unity 6000.6.5f1 격리 Sprite Import 3/3 PASS**는 재실행하지 않고 기존 증거를 재사용. **대표 최종 미술 승인 대기**, 정식 F3 Scene·길폭/Collider/Sorting·실제 통행 **NOT_VERIFIED**.

| 기존 F3 PNG (실제 원본, 신규 제작 아님) | 규격 | 회수한 SHA256 | 제한 |
|---|---|---|---|
| `F3_SnowRock_GroundTransition_256x256_RGBA_CANDIDATE.png` | 256×256 RGBA | `d63c8d5ae60921dc7007caf839b720d68dda8078f273b86f1404f6121475a4be` | **단일 겹침 오버레이 ONLY, 반복 Base 타일 금지** |
| `F3_LowFrostRock_Outcrop_320x192_RGBA_CANDIDATE.png` | 320×192 RGBA | `ddbba749c8f60f3b207769c35883aa844e80e782e8dc512077608d2568f07bb1` | 통행로 밖 배치, 접지/Sorting/Collider 미검증 |
| `F3_FlushStone_EdgeMarker_256x128_RGBA_CANDIDATE.png` | 256×128 RGBA | `ccb99fa51dec1c33d07eaca3fd0fede994b65f0fd5776c98cda690b197ffedc0` | 바닥형 **시각 경계**. 청색이 얼음/물/위험으로 오인되는지 실제 F3 맵에서 검증 필요 |

이번 작업 시작 시 원본 3PNG를 ChatGPT Library `/LIMITLESS/Issue123_F3_SnowPass_Pilot_20261123/`에서 직접 회수했고 위 SHA와 일치. 재작화/편집·덮어쓰기0. 이전 납품 ZIP `LIMITLESS_Issue123_F3_SnowPass_3Asset_ArtPilot_20261123.zip` SHA256 `5704d754355d7a4d353be8324c0c442e0b8672e623fe7492849e40c49c4e8be4`, CRC PASS. 이번 검토 ZIP에는 원본 게임용 F3 PNG를 **중복 재납품하지 않고**, 원본 PNG의 비변형 합성 검토판만 담음.

## 2. 최종 미술 승인용 자료 — 3장

| 검토 자료 | SHA256 | 판정 범위 |
|---|---|---|
| `Review/F3_OriginalPixels_1to1_F2F3_FinalArtReview.png` | `ca443bcfe9f2ef8f0b76e6550bd68b96f456fef80f29e38f4a23eb22066ac651` | F2 실제 PNG와 F3 3종의 원본 픽셀 **1:1 화면 좌표** 비교. PNG 콘텐츠 스케일·색상 변경0 |
| `Review/F3_AccessibleRoute_OfflinePlacement_NotValidated.png` | `3c82b1574bc89e6869db78c927ab480fe97184e3ed4cff84073f0f2bd1767d56` | 중앙 동선 비움/길가 암반·시각 표식·단일 전환 오버레이 배치 **개념도** |
| `Review/F4_WhiteConifer_BlizzardPalette_ConceptOnly.png` | `aeea0f06d4c7a1d077f56ad70f11c633460f5173f23c0fa2dbc09e9359c87720` | F2→F3→F4→F5 명암 변화, F4 색조와 연출 가이드만. 게임 아트 아님 |

**동배율 보드 기준:** F2의 승인 `F2_SnowStumpPerch_NaturalBroken_v4` 320×384 실물과 F2 미승인 참고 후보(`F2_PathEdge_LowFence_v3` 256×192, `F2_FirstSnowTrailSign_v3` 288×384)를 명확히 구별. F3 각 후보 256×256 / 320×192 / 256×128을 **이미지 1원본 픽셀 → 보드 1픽셀**로 합성. F2 384px 소재의 완전 투명 상단만 표시 공간을 위해 생략하고 실체 픽셀 내용은 그대로; 전체 스케일 정규화나 타일링 없음. F2의 미승인 소품을 공식 승인 아트라고 주장하지 않음.

**동선 배치도 원칙:** F2 숲 측 승인 그루터기는 진행선 밖, F3 낮은 암반은 길 상부 바깥, 지면 전환은 하부 **단일 오버레이 후보**, 돌 경계는 길가 낮은 시각 표식. 도식의 길 넓이/가이드 라인은 **픽셀 그림의 설명용 요소이며 4.5WU 실측값이나 정확 Scene 좌표가 아님**. 원본 3 PNG를 미리보기에 합성할 때 리샘플링·재색칠하지 않았음.

F3 파일 3/3 기술/격리 Import PASS 근거 유지. 그러나 **실제 F3 Scene, 도보·휠체어 4.5WU 유효폭, Pivot/Sorting/Collider/접지 및 경계의 푸른색 인지**는 모두 `NOT_VERIFIED`. 기존 960/1280/1680 격리 시험 렌더를 본편 GameView로 오인하지 않음.

## 3. F4 백색 침엽림 — 신규 게임 아트 없는 한 페이지 기획

[F4 정식 지역 및 몬스터 조우 기획](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/08_몬스터/Chapter3_북부몬스터7종_전투밸런스_20261118.md) 기준 F4는 **백색 침엽림**. F3 암석·설원 길에서 F4 침엽수 그늘과 방향성 강한 눈발로 전환; F5의 짙은 균열 대비는 F4에서 미리 복제하지 않음. F4 첫 눈보라 정령 **단독**, 이후 얼음갑충+정령 혼합(갑충 단독 유지), 2개체 목표, **동상 해제 수단 없는 진입 금지** 등 기존 정본 유지.

**기획용 팔레트 후보(게임용 파일 수정/승인 아님):**
- 주 통행로 밝은 눈 `#DCEAF0`; 풍설 흐름 밝은 회청 `#B9CFDC`; 수목/암반 중간 `#91AABB`; 수목 깊은 윤곽 `#4A6A81`; 기존 안전거점이 기획 승인된 경우에 한한 온기 강조 `#D8C8A6`. 이 팔레트는 제안 단계라 현 F3 공식 PNG에 색 보정 적용0.

**필요 아트 후보 우선순위 (제작/수량 최종 승인 아님):**
1. **P0 재사용 감사:** F2/F3 승인 나무·암반·경계·설면을 그대로 활용할 수 있는지 먼저 확인. F3 GroundTransition을 반복 타일로 전용 금지.
2. **P1 백색 침엽수 실루엣 소량 후보:** F2 나무를 단순 재색칠하지 않고 흰 가지·청회색 깊은 줄기로 F4의 식생 밀도 구분, 경로/인물 가리지 않는 높이.
3. **P1 바람에 쌓인 적설 자국/지면 오버레이 소량 후보:** F3 눈·암석 단일 오버레이 중복 금지. 반복형 Base/타일 규격은 별도 검토.
4. **P2 설분/바람방향 VFX 후보:** 투명도·밀도 낮음, 전투 예고와 눈보라 정령 구분, 저모션 대체/성능 검증 필요. 이번엔 생성0.
5. **P2 숲 그늘·낮은 눈둔덕 경계:** F3 저상 암반·바닥 표식 재사용 먼저 검토하고 부족한 경우만 발주.

**접근성:** 밝고 넓게 읽히는 길, 출입구/전투 예고 가림 금지, 과밀 설분·점멸·강제 흔들림 지양. 실측 4.5WU 및 F4 Scene은 `NOT_VERIFIED`; 실제 GameView/Collider·조우 위치/정식 스폰은 별도 기획과 승인 필요. 이번에는 **F4 신규 PNG/타일/Scene/수치/Unity 구현0**.

독립 본문 및 팔레트 가이드는 납품 ZIP의 `Docs/F4_WhiteConifer_TransitionAndAssetCandidates_20261124.md` 동봉.

## 4. 파일 무결성 및 납품

- **`LIMITLESS_Issue123_F3_FinalArtReview_F4Concept_20261124.zip`**, **1,113,445 bytes / 8 entries**, SHA256 **`191d9f52c6c4baba35cb1cd276ffa98573b9c64875baa5db1463320438520098`**.
- ZIP 재개봉 CRC PASS, Manifest payload **6/6 파일 크기·SHA·목록 완전커버 PASS**, SHA256SUMS payload6+Manifest1 **7/7 PASS**. Manifest 자기참조·SUMS 참조 제외, SUMS 자기참조 제외/순환 없음. QA source inventory JSON 포함.
- ChatGPT Library `/LIMITLESS/Issue123_F3Review_F4Concept_20261124/`에 ZIP·검토보드3·F4 기획 Markdown **5파일 업로드 성공**. 대화에도 실제 ZIP/PNG/MD 링크. 사용자 PC F: 자동 복사 및 원격 GitHub 이미지 바이너리 업로드0.

## 5. 승인 게이트·절대 보호

**바울 대표 F3 3종 최종 미술 승인 필요 YES**. F4는 **기획안 검토 및 후속 별도 미술 발주 승인 필요**, F4 신규 아트0. Codex/Unity 재검수·대형 복제0. Unity6000.6.5f1, #85 Codex 단독 복구, 11/23 원격 안전 체크포인트 `65cceb2d6c3283d18abec6e27d8a6f1296411f17`, Package/.meta/GUID·Scene·Prefab/Collider/Quest/Spawn/Save/UserData/Discord·키·기존 승인 F1/F2/B352/M1~M6/Defeat4 후보/BattleBG/BGM/SFX/TTS/main 보호.

`docs/art-new`에 문서/최신 인계만 커밋하고 Issue #123에 댓글 보고. main 직접 commit/Push/Merge0, **본편 적용0**.
