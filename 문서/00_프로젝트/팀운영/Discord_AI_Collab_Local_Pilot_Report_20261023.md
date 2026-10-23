# Discord AI 협업 로컬 파일럿 완료 보고 — 2026-10-23

바울 대표 첨부 지시에 따라 **0단계 조사와 1·2단계 로컬 모의 파일럿**, 3단계의 파일 검역 일부를 완료했습니다. 실제 다중 AI·Discord 운영 완료는 아닙니다. 원격 보고/Push/병합·제품 적용은 하지 않았습니다.

## 구현과 검증 판정

| 대상 | 판정 | 직접 확인한 범위 |
|---|---|---|
| 독립 Python 환경 | PASS | 기존 py3_12 3.12.14 읽기 확인, 전용 venv, system-site-packages=false |
| 영속 작업 큐 | LOCAL_PASS | SQLite·이벤트 receipt·최대2개 claim·임대 만료·서로 다른 프로세스 재시작 |
| 역할·인계 | MOCK_PASS | 역할8개, 요청 시 TTS/BGM/영상, SFX PAUSED, 고정 next·깊이4 |
| Discord | MOCK_PASS / LIVE_NOT_VERIFIED | SDK 실제 import, 명령9개 등록, 가짜 interaction/첨부로 콜백 실행. 계정 연결0·외부 메시지0 |
| GitHub | READ_ONLY | 공식 연결 도구로 #63/#64/#65·PR54 조회. Python GET 어댑터는 모의 transport 테스트. 쓰기0 |
| Codex runner | PREVIEW_ONLY | 공식 CLI 버전·인증상태·help 확인, 읽기 전용 명령 생성. 추론 실행기는 차단 |
| 첨부 검역 | LOCAL_PASS | 합성 테스트 ZIP의 SHA/CRC/Manifest·RGBA decode·해상도/프레임·악성 ZIP 거부 |
| 승인 | LOCAL_PASS | 숫자 ID, SHA·scope 결합, 변경 전/후 무효, 동일 승인 유지, 모의 미술 승인 차단 |
| 예산·quota | LOCAL_MOCK_PASS | task/day/month micro-USD 선점, 실패/결과불명 보존, 초과 시 정지·유료 fallback 없음 |
| 실제 Unity·미술/권리 승인 | NOT_VERIFIED | 이 작업에서 실행·판정하지 않음 |

`TECH_PASS`는 검증한 해당 계약의 결과이며 `ART_APPROVED`가 아닙니다. 모의 보고 evidence의 mode=MOCK, unity=NOT_VERIFIED, external_ai_called=false가 보존됩니다. 보고 후 DONE은 **인계 완료**만 뜻합니다.

## 실행 근거

- 최종 신규 테스트 **57 PASS / 0 FAIL / 0 ERROR / 0 SKIP**, 84.739초. [개별 결과](Evidence/DiscordAICollab20261023/local-test-results.json).
- [모의 demo](Evidence/DiscordAICollab20261023/mock-demo-result.json): #63/#64/#65를 참조하는 복제 이벤트3개→역할 작업6개→APPROVAL_PENDING3개. 독립 프로세스 재실행에도 중복 없음. 실제 이슈 작업 재실행 아님.
- [모의 감사 장부](Evidence/DiscordAICollab20261023/mock-audit.json): 33개 이벤트, task/triage/claim/report/handoff/owner pending. 입력·토큰 원문을 audit에 넣지 않음.
- `compileall`·Git 공백 검사를 통과했습니다. Windows 샌드박스의 asyncio socketpair 차단으로 멈춘 **테스트 프로세스만** 식별 후 종료하고, 승인된 로컬 테스트 환경에서 재실행했습니다. Unity/사용자 화면·포커스 조작0.
- 초기 테스트에서 예산 테스트의 가상 날짜를 실행 임대 만료 뒤로 넘기는 fixture 오류1개를 수정했고 최종 결과에 남은 오류는 없습니다. 이전 53개 통과를 57개에 합산하지 않습니다.

검증 범주: 비소유자·타 guild/channel·bot 거부, SFX 정지/재개, on-demand 역할의 자동 발주 거부, 동시 claim8회 중2개만 실행, 중복 이벤트/키 재사용·인계 재처리, 깊이 제한, KST 일요일, 멈춤/만료 결과 차단, 기술·미술 분리, SHA·scope/변조/동일승인, 미승인 게임·셸·push·merge·게시 거부, 예산·토큰 기록·quota, CRC손상·traversal·ADS·예약명·symlink·zip bomb·위장 실행파일·PNG/Manifest 계약, URL/SSRF 거부입니다. 서브테스트 공격 사례를 별도 PASS 숫자로 부풀리지 않았습니다.

## 보호 범위

