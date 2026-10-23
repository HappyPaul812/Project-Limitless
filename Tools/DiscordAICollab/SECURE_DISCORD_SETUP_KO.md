# LIMITLESS AI Team 토큰 등록·저장·연결 검사

서버 초대는 바울 대표가 완료하셨습니다. 토큰은 **이 채팅/GitHub에 보내지 않고**, 같은 Windows 사용자 계정의 로컬 PowerShell에서만 입력합니다.

## 처음 한 번 등록

PowerShell에서 아래를 실행하세요. Bot Token 값 자체를 명령줄에 쓰지 않습니다.

```powershell
pwsh -NoProfile -File "F:/study/codex/Project-Limitless/Temp/DiscordAICollab20261023/Worktree/Tools/DiscordAICollab/register-discord.ps1" -Action Setup
```

화면의 네 질문에 입력합니다.

1. **바울 대표 Discord 사용자 숫자 ID**: 표시명/닉네임이 아닙니다.
2. **초대 완료한 서버 숫자 ID**.
3. **테스트 텍스트 채널 숫자 ID**: 첫 채널에 고정 테스트 메시지1개를 보냅니다. 여러 채널은 공백 없이 쉼표로 구분합니다.
4. **Bot Token**: Developer Portal의 LIMITLESS AI Team 앱 → Bot에서 발급한 값을 숨김 입력창에 붙여넣고 Enter. 입력값은 표시되지 않습니다.

숫자 ID는 Discord 개발자 모드를 켜고 계정/서버/채널의 **ID 복사**로 얻습니다. 봇은 해당 채널의 View Channels·Send Messages, 앱은 applications.commands 범위가 필요합니다. Administrator 권한이나 사용자 계정 토큰은 필요하지 않습니다.

## 저장 방식

`Tools/DiscordAICollab/data/private/discord/connection.json`에 **암호화된 토큰과 숫자 ID**를 저장합니다. Windows DPAPI CurrentUser를 사용하고 저장 폴더 ACL을 현재 사용자 전용으로 설정합니다. `data/` 전체는 Git에서 제외됩니다. 평문 .env·소스·로그·영구 환경변수에는 저장하지 않습니다.

이 저장본은 같은 PC/Windows 사용자에서 재사용하는 용도입니다. 같은 사용자 권한으로 실행되는 프로그램까지 막는 별도의 비밀 금고는 아닙니다. 파일을 다른 담당자나 GitHub에 보내지 마세요. 토큰이 노출되면 Discord에서 재발급 후 명시적으로 교체합니다.

기존 등록은 자동으로 덮어쓰지 않습니다. 토큰 교체가 필요할 때만 위 명령의 끝에 **-Replace**를 붙입니다. 실제 Save/UserData·Unity·승인 아트에는 쓰지 않습니다.

## 등록 직후 자동 검사

등록 명령은 저장 뒤 최대45초 동안 다음만 검사하고 연결을 닫습니다.

- 토큰 인증과 Gateway READY.
- 지정 서버의 `/limitless` 명령9종 등록·재조회. 같은 앱에 다른 guild 명령이 있으면 삭제하지 않고 중단합니다.
- 지정 채널이 그 서버의 텍스트 채널인지 확인.
- 고정된 연결 테스트 메시지1개 전송. mention/AI 호출/미술 승인/제품 변경 없음.

결과는 **data/phase2/discord-live-test.json**에 비밀 없이 저장합니다. PASS여도 실제 사용자가 명령을 입력하고 권한 차단을 확인한 것은 아니므로 slash_interactions는 NOT_VERIFIED로 구분합니다.

성공/실패와 관계없이 토큰 평문은 프로세스 실행 중 메모리/자식 환경에서만 사용하고, 스크립트 종료 시 원래 환경을 복원합니다. 암호문·예외 원문도 콘솔에 출력하지 않습니다. 잘못된 토큰/권한/채널은 오류 종류만 기록하며 무한 재시도하지 않습니다.

## 다음 실행

같은 명령의 **-Action Setup**을 다음 중 하나로 바꿉니다.

|Action|동작|
|---|---|
|Status|등록 유효성만 표시. 토큰·숫자 ID·암호문 표시 안 함|
|Test|저장된 토큰으로45초 검사·테스트 메시지1개 전송 후 닫음|
|Run|저장된 토큰으로 봇 유지 실행. Ctrl+C로 종료|

Run 중에는 대표 계정으로 `/limitless status`, `/limitless pending`을 확인하고, 별도 비소유자 계정의 approve/pause가 거부되는지 확인할 수 있습니다. 기존9명령의 상세 절차는 [Phase2 안내](PHASE2_SETUP_KO.md)를 따릅니다. 실제 미술을 연결시험 목적으로 승인하지 마세요.

등록 뒤 이 대화에는 **“등록 완료”** 또는 비밀을 제외한 오류 종류만 알려 주시면 됩니다. Codex는 결과 파일로 실제 성공 여부를 확인할 수 있습니다. 아직 토큰을 입력하지 않았다면 WAITING_FOR_USER_SETUP이며 서버 초대만으로 접속 PASS가 되지는 않습니다.

이번 단계는 Discord 연결만 허가합니다. Codex 추가 추론·유료 API·아트 제작·본편 적용·Push/main병합·SFX 활성화는 실행하지 않습니다.

공식 근거: [Microsoft DPAPI 기본 암호화](https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.security/convertfrom-securestring?view=powershell-7.6), [Discord Gateway](https://docs.discord.com/developers/events/gateway), [명령 등록](https://docs.discord.com/developers/interactions/application-commands).
