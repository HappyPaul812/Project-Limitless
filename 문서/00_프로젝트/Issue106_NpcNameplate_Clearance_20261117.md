# Issue #106 — NPC 이름표와 승인된 E 안내 간격 수정 (2026-11-17)

**LOCAL_ORIGINAL_WORKING_TREE_APPLIED / FINAL_DELTA_155_PASS_0_FAIL / ORIGINAL_COMPILE_PASS / CONSOLE_ERRORS_0 / USER_REALPLAY_PENDING / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

## 결과·사용자 추가 승인

[Issue106](https://github.com/HappyPaul812/Project-Limitless/issues/106), 현재 로컬 카탈로그와 프로젝트 문서·인계를 확인했습니다. 원본과 최근NPC102 제품은 **NameplateOffset Y1.02**였으며 OGA11/13 신규 매핑이 존재했습니다. 원격 main 파일로 덮어쓰지 않았습니다.

최종 이름표 Y는 **1.02→1.37(+0.35WU)**입니다. +0.15/+0.20 초기 시안으로는 실제 글자·그림자 bounds가 여전히 Sprite 상단과 겹쳤습니다. 픽셀/PPU/Transform을 바꾸지 않고 실제 bounds 기준 작은 간격을 확보하는 가장 가까운0.01 단위 값을 선택했습니다.

Y만 옮긴 초기 후보는 수치검사94PASS였으나 육안에서 `[E] 대화하기` 상자와 겹쳐 **HOLD/원본미적용** 처리했습니다. 당시94PASS를 최종PASS로 승격하지 않았습니다. Issue의 “E상호작용 안내 위치 유지”와 충돌을 보고한 뒤, 바울 대표가 **“E 안내 상향도 허용하고 검증 계속”**을 직접 선택했습니다. 이 추가 승인으로 두 번째 C# 수정만 허용 범위에 포함했습니다.

- 상시 이름표가 있는 공용 NPC E 안내: **Y1.48→1.87**.
- 그 NPC의 기존 퀘스트 표식이 활성화되면, **표식은 그대로 두고 E 안내만 실제 표식 상단+0.04+안내 반높이로 이동**합니다. 현재 크기에서는Y2.48입니다. 같은 대상에서 표식이 켜지거나 꺼져도 표시중인 안내 높이가 따라갑니다.
- 이름표가 숨겨진 일반 주민과 별도 연출 NPC의 E 안내: **기존Y1.48 유지**.
- 이름표/안내의 X·글자 크기·폰트·색상·이름·그림자 상대위치·sorting·활성조건·상자 크기·UI scale은 변경하지 않았습니다. 퀘스트 로직/표식 위치/상태조건도 변경하지 않았습니다.

## 실제 제품 변경2파일

1. `Unity/Client/Assets/_Project/Scripts/NPC/VillageNpcAppearanceCatalog.cs`: NameplateOffset Y한값+한국어 설명주석1줄.
2. `Unity/Client/Assets/_Project/Scripts/NPC/NpcInteractionPrompt.cs`: 실제 안내가 보이는 LateUpdate에서만 허용된 상향위치를 산정. NPC 생성Awake 이후 확정되는 이름표 정책을 기준으로 판단하며 기존 SetVisible/입력·대화 흐름은 유지.

제품 diff **26추가/1삭제**, 브랜치 **`codex/ch1-npc-nameplate-clearance-20261117`**, 제품 commit **`4f62cce85c05ccf1b7de5f3f5ef0aae3c9620ebf`**. 최근NPC102 매핑2줄과 기존 매핑 전체를 보존했습니다. PNG/meta/GUID/PPU/Pivot/Transform/Collider/Scene/Prefab/Quest/Save/UserData/Discord 수정0입니다. 원본 C#2개는 검증된 제품과 동일하며 기존 줄바꿈을 보존했습니다.

## 실측 bounds (NPC 발 기준 월드Y)

17개 공용 표시 이름표를 실제 시작마을8·Arbel5·Field03 카타콤4에서 측정했습니다. Sprite alpha>8 가시 상단은1.142857WU입니다.

| 항목 | 기존 | 최종 |
|---|---:|---:|
| NameplateOffset Y |1.02|1.37|
| 보통 길이 TextMesh 하단 |0.852422|1.202422|
| 그 그림자 하단 |0.834422|1.184422|
| 머리 상단↔글자/그림자 하단 간격 |-0.308435|+0.041565|
| 긴 이름 간격 |-0.286092|+0.063908|

+0.15 시안의 간격은-0.158435, +0.20은-0.108435여서 머리와 겹칩니다. 최종17개는 **0.041564~0.063908WU** 간격이며, 짧은 이름의 최종 글자 하단 표기는 같은 글꼴/해당 측정 프레임 기준입니다. 실제 포커스 화면에서 E 상자↔이름표 간격은 **0.062422~0.084766WU**, 활성 퀘스트 표식↔E상자는 **0.04WU**입니다. TextMesh 기하 bounds/그림자와 실제 Sprite alpha를 함께 비교했으며, 고정 캔버스512 높이를 가시높이로 오인하지 않았습니다.

## 최종 최소 Delta QA

Unity **6000.6.5f1**, 기존 `Temp/Art95_91Unity20261111/QA/Client`·Library 재사용. 새 프로젝트/대형QA 복제0. 숨김 Batch에서 **155PASS/0FAIL**; 전체 전투/Quest/Save 회귀 재실행0.

- 960/1280/1680×720, 시작마을 은행안내인·훈련장관리인·주민대표/Arbel파티안내인/카타콤감시인 **5표본×3해상도×전후2=30실제렌더링**. Before는 같은 격리 씬에서 기존 좌표1.02/1.48을 재현한 비교이며 합성원화가 아닙니다. After는 최종 컴파일된 제품 동작입니다.
- 17개 공용 이름표의 얼굴/모자/머리카락 비가림·TextMesh/그림자 간격 PASS. 두 지역 재사용NPC를 포함한 이름표끼리 겹침쌍0→0.
- 표본5의 이름표·E 상자 화면 내 bounds PASS, 육안으로 HUD/다른 문구 겹침 확인. 퀘스트 표식이 있는 주민대표도 이름표→기존표식→E안내 순서로 분리됨을 세 해상도에서 확인했습니다. 모든 가능한 플레이어 위치의 offscreen NPC 표시를 전수 검증한 것은 아닙니다.
- 기존 일반 주민 OGA07/11/13/19 등의 상시 이름표 숨김·일반 주민/비카탈로그NPC E1.48 보존 PASS. OGA11/13 승인매핑 보존 PASS.
- 도보·휠체어 실제 가상 E/패드A 대화 열기·Esc/패드B 닫기 PASS. 같은 상호작용 대상에서 표식 활성/비활성 전환 시 E높이2.48↔1.87 반응 PASS(합성QA 상태만).
- 신규 게임Runtime오류0. 격리의 기존 `UnityEditor.Search` 시작 색인 예외는 별도 환경로그로 남아 있으며 전체격리로그 무오류라고 하지 않습니다. Issue85 패키지/GUID HOLD 유지·수정0.

실패/보완 기록도 private Temp에 유지했습니다. Y만 바꾼 후보의 E겹침은 실제 육안 검수HOLD였고, 중간 안내 검사1건은 복제NPC의 비활성 안내자식을 선택한 검사기 문제여서 실제 활성Canvas로 정정했습니다. 후보 변경 사유가 생겼을 때만 같은 소형 표적QA를 재실행했습니다. 최종 판정은 StackVerifiedEvidence의155개 결과만 사용합니다.

## 원본 적용·Import·보호

실행중 원본 Editor를 Unity MCP CLI로 읽기전용 확인했습니다. 최초/최종 모두 **Bootstrap·PlayOFF·PausedOFF·compileFalse·Scene dirtyFalse·selectionNull·focusedNull**, 원본Scene/선택/OS포커스 조작0. 현재CLI Pipeline은 연결되지 않지만 기존 MCP for Unity 원본 인스턴스 연결로 상태를 확인했으며 사용자가 켠 Editor를 종료하거나 Batch로 다시 열지 않았습니다.

PASS후 원본 working tree에 **C#2개만 적용**, 두 파일만 `AssetDatabase.ImportAsset`으로 표적Import했습니다. 전체Refresh/Play/Scene저장/새 원본검사파일 추가0. 실제 로드된 Offset1.37·안내LateUpdate·OGA11/13 매핑·컴파일 완료를 확인했고 **Console errors0 / warnings42**입니다. Console Clear0. main HEAD **`52f84e643d5d860665eadfe1614a30c745209f00` 그대로**, 제품은 원본미커밋 working tree로 적용됐고 별도브랜치에 커밋했습니다. main 직접commit/merge/Push/reset/stash0.

완료문서 갱신 전 보호 **8346개 중 C#2개만 변경·나머지8344 SHA변경0**, 코드2개 제외 기존 git status byte-exact 동일. PNG/meta/PPU/Pivot/기존Scene/Save/UserData/Discord·카탈로그최근매핑을 보존했습니다. 이후 보고서/CODEX_LATEST/CURRENT_STATUS 3문서만 별도갱신합니다. 최종 Docs SHA/보호 receipt는 Issue106 실제댓글·`Temp/Issue106Nameplate20261117/Final.json`에 기록합니다. F:실측 **321445638144bytes**(시점값).

## 대표 확인과 근거

새 Play에서 시작마을 **은행안내인(4.4,2)**·**훈련장관리인(6.4,-2.1)**·**주민대표(0,1.5)**의 얼굴 위 이름표를 확인하시면 됩니다. 가까이 접근하면 E안내가 이름표 위에, 퀘스트 표식이 있으면 그 위에 따로 표시됩니다. 주민1/2/3/4는 상시이름표가 없는 것이 정상입니다. 대표 실제게임 수용은USER_REALPLAY_PENDING입니다.

근거는 private `Temp/Issue106Nameplate20261117/` 아래 Evidence/bounds-before.txt, StackVerifiedEvidence/checks.txt·status.txt·Errors.txt(0)·EnvironmentErrors.txt·bounds-after.txt·30PNG, Unity-StackDelta.log, original-state-after.json, catalog-before.cs/prompt-before.cs, Final.json입니다. 검수용 이미지·합성Save·개인보호목록·검사기는 제품커밋에 포함하지 않았습니다. 사용skill reference: unity-cli/references/integration-advanced.md 및 common-workflows.md, unity-mcp tools-reference.md, 기존 uGUI 유지기준. 추가 기획/원화/패키지 작업은 자동 시작하지 않습니다.
