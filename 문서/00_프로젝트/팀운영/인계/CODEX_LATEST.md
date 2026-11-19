## 2026-11-19 · 눈보라 정령 Idle8 v2 승인 기준 자산 반영

- 승인 v2 PNG, 8개 Sprite, Idle 0.96초 반복 클립, 컨트롤러와 시각 프리팹을 원본 프로젝트에 반영했습니다. Unity 6000.6.5f1 격리 임포트·클립 샘플링·프리팹 연결 PASS.
- 정식 M5 등록 및 F4/F5 참조 연결은 후속입니다. v1·KeyPose 보존, Save와 전투·다른 몬스터 데이터 변경 없음. 제품 `32297d6ad37a1be7f31f94e98b57dcfb91a5886e`, 원격 Push 미실행.

[상세 기록](../../Issue112_BlizzardWisp_Idle8_V2_Apply_20261119.md)

## 2026-11-19 · Issue #112 눈보라 정령 Idle8 v2 오프라인 검수

- KeyPose·v1 보존과 v2 ZIP/시트/분리셀/GIF/비교판 확인. v2 본체 띠 회전감 개선, 512/128 회전 방향 명확, 40은 소용돌이 인상 위주. 8프레임·120ms 유지. 미술 승인 권고 및 대표 검토 대기, Unity 미적용.
- [검수 및 비교](../../Issue112_BlizzardWisp_Idle8_V2_Review_20261119.md). 로컬 codex/issue112-wisp-idle8-v2-review, 원격 Push 미실행.

## 2026-11-19 · Chapter 1·2 NPC 리뉴얼 종료

- Unity 6000.6.5f1 실제 격리 Play Mode에서 아르벨 펫 분양·관리·주민·포털 관리인 4명 표시 및 상호작용 50 PASS / 0 FAIL / 게임 런타임 오류 0. 포털 이동 비활성. 아르벨·카타콤 공용 원화 8명의 실제 Sprite와 승인 파일 보존 확인.
- 개인 Save 미접근. 합성 슬롯으로 독침벌 분양 100T·추가 비용 없는 장착 및 기록 검증. 주민·치유사 이름표의 일부 구도 겹침은 이전 캡처에도 있는 배치 특성으로 기록.
- 관련 제품 커밋 `44d4ab5449131ca51876791f5ca3879aaaaf7a5d`, `31a72cf42a890a7c43e32005ba583172a5733256`. 이번 작업은 승인 범위 검증과 종료 문서만 반영했습니다. 원격 Push 미실행.

[종료 보고서](../../Chapter1_2_NPC_Renewal_Closure_20261119.md)

## 2026-11-19 · 아르벨 주민·포털 관리인 승인 원화 적용

- Arbel (1.5,-1.5) 마을 주민의 Scene 인스턴스와 (-7,-3) 포털 관리인의 외형을 승인 PNG로 교체했습니다. 기존 위치·대사·기능 및 포털 비활성을 유지합니다.
- 두 ZIP/PNG SHA와 기술 검수 기록 재대조, 변경 C# 및 Unity 6000.6.5f1 배치 컴파일 통과. 격리 Editor에서 두 PNG의 Sprite 로드, 주민 Scene 참조·좌표·색·기존 Trigger 및 관리인 카탈로그 연결을 확인했습니다. 실제 Play에서 포털 준비 안내와 주민 인사를 확인할 수 있습니다.
- 제품 커밋 `31a72cf42a890a7c43e32005ba583172a5733256`, 로컬 작업 브랜치 `codex/issue118-chapter3-balance`, 원격 Push 미실행.

[상세 적용·검증 기록](../../Arbel_Resident_Portal_Art_Apply_20261119.md)

## 2026-11-19 · Chapter 3 전리품 미술 승인 및 아르벨 펫 NPC 원화 연결

- 승인 아르벨 펫 분양·관리 PNG 2종을 원본에 반영했습니다. 변경 C# 컴파일·개별 SHA 일치. Unity 6000.6.5f1 격리 배치는 컴파일 단계에서 시간 제한으로 중단되어 실제 아르벨 화면·상호작용 확인이 남았습니다.
- 전리품 PNG 6종은 정식 등록 대상 0/6으로 적용 준비만 완료했습니다. 다음 작업은 새 Item ID·드롭 등록 확인 후 연결입니다. 경험치 보상 보류.
- 제품 커밋 `44d4ab5449131ca51876791f5ca3879aaaaf7a5d`, 로컬 `codex/issue118-chapter3-balance`, 원격 Push 미실행.

[상세 적용·검증 기록](../../Chapter3_Art_Apply_20261119.md)

## 2026-11-19 · #116/#117 실물 검수 및 #118 후속

- 지정 ZIP 두 건의 납품 SHA 일치, 독립 PNG 7종 기술 PASS. Chapter2 승인 7종 및 #113/OGA16/OGA17/레온 동일 크기 비교판은 Temp/Issue116_117_118에 보관. 대표 미술 승인 대기 / 게임 미적용.
- Unity 6000.6.5f1 라이선스는 제한 환경의 클라이언트 IPC 실패를 확인했고 일반 실행 환경에서 성공. 이후 배치 시작 지연으로 새 Unity 실행 전투 미검증. 기존 올빼미 증거 재사용.
- 합성 Lv19~21 폭군 36조건 모델과 Main25~27 EXP 비교. +3500/+3500/+4000은 선택전 0회에서 종료 Lv22~23. 기획 승인 필요.
- [후속 검수 보고서](../../Issue116_117_118_Art_And_Balance_Followup_20261119.md). 로컬 codex/issue118-chapter3-balance, 원격 Push 미실행.

## 2026-11-19 · Issue #118 Chapter 3 밸런스 PARTIAL

