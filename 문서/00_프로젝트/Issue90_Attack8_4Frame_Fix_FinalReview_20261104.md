# Issue #90 Attack8 수정 4프레임 최종 집중 검수 — 2026-11-04

**TECH_PASS / USER_FINAL_ART_APPROVAL_REQUIRED / NOT_UNITY_APPLIED**

## 기준과 범위

최신 납품 [6623739390](https://github.com/HappyPaul812/Project-Limitless/issues/90#issuecomment-6623739390), 기획 [6623743727](https://github.com/HappyPaul812/Project-Limitless/issues/90#issuecomment-6623743727), 납품 문서 ac917c83544003799cabe97ac6ed4052abee64ec를 확인했습니다. 사용자 지시대로 수정02/04/05/07과 승인00/03/06·유지후보01의 보존, 전체10FPS 및07→승인Idle00만 검증했습니다. Owl/Walk/기존37PASS 및99PASS 전수검사는 호출하지 않았습니다.

실제 F:/Downloads/LIMITLESS_ATTACK8_ISSUE90_TARGETED_4FRAME_FIX_20261104.zip 전체 SHA256을 1회 계산: `58c51cb49d63cde4669dc183da8583701e7e0bc9dceae9fae5542a5573f0fc9d` — 보고값과 일치. ZIP CRC/Manifest 전수 반복은 하지 않았습니다.

|프레임|실제 SHA256|이전 납품 대비|
|---|---|---|
|00|439ef26e4528e9f57877b471f602f9cfaa2f7bcd2a86b2fce991ec2e17ae5bf2|불변|
|01|5473314b0970a7a8e99f9e39f8e04f1fb7997ab0835ceb48db947071a19a4fe7|불변|
|02|85ca201cecb4b51acad26b39f5146fe6637c8408dd08b02eb2723f0c65193622|수정|
|03|0d0e24c2fae579f2f944255fe3bd3841216299b14f439cc3612906269020f697|불변|
|04|3ccdfa06022ea900c60857211db81042f44489719e6235a886c60d351d5ead7a|수정|
|05|7ed984d56c41f284ad8fc8646275d674f7a6e533649b41644f0baa5c42d15bde|수정|
|06|0501a452467a07f34688abdfff5ea5ef49b0dfd93e1c99bdc368229986d44096|불변|
|07|35af96b928108206488b1b27bdcf607d3f9f890bc0d3b7df2a16d4a932a6955e|수정|

승인00/03/06과 후보01은 바이트 동일입니다. 01을 새 승인으로 승격하지 않았습니다. Idle00 SHA `c600cd2264f6a0323771288666d16b49aa213ad938036d0746ed6f7f1d50adb9`.

## 수정4장 집중 관찰

좌표는512×512 소스, 우·하단 미포함입니다. 원본PNG 수정 없이 읽기만 했습니다.

- 02 변경bbox [343,221,501,405]: 오른쪽 관절/발 끝 실루엣 연결을 보완. 03 승인 포즈까지 오른쪽 방향 유지. 완전한 해부학적 관절 자연스러움을 자동 통과로 판정하지 않습니다.
- 04 변경bbox [393,226,497,317]: 이전 [491..496,278..286]의 떨어진 작은 알파 잔여 제거 확인.
- 05 변경bbox [78,136,370,355]: 이전 왼쪽 [78..86,308..330] 작은 조각 제거. 몸체까지 넓게 이동한 수정이며 04→05의 몸체 높이/다리 벌림 변화는 실제128px에서도 남아 있습니다. 대표님 최종 모션 판단 필요.
- 07 변경bbox [362,216,475,337]: 이전 [459..461,309..314] 고립 조각 제거. 연결된 갈고리 모양 오른쪽 다리와 Idle00 전환 시 다리 벌림·몸체 외형 변화는 남은 미술 판단입니다.

네 장 모두 alpha>8,8연결 기준40px 이하 고립 성분0입니다. 알파 연결성은 관절 구조/접지 정확성의 증명이 아닙니다. 40px에서도 재생되지만 관절 세부는 축소되어 원본512px ROI와128px 캡처로 함께 검토했습니다. 새 PNG 제작·리터치하지 않았습니다.

## 실제 Unity 재생

격리 경로 `F:/study/codex/Project-Limitless/Temp/Issue84OwlSpiderUnityDelta20261103/QA/Client`. Unity6000.6.5f1(3ff58d469c8a), 배치 PlayMode. 원본 Editor/MCP 장면은 조작하지 않았습니다. 신규 Assets/Issue90Input 및 Issue90GeneratedRun1만 사용했습니다.

10FPS·8키·비루프·0.8초 Clip,PPU256,중심pivot,Scale0.7,오른쪽 방향. 실제 Animator Attack→Idle(exitTime1,transition0)로 전후 각96샘플/총192샘플을 기록했습니다. **16PASS/0FAIL**, 새 런타임 오류0. Editor SearchDatabase 초기화 ArgumentOutOfRangeException은 환경 오류로 별도 보존했으며 제품 무오류 판정에 섞지 않았습니다.

수정후 실제 최초 관측:00=0,01=.0875,02=.1875,03=.2875,04=.3875,05=.4875,06=.5875,07=.6875,Idle00=.7875초. 첫 샘플 재시작 위상12.5ms 차이가 있고 이후 프레임 유지100ms,0.8초 경계 허용±25.1ms 안입니다. 전본은0/.1/…/.7/Idle=.8초. 잘못된07→Attack00 루프 없이 승인Idle00로 전환했습니다. 마지막07 포즈와 Idle00 외형 차이를 보간해 숨기지 않았습니다.

카메라 실제 RenderTexture 캡처180PNG(전후Stage640×360,40px/128px native+고유9키). [실제 전후128px](팀운영/인계/Evidence/Issue90Attack8FixReview20261104/Unity_128px_BeforeAfter_00to07_Idle00.png), [수정후 실제Unity 동영상](팀운영/인계/Evidence/Issue90Attack8FixReview20261104/Unity_After_Attack8_ReturnIdle00.mp4), [Checks](팀운영/인계/Evidence/Issue90Attack8FixReview20261104/UnityEvidence/Checks.txt), [Trace](팀운영/인계/Evidence/Issue90Attack8FixReview20261104/UnityEvidence/Trace.json), [전체 증거SHA](팀운영/인계/Evidence/Issue90Attack8FixReview20261104/SHA256.json).

동영상은20FPS로 저장한 실제Unity 캡처이며 소스 애니메이션10FPS입니다. 루트 고정만 검사했고 실제 FootLock·표적 Contact·게임 피격 판정은 미검증입니다. 본편 전투/전체 QA를 실행하지 않았습니다.

## 보호·인계

기존 fixture 입력/기존 QA 증거270파일 SHA 전후 불변. 원본 Unity/Save/Scene/승인 아트 미변경. 원본main HEAD634a91d3e4174c87761c0d377cfa5a695ca79e7f·tracked dirty308개 유지. 작업브랜치 `codex/issue90-attack8-4frame-review-20261104`; 직전4206e4f267860ed1fcfd65acc166f8846e2f70e3, 결과commit SHA는 Issue90 실제 댓글에 기재합니다. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. Push/병합0.

**바울 대표님의 최종 미술 승인 필요**: 수정02/04/05/07의 관절 자연스러움,04→05 회복 변화,07→Idle00 복귀 실루엣. 기술 재생 통과와 사용자 미술 승인을 구분합니다. 승인00/03/06 재제작 금지,01은 유지후보 상태 유지. 원본게임 미적용 상태에서 최종 판단을 요청합니다.
