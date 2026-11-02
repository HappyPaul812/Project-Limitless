# Main24 비결제 첫 플레이 — Issue #80 / 2026-11-03

## 실행 범위와 기준

바울 대표의 이번 채팅 구현 지시와 **격리 후보 ID 정의 허용** 답변을 근거로 구현합니다. 공식 상품·가격·소유권·무료 한기 아이템 정책은 확정하지 않습니다.

- 실제 로컬 main/시작 HEAD: `ab47b931fec92976170f9f50aabde1822d78bb90`.
- GitHub connector로 확인한 원격 main: `bfd281545632877325f5d21e04cefb924042c465`; origin/main도 동일. 로컬의 #46·#73 완료 기능을 보존하기 위해 로컬 main에서 분기했습니다. git 네트워크 DNS 실패는 GitHub connector 읽기로 보완했습니다.
- 브랜치: `codex/main24-first-playable-no-purchase-20261103`.
- 격리 worktree: `Temp/Main24FirstPlayable20261103/Worktree`.
- QA 복제본: `Temp/Main24FirstPlayable20261103/QA/Client`. tracked Unity 파일만 복사하고 기존 합성 QA Library 캐시만 재사용합니다. UserData/개인 Save/UserSettings 복사 없음.
- Unity 개발 기준: **6000.6.5f1 / 3ff58d469c8a**. 원본/후보 worktree ProjectVersion·Packages는 보존합니다. 테스트 복제본에서 Editor가 생성하는 메타데이터는 납품에 포함하지 않습니다. #79 버전 정렬 후보 `5f619bc0`도 변경하지 않습니다.
- #36·#46·#70·#78·#79·#80 및 댓글, GitHub main 인계와 docs/planning 최신 인계 확인. main 인계의 10월 미구현 기록보다 실제 로컬 #46·#73 및 11월 #80 지시가 최신입니다.

## 한 장 플레이 시나리오

1. Main23 중앙 안전거점 등록을 완료합니다. 기존 Save/체크포인트는 그대로이며 Main24는 시작 가능 상태가 됩니다. 강제 대화나 자동 전송은 없습니다.
2. 남동쪽 **스킬상점 안내 / 무료 6칸 장착** 표식으로 이동합니다. 클릭 또는 E/F로 메뉴를 엽니다. 안내를 끝까지 들으면 Main24가 시작되고 다음 목표가 갱신됩니다. Esc·메뉴 닫기로 취소하면 시작/안내 진행을 기록하지 않으며 재진입할 수 있습니다.
3. 안내는 기본3 무료·최대6칸·전투 밖 어디서나 무료 장착/해제·강화판의 기존 슬롯 승계 정책을 설명합니다. 무료 장착 메뉴는 기존 `SkillManagementPresenter`를 열며 K/기존 화면 버튼도 유지됩니다. 판매 목록은 없고 **상품 준비 중 / 구매 없음**을 명시합니다.
4. 남서쪽 **보급 안내** 표식에서 한기 2턴 행동불가 위험과 해제 수단 준비를 끝까지 확인합니다. 무료 물품 지급·소모·구매·새 전투효과는 없습니다. 이야기를 진행하는 데 구매가 필요하지 않음을 알립니다.
5. 중앙 길을 따라 북문으로 직접 이동하면 북문 도착이 기록됩니다. 북문에서 F2 첫눈의 숲 방향과 서리올빼미 한기 위험을 확인합니다. 이 시점 상태는 **F2 진입 대기 / 전투 준비 중**입니다. Main24 전체 완료·보상·Main25 해금은 발생하지 않습니다.
6. 마을에서 자유 이동·저장·Continue·장착 변경·남문 F1 왕복을 계속할 수 있습니다. 기존 휠체어/도보 이동 및 마우스 경로 탐색을 그대로 사용합니다.

## ID와 구조

