# LIMITLESS / Issue #97 — VenomSnake Anim8 모션 연속성 두 프레임 표적 보완 v2

**일자:** 2026-11-10 · **담당 브랜치:** `docs/art-renewal`  
**최종 상태:** `VENOMSNAKE_ANIM8_CONTINUITY_V2_TARGETED / TWO_CANDIDATE_PNG_TECH_PASS / IDLE4_KEEP / USER_ANIM_VISUAL_APPROVAL_PENDING / NOT_UNITY_APPLIED`

## 근거·원본 보호
- [#97 최신 총괄 기획 8FPS 실물 검수](https://github.com/HappyPaul812/Project-Limitless/issues/97#issuecomment-6761585133) 및 [이전 제작·검증 보고](https://github.com/HappyPaul812/Project-Limitless/issues/97#issuecomment-6761154879), `COMMON_RULES.md`, `WORLD_ART_RENEWAL_LATEST.md` 확인.
- 바울 대표 **최종 미술 승인 v3 정지 원화** `VenomSnake_KeyArt_v3.png` SHA256 **`7001d199b279cb8e541fc603bd62351ab460d3b44da91bc91a2d65dc099f8fd0`** 원본 바이트 보존. 이번 신규 아트는 **지난 2026-11-10 제작 Anim8 파일럿의 표적 보완 후보**, v3 정지 원화 자체를 수정한 것이 아니다.
- 기존 **Idle00~03 4장, Attack00·Attack02 2장, 합계 6장 PNG 정확한 byte-for-byte KEEP**. 기존 8PNG 전체·원본 DELVERY_ONLY ZIP `07f5922d89078b869b95abcb165a3f66355cc49a9ea63667f631c438bad74256`도 별도 보호. **원본 PNG 덮어쓰기 0, 전체 재작업 0, 신규 후보 정확히 2장**.

## 1. 실제 표적 수정과 SHA

| 슬롯 | 신규 파일 (512×512 RGBA) | SHA256 | 수정 내용 |
|---|---|---|---|
| **Attack01** | `Candidates_ChangedOnly/VenomSnake_Attack01_Continuity_v2_512_RGBA.png` | **`7caeb3e9f622dc2e5567233ef5c52b7303b6f85dbe8621cb2ff68dc97984db2c`** | Attack00과 Attack02 사이에 급히 낮고 넓어지는 목·몸통 비율을 완화. 좌우 비율0.95, 세로1.12 및 소폭 중심 이동. 118,770 RGBA 픽셀값이 기존 후보와 달라짐 |
| **Attack03** | `Candidates_ChangedOnly/VenomSnake_Attack03_Continuity_v2_512_RGBA.png` | **`4e93ef15bd26b8692f56f93aa97dd01434f3c995156704575c45f28e9da6ad0a`** | Attack02 타격에서 한 프레임 만에 크게 치솟는 목 길이·확대 코일 변화를 완화. 좌우1.04/세로0.88 및 소폭 왼쪽 정렬. 129,412 RGBA 픽셀값 변경 |

- **보정 기법은 정직하게 구분:** 지정된 **두 프레임만** premultiplied-alpha 기반의 비등방성 기하 좌표 리샘플링을 적용한 **움직임 연결 후보**다. 새 얼굴·독니·다리/목을 수작업 해부학적으로 전면 재작화했다는 주장은 하지 않는다. 다른 프레임은 픽셀·PNG 바이트 모두 그대로.
- 후보 PNG는 각각 **512×512 RGBA**, 네 변 alpha>8 접촉 픽셀 **0**, 배경 투명. 새 PNG2와 전/후 SHA·달라진 픽셀 수, 실루엣 좌표 증거는 `Evidence/QA_Results.json` 및 `BeforeAfter_SHA.csv`에 기록.
- 원본 전체 프레임의 checksum은 ZIP/QA의 `original_sha256`에, 새 후보는 `candidate_sha256`에 기록.

## 2. Idle4 및 Attack4 8FPS 연속성 검토

| 구간 | 기존 IoU | 보완 후 IoU | 판정 |
|---|---:|---:|---|
| Idle00→01→02→03→00 | 기존 0.825~0.875 | 바이트 KEEP / 동일 | 수정 불필요: 미술 승인 전까지 KEEP |
| Attack00→01 | 0.68984 | **0.74934** | 실루엣 연결 개선, 자연스러운 모션 최종 판단 대기 |
| Attack01→02 | 0.78288 | **0.79576** | 연결 개선 |
| Attack02→03 | 0.66340 | **0.72689** | 급변 완화 |
| Attack03→Idle00 | 0.83199 | **0.82008** | 소폭 감소, 별도 Idle 복귀 미술 판단 필요 |

**IoU는 alpha 실루엣 중첩 지표**이며 머리·턱·목의 뼈대 연결, 독니 접촉, 하중/발 접지 또는 Unity Runtime 타격을 검증한 것이 아니다. 큰 모델의 별도 원화들이 가진 텍스처 디테일 차이는 이 보정만으로 해결되지 않음. **`MOTION_ART_REVIEW_PENDING` 유지**.

### 8FPS·시트/게임 셀 계약
- 원본 8PNG와 보정 두 장을 합쳐 만든 **OFFLINE 검수용** 전체8 시트 `Sheets_ReviewOnly/VenomSnake_Anim8_4x2_512cell_AFTER_OFFLINE.png` = **2048×1024 (512 셀×4열×2행)**. Idle/Attack 별도 마스터 4×1·128/40/32 셀 시트도 포함.
- **Idle/Attack Before·After GIF**: 각 4프레임 120·130·120·130ms로 **500ms / 평균 8FPS**, 128px·40px·32px 모두 실제 프레임·duration 디코딩 PASS. GIF는 10ms 단위라 개별 125ms는 정확히 지정 불가. 128/40 전후 보드·32px 가독성 비교·512 전후판 포함.
- 기존 제품 `04_VenomSnake.asset`: 32×32 cell/4열/각4프레임/8FPS, **`sourceFacesRight=0`**, 신규 원화 우향. 512 마스터 시트나 128/40/32 축소 견본은 **현 제품 PPU/Pivot/시트·visualSize(0.9,0.75) Unity 실제 설정을 통과한 교체 자산이 아님**. 격리 Unity6000.6.5f1 실제 Field/Battle/Flip/Hit/Idle 복귀 QA 후 본편 적용 여부 별도 결정.

## 3. 실제 납품/무결성

- **전체 검수·시트 ZIP:** `LIMITLESS_VENOMSNAKE_ISSUE97_MOTION_CONTINUITY_TARGETED_v2_20261110.zip`, **11,322,672 bytes / SHA256 `a3ac47cc6cb9c63a6429009efb781bc3728cfd767fa059a46e9960846a41f393` / 47 entries / ZIP CRC PASS / Manifest 45/45 SHA+bytes PASS / SHA256SUMS 46/46 PASS**.
- **가벼운 두 PNG 미술 검토 ZIP:** `LIMITLESS_VENOMSNAKE_ISSUE97_2POSE_REVIEW_ONLY_v2_20261110.zip`, **908,035 bytes / SHA256 `9a6b0315fd66eb8a66992085c4a7bbcfb7558759e00dab9a3de54050efe3cc73` / 16 entries / CRC PASS / Manifest 14/14 PASS**.
- ChatGPT **라이브러리 `/LIMITLESS_Art_Delivery/VenomSnake_Issue97_Continuity_v2_20261110/`**에 2 ZIP, 후보 PNG2, 128/40 기존/보정 GIF 각2, 전후 비교판2, 32px 가독성, 4×2시트 **총12개 항목 업로드 성공**. 사용자 Windows `F:\Downloads` 자동 복사·GitHub PNG/ZIP 직접 Push 없음.

## 4. 보호와 다음 단계
- 승인 v3 정지 원화, 기존 Anim8 8PNG, Idle4, Attack00/02, 기존 Ch2 공식 몬스터·Boss·WolfBearFox/OrganicSpider/VenomBee·승인 환경, #95 NPC 두 후보, #96 HP/MP 진행 전 상태, Unity 6000.6.5f1 Scene/Prefab/GUID/meta·MonsterDefinition·AI/독/Drop·Save/UserData 및 `main` **변경0**.
- 이번 회차 **Codex 추가검증0**, Unity 게임 적용·확정 애니메이션 양산·main 직접 commit/Push/merge 0.
- 바울 대표에게 **128px와 40px Attack 전후 GIF의 몸통/목 연결 및 32px 실루엣**을 확인받아 신규 모션 **미술 승인 여부** 결정. 승인을 얻어도 실게임 importer/방향/PPU/Idle복귀 QA는 별도 단계.
- 현재 정확 상태: `VENOMSNAKE_ANIM8_CONTINUITY_V2_TARGETED / SOURCE_PNG6_BYTE_KEEP / TWO_MODIFIED_PNG_TECH_PASS / USER_ANIM_VISUAL_APPROVAL_PENDING / RUNTIME_NOT_VERIFIED / NOT_UNITY_APPLIED`.
