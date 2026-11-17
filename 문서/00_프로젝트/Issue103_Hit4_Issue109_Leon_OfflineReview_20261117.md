# Issue103 Hit4·Issue109 Main13 레온 오프라인 검수 — 2026-11-17

**BOTH_FILE_TECH_PASS / HIT4_ART_REVIEW_PENDING / LEON_USER_ART_APPROVAL_PENDING / BOTH_NOT_APPLIED / UNITY_NOT_IMPORTED / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

바울 대표의 이번 **두 ZIP 검수만·게임 적용 금지** 지시를 따랐습니다. [#103 승인/Hit4 발주](https://github.com/HappyPaul812/Project-Limitless/issues/103#issuecomment-6938653973)·[최신 Hit4 납품](https://github.com/HappyPaul812/Project-Limitless/issues/103#issuecomment-6939185207), [#109 레온 납품](https://github.com/HappyPaul812/Project-Limitless/issues/109#issuecomment-6939150254)을 확인했습니다. 오프라인 Temp 추출·PNG/GIF 분석만 수행했고 Unity 실행/상태조회 도구/Import·GUID 수정·게임 적용·Push·Merge는 하지 않았습니다.

## A. IceBeetle Hit4

**FILE_TECH_PASS / ART_REVIEW_PENDING / NOT_APPLIED**, 사용자 승인 필요 **YES(Hit4)**.

- 실제 `F:/Downloads/IceBeetle_Issue103_Hit4_20261117.zip` **3131456bytes/12파일**, ZIP SHA256 **`982328b9a96f13330b8cdf9d8be6433abd9907fa1b1826dbec0ec352bc6bee55`**.
- CRC PASS. Manifest10/10 실제bytes/SHA·payload전체커버, SHA256SUMS11/11 payload+최종Manifest 포함, 자기/순환참조·중복·위험경로0.
- 게임용 시트 `Sprite/IceBeetle_M4_Hit4_2048x512_RGBA_CANDIDATE.png` **2048×512 RGBA·가로4×512셀**, SHA256 **`cf1dd6c78ffeffd12f8d026dda7ae362e09ffd0a7a3dee536d86569df4f51a3c`**. 분리 QA4장 실제셀 전RGBA 일치·4프레임상이·각셀/시트 외곽alpha0.
- 구성: PNG8(시트1/분리4/128·40스트립2/512·128·40비교판1)+GIF1+문서/Manifest/SUMS3.
- 승인 KeyPose SHA `b7ff0592226c2a1f28270053cf03483b6051c69edb412b59a7965ccf8699b3ad`, Idle4 `bd06c6d9a901d4c3550882ad4ce061629195b60e75f04636ec4814ebb0967793`, Attack4 `d8dab5e411f7eeb0aa73b1e412b8d820ecfeb38d514c60cf5fef7eb29af4b236`를 기존 실물에서 재확인·보호. Hit1~3은 승인Idle/Attack 어느셀과도 전RGBA 동일하지 않으며 **Hit4번은 KeyPose/Idle1번/Attack4번과 전RGBA동일**, 분리Hit03 PNG도 승인KeyPose와 파일바이트동일.
- **접지 y430..511 RGBA4/4 승인KeyPose동일**. alpha8 bbox는1번(20,119,481,464)·2번(25,139,483,464)·3번(27,129,483,464)·4번(29,125,484,464). 하단464 고정·셀밖잘림0. 소스설명은 KeyPose 상부변위/보간 파생이며, 별도생성시안이나공격셀그대로재사용이아니라고명시합니다. 원본 승인PNG변경0.
- 128/40은 원시트 각셀 직접LANCZOS와 전RGBA동일. 비교판12배치 실제원본 전체불투명픽셀일치. GIF실제4프레임512²/loop0·**90/160/110/190ms=550ms 반복**, 원시트배경합성+MEDIANCUT256색 결과와 디코드4/4RGB정확일치. 고정FPS/UnityAnimator 설정으로 오인하지 않습니다.
- **미술 의견:**1번짧은후방충격→2번갑각움츠림→3번회복→4번기존대기로읽힙니다. 전방밀치기Attack과흐름구별·승인청록결정/금빛미광/작은다리/우향유지. 과장점프·갑각파손·새상태이상VFX·특정이상프레임발견0. 40px에서는반동/세부움츠림판독약하므로512/128와가변간격GIF를함께검토권고. 최종피격감/화풍승인은바울대표판단으로남깁니다.
- Unity Import/Assets복사/GUID생성·수정0. Defeat4/수치/스킬/Spawn/MonsterID 추가 구현0.

## B. Main13 아르벨 지역 책임자 레온

**FILE_TECH_PASS / USER_ART_APPROVAL_PENDING / NOT_APPLIED**, 사용자 승인 필요 **YES**.

- 실제 `F:/Downloads/Limitless_Main13_Leon.zip` **732482bytes/9파일**, ZIP SHA256 **`dfaaf5fe453dadeaa268a24edbdcefafb6a9b0266db4c6e6251131b741d6504a`**.
- CRC PASS. Manifest7/7 실제bytes/SHA·payload전체커버, SHA256SUMS8/8 최종Manifest포함·자기/순환참조·중복·위험경로0.
- 원본 `Main13_Leon.png` **512×512 투명RGBA**, SHA256 **`ff7cfd081e7727a7ab228bae0a5ece879727b98594514c932d4a616396bc8fdc`**, alpha8bbox(158,57,354,469), 외곽alpha0. 캔버스에고립외부이미지발견0. 구조/비교판/출처설명을함께확인했으나생성원본전체에대한외부출처감식을수행했다는뜻은아닙니다.
- 구성PNG4(원본1/128·40각1/비교판1)+QA/README/SourceDeclaration/Manifest/SUMS5.128/40 직접LANCZOS 전RGBA일치. 비교판 OGA09/승인DistinctOGA20/승인Main07/새레온의512·128·40 총12배치 실제원본불투명전체일치.
- **OGA09 차이:**현재OGA09는은회색머리·푸른긴옷·지팡이·단순평면화풍입니다. 새레온은묶은갈색계열머리·수염·적갈색어깨천·청록실용복·장화/가죽허리주머니·무기없는선자세라시점/실루엣/배색/소품이뚜렷이구분됩니다. 다른주민대표원본을덮어쓴것이아닌신규후보입니다.
- **미술 의견:**차분한표정·수염과균형잡힌자세는신뢰감있는현장형책임자로읽힙니다. 어깨천/장화/가죽주머니가건조해지는서부거점의실용복과어울리며대형무기·왕관등과장된권위장식은없습니다. 작은문양/금속장식은있으므로장식강도·색조는미술승인항목입니다. **기존 OGA09보다 세밀한 명암·다른 치비 비례**가있어개성구분과별개로게임전체화풍일관성은최종검토가필요합니다.40px에서는수염/문양세부가약하나실루엣/적갈색어깨천은읽힙니다. 부상·붕대/고어·새상태설정발견0.

### 공식 레온 자료 확인 범위

정본 `문서/03_스토리/Chapter2_도입부_Main12_14.md`는 Main13 「메마르는 땅」, 아르벨 지역책임자 `arbel-leon`, 주민생활·보급거점·완전사막이아닌서부를확인합니다. 코드 Chapter2IntroFlow는레온을Arbel(0,3)의상주지역책임자로설치하고, 실제카탈로그는 **`arbel-leon -> VillageNpcSprites/Eldiran/OGA09_VillageRepresentative_Original_Approved`**를공유합니다. 이것을레온전용승인원화로오인하지않았습니다.

현재로컬`Assets/_Project`전체시각파일이름(leon/레온),`Art/Portraits`와Resources의등록정의,현재인계/정본을확인한범위에서 **전용레온PNG/공식Portrait정의미발견**. 현재Portrait정의7개 speakerId 중 arbel-leon 없음. VillageRepresentative 정의도 starter-village-main-guide용이며 portrait fileID0입니다. 따라서 **공식전용초상화와의디자인일치는NOT_VERIFIED_REFERENCE_NOT_FOUND**이고“충돌없음을전수입증”한것이아닙니다. 다른브랜치/PC전체개인원화폴더전수조사는하지않았습니다. 근거 `Temp/Art103Hit109Leon20261117/leon-reference-search.json`.

새나이/가족관계/부상/정식복식설정·새초상화를캐논으로확정하지않았습니다. 원본Mapping/OGA09/Scene/PPU/Pivot/대사·퀘스트/Unity변경0. 승인후별도적용지시가있을때만arbel-leon전용경로와맞춤PPU/Pivot·여타OGA09공유자보호·3해상도/상호작용최소QA를검토합니다.

## 공통 판정·보호·다음 단계

**두ZIP 필수기술검수 모두통과**. Hit4와레온각각최종미술승인필요. 비교판은오프라인동일512캔버스/실제128·40축소이며실제게임크기·PPU·접지·HUD·Animator검증화면이아닙니다. 특히레온의새화풍과40px세부판독,미발견공식초상화참조범위를함께안내해야합니다. 승인만으로자동Unity적용을착수하지않고별도범위/지시와최소QA게이트를따릅니다.

Unity6000.6.5f1·Issue85패키지/GUID HOLD 유지. **Unity 도구호출/실행/Import·제품코드/Assets/GUID/Scene/Save/UserData/Discord 변경0**. Main04/Main07·시작마을NPC12·승인KeyPose/Idle/Attack·기존미커밋/미추적보호. 보호8354파일종료SHA는최종receipt에서허용보고문서외변경0을대조하고비보고git상태byte-exact를확인합니다. 검수사본/파생QA이미지는Temp에만있고외부업로드0.

기존소형문서worktree재사용, 브랜치 `codex/art103-hit109-leon-review-20261117`에이상세보고·CURRENT_STATUS·CODEX_LATEST3문서만로컬commit. rootmainHEAD52f84e643d5d860665eadfe1614a30c745209f00유지, main직접commit/Push/Merge/reset/clean/stash0. 최종로컬SHA/보호대조는Issue103/109댓글·`Temp/Art103Hit109Leon20261117/Final.json`. 문서미Push상태로원격문서링크가유효하다고주장하지않고Issue댓글만실제작성합니다.

증거: `audit.json`·`boards.json`·`Hit4_DecodedGIF.png`·`Hit4/`·`Leon/`·`leon-reference-search.json`. 기존PC외부백업/Issue85HOLD상태는변경하지않았습니다.
