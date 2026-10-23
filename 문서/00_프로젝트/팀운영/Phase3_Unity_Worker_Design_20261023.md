# Phase3 후속 격리 Unity Worker 설계 — 미실행

현재 구현은 파일 수령/정적 검사와 **BLOCKED 상태 Codex 검수 요청 생성까지**입니다. 이 설계는 AI 호출·Unity 실행·승인·정식 적용을 활성화하지 않습니다. 핵심4역할 자동 인계 파일럿도 재호출하지 않습니다.

## Worker 입력 계약

- delivery SHA/격리 artifact 경로, 원본 Manifest·PNG sidecar의 별도 SHA, 기준main SHA, 관련Issue/담당 아트 역할, 정적 검사 report, 명시적 검수 대상과 보호 대상을 고정합니다.
- ZIP은 내부 Manifest가 artifact SHA에 포함됩니다. 독립 PNG는 향후 Worker 시작 시 sidecar SHA도 별도로 고정·재검증해야 합니다. 동일 PNG에 나중에 바뀐 sidecar만 보내는 것을 자동 새 승인으로 처리하지 않습니다.
- Issue59 기존 KEEP/HOLD·남문 신규자산0, Issue63 단일포즈PASS/애니메이션교체HOLD/권리충돌/대표승인대기를 현재 계약으로 상속합니다. 기술 검증과 제작/미술승인/교체 권한을 구분합니다.
- 명시적 Worker 실행 승인·시간/메모리/동시한도·실패중지 정책을 기록한 뒤 해당 업무만 BLOCKED에서 실행가능 상태로 전환합니다. 현재 수령 TECH_PASS를 Codex 검수 TECH_PASS로 승격하지 않습니다.

## 격리 실행과 보호

1. 기존 원본·기존 검수 worktree를 실행/수정하지 않고 새 검수 checkout과 임시 Import 영역을 사용합니다. 원본 Scene/승인PNG/GUID/meta·Wolf/Bear/Fox·Ch2 공식 몬스터/환경·게임 로직은 지문을 남겨 보호합니다.
2. quarantine에서 받은 파일을 재해시하고 명시된 경로/확장자만 새 격리 프로젝트에 복사합니다. 업로드 스크립트/메타/Unity 프로젝트를 실행하지 않습니다. 실제 Import 설정/PPU/Pivot/Sorting/Collider 계약은 자동 추측하지 않습니다.
3. Save/UserData는 별도 검수 경로로 고정합니다. 사용자 원본 Editor·OS 포커스·기존 Scene 저장 상태를 건드리지 않고 배치/백그라운드 테스트와 로그를 우선합니다. foreground 검증이 꼭 필요하면 별도 사전 허락을 받습니다.
4. 검사 범위는 변경분만 정합니다. 이전 Main23/펫/4역할 AI 전수 QA를 반복하지 않습니다. 코드·Scene·Save·승인아트 전후지문/Console·컴파일·경로 통행 등 계약에 필요한 검증만 수행합니다.

## GameView·애니메이션 비교자료

검수 카메라/Canvas의 실제 RenderTexture 캡처를 기본으로 하고, 게임 화면 캡처·검토 Canvas·오프라인 미리보기를 서로 다른 증거 유형으로 기록합니다. 기존 MinimalDelta65의 격리 캡처 기법은 참고 가능하지만 특정 worktree를 그대로 실행하지 않습니다.

원본·후보를 같은 배율/카메라/조명/배경과16:9·4:3·21:9 계약으로 비교합니다. frame_index/방향/셀크기/배율/시각과 원본PNG·Sprite GUID·Manifest SHA를 비교판에 연결합니다. 기존 Idle/Attack 프레임은 읽기 전용으로 샘플링하고, 신규1포즈에 없는19프레임을 생성하거나 기존 애니메이션을 교체하지 않습니다. 불투명도·alpha/경계·작은크기 식별성·기존 캐릭터 비례·Collider와 휠체어 동선은 각기 별도 검수 항목입니다.

출력은 실제 PNG/필요한 애니메이션 미리보기·index·SHA 목록·기술 판정·미결 항목입니다. 배치 렌더가 되지 않으면 NOT_VERIFIED로 종료하고 오프라인 Preview를 실제 Unity 화면으로 대체하지 않습니다.

## Discord 검토 자료와 승인·반려

업무ID/후보 artifact SHA/기준SHA/적용 범위, 기존·후보 실제 비교 이미지, 기술결과, HOLD·권리 미결, 읽을 수 있는 납품 파일을 전송합니다. 큰 묶음은 승인된 공유 경로를 사용하며 로컬 경로 문자열만으로 원격 열람 가능하다고 주장하지 않습니다. 인증정보·개인 Save·환경/비밀 로그는 자료에서 제외합니다.

기존 `/approve`/`/reject`의 owner ID·SHA·scope·실물 재해시 게이트를 재사용하되, **실제 아트 검수 evidence**와 권리/보완 확인 기록이 있어야 승인대기로 이동합니다. MOCK/REAL_CLI_READONLY/이번 수령 업무를 미술 승인 근거로 사용하지 않습니다. 대표가 실제 비교자료를 보고 명시적으로 승인·보완·반려해야 하며 AI는 이 결정을 대신하지 않습니다. 후보 파일·범위가 바뀌면 종전 승인을 자동 재사용하지 않습니다.

## 승인 이후 정식 적용 요청

미술 승인 뒤에도 별도 요청으로 적용 후보 diff·보호 지문·대상 Scene/자산·되돌리기·변경분 QA 계획을 준비합니다. 사용자 승인 범위 밖 변경0을 확인한 뒤 정식 적용을 요청합니다. 원격 Push/main 병합은 적용 승인과 별개이며, 명시 지시 전 실행하지 않습니다.

**설계 완료 / UNITY_WORKER_NOT_STARTED / ADDITIONAL_AI_0 / NOT_PRODUCTION_APPLIED**.
