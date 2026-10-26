# Issue #66 Host ↔ 고정 Worker 최종 접합·설치 전 준비

**CODE_JOIN_COMPLETE / OFFLINE_VERIFIED / NOT_INSTALLED.** 기존에 남았던 Host와 고정 Worker 접합 코드를 완성했습니다. 서로 다른 실제 Windows 계정 간 IPC, 계정 생성/설치, 실제 Discord→AI는 아직 실행하지 않았습니다. 실전 성공을 뜻하지 않습니다.

브랜치 `codex/discord-usage-diagnostics-20261026`, 기준 `ef1dec16bf285024c9caa733845ff93985e07d7f`. 마지막 관련 로컬 commit은 CURRENT_STATUS/인계 및 Issue #66 완료 댓글에 기록합니다. Push/main 병합 없음.

## 완료된 실행 경로

`5버튼 callback → LiveSession0Bridge → Windows 인증 pipe → C# Session0Host → 익명 stdin Windows Job → broker-host-entry/HostRuntime → 서비스 단발 장부 → FixedPlanningWorker → 고정 PowerShell wrapper → 취소 가능한 Job → fixed-planning-entry → 기존 frozen inference-once90.run → strict JSONL → 안전한 pipe 응답 → 단발 Discord 보고`.

- Windows가 생성한 실제 Peer만 C#에서 수집하고, 네트워크 입력의 SID/admin 필드를 권한으로 사용하지 않습니다. Python으로 넘기는 Peer는 Host가 생성한 익명 stdin 안에만 담습니다. 직접 entry를 실행해도 현재 토큰 SID·Session0·LSA logon4·비관리자를 다시 검사합니다. 합성 security-context를 지정하는 운영 옵션은 없습니다.
- BOT/ADMIN/WORKER 세 SID 분리, 관리자 상승 토큰·그룹·integrity 및 정확한 사전 arm, 대표/guild/channel/request/task SHA/ticket SHA·범위·만료를 검사합니다. durable consume는 Worker 준비·usage보다 먼저 확정합니다. RPC별 process는 장부를 복구하지 않고 Host startup에서만 미완료/미사용 arm을 폐기합니다. 기존 소비는 유지합니다.
- wire에서 exe/argv/prompt/파일 경로를 받지 않습니다. 고정 Planning 한국어 한 문장만 기존 engine에 요청합니다. 기존 ChatGPT keyring·builtin OpenAI·읽기 전용 Sandbox·43→44/불변40 SHA 검증·JSONL 검증을 유지합니다.
- frozen entry 파일을 바꾸지 않았습니다. sidecar가 **서비스 자기 시험 폴더의 단발 permit 위치만** 주입하고, 기존 entry의 필드·정책·CLI·SHA·인증 검증을 그대로 실행합니다. 운영 boot approval이나 기존 global permit을 쓰거나 덮어쓰지 않습니다. 새 permit은 소비된 서비스 장부와 정확한 ticket을 확인한 뒤 만들고 종료 시 비활성화합니다.
- 신규 broker Code와 기존 frozen47 Legacy package를 별도 디렉터리에 두어 기존 exact44 collab 파일 목록을 오염시키지 않습니다. 준비물은 공개 코드/독립 Python 표준 런타임뿐이며 인증·토큰·프로필·운영 DB를 복사하지 않았습니다.
- ticket 생성부터 전달·준비·usage·추론·종료까지 최대90초입니다. 남은 시간을 차감하고 외부 Gateway Job은 최대85초, 내부 Worker/engine도 남은 예산으로 제한합니다. root Job의 kill-on-close와 중첩 Job으로 Host/중개기 종료 시 자손을 정리합니다. 취소는 별도 control pipe와 Worker stdin EOF로 전달합니다. 결과 불명·오류·재시작 후 재실행0입니다.
- callback은 이벤트 루프를 막지 않고 bridge를 호출합니다. 같은 callback을 다시 불러도 재전달하지 않으며, 서비스 소비 장부가 재시작 이후도 차단합니다. Discord 전송 실패는 UNKNOWN_NO_RETRY/체크포인트로 남기고 재추론하지 않습니다.

