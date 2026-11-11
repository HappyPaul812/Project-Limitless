# Issue #84 — Owl Attack4 + Forest Spider Walk8 실제 Unity 애니메이션 Delta / 2026-11-03

**OWL_CLIP_DELTA_PASS / SPIDER_WALK_CLIP_DELTA_PASS / ACTUAL_GROUND_LOCK_NOT_VERIFIED / WALK_CONTACT_HOLD / USER_FINAL_ART_REVIEW_PENDING / NOT_ORIGINAL_APPLIED / REMOTE_PUSH_NO**

실제 Unity 6000.6.5f1 PlayMode에서 **AnimationClip과 AnimatorController**를 생성해 재생했습니다. **신규 37 PASS / 0 FAIL**이며, 실제 Animator/Sprite 시간 추적 504개 샘플과 Camera RT 캡처로 확인했습니다. Spider의 클립과 외곽 표시 기술은 통과했지만, **독립된 8다리의 실제 지면 접지와 미끄러짐은 HOLD / NOT_VERIFIED**입니다. 기술 통과와 바울 대표의 최종 미술 승인·제품 교체는 별개입니다.

## 1. 최신 기준·입력·격리

GitHub최신공통규칙·CODEX_LATEST(main의과거스냅샷과실제로컬최신구별), #60/#74/#83/#84 본문·최신댓글을읽었습니다. #84 명시브랜치 **`codex/owl-spider-unity-animation-delta-20261103`**, 출발직전후속검수 **`3d61b4912344d3e411fbf76076654eb1849a6efa`**입니다.

- Owl ZIP `F:/Downloads/Limitless_FrostOwl_Attack02_Continuity_20261103.zip` SHA **d6648b7451dc2184c2187c0b1c2f8e0f52fc800ac6975753d0f6d38f6fbf9192**.
- Spider ZIP `F:/Downloads/LIMITLESS_WALK8_EDGE_FIX_20261103.zip` SHA **771bd1813608aa3e350c6124ab0d43293c6d2988b1833014da64484d5272c8da**.
- 둘다이전검수SHA와동일하여CRC/Manifest/PNG·GIF품질QA를반복하지않았습니다. 코어PNG12+승인참조2만검증fixture에복사했고Import후파일SHA도그대로입니다. Owl승인Idle00 **81a31cf3…**, Spider승인v5/Idle00 **c600cd22…**의전체SHA는Input/보호근거에기록했습니다.
- fixture: `F:/study/codex/Project-Limitless/Temp/Issue84OwlSpiderUnityDelta20261103/QA/Client`. 기존#82 최소fixture의Library캐시만재사용, 이전Assets/그루터기QA/게임코드·Save/UserData는복사/로드/실행0.
- 새fixture에만Animation/Director/ImageConversion/JsonSerialize 기본모듈4개와기존6000.6.5f1 ProjectVersion을사용했습니다. 원본/작업브랜치ProjectVersion·Packages·BuildSettings는변경0. 구버전설치0. 원본Editor/OS포커스변경0.
- UnityMCP도구미노출을확인하여기존Editor `E:/Program Files/Unity/Hub/Editor/6000.6.5f1/Editor/Unity.exe`를숨김batchmode로실행했습니다. 원본프로젝트Editor를조작하지않았습니다.

## 2. 실제 Clip·재생 검증

| 항목 | Owl Attack4 | Spider Walk8 |
|---|---|---|
| 실제Asset/Curve | AnimationClip/SpriteRenderer.m_Sprite,키4 | AnimationClip/SpriteRenderer.m_Sprite,키8 |
| 실제속도/길이 | 8FPS /0.5초 | 10FPS /0.8초·loopTime=true |
| Import | PPU512,중앙Pivot(.5,.5),Point,FullRect,Uncompressed | PPU256,중앙Pivot(.5,.5),Point,FullRect,Uncompressed |
| Scale/방향 | Scale1 | Scale.7,flipX=false,정의sourceFacesRight=1,+X이동 |
| 실제재생순서 | 00→01→02→03→Idle00,2회 | 00~07 반복,각속도에서2회완료+다음00진입 |
| 실표시크기 | 40px·128px캔버스실제투영PASS | 40px·128px캔버스실제투영PASS |
| 알파/경계 | Import후코어모든변0·실RT외곽0 | Import후Walk8모든변0·실RT외곽0 |
| 적용준비판정 | 격리클립/표시기술PASS | 격리클립/경계기술PASS /실접지HOLD |

