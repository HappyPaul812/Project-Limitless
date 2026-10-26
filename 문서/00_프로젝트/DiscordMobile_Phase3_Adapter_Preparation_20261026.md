# Issue #66 Phase 3 — 모바일 Discord 제한 시험 준비

**OFFLINE_READY / LIVE_TRIAL_HOLD / NOT_DEPLOYED.** Phase1 VERIFIED·Phase2 24 PASS 보존. 이번 모델/공식 usage/Discord 접속·게시/명령 등록/운영 SQLite 업무0. MANUAL/실제 AI 업무 HOLD 유지, A 미활성.

## 구현·검증

`collab/mobile_discord_trial.py`는 실제 discord.py 2.7.1 slash command 객체/callback과 기존 Phase2 `OfflineMobileRelay`·고정 FakeExecutor를 연결합니다. 기존 `mobile_offline.py`, 4역할 표/인계/JSONL 검증기, `bot.py`, `/limitless approve`는 수정하지 않았습니다.

- 별도 guild 전용 `/limitless-trial`: task / plan / approve-execution / cancel-execution / run-fake / handoff / status.
- SDK interaction의 application ID, 대표 사용자 숫자 ID, guild/channel ID, bot 여부, 사용자 Use Application Commands 및 봇 View Channel/Send Messages를 매번 확인합니다. 시험 앱과 운영 앱은 달라야 합니다. DM 거부·기본 명령 권한0·대표 명시 허용 필요. 관리자도 owner allowlist를 우회하지 못합니다.
- interaction ID+사용자/서버/채널/명령/인자 fingerprint를 영속 보존합니다. STARTED를 먼저 commit하고 중복/중단 시 자동 재실행하지 않습니다.
- 전체 task SHA·역할·고정 읽기 범위·base/upstream·한도·TTL 최대300초를 기존 승인 게이트로 확인합니다. 승인만으로 실행하지 않습니다. 시험 root1개·4역할 가짜 호출 최대4개. 기존 미술 승인과 별개입니다.
- 취소는 미소비 승인을 CANCELLED로 바꾸고 root 취소를 후속 분기에도 적용합니다. 소비/근거/보고를 삭제하지 않습니다. 임대 무효화 후 결과 채택을 거부하며 실제 프로세스 취소 구현은 이번 범위가 아닙니다.
- FakeExecutor→기존 JSONL→FakeDiscord receipt와 실제 followup receipt를 별도 보존합니다. 실제 message ID 확인 전 인계 차단. 결과불명은 UNKNOWN_NO_RETRY/checkpoint, 자동 전송/실행 재시도 없음.
- ephemeral·AllowedMentions.none·최대1800자 한국어 응답, 역할/상태/승인 SHA/실제 AI0 표시. 제목·원시 SDK 오류/토큰을 결과에 반사하지 않습니다.
- TrialQuota는 공식 관측이 아닌 명시적 합성 입력입니다. Discord에서 갱신할 수 없습니다. 5시간/주간 중 하나 소진이면 HOLD, 둘 다 소진이면 늦은 reset까지 대기합니다. reset 이후도 새 관측이 필요합니다. 미확인/미래/만료 관측도 HOLD. 대기 중 승인 미소비, 회복 후에도 원래 승인 기간·취소·SHA 재검증. 관측 갱신만으로 자동 실행하지 않습니다.

신규 **18 PASS / 0 FAIL / 0 ERROR / 0 SKIP**, 패키지 smoke **5/5 PASS**. 실제 SDK callback을 가짜 interaction으로 완료하고 login/start/sync·socket·프로세스를 차단했습니다. 차단 경계 호출 시도0·scratch 밖 Python 쓰기 시도0. 기존 Phase1/Phase2 24/38/22+12/native/Unity 전체 검사 재실행0. `Evidence/DiscordMobilePhase3Prepared20261026/OfflineTests.json`, `PackagedImportSmoke.json`에 보존했습니다.

오프라인 재현 명령(실연결 없음):

```powershell
$env:PYTHONDONTWRITEBYTECODE='1'
& 'F:/study/codex/Project-Limitless/Temp/DiscordAICollab20261023/Worktree/Tools/DiscordAICollab/.venv/Scripts/python.exe' 'F:/study/codex/Project-Limitless/Temp/DiscordUsageDiagnostics20261026/Worktree/Tools/DiscordAICollab/tests/run_mobile_discord_trial_offline.py'
```

기존 venv의 SDK만 읽어 사용했고 패키지 설치/인증/운영 설정은 변경하지 않았습니다.

## 패키지·승인 파일

준비 루트: `Temp/DiscordUsageDiagnostics20261026/MobileDiscordPhase3Prepared20261026/`.

