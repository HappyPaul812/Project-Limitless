# Owl Attack4 Continuity + Spider Walk8 Fix — 읽기 전용 통합 검수 / 2026-11-03

**LOCAL_ART_REVIEW_DONE / USER_VISUAL_REVIEW_PENDING / NOT_UNITY_APPLIED / REMOTE_NOT_PUSHED**

바울 대표 제공 ZIP2개를 직접 열어 이전 납품과 대조했습니다. **Owl: DESIGN_CONTINUITY_REVISE**, **Spider: WALK_CONTACT_REVISE / MOTION_REVISE_CONTINUE**입니다. 파일무결성 PASS와 모션·최종미술승인 상태는 별개입니다. 실제 개선과 남은 결함을 각각 기록하며 원본 PNG 수정/게임 적용은 하지 않았습니다.

## 최신 기준·보호 범위

GitHub 최신 공통규칙, CODEX_LATEST(main의 과거기록과 로컬 최신 구별), docs/art-new SPRITE_ART_LATEST·#74 fa080e64/7943b2fe 납품, docs/art-renewal WORLD_ART_RENEWAL_LATEST·#60 ae465f35/f95e88ce 납품, #74/#60/#82/#83 최신 댓글을 확인했습니다. 다운로드 재현용 Python은 실행하지 않았습니다. ZIP 실물이 있으므로 재생성할 필요가 없습니다.

- branch **`codex/owl-spider-followup-review-20261103`**, 출발 **`ed95f352c125fc407436f1572867e058998d23ed`**(이전 통합검수 최종).
- worktree `F:/study/codex/Project-Limitless/Temp/OwlSpiderFollowupReview20261103/Worktree`.
- 기존 승인원화·Idle은 SHA 보존만 확인, 정상 Idle8의 품질 전수 재검사0. **Bee 관련 재검수0**.
- Unity 실행/원본 Scene/Prefab/BuildSettings/Packages/Save/UserData 변경0, 개인 Save 열람·해시·복사0, Discord66/포커스·main commit/Push/merge0.
- Main24 #80 81PASS, #83 90/23PASS, 저장85/72/72, 그루터기21, Main23 기존QA **재실행0**.

## 1. 실제 ZIP 공통 검사

| ZIP (F:/Downloads/) | bytes / files | SHA256 | CRC·경로·중복 | 동봉 Manifest / SUMS |
|---|---:|---|---|---|
| `Limitless_FrostOwl_Attack4_Continuity_20261103.zip` | 4,623,041 / 22 | `dd50da39530380047c8d4a6dd728d624ff828fc2dde089abc708be0558bc6856` | PASS / unsafe0 / 중복0 | 등록파일 18/18, SUMS 21/21 PASS |
| `LIMITLESS_WALK8_FIX.zip` | 4,687,734 / 18 | `d840e5ddd8ac2666e4b0c7a9f4b2d0bdfc688b5ff03e01a0019ba7808d6b86b8` | PASS / unsafe0 / 중복0 | 등록파일 16/16, SUMS 17/17 PASS |

두 ZIP 모두 실제 최신 납품 SHA와 일치합니다. 절대경로/상위이동/드라이브/콜론/symlink/대소문자 정규화 중복 검사 후 전용 Temp에만 추출했습니다. 비압축 용량 Owl4,651,610bytes / Spider4,722,144bytes. Manifest/SUMS 모두 동봉, 누락을 PASS로 뭉뚱그린 항목은 없습니다. Owl Manifest는18등록파일·기존Idle8의 SHA참조이며 전체21개 파일 검증은 SUMS로 확인했습니다. Spider는 독립Manifest16/16·SUMS17/17입니다.

총 PNG27 중 **새 SHA24개만 decode**, 동일 SHA3개(Owl03·Spider00/04)는 과거QA 재사용. GIF4개 모두 decode. 코어PNG512×512 RGBA, Owl2048×512/Spider2048×1024 시트 규격 확인. 비교보드4개는 RGB 자료, GIF는 배경합성 프리뷰로 구분했습니다. Owl 신3프레임 alpha 네 변 접촉0. **Spider의 새 캔버스 경계 접촉은 아래 별도 결함**이므로 전체알파 품질 PASS로 표현하지 않습니다.

## 2. Owl — 부분 개선 확인 / DESIGN_CONTINUITY_REVISE

