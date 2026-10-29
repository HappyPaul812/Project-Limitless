# LIMITLESS / Main24 F2 / Issue #75 — 첫눈의 숲 P2 독립 환경 소품 4종 파일럿

**작업일:** 2026-10-29 KST  
**브랜치:** `docs/art-new`  
**상태:** `P2_ART_CANDIDATE / OFFLINE_FILE_QA_PASS / PPU_PIVOT_SPEC_PENDING / USER_ART_REVIEW_PENDING / NOT_UNITY_APPLIED`

## 1. 배경·우선순위

- [Issue #75 P2 단독 제작 기획](https://github.com/HappyPaul812/Project-Limitless/issues/75). [#74 보호·검수](https://github.com/HappyPaul812/Project-Limitless/issues/74#issuecomment-6489115424) 준수. **서리올빼미 Idle8/Attack4 제작 작업과 완전히 독립된 P2 소품 ZIP**이며 게임 런타임·전투 몬스터 정의와 무관.
- 기존 F2 P1 GroundOverlay/SnowPine/FrostShrub 3 PNG는 `KEEP_CANDIDATE`, 울타리 v2는 바위/관목 접지 alpha 혼선으로 `ART_REVIEW/HOLD`, 위 자산은 어떤 파일도 편집하지 않음. Owl v1/v2/v3 원본·프레임/애니메이션도 이 P2 패키지 작업으로 수정하지 않음.

## 2. 실제 납품 파일 및 SHA

- **ZIP 이름:** `F2_P2_Environment_4Assets_20261029.zip`
- **ZIP SHA256:** `d749d7fd2937849326e9ecc3ca9f9e83f5646447e27d21fd1c3206ea0f650088`
- **크기:** 640,244 bytes, 9 entries. ZIP CRC `testzip() PASS`, `SHA256SUMS.txt` **8/8** 대조 PASS.
- **현재 ChatGPT 대화 다운로드 경로:** `/mnt/data/F2_P2_Environment_4Assets_20261029.zip`. GitHub에 ZIP·PNG 바이너리 업로드 **0**, 텍스트 보고만 커밋. 사용자 Windows `F:\Downloads` 로컬 복사본 수령 및 SHA 확인 **미실시**.
- 후보 PNG 4개 전부 **독립 투명 RGBA**, 개별 `Candidate/` 아래 새 경로. 128·40 실제 종횡비 보존 축소·한눈 비교판 포함. 파일 내부 `Manifest.json.artwork_files`에 전 PNG SHA256, 크기, bbox 및 전체 목록.

| 새 후보 PNG | 실제 크기 | SHA256 |
|---|---|---|
| `Candidate/F2_FallenSnowBranch_v1_CANDIDATE.png` | 384×320 RGBA | `b05856bb255b6072f0ec91987cfb07a17a464ff60460fde0b156bdda0e4318e0` |
| `Candidate/F2_SnowStumpPerch_v1_CANDIDATE.png` | 320×384 RGBA | `7a5ce6129dd1856dad7fb4b521b9d890e7a2b0942397d2fa6a1fc4aaae6d25cc` |
| `Candidate/F2_FirstSnowTrailSign_v1_CANDIDATE.png` | 288×384 RGBA | `faa0b93dd7617e1d02391ab78b0b052861f5cb490e85277f869c002422e5c410` |
| `Candidate/F2_GentleFirstSnow_Overlay_v1_CANDIDATE.png` | 512×512 RGBA (저불투명 눈 입자) | `3a57f2c936bea6a07f4d80d3e49f0dbce7b8df9398fc33244d1c48a999702bf9` |

## 3. 산출방식/접근성·미술 품질

- 생성된 별도 F2 환경 아트 원본에서 눈 가지·나무 그루터기/횃대·표지판을 각각 분리해 알파 배경 제거·픽셀 단위 크롭 후 **원본 P1과 별도 후보로 저장**. 세 자산은 기존 F1 소품 PNG 단순 트레이스나 원본 재색칠이 아닌 신규 파일럿. 눈발은 별도 절제한 512 RGBA 반투명 독립 레이어(정적 파일)로 제작.
- **쓰러진 눈 가지**는 숲길 가장자리 바깥 장애물로 오해되지 않도록 낮게 배치할 후보. **그루터기/횃대**는 처음 생성 아트에 포함된 작은 올빼미 그림을 **삭제하여 기존 승인 Frost Owl v3 중복 묘사 방지**; 남은 그루터기/빈 목재 받침 후보. **표지판**은 화살 형태 자체로 길의 방향을 구분하되 조형물 내부의 미확정 가상 지명 문양은 실제 게임 문구 확정 자료가 아니므로 배치 전 교체/제거 검토 필요. **눈발**은 낮은 불투명도 정적 레이어로 사용, 시력·감각 과민에 영향을 줄 수 있는 높은 밀도나 번쩍이는 애니메이션 만들지 않음.
- **품질상 미결**: GrabCut 계열 배경 제거 후 낮은 눈바닥이 불규칙하게 끊어 보이는 일부 픽셀 가장자리/원화 기반 소품 음영을 실제 승인 픽셀 아트 스타일로 다듬어야 할 가능성. 오프라인 PNG 스펙 PASS가 미술 최종 PASS를 의미하지 않는다.
- `Preview/OnePage_F2_P2_Review.png`는 4종 후보를 한눈에 보여주는 **OFFLINE** 비교판. `Preview/F2_P2_128_40_ActualScale_Compare.png`는 **실 PNG의 원종횡비를 유지한 128px·40px 축소**. 3개 소품의 낮은 발판이 통행을 가리거나 휠체어 Collision을 유발하지 않는지는 **Scene 없음 / NOT_TESTED**. 표지판 목적지·QuestID·Sprite Sorting·Pivot/PPU/Footprint 역시 미정.
- F1 관련 GitHub `.png.meta` 읽기: NorthF1 주요 소품·지면 `PPU128`을 사용하나 Pivot은 관목 `(0.5,0.0546875)`, 길 overlay `(0.5,0.5)` 등 서로 다름. 따라서 **신규 후보의 PPU/Pivot/가림·footprint·Tilemap 좌표는 `SPEC_PENDING`**, 파일 해상도를 게임 세계단위 계약으로 확정하지 않음. Unity `.meta` 새로 생성하지 않음.

## 4. 검수/보호와 다음 단계

- OFFLINE: PNG 4/4 decode·RGBA·투명 alpha PASS; ZIP CRC PASS, SHA256SUMS8/8 PASS. PPU/Pivot/현장 배치·길/휠체어 통행·실제 16:9/4:3/21:9 Unity GameView **NOT_RUN**.
- 승인 자산 Main23 환경43/지면9·Main22 F1·기존 몬스터·서리들쥐·Wolf/Bear/Fox·Chapter1/2 Spider/Bee 리뉴얼·Main23 Orna visible80px / Residence & Inn KEEP · 새 RoofGable/Trim/EntryDoor·Inn Standalone HOLD·Quest/Save/UserData/Scene/Collider/Sorting/GameObject·`main` 모두 변경0.
- **현재 신규 P2 4종의 바울 대표 미술 최종 승인 필요**(`USER_ART_REVIEW_PENDING`). 4종 개별 후보 및 눈발 약한 가독성·길 장애물/표지판 가짜 글자 여부를 대표 검토 → 승인 후보만 Codex 격리 Unity 실제 PPU·Pivot·Collision/동선·3화면비 QA → **별도 본게임 적용 승인**. Runtime VFX/ParticleSystem, F2 맵 양산/자동 본편 적용 0.
