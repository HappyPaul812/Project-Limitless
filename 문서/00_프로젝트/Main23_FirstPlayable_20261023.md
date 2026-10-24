# Chapter3 Main23 「따뜻한 땅을 떠난 자들」 첫 플레이 — 2026-10-23

**ISOLATED_FIRST_PLAYABLE_PASS / MAIN23_ISOLATED_FIRST_PLAYABLE / USER_VISUAL_PLAY_REVIEW_PENDING**

기준 main `47ad2eecb40135b2ee28e4947d642ebdbf0bf68a`, [Issue #56](https://github.com/HappyPaul812/Project-Limitless/issues/56), docs/planning의 10/23 착수 계약 blob `5e6b1bcf12d42891d9be798f37729920388efc98` 및 10/21 기술 계약을 대조했습니다. 문서화 → 격리 구현 → 실제 Unity 검증 순서로 진행했습니다. 원본 적용·Push·PR·main 병합은 하지 않았습니다.

- 브랜치: `codex/chapter3-main23-first-playable-20261023`
- worktree: `Temp/Main23FirstPlayable20261023/Worktree`
- 제품 commit: `dcf9d6af71940197c2c71e621253cd5de86cbdd8` (20파일)
- 검증: 신규 Play Mode 106 + 추가 delta 28 + 보호/정적/최종 Scene 검사 14 = **148 PASS / 최종 FAIL 0**. 기존 Main22 136/F1 64/Mouse40/Wolf/Bear/Fox 전체 QA 재실행 0.

## 5목표와 실제 동선

|목표|구현·실제 검증|결과|
|---|---|---|
|1 F1 심부 이동|Main22 완료 Save에서 Available, 쉼터에서 직접 시작. 실제 Player y≥13 도착 후 진행|PASS|
|2 서로 다른 흔적2|동물 (-2,15), 장작·짐수레 (2,18). 대화 종료 후 별도 Main23ClueFlags 기록. 중복 조사 차단|PASS|
|3 마을 남문 도착|F1 (0,22) 명시 이동 → World_FrostwayVillage (0,-8). Spawn 완료 후 도착 인정|PASS|
|4 주민·태온·하렌|주민 표식 (-3,-4)에서 증언과 형제의 판단 장면을 끝까지 읽어야 진행. 창 열기만으로 완료되지 않음|PASS|
|5 안전거점 등록|중앙 (0,0)에서 명시 등록, 안전 Spawn (0,2) 확인 → LastSafeZone 활성 → 목표 완료 → 저장. 등록 전 취소 가능|PASS|

Quest ID는 기술 예약안 `main_23_southbound_traces`입니다. 보상은 첫 플레이 임시 무지급이며 정식 경제 수치 확정이 아닙니다. 완료 후 자유 이동이 유지되고 Main24 Definition/자동 시작은 없습니다.

F1 기존 남부를 이동하지 않았습니다. Bounds만 x[-16,16], y[-12,24]로 북쪽 12WU 확장했습니다. 기존 YAML 블록 중 Bounds, Bounds 자식 목록, Scene 루트 목록 외 변경0이며 제거는 옛 북벽 3블록뿐입니다. Main22 조사·전투·쉼터 좌표, Quest Asset과 기존 Spawn을 보존했습니다. 북쪽 연장 지면/양옆 벽/명시 선택용 북단 stop을 추가했습니다.

새 마을 Bounds는 40×28WU, Grid 셀은 2WU입니다. 승인 `FrostwayVillageV10`의 기존 Sprite9/Tile9를 그대로 참조합니다. 새 이미지 제작·PNG/meta 수정0입니다. 남문 (0,-10) → F1 (0,20) 왕복이며 입구와 상호작용 위치는 2WU 떨어져 진입 직후 재전환되지 않습니다. 명시 선택 출구는 물리 stop을 유지합니다. 북문 (0,9)은 안내만 제공하고 외곽 벽이 닫혀 있습니다. 동쪽 포탈 기능/자동 Trigger/목적 Scene/전투 Spawn은 없습니다.

## 임시 미술과 서비스 경계

건물·NPC 전체49종을 최종 승인으로 취급하지 않았습니다. 여관/파티의 단순 충돌 영역은 노란 윤곽선과 `임시 표식` 텍스트로 구분합니다. 주민·서비스 위치도 임시 기능 표식입니다. 승인 건물 배치나 최종 마을 미술 완료를 의미하지 않습니다.

여관 HP/MP 회복, 기존 파티 관리, 기존 펫 분양·관리 NPC 설치를 재사용했습니다. 펫 역할은 각각1개이며 기존 Wolf v5/Bear v4/Fox v7 PNG 바이트와 정의에 제품 변경이 없습니다. 새 강제 구매·품목·가격·탈렌트 차감·스킬 소유 Save는 추가하지 않았습니다. 스킬 판매 표식은 준비 중 안내만 합니다. 정식 상점·건물 미술과 일반 보급 카탈로그 추가는 이번 완료 범위가 아닙니다.

주민·세린·태온·하렌·폴·미엘 텍스트는 STORYBOARD_V1 기반 임시 대사입니다. 정식 Dialogue/TTS ID가 아니며 미엘 과거와 최종 형제 화해 대사는 넣지 않았습니다. Main24/F2 전투/한기/새 몬스터/동쪽 포탈은 미구현 유지입니다. 파티 슬롯 구조 변경0.

## Save/Continue와 실패 처리

- Version1/기존 슬롯 체계를 유지하며 Main23ClueFlags 선택 필드만 추가했습니다. 구버전 누락0, 마스크 `&3`, Reset 연결을 확인했습니다.
- 시작/흔적1개/마을 도착/증언 후/완료 후 합성 QA Save를 사용했습니다. 실제 저장 파일을 읽어 RestoreSession과 Scene 로드를 실행해 재개를 확인했습니다. 개인 UserData는 QA 입력으로 사용하지 않았습니다.
- 흔적 비트가 정본입니다. 손상 저장에서 횟수만2로 바뀌어도 실제 독립 흔적 수로 보정하며 뒤 목표를 건너뛰지 않습니다. Main22 진행은 보존합니다.
- 시작 전 마을 방문·증언·거점 클릭은 시작/목표를 소급 완료하지 않습니다. F1 왕복을 허용합니다.
- `BattleSceneFlow.ReturnAfterDefeat()`의 실제 Scene 전환 경로로 새 LastSafeZone Scene/Spawn 복귀를 확인했습니다. 기존 Main22 전투를 다시 수행한 검증은 아닙니다.
- 쓰기 실패를 QA 폴더 안에서 유발했습니다. 기존 QuestService 의미대로 **메모리는 완료, 디스크는 등록 전 상태**가 될 수 있습니다. 실패 안내가 표시되고, 기존 파일 Continue는 등록 전 단계로 복원되며 재등록 후 Save/Continue가 성공했습니다. 원자적 디스크 트랜잭션으로 변경했다고 주장하지 않습니다.

## 검증 환경과 근거

설치 Editor `6000.6.5f1 (3ff58d469c8a)`의 숨김 batch 프로세스에서 실제 Play Mode·PlayerController·Rigidbody2D·Input System·Scene 로드를 실행했습니다. 저장소 버전은 `6000.5.7f1`, 사용자 원본 로컬 설정은 `6000.6.5f1`로 차이가 있습니다. 6.6 자동 환경 변환은 검증에만 사용하고 패키지·ProjectVersion·범위 밖 메타데이터는 제품 commit에서 제외·복원했습니다. 실행 환경 파일과 raw 로그는 로컬 `Temp/Main23FirstPlayable20261023/Runtime/Environment` 및 상위 로그에 남겼습니다.

가상 Keyboard/Mouse로 실제 입력 경로를 실행했습니다. 클릭 대상 owner, E/F 조사, Escape 파티 취소, W 실제 도보 이동, 휠체어 물리 경로·양문·주민·거점·여관·파티 접근을 확인했습니다. 사용자의 실제 입력이 QA와 섞이지 않도록 QA 프로세스 안의 물리 장치 입력은 비활성화했습니다. OS 입력·창·foreground/Game View 포커스 전환0입니다. 실제 사용자 장치 손플레이는 별도 확인 사항입니다.

아래 PNG는 **Unity Camera + Canvas를 RenderTexture로 실제 렌더한 화면**이며 데스크톱/foreground Game View 캡처로 표시하지 않습니다. 3비율에서 카메라 viewport 전체가 Bounds 안에 있는지도 검사했습니다.

- [F1 두 흔적 완료 화면](Evidence/Main23FirstPlayable20261023/F1_TwoTraces_UnityCameraUI.png)
- [주민 증언 화면](Evidence/Main23FirstPlayable20261023/Village_Testimony_UnityCameraUI.png)
- [16:9 마을 완료 화면](Evidence/Main23FirstPlayable20261023/Village_UnityCameraUI_1280x720.png)
- [4:3 마을 완료 화면](Evidence/Main23FirstPlayable20261023/Village_UnityCameraUI_960x720.png)
- [21:9 마을 완료 화면](Evidence/Main23FirstPlayable20261023/Village_UnityCameraUI_1680x720.png)
- [106개 Play 검사](Evidence/Main23FirstPlayable20261023/PlayChecks.txt), [28개 추가 delta 검사](Evidence/Main23FirstPlayable20261023/DeltaChecks.txt), [14개 최종 검사](Evidence/Main23FirstPlayable20261023/StaticChecks.txt), [F1 블록 보호 근거](Evidence/Main23FirstPlayable20261023/F1DeltaProof.json)

초기 실패는 생성 Quest 리소스 ID 갱신과 Start/Update의 이동 컴포넌트 중복 설치를 수정한 뒤 해소했습니다. 실패 시도 로그는 로컬 보존했습니다. 최종 기능 런타임의 예상하지 않은 오류0, 컴파일 오류0, Missing Script0, Tile9 참조 정상, BuildSettings 기존 순서/configObjects 보존 및 SceneGUID 중복0입니다. **기존 Unity Editor Search 인덱스 ArgumentOutOfRangeException은 별도 OPEN**이며 제품 런타임 오류0과 혼동하지 않습니다. 저장 실패 QA에서 의도한 오류는 별도 기록했습니다.

## 원본 보호와 남은 확인

원본 main HEAD `47ad2eec`·working status 동일·staged0을 재확인했습니다. 원본 Save/UserData 및 승인 마을 리소스 총61파일 SHA256 불일치0, 승인 펫3 PNG 원본/격리 SHA 일치입니다. 원본 게임 파일·개인 설정·기존 미커밋 작업을 수정하지 않았습니다.

바울 대표께 남은 확인은 격리 Scene의 미술/임시 표시 수용 여부와 실제 장치 손플레이입니다. 건물·NPC 정식 미술, 정식 대사/TTS·보상/가격, Main24·SkillSave #46·F2·동쪽 포탈, 기존 Editor Search 이슈는 미완/별도 게이트로 유지합니다. 원본 적용·Push·main 병합은 별도 지시 전 진행하지 않습니다.
