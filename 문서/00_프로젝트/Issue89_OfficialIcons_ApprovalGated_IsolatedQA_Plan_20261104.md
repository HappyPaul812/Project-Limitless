# Issue #89 정식 상태 아이콘3 접수·승인 후 B HUD 격리 QA 준비

**THREE_ICON_ZIP_SHA_MATCH / USER_ICON_ART_APPROVAL_PENDING / ISOLATED_QA_SCOPE_READY_NOT_EXECUTED / NOT_ORIGINAL_APPLIED**

최신기획6627088203/4bdde36e·아이콘납품c2125d5258dcce8cc1eac81e76453e2e6e015c52를확인했습니다. 눈송이/세기둥/금간얼음의서로다른형태가있는**정식용아이콘후보3종은납품됨**으로갱신합니다. 이전임시VFX아이콘이정식승인됐다는뜻이아닙니다. 바울대표아이콘미술승인기록은아직없어**이번에는계획만준비**합니다.

128×128RGBA 후보매핑:
- Chill → Icons/ColdStatus_Chill_128_RGBA_CANDIDATE.png,보고SHA1f3ac724590a2d490cbe4a7cd28856c8ce5d888b130dde46a7d0008e6a3d0a16
- Frostbite → Icons/ColdStatus_Frostbite_128_RGBA_CANDIDATE.png,b814b1df3b0a5dbba46205d3b6edf91339cc4f45d118ec19ac53afab43c89a03
- FrozenNecrosis → Icons/ColdStatus_FrostNecrosis_128_RGBA_CANDIDATE.png,da2f175d50c47961df4c2fe6a227219a594ac9c3b0874c2c50e9edf39644be9d

동봉12/14/20/40축소판·14px흑백판은기존OFFLINE자료로재사용합니다. 작은눈송이/기둥/균열형태차이는보이나실제B HUD접근성PASS로옮기지않습니다.

## 승인 후 최소 범위 (이번 미실행)

1. 기존격리fixture `Temp/Nov04F2Smoke20261104/QA/Client`와B레이아웃후보재사용. 새입력전용폴더에3PNG만복사,단일Sprite/중앙pivot/압축off/미맵off·알파보존. 128원본Point설정을우선시험하고실제작은배율에서경계가나쁘면960대표한장만Bilinear대조. PPU/Pivot/Atlas는QA사본값과제품공식SPEC을구분,임의카탈로그/Atlas등록0.
2. 기존 `BattleCombatantStatusViewModelFactory.ApprovedColdIconResolver`의 **아이콘해결참조3개만**승인아이콘Sprite로연결. 전장VFX3Sprite변수와아이콘Sprite변수를분리해VFX그림까지바뀌지않게함. 시험종료시resolver=null로복구. 원본코드패치0.
3. B기준고정:40px가시경계8px/128px본체뒤·하향24px/960상태두행/1680HUD경계8px. 이름·HP·스킬메뉴·상태명·턴·괴사1/3,2/3!·다음중첩즉시KO문자유지. 아이콘안문자굽기/지속3/3/상태로직변경0.

대표신규RenderTexture **5장으로시작**(기존B구아이콘캡처재사용,50/99전수반복0):

|조건|상태내용·목적|
|---|---|
|960×720 밝은설원·40px VFX·괴사1/3|한기2턴/동상2턴/괴사1/3·실제약12.12px아이콘식별|
|960×720 밝은설원·40px·괴사2/3|위험문구/숫자/이름을아이콘변경으로가리지않는지|
|960×720 기존어두운전장·40px·괴사2/3|어두운균열아이콘·배지경계형태와문자|
|1280×720 기존어두운전장·128px·괴사2/3|현재밀집HUD에서아이콘변경의폭/텍스트간섭만확인|
|1680×720 기존어두운전장·128px·괴사2/3|HUD경계/아이콘표시배율과문자간섭만확인|

모든배지에냉기3종문자가동시에있는합성검증상태를사용하여매상태마다전투를다시시작하지않습니다. 각화면의아이콘실화면Rect를측정하고기존12.124px를모든해상도에고정값으로오인하지않습니다. 실제HUD크롭을색/흑백으로같이보고아이콘단독형태와문자포함식별을분리판정합니다. 기존OFFLINE실축소PNG들을추가Unity실행없이참고합니다.

배지Image.sprite가정확히3승인파일에매핑되는지,문구누락/잘림/겹침/아이콘타입혼동/알파테두리와마우스·키보드선택표시가림만새Delta관찰합니다. 승인VFX원본3/기존Evidence101/50결과는해시보존대조만하며동작전수QA/피해/해제/화로/접근성전체인증은반복하지않습니다. 문제가있으면해당한조건만배치·텍스트최소보완후재확인하고고유결과를중복PASS로세지않습니다.

#92흑요석미승인배경을먼저게임에연결하지않습니다. 현재는기존Darkfixture가대표어두운배경이며실Obsidian가독성은양쪽아트승인후별도해당조건1장확인대상입니다. 타체형/3인전체HUD·모든동적상태/성능전수는이최소범위밖입니다. 미술승인후에도원본적용은별도승인입니다.


## 실제 F: ZIP·보호·보고 경계

`F:\Downloads\LIMITLESS_Cold_Status_Icons_3_GameReady_20261104.zip` 165,753bytes. 전체SHA256 **f758b59b73847f18c13e0d48aafee2f58eac33802ec597a1c739078335b2baa3**,기존납품보고와정확일치. 이번전체ZIP해시는**1회만**계산했습니다. CRC/Manifest/SUMS/기존QA 전수대조는반복하지않고기존납품·기획검수기록을재사용했습니다. 선택이미지를읽어비교한것은새전체QA가아닙니다. [실측기록](Evidence/Issue89Followup20261104/ActualWholeZipHash.json).

Unity6000.6.5f1고정·이번Unity실행/Import/PlayMode/빌드0. 개인Save/UserData·원본Editor/Unity/Scene/Prefab/Catalog/GUID·사용자미커밋·승인아트수정0. 원본main634a91d3e4174c87761c0d377cfa5a695ca79e7f·tracked dirty308보호. main직접commit/Push/병합0. 기존153/15/99/50QA재실행·새PASS추가0. USER_VISUAL_REVIEW_PENDING / NOT_UNITY_APPLIED.

브랜치 codex/issues63-89-92-followup-20261104. CODEX_LATEST/CURRENT_STATUS갱신. 이이슈관련실제commit은각Issue결과댓글참조. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 비교판/근거는로컬보존,원격에는실제댓글/경로/SHA만기록합니다.