## 보안 경계와 CLI 신뢰

서비스 계정이 만든 새 Host 프로세스와 그 토큰에만 새 봇 SID의 `PROCESS_QUERY_LIMITED_INFORMATION`/`TOKEN_QUERY`를 부여하는 코드가 있습니다. 이는 pipe 서버 SID/Session 확인용이며 메모리 접근·토큰 복제·쓰기·임의 실행 권한을 주지 않습니다. 기존 프로세스/서비스 계정/인증 파일 ACL은 변경하지 않습니다. 이 동작 자체도 아직 실제 서비스 계정에서 실행하지 않았습니다. 실패하면 HOLD하며 권한을 넓혀 우회하지 않습니다.

관리자 SID와 소유자가 같다는 이유로 사용자-writable 실행 파일을 신뢰하지 않습니다. 실제 모델 모드는 CLI 파일·의존 파일·경로에 대해 SYSTEM/Administrators/TrustedInstaller 이외 writer/교체 권한을 거부합니다. 일반 사용자 SID 소유 코드가 이 검사에서 거부되는 것을 새로 확인했습니다. 현재 운영 CLI 경로·파일·ACL은 바꾸지 않았습니다. 실제 모델 모드에서 기존 CLI 신뢰 조건을 충족하지 못하면 HOLD하며, 보호된 격리 복사본 등은 별도 명시적 승인 후 다뤄야 합니다.

**FAKE_ONLY는 기존 CLI를 import/실행하거나 usage를 조회하지 않으므로 이 실제 CLI 실행 게이트와 분리했습니다.** Fake IPC 설치·시험이 곧 모델 실행 허가가 되지 않습니다. 설치기는 FAKE_ONLY만 만들며 실제 모델 모드 활성화 기능이 없습니다.

설치기 자체도 코드·디렉터리를 처음 생성하는 순간부터 BA(Administrators) 소유로 게시합니다. 사용자 소유 파일을 만든 후 권한을 고치는 틈을 없앴고, 검증한 같은 bytes만 새 위치에 씁니다. 관리자 실행은 아래처럼 고정 SHA를 확인한 메모리 scriptblock으로 시작합니다. 실제 관리자 게시/계정 생성은 미실행이며, 게시기의 컴파일과 비관리자 생성 거부만 확인했습니다.

## 검증 결과·재개 범위

**접합 관련 신규29 PASS / 0 FAIL**:

- 한도 중단 전 완료한 HostRuntime→FixedPlanningWorker→sidecar 합성 통합17, 익명 stdin/동시 Job/취소/권한 guard 네이티브5를 보존했습니다. 재개 후 이 성공 시험은 반복하지 않았습니다.
- 재개 후 미실행 callback 수정에 대한 시험4, 사용자 소유 실행 코드 거부1, 원자적 게시기 compile/비관리자 거부2를 추가했습니다.
- 실제 추론 분기는 가짜 engine loader로 검증했으며 실제 model/usage는 호출하지 않았습니다. 실제 계정 IPC 성공으로 확대하지 않습니다. FAKE_ONLY 분기는 engine import 없이 NOT_VERIFIED/HOLD를 반환하고, 보호된 safe-result의 `FAKE_FIXED_WORKER_COMPLETE`로 경로 도달을 확인합니다. 가짜 성공을 실제 VERIFIED로 발급하지 않습니다.

기초 패키지 검사4 PASS(독립 런타임 import, frozen worker import-only, SHA, 설치기 Plan)을 재사용했습니다. 마지막 수정 후에는 최종855파일 SHA·허용된 변경5파일·동일 Runtime/Legacy 무결성3항목만 확인했습니다. 기존 Phase1/웹 Fake4/35/42 전체 성공 검사 반복0입니다.

현재 Codex 제한 토큰의 LSA 읽기는 `HOST_LOGON_QUERY_HOLD_C0000022`로 차단됐습니다. 권한을 바꾸거나 재실행하지 않았으며, 이를 기존 서비스 계정의 실패로 단정하지 않습니다. 실제 계정 간 토큰/DACL/LSA/IPC의 긍정 검증은 설치 후 fake-only 단계에 남깁니다.