|종류|사용값|상태|
|---|---|---|
|Scene|`World_FrostwayVillage`, `World_NorthF1`|기존 Scene 재사용, 파일 변경 없음|
|선행 Quest|`main_23_southbound_traces`|기존 5목표/보상/저장 불변|
|Main24 Quest|`main_24_first_snow_warning`|이번에 허용된 격리 후보|
|기존 예약 NPC|`frostway-skill-shop`, `frostway-supply`|Main23 기술계약 예약값 재사용; 신규 이름/외형 없음|
|목표|`hear_skill_guidance`, `hear_chill_preparation`, `approach_north_gate`, `await_f2_opening`|격리 후보, 마지막 목표를 발생시키는 코드 없음|
|목표 target|`frostway_main24_north_gate`, `frostway_main24_f2_ready`|격리 후보. 후자는 미구현 대기용이며 완료 통지 없음|
|대사|`main24_candidate_skill_01~04`, `main24_candidate_supply_01~03`, `main24_candidate_north_01~02`, `main24_candidate_guide_01`|TTS 미등록 격리 후보, 기존 Voice/Story ID 변경 없음|

새 Save 필드/Writer/복구/구매 API 없이 기존 Quest objective count와 #46 6칸 저장을 사용합니다. NPC 안내가 끝난 콜백에서 현재 단계와 일치하는 목표만 통지하므로 역순·반복 대화가 다음 목표를 넘기지 않습니다. 북문은 실제 Scene·Spawn 완료·거리·모달 상태를 확인합니다. 마지막 F2 대기 목표는 어떤 안내 콜백도 완료하지 않습니다.

## QA 결과

**FIRST_PLAYABLE_ISOLATED_PASS / MAIN_NOT_APPLIED / F2_NOT_IMPLEMENTED / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**

- 실제 Unity6000.6.5f1 백그라운드 D3D11 Play Mode **81 PASS / FAIL0**. 기존 Main23 148/136·저장85/72/72 전수 재실행0입니다.
- Main23 마지막 체크포인트 등록→Main24 Available/강제시작 없음, 안내 취소/재진입/반복, 스킬상점 선택→실제 기존 Canvas6칸 장착·해제·6번째 슬롯 저장, 보급 안내→북문 도착/대기, 화폐77/무료 소유3 유지, 기존 안전거점·Save/Continue·마을/F1 왕복 PASS.
- 가상 InputSystem E/F/Esc/마우스 클릭과 실제 UI 버튼 핸들러, 휠체어·도보 실제 경로/키보드 이동 PASS. 물리 키보드·마우스/게임패드/발 조작/스크린리더는 NOT_VERIFIED입니다. 새 게임패드 시험을 실행했다고 주장하지 않습니다.
- 1280×720/960×720/1680×720 실제 Unity Camera+Canvas 렌더와 기존6칸 UI, 총 PNG4장을 검토했습니다. HUD의 현재 목표/방향/거리/조작 및 F2 준비중 문구가 잘리지 않습니다. **OS GameView 캡처가 아니며 OFFLINE HTML 시안도 아닙니다.** 승인 아트 재검수·미술 승인 대체용 이미지가 아닙니다.
- 변경 범위·기존 writer/6슬롯/스킬UI/Main23 Quest/Version/Packages/BuildSettings 보호·ID 중복·F2 미완·QA 소스 동일성 정적 **14 PASS / FAIL0**, 전체 변경 `git diff --check` PASS.
- 최종 컴파일 오류0 / 제품 런타임 오류0. 기존 `UnityEditor.Search.SearchDatabase` 초기화 `ArgumentOutOfRangeException` 1건은 별도 환경 OPEN. 기존 deprecated API 경고는 미수정입니다.
- 초기 실패 보존: 첫 실행은 캐시 복제의 Search 소스3폴더 누락으로 패키지 컴파일 실패→동일 기존 QA 캐시에서 복구. 둘째 실행은 새 QA 도구 captureWidth 선언 누락→QA만 수정. 셋째 실행에서81PASS. 초기 정적6PASS/8FAIL은 CRLF 체크아웃 바이트와 LF blob 비교 오검출이며 Git 필터 적용 blob 비교로14PASS. 제품 기능 실패와 구분합니다. 상세 [RuntimeSummary](Evidence/Main24FirstPlayable20261103/RuntimeSummary.json), 원시 실행로그는 QA 폴더에 로컬 보존하며 라이선스/개인경로가 섞일 수 있어 게시하지 않습니다.