실행은Sprite배열을임의교체하는PNG루프가아닙니다. UnityEditor API로실제.anim과.controller를만들어 **PlayMode Animator 자동평가후LateUpdate**에서Sprite/상태/normalizedTime을읽었습니다. 80Hz 고정시뮬레이션추적으로프레임중간유지구간의실Sprite가Clip시간과맞는지검사했고100샘플이상통과했습니다. 반복/전환도실제Animator결과입니다.

### Owl

Attack4는각125ms키,0.5초이후duration0 exitTime1의실제Animator전환으로별도Idle00 Clip에복귀했습니다. 두번모두00/01/02/03/Idle순서·실IdleSprite참조를확인했습니다. **01→02→03→Idle00**에서승인몸통·발/꼬리가이어지고03과Idle00은동일그림으로복귀합니다. 40px렌더에서도00날개모음/01펼침차이가보이며,128px개별RT로얼굴/몸통·날개/꼬리전환을확인했습니다. 최종움직임의주관적수용은대표판단대기입니다.

본체실제SpriteRendererSortingOrder를기준으로보라색검증proxy의앞/뒤중첩픽셀이바뀜을확인했습니다. proxy는정렬시험용단순도형이며제품VFX/전투타깃이아닙니다. **분리된미승인냉기VFX는Import0**. 기존승인v3·Idle8의SHA보존,정상Idle8전수재생/QA0(복귀에필요한Idle00만사용).

40px는**512캔버스의실제투영크기**입니다. 알파가시폭은포즈별34~37px/가시높이30~36px였습니다. 128px캔버스에서는가시폭109~120/높이97~118px. 본편Battle UI·상대슬롯/패널가림은이번미연결·NOT_VERIFIED이며,새전투를만들지않았습니다.

### Spider

작업브랜치의실제 `Resources/MonsterDefinitions/03_ForestSpider.asset`에서 `animationFramesPerSecond=10`, `visualScale=.7`, `sourceFacesRight=1`, `fieldMoveSpeed=.65`를읽기확인했고기존Walk PNG.meta PPU256/중앙Pivot과대조했습니다. 정의/원본meta는수정하지않았습니다. fixture는우향계약에따라flipX=false와+X이동을사용했습니다.

실제1WU고정격자지면위에서다음세속도로클립을강제재생했습니다. **0속도는형태관찰용Walk이며게임Idle로해석하지않습니다.**

| 테스트속도 | 한루프0.8초전진량 | 실제관찰 |
|---:|---:|---|
| 0 WU/s |0WU| 제자리포즈변화기준,두루프정상 |
| **0.65 WU/s** |**0.52WU**| 실제정의속도기준,두루프·+X이동정상 |
|1.3 WU/s|1.04WU| 두배속도비교,두루프·+X이동정상 |

각128 PlayMode샘플의실제프레임순서는00~07두번후다음00으로일치했습니다. 128px/40px실유기형실루엣·발끝변화와루프/지면대비를캡처했습니다. 가시폭은40px캔버스에서36~38px/높이23~24px,128px에서117~123/높이75~78px. ImportedTexture의모든Walk8 네변alpha0,실캡처에서도잘림재발없음.