실제 모델0·공식 usage0·실제 Discord 연결/등록/게시0·운영 SQLite 업무0·새 계정/서비스/task 생성0·실제 권한/자동 시작 변경0·인증/토큰 복사0. MANUAL/A 미활성, 기존 봇·아트·OnBoot2개·Unity/게임/Save/Push/main/PC 보호. 운영 건강/동적 DB를 새로 검증한 것은 아닙니다.

## 최종 준비물

최종 후보만 사용합니다. 앞선 draft/V1/V2/V3/V4 폴더는 설치 대상이 아닙니다.

`F:\study\codex\Project-Limitless\Temp\Session0HostBundle20261026Ready`

- HostManifest SHA256: `9aa4a60d6dd57c320f2d268ef96dcf86694e53ca9dfdcbb6a3e97d1f33dfe7ec`
- Deployment/install-session0-trial.ps1 SHA256: `337ad613ef6471ae6f4058043965bb3b812a0e6b31adc05ce1efb61ce0158829`
- 공개 코드/독립 런타임/frozen Legacy 총855파일. 운영 토큰/인증/DB 없음.
- 코드별 전후 SHA와 시험 원문: `팀운영/Evidence/Session0HostWorkerJoined20261026/`.

## 별도 승인 후에만 실행할 최소 설치·확인 명령

**지금 실행하지 않습니다.** 아래는 대표가 계정·설치 변경을 별도 승인한 다음 새 관리자 PowerShell 7(x64)에서 직접 수행할 절차입니다. PIN/자동 로그인 변경이나 기존 OnBoot 수정은 없습니다.

```powershell
$trialBundle = 'F:\study\codex\Project-Limitless\Temp\Session0HostBundle20261026Ready'
$trialBytes = [IO.File]::ReadAllBytes((Join-Path $trialBundle 'Deployment\install-session0-trial.ps1'))
$trialSha = [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($trialBytes)).ToLower()
if ($trialSha -cne '337ad613ef6471ae6f4058043965bb3b812a0e6b31adc05ce1efb61ce0158829') { throw 'INSTALLER_SHA_HOLD' }
$trialInstaller = [scriptblock]::Create([Text.Encoding]::UTF8.GetString($trialBytes))
& $trialInstaller -Action Install -Bundle $trialBundle -BundleManifestSha '9aa4a60d6dd57c320f2d268ef96dcf86694e53ca9dfdcbb6a3e97d1f33dfe7ec' -ConfirmSystemChanges
```

