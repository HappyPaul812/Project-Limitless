> **현재 상태 갱신 / 2026-11-03:** 바울 대표가 자연형 그루터기v4 SHA `22f2f5816b3fd2ffe45234ec807bb3d81a83f87440eb8eb1f338e0dff33404a8`만 **USER_APPROVED**했습니다. 예전 그루터기v1/v2/v3·FULLREDRAW는 **SUPERSEDED / ARCHIVED_CANDIDATE**. 표지판·울타리·Spider·Bee 승인은 포함하지 않습니다. [승인 정본](F2_NaturalStump_v4_UserApproval_And_UnityDeltaPrep_20261103.md). 아래 본문은 당시 검수 이력이며 Unity미적용 상태는 그대로입니다.

# Issue #81 — Main24 F2 + Spider + Bee 개별3 ZIP 델타 검수 / 2026-11-03

**LOCAL_ART_DELTA_REVIEW_DONE / USER_VISUAL_REVIEW_PENDING / NOT_UNITY_APPLIED / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**

## 결론과 대표가 볼 자료

단일 비교판: `F:/study/codex/Project-Limitless/Temp/ArtMain24ThreeZip20261103/Review/index.html`

HTML은 이미지·프레임을 data URI로 포함하는 독립 파일입니다. 외부 네트워크/서버 없이 열 수 있습니다. **OFFLINE이며 실제 Unity/GameView가 아닙니다.**

HTML SHA256: `12ee5ace73b2daef55c0dd8c4a9ab8e59ca3e2982d25a00c7eb851feefdb7e43`.

|대상|미술 판정(검수 의견)|다음 최소 결정|
|---|---|---|
|F2 표지판 국소 v3|REVISE|청색은 줄었으나 목재 메움 얼룩/색면 경계 잔존|
|F2 그루터기 국소 v3|REVISE|작은 횃대의 인공 원판·굵은 테두리 잔존|
|F2 LowFence 국소 v3|REVISE|하단은 옅어졌으나 좌우 직선 눈판/알파 경계 잔존|
|F2 독립 전면 대안 표지판|KEEP_CANDIDATE|목재결·화살표 윤곽이 더 명확하여 우선 미술 선택 권고|
|F2 독립 전면 대안 울타리|KEEP_CANDIDATE|불규칙 눈 가장자리/접지가 더 자연스러워 우선 미술 선택 권고|
|F2 독립 전면 대안 그루터기|HOLD|본체는 자연스러워졌으나 작은 원판 형태는 남아 별도 시각 선택 필요|
|Organic Spider v3 알파|REVISE / ALPHA_FINAL_HOLD|RGB 보존·282 alpha 감소 확인. 앞다리 아래 회백색 띠가 남아 **ALPHA_FIX_OK_CANDIDATE 아님**|
|Bee 기존5 키포즈·GIF|REVISE / MOTION_REVISE_CONTINUE|이전과 동일7파일. 새 개선/완성20프레임 아님. 공격축·관절·회수 보완 필요|

KEEP_CANDIDATE는 Codex 검수 추천이며 바울 대표 최종 미술 승인이 아닙니다. 전면 대안3장은 `Reference_Alternative_Concept`이며 국소 수정본과 별개입니다. 모든 대상의 게임 적용은 **RUNTIME_HOLD**. 실제 Collider/휠체어 통행/PPU·Pivot/Scene·Battle 배율은 Unity 미검증입니다.

## 실제 ZIP 무결성

- `Limitless_F2_MinimalFollowup_20261103.zip`: **1,604,092 bytes / 14 entries**, SHA256 `310d1755b14ab67f744eea420dd1f8e6c41e397fb8689ea2f07b4d7981323a28`. CRC PASS, 중복·안전경로 PASS.
- `LIMITLESS_BEE_ONLY.zip`: **908,585 bytes / 9 entries**, SHA256 `b3b26bc8c9ff3e360b9a282dcd950ac64dbdbcb5572a05e7328023f571da163d`. CRC PASS, 중복·안전경로 PASS.
- `LIMITLESS_SPIDER_ONLY.zip`: **1,512,578 bytes / 6 entries**, SHA256 `c82c808704fa3f568e4aa042c42f24e18dca246284148de290e95f07d844f667`. CRC PASS, 중복·안전경로 PASS.

