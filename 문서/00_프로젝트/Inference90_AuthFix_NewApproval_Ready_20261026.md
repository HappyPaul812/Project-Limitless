# Issue66 인증 수정본 신규 단발 승인 — 관리자 직접 실행 대기 (2026-10-26)

**AUTHFIX_NEW_ONCE_AUTHORIZED / PHASE1_NOT_STARTED / PHASE2_NOT_STARTED / MANUAL / AI_EXECUTION_HOLD.**

바울 대표의이번명시승인으로 d9fa4f7c 수정후보의읽기전용추론최대1회를새요청으로준비했습니다. 기존모델승인/소비슬롯을재사용하지않습니다. 추가승인을다시묻는단계가아니며현재agent비관리자Session9·서비스Windows암호미보유라대표직접실행이필요합니다.

## 승인된 후보와 새 요청

- 기준commit **d9fa4f7c4e7eeb8f78c34ab6bc19ec92d4595635**. entry SHA **b841cf6557046ada96b67564be7e093dcb0666980577de8ab37273dd0c22b1ab**를고정했고후보코드는수정하지않았습니다.
- 서비스승인config SHA **818c2e7b52f1c574fd1f9e0ac49dd876ad10746ff5ad95ce427d636938747bc3**. 서비스SID로정제한review93fa150e의projection20b0e1b0를다시대조. 실제시험worker도현config SHA를확인하고다르면usage/exec전에HOLD합니다.
- cli_auth_credentials_store="keyring" / features.view_image=false·image_generation/sleep_tool/tool_suggest=false·기존도구/웹/앱/MCP/하위agent차단·read-only/never 유지. builtin OpenAI/기존ChatGPT구독/고정CLI4fd2f057…·CODEX_HOME·인증자료변경0.
- **새request `d56fdba9618044b98f98fe98e4cff211`**, 승인범위 AUTHFIX_D9FA4F7C_NEW_READONLY_ONCE_20261026. prepared permit은approved=false/expiry0인비활성상태입니다. 이것은승인부족이아니라로컬보안입력·필수gate가끝나기전활성permit을발급하지않는절차입니다.
- 실제ProgramData새trial/activepermit/launchmarker가없음을확인했습니다. **현재신규추론0·usage0·소비0·배포0**입니다.

## 격리 패키지·이전 자료 보호

새47파일ZIP: 기존검토runtime44(미변경40 bytes) + roles1 + 후보entry/core2. 기존운영43파일SHA/공식전체45패키지SHA/기존PS보안모듈/역할파일SHA를필수무결성gate로확인했습니다. 기존테스트전체를실행한것은아닙니다.

| 산출물 | SHA256 |
| --- | --- |
| 새 inference-package.zip | `e5315fcc4da62f30baa543898073c2dd005d8657bced4f3dba865f23dc61b82e` |
| 최종 Manifest.json | `5de623d338ccca50fe736f21127353e4f315ab90d50b8f3bc775329dfc52c293` |
| 후보 entry | `b841cf6557046ada96b67564be7e093dcb0666980577de8ab37273dd0c22b1ab` |
| 기존 core | `c9fa6329849ed28713bbfb0fb2f7ded043a04c721c02c10c80fb71d5206adcdf` |
| 기존 90초 Job | `93a9cd26ee7bf268e9327cb10bed92cc27d273ffc697e9da5b679675cca846cf` |

위ZIP/Manifest는 `Temp/DiscordUsageDiagnostics20261026/InferenceTrial90AuthFixPrepared20261026/`에만준비했습니다. 운영배포는없습니다. 기존717a trial·이전prepared ZIP5019b497/Manifest5607ae30·사용량소비marker2개·717a소비marker를그대로보존했습니다. 새Manifest는이전marker **3개** SHA를전후검사합니다.

## 실행기 변경과 최소 검증

기존성공한90초감독/등록/서비스토큰/정리절차를복사한**새전용** `run-approved-inference-authfix-once90.ps1`을사용합니다. 이전launcher는수정하지않았습니다. 바뀐부분은새prepared경로/새child파일명/승인범위label과서비스keyring증거SHA·정확후보entry SHA 선행gate뿐입니다.

기존prepare도구는명시된두launcher basename과이전717a marker만받는옵션을추가했고,새authfix준비도구가새디렉터리에서만호출합니다. 기존준비디렉터리가존재하면새request를자동재발급하지않고HOLD하는정책은동일합니다.

| 변경/신규 파일 (Tools/DiscordAIServiceDiagnostics) | 현재 SHA256 |
| --- | --- |
| `run-approved-inference-authfix-once90.ps1` | `a9986f29c4a6ee8dc4d52439a86fce3cf451ac3f4e03b1daaae4d1ede0ec726a` |
| `prepare-inference-authfix-once90.py` | `c8acaf5c5f5ac6a16216d6a41dfebfd91f1e654e2c9fcfe4d04fd3ec233f9f60` |
| `prepare-inference-once90.py` | `58b9791c988ec2d4daa16a4b659a9c802455143572eebbe5d0e28f529bd7adb7` |

