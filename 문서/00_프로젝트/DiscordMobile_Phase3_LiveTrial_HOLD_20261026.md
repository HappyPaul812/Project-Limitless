# Issue #66 Phase3 실제 모바일 시험 — 시험 앱 미확인 HOLD

바울 대표의 별도 시험 앱 등록·FakeExecutor 왕복·종료 정리 승인은 확인했습니다. **승인 부족이 아니라 시험 앱/소유권/로컬 보안 설정 미확인 때문에 실행 전 HOLD**합니다. 시험 앱이 외부에 실제로 없다고 단정하지 않습니다.

## 확인한 사실

- 시작 브랜치 `codex/discord-usage-diagnostics-20261026`, HEAD `6fcaafa06ca5b89bde7eeffe55431afa125ddb40`, clean.
- 기존 ProgramData 경로에서 모바일 시험 루트를 찾지 못했고, 확인한 시험 환경변수 이름도 없습니다. prepared 루트에는 비활성 Manifest와 ZIP만 있습니다. 전용 시험 application/owner/guild/channel 설정 또는 전용 로컬 보안 저장소를 확인하지 못했습니다.
- Developer Portal 계정·소유 앱 목록은 조회하지 않았습니다. 운영 토큰/서비스 인증을 읽거나 복사하여 시험 앱을 찾지 않았습니다. 따라서 외부 앱의 부재를 증명한 것은 아닙니다.
- 시험 앱 ID·대표 소유권·제한 서버/채널·bot 권한 모두 NOT_VERIFIED. 별도 시험 Host 준비/기동·등록·모바일 실행 단계에 진입하지 않았습니다.
- 기존 ZIP `103ecc96c6796d288986eb6ba3ddf5e8612153010b4976f4b9c39a4dcc88099b`, Manifest `f69049c61737476e6707bcb9559957e88115fbc19da0a56a92c8ae339348e5a7`, adapter/Phase2 source/boot approval 고정 SHA 일치. Phase1/2 증거의 이전 raw SHA 보존, Git 내용은 CRLF 차이를 구분해 대조했습니다. 증거를 재작성하지 않았습니다.
- 기존 Phase1 VERIFIED·Phase2 24 PASS·Phase3 18 PASS/패키지5 PASS 결과 보존, 정상 테스트 전체 재실행0. 이번에는 필요한 파일 무결성 확인만 했습니다.

`Evidence/DiscordMobilePhase3LiveHold20261026/PreflightHold.json`에 비밀 없는 발견 범위·SHA·미실행 횟수·HOLD 사유를 기록했습니다.

## 대표가 Developer Portal에서 수행할 최소 절차

이미 별도 시험 앱이 있다면 새로 만들지 않고 아래 설정과 소유권만 확인합니다. 기존 LIMITLESS AI Team 앱을 수정하지 않습니다.