**발 접지·미끄러짐 판정: HOLD / ACTUAL_GROUND_LOCK_NOT_VERIFIED.** 이 스프라이트에는 발별 rig, footCollider, 접촉점 계측 데이터가 없습니다. 실제 클립과 루트 이동은 검증했지만, 발이 언제 지지·스윙 상태인지, 발끝이 실제 바닥에 고정되는지는 측정하지 못했습니다. 속도만 바뀌고 클립은 10FPS를 유지하므로 발 접촉과 속도 연동이 검증됐다고 주장할 수 없습니다. 40px에서는 미세한 발끝 변화가 작게 보이며, 이동 영상만으로 미끄러짐이 없다고 선언하지 않습니다.  기존Walk00/03/04/05·승인v5/Idle8보호,Attack8/Shoot8·Ch4·Bee검수/제작0.

## 3. 실제 캡처·영상 / OFFLINE 열람판 구별

**한페이지 증거판:** `F:/study/codex/Project-Limitless/Temp/Issue84OwlSpiderUnityDelta20261103/Review/index.html`
HTML SHA256 **41e8e303062a29c753a55f2702940038b9b469e884e4f9e2f7570ecea9566173**.

이HTML은OFFLINE열람파일이며,안의PNG/MP4는**실제UnityPlayMode Camera RenderTexture 캡처**입니다. 납품GIF나소스PNG를모의재생한것이아니며,OS GameView/사람손플레이/본편전투Scene이아닙니다.

- 실제영상4: [Owl](Evidence/Issue84OwlSpiderUnityDelta20261103/Media/OwlStage.mp4), [Spider0](Evidence/Issue84OwlSpiderUnityDelta20261103/Media/SpiderStage0.mp4), [Spider.65](Evidence/Issue84OwlSpiderUnityDelta20261103/Media/SpiderStage1.mp4), [Spider1.3](Evidence/Issue84OwlSpiderUnityDelta20261103/Media/SpiderStage2.mp4).
- 영상은640×360 **기록20FPS**,Owl30프레임/1.5초,Spider각32프레임/1.6초. **AnimationClip8/10FPS와기록FPS를구별**합니다. 원본RT126장을순서그대로인코딩했고전영상126프레임decode PASS입니다.
- 투명Native RT **40·128각61장=122장**,Sprite캔버스가정확40/128px가되도록투영검사했습니다. 파일44×44/132×132의사방2px은증거캡처여백입니다. 실제알파실루엣을가리지않도록추가한카메라여백이며Sprite축소/캔버스원본변경아닙니다.122장전체캡처외곽alpha접촉0.
- 개별대표프레임은동Evidence의Native40/Native128,접촉판은Media의Owl/Spider_UnityNative128_40_Contact.png입니다. 접촉판은실제RT를배치한열람자료이며게임원화합성이아닙니다.
- HTML이미지/영상내장·네트워크의존0. headless브라우저이미지·영상메타데이터/코덱·PNG제어/실시간·폭검사 **23 PASS/0 FAIL**. 최초자동메타데이터읽기에서하단영상3개가아직로딩전이었고,재생요청후4개모두640×360확인/실영상decode도4/4통과했습니다. 개인브라우저/포커스변경0.

[37개실행체크](Evidence/Issue84OwlSpiderUnityDelta20261103/checks.txt), [실제Editor설정](Evidence/Issue84OwlSpiderUnityDelta20261103/EditorSpec.json), [504실재생샘플](Evidence/Issue84OwlSpiderUnityDelta20261103/Trace.json), [렌더픽셀/이동·순서분석](Evidence/Issue84OwlSpiderUnityDelta20261103/CaptureReadback.json), [캡처·영상SHA](Evidence/Issue84OwlSpiderUnityDelta20261103/EvidenceSHA256.json), [생성Clip/Controller/Scene SHA6](Evidence/Issue84OwlSpiderUnityDelta20261103/GeneratedAssetSHA.json).

## 4. 새 오류와 검증기 수정 이력

