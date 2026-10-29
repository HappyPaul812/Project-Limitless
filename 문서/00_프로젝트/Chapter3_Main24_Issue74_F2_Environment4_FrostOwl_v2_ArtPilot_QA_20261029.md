# LIMITLESS / Chapter3 Main24 / Issue #74 / F2 독립 환경 4종 + 서리올빼미 v2

**일자:** 2026-10-29 KST  
**브랜치:** `docs/art-new`  
**요청 근거:** [Issue #74](https://github.com/HappyPaul812/Project-Limitless/issues/74), [#71](https://github.com/HappyPaul812/Project-Limitless/issues/71), [#72](https://github.com/HappyPaul812/Project-Limitless/issues/72)  
**상태:** `ART_CANDIDATE / OFFLINE_PNG_QA_PASS / USER_ART_REVIEW_PENDING / UNITY_SCALE_NOT_VERIFIED / NOT_UNITY_APPLIED`

## 실제 파일 납품

- **파일명:** `Limitless_Main24_F2_Issue74_v2.zip`
- **SHA256:** `892345e78b83f09c932f4f1e2eeb2bf311c331c7490b325bda04fbdbaee54c4e`
- **크기:** 1,897,130 bytes, 15 entries, ZIP `testzip()` CRC PASS, `SHA256SUMS.txt` **14/14** 실물 재검증 PASS.
- **납품 위치:** 현재 ChatGPT 대화에서 생성된 ZIP `/mnt/data/Limitless_Main24_F2_Issue74_v2.zip` 링크로 제공. 사용자 Windows F: 경로 자동 저장/PC 내 파일 검증은 미실시. ZIP/PNG 바이너리를 GitHub에 업로드하지 않았으며 본 보고·인계만 커밋.
- **신규 후보 PNG 정확히 5개** (`Candidate/` 아래만 게임 자산 후보). 비교판·실축소 PNG는 `Preview/` 구분, 기존 공식 승인 파일의 재등록/덮어쓰기 없음.

| 신규 파일 | 원본 규격 | SHA256 |
|---|---|---|
| `Candidate/Environment/F2_FirstSnow_GroundOverlay_v1_CANDIDATE.png` | 256×256 RGBA 투명 / bbox (10,8)-(246,250) | `61141ace90d5c64c2277da0cb7e27d9674bce6c05f276fb3b9801cd05ffa272a` |
| `Candidate/Environment/F2_SnowPine_v1_CANDIDATE.png` | 256×320 RGBA 투명 / bbox (8,45)-(248,314) | `738827ff84a602ecc1c377e2c8c1398566cdec414f272c4b74855f850ed5075a` |
| `Candidate/Environment/F2_PathEdge_LowFence_v1_CANDIDATE.png` | 256×192 RGBA 투명 / bbox (10,8)-(245,186) | `77c17a36fcdc1532e44f797d6a59f96f7c9f1924dc6293c9ff3d532a98bf3535` |
| `Candidate/Environment/F2_FrostShrub_Low_v1_CANDIDATE.png` | 256×160 RGBA 투명 / bbox (41,8)-(215,154) | `ba66a6754b90db16d26c60e4c634a90cd27d5fd7a8ccc8ded931591071ff85f2` |
| `Candidate/Monster/FrostOwl_KeyPose_512_v2_CANDIDATE.png` | **512×512 RGBA 투명**, bbox (19,19)-(499,481) | `91d3e638a94e873064a2ed06ead333535865301691a77952c4ffa81f376b5000` |

## 아트 실제 제작·비교

- **환경4**: 이미지 생성 컨셉에서 오브젝트 4개를 분리하여 투명 마스킹·후처리하고 개별 RGBA 파일로 저장. 흙 위에 엷게 남는 눈의 가장자리 모티프, 눈 덮인 소나무, 낮은 울타리, 낮은 서리 관목을 각1개 제작. 길 중심을 가리는 대형 설치 금지; 소나무는 길 바깥, 울타리·관목은 길 가장자리 배치 후보. 눈/길 구분은 흙 노출+눈 형태/경계 모양으로 제안.
- **올빼미 v2**: 원본 v1 `Candidate/Monster/FrostOwl_KeyPose_512_CANDIDATE.png` SHA256 **`f60c41cfdc6f3d1bc1b05b62a2281d5f607dec322b9c5c4c3090cac725c09ab1`** 실물 확인·수정 없이 보존. 얼굴의 눈·부리 주변과 꼬리 아래쪽으로 국소 대비/명암 조정한 새 v2만 생성. 512 정사각 RGBA, **v1-v2 alpha 마스크 512² exact equal**, 원래 투명 bbox·실루엣·날개·자세 모두 보존. RGB 변경 27,468 pixels(알파0 영역 포함 RGB 연산 수치; 눈에 보이는 범위는 얼굴/꼬리 중심). 40px은 후보기술 축소일 뿐 바울 대표의 게임 크기 인가 아님.
- **v1↔v2 실제 파생 128·40px**: ZIP `Preview/FrostOwl_v1_128px_DERIVED.png`, `_v1_40px_`, `_v2_128px_`, `_v2_40px_`는 각 실제 512² PNG를 LANCZOS 축소해 계산. `Preview/Issue74_OnePage_Review_OFFLINE.png` **2050×1260**에 환경4+올빼미v1-v2 512/128/40 및 40×4 NEAREST 한장 포함. SHA `6255e4d3d7f95af58f45208bdc20813088e2acd05a4594bf92c7479cb76cdafa`.
- `Preview/FrostwayApprovedGround_vs_F2Overlay_OFFLINE.png` **1590×620** 은 기존 v10 패키지의 **게임 원본** `Tiles/Frostway_Ground_Earth_01.png`, `ThinFrost_01.png`, `Gravel_01.png` 실 PNG를 읽고 새 첫눈 오버레이를 별도의 사본에 합성한 **오프라인 참고판**. SHA `2fc5150fc3822504ee117179a34775a702dbe9397a0e9025d31a25ac15bdb95f`. 기존 F1 실 Unity GameView나 동일 Tilemap/PPU 내 시연은 아님.
- 환경 소품은 서릿길/F1 원본 **톤 참조**로 작업. 실제 배치 시 F2 첫눈에 맞춰 자산의 색감/눈 밀도와 지면-오버레이 접합 추가 QA 필요. 개별 **PPU/Pivot/Footprint를 게임 반영 계약으로 확정하지 않는다**.

## 규격의 실제 근거 vs 미확정

- 실제 `main` GitHub Unity `.png.meta` 읽기: `NorthF1_EarthToFrost_Transition_Overlay_v3`, `NorthF1_Path_WornTrail_Overlay_v2`, `NorthF1_Prop_FrostShrub_01_v2`, `NorthF1_Prop_Waymarker_01_v3`, `Frostway_Ground_Earth_01` 등 **Importer PPU128** 관찰. Pivot은 F1 관목(0.5,0.0546875), F1 길 overlay(0.5,0.5), 승인 Frostway ground(0.5,0) 등 **용도별 달라 그대로 복붙 불가**.
- 이번 독립 후보의 **새 `.meta` 생성/Unity Import/실제 WU/Collider/Sorting·경로 침범 검사는 하지 않음**. `NEW_ASSET_PPU_PIVOT_FOOTPRINT=SPEC_PENDING`; 단, 크기는 위 실제 PNG 픽셀 규격 확정. F1에서 참조한 PPU128은 권장 참고값이지 F2 신규 계약 승인값이 아님.
- 실제 F2 맵 전체/타일 대량 생산0, 경계 타일 연결 여부 `SEAM_NOT_VERIFIED`, 휠체어 실제 이동 폭 `NOT_TESTED`. 새로운 몬스터·스킬/한기/전투 ID·VFX·Spawn/Idle/Attack/Hit/Defeat 생성0.

## 보존·QA·다음 승인 게이트

- 이전 **#71 owl v1 SHA**는 덮어쓰기 없이 정확히 일치. 기존 승인 Main23 환경43·승인지면9, F1·서리들쥐·Wolf/Bear/Fox, Chapter1·2 숲거미/독침벌/공식 몬스터, 오르나 **visible alpha80px (canvas84.63px)**, Residence·Inn KEEP/모듈HOLD, Save/Quest/Scene/GUID/.meta/UserData 모두 변경0. 이번 작업은 새 ZIP과 보고서만; 원본 Unity·게임 파일은 직접 편집하지 않음.
- QA: PNG 5/5 RGBA/Alpha presence/bbox PASS; 올빼미 v1 SHA, alpha unchanged PASS; 128/40 실제 파생4종 생성; ZIP 15 files CRC PASS / 14 sums PASS. 시각적 최종 판단, F2 인게임 스타일·겹침·가독성, Ch2·서리들쥐 동일PPU/실 Unity 비교는 **HOLD/NOT_RUN**.
- `ART_CANDIDATE / OFFLINE_PNG_QA_PASS / USER_ART_REVIEW_PENDING / UNITY_SCALE_NOT_VERIFIED / NOT_UNITY_APPLIED`. 바울 대표 **최종 미술 승인 아직 없음**. 승인→Codex 별도 격리 Unity PPU/Pivot/Collider/타일 경계/휠체어 길/올빼미 렌더 추가 검증→**별도** 본게임 적용 승인 순서. `docs/art-new` 보고서→`SPRITE_ART_LATEST.md` 인계→Issue #74 댓글, `main` 직접 커밋·Push·병합 없음.