신규wrapper구문/자동변수·switch충돌0 PASS, 비관리자가드 PASS, 새request/ZIP47집합·SHA PASS, 이전marker3 SHA PASS, 정확d9fa entry·비활성permit PASS. **기존38건·native7·예산·22+12·제품전체검사재실행0**, 기존PASS를재사용했습니다. 후보보안정책·검증기·Sandbox·운영승인목록을완화하거나확대하지않았습니다.

## 바울 대표의 정확한 관리자 직접 명령

관리자PowerShell에서아래명령을**한번만**실행합니다. Windows서비스계정암호는로컬보안입력에만입력하며채팅/Discord로보내지않습니다. 이는새승인된trial에대한명령이며과거717a명령을다시실행하는것이아닙니다.

```powershell
pwsh -NoProfile -File "F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\run-approved-inference-authfix-once90.ps1"
```

승인SID/Session0/logon4·Password/Limited·새패키지/SHA·기존승인·MANUAL·currentconfig를통과한후공식사용량AppServer최대1개를먼저종료하고추론exec최대1개를시작합니다. LIMITLESS자동재실행0·SchedulerRestartCount0,공식builtin HTTP/stream기본재시도허용·원격서버요청/구독소비정확1회보장불가를유지합니다.

Task90초/준비경과차감Job최대75초/worker전체최대70초/로그3초예약·부모대기80초후정리구조는동일합니다. 관리자파일배포·사람의암호입력대기는서비스실행예산과구분하며OS지연까지90.000초에완료를보증하지않습니다. 시간초과나정리미확인은HOLD/NOT_VERIFIED. 원격접수요청은로컬kill로회수할수없습니다.

응답은정확히 **LIMITLESS 기획 담당 AI입니다. 읽기 전용 연결 시험에 성공했습니다.** 입니다. 실패하면보호프로필의DPAPI원본/정제오류·stderr·exit·단계증거를보존하고재시도하지않습니다. 성공/실패와무관하게새permit을비활성보존본으로옮기고해당임시Task만제거하며Job자식0과기존보호결과를확인합니다.

## 실제 결과 확인과 조건부2단계

이번agent에서는관리자실행을대신하지않았습니다. 실행뒤실제결과는아래파일입니다.

`C:\ProgramData\LimitlessRelay\InferenceTrials\d56fdba9618044b98f98fe98e4cff211\Output\summary.json`

전달JSON만의statuslabel이아니라로컬summary·요청/실행소스SHA·자식실행계정/Session을대조하고, 아래를모두확인해야1단계VERIFIED입니다.

1. 추론CLI process.exit_code0, Worker/Job/Taskexit0.
2. 정상turn.completed정확1.
3. 실제고정최종응답존재.
4. validation.status PASS·금지항목/오류없음.
5. 승인read-only·도구차단·정확후보/SHA·keyring/Session0범위준수.
6. 승인/소비marker·부팅/장부/기존파일·권한보호,permit비활성·TaskREMOVED·Job자식0/정리확인.

실제VERIFIED가확인되면 **추가지시를기다리지않고즉시2단계오프라인구현·검증**을진행합니다: 모바일Discord → 승인SQLite업무큐 → 기존검증4역할(PlanningAgent/CodexAgent/아트담당연결코드재사용) → 결과검증 → Discord보고. 승인·권한·한도/역할인계·중복실행방지/실패HOLD·체크포인트/모바일승인명령을가짜실행기로검증합니다. 추가실제모델·A활성화·운영봇/아트재시작·변경은하지않습니다.

**현재는실행결과없음/1단계미시작이므로2단계도미착수**입니다. 실패라면자동재시험/2단계착수없이HOLD. 과거실패717a의NOT_VERIFIED는새시험의VERIFIED증거로대체되지않습니다.

## 보호·기록

MANUAL/AI_EXECUTION_HOLD유지, 기존봇/아트/OnBoot/SQLite/인증/CLI/권한/Unity·게임·아트·Save변경0, Push/main병합/PC종료·재부팅0. 추가공식CLI/config조회도이번0이며파일SHA·구문·패키지scope만확인했습니다. 새실전permit은대표직접실행에서만활성화됩니다.

브랜치 `codex/discord-usage-diagnostics-20261026`, 실제후보commit d9fa4f7c4e7eeb8f78c34ab6bc19ec92d4595635. 이번준비commit은Issue66/최종보고참조. `Evidence/Inference90AuthFixPrepared20261026/`에비활성Manifest/Preparation/ReadyCheck보존·CURRENT_STATUS/CODEX_LATEST갱신. **LOCAL_COMMITTED / REMOTE_NOT_PUSHED / OWNER_ADMIN_LAUNCH_PENDING**.
