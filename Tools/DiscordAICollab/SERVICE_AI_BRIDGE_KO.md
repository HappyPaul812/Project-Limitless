# 서비스 계정 A 실행기 연결 준비

2026-10-24 KST. 현재 MANUAL 유지, 실제 AI 호출0. 기존 부팅/아트/SQLite를 재설치하지 않습니다.

## 연결 경로

기존 OnBoot → C: boot-service.ps1의 SID/무결성 검사 → 기존 boot_relay.Supervisor → register-discord.ps1 -BootRelay → 동일 byte lock을 확보한 run_bot → service_ai.connect_boot_ai → 서비스 계정 공식 읽기 검증 → 기존 UsageMonitor + ResumeQueue → 기존 real_roles.run_once입니다.

PlanningAgent·CodexAgent·NewArtAgent·RenewalArtAgent의 과거 VERIFIED 경로는 공용 run_once를 사용합니다. 과거 four_role_pilot 등의 승인 슬롯을 다시 실행하지 않습니다. 이번 연결은 읽기 전용 텍스트 검토만 지원하며 Unity/일반 개발/아트 생성 worker를 새로 연결하지 않습니다.

## 인증 성공과 실행 승인은 별개

- Windows 실제 토큰 SID가 기존 부팅 승인 service_sid와 일치하고 LIMITLESSRelaySvc 일반계정일 때만 공식 조회를 시작합니다.
- 서비스 계정 자신의 Windows 프로필과 .codex만 사용합니다. 상위 CODEX_HOME/HOME, API·Discord·GitHub 키를 하위 CLI에 전달하지 않습니다. 다른 사용자 비밀을 읽거나 복사하지 않습니다.
- 공식 app-server의 initialize/initialized/account/read(refreshToken=false)/account/rateLimits/read만 사용합니다. 로그인·토큰 발급·모델 turn·한도 리셋 API는 호출하지 않습니다. account.type=chatgpt가 아니면 HOLD입니다. [공식 App Server 문서](https://learn.chatgpt.com/docs/app-server)
- 인증 또는 사용량 형식/회복 시각이 불명확하면 중계는 relay_only로 유지하고 AI만 HOLD합니다. 인증 성공·한도 소진은 구분하며 소진 중에는 기존 usage gate가 재개를 차단합니다.
- 연결 성공은 MANUAL을 변경하지 않습니다. 대표가 Discord에서 명시적으로 A를 선택하고 정확한 미소비 업무 승인이 있어야 ResumeQueue가 실행합니다.
- 실제 서비스 프로세스는 재시작하지 않았습니다. 새 연결은 다음 정상 시작에 로드되며 현재 실행 중인 Bot은 기존 AI HOLD 상태입니다.

## 서비스 계정에서 실제 읽기 점검

현재 Codex 실행 계정은 windows이며 초기 독립 프로브는 SERVICE_ACCOUNT_CONTEXT_REQUIRED로 HOLD했습니다. 이후 바울 대표가 관리자 pwsh -File 명령으로 실제 서비스 계정 단발 점검을 완료했습니다. 서비스 SID/일반계정/자기 프로필 검사 후 SERVICE_CODEX_PROFILE_NOT_INITIALIZED로 HOLD했습니다. 서비스 계정의 초기화된 .codex 프로필을 확인하지 못해 공식 인증/사용량 RPC를 시작하지 않았으며, 구독 인증 성공이나 실제 회복 시각은 아직 미확인입니다. 임의 설치·로그인·다른 계정 비밀 복사 없이 AI HOLD를 유지합니다.

다음은 바울 대표가 관리자 PowerShell에서 직접 실행하는 **단발 읽기 점검**입니다. 서비스 계정 암호는 로컬 Windows 입력창에만 입력하고 채팅에 보내지 마세요.

```powershell
pwsh -NoProfile -File "F:\study\codex\Project-Limitless\Temp\DiscordAICollab20261023\Worktree\Tools\DiscordAICollab\probe-service-ai.ps1" -Action RunOnceAsService
```

이 도구는 기존 OnBoot2종을 수정하지 않고 LIMITLESS-AI-Readiness-Once 임시 점검 작업만 만듭니다. Password/Limited·트리거없음·한번실행이며 기존 Bot/Art는 시작하지 않습니다. 결과 nonce를 확인하고 완료된 점검 작업만 삭제합니다. 실행 중이면 강제 종료/삭제하지 않습니다. 결과는 data/boot/service-ai-probe-<nonce>.json에 비밀 없이 저장합니다. 읽기 점검이 성공해도 A 활성화·업무 승인·재시작은 수행하지 않습니다.

HOLD_SERVICE_CODEX_PROFILE_NOT_INITIALIZED / SERVICE_CODEX_CLI_NOT_FOUND / CHATGPT_SUBSCRIPTION_REQUIRED / 공식조회 실패가 나오면 AI HOLD를 유지합니다. 이 도구는 설치·로그인·다른 계정 인증 복사로 복구하지 않습니다. 별도 로컬 설정이 필요하면 그 조건을 보고합니다.

## 사용량·체크포인트 복원

5시간·주간 양쪽 창이 소진되면 늦은 회복 시각을 기다리고, 시각 경과 후 새 공식 관측을 요구합니다. 조회 실패는 UNKNOWN·3회 후 회로차단, 300초 지난 관측도 실행 근거로 쓰지 않습니다. 현재 공유DB 관측은 과거 다른 실행 문맥의 기록이므로 서비스 계정의 현재 사용량이나 회복 시각으로 간주하지 않습니다.

재시작 시 기존 DB와 operation_checkpoints를 유지합니다. control_resume의 RUNNING은 NOT_VERIFIED/자동재시도 금지, HELD는 정확한 승인·정본SHA·실행파일SHA·사용량·동시 제한을 다시 검사합니다. 주일에도 동일 검사 후 허용합니다. 승인 유효기간600초는 늘리지 않았으므로 장시간 소진 후 기존 승인이 만료되면 재승인이 필요합니다.

## 첫 읽기 전용 1건의 준비 순서

1. 실제 서비스 계정 프로브에서 READONLY_LINK_READY와 양쪽 창의 공식 관측 확인.
2. 기존 실행기를 정상 재시작하여 service-ai-readiness.json의 결과와 동일 Bot/Art lock·heartbeat 확인. 기존 OnBoot 경로를 유지하고 병행 Bot을 새로 띄우지 않음.
3. 최신 정본 SHA로 PlanningAgent 단일 읽기 검토 계획을 준비. 과거 소진 슬롯 재사용 금지, 새 정확한 계획/슬롯을 별도 검토·승인. 현재 새 슬롯 추가/승인/큐 삽입은 하지 않음.
4. 바울 대표의 명시적 A 활성화와 해당 1건의 정확한 실행 승인 후에만 기존 run_once로 90초·1회·도구없음·유료전환없음 검증. 현재 호출 승인 없음.
5. Discord 요청→큐 선점→구독 실행→결과 검증→사용량/업무 장부→Discord 영수증까지 확인하고 반복 실행 없이 종료.

실제 A 활성화 전 남은 조건: 실제 서비스 계정 인증/공식 두 창 조회, 새 코드의 정상 로드·동일서비스 확인, 새 단일 계획 승인, 대표 A 활성화, 이후 승인1건 실증입니다. 실제 PIN전 부팅·USB Wi-Fi 단절/복구 실증은 기존 미검증 경계를 유지합니다. 게임·Save·미술 최종 승인·Push/main 병합 보호는 동일합니다.


서비스 계정 프로필/CLI 접근·공식 기기 인증 초기화의 최신 절차는 [SERVICE_CODEX_AUTH_KO.md](SERVICE_CODEX_AUTH_KO.md)를 따릅니다. 초기 .codex 부재를 서비스 계정의 공식 인증 미지원으로 단정하지 않습니다. 2026-10-24 실제 서비스 콘솔에서 CLI/device-auth 지원까지 확인했고 대표 직접 로그인을 기다립니다.