1. [Discord Developer Portal](https://discord.com/developers/applications)에 대표 계정으로 로그인합니다. 개인 소유의 새 application을 만들거나 대표 소유 시험 앱을 선택합니다. 이름 예: `LIMITLESS Mobile Trial`. General Information의 Application ID를 확인하고 운영 앱 ID와 다른지 확인합니다. Team 소유라면 대표의 소유/관리 권한도 확인해야 합니다.
2. Bot 페이지에서 **시험 앱의 bot**만 준비합니다. Privileged Gateway Intents는 켜지 않습니다. 기존 운영 bot token을 reset/복사하지 않습니다. 새 시험 token은 로컬 보안 입력 시에만 사용하며 채팅·GitHub·Discord·명령 인자로 보내지 않습니다.
3. Installation에서 Guild Install을 사용하고 `bot`, `applications.commands` scope를 설정합니다. 시험 서버에만 설치하고 bot 권한은 `View Channels`, `Send Messages`로 제한합니다. Administrator/Manage Server/Message Content 권한을 추가하지 않습니다. [Discord 공식 앱 설치 안내](https://github.com/discord/discord-api-docs/blob/main/developers/quick-start/getting-started.mdx).
4. 대표가 관리하는 제한 시험 서버/텍스트 채널 하나를 정합니다. 시험 채널은 대표와 시험 bot만 접근하도록 설정합니다. 운영 bot/아트 채널의 권한은 바꾸지 않습니다.
5. Discord 설정의 Developer Mode를 켜고 대표 본인의 User ID, 시험 Server ID, Channel ID를 복사합니다. Application ID와 함께 로컬 비밀 없는 설정에 사용할 4개 ID입니다. [Discord 공식 ID 확인 안내](https://support.discord.com/hc/en-us/articles/206346498-Where-can-I-find-my-User-Server-Message-ID).

다음 정보를 확인하면 됩니다: **시험 Application ID / 대표 User ID / 시험 Guild ID / 시험 Channel ID / 대표 소유 확인**. ID만 전달할 수 있고 **token·암호·인증 자료는 보내지 않습니다**. 시험 token은 향후 실제 Host 실행 Windows 계정의 전용 로컬 DPAPI 보안 저장소에 보관해야 합니다. 지금 운영 저장소를 재사용하거나 사용자/서비스 계정 간 인증 자료를 복사하지 않습니다.

## 설정 확인 후 같은 승인 범위에서 이어갈 절차

이번 모바일 시험 승인은 실행되지 않았습니다. 설정 준비를 이유로 승인·소비 마커를 초기화하거나 기존 슬롯을 재사용할 필요가 없습니다. 실제 앱/ID/소유권·최소 권한·token 저장소의 계정/ACL을 확인한 뒤 현재 승인 범위에서 Host와 등록을 준비합니다. 별도 앱이 준비되지 않으면 운영 앱으로 자동 대체하지 않습니다.

한시적 Host는 이전 adapter/고정 패키지 SHA를 그대로 재사용하는 별도 진입점으로 준비해야 합니다. 현재 후보에는 login/start/sync 진입점이 없으므로 **아직 Host 구현/기동 완료로 보고하지 않습니다**. 필요한 신규 Host/보안 wrapper의 SHA와 전체 제한 시간·접수 차단/프로세스 종료·정리 방법을 고정한 뒤 실행합니다. 운영 부팅 작업을 변경하지 않습니다.

등록 전 해당 시험 app/guild 명령 목록·ID·권한 백업 → 동일 이름 없음을 확인 → **단일 guild CREATE 한 번** → 반환된 command ID 보존. 전체 sync/copy_global_to/bulk overwrite 금지. 기본 명령 권한0과 대표 allowlist를 모두 유지하고 등록 후 시험 명령에 대표/시험 채널만 명시 허용합니다. 기존 `/limitless approve`는 변경하지 않습니다.

### 모바일 순서 — 실제 등록·Host READY 확인 후에만

```text
/limitless-trial status
/limitless-trial task title:휴대전화 연결 시험
/limitless-trial plan task_id:<기획 task ID>
/limitless-trial approve-execution task_id:<기획 ID> sha256:<plan의 전체64자리SHA> ttl_seconds:300
/limitless-trial run-fake task_id:<기획 ID>
/limitless-trial handoff task_id:<기획 ID>
```

반환된 NewArt/RenewalArt task 각각 plan→approve-execution→run-fake를 수행합니다. 기획 ID에 handoff → 반환된 Codex task의 plan/승인/run-fake → 기획 ID handoff로 기획 결과 기록을 확인합니다. 최대 root1·FakeExecutor4회·동시1, 실제 AI/공식 usage/운영 DB 업무0. 승인 만료·취소·잘못된 SHA·전송 불명은 HOLD이며 자동 재실행하지 않습니다. 취소 명령은 소비 전 `/limitless-trial cancel-execution task_id:<ID>`입니다.

root1 제약 때문에 성공한 root를 취소 테스트용으로 초기화하지 않습니다. 취소/만료 검사를 독립 실행으로 추가하려면 기존 소비를 보존하고 root1/호출4 상한 안에서 시험 시나리오를 먼저 확정해야 합니다. 현재는 어떠한 모바일 시나리오도 시작하지 않았습니다.

시험 종료 시 intake 차단→Host 종료/자식0 확인→**이번 CREATE 반환 ID 하나만 DELETE**→전후 목록 대조. 시험 승인/소비/전송 receipt/체크포인트는 보존합니다. 등록 결과가 불명이면 자동 CREATE를 재시도하지 않습니다. 삭제 실패도 증거와 해당 ID를 보존하고 HOLD합니다.

## 이번 결과·보호

| 항목 | 결과 |
| --- | --- |
| 별도 앱·소유권·권한 | NOT_VERIFIED |
| 실제 명령 등록/삭제 | 0 / 0 |
| Host 시작/종료 | 0 / 해당 없음 |
| 모바일 명령·응답 | NOT_ATTEMPTED |
| FakeExecutor / 실제 모델 / 공식 usage | 0 / 0 / 0 |
| Discord 게시 / 운영 SQLite 업무 | 0 / 0 |
| 신규 permit/소비/시험 슬롯 | 없음 |
| 정리 | 새 Host/명령이 없으므로 제거할 자원 없음 |
| 운영 상태 | MANUAL 유지, 운영 bot·아트·부팅·인증/CLI/CODEX_HOME·승인 파일 변경0 |

Unity/게임/아트/Save·Push/main 병합·PC 종료/재부팅0. 운영 프로세스의 현재 실시간 건강을 새로 판정한 것은 아닙니다. 기존 원본 HEAD `bfd281545632877325f5d21e04cefb924042c465`, 기존 tracked dirty308 상태를 보존합니다.

**실제 AI 연결은 모바일 Fake 왕복 성공 이후에도 별도 승인·공식 usage/Session0 Worker 접합·정확한 실행 scope/한도·검증·정리 확인이 필요합니다. A 모드는 계속 미승인입니다.**

이 HOLD는 사용자가 지정한 ‘별도 시험 앱 미준비 시 Portal 최소 안내 후 중지’ 분기에 따른 것입니다. 새로운 실행 권한을 요청하는 것이 아닙니다. 실제 로컬 문서 commit과 Issue66 보고 링크는 완료 보고에 기록합니다.
