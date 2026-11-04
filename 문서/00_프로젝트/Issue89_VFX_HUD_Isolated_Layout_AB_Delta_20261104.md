# Issue #89 냉기 VFX·HUD 실제 격리 A/B Delta / 2026-11-04

## 결과와 추천
**ISOLATED_LAYOUT_DELTA_EXECUTED / NEW_UNIQUE_50_PASS_0_FAIL / B_LAYOUT_RECOMMENDED / USER_LAYOUT_REVIEW_PENDING / NOT_ORIGINAL_APPLIED**.

[최신 실행 지시](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6623744401)에 따라 제안만 반복하지 않고 **실제 격리 Unity6000.6.5f1 (3ff58d469c8a)** Play Mode에서 UI RectTransform·그리기 순서·상태배지 배치를 변경해 A/B 캡처했습니다. **B 묶음(40px 위치 분리 + 128px 뒤/아래 + 960 두 행 + 필요한 전장 경계 조정)**을 추천합니다. 승인PNG 미술은 유지하고 레이아웃 최종 사용 판단은 대기합니다.

기존99PASS를 새 검사에 더하지 않으며 기존12조건/49캡처를 전수재실행하지 않았습니다. 기존Evidence101파일의SHA **101/101 동일**, 승인원본3PNG SHA **3/3 동일**로 실제 보존을 확인했습니다. 현재개인Save/UserData는접근하지않았습니다.

## 최소 실행 범위
같은조건의A(기존)→B(조정)를연속렌더했습니다. 새아트/Particle를만들지않고 기존fixture `Temp/Nov04F2Smoke20261104/QA/Client`·기승인PNG3·합성Save 가드를재사용했습니다.

| 대표조건 | 상태 | 실제 A/B쌍 |
|---|---|---:|
| 960x720·밝은설원·40px | 한기,동상,동결괴사2/3,동결괴사1/3보충 | 4 |
| 1280x720·어두운전장·128px | 동상,동결괴사2/3 | 2 |
| 1680x720·어두운전장·128px | 동결괴사2/3·HUD전장경계 | 1 |

총 **7쌍·14장 실제Unity A/B PNG** + F2배경촬영근거1장 =15PNG. 세대표조건일뿐이전12조건매트릭스가아닙니다. A/B병렬비교판3장·1:1상태크롭1장은실캡처를그대로나란히둔파생자료이며원본PNG와구분했습니다.

## 실제 변경 내용·수치
### 40px VFX 분리
현재40px전체캔버스크기는유지합니다. GPU로Sprite알파를읽어캐릭터와VFX의가시경계(알파>8/255)를구하고두경계사이화면간격을8px로맞췄습니다. 투명캔버스여백을간격으로세지않습니다. 같은캐릭터본체/색/원본알파를수정하지않았습니다.

| 상태 | A 가시경계 간격 | B 가시경계 간격 |
|---|---:|---:|
| 한기 | -26.253px | 8.000px |
| 동상 | -27.034px | 8.000px |
| 동결괴사1/3·2/3 | -28.753px | 8.000px |

음수는겹침을뜻합니다. B실캡처에서는바퀴청색장식과VFX가독립된위치로보여고리/결정인지가개선됐습니다. 한기의흰연무는여전히설원에서옅으므로상태명/턴등별도UI가필요하며VFX단독최종가독성PASS로쓰지않습니다. 동상/괴사의작은그림은색외세부차이가제한적이지만바퀴혼합은제거됐습니다.

### 128px 뒤 레이어·위치
UI상동일부모아래에서VFX를캐릭터SpriteImage보다먼저그리는형제순서로옮기고기존시험점보다화면24px아래로이동했습니다. GameObject레이어나원PNG를바꾼것이아닙니다. 128px크기·알파는유지했습니다.

동상/괴사/1680괴사세쌍에서 **B=뒤그리기확인·상대하향24px확인**. 실제캡처에서얼굴·갑옷·휠체어본체가앞에보이고결정은주변/아래로남아기존하체차폐가줄었습니다. UI사각형의투영겹침자체는남으며(1280 canvas간격 -77.625→-53.625px)그리기순서로본체를보호하는것이므로'기하겹침0'이라고표현하지않습니다. 정적일부캡처를넘는픽셀별완전비차폐·다른외형은검증하지않았습니다.

### 960 HUD 두 행
실제상단HUD의기존상태배지오브젝트를재사용해첫행3/둘째행2칸으로재배치했습니다. 기존이름/HP상단위치는고정하고합성플레이어의상태행만높이를확보했습니다.

- 상태배지루트는첫행94x22UI/둘째행140x22UI,Text14UI·아이콘14UI. 이름/HP영역과겹치지않게배치했습니다. 긴괴사문구와기존굳건한자리도둘째행에남았습니다.
- 선언글자크기환산은 **A8.660→B12.124 화면px**. A는BestFit가있어실제글자가선언값보다더작을수있다는점을구분합니다. B는자동축소를끄고라벨preferredWidth가영역안에들어가는것을확인했습니다.
- 한기2턴/동상2턴/동결괴사1/3·2/3!/행동중/굳건한자리정보를유지했습니다. '다음중첩즉시KO'는별도VFX설명Text로도유지했습니다. 지속3/3은없습니다. 임시상태아이콘은종전승인VFX를쓰는QA연결그대로이며공식아이콘승격/새아이콘제작0.
- HUD높이116→174UI,플레이어행32→82UI로늘리면서이름·HP의화면Rect(위치/크기)는A와동일합니다. 스킬메뉴/선택강조/취소배치도유지했습니다.