[보호 결과](Evidence/DiscordAICollab20261023/protection-result.json): 원본 Git 추적 및 비무시 미추적 **7,343개 파일 SHA 변경0**, 원본 porcelain 상태 **858→858 동일**, HEAD **47ad2eecb40135b2ee28e4947d642ebdbf0bf68a** 불변. 이 SHA 집계는 ignored Library/Temp/UserData 전체 감사라는 뜻이 아닙니다. 원본 UserData에는 파일23개가 있음을 읽기 전용 확인했고 이 작업에서 Save/UserData 읽기·쓰기 실행기를 연결하지 않았습니다.

기존 Main23FirstPlayable·ArtPilots·ArtReview65·MinimalDelta65 worktree4개는 최종 Git clean 상태를 확인했습니다. 파일럿 도구/venv/data/증거는 새 worktree 또는 이 작업의 Temp 루트에만 있습니다. 기존 148/136/41/15 PASS 및 후속 아트 QA를 재실행하지 않았습니다. 기존 승인 PNG·원본 ZIP·GUID/meta·Scene/C#·펫 변경0.

## 공식 제품 조사·비용

실제 설치: codex-cli 0.163.0-alpha.11.2, ChatGPT 로그인 상태. read-only/JSONL/ephemeral 옵션은 로컬 help로 확인했습니다. 모델별 실제 inference access·잔여량은 이번 추론0회이므로 미실증입니다.

[공식 인증](https://learn.chatgpt.com/docs/auth)은 ChatGPT 구독 로그인과 API 과금을 구분합니다. [공식 가격/한도](https://learn.chatgpt.com/docs/pricing)는 Plus의 Codex 사용이 작업·모델별 공유 한도와 주간 한도 영향을 받을 수 있음을 안내합니다. 따라서 24시간 무료 무제한 자동화를 보장하지 않습니다. [공식 비대화형 CLI](https://learn.chatgpt.com/docs/non-interactive-mode)가 1차 후속 후보이며, 장기 대화 UI에는 [공식 app-server](https://developers.openai.com/siwc/token-sharing-open-source/codex-app-server)를 별도 검토합니다. 기존 대화에 자동 접속하는 기능은 아닙니다.

이번 파일럿의 **추가 AI/API 호출0·추가 AI/API 비용 USD0**, Discord 실연결0, 새 이미지·음성·음악·영상 생성0, 외부 서버 배포0. 패키지 설치 다운로드 및 GitHub/공식 문서 조회는 수행했습니다. 이번 Codex 대화 자체의 기존 구독 사용량을0이라고 주장하지 않습니다. API 요금은 [공식 API 가격표](https://developers.openai.com/api/docs/pricing)와 활성화 당시 모델을 다시 확인해야 하며 자동 결제/키 fallback은 없습니다.

## 미완료와 다음 승인

1. **실제 Discord 연결**: 바울 대표의 테스트 guild·소유자 숫자 ID·허용 채널 ID 지정, bot 생성/초대 및 slash 등록 승인 필요. 비밀 토큰은 환경변수로만 설정. 자동 채널 생성/역할별 전송 outbox는 아직 미구현.
2. **실제 AI worker**: 공식 로그인·모델 접근·구독 한도와 OS/MCP 격리 검증, 단일 읽기 전용 추론에 대한 승인 필요. CodexRunner.run은 현재 항상 차단. 자연어→셸 연결 없음.
3. **3단계 나머지**: 실제 아트 ZIP 계약별 Manifest 변환, 공식 아트 인덱스, Unity 검사/캡처 실행기는 별도 구현·검증 필요. 현재 strict Manifest 불일치 시 기존 납품물을 수정하지 않고 거부.
4. **4·5단계**: Gemini quota 연동, BGM/영상 제작, 월간 영상 scheduler, 승인 파일 적용기, GitHub 쓰기/배포 등 미구현. SFX PAUSED. 실제 판매/결제·게임 Save 변경 없음.
5. AI 사용, Discord 실연결, 외부 배포, 게임 적용, 원격 Push, main 병합은 각각 별도 승인. 이번 완료 보고는 그 승인을 대신하지 않습니다.

사용법·복구·채널안·보안 경계는 [도구 운영 안내](../../../Tools/DiscordAICollab/README.md), 범위는 [착수 설계](Discord_AI_Collab_Design_20261023.md)를 따릅니다.

## 브랜치·커밋·GitHub 인계

- Worktree: `F:/study/codex/Project-Limitless/Temp/DiscordAICollab20261023/Worktree`
- Branch: `codex/discord-ai-collaboration`
- 설계: `ba82c396` / 구현·테스트·운영 안내: `094ea98d`.
- 본 보고·증거·CURRENT_STATUS·CODEX_LATEST는 후속 문서 커밋으로 보존합니다. 최종 SHA는 이 브랜치 HEAD를 참조하세요.
- **LOCAL_COMMITTED / REMOTE_NOT_PUSHED / GITHUB_WRITE_NOT_PERFORMED**. 초기 GitHub 읽기 전용 범위이므로 새 이슈/댓글/PR 생성0. #63/#64/#65 실제 아트 담당에게 새 모의 결과를 실제 QA로 인계하지 않았습니다.