### 실제 변화

- 이전 Attack4와 비교한4장 모두 다른 파일입니다. 단, 03은 새 그림 생성이 아니라 **승인 v3/Idle00 원본 byte exact 재사용**입니다. 새 원화3개 + 승인원화복귀1로 구별합니다.
- 얼굴/눈·머리깃 중심이 이전보다 승인v3를 알아보기 쉽게 복원됐습니다. 기존에 컸던 Attack01의 독립 냉기 궤적·입자는 본체에서 빠지고 `VFX_UNAPPROVED_HOLD/` 별도 PNG로 분리됐습니다. 승인 원화에 원래 있던 작은 냉기 디테일까지 제거 대상으로 취급하지 않습니다.
- 준비00 날개모음→발동01 펼침→02 하강→03 복귀 대비는 유지됐으며 40px에서 날개 상태가 구분됩니다. 4프레임 시트의 RGBA는 개별PNG와4/4 완전 일치합니다. 새 매트/상하좌우 잘림은 관찰되지 않았습니다.

| 파일 (Candidate/Attack/) | 이번 SHA256 | 이전 대비 RGBA 변경 |
|---|---|---:|
| `FrostOwl_v3_Attack_00_512_RGBA_CANDIDATE.png` | `b6029c30bf76e8b3f32843184f812c7ca6254f07b79214eca4069cd42a8fe547` | 127,837px |
| `FrostOwl_v3_Attack_01_512_RGBA_CANDIDATE.png` | `a98aab1c6a14caae1ed255882e90ec7fc3f1bdfa28693409d307a1cc1fed7142` | 134,605px |
| `FrostOwl_v3_Attack_02_512_RGBA_CANDIDATE.png` | `a7273d0d3c0b20d2a005b90f158906aa9760ef551dbba11aa3e80c5e692b9851` | 101,254px |
| `FrostOwl_v3_Attack_03_512_RGBA_CANDIDATE.png` | `81a31cf37cb834799ab5c9428da612ab13a5a496025805685daefed7b5d3c467` | 119,068px |

### 경계 동일성과 모션 연속성은 구별

**Attack03 SHA `81a31cf37cb834799ab5c9428da612ab13a5a496025805685daefed7b5d3c467` = 승인정지/Idle00**, 파일바이트와RGBA 완전동일입니다. 따라서 **03→Idle00의 이미지 경계 점프0**은 확인됐습니다. 기존 Idle8의 SHA8개도 보호참조와 전부 일치하며 이번 ZIP에 실제Idle8은 미동봉입니다.

하지만 **02→03**에서 머리/몸 비례, 몸통·꼬리·발 위치와 깃털의 선/질감이 바뀌는 느낌은 여전히 남습니다. 00~02의 얼굴을 알아볼 수 있게 복원한 개선과, 전체 날개·몸통이 승인03으로 자연스럽게 이어지는 완성도는 같지 않습니다. 기존 대비 개선은 인정하되 **ATTACK4_CONTINUITY_OK_CANDIDATE로 올리지 않고 DESIGN_CONTINUITY_REVISE**로 둡니다. 최소 보완은 00~02의 승인얼굴/몸통 비례·깃 질감/anchor를 일관되게 맞추고 02→03의 급전환을 정리하는 것입니다. 정상Idle 재제작은 불필요합니다.

본체 Attack4 GIF는 **4프레임120/130/120/130ms, 주기500ms=평균8FPS**, PNG00→03 순서일치·무한루프. 별도 `Attack03_to_ApprovedIdle00_SEAM_8FPS.gif`는 동일이미지 병합 결과 **실제1프레임/250ms 정지 프리뷰**입니다. 이를8FPS 전환 애니메이션이나02→03 자연스러움의 근거로 쓰지 않습니다.

분리 VFX SHA `46215c1cf7fd3fa3f94af92f4dd457436cf70711ca95b1bed86dc527f49d761c`는 **UNAPPROVED_HOLD**입니다. 본체와 분리된 점은 개선 PASS, 별도 효과의 미술/SkillVFX/게임 사용승인은 아닙니다. Hit/Defeat/실제한기VFX/Unity전투 검수0.

## 3. Spider — 실제6프레임 수정 / WALK_CONTACT_REVISE

### 변경 범위·보존

