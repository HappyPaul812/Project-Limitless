# Issue65 실제 비교판·최소 배치 후속 검수

**LOCAL_COMMITTED / REMOTE_NOT_PUSHED / USER_ART_REVIEW_PENDING / NOT_PRODUCTION_APPLIED**

브랜치 `codex/art-pilots-review-boards-20261023`, 기반 `91510554`(#64 보고)·`3bb4f983`(#64 검수기), 신규 검수기 commit `7076fadeb50f8aa19c1d1e6008311451c586c07e`. #65/#64를 확인했고 기존41/15/148/136 전수 재실행0. 원본과 #64 worktree는 변경하지 않았다.

## A — 검수 재사용

#64의 ZIP SHA·CRC·내부Manifest, 독침벌 활성Idle/Attack원본·라이선스·Chapter2공식5종 동배율·alpha3배경판을 재사용했다. 실제 이전 HEAD와 파일SHA를 `ReuseReceipt.json`으로 확인했다. 기존41PASS/15PASS를 이번 신규PASS에 합산하지 않는다. 독침벌 신규 런타임 검사0, 기존10+10프레임/MonsterDefinition/GUID/독/AI/Drop/Spawn 수정0. 새 미술이나20프레임을 생성하지 않았다.

**독침벌 기술 관찰(기존 근거 재사용):** 새512단일자세는 좌향, 현Pilot Bee는 우향. 40/128px 공통Image박스와 전투계약(벌115.6×95.2/기타136×112), Chapter2공식11~15 explicitIdleFrames[0]를 사용한 실제UnityCanvas 비교판을 그대로 제공한다. 이 판은 제품Battle Scene 교체가 아닌 검토Canvas이며 40/128은 실루엣을 따로 키운 것이 아닌 같은 캔버스 예산이다. 작은 크기에서 침·앞다리/날개맥이 약해지고128px는 세부 식별이 좋아진다.512Master는 외곽alpha0, 납품Preview40/128의 경계접촉과 CC BY3/4 출처보류는 유지한다. 원본크레딧 Jana Ochse / 2D!PIXX·[OGA 배포 페이지](https://opengameart.org/content/pilotbeegamecharacter)와 동봉LICENSE 확인은 #64 증거이며 이번 새 권리 해결/애니메이션PASS가 아니다.

## B — 신규 격리 Delta

**33 PASS / 최종FAIL0**, 별도 고정카메라 촬영 확인 **8 PASS / FAIL0**, 최종런타임오류0/컴파일오류0. 처음 검수기 식별용GetInstanceID가 Unity6.6에서 금지되어 컴파일1회 실패했고, 이름·타입·bounds 지문으로 수정했다. 이 초기 실패 시 Play 검사 실행0. 최종촬영은 카메라보간의1~2px 차이를 없애기 위해 비교중CameraFollow만 고정했다. 실제 이동33검사는 재호출하지 않았다. 기존Editor Search 예외는 환경기록으로 분리하여 OPEN 유지한다.

- 오르나 전체캔버스40/82/96/128px 표시를 기존NPC 참고복사본·현재 휠체어플레이어와 동일장면 비교.40/128은1280×720,960×720,1680×720;82/96은16:9 추가비교.
- 실제 Main23MouseTravel→PlayerController/Rigidbody로40/96/128에서 휠체어 통과, 플레이어 전면정렬 확인.82는 같은 Collider0·위치/정렬 조건의 비례 권고이며 별도 통과검사행을 부풀리지 않는다.
- 주택 기존전체Sprite/지붕 유지·신규Gable/Trim 미설치, EntryDoor만 시연.각3화면비 KEEP_OLD/EntryDoorOnly의 카메라Transform 완전동일 확인. 기존 문앞 바닥포켓 실제 접근.
- 여관 신규간판 미설치·기존본체SpriteRenderer1개만 유지, 기존 내장간판 하나 표시.3화면비 및 휠체어 여관 접근 확인.
- 이 배치안 적용 전후 정적Collider 전체지문 동일. 남북3WU·동쪽7×7 예약공간에는 새오브젝트/Collider 설치0. Quest/F1/F2/포탈/Save/Main24 기능 수정0. 사용자 실제장치 손플레이는 NOT_TESTED.

검수 환경은 숨김 Unity6000.6.5f1 batch Play Mode, 합성저장·virtual Keyboard/Mouse. 저장소6000.5.7f1과의 버전 차이는 #64와 동일한 환경제약이다. 실제OS GameView/Editor포커스 조작0.21개 신규Camera+Canvas RenderTexture와 #64자료3개, 무배율크롭판3개·축소개요1개를 분류해 기록했다.33검사에는 화면크기/정렬/경로항목이 포함되므로33개의 별개 게임기능 완료를 뜻하지 않는다.

## 자산별 기술 판정과 권고

| 대상 | 기술 결과 | 미술 검토·최소 권고 |
|---|---|---|
| 오르나 | TECH_PASS 표시·통행·전면정렬 | **82px 전체캔버스 권고**,96px는 가독성 절충안.40px 작음/128px 큼. 모든NPC의 규격이나 PPU를 바꾸는 지시 아님. 질감·비례는 USER_ART_REVIEW_PENDING. |
| 남문 제안 주택 | TECH_PASS 신규Gable/Trim제외·기존본체/충돌유지 | EntryDoorOnly에도 작은박공·계단이 원래 문과 중첩하여 ART_HOLD_JOIN. **기존 주택 KEEP_OLD 우선**, 필요 시 문틀/문짝 독립1PNG만 최소 보완. |
| 여관 | TECH_PASS 간판중복해소·접근 | **KEEP_OLD(기존 Inn본체 내장간판1개)** 권고. 신규StandaloneSign은 보존·사용보류. 기존조각 Sign_Inn.png를 따로 설치한 것이 아님. |
| 독침벌 | A 재사용 TECH_PASS 단일그림·동배율비교 | 교체ART_HOLD, 좌향1자세/기존우향20프레임·권리버전 보류. 최종미술 결정 전 모션제작/본편교체0. |

기존NPC 캔버스82.286px·플레이어 본체72px(현재실제Mobility/Physical_Guardian_Male,PPU128). 후보82/96/128은 기존NPC높이의 약1.00/1.17/1.56배, 플레이어 본체의1.14/1.33/1.78배. 후보alpha인물높이는484/512배라82px예산에서약77.5px이다. 픽셀NPC와 페인터리원화의 질감차이는 크기만으로 해결되지 않는다. 근접시 플레이어 이름표가 NPC하체를 가릴 수 있으며, NPC가 휠체어본체 앞을 덮지 않는 정렬은 확인했다.128은 배경을 더 많이 가리므로 기본월드표시에 강제하지 않는다.

## 좌표·원본Gap3행과 이번 실제 배치 구분

`A_OriginalGapThreeRows.csv`는 #58의 제안3행을 그대로 발췌했다. 예전 제안과 실제실험값은 다르며 임의로 정식규격으로 확정하지 않는다.

| 대상 | #58 제안 | 이번 실제 시연 |
|---|---|---|
| Orna | -3,-4;PPU128;128×192;1×1.5WU;Collider.5×.34 제안 | 좌표동일;새512PNG PPU512/pivot(256,0);40:.5556,82:1.1389,96:1.3333,128:1.7778 scale.자체Collider0/기존클릭Owner유지/order2400. |
| 주택접합 | 모듈주택-13,1;4×4WU예산/기존Collider2.2×1.7 제안 | 실제Residence는 북서(-12.5,7.2),PPU128/pivot(251,0)/3.922×2.922WU/Collider3.1×1.6 유지.문(-12.5,7.3),PPU256/pivot(287.5,0)/scale.55/1.235×1.983WU/그룹1280·내부3/신규Collider0.남문집 신설·이동0. |
| 여관간판 | -7.2,3.4;PPU128;1×1WU;본체+1;Collider0 제안 | 새간판 미설치.기존Inn(-6.55,2.2)/PPU128/pivot(270,0)/scale1/4.219×3.461WU/그룹1780·내부0/Collider3.5×1.85 유지. |

지면9/기존43 GameArt/승인펫/공식Ch2·보스는 원본변경0. #64의 큰겹지붕은 A재사용이므로 같은피험씬·새촬영인 것처럼 표시하지 않았다. B의KEEP_OLD와EntryDoorOnly는 새고정카메라동일조건쌍이다. EntryDoor의 자체 작은박공이 기존다락창/문/계단 위로 겹쳐 보이는 부분까지 비교판에 남겼다.

## 실제 이미지와 SHA

- [4주제 축소개요](Evidence/ArtReview65/Overview_4Topics_SCALED_ContactSheet.png) — 썸네일이므로40px판정용아님.
- [Board1 오르나40/82/96/128](Evidence/ArtReview65/Board1_Orna_40_82_96_128_UNAPPROVED.png) — B신규동일카메라 원본크롭, 배율변경없음. 왼쪽기존NPC는그림 참고복사본이며 NPC추가 아님.
- [Board2 기존주택·EntryDoorOnly](Evidence/ArtReview65/Board2_Roof_Excluded_EntryDoorOnly_UNAPPROVED.png) — A#64겹지붕재사용/B#65기존·문단독을 명시분리.
- [Board3 여관KEEP_OLD](Evidence/ArtReview65/Board3_Inn_OneSign_KEEP_OLD.png) — A#64중복/B#65간판하나 구분.
- [Board4 독침벌·공식Ch2 동배율](Evidence/ArtReview65/A64_Bee_Ch2_Original_UnityBoard_UNAPPROVED.png) — A#64실제UnityCanvas 원본 byte-identical 재사용.

PNG28종 각각 크기·분류·전체SHA는 `ImageIndex_SHA256.csv`.크롭영역/원본파일SHA/재배율없음은 `BoardSourceMap.json`.원본급21개 전체화면을 보존한다. 축소개요를 실제Unity한화면이나40px실측원본으로 주장하지 않는다.

## 보호·GitHub 인계·승인

원본/기존Main23/Issue64worktree **21,170파일 SHA 변경0**(이전14,206포함),3곳 HEAD/index/status 불변.새 worktree의Unity 자동환경변환244파일만복구했다.기존Scene/Resources/PNG/원본Meta/Save/UserData 교체0.새Editor검수기와 문서/이미지만 로컬기록.새Repo복사본내UI입력과합성Save만사용했다.

GitHub댓글은 사용자명시지시에따라 #65 본보고·#59 Main23·#63 독침벌 근거를 직접인계한다. 연결도구는PNG바이너리첨부미지원이므로댓글에실측SHA·좌표·판정·로컬경로를기록하고 **REMOTE_IMAGE_UNAVAILABLE**를 명시한다. 로컬commit이 원격에있다고가장하지 않는다.코드/사용자저장/개인파일/원본대량시스템파일을업로드하지 않는다.본편적용/원격gitpush/main병합0. **최종미술승인필요**; 오르나82px·주택KEEP_OLD또는문최소보완·여관KEEP_OLD·독침벌방향/모션을바울대표가결정한다.


## 바울 대표 로컬 검토 묶음

새 파일 `F:/Downloads/Limitless_Issue65_ArtReview_20261023_4a3afef6.zip` — 41336680 bytes, 38 entries, CRC PASS. SHA256 `4a3afef6e0abfc3a6b90afd19fe35005c67d9522bcefbdc5993a2777d52c95d1`. 새 폴더에 압축을 풀고 **index.html**을 열면 네 주제 비교판과 3화면비 전체화면을 볼 수 있습니다. 기존 ZIP 덮어쓰기0.


## GitHub 직접 인계 완료

[Issue65 완료 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/65#issuecomment-6358407741) · [Issue59 신규 아트 인계](https://github.com/HappyPaul812/Project-Limitless/issues/59#issuecomment-6358419225) · [Issue63 리뉴얼 인계](https://github.com/HappyPaul812/Project-Limitless/issues/63#issuecomment-6358419908). 텍스트 댓글만 게시했고 실제 PNG 바이너리는 원격 미게시입니다. 로컬 보고/이미지 commit `a43c989e7636fcdad740ce43268f51854c9d6a95`.