- 합성 31/56/81전 종료 Lv18~19/19~21/20~22. 로컬 C# 성장 함수 실행, 첫 한기 기존 격리 Unity 실측 재사용. 신규 Unity 실행 Licensing 초기화 실패. F3~5/폭군/화로/스킬25/펫4 실제 연결 및 3인 조합 실측 미검증.
- Main22~24 로컬 0EXP/0T와 승인 보상 차이 및 후보 대사 ID 기록. 원본 게임·Save/UserData 미접근·변경 없음. 메인 진행 EXP 보강 또는 낮은 레벨에서 보스 플레이 검증을 비교하고, 변경 전 승인 필요.
- [검증표·권고안](../../Issue118_Chapter3_Balance_QA_20261119.md) · [Issue #118](https://github.com/HappyPaul812/Project-Limitless/issues/118). 로컬 보고 브랜치 codex/issue118-chapter3-balance. 원격 Push 미실행.

## 2026-11-17 일일 마감 — 원본 승인분 보존·복구 원격 검증

**APPROVED_PRODUCTS_PRESERVED / REMOTE_RECOVERY_PUSH_VERIFIED / LOCAL_FF_SYNC_COMPLETE / NON_GIT_OFFSITE_INCOMPLETE / DAY_CLOSED**.

- 승인제품누적19경로 원본blob동일·NPC12/이름표Y1.37·E/Main04·07/레온/파티/잔여참조보존. 오늘새기능/미적용아트적용/반복QA0. 담당보고15/Codex기록통합·과거HOLD와최종적용구분.
- 얼음갑충전체미술/게임적용승인이나등록데이터없어미적용·기존44PASS재사용. 펫분양독립원화최종승인/게임미적용,눈보라미술미승인/미적용,족제비Defeat미술승인대기,Placeholder미결·#85HOLD유지.
- 복구backup/2026-11-17-safe-checkpoint=1차c23a98fee363ac38dca669b802c49ccc54ecb014 일반Push성공·별도bareGit57경로SHA재수신검증. 원격mainbfd28154유지·main직접commit/Push/Merge0·Clean계보선별/개인키·Save등업로드0.
- 현재8394파일/약1.223GB 보호·어제사본재사용+69파일3.1MB증분. 비Git외부백업미완·로컬사본은오프사이트아님. Unity6000.6.5f1 compileFalse/오류0/경고42·Scene/Play/포커스유지. 원본로컬FF완료·마감문서최종복구반영결과는별도확인,결과Issue114/Final.json.

[상세 마감](../../EOD_20261117_SafeIntegration_Recovery.md).

---

## 2026-11-17 대표 결정 — 펫분양 최종승인 / 얼음갑충 게임적용 승인·등록데이터 막힘

- **#113 USER_ART_APPROVED / UNITY_NOT_APPLIED**: 이번대표직접승인반영·기존ZIP/CRC/Manifest7/SUMS8재확인. 문서/리뉴얼인계만갱신·Arbel(5,-3)기존승인OGA17유지·OGA16/레온/NPC/Placeholder변경0.
- **#103 ART_APPROVED / GAME_APPLY_USER_AUTHORIZED / BLOCKED_MISSING_REGISTRATION_DATA / NOT_APPLIED**: 이전미승인제한을막힘원인으로삼지않음. 실제원본Definition21종M4일치0·IceBeetle자산0·M4F3/F4씬/조우등록없음·수치/승인된재사용대상없음. 이번새스탯금지와기존전투보존을지키며실제연결할대상없어제품변경전중단. 기본값/다른갑충수치임의복사/기존몬스터대체안함. 후속은등록수치또는재사용대상·배치/조우계약확정필요.
- 승인5종SHA불변·DefeatZIP c04ad4e6…CRC/Manifest10/SUMS11재확인,동일재생코드·시트의기존격리44PASS재사용(반복QA0). Idle반복/AttackHit복귀/Defeat최종유지근거유지,실제M4전투QA미검증. 원본6000.6.5f1 compileFalse/오류0/경고42·Bootstrap/PlayFalse/dirtyFalse/포커스null.
- **#112 USER_ART_REVIEW_PENDING / UNITY_NOT_APPLIED** 유지·추가작업0. 제품commit없음·문서5개만·codex/icebeetle-apply-pet113-approved-20261117·실제SHA/보호8356대조는Final.json/Issue103·113댓글. Save/UserData/Discord/#85HOLD/승인원본/dirty보호·main직접commit/Push/Merge0.

[실제 결과와 막힘 상세](../../Issue103_GameApplyAttempt_113_Approval_20261117.md).

---

## 2026-11-17 Issue103 — Defeat4 승인 실물·5종 재생 검수 / M4 계약 HOLD

**ART_APPROVED / UNITY_NOT_APPLIED(CONTRACT_HOLD) / ISOLATED_PREVIEW44PASS / PRODUCTION_QA_PARTIAL / REMOTE_NOT_PUSHED**.

- 최신기획17468d79·Issue103 최종승인 확인. Defeat ZIPc04ad4e6…/3,134,616bytes·CRC/Manifest10/SUMS11 PASS, PNG9cd3fd4c…/2048×512RGBA·912,389bytes·4셀QA/축소본일치·GIF비루프160/190/220/1600ms. 기존KeyPose/Idle/Attack/Hit 승인SHA 모두일치·원본수정0.
- 원본Resources MonsterDefinition21종 중 M4일치0·IceBeetle자산검색0. 기존4종도게임미적용. 기획의 계약없으면격리미리보기만 조건에따라 새ID/스탯/Spawn/제품PNG/meta/클립/컨트롤러/전투/펫 추가0, 원본적용0·제품commit없음.
- 기존QA Unity6000.6.5f1 숨김 재사용·기존재생기 SpriteRenderer+UIImage 44PASS: Idle반복/Attack·Hit복귀/Defeat4셀·최종1.6초유지·공격피격부활방지. 미리보기PPU512/Pivot256,48은임시이며제품확정아님. 런타임오류0/격리Search색인예외1별도·기존compilerwarning701행/신규probe경고0. 원본compileFalse/오류0/경고42·Bootstrap/PlayFalse/dirtyFalse유지.
- 다음:기존계약부재보고후 M4 등록/적용계약 별도확정 필요. 실제전투QA/필드크기미검증·미적용을완료로표기하지않음. 보호8356대조·Save/UserData/Discord/NPC12/레온/Main04·07/Placeholder/#85HOLD유지·Push/Merge0. 브랜치codex/issue103-defeat113-review-20261117·문서SHA는Final.json/Issue댓글.
- 확인:게임좌표없음, 실제Unity캡처 Temp/Issue103Defeat113Review20261117/IceBeetle_ActualUnityPreview.png 및납품GIF.

[상세 보고](../../Issue103_Defeat4_Approved_IsolatedPreview_ContractHold_20261117.md).

---

## 2026-11-17 Issue111 — 잔여 NPC 승인 원화 재연결 / 단색 주민 조사

**LOCAL_ORIGINAL_WORKING_TREE_APPLIED / DELTA160PASS_0FAIL / ORIGINAL_ERRORS0 / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

- 레온#109 완료 후 Catalog 두 경로만 수정: Arbel 분양담당→승인OGA17, 서쪽주민→승인OGA11. Main21 완료 카타콤·시작마을 PetAdoption fallback도 같은 승인OGA17 실물 확인. PNG/meta/GUID/PPU/Pivot 변경0·외부PNG삭제0.
- 기존QA6000.6.5f1 재사용·960/1280/1680 렌더·E/패드A/주민대화/분양 확인·실제100탈렌트결제/합성Save/ESC·B닫기 및 기타NPC속성 보존160PASS. 원본 compileFalse·오류0/경고42·Bootstrap/PlayFalse/dirtyFalse/포커스유지. 3지역×2상태56런타임NPC 중legacy렌더0·외부PNG12보관(게임전체전수아님). Frostway 공통fallback정적영향은같음/실물QA범위밖.
- Arbel(1.5,-1.5) VillageNpc는 초기 VillageNpcPlaceholder 프리팹 잔존·role/StableID없음·일반 인사대사·PlaceholderVisual의 노란1×1런타임Sprite. 외부PNG아님·삭제/역할추가0·향후정리기획미결.
- 레온/NPC12/Main04·07/#106/Save/UserData/Discord/기존dirty 및 #85HOLD보호·대형복제/Push/Merge0. 제품 `e4478f57f2e60a94d32a07b79d00b6fed1cf975e` / codex/issue111-residual-npc-20261117, rootmain52f84e64유지. 8356파일최종보호대조·문서SHA는Temp/Issue111Residual20261117/Final.json·Issue111댓글.
- 대표 확인:Arbel(5,-3)/(-5,2), Main21완료Field03(-2.05,-3.55),시작마을(-5.8,-3.6). 새Play실제수용확인대기.

[상세 보고](../../Issue111_ResidualNpc_ApprovedReferences_20261117.md).

---

## 2026-11-17 Issue109 — 승인 레온 전용 외형 원본 적용 / #110 보존

**USER_ART_APPROVED / PORTRAIT_OUT_OF_SCOPE / LOCAL_ORIGINAL_WORKING_TREE_APPLIED / DELTA178PASS_0FAIL / ORIGINAL_ERRORS0 / ISSUE110_ALREADY_DONE_NOT_REPEATED / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

- Leon ZIPdfaaf5fe…CRC/SHA재확인·기존오프라인PASS재사용. PNGff7cfd08…/GUIDed9108a8402b48d3a8ae5c2b9b4a8769/PPU360.5/Pivot(.5,.083984375)=(256,43),가시높이1.142857WU·발접지0. arbel-leon만전용Main13_Leon_Approved로교체·OGA09/기타참조/좌표(0,3)/대사/Quest데이터불변·초상화0.
- 큰목표판-E중첩재현후레온전용후처리만추가:추적target arbel-leon/화면안때오른쪽286px·상단114px피함. 공용이름표1.37/Resident숨김/E1.48/표식1.92및공용navigation/Main04/07변경0. 제품3파일(신규PNG/meta+Catalog)만원본적용.
- 기존QA6000.6.5f1·178PASS·3해상도HUD ON9화면·모든역할NPC불투명픽셀과큰판겹침0/E상자분리·실제E/패드A·Main13최종대화완료/Main14시작·보고진행/합성Save재진입PASS. 레온분기표적검증이며전체여정/전투전수아님. 원본Import/Resources/compileFalse/초상화NONE·오류0·경고104.
- #110제품6c81a03d…/85PASS완료재확인,두파티맵/Scene실물유지·반복수정0. Main04/07/NPC12/Save/Discord/dirty·Issue85HOLD보호. 제품`c00fbb63c65c67d36a2cd59736f6822c1f3368c3`/codex/issue109-leon-approved-20261117·rootmain52f84e64유지·Push/Merge0. 최종DocsSHA/8354대조는Issue109/110댓글·Temp/Issue109LeonApply20261117/Final.json. 확인:Arbel레온(0,3),대표새Play수용대기.

[상세 적용·검증 보고](../../Issue109_Leon_Approved_OriginalApply_20261117.md).

---

## 2026-11-17 Issue110 — 카타콤·아르벨 파티 안내인 외형 참조 수정

**LOCAL_ORIGINAL_WORKING_TREE_APPLIED / TWO_MAPPING_ONLY / DELTA85PASS_0FAIL / ORIGINAL_ERRORS0 / OTHER_LEGACY_REPORTED_NOT_CHANGED / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

- safezone-catacomb-party/arbel-party-guide 두경로만기승인OGA10_PartyManager_HandFix_v2_Approved로수정. PNG6be1a2d7…/GUID012b7ee363574fe3900bbd56a472b984/PPU360.5/Pivot255.5,42 원본불변. 카탈로그2줄외매핑/이름표1.37/E정책·NPC위치/대화/편성기능·PNG/meta/GUID변경0.
- 기존QA/6000.6.5f1·85PASS·960/1280/1680/접지/이름표/실제E·패드A·편성취소/전후열확정/합성Save/재진입PASS. 원본두맵/Resources/compileFalse/오류0·경고42. 보호8354대조최종은Final.json.
- 실제잔여외부:Arbel펫분양OGA17/서쪽주민OGA11,Main21후카타콤·시작마을동적펫분양도OGA17fallback. 나머지기본기능NPC승인원화·세린공식Sprite. Arbel(1.5,-1.5)마을주민은외부PNG아닌활성1×1단색placeholder. 모두보고만·미변경. 구형PNG12종중관찰3Scene/2상태실사용2종·미표시보관10종(전프로젝트미사용입증아님/삭제0).
- 제품`6c81a03d229f0075b39dcbfa07656182e7f6bae5`/codex/issue110-party-appearance-20261117·rootmain52f84e64유지·Push/Merge0. Main04/07/NPC12/Save/Discord/dirty·Issue85HOLD보호. 확인:Field03(2.85,-2.55)/Arbel(-3,-2)파티안내인,옆에서E/A. 다음은대표새Play수용·나머지잔여는별도지시.

[상세 수정·잔여 감사](../../Issue110_PartyGuide_LegacyReferenceFix_20261117.md).

---

## 2026-11-17 Issue103 Hit4·Issue109 레온 — 오프라인 검수

**BOTH_FILE_TECH_PASS / HIT4_ART_REVIEW_PENDING / LEON_USER_ART_APPROVAL_PENDING / BOTH_NOT_APPLIED / UNITY_NOT_IMPORTED / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

- Hit4 ZIP982328b9…3131456bytes/12파일·CRC/Manifest10/SUMS11 PASS.2048×512RGBA·4셀·분리QA동일·하단y430..511RGBA4/4승인원본동일·마지막KeyPose/QA파일바이트동일.처음3셀은승인Idle/Attack어느셀과도전RGBA동일하지않음. GIF실제90/160/110/190ms·550ms반복/직접축소·비교판PASS.피격→움츠림→복귀/파손·과장점프·새VFX발견0·40px세부약함·최종미술승인필요.
- Leon ZIPdfaaf5fe…732482bytes/9파일·CRC/Manifest7/SUMS8 PASS.512RGBA/직접128·40/실제OGA09·승인OGA20·Main07비교판PASS.묶은머리/수염/적갈어깨천/청록복/장화·주머니로OGA09와구별·책임자/실용복읽힘·부상0.세밀한명암의화풍조화는대표승인필요. 현재로컬Portrait정의7/관련Assets파일명/정본에서레온전용공식초상화미발견(일치NOT_VERIFIED),전PC/모든브랜치조사아님.
- 게임코드/Assets/GUID/Unity도구/Import/Save/Discord변경0·Main04/07/NPC12/승인원본·Issue85HOLD보호. codex/art103-hit109-leon-review-20261117 문서3개만로컬commit·최종SHA/8354보호대조는Issue103/109댓글·Temp/Art103Hit109Leon20261117/Final.json. 다음:각각미술승인후별도최소QA/적용지시,자동적용0.

[대상별 상세 검수](../../Issue103_Hit4_Issue109_Leon_OfflineReview_20261117.md).

---

## 2026-11-17 Issue108 — Main07 승인 여행자 원본 적용

**USER_ART_APPROVED / LOCAL_ORIGINAL_WORKING_TREE_APPLIED / DELTA134PASS_0FAIL / ORIGINAL_ERRORS0 / MIEL_REFERENCE_ONLY / USER_REALPLAY_PENDING / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

- 승인ZIPb943fb91…CRC/SHA재확인·기존오프라인PASS재사용. PNG90c727b9…/GUID2cf0a7d9462740c58756aca0f18f3391/PPU430/Pivot(.498046875,.08984375)=(255,46),가시높이.8WU·접지0. 기존(4.4,.2)/scale1/sorting8/white유지. 제품3파일(새PNG/meta+Main07Flow)만원본적용.
- Main07전용큰목표판만왼쪽286px분리/상단114px회피·실제목표/거리/방향유지. Main04/공용navigation/#106/NPC12변경0. 실제폴1.044WU대비앉은높이76.6%. **현재Main07미엘필드Sprite없음**:기존상호작용/대사유지,공식미엘같은.72축척QA참고비교만하고새미엘배치0.
- 기존QA/6000.6.5f1·134PASS·8목표/E/F/A/Story전투진입·도망재도전/승리결과처리복귀/재회/작별/완료숨김/합성Save읽기·재진입PASS. 승리는QA결과처리호출(전체플레이아님).3해상도4단계HUD ON12렌더·통상1.1WU거리범위얼굴/안내겹침검사,임의중첩좌표전체보장아님. 원본Import/compile/Resources·오류0·경고104.
- 제품`f1b0fc9258003d3c12e9c1f1bd830dda84f51b21` /codex/main07-traveler-approved-20261117·rootmain52f84e64유지·Push/Merge0. 최종DocsSHA/보호8352대조는Issue108/Temp/Issue108Apply20261117/Final.json. 확인:Field_02바퀴자국조사후여행자(4.4,.2),폴(6.5,-2.35)→복귀(5.2,.2). 다음은대표실제Play수용·미엘실Sprite추가는별도범위.

[상세 적용·검증 보고](../../Issue108_Main07_Traveler_OriginalApply_20261117.md).

---

## 2026-11-17 Main04 가림 수정·원본 적용 + Attack103/Main07_108 오프라인 검수

**MAIN04_LOCAL_ORIGINAL_APPLIED / DELTA65+59PASS / ORIGINAL_ERRORS0 / ATTACK4_FILE_TECH_PASS_ART_PENDING_NOT_APPLIED / MAIN07_FILE_TECH_PASS_USER_ART_PENDING_NOT_APPLIED / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

- Main04 만남Y5.5→3.4·여행자(8.15,5.15)→(8.85,3.05)·전용LabelY1.15→1.65. 미엘전용 E1.48/54→1.66/40·표식1.92/50→2.08/36. 현장근처만Bounds내120px프레이밍·Main04파란목표판기존빈공간선택재사용. #106공용1.37/E·공용Camera/원본Scene/글자색크기불변.
- 제품4파일(새PNG/meta+Main04Flow+QuestNavigationPresenter)만원본working tree적용. PNG2687c1c6…GUIDf325180de4944364a0dd74c1a814d6df/PPU360/Pivot(.5,.08203125).65기능(승리는QA결과처리호출)·59최종화면표적PASS·3해상도/4방향/실불투명픽셀과글자·파란판겹침0. 원본실제Import/compile/Resources/Console오류0·경고104. 대표새Play수용대기.
- Attack4 ZIPebb41720…12파일CRC/Manifest10/SUMS11 PASS·2048×512RGBA·하단접지4/4동일·마지막KeyPose전픽셀동일(첫장준비자세차이).GIF180/110/130/180ms·축소/보드직접원본일치. 최종미술승인필요·미적용.
- Main07 ZIPb943fb91…9파일CRC/Manifest7/SUMS8 PASS·512RGBA/직접128/40/실제비교판PASS. 회색앞머리/자주외투/바닥지지·다리붕대로Main04·주민구별/고어0·동일인캐논확정0. 최종미술승인필요·미적용.
- Main07/승인NPC12/Save/Discord/dirty·Issue85HOLD보호. rootmain52f84e64유지·제품`3f66fdb25d9445738f8b484d065d92d84e462442`/codex/main04-layout-art103-108-20261117·Push/Merge0. 최종DocsSHA/보호대조는Issue103/107/108댓글·Temp/Main04Fix_Art10310820261117/Final.json. 확인:Field_01 미엘(7.55,3.5)/여행자(8.85,3.05),Main04진행중.

[상세 적용·검수 보고](../../Main04_LayoutFix_Attack103_Main07_108_Review_20261117.md).

---

## 2026-11-17 Issue107 — 승인 Main04 여행자 조건부 적용 검증 HOLD

**USER_ART_APPROVED / FILE_TECH_PASS / FUNCTIONAL44PASS / VISUAL_DELTA_FAIL / ORIGINAL_NOT_APPLIED / REMOTE_NOT_PUSHED**.

- ZIP286fe97b…/PNG2687c1c6… 재확인·승인 유지. 전용512RGBA 후보 GUIDf325180de4944364a0dd74c1a814d6df·PPU360/Pivot(.5,.08203125)·앉은가시높이.966667WU, 접지0·기존LabelY1.15/tint.82/sorting7/위치 유지. 원본 제품3파일 미적용, 후보Temp/격리QA에만 보존.
- 기존QA/6000.6.5f1·기능44PASS·실제E/전투진입/도망재도전/승리결과처리복귀/완료후숨김검사, 오류0. 승리는QA의기존EndBattle(true)결과처리호출이며전체전투플레이아님. 최종PPU360/3해상도추가표적렌더PASS와시각합격은구분.
- **시각FAIL:**960/1280/1680 기존OGA07에서도현재목표/F10상단HUD가미엘얼굴·여행자이름표가림;HUD숨김진단에서도이웃미엘/StoryLabel간섭. PASS조건미충족으로원본적용HOLD. 기존Scene/Camera/Bounds원본-QASHA동일. 공용HUD/배치임의변경0.
- Main07/승인NPC12/Save/Discord/dirty·Issue85HOLD보호. 원본Bootstrap/PlayOFF/compileFalse/오류0·파일변경0. rootmain52f84e64유지. codex/main04-traveler-approved-20261117 문서만로컬commit·최종SHA는Issue107/Temp/Issue107Apply20261117/Final.json. 다음:Main04한정가림해소범위결정→동일DeltaQA→PASS후원본반영. 미술재승인불필요.

[상세 검증·HOLD 근거](../../Issue107_Main04_Traveler_ConditionalApply_HOLD_20261117.md).

---

## 2026-11-17 Issue103·107 — Idle4·Main04여행자 오프라인 검수

**BOTH_FILE_TECH_PASS / IDLE4_ART_REVIEW_PENDING / TRAVELER_USER_ART_APPROVAL_PENDING / UNITY_NOT_IMPORTED / NOT_APPLIED / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

- 얼음갑충ZIP8a9ecec0…3010425bytes/12파일·CRC/Manifest10/SUMS11 PASS. 2048×512RGBA4셀·독립승인KeyPose와Frame00RGBA/QA00파일바이트동일·하부y397~5114셀동일. 미세갑각움직임/접지안정·특별이상프레임없음.128/40보이는RGBA·알파직접축소동일, GIF원본양자화4/4동일·반복·실제120ms(약8.33FPS,8FPS표기주의).
- 여행자ZIP286fe97b…650099bytes/9파일·CRC/Manifest7/SUMS8 PASS.512²투명RGBA1·128/40전RGBA직접축소동일·실제OGA07legacy32/승인OGA07/새여행자분리비교. 앉은자세·팔붕대·여행장비로주민구별/고어0·40px부상세부약함. 새부상캐논확정0·사용자승인필요.
- Unity/원본게임/Scene/GUID/Import/Save/Discord변경0·Unity6000.6.5f1/Issue85HOLD유지. 문서3개만로컬commit·codex/art103-107-offline-review-20261117, 최종SHA/보호대조는Issue103/107댓글·Temp/Art103107Offline20261117/Final.json. 다음:두대상각각미술승인후별도최소UnityQA/적용지시,자동착수0.

[대상별파일·미술검토 상세](../../Issue103_107_Offline_FileArtReview_20261117.md).

---

## 2026-11-17 Issue104·105 — 주민4·독립 남문경비 검증·원본 적용

**FILE_TECH_PASS / TAEON_DIRECT_COMPARE_DISTINCT / BOTH_LOCAL_WORKING_TREE_APPLIED / DELTA118PASS_0FAIL / ORIGINAL_IMPORT_COMPILE_CONSOLE0 / USER_REALPLAY_PENDING / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

- ZIP807f7d6b…/2166760bytes·CRC/Manifest12/SUMS13 PASS. PNG2 512²투명RGBA·실제128/40일치. OGA19기존주민구별, 새OGA20을공식태온초상화/실제필드용Idle00Sprite와직접비교해강한동일인/가족유사성발견0. 구형닮은OGA20시안HOLD별도보존.
- 신규PNG2/meta2+Catalog4매핑줄(제품5파일)만. 둘다PPU367.5/Pivot(.4990234375,.076171875)·가시높이1.142857WU. OGA20남문/카타콤/ArbelPortal3공유외형검증. 기존StableID/역할/대화/Collider/Scene/승인NPC10·태온원본/Saves/Discord보호.
- Unity6000.6.5f1기존QA재사용118PASS·960/1280/1680/발접지/실제E·패드·재진입PASS. 원본MCPImport·Resources/GUID/PPU/Pivot·compile완료/Console오류0(경고42). #106이름표Y1.37/E코드SHA불변·주민4/Portal숨김정상. 기존격리Search예외별도·Issue85HOLD유지.
- 제품 `a0d5674bc83ddae5223cc4c62f23b2889876f2ed` / `codex/npc104-105-approved-original-20261117`. 원본working tree적용·main52f84e64유지·Merge/Push0. 최종DocsSHA는Issue104/105댓글·Temp/Issue104105NPC20261117/Final.json. 확인: 주민4(7.2,0),남문(2.4,-5.1),Field03감시인(2.85,-4.55),ArbelPortal(-7,-3). 다음은대표새Play수용.

[태온비교·제품5파일·실제QA 상세](../../Issue104_105_OGA19_OGA20_OriginalApply_20261117.md).

---

## 2026-11-17 Issue106 — NPC 이름표·승인된 E안내 상향 적용

**LOCAL_ORIGINAL_WORKING_TREE_APPLIED / DELTA155PASS_0FAIL / ORIGINAL_COMPILE_CONSOLE0 / USER_REALPLAY_PENDING / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

- NameplateY1.02→1.37(+.35). 실제alpha상단/글자·그림자bounds기준+.15/.20은미해소, 최종머리간격.0416~.0639WU. E상자충돌을보고한후대표가“E 안내 상향도 허용하고 검증 계속”추가승인. 표시이름표NPC E1.48→1.87, 기존Quest표식활성시안내만표식위(현재2.48). 주민/별도NPC E1.48유지·퀘스트표식원본변경0.
- 기존QA3지역/960·1280·1680·17이름표/30전후실제렌더·155PASS. 얼굴/E/표식분리·잘림·이름표상호겹침0·도보/휠체어E·패드A·주민숨김/NPC102매핑보존. 격리기존Search환경예외별도·Issue85HOLD유지.
- 원본MCP표적Import·실제Offset1.37/컴파일완료/Console오류0(경고42). Bootstrap/PlayOFF/dirtyFalse/선택·포커스보존. 카탈로그Y+설명주석/NpcInteractionPrompt2코드만원본working tree적용·보호8346중그외8344SHA동일. PNG/meta/GUID/PPU/Pivot/Collider/매핑/Save/Discord보호.
- 제품 `4f62cce85c05ccf1b7de5f3f5ef0aae3c9620ebf` / `codex/ch1-npc-nameplate-clearance-20261117`. main52f84e64그대로·Merge/Push0. 최종DocsSHA는Issue106댓글/Temp/Issue106Nameplate20261117/Final.json. 확인: 은행(4.4,2), 훈련장(6.4,-2.1), 대표(0,1.5) 새Play. 다음은대표실게임확인.

[실측·전후·최종검증 상세](../../Issue106_NpcNameplate_Clearance_20261117.md).

---

## 2026-11-17 Issue102 — 승인 OGA11·OGA13 로컬 원본 적용

**USER_ART_APPROVED / LOCAL_ORIGINAL_WORKING_TREE_APPLIED / DELTA_53_PASS / ORIGINAL_IMPORT_COMPILE_PASS / ORIGINAL_CONSOLE_ERRORS_0 / USER_REALPLAY_PENDING / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

- 승인NPC2 PNG원본SHA보존·독립GUID2·시작마을StableID외형매핑2줄만 교체(제품5파일). OGA11 PPU358.75/Pivot(.4990234375,.080078125), OGA13 PPU360.5/Pivot(.5,.080078125), 가시높이1.142857WU. 기존8/OGA20HOLD/Arbel기존OGA11·Scene/대화/Collider/Nameplate/Save/Discord보호.
- Unity6000.6.5f1 기존QA재사용53PASS·960/1280/1680/발접지/주민이름표숨김/실제E·패드대화/재진입 PASS. 원본숨김Batch Import·compile·Resources/GUID/PPU/Pivot PASS·Console오류0(경고104). 격리기존Search시작예외별도, Issue85HOLD유지. 창/포커스조작0.
- 제품 `b7ace6b7f7958b4d74457f4c31db01e667b28856` / codex/npc102-approved-original-20261117. 원본은검증제품5파일 **working tree적용**, mainHEAD52f84e64유지·Merge/Push0. 기존dirty보호위해원본main에제품commit합류를주장하지않음. 완료문서3개만후속Docscommit, 최종SHA는Issue102댓글/Temp/Issue102NPC20261117/Final.json.
- 확인: 남동주민2(3.2,-2.6), 서쪽주민3(-7.2,0). 일반주민상시이름표숨김정상·E/패드A기존대화. 다음은대표새Play확인; Defeat4승인/Unity적용확대0.

[적용·검증 상세](../../Issue102_OGA11_OGA13_OriginalApply_20261117.md).

---

## 2026-11-17 Issue102·100 — NPC2·SnowStoat Defeat4 오프라인 검수

**FILE_TECH_PASS(각 ZIP) / NPC_STYLE_REVIEW_PENDING / DEFEAT4_ART_REVIEW_PENDING / USER_ART_APPROVAL_PENDING / UNITY_NOT_RUN / GAME_NOT_APPLIED / REMOTE_NOT_PUSHED**.

- NPC102 ZIP c6e2268f…/1436792bytes·CRC/Manifest11/SUMS12 PASS. OGA11/13 512²투명RGBA2장·직접128/40프리뷰4개 픽셀동일·승인NPC8실물SHA동일·비교판10종 라벨/실물일치. 후보 구별가능, 화풍은 OGA07/10에 가깝고 최종승인대기.
- Defeat4 ZIP34f23d12…/1454055bytes·CRC/Manifest10/SUMS11 PASS·메타순환0. 시트8bc97ce0…2048×512/4셀·분리4셀동일·낮아짐→엎드림→마지막유지/회복0. 128/40보이는RGBA·알파직접축소동일(완전투명RGB차이별도), GIF원본MEDIANCUT변환4/4동일·비반복120/120/120/1750ms. 승인KeyPose/Idle4/Attack4v2/Hit4실물SHA4/4보호.
- Unity6000.6.5f1·Issue85HOLD·Save/UserData/Discord/승인원본보호. Import/게임적용/Assets수정/Push/Merge0. 기존 소형 문서worktree재사용, codex/art100-102-offline-review-20261117에 문서3개만 로컬commit. 최종SHA/보호대조는Issue100/102댓글·Temp/Art100_102Offline20261117/Final.json. 다음은대표최종미술승인, 적용자동착수0.

[실물·SHA·검수 상세](../../Issue100_102_Offline_DeliveryReview_20261117.md).

---

> 최종확인: 백업8342 SHA PASS·bundle verify0·PNG6동일·기존비보고status동일. 마지막관련commit `e0ac9a1f16aeb9f44c32b2f6ed4aab48089ed702`. F: 321531150336bytes. 외부백업/Unity표적QA HOLD.

## 2026-11-17 Issue85 — 기존 수정 진단·외부 백업 BLOCKED

**OFFSITE_BACKUP_BLOCKED_NO_DESTINATION / STATIC_PASS / UNITY_QA_NOT_RUN_HOLD / REMOTE_NOT_PUSHED**.

- 내부 NVMe/SATA2개·C/D/E/F Fixed만 확인, 승인된 PC 밖 비공개 목적지 미확인. 임의 업로드/암호생성0. 어제 bundle/8342 보호목록 재사용; SHA·보호·실제 F:와 마지막 commit은 Temp/Issue85Resume20261117/Final.json 및 Issue85 댓글에 기록.
- Git15.1.0/14.0.3/3.0.1·GUID6중복과 원본16.0.1/15.0.1/4.0.1·GUID6고유를 분리. 현재 Assets GUID 중복0. 기존 변경을 재수정/제품커밋하지 않음. 전체 manifest/lock에는 세 패키지 외 기존 차이도 있음.
- 원본Tooling4 오류대상8파일 읽기 성공. 기존격리Tooling3은 열거되나 일반 긴 경로8파일 읽기 실패. 원인/Import 예외 해소 미확정. A/B 안전조건 중 외부 백업 미충족으로 Import/컴파일/GoldenArrow8·Healer5 실제 로드 NOT_RUN/HOLD. 현재 Unity 프로세스/연결Editor없음·live Console미검증, Editor실행/포커스조작0.
- 원본main52f84e64 유지·Save/UserData/Discord/승인아트/기존dirty보호. 76오래된worktree NOT_VERIFIED 유지. 기존 소형 문서worktree 재사용, 별도 codex/issue85-existing-fix-audit-20261117 문서만 로컬 commit·Push0. 다음: 승인 외부 암호화 복구수단 확보 후 표적QA 게이트 결정.

[상세 진단](../../Issue85_ExistingFix_OffsiteAudit_20261117.md).

---

## EOD 2026-11-16 — 안전 통합·원격 부분 복구 / 오늘 개발 종료

**LOCAL_APPROVED_DOC_SYNC / BACKUP_BRANCH_PUSH_DONE / REMOTE_MAIN_PUSH_HOLD / OFFSITE_ASSET_BACKUP_INCOMPLETE / DAY_CLOSED**.

- 원본시작main977f3b6c, 별도codex/eod-safe-integration-20261116에서완료Markdown19+담당인계3/EOD/CODEX/CURRENT만선별하여FF-only동기화. NPC02/17·10/07이미적용보존, StoatKeyPose/Idle4/Attack4v2/Hit4미술승인·Unity미적용,Defeat4미제작. 제품/패키지/GUID수정0.
- **원격backup/2026-11-16-safe-checkpoint=9d84b81934a7cff7dcf4bf5083de3f1f45ea7201 일반Push성공**. 신규383객체전수범위/비밀패턴/금지자료/제외이력0. 빈소형bareGit에HTTPS재fetch하여NPC9파일/README바이트확인. Clean계보선별복구본/릴리스아님/원본main전체미러아님.
- 원격main=bfd281545632877325f5d21e04cefb924042c465 유지·Push미실행. Issue85 Import예외/Git트리GUID6중복HOLD유지, 현재원본dirty의package/GUID수정과구분. 원본6000.6.5f1/compileidle/기존QA재사용. 기존미커밋package16.0.1/15.0.1/Tooling4.0.1을임의커밋하지않음.
- 원본dirty302/untracked550·8342파일보호,기존backup8315재사용+delta27만복사/SHA일치. 210branch중미Push171·160worktree목록(81status확인/76timeout/3기존경로불가)·비GitDownloads1479파일목록. Save/UserData/아트/TTS/영상/Discord파일변경0. **비공개외부저장소미확인/비Git오프사이트미완료**. F:백업과로컬bundle은PC고장전체복구아님.
- 최종로컬SHA/서버SHA/F:여유/로컬bundle결과는Issue85및Temp/EOD20261116/Final.json. 다음은사용자재개지시후외부암호화복구/Issue85별도검증, **오늘추가작업자동시작금지**.

[EOD 전체 결과](../../EOD_20261116_SafeIntegration_Recovery.md).

---

## 2026-11-16 Issue101 — 승인 OGA10·OGA07 v2 원본 게임 적용

**APPROVED_V2_LOCAL_ORIGINAL_APPLIED / DELTA_53_PASS / ORIGINAL_CONSOLE_ERRORS_0 / USER_REALPLAY_PENDING / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

- 이번 바울 대표 최종 미술승인/적용지시를 근거로, `codex/npc101-approved-original-20261116` 제품 `81f8a45df4182ad2aa64a71b5408217616b5b3d0` 5파일만 원본에FF-only/no-autostash 반영. PNG2 원본SHA일치·독립GUID, StableID2개 Resources매핑만 교체. main직접commit/Push/PRMerge0.
- Unity6000.6.5f1·둘다PPU360.5/Pivot(.4990234375,.08203125)/가시높이1.142857WU.960/1280/1680·발접지·이름표·실제E/패드대화·기존6종/타Scene참조 보존 **53PASS**. 주민1은 기존상시이름표숨김 유지. 파티안내인은 초기진행 동적대사 유지, 게임로직 변경0.
- 원본 Import/compile/실제Resources·GUID·PPU·Pivot/Console error0. QA의 기존UnityEditor.Search색인오류는별도환경로그(전체QA Console0 주장은아님). Issue85패키지변경0/HOLD유지.
- 기존4678파일SHA대조에서허용카탈로그만변경, 사용자git상태바이트동일. 승인6NPC/OGA20/Scene/Save/UserData/Discord보호. 기존QA/소형worktree재사용, 복제/대형백업/캐시삭제0. 원본Scene/Play/선택/포커스보존.
- 바울 대표확인: 시작마을서쪽 파티안내인(-6.4,-2.1), 주민1(-3.2,-1.2). 다음은직접실게임확인, 추가아트재작업자동착수없음. 후속Docs최종SHA는Issue101/Temp/Issue101NPC20261116/Final.json기록.

[적용·검증 상세](../../Issue101_OGA10_OGA07_v2_OriginalApply_20261116.md).

---

## 2026-11-16 Issue #99 — OGA02·OGA17 승인 원화 로컬 원본 적용

**LOCAL_ORIGINAL_APPLIED / APPEARANCE_DELTA_62_PASS / USER_REALPLAY_PENDING / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

- 사용자 최종 적용 승인에 따라 `codex/ch1-oga02-17-original-apply-20261116` 제품 `7b84fa76e8948bb09febdf688898cdaa1ef231c8`의 5파일만 로컬 main에 FF-only/no-autostash 동기화. main 직접 커밋·Push·병합 커밋 없음.
- Unity 6000.6.5f1 고정. 승인 PNG 원본 SHA 동일. OGA02 PPU357.875/Pivot(.5,.083984375), OGA17 PPU358.75/Pivot(.4990234375,.08203125), alpha≥8 가시 높이 각각1.142857WU.
- 재사용 QA 960/1280/1680×720·발 접지·이름표/Collider·기존 안내 대화·키보드/패드·잡화점 실제 마우스 구매/판매·가격·Scene 재진입 62PASS. **장비상인 거래는 기존 미구현(N/A); 훈련장은 기존 안내 대화만 유지.** 월드 NPC 마우스 직접 대화도 기존 미지원. 기능 추가 없음.
- 원본 Import·compile idle·Resources/GUID/StableID 연결 확인, Console error0. 기존4674파일 대조에서 허용된 카탈로그만 변경, 사용자 git status 바이트 동일. OGA03/06/09/16·HOLD OGA20·기존 OGA17 Arbel 참조·Scene/가격/Save/UserData/Discord 보존. 원본 Scene/Play/선택/포커스 조작 없음.
- 대표 확인: 시작 마을 동쪽 훈련장 관리인(6.4,-2.1), 장비상인(5.8,1.2). E/F 또는 패드 A로 기존 안내 확인. 장비 거래 신규 구현으로 오해하지 않을 것.

[적용·검증 상세](../../Issue99_OGA02_OGA17_OriginalApply_20261116.md).

---

## EOD 2026-11-11 — 승인 문서 로컬 통합 / 원격 main Push HOLD

**LOCAL_APPROVED_DOC_SYNC / REMOTE_MAIN_PUSH_HOLD / USER_WORK_PRESERVED / DAY_NOT_CLOSED**.

- 실제시작main469b219ffbdbf4c8e5b51e977bbe2940eb6119a2, origin/main bfd281545632877325f5d21e04cefb924042c465. 원격은조상/로컬67커밋선행. 승인제품anchor11/11이미포함·게임재적용0. 누락승인보고8/최신인계3+EOD보고/CODEX/CURRENT만 별도codex/eod-approved-main-push-20261111에서선별, 문서커밋을로컬FF-only/no-autostash로동기화합니다. 최종main은동기화후본EOD브랜치HEAD이며실제SHA는Issue85/Temp/EODApprovedPush20261111/Final.json에기록합니다.
- **Push 미실행**: 전체전송이력67커밋/721경로에97992cda의로컬사용자meta보관5개 및미채택PilotBee비교원본·생성검사기포함. 이번제외조건과충돌하며삭제커밋만으로부모이력전송을막을수없습니다. FF-only/강제정리금지조건아래임의이력재구성/범위확대0. REMOTE_MAIN_PUSH_SUCCESS/DAY_CLOSED 선언0.
- private백업8322/1220459912bytes·원본/백업SHA일치·dirty302/untracked550보호. Unity6000.6.5f1/compileidle/Console0·Scene/Play/선택/포커스보존. 새코드0이므로81/99/68/46전수반복0. 키패턴검출0/50MiB이상0이나모든기밀부재보증아님. 원격protectedfalse/rulesets[]/push권한확인, protection상세API403/우회0.
- [전체감사/승인대조](../../EOD_20261111_Integration_PushAudit.md), [전송전체명세](../../EOD_20261111_PushScope_Manifest.md). OGA03/09/06/16·BG사용자수용, Owl정식81PASS제품469b219f는대표새실플레이 PENDING. VFX소유자/몸체밀집PARTIAL·동상/동결괴사/정화Ⅱ/아이템/Discord/C SnowEdge/OGA20HOLD유지. 추가작업자동시작0.

---

## 2026-11-11 Issue95·91 — 승인 아트4종 원본 로컬 적용 완료

**APPROVED_ART_4_LOCAL_ORIGINAL_APPLIED / UNITY_IMPORT_DELTA_QA_PASS / USER_REALPLAY_PENDING / REMOTE_NOT_PUSHED**.

- 바울 대표의12파일 본편 적용 승인에 따라 검증 제품 `d40bb17057da555322993c1b1c2fd7944b95bf78` 그대로 원본에 FF-only/no-autostash 반영. 별도 `codex/art95-91-original-apply-20261111`. 신규10/수정2, 새그림·Scene/Prefab/Collider/기존GUID/meta변경0. OGA06/16 PPU329/320.25·발끝Pivot·StableID5, Snowroad/F2 v2·독립Ch3Catalog 유지.
- 기존107PASS 재사용/전수재실행0. 추가 격리 Import7PASS/원본 Import·compile·Resources9PASS/Console오류0. Scene·PlayOFF·선택·포커스 보호. F2B352·Ch1/2·Save/UserData·dirty302/untracked550 보존, 보호8294 중 제품코드2만 변경·그외8292SHA동일(완료문서2파일 별도허용).
- 시작main `3a44dd0483759dc4921b22a914853395701789d7`→제품d40bb170. 완료보고도 작업브랜치에서 commit→FF; 최종SHA는 Issue95/91 완료댓글·Temp/Art95_91Apply20261111/Final.json. main직접commit/원격Push/Merge0.
- [적용·QA·실플레이 안내](../../Issue95_91_ApprovedArt_OriginalApply_20261111.md). 캡처는 기존107 실제GameView 재사용이며 원본 신규Play촬영 아님. 추가 배포승인 대기 없음. 다음은 바울 대표의 새Play→마을 치유/은행 E·Field03/Arbel 재사용 외형·Chapter3 서릿길/F2 전투 배경 실플레이 수용 확인. 격리 MCP 환경로그 미결은 별도, 원본Console0.

---

## 2026-11-11 Issue75 — 승인 B352 제품5파일 원본 적용 완료

**F2_B_LOCAL_ORIGINAL_APPLIED / UNITY_DELTA_QA_PASS / USER_REALPLAY_PENDING / REMOTE_NOT_PUSHED**.

- 사용자 최종 승인 “어. 승인.”. 제품commit **c951e0c533ac5e1a12090ea446fb05c8fd91fd61**(정확5파일). 신규 Builder/FirstSnow+meta4·F2Scene1, 기존컴포넌트/블록불변·Root/Builder57줄추가. GUID6dc8e6f8a6804021aa21cf12e31105f3 / 1f2b49e1abdb4374b2376945c6778c10. 새그림·Pine/Shrub/C/HUD이식0.
- Unity6000.6.5f1 새37PASS/0FAIL: 실제입장자동Start352/중복방지·3화면비·휠체어왕복1회·퇴장/재입장/2차퇴장 소유자원해제. 파일럿33·기존24/68/90/81 전수반복0. Owl그림영역 밖1280픽셀0·배치manifest동일, 기존HUD가림은유지. Runtime/C#오류0·기존Editor환경OPEN분리.
- [상세·실제GameView·정확5파일](../../Issue75_F2_B352_OriginalApply_20261111.md), Evidence/F2B352Apply20261111. 원본보호5130/649825058bytes 백업·SHA/status302·550 동일. 시작main7e8c9f1b, branch codex/f2-b352-original-apply-20261111.
- 원본main7e8c9f1b→제품/보고da34c933 FF-only/no-autostash 완료. 포커스 전환 없이 원본Refresh/compile 후 Builder로드·GUID2·PPU128/중앙Pivot·Console0·기존Scene/dirtyfalse/PlayOFF/selectionnull 유지 확인. 보호5130 중 승인Scene1만 변경·나머지5129SHA 동일·status302/550 byte-exact 동일. 완료문서도 작업브랜치commit→FF하며 최종SHA는 Issue75/Temp Final.json 참조.
- main직접commit/원격Push/Merge0. 추가 적용 승인 대기 없음. 바울 대표 새Play→기존Main24 서릿길마을 북문→F2 굽은길 왕복·퇴장/재입장 실플레이 확인만 남깁니다.

---

## 2026-11-10 EOD — 승인 작업 통합·로컬 안전 동기화 마감

실제 EOD 검증 완료: 원본 main에 e999b4c310b519138cfb3315f1b2d7bd00e54ea0을 FF-only/no-autostash 반영했습니다. 사용자 상태302/550 byte-exact 동일, 보호8244개 SHA 동일, 허용 인계문서2개만 변경, Unity 제품 변경0. Unity6000.6.5f1·Scene·Play·선택·포커스 유지, 컴파일 중 아님·Console 오류0. 최종 완료 기록 commit도 같은 방식으로 반영하며 최종 SHA는 Temp/EOD20261110/Final.json 및 Issue 댓글에 기록합니다.

EOD_20261110_LOCAL_MAIN_SYNC_SUCCESS / USER_WORK_PRESERVED / REMOTE_NOT_PUSHED / DAY_CLOSED


- 승인 제품 #96 HP·MP, #97 맹독뱀, #94 던전 충돌, #93 및 앞선 Bee/Spider/펫/Chapter2/TTS는 모두 `ALREADY_IN_LOCAL_MAIN`. 대표 실플레이 수용 반영, 제품 재적용·기존 QA 반복0.
- 출처 감사 #57의 공식 최종 문서3개만 선별 통합합니다. Chapter1 잔여7종·기본펫3종 `ORIGINAL_CONFIRMED`; 슬라임은 대표 원본 선언으로 마감, 존재하지 않은 ZIP의 픽셀 검증은 `NOT_RUN`입니다.
- F2 B/C·SnowEdge, NPC OGA06/16, Ch3 BGv2, Shoot8, 미승인 냉기·VISUAL_PARTIAL·Discord는 `HOLD`, 통합 제외.
- 시작 main `8430f40026ac9258f2ef3323b55dfb4983f872a7`; 작업 브랜치 `codex/eod-safe-sync-20261110`. 문서 전용 commit 후 `--ff-only --no-autostash`로 안전 반영하며 main 직접 commit·원격 Push/Merge0.
- 원본8246파일/1,210,483,913bytes 백업·SHA 재읽기 PASS. tracked dirty302/untracked550 및 Save/UserData·Unity6000.6.5f1·Editor 상태 보호. 제품 파일 변경0으로 대규모 QA 재실행하지 않습니다.
- 상세 근거: `문서/00_프로젝트/EOD_SafeSync_20261110.md`. 최종 실제 commit/main SHA와 보호·동기화 결과는 원본 `Temp/EOD20261110/Final.json` 및 관련 Issue 댓글에 기록합니다. 같은 commit 안에 자신의 SHA를 적는 대신 이 완료 영수증으로 확정합니다. 성공 상태는 최종 검증 이후에만 보고합니다.
- 추가 개발 없이 오늘 작업을 종료합니다. 아래 기록은 당시 이력이며 최신 수용·보류 상태는 이 EOD 기록을 우선합니다.

## 2026-11-10 Issue #96 — 시작마을 HP·MP 오리지널 아이콘 실제 적용

**STARTERVILLAGE_SHOP_POTIONS_ORIGINAL / UNITY_DELTA_QA_PASS / LOCAL_ORIGINAL_APPLIED / USER_REALPLAY_REVIEW_PENDING / REMOTE_NOT_PUSHED**

- 최종 미술 승인 댓글6766154803·PLANNING 확인. 실제 F:/Downloads ZIP SHA d1ce0e1c…62d7ead 일치·CRC/Manifest11 PASS. 승인128² PNG2만 신규 등록(HP675a6292…497f64 / MP39b90dfc…db64a), Preview/고해상도 소스 게임 추가0.
- 별도 `codex/ch1-shop-potions-original-20261110`, 구현 **`f5f3a07aa4c5b797c417e6bcc80783d8b5501d05`** → 원본 local main ff-only 반영. 신규 PNG2/meta2와 ShopPresenter1만 변경. World_StarterVillage + shop_general_starter_village + HP/MP itemID 조건으로 거래창64² Image만 override. 원화Tint white, 미취득 시 기존표시. Kenney PNG/meta/GUID·공용 ItemDefinition·다른화면/다른상점 불변.
- Unity6000.6.5f1 실제 GameView Before13 PASS / After46 PASS / 원본 반영파일 동일 격리 재실행46 PASS. 가상 마우스/키보드·구매/판매·수량/가격/설명·HP30/MP12·이동잠금/해제·다른상점/마을밖 조건·Save 안정ID PASS, Runtime Error0. 기존 전체QA 반복0.
- 원본 Editor 표적Import·컴파일완료·ConsoleError0, 실제독립Sprite/PPU100/Pivot64 확인. 원본Scene/Play/선택/포커스 무조작. 보호5128 중5127 SHA불변·승인 기존코드1변경·신규4파일 추가, 사용자dirty302/untracked550 동일·Save/UserData 보존.
- [실제캡처·PNG SHA/GUID·QA·보호·확인 방법](../../../../docs/art-renewal/issue96-shop-original-20261110/REPORT.md). 원본 새 Play → 시작마을 잡화 상인 → 물건을 본다. 기술HOLD 없음·대표 손플레이 수용 확인 대기. 문서/최종main SHA는 #96 댓글 및 Temp/Issue96Original/Evidence/FinalResult.json 참조. main직접commit/원격Push·Merge0.
- Issue57 출처 감사는 별도 `codex/ch1-monster-provenance-final` / `1c8f94c9`에서 마감(잔여7종·펫3종 ORIGINAL_CONFIRMED, 슬라임 pixel NOT_RUN·ZIP대기 해소). F2/NPC 신규 적용0, 기존 승인 아트 보호.

---

## 2026-11-10 Issue #97 — 승인 맹독뱀8프레임 실제 게임 반영

**VENOMSNAKE_ANIM8 / UNITY_QA_PASS / LOCAL_ORIGINAL_APPLIED / USER_REALPLAY_REVIEW_PENDING / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**

- 최신 기획 승인·PLANNING 확인. 기본6장+Continuity_v2 Attack01/03만 최종8장 구성. 두 ZIP 지정SHA·CRC·SUMS/Manifest PASS. 미채택 신규 Attack03 사용0.
- 별도 `codex/venomsnake-anim8-unity-20261110` 구현 **`485a8dffad37c02ffcef67ee9abd4ec92a3bdefe`** → 원본 로컬 main ff-only 반영. 신규640셀 시트2/메타2·정의 시각6필드·뱀 모션완료대기만 변경. 승인512 원본 픽셀8/8 동일, 기존 크기·GUID/meta·독/AI/드롭/EXP 보존.
- Unity6000.6.5f1 실제 Field_02/Battle 기본61+보완24 PASS, 반영 원본6파일 동일 복제본 최소 재실행24 PASS. Idle4/Attack4 8FPS·Idle복귀·우향·32/39px 가독성·실제 맹독/쿨다운·Bee/Spider 모션·펫 참조 PASS, Runtime Error0. 초기 검증기 계측/파일명 가정 정정은 상세 보고서에 공개. 기존 전체QA 반복0.
- 원본 Editor 표적Import·컴파일완료·실제 프레임팩토리8/8·ConsoleError0. 사용자 Scene/Play/선택/포커스 무조작. 원본 보호3921 SHA변경0, dirty302/untracked550 동일, Save/UserData·PNG/WAV/meta·Packages/BuildSettings 보존.
- [상세 보고·8원본SHA·GUID·실제캡처5장](../../../../docs/art-renewal/issue97-anim8-20261110/REPORT.md). 제품 적용 완료, 바울 대표 새 Play **Field_02** 맹독뱀 접촉→Battle에서 최종 손 플레이·미술 수용 확인 필요. 기술HOLD 없음. 원격Push/Merge·main직접commit0. 문서/최종main SHA는 #97 결과댓글 및 Temp/Issue97/Evidence/final-result.json 참조.
- 직전 #75 B/C 격리QA `codex/f2-bc-unity-delta-20261110` / `17b045e020153658e1e597aa689e076a27c40fa6`의 원본 미적용 상태 유지. F2/NPC 신규 적용0. HP·MP 상점 아이콘의 기존 HOLD도 별개로 유지.

---

## 2026-11-10 — F2·맹독뱀 ZIP 검토 / 시작마을 상점 물약 아이콘 HOLD

**F2_BENDSNOW_REVIEW_DONE / VENOMSNAKE_REVIEW_DONE / STARTERVILLAGE_SHOP_ICON_REPLACEMENT_HOLD / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

- F2 ZIP SHA `151094aacd847ee7b4338ca08a827d0edaa21a1122df357379f0bd493afabd8a`, 10entries·CRC·SUMS8/8 PASS. **부분 수정 필요**: 곡선·눈 불규칙성은 개선됐으나 비교판은 생성 콘셉트로 실제 ROI/4.5WU/HUD 증명 아님. 방향 수용 기록 존중·SPEC_PENDING·본편 미적용.
- 뱀 ZIP SHA `5c708234539a7751c1646508feb5fc66e8a5e11439b222494145e99923980b8b`, 14entries·CRC·SUMS13/13·Manifest12/12 PASS. **단일포즈 승인 상정용 PASS**, v3 원본 SHA동일. 512/128/40 직접 검토·40px 세부 제한·왼쪽alpha25px 유지. 사용자 최종 미술/게임기술 승인 별개. NPC는 뱀 마감 판단 후 다음 업무, 이번 제작0.
- 바울 대표가 대상은 **월드 소품이 아닌 시작마을 상인 거래창 아이콘**이라고 확정. HP/MP ItemIcons PNG는 Kenney flask_full/flask_half 원본과 SHA 일치. GUID a9ac67de65b88774ea6e90e22805af16 / c062cadfd7ff1b94e8c89e77f74cf048. 공용 ItemDefinition 참조라 직접교체 시 다른상점/전투 영향. 기존 내부 시트는 해독제/절연약 등으로 HP/MP 대체2종 미확인 → **교체HOLD·제품변경0·구현커밋 없음**.
- Unity6000.6.5f1 실제 상태·의존성50/외부0·Preview Renderer286 확인. 월드 참고 렌더1장 확보(Play/거래창 캡처 아님). 교체 전후/이동/NPC/구매판매 DeltaQA NOT_RUN, 기존 전수QA 재실행0. Console Error0·활성Scene/Play/선택/포커스 변경0. 과거worktree Bootstrap이 활성Scene인 상태도 보존.
- 원본 dirty302/untracked550 경로 동일·보호3921 SHA변경0, Save/UserData/승인 PNG·WAV/meta/Packages/BuildSettings 보존. 원격Push·병합/main직접commit0. 브랜치 `codex/20261110-main-renewal-prop-batch`, 보고커밋 `f96e49e793d4ad54d560bfe6354807161187e60a`; 로컬main에는 안전한 경우 보고·인계만 FF, 제품 적용HOLD. 최종문서/main SHA는 Git HEAD·#75/#43/#57 결과댓글 참조.
- 다음: 기존 내부 **HP·MP 회복약2종 원본 경로/역할** 확인 → 시작마을 거래창 한정 연결 → Unity 숨김 Play 표적 전후/거래/이동잠금 QA → 안전FF. 새아트/NPC/뱀모션 제작 금지 유지.
- 상세: [검토·참조·HOLD 근거](../../Batch_ArtReview_StarterVillageShopIcons_20261110.md), `Evidence/BatchArtShop20261110/`. 로컬 문서 미Push로 원격보고서 미게시, Issue 상태보고와 구분.

---

## 2026-11-09 Issue #94 — 던전 B1·B2 실제 내부 벽 충돌 수정

**LOCAL_MAIN_APPLIED / LOCAL_COMMITTED / REMOTE_NOT_PUSHED / USER_REALPLAY_PENDING**.

- B1·B2 방/복도 바깥 내부 허공 Collider 누락을 수정했습니다. 승인 바닥 합집합의 여집합을 Static CompositeCollider2D로 구성하고, B1 잔해 위 접지보다 좁은 틈1곳을 닫았습니다. PNG·Scene·기존 퀘스트/TTS/숲거미/귀환 보존.
- Unity6000.6.5f1 실제 Play 표적 **110PASS/0FAIL**. B1 72/B2 56 방향 스트레스 관통0, 각13200점 마스크 일치, 도보·휠체어·가상 키보드/패드/안내 API, 조사·보스문·석판 경로, B1↔B2→Field03, 벽 안 Continue·복구 Save 비덮어쓰기 통과. 기존 Editor Search 내부 예외1은 별도이며 Runtime 프로젝트 오류0. 이전 전체QA 재실행0.
- 보호995 중994 SHA불변·이미지/음성1369불변·dirty302상태동일·변경경로겹침0. 원본 슬롯01만 계속 실행 중인 게임이 좌표2필드를 자체 자동 저장하며 Codex 원본Save 쓰기0. 슬롯02/03은 시작부터 없고 과거 복제본만 사용했습니다.
- 제품 로컬 main SHA **5024d2b6ffd1a07f90dbb15cf073eed16bb3630a**, 별도 `codex/fix-20261109-dungeon-b1-b2-collision` 커밋→ff-only 적용 완료. 문서 포함 최종SHA는 실제 Git HEAD·Issue94 댓글·Temp/Issue94/Evidence/local-main-final.json 참조. main 직접commit/원격Push·병합0.
- 원본 B2 Play/Scene/선택/포커스 무조작. **새 Play에서 적용을 확인해 주세요.** 물리 장치 손 플레이는 사용자 수용 확인 사항입니다. B1/B2 기존 마우스 자동 길찾기는 미구현이며 공용 안내 API/마우스 귀환 버튼 검증과 구분합니다.
- [원인·수정·QA·캡처](../../Issue94_DungeonCollision_20261109.md), Evidence/Issue94Collision20261109/. 다음 권장: 새 Play에서 두 층 벽·모서리·목표·귀환 수용 확인.

---

## 2026-11-05 최종 통합·로컬 동기화·하루 마감

**USER_REALPLAY_ACCEPTED / LOCAL_MAIN_SYNC_SUCCESS / REMOTE_NOT_PUSHED / DAY_CLOSED**.

- 최신 GitHub PLANNING 20:44 및 Issue93 마감 지시6657174333, 신규/리뉴얼 아트·TTS·BGM 인계를 대조했습니다. 기존 승인 #63 Bee5, #89 정식아이콘/B HUD, #90 Spider Attack8/Idle8/Walk8, #92 Ch2 배경은 승인 통합8768cbac→안전동기화674e6b0f를 거쳐 현재 main의 조상입니다. TTS162는 b0a666be로 이미 통합되어 옛 담당 인계의 미반영 표기는 과거 기록입니다.
- 오늘 Main11 동행 필터·B1/B2 귀환·숲거미 공통공격(6f094a40/706e7010), Main11 WAV10·Field03 입구(71919894/1a233d01/0fb8b8c4)는 이미 main 포함. 대표님 “잘돼” 실플레이 수용을 반영했습니다. **누락된 승인 제품0, 제품 신규 통합0, 중복 적용0**. docs/* 전체 병합 없이 최신 정본의 승인 상태만 대조했습니다.
- 보류: #91 Ch3 BG v2·Shoot8·미확정 냉기 Runtime·기존 VISUAL_PARTIAL·미승인 BGM/시안. 이번 마감에서 제작/추가 QA/무인 작업을 시작하지 않습니다.
- 마감 시작 main **0fb8b8c4a02a7f21fc5bf78360fb49c371a28558**. 별도 **codex/eod-safe-sync-20261105** worktree에서 이 마감 기록만 작성하여 충돌0 조건으로 로컬 main ff-only 반영합니다. 최종 문서 포함 SHA는 실제 Git HEAD 및 Issue93 마감 댓글 참조. main 직접 커밋/원격 Push/원격 병합0.
- Unity **6000.6.5f1 (3ff58d469c8a)** 유지. 제품 변경0이므로 기존 QA65/9/18/19 및121/37/15/7은 재실행하지 않았습니다. 직전 컴파일·실재생·전후 화면 근거는 Issue93 상세보고에 유지합니다. 원본 Editor/Scene/포커스 조작0.
- 원본 Save01/02/03·UserData·승인 PNG/WAV·사용자 tracked dirty302/untracked550·Packages/Scenes/Prefabs/meta/GUID/BuildSettings 보호. 마감 전후 현재 존재하는 보호996파일 SHA와 원본 git status의 동일성을 확인합니다. 등록 worktree137개·로컬브랜치174개 전체 감사 완료. 이전 보호목록의 원본 Save02/03은 이번 감사 시작부터 원본 경로에 없고, 슬롯01과 기존 검증 복제본은 존재합니다. 원본 복원/삭제는 수행하지 않습니다. 기존 검증 worktree overlay와 사용자 파일은 정리·삭제·커밋하지 않습니다.
- 오늘 작업 종료. 후속 작업은 새 사용자 지시가 있을 때만 시작합니다.

---

## 2026-11-05 Issue #93 재발 — Main11 음성·Field03 입구 실제 수정

**LOCAL_MAIN_APPLIED / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

- WAV 실내용 분할 오류 확인: 미엘001~005·폴001/002/003/008/009의 기존 원본 PCM만 복구. 정상18 WAV·원문/TTS ID·기존meta28·카탈로그 유지, TTS 생성/API0. 보스 후001/B1조사002/B2조사003 실제 AudioSource 출력·Unity PCM 재전사 정상 대응.
- Field03 불투명 Quad와 큰 목표판 가림 제거. 승인 계단 PNG 재사용·지면 표시, 작은 옆 안내, 실제 입구3m 기준 안내 전환(3.5m Navigation 유지). Player Sorting/Scene/Bounds/Spawn/안전지대/Quest 순서 유지.
- Unity6000.6.5f1 신규 Runtime65PASS/0FAIL, 최종F·거리9PASS/0FAIL, 실제 화면18PASS/0FAIL, 정적19PASS/0FAIL. 중복 합산/기존 전체QA 재실행0. 사용자 명시 허락 후 별도 검증 창에서 도보·휠체어960/1280 전후8장 실제 캡처·창 자동 종료.
- 원본Save01/02/03 복제 시험. 보호998 중 허용 WAV10만 수정·나머지988 불변, 기존dirty302/untracked550 보호. 숲거미Attack8·B1/B2귀환 유지. 검증 전용 Packages/제품명/설정은 제품에 미포함.
- 구현71919894d15765c31a6ebb5758d2ade4cb5ba90e는 main FF 완료. 후속 거리 정합·마감은 codex/fix-20261105-main11-voice-field03-entrance에서 커밋→안전FF, 최종 SHA는 Git HEAD/Issue93 마감댓글 참조. main 직접commit·원격Push/병합0.
- 사용 한도 후 재개에서 완료한 음성·빌드·화면 검사는 반복하지 않았다. 다음 권장: 바울 대표 실플레이 청취·입구 가독성 확인. 새 TTS/아트 제작 불필요.
- [실제 원인·변경파일·QA·전후 캡처](../../Issue93_Main11Voice_Field03Entrance_20261105.md), Evidence/Issue93VoiceEntrance20261105/.

- 최종 제품·거리 정합 SHA **1a233d01102b7887656e0219702eb1359a831798** main FF 완료. 원본Editor import/compile 완료·C#오류0·입구리소스/미엘3 Clip 갱신 확인·Scene dirty=false. 문서 포함 최종HEAD는 Issue93 마감 댓글 참조.

---

## 2026-11-05 Issue #93 P1 3건 수정·로컬 main 반영

**LOCAL_MAIN_APPLIED / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

- 숲거미 일반공격은 승인 Attack8, 독액 분사는 구 Shoot 경로가 원인임을 실제 Battle Controller에서 확인. Shoot 경로를 비워 승인 Attack8 공통 사용. 피해·효과 불변, Shoot8 HOLD·승인 PNG 재제작0.
- 복제 슬롯02·03은 Main11 index2 / 태온·미엘 동행. 비동행 폴 등만 무음 조사 지문, 동행 원문·TTS ID28/28 유지. 현재 목표·거리·순차 완료 보호, 같은 프레임 대화 재입력 차단.
- B1→Field03·B2→B1 확인형 귀환, E/F·패드A·마우스 버튼. 퀘스트 미완료 귀환·재입장·Continue, 보스/석판/몬스터 대기·Inventory 보호. Scene/Bounds/Spawn 원본 재생성0.
- Unity6000.6.5f1 숨김 Play Delta **121PASS/0FAIL**, 보완 **37PASS/0FAIL**, 최종 입력 **15PASS/0FAIL**, 정적7PASS/0FAIL. 원본 main 백그라운드 컴파일 완료·신규 타입 로드·C# 오류0도 확인. 중복 합산 금지·기존 전체QA 재실행0. OS 포커스 전환0. 가상 입력/UI callback 검증이며 실제 물리 장치 장시간 플레이는 사용자 확인 사항. 기존 Editor Search 내부 예외는 별도 유지.
- 사용자 변경302·미추적550 보존, 보호942 SHA변경0(원본Save02/03 포함), 승인Spider24 PNG 변경0. 구 Git Packages 불일치는 사용자 로컬 Packages를 QA 복사본에만 overlay; 원본 패키지/설정 미채택·미수정.
- 구현 커밋 `6f094a4095e9298d20e879d842f29b02bca6587a` / `codex/fix-20261105-dungeon-spider-dialogue` → 원본 local main FF 완료. main 직접commit·원격Push/병합0. 후속 문서 커밋은 같은 브랜치에서 FF로 전달.
- 다음 권장: 실제 사용자 플레이에서 귀환 안내 가독성 확인. Shoot8 미승인 유지. [원인·파일·QA·전후 캡처](../../Issue93_P1_Dungeon_Spider_Dialogue_20261105.md), `Evidence/Issue93_20261105/`.

---

## 2026-11-04 긴급 로컬 main 동기화 실제 완료

**LOCAL_MAIN_SYNC_SUCCESS / USER_WORK_PRESERVED / REMOTE_NOT_PUSHED**. 이전 HOLD 보고는 아래보다 과거 상태입니다.

- 실제 원본 `F:/study/codex/Project-Limitless` main을634a91d3→**97992cdaff69a38b458a715fb4073907fb926d4c**로 fast-forward 완료(후속마감문서도별도브랜치→FF). 승인통합브랜치 `codex/integrate-20261104-approved` **8768cbac91a291f13769c6bfa18a831673652acb** 유지, 보존/동기화브랜치 `codex/local-main-safe-sync-20261104`.
- 308/550 전수분류·CSV/JSON,6경로겹침 해결6/HOLD0. Main24는 승인본과 바이트동일, PilotBee 수정meta5는 원래Git버전과 별도로 보존. 나머지302dirty·550untracked 원본유지. 새/미승인 자료 자동채택0.
- F: 별도백업 `F:/LIMITLESS_Backups/20261104_SafeMainSync_201027`: 고유881개 SHA일치(변경/미추적858+private23), 원본PilotBee21파일 보존이동 SHA일치. source858·private23 바이트불변. 승인PNG37동일/GUID유일.
- 격리 dirty재현FF 및 실제FF PASS, 신규 Git/파일Delta **16PASS/0FAIL**. Unity6000.6.5f1 고정, 승인8768 대비Unity코드/자산변경0이라 기존60/52·격리BuildSucceeded 재사용, Unity전수QA/Play 재실행0. 기존원본user Scene/Prefab/Packages/BuildSettings/Save보호.
- #63/#90/#89표시계층/#92파일은 원본main 반영됨. #89자동냉기런타임·미정규칙은완료아님. #91미승인/Shoot8 HOLD·기존시각PARTIAL 유지. 원격Push/merge/main직접commit0. 이 동기화에 추가사용자승인필요0. 남은사용자작업정리/미정기획승인은별도.
- [상세결과](../../LocalMain_SafeSync_20261104.md), 근거 `Evidence/SafeMainSync20261104/`. 마지막관련 코드/보존commit97992cda, 최종문서commit은GitHEAD/이슈마감보고참조.


GitHub 실제 보고 완료(댓글 API 성공 확인): [#63](https://github.com/HappyPaul812/Project-Limitless/issues/63#issuecomment-6628635508), [#89](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6628636928), [#90](https://github.com/HappyPaul812/Project-Limitless/issues/90#issuecomment-6628638252), [#92](https://github.com/HappyPaul812/Project-Limitless/issues/92#issuecomment-6628639926). 보고 기준 구현 b5a41b3089ed43abd725c836d8934d27cf88ca58 / 문서 aa0e48f7b3c1dd2ee0d39d15a50738463d6a6b1c. 원격 git Push·원본 main 동기화는 수행하지 않았습니다.

## 2026-11-04 최종 마감 — 승인 자산 격리 통합, 원본 동기화 보류

- 브랜치 `codex/integrate-20261104-approved`, 구현 `b5a41b3089ed43abd725c836d8934d27cf88ca58`, LOCAL_COMMITTED / REMOTE_NOT_PUSHED.
- #63 신규5포즈 / #90 Attack8+기승인Idle·Walk / #89 정식아이콘3+B표시계층 / #92 배경2·6필드catalog를 별도 통합본에 연결. 승인 PNG37 SHA 동일. 원본에 실제 적용한 것은 아님.
- Unity6000.6.5f1 고정, 신규 연결60PASS/0FAIL, 보완 시각한정52PASS/0FAIL(중복 합산 금지). 실제3인 UI의 태온·휠체어 겹침 및 기존 하단안내 겹침으로 VISUAL_PARTIAL. 냉기 규칙·자동 표시 수명 미구현 유지, #91 미승인·Shoot8 HOLD.
- 원본main634a91d3, tracked dirty308/untracked550 그대로. tracked7374·개인Save/UserData23 해시 변경0. **LOCAL_MAIN_SYNC_HOLD_DIRTY / ORIGINAL_NOT_APPLIED**. 원본 Editor·Scene·Prefab·GUID/meta·BuildSettings·Packages 보호. main 직접commit/Push/병합 없음.
- 다음 적용 조건: 사용자 변경을 보존한 동기화 경계 정리와 시각 PARTIAL 별도 결정. 오늘 후속 제작/기존 전수 QA 없음. 결과는 `EOD_Approved_LocalIntegration_20261104.md` 및 `Evidence/EODApproved20261104/`에 기록. 아래 과거 pending 기록은 각 당시 상태이며 이번 승인 정본이 우선합니다.

## 2026-11-04 Issue #92 실제ZIP 후속 준비

전체ZIP SHA1회대조일치.실제Ch1 3+Ch2 2를1672×941 1:1비교·5패널화소일치. 세부재질/거리명암방향공유,Ch2저채도/가로암반띠반복차이. 두후보유지/미술검토추천·실Unity가독성미검증. [결과/근거](../../Issue92_Ch1_Ch2_Actual_Style_Comparison_20261104.md). USER_VISUAL_REVIEW_PENDING/NOT_UNITY_APPLIED·원본/Save/main/Push보호. branch codex/issues63-89-92-followup-20261104·각commit은해당Issue댓글·LOCAL_COMMITTED/REMOTE_NOT_PUSHED.

---

## 2026-11-04 Issue #89 실제ZIP 후속 준비

전체ZIP SHA1회대조일치.정식용아이콘3납품/SHA확인·미술승인대기. B HUD resolver아이콘참조만바꿀신규대표5캡처/Rect·문자Delta계획준비·실행0·50/99보존. [결과/근거](../../Issue89_OfficialIcons_ApprovalGated_IsolatedQA_Plan_20261104.md). USER_VISUAL_REVIEW_PENDING/NOT_UNITY_APPLIED·원본/Save/main/Push보호. branch codex/issues63-89-92-followup-20261104·각commit은해당Issue댓글·LOCAL_COMMITTED/REMOTE_NOT_PUSHED.

---

## 2026-11-04 Issue #63 실제ZIP 후속 준비

전체ZIP SHA1회대조일치.11/04신규3한벌+승인Sting/Recover v3추천·예전3도신규출처로보존·혼합/재제작0. 신규3미술승인대기·기존15/153미실행. [결과/근거](../../Issue63_OldNew3Pose_FinalChoice_20261104.md). USER_VISUAL_REVIEW_PENDING/NOT_UNITY_APPLIED·원본/Save/main/Push보호. branch codex/issues63-89-92-followup-20261104·각commit은해당Issue댓글·LOCAL_COMMITTED/REMOTE_NOT_PUSHED.

---

## 2026-11-04 Issue #63 PilotBee 미사용 결정·라이선스 조사 중단

바울대표최종제품PilotBee미사용. BY3/4충돌해결조사중단·과거이력보존,개발선행조건에서제외. 현재Resources2PNG+02_VenomBeeGUID참조잔존으로빌드제외완료아님·최종Player미검사. Prepare/Idle3는외부원본셀직접일치0·실SHA가11/03신규생성/보완납품계보와일치;최신기획의외부계열분류와차이보고(원생성마스터미동봉한계). 승인Sting/Recover·격리15/후보153보존·새Unity/빌드/원본적용0. [출처·빌드제외확인](../../Issue63_PilotBee_NotForFinal_Exclusion_And_Provenance_20261104.md). 라이선스해결대신최종자산배제확인필요·별도원본적용승인유지. branch codex/issue63-pilotbee-exclusion-audit-20261104·SHA는Issue63댓글·LOCAL_COMMITTED/REMOTE_NOT_PUSHED.

---

## 2026-11-04 Issue #63 승인v3 등록·격리 연결·권리 조사

Sting28500201…/Recover69d0944b… 최종미술승인등록·BYTE_KEEP,Prepare/Idle보호. 지정F:ZIP현재없어기존검수HTML내승인SHA일치PNG바이트복구/대장보존. Unity6000.6.5f1공용재생기/Presenter로키포즈반복10슬롯1회집중15PASS/0FAIL·피해훅.27초02/완료1.01초Idle00복귀·09Recover유지. 후보A153재실행0·기존70/입력5SHA불변. 웹BY3/ZIPBY4충돌실확인·둘다상업조건부허용이나버전/표시미확정RIGHTS_HOLD. 현행GUID외부복사본참조와README미연결서술충돌보고. 원본에는후보A완료대기API미반영·별도통합필요. 실표적침끝/전체작화/별도원본적용승인미완료·NOT_UNITY_APPLIED. [결과·실Unity·권리조사](../../Issue63_VenomBee_v3_Approved_Connection_And_Rights_20261104.md). 브랜치codex/issue63-v3-approved-connection-rights-20261104·이번SHA는Issue63댓글참조·LOCAL_COMMITTED/REMOTE_NOT_PUSHED.

---

## 2026-11-04 Issue #91 실제Ch1-v2 1:1 최종 비교판

원본Ch1 3+검수완료Ch3v2 2의1672×941패널5/5화소일치·입력전후SHA5/5불변. 비교판1개5144×4400/SHAae44f57176aaf9aac52d169e6e63ef8e2af2a9ff4b3fa8e6833626b77a1b64f3. ZIP중복검수/새Unity/전수QA/새v3없음. 정식적왼쪽·아군오른쪽후열x=.86참조,원본(1391,433)정적가정의오른쪽수피/가지경쟁위험명시. 과거실Unity캡처와OFFLINE도식구분. [비교판·검토](../../Issue91_Ch1_Ch3v2_Final_1to1_Comparison_20261104.md). USER_VISUAL_REVIEW_PENDING/NOT_UNITY_APPLIED. 브랜치codex/issue91-ch1-v2-final-comparison-20261104·실제SHA는Issue91댓글·LOCAL_COMMITTED/REMOTE_NOT_PUSHED.

---

## 2026-11-04 Issue #91 간편v2 ZIP 실제접수검수

F:/Downloads/ch3_bg_v2_easy.zip 3,944,727bytes/5entries/SHA0843afdbce6c1e6dcb1117240eaafbc33e894f126f9975ba46e18da8f04ef292. CRC/SUMS4/4통과·Manifest없음. 배경1672×941 RGB2장 기존보고v2SHA정확일치·새미술변경없음. v1대비455966/470950화소변경·지정ROI밖0. OFFLINE정식방향모조배치판있음/실Sprite·정식UI검증없음/Ch1-v2직접비교없음. **USER_VISUAL_REVIEW_PENDING / NOT_UNITY_APPLIED**,바울대표v2최종미술승인필요. [실제납품검수](../../Issue91_EasyZip_Actual_Intake_20261104.md). 브랜치codex/issue91-easyzip-intake-20261104·SHA는Issue91댓글참조·LOCAL_COMMITTED/REMOTE_NOT_PUSHED.

---

## 2026-11-04 Issue #90 Attack8 전체8장 최종 미술 승인 확정

바울 대표 직접 확인·“좋아” 승인에 따라 수정본ZIP58c51cb4…의00~07 전체8장 USER_ART_APPROVED. 기존00/03/06·01 BYTE_KEEP 유지,01은이번전체승인에포함. After8 SHA직전검수와불변. Shoot8별도HOLD. 기존16PASS보존·새QA/원본Unity적용0·FootLock/실접촉미검증상태보존. [승인대상8SHA·경계](../../Issue90_Attack8_Final_User_Art_Approval_20261104.md). 브랜치codex/issue90-attack8-final-art-approved-20261104·이번SHA는Issue90댓글참조. LOCAL_COMMITTED/REMOTE_NOT_PUSHED.

---

## 2026-11-04 Issue #89 B안 대표 시각 마무리

최신기획6624214599확인·기존Unity B실캡처7장만재사용해대표판/설원40px·HUD1:1상세작성. Unity실행0·50/99재검사0·새PASS추가0. 한기2턴/동상2턴/괴사1/3·2/3+KO문자포함식별가능. VFX단독한기대비·소형임시아이콘은미완료 **OFFICIAL_ICON_PENDING / VISUAL_PARTIAL_REMAINS**. 문자유지조건부B원본적용후보,별도승인전ORIGINAL_NOT_APPLIED. 기존101증거/승인3PNG SHA불변·원본Save/main/dirty보호. [대표판·판정](../../Issue89_B_Visual_Wrapup_20261104.md). 브랜치codex/issue89-b-visual-wrapup-20261104·실제SHA는Issue89결과댓글참조·LOCAL_COMMITTED/REMOTE_NOT_PUSHED.

---

## 2026-11-04 Issue #90 Attack8 수정4장 실제 최종 검수

실제F: ZIP SHA58c51cb4…보고일치. 승인00/03/06·유지후보01 바이트동일,02/04/05/07만수정. Unity6000.6.5f1격리 실제Animator 전후192샘플·180캡처·집중16PASS/0FAIL,10FPS/0.8초/07→승인Idle00확인. 기존270증거불변·기존전수QA미실행. 04→05자세변화·07→Idle실루엣은 **USER_FINAL_ART_APPROVAL_REQUIRED / NOT_UNITY_APPLIED**. FootLock/실표적접촉미검증. [상세·실제캡처](../../Issue90_Attack8_4Frame_Fix_FinalReview_20261104.md). 원본Unity/Save/main/dirty보호·LOCAL_COMMITTED/REMOTE_NOT_PUSHED,브랜치codex/issue90-attack8-4frame-review-20261104·이번SHA는Issue90댓글참조.

---

## 2026-11-04 Issue #89 실제 표시/HUD A/B 수정·검증

Unity6000.6.5f1격리실행·대표3조건7쌍/14실제A/B캡처. 신규최종고유50PASS/0FAIL(기존99전수미실행). **B추천**:40px가시알파간격8px,128px뒤그리기+24px하향,960상태두행·선언글자8.66→12.12화면px,1680 HUD겹침-30.15→8px. 이름/HP/스킬메뉴Rect유지. 기존Evidence101/승인PNG3 SHA전부불변·원본Unity/Save/main/dirty/Push보호. [실제전후·코드·측정](../../Issue89_VFX_HUD_Isolated_Layout_AB_Delta_20261104.md). 작은아이콘/한기설원대비·타외형/다인HUD는남은시각판단,USER_LAYOUT_REVIEW_PENDING/NOT_ORIGINAL_APPLIED. 직전06e871f9·이번SHA는Issue89결과댓글참조. LOCAL_COMMITTED/REMOTE_NOT_PUSHED.

---

## 2026-11-04 Issue #91 Ch1·Ch3 원본5장 1:1 비교 완료

실제Forest/Grassland/Catacombs+C3Snowroad/F2 모두1672x941 RGB. 비교판5144x4200에각원본1:1·패널화소5/5·입력SHA전후5/5일치. F:/Downloads/LIMITLESS_Issue91_Ch1_Ch3_1672x941_1to1_Comparison_20261104.png 저장, SHA e10aa647089ee9b665058f3428e589b50dbfbdd97b508bf87ca7f361b42ceee9. Ch1미세재질/명암 대 C3기하평면차이·원경/길/수관/오른쪽수간만좌표후보특정. 정식아군오른쪽/적왼쪽과기존목업반대배치구분. [상세·실비교판](../../Issue91_Ch1_Ch3_SameScale_ActualPNG_Review_20261104.md). USER_VISUAL_REVIEW_PENDING/NOT_UNITY_APPLIED 유지·원화수정/새BG/Unity/Save/main/Push0. 직전064448a8·이번SHA는Issue91결과댓글참조.

---

## 2026-11-04 Chapter2 배경 읽기 전용 감사·신규 ZIP 일치

#91 C3배경2/#90 Attack8 ZIP F:SHA 각각1회 계산·최신총괄값정확일치. #90 구납품SHA63자오탈자는최신64자와구분기록. 로컬PNG949목록/Scene39/Prefab13/Catalog·GUID감사:Field04~10기본색상전장,Field11심부지면512재사용,Main19Background는월드연출. Ch2전용전투BG세트미발견·신규작화발주아님. #89는40px분리위치/128px뒤레이어·하향/HUD분리3안제안만·99PASS재실행0. 신규BG2/Attack중간5미술승인대기유지. [상세·근거](../../Chapter2_BattleBackground_ReadonlyAudit_Zip91_90_Vfx89_20261104.md). Unity6000.6.5f1·원본/Save/Editor/dirty보호·제품수정/Push/merge0. 직전관련9c799745·이번SHA는Issue91/90/89댓글참조. LOCAL_COMMITTED/REMOTE_NOT_PUSHED.

---

## 2026-11-04 Issue #89 v2 격리 Unity 시각 Delta 실제 실행

6000.6.5f1·실제PlayMode/Camera/Canvas RT로40/128px×설원/어두운전장×3해상도12조건 캡처완료. 자동99PASS/0FAIL·컴파일/새Runtime오류0(기존Search시작예외1 별도). 실제49PNG·측정·로그 보존. **VISUAL_PARTIAL**:40px 바퀴와혼합/상태구분약함,128px 하체가림,작은상태HUD가독성·기존상세팝업차폐 검토. 얼굴/HP/2x3메뉴는VFX차폐없음. 승인미술유지·원본적용/전수QA0. [상세·실캡처](../../Issue89_V2_UnityVisualDeltaQA_20261104.md). 다음은부착위치/크기·별도아이콘/HUD가독성판단. 원본main/Save/승인아트/dirty보호. LOCAL_COMMITTED/REMOTE_NOT_PUSHED·직전a4cd85af·최종SHA는Issue89댓글참조.

---

## 2026-11-04 Issue #89·#90 v2 미술 승인 확정

F: v2 ZIP 전체 SHA를 각각1회 계산해 기획 승인값 정확일치. #89 USER_ART_APPROVED_SHA_MATCH, #90 ATTACK3_KEYPOSE_USER_ART_APPROVED_SHA_MATCH. [승인·QA준비·리뉴얼 인계](../../Issue89_90_V2_ArtApproval_QAPrep_Handoff_20261104.md). #89 격리6000.6.5f1의40/128px·HUD·설원·3화면비12조건 QA 준비서 완료(실행전), #90 승인3키포즈KEEP·미제작중간5장만 제작가능 인계. Unity/본편적용·전체QA반복0, 원본main/Save/승인아트/사용자dirty보호. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 직전관련fd70f8e8, 이번문서commit은Issue#89/#90결과댓글참조.

---

## 2026-11-04 Issue #89·#90 F: 원본 ZIP 확인 / 미술 승인 대기

두 ZIP 전체 SHA256이 최신 총괄 보고와 정확히 일치합니다. Spider Contact 전후 두부/몸통 높이·앞다리 관절 대응과 Recover 지지발, 한기 설원 대비·동상/동결괴사 40px 실루엣만 최소 보완 후보로 특정했습니다. 실제 프레임 OFFLINE 검토이며 Unity 재검증/제작/본편 적용0·6000.6.5f1 유지·Save/승인아트/main/사용자 dirty 보호. [상세 검수](../../Issue89_90_FDrive_Zip_VisualReview_20261104.md). 다음은 표적 미술 승인/소량 보완 판단입니다. 마지막 관련 commit은 이번 문서 commit 및 Issue #89/#90 댓글 SHA 참조. LOCAL_COMMITTED / REMOTE_NOT_PUSHED.

---

## 2026-11-03 Issue #86 마지막 F2 등록 HOLD 해소

실제 QA/보고 `b36a67dc02ac308dfcaa352c5ffce7fef399b324`. [원본 동기화·Issue #86 영수증](../../Evidence/F2BuildSettingsFinal86_20261103/Receipt.md).

**F2_BUILDSETTINGS_SCENE_REGISTERED_PASS / ORIGINAL_LOCAL_SYNC_DONE / UNITY_VERSION_6000_6_5F1_CONFIRMED / REMOTE_PUSH_NO**. 제품 `35b87d297392ba7d0e02c6657a94741c875abc10`을원본ff반영. 기존25개path/GUID/order/enabled·config전체바이트보존, 활성F2한건만추가. 격리Unity표적26PASS/0FAIL·실행전후BuildSettingsSHA동일·자동직렬화변경0. 원본사용자app-ui변경은미커밋상태그대로·846파일바이트불변/BuildSettings3줄허용변경·status343/857/staged0. 버전/Packages불변·개인Save/원격작업0. 원본Editor손플레이는미검증이며동일후보설정의격리실경로검증PASS. [최종등록/백업/QA](../../F2_BUILDSETTINGS_FINAL_SYNC_20261103.md). branch codex/f2-buildsettings-final-sync-20261103. 완료작업재통합/기존44전수재실행0.

---

## HISTORY — 2026-11-03 Issue #86 안전 통합·F2 등록 전 상태

실제 QA/보고 커밋 `11e6c746166ecc967ae78a01b31617ab49251968`. [원본 반영·Issue #86 영수증](../../Evidence/LocalSync86_20261103/Receipt.md).

**STAGING_INTEGRATION_PASS / APPROVED_FILES_LOCAL_SYNC_DONE / ORIGINAL_LOCAL_SYNC_HOLD(F2 BuildSettings) / UNITY_VERSION_6000_6_5F1_CONFIRMED / REMOTE_PUSH_NO**. 실제Unity44PASS/0FAIL·신규GUID/Quest중복0 후 원본main ab47b931→ac12abb0645f06fe84d4ae246b584a19d29ebba5 ff-only. 버전파일바이트유지하면서정확6.5/3ff58d469c8a Git기록반영. 승인코드80/83/85·Owl/Stump·Spider9참조통합,46/73중복0. 사용자847파일/기존148refs보존·개인Save열람0·원본status858→857(버전정리1). 원본F2등록은보류:25Scene/app-ui보존·26Scene은QA제안만, 원본전체플레이동기화완료아님.

[통합보고·HOLD·백업](../../LOCAL_SYNC_REPORT_20261103.md). branch codex/local-sync-approved-20261103 / 제품ac12abb0. OFFLINE실제RT열람판 F:/study/codex/Project-Limitless/Temp/LocalSync20261103/Review/index.html. Bee v3 ART_OK_CANDIDATE/타이밍153·F2환경68·Writer70/77/78/미완몬스터·구매후속은기존branch보존/활성화0·Push0. 다음F2등록범위검토,44/기존QA전수반복금지.

---

## 2026-11-03 Issue #85 — F2 서리올빼미 실제 전투 후보 완료

**F2_ENCOUNTER_PASS / OWL_BATTLE_RUNTIME_PASS / COLD_STATUS_HOLD / HIT_DEFEAT_GENERIC_FEEDBACK_ONLY / NOT_ORIGINAL_APPLIED**. 실제 Unity6000.6.5f1 F2→Battle UI→공격/HP·승패→귀환 **신규81 PASS/0 FAIL**. 이는 #80 기존81 재실행이 아니며 #83 90/#84 37/저장QA 반복0. 실제925샘플, 승리/패배 영상326프레임·세 비율·같은Battle Image40px 확인.

승인 Idle8/Attack4 PNG12 바이트 보존·8FPS/PPU512/중앙/Scale1을 기존 MonsterDefinition 명시배열→UI Image 재생기에 연결. 정식 ID/수치 미확정이라 기존 슬라임 수치 차용 CANDIDATE 조우·보상0, 펫/Quest 해금0. E/F/마우스·취소/중복진입·도보/휠체어 실제Collider 접근, 승리F2안전복귀·패배기존회복/마을거점복귀→F2재진입, 합성Save/Continue HP/Main24 보존 확인. 기존 주사위/턴 순서는 그대로입니다. 한기/VFX·상품·해제템·Main24완료/보상/새HitDefeat아트 구현0.

[기능·실제 UI·QA·적용조건](../../Issue85_FrostOwl_F2_ActualBattle_DeltaQA_20261103.md). 열람판 `F:/study/codex/Project-Limitless/Temp/Main24FrostOwlFinish20261103/Review/index.html` (OFFLINE/내용은실UnityRT/OSGameView아님), SHA250621e946a411de9fa55facd84ad5cf2d74aea27032b158f035fcb8a89210bd. branch `codex/main24-frost-owl-finish-20261103`, 출발 d3dae9c5; Bee 타이밍·저장복구 코드 병합0. Source6파일+승인12PNG/새Resources/meta/Tools QA/문서만 후보. 원본main ab47b931/status344(상세858), Scene/Prefab/BuildSettings/Packages/ProjectVersion·개인Save/UserData·TTS/Discord66/Push/merge 보호. F2Scene중복생성0·기존그루터기SHA보존.

다음은 정식 Monster/Spawn ID·밸런스/보상·한기/대응수단 결정 및 원본 통합 범위 승인입니다. fixture BuildSettings만 연결했고 작업브랜치 BuildSettings는 변경하지 않았습니다. 실제커밋/Issue댓글은 후속영수증 참조. 기존 승인 Owl/Spider 모션 재작화 금지, Spider접지 NOT_VERIFIED 유지.

---

## 2026-11-03 Owl·Spider 최종 미술 승인 / Bee 표적·권리 계약 조사

**Owl Attack4 / Spider Walk8 USER_ART_APPROVED** 확정. #84 기존 Unity37PASS 재사용·재실행0. Owl ZIP d6648b7451dc2184c2187c0b1c2f8e0f52fc800ac6975753d0f6d38f6fbf9192 / Spider ZIP 771bd1813608aa3e350c6124ab0d43293c6d2988b1833014da64484d5272c8da 동결. 기존 Idle·정지원화 보호·추가 재작화 금지. Owl VFX HOLD, Spider 발 접지·미끄러짐 NOT_VERIFIED / WALK_CONTACT_HOLD, Organic Attack8/Shoot8 미제작. 과거 미술 승인 대기는 이번 승인으로 해소됐습니다.

Bee 실제 코드: 적 좌측→아군 우측, 전열/후열·Column별 대상, 명목 .25초 피해(index02@10FPS), 정지거리145 UI. 기본 플레이어와 사각형 사이도 최소31.2 UI 간격이고 ContactFrame/socket 없음. Attack10은 약.61초에 조기 종료될 수 있어 **TARGET_CONTACT_HOLD / MOTION_REVISE_CONTINUE / FULL_COMBAT_READY_NO**. 필요한 보완은 Sting/Recover 두 후보뿐이며 정확 접점·거리/시간 승인 전 계획 문서까지만 진행합니다.

PilotBee 원배포 웹BY3.0 vs 원본 ZIP 내부BY4.0 충돌 재확인. MonsterValidation의 활성2PNG가 Bee 정의에 참조되며 외부 보관 원본과 byte-exact여서 **RIGHTS_HOLD**. ThirdParty 보관 파일 직접참조0과 활성 사본 사용을 구분했습니다. 새 SHA만으로 독립 권리를 확정하지 않습니다.

[승인·좌표·권리 근거](../../OwlSpider_FinalApproval_BeeTarget_RightsContract_20261103.md) / [리뉴얼 최소 지시서](../../VenomBee_Minimal_StingRecover_ArtBrief_20261103.md). branch `codex/bee-target-rights-and-art-approval-20261103`, 출발 c6eab1bb. 문서·조사 JSON/TXT만 변경, 원본 Unity/Save/UserData/Scene/Prefab/BuildSettings/Packages·Discord66·Push/merge0, Unity 실행/기존QA 반복/신규PNG0. NOT_UNITY_APPLIED / REMOTE_NOT_PUSHED.

---

## 2026-11-03 Issue #84 실제 Unity AnimationClip Delta

**OWL_CLIP_DELTA_PASS / SPIDER_WALK_CLIP_DELTA_PASS / ACTUAL_GROUND_LOCK_NOT_VERIFIED / WALK_CONTACT_HOLD / USER_FINAL_ART_REVIEW_PENDING / NOT_ORIGINAL_APPLIED / REMOTE_PUSH_NO**. 독립Unity6000.6.5f1 PlayMode/실제Animator·Clip으로신규37PASS/FAIL0. Owl8FPS/.5초/PPU512/중앙/Scale1→Idle00두회,Spider10FPS/.8초loop/PPU256/Scale.7/우향·0/.65/1.3WU/s각두loop·40/128투영확인. 실시간504샘플·RT122장외곽alpha0,실영상4개/126프레임. 발별rig/Collider·접촉계측없어실접지/미끄러짐은NOT_VERIFIED,전체보행승인아님.

기존ZIP2SHA동일로파일품질QA재사용·기존게임전수QA/Bee재검수0. 새Runtime오류0/compile0,기존Editor.Search예외1환경로그분리. 초기생성기끝키길이오류수정/초기기록보존. [상세·설정·실영상](../../Issue84_OwlAttack4_SpiderWalk8_UnityAnimationDelta_20261103.md). 증거판 `F:/study/codex/Project-Limitless/Temp/Issue84OwlSpiderUnityDelta20261103/Review/index.html` SHA `41e8e303062a29c753a55f2702940038b9b469e884e4f9e2f7570ecea9566173`(OFFLINE열람/내용은실UnityRT/OSGameView아님),브라우저23PASS. branch `codex/owl-spider-unity-animation-delta-20261103`,출발3d61b491. Tools검증코드2+문서/근거만로컬commit,원본main/status344(상세858)·참조28SHA/Save보호·원본적용/Push/merge0.

---

## 2026-11-03 Owl Attack02 + Spider Edge Fix 독립 읽기 검수

**LOCAL_ART_REVIEW_DONE / USER_VISUAL_REVIEW_PENDING / NOT_UNITY_APPLIED / REMOTE_NOT_PUSHED**. ZIP2 CRC/SHA/Manifest/SUMS PASS(Owl17/19,Spider18/19),새PNG13decode·동일14QA재사용·GIF4순서/실시간확인. **Owl ATTACK02_CONTINUITY_OK_CANDIDATE:**02만126301px수정·00/01/03exactKEEP,03하단y345~511exact/01→02·02→03IoU개선·평균8FPS,미승인VFX재결합0. **Spider WALK8_EDGE_FIX_OK_CANDIDATE:**문제4장만8813/9413/10604/10446px수정·00/03/04/05KEEP,8장모든네변alpha>0접촉0·512/몸통보존·정확10FPS. 전체보행/실접지승인은별도대기.

[정확SHA·통합보고](../../OwlAttack02_SpiderWalk8EdgeFix_ReadOnly_Review_20261103.md). OFFLINE/Unity아님 비교판 `F:/study/codex/Project-Limitless/Temp/OwlSpiderEdgeReview20261103/Review/index.html` SHA `26aa98c601f8c19ba7916f9b5f1f627fc52483b5700dd3262bec4d9cf881565c`·브라우저34PASS. branch `codex/owl-spider-edge-review-20261103`,출발34f2ad88.원본main/status344(상세858)·28참조SHA/개인Save보호·Unity/기존QA·Bee재검수/원본적용/Push/merge0.

실제 검수 commit **`9efa30def1c86ee05b90ac1e7b1bfb6a17ca3ea5`** 기준 [#74 통합검수](https://github.com/HappyPaul812/Project-Limitless/issues/74#issuecomment-6596846762) · [#60 Edge수정후보](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6596853232) · [#83 보호/연결](https://github.com/HappyPaul812/Project-Limitless/issues/83#issuecomment-6596854163) 실제 댓글 등록 성공. 원격에는 댓글만 게시·파일Push0, 후속은 영수증만 로컬 기록합니다.

---

## 2026-11-03 Owl Continuity + Spider Walk8 Fix 독립 읽기 검수

**LOCAL_ART_REVIEW_DONE / USER_VISUAL_REVIEW_PENDING / NOT_UNITY_APPLIED / REMOTE_NOT_PUSHED**. 두ZIP CRC/SHA/Manifest/SUMS PASS(Owl18/21,Spider16/17), 새PNG24decode·동일3QA재사용·GIF4확인. **Owl DESIGN_CONTINUITY_REVISE**: 얼굴복원/새VFX분리/03=Idle00exact 개선,00~02몸통/깃질감·02→03연속성 보완. 별도VFX_HOLD. **Spider WALK_CONTACT_REVISE**:00/04KEEP·6실변경·정확10FPS, 접지교대미흡과새01/02오른쪽·06/07왼쪽캔버스접촉(최대255/255/255/41)발견. 승인v5/Idle8보호. seamGIF는실1프레임250ms정지자료,8FPS모션증거아님.

[통합 상세·전체SHA·근거](../../OwlContinuity_SpiderWalk8Fix_ReadOnly_Review_20261103.md). 단일OFFLINE/Unity아님 비교판 `F:/study/codex/Project-Limitless/Temp/OwlSpiderFollowupReview20261103/Review/index.html` SHA `ca98de10c55e0ef0b4601fe44703780ec78ffef8c6dd4a73908c1e9763d02d74` · 브라우저34PASS. branch `codex/owl-spider-followup-review-20261103`, 출발ed95f352. 원본main/status344(상세858)·28참조SHA보호·Unity/Save접근/기존QA·Bee재검수0·Push/merge0. 최종미술승인과실제품적용은별도입니다.

실제 검수 commit **`5fdc6201d9484967454e7aa53f7088460ac519c0`** 기준 [#74 통합결과](https://github.com/HappyPaul812/Project-Limitless/issues/74#issuecomment-6595705479) · [#60 Spider 새경계결함](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6595712350) · [#83 보호/연결](https://github.com/HappyPaul812/Project-Limitless/issues/83#issuecomment-6595713003) 실제 댓글 등록 성공. 원격에는 댓글만 게시·파일Push0. 후속은 보고 영수증만 로컬 기록합니다.

---

## 2026-11-03 Main24 후속 아트3ZIP 독립 읽기 검수

**LOCAL_ART_REVIEW_DONE / USER_VISUAL_REVIEW_PENDING / NOT_UNITY_APPLIED / REMOTE_NOT_PUSHED**. Owl5ba6ecff(3153329B/18), Bee450b78e4(709946B/9), Spiderd4c7c73a(7953765B/33) CRC·경로/중복PASS, SUMS17/7/31 전부 일치. Owl Manifest신규4+Idle참조8 확인, Bee7/Spider31 독립Manifest일치. 새PNG34/GIF9만 decode, 동일SHA PNG7 QA재사용.

**Owl ANIM_REVISE_CONTINUE:** 새Attack4·40px 날개 대비 개선/클리핑0·8FPS정확, 승인v3 대비 얼굴·깃털/몸통·추가냉기궤적은 디자인/VFX_SCOPE_HOLD. Idle8/정지SHA보존. **Bee TARGET_CONTRACT_HOLD/RIGHTS_HOLD:** 기존Attack3/Idle2 byte exact, 새것은TEST_ONLY보드1장뿐/새StingRecover0. **Spider MOTION_REVISE_CONTINUE:** 승인v5/Idle00c600cd…KEEP·나머지15새PNG; Idle호흡후보, Walk8 독립관절/지지·스윙 가독성 보완. ‘8FPS’GIF4개 실제120ms=8.333FPS(명세와차이), 10FPS정확. Idle시트00 차이는alpha0 RGB만/가시동일.

[단일 보고·근거](../../Art_OwlAttack4_BeeTarget_SpiderIdleWalk_Review_20261103.md). 비교판 `F:/study/codex/Project-Limitless/Temp/ArtOwlBeeSpiderReview20261103/Review/index.html` SHA `d08aea841659a8840d04aa9ddcf03605e9b392971e999212b4748eea560b9e20`, OFFLINE/Unity아님·실제 GIF순서9/9·브라우저37PASS. branch `codex/art-owl-bee-spider-review-20261103`, 출발24232ccd. 원본main/status344(상세858)·이전원본SHA·Save/UserData/Unity보호, Editor/기존모든QA재실행0·원본적용/Push/merge0. 다음은 Owl연속성/FX·SpiderWalk국소보완·Bee실표적/권리 계약, 미승인모션 양산/전투교체 금지.

실제 검수 commit **`acf2031029741185d17190562a5ec0070d5d773b`** 기준 GitHub [#74 통합보고](https://github.com/HappyPaul812/Project-Limitless/issues/74#issuecomment-6594778265) · [#60 Spider](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6594793586) · [#63 Bee](https://github.com/HappyPaul812/Project-Limitless/issues/63#issuecomment-6594794350) · [#83 연결](https://github.com/HappyPaul812/Project-Limitless/issues/83#issuecomment-6594795090) 댓글 등록 성공. 원격에는 댓글만 게시했고 PNG/GIF/ZIP/HTML/문서파일 Push0입니다. 후속 커밋은 이 영수증과 보고서 끝 공백 정리만 포함합니다.

---

## 2026-11-03 Issue #83 — Main24 북문→F2→북문 실제 격리 왕복 PASS

**MAIN24_F2_TRAVERSAL_ISOLATED_PASS / USER_PLAY_REVIEW_PENDING / NOT_ORIGINAL_APPLIED / REMOTE_NOT_PUSHED**. 기존 Main24 후보를 재사용해 `World_NorthF2_CANDIDATE` 안전길·관찰·북문 복귀 구현. 실제 Player/Collider 휠체어·도보·E/F/마우스·취소·반복출입·합성 SaveContinue·6번째 슬롯/화폐/마을거점·F1 연결 보호 **신규 Unity90 PASS + 정적23 PASS**. 기존81/저장85·72·72/Main23/그루터기21 재실행0.

승인 Stump SHA22f2f581… 그대로 PPU128/Pivot(.5,11/384)/Scale1, 실제 줄기충돌·우회·Player 앞뒤Sorting·3비율 RT 확인. 눈/흙길/나무3PNG는 명시 PLACEHOLDER, 미승인 표지판/울타리·전투·몬스터·구매·Main24 완료/보상·Main25/SpiderBee 적용0. 기존 네 목표ID와 마지막 대기 목표 보존. Scene BuildSettings 연결은 QA 복제본에만 추가, 원본/작업브랜치 설정 변경0.

branch `codex/main24-f2-first-traversal-20261103`, 기능 **`e32d814f7f0dee8622cf386541eab94bf1fc1d69`**. 원본main ab47b931/status344 전후 동일·지정원본8SHA보호·개인Save/UserData열람/복사/해시0. 실제6000.6.5f1 숨김 PlayMode/Camera RT6장, OS GameView/사람 손플레이 NOT_VERIFIED. 기존 Editor.Search 예외1건 별도보존(게임Runtime오류0). [보고·근거](../../Main24_F2_FirstTraversal_Isolated_Issue83_20261103.md). 로컬 열람판 `F:/study/codex/Project-Limitless/Temp/Main24F2FirstTraversal20261103/Review/index.html`. GitHub 보고 영수증은 후속 문서로 기록합니다.


GitHub 보고 영수증: 기능 `e32d814f7f0dee8622cf386541eab94bf1fc1d69`, QA/보고 `8cfd6d0cbffdd8fcdfac4501d6824d9f5d113b05` 기준 [Issue #83](https://github.com/HappyPaul812/Project-Limitless/issues/83#issuecomment-6594041936) 실제 상세 댓글 및 [#80](https://github.com/HappyPaul812/Project-Limitless/issues/80#issuecomment-6594047044) · [#82](https://github.com/HappyPaul812/Project-Limitless/issues/82#issuecomment-6594047914) 연결 댓글 등록 성공. 원격에는 댓글만 게시, 파일 Push/PR/원본 적용0. 본 후속 커밋은 보고 영수증만 추가합니다.

---

## 2026-11-03 Issue #82 — 그루터기 독립 Unity21PASS / Spider v5 알파후보 / Bee 모션HOLD

승인그루터기 SHA22f2f581…1PNG만 독립Unity6000.6.5f1 fixture로 Import: PPU128/Scale1·Pivot(0.5,11/384) 후보, alpha/접지·3화면비·Sortingproxy·대안크기 **21PASS/FAIL0**. 실제F2 Scene/길/Collider없어도보·휠체어·실배치 NOT_VERIFIED. 기존ProjectVersion/Packages·게임원본변경0.

spider.zip SHA7f4622af…/13항목, bee.zip SHAa396a2be…/20항목 CRC/경로PASS. 합본Manifest가동봉되어각각미동봉참조16/9개는부분범위; 두ZIP합집합Manifest27/SUMS28PASS. v5c600cd22…RGB262144보존·alpha380감소·실Field03경계비교로 **ALPHA_FINAL_OK_CANDIDATE/최종미술승인대기**. 새32보류. Bee최신3SHA/실픽셀변경확인·Idle2exact, 실제Sting침끝우하향관찰은보고서'좌측'과차이로명시. 실표적·회수연속성 **BEE_MOTION_HOLD/RIGHTS_HOLD**, 전체20/교체0.

단일결과판 `F:/study/codex/Project-Limitless/Temp/Issue82StumpSpiderBee20261103/Review/index.html` SHA `730c8481819d6dcbefdb3b6f772e3f6914d7f37358169f85b11414ba1de4a748`. OFFLINE비교와실제UnityCamera캡처7장구분·브라우저14PASS. [상세보고](../../Issue82_Stump_UnityDelta_SpiderV5_BeeAttack3_20261103.md). branch `codex/issue82-stump-spider-bee-delta-20261103`·문서3+격리QA스크립트1만로컬commit. 원본main/status344·Main24fd869f60/81·14PASS보존·Save/Unity원본/Discord/포커스/기존전수QA0. **STUMP_USER_APPROVED / STUMP_ISOLATED_UNITY_PASS / SPIDER_ALPHA_CANDIDATE / BEE_MOTION_HOLD / NOT_ORIGINAL_APPLIED / REMOTE_PUSH_NO**.

실제 검증/보고 commit `b50ce140cf155900829bd167aefb6225f806fa9a`. GitHub [#82](https://github.com/HappyPaul812/Project-Limitless/issues/82#issuecomment-6593291711) · [#75](https://github.com/HappyPaul812/Project-Limitless/issues/75#issuecomment-6593303847) · [#60](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6593304719) · [#63](https://github.com/HappyPaul812/Project-Limitless/issues/63#issuecomment-6593305654) · [#81](https://github.com/HappyPaul812/Project-Limitless/issues/81#issuecomment-6593306460) 결과 댓글 등록 성공. 원격에는 댓글만 게시했고 파일Push/원본적용0. 이 후속 commit은 문서 영수증만 기록합니다.

---

## 2026-11-03 바울 대표 승인 — F2 자연형 그루터기 v4 USER_APPROVED

`F2_SnowStumpPerch_NaturalBroken_v4_CANDIDATE_320x384_RGBA.png` SHA `22f2f5816b3fd2ffe45234ec807bb3d81a83f87440eb8eb1f338e0dff33404a8`를 이번 채팅 명시 승인으로 **USER_APPROVED / APPROVED_ART_CANDIDATE** 처리했습니다. 기존파일 SHA 재확인·바이트변경0. 예전 Stump v1/v2/v3·FULLREDRAW는 **SUPERSEDED / ARCHIVED_CANDIDATE**로 문서분류하고 삭제/이동하지 않습니다.

승인은 그루터기1PNG에만 해당합니다. Sign/Fence FULLREDRAW는 KEEP_CANDIDATE/미승인 그대로, Spider ALPHA_FINAL_HOLD·Bee MOTION_REVISE/RIGHTS_HOLD 미승인 유지. **UNITY_DELTA_PENDING / NOT_UNITY_APPLIED**. 다음은 별도 격리1PNG 최소 표시·알파·실비율/시야·통행 검증 필요. PPU/Pivot·Collider·휠체어는 SPEC_PENDING/NOT_VERIFIED. F2 미구현맵/전투 신규개발 허가 아님.

[승인 정본·최소Delta 준비](../../F2_NaturalStump_v4_UserApproval_And_UnityDeltaPrep_20261103.md). 브랜치 `codex/f2-natural-stump-approval-20261103`, Markdown5개만로컬반영/Push·main·merge·Unity·Save/UserData변경0. Main24 81/14 및 기존QA 재실행0. 아래 KEEP/승인대기/HOLD는 과거 당시 기록이며 현재 승인 상태는 본 항목을 따릅니다.

승인 문서 commit `fe92433aac1d5768a438f2dd3d5366f480f5cacb` 기준 [Issue #75](https://github.com/HappyPaul812/Project-Limitless/issues/75#issuecomment-6592422354) · [Issue #81](https://github.com/HappyPaul812/Project-Limitless/issues/81#issuecomment-6592423200) 실제 USER_APPROVED 보고 등록 성공. 로컬 Markdown만 반영/원격 파일Push0, 본 후속은 영수증 기록입니다.

---

## 2026-11-03 Issue #81 후속 — 자연형 그루터기·Spider v4·Bee 새5 실제 델타

ZIP2실물 CRC/경로·보고SHA일치. F2 820391bytes/6항목·Manifest3/SUMS5PASS, Renewal2444044bytes/35항목·Manifest33/SUMS34PASS. 미동봉F2큰source원화 자체는미검증.

**자연형그루터기v4 KEEP_CANDIDATE**: 원판/가구지주 제거·불규칙 부러진 가지·40px실루엣 유지. Sign/Fence FULLREDRAW KEEP그대로. **Spider REVISE/ALPHA_FINAL_HOLD**: RGB262144동일·alpha491감소에도 밑띠잔존·ROI끝절단경계, 최종OK아님. **Bee NEW_ART_DELTA_CONFIRMED + REVISE/MOTION_REVISE_CONTINUE/FULL_COMBAT_READY_NO**: Old5정확보존·New5는 SHA/픽셀모두다르지만 머리우향/침좌향·관절/회수미완. GIF2/3포즈 각100ms·10FPS순서확인. RIGHTS_HOLD유지·양산/교체0.

단일 OFFLINE(게임뷰아님): `F:/study/codex/Project-Limitless/Temp/Issue81ArtDeltaFollowup20261103/Review/index.html` SHA `83b32736108df60d66e401da7270e4ae8556ee5ffc27d74d95515e2fa0d5a6ef`. 512/128/40·실Field03배경·이전/신규순서제어, 브라우저18PASS. 입력48SHA·원본main/status344·Main24 fd869f60/81·14PASS보존. Unity/개인Save/Quest/버전/Discord변경0·전수QA0.

[후속 실제 검수보고](../../Issue81_NaturalStump_SpiderBee_ActualDelta_Review_20261103.md). 브랜치 `codex/issue81-art-delta-review-20261103`, Markdown2개만로컬commit/Push·본편적용0. **LOCAL_ART_DELTA_REVIEW_DONE / USER_VISUAL_REVIEW_PENDING / NOT_UNITY_APPLIED**. 다음은대표그루터기1개미술판단, Spider경계·Bee공격축최소보완뿐이며 32/20프레임 양산승인 아님.

검수/인계 commit `cae0d8efebbfbf12bec51e15dd7ea8746f03bacc` 기준 실제 GitHub 보고 [#81](https://github.com/HappyPaul812/Project-Limitless/issues/81#issuecomment-6591984789) · [#75](https://github.com/HappyPaul812/Project-Limitless/issues/75#issuecomment-6591993553) · [#60](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6591994340) · [#63](https://github.com/HappyPaul812/Project-Limitless/issues/63#issuecomment-6591995213) 등록 성공. 원격에는 댓글만 게시했고 파일Push/게임적용0. 본 후속 commit은 문서 영수증만 기록합니다.

---

## 2026-11-03 Issue #81 — F2/Spider/Bee 개별3ZIP OFFLINE 델타 검수

세 ZIP 실물 CRC·경로 PASS. F2 보고 ZIP SHA 일치/SUMS13·Manifest참조14 PASS. Bee/Spider 전체Manifest 미동봉은 NOT_SUPPLIED. Bee5PNG/GIF2는 이전과 SHA7/7동일·새개선0, 기존20 참고SHA는 실제 로컬20/20일치. 합본ZIP 자체는 로컬미확보/전체동일성미검증.

F2 국소v3 표지판/그루터기/울타리 REVISE; 독립전면대안 표지판·울타리 KEEP후보/그루터기HOLD. Spider RGB262144동일·alpha282감소 확인에도 하단회백색띠잔존으로 ALPHA_FINAL_HOLD(완전제거OK 아님). Bee 공격축·관절·회수 REVISE, GIF2/3프레임 각100ms/10FPS·체커배경포함. PilotBee CC BY3/4권리HOLD 유지. 대표미술승인/실Unity길·Collider/휠체어/본편적용은 별도.

단일 OFFLINE 비교판 `F:/study/codex/Project-Limitless/Temp/ArtMain24ThreeZip20261103/Review/index.html` SHA `12ee5ace73b2daef55c0dd8c4a9ab8e59ca3e2982d25a00c7eb851feefdb7e43`. 밝음/어둠/실Field03지면·128/40·GIF/PNG순서제어 제공, 브라우저18PASS. 입력70SHA변화0·원본main/status344동일·Main24 #80 HEAD fd869f60/81·14PASS보존·Unity/Save/Quest/버전/Discord변경0·전수QA반복0.

브랜치 `codex/art-main24-3zip-delta-20261103`; 문서2개만로컬commit/Push·본편적용0. [상세보고](../../Art_Main24_ThreeZip_Delta_Review_Issue81_20261103.md). **LOCAL_ART_DELTA_REVIEW_DONE / USER_VISUAL_REVIEW_PENDING / NOT_UNITY_APPLIED**. 다음은 F2대안2개선택+그루터기형태, Spider밑띠/Bee기존5포즈 표적보완뿐이며 양산승인 아님.

실제 검수 commit `3af7f08d4febf062865ef5b437e0951e80315bd6`. GitHub 보고 영수증: [#81](https://github.com/HappyPaul812/Project-Limitless/issues/81#issuecomment-6590636705) · [#75](https://github.com/HappyPaul812/Project-Limitless/issues/75#issuecomment-6590647264) · [#60](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6590648055) · [#63](https://github.com/HappyPaul812/Project-Limitless/issues/63#issuecomment-6590648852). 댓글 등록 성공/파일Push0, 본 후속은 문서 영수증만 기록합니다.

---

## 2026-11-03 Issue #80 — Main24 비결제 첫 플레이 격리 PASS / 본게임 미적용

- Main23 완료 뒤 기존 마을 표식으로 스킬 기본3무료/최대6칸/전투밖 무료 장착 안내→보급 한기2턴·해제수단 안내→북문 이동·서리올빼미 위험 예고를 구현했습니다. 기존6칸 UI·Save 재사용, 구매/탈렌트차감/소유권 변경/무료 아이템 지급0. 후보 ID 정의는 바울 대표 이번 채팅 허용 근거이며 정식 Story/TTS 잠금과 구분합니다.
- F2 Scene/첫 전투는 미구현이므로 마지막 목표를 **F2 진입 대기**로 유지합니다. Main24 완료/보상/다음 Quest 해금0. 마을 자유이동·남문F1 왕복·체크포인트·Save/Continue 보존.
- Unity **6000.6.5f1** 배치 격리 Play Mode **81 PASS/FAIL0**, 신규 정적14PASS/FAIL0·diff공백PASS. E/F/Esc/마우스는 가상입력, 실제6칸 UI 핸들러·휠체어/도보 경로·Continue 검증. Camera+Canvas PNG4장/3화면비 확인. 물리 입력/패드/발·스크린리더/본게임 적용 미검증, 기존 Search 초기화 예외OPEN.
- 브랜치 `codex/main24-first-playable-no-purchase-20261103`, 제품 commit `70e363bea43d72c7e68abb089952e2d6f6a77a8b`. 로컬 main `ab47b931`, GitHub main `bfd28154` 실측. #70/77/78 미병합, #79 정렬후보/ProjectVersion/Packages/원본Save·아트·TTS·Discord#66/Editor포커스 보존. **FIRST_PLAYABLE_ISOLATED_PASS / MAIN_NOT_APPLIED / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.
- 다음: F2 공식Scene·첫 한기전/무자금 해제수단 승인, Owl Attack HOLD 해소, 상품25·가격/효과/ID 별도 승인. 기존85/72/72·Main23 148/136 전수 반복0. [상세 보고](../../Main24_FirstPlayable_NoPurchase_20261103.md).

- GitHub 보고 실제 등록 완료: [Issue #80 결과 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/80#issuecomment-6590035953). 제품 `70e363be`, QA/문서 `5b02b56e`; 파일 Push 없음. 이 영수증은 문서 후속 commit입니다.

---

## 2026-10-29 Issue #76 — LOCAL_SYNC_DONE (#46·#73만) / #70 HOLD

실제 원본 `F:/study/codex/Project-Limitless`의 main을 ff-only 동기화했습니다. 시작 `bfd281545632877325f5d21e04cefb924042c465` → 제품 `27f7d1db9c3510fd39ca8bc34f09d8b8cb20e9f1`. 통합 브랜치 `codex/local-sync-20261029`, #46 제품 commit `356f9226b8ed83120f828709045d50a5041055a7`, #73 제품 commit `27f7d1db9c3510fd39ca8bc34f09d8b8cb20e9f1`. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. main 신규 commit·Push·PR·원격 병합0.

신규 정적11 + 격리 Unity29 PASS(실제 Continue 포함). #70 경계9개 재현 중 Writer1 최신 진행도가 Writer2에 숨겨지는 문제가 확인되어 HOLD_INTEGRATION·원본 제외했습니다. Version1 Save 유지, 퀘스트 재료·보상·중복방지 불변. BuildSettings25 Scene/GUID와 사용자 app-ui 보존, 원본 사용자 dirty status 동일, 제품 외7085 비개인 tracked SHA 동일. 개인 Save/UserData 열람·SHA·복사·수정0, 기존 worktree/Temp 삭제0, 화면포커스/Discord #66 변경0.

Owl v3 외형·유기형 Spider 디자인 승인 유지, 각각 Attack/왼 앞다리 알파 보완 대기. 메카닉 Spider는 Chapter4 보존, F2/Bee/상품25종·가격·해금·무자금 한기 대응은 미적용·미승인 상태 유지. 다음: Writer 분기 안전성 → 원본 수동 import/지정Unity5.7 미검증 → 미술 국소 보완/상품 결정. 격리 검증 Unity6.5만 PASS, 원본 Editor 실행0·Search 환경 예외 OPEN.

[상세 보고](../../Local_Integration_And_Original_Sync_Issue76_20261029.md) · [Issue #76](https://github.com/HappyPaul812/Project-Limitless/issues/76)

### Issue #76 GitHub 보고 확인

보고·QA 문서 commit `dd53ec3265cb3b0039c565936b6dbb610fc0a1d4`까지 실제 원본 ff-only 반영 완료. [실제 Issue #76 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/76#issuecomment-6495733874) 등록 성공. #70 HOLD 경계와 #46·#73 동기화도 각 Issue에 보고했습니다. 이 영수증은 별도 문서 커밋으로만 추가하며 제품 HEAD는 `27f7d1db9c3510fd39ca8bc34f09d8b8cb20e9f1` 그대로입니다. 최종 문서 포함 HEAD는 Issue #76 마감 댓글을 따릅니다.


---

## 2026-10-24 EOD 마감 — PR68 MERGED / 원본 동기화 PARTIAL·HOLD

[PR68](https://github.com/HappyPaul812/Project-Limitless/pull/68) 실제 merge **`b0a666be631e87e451d8bc8d6549a6b664b03ee7`**. Main23 KEEP 기능20파일 + TTS162(Unity189파일/140교체22신규) + 재현도구3파일, 정본·인계·QA 근거241파일을 포함한 총453파일 통합 완료. 신규 정적472/격리 Unity25 PASS·compile0/제품runtime오류0, 기존 전수QA 재실행0. 기존 Search예외1/비음성GUID중복/지정Unity6000.5.7 직접검증은 OPEN. 미승인 아트·오디오 적용0, Main24 Save 미구현, Discord #66 MANUAL/HOLD·추론/재시도/설정변경0.

**원본 동기화는 HOLD입니다.** 원본 `EditorBuildSettings.asset`의 사용자 `com.unity.dt.app-ui` 설정과 Main23 Scene 등록이 같은 경로를 수정하여 복제본 ff-only가 거부됐습니다. 계약에 따라 원본 merge/덮어쓰기/index우회0. 원본 main **`47ad2eecb40135b2ee28e4947d642ebdbf0bf68a`**, 이 제품병합 시점 GitHub/origin main **`b0a666be631e87e451d8bc8d6549a6b664b03ee7`**; 3자 불일치이므로 SYNC_COMPLETE 아님. 후속 docs-only PR 이후 최종 원격SHA는 Issue67·로컬 `Temp/EOD20261024/FinalProof.json`을 따릅니다.

85 worktree 백업14433파일(1,636,665,989bytes). 원본 사용자858/보호6842/TTS1430 SHA 불일치0, 원본 status바이트/index항목/HEAD 보존, Save/UserData·Editor·포커스 변경0. 다음은 사용자 app-ui 설정을 보존한 BuildSettings 의미 결합과 안전 원본 동기화 범위 확정입니다. 자동 재작성하지 않았습니다. [상세](../../EOD_ApprovedIntegration_LocalSync_20261024.md).

## 2026-10-24 EOD 승인분 통합 — Delta QA PASS / PR 진행 / 원본 SYNC HOLD

Main23 KEEP 아트 Scene과 5목표·접근성 보완20파일 + 승인 TTS162(Unity189/140교체22신규/Catalog312)를 선별 통합했습니다. 제품 commit `26643f16531e5d2b7be635d95c02ff06c12c3a88`. 신규 정적472·격리 Unity25 PASS, 기존 전수QA 재실행0. 미승인 시안18 PNG/오르나/남문 부품/여관 간판/독침벌/NPC/BGM/SFX 제외. Main24 Save 미구현·OPEN16·Discord #66 MANUAL/HOLD 유지.

원본858파일/보호6842/TTS1430 SHA 보존, 85 worktree 비파괴백업 완료. 원본 BuildSettings 사용자 app-ui 설정과 Scene 추가의 경로 충돌로 복제본 ff-only 거부. 원본 main `47ad2eec` 유지·동기화 HOLD이며 SYNC_COMPLETE 아님. Editor·Save·포커스 변경0. [상세](../../EOD_ApprovedIntegration_LocalSync_20261024.md).

## 2026-10-22 EOD 마감 — PR54 병합·원본 SYNC_COMPLETE 확인

[PR54](https://github.com/HappyPaul812/Project-Limitless/pull/54) merge `e2801286ccd26fe2060d9fee658ac272da4b4f2b`: 승인 Bear v4·Fox v7 공유 코드 양쪽 보존6파일 + 마을 최종승인9 Sprite/Tile 신규등록39파일 = 제품45파일, 담당 문서·근거152파일, 총197파일 통합. 신규 정적10/격리Unity15 PASS, 기존154/162/Wolf/Main22 전체QA 재실행0.

실제 GitHub main=origin/main=원본local main=e2801286, index=HEAD/staged0 확인. 백업912파일190,738,290bytes SHA불일치0, 보호7179파일 중 승인문서6개 외 변경0. Save/UserData/개인설정/Editor·미커밋 작업 보존. 이미 원본적용 Bear/Fox6파일 재작성0. dirty310→308/untracked554→550은 승인6경로 Git 편입이며 그 밖 상태 동일. 원본Editor/Play/포커스0, 자연Import·손플레이는 미검증.

이 문서는 PR54 완료 시점 스냅샷입니다. 문서 후속PR 이후 최종3자 SHA는 로컬 `Temp/EOD20261022/OriginalLocalSync_Proof.json` 및 Issue #53 완료기록을 정본으로 확인합니다. 마지막 관련 제품통합 commit e2801286. 추가제품 변경0.

OPEN16 미해결16 유지, Main23 전체·Main24 Save #46 HOLD. 미승인TTS READY104/152·Main21 KEEP WAV7·미완BGM/SFX 게임 적용0. 다음은 별도 승인 후 해당 범위만 진행합니다. 기존 승인기능 재구현·전체QA 반복 금지.

## HISTORY — EOD 실행 전 기록 보존

## 2026-10-22 EOD 승인분 통합 — 신규 delta PASS / PR·원본 동기화 대기

Issue #53: Bear Run6 v4 + Fox Run6 v7 공유 펫 정의를 모두 보존하고 승인 제품6파일을 선별했습니다. 기존154/162 전수QA 재실행0, 원본 이미 적용된 파일 재복사0. 마을 최종승인9 PNG는 별도 FrostwayVillageV10 Sprite9/Tile9 및 meta39파일로 등록, 기존49/Scene/Sorting/통행 변경0. 제품 총45파일. 격리 Unity15 PASS/0FAIL, 신규 정적10 PASS. 기존 Wolf/Main22/F1/서리들쥐 재검수0.

담당 문서는 오늘 변경분만 파일별 병합·고유 HISTORY 보존. TTS READY104/152는 청취승인이 아니며 Main21 KEEP WAV7 및 미완 BGM/SFX 제품 적용0. OPEN16 미해결16, Main23 전체·Main24 Save #46 HOLD.

원본912파일190,738,290bytes SHA백업, 총7179 SHA 보호. 원본Editor/Play/포커스0. 브랜치 codex/eod-integration-20261022. 마지막 승인 제품 commit cad73927(선행 Bear 9cf0e569). 최종 PR/merge/동기화 증거는 후속 마감 기록으로 확정합니다.

## HISTORY — 실행 전 기존 기록

## 2026-10-21 EOD 완료 — PR48 병합 / 원본 SYNC_COMPLETE

[PR48](https://github.com/HappyPaul812/Project-Limitless/pull/48) 승인 Wolf6+Main22/F1/Mouse52 제품58 및 담당 문서 선별통합 **`c2750d1faea7ef9d54df1b4fcd7d6efb2bd49ac8`**. 이 완료증거 작성시 GitHub main=origin/main=원본local main, index=HEAD/staged0 실측 **SYNC_COMPLETE**. 후속문서 PR 이후 최종SHA는 `Temp/EOD20261021/OriginalLocalSync_Proof.json` 및 Issue47 완료기록을 따릅니다.

기존 Wolf106/Main22 136/F1 64/Mouse40 재사용·전수재실행0. 신규 정적38/격리Unity14 PASS. Main22는 PROTOTYPE 대사/수치·보상·경제 미확정. Village v5 타일3 ART_HOLD/OPEN16 해결0·Main23/24미구현, 청취미승인TTS READY64/152·미완BGM/SFX/영상은 제품제외 유지.

957파일196,911,769bytes 백업·6758SHA 보호. 승인범위밖 SHA/사용자상태차이0, 이미승인50파일 무재작성. BuildSettings 개인config·원본문서고유이력 보존. dirty314→308/untracked595→550/staged0, 승인경로 편입에 따른 변화. 원본Editor/Play/Refresh/포커스0, 자연Import·손플레이 미검증·URP/Search OPEN. reset/clean/stash/force/main직접Push0.

내일: 대표 평소 자연Import·기존 Main21완료 캐릭터 북문→Main22 시작 확인, 마을타일 수용/OPEN16 처리범위·정식대사/TTS·보상 설계·Main24Save #46 별도 결정. 완료 기능 재구현/전체QA 반복 금지. [완료 보고](../../EOD_ApprovedIntegration_LocalSync_20261021.md).

## HISTORY — 아래는 실제 EOD 실행 전 기록

## 2026-10-21 EOD 승인분 선별 통합 — 신규 최소검증 완료 / PR·원본동기화 진행 중

Wolf Run6 v5 승인6파일 + Main22/F1/서리들쥐52파일 선별, 제품58파일. Main22 대사·TTS·전투수치·경제·보상은 PROTOTYPE/DESIGN_NOT_LOCKED. 기존 Wolf106/Main22 136/F1 64/Mouse40 각 QA 재사용·전수재실행0. 새 정적38+격리 Unity14 검증 PASS, Scene/Quest/리소스·Save 선택필드 연결 확인.

Village v5 타일3 미술HOLD·OPEN16 해결0·본편미구현, 청취 미승인TTS READY64/152·미완BGM/SFX/영상 제품제외. 담당 문서는 파일별 선별·의미병합/고유HISTORY 보존. raw 진단로그는 개인경로/라이선스정보 비공개로 로컬보관, 공개본에는 판정마커만 유지.

새 실측 dirty314/untracked595/staged0, 사용자957파일196,911,769bytes 백업·보호6758SHA. 원본Editor/Play/Refresh/포커스 조작0. branch `codex/eod-integration-20261021`, 제품 commit `b53344ce`. 아직 PR병합·원본SYNC 완료 선언 아님. [EOD 보고](../../EOD_ApprovedIntegration_LocalSync_20261021.md).

## 2026-10-20 EOD 완료 — PR41 병합 / 원본 SYNC_COMPLETE

[PR #41](https://github.com/HappyPaul812/Project-Limitless/pull/41) 승인 통합 main **`db242b77377b7c2ec08664f102b1a6bab0b3ffb3`**, 통합 head `ccad0344dacf647bd2c3530d2284783e24e7f784`. 완료 증거 작성 시 **GitHub main=origin/main=원본 local main**, index=HEAD 및 stage0 직접 확인. 후속 완료문서 PR 이후의 최종 SHA는 원본 `Temp/EOD20261020/OriginalLocalSync_Proof.json`에서 재확인합니다.

Field09 승인5종+B파생Glow 제품14파일 정식 통합·원본 디스크 동기화. Scene Sprite참조6개 외 차이0, 공식Glow PNG/meta/GUID·Transform/Sorting/Collider·Main18/Save코드 보존. Chapter3 승인1포즈·Idle PNG4는 Unity밖 승인참조로만 보존, 게임연결0. 담당 문서 파일별 선별·의미병합/기존 main 이력 유지. EOD최소 정적13+격리Unity11=24PASS0FAIL, 기존39/14/34/과열UI10 재사용·전체회귀0, 원격CI등록0·충돌0.

904파일188,454,129bytes 검증백업, 보호기준6,558SHA 중 승인된기존파일8개갱신 외 비인가변경0·사용자904SHA불일치0. tracked dirty308/untracked548/staged0 및 status바이트 그대로. Save/UserData/개인Settings·원본Editor Bootstrap/PlayOFF/비포커스 보호. reset-hard/clean/stash/force/main직접Push0. 원본 자연Import·손플레이 NOT_VERIFIED, Search/URP OPEN.

**내일:** 서리들쥐 전투Fixv2(518d210a)는 별도보관·QA/대표미술승인대기, 변경셀/Idle연결만 검수. 미승인F1/마을환경·BGM미완료루프·TTS/SFX/영상·Field09실패자산 제품제외 유지. BGM루프·권리확인, 잔여Field09/Chapter3정식구현은 별도승인.

[완료 보고](EOD_ApprovedIntegration_LocalSync_20261020.md) · [동기화 증거](Evidence/EOD20261020/OriginalLocalSync_Proof.json) · [보호 증거](Evidence/EOD20261020/UserOriginalProtectionProof.json)

## HISTORY — 아래는 EOD 실행 전 준비 기록

## 2026-10-20 EOD 승인분 통합 — 최소 QA 완료 / PR·원본 동기화 진행 중

Field09 승인 환경5종+B파생Glow 제품14파일만 선별: PNG6 신규/meta/폴더meta +Scene Sprite참조6개. 공식Glow PNG/meta/GUID 및 Transform/Sorting/Collider/Quest/Save코드 불변. 기존39PASS·Idle14PASS 재사용, EOD 정적13+격리Unity11=24PASS0FAIL. Chapter3 승인1포즈·Idle4PNG는 Unity밖 `아트/몬스터/Chapter3/ApprovedReference`에 참조보존·게임연결0. 담당 문서는 파일별 선별/의미병합, 기존 main 고유 이력 보존.

사용자 변경904파일188,454,129bytes 백업/보호기준6,558SHA 기록. Combat Fix v2 ZIP518d210a는 CRC/SHA 확인 후 별도Temp아카이브, QA/미술승인대기. F1/마을미승인환경·BGM미완료루프·TTS/SFX/영상·Field09실패자산 제품제외. 내일 우선: 전투Fixv2 변경셀·Idle전환 검수→대표 미술승인; BGM루프/권리확인. 원본Editor 조작0, 자연Import/손플레이 미검증. 브랜치 `codex/eod-integration-20261020`, 기준main12c69c72. 아직 원격병합/원본Sync 완료 선언 아님.

[EOD 실행 보고](EOD_ApprovedIntegration_LocalSync_20261020.md) · [선별/검증 근거](Evidence/EOD20261020/IntegrationDeltaQA.json)

## 2026-10-19 EOD 승인 통합 완료 / 원본 SYNC_COMPLETE

승인 기능 [PR #27](https://github.com/HappyPaul812/Project-Limitless/pull/27) main 병합 **`f7a82907221e322a29b0b4cab24f7a4b4a0d197a`**, 통합 head `57f438ab3c72fabf91c127ff52398b3b9e17997d`. 이 완료 증거 작성 시 GitHub main=origin/main=원본 local main, index=HEAD 실제 확인 **SYNC_COMPLETE**. 후속 문서 전용 PR 병합의 최종 SHA는 원본 `Temp/EOD20261019/OriginalLocalSync_Proof.json`을 최종 재검증하며, 아래 SHA는 기능 통합 확정점입니다.

Main09 승인27·재회순서/Main10 승인18·조사NPC 우선권/Field06 v1.1 B42·Ground49·안전Spawn/Field07 v1 B42·투명Ash7 공식 통합. 게임168파일 원본 승인본과 동일하여 재기록0, 기존 Main18·Field05·Main09 UX 중복0. 정적8+실제Unity10=18PASS0FAIL, 기존 대규모 QA 반복0. 기획/리뉴얼/TTS 문서75파일 선택·의미 병합 및 충돌문서8개의 main/담당 고유 문단 보존 검사 PASS.

1,059파일 SHA백업(203,125,963bytes)·39worktree 기록·보호4,339SHA/백업1,059SHA 불일치0. 추가 ZIP4·고유커밋2 patch 아카이브 보존. 최종 사용자 tracked dirty308/untracked548/staged0 유지. UserData·Save·Settings 보호, 원본Editor/Play/Refresh/포커스 조작0. 기존 로컬 문서2커밋 main 조상 유지.

Field09 시각FAIL PNG/Scene/ZIP, 미승인 독사·늑대/TTS, Chapter3 구현은 제외. 원본 Editor 자연Import·손플레이/Main15·16 전주행 NOT_VERIFIED, Search/URP OPEN. 다음: 대표 평소 자연Import·승인 화면 확인 → Field09 수정 납품/미승인 TTS 청취 승인 대기 → Main22 기획·ID·아트 게이트. 완료 기능 재구현/전체 QA 반복 금지. [마감 보고](../../EOD_Integration_LocalSync_Closeout_20261019.md)

## HISTORY — 이전 완료·승인 단계 기록

## 2026-10-19 로컬 main 문서5 충돌 복구 완료 및 다음 단계

문서5 의미 병합 PR #25 기능 `bc0ffac095746a499a31792aea2bef0a7e39691b` → main `30954827ebbe9d2f2fd4e6bca22abd2f50e6acc7` 완료. 격리 리허설 후 **원본 HEAD/index/작업파일 SYNC_COMPLETE** 확인. dirty·미추적·UserData·Settings 1,430파일 백업, 문서5개 의미 병합 외1,425파일 SHA 불일치0. 기존 문서 커밋2개 이력 유지. 남은 tracked dirty308/untracked548은 사용자 변경으로 보존하며 staged0입니다. 최신 main은 후속 보고 PR 이후 최종 검증 SHA를 따릅니다.

Main09/Field05/Main18 재구현·대규모 재QA0, 원본 Editor/Play/Refresh/포커스0. 자연 Import와 실제 Main09/Field05 화면은 바울 대표의 평소 사용 시 확인 사항입니다. Search/URP 및 로컬 Unity 버전 차이 기존 OPEN 유지.

Main20 세린5/Main21 17 총22음성 Unity미등록. 기존 로컬 후보17개 PCM/SHA/원문 PASS이나 음색 오류 보고9개·나머지8개 청취 승인 미확인. 후보 없는5개(태온1/세린1/Voice미정 하렌3) 별도. API/생성/Unity음성변경0. [정확한22개 원문 CSV](../../Main20_Main21_TTS_Missing_ReadOnly_20261019.csv)와 [복구·보호·다음 실행 보고](../../LocalMain_Doc5_Recovery_Closeout_20261019.md) 참고. Main22는 기획 승인·정식ID·북부 아트가 아직 필요하며 기존 북문 동선/Save 연결 읽기 감사부터 진행합니다.

## 2026-10-19 문서5 의미 병합 및 보호 경계

로컬 작업복사·local main 커밋·공통 조상·최신 원격 main을 대조했습니다. 최신 Main09(PR21/24)·Field05(PR22/23)·Main18(PR18/19)의 병합·원본 디스크 반영은 완료 상태로 유지합니다. 과거 기록의 승인 대기/미구현은 과거 당시로 구분합니다. 기획·리뉴얼 담당의 원격 전용 기록도 삭제하지 않았습니다.

충돌 원본 네 버전과 SHA256·줄바꿈·시각은 로컬 `Temp/Doc5Recovery20261019/Versions`, 전체 dirty·미추적·UserData·Settings 백업은 `Backup`/`BackupManifest.json`에 보존했습니다. 문서 정합과 원본 Git 동기화는 별개이며, HEAD/index/작업파일 동기화는 격리 리허설 후 실제 확인 결과로만 선언합니다. 원본 Editor 자연 Import·손플레이는 미검증이며 기존 Search/URP 환경 OPEN을 유지합니다.

## 2026-10-19 Main09 최종 승인·main 병합·원본 디스크 적용 완료

- 바울 대표의 명시적 승인 후 [PR21](https://github.com/HappyPaul812/Project-Limitless/pull/21) **main병합 `f3196b6b4fc4dfaf8a901a6a29ea07254ae70325`**, 승인PR head `ed8efda34902f374a92f78e98cb5dd6d5af1b8fd`, 기능 `932c3573a0ad4a4d94c0b92c4d0bb932662ae419`. 기존427PASS/0FAIL을 재사용했습니다.
- 원본 Main09 기능9파일(기존7+신규helper/meta2) 적용 완료. 적용 직전 기존파일과 기준main 일치/충돌0 확인·백업, 적용후 검증브랜치 SHA9/9일치. 기존Quest ID/대사/폴합류/보상/위치/Collider/Save/승인음성 보호. [원본적용증거](Evidence/Main09Clarity20261019/OfficialApplyResult.json).
- 원본Editor/Play/포커스 조작0. 원본 Editor 자연Import·Runtime은 미검증, 기존 Search/URP·Editor버전차이·물리OS입력은 환경PARTIAL. 전체6단계·입력·SaveContinue의 실제 격리Unity427PASS와 구분합니다.
- 승인Field05 B42도 원본디스크 적용·PR22 main병합 b9590938 완료 상태 유지. Main18 PR18/19 재작업0. UserData23/Main08·Main18 WAV/meta/Catalog 변경0, 승인범위 외 원본 변경0.
- 원본 local main은 ff022e63 유지. 문서5개 내용충돌로 LOCAL_SYNC_PARTIAL이며 reset/clean/stash/forcePush 없이 사용자편집과 기존문서2커밋을 보존했습니다. 다음: 원본 Editor가 자연 Import한 후 정상 플레이 확인, 사용자문서5개 의미병합 후 로컬main동기화. Main09 승인 대기는 해제됐습니다.

## 2026-10-19 Main09 퀘스트 안내 수정 — 427 PASS / 원본 미적용

- `codex/main09-quest-clarity-20261019`, 기능 `932c3573a0ad4a4d94c0b92c4d0bb932662ae419`. 6단계 행동/조작·바닥 링·화살표·플레이어XY거리·폴 재대화·로그/토스트 동기화. 여관NPC와 조사 입력 충돌을 현재 Main09/1.6반경 한정 우선처리로 해결했습니다.
- 실제 키보드도보/게임패드휠체어 전체6단계348 + 최종UI/추적/전투Scene회귀79 PASS/0FAIL. 16:9/4:3/21:9 화면, 폴합류/보상/SaveContinue PASS. 대사·Scene·WAV·Collider 불변, 원본4844SHA/UserData23보호.
- 기존Search/URP·Editor버전·물리OS입력은환경PARTIAL. [실제 QA/화면](Main09_Quest_Clarity_Unity_QA_20261019.md). main병합·원본적용0, 전용PR검토대기. 다음: 승인된Field05 B42는 기존결과 재사용통합(추가미술승인 불필요), local main문서충돌5개보존.

## 2026-10-19 재개 작업 완료 — Main09 PR21 / 승인 Field05 PR22 병합·원본 적용

- 우선순위1 Main09: 실제 도보/키보드·휠체어/게임패드 6단계348 + 최종UI/추적/Scene회귀79 = **427PASS/0FAIL**. 기능 `932c3573a0ad4a4d94c0b92c4d0bb932662ae419`, QA commit `27983bb3d5652dfd6fec0b837ad4a63e98fc5ed5`. [PR21](https://github.com/HappyPaul812/Project-Limitless/pull/21) Push/검토대기. **Main09 main병합·원본디스크적용0**, UX개선화면 검토후 다음 반영 대상. 기존여관과조사입력충돌도 현재Main09/1.6반경한정 해결. 대사/좌표/Collider/승인음성 보존.
- Field05: 대표 최종승인 범위 PNG14/Road42/환경375 정식 반영. [PR22](https://github.com/HappyPaul812/Project-Limitless/pull/22) **main병합 `b95909387a7b6f597417041bfcd72f2b866d4e47`**, PR head `81b1c82df4fb0203d2958fc90804fc4956fb3cc5`. 원본디스크Scene1+신규32경로 일치/SceneGUID보호. 기존416+84검증재사용+실제Continue/Quest/Meta66PASS/0FAIL. 새Field05플레이환경/빌드/화면/텔레포트테스트0. [정식QA](Field05_B42_Official_Apply_QA_20261019.md).
- 원본4844SHA 중 승인Field05Scene1만변경, UserData23/Main08·Main18음성/Arbel/Field04/기존meta보호. Main18 PR18/19 재작업0. 원본Editor/Play/포커스조작0; 원본 자연Import/Runtime·물리OS입력은미검증, 기존Search/URP·Editor버전은환경PARTIAL.
- local main `ff022e63dfe364f559fcc29819d27b123ad31127` 보존. tracked dirty320/미추적1053, 원격겹침517중512동등/문서5내용충돌로 **LOCAL_SYNC_PARTIAL**. 기존로컬문서2커밋 ff022e63/3a37aa2e의 main조상이력 보존확인. reset/clean/stash/forcePush0. [최종보호](Evidence/Field05Official20261019/FinalLocalProtection.json).
- 다음 재개: Main09 PR21을 최신 main과 충돌없는 상태로 유지하고 대표 UX확인 후 main·원본 반영. 사용자 원본문서5개는 양쪽내용 의미병합 검토 후 로컬main동기화. 기존검증/아트/음성을 재제작하거나 수천건 QA를 반복하지 않습니다.

## 2026-10-19 Field05 B42 원본 디스크 정식 적용 — 66 추가 PASS / Push·PR 준비

- 바울 대표 최종 미술 승인에 따라 기존PNG14/환경375/Road42 후보를 원본 Scene+신규32경로로 반영했습니다. 기존Collider21·비도로Transform·SceneGUID·UserData23·승인음성 보호. 원본Editor조작0/자연ImportRuntime미검증.
- 기존416+84 PASS 재사용 + 실제SaveContinue/Scene/Quest/Meta 연결66 PASS/0FAIL. 새화면/별도플레이환경/빌드/텔레포트테스트0. [정식적용QA](Field05_B42_Official_Apply_QA_20261019.md).
- Main09 PR21은 실제427PASS/0FAIL로 Push·검토대기, main병합/원본적용0. 다음: 승인된Field05전용PR병합 후 정확SHA인계, 원본문서5충돌은보존해 local sync PARTIAL 유지.

## 2026-10-19 Field05 B42 최종 미술 승인 — 정식 통합 준비

- 바울 대표의 최신 재개 지시로 최종 미술 승인 완료. 기존 기능 ebb363567b3932cce8af12e315dda03362c8fd19/최종후보 f8afb80ce744fd1eef78ce1c5c7eb1d07187ebee와 416+84 PASS를 재사용합니다. 추가 비교 화면/플레이 검수 빌드/강제 Field05 입장 도구는 만들지 않습니다.
- 원본 Scene은 HEAD 대비 미커밋 변경 없음, 기존 Collider21개와 비도로 Transform 동일, 새 Asset32경로 충돌0을 확인했습니다. 승인PNG14/도로42/환경375만 정식 적용 대상으로 준비합니다.
- Main09는 먼저 완료: 기능932c3573a0ad4a4d94c0b92c4d0bb932662ae419, 최종27983bb3d5652dfd6fec0b837ad4a63e98fc5ed5, PR21 Push/검토 대기, 실제427 PASS/0FAIL. Main09 main병합/원본적용0. Main18 재작업0.
- 다음: Field05 Meta/GUID/Quest/Save 변경분 회귀→원본 디스크 안전적용→전용 Push/PR/승인범위 main 병합. 사용자dirty문서5개와Save/Editor/포커스보호 유지.

## 2026-10-19 Field05 v2 Road4 / B42 최종 후보 — 기술 PASS / 미술 승인 대기

- 전용 `codex/field05-eastwest-road42-v2-20261019`, 기능 `ebb363567b3932cce8af12e315dda03362c8fd19`. 새 v2 SHA/CRC/PNG4 Manifest·Ground3 Props7 원본SHA PASS. 환경375=315/42/18, PNG14, Road12추가, 선택7/새Portal0.
- 실제 최종 Unity 기능416 PASS/0 FAIL, 저장Scene재로드84 PASS/0 FAIL, 16:9·4:3·21:9 3안 비교63장. 기존 Search/URP/Editor버전차이만 환경 PARTIAL. [검수자료](Field05_Road42_v2_Final_Unity_QA_20261019.md).
- **원본 Field05 정식 적용·Push·PR·main 병합0. 새 v2 화면 미술 승인 필요.** 기존 Collider/좌표/Spawn/Save/Arbel·Field04 보호. 다음: 대표 갤러리 승인 후 통합.

## 2026-10-19 통합 마감 — Main18 원격 병합·원본 디스크 적용 / Field05 최종 미술 승인 대기

- Main18 승인10 Push→PR18→main 병합 `6f988dad77316347be69ac3b0979237771c09ed9`, 기능 `94a68cee54be397b16d8bacd4c3f930bad2eece3`. 기존1370 PASS/0FAIL·22/22 재사용 + 실제재생/SaveContinue delta394 PASS/0FAIL. 정상25/Main17세린11/PCM35/기존Catalog280 보호. 원본Unity WAV10/meta10/Catalog만 정식반영, 원본Editor미조작/Import Runtime미검증.
- Field05 새 v2 Road4 B42, PNG14·환경375·기능416+저장Scene84 PASS/0FAIL. 세화면비63장 비교, 최종미술승인PENDING. `codex/field05-eastwest-road42-v2-20261019` 최종 `92746215df23e2fe57e3393a563c839dca66a29a`. Field05 원본적용/Push/PR/main병합0.
- 원본dirty319+미추적1001,1368고유파일백업·UserData23 보호. 겹침464/문서충돌5로 local main ff022e63 보존·동기화PARTIAL. Search/URP/Editor버전차이·기존meta중복은환경OPEN. [실행결과·재개점](Main18_Field05_Integrated_Closeout_QA_20261019.md).
- 다음: 대표 Field05 최종화면 승인, 원본문서5개 의미병합 후 로컬main동기화. 주간재설정권 사용은 실행하지 않았습니다.

## 2026-10-14 최종 마감 — 음성 PR14 병합 완료 / 로컬 main 보존 중단

- 바울대표전후보청취승인·오늘병합지시로 007~011 정식연결 완료. 음성구현 `2ff11baa878156f9b7c537d9580e70cd1f5568e4`, [PR14](https://github.com/HappyPaul812/Project-Limitless/pull/14) merge `d05a325b05f2eab79ee18c5ca65e9ee523a5e5ab`. PR12Arbel/PR13기획 중복적용0·TTSAPI0·C3DRAFT구현0.
- 최신통합본 음성250+Arbel1729+Main21펫106+기존전투/Field04회귀232+Scene26=2343PASS/FAIL0. 승인8/미엘/Player무음·Quest/Save보호, 본문/호흡PCM동일·clipping0. 최종WAV007 3.267208/008 4.495667/009 3.017417/010 4.130042/011 2.550625초. 원본디스크 Catalog5GUID만변경+새11파일·기존WAV/meta불변.
- 전체PARTIAL: 기존URP/Search환경OPEN·원본liveImport/실물입력/스피커미검증·local同期STOP. 원격/추적main은음성병합시점 `d05a325b05f2eab79ee18c5ca65e9ee523a5e5ab`, localmain `ff022e63dfe364f559fcc29819d27b123ad31127`. 로컬전용문서2커밋으로ff-only불가를격리복제에서확인, 강제이동/reset/clean/stash/forcePush0. SYNC_COMPLETE아님. 미커밋1274경로/5851SHA와Save백업보존.
- 원본EditorBootstrap·PlayOFF·PausedOFF·Focusedfalse·Compilingfalse전후유지,강제Refresh0. [최종QA](../../EndOfDay_Integration_Voice_LocalSync_QA_20261014.md). 다음:2로컬문서커밋+기존dirty의보존/통합을별도로검토후로컬동기화재개. 음성재승인·재제작요청불필요.

## 2026-10-14 마감 음성007~011 정식 연결·통합 QA

- 바울대표전후보청취승인에따라5WAV복구/앞저음량여백최소축소·Catalog5GUID교정. 신규TTS0·승인005006+기존6/미엘/Player/QuestSave보호. 최종007 3.267208·008 4.495667·009 3.017417·010 4.130042·011 2.550625초.
- 최신main b5174f2f(PR12Arbel/PR13기획)기준 음성250+Arbel1729+Main21펫106+전투회귀232+Scene26=2343PASS/FAIL0. 기존환경/원본liveImport미검증은PARTIAL. Arbel중복적용0·C3DRAFT구현0.
- 음성전용codex/main08-taeon007011-final-20261014, 구현2ff11baa878156f9b7c537d9580e70cd1f5568e4. [마감QA](../../EndOfDay_Integration_Voice_LocalSync_QA_20261014.md). QA통과후이브랜치PR병합예정이며실제병합SHA는최종인계에추가기록합니다.
- localmainff022e63 대 originmain 이력분기(로컬문서2커밋). 격리ff-only실패로원본main동기화STOP/PARTIAL,강제이동0·1274dirty백업·Save보호. 승인Catalog1만디스크수정+새11파일·원본HEAD/Editor제어0. 다음:로컬2커밋/dirty비파괴정리후동기화재개.

## 2026-10-14 Sprite 참조 복구 — 필수 게이트 PASS

- 새 codex/sprite-reference-recovery-20261014: 동일PNG11·Sprite123 등록, Arbel/Field05~08의1747 Sprite GUID만 복구. Scene 역치환 동일/ThirdParty·원본미커밋·5221SHA/HEAD/status 보호.
- 실제23 BuildScene 참조3654/MissingScript0/Runtime null0. Edit null2는 VillageNpc 자동할당 정상. 배경5 RenderTexture 확인, 로컬 rect/pivot/PPU123 PASS.
- 이전854범위 새855 PASS(waypoint 추가계측1)/FAIL0, 서부85·의뢰589·Quest 기반/침묵 ALL PASS, 기존정적83·확대322 PASS. Field04 42/375/PNG52·Main08 승인6·B/C/15효과/Save/Chapter3 불변.
- 종합PARTIAL: 기존Search·정본6000.5.7f1·손플레이/가독성 미검증과 기존Spawn/북쪽틈/음성005~006/장착영속 미구현 유지. 원본Editor·포커스/Save/TTS/BGM 조작0.
- PR10 Ready/정상 main merge 조건 충족. 실제 원격 Push/통합HEAD/mainSHA·병합 여부는 PR10 최신 결과와 완료 보고를 확인하며 이전BLOCKED 기록은 역사로 보존합니다. 다음: 남은 기존 결함을 별도 작업으로 처리.
- 마지막 관련 복구 commit `af1c9cfddbf8e431a32d78d5a643d91a7a099da0` Push 완료. EOD merge `359b0622376c24d78caa025ec33df5fef71ccaa7`·충돌0/동일Unity트리/통합23참조·Runtime·정적83 재PASS. 최종 통합 문서 commit·원격 병합 SHA는 PR10 최신 결과 참조. [상세 QA](../../Sprite_Reference_Recovery_Final_QA_20261014.md).

## 2026-10-13 Main08 승인6개 정식 음성 연결·원본 디스크 반영

## 2026-10-13 EOD 최종 합본 — BLOCKED / MAIN 미병합

PR8/9충돌해결·Field04공식42/375/PNG52·Main08승인6·B/C·최신기획·담당문서 통합. 새합본 주요기능854PASS/FAIL0, 별도전체Scene참조FAIL: 기존main에서도Arbel/Field05~08환경Sprite누락 재현, 실제Arbel배경누락 확인. Field04362→0, 신규참조회귀증거0이나 전체Sprite0게이트미충족으로PR10Draft유지/main병합중단. 원본5199파일SHA/HEAD/dirty불변. 상세 `EOD_Integration_Final_QA_20261013.md`. 다음: 동일바이트Project등록·SpriteImporter복구안 검토 후 전체참조QA 재실행. 스킬영속Save·Chapter3상점·Main18추가TTS 미구현/HOLD보호. 최종브랜치SHA는PR10head·완료보고 참조.


- codex/main08-approved-voice-apply-20261013:바울대표직접청취승인6WAV를Main08/ApprovedRecovery20261013에새GUID등록/Catalog6참조교정. 원음WAV/GUID/meta그대로·목적지/출처16백업·승인입력6보관. 원본프로젝트디스크에도새13파일+Catalog반영완료,원본mainHEAD/기존미커밋/Save/설정/Scene/BGM/TTS보호.
- 최종등록Runtime177PASS/FAIL0,승인ClipPCM/GUID/경로6/6·Trace01~03/초상화/Next/Mixer/Quest/실제BootstrapContinuePASS. 정본6000.5.7f1미검증/실제6000.6.5f1·기존Search로PARTIAL,조우억제대화fixture·실물입력미검증. 원본Editor강제Refresh/Play/포커스전환0,즉시메모리반영미검증.
- 승인밖태온005/006기존발화·합본문제2건과대화중일반조우사례OPEN;전체Main08음성내용PASS아님. 새TTS/API·Push·mainCommit/병합0. Main08_ApprovedVoice_Integration_QA_20261013.md 참조. 마지막Commit은 `Fix: 승인 Main08 음성을 원음 보존 등록으로 연결` 이력확인.

## 2026-10-13 Main08 기존 음성 복구 후보6개 납품 — 청취 승인 대기

- codex/main08-voice-recovery-candidates-20261013: 태온002←003/003←004·미엘001←002 재사용, 태온004 긴정본은기존005에서발견해복사. 미엘합본003의450ms저신호간격에서0샘플분리해002/003 후보작성. 후보6decode/정본ASR일치·복사4byte동일·분리2PCM합산원본동일 PASS.
- 후보폴더/6재생기HTML/ID별CSV·길이RMS/Peak/원본SHA/경계보고제공. 사용자청취6대기·재제작현재확정0, 부적합ID만후속분류. 원본WAV/GUID/meta/Catalog/Unity/Save/설정·TTS/API/화면/Push변경0. 새RuntimeQA실행0.
- Main08_ExistingVoice_Recovery_Candidates_20261013.md 참조. 마지막관련Commit은 `Docs: Main08 기존 WAV 복구와 청취 후보 납품` 이력확인. 승인전정식적용금지.

## 2026-10-13 Main08 Field02 음성 오류 — 원본 내용·분할 문제 후보 확인

- codex/main08-field02-voice-20261013: docs/planning 사용자QA와 로컬 제작/WAV/Catalog 대조. ID/GUID·화자·자막/Manifest와 원본PCM 동일, 태온002 1.03초/RMS-48.98dBFS·발화검출없음, 미엘001 Main05문장/002 앞문장/003 정본문장+앞문장 ASR검출. 원인층은제작원본 내용↔ID/분할경계, 원생성내부와인간청취승인은미검증.
- 실제격리6000.6.5f1 Trace01~03/Clip/Next/Mixer/Quest/Save·BootstrapContinue129PASS/FAIL0, 기존Search로PARTIAL/정본6000.5.7f1미검증. 기존다른ID 재사용·두문장분리 후보만문서인계, 새TTS/음성교체/Catalog·게임코드/원본변경0. 원본오류수정완료로보고하지말것.
- 다음: 바울 대표/TTS담당이정확한ID·원문별재사용/분리후보청취; 불가시에만재생성준비·승인후적용. Main08_Field02_Voice_Technical_QA_20261013.md 참조. 마지막관련Commit은 `Docs: Main08 음성 원본 내용과 재생 경계 감사` 이력확인. Push0.

## 2026-10-13 PR7 Importer 동일성 검증·로컬 반영

- 후보 d57ff66 → PR7 로컬 통합 codex/integrate-abc-planning-20261013 / 코드병합 2fbe9577b212ee46f4525a597c3bcd4d7833f055. 정상 로컬 설정 대비 2,016,000 픽셀 차이0·363 누락0·환경 Collider/Importer 동등·PNG7 바이트 동일. 새 Field04 228 PASS/FAIL0, 실제6000.6.5f1/정본6000.5.7f1 미검증·기존Search PARTIAL.
- 별도 로컬 codex/local-pr7-importer-20261013 / Temp/PR7Apply20261013/local-working 사용. 원본 main/337항목·822파일/4317SHA 보호. 자동Push/main병합0; 원격PR7 Draft 유지. 서쪽Spawn 겹침·북쪽틈 OPEN, v2 적용0·장착 영속 Save 미승인. [새 상세보고](../../PR7_Importer_Apply_Verification_20261013.md).

## 2026-10-13 PR7 기존 Sprite 등록 복구 후보 — 원본 미적용

- PR7 3aca기준별도codex/pr7-importer-fix-proposal-20261013. ThirdParty원본변경0, _Project/Art/Environment/LegacySpriteRegistration20261013에기존PNG7byte동일복사+새meta/GUID, Field04362참조GUID만교정. 역치환시기존Scene텍스트동일/Collider·좌표·Quest·Spawn·localID보존/v2미사용.
- 실제격리누락362→0/363, 기존Road30남북유지·Collider동일·수정후228회귀PASS, 기존Search예외로PARTIAL. 다른Scene전역이관/정본6000.5.7f1/손입력미검증. 기존서쪽Spawn/북쪽틈은미해결.
- PR7_Importer_Fix_Proposal_20261013.md와Evidence/PR7_Importer_Proposal_20261013의새등록manifest·실제Preview/회귀참조. 수정후보는main/PR7/사용자원본에반영0, 원본4,317SHA/822dirty/status/HEAD보호/Push0/TTS0.
- 마지막수정후보Commit `d9ea3be7f0a0ab6e361b679a71f6e1bb4c59e611`. 다음:별도수정PR검토·통합후새HEAD재QA, 현재원PR3aca는BLOCKED/Draft유지. 미술v2적용·Save스키마·신규상품을이교정과혼동하지않음.

## 2026-10-13 A — Field04 v2 실제 동서 도로 후보

- A 브랜치 codex/field04-v2-road-20261013 / Temp/ABC20261013/A / 검수SHA f6222caacea500dfd71e9d9537cb2f9eca9396e9. main0e04766/planning c0f1d7c 확인. 원본 main/822미커밋/4,318 SHA/status/HEAD 보존.
- Field04 원본 공식 Scene은 이 브랜치에서도 변경0. 격리 ReviewScenes/Field04_v2_Candidate30.unity와Candidate42.unity 및 실제RT Preview를 생성했습니다. Artifact `.unity.txt`는 검토자료이며 단독 실행자산이 아닙니다. 정확한30객체 fileID/old좌표 일치, 도로30 실제 이동, 연장12는 추가승인 후보입니다.
- 최종674+244 PASS/FAIL0. PPU128·나무5·RockNorth접지 보완/Quest·Battle·Save·147물리경로/Camera·실제Physics Exit 통과. 초기Importer누락 화면은 폐기하지 않고Temp에보존, 최종자료에서는 사용하지 않았습니다. 기존Search·서쪽Spawn·북쪽틈은OPEN, 사용자미술/손플레이/6000.5.7f1 미검증.
- 바울 대표가 v2 색감과30/42안 중 선택 승인할 때까지 원본아트영구적용0. 승인후최신main 새Codex브랜치에서PNG/meta등록과최소Scene참조/좌표변경·전체회귀 수행. B 스킬과 C 펫은별도worktree/브랜치로 유지; Push/main병합/TTS호출0.
- 정본 보고: ../../Field04_v2_Road_Candidate_QA_20261013.md. 후임은특히초기Preview가아닌최종PPU128 Preview/knownSearch원문/원본Spawn진단을확인해야합니다.

## 2026-10-13 작업 C — Chapter2 안전지역 펫 확대 / 격리 구현·QA PARTIAL

- 브랜치 `codex/safezone-services-20261013`, worktree `Temp/ABC20261013/C`, main기준0e04766/planning정본c0f1d7c. 원본main/사용자미커밋·Save·아트·TTS/BGM 무접촉, Push0.
- 정식거점3(StarterVillage/Arbel/Field03부분CatacombEntrance). Main21 기존완료로 신규4NPC 조건부설치, Arbel기존2명 중복0/불변. SameScene 타슬롯/Continue 재평가, generated만 제거. 신규그림0·기존Arbel역할Sprite재사용, 좌표는후보.
- 공용BeastService·100가격/발견/소유/장착/실패rollback·SaveVersion1보존. 사수0명도분양하던기존정본차이보완(사수보유목록검사/안내); 타직업+보유사수는허용.
- 보고서 `../../SafeZone_PetService_Rollout_Implementation_QA_20261013.md`; QA entry `SafeZonePetServiceAudit.RunBatch`는공유격리Client에서루트담당만순차실행. 최종Unity6000.6.5f1 Compile/신규Runtime오류0/실제UI/100구매·공유/완료·미완료BootstrapContinue/타슬롯/Save실패rollback/몸겹침/actualCollider BFS PASS106. 기존Editor Search예외1별도원문기록·전체PARTIAL. 전체Main21재플레이·원본6000.5.7f1・손플레이/이름표·NPC미술승인은미검증. 마지막구현Commit `0d151e3f0222dac992145e69717f7607158b8106`, 결과Evidence/SafeZonePetServices_20261013.
- C3공용확대는문서설계만: 최초구매접근과클리어전체판매망분리, 서비스중복방지·캐릭터별공유소유권/Save승인필요. 미정Quest/flag/스킬ID/상품/가격/NPC코드생성0. 기존B스킬슬롯구현과독립.

## 2026-10-13 B — 장착·전투 UI 구현 인계

- 구현 worktree Temp/ABC20261013/B / branch codex/skill-loadout-ui-20261013 / 구현·QA c4ed7c9fd2ed9b3fe5cf3ea1c57387ef51d5e816. 원격main0e04766·planningc0f1d7c기준. [상세보고·실제Preview](../../Skill_Loadout_UI_Implementation_QA_20261013.md).
- SkillLoadoutService는 현재시작3개만 실제공급, 캐릭터+직업별6칸/독립보관함·무료해제/교체·중복/타ID차단·전투중변경금지. Battle 시작에ID/칸위치스냅샷,2행×3열/빈칸버튼0/뒤로별도. World/Field/Dungeon/Arbel K·패드Menu/Start관리, 닫기월드포커스null. 후기동료해금명단·36행스크롤/Mask/focus검사포함.
- Root 격리6000.6.5f1 Batch에서72UI+154기존15스킬+28 C/B펫공존PASS, 기존BattleSilenceAudit 직접ALL PASS. 상세팝업은21:9재현 후 안내상단+8px만최소보정. 신규Runtime/Compile오류0, 기존Search예외1로PARTIAL. 정본6000.5.7f1/실물입력/장기가독성NOT_VERIFIED.
- Save버전1·5슬롯/schema 변경0. 장착은이번실행만유지, 새게임/Save로드/재시작때시작3개로초기화; 화면에제한명시. 신규가격/효과/Chapter3ID/실제구매/강화정의생성0. RegisterApprovedDefinition은내부미래연결계약이며 영속거래/직업/Modal검증이연결돼야구매가완성됨.
- Original4,318보호파일SHA·822dirtyfiles/status/HEAD불변, 원본Focus/Editor·Scene·PNG·기존meta·Save·TTS/BGM·ThirdParty보호, Push/mainmerge0. B근거는 Evidence/SkillLoadout_20261013, Helper는 `.cs.txt`이며원본실행금지. C+B공존helper는C overlay도필요.
- 다음: 대표님UI검토·Save소유권/직업/안정ID/버전1호환·저장실패롤백계약확정. 승인된브랜치Push/PR/main반영은사용자결정. 가짜Chapter3스킬/강화상품을만들거나예시를정식데이터로옮기지말것.

후임 시작 문구: “LIMITLESS Codex B 담당입니다. Skill_Loadout_UI_Implementation_QA_20261013·CURRENT_STATUS·이 인계를 읽고 codex 작업브랜치만 사용하세요. 기존15/Save/822 사용자 파일 보호, 6칸 장착은 세션임시이며 영속 저장과 신규 상품은 미확정입니다. 원본 main·Editor·Focus·아트·TTS·Push를 건드리지 마세요.”
## 2026-10-13 새 담당별 브랜치 규칙·통합 v2

- 사용자최신규칙: AI main직접커밋금지. 기획docs/planning, Codex codex/주제별브랜치+별도worktree. 이전HEAD:main명령사용금지, ../../Git_BranchOnly_Integration_20261013.md의작업브랜치Push안내우선. CodexPush0/main병합은사용자별도결정.
- 새 worktree Temp/GitIntegration20261013V2/main-integration / codex/integrate-main-20261013-v2. 이전통합961c90d에서분기후최신origin/main73c37bf보존merge71f71bb, 충돌0. 두SHA모두조상·Unity변경0. 기획docs/planning74c5a0b정책읽기만/다른브랜치변경0.
- 원본main ff022e6/337항목822파일·4,750SHA보호, 이전통합worktree961c90d도보존. reset/stash/amend/force/아트적용/TTS/원본Editor조작0. 기존Rockv2/Field04검수및승인대기상태유지.

새세션문구: “LIMITLESS Codex 담당입니다. Git_BranchOnly_Integration_20261013·이인계·CURRENT_STATUS를읽고codex작업브랜치만사용하세요. 원본main/미커밋/961c90d통합이력보호, Push/main직접작성/승인전자산적용금지입니다.”

## 2026-10-13 Git 통합·Rock B v2/Field04 검수 인계

- 최신검수는 ../../ArtInspection_20261013_Summary.md 및 Rock/Field04각보고서. 최종원격405ce12+로컬A/B 안전통합은 Temp/GitIntegration20261013/main-merge / codex/integrate-main-20261013. Push0, 사용자정확명령은총괄참조. 원본main ff022e6/337항목822파일 보호; 단순원본git push는분기상태임.
- Rockv2 픽셀/실제접지PASS·USER_ART_REVIEW_PENDING. Field04 CRC/PNG/tile27/실제Preview/기능QA완료·전체PARTIAL; 나무5참조Big/Medium역대응/RockNorth보완필요. Literal과Proposal362/363을구분. 둘다원본영구적용0/Optional0.
- 원본서쪽Spawn native겹침/북쪽비출구Collider틈·Search기존예외 유지. Main13/Continue2/왕복/3UI전투·35초/휠체어179/Camera12 PASS, 실패로그보존. 미술승인/손플레이/독립권리미검증. 새TTS호출/SkillDefinition/UI/Save코드추가0.
- 기존격리 Temp/Field01RenewalQA/Client 재사용·검수Batch종료. Temp/ArtInspection20261013 원시로그/소스, 각Results/Resume1/Resume2 분리, Evidence/ArtInspection_20261013 문서용정리본/실제GPU이미지. 원본Editor Bootstrap/Edit/비포커스무접촉, 보호4,750SHA불변.
- 검수SHA: Rock `975fb636a7a7a7527bc704c21019853301d83bac`, Field04 `dcb1f14524de6cfb295cbf5e5d6cfb83d24229a5`. 다음: 사용자미술검토/납품표와RockNorth보완; 승인없이정식적용/TTS/Push금지. 새담당은원격리뉴얼신입인계와현재QA를함께읽고중복제작하지말것.

새세션문구: “LIMITLESS Codex 담당입니다. ArtInspection_20261013_Summary·CURRENT_STATUS·이인계를 읽고 원본337/822·Save·TTS와통합worktree를 보호하세요. Rockv2미술승인대기/Field04대응보완, 원본영구적용0·TTS/Push0입니다.”

## 2026-10-12 B2 Push 확인 후 봉인문·Rock B 인계

- 원격47a65e2(B2완료) 확인 후 후속 시작. B1 봉인문은 기존 벽 안의0.75×0.625 상태 표식/기존 하강로 B2이동 안내로 해결. 구현 `6c78cdb833e873639b0a027d5cfdc7efb5a83b3f`, B1체크388/B2경계139 PASS. 전체 PARTIAL(Search·사람가독성 미검증).
- Rock B v2 지정경로/FDownloads/사용자Downloads 검색 미발견. ART_FIX_PENDING, v1 임의수정 금지. 공식375/376·기존RockNorth1 보호. 실제v2 납품 뒤에만 검증/교체.
- 사용자337/822·Save/meta/모든Scene/승인PNG/TTS/BGM 보호. 신규이미지/Scene변경0. 원본Editor포커스무접촉, Push미실행. 별도 봉인문/Rock B QA문서 참조.

B2 정식 구현 Commit: `2babb13c41bd1c2c75ad69bb93381dc3ed71547a`.

## 2026-10-12 B2 정식 통합·봉인실 표시 인계

- USER_ART_APPROVED. PNG31 정식 등록, B1 공통 렌더링 코드 불변 재사용. B2 앵커54 보존/Tiled18/원크기소품8/줄눈숨김28/표시26. B1 PNG·Scene·프로필·문 TECH_HOLD 보호.
- Main11 기존 단서7은 원래Sprite를 유지하며 처치 전 숨김/후 표시. 새 석판 그림은 목표10~11/아이템0만 표시하고 회수·완료 후 숨김. Inventory/Quest/장벽 조건은 기존 그대로.
- 일반4UI승리·35초/보스23공격 Phase2소환·도망금지/Main11완료/Continue·재입장/휠체어/실제RT/Importer31 PASS. 전체 PARTIAL(Search 기존예외·사람손플레이/장시간가독성/권리 미검증).
- 정본 Dungeon01_B2_Renewal_Official_Integration_QA_20261012.md. 사용자337/822·Save/GUID/TTS/BGM 보호. 원본 Editor31832 무접촉. 선택 Commit 후 사용자 직접 Push, Codex Push 금지. 다음 권장: 실제 화면 가독성 확인.

## 2026-10-12 Dungeon B1 정식 적용 인계 추가

- USER_ART_APPROVED. 공식 PNG25 등록, Scene 컴포넌트 1개 추가(14줄), 기존63 앵커/Collider 보존. 프로필 Tiled20/원크기소품8/중복세부숨김33/문2기존유지, 생성28 재사용. B2 적용 없음.
- Main11 B1/4조우 UI승리/35초 리스폰/왕복/Continue/휠체어 통과. 전체 PARTIAL: Search 기존예외와 신규문 TECH_HOLD. 신규닫힘·열림 문 그림은 연결하지 않았고 기존 해금 동작은 정상.
- 정본: Dungeon01_B1_Renewal_Official_Integration_QA_20261012.md 및 Evidence/Dungeon01B1Official_20261012. 정상 아트 적용을 반복하지 말 것. 다음 작업은 사용자 화면 확인·문 시각/Collider/상호작용 정합 설계.
- 사용자337항목(822파일), Save/meta/승인음성/B2 보호. Push 금지, 사용자 직접 Push. 정식 구현 Commit `c47806bee9213775b98fb1b7b17b67b3f57f1c54`; 상태 기록 Commit은 Git 이력 참조.

# Codex 최신 인계 — 2026-10-12

## 최신: Dungeon01 B2 검수 완료 / USER_ART_REVIEW_PENDING

[B2정본](../../Dungeon01_B2_Renewal_Art_v1_Inspection_20261012.md)이최신입니다. ZIP51/PNG44/실사용31, 기존Scene54공용Sprite1+Main11Runtime단서7, 격리후보26환경자식/7단서자식/석판그림1입니다. 영구적용0. B1/B2모두승인대기, Field03정식375/376·RockB보류보호.

실제B2조사/일반4정상전투·35초/왕복/보스24UI공격승리·Phase2소환2/장막/충격준비/도망금지·단서조사/석판1/정상귀환Main11완료PASS. PrimaryContinue2회와완료실제Save별도View복원, 지체의길8지점244waypoint/카메라4BoundsPASS. B1조사는기존증거/checkpoint서비스fixture만재사용했습니다.

중요: **기존단서7개가보스처치전에도enabled**. 물리/순차조건차단은PASS지만시각선노출위험은원본에남아있습니다. 이번엔QA에서만숨김/후표시를제안했으며게임수정금지범위를지켰습니다. 승인후최소통합에서별도기술검토필요합니다. Search기존예외유지/전체PARTIAL, 사람가독성·세부밸런스·독립권리증빙미검증입니다.

초기QA컴파일2코딩오류/3진단만수정후통과, 원시로그보존. 결과Dungeon01B2Inspection_20261012 및Dungeon01B2Views_20261012, 소스/로그Temp/Dungeon01B2Inspection20261012입니다. QA종료, 기존격리프로젝트재사용/원본Editor PID31832무접촉. 사용자337항목/822파일·원본게임3738SHA보존. 기준로컬/원격395dd5d일치. 검수 Commit `4e30cd89f2ed9b2a2cefa4dcfd96c37e2062a012`, Push0입니다. Collider의B1 Scope집계오류는B2실제25행/Box16개를별도수집해수정했으며게임PASS반복0입니다. **승인전B1/B2원본아트/Scene영구적용금지.**


## 최신 작업: Dungeon01 B1 검수 완료 / 승인 대기

USER_ART_REVIEW_PENDING, 원본영구적용0/전체QA_PARTIAL. [B1검수](../../Dungeon01_B1_Renewal_Art_v1_Inspection_20261012.md)와CURRENT_STATUS를최신으로따릅니다. ZIP44/PNG38/실사용25, 실제Scene63Renderer공용1Sprite입니다. 전역1:1교체는왜곡하므로정식통합은역할별반복/비율/문상태설계가필요합니다. 격리후보30자식Renderer만사용했으며원본Transform/Collider/Sprite형상불변입니다.

Main11 B1목표0~4/실제B2하강index5, Continue3회, 4일반조우정상UI승리·35초리스폰·중복0, B2귀환Spawn(0,6.8), 실제물리남쪽출구Field03(.55,-2.65)/재진입, 휠체어6지점233waypoint+출구PASS. 옛Builder출구이름/Scene교체후Rigidbody계측오류는QA만고치고원시실패보존,통과검사반복0입니다. B2보스전체/봉인실/석판/Main11완료·사람가독성/OS직접조작·독립권리증빙은NOT_VERIFIED입니다. Search기존예외유지.

원본3738파일모두SHA불변, 사용자337/822보호. 검수전원격/로컬7f30f60일치(사용자Field03Push확인), 원본Editor/Save/meta/승인아트/TTS/BGM무접촉. 격리기존프로젝트재사용, 결과Dungeon01B1Inspection_20261012, 소스/로그Temp/Dungeon01B1Inspection20261012, 자료Evidence/Dungeon01B1Renewal_v1_20261012입니다. QA프로세스종료. 현재 검수 Commit은 `dce84d58cdf361dba145e8f1c22b66cabf639ff8`입니다. **아트승인전B1Scene영구적용금지**. Field03RockB보류는계속유지합니다.


LIMITLESS Unity 구현·검증 담당. AGENTS/팀운영 START_HERE·COMMON_RULES·담당 CODEX/PROJECT_CONTEXT/CURRENT_STATUS 순서로 확인합니다. 사용자에게한국어존댓말, Push/TTS API/원본Editor조작 금지입니다.

## 현재 완료

- Field01복구, 하렌/세린 공식아트, Main18정상12·Main19정상13 TTS, Chapter2 QA/Chapter3설계, Field02정식12종/373참조는 완료상태를보호합니다. Main18오류10개무음·REGEN_REQUIRED/Schedar 유지, 재작업금지.
- **Field03 USER_ART_APPROVED / 정식11종·375/376참조 / QA_PARTIAL**. Scene362+Theme13 신규Sprite, Rock B1종/1참조는 기존Sprite로보류. [정본QA](../../Field03_Renewal_Official_Integration_QA_20261012.md), [최초검수](../../Field03_Renewal_Art_v1_Inspection_20261012.md).
- PNG원본byte보존, 신규26파일/GUID15개충돌0, 기존좌표·Collider·Sorting·Tint/CameraColor불변. 보조8종추가0/게임C#변경0. Rock B상단14분리픽셀·낮은접지는 [아트수정요청](../../Evidence/Field03Official_20261012/RockB_Art_Correction_Request.md) 참고. 원본PNG임의수정금지.

## 검증과 남은 범위

- Main08전체(처음Field02조사4포함), Main09폴해금, Main10단계별입구/완료, Save→BootstrapContinue3회 PASS. Main11은 **Dungeon01 B1진입에서시작**하며Field03에서는Available/활성Main없음이정상입니다.
- 첫QA가Main11 Field03자동시작을가정해NullReference실패했습니다. 게임문제가아닌QA가정오류로스크립트만수정, Main10완료Save에서재개했습니다. 최초168PASS, 재개316PASS/FAIL0. 모델capacity중단뒤남은프로세스이미종료, 통과검사재실행0.
- 6일반몬스터실제Attack/HitArea/UI승리/복귀, 현행**35초**리스폰(30초미생성), 최종6개중복0. 타이머설정변경금지. Field02/Dungeon01왕복·휠체어11지점112waypoint·Field01/02/마을회귀 PASS.
- Search.SearchDatabase/IndexationOnStartup 예외재현으로전체PARTIAL. 폴/파티안내인 이름·대화안내겹침은최초검수에도존재. 실제OS키/패드·장시간가독성/밸런스·Main11전체던전·외부소스권리는NOT_VERIFIED.

## 보호·재개 경로

원본 `F:/study/codex/Project-Limitless/Unity/Client`, 원본Unity PID31832 및그ImportWorker 조작금지. 사용자337status항목/822파일(CURRENT_STATUS 사용자8줄포함) 보호, 기존게임3712중Field03Scene1만허용변경/다른3711SHA불변입니다. Save/설정/meta/승인아트/TTS/BGM/ThirdParty 원본보호.

격리프로젝트 `Temp/Field01RenewalQA/Client` 재사용. 결과 `Field03Official_20261012`, 원시로그/소스/보호스냅샷 `Temp/Field03Official20261012`. 별도AuditSaveDirectory/Settings/음소거. 통합QA프로세스는종료했습니다. 증거정본 `문서/00_프로젝트/Evidence/Field03Official_20261012`.

기존통합worktree `Temp/GitIntegration20261012/main-merge`55ac89f 유지, 무단삭제금지. 시작로컬/원격main9c7457b일치. 원본CURRENT_STATUS8줄은선별index에서제외하며파일을되돌리지않습니다.

## 다음 순서

1. 정상11종은정식완료입니다. Rock B수정본만새버전으로납품받아확대검수후1참조를연결합니다. 보조8종은별도기획없으면미배치유지합니다.
2. 사용자직접플레이로가독성/조작감확인, Search예외는별도범위에서조사합니다.
3. 이번Commit후Push는사용자가직접수행합니다. 자동Push금지입니다.

새세션시작문구: “LIMITLESS Codex 담당입니다. AGENTS·CURRENT_STATUS·이인계를확인하고 Field03 정상11종은보존한채 Rock B후속범위만진행해주세요. 원본Editor·337사용자변경·Save보호, Push/TTS API금지.”

정식 통합 Commit은 `dcd913e4dcb870528206623120a0d2d1af0059d5`입니다. 후속 문서 Commit은 이 SHA를 CURRENT_STATUS/QA에 기록합니다.


## 2026-10-13 Field04 v2 42도로 정식 적용

승인된 Road30 재배치+연장12 및 Grass315/Props18 적용. 새 격리 QA 253 PASS, 기존 Search/서쪽Spawn/북쪽경계/정본Unity버전 차이는 PARTIAL·OPEN. 원본 디스크 APPLIED. Main08/사용자Save/TTS/BGM 보호. 브랜치 codex/field04-v2-official-42-20261013; 관련 SHA는 해당 브랜치 최종 커밋 및 완료 보고 참조. 상세 `Field04_v2_Official42_Apply_QA_20261013.md`. 사용자 직접 플레이 확인 권장.

## 로컬 고유 이력 보존 — 과거 당시 기록

다음은 2026-10-19 의미 병합에서 보존한 로컬 작업복사·로컬 커밋의 고유 기록입니다. 과거의 미완료·승인 대기·재개 지시는 당시 상태이며, 현재 상태는 문서 상단과 최신 main 완료 기록을 따릅니다. 기존 수치·승인·OPEN·근거 경로는 변경하지 않았습니다.

### [과거 당시] 2026-10-14 최종 마감 확인 — 음성 완료 / 로컬 Git 동기화 PARTIAL (local_working)

- 태온007~011 승인후보를 정식 연결하고 앞 저음량 공백만 보정했습니다. 음성 PR14 merge d05a325b05f2eab79ee18c5ca65e9ee523a5e5ab, 마감문서 PR15 merge 9110bc7624e08ddbdd93b96510e8fafc5fbe341c. 신규TTS0·기존WAV/meta덮어쓰기0·승인005006/기존6·미엘/Player/Quest/Save 보호.
- 격리 최신통합본 음성250+Arbel1729+Main21펫106+전투/Field04회귀232+23Scene26=2343 PASS/0 FAIL. 기존 URP/Search·원본liveImport/물리스피커/OS입력 미검증은 별도 PARTIAL. Arbel/기획중복적용0·Chapter3DRAFT구현0.
- 최종 원격main/originmain 9110bc7624e08ddbdd93b96510e8fafc5fbe341c; 사용자 local main/HEAD ff022e63dfe364f559fcc29819d27b123ad31127. 로컬 문서전용2커밋 이력분기로 격리ff-only실패, 원본main이동/reset/clean/stash/forcePush0. **완전동기화 미완료**. 1274 dirty·미추적 백업/5851 SHA/UserData backup: Temp/EndOfDay20261014.
- 원본 EditorBootstrap·PlayOFF·PausedOFF·Focusedfalse·Compilingfalse 유지·강제Refresh0. [최종QA](../../EndOfDay_Integration_Voice_LocalSync_QA_20261014.md). 다음: 로컬전용3a37aa2e/ff022e63 문서커밋과dirty보존 통합방식 검토후동기화재개. 원본Play/포커스강제조작 금지, 음성재승인 요청불필요.

### [과거 당시] 2026-10-14 통합 작업 A/B 완료·재개 경계 (local_working)

- 기준main314f7fbb(PR11승인005006병합). A승인아트23 정식원본디스크반영·3953PASS/FAIL0·전체PARTIAL(기존환경/원본liveImport미검증), 전용codex/arbel-v3-official-apply-20261014 Push652867c988d015f0eb65ccb82591d2ef3a36551b 확인. [A QA](../../Arbel_v3_Official_Apply_QA_20261014.md)·[A 인계](../../Arbel_v3_Official_Apply_Handoff_20261014.md).
- B007~011 기존원본재사용후보5·PCM소스일치5/5·009010재결합원본동일·011최종2.800042초(선행1.079958초제거). 원본Field02/03실행97PASS/FAIL0,ASR일치3/5·007/008어휘청취확인필요. 전체PARTIAL/청취승인대기. [B QA](../../Main08_Taeon007011_Content_Recovery_Candidates_QA_20261014.md)·[B 인계](../../Main08_Taeon007011_Handoff_20261014.md)·[청취](../../Evidence/Main08Taeon00701120261014/index.html).
- B별도codex/main08-taeon007011-audit-20261014 Worktree는문서·후보uncommitted 상태. B정식Catalog연결NO/CommitPush0/TTSAPI0. 승인005006+기존6·정상미엘·Player무음·모든기존WAV/GUID/meta 보호. 007~011새후보연결을이미완료했다고주장금지.
- 원본4001중4000동일·승인ArbelScene1만변경+신규55, 추가TTS/Story392동일. rootmainHEADff022불변·원본Editor제어0·main직접CommitPush병합0. A/B파일같은Commit혼합0.
- 다음 우선작업: (1)원본Editor정상재개후A자연Import확인 (2)바울대표B개별청취승인기록,특히007폭이/포기·008조사한/조사하는 (3)승인된B ID만신규GUID/WAV폴더Catalog연결후Field02/03·승인8·미엘·Player·SaveContinue재QA와별도허가된브랜치CommitPush. 확정TTS_REGEN_REQUIRED0, 청취에서본문불일치확정시해당ID만재조사.
- 다른담당: 기획은B청취승인경계와Portal-나무/Teleport미구현OPEN유지, 아트담당신규PNG납품필요0. Main08대화중조우OPEN은QAfixture에서만억제했고수정하지않음.
- 후임은각QA/인계·CURRENT_STATUS·최신원격Gate를대조하고원본dirty를reset/clean하거나원본Play/focus를조작하지말것. 재개자료 Temp/ArbelMain08Integration20261014 및A/B인계문서.

### Issue #84 로컬 커밋·GitHub 보고 영수증
- 검증 코드·증거 커밋: `bde96a92e14c663e33ce7640f98f863d58a0d306` (`codex/owl-spider-unity-animation-delta-20261103`).
- 실제 댓글: [#84](https://github.com/HappyPaul812/Project-Limitless/issues/84#issuecomment-6597692700), [#74](https://github.com/HappyPaul812/Project-Limitless/issues/74#issuecomment-6597702364), [#60](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6597703337).
- GitHub에는 댓글만 보고했습니다. `LOCAL_COMMITTED / REMOTE_NOT_PUSHED / NOT_UNITY_APPLIED / USER_VISUAL_REVIEW_PENDING`.
- Owl 클립 기술 PASS, Spider 클립 기술 PASS / 실제 발 접지·미끄러짐 NOT_VERIFIED·WALK_CONTACT_HOLD. 최종 미술 승인은 별도입니다.
## 실제 커밋·GitHub 보고 영수증

- 승인·계약 조사 커밋: `ba8aa9749d37d8861a7cfa340a37fcf81ac9fa26` (문서·조사 근거14파일).
- 실제 댓글: [#63 통합](https://github.com/HappyPaul812/Project-Limitless/issues/63#issuecomment-6598409883), [#60 Spider](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6598417647), [#74 Owl](https://github.com/HappyPaul812/Project-Limitless/issues/74#issuecomment-6598418547), [#84 QA 승인](https://github.com/HappyPaul812/Project-Limitless/issues/84#issuecomment-6598419336).
- 브랜치 `codex/bee-target-rights-and-art-approval-20261103`. GitHub 댓글만 게시했고 로컬 문서·커밋은 원격에 없습니다. `LOCAL_COMMITTED / REMOTE_NOT_PUSHED / NOT_UNITY_APPLIED`.
- Owl/Spider `USER_ART_APPROVED`, Spider 실제 접지 `NOT_VERIFIED / WALK_CONTACT_HOLD`, Owl VFX HOLD. Bee `TARGET_CONTACT_HOLD / RIGHTS_HOLD / FULL_COMBAT_READY_NO`.
## 실제 기능 커밋·GitHub 보고 영수증

- 기능·승인 자산 연결·QA 커밋: `61dce08b271bd2e801205a711a8af7d54400c84f`.
- 실제 [Issue #85 구현·실증 보고](https://github.com/HappyPaul812/Project-Limitless/issues/85#issuecomment-6601090552) 게시 완료. GitHub에는 댓글만 있으며 코드·캡처·PNG 파일은 원격에 없습니다.
- 브랜치 `codex/main24-frost-owl-finish-20261103`. 신규 #85 81 PASS/FAIL0, 기존 #80/#83/#84/저장QA 재실행0.
- `F2_ENCOUNTER_PASS / OWL_BATTLE_RUNTIME_PASS / COLD_STATUS_HOLD / HIT_DEFEAT_GENERIC_FEEDBACK_ONLY / LOCAL_COMMITTED / REMOTE_NOT_PUSHED / NOT_ORIGINAL_APPLIED`.
- 원본 적용은 바울 대표 별도 승인 전 보류. 정식 ID·밸런스/보상·한기/대응수단·통합 범위 결정이 남습니다.

- 원본 Editor 최종 확인: Play 종료·Scene dirty=false 상태에서 Issue94 스크립트3개/프로필2개만 포커스 없이 ImportAsset한 뒤 컴파일 완료. Unity6000.6.5f1, DungeonFloorCollision 타입 로드=true, compiling=false, C# Console 오류0, external_changes_dirty=false. 원본 Play/Scene/선택/포커스 제어0. 이제 새 Play에서 사용자 수용 확인이 가능합니다.
