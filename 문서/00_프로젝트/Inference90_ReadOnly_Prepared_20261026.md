# Issue66 읽기 전용 실제 추론 실행 준비 (2026-10-26)

**READY_FOR_OWNER_MANUAL_LAUNCH / NOT_DEPLOYED / NOT_LAUNCHED / MANUAL / AI_EXECUTION_HOLD.** 바울 대표가 builtin 내부 HTTP/스트림 기본 재시도를 승인했습니다. 이번 요청 범위는 준비·오프라인 검증뿐이며 실제 모델/공식 사용량을 실행하지 않았습니다. 기존 자동 업무의 HOLD는 해제하지 않았습니다.

## 승인 정책과 새 요청

- LIMITLESS app retry0·Scheduler RestartCount0. 공식 builtin OpenAI/기존 ChatGPT 구독 인증·CLI 경로/SHA·CODEX_HOME 유지, custom provider/API/URL 전환0.
- 대표 추가 답변으로 **usage 전용 App Server 최대1개 + 추론 exec 최대1개**가 허용됨을 확인했습니다. 동시에 실행하지 않고 usage reader의 종료0·정리 확인 후에만 exec를 시작합니다. login/features 추가CLI는 없습니다. 실제 서버 요청·구독 사용량의 정확1회는 보장하지 않습니다. 내부 retry횟수가 확인되지 않으면 UNKNOWN입니다.
- 새 승인 요청 **`717a0afa735d485daf790579b964e8c2`**. prepared Manifest의 approved=false/expiry0, ProgramData 배포·활성permit·소비마커 모두 **없음**을 파일 상태로 확인했습니다. 이 작업에서 신규 슬롯 실행·소비0입니다.
- 이전 e0d2e6fb9a6e48708b35052c02ad133c 문서예약과 773d/987a 사용량 소비 증거를 재사용·초기화하지 않았습니다. 최종 시간 예산을 더 엄격히 조정하면서 같은 **미소비 새 request**를 유지했고, 초기 prepared Manifest/기록도 `.before-final-budget.json`으로 보존했습니다.

## 실행 경로와 시간 예산

관리자 직접 명령 → 새 전용 ProgramData trial 배포/SHA·권한·기존 runtime/MANUAL 사전검사 → 로컬 서비스 Windows 암호 입력 → Password/Limited 임시 작업 → 실제 서비스 SID/Session0/logon4 확인 → 새 Windows Job → Python 준비·공식 usage 사전1회 → 고정 요청의 exec 최대1회 → JSONL/최종 응답 검증 → Job 프로세스0 → permit 비활성/임시작업 제거/보호 확인.

기존 20초 `UsageOnlyJob.cs`는 **변경0**입니다. 별도 `InferenceJob90.cs`는 suspended 프로세스를 먼저 Job에 넣고 시작하며 kill-on-close를 유지합니다. 감독자 사망·timeout·루트 조기종료에도 해당 트리만 종료합니다.

| 시간 계층 | 최종 제약 |
| --- | --- |
| 관리자 실행 시계 | 로컬 암호 입력 완료 후 등록·실행·정리까지 측정. 대기 상한80초, 이후 정리 여유. 최종90초 이상이면 NOT_VERIFIED |
| 서비스 임시 작업 | ExecutionTimeLimit90초 / RestartCount0 / hard termination 허용 |
| 서비스 준비+Job | 준비 경과를 뺀 `min(75000,77000-경과ms)`; native 최후 guard85초. 남은 예산6초 미만이면 호출 전HOLD |
| Python 준비+usage+exec+로그 | `min(70000,Job예산-3000)`ms 전체 절대deadline. usage는 기존18초/정리예산을 포함하는 성공 reader 구조 재사용 |
| exec | 위 deadline의 남은 시간에서 보호로그3초 이상을 남김. 90초를 단계마다 새로 주지 않음 |

관리자 파일배포/보안 입력 대기시간은 비대화형 서비스 실행90초와 분리합니다. OS 스케줄링·권한 API·디스크 지연까지 벽시계90.000초 안에 끝난다고 보증하지 않습니다. 시간초과/정리 미확인은 NOT_VERIFIED이며 Scheduler90초와 Job이 자손 실행을 제한합니다. 이미 서버에 접수된 요청을 로컬 종료가 취소하거나 사용량을 회수하지는 못합니다.

