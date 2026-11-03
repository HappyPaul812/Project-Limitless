# Issue #82 — 승인 그루터기 Unity 표시 Delta + Spider/Bee 새ZIP 검수 / 2026-11-03

**STUMP_USER_APPROVED / STUMP_ISOLATED_UNITY_PASS / SPIDER_ALPHA_CANDIDATE / BEE_MOTION_HOLD / NOT_ORIGINAL_APPLIED / REMOTE_PUSH_NO**

## 결론·다음 단계

- 승인 그루터기1PNG: Unity6000.6.5f1 독립 TestStage **21PASS/FAIL0**, **STUMP_ISOLATED_SPRITE_DISPLAY_PASS**. 실제 F2 Scene/길/Collider가 없어 **REAL_F2_ACCESSIBILITY_NOT_VERIFIED**. 표시 검증 통과를 본게임 적용 완료로 승격하지 않습니다.
- Spider v5: 최신 보고SHA 일치, RGB262144전체보존·alpha380감소 확인. 이전 길쭉한 밑띠/큰단차가 해소되고 다리윤곽이 이어져 **ALPHA_FINAL_OK_CANDIDATE / 최종미술승인대기**. 기존 디자인 재심사0, 새32프레임 양산은 보류.
- Bee Attack3: 최신3SHA·실제새변경일치. **NEW_ATTACK3_DELTA_CONFIRMED**, 그러나 실제 표적/접촉점·회수연속성은 **HOLD**. 실제 Sting PNG의 침끝은 **우하향으로 관찰**되어 담당 최신 댓글의 '좌측후방독침' 설명과 차이가 있습니다. 이 차이를 보고하고 실제 공격축/Slot 계약을 확정하지 않았습니다. 새20양산/게임교체0, CC BY3/4 **RIGHTS_HOLD** 유지.

## 실제 ZIP와 PNG SHA

- `spider.zip`: **1,068,746bytes / 13entries**, SHA256 `7f4622af9c16e143aed7a490722a8d8b88d667964b2f3267611581b59fa0de5c`. CRC PASS·안전경로PASS·중복0. Manifest/SUMS 동봉. 개별ZIP 포함분은 Manifest **11/27 일치**·SUMS **12/28 일치**, 미동봉 참조는 각각16/16개입니다.
- `bee.zip`: **1,380,348bytes / 20entries**, SHA256 `a396a2be0249602e0532f6d0e22e11dad5437c728d1fab7abfe038dcf734a164`. CRC PASS·안전경로PASS·중복0. Manifest/SUMS 동봉. 개별ZIP 포함분은 Manifest **18/27 일치**·SUMS **19/28 일치**, 미동봉 참조는 각각9/9개입니다.

두 분리ZIP에 같은 합본용 Manifest/SUMS가 실려 있습니다. 개별 ZIP 전체 Manifest가 PASS라고 뭉뚱그리지 않습니다. 포함된 파일의 불일치0, 두 ZIP 합집합의 동명 파일 충돌0, **합집합 Manifest27/27·SUMS28/28 PASS**입니다. 담당의 합본ZIP 자체는 로컬 미확보로 전체ZIP 동일성은 NOT_VERIFIED이며 내부 PNG SHA만 확정합니다.

- Spider `ForestSpider_Organic_v5_FORELEG_CONTOUR_ALPHA_512.png`: `c600cd2264f6a0323771288666d16b49aa213ad938036d0746ed6f7f1d50adb9`.
- Bee `Attack_Prepare_REPAINT_v2_315x253.png`: `e337921236b7b947f92bbb5d88824bc6729d6668a050a85fc2f519ff4df26212`.
- Bee `Attack_Sting_REPAINT_v2_315x253.png`: `391ed94e50ffc4ddcf3bc4442e9913e0b947cc9d5d9a57aaf865f46556e021ce`.
- Bee `Attack_Recover_REPAINT_v2_315x253.png`: `2afc69e803334b1ededdadb39e786f297d8fe3d9918e72997943c9f1593c740e`.
- 승인 그루터기: `22f2f5816b3fd2ffe45234ec807bb3d81a83f87440eb8eb1f338e0dff33404a8`, 승인 원본과 fixture 복사본/import후 모두 동일.

