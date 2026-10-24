> **최신 후속 상태 — 2026-10-24 09:10 KST:** 아래 등록 전 내용은 준비 당시 기록입니다. 대표의 관리자 등록 완료 및 `verify-boot-tasks.ps1 -StartTest` 결과 `PASS_NONINTERACTIVE_BATCH_ONLY`를 확인했습니다. 등록2/2, 활성 일반계정·관리자 아님·Password/Limited·BootTrigger는 관리자 결과 파일 근거입니다. 현재 일반권한의 직접 태스크 조회는 접근 거부이므로 등록 결과 파일과 현재 서비스 상태를 구분합니다. 후속 읽기 조회에서 Bot/Art RUNNING 및 heartbeat 3초 이내, MANUAL·재개큐0·업무21·usage13 확인. 추가 서비스 재시작/AI/Unity/재부팅/Push/merge는 수행하지 않았습니다. 실제 재부팅 후 PIN 전 기동·USB Wi-Fi 재연결은 NOT_VERIFIED입니다. 이는 비대화형 수동 시작 시험 PASS와 별개이며 별도 재부팅 승인 시 확인합니다. 근거: `../Evidence/BootRelay20261024/resume-verification.json`. 마지막 관련 기존 commit `c8965d33`.

# PIN 입력 없는 부팅 중계 — 등록 전 승인안 / 2026-10-24 KST

**준비 코드·비활성 Boot XML 완료, 실제 등록·계정 생성·부팅 실증 전입니다.**
브랜치 `codex/discord-ai-collaboration`, 구현 `be274f93842ac726f959afbe50d6f6b1bddd70eb`.
기존 실행 중인 Bot/Art·MANUAL·원본을 유지했습니다. 준비 코드는 아직 기존 실행 프로세스에 로드하지 않았습니다.

## 선택한 방식과 등록 대상

| 항목 | 등록 승인안 |
|---|---|
| 작업 이름 | LIMITLESS-Bot-OnBoot / LIMITLESS-Art-OnBoot |
| 트리거 | 시스템 시작 +30초, 네트워크 사용 가능 조건 |
| 실행 계정 | 별도 로컬 일반 계정 LIMITLESSRelaySvc, Administrators 가입 금지 |
| 로그온 방식 | Password / 사용자 로그인 여부와 관계없이 실행 |
| 권한 | LeastPrivilege, Log on as a batch job |
| 부팅 진입점 | C:\ProgramData\LimitlessRelay\boot-service.ps1 |
| 중계·DB·납품 경로 | 기존 격리 Tools/DiscordAICollab 및 단일 SQLite 유지 |
| 중복 방지 | IgnoreNew + 감독기별 lock + 기존 Bot/Art byte lock |
| 재시작 | OS 최대3회/1분; 인증/권한/설정 HOLD는 정상 종료해 폭주 방지 |
| 실행 모드 | MANUAL, AI HOLD, 실제 종료·Unity Worker·Push 없음 |
| 현재 상태 | 계정 미생성·새 계정 토큰 미설정·C: 배포0·OS 등록0·XML enabled=false |

기존 OnLogon 승인안은 보관만 하며 새 운영의 기본안은 OnBoot입니다. 두 방식을 동시에 활성화하지 않습니다. 현재 OnLogon 등록도0입니다.

Windows 로그인/PIN/암호·자동 로그인을 변경하는 코드가 없습니다. 서비스 계정의 별도 암호는 향후 대표가 Windows 등록 화면에서 입력하며 채팅·XML·스크립트·Git·로그에 평문으로 저장하지 않습니다. 기존 대표 Windows 로그인 암호나 PIN을 받을 필요가 없습니다. 시스템 시작 트리거를 사용하므로 Windows 서비스 래퍼/SYSTEM 권한을 추가하지 않습니다.

