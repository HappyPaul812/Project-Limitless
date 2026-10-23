# Phase3 수령 서비스 자동 시작·중지 설계 (미등록)

2026-10-23 / Asia/Seoul

## 실제 조사

수령 서비스의 heartbeat와 Python 런처/자식 관계를 확인했습니다. 프로세스는 22:36:41 KST에 시작한 런처36296 → 자식50196이며 SQLite/파일 lock으로 서비스 중복 실행을 막습니다. 별도 봇과 구분한 파일 수령 전용 프로세스입니다.

작업 스케줄러의 실행 인수, HKCU/HKLM Run 값, 사용자/공용 시작 폴더의 LNK 대상에서 `Phase3Run` 또는 `collab.delivery_discord`를 찾은 결과 모두 0건입니다. 이 검사는 명시적인 관련 등록 경로를 조사한 결과이며 모든 임의 래퍼/관리 프로그램의 부재를 증명하지는 않습니다. 실제 재부팅·로그아웃·서비스 종료 시험은 하지 않았습니다.

현재 수동으로 실행한 프로세스가 재부팅 후 자동으로 시작되거나 로그아웃 후 계속 실행된다고 보장할 수 없습니다. 로그아웃 후 지속 운영을 제공하는 별도 Windows 서비스는 이번 작업에서 구성하지 않았습니다.

## 권장안 — 별도 승인 후 동일 사용자 로그인 시 시작

Windows Task Scheduler에 사용자 전용 고정 작업 하나를 등록하는 방식입니다. 트리거는 토큰을 암호화한 동일 사용자의 로그인 후 약30초, InteractiveToken·최소 권한, 다른 사용자의 로그인이나 SYSTEM 계정으로 실행하지 않습니다. 기존 토큰은 Windows CurrentUser DPAPI와 사용자 전용 ACL을 유지합니다. [Microsoft DPAPI](https://learn.microsoft.com/en-us/windows/win32/api/dpapi/nf-dpapi-cryptprotectdata)는 일반적으로 암호화한 사용자·컴퓨터 조건에서 복호화하도록 정의합니다.

작업의 실행 대상은 고정 `pwsh -NoProfile -WindowStyle Hidden -File <격리 도구>/register-discord.ps1 -Action Phase3Run`입니다. 작업 인수나 XML에 Bot Token·비밀번호를 넣지 않습니다. 파일 수령만 시작하고 네 AI/Unity/새 Slash 명령 등록은 실행하지 않습니다. UI 포커스를 바꾸지 않습니다.

등록 전 필요한 보강:

1. 사용자/경로 ACL·고정 스크립트 SHA·격리 보관 경로·DB 손상 여부를 검사합니다.
2. 별도 영속 `autostart_enabled`를 기본 false로 두고 대표의 등록 승인 이후에만 true로 설정합니다. 기존 stop-request를 자동 시작이 지워 대표의 수동 중지 의사를 무효화하지 않도록 전용 시작 게이트가 필요합니다.
3. MultipleInstances=IgnoreNew와 기존 file lock을 함께 사용합니다. 강제 종료 비허용, 자동 재시작은 최대3회·간격1분 등 제한을 둡니다. 대표 수동 중지/비활성 상태에서는 자동 재시작하지 않습니다.
4. 종료 요청 시 진행 중인 수령·DB 트랜잭션·Discord outbox 처리가 끝난 뒤 stop하도록 합니다. 미전송은 PENDING, 전송 성공 불명은 UNCERTAIN으로 보존하고 중복 게시하지 않습니다.
5. 마감 예약의 신규 접수 차단 상태·주일 생산/QA 금지·AI/Unity worker 비활성을 유지합니다. 수령 서비스 시작은 미술 승인이나 실행 권한 확대가 아닙니다.
6. 허용한 사용자·경로·작업 이름과 취소 방법을 사전에 확인한 뒤 등록합니다. 지금은 등록/재시작/정지하지 않았습니다.

[Microsoft Task Scheduler](https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/schtasks-create)의 ONLOGON과 사용자·최소 권한 계약을 기준으로 합니다. 일반 사용자 로그인 기반 방식은 로그아웃 중 지속 수령을 보장하지 않습니다. 그 요구가 생기면 사용자 전용 서비스 계정·프로필/DPAPI·네트워크·권한·로그 보호를 별도 설계·승인해야 합니다. LocalSystem으로 바꾸거나 암호 저장/자동 로그인을 우회하지 않습니다.

## 현재 수동 시작·안전 중지

```powershell
# 수동 시작: 대표가 같은 Windows 사용자로 직접 실행
pwsh -NoProfile -File "F:\study\codex\Project-Limitless\Temp\DiscordAICollab20261023\Worktree\Tools\DiscordAICollab\register-discord.ps1" -Action Phase3Run

# 안전 중지 요청: 해당 수령 서비스만 다음 루프 경계에서 종료
pwsh -NoProfile -File "F:\study\codex\Project-Limitless\Temp\DiscordAICollab20261023\Worktree\Tools\DiscordAICollab\register-discord.ps1" -Action Phase3Stop
```

`data/phase3/service-status.json`의 STOPPED/heartbeat를 확인합니다. 전체 Python 종료·프로세스 이름 일괄 kill·봇/Unity 강제 종료는 하지 않습니다. 아직 없는 자동 시작 등록을 위 명령이 생성하지 않습니다. 향후 자동 시작을 등록하면 별도 영속 비활성 게이트와 작업 Disable/삭제 기능도 함께 제공해야 합니다.

PC가 완전히 꺼진 동안 수령/재개는 불가능합니다. 다음 로그인 후 Discord 커서 이후의 메시지를 SDK로 조회하고 동일 이벤트/SHA를 중복 처리하지 않도록 복원하는 경로를 사용합니다. 오래된 첨부의 가용성이 보장되지 않으면 재업로드 요청/HOLD이며 자동 복구 성공을 추정하지 않습니다. 이번에는 실제 전원/로그아웃 복원 시험을 하지 않았습니다.
