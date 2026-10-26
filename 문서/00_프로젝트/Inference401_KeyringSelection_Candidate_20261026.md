# Issue66 실제 추론 HTTP401 원인 확인 / keyring 선택 수정 후보 (2026-10-26)

**DIRECT_AUTH401_CONFIRMED / CANDIDATE_OFFLINE_PASS / NOT_DEPLOYED / STAGE1_NOT_VERIFIED / STAGE2_NOT_STARTED.**

바울 대표의로그전용작업 review `2a87401a0faa4b0b989870c9b3096930`는서비스Session0/logon4에서exit0·원본로그SHA불변·프로세스0·임시Task제거로성공했습니다. 전달SHA와실제로컬projectionSHA **2adef4fd1fbd77b1fcd8748b84a677169d2915993229d68f08fb6571a24be41d**가일치합니다. 이성공은로그조사성공이며추론1단계VERIFIED가아닙니다.

## 직접 확인한 실패 원인

오류10개와turn.failed는모두 **HTTP401 Unauthorized / Missing authentication**입니다. JSONL의구조화error_code는정제결과null이며추정하지않습니다. HTTPstatus401은별도로추출된실제값입니다.

| JSONL 줄 | HTTPstatus | 구분 |
| --- | --- | --- |
| 5 | 401 | 재연결 알림 |
| 6 | 401 | 재연결 알림 |
| 7 | 401 | 재연결 알림 |
| 8 | 401 | 재연결 알림 |
| 10 | 401 | 재연결 알림 |
| 11 | 401 | 재연결 알림 |
| 12 | 401 | 재연결 알림 |
| 13 | 401 | 재연결 알림 |
| 14 | 401 | 재연결 알림 |
| 15 | 401 | 최종 오류 |

line16 turn.failed도동일401인증누락입니다. 줄9 item완료는WebSockets에서HTTPS transport로전환하며같은인증오류를기록했습니다. stderr도WebSocket연결실패HTTP401을반복합니다. 따라서단순timeout·씬/Save·출력검증기때문에모델응답이숨겨진상황으로볼수없고,추론요청이인증실패로끝난것은확인됐습니다.

- FORBIDDEN_ITEM3의실제타입은 **error/error/error**. 줄2·3은turn.started이전의설정관련진단, 줄9는시작후transport전환오류입니다. **도구실행항목이아닙니다.** 기존검증기의error/미지item거부는그대로두었습니다.
- 실제CLI exit1·timeoutfalse·완료turn0·agent_message0·최종응답없음. 원격의성공한모델응답증거는없습니다. 원격요청수·소비량은UNKNOWN이며모델사용량0으로단정하지않습니다.
- 실제launchmetadata: CLI SHA4fd2f057…/기존builtin openai/forced chatgpt·read-only·approval never·도구차단인수존재, ignore-user-config=true/ephemeral=true/provider override=false. 인수존재만으로Sandbox실제강제나exec인증성공을확정하지않습니다.
- 원래CLI stderr1239bytes, 이번서비스정제stderr파일1246bytes는정제후문자열크기가달라진것입니다. 원본DPAPI JSONLSHA대조와로그5개전후SHA일치가확인됐습니다.
- 정제기한개의QUOTA_OR_RATE_LIMIT 신호는HTTP429증거가아닙니다. 기존분류기가무맥락부분문자열429도검색했으므로식별자숫자등에오판가능했습니다. 실제HTTPstatus는10개모두401이며이진단분류버그만고쳤습니다. 기존projection파일은수정하지않았습니다.

## 인증 경로 차이와 최소 수정 후보

기존공개서비스인증결과 service-codex-auth-local.json은AUTH_AND_USAGE_VERIFIED·설정SHA **818c2e7b52f1c574fd1f9e0ac49dd876ad10746ff5ad95ce427d636938747bc3**를기록합니다. 승인인증스크립트가만드는두줄(keyring/forced chatgpt, CRLF)의SHA와일치합니다. 이과거기록의CLI는교체전바이너리였으며현재config의직접스냅샷은아닙니다. 현재CLI/인증을과거버전으로되돌리지않습니다.

