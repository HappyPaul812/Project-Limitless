# LIMITLESS / Chapter3 Main24 F2 / Issue #75 — 길 굽음·눈 패치 보완 미술 시안 납품 및 대표 피드백 (2026-11-10)

**담당:** 메인 신규 아트 `docs/art-new`  
**근거:** [#75 2026-11-10 총괄 기획 지시](https://github.com/HappyPaul812/Project-Limitless/issues/75#issuecomment-6755493865) · [기존 격리 68PASS](https://github.com/HappyPaul812/Project-Limitless/issues/75#issuecomment-6603987141) · 본 대화 바울 대표 피드백 **“후보가 괜찮네..”**.  
**상태:** `USER_ART_DIRECTION_ACCEPTED / OFFLINE_CANDIDATE_FILE_QA_PASS / SPRITE_IMPORT_SPEC_PENDING / F2_REAL_BEFORE_AFTER_NOT_VERIFIED / UNITY_NOT_APPLIED`.

## 1. 최종 납품 실물

- ZIP: **`LIMITLESS_Issue75_F2_BendSnow_ArtCandidate_20261110.zip`**, **3,435,324 bytes / 10 files**, **ZIP SHA256 `151094aacd847ee7b4338ca08a827d0edaa21a1122df357379f0bd493afabd8a`**.
- ZIP CRC `testzip PASS` / `SHA256SUMS.txt` **8/8 파일 실제 해시 검증 PASS**.
- **신규 독립 이미지 1장:** `Candidate/F2_SnowPath_BendEdge_ArtApproved_CANDIDATE_1774x887_RGBA.png`, **1774×887 RGBA 투명 PNG**, SHA256 `1db66eb4f92931318d5fafd46b2cd337fb9d47033537b428e21c1471d71490dd`, 원본 생성 파일과 byte-exact 복사. 1774×887 크기는 사용자 승인한 시각 시안의 실제 크기이지 **F2 공식 256×256 Tile/PPU/Pivot/Atlas 규격이 아님**.
- **시각 검토 원본 1장:** `Preview/F2_BendSnow_ConceptComparison_OFFLINE.png`, **1448×1086 RGB**, SHA256 `849e1266aa359b2730822c531024e098eedc28f107877c69011301136f67203a`. 이 비교판의 **Before/After는 생성된 콘셉트 목업**이며 #83/#85 실제 F2 Unity CameraRT의 전후 사진이 아니다. 실제 F2에 적용됐거나 물리 폭이 증명됐다고 보고하지 않는다.
- **실제 축소판:** `Preview/F2_BendSnow_Width128_RatioPreserved.png` (**128×64**) 및 `Preview/F2_BendSnow_Width40_RatioPreserved.png` (**40×20**), 원본 비율을 지키는 LANCZOS 축소; 128×128/40×40으로 왜곡하지 않음.
- Manifest/README_KR/DesignNotes/SourceDeclaration/QA_Result/SHA256SUMS ZIP 동봉. 파일들은 ChatGPT **Library `/LIMITLESS/Issue75_F2_BendSnow_20261110/`에 ZIP과 비교판 개별 업로드 성공**. GitHub에는 보고·인계 문서만 작성, PNG/ZIP 바이너리 직접 업로드하지 않음. 대표 PC F:\Downloads 자동복사·로컬 SHA 대조는 미실시.

## 2. 승인 범위의 정확한 구분

- 바울 대표는 신규 시각 후보를 보고 **“후보가 괜찮네..”**라고 피드백. 따라서 **F2 잔설·굽은 흙길의 미술 방향 긍정 수용**으로 기록한다. 단, **완성 게임 Sprite Import 규격·실제 F2 이미지 교체·정식 적용 승인까지 확대하지 않는다**.
- **이 시안은 길·잔설·돌/관목을 함께 포함한 긴 투명 이미지**로서, 기존 F2 공식 Ground9·F1 WornTrail·FirstSnow Overlay 조합과 중복될 수 있다. Codex는 정식 작업 전 기존 승인 파일들의 재사용으로 같은 시각 효과가 나오는지 확인하고, **긴 후보 전체를 자동으로 Tilemap/Collider에 배치하지 말 것**. 필요한 경우 가장자리 보완 파트만 별도 실제 256px tile/PPU/Pivot 측정 후 검토해야 하며 추가 제작은 별도 승인 필요.
- 기존 #75 Unity6000.6.5f1 **68 PASS / 0 FAIL**은 당시 기존 아트로 placeholder를 교체한 **격리 기능 결과**이고, 이번 새 후보의 기술 테스트와는 무관하다. 실제 4.5WU 길폭/물리 Collider·휠체어 통행 **이번에는 실행·수정·검증0**.
- 승인 Owl v3 Idle8/Attack4, NaturalBroken Stump v4, Frostway Ground9·Environment43, F1 WornTrail/F2 Overlay/P1·P2, 미승인 Sign/Fence, 정식 냉기 VFX/상태아이콘 및 Chapter3 BattleBG v2 모두 원본 수정0. 원본 Unity Scene/Prefab/Renderer/Collider/Save/UserData/BuildSettings/Packages/main 변경 및 Push/병합0.

## 3. 시각 판단과 후속 게이트

- 본 시안은 **흙길 곡선을 완만하게 보이도록 눈·잔설 경계를 들쭉날쭉하게 설계**한 미술 방향이며, 눈 패치가 같은 형태로 반복되지 않는 구도를 제안한다.
- Codex가 기존 F2 실제 `After_Bend_1280` 캡처 x180~350/y100~650 눈 반복, x480~880/y180~510 굽음의 원본과 동일 위치에 기존 F2 승인 소스를 재배치한 비파괴 안과 이 시안을 비교해야 **현장 결함 해소**를 판정할 수 있다.
- **다음 단계(아직 실행 금지):** 바울 대표의 별도 적용 지시 후, Codex 격리 Unity에서 원본 폴더 밖 시험 사본으로 실제 기존 지면/길/Overlay 조합과 본 아트를 비교, 960/1280/1680 CameraRT·4.5WU 휠체어/도보 실제 통행·길 경계 가독성·Tile 이음새 최소 Delta QA. 그 이후 별도 원본 게임 적용 승인.
- 원본 변경이 전혀 없으므로 기존 68PASS·Owl/Stump·BattleBG 및 기타 전수 QA 재실행0.

**현재 판정:** `ART_DIRECTION_USER_ACCEPTED / GAME_READY_IMPORT_NOT_VERIFIED / OFFLINE_ONLY / NOT_UNITY_APPLIED`. 화면을 보고 승인된 미술 방향을 보존하되 **게임용으로 검증된 최종 Sprite라는 주장은 하지 않는다**.
