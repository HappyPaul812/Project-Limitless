# Discord 실연결 후속 — 실제/모의 구분

## 범위 및 보호

바울 대표의 서버 초대·봇 온라인·실제 메시지 확인 후 진행. 기존 도구/큐/9명령을 재사용합니다. 실제 AI 호출0·추가 API 비용0·게임 변경0·SFX PAUSED·Push/main병합0. 원격 기록 요청과 Push 금지 조건을 함께 준수하여 작업 브랜치에 로컬 커밋만 준비합니다. GitHub 원격에는 아직 반영하지 않습니다.

## 실제 Discord

- 기존 사용자 Setup 결과: Gateway PASS, 원격 명령9종 PASS, 메시지 전송 PASS. 증거 discord-live-test.json, message_id 1563121648474001449.
- Setup은 검사 후 연결을 닫습니다. 후속 명령 시 오프라인 신고를 받고 저장된 DPAPI 토큰으로 Run 재접속, READY 확인. 토큰 재입력/출력 없음.
- 바울 대표가 status/pending/handoff 실제 응답을 확인하셨습니다. 로컬 응답 전송 영수증은 status1·handoff1 확인, pending은 사용자 확인 PASS / 자동 영수증 NOT_VERIFIED로 구분합니다. 봇으로 사용자 Slash 실행을 위조하지 않습니다.
- 모의 업무 L-3748cd0899ca4c80의 실제 handoff 후 Codex L-5ec5fde0032c4b31→기획 L-c2bb1722a38d485b APPROVAL_PENDING까지 완료. 3단계 모두 art=USER_REVIEW_PENDING. 실제 채널 안내1563123432705626256, 결과1563124488072204420 전송 성공.
- HandoffFinish는 실제 handoff 장부가 있으면 후속 모의 작업을 완료하며, 없으면 상태를 바꾸지 않고 WAITING_FOR_USER_HANDOFF를 보고합니다. 마지막 단계도 실제 미술 승인이 아닙니다.

## 모의 전달 검증

같은 시나리오를 로컬 복제 큐에서 단일 root로 검증: 리뉴얼→Codex→기획→APPROVAL_PENDING. 8 점검 PASS/0 FAIL. 신규 재시작·중복 방지·실제 아트 없는 승인 거부 회귀 테스트1 PASS. 최초 OS 임시폴더 sandbox 권한 오류는 작업폴더 내 임시경로로 수정 후 통과. 기존57/22 및 Unity QA 전수 재실행0.

메인 아트 NewArtAgent→CodexAgent는 역할 설정 확인 PASS이며 실제 별도 업무 실행 NOT_VERIFIED. 4역할이 실제 AI 세션으로 자동 실행된 것은 아닙니다. MockWorker/후속 도구는 고정 모의 보고만 생성합니다. 신규 아트도 Codex 검수→바울 대표 최종 미술 승인→승인 범위 적용을 유지하며 모의 TECH_PASS를 미술 승인으로 승격하지 않습니다.

## 인증 설정

설치 CLI0.163.0-alpha.11.2. cli_info가 인증 원문을 메모리에서만 판별하여 ChatGPT 구독 인증 가능=True. 잘못된 built-in provider override 제거 상태에서 features list 설정 검사 PASS. shell/apps/browser/computer/multi_agent 비활성, API 키 fallback 없음. [공식 인증 문서](https://learn.chatgpt.com/docs/auth).

이번 추론 실행0. 과거 bootstrap 실패1회 장부는 보존하고 승인 ID를 재사용하지 않습니다. 실제 모델 추론 성공·정확한 추론별 구독 사용량은 NOT_VERIFIED. 별도 승인 후 새 고정 ID·읽기 전용1회·시간상한으로만 가능합니다. 현재 대화의 구독 사용량을 0이라고 주장하지 않습니다.

## 원본 보호 및 저장

기존 보호 스냅샷7356개 SHA 변경0·누락0·원본status 동일. 원본HEAD47ad2eecb40135b2ee28e4947d642ebdbf0bf68a 유지. 원본Unity/Save/UserData/승인아트 변경0. 토큰은 현재 Windows 사용자 DPAPI+사용자 전용 ACL, data/private는 Git 제외. 증거 폴더에는 비밀 없는 선별 JSON만 보관합니다.

브랜치 codex/discord-ai-collaboration. 이번 구현 커밋은 후속 CURRENT_STATUS/인계에 기록합니다. 원격 Push·main병합 금지 유지. 실제 비소유자 Discord 권한 거부는 NOT_VERIFIED(로컬 모의 거부 PASS). 메인 아트 실제 업무 실행과 실제 AI 추론도 미검증입니다.
