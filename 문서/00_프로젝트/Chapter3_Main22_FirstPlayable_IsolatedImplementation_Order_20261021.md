# LIMITLESS — 2026-10-21 Main22 첫 실제 플레이 vertical slice 구현 명령 (Codex)

## 0. 발주 근거 및 현재 확정 상태
- 바울 대표가 Chapter3 첫 실제 플레이(Main22)를 최우선으로 지시하고 서리들쥐 모션, 북부 F1 6 PNG를 최종 미술 승인. Codex F1 격리 Unity preflight **64 PASS / 0 FAIL**, branch `codex/chapter3-f1-unity-preflight-20261021`, 최종 commit `8cf6cd7ebeb2f9b5d2f08c9151ceca84bb271c77` (사용자가 공유한 보고). 실제 GameView 16:9/4:3/21:9, 승인 PNG6 SHA/PPU/Pivot/TileGrid/Sorting·흙/서리 접합, 도보·휠체어 길·소품 Collider·카메라 경계, 별도 임시 출입→임시 안전지점 Save/Continue 64 PASS. **전투 40 PASS는 재실행하지 않음**.
- F1 화면에서 길의 연속 가시폭 최소 약 **1.45 WU**, 공통 Collider 지름 **1 WU**. 이 수치는 Codex 격리 테스트 값으로 main GameView와 World_StarterVillage 실제 마을 북문 물리 충돌 검사로 과장하지 않음.
- **미구현**: 실제 World_StarterVillage 북문 출구, Main21 완료 후 Main22 Quest Available/ID/자동진행 정책, 정식 F1 Scene 연결, Main22 5목표·지정 서리들쥐 전투·안전쉼터 연동, 원본 Unity 정식 적용/PR/main 병합. User original 4351파일·Save/UserData·Editor 기존 상태 보호, 원본 실제 손플레이 미검증.
- 근거 상세스토리 `문서/03_스토리/Chapter3_Main22_북풍의징후_상세기획_v1_20261014.md` (docs/planning), `Chapter3_20261021_F1_Main22_FirstPlayable_Priority_And_BossDefer.md`, [Issue #35](https://github.com/HappyPaul812/Project-Limitless/issues/35). 상세스토리 대사는 아직 최종 스크립트/VoiceID/TTS 승인이 아닌 **프로토타입 대사 후보**. 자체 Quest ID/Spawn/Reward/Monster ID는 실제 정의와 충돌검사 이후에만 확정·정본 문서화, 임의 main data 오염 금지.

## 1. 이번 구현 승인 범위 — 격리 worktree 최초 플레이 한 구간만
- **Codex feature branch `codex/chapter3-main22-first-playable-20261021` (별도 clean worktree)**. 최신 remote main·기존 Main21 실제 엔진·F1 QA worktree·서리들쥐 Combat Apply40PASS branch들의 상태와 승인 PNG SHA를 대조하고, **기존 입증된 파일/스프라이트 연결을 재활용**하며 재구현·중복 Scene 생성 금지.
- 승인 F1 PNG6, **동일 SHA+신규 안전 Meta/GUID**를 사용한 별도 `NorthF1` 실제 탐험 Scene 제작 또는 기존 격리 QA Scene을 제작용으로 최소 승격. Editor Import 설정·Sprite atlas/Prefab/TileGrid/물리 동선·Camera/Rigidbody·음성 참조는 F1 64PASS 검증 사양 재사용. 미정 최종 자산명은 파일 실제 고유 이름/Scene ID 충돌 검사 후 확정.
- `World_StarterVillage` 기존 북쪽 지형·NPC·Collider/Trigger/사용자 수정사항 보호. Main21 완료 후 자유 탐색과 기존 `SafeZonePetServiceRollout` 펫 분양/관리2 NPC 선택 사용을 보장하며, **명시적 북문 조작/확인**으로 F1 남쪽 안전 Spawn 이동. 북문이 기존 Scene에 없는 경우 기존 객체/출구/Marker 사용 여부를 먼저 읽고 **필요한 것 하나만** 신규; 원래 Chapter1 NPC·퀘스트·E 상호작용 우선순위 보호. F1에서는 명시적 남문 상호작용으로 마을 안전하게 역복귀, Save/Continue 좌표·포탈 무한 튕김 없는지 확인.
- `Main21_ReturningWarmth.asset`의 완료 13번째 목표와 Save를 절대 훼손하지 말고, 새 Main22 **Available 상태**를 해금하고 사용자가 북문에서 직접 출발할 때 01을 완료하도록 한다. 연동은 기존 `QuestService`의 실제 방법을 재사용. Main21부터 자동강제 화면전환/펫 구매 강제 금지. 필요시 `nextMainQuestId`를 안전하게 업데이트하되 Main21 보상/13Goal/대사/GUID 불변을 증명.
- **Main22 목표5** (정식 목표 ID/구조는 현 엔진에 맞춰 local code와 함께 잠금): (1) 북문에서 명시적 이동→F1 진입; (2) 흙·풀 사이 얇은 서리 조사점 1개 상호작용; (3) 남하 동물 이동 단서 2개 확인—표시는 '두 단서를 조사'로 명료하고 UI 카운트/마커 제공; (4) **고유 지정 서리들쥐1기 승리** 시에만 완료, 임의 일반몬스터 처치로 진행X; (5) F1 안전 쉼터에서 짧은 정보 정리→Main22 완료/다음 Main23 제작대기 상태. 기존 Save/Continue/패배 체크포인트·전투 도주 정책에 맞춰 패배/도주 시 재도전 가능/동일 지정몹 반복 스폰 및 중복 보상 금지.
- 사용자 인터페이스: 새 Quest 시작 시 '북문 어디? 어떻게 조사?'가 명확하도록 HUD 현재 단계·이동 가능한 길 표식·조사 가능한 오브젝트 이름·마우스 클릭/E 기존 사용법 지침·장애인 접근성. **발로 마우스 사용** 등 최소입력/반응시간 강요X. 조사 확인/결과 요약 텍스트는 임시 프로토타입 메시지로 명확히 표시하고 기존 TTS/WAV GUID를 훼손하지 않는다.
- 신규 전투 스프라이트/몬스터 AI/스탯 밸런스를 **임의 추가 발명 금지**. 기존 승인 `FrostFieldMouse` Idle4/Attack4/Hit4/Defeat4 연결(격리 Battle40PASS)을 그대로 사용, Main22 첫 몬스터 단독1기. 정확한 ID·레벨/EXP/잡템/탈렌트/퀘스트 보상은 기존 밸런스 카탈로그 확인 후 미확정이면 **중립적인 프로토타입 보상 없음**으로 기록하고 반드시 사용자 확인 게이트로 남김. 신규 상점/인벤토리 아이콘·잡템 누락은 별도 스프라이트 발주 대장에서 계속 관리, 완성이라 주장 금지.

## 2. QA (기존 F1 64 PASS, Mouse Combat 40 PASS 재사용)
- 이번 새 변경점만 **Main21→Main22 Available/북문 명시적 선택→F1 안전 스폰→조사01/02·단서2→지정 전투 1승리→쉼터→완료**, 전투 패배/도주/재진입/실패·리셋, 저장 후 이어하기(각 중요 단계 1+), F1↔마을 2방향 문, SaveGUID 유효성, 퀘스트 Marker/HUD 명확성, 펫 서비스 접근을 최소 delta QA.
- 도보·휠체어 통행 경로/카메라, 기존 F1 16:9/4:3/21:9 GameView 64PASS는 **이미 검증된 샌드박스 버전·해시가 동일하면 재사용**; Scene/Collider를 새로 수정한 경계만 추가검사. Monster Battle40PASS 중복회귀 금지. 프레임 매핑 변경 생기면 그 영향 범위만 재검사.
- 기존 사용자 original Unity Editor/Play/dirty/untracked/Save/UserData/Main09/10·Main21/Field09 공식 Art·Chapter2 및 모든 미승인 Stage3 ZIP, main SHA 보존. 격리 작업 전 본래 Git 원본 상태/파일 매칭 SHA Snapshot, 격리 작업 후 'original changed0' 입증. **원본 로컬 main 자동 복사·GameView 강제 플레이·Push/merge, main 직접 커밋 금지**.
- 실제 Unity로 뒷받침되지 않은 결과는 '오프라인 미리보기'로 라벨링. 대량 Scene/Quest/boss/TTS/BGM/새 Spell/새 몬스터·설빙의 폭군, Main23~29, 한기·동상·동결괴사 추가 금지.
- 결과: feature branch/작업커밋, 영향 파일/구현 코드, 현행 ID 잠금표와 임시 상태, 실제 최소 GameView 3비율 캡처, Main22 퀘스트 HUD 안내 캡처, QA PASS/FAIL와 미결 blocker, 원본보호 SHA·Save, 원본 적용/remote main 변경0, **사용자 직접 플레이 승인 필요 여부** 보고.

## 3. 단계 기준
- Codex가 한 번에 구현할 수 있는 범위는 **격리된 Main22 전부 5목표 첫 플레이**까지. 그 다음 바울 대표가 실제 GameView/퀘스트 동선·전투/마을 복귀 QA를 승인하면 **원본 Unity 최소 반영과 PR→main merge**를 별도 승인받는다. 장시간 구현이 사용자 작업에 충돌할 경우 F1 북문 연결+첫 조사까지 먼저 마일스톤을 나누어 보고하되 다음으로 계속 진행할 수 있는 branch/계약을 보존한다.
- F1/Pet/Wolf Run art 담당 작업은 병렬로 지속, 현재 Codex의 Main22 구현에 별도 재발주/중복 QA 의뢰하지 않는다. 실물 북부 잡템·스킬 아이콘·상점/소모템 아트 누락은 기획이 별도 카탈로그 관리.