F2는 담당 #75 보고의 ZIP SHA와 정확히 일치합니다. SHA256SUMS **13/13 PASS**, Manifest 참조 **14/14 PASS(대안 중복 참조 포함, 고유11파일)**. Source v2 SHA3개와 보고된 v3·대안 SHA6개도 실제 바이트 일치합니다.

Bee/Spider 개별 ZIP에는 전체 `Manifest`·`SHA256SUMS`가 **미동봉**입니다(`NOT_SUPPLIED`). 임의로 Manifest PASS라고 보고하지 않습니다. Bee의 Existing20 JSON은 ZIP에 없는 기존20 PNG의 참고 SHA 목록이며 전체 ZIP Manifest가 아닙니다. 그 참고 SHA20개는 로컬 이전 납품20 PNG와 **20/20 일치**합니다.

11월3일 합본 `LIMITLESS_Spider_AlphaFinal_Bee_MotionPrep_20261103.zip`(담당 보고 SHA `56f194561895ad875de3d89d75385471ce6709780d4fb52ad4fa82e222739f48`)은 이번 로컬에 없어 **합본 전체와의 직접 대조는 NOT_VERIFIED**입니다. 개별ZIP 자체를 검사했으며 합본과 같다고 가정하지 않았습니다. Spider v3는 합본 보고의 특정 PNG SHA와 일치하고, Bee7파일은 실제 이전 10월29일 납품 바이트와 일치한다는 범위만 확정합니다.

새 PNG는 실제 decode/크기·RGBA·alpha 범위/bbox를 확인했습니다. F2 후보/대안6은 각각288×384·320×384·256×192 RGBA, Spider512²/128²/40² RGBA입니다. alpha 범위0~255로 투명 채널 정상이며, 이것은 미술 경계나 게임 적합성 PASS를 뜻하지 않습니다. RGB 비교판은 RGB 프리뷰로 분류하며 투명 Sprite로 오인하지 않습니다. 기존 SHA와 같은 Bee PNG5/Spider v2는 이전 QA를 재사용하고 표시를 위한 로드만 수행했습니다.

## F2 실제 변경과 시각 판정

- 표지판: v2 대비 RGB **1,512pixel**, alpha **0** 변경. bbox `[97,155,210,195)`. 청색 흔적은 감소했으나 넓은 메움 색면이 기존 목재결과 이어지지 않아 국소본 REVISE. 전면 대안은 질감·화살표 윤곽이 더 명확하므로 별도 KEEP 후보.
- 그루터기: RGBA union **1,795pixel**, RGB **1,667**, alpha **883**(그중 증가294) 변경. bbox `[192,157,272,204)`. 원판 윗면 나이테 개선은 있으나 작은 횃대의 원판·테두리가 계속 보여 국소본 REVISE. 대안은 전면 다른 디자인이며 원판 느낌도 남아 HOLD.
- LowFence: RGB **0**, alpha **13,146 감소**, 증가0. bbox `[10,104,245,175)`. 하단 불투명도 감소는 확인되나 좌우 직선 경계가 남아 국소본 REVISE. 전면 대안은 윤곽이 불규칙해 접지가 더 자연스럽지만 길폭/Collider 승인 아님.
- 원본 캔버스/투명 여백을 보존해 긴 변256/128/40px로 비교했습니다. 40px에서는 형태 식별은 가능하나 표지판 정보/세부와 횃대 윗면 질감은 크게 축소됩니다. 게임 목적지 텍스트나 필수 정보를 Sprite만으로 전달해서는 안 되며 실제 UI/배치는 이번에 구현하지 않았습니다.
- 기존 FallenSnowBranch/GentleFirstSnow와 P1 환경, 승인 Owl v3/메카닉Spider/기존 후보는 범위 한정 SHA만 보존 대조했습니다. Owl Attack HOLD 변경0. 전체 환경 재검수0.

