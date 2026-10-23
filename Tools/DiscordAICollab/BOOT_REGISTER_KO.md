# 부팅 작업 실제 등록 절차

1. **현재 Windows 사용자의 PowerShell 7을 관리자 권한으로 실행**합니다. 대표의 Windows PIN/암호를 제거하지 않습니다. 다른 관리자 계정을 사용해야 하는 경우 기존 파일 소유권을 바꾸지 말고 먼저 중단해 알려 주세요.
2. 아래 명령을 실행합니다.

```powershell
pwsh -NoProfile -File "F:\study\codex\Project-Limitless\Temp\DiscordAICollab20261023\Worktree\Tools\DiscordAICollab\install-boot-tasks.ps1"
```

3. 대표 Discord 숫자 ID·서버 ID·기존 허용 채널 ID를 입력합니다. 복수 채널은 공백 없이 쉼표로 구분합니다. 서비스 계정 암호를 숨김 입력합니다. 계정이 없으면 암호 확인 후 일반 계정으로 생성합니다. 기존 계정 암호를 임의 변경하지 않습니다.
4. 별도 **LIMITLESSRelaySvc 보안 입력 창**이 열리면 현재 Bot Token을 숨김 입력합니다. 토큰을 재발급하거나 채팅/GitHub에 보내지 마세요. 원본 암호화 저장소나 Codex 인증을 다른 계정에 자동 복사하지 않습니다.
5. 서비스 계정 문맥에서 DPAPI 저장·Discord REST 인증·Python/SQLite/MANUAL·납품함 경로를 검사합니다. Gateway·AI·아트 감시기는 시작하지 않습니다. 실패하면 등록 HOLD입니다.
6. C: bootstrap·관리자 소유 읽기 전용 승인 파일을 배포하고 두 OnBoot 작업을 등록합니다. 전용 계정은 관리자 그룹 가입을 거부하며 SeBatchLogonRight만 추가/읽기 확인합니다. 코드 RX·운영 DB/납품함 Modify만 부여하고 기존 비밀 저장소의 보호 ACL을 유지합니다. 기존 작업을 덮어쓰지 않습니다.
7. 두 작업의 Password/최소 권한/Boot+30초/실행 경로/활성 상태를 확인합니다. 현재 봇과 감시기는 계속 실행하며 새 작업을 수동 시작하지 않습니다. 실제 PIN 전 부팅은 재부팅 시험 전까지 NOT_VERIFIED입니다.

등록 결과는 `data/boot/registration-result.json`, 새 계정 설정 결과는 `data/boot/service-setup-result.json`에 비밀정보 없이 저장합니다. 오류 시 이번에 새로 등록한 작업은 비활성화하고 고정 실패 단계만 기록합니다. 부분 생성된 계정·ACL은 임의 삭제/초기화하지 않습니다. 암호 원문은 Windows 등록 API 전달을 위한 일시 메모리에서만 사용하고 파일·명령줄·로그에 저장하지 않습니다.

확인은 같은 관리자 창에서 다음 명령으로 가능합니다.

```powershell
pwsh -NoProfile -File "F:\study\codex\Project-Limitless\Temp\DiscordAICollab20261023\Worktree\Tools\DiscordAICollab\install-boot-tasks.ps1" -Inspect
```

이번 단계에서 실제 재부팅·종료·AI·유료 API·게임 변경·Push·main 병합을 실행하지 않습니다. 기존 전체 테스트를 반복하지 않았으며 새 스크립트 구문2개와 배치 권한 네이티브 보조 코드 컴파일만 확인했습니다. 실제 OS 등록 성공은 결과 파일과 작업 스케줄러를 확인한 뒤 기록합니다.