새PNG/GIF는 실제 decode·크기·RGBA/alpha/bbox 확인. Spider512² RGBA, Bee315×253 RGBA. 보조128/40 이미지와 RGB비교판은 별도 기록, GIF는 불투명 팔레트 프리뷰입니다. SHA같은 Spider v4/옛 Attack3/정상Idle2는 기존 QA 재사용하며 delta 비교/표시 로드만 했습니다. 기존20 전체 재검수0.

## Spider v5 실제 경계 검사

v4 reference `c9eedca2...`는 이전 납품과 바이트 일치. v5는 RGB262,144픽셀 변경0, **alpha380픽셀 감소·증가0**, bbox `[23,341,73,389)`로 보고된 윤곽 마스크 범위 일치.

밝음/어둠/실제 Field03 바닥 `LL_C1_F03_ForestFloor_A_128_v1.png` 반복배경에서512/128/40과 동일 앞다리 영역4배 최근접 확대를 비교했습니다. v4의 긴 회백색 밑띠·눈에 띄는 직선 단차가 줄고 하단 다리/발끝 윤곽이 자연스럽게 이어집니다. 작은 털/반투명 경계는 최종 시각 판단 대상이지만 이전 큰 잔상 문제는 개선됐다고 보아 **ALPHA_FINAL_OK_CANDIDATE**로 보고합니다. 미술 승인/본게임32프레임 제작이 자동 승인된 것은 아닙니다.

실제 Field03 지면 PNG를 썼지만 조명/Sorting/Scene 재현은 아닌 **OFFLINE 합성**입니다. 기존 유기형 디자인/RGB·메카닉v3 Chapter4 ARCHIVE_ONLY는 보존했습니다.

## Bee 최신 Attack3 — 개선 사실과 미해결 분리

이전 New5 Attack3를 OLD_KEEP에 정확히 보존했습니다. 새3장은 담당 보고의 실제SHA와 모두 일치하고 디코딩픽셀도 다릅니다.

|포즈|RGB변경|alpha변경|RGBA union변경|
|---|---:|---:|---:|
|Prepare|36,978|32,618|37,807|
|Sting|31,147|28,249|31,970|
|Recover|38,001|33,864|39,037|

- Idle Folded/Spread2개는 이전 정상포즈 SHA와 정확히 일치합니다. 기존20은 이번ZIP에 없으며 원본을 수정/재생산하지 않았습니다.
- 복부 뒤쪽에 침이 연결된 형태는 유지합니다. 최신 Sting은 복부를 아래/앞으로 굽혀 **침끝이 화면 우하향으로 보입니다**. 이전 왼쪽으로 길게 뻗은 침과 다릅니다. 원본391ed94e…를 직접 확인한 관찰이며, 담당 보고의 '좌측후방독침' 문구와 불일치하므로 임의로 동일 판정을 반복하지 않았습니다.
- 우측 표적을 향한 자세로 해석할 가능성은 있지만 **실제 Battle Slot·표적 위치·투사체/몸통 이동·접촉 타이밍이 연결되어 있지 않아 실제 타격정합성은 NOT_VERIFIED**입니다. 이미지에서 오른쪽 성분이 보인다는 사실을 ATTACK_AXIS_APPROVED로 승격하지 않습니다.
- Prepare→Sting→Recover에서 몸통/복부·다리/날개 자세 차이는 커졌습니다. 그러나3키포즈 급전환으로 침 회수·날개뿌리 회전·관절 연속성이 완성됐다고 판단하기 어렵습니다. 40px에서 굽은 복부와 침의 방향 변화는 보이나 접촉점/침끝은 작고 몸통과 합쳐져 혼동될 수 있습니다.
- 동봉 GIF **3프레임·각100ms=10FPS·loop0**, 합성 RGB거리와 실프레임으로 Prepare→Sting→Recover 순서 일치 확인. 원본 GIF와 PNG순서제어는 비교판에서 별도 표시했습니다. **MOTION_CONTINUITY_HOLD / REAL_TARGET_NOT_VERIFIED / FULL_COMBAT_READY_NO**. 후속은 실제 공격축/표적계약·필요한 연결/회수만 정리하고 전체20 양산으로 확대하지 않습니다.
- PilotBee CC BY3.0 웹/CC BY4.0 패키지 출처불일치 RIGHTS_HOLD 계속 유지.