### 전장 상단 경계
960은두행HUD확장에맞춰전장상단만줄이고하단은유지했습니다. 1680은기존겹침을동일방식으로정리했습니다. 실제HUD하단과전장상단의측정간격은 **960 A30.806→B8.000px**, **1680 A-30.150→B8.000px**입니다. 1280의기존1.600px은이번경계표적이아니어서변경하지않았습니다.

B7쌍모두VFX전체캔버스가전장세로범위안에있고이름/HP/스킬메뉴의화면Rect가A와동일함을측정했습니다. 본체위치는전장높이변화에따라960/1680에서따라움직이지만전장하단/명령부는보호됩니다. 실제캡처로얼굴·상단HUD·스킬메뉴와VFX가분리됨을확인했습니다.

## 신규 검증과 남은 제한
- 신규 **최종고유검사ID50 PASS/0FAIL**. 최초새A/B50개중960/40간격정의를투명캔버스에서가시알파로정밀화하여영향받는네쌍만재실행했습니다. 재실행34개는이전동일ID결과를대체했고중복PASS로더하지않았습니다. 통과한128/1680은다시실행하지않았습니다.
- 50개는진입준비2,14캡처크기,7개B상태내용,40px가시경계4,128뒤/하향3,960두행4/라벨영역4,HUD경계5,이름/HP/메뉴위치유지7입니다. 미술/접근성50PASS를뜻하지않습니다.
- 최종두실행컴파일오류0·새Runtime오류0·엔진종료0. 각시작의기존Editor.Search예외1은환경항목으로분리했습니다. 초기QA코드의금지된GetInstanceID사용2건은Sprite키캐시로수정했습니다. CLI자동-quit로캡처전종료된시도는QA완료로세지않고직접배치로실제완료했습니다.
- **권장안B**의분리·하체보호·960문자가독성은A보다명확히개선됐습니다. 다만40px한기설원대비·작은상태아이콘의즉각구분,다른성별/길·일반도보·3인전체HUD·동적VFX/성능·모든화면비는이번범위밖또는남은시각판단입니다. 기존원화승인은그대로두고전체VISUAL_PARTIAL을무조건완료로올리지않습니다.
- 실행코드는Tools/Issue89LayoutDelta의격리표시후보입니다. 실제게임의정식상태/해제/화로/피해/스킬로직을연결하거나원본BattleSceneController를수정하지않았습니다.

## 실제 캡처·코드·보호
- [960 설원 40px A/B](Evidence/Issue89LayoutDelta20261104/Snow_960x720_40px_Chill_Stack2_AB.png)
- [1280 어두운전장 128px A/B](Evidence/Issue89LayoutDelta20261104/Dark_1280x720_128px_Necrosis_Stack2_AB.png)
- [1680 HUD경계 A/B](Evidence/Issue89LayoutDelta20261104/Dark_1680x720_128px_Necrosis_Stack2_AB.png)
- [실측표](Evidence/Issue89LayoutDelta20261104/MeasuredABSummary.json), [최종고유50검사](Evidence/Issue89LayoutDelta20261104/CombinedChecks.txt), [기존101/승인3 보존](Evidence/Issue89LayoutDelta20261104/ProtectionAfter.json), [실제PNG/JSON SHA](Evidence/Issue89LayoutDelta20261104/CaptureManifest.json).
- 실제14개A/B원본은같은Evidence폴더의 `_A.png`/`_B.png`입니다. 합성비교판과달리원본은Unity Camera/Canvas에서직접생성했습니다. F2배경은이전과동일한격리카메라촬영소스를전장배경으로표시한시험가정입니다.
- 전체로컬열람판 `F:/study/codex/Project-Limitless/Temp/Issue89LayoutDelta20261104/index.html`.
- 입력PNG3해시는기승인 e5083152…/5b8ca776…/59621e0b…와일치하고전후불변입니다. 기존99PASS의49PNG/검사/측정파일을포함한101파일의전후SHA도전부동일합니다. 보호Manifest에개인Save경로/바이트를포함하지않습니다.
- 원본main634a91d3e4174c87761c0d377cfa5a695ca79e7f 유지·trackeddirty308유지(전체바이트검사대체아님). 쓰기는Temp격리QA/별도worktree에만한정했습니다. 원본Unity/Save/UserData/승인PNG/Scene·Prefab/Packages/BuildSettings·사용자미커밋파일수정0, 사용자Editor/OS포커스조작0·main직접commit/Push/merge0.
- 로컬브랜치 `codex/issue89-vfx-layout-delta-20261104`, 직전관련06e871f916ecf6f1775eeb90f3e8d80836d25508, 이번commit은Issue89댓글에기록합니다. CODEX_LATEST/CURRENT_STATUS갱신. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 원격에는실제결과댓글과캡처경로/SHA만보고하며PNG가GitHub에첨부됐다고주장하지않습니다.
