# Issue #102 NPC2·Issue #100 SnowStoat Defeat4 오프라인 실물 검수 — 2026-11-17

## 판정

| 대상 | 파일 기술 | 미술 판정 | 적용 |
|---|---|---|---|
| OGA11 주민2·OGA13 주민3 | **FILE_TECH_PASS** | **STYLE_REVIEW_PENDING / USER_ART_APPROVAL_PENDING** | UNITY_NOT_RUN / GAME_NOT_APPLIED |
| SnowStoat Defeat4 | **FILE_TECH_PASS** | **DEFEAT4_ART_REVIEW_PENDING / USER_ART_APPROVAL_PENDING** | UNITY_NOT_RUN / GAME_NOT_APPLIED |

기술 HOLD 발견0. 최종 미술 승인으로 승격하지 않습니다. [#102 최신 납품](https://github.com/HappyPaul812/Project-Limitless/issues/102#issuecomment-6931445900), [#100 최신 Defeat4 납품](https://github.com/HappyPaul812/Project-Limitless/issues/100#issuecomment-6931466184), Issue 본문과 CODEX_LATEST를 먼저 확인했습니다. Issue100 본문의 최초 1포즈 제한은 후속 Defeat4 발주·납품과 이번 사용자 오프라인 검수 지시로 범위를 구분합니다. 기존 KeyPose/Idle4/Attack4v2/Hit4 승인 상태를 변경하지 않습니다.

## 실물 ZIP 무결성

| 사용자 제공 파일 | 실제 바이트 / 파일 수 | 실제 SHA256 |
|---|---|---|
| F:/Downloads/Limitless_NPC_102.zip | 1436792 / 13 | `c6e2268f817df91447f462ad7b7f30de3710fcc2abb168556884f7136c285b95` |
| F:/Downloads/SnowStoat_Issue100_Defeat4_20261117.zip | 1454055 / 12 | `34f23d12ee4d4580e72f00ec0deb143b1d5b33aaa6d8d132c6495579961d6ad2` |

두 ZIP 모두 최신 납품 보고와 일치합니다. ZIP CRC PASS, 중복 entry/위험 경로0.

- NPC: Manifest payload **11/11 크기·SHA 일치**, SHA256SUMS **12/12 일치**(payload11+확정Manifest).
- Defeat4: Manifest payload **10/10 크기·SHA 일치**, SHA256SUMS **11/11 일치**(payload10+확정Manifest).
- 두 Manifest는 자신과 SUMS를 제외하고, SUMS는 자신을 제외하며 확정 Manifest를 포함합니다. payload 전체 포함·자기/순환참조0. 미수록 AI 생성원본 SHA는 출처 식별자이며 ZIP 내부 파일의 체크섬 의존관계와 구분합니다.

## A. NPC #102

| 대상 | ZIP 내부 경로 | PNG 실측SHA256 |
|---|---|---|
| OGA11 주민2 | Candidates_512/OGA11_Resident02_Original_512_RGBA.png | `5fde2e4cfbcdca2839f7da1071dbbbc4b7f239ee85ab7f7b9985be48a5cb91f9` |
| OGA13 주민3 | Candidates_512/OGA13_Resident03_Original_512_RGBA.png | `0b2b5a1a19965eeff4bbdccff0072d25c7881e6695bdc32b146542311be7f849` |

두 PNG 모두 실제 **512×512 RGBA**, alpha0~255·외곽 alpha접촉0. alpha>8 유효영역은 OGA11 [149,61,362,471), OGA13 [169,59,343,471), 유효높이410/412px·최하단 y470(0기준, 포함좌표)입니다. QA의 bbox 오른쪽/하단은 포함좌표로 읽으면 일치하며 이 보고는 끝좌표 제외 방식입니다.

납품 Preview128/40 4개 모두 PNG512의 **LANCZOS 직접 축소 RGBA 픽셀과 완전 일치**. 비교판512/128/40의10종 모두 실제 승인참조8+후보2의 불투명 전경픽셀 전체가 대응 위치와 정확히 일치했습니다(30개 배치). OGA번호·라벨·정본/프리뷰가 서로 맞으며, 무관 외부 캐릭터/검색이미지로 바뀐 표시를 발견하지 못했습니다. 전 세계 이미지 출처/AI 학습자료에 대한 독립 권리 감사까지 수행한 것은 아닙니다. SourceDeclaration은 AI 신규 생성·비트레이싱을 선언하며 고해상도 생성원본 자체는 ZIP에 없어 생성 이력을 독립 재현하지 못했습니다.

**실물 시각 의견:** OGA11은 모자·조끼·바지와 한쪽 바구니, OGA13은 숄·치마·짧은 머리 실루엣으로 구별됩니다. 128px에서 얼굴/의복/소품이 읽히며40px에서도 상·하의 형태로 구별 가능하지만 눈·손·바구니 속 물건의 세부는 어렵습니다. 정면에 가까운3/4 시점·발 접지·전체 높이는 OGA07/10과 크게 어긋나지 않습니다. 따뜻한 갈색/올리브 의복과 명암도 해당 두 승인NPC에 가깝습니다. 기존8종 자체에 평면적인 굵은 선(OGA03/06/09/16 등)과 세밀한 묘사(OGA07/10)가 섞여 있어 **전8종 완전 화풍 일치 PASS로 확정하지 않습니다**. 새 후보2의 기존 승인8 SHA 중복0, 원본 Resources의 기존 OGA11/13 저해상도 PNG를 덮어쓰지 않았습니다. 새 주민 설정·직업·스토리를 확정하지 않습니다.

## B. SnowStoat Defeat4

- 실제 게임용 `Sprite/SnowStoat_M2_Defeat4_2048x512_RGBA_CANDIDATE.png` **2048×512 RGBA**, 가로4×512셀, SHA256 **`8bc97ce0af5d4896d2259df39972ea018ea1a6e494883349cc3b3065926bfe4f`**. 외곽 alpha0, 독립4프레임은 실제 시트 각셀의 RGBA와 **4/4 완전 일치**, 4셀 콘텐츠 모두 상이.
- Manifest rendered_frame_bboxes는 alpha>0 측정으로 **4/4 일치**: [28,229,478,458), [27,269,481,459), [27,314,482,459), [27,336,476,459). alpha>8 몸체 높이는222→185→138→115px로 낮아집니다. 몸체 하단은 y454/455 부근, 극저알파까지 포함한 아래경계는457/458로 약1px 변화입니다. 이 정적 바닥 정렬이 Unity Pivot/실전 접지 검증을 대신하지 않습니다.
- **실물 흐름:** 00 긴장한 서있는 자세(균형 상실 표정은 약함)→01 앞다리 굽힘·몸 낮춤→02 몸통/머리를 더 내리며 엎드림→03 눈을 감고 주둥이를 바닥 가까이 둔 패배 자세 유지로 읽힙니다. 02/03에서 일어서거나 Idle로 회복하는 프레임0. 과장 회전/유혈0. 128px에서 자세변화가 읽히며40px에서는 서있음→낮아짐은 구별되지만02/03의 눈·세부 자세 차이는 제한적입니다.
- 기존 승인4종과 동일 셀/배율로 비교했습니다. 흰 긴 몸통·짧은 다리·귀/뾰족한 주둥이·회청색 음영·검은 꼬리끝을 유지하며 큰 캐릭터 정체성 이탈은 발견하지 못했습니다. 포즈 변경으로 머리/몸의 정확 픽셀비율이 고정된 것은 아니며 **최종 형태·표현 선호는 대표 미술 승인 대기**입니다.
- 납품128/40 스트립을 실제 PNG 각셀에서 LANCZOS 축소해 대조: **alpha와 보이는 RGBA 모두 일치**. alpha0 영역의 보이지 않는 RGB 채널값만128판527개·40판152개 차이가 있어 raw바이트 완전동일이라고 하지 않습니다. 이 차이는 합성 화면에 영향을 주지 않습니다. 40판 전체시트를 한번에 축소하면 경계 필터 차이가 있으므로 **각512셀을 개별 축소한 결과**로 대조했습니다.
- 납품512/128/40 검토판의 불투명 전경은 원본4셀과 대응크기/위치에서 전수 일치(512배치4+128밝고어두운8+40밝고어두운8). 원본에서 파생된 표시임을 픽셀로 확인했습니다.
- GIF 실제4프레임·512²·loop extension 없음, 지연 **120/120/120/1750ms**. 125ms는GIF10ms단위에서120ms로 기록되므로 정확8.000FPS라고 표현하지 않습니다. 원본셀을 배경RGB(227,234,242)에 합성하고256색MEDIANCUT 양자화한 화면과 **디코드4프레임 RGB 전부 정확 일치**. 마지막은03엎드린 자세이며 반복/일어남0. 마지막 정지 의미는 일반적인 비반복 GIF 재생 기준이며 Unity Animator 미검증입니다.

## 보호·검증 범위·기록

기존 승인NPC8 실물 SHA8/8, 설원족제비 승인KeyPose/Idle4/Attack4v2/Hit4 실물 SHA4/4를 재확인했습니다. 신규 시트가 기존 원화를 대체/덮어쓰지 않았습니다. 승인4종은 기존ZIP에서 읽기전용 취득해 Temp의 비교사본으로만 사용했습니다. Unity6000.6.5f1 고정·Issue85 HOLD 유지. Unity 실행/Import·Assets/Scene/Prefab/meta/GUID/Save/UserData/Discord 수정·삭제0. 새 그림/후보 PNG 수정0. 추출·검수이미지·JSON은 private Temp에만, 이미지/ZIP을 Git에 추가하지 않았습니다.

사용한 로컬 문서브랜치 `codex/art100-102-offline-review-20261117`, 기존 소형 문서worktree 재사용·대형QA 복제0. 보고서/CODEX_LATEST/CURRENT_STATUS 3문서만 로컬 commit, 원본main52f84e64 유지, Push/Merge/reset/clean/stash0. 최종 SHA는 관련Issue 실제댓글과 `Temp/Art100_102Offline20261117/Final.json`에 확정합니다. **LOCAL_COMMITTED / REMOTE_NOT_PUSHED**이며 GitHub에는 Issue댓글만 작성합니다.

검사 근거: private `Temp/Art100_102Offline20261117/audit.json`, `visual-verification.json`, 실제납품 추출본NPC/Defeat·승인참조ApprovedStoat·직접만든동배율비교판Approved4_vs_Defeat4_OFFLINE.png. 모두 오프라인이며 Unity 캡처가 아닙니다. 보호목록8342 종료대조/기존비보고git상태의 정확한 결과는 Final.json에 기록합니다.

다음은 바울 대표의 NPC2종·Defeat4 최종 미술 승인입니다. 이 검수로 Unity 적용을 자동 시작하지 않으며, 추가 구현/패키지/GUID/Issue85 기술 작업으로 범위를 넓히지 않습니다.


최종 보호 대조: 기존8342개 중 허용된 CURRENT_STATUS/CODEX_LATEST2개만 보고 갱신, 그 밖8340개 SHA변경0. 기존 비보고 git status byte-exact 동일·입력ZIP2 SHA불변. 원본main52f84e643d5d860665eadfe1614a30c745209f00 유지. 최종 commit SHA는 실제 Issue댓글/Final.json에 기록합니다.