재현: 새 QA 복제본에 격리 worktree의 tracked Unity 파일을 복사하고, 개인 UserData/UserSettings 없이 배치 실행합니다. 실행기 자체가 정확한 QA 경로와 batchmode를 검사하며 합성 Save/Settings 루트를 주입합니다. `Unity.exe -batchmode -projectPath <repository>/Temp/Main24FirstPlayable20261103/QA/Client -executeMethod ProjectLimitless.EditorTools.Main24FirstPlayableQA.Start -logFile <QA로그경로> -force-d3d11`. 변경범위 정적 재현은 `python Tools/QA/main24_first_playable_scope.py`입니다. 원본에서 QA 실행 금지.

### 보존과 커밋

제품/QA/재현도구9파일 commit **`70e363bea43d72c7e68abb089952e2d6f6a77a8b`**. Main23 Quest/Scene·Spawn·Collider/마을 원본·승인 PNG/TTS·기존6칸 Save·BuildSettings/Packages/ProjectVersion 변경0입니다. #70·#77·#78 코드 merge/Pick/import0. 확인한 기존 격리 HEAD는 #70 `a6f2b0d5`, #78 `fd657384`, #79 버전정렬 `5f619bc0`이며 그대로 보존했습니다.

원본 main은 `ab47b931` 유지, 초기 git status344항목이며 완료 전후 status344항목의 SHA256 지문 일치도 확인했습니다([Git 보호 근거](Evidence/Main24FirstPlayable20261103/OriginalGitAfterQA.json)). 개인 Save/UserData 파일의 열람·복사·해시·수정0, 원본 Editor 실행/Scene전환/foreground/Computer Use0, Discord#66/계정/OS설정 변경0입니다.

근거: [81개 실행 검사](Evidence/Main24FirstPlayable20261103/checks.txt), [정적14개](Evidence/Main24FirstPlayable20261103/StaticScope.json), [스킬6칸](Evidence/Main24FirstPlayable20261103/Main24_SixSlots_1280.png), [4:3 HUD](Evidence/Main24FirstPlayable20261103/Main24_Hud_960.png).

## 남은 범위 (최대 5개)

1. F2 Scene/Spawn/Bounds·길·왕복 및 공식 퀘스트 연결. 현재 **F2_NOT_IMPLEMENTED/HOLD**.
2. 첫 서리올빼미 지정전투·한기 예고·대응 기회. 승인 v3 디자인은 보존하며 Attack HOLD 모션 적용 없음.
3. 무료 한기 해제 수단의 수량·재공급·도망/패배 정책 승인 후 구현.
4. 상품25종 이름·가격·효과·stable ID·해금·소유권 승인 후 별도 결제 구현.
5. 격리 후보 Quest/Dialogue ID·문구의 정식 잠금 및 물리 입력/스크린리더·본게임 적용 검증.

본게임 원본 적용/원격 Push/PR·병합 없음. 문서와 PNG는 로컬 브랜치에만 있으며 GitHub에는 Issue #80 결과 댓글만 게시합니다. 정식 적용은 이번 작업에 포함하지 않습니다.

## GitHub 보고 영수증

[Issue #80 실제 결과 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/80#issuecomment-6590035953) 등록 성공. 제품 `70e363bea43d72c7e68abb089952e2d6f6a77a8b`, QA/문서 `5b02b56e467e5712bbe184b6f21c8e4640b2c76d`를 보고했습니다. 본 후속 commit은 영수증만 기록하며 제품 변경·Push·원본 적용은 없습니다.
