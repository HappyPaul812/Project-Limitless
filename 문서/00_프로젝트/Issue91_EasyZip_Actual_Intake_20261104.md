# Issue #91 ch3_bg_v2_easy.zip 실제 납품물 검수 — 2026-11-04

**EASY_ZIP_CRC_SUMS_PASS / PAYLOAD_MATCHES_REPORTED_V2 / MANIFEST_NOT_INCLUDED / CH1_V2_COMPARISON_MISSING / USER_VISUAL_REVIEW_PENDING / UNITY_NOT_TESTED / NOT_UNITY_APPLIED**

최신 기획 [6623743094](https://github.com/HappyPaul812/Project-Limitless/issues/91#issuecomment-6623743094), 실제v2 납품 [6624133353](https://github.com/HappyPaul812/Project-Limitless/issues/91#issuecomment-6624133353), `docs/art-new` 보고4222f194cc040dff2422b7c6ef822ed6bdf683ae·인계fef60f8dfd0b79d89602521c7a1f4a886a15bd3d를 확인했습니다. 이번에는 파일 접수 검수만 수행하고 새 배경 제작·새 화풍 비교판 제작·Unity 실행을 하지 않았습니다.

## 실제 ZIP

- 경로 `F:/Downloads/ch3_bg_v2_easy.zip`
- 실제3,944,727bytes·5 entries. 전체SHA256 **`0843afdbce6c1e6dcb1117240eaafbc33e894f126f9975ba46e18da8f04ef292`**.
- CRC `testzip` 전체5엔트리 PASS(오류엔트리없음).
- `SHA256SUMS.txt` 4개PNG 전부포함·실파일4/4 SHA일치. SUMS자기자신은목록대상아님.
- **Manifest.json 없음 → MANIFEST_NOT_INCLUDED / 별도Manifest대조 불가**. SUMS통과를Manifest통과로 간주하지 않습니다.
- README/SourceDeclaration/QA/개별ROI전후판도 이 간편ZIP에는 없습니다. 이검수자가 만든 IntakeQA.json은 원본 납품Manifest가 아닙니다.

정식v2ZIP은7,880,511bytes/24entries/SHA cccb5b92c6fb319ffb7a2c1da213634db05f99c6150277106732738a20b37181로 보고되어 있습니다. **이번5파일ZIP은 다른 컨테이너이므로 전체ZIP SHA가 다른 것은 불일치 오류가 아닙니다.** 간편ZIP의 별도기대SHA는최신GitHub기록에없어 새 실측값으로접수합니다. 정식24개ZIP 전체를 로컬에서열어대조했다는 주장은하지않습니다.

## PNG 규격·실제 SHA

|파일|규격|bytes|SHA256|
|---|---|---:|---|
|Snowroad_v2.png|1672×941 RGB|711,832|339beeb80d313c589cf50842fc9f6964049b0752e783d83ac8a7097c768205ab|
|Forest_F2_v2.png|1672×941 RGB|772,045|4d57949a1a9d2ae5600d4ba1b95989153fed6a0d8f47f8fc52e6f670a013535a|
|Before_After.png|1920×1290 RGB|1,155,827|67d974ece23be95b60535695b63d1cb1d2a35cc9e62eabb3fd49a10b7827c912|
|Battle_UI_Preview.png|1320×1580 RGB|1,325,594|2e6ed87fc8945bfffff0dfacefd2b3b267ac4fc5443b58cda6304a6af19135b9|

배경2장은1672×941 RGB 불투명정적PNG이며 명칭만간소화됐습니다. 디코드4/4성공. Before_After/Battle_UI_Preview는 게임용배경이아닌검토용이미지입니다.

## 기존v1·보고v2 대비

두 배경은 **기존 정식v2보고PNG와 SHA 각각정확일치**합니다. 따라서 이번간편ZIP에 새v3미술변경은없고,파일명/패키지구성축소입니다. 원격보고해시대조이며 정식ZIP전체검수와구분합니다.

실제로보존된v1로컬PNG2개와동일1672×941 RGB화소비교:
- Snowroad v1 SHA a5f3bb960b5c5e6fe4140cb2de64e726f4bc7fabb8674f895789fed128adcd25 → v2 339beeb8… . 변경455,966px/28.9805%,bbox[651,22,1318,939]. S1[650,20,1320,475]·S2[700,535,1180,941] **밖변경0**.
- F2 v1 SHA93388ee52fd75da4a82df0b68e03b2083167a25f339d97b41dc71c807125282b → v2 4d57949a… . 변경470,950px/29.9329%,bbox[1,1,1671,924]. F1L[0,0,480,330]·F1R[1240,0,1672,340]·F2[1310,240,1640,925] **밖변경0**.

좌표는원본좌상단기준,우·하단제외입니다.


동봉전후판을실제로열어확인했습니다. Snowroad 원경구름·산면/근거리눈길,Forest 수관·우측수간에질감·국소명암을보완했고기존구도는유지됩니다. 이는지정범위표적수정확인이며Chapter1화풍동등PASS나최종미술승인이아닙니다.

## 가독성·화풍 비교 자료 존재 여부

|요구자료|실제접수|한계|
|---|---|---|
|v1/v2 두지역전후판|Before_After.png 있음|축소OFFLINE전후,원배율ROI전후판은간편ZIP에없음|
|정식방향 배치참고|Battle_UI_Preview.png 있음|적왼쪽2·아군오른쪽3,명령패널·3인HP/MP 모조도식. 실제Sprite/정식BattleUI/Unity캡처아님|
|실전아군3·적·HUD가독성|없음|모조실루엣은복잡한외형·휠체어·상태명/중첩/VFX/팝업을대신검증하지못함|
|기존Chapter1와새v2동일배율화풍비교|없음|동봉전후판에는Ch1패널없음. 별도기존F:비교판은Ch1-v1기준이라v2직접비교PASS가아님|

UI자료에서아군후열은오른쪽큰나무/눈쌓인가지앞에있습니다. 오른쪽배치참고가있다는요구는부분충족하지만,최종실Sprite윤곽/상태정보가독성은미확인입니다. 실제게임960/1280/1680검증자료는없습니다. 자료에적힌OFFLINE/실제UI아님표기를유지하고본편실행근거로승격하지않습니다.

기존 F:/Downloads/LIMITLESS_Issue91_Ch1_Ch3_1672x941_1to1_Comparison_20261104.png(당시SHA e10aa647…)는 별도기존Ch1-v1근거로존재하며이번간편ZIP에는없습니다. 이번납품검수범위를넘어새Ch1-v2비교판을추가제작하지않았습니다.

## 승인 필요와 다음 경계

**바울 대표의 Snowroad/F2 신규v2 최종미술승인 필요(미승인)**입니다. 파일기술검수통과와미술승인을구분합니다. Manifest/출처문서/ROI판/Ch1-v2직접비교자료의누락을명시한접수상태이며완전한납품계약충족으로보고하지않습니다. 기존정식v2보고는외부근거로연결하되간편ZIP에없는파일을들어있다고쓰지않습니다.

다음은누락문서/Ch1-v2직접비교와바울대표최종미술판단입니다. Unity6000.6.5f1격리실UI/실아군3·적/3화면조건QA는승인후별도작업,본편적용은다시별도승인입니다. 이번Unity실행/Import/Scene/Prefab/Catalog/GUID/meta수정0.

## 보호·기록

기존Ch1공식3·C3v1후보2의기존InputManifest SHA와현재실파일5/5동일. 승인Attack8바이트/기존VFX·기존QA·개인Save/UserData·원본Unity·사용자미커밋파일수정0. 새파일은Temp와문서worktree에만작성했습니다. 원본main634a91d3e4174c87761c0d377cfa5a695ca79e7f·tracked dirty308유지,원본Editor/포커스조작0.

[실제검수JSON](Evidence/Issue91EasyZipIntake20261104/IntakeQA.json),[기존5장보호](Evidence/Issue91EasyZipIntake20261104/PriorInputProtection.json),[동봉전후판사본](Evidence/Issue91EasyZipIntake20261104/Before_After.png),[동봉OFFLINE배치자료사본](Evidence/Issue91EasyZipIntake20261104/Battle_UI_Preview.png).

브랜치 `codex/issue91-easyzip-intake-20261104`,직전c43d9b1b8a109fc18d5898d15f331cc9997cad9c. CODEX_LATEST/CURRENT_STATUS갱신. 실제commit은Issue91댓글에기록합니다. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. main직접commit/Push/병합0. 바이너리는로컬기록만하며GitHub첨부/Push하지않습니다.