최종 compile오류0·클립Runtime새오류0·시험37PASS/FAIL0. 다만 **기존 UnityEditor.Search.SearchDatabase 시작예외 ArgumentOutOfRangeException 1건**은그대로있어[환경로그](Evidence/Issue84OwlSpiderUnityDelta20261103/editor_environment_errors.txt)에분리보존했습니다. Console전체가깨끗하다고표현하지않습니다. Licensing토큰갱신경고이후entitlement확인성공·배치정상종료,계정/라이선스설정수정0.

초기실행과재실행로그는Temp에보존했습니다:
1. 생성기에서중복마지막Sprite키를넣어Owl길이.625/Spider.9초가되는검증기문제를발견. Unity의Sprite키가마지막키한프레임을자동포함하는것을실물.anim에서확인하고 **키4/8개만생성**하도록수정했습니다. 후보PNG변경0.
2. 수정후재생/전환검사는통과했으나기존Editor.Search예외를새Runtime오류와합산한마지막검사실패. **해당예외문구+SearchDatabase스택만**환경로그로분리하도록바꾸고실제투영/경계검사를더해최종실행했습니다. 그밖의오류는계속실패처리합니다.
3. 최종실행결과는 `QA/UnityEvidenceVerified/`와`unity-verified.log`입니다. 이전`UnityEvidence/`·`UnityEvidenceFinal/`의실패를숨기거나자산실패로보고하지않습니다.

## 5. 재현 코드·원본 보호·다음 최소 게이트

실제사용한검증코드: [Editor제작기](../../Tools/QA/Issue84UnityAnimationDelta.cs), [PlayMode관찰기](../../Tools/QA/Issue84ClipProbe.cs). **제품Assets밖Tools**에만로컬커밋했습니다. 재현시해당#84 fixture의Assets/Editor·Assets에각각복사하고,14입력PNG와최소4모듈환경에서 `-batchmode -executeMethod Issue84UnityAnimationDelta.Run`을실행합니다. PlayMode가자체종료하므로`-quit`없이사용합니다. 경로/버전가드가원본프로젝트실행을차단합니다. 생성Scene/Clip은fixture `Assets/GeneratedVerified/`에만있고제품Scene/BuildSettings에추가하지않았습니다.

원본main **ab47b931fec92976170f9f50aabde1822d78bb90**,기본status344/상세-uall858전후동일·이전승인/Idle참조28SHA동일·ZIP2/Import후입력12+승인참조2 PNG동일. [보호근거](Evidence/Issue84OwlSpiderUnityDelta20261103/ProtectionAfter.json). 개인Save/UserData접근0·원본Unity/Scene/Prefab/Packages/ProjectVersion/BuildSettings/MonsterDefinition/TTS/Discord66변경0·기존전체QA반복0·maincommit/Push/merge0.

**적용준비판정:** Owl은클립/표시설정의격리기술준비PASS이나최종미술·본편렌더러/Battle UI연결승인필요. Spider는클립/경계기술PASS이나발별지지/접촉·이동속도동기화확인전전체보행준비HOLD입니다. 다음은대표의실제캡처미술검토와필요한발anchor/지지·스윙기준확정/실지면최소계측입니다. 이번작업에서본편적용/전투/새모션/Save복구를구현하지않고종료합니다.

### Issue #84 로컬 커밋·GitHub 보고 영수증
- 검증 코드·증거 커밋: `bde96a92e14c663e33ce7640f98f863d58a0d306` (`codex/owl-spider-unity-animation-delta-20261103`).
- 실제 댓글: [#84](https://github.com/HappyPaul812/Project-Limitless/issues/84#issuecomment-6597692700), [#74](https://github.com/HappyPaul812/Project-Limitless/issues/74#issuecomment-6597702364), [#60](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6597703337).
- GitHub에는 댓글만 보고했습니다. `LOCAL_COMMITTED / REMOTE_NOT_PUSHED / NOT_UNITY_APPLIED / USER_VISUAL_REVIEW_PENDING`.
- Owl 클립 기술 PASS, Spider 클립 기술 PASS / 실제 발 접지·미끄러짐 NOT_VERIFIED·WALK_CONTACT_HOLD. 최종 미술 승인은 별도입니다.