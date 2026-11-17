# Issue #103 얼음갑충 Idle4·#107 Main04 여행자 오프라인 검수 — 2026-11-17

**OFFLINE_ONLY / BOTH_FILE_TECH_PASS / ART_APPROVAL_PENDING / UNITY_NOT_IMPORTED / NOT_APPLIED / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

바울 대표의 이번 **“두 ZIP 모두 기술 검수 먼저…게임 적용은 아직 하지 말고, 파일 검수와 미술 검토 의견만”** 지시를 따랐습니다. [#103 승인 KeyPose](https://github.com/HappyPaul812/Project-Limitless/issues/103#issuecomment-6934713199)·[최신 Idle4 납품](https://github.com/HappyPaul812/Project-Limitless/issues/103#issuecomment-6935149466), [#107 최신 여행자 납품](https://github.com/HappyPaul812/Project-Limitless/issues/107#issuecomment-6935879848), 프로젝트 컨텍스트·CURRENT_STATUS·CODEX_LATEST와 실제Main04코드를 확인했습니다. Unity 실행/도구호출/Import·게임적용·GUID수정·Push·Merge0입니다.

## A. 메인 #103 — 얼음갑충 Idle4

- **최종 판정: FILE_TECH_PASS / ART_REVIEW_PENDING (Idle4).** 기존 KeyPose의 USER_ART_APPROVED와 새 Idle4 승인을 분리합니다.
- **기술 검수:** ZIP CRC PASS, Manifest10/10 크기·SHA, SHA256SUMS11/11(payload10+확정Manifest) PASS. 전체payload포함·자기/순환참조/위험경로/중복entry0.
- **파일 구성:** 3010425bytes/12파일. 시트1·분리QA4·128/40스트립2·512/128/40비교판1=PNG8, 반복GIF1, SourceDeclaration/Manifest/SUMS3.
- **ZIP SHA256:** `8a9ecec00a98cb094e155aa31f7e737190358d9481ad333d2371e8013281ea87`. 최신 납품과 일치.
- **게임용 시트:** `Sprite/IceBeetle_M4_Idle4_2048x512_RGBA_CANDIDATE.png`, 실제2048×512RGBA·4×512셀, SHA256 `bd06c6d9a901d4c3550882ad4ce061629195b60e75f04636ec4814ebb0967793`. 외곽alpha0, 분리4프레임은 시트각셀RGBA와4/4동일, 네 프레임콘텐츠상이.
- **승인 KeyPose 대조:** 별도 `F:/Downloads/IceBeetle_Issue103_OnePose.zip` 실물 SHA `ca56030e22d581ef58bafe51b66cc9181478dfa8343f16f33155aeda8f14323a`·CRC/Manifest6/SUMS7 PASS. 승인PNG SHA `b7ff0592226c2a1f28270053cf03483b6051c69edb412b59a7965ccf8699b3ad` 일치. **시트 첫셀 RGBA가 이 독립 승인원본과 완전동일**, 제출QA00 PNG도 승인PNG와 **파일바이트 전체동일**입니다. 새시트자기자신만 대조한 검사가 아닙니다.

| 프레임(사람기준/파일번호) | KeyPose 대비 달라진RGBA픽셀 | 변화bbox [x1,y1,x2,y2) | y397~511 하부/발 |
|---|---:|---|---|
|1 (00)|0|None|동일|
|2 (01)|78587|[28, 127, 484, 396]|동일|
|3 (02)|78571|[29, 127, 485, 396]|동일|
|4 (03)|74352|[29, 127, 484, 395]|동일|

2~4프레임 가시영역 중심의 세로 차이는 원본 대비약+0.468/-0.740/-0.095px, 외곽bbox는 최대1px변화이며 하단은모두y464(끝제외)로고정입니다. 높은 변경픽셀수는 상부의미세리샘플링을포함하며 그 숫자를 큰동작으로오인하지 않습니다. 하부다리/발은 전RGBA가동일하여 접지훼손0입니다.

- **실제 축소/비교판:** 128/40스트립은 각셀LANCZOS축소와 **알파 및 보이는RGBA 전부동일**. 완전투명alpha0의숨겨진RGB채널만806/124개차이여서 rawbyte완전동일이라고하지않습니다. 납품512/128/40보드의12개배치 불투명전경전체를 원본/직접축소와대조해일치했습니다.
- **GIF:** 실제512²/4프레임/loop0(무한반복), 각원본셀을RGB(27,39,55)배경에합성후256색MEDIANCUT양자화한결과와 디코드4프레임RGB완전일치. 원본에서직접파생된표시입니다. **실제지연은120/120/120/120ms**, 1주기480ms·평균약8.33FPS입니다. 문서의125ms/8FPS는GIF10ms단위저장으로실제파일과차이가있으므로 **정확8FPS재생증명으로사용하지말것**. 이는이번필수PNG규격/무결성실패는아니나 표시자료의정확한속도가필요하면문서/생성설정정정권고. 이번파일수정0.
- **미술 코멘트:** 두꺼운청록빙갑·미세갑각내림/올림/원자세복귀로읽히고 작은다리의접지가안정적입니다. 과장점프/행보/실루엣재디자인을발견하지못했습니다. **특별히이상한프레임없음**. 128에서는미세변화가보이지만40에서는갑각결정·움직임세부판독이약하며, 이는과장없는Idle이라는기준과충돌하지않습니다.
- **Unity 적용 여부: NOT_APPLIED. 사용자 승인 필요: YES, 새 Idle4만.** KeyPose를재승인대상으로돌리지않으며 시트/GIF를이번에수정하거나재생성하지않았습니다.

## B. 리뉴얼 #107 — Main04 부상당한 여행자

- **최종 판정: FILE_TECH_PASS / USER_ART_APPROVAL_PENDING.** 사용자미술승인전후를섞지않습니다.
- **기술 검수:** ZIP CRC PASS, Manifest7/7 크기·SHA, SHA256SUMS8/8(payload7+확정Manifest) PASS, 전체payload포함·자기/순환참조/위험경로/중복entry0.
- **파일 구성:** 650099bytes/9파일. 원본PNG1·128/40PNG2·512/128/40비교판1, QA/README/SourceDeclaration/Manifest/SUMS5.
- **ZIP SHA256:** `286fe97b6f68f44f45d22a6a4940b9e5b84fc09d8b53d7d9333211e3bc4311a3`. 최신납품과일치.
- **실제 원본:** `Candidates_512/Main04_WoundedTraveler.png`, 512×512RGBA투명·외곽alpha0, bbox[106,122,406,470), 폭300/높이348px. PNG SHA256 `2687c1c62e5bd20e6e0fb16c3428225aa28fc156c80a5c08b6af2350d9e9715d`. 128/40프리뷰는원본LANCZOS축소와 **RGBA전픽셀동일**. 비교판의승인OGA07/OGA11+새여행자×512/128/40=9배치도불투명전경전체대조일치.
- **기존 OGA07 임시본과 차이:** 현재 MainQuest04FieldFlow.CreateWoundedTraveler는 여전히 `VillageNpcSprites/Eldiran/OGA07_Resident01` **legacy32px Sprite**를 사용하며, 납품비교판의512승인OGA07은 그와다른참조입니다. 이번에 실제legacy32/승인OGA07/새여행자3종을별도비교해구분했습니다. 기존임시본은서있는마을주민을어둡게표시한외형이고, 새후보는 **땅에앉아붕대한팔을감싸며 여행망토·배낭·침낭을지닌독립여행자**로상황정보가늘었습니다. 기존OGA07/OGA11원본교체나리뉴얼재적용0.
- **미술 코멘트 1:** 앉은자세·어깨/팔을감싼손·붕대·걱정/피로표정으로치료중쉬는상황이자연스럽게읽힙니다. ‘회복완료’를확정하는그림은아닙니다.
- **미술 코멘트 2:** 기존서있는주민들과자세·망토/여행장비·체격으로구별되며 과한피·노출상처·고어/극단적부상표현없음. 붕대부위/성별/신상/후유증·Main07인물과의관계는새캐논으로확정하지않습니다.
- **미술 코멘트 3:** 128에서는부상단서가읽히지만40에서는붕대·표정이약하므로 대표시각검토는512/128과40을함께보여드리는것이좋습니다. 자동재제작/과장된상처추가권고는하지않습니다.
- **Unity 적용 여부: NOT_APPLIED. 사용자 승인 필요: YES.** 기존코드/tint(.82)/Sorting7/StoryLabelY1.15/Quest·Save를변경하지않았습니다. 후속승인시앉은자세의상대체격·맞춤PPU/Pivot·미엘과동시표시·기존tint/라벨간격을별도최소QA해야합니다. 512PNG를기존PPU28로단순직접Import하지않습니다.

## 공통 보호·판정과 다음 단계

**두 ZIP 모두 필수파일기술검수통과**, 미술승인/Unity동작PASS가아닙니다. 사용자에게는모든비교판/GIF가 **오프라인자료이며게임화면이아님**, A의움직임은미세하고GIF실속도는약8.33FPS, B의팔붕대는시각제안이며세부는40px에서제한적임을함께안내합니다.

다음은 **Idle4와여행자각각의사용자미술승인**입니다. 승인뒤별도적용지시/작업범위에따라최소Unity검증을거쳐적용가능합니다. Idle4는현재KeyPose승인만으로몬스터ID/Spawn/스킬·Attack/Hit/Defeat를자동추가하지않으며, 여행자는Main04의해당임시참조한곳만후보로삼고다른주민/스토리로확대하지않습니다. 이번작업은여기서끝나며자동Import/게임적용없습니다.

원본 Unity6000.6.5f1·Issue85 HOLD·승인원화/Save/UserData/Discord·기존미커밋/미추적보호. 원본Unity파일수정/Import/Play/Scene·GUID/meta변경/새대형QA/Push/Merge/reset/clean/stash0. Unity도구/실행자체를이번에는사용하지않았습니다. 현재코드및모든보호원본SHA의종료대조는private `Temp/Art103107Offline20261117/Final.json`에기록합니다.

로컬브랜치 **`codex/art103-107-offline-review-20261117`**, 기존소형문서worktree재사용. 본보고/CODEX_LATEST/CURRENT_STATUS **문서3개만로컬commit**, 원본mainHEAD52f84e643d5d860665eadfe1614a30c745209f00유지. 최종실제SHA는관련Issue103/107댓글·Final.json에기록하며원격문서링크로가장하지않습니다. GitHub에는결과댓글만실제작성·바이너리업로드0.

상세근거: private `Temp/Art103107Offline20261117/audit.json`, boards-verification.json, Idle4/Traveler/KeyPose검수사본, Idle4_GIF_DecodedFrames.png, Main04_Legacy_Approved_New_OFFLINE.png. 생성원본1254PNG의이력을독립재생성한것은아니며ZIP에포함되지않은생성원본SHA는출처식별정보로구분했습니다. 기존완성원화에대한쓰기0입니다.
