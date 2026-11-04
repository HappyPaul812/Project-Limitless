# Issue #91 Chapter1·3 실제 원화 5장 동일 배율 비교 / 2026-11-04

## 납품·최종 상태
**USER_VISUAL_REVIEW_PENDING / NOT_UNITY_APPLIED** 유지. 배경 원본 수정0·새 게임용 배경 PNG0·Unity 실행/Import0. 신규 산출물은 **검수용 비교판 한 장**과 기록입니다.

- 실제 저장 위치: `F:/Downloads/LIMITLESS_Issue91_Ch1_Ch3_1672x941_1to1_Comparison_20261104.png`
- 비교판 **5144x4200 RGB / 11,341,439bytes**. 큰 판 안에 원본5개를 **각각1672x941,1:1** 배치했습니다. 출력 전체를1672x941로 줄인 비교가 아닙니다.
- 실제 F: 저장 파일 SHA256: **e10aa647089ee9b665058f3428e589b50dbfbdd97b508bf87ca7f361b42ceee9**.
- [브랜치 내 동일 비교판](Evidence/Issue91SameScale20261104/LIMITLESS_Issue91_Ch1_Ch3_1672x941_1to1_Comparison_20261104.png), [입력5장 SHA/경로](Evidence/Issue91SameScale20261104/InputManifest.json), [패널 화소일치5/5](Evidence/Issue91SameScale20261104/ComparisonQA.json), [입력 전후SHA동일5/5](Evidence/Issue91SameScale20261104/InputProtectionAfter.json).