## 승인 그루터기 독립 Unity Delta — 실제 실행

기존 프로젝트에서 F2 Scene 검색결과 없음/#80 F2미구현 근거와 일치. 공식 F2맵을 만들지 않고 `Temp/Issue82StumpSpiderBee20261103/QA/Client`에 **승인PNG1개와 Editor 시험스크립트만** 넣은 최소 Built-in fixture를 사용했습니다. 게임 코드/씬/저장/UI를 복사하거나 Play하지 않았습니다. 기존6000.6.5f1 QA ProjectVersion을 새fixture에 바이트복사했으며 원본·후보worktree의 ProjectVersion·Packages·BuildSettings 변경0입니다. 새fixture의 최소 built-in module manifest만 사용했습니다.

- 실행 Editor: **6000.6.5f1 / 3ff58d469c8a**, 숨겨진 batch D3D11. Unity MCP도구는 세션에 없어 원본Editor 연결/화면조작0.
- 실제 TextureImporter Sprite single, FullRect, alphaIsTransparency, Point, mipmapOFF, Uncompressed, NPOTNone를 설정해 Import했습니다.
- **PPU128/Scale1은 테스트 후보**. 기존 NorthF1 FrostShrub/Waymarker importer PPU128/Point를 읽고 범위를 잡았습니다. 원본320×384의 전체사각형은2.5×3WU, 가시alpha bbox(texture bottom origin) `[8,11,312,243)`입니다.
- 실제 보이는 최하단에 맞춘 custom Pivot **(0.5,11/384≈0.028645834)** 후보. 가시높이 **1.8125WU**, ortho5/화면높이720에서 **130.5px**. 투명여백이 아니라 발의 보이는 끝을 y0 시험선에 맞춰 접지 확인했습니다. 정식 F2 Pivot/Footprint 확정은 아닙니다.
- 1280×720 / 960×720 / 1680×720 모두 화면 안에 전체Sprite가 보이고 같은 가시높이로 정상 표시됩니다. 기존미승인 표지판/울타리/Spider/Bee Import0.
- SortingOrder -10/+10의 파란 **직사각형proxy**를 앞뒤로 겹쳐 실제 가림픽셀 변화를 확인했습니다. 이는 정렬 기능만의 검증이며 Player/휠체어/자동Y-sort/실제시야가림 PASS가 아닙니다.
- 대안 PPU256/Scale1은 크기가 절반, PPU128/Scale.75는1.875WU가 되어 후보128/1과의 차이를 캡처했습니다. 후보128/1은 독립표시에서 적합하지만 정식환경비례는 실제F2준비후 결정해야 합니다.
- 중립1WU격자 TestStage에서 alpha/바닥접지 확인. 기존 F2 SnowPine/FrostShrub 참고와는 공통PPU128 가정의 **OFFLINE 동배율 색감/크기**만 비교했습니다. 청백색눈·갈색수피의 큰불일치는 없지만 이참고PNG들을 Unity에 Import하거나 실제 F2배치PASS로 보고하지 않았습니다.
- **최종 신규21PASS/FAIL0**. 첫 실행에서 Unity6.5의 TextureImporter에 없는 spriteAlignment를 사용한 QA스크립트 컴파일1오류→TextureImporterSettings로 수정후 재실행21PASS. 초기로그 보존, 최종script컴파일오류0·시험예외0. 기존 Main24/저장/Main23QA 호출0.
- 실제 길/Collider/Bounds가 없어 **도보·휠체어 통행·실F2 Sorting/시야는 NOT_VERIFIED/SCENE_MISSING**. 실제Player를 넣거나 임의맵을 만들어 PASS 처리하지 않았습니다.

## 비교판·실제 캡처 구분

단일 결과 비교판: `F:/study/codex/Project-Limitless/Temp/Issue82StumpSpiderBee20261103/Review/index.html`

HTML SHA256: `730c8481819d6dcbefdb3b6f772e3f6914d7f37358169f85b11414ba1de4a748`.