[공식인증문서](https://learn.chatgpt.com/docs/auth)는keyring을OS인증저장소로설명하고사용불가시실패한다고명시합니다. [공식비대화형문서](https://learn.chatgpt.com/docs/non-interactive-mode)는ignore-user-config가CODEX_HOME/config.toml을읽지않는다고설명합니다. [공식샘플](https://learn.chatgpt.com/docs/config-file/config-sample)은기본저장소file을표시합니다. **forced_login_method=chatgpt는인증방식제한이지keyring선택을대체하는설정이아닙니다.**

사용량AppServer는설정을읽지만추론명령은ignore-user-config를쓰면서keyring선택을재지정하지않았습니다. **실제인증누락401은확정, 이저장소선택누락이유일한물리원인인지는아직미검증**입니다. 서비스현재config/키링실제조회상태를읽었다고주장하지않습니다.

격리entry만다음과같이수정했습니다.

1. 실제서비스SID확인과기존permit/CLI/44개SHA검사후,서비스자기config.toml을최대4096bytes읽어**과거승인SHA와정확히같을때만**진행합니다. 누락·SHA차이면usage/모델전HOLD.
2. 기존저장소선택 **`-c cli_auth_credentials_store="keyring"`**만명시합니다. file/keyring전환·인증재설정·토큰/키링내용읽기·복사·로그인·API/URL전환은없습니다.
3. ignore-user-config·read-only·never·금지도구차단·기존검증기·승인게이트를그대로유지합니다. 사용량reader·runtime44·운영코드도수정하지않습니다.

**후보는미배포·실전미검증**입니다. 시작전설정진단item2개도성공판정에영향을줄수있으므로검증기를완화하지않고정확한설정키/내용부터확인해야합니다. keyring한줄만으로모든실패가해결됐다고보고하지않습니다.

## 남은 오프라인 자료 확인 — 새모델시험아님

로그정제기에서인증헤더/기본저장소/무시된설정에관한진단어휘와고정설정키식별을보완했습니다. 서비스계정안에서현재config.toml의 **SHA·저장소enum·강제로그인enum만**추출하고내용/인증파일/비밀은외부로내보내지않습니다. 현재값은현재시점메타데이터로표시하며실패시점값으로소급단정하지않습니다. 이파일도전후SHA불변을검사합니다.

다음명령은기존에승인된로그/실행환경읽기뿐이며 **모델0·usage0**입니다. 지금agent는비관리자라대표의관리자로컬서비스암호입력이필요합니다. 추가AI재시도승인을묻는것이아닙니다.

```powershell
pwsh -NoProfile -File "F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\review-existing-inference-log.ps1"
```

원본717a로그·이전review/소비marker는보존하고새진단review폴더에정제결과만둡니다. 현재서비스설정과설정경고키확인후최소후속수정여부를결정합니다.

## 검증·SHA

**55 PASS / 0 FAIL = 변경entry 관련38 + 정제기17**. 정상경로에서도keyring선택이들어가고설정누락/변경시usage0·exec0, 잘못된문장/도구/error/시간초과/승인·SHA·재소비차단이유지됨을합성데이터로확인했습니다. HTTP401+식별자숫자429를quota로오판하지않고명시적HTTP429만구분합니다. 서비스config는합성두줄fixture만썼고실제서비스읽기는추가실행하지않았습니다. 기존native7/예산1/제품·22+12전체PASS는재실행하지않았습니다.

| 변경파일 (Tools/DiscordAIServiceDiagnostics) | 전SHA256 | 후SHA256 |
| --- | --- | --- |
| `inference-once90.py` | `14a502cd36ec2d54d6f381212bad8125aecc503f76bba1adf53d7f0d7f99a367` | `058d6274cdc430eff86eaae7eccf33256681ec4175dc934e3290b3f869fdd2cc` |
| `project-existing-inference-log.py` | `0a11e451d023b31197535bbaddcb1dc33db171133af22e910b781bf586454be9` | `bee6ce254079cb5ad61ffff898c0940affa20229b5908f6902132475c34aa4fe` |
| `test_inference_once90.py` | `6040bbb25c002a3655e02e884d5cf2c7a2326c169f0bed0afabed371aff2c93d` | `880670aba528d3da354307077045e6fc262e06b9ebc78e1c5518df2fb7d02071` |
| `test_existing_log_projection.py` | `42fd354fcc09392ec4ba4deb2a358ec0efebea27c240d840b37215914ee25f8c` | `297099bb6ece84e286660cf954f9553afa639fcafb59c7cd33b47633a6f63b37` |

이전717a배포본·prepared ZIP5019b497…·Manifest5607ae30…는**변경하지않았습니다**. 후보entry와기존ZIP의SHA가다르므로기존패키지/명령을후보재시험에사용하면안됩니다. 기존717a모델승인은소비됐으며새모델request/permit/실전패키지를발급하지않았습니다. 새로운실전이필요하면후보diff/새패키지·SHA/범위를다시명확히하고대표의**별도신규승인**을받아야합니다.

## 보호·조건부후속

이번실제모델/공식usage/CLI실행/새실전Task·permit·slot0, 운영배포/기존승인SHA/계정·ACL/인증/OnBoot/봇·아트/SQLite/Unity·게임·Save/Push/main병합/종료·재부팅변경0. 기존로그진단summary의source_logs_unchanged/ledger/boot/approval/717a소비marker보호true·TaskREMOVED·서비스Jobexit0/자식0을확인했습니다. MANUAL/AI_EXECUTION_HOLD 유지.

현재1단계NOT_VERIFIED이므로2단계미착수. 추후**별도승인된실제1단계가6조건모두VERIFIED**이면추가지시없이2단계오프라인연결구현·검증을즉시시작하는대표승인은계속유효합니다. 실제추가모델/A/운영봇변경은허용되지않습니다.

브랜치 `codex/discord-usage-diagnostics-20261026`, 기반 `446b62d1c5c21e261847ce0fa1da725a38b3e567`. 최종로컬commit은Issue66/최종보고참조. 증거 `Evidence/Inference401KeyringCandidate20261026/` projection/summary/Assessment. **LOCAL_COMMITTED / REMOTE_NOT_PUSHED / CANDIDATE_NOT_DEPLOYED / NEXT_READONLY_DIAGNOSTIC_WAIT_OWNER**.