이 명령은 새 `PAUL-PC\LIMITLESSBotTrial`을 Users 권한으로 만들고 새 계정에만 Batch logon 권한을 부여합니다. 24시간 만료 시험 계정이며 Administrators/RDP 권한을 추가하지 않습니다. 새 고정 작업 `LIMITLESS-Trial-S0Host`를 기존 Worker SID/Password logon/Limited/IgnoreNew/재시작0/최대15분/**비활성·trigger 없음**으로 등록합니다. 기존 Worker의 암호는 대표의 로컬 Get-Credential 입력만 사용하고 Windows Scheduler 외에 저장하지 않습니다. 기존 인증/암호/토큰 저장소를 읽어 재사용하지 않습니다.

기존 이름/설치/작업이 있으면 덮어쓰지 않고 HOLD합니다. 새 작업의 실행/수정 권한은 관리자/SYSTEM, Worker는 조회만 허용합니다. Code는 관리자 소유·Worker/Bot 읽기 실행, 서비스 상태 폴더는 Worker 수정·Bot 접근 없음입니다.

설치 성공 후 읽기 전용 확인:

```powershell
& 'C:\Program Files\LimitlessRelayTrial\Deployment\install-session0-trial.ps1' -Action Verify
```

세 SID 분리, Code/상태 ACL, 새 작업 Disabled/trigger0/FAKE_ONLY를 확인합니다. 실제 계정 생성·모듈 가용성·TaskScheduler/LSA 권한·보안 descriptor 적용은 아직 현장 미검증이므로 실패 시 HOLD하고 자동 재설치하지 않습니다. 부분 설치 실패 시 새 계정은 비활성화하며, 부분 설치 자산을 확인한 뒤 별도 복구합니다.

## 설치 후 별도 승인된 fake-only 계정 IPC 확인

이 단계도 이번에는 실행하지 않았습니다. 모델·usage·Discord·토큰이 필요 없습니다.

관리자:

```powershell
& 'C:\Program Files\LimitlessRelayTrial\Deployment\install-session0-trial.ps1' -Action StartFake -ConfirmSystemChanges
runas.exe /profile /user:PAUL-PC\LIMITLESSBotTrial powershell.exe
```

새 봇 콘솔에서(신규 계정의 암호는 로컬 입력만):

```powershell
& 'C:\Program Files\LimitlessRelayTrial\Runtime\python.exe' -I -B -X utf8 'C:\Program Files\LimitlessRelayTrial\Code\Tools\DiscordAIServiceDiagnostics\probe-session0-host.py' --action prepare --plan "$env:USERPROFILE\limitless-fake-plan.json"
```

관리자 콘솔에서 정확한 새 plan에 사전 arm(파일은 요청 데이터이며 그 파일만으로 실행 권한을 얻지 않습니다. 실제 관리자 토큰의 pipe 승인이 추가로 필요합니다):

```powershell
& 'C:\Program Files\LimitlessRelayTrial\Runtime\python.exe' -I -B -X utf8 'C:\Program Files\LimitlessRelayTrial\Code\Tools\DiscordAIServiceDiagnostics\probe-session0-host.py' --action arm --plan 'C:\Users\LIMITLESSBotTrial\limitless-fake-plan.json'
```

다시 봇 콘솔에서:

```powershell
& 'C:\Program Files\LimitlessRelayTrial\Runtime\python.exe' -I -B -X utf8 'C:\Program Files\LimitlessRelayTrial\Code\Tools\DiscordAIServiceDiagnostics\probe-session0-host.py' --action submit --plan "$env:USERPROFILE\limitless-fake-plan.json"
```

계정 프로필 경로가 다르면 prepare에서 사용한 실제 새 계정 경로를 사용합니다. 응답 HOLD만으로 fake 성공이라고 판단하지 않습니다. 관리자가 새 서비스 상태 `Workers/<request>/safe-result.json`의 `FAKE_FIXED_WORKER_COMPLETE`, model0/usage0, 소비1, Job/작업 종료0을 대조해야 fake IPC PASS입니다. 실제 VERIFIED는 이 단계에서 나오면 안 됩니다. 실패하면 반복 호출하지 않습니다.

## 계정별 토큰·PIN 없는 부팅·USB 무선랜

- 기존 시험/운영 토큰이나 DPAPI 파일을 새 SID로 복사·이동·재암호화하지 않습니다. 향후 Discord 시험 승인 후 대표가 Limitless-test 전용 새 토큰을 Portal에서 준비하고 **새 봇 콘솔**에서 다음 로컬 입력만 합니다. 운영 앱 토큰은 사용하지 않습니다.

```powershell
& 'C:\Program Files\PowerShell\7\pwsh.exe' -NoProfile -File 'C:\Program Files\LimitlessRelayTrial\Code\Tools\DiscordAIServiceDiagnostics\setup-trial-bot-own-token.ps1' -ConfirmLocalTokenInput
```

자기 LOCALAPPDATA의 새 전용 저장소에 CurrentUser DPAPI로 저장하고 자기 읽기만 검사합니다. token을 출력하지 않고 API 호출0이며, app ID/소유권/서버 권한은 이후 승인된 실제 Discord 접속 전 별도로 확인합니다. 이 입력 명령도 이번에 실행하지 않았습니다.

- Password/배치 logon4의 별도 신규 작업은 사용자 PIN 입력 없이 부팅 후 실행하는 구성에 사용할 수 있습니다. 하지만 현재 후보는 자동 시작을 등록하지 않았습니다. 향후 별도 승인으로 **새 시험 작업에만** OnBoot 지연·네트워크 대기를 추가하며 기존 두 작업은 보존합니다. 현재 Host는15분 한시 시험용이며 장기 A 운영용으로 승인된 것이 아닙니다.
- USB 무선랜 지연 시 Host의 로컬 IPC 준비와 인터넷 준비를 분리합니다. 봇은 네트워크가 준비되기 전 신규 업무/승인 소비를 하지 않고 최대10분, 2→5→10→20→30초 상한 간격으로 기다리는 설계를 사용합니다. 이는 모델 재시도가 아닙니다. 해당 자동 시작/연결 루프는 이번에 활성화하지 않았습니다.
- 수락·소비 뒤 네트워크/usage/추론이 실패하면 HOLD와 체크포인트를 보존합니다. 링크가 회복돼도 소비한 업무를 자동 재실행하지 않습니다. A 모드나 회복 후 자동 AI 재개는 별도 승인/검증 대상입니다.

## 종료·되돌리기

별도 승인된 시험 종료 시 관리자:

```powershell
& 'C:\Program Files\LimitlessRelayTrial\Deployment\install-session0-trial.ps1' -Action Stop -ConfirmSystemChanges
# 새 시험 작업 제거와 새 계정 비활성화까지 승인한 경우만:
& 'C:\Program Files\LimitlessRelayTrial\Deployment\install-session0-trial.ps1' -Action Remove -ConfirmSystemChanges
```

새 작업만 종료/비활성화하고, 기록된 Host PID와 자손을 대조해 남은 프로세스가 있으면 HOLD합니다. 이미 부모가 끝났어도 기록된 PID의 자손을 확인합니다. 알려진 PID가 다른 명령으로 재사용됐거나 CIM 조회가 불가능하면 정리 PASS로 표시하지 않습니다. Remove는 새 작업만 해제하고 기록 SID가 맞는 새 봇 계정만 비활성화합니다. 기존 서비스/봇/OnBoot/인증은 건드리지 않습니다.

계정 프로필·Code·소비 장부·진단은 삭제하지 않습니다. 완전한 파일/계정 삭제는 보존 증거 검토 후 별도 작업입니다. 현재 실제 설치0이므로 운영 되돌리기 작업도0입니다.

## 실제 Discord → AI 1건까지 남은 조건

1. 대표의 별도 계정/설치 승인 및 위 실제 fake-only SID/IPC/종료 검증 PASS.
2. 기존 CLI가 사용자-writable이면 실제 모델 모드는 HOLD. 기존 경로/ACL을 몰래 바꾸지 않고 보호된 격리 CLI 등 필요한 변경을 별도로 승인받아야 합니다.
3. 새 봇 자기 token/시험 앱·대표/guild/channel 권한 및 시험 SDK Host 배포 범위 확인. 실제 앱 명령 단일 CREATE/정확 ID 삭제와 별도 모델 실행 승인 필요.
4. Fake 소비/기존 소비를 초기화하지 않습니다. 실제 시험은 새 request·새 보호 state epoch/장부·새 manifest/task SHA·정확한 사전 arm을 승인받아 준비합니다. 이번 설치기는 실제 모델 모드를 활성화하지 않습니다.
5. 허용 작업은 PlanningAgent 고정 읽기 전용 한 문장1건, exec1/App Server1, 자동 재실행0, 로컬90초, 공식 내부 HTTP/stream 기본 재시도만 허용합니다. 원격 요청 정확1회는 보장하지 않습니다. 실제 4역할/A 활성화는 포함하지 않습니다.

참고: [Windows scheduled task 설정](https://learn.microsoft.com/en-us/powershell/module/scheduledtasks/new-scheduledtasksettingsset), [Windows 사용자 보안 경계](https://www.microsoft.com/en-us/msrc/windows-security-servicing-criteria), [named pipe 권한](https://learn.microsoft.com/en-us/windows/win32/ipc/named-pipe-security-and-access-rights).

구현 로컬 commit: `5f1dad2284d7c2cf78c4d80d451555c3619eb60a`. 문서 마감 실제 commit은 Issue #66 완료 댓글에 기록합니다. Push 없음.