**Walk00·04는 이전 byte exact KEEP**, 01/02/03/05/06/07만 실제 변경됐습니다. 독립 계산 변화량은 동봉CSV와 일치합니다.

| Walk 프레임 | 이번 SHA256 | 이전 대비 RGBA 변화 | 결정 |
|---|---|---:|---|
| 00 | `cc1b5670c27bda75ebe5e8dc720cfa893b97016f927e926caa3aa5c7aaa291e6` | 0px | KEEP |
| 01 | `4dbd78009d3d6f6ee6a1ae1c0811f7d8684568cc1191c82a538602081d8c0bf8` | 67,113px | 수정 / REVISE |
| 02 | `168369181f890acaebef2c9e898df105ae29133953b03dae1c0cb6b9b997748b` | 72,909px | 수정 / REVISE |
| 03 | `64a7297d2e43cdde8376e2d99efef97b2468660e2d9d4c9fca2941e0469d83e9` | 63,603px | 수정 / REVISE |
| 04 | `7de4a98be27b2f7f2021b83742e788f0c6f5e7e21474d9ec6fccdac0083df7e9` | 0px | KEEP |
| 05 | `435ed6e6451a60f42670010b64cf78099ce1170fb925a11cb3e22bcc967b3785` | 71,876px | 수정 / REVISE |
| 06 | `9ed5850db957e476888d04482f729af163bf06438622ae8be2d1cecfac942223` | 80,129px | 수정 / REVISE |
| 07 | `9155976e57b82bddbae21d218f8a42a5d931bd7502b89a812b265b6dccd09c20` | 68,958px | 수정 / REVISE |

승인 v5/기존Idle00의 **`c600cd2264f6a0323771288666d16b49aa213ad938036d0746ed6f7f1d50adb9`**와 기존Idle8 SHA8개는 보존됐습니다. 이번Fix ZIP에는 원화/Idle 실물 대신 승인원화SHA 참조만 있습니다. 기존실물과 이전검수기록을 대조했으며 새Idle 재작화/전수품질QA0. Idle8 KEEP_CANDIDATE·Ch4메카닉 ARCHIVE_ONLY 유지, Attack/Shoot 검수·양산0.

### 보행 읽힘과 새 경계 결함

수정6장에서 앞다리·뒤다리 발끝의 들기/옮기기 차이는 늘었습니다. 다만 여전히 국소픽셀 리매핑 중심이고 **독립8다리의 지지/스윙·착지와 무게이동이128/40에서 충분히 명확하지 않습니다**. 40px에서는 작은 발끝 변화가 몸 흔들림과 섞입니다. 동봉PhasePlan의 색/원은 **제작자가 표시한 단계계획 주석**이며 실제 지면고정/접촉/하중측정이 아닙니다. 실제 지면·이동속도/발anchor 계약이 없는 OFFLINE 이미지로 미끄러짐 해결을 PASS하지 않습니다.

특히 이전에는alpha0이던 캔버스 끝에 새 발 픽셀이 닿습니다. 전체512코어PNG를 직접 검사한 결과:

| 수정프레임 | 변 | alpha>0 행(끝포함) | 최대alpha | alpha>8 접촉픽셀 |
|---|---|---|---:|---:|
| 01 | 오른쪽x511 | y277~302 | 255 | 25 |
| 02 | 오른쪽x511 | y262~288·294~296 | 255 | 25 |
| 06 | 왼쪽x0 | y328~385 | 255 | 56 |
| 07 | 왼쪽x0 | y345~351·353~371 | 41 | 15 |

**01/02/06은 불투명 다리 윤곽이 캔버스에서 절단되고07도 반투명 가장자리접촉**이 남습니다. 이전같은프레임 네변최대alpha는0이었습니다. 단순투명RGB 차이와 다른 실제가시/알파 결함입니다. 원화 전체축소/정상Idle변경으로 우회하지 말고 해당 수정프레임의 발 변형범위·외곽 여백을 보완해야 합니다. 독립 ROI 비교판에 빨간캔버스 경계와4배 참고확대로 증거를 남겼습니다.

시트의00/04는 RGBA exact, 수정6셀의 차이는 각407/414/437/385/424/339px로 **모두alpha0 RGB만**, 알파/가시변화0입니다. 이 무해한 저장표현 차이를 위 실제가장자리잘림과 혼동하지 않습니다.