- 정확16파일: 기존 승인 raw bytes13(보안 PS모듈 포함)+Phase1 고정 roles1+신규 런타임 모듈2(Phase2/Phase3).
- 후보 의존 runtime4개(exec_diagnostics/intake/operations_safety/output_parser)는 승인본과 CRLF/LF만 달랐습니다. 승인 source SHA 및 정규화 내용 동일을 확인하고 **기존 승인 바이트를 패키지에 사용**했습니다. roles도 Phase1 고정 SHA5ce1…의 기존 바이트를 사용했습니다. 승인 목록을 새로운 줄바꿈 SHA로 갱신하지 않았습니다.
- 보수적 상대 import 의존성 폐쇄/SHA를 기록했습니다. 라이브러리에 기존 실제 runner 함수가 있어도 어댑터에서 호출하지 않습니다. FakeExecutor/FakeDiscord 이외는 거부합니다.
- ZIP 집합/SHA, 패키지 import, 7명령 payload 동일성 smoke PASS. token/암호/인증정보/운영 DB 복사0.
- **Manifest approved=false / permit 미발급 / ProgramData 미배포**. `CommandPayload.NOT_REGISTERED.json`은 로컬 직렬화 결과입니다. 기존 승인43/44를 자동 확대하지 않았습니다. 정확한 시험 manifest/ID/경로/버전/ACL의 별도 승인이 필요합니다.

정확한 코드/시험 파일 SHA·ZIP/Manifest SHA는 아래 자동 생성 표와 `Manifest.UNAPPROVED.json`, `ChangeHashes.json`을 따릅니다. SHA는 raw bytes이며 이후 checkout 줄바꿈 변환 시 재대조가 필요합니다.

## 실제 왕복 시험 계획 — 이번에는 미실행

운영 봇을 건드리지 않는 **별도 시험 application/bot + 제한 guild/channel**을 제안합니다. 운영 토큰을 복사하여 두 번째 Gateway를 띄우지 않습니다. 시험 token은 대표의 로컬 보안 설정만 사용하고 채팅/명령 인자/패키지에 넣지 않습니다. 기존 Codex/ChatGPT 인증은 변경하지 않습니다.

시험 앱 생성·초대·로그인은 하지 않았습니다. 실제 owner/app/guild/channel ID, 설치 scope/채널 권한, 서비스 SID ACL, 현재 원격 명령 목록은 **미확인**입니다. 미래 승인 시 확인하고 불일치면 HOLD합니다. 현 모듈은 login/start/run/sync/자동 시작 기능을 제공하지 않으므로 지금 실행할 관리자 실연결 명령은 없습니다. 별도 한시적 시험 host의 기동·종료 경로를 다음 승인 범위로 확정해야 합니다.

### 등록 영향과 필요한 승인

