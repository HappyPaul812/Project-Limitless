# Issue #66 — 앱 ID 정정 완료 / 시험 token 로컬 입력 준비

대표의 최신 확인: **시험 `1564156403571105906` / 운영 `1563102982386749440`**. 서로 다른 앱입니다. 이전 동일 앱 HOLD는 용어 혼동에 따른 판정이므로 정정합니다. 새 시험 앱을 다시 만들 필요가 없습니다. 이전 기록은 역사로 보존하며 현재 판정으로 적용하지 않습니다.

기존 공개 운영 설정에서 대표 승인 User ID와 Guild `1563112744436244541`의 일치를 확인했습니다. 시험 채널은 `1564163234338967552`입니다. 운영 token·서비스 DPAPI 저장소는 읽지 않았습니다.

## 지금 필요한 대표의 로컬 입력

현재 Windows 계정의 **일반 PowerShell**에서 다음을 한 번 실행합니다. 다른 사용자로 실행하지 않습니다.

```powershell
pwsh -NoProfile -File "F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\setup-mobile-trial-token.ps1"
```

숨김 입력창에 **Limitless-test의 새 전용 Bot Token**을 직접 입력합니다. token을 ChatGPT/Discord/GitHub에 보내거나 명령 인자에 넣지 않습니다. 운영 앱 token은 사용하지 않습니다.

이 명령은 다음만 수행합니다.

1. 서로 다른 두 고정 앱 ID, 기존 boot approval·Manifest·보안 모듈 SHA, 기존 대표/guild 승인값을 확인합니다.
2. 새 시험 저장 루트가 없을 때만 token을 SecureString으로 받습니다. 기존 폴더/저장소가 있으면 덮어쓰기·ACL 수정 없이 HOLD합니다.
3. 입력 token으로 고정 `GET https://discord.com/api/v10/oauth2/applications/@me`를 호출부 기준 최대1회 실행하여 시험 application ID와 대표 소유권(개인 owner 또는 team owner)을 검증합니다. 명시적 재시도/redirect 없음. HTTP15초·본문 읽기15초/64KiB 제한. 원시 응답·오류·헤더/token 출력 없음.
4. 검증 성공 후 승인된 DiscordSecretStore의 동일한 DPAPI_CURRENT_USER 방식으로 새 `Temp/MobileTrialCredentials20261026/LimitlessTest/private/connection.json`에 암호화 저장합니다. 새 전용 폴더에만 기존 보안 방식의 현재 사용자 ACL을 적용하며 기존 서비스/운영 ACL은 변경하지 않습니다.
5. 같은 Windows 계정에서 암호화 저장소 읽기/ACL 확인 후 `TRIAL_TOKEN_READY_HOST_NOT_STARTED`를 출력합니다. token은 메모리와 암호화 저장소에서만 취급합니다. 새 결과 파일은 `Temp/MobileTrialCredentials20261026/LimitlessTest/setup-result.json`입니다.

**이 명령은 Host·봇 접속·명령 등록·모델·공식 사용량·운영 SQLite를 실행하지 않습니다.** token 검증 실패는 HOLD이며 자동 재실행하지 않습니다. 성공/실패 결과 JSON만 전달하시면 됩니다. token 자체는 보내지 않습니다. 기존 저장소를 삭제/초기화하지 않습니다.

## 현재 검증과 다음 단계

PowerShell 구문 PASS, 새로운 순수 함수 fixture8 PASS/0 FAIL(두 앱 분리/동일/잘못된 값·개인/팀 소유·잘못된 owner/앱/소유 미확인) 및 `-Action Inspect` 공개 사전 점검 PASS. token 입력/네트워크0으로 확인했습니다. 기존 Phase1/2/Phase3 전체 검증은 반복하지 않았습니다.

현재 상태는 **APP_IDS_DISTINCT / LOCAL_PREFLIGHT_PASS / WAIT_LOCAL_TRIAL_TOKEN**. 실제 API의 앱·소유권/서버·채널 권한은 아직 검증 전이며 token 저장도 미실행입니다. Host 진입점은 아직 구현·기동 완료가 아닙니다.

token 저장 결과 확인 후 기존 승인 범위에서 별도 시간 제한 Host·정확 SHA/단일 CREATE/반환 ID·만료/중지/단일 DELETE 경로를 준비하고 검증합니다. 그 뒤에만 연결 상태·모바일 첫 명령 `/limitless-trial status`·만료 시각과 종료 방법을 보고합니다. 지금은 모바일 명령이 등록되지 않았습니다.

기존 16파일 ZIP/Manifest·Phase2/3 source·boot approval 불변. 새 script만 추가했고 운영 bot/아트/부팅/운영 DB/CLI/CODEX_HOME/인증/ACL/Unity·게임·Save/Push/main/PC 변경0. MANUAL·A 미활성/실제 AI 금지 유지.

기존 앱 동일 판정의 Issue66 보고도 이번 두 ID로 정정합니다. 브랜치 `codex/discord-usage-diagnostics-20261026`; 구현/문서 commit은 Issue66 완료 보고에 기록합니다. 새 입력 스크립트는 기존 불변 ZIP에 섞지 않고 별도 SHA로 검토·보존합니다.

Bot application endpoint: [Discord official OAuth2 documentation](https://docs.discord.com/developers/topics/oauth2#get-current-bot-application-information); matches installed discord.py 2.7.1 HTTPClient.application_info.