납품GIF2개(128/40)는 **각8프레임×100ms=정확10FPS, 00→07 순서일치·loop0**. 이번에는8FPS 비교GIF가 없습니다. 기존게임10FPS 계약은 바꾸지 않았습니다. 정확한시간과실수정은 인정하되, 새잘림과접지표현보완 전 **WALK8_FIX_OK_CANDIDATE 아님 / WALK_CONTACT_REVISE / MOTION_REVISE_CONTINUE**입니다.

## 4. 단일 OFFLINE 비교판과 근거

**`F:/study/codex/Project-Limitless/Temp/OwlSpiderFollowupReview20261103/Review/index.html`**

HTML SHA256 **`ca98de10c55e0ef0b4601fe44703780ec78ffef8c6dd4a73908c1e9763d02d74`**.

이전Owl4/이번4/02→03→Idle00 진단순서/승인v3, Spider승인원화/이전8/수정8, 128px·40px 실크기/4배참고, 납품GIF, 발단계계획과독립경계ROI, 모든파일명/전체SHA·KEEP/REVISE/HOLD를 한 파일에 내장했습니다. **OFFLINE 비교판 / Unity GameView 아님**, 지면배경은 실제Field03PNG의OFFLINE참고이며 Scene재현이아닙니다. 분리VFX는HOLD영역에만전시합니다.

별도프로필headless브라우저에서 이미지decode·7시퀀스/앞뒤/슬라이더·시간재생/정지·128/40폭·배경/가로폭 **34 PASS / FAIL0**. 개인브라우저/포커스변경0. 납품본을재작성하지않고실제GIF시간으로재생합니다.

근거: [Audit/Manifest/SHA/alpha](Evidence/OwlSpiderFollowupReview20261103/Audit.json), [픽셀델타/원형/시트](Evidence/OwlSpiderFollowupReview20261103/DeltaAnalysis.json), [GIF실제순서/시간](Evidence/OwlSpiderFollowupReview20261103/GIFOrder.json), [새경계접촉](Evidence/OwlSpiderFollowupReview20261103/SpiderEdgeContacts.json), [브라우저34](Evidence/OwlSpiderFollowupReview20261103/BrowserQA.json), [보호28참조](Evidence/OwlSpiderFollowupReview20261103/ProtectionAfter.json).

## 5. 대표 최종 미술 판단과 다음 최소 작업

- **Owl:** 얼굴복원·VFX분리·03=Idle00은확인됐습니다. 나머지몸통/깃질감·비례와02→03연속성 보완후대표최종Attack미술판정. 별도냉기VFX는계속미승인HOLD. 정상Idle재작화/HitDefeat확대금지.
- **Spider:** 6수정은실제이나 먼저01/02/06/07 가장자리발절단을고치고, 수정범위안에서지지발·들기/이동/착지교대를128/40에읽히게보완. 대표Walk미술승인/향후실제지면Delta확인전접지PASS·AttackShoot확대금지.
- 본검수는최종미술승인/Unity기술QA/본편적용과별개입니다. 이번에Unity를열지않았으므로PPU/Pivot/게임속도/접지Runtime은NOT_VERIFIED입니다.

원본main **ab47b931fec92976170f9f50aabde1822d78bb90**, 기본status344/상세-uall858 전후동일. 입력ZIP2/사용한승인·이전참조28SHA모두보존. 기존게임소스·설정/아트원본/Save/UserData보호, Bee/기존게임QA재검수0, main/Push/merge0. 문서와JSON검수기록만로컬커밋, ZIP/PNG/GIF/HTML실물은Temp보존·원격바이너리업로드0.

**LOCAL_ART_REVIEW_DONE / USER_VISUAL_REVIEW_PENDING / NOT_UNITY_APPLIED.**

## GitHub 보고 영수증

실제 검수 commit **`5fdc6201d9484967454e7aa53f7088460ac519c0`** 기준 [#74 통합결과](https://github.com/HappyPaul812/Project-Limitless/issues/74#issuecomment-6595705479) · [#60 Spider 새경계결함](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6595712350) · [#83 보호/연결](https://github.com/HappyPaul812/Project-Limitless/issues/83#issuecomment-6595713003) 실제 댓글 등록 성공. 원격에는 댓글만 게시·파일Push0. 후속은 보고 영수증만 로컬 기록합니다.