1. 현재 명령 목록/ID/permissions를 읽기 전용 스냅샷. 시험/운영 앱이 다르고 같은 시험 명령이 없어야 합니다. 자동 교체 금지.
2. `/limitless-trial` **단일 guild 명령 생성**만 계획합니다. global/전체 bulk overwrite/기존 `/limitless` 수정 금지. 반환 command ID/payload SHA 기록.
3. Discord bulk overwrite는 guild 전체 명령을 교체합니다. 기존 bot.setup_hook의 copy_global_to+sync를 시험 등록에 재사용하지 않습니다. [Discord 공식 명령 문서](https://docs.discord.com/developers/docs/interactions/slash-commands), [discord.py add/sync 설명](https://discordpy.readthedocs.io/en/stable/interactions/api.html#discord.app_commands.CommandTree.add_command).
4. 기본 권한0인 명령에 대표·허용 채널을 명시 허용합니다. 초대 scope는 bot/applications.commands, 봇 채널 권한은 View/Send. Administrator/Message Content 추가 불필요. 플랫폼 허용과 코드 allowlist를 모두 확인합니다. [Discord 공식 권한 문서](https://docs.discord.com/developers/docs/topics/permissions).
5. 새 보호 ProgramData 시험 루트와 합성 DB/manifest/receipt 사용. 운영 SQLite migration/복사/슬롯 재사용0. 기존 ACL을 바꾸지 않고 새 경로의 서비스 SID·관리자 접근을 검토합니다.
6. 다음 승인 범위 제안: 단일 guild 등록1개+종료 후 해당 ID 삭제, 한시적 시험 Gateway, root1/가짜 호출최대4, 실제 AI/공식 usage0. 시험 시간·보고 한도·고정 quota 시나리오·시험 token 공급·host 종료/프로세스 정리를 명시해야 합니다. 운영 봇 재시작은 필요하지 않습니다.

### 대표 휴대전화 명령 — 등록·실연결 승인 후에만

**현재 등록되지 않았으며 지금 실행하지 않습니다.** placeholder는 응답에서 복사합니다.

```text
/limitless-trial status
/limitless-trial task title:휴대전화 연결 시험
/limitless-trial plan task_id:<응답 task ID>
/limitless-trial approve-execution task_id:<ID> sha256:<전체64자리SHA> ttl_seconds:300
/limitless-trial run-fake task_id:<ID>
/limitless-trial handoff task_id:<최초 기획 ID>
```

두 아트 task 각각 plan→승인→run-fake, 최초 기획 task handoff→Codex task plan/승인/run-fake→기획 handoff로 결과 기록을 확인합니다. 실제 모델0 유지. 취소는 소비 전 `/limitless-trial cancel-execution task_id:<ID>` 후 run-fake HOLD를 확인합니다. 취소/소비 trial을 초기화하지 않습니다.

대표 외 사용자/다른 채널 거부, 중복 실행0, 실제 followup receipt, 잘못된 SHA·만료·취소 HOLD를 확인합니다. quota 소진/회복은 승인된 로컬 합성 fixture로만 조작하고 공식 사용량으로 위장하지 않습니다. 관측5분 만료 시 HOLD가 정상이며 새 관측도 승인 기간을 연장하지 않습니다.

### 영향·되돌리기

이번 준비의 운영 영향0. 미래 시험은 시험 앱 명령1개/ephemeral 응답/새 시험 DB·증거에 한정합니다. 별도 시험 앱이 없으면 HOLD하며 운영 앱으로 자동 대체하지 않습니다.

종료 시 intake/worker 차단→시험 host만 종료·잔여 프로세스0 확인→**반환된 시험 application/guild/command ID 하나만 삭제**→원격 목록을 이전 스냅샷과 비교합니다. 전체 sync/bulk overwrite로 롤백하지 않습니다. 시험 DB/승인/소비/오류·전송불명/checkpoint/manifest는 보존합니다. 필요하면 시험 앱 접근만 회수합니다. 기존 운영 DB 복원/삭제·부팅/인증/ACL 변경은 필요하지 않습니다. 토큰 폐기도 별도 시험 token에만 적용합니다.

## 미래 A 모드 설계 — 활성화/실제 실행 없음

Discord 업무→승인 범위→공식 usage→Session0 제한 Worker→4역할 검증/인계→실제 보고 receipt가 목표입니다. 현재 어댑터는 MANUAL+Fake 전용이므로 A에서 그대로 실 Worker를 실행하지 않습니다. 기존 ResumeQueue/checkpoint의 미소비 승인·scope/base/실패/전송 상태를 재사용하는 별도 접합 검토가 필요합니다.

공식 usage 실패/5시간·주간 소진은 새 실행만 보류합니다. reset 뒤 **새 공식 AVAILABLE 관측**, CLI/인증 방식/설정 SHA·원본 SHA·승인 기간/취소·역할·예산·남은 횟수·시간창을 모두 재검증합니다. 소비/실패/결과불명 요청 재실행 금지. 승인 기간이 끝나면 재승인이 필요합니다.

300초 시험 승인은 긴 대기 뒤 자동 갱신하지 않습니다. 장기 A 승인이 필요하면 유효기간·정적 역할 DAG/템플릿 SHA·범위·원본 SHA·누적 한도를 별도 승인하고 파생 task/upstream SHA를 결정적으로 결합해야 합니다. 모델 자유 텍스트를 새 권한/작업 승인으로 해석하지 않습니다. 후속 정확 task SHA 승인이 없으면 대표 승인 대기입니다. 장기 승인 정책/공식 재조회 주기·횟수/실 Worker 접합은 아직 미승인·미구현입니다.

## 기록·중지

브랜치 `codex/discord-usage-diagnostics-20261026`, 시작 commit `d2db1e47b541e3e578985dc746eb31ec066b318c`. 최종 commit은 Issue66 완료 댓글에 기록합니다. CURRENT_STATUS/CODEX_LATEST 갱신. 운영 bot/아트 감시 수정·재시작0이며 실시간 건강을 새로 조회하여 RUNNING 판정한 것은 아닙니다.

실제 모바일 왕복은 **별도 승인+시험 앱/ID/권한/보호 경로·정확 manifest/inventory·단일 등록·시험 host 승인** 후에만 가능합니다. 준비 완료 후 중지합니다. 실제 AI/A/운영 배포/Unity·Save·게임/Push/main/PC 종료·재부팅0.

## SHA256

| Path | Before | After SHA256 |
| --- | --- | --- |
| `Tools/DiscordAICollab/collab/mobile_discord_trial.py` | NEW | `dbe27663a4d47bcbc1c30e8d85cc1c343ec46476b151967d582b2df9824d06de` |
| `Tools/DiscordAICollab/tests/test_mobile_discord_trial.py` | NEW | `27eb0018a354f7b0c6882b497ced0aafe8a50cf9391c88f9379e0e3413d59a09` |
| `Tools/DiscordAICollab/tests/run_mobile_discord_trial_offline.py` | NEW | `3857b49523b23335c5eb00a22acbc038acd7fdde28a21c18787dd13fb943912e` |

- Boot approval (unchanged): `3e032ebcce3d91bd762a8dec81048d04607ddde761afd62489c0fa2b8e49140f`
- ZIP: `103ecc96c6796d288986eb6ba3ddf5e8612153010b4976f4b9c39a4dcc88099b`
- Manifest approved=false: `f69049c61737476e6707bcb9559957e88115fbc19da0a56a92c8ae339348e5a7`
- Phase2 mobile_offline (unchanged): `aef705df03cffb4e631a5e5514a0924776d2f19fac7d66404dff1b5dbfc7ce98`