## 고정 요청·도구/파일 차단·로그

최종 응답은 아래 문자열과 정확히 일치해야 합니다. CLI가 남기는 마지막 줄바꿈만 허용합니다.

> LIMITLESS 기획 담당 AI입니다. 읽기 전용 연결 시험에 성공했습니다.

stdin에는 이 문자열만 정확히 답변하도록 하는 고정 지시와 도구·파일·외부 시스템 접근 금지만 전달합니다. 기존 문서를 모델에 읽히지 않습니다. 기존 read-only Sandbox/approval never·shell/apps/browser/computer/multiagent/MCP/plugin 차단을 유지하고 view_image/hooks/memories도 끕니다. `--ignore-user-config`는 기존 인증 HOME을 바꾸지 않으며 지원되지 않는 `model_providers.openai` 설정은 없습니다. 빈 작업공간과 패키지는 서비스 ReadAndExecute, 새 Output만 Modify이며 기존 ACL을 건드리지 않습니다.

새 고정문장 검증기는 기존 JSONL 실행 검증의 실패/알 수 없는 이벤트/도구 항목 거부를 유지하고, 이번 plain-text 계약에 맞춰 최종파일·final 이벤트·고정문장을 대조합니다. thread/turn 시작·완료 각각1개, 종료0, 정상 토큰 사용량, 정확한 최종 응답이 모두 있어야 PASS입니다. 기존 role JSON 파서를 약화하거나 운영 파서를 수정하지 않았습니다.

Python stdout256KiB/stderr32KiB, supervisor stdout64KiB/stderr8KiB로 동시 drain하고 초과분은 버리며 성공은 차단합니다. 루트 timeout 후 파이프 join도 제한합니다. 원본 JSONL은 서비스 자기 프로필의 CURRENT_USER DPAPI, 정제stderr·구조화 error·stage·실제 exit·실행 메타데이터도 같은 보호 프로필에만 남깁니다. usage RPC 오류는 기존 ProtectedUsageLog로 보존합니다. 환경값·암호·토큰을 공개 로그에 복사하지 않습니다. 임시 최종응답 파일은 검증 후 제거합니다.

## 신규 오프라인 검증

**44 PASS / 0 FAIL**: Python36 + native Job7 + 준비/usage/추론/정리 예산1. 최종 PS 구문·비관리자 차단·패키지47개 SHA/집합 확인도 PASS입니다.

- Python: 정확문장/최종파일/완료턴·사용량, 잘못된 UTF8/중복JSON/다른문장/도구/실패/미지 이벤트/중복turn 차단, usage 오류·소진·오래된값 선행차단, timeout/대량출력/보호로그실패 재시도0, 재호출시 추가usage/exec0, 실제 파일 BOM 읽기, 정확44·미변경40/만료·무승인·request/root/entry/CLI SHA/정책확대 거부.
- Native: 최대허용예산 정상종료, 양쪽pipe 대량출력 상한, timeout 자손종료, 루트조기종료 자손정리, **감독 부모 강제종료 시 kill-on-close**, 예산guard, PS 구문/자동변수·switch충돌0.
- 예산: 준비20초 소모 후 Job57초/worker54초로 감소, 준비72초이면 시작차단. 테스트는 축소된 시간의 가짜 Python으로 종료 경로를 확인했습니다. 실제 서비스에서90초를 기다리는 시험이나 실제CLI 호출은 아닙니다.
- 개발 중 fixture의 긴 Windows 경로·제한 sandbox의 asyncio loopback socketpair 때문에 중단된 가짜시험은 경로 단축/즉시완료 fake coroutine으로 수정했습니다. BOM 인코딩 별칭 오류도 수정하고 실제 BOM 파일 테스트를 추가했습니다. 최종44건은 위 수정본PASS이며 이전의 중단/실패를 PASS에 포함하지 않았습니다.
- 기존22+12/환경/제품 대규모PASS는 재실행0. 공식CLI/공식usage/실제모델/Discord/운영SQLite 업무0. fake Python·가짜coroutine만 사용했습니다.

## 파일·SHA 및 패키지

아래7개는 모두 **신규**이며 운영 파일의 승인 SHA를 갱신하지 않았습니다.