비교판 안에서 **OFFLINE Spider/Bee/환경 비교**와 **실제 Unity Camera RenderTexture 캡처**를 제목부터 구분했습니다. 실제 캡처도 OS GameView 스크린샷/본게임F2/손플레이 화면은 아닙니다. 브라우저 이미지/의존성·두순서3/3·제어/배경 기능검사 **14PASS/FAIL0**.

실제 Unity 캡처 로컬 경로: `F:/study/codex/Project-Limitless/Temp/Issue82StumpSpiderBee20261103/QA/UnityEvidence/`

- `PPU256_Scale1_1280.png`: `5514d4ca2bc90cb07b9ce9abd3493197b2e86201e8096f10f1443b2596695871`
- `Scale0_75_1280.png`: `05b89d4fdd8cb5f76e24d7486a09c4fe76b1f68f4ca521b164eff69bdbd8cb3d`
- `Sorting_ProxyBehind.png`: `8b041b12f24429b352d0f4a0b783f10a85ce31816356240b437fa4b59a39627c`
- `Sorting_ProxyInFront.png`: `aa576fc0b10400699681101c9375982d5e2895c5607fb94ba50cabdd1848fcaf`
- `Stump_PPU128_Scale1_1280x720.png`: `1255f0ddfe3ba2922e85a89084e0992573797aac463a04e665473c1a3241351d`
- `Stump_PPU128_Scale1_1680x720.png`: `f43aef4dbc50a0059ec0e8f261d921b4e95cc4803bb2f2fcafb2b6ceb5f26ade`
- `Stump_PPU128_Scale1_960x720.png`: `5e89c72023c555ccb3454bdc06c7065b236f46b9f2c9916d7fa2f7c97470701a`

재현 스크립트: `Tools/QA/Issue82StumpUnityDelta.cs`는 제품Assets밖에 보관합니다. 해당 독립QA 경로·batchmode만 허용하고 승인SHA/Unity버전을 검사합니다. 같은새fixture의 `Assets/Editor/`에 복사하여 `Unity.exe -batchmode -projectPath <repository>/Temp/Issue82StumpSpiderBee20261103/QA/Client -executeMethod Issue82StumpUnityDelta.Run -logFile <QA로그> -force-d3d11`로 실행합니다. 게임원본에서 실행금지. 승인PNG 외 아트Import0.

## 보호·문서화

기록한 입력·참고/생성캡처 **45파일 SHA 변화0**. 원본main `ab47b931fec92976170f9f50aabde1822d78bb90`/status344지문동일, 기존Main24 HEAD `fd869f60c9fde438a3dd97885cb6f447efc4454b` 유지. 개인Save/UserData 열람·복사·해시·수정0. 미승인F2표지판/울타리·Spider/Bee 본게임교체0, Scene/Prefab/BuildSettings/Packages/ProjectVersion·Discord#66·원본Editor/포커스변경0, main직접commit/Push/merge0.

브랜치 `codex/issue82-stump-spider-bee-delta-20261103`, 출발점 `f820208df2c3e132bfa6bad6eb5c5a9f2aed7fa3`. 보고서·CODEX_LATEST·CURRENT_STATUS와 제품Assets밖 격리QA C#만 로컬 커밋합니다. PNG/HTML/JSON/fixture/log는 Temp에 보존하고 원격Push하지 않습니다. 원격에는 Issue결과댓글만 실제 게시합니다.

근거: `Audit.json`, `InputSHA256.json`, `GIFOrder.json`, `ProtectionAfter.json`, `UnityCaptureSHA256.json`, `QA/UnityEvidence/checks.txt/spec.txt/status.txt`, `Review/BrowserQA.json`. 원시 Unity로그는 라이선스정보/로컬경로가 있어 로컬에만 보존합니다.

다음: 승인그루터기는 실제F2 Scene/길이 준비된 뒤1PNG 배치·시야·통행만별도검증/적용, Spider는v5최종알파미술승인 후다음제작범위결정, Bee는실제표적/공격축계약과회수연결보완·권리충돌해결이먼저입니다. 이번 결과를32/20프레임양산/게임교체/새F2구현승인으로확대하지 않습니다.
