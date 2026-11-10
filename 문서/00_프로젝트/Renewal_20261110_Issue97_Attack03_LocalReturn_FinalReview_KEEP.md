# LIMITLESS / Chapter1 Renewal Art / Issue #97 — VenomSnake Attack03 복귀 연결 최종 표적 검토

**날짜:** 2026-11-10 · **담당 브랜치:** `docs/art-renewal`  
**권고 상태:** `ATTACK01_V2_KEEP / ATTACK03_V2_KEEP_RECOMMENDED / LOCAL_ATTACK03_REVIEW_CANDIDATE_ONLY / USER_FINAL_ANIM_APPROVAL_PENDING / NOT_UNITY_APPLIED`

## 1. 기획 지시 및 실물 정합
- [Issue #97 최신 총괄 검수 지시](https://github.com/HappyPaul812/Project-Limitless/issues/97#issuecomment-6762248009)와 앞선 [리뉴얼 v2 납품](https://github.com/HappyPaul812/Project-Limitless/issues/97#issuecomment-6761924550), `COMMON_RULES.md`, `WORLD_ART_RENEWAL_LATEST.md` 확인.
- 사용자 제공 Windows 경로 `F:\Downloads\VenomSnake_Continuity_v2_2POSE_SMALL.zip`의 PC 원본을 직접 읽은 것이 **아니며**, 앞서 납품한 ChatGPT 라이브러리 실물/작업공간의 동일 기준 Attack03 v2 PNG를 확인. v2 Attack03 SHA256 `4e93ef15bd26b8692f56f93aa97dd01434f3c995156704575c45f28e9da6ad0a` 일치.
- **USER ART APPROVED 원본 v3**: `VenomSnake_KeyArt_v3.png` SHA256 `7001d199b279cb8e541fc603bd62351ab460d3b44da91bc91a2d65dc099f8fd0` KEEP. Idle00~03, Attack00/02, **Attack01v2** 원본 보호; **총 7프레임 수정 0**. 기존 Attack03 v2도 원본 바이트 불변. Unity 6000.6.5f1 및 게임 내 참조 변경 0.

## 2. 수정 후보 정확히 1장 및 미술 KEEP 판정

- 후보: `Candidate/VenomSnake_Attack03_HeadNeck_LOCAL_REVIEW_v3_512_RGBA.png` **512×512 RGBA**, SHA256 **`a21c6a01b5a5facdec35509cd79b12fff5b6b04d686482f45745ab6a7aeab9d9`**.
- **원본 Attack03 v2** SHA256 `4e93ef15bd26b8692f56f93aa97dd01434f3c995156704575c45f28e9da6ad0a`, 후보 vs v2 **53,820 RGBA 픽셀값 변경**, 변경 bbox `x188..471 / y66..322`.
- **사용한 방식:** v2 Attack03의 **머리·턱·목 상단 x>185, y<325에만** 부드러운 국소 변위 적용(최대 **오른쪽 +5px / 아래 +8px**), 목에서 점차 감쇠. **y≥325의 하부 똬리 전체·x≤185 좌측 몸통은 픽셀 완전 동일**. 승인된 v3 원화·정상 프레임은 재인코딩/재작화하지 않음. 전체 Sprite 세로압축/리컬러/AI 생성 원화로 전면 대체 금지 지시를 준수.
- 대화 중 별도로 생성된 큰 독사 그림/장식 보고용 인포그래픽은 외형·스케일이 달라 **후보로 부적합하여 전부 납품에서 제외**. 이번 실물 후보는 **Attack03 v2만을 출처로 한 제한 영역 래스터 수정**으로, 새 해부학적 관절 재작화 완성이라고 주장하지 않는다.

### 정량 및 시각 검토

| 연결 | v2 alpha>16 IoU | 로컬 후보 IoU | 변화 |
|---|---:|---:|---:|
| Attack02→Attack03 | 0.726891 | 0.727948 | +0.001057 |
| Attack03→Idle00 | 0.820077 | 0.820179 | +0.000102 |

- **두 수치는 미세 상승에 그쳐 128/40/32px 실축소 재생에서 확실한 개선을 입증하기 어렵다.** 자연스러운 목 회전·턱/비늘 일관성은 비교 이미지와 최종 미술 검토가 필요.
- **정직한 권고: `ATTACK03_V2_KEEP_RECOMMENDED`**. 후보를 별도 납품은 하되 바울 대표가 시각적으로 더 낫다고 명시 승인하기 전에는 v2를 최종 유지. **추가 수정 무한 반복을 중단**. v2를 USER_ART_APPROVED로 임의 전환하지 않으며, 새 후보 또한 자동 최종 채택 아님.
- 32px에서는 독니·눈 디테일 소실과 목이 뭉치는 부분이 남는다. 32px 검사 결과는 **OFFLINE**이고 게임 내 PPU/visualSize(0.9,0.75)/실제 좌우 Flip 검증 전이다.

## 3. 실제 비교·8FPS 자료 및 ZIP 검증

- `Review/Attack02_Attack03v2_Local_Idle00_{512,128,40,32}px_OFFLINE.png`: 실제 소스 512×512 전체 캔버스를 동일 조건으로 다운샘플해 4열 비교. **비교판에 표시된 시안 4개 중 신규 후보는 Attack03 하나**. Attack02·Idle00·기존 Attack03는 읽기전용.
- `Review/Attack4_{BEFORE_v2,LOCAL_REVIEW}_{128,40,32}px_8FPS_OFFLINE.gif`: Attack00→01v2→02→03v2/로컬, 기존/수정 각각 4프레임, **120·130·120·130ms = 500ms, 평균 정확8FPS**.
- `Review/Attack01_02_03_Idle00_{BEFORE_v2,LOCAL_REVIEW}_{128,40,32}px_8FPS_OFFLINE.gif`: Attack01v2→Attack02→Attack03→Idle00 4프레임의 OFFLINE 연결 테스트. **실제 Unity 런타임 복귀/적중 테스트가 아니다**.
- **ZIP** `LIMITLESS_VENOMSNAKE_ISSUE97_ATTACK03_LOCAL_REVIEW_20261110.zip`: **1,563,927 bytes / SHA256 `5bfb44b15b4eb70b5b7285afe9673e3199c6371428dc7bc094442cf376238bda` / 23 entries / CRC PASS / Manifest 21/21 SHA256+bytes PASS**. `SHA256SUMS.txt`, `Evidence/QA_Results.json`, SourceDeclaration 및 README 포함.
- **배포:** ChatGPT 라이브러리 `/LIMITLESS_Art_Delivery/VenomSnake_Issue97_Attack03_FinalReview_20261110/`에 ZIP·후보 PNG·128/40/32 비교판·128/40 전후 GIF·Attack→Idle GIF·QA 11개 업로드. 사용자 Windows `F:\Downloads` 자동복사나 Github 바이너리 Push 없음.

## 4. 원본 보존·Unity 및 후속
- 보호: approved v3, 기존 8PNG 및 기존 v2 후보, 7개 정상 프레임 정확한 SHA 유지, MonsterDefinition `04_VenomSnake.asset`, 구형 32×32/4열/4fr/8FPS/`sourceFacesRight=0`, AI·독·Drop·전투 수치, Scene/Prefab/GUID/meta·Save/UserData·main 변경 0. `docs/art-renewal` 문서 기록만, main 직접 commit/Push/merge 없음.
- **바울 대표 최종 애니메이션 미술 승인**은 여전히 필요. 시각 승인 뒤 Codex Unity 6000.6.5f1 격리의 32px PPU/Pivot/Flip/Idle 전환·독/Hit 타이밍 QA. 승인이 게임 적용 승인은 아님.
- **다음 제작 큐**: Issue #96 HP·MP 오리지널 소형 아이콘 2종. 해당 범위 작업은 #97 검토 기록 후 별도 발주/진행.

**종료 판정:** `ATTACK01_V2_KEEP / ATTACK03_V2_KEEP_RECOMMENDED / 1_LOCAL_REVIEW_PNG_DELIVERED / ORIGINAL_8PNG_BYTE_KEEP / USER_ANIM_APPROVAL_PENDING / NOT_UNITY_APPLIED`.