## Organic Spider — 디자인 보존과 알파 미결

ZIP의 `ForestSpider_Organic_v2_REFERENCE_KEEP_512_RGBA.png`는 기존 채택 디자인 v2 SHA `c949f37ad5511753b064b83d521e79bb76409b0427cd0abf0c9742d6d9d46661`와 일치합니다.

v3는 담당 보고 SHA와 일치하며 **RGB 262,144/262,144 동일**, alpha282픽셀만 감소·증가0, 변경 bbox `[35,304,96,354)`입니다. RGB·원화 형태를 새로 그리지 않은 사실은 확인됐습니다.

하지만 검수 확대에서 앞다리 아래 분리된 회백색 밑띠/경계가 남아 **완전 제거로 판정하지 않습니다**. 512·128·40px에서 전체 거미 실루엣은 유지되지만, 40px에서는 털·다리 분리가 약합니다. 비교판에 밝은/어두운/실제 Field03 바닥 PNG 반복 배경을 전환하고, 앞다리 bbox 주변을 최근접4배 확대해 볼 수 있게 했습니다. Field03 바닥은 `LL_C1_F03_ForestFloor_A_128_v1.png` 실물이며, 실제 Scene/조명/Sorting 재현은 아닙니다.

권고는 **같은 디자인/RGB를 보존한 하단 띠 국소 마감만**입니다. 유기형 디자인 긍정 채택을 다시 승인받으라는 의미가 아닙니다. 새 Idle/Walk/Attack/Shoot32프레임 제작·게임 교체0. 메카닉 v3 Chapter4 ARCHIVE_ONLY는 수정하지 않았습니다.

## Bee — 전체20과 키포즈를 구분

- 이번 개별 ZIP: Idle PNG2(238×215), Attack PNG3(315×253), GIF2, 비교PNG1, Existing20 JSON1. **전체20 PNG 미동봉**.
- 키포즈 PNG5와 GIF2는 이전 `Temp/RenewalSpiderV3BeeRedrawn20261029/Extracted/Bee/`의 대응 파일과 **SHA7/7 동일**. 새 키포즈 개선본0, 재작화0. 기존 오프라인 Idle10/Attack10은 별도 `Temp/ArtIntegratedReview20261029/Extracted/Renewal/VenomBee/` 원본20파일에 있으며 JSON20/20 일치. 현재 본게임 PilotBee를 이것으로 교체한 것이 아닙니다.
- Idle은 접힘→펼침2포즈, Attack은 Prepare→Sting→Recovery3포즈입니다. 실제 GIF 각 프레임 **100ms=10FPS**, Idle총200ms/Attack총300ms, 무한 반복(loop0). GIF 실제 decode 순서와 PNG 대응을 합성 RGB 차이 및 프레임 시각으로 확인했습니다. GIF에는 **체커 배경이 이미지에 구워져 있으며 투명 Sprite가 아닙니다**. HTML에는 원본 GIF와 별도 PNG 순서 재생/정지/앞뒤이동을 모두 제공합니다.
- 날개 펼침/접힘은 보이나 뿌리 회전·중간 포즈가 없어 연속 날갯짓 완성 아님. 다리 위치·각도 차이는 있으나 관절의 연속적인 굽힘과 접지/몸통축 일관성을 입증하지 못합니다.
- 머리 우향·독침 좌향의 공격축 불일치가 남습니다. Sting에서 복부/침이 뻗고 Recovery에서 되돌아오지만3포즈 급전환으로 준비→타격→회수의 연결과 표적 방향이 자연스럽게 완성됐다고 판단하지 않습니다.
- 40px 원종횡비에서 복부/침·날개 실루엣 변화는 보이나 다리 관절/공격 방향 구분이 약합니다. 전체20 존재만으로 `FULL_COMBAT_READY`나 전투 교체 완료라고 판정하지 않습니다.
- PilotBee 웹 CC BY3.0 vs 동봉 CC BY4.0 충돌은 **RIGHTS_VERSION_CONFLICT_OPEN** 별도 유지. 라이선스나 제품 Sprite/GUID를 수정하지 않았습니다.

