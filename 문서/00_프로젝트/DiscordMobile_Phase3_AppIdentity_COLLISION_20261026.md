# Issue #66 — 운영/시험 Application ID 동일: 실행 전 HOLD

## 판정과 근거

**HOLD_PRODUCTION_AND_TRIAL_APPLICATION_ARE_SAME**.

대표가 시험 앱 `Limitless-test`의 ID `1564156403571105906`을 전달했고, 운영 앱 ID 확인 질문에도 같은 값을 제공한 뒤 재확인에 **“같은 앱”**이라고 답하셨습니다. 이번 시험은 운영 앱과 다른 앱에서만 허용되므로 이 조건은 FAIL입니다. 같은 서버/다른 채널/다른 앱 이름만으로 별도 application이 되지는 않습니다. 운영 앱의 이름이나 token을 변경하여 우회하지 않습니다.

대표 승인 User ID는 기존 운영의 공개 `data/boot/setup-public.json`에서 대조했습니다. 운영 token/DPAPI 저장소는 열지 않았습니다. Guild ID는 기존 공개 설정과 일치합니다. 사용자 ID 원문은 이 문서와 GitHub 보고에 복제하지 않고 일치 여부·공개 근거 SHA만 증거에 기록합니다.

서버 `Limitless AI Team` / Guild `1563112744436244541`, 채널 `ai-시험실` / Channel `1564163234338967552`는 대표 전달값으로 보존합니다. 실제 Discord API 권한 검증은 아직 수행하지 않았습니다. 운영/시험 앱 동일 판정의 출처는 대표의 직접 확인이며 운영 token을 사용한 API 조회 결과가 아닙니다.

## 이번에 실행하지 않은 항목

- token 입력 요청/열람/복사·저장소 생성0.
- Host 새 구현/기동0, 로그인/Gateway/Discord REST0.
- 명령 CREATE/DELETE0, 반환 command ID 없음.
- 모바일 왕복 NOT_ATTEMPTED, FakeExecutor/실제 모델/공식 usage/운영 SQLite 업무0.
- 새 permit/소비 마커/실행 승인 슬롯0. 정리할 신규 Host·등록 명령 없음.
- 기존 Phase1 VERIFIED/Phase2 24/Phase3 18+패키지5 전체 재검증0.

부족한 승인 때문이 아니라 **승인된 대상과 실제 앱이 충돌해서** HOLD합니다. 현재 지시의 “앱 ID 검증 실패 시 HOLD”, “운영 앱 변경 금지”를 적용합니다. 실행 권한을 다시 묻거나 운영 앱 사용을 제안하지 않습니다.

## 대표가 수행할 최소 수정

1. [Developer Portal](https://discord.com/developers/applications)에서 기존 앱을 선택/이름 변경하지 않고 **New Application**으로 새 시험 앱을 만듭니다. 예: `Limitless-mobile-trial`.
2. 새 Application ID가 **`1564156403571105906`과 다른 값**인지 확인합니다. 대표 계정 소유 앱이어야 합니다. 새 ID와 소유 확인만 전달하시면 됩니다. 대표 승인 ID는 이미 확인했으므로 다시 요청하지 않습니다.
3. 새 앱을 Guild Install, `bot` + `applications.commands`, View Channels/Send Messages만으로 기존 시험 서버에 설치합니다. 기존 `ai-시험실`에 새 시험 bot의 접근을 허용하되 기존 운영 bot/아트/채널 권한은 변경하지 않습니다. Privileged Intents/Administrator를 추가하지 않습니다. [Discord 공식 앱 생성·설치 안내](https://github.com/discord/discord-api-docs/blob/main/developers/quick-start/getting-started.mdx).
4. 새 앱 전용 token은 **채팅·Discord·GitHub에 보내지 않습니다**. 새 앱 ID가 확인된 뒤 실제 Host 실행 계정의 별도 DPAPI 저장소에 대표가 로컬 보안 입력하도록 안내하겠습니다. 기존 운영 token/서비스 계정 인증·ACL은 변경하지 않습니다.

서버/채널은 새로 만들 필요가 없습니다. 새 앱 정보가 확인되면 기존 미실행 시험 승인 범위에서 별도 Host/보안 입력/한시 실행·정리 경로 준비를 이어갑니다. 이번 소비 마커를 초기화할 작업은 없으며 기존 과거 마커는 그대로 보존합니다.

## 이후 순서와 보호

서로 다른 앱 ID·실제 token/application/대표 소유·최소 권한 확인 → 고정 패키지 재사용과 별도 Host 진입점/정확 SHA/전체 시간 제한·중지 경로 준비 → 명령 inventory 백업 → 동일 이름 없을 때 단일 guild CREATE1 → 반환 ID 기록 → Host READY/만료 시각 보고 → 대표 모바일 시험. 이 단계 전에는 휴대전화 명령을 실행하지 않습니다.

모바일 첫 명령은 READY 확인 후 `/limitless-trial status`. 이후 task→plan→approve-execution→run-fake→handoff 순서이며 root1/Fake최대4/동시1입니다. 기존 `/limitless approve`는 미술 승인 전용으로 보존합니다. 종료는 접수 차단→Host/자식0→이번 반환 command ID 하나만 DELETE→목록 대조·승인/소비/receipt/checkpoint 보존입니다. sync/bulk overwrite와 자동 재실행은 금지합니다.

이전 ZIP `103ecc96c6796d288986eb6ba3ddf5e8612153010b4976f4b9c39a4dcc88099b`, Manifest `f69049c61737476e6707bcb9559957e88115fbc19da0a56a92c8ae339348e5a7`, adapter/Phase2 module/boot approval SHA 불변을 확인했습니다. 새 증거는 `Evidence/DiscordMobileAppCollision20261026/CollisionHold.json`입니다. 코드/패키지 변경0, 문서만 추가했습니다.

MANUAL·실업무 HOLD·A 미활성, 기존 운영 bot·아트·부팅·운영 DB·CLI/CODEX_HOME/인증/ACL 변경0. Unity/게임/Save/Push/main/PC 종료·재부팅0. 기존 원본 HEAD `bfd281545632877325f5d21e04cefb924042c465`·tracked dirty308 보존. 실제 AI 연결·A 모드는 모바일 Fake 왕복 성공 후에도 별도 승인 대상입니다.

브랜치 `codex/discord-usage-diagnostics-20261026`, 시작 HEAD `4f9447075538a14aab226b543bd0b3c351d2b687`. 실제 문서 commit과 Issue66 링크는 완료 보고에 기록합니다. 준비된 Host나 연결 성공이 없으므로 만료 시각/종료 성공을 만들어 보고하지 않습니다.
