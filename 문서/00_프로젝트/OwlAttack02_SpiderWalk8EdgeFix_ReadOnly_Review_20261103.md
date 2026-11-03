# Owl Attack02 Continuity + Spider Walk8 Edge Fix — 실제 읽기 전용 통합 검수 / 2026-11-03

**LOCAL_ART_REVIEW_DONE / USER_VISUAL_REVIEW_PENDING / NOT_UNITY_APPLIED / REMOTE_NOT_PUSHED**

바울 대표 제공 ZIP2개를 직접 검사했습니다. 이번 최소 수정 목표에 대해 **Owl ATTACK02_CONTINUITY_OK_CANDIDATE**, **Spider WALK8_EDGE_FIX_OK_CANDIDATE**입니다. Owl 한 프레임 전환과 Spider 외곽 잘림이 실제 개선됐다는 후보 판정이며, 전체모션 최종미술승인/Unity기술QA/실제접지/본편적용 완료가 아닙니다.

## 1. 최신 기준과 작업 경계

GitHub 공통규칙·main CODEX_LATEST(과거 스냅샷과 실제 로컬 최신 구별), docs/art-new SPRITE_ART_LATEST·#74 신규 4ccb55b9/92d696b2 납품, docs/art-renewal WORLD_ART_RENEWAL_LATEST·#60 edd38a33/bb5a20ea 납품과 #74/#60/#82/#83 최신 댓글을 확인했습니다. 다운로드 재현 스크립트는 실행하지 않았습니다. ZIP 실물이 있으므로 재생성0.

- 브랜치 `codex/owl-spider-edge-review-20261103`, 출발 이전검수 최종 **34f2ad8870ac91395271340e574ae911881b7f11**.
- worktree `F:/study/codex/Project-Limitless/Temp/OwlSpiderEdgeReview20261103/Worktree`.
- 원본Unity/Scene/Prefab/BuildSettings/Packages/아트원본 변경0, 개인Save/UserData 열람·해시·복사·수정0, Discord66/OS포커스/main commit/Push/merge0.
- Unity실행0, 기존Main24 #80 81PASS/#83 90·23PASS/저장85·72·72/그루터기21/Main23 QA재실행0. **Bee재검수0**, 정상Idle8 품질QA재실행0.

## 2. ZIP 공통 기술 검사

| 실물ZIP (F:/Downloads/) | bytes / files | ZIP SHA256 | CRC·경로·중복 | 동봉 Manifest / SUMS |
|---|---:|---|---|---|
| `Limitless_FrostOwl_Attack02_Continuity_20261103.zip` | 3,992,293 / 20 | `d6648b7451dc2184c2187c0b1c2f8e0f52fc800ac6975753d0f6d38f6fbf9192` | PASS / unsafe0 / 중복0 | 등록17/17, SUMS19/19 PASS |
| `LIMITLESS_WALK8_EDGE_FIX_20261103.zip` | 4,431,916 / 20 | `771bd1813608aa3e350c6124ab0d43293c6d2988b1833014da64484d5272c8da` | PASS / unsafe0 / 중복0 | 등록18/18, SUMS19/19 PASS |

양쪽모두최신납품ZIP SHA와일치합니다. 비압축용량 Owl4,027,931bytes / Spider4,474,924bytes. 절대경로/상위이동/드라이브·콜론/symlink/대소문자정규화중복 검사 후 전용Temp에만추출했습니다. Manifest/SUMS는모두동봉. OwlManifest17등록파일/전체SUMS19, SpiderManifest18등록파일/전체SUMS19로범위를구별했습니다.

PNG27중 **새SHA13개만 decode**, 이전동일14개는기존QA+이번SHA동일성으로재사용했습니다. GIF4개모든프레임 decode. 코어PNG512×512 RGBA/투명정상, 시트Owl2048×512·Spider2048×1024. 비교보드4개는RGB, GIF는배경합성프리뷰이며게임투명Sprite가아닙니다. 새본체프레임과Spider8장전체네변alpha>0 접촉0·새사각매트없음. 기존동일PNG의품질전수검사를반복하지않았습니다.

## 3. Owl Attack02 — ATTACK02_CONTINUITY_OK_CANDIDATE

### 변경/보존

이번원본프레임변경은 **Attack02 한장**입니다. Attack00/01/03은바이트동일. 03은여전히승인v3/Idle00 **81a31cf37cb834799ab5c9428da612ab13a5a496025805685daefed7b5d3c467**입니다.

| Attack | 이번SHA256 | 이전대비RGBA변화 |
|---|---|---:|
| 00 | `b6029c30bf76e8b3f32843184f812c7ca6254f07b79214eca4069cd42a8fe547` | 0px |
| 01 | `a98aab1c6a14caae1ed255882e90ec7fc3f1bdfa28693409d307a1cc1fed7142` | 0px |
| 02 | `6dcb1799e0970cd08ceadc111e7e2645f21eb973ae75f9b859c2ce356d07e936` | 126,301px |
| 03 | `81a31cf37cb834799ab5c9428da612ab13a5a496025805685daefed7b5d3c467` | 0px |