## 정확한 후보 파일 SHA256

- `F2/Candidate/F2_FirstSnowTrailSign_v3_CANDIDATE.png`: `70b8ed868d61b1abc37d8f775116613ea45352b80fd53fa0cacf7d29a61ce37c`
- `F2/Candidate/F2_PathEdge_LowFence_v3_CANDIDATE.png`: `3aa0a8081ff1a58a4ab7157a974a550fc0b6c57a8263e1a81774b0027edadd6e`
- `F2/Candidate/F2_SnowStumpPerch_v3_CANDIDATE.png`: `eb32bf200da3f08023f0f83b32df9a2f75b9357fa8ffc1f2647b116a5f5b60b8`
- `F2/Reference_Alternative_Concept/F2_FirstSnowTrailSign_FULLREDRAW_REFERENCE.png`: `864e964ee4cc3868bd315af2249fe92d04e888937ae66f963232636cda8e4488`
- `F2/Reference_Alternative_Concept/F2_PathEdge_LowFence_FULLREDRAW_REFERENCE.png`: `49c2969b8607d91b6bf15cb0e4485bcc3f25379669064b025d63b099c9c62e8c`
- `F2/Reference_Alternative_Concept/F2_SnowStumpPerch_FULLREDRAW_REFERENCE.png`: `251fad7f5c6271f4f22dab6eeb4389e75864e4ec11380888b267b8892a435af3`
- `Spider/Spider/ForestSpider_Organic_v2_REFERENCE_KEEP_512_RGBA.png`: `c949f37ad5511753b064b83d521e79bb76409b0427cd0abf0c9742d6d9d46661`
- `Spider/Spider/ForestSpider_Organic_v3_EDGE_FINAL_CANDIDATE_512_RGBA.png`: `52d2e5fbf0090bf9925cd2fc295df1afd1e20ad6f1368dac10368d3e1fa8a137`
- `Spider/Spider/v3_EDGE_128px.png`: `80ba6a42533f206f452d9a27b3eb30884edd3556ce3e63f8f0e6c536a5f21722`
- `Spider/Spider/v3_EDGE_40px.png`: `3b2d71cf035279f6a6578402044b412b8ad2052b6070755fbfd67346b37b03aa`
- `Bee/Bee/VenomBee_Attack_Key01_Prepare_315x253_RGBA.png`: `2b78256f9d8c2d6813447b4f65fb9fd4c5b466e0e6df9f1b121ef23e4141965e`
- `Bee/Bee/VenomBee_Attack_Key02_Sting_315x253_RGBA.png`: `03baaaf637dc196b6ca89b69efcf536799b813837b078ee6c93182d2b19b62c3`
- `Bee/Bee/VenomBee_Attack_Key03_Recovery_315x253_RGBA.png`: `6ac34146367c5fcf7b35dba41788d61269149e7df2eb63836246928326b96fed`
- `Bee/Bee/VenomBee_Attack_Keyposes_REVIEW_ONLY_10FPS.gif`: `71de7aa7a1806d765a8f00d7cca78361eb270f3cf9a7720eb5c6cfaaaa39bce4`
- `Bee/Bee/VenomBee_Idle_Key01_WingsFolded_238x215_RGBA.png`: `aa4e85942704b9c14192ed787073f85c4bcca788d509a5c708be412e54dddd1f`
- `Bee/Bee/VenomBee_Idle_Key02_WingsSpread_238x215_RGBA.png`: `fdba594ccaf3cc50f38e25d0e18487ced1b70b634c05f62239f15d009cbbc759`
- `Bee/Bee/VenomBee_Idle_Keyposes_REVIEW_ONLY_10FPS.gif`: `9db1718394ec3e03935ac7a31b0c2c1d13247fa5a59cec3a8d1237b0da2317d5`

