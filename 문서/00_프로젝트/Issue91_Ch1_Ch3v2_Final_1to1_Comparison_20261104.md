# Issue #91 Chapter1 실제3종·Chapter3 v2 실제2종 최종1:1 비교

**CH1_V2_1TO1_COMPARISON_DELIVERED / USER_VISUAL_REVIEW_PENDING / UNITY_NOT_TESTED / NOT_UNITY_APPLIED**

[총괄 기획6624809962](https://github.com/HappyPaul812/Project-Limitless/issues/91#issuecomment-6624809962)와 기획정본2eda4a655a0b0f4f28992df63107a7f8805b7d3f를 확인했습니다. 검수완료된 v2 PNG를 그대로 재사용했으며 ZIP/CRC/Manifest/SUMS/정식v2동일성 검수를 반복하지 않았습니다. 입력5장의전후SHA는원본보호증거로만기록했습니다.

## 요청 산출물 한 장

`F:\study\codex\Project-Limitless\Temp\Issue91Ch1V2Final20261104\LIMITLESS_Issue91_Ch1_Ch3v2_1672x941_1to1_FinalComparison_20261104.png`

- 실제5144×4400 RGB / 13,068,336bytes.
- **SHA256 `ae44f57176aaf9aac52d169e6e63ef8e2af2a9ff4b3fa8e6833626b77a1b64f3`**.
- 실제Forest/Grassland/SilentCatacombs 원본3개+Snowroad_v2/Forest_F2_v2 후보2개를 각각 **1672×941·1:1**로 배치했습니다. 5원본패널은입력RGB화소5/5정확일치,리사이즈·샤프닝·색보정·그림덧칠0.
- 세부크롭은같은원본좌표의256×256을1:1로배치. 논리도트격자/화풍품질점수를임의확정하지않았습니다.
- 새비교PNG는이1개이며기존Ch1-v1판을v2로이름만변경한것이아닙니다. 기존v1/v2전후판은직전납품자료를참조하고새전후판을만들지않았습니다.

[실제 비교판](Evidence/Issue91Ch1V2Final20261104/LIMITLESS_Issue91_Ch1_Ch3v2_1672x941_1to1_FinalComparison_20261104.png) · [패널화소·입력SHA·배치가정 근거](Evidence/Issue91Ch1V2Final20261104/ComparisonEvidence.json).

## 실제동일배율 관찰

Ch1은수피/이끼·풀/돌·석재/부스러기등작은불규칙색면과재질별명암·거리별세부감소가풍부합니다. v2는지정부분의구름/산면·눈길·수관/수피의질감과국소명암이늘었으나큰타원수관·삼각수목·넓은평면길의구조가남습니다. 세밀한질감과주변큰단색면간밀도차이가보이며Ch1화풍동등PASS를부여하지않습니다. 계절/지역의색차이를결함으로단정하거나전면재제작을지시하지않습니다.

남은시각판단(소스좌상단원점,우/하단제외):
- Snowroad 원경[650,20,1320,475]:새질감과큰타원구름/산면의표현체계조화.
- Snowroad 눈길[700,535,1180,941]:눈/길질감은늘었으나곧은가장자리·반복형태와Ch1의불규칙근거리재질차이.
- Forest 수관[0,0,480,330]·[1240,0,1672,340]:눈/잎의국소명암은늘었지만큰타원덩어리실루엣이남음.
- Forest 우측수피·가지[1310,240,1640,660],접지·전경눈/바위[1310,660,1640,925]:후열주변의수피명암/눈테두리밀도와실캐릭터윤곽경쟁가능성.

## 정식배치·후열가독성 및자료경계

기존격리프로젝트 BattleSceneController.cs 516·518행의 아군전열x=.68/후열x=.86,y=.68-.21×Column과최신기획의정식방향을참조했습니다. 적왼쪽(.32/.14),아군오른쪽(.68/.86)입니다. 기존전장Rect1280기준x50..1230,y228..550에전체PNG를Fit한정적가정으로후열0≈원본(1391,433),전열0/1≈(1113,433)/(1113,521)입니다. 실제후열Sprite크기/피벗/알파경계측정값이아닙니다.

F2후열0이우측수피/가지ROI안에있어짙은수간/가지와어두운외형윤곽이섞이거나밝은눈테두리가상태/VFX와경쟁할수있습니다. 배경은뒤에그리므로본체를기하학적으로덮는다고주장하지않습니다. 실제3인Sprite·후열선택표시/장비/휠체어·상태정보를이번모형만으로PASS처리하지않습니다.

비교판하단왼쪽은기존 `문서/00_프로젝트/Evidence/SkillLoadout_20261013/Runtime_Final/battle_1280x720.png` **과거실제Unity캡처(1280×720 1:1)**입니다. 3인HUD/캐릭터·적·상세팝업점유참고이며C3v2실행사진이아닙니다. 하단중앙F2는별도복제패널에참고사각형/중심/ROI를그린 **OFFLINE도식·실제Sprite/UI아님**으로명시했습니다. 위원본5패널에는모형선/주석을겹치지않았습니다.

이번새Unity실행/Import/PlayMode/전수QA0. 실제3인·적·960/1280/1680가독성은미술승인후별도격리DeltaQA항목이며이번정지비교로통과시키지않습니다.

## 보호·상태

**입력5장SHA전후5/5동일**(전체해시는JSON참조). Ch1공식3/Ch3v2후보2원PNG수정0·새v3배경0. 승인Attack8/VFX·원본Unity/Editor/Save/UserData/Scene/main/사용자미커밋수정0. Unity6000.6.5f1유지. 원본main634a91d3e4174c87761c0d377cfa5a695ca79e7f·trackeddirty308유지. Push/병합0.

바울대표의두v2최종미술승인은**대기**합니다. 이번납품으로Ch1-v2직접비교자료누락만해소했고,전체화풍동등/후열실게임가독성/원본적용승인으로확대하지않습니다. 기존간편ZIP의Manifest미포함은별개이력으로유지합니다.

브랜치 `codex/issue91-ch1-v2-final-comparison-20261104`,직전44232fd99f5cbbe0ef42973ec3e233da85b10889. CODEX_LATEST/CURRENT_STATUS갱신. 실제commit은Issue91결과댓글참조. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 원격에는결과·로컬실물경로·SHA만기록하고PNG업로드/Push하지않습니다.