| Tools/DiscordAIServiceDiagnostics 파일 | 변경 전 | 변경 후 SHA256 |
| --- | --- | --- |
| `inference-once-core.py` | 신규 | `c9fa6329849ed28713bbfb0fb2f7ded043a04c721c02c10c80fb71d5206adcdf` |
| `inference-once90.py` | 신규 | `14a502cd36ec2d54d6f381212bad8125aecc503f76bba1adf53d7f0d7f99a367` |
| `InferenceJob90.cs` | 신규 | `93a9cd26ee7bf268e9327cb10bed92cc27d273ffc697e9da5b679675cca846cf` |
| `prepare-inference-once90.py` | 신규 | `040a95d2a46e3d55752b1634114d895629e6b79cc267368ba6ff729142f9570e` |
| `run-approved-inference-once90.ps1` | 신규 | `552506b176e662e101b8c607c2d49cce486b07de7af184ff450a2e76d449640d` |
| `test_inference_job90.ps1` | 신규 | `05d00ce1b29037b2f520751bdf319cbb70d1617e01d900fb4f164268b7d2a037` |
| `test_inference_once90.py` | 신규 | `6040bbb25c002a3655e02e884d5cf2c7a2326c169f0bed0afabed371aff2c93d` |

ZIP SHA **`5019b4976051a00de1d657f2396762d2003cd4b901c4eec07fe1daaefcc4e477`**. 최종 Manifest SHA **`5607ae30a10c6adf2e27aeead75bdc509a3f5dd20c53767f7d3a984b86d64213`**. 패키지는 기존44/미변경40 바이트·roles1·새entry/core2=47파일입니다. 기존 package45/CLI·운영43 승인파일의 SHA도 준비단계에서 일치 확인했으며 운영배포·승인목록 확대는 없습니다. 상세 `Evidence/Inference90Prepared20261026/Preparation.json`, `Manifest.PREPARED_NOT_LAUNCHED.json`을 참조합니다.

## 바울 대표의 다음 직접 실행 명령

현재는 **실행하지 않고 준비 후 중지**합니다. 다음 단계에서 관리자 PowerShell을 직접 열어 아래 명령을 **한 번만** 실행하시면 됩니다. 암호는 로컬 보안 입력에만 입력하고 채팅/Discord에 보내지 않습니다. 관리자 권한·암호를 에이전트가 대신 취득하지 않습니다.

```powershell
pwsh -NoProfile -File "F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\run-approved-inference-once90.ps1"
```

이 명령은 별도 격리 시험만 배포하며 기존 운영 code/boot-approval/OnBoot/봇·아트/장부/MANUAL을 변경하지 않습니다. 고정 request/패키지·CLI/기존40 SHA/새 ACL/서비스 토큰·Session0/logon4가 어긋나면HOLD. 실제usage 성공 후에만 exec1회입니다. 실패해도 같은 명령을 다시 실행하지 마세요.

종료 시 신규permit을 trial의 비활성 보존본으로 옮기고 해당 임시 작업만 제거합니다. 소비마커·정제 진단·미시작/실행 결과는 보존합니다. 실제계정/Session·launch횟수·최종응답·JSONL/토큰·프로세스0·permit/task 정리·부팅/승인/장부 보존은 새 summary.json을 확인한 후에만 실전PASS로 판정합니다.

최종 결과 경로: `C:/ProgramData/LimitlessRelay/InferenceTrials/717a0afa735d485daf790579b964e8c2/Output/summary.json`. 상세 로그는 서비스 `.codex/limitless-diagnostics/<request>` 및 `.codex/limitless-inference-supervisor/<request>`에만 둡니다. 현재 이 trial은 존재하지 않습니다.

## 상태·인계

이번에는 기존운영/부팅/SQLite/MANUAL/CLI/CODEX_HOME/인증/ACL/Unity·게임·Save 변경0, 봇·아트 재시작0, A활성화0, Push/main병합/종료·재부팅0. 최신 운영프로세스 건강을 새로 측정한 것으로 주장하지 않습니다. 내부retry 정책승인과 실제모델시험 준비완료는 실전성공과 다릅니다.

브랜치 `codex/discord-usage-diagnostics-20261026`, 기반 `67669b9b69b297280381a258bb3c5a80ae427bcd`. 최종 로컬 commit은 Issue66/최종보고를 따릅니다. **LOCAL_COMMITTED / REMOTE_NOT_PUSHED / OWNER_MANUAL_LAUNCH_PENDING. 실제 모델을 실행하지 않고 중지합니다.**