## 재현 근거와 보호

검사/출력은 모두 `Temp/ArtMain24ThreeZip20261103/` 아래입니다.

- `Audit.json`: ZIP/Manifest/SUMS·PNG spec·pixel delta·기존20 대조.
- `InputSHA256.json`, `ProtectionAfter.json`: 입력 원본 전후 SHA 및 원본 Git/Main24 브랜치 보호.
- `GIFCompositeOrder.json`: 투명영역 RGB로 인한 오판을 피한 배경합성 MSE 프레임 대응. `GIFFrameOrder.json`의 raw RGBA 거리만으로 순서를 판정하지 않았습니다.
- `Review/BrowserQA.json`: headless 브라우저 이미지 decode·네트워크 없는 이미지 의존성·4모션10/2/10/3프레임·앞뒤제어·4배경·가로폭 **18 PASS/FAIL0**. 렌더 확인용 headless 앵커 스크린샷은 일부 빈 뷰포트로 나와 게임뷰/미술 근거로 사용하지 않았습니다. 실제 PNG 비교판·경계 확대를 직접 검토했습니다.
- `Review/index.html`: 최종 단일 OFFLINE 비교판. Python 초기 실행의 인코딩 오류는 UTF-8 명시로 수정했으며 소스 아트 변경이나 기술 검사 실패가 아닙니다.

입력 **70파일 SHA 변화0**, 원본 main `ab47b931fec92976170f9f50aabde1822d78bb90`, status344항목 전후 지문 동일. Main24 기존 HEAD `fd869f60c9fde438a3dd97885cb6f447efc4454b` 보존. 원본 Unity·Scene·Prefab·Quest·Save/UserData·BuildSettings·Packages/ProjectVersion·Discord#66 변경0, 개인 Save 열람/복사/해시0, Unity 실행/설치/버전변경/전체QA0, 화면포커스 조작0.

브랜치 `codex/art-main24-3zip-delta-20261103`, 출발점 `fd869f60c9fde438a3dd97885cb6f447efc4454b`(Main24 #80 최종 영수증). **이번 delta는 본 보고서와 CODEX_LATEST Markdown2개만 커밋**합니다. HTML·원본/비교PNG·JSON·도구는 Temp에 보존하며 원격 Push하지 않습니다. main 직접 커밋/Push/병합·본편 적용0.

다음 최소 작업: ①대표가 F2 대안 표지판·울타리 선택, 그루터기 형태 HOLD 결정 ②Spider 앞다리 밑띠만 국소 보완 ③Bee 기존5포즈 공격축·회수·관절만 표적 보완. 새32/20프레임 양산이나 Unity 통합으로 자동 확대하지 않습니다. Main24 #80의81/14 PASS는 그대로 보존합니다.

## GitHub 보고 영수증

검수 보고/인계 commit `3af7f08d4febf062865ef5b437e0951e80315bd6` 기준으로 [#81](https://github.com/HappyPaul812/Project-Limitless/issues/81#issuecomment-6590636705) · [#75](https://github.com/HappyPaul812/Project-Limitless/issues/75#issuecomment-6590647264) · [#60](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6590648055) · [#63](https://github.com/HappyPaul812/Project-Limitless/issues/63#issuecomment-6590648852) 실제 댓글 등록 성공. 본 후속 commit은 영수증만 기록합니다. 원격에는 댓글만 게시했고 문서/HTML/PNG Push·게임 적용은 없습니다.
