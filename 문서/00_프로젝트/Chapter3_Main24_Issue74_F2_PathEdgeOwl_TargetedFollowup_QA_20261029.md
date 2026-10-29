# LIMITLESS / Main24 F2 / Issue #74 — PathEdge 접지·서리올빼미 v3 표적 보완

**작업일:** 2026-10-29 KST  
**담당 브랜치:** `docs/art-new`  
**최종 상태:** `ART_CANDIDATE / OFFLINE_PNG_QA_PASS / USER_FINAL_ART_REVIEW_PENDING / UNITY_NOT_VERIFIED / NOT_UNITY_APPLIED`.

## 1. 근거 및 결정

- [Issue #74 Codex 통합 읽기 전용 검수](https://github.com/HappyPaul812/Project-Limitless/issues/74#issuecomment-6488218748): 기존 환경 4 PNG, 올빼미 v2 기술 파일 검증 PASS. GroundOverlay·SnowPine·FrostShrub 3종 `KEEP_CANDIDATE`; PathEdge는 눈판/울타리/바위/관목의 접지 클러스터 `TARGETED_REVISE`; 올빼미는 v2 알파·날개 형상 유지하되 40px 얼굴·부리·꼬리 부분 개선 여부 검토. 실제 Unity PPU/Collider·휠체어 길 가독성은 `SPEC_PENDING`.
- [Issue #74 기획 후속](https://github.com/HappyPaul812/Project-Limitless/issues/74#issuecomment-6488506287): 기존 3종·Owl v1/v2 원본 보존, 울타리 최대1 PNG, Owl 선택적 최대1 PNG, 128/40 실픽셀 비교, 사용자 최종 미술 승인 전 본게임 및 main 병합 금지.
- 본 작업은 **위 조건에 따라 독립 신규 후보 PNG 2장만 제작**. 기존 승인43·승인지면9·F1 환경·공식 몬스터/NPC/펫·숲거미/독침벌·Main23/24 Scene/Quest/Save/.meta/GUID 수정0.

## 2. 실제 납품 ZIP (게임용 후보 PNG 2개)

- `Limitless_F2_Issue74_Followup.zip` **1,560,774 bytes**, **9 entries**, **ZIP SHA256 `eee82ef0d9e0c8092f27258404aa12e1b917af68521128a6283b4ee5a3fba3a4`**.
- ZIP `testzip()` **CRC PASS**, `SHA256SUMS.txt` **8/8** 본문 파일 SHA256 검증 PASS. `Manifest.json`과 `QA/Offline_QA.json`의 원본 바이트 보존/변경 통계 확인.
- ZIP은 ChatGPT 현재 대화 파일 다운로드 **`/mnt/data/Limitless_F2_Issue74_Followup.zip`**로 전달. **원격 GitHub에 PNG/ZIP 바이너리 업로드하지 않았고 사용자 PC `F:\Downloads` 자동복사/해시 재확인 없음**.
- ZIP에는 신규 독립 PNG 2, 비교 PNG 3, QA JSON, Manifest JSON, 한국어 안내, SHA 목록 포함. 기존 원본 5 후보를 다시 납품해 별도의 승인 자산으로 혼동하지 않도록 새 ZIP에 원본 바이너리는 중복 수록하지 않고 **원본 SHA 목록으로 보호**.

| 새 독립 후보 PNG | 실제 크기·모드 | SHA256 |
|---|---|---|
| `Candidate/Environment/F2_PathEdge_LowFence_v2_CANDIDATE.png` | 256×192 RGBA, bbox(10,8)–(245,175) | `5a79c4ab41e5700dcaac1a68a0929323f10adf0643b7042bf0d3021ee26788ff` |
| `Candidate/Monster/FrostOwl_KeyPose_512_v3_CANDIDATE.png` | 512×512 RGBA, bbox(19,19)–(499,481) | `81a31cf37cb834799ab5c9428da612ab13a5a496025805685daefed7b5d3c467` |

### PathEdge v2: 하단 접지 클러스터만 수정

- 원본 `F2_PathEdge_LowFence_v1_CANDIDATE.png` SHA `77c17a36fcdc1532e44f797d6a59f96f7c9f1924dc6293c9ff3d532a98bf3535` **원본 바이트 보존**.
- 울타리 상단 포스트/레일·눈 윗면과 기존 관목·바위 배치 구성은 그대로. 바닥 앞쪽의 연속된 눈판 알파를 비정규 접지 가장자리로 점진 감쇠하고 주변 낮은 장식 밀도만 조금 완화하여 길 경계가 두툼한 단일 판처럼 보이지 않도록 시각 후보화.
- 원본 대비 alpha 변경 위치 **11,089px**, 상단 **y<94 변경0**. 신규 지형 타일·울타리 증식·Collider·footprint 지정·실 Unity 도보·휠체어 길 침범 검증 **0**. 특히 path edge 표면의 자연스러움은 대표 미술 검토 필요.

### Frost Owl v3: 얼굴·눈·부리·꼬리만 명암 보완

- 1포즈 기존 v1 SHA `f60c41cfdc6f3d1bc1b05b62a2281d5f607dec322b9c5c4c3090cac725c09ab1`, 기존 v2 SHA `91d3e638a94e873064a2ed06ead333535865301691a77952c4ffa81f376b5000` **원본 바이트 모두 보존**.
- v2에서 v3는 **512×512 alpha 262,144 픽셀 완전 동일**. bbox (19,19)–(499,481), V형 날개·전체 실루엣·투명 여백 동일. RGB 변경 **25,114픽셀**, **얼굴(250≤x≤364,231≤y≤324) / 꼬리(165≤x≤328,367≤y≤482) 밖 변경0**. 색의 기존 밝은 영역을 약간 더 밝게, 눈테두리/부리·꼬리 깃 분리 경계를 약간 더 짙게 하되 자세·종류·원본 스프라이트 실루엣 변화 없음.
- 확대했을 때는 국소 명암이 강화되지만 **40px에서 사용자 최종 눈·부리·꼬리 판독성 합격이라고 아직 주장하지 않음**. 기존 Ch2 몬스터와 동일 Unity PPU/실화면 비교도 `NOT_VERIFIED`. Idle/Attack/Hit/Defeat·전투효과·스폰/몬스터ID 구현0.

## 3. 사용자 비교판 / 실제 PNG 파생

- `Preview/Issue74_Followup_OnePage_Review_OFFLINE.png`, **1640×960**, SHA `e71d2a7b241851e79c61cea970eee8ef0df309e15ae112723c4c1c85f58a878a`: 보호 환경3·울타리 v1/v2·Owl v1/v2/v3를 한눈에 비교.
- `Preview/Fence_v1_vs_v2_256_128_40_OFFLINE.png`, **1260×740**, SHA `adbac7b5c9827fa9528d7041fef59f95752b4d0bd0c2639cd3d0a8ff0d1e1725`: 울타리 원본 대비 보완; 128/40 실제 원본 픽셀에서 LANCZOS 축소 (원래 비율 유지).
- `Preview/FrostOwl_v1_v2_v3_512_128_40_OFFLINE.png`, **1650×930**, SHA `f6192b30d418b019fefc1c0c6f9ecb3e1f714c80251064afeff8abed4fb7e609`: 각각 **실제 512/128/40 RGBA 파일에서 표시용 체커 배경에 합성**, 원본 알파/윤곽 보존. 게임 런타임 사진이 아닌 **OFFLINE 참고**.

## 4. 보호·검수 분리·다음 게이트

- 입력 ZIP `F2_art_74.zip` SHA **`892345e78b83f09c932f4f1e2eeb2bf311c331c7490b325bda04fbdbaee54c4e`** 확인. 그 내부 GroundOverlay·SnowPine·FrostShrub 3종 `KEEP_CANDIDATE`, 기존 PathEdge v1·Owl v2는 수정하지 않음. Owl v1도 기존 실물 원본 SHA 검증.
- 오르나 alpha-visible 80px/wholecanvas84.63px, 기존 Residence/Inn KEEP, 신규 RoofGable/Trim/EntryDoor/독립간판 HOLD, F1/서리들쥐/Wolf·Bear·Fox, 기존 Ch1·2 몬스터·리뉴얼 작업 보호.
- **기술적으로 완료한 것:** PNG decode/해상도/투명 RGBA, 울타리 상단 변경 없음, 올빼미 alpha 동일/변경 영역 내 RGB 수정, ZIP CRC·SHA 전수. **아직 실행하지 않은 것:** 실제 Unity/3화면비/PPU·Pivot/Collider·Sorting·F1→F2 지형 이음매·실 휠체어 통행·게임 40px 미술 승인.
- **대표 최종 미술 승인 필요:** `USER_FINAL_ART_REVIEW_PENDING`. 후보와 실제 Unity 적용은 다름. 추후 승인 시 Codex 격리 Unity에서 두 PNG만 PPU/Sorting/배치/길 가림·몹 판독성 최소 검증 → 별도 본편 적용 승인. 자동 Unity 적용0/`main` 직접 commit·Push·merge0.

**정식 운영:** 본 보고서 `docs/art-new` commit → `SPRITE_ART_LATEST.md` 갱신 → Issue #74 결과 댓글 순서.