Windows 공식 근거: [로그온 종류](https://learn.microsoft.com/en-us/windows/win32/api/taskschd/ne-taskschd-task_logon_type), [BootTrigger](https://learn.microsoft.com/en-us/windows/win32/taskschd/boot-trigger-example--scripting-), [배치 로그온 권한](https://learn.microsoft.com/en-us/previous-versions/windows/it-pro/windows-10/security/threat-protection/security-policy-settings/log-on-as-a-batch-job).
InteractiveToken은 이미 로그인한 세션이 필요합니다. S4U는 네트워크/암호화 파일 제약 때문에 선택하지 않았습니다.

## 구현한 안전 경로

- `boot-service.ps1`: Preview/Inspect/StartIfApproved. 계정 생성·태스크 등록 기능은 없습니다. C:의 별도 승인 파일이 없으면 즉시 HOLD합니다. 승인 파일은 서비스 계정 소유/쓰기 가능 상태를 거부하고 보호 ACL·서비스 SID·일반 계정·배포 SHA를 확인합니다.
- `collab/boot_relay.py`: Bot/Art 고정 실행만 허용. 사용자 정의 명령은 받지 않습니다. 기존 사용자 CODEX_HOME/API 키를 자식 환경으로 전달하지 않습니다. 하위 stdout/stderr 원문은 버리고 상태 코드만 기록합니다.
- 기존 wrapper에 `-BootRelay` 선택 경로만 추가. 기본 로그인 경로는 보존합니다. 부팅 경로는 `data/private/boot-discord`를 사용하며 기존 `data/private/discord`를 읽거나 복사하지 않습니다.
- 부팅 Bot은 UsageMonitor와 실제 AI 재개 실행기를 연결하지 않습니다. /usage는 서비스 계정 인증 미검증·AI HOLD로 응답하며 /resume 및 A/B 전환을 거부합니다. 기존 상태가 MANUAL이 아니면 강제로 바꾸지 않고 시작을 보류합니다.
- 부팅 Gateway는 SDK reconnect를 켜고, Art REST 폴링 실패는 부팅 프로필에서만 최대120초 백오프합니다. 초기/종료 후 연결 상태는 공개 Discord HTTPS로 재확인합니다. 인증·런타임 오류로 보이면 무작정 재실행하지 않습니다.
- C: bootstrap은 F: 실행 경로를 최대12×30초 기다립니다. 네트워크는 5~120초 간격 최대12회 기다리고, 종료 후 네트워크 손실이 확인된 자식 재시작은 최대3회입니다. 초과하면 HOLD·운영자 확인입니다. 자동 재부팅이나 AI 반복 호출은 없습니다.
- SQLite·예약·납품 SHA/커서·기존 lock을 그대로 사용합니다. 새 DB를 별도로 만들어 두 큐가 갈라지는 방식은 사용하지 않습니다. 끊긴 AI 실행은 NOT_VERIFIED로 보존하며 재시도하지 않습니다. 중지는 기존 안전 stop marker로 요청하고 강제 종료하지 않습니다.

기존 token은 사용자 DPAPI에 묶여 있으므로 다른 계정에서 복호화된다고 가정하지 않습니다. [DPAPI 공식 문서](https://learn.microsoft.com/en-us/windows/win32/api/dpapi/nf-dpapi-cryptprotectdata). 새 계정 자신의 사용자 범위 DPAPI·전용 ACL로 토큰을 별도 등록해야 합니다. Machine-scope 암호화로 바꾸지 않습니다.

## 필요한 권한·설정 — 실제 등록 전에 승인 필요

1. 로컬 일반 계정 생성 및 전용 프로필 초기화, 배치 로그온 권한 확인. 관리자/SYSTEM 실행·PC 종료 권한은 주지 않습니다.
2. C: bootstrap/승인 파일을 대표 또는 관리자가 관리하고 서비스 계정에는 읽기/실행만 부여. 서비스 계정이 승인 파일을 생성·변경·소유하지 못하게 합니다. 별도 상태/로그 경로만 쓰기 허용합니다.
3. Tools 코드·Python 실행 환경은 선별 읽기/실행 권한, `data/phase2`, `data/phase3`, `data/boot`, 납품함·검역·lock 경로에 필요한 수정 권한만 부여합니다. Unity/Assets/Save/UserData/승인 아트·원본 Git에는 쓰기 권한을 추가하지 않습니다. 상위 폴더의 광범위 재귀 ACL 변경은 금지합니다.
4. 기존 사용자 `data/private/discord`, `.codex` 자격 증명은 서비스 계정에 열어주지 않습니다. 새 `boot-discord` 폴더는 서비스 계정만 소유·접근하도록 구성하고 대표가 해당 계정 문맥에서 토큰을 숨김 입력합니다. 기존 토큰의 복호화/복사/회전은 이번에 수행하지 않았습니다. 서버/대표/채널 ID는 기존 운영 계약과 일치해야 합니다.
5. 기존 감시기를 안전 중지한 뒤 같은 DB/lock에 대해 서비스 계정의 읽기·쓰기·중복 차단을 시험합니다. 실제 파일·원본을 손상시키지 않는 TEST_ONLY 사본 및 SQLite 백업으로 확인해야 합니다. 실패하면 기존 사용자 서비스로 복귀하며 양쪽을 동시에 가동하지 않습니다.
6. USB WLAN의 시스템 시작·모든 사용자 연결 가능 프로필과 F:/Python 런타임을 배치 계정에서 확인해야 합니다. Wi-Fi 암호 표시/프로필 키 내보내기·PIN 제거·자동 로그인은 하지 않습니다.
7. 배포 SHA 40개 및 계정 SID를 고정한 승인 파일을 준비하고 Windows 등록 화면으로 두 작업을 등록합니다. 먼저 비대화형 수동 시험을 별도 승인받아 수행한 뒤 부팅 트리거를 활성화합니다. 실제 재부팅 검증은 다시 별도 승인입니다.

미등록 XML 실제 경로:
- `Tools/DiscordAICollab/data/boot/LIMITLESS-Bot-OnBoot.xml`
- `Tools/DiscordAICollab/data/boot/LIMITLESS-Art-OnBoot.xml`
이 파일들은 설정 검토용이며 계정·ACL·배포·토큰 설정 전에는 사용하면 안 됩니다.

## 현재 환경과 확인의 한계

현재 USB 네트워크 어댑터1개 활성, WLAN AutoConfig Running/Automatic, F: 고정 볼륨·현재 접근 가능, 기존 납품함도 현재 사용자에게 접근 가능합니다. 공개 Discord HTTPS 실제 확인 PASS입니다.

정확한 납품함은 `F:\study\codex\Project-Limitless\Temp\ArtDeliveryInbox`, DB는 격리 tool의 `data/phase2/relay.sqlite`입니다. Python venv는 `E:\Program Files\anaconda3\envs\py3_12`를 사용하므로 E: 실행 환경도 서비스 계정에서 확인해야 합니다. C: bootstrap을 준비해도 F: 또는 Python 기반 환경이 로그인 전 잠겨 있으면 중계 실행은 HOLD입니다. 부팅 때 볼륨이 접근 가능하다고 단정하지 않습니다.

서비스 계정이 아직 없으므로 **로그인 전 배치 권한/토큰 복호화/USB Wi-Fi 재연결/F:·납품함 접근/재부팅 복원은 NOT_VERIFIED**입니다. 현재 사용자 HTTPS 성공이나 합성 재연결 테스트를 로그인 전 실제 검증으로 표현하지 않습니다.

## Codex 구독 인증

OpenAI Docs의 [인증 안내](https://learn.chatgpt.com/docs/auth#login-on-headless-devices)에는 headless device-code 로그인(베타)과 계정별 인증 저장 방식이 안내되어 있습니다. 하지만 이 PC의 새 서비스 계정·비대화형 작업에서 구독 인증이 동작한다는 근거는 아직 없습니다. 기존 사용자의 auth.json/keyring/CODEX_HOME을 복사하거나 정책을 우회하지 않습니다. 유료 API 방식으로 전환하지 않습니다.

이번 부팅 프로필은 인증 성공 여부와 관계없이 **Discord 중계만 허용**합니다. 향후 Codex가 필요하면 대표가 새 계정 문맥에서 공식 로그인을 수행하고, 배치 작업에서 `codex login status` 및 허용된 공식 account 읽기만 검증한 뒤 별도 승인을 받아 AI 실행기를 연결해야 합니다. 인증 정보/일회용 코드는 Discord·GitHub·채팅에 게시하지 않습니다. 이번 Codex CLI 로그인·실제 추론·API 호출은0입니다.

## 검증 결과·보호

| 구분 | 결과 |
|---|---|
| 새 부팅 경로 격리 테스트 | 13 PASS / 0 FAIL |
| 기존 일반 프로필·중단 큐 선별 호환 | 3 PASS / 0 FAIL |
| PowerShell 구문 | 2파일 PASS |
| 승인 없는 시작 | HOLD, 프로세스 시작0 |
| 현재 사용자 Discord HTTPS | PASS |
| 실제 서비스 계정/로그인 전/USB 재연결/부팅 | NOT_VERIFIED |
| 원본 보호 | 7356 SHA변경0·누락0·git 상태 동일 |
| 기존 DB | 21업무·13usage·승인·설정·아트5 기록 동일 |
| 기존 서비스 | Bot/Art RUNNING, 재시작0, MANUAL·재개큐0 |
| 금지 작업 | 계정 생성·OS 등록·C: 배포·부팅·로그아웃·종료·AI·유료API·Push·merge 모두0 |

기존 4역할 VERIFIED·전체 QA·아트 수령 시험은 반복하지 않았습니다. 기술 검수와 바울 대표 미술 승인은 계속 분리하며 제품 적용을 실행하지 않습니다.

## 해제·복귀

등록 후에는 두 OnBoot 작업을 ‘사용 안 함’으로 바꾸고 기존 안전 중지 스크립트로 Bot/Art를 멈춥니다. 예약 해제만으로 실행 중 프로세스를 강제 종료하지 않습니다. 승인 파일을 관리자가 철회하면 다음 시작은 HOLD합니다. 기존 사용자 프로필 재가동 전에는 부팅 프로세스가 중지됐는지 확인하고 같은 DB/lock을 사용합니다. 토큰·업무 DB·납품함은 삭제하지 않습니다.

근거는 `문서/00_프로젝트/Evidence/BootRelay20261024/`에 있습니다. 등록 승인은 **전용 일반 계정·배치 로그온/제한 ACL·새 계정 DPAPI 토큰 설정·C: bootstrap 배포·두 OnBoot 작업**의 범위를 명시해야 합니다. 현재는 해당 OS 변경을 실행하지 않았습니다.
