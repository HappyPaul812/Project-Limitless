# Issue #63 VenomBee v3 승인 등록·격리 연결·권리 조사 — 2026-11-04

**VENOMBEE_V3_USER_ART_APPROVED / APPROVED_PNG_BYTES_KEEP / ISOLATED_V3_CONNECTION_PASS / RIGHTS_HOLD / TARGET_CONTACT_NOT_VERIFIED / NOT_UNITY_APPLIED**

[바울대표최종승인6624979503](https://github.com/HappyPaul812/Project-Limitless/issues/63#issuecomment-6624979503)·이전v3검수6604123613·리뉴얼제작676ec9d440c6d3e59b9285ec3d3672113128dfb0를확인했습니다. 기존후보A153PASS를재실행/합산하지않고 **변경v3 연결1회·집중15PASS/0FAIL**만실행했습니다.

## 최종 승인 등록·SHA 보호

- Sting v3 **315×253 RGBA** SHA `2850020146718f30ac320cd6bd0c42dcf31332ed2820a718d06bcc76949a2d67`
- Recover v3 **315×253 RGBA** SHA `69d0944b3565fe2e4cc92034e51df2006847103a366db18d21221561fa5a19bb`
- Prepare KEEP SHA `e337921236b7b947f92bbb5d88824bc6729d6668a050a85fc2f519ff4df26212`
- IdleFolded KEEP SHA `d17114176c13c2911cd533a0e7a363a1f4ae2ca110b540bcec5d63a2aa941605`
- IdleSpread KEEP SHA `d35f4583c0fa119396a40a6637f48b36b677fe00327640c655d86bde3dd47820`

승인등록부 [ApprovedInputRegistry.json](Evidence/Issue63ApprovedV3Delta20261104/ApprovedInputRegistry.json) 및 승인원본바이트2개를별도 `ApprovedPNG_BYTES_KEEP/`에보존했습니다. Unity원본자산/카탈로그등록이아닌 **문서브랜치의승인자산대장등록**입니다. 두포즈최종미술승인대기는해소됐으며새작화/리터치/재저장0입니다.

지정F:/Downloads/VenomBee_StingRecover_v3_20261103.zip은이번조회시존재하지않았습니다. 예전ZIP SHA1169aca1c46f4322b85e7247b33a6cbc1fb684e01e0f04fb2ea7f9e9c0194ff0는이전실검수기록값으로만인용합니다. 이번에F: ZIP을열었다거나CRC를다시통과했다고하지않습니다. 대신이전검수의자급형HTML에내장된PNG에서 **각승인SHA와정확일치하는바이트만**복구했습니다:
`Temp/BeeStingRecoverV3Review20261103/Worktree/문서/00_프로젝트/Evidence/BeeStingRecoverV3_20261103/index.html`,SHA2bffa3c14b06b34986cf8dca6332bca8b124acdafc1a91d328772feda4e90ae3. 기존Prepare/Idle은기존Reference_KEEP실파일을읽었습니다. 원본/복구본/실행입력의출처와SHA를등록부에구분합니다.

## 실제 연결 방안과 최소 실행
**원본과 후보A 구현 차이:** 원본 main의 재생기는 PlayAttack()이며 후보A의 PlayAttack(bool)·IsAttackPlaying 및 Presenter 완료대기 인자가 아직 없습니다. 이번 PASS는 그 후보A 변경 코드가 있는 격리 fixture 위 v3 포즈 연결의 결과입니다. 원본에 파일만 교체하면 동일 동작한다는 뜻이 아닙니다. 별도 원본 적용 승인 후 후보A 타이밍 변경의 통합·컴파일/표적 검증도 필요하며, 이번에 원본 코드를 수정하지 않았습니다. [코드 SHA 구분](Evidence/Issue63ApprovedV3Delta20261104/OriginalVsCandidateCode.json).



기존게임표시는Animator Clip이아니라 **MonsterSpriteSheetAnimation.Configure(Image,MonsterDefinition)**와 **BattleActionPresenter.PlayMeleeAttack** 조합입니다. 이전후보A의해당3코드를바이트수정없이재사용했습니다. 새v3에실제독립10장중간작화는없으므로,승인키포즈반복시트를새격리폴더에만조립했습니다:

|Attack10@10FPS 슬롯|연결|
|---|---|
|00–01|기존Prepare|
|02–03|승인Sting v3|
|04–08|승인Recover v3|
|09|Recover유지(이전납품GIF의Prepare재등장교정)|
|1초후|기존IdleFolded00→IdleSpread01|

공격315×253/10열,Idle238×215/2열,중앙pivot,런타임공격PPU315·IdlePPU238,우향/visualScale.85계약을읽었습니다. 시험UI는공격전체캔버스폭128px·원비율이며실게임스케일최종확정이아닙니다. 승인PNG를수정하지않고별도QA시트에동일RGBA를배치한것입니다. **3키포즈반복10슬롯이지독립Attack10완성아님**.

격리프로젝트 `F:/study/codex/Project-Limitless/Temp/BeeAttackTiming20261103/QA/Client`,Unity **6000.6.5f1(3ff58d469c8a)** 배치PlayMode에서새 `BeeV3DeltaBatch.Run`만호출했습니다. 기존 `BeeTimingBatch.Run`·153검사/배속/일시정지/다른몬스터/저장/큐전수항목은호출하지않았습니다.

**새집중15PASS/0FAIL**:실제PlayMode/버전1,승인Sting·Recover·09Recover·00Prepare셀일치4,10슬롯관측1,피해02·시간2,콜백1회1,09완료전유지1,1초완료1,Idle00복귀1,09Prepare재등장없음1,루트·방향복구1,새Runtime오류없음1.

실제126개샘플·38UnityPNG(20FPS Stage26+고유키12). **피해훅 .2699998초/index02,완료1.0099993초,완료즉시venom_bee_Idle_00**. 기존1초계약허용범위내입니다. Time.captureDeltaTime=.01의결정적게임Clock검증이며새벽시계성능/실시간FPS시험은아닙니다. 합성피해훅은호출만관찰하고0을반환하므로실HP/독/피해량연결PASS가아닙니다. 기존153의실시간/배속결과를새v3검사로재명명하지않았습니다.

컴파일오류0·새Runtime오류0·엔진종료코드0. 기존코드의CS0618폐기예정API경고는유지,Editor.Search.SearchDatabase초기화예외1건은EnvironmentErrors에별도보존했습니다. 에셋없는실게임대상을대신한흰합성사각형은실제아군Sprite/히트박스가아니며캡처상침끝과표적사이여백도남습니다. **TARGET_CONTACT_NOT_VERIFIED / FULL_COMBAT_READY_NO**. 새포즈가연결됐다는사실을침끝접점/관절보간/전투완성으로확대하지않습니다.

[실제Unity주요키크롭](Evidence/Issue63ApprovedV3Delta20261104/Unity_V3_Connection_Actual_Keys.png)·[실제Unity영상](Evidence/Issue63ApprovedV3Delta20261104/Unity_ApprovedV3_Attack_ReturnIdle.mp4)·[Checks15](Evidence/Issue63ApprovedV3Delta20261104/UnityEvidence/Checks.txt)·[126Trace](Evidence/Issue63ApprovedV3Delta20261104/UnityEvidence/Trace.json). 영상저장20FPS와소스키포즈교체10FPS를구분합니다.

## 외부원본 권리 별도 조사

[상세권리조사·원문출처·표시안](Evidence/Issue63ApprovedV3Delta20261104/RightsInvestigation.md).

- [현재OpenGameArt페이지](https://opengameart.org/content/pilotbeegamecharacter)는CC-BY3.0/2DPIXX를표시하고,로컬보존원본ZIP내LICENSE.TXT는CC BY4.0/Jana Ochse·2D!PIXX입니다. ZIP내LICENSE와옆파일바이트동일확인. **버전충돌미해결RIGHTS_HOLD**이며자동듀얼라이선스/4.0업그레이드로해석하지않았습니다.
- [CC BY3.0공식](https://creativecommons.org/licenses/by/3.0/)·[CC BY4.0공식](https://creativecommons.org/licenses/by/4.0/)은둘다조건준수시상업사용·수정·배포를허용합니다. 제작자·원본출처·올바른버전링크·제공고지보존·수정표시·후원오인금지·허용권제한금지등의조건을정리했습니다. 상업사용자체가금지된것은아니지만이파일의적용버전/표시결정을해결한것은아닙니다.
- 현재MonsterDefinition의Idle/Attack GUID가원본과바이트동일한프로젝트복사본을참조합니다. 기존README의검증전용/미연결서술과충돌합니다. 문서/코드를임의로수정해맞추지않고 [ActiveSourceReadOnly](Evidence/Issue63ApprovedV3Delta20261104/ActiveSourceReadOnly.json)에보고했습니다.
- 신규Sting/Recover의별도생성원화선언과바울대표미술승인은기존외부원본의라이선스를해결하거나새PNG의독립권리/비침해/독점소유를법적으로보증하지않습니다. 새SHA는파일식별증거입니다. 기존Prepare/Idle2의KEEP와외부원본사용권도구분했습니다.
- 저작자에게보낼버전/표시확인질문범위를정리했지만실제연락은발송하지않았습니다. 확인된권리자답변또는공식정정자료와배포크레딧정리전RIGHTS_HOLD를유지합니다.

## 보호·남은 적용조건

기존153관련출력·실시간보충근거·이전입력·외부원본·사용재생코드·이전비교HTML **70파일전후SHA불변**,승인/KEEP입력 **5/5 SHA불변**. [보호결과](Evidence/Issue63ApprovedV3Delta20261104/ProtectionAfter.json). 개인Save/UserData접근0,원본Unity/Editor/Scene/Prefab/MonsterDefinition/GUID/meta/기존외부에셋교체0. 승인Owl/Spider/VFX/배경과사용자미커밋수정0. 원본main634a91d3e4174c87761c0d377cfa5a695ca79e7f·tracked dirty308유지·포커스조작0.

남은게이트: **①PilotBee버전·귀속표시/전체자산출처정리 ②실아군표적침끝/행동위치표적QA ③반복키포즈구성의제품채택판단(독립중간프레임완성아님) ④별도원본적용승인**. 두승인포즈를다시미술대기로돌리지않고원본적용/외부교체만HOLD합니다. 이번원본게임적용·20프레임교체·새작화·원격Push·병합0.

브랜치 `codex/issue63-v3-approved-connection-rights-20261104`,직전59290d941f473e265bc12ffe4177a215e24295b5. CODEX_LATEST/CURRENT_STATUS갱신,실제commit은Issue63댓글기록. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 로컬승인2PNG/캡처를보존했으며원격에는댓글·경로·SHA만보고합니다.
