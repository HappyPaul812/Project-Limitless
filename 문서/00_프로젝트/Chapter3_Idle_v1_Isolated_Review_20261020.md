# Chapter3 서리들쥐 A·설빙의 폭군 B Idle v1 검수

2026-10-20 — **종합 PASS / A·B 기술 PASS·미술 PASS / 사용자 모션 미술 승인 권고**.
정식 등록·본게임 통합·Push·main 병합은 하지 않았다.

## 정본·작업 범위

[Issue #30 최신 Idle 납품 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-6284708608), 기획의 [1포즈 승인](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/Chapter3_ApprovedAB_1Pose_ArtRework_v2_UserApproval_20261020.md)(blob `cf5ba41783b0411199d7a00675bde9d2e5d42f25`), [Idle 확정 실행 계약](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/Chapter3_Idle2_And_Field09_Phase2Fixv2_Execution_20261020.md)(blob `bf49260e278e33eba3987a328c5e4abec0cd9245`), [art-new 납품 QA](https://github.com/HappyPaul812/Project-Limitless/blob/docs/art-new/문서/00_프로젝트/Chapter3_ApprovedAB_Idle_4Frame_v1_Production_QA_20261020.md)(blob `937f9c5227a110e705a822dc3e2bfe54b922b15d`) 확인.

로컬 Codex 최종 계약 `b1793f85`의 셀512/PPU512/Pivot하단중앙/4프레임/8FPS/Scale1·3을 사용했다. 이전 임시314 규격은 쓰지 않았다. AGENTS·PROJECT_CONTEXT·CURRENT_STATUS·CODEX_WORKFLOW·공통/담당·최신 인계도 확인했다.

브랜치 `codex/chapter3-idle-v1-review-20261020`, 시작 main `12c69c72f9c09f716ee6242418b1d62f83a4a0d2`. 관리형 격리 worktree를 재사용하되 완료된 Field09 통합 브랜치는 그대로 보존했다. 원본 main/Unity Editor를 조작하지 않았다.

## ZIP·파일 — PASS

`F:\Downloads\Limitless_Chapter3_FrostFieldMouse_FrostTyrant_Idle_v1.zip`

| 대상 | SHA256 |
|---|---|
| ZIP | `6c01e55c147ab57147c767f43503553164197cc037a3834ecd2d15d10d96a23c` |
| `Sprites/FrostFieldMouse_A_Idle_4Frames.png` | `5afebfa1f95ca5673162513fdea63b025546aa836550c0ceb17f0334659988f2` |
| `Sprites/FrostTyrant_B_Idle_4Frames.png` | `9d9b8d05be1813c013a5d784d749ce2d8ef751541ac36ce90712f3255cb53f43` |

6,953,861bytes / 12파일, CRC/testzip·SHA256SUMS·납품 SHA 일치, PNG2 디코딩 PASS. 구성은 시트2·프레임판2·APNG2·GIF2·128px비교1·Manifest/OfflineQA/SHA목록3이다.

**별도 README는 없다.** Manifest.md에 방향·Import·프레임·제작 방식·범위 설명이 있어 검수에 필요한 정보는 확인했다. 별도 README 보완은 납품 문서 권고이며 이번 시트·모션 검수의 차단 결함으로 세지 않았다.

## 프레임·투명도·승인 원본 일관성 — PASS

두 시트 모두 **2048×512 RGBA, 가로4×512×512**, 우향1종. 두 캐릭터의 **00프레임은 승인 PNG와 디코딩 RGBA 픽셀이 정확히 같다**. 승인 SHA는 A `0516914ddad36af5854065d66d01b9664329aa89eea8b394480eed6af92e32c2`, B `f1a3c5f91a5fef69cd881e27b578c32253d0124d5d8628a8ca748f7479333830`로 일치한다.

| 항목 | 서리들쥐 A | 설빙의 폭군 B |
|---|---|---|
| 01/02/03의 원본 대비 변경 픽셀 | 86,696 / 82,012 / 86,604 | 135,355 / 129,689 / 134,738 |
| 4프레임 RGBA 해시 | 모두 다름 | 모두 다름 |
| 최외곽 알파·잘림 | 0 / 없음 | 0 / 없음 |
| 프레임 하단 접점 | alpha bbox 하단y490 고정 | 동일 |
| y438~511 접지 band | 매 프레임 원본 픽셀과 동일 | 동일 |
| 작은 고립군집(7px미만) | 최대alpha4/255 | 최대alpha3/255 |

작은 알파 군집은 승인 원본에도 있던 극미량 투명 성분이다. 새 불투명 잡픽셀이나 사각 테두리로 보이지 않았다. 캔버스 바깥으로 꼬리·뿔·발톱이 잘리지 않는다. 승인 원본의 하단22px 여백과 하단중앙 Pivot 기준을 그대로 유지했다.

납품 방식은 승인 그림의 **지역 가중 비강체 변형/보간**이다. 동일 그림4장 복제나 몸 전체를 통째로 위아래 이동한 자료가 아니다. 발밑은 고정하고 상부·꼬리·팔 영역이 다르게 변한다. 신규 해부학을 다시 그린 프레임이나 수작업 픽셀 수정 과정까지 독립 검증했다고 주장하지 않는다.

## 시각 판정

### 서리들쥐 — 기술 PASS / 미술 PASS

등·몸통의 작은 호흡과 머리/수염·꼬리의 국소 변화가 있고 발끝은 고정된다. 승인된 긴 꼬리·앞니·발톱·날카로운 눈과 긴장된 체형이 유지되며 둥근 마스코트 인상으로 회귀하지 않았다. 128px에서도 주요 실루엣이 유지된다. 실사용112px 셀에서는 움직임이 매우 절제돼 보이지만, 확인판/루프에서 반복되는 국소 움직임이 확인되므로 복제 정지 화면으로 판정하지 않았다.

### 설빙의 폭군 — 기술 PASS / 미술 PASS

흉곽·어깨와 긴 팔이 소폭 연동하며 뿔·두개골·거대한 손/발톱이 유지된다. 다리가 미끄러지거나 몸 전체가 튀는 동작이 없고 체급감이 줄지 않았다. 공식 열맥 거신과 같은Scale3 비교에서 큰 상체와 팔의 압박감이 유지된다. 과장된 큰 동작은 없는 짧은 Idle 파일럿이며 공격/피격 동작의 품질까지 승인하는 것은 아니다.

## 루프와 실제 격리 Unity

납품 APNG는 **125/125/125/125ms**, 총0.5초다. GIF는 포맷의10ms 정밀도 때문에120/130/120/130ms이며 합계0.5초이므로125ms APNG·엔진 재생을 판정 기준으로 삼았다.

03→00의 합성 RGB 변화 평균은 A12.788/B19.598로, 다른 인접 전환의 A11.532~12.786/B17.800~19.678 범위와 비슷하다. 이 수치만으로 자연스러움을 확정하지 않고 전프레임/실사용 비교와 접점 고정을 함께 보았다. 03→00에만 큰 위치·모양 점프가 보이지 않아 **루프 자연스러움 PASS**다.

기존 숨김 Batch Client `Temp/Field05Review20261019/Client`의 QA 전용 폴더에 원본 바이트를 복사하고, **실제 Editor Importer/ISpriteEditorDataProvider**로 분할했다. CreateAndDeleteSprite/EditName/EditRect/EditPivot 지원 권한을 먼저 확인했고, `.meta`를 수동으로 편집해 분할하지 않았다.

**Unity 변경분14PASS / 0FAIL**:

- 각 시트4 Sprite, 셀512/PPU512/실제 Pivot(256,0) 확인.
- 임시 SpriteRenderer의 월드 폭 A1/B3 확인.
- 공용 `MonsterSpriteSheetAnimation`과 메모리 전용 Definition으로 8FPS 반복. 신규 정식 MonsterDefinition·Scene·Animator·AnimationClip은 만들지 않았다.
- 각25회 프레임 변화에서 **00→01→02→03→00 순환** 확인. 시작 부분을 제외한 루프 주기 중앙값은 두 종 모두 **0.49992초**였다. 프레임 관측 간격 중앙값은 약0.127초로 Editor 스케줄링 간격이 포함되며 엔진 설정은8FPS다.
- 1280×720 실제 GameView4장과 캡처 프레임 APNG를 확보했다. 신규 제품 오류0, 기존 UnityEditor.Search 인덱스 진단은 로그에 분리한다.

GameView는 **전용 QA Canvas에서 현재 전투 UI와 같은 Image Rect·preserveAspect·CanvasScaler를 재현한 비교 화면**이다. 실전 전투 Scene에 두 몬스터를 정식 구현한 것이 아니다. A rect136×112(정사각 셀112×112), B408×336(셀336×336)로 승인 원본·Idle·공식Ch2를 비교했다. A의 가시 크기는 약102×80 UI단위, B는 약307×305로 계약과 일치한다. 비교용 공식 몬스터/보스는 첫 Idle의 정적 기준이며 기존 애니메이션을 수정하지 않았다.

## 권고·보호·다음 단계

**종합 PASS. 바울 대표의 Idle 모션 미술 승인 권고.** 승인 후 별도 격리 정식 Import/데이터 연결 단계로 진행할 수 있다. 이번 판정만으로 본게임 적용/Attack·Hit·Defeat 제작/다른 몬스터 확장을 시작하지 않는다.

원본 Assets/_Project·UserData·Settings·Packages **3,546파일 SHA변경0**, 원본 Git HEAD/status 동일, 승인1포즈 원본 불변. 원본 Editor Bootstrap/PlayOFF/비포커스 유지. 검수 브랜치의 제품 Unity 파일 변경0, 새 코드는 격리 QA 하네스/문서 근거로만 보존했다. Scene·Save·UserData·F1/서릿길마을·기존 공식 자산·완료된 Field09 브랜치 보호. Push/main 병합0.

[움직이는 비교·Unity 루프](Evidence/Chapter3IdleV1_20261020/index.html) · [실제 GameView0](Evidence/Chapter3IdleV1_20261020/Runtime/GameView_Frame0.png) · [파일/프레임 수치](Evidence/Chapter3IdleV1_20261020/Validation.json) · [실제 프레임 추적](Evidence/Chapter3IdleV1_20261020/RuntimeTiming.json) · [원본 보호](Evidence/Chapter3IdleV1_20261020/Protection.json).