새02 SHA **6dcb1799e0970cd08ceadc111e7e2645f21eb973ae75f9b859c2ce356d07e936**, 이전a7273d0d… 대비126,301픽셀변경입니다. 작은픽셀수정으로과장하지않고 **파일단위1개수정/광범위한02내용변경**으로기록합니다. 승인03을바탕으로몸통·발·꼬리/얼굴을일치시키고바깥날개를중간각도로바꾼후보입니다. 독립비교에서 **y345~511전체RGBA가03과exact**임을확인했습니다.

### 실제 연결 개선

이전02의다른몸통·꼬리·발형태가승인03으로갑자기바뀌던문제가줄었습니다. 새02는승인03의질감/몸체를그대로읽을수있고,01→02→03에서날개각도차이로이행합니다. 03→Idle00 byte exact도유지됩니다. 40px에서는00날개모음/01펼침대비가유지되고새02→03의몸체변화가감소했습니다.

독립계산한alpha>8 실루엣IoU: **01→02 .437692→.676079**, **02→03 .441515→.865426**로제작보고와일치합니다. 이는급변감소의수치참고이며미술승인/자연스러운타격의자동PASS지표가아닙니다. 이전에보류했던 **02→03 연속성의최소보완은후보로수용가능**하다고판정합니다. 보호00/01을더바꾸는작업은이번에요구하지않습니다. 전체Attack타이밍·표현의최종미술수용은바울대표판단대기입니다.

본체4GIF는120/130/120/130ms·500ms주기=**평균8FPS**,00→01→02→03순서정확. 별도760×620주석GIF는실4프레임 **01→02→03→Idle00**,같은120/130교대이며마커·라벨때문에마지막두프레임도구분됩니다. 실제Sprite영역을잘라PNG와대조해순서를확인했습니다. 마커/텍스트는게임PNG에없습니다. 이전1프레임SEAM정지자료를이번움직이는GIF와혼동하지않습니다.

시트4셀은개별PNG와가시RGBA/alpha일치·새잘림0. 승인v3/Idle8원화보호(정상Idle품질재검수0). 이미분리한미승인VFX는이번ZIP미동봉, Attack01이이전과byte exact이므로재결합0입니다. 이전VFX SHA46215c1c…도보호확인. 새02에보이는작은냉기디테일은승인03원형에있던것이며새독립VFX로승격하지않습니다. Hit/Defeat/한기VFX/Unity전투적용0.

## 4. Spider — WALK8_EDGE_FIX_OK_CANDIDATE (외곽 수정 한정)

**문제01/02/06/07 네장만실변경**, 00/03/04/05전부byte exact입니다. 03/05추가변경사유는필요없습니다. 512캔버스유지·전신축소/캔버스확장0. 중앙몸통가시ROI(x128~399,y100~269)는모든프레임에서이전과동일합니다. 가시변경은오른쪽끝x412~511 또는왼쪽끝x0~119의다리영역에한정됐습니다(투명RGB정리의비가시변경과구별).

| Walk | 이번SHA256 | RGBA변경 | 상태 |
|---|---|---:|---|
| 00 | `cc1b5670c27bda75ebe5e8dc720cfa893b97016f927e926caa3aa5c7aaa291e6` | 0px | KEEP |
| 01 | `550ad6615e5315ee2d4680bf2c885a03abd1b422a5d4fce5074a1ead971f0d64` | 8,813px | EDGE_FIX_CANDIDATE |
| 02 | `c4935be601a362682711fec1824293b57f3215247daca4c62a85ae5b0cae3547` | 9,413px | EDGE_FIX_CANDIDATE |
| 03 | `64a7297d2e43cdde8376e2d99efef97b2468660e2d9d4c9fca2941e0469d83e9` | 0px | KEEP |
| 04 | `7de4a98be27b2f7f2021b83742e788f0c6f5e7e21474d9ec6fccdac0083df7e9` | 0px | KEEP |
| 05 | `435ed6e6451a60f42670010b64cf78099ce1170fb925a11cb3e22bcc967b3785` | 0px | KEEP |
| 06 | `29a0ae62f37f2044c8b3dcd206bc3456c2ee141484f79c5bfe224c35566edf49` | 10,604px | EDGE_FIX_CANDIDATE |
| 07 | `edf725d87aafafcb81d01f09b121bf1e20a71b4fbfd829dbd1b0b6a3abf1b189` | 10,446px | EDGE_FIX_CANDIDATE |

### 이전 잘림 재현영역 / 이번 결과

| 프레임/변 | 이전문제좌표 | 이전alpha>0접촉수 | 이번문제영역·전체변 |
|---|---|---:|---|
| 01 오른쪽x511 | y277~302 | 26 | 0 |
| 02 오른쪽x511 | y262~288·294~296 | 30 | 0 |
| 06 왼쪽x0 | y328~385 | 58 | 0 |
| 07 왼쪽x0 | y345~351·353~371 | 26 | 0 |