## 읽은 최신 근거
[Issue91 신규아트 검토댓글](https://github.com/HappyPaul812/Project-Limitless/issues/91#issuecomment-6622873655), [기획 접수](https://github.com/HappyPaul812/Project-Limitless/issues/91#issuecomment-6622867188), `docs/art-new/문서/00_프로젝트/Chapter3_BattleBG_Issue91_Style_ReadabilityReview_20261104.md`(d0babe4b7448f2755d263c4822abe139084ec7ae), SPRITE_ART_LATEST 최신 Issue91 항목(a7b52eedb3bf8cd81eeade461322e3a54980bdd4)을 확인했습니다.

이전 아트 검토는 Ch1 실제화소 수급 실패로 CH1_STYLE_PARITY_NOT_VERIFIED였습니다. 이번에는 실제 로컬 Ch1 원본3장과 이미 동일성 확인된 F: ZIP의 C3 Candidate2장 화소를 확보해 **실물1:1 비교 수행**으로 갱신합니다. 다만 비교 수행은 화풍 동등PASS나 미술승인/Unity검증이 아닙니다.

## 실제 입력5장
모두1672x941 RGB. Ch1은 실제제품 PNG를읽기만하고 C3는 기존ZIP Candidate엔트리를Temp에바이트그대로추출했습니다. 색보정/샤프닝/리사이즈를하지않았습니다. 원본5패널에는선/주석을덧그리지않고 별도영역에설명했습니다.

| 원본 | SHA256 |
|---|---|
| Ch1 BattleBG_Forest.png | 820f1542ae55c97254ad38df7830d36226b89a7751a6e627dbe715ad75bf7720 |
| Ch1 BattleBG_Grassland.png | ee46adad2ed360f73ddbdc012aed29e90f25b0c9590485985df3a3eaa20ccb98 |
| Ch1 BattleBG_Silent_Catacombs.png | 92301abcaacb58337edbd1b7506d747565ead795530d24f763ba35072babc767 |
| C3 C3_BattleBG_FrostwaySnowroad_v1_CANDIDATE_1672x941.png | a5f3bb960b5c5e6fe4140cb2de64e726f4bc7fabb8674f895789fed128adcd25 |
| C3 C3_BattleBG_FirstSnowForest_F2_v1_CANDIDATE_1672x941.png | 93388ee52fd75da4a82df0b68e03b2083167a25f339d97b41dc71c807125282b |

## 동일 배율 화풍 관찰
| 항목 | Chapter1 공식3장 | Snowroad | FirstSnowForest |
|---|---|---|---|
| 도트/입자 크기 인상 | 작은 불규칙 색면/붓결이 겹치며 재질마다 밀도가 다름 | 큰 삼각 산·타원 구름·선형 경계와 작은 잡음이 분리됨 | 큰 타원 수관·단색 기둥과 얇은 계단형 테두리가 두드러짐 |
| 질감 | Forest 수피·이끼/잎, Grassland 풀·흙·꽃, Catacombs 석재·부스러기 재질이 구분됨 | 눈은 균일 입자, 길은 넓은 평면과 반복 조각 위주 | 수간 넓은 면의 수피 정보가 적고 눈·길 질감 대비가 약함 |
| 명암 | 표면 방향과 차폐에 따른 국소 명암, 작은 반사/밝은 부분이 분포 | 산과 나무가 소수의 넓은 명도층으로 나뉨 | 수관/기둥의 큰 단색면 비중이 큼 |
| 원경 | Forest의 푸른 대기층, Grassland 산·성·수목의 거리별 세부 감소, Catacombs 깊은 아치/계단 | 산과 구름의 기하 실루엣·수평층, 반복 수목 윤곽이 강조됨 | 가까운 굵은 나무와 먼 침엽수 띠 사이 질감·형태 연결이 단순함 |
| 지면 세밀함 | 흙/풀/바위·석판 균열이 크기와 방향을 달리하며 퍼짐 | 직선 길 테두리·반복 눈 조각이 더 눈에 띔 | 비슷한 지면 입자와 평평한 일부 나무 밑동이 눈에 띔 |

도트크기를고정된논리픽셀피치로단정하지않습니다. 완성PNG만으로원작의격자를역산할근거는없습니다. 대신모든이미지의동일원본좌표에서지면(640,680)-(896,936),원경(920,180)-(1176,436)의256x256크롭을1:1로실어세부밀도차이를판독할수있게했습니다.

전체 BT.709 평균명도는 Forest100.0 / Grassland153.1 / Catacombs67.4 / Snowroad166.2 / F2 128.5입니다. 장면·계절·주야가다르므로수치를맞추라는보완지시나품질점수가아닙니다. 핵심은평균색을같게하는것이아니라재질별질감/명암과원경의구체성차이입니다. 현후보는Ch1과같은세부표현체계라고승인할근거가부족하지만,전면재작화는지시하지않습니다.

## 보완 후보 좌표만 특정
원본좌상단(0,0),우/하단제외. 현재파일을수정하지않았습니다.

| ID/대상 | 원본 좌표 | 필요한 검토만 |
|---|---|---|
| S1 Snowroad 구름·산 | (650,20)-(1320,475) | 큰 타원/단색 산면의 국소 비대칭 경계·눈 재질/거리별 세부 감소 보완 후보. 원경 구도/설산 콘셉트 유지 |
| S2 Snowroad 길·눈 | (700,535)-(1180,941) | 직선 가장자리와 반복 조각을 불규칙 눈/흙 경계·근거리 질감으로 구분하는 표적 후보. 중앙 전투 공간 보존 |
| F1 F2 좌/우 수관 | (0,0)-(480,330), (1240,0)-(1672,340) | 타원 덩어리의 잎/눈 군집과 국소 명암·윤곽 분리 후보. 나무 전량 재작화 지시 아님 |
| F2 F2 오른쪽 수간·밑동 | (1310,240)-(1640,925) | 수피/접지 세부 및 아군 후열 뒤 명암 경쟁 검토. 나무 삭제·배치 변경을 확정하지 않음 |

## 정식 Battle UI·3인/적 참조와 목업 한계
비교판에는 기존 실제Unity캡처 `문서/00_프로젝트/Evidence/SkillLoadout_20261013/Runtime_Final/battle_1280x720.png`(1280x720)를그대로배치했습니다. 실제3인HP행·태온/플레이어/미엘과적배치,상세팝업/메뉴점유참고입니다. 해당캡처는과거검증자료이며현재C3배경합성/새실행화면이아닙니다. 팝업이캐릭터일부를가리는한계도표시했습니다. 현재 `BattleSceneController.cs:509~518`의전열/후열배치를별도코드도식으로그려원본캡처와혼동하지않게했습니다.

- 정식적=왼쪽 x.32/.14,아군=오른쪽 x.68/.86,세로y=.68-.21*Column.3인예시태온전열0/플레이어전열1/미엘후열0이며실제편성에따라달라집니다.
- 정식UI는상단이름/HP·상태행,하단명령·2x3스킬/취소,선택강조/상세팝업이있습니다. 기존목업 `UI_Mock_Forest.png`는비공식실루엣/다른체력창으로아군3을왼쪽·적1을오른쪽에두어정식배치와반대입니다.
- 따라서목업의'왼쪽아군과전경나무간섭'을정식배치위험으로그대로옮길수없습니다. 전체1672x941배경을1280x720화면에Fit하는가정으로정식코드를역투영하면후열0중심약(1392,434),전열0/1중심약(1114,434)/(1114,522)이며F2오른쪽수간영역과경쟁합니다.
- 배경은캐릭터뒤에그리므로직접스프라이트를덮는기하가림이아니라실루엣·명암가독성의위험입니다. 원본그림좌표와정식CharacterImage/ActionRoot·Sprite실루엣범위는동일하지않아이번정적자료만으로최종가림수치를확정하지않습니다.
- 목업의중앙여백만으로밝은옷/적40px·상태/VFX/HP·MP·스킬버튼·960/1280/1680·다인파티실가독성PASS를부여하지않습니다. 이번에정식캐릭터/적을C3에새합성하거나Unity로재검증하지않았습니다.

## 검증·보호·인계
비교판최종레이아웃과요약/주의문구를시각확인했습니다. 원화패널5/5 RGB화소일치,입력5/5전후SHA동일,실제FDownloads저장파일SHA가생성물과동일합니다. C3 2후보는기존ZIP엔트리바이트를유지한Temp사본이며원ZIP/원PNG에쓰기0. 납품한것은검수판1장뿐이며Temp의축소/크롭은레이아웃검수용입니다.

로컬브랜치 `codex/issue91-ch1-ch3-same-scale-review-20261104`에비교판·증거·미술검토·인계를기록합니다. 직전관련064448a8b52d92f1c506dd2b37c713814be4eb9d. 이번실제SHA는Issue91결과댓글에기록합니다. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 신규아트인계도이로컬브랜치에만기록하며원격docs/art-new의장문이력을수정하지않습니다.

Unity/Save/UserData/Scene/Prefab/Packages/BuildSettings/기존승인아트/main수정0, 원본Editor/포커스조작0, 원격Push/병합0. 다음은바울대표의비교판시각판단이며사용자최종미술승인/원본적용으로승격하지않습니다.