**모든8프레임의네변전체 alpha>0 접촉0**입니다. 이전보고의alpha>8수25/25/56/15와이번표의alpha>0수26/30/58/26은임계치차이이며값을섞지않습니다.

독립이전/이번동일ROI확대에서경계에서잘리던발끝이안쪽으로곡선을그리며가늘어지고털/다리재질도이어집니다. 가장자리한줄을지우는사각alpha삭제만이아니라발끝의형태/가시픽셀영역이바뀌었으며몸체전체가작아지지는않았습니다. 발끝의길이·휘어짐/마감에대한최종취향판단은대표미술검토대기지만, **지적했던캔버스잘림은해소되어EDGE_FIX_OK후보**로보고합니다.

기존승인v5원형 **c600cd2264f6a0323771288666d16b49aa213ad938036d0746ed6f7f1d50adb9**는ZIP Source_KEEP와byte exact,기존Idle00및Idle8 SHA도보존. Idle품질전수재검사0. 시트8셀은alpha/가시RGBA동일(완전투명RGB차이는별도데이터기록),게임Runtime분리프레임계약변경0.

128/40GIF는각8프레임100ms×8=**정확10FPS**,00→07·loop0확인. 8FPS자료는이번미동봉입니다. 경계수정은기존보행역할을새로설계한것이아니므로 **8다리독립지지·들기/이동/착지·하중/실제지면고정/미끄러짐전체가새로해결됐다고판정하지않습니다**. 128/40실루엣·루프는유지되며,발접지계획표는주석자료입니다. **EDGE_FIX_OK_CANDIDATE / WALK_CONTACT_ART_REVIEW_PENDING / ACTUAL_GROUND_LOCK_NOT_VERIFIED**로분리합니다. Attack/Shoot제작·검수0,Ch4메카닉변경0,본편적용0.

## 5. 단일 OFFLINE 비교판·근거

**`F:/study/codex/Project-Limitless/Temp/OwlSpiderEdgeReview20261103/Review/index.html`**

HTML SHA256 **`26aa98c601f8c19ba7916f9b5f1f627fc52483b5700dd3262bec4d9cf881565c`**.

이전Continuity4/이번Attack02보완4/01→02→03→Idle00 PNG재생·납품실제GIF,Spider승인원화/이전Walk8/이번EdgeFix8·01/02/06/07동일경계ROI확대·발단계계획표를포함합니다. 각128/40실크기/참고확대,이전/새파일전체SHA·KEEP/CANDIDATE/HOLD,PNG프레임앞뒤/슬라이더·재생/정지·배경을지원합니다. **OFFLINE 비교판 / Unity GameView 아님**. 지면은Field03PNG의OFFLINE참고로Scene재현이아닙니다.

별도headless프로필브라우저 **34 PASS / FAIL0**(이미지decode·7시퀀스프레임수/제어·시간진행·배경/가로폭). 개인브라우저/OS포커스변경0. 원본PNG/GIF재작성0,모든이미지/GIF데이터내장·네트워크의존0.

근거: [ZIP/개별전체SHA·Manifest](Evidence/OwlSpiderEdgeReview20261103/Audit.json), [픽셀/범위/원형·시트/IoU](Evidence/OwlSpiderEdgeReview20261103/DeltaAnalysis.json), [GIF실순서/시간](Evidence/OwlSpiderEdgeReview20261103/GIFOrder.json), [모든경계alpha접촉](Evidence/OwlSpiderEdgeReview20261103/EdgeContacts.json), [브라우저34](Evidence/OwlSpiderEdgeReview20261103/BrowserQA.json), [원본보호](Evidence/OwlSpiderEdgeReview20261103/ProtectionAfter.json).

## 6. 최종 미술 승인 필요 항목·다음 최소 단계

- **Owl:** 이번Attack02연속성후보의01→02→03→Idle00재생을대표가최종시각승인할지판단. 이번개선후보를전체전투모션/Unity승인으로자동확장하지않습니다. 승인후라면02/연결구간8FPS만별도최소UnityDelta가다음단계입니다.
- **Spider:** 경계수정수용과전체Walk미술·접지를별개로판단. 현재4수정PNG의외곽후보는수용가능하며,반복전체재작화보다대표모션검토/향후실제지면·속도/발접지Delta가먼저입니다. 기존Idle원형재제작/AttackShoot확대는승인전보류.
- **실행하지않은것:** Unity재생/PPU·Pivot·Battle표시·실제발접지/원본게임적용. 모두NOT_VERIFIED/NOT_APPLIED이며기존QA결과를재실행하지않았습니다.

원본main **ab47b931fec92976170f9f50aabde1822d78bb90**,기본status344/상세-uall858전후동일. 입력ZIP2·사용한이전/승인·분리VFX참조28SHA보존,새Source_KEEPv5 SHA도기승인과동일. 문서4+JSON7만로컬커밋,HTML/PNG/GIF/ZIP은Temp보존·GitHub바이너리/파일Push0.

**LOCAL_ART_REVIEW_DONE / USER_VISUAL_REVIEW_PENDING / NOT_UNITY_APPLIED.**
