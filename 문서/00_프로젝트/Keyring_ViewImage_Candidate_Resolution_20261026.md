# Issue66 현재 keyring 확인·view_image 제어 후보 갱신 (2026-10-26)

**OFFLINE_CANDIDATE_READY_FOR_SEPARATE_TRIAL_REVIEW / NOT_DEPLOYED / STAGE1_NOT_VERIFIED / STAGE2_NOT_STARTED.**

## 이번 확인 결과

서비스계정의기존로그진단 review `93fa150e749d4d6ea661d6040e3aed8b`는exit0·Session0/logon4·원본로그불변·Job자식0/정리true·임시Task제거로성공했습니다. 실제projection SHA **20b0e1b09c31b3f772b46f773448e4d30662482a839c21e1e0f58816824fcb15**가전달값과일치합니다. 외부에는서비스가정제한결과만읽었고원문/계정비밀은복사하지않았습니다.

현재서비스config SHA **818c2e7b52f1c574fd1f9e0ac49dd876ad10746ff5ad95ce427d636938747bc3**는과거승인기록과동일하고, **cli_auth_credentials_store=keyring / forced_login_method=chatgpt**입니다. 현재시점의읽기전용메타데이터이며실패시점의직접스냅샷으로소급단정하지않습니다.

기존717a 오류10개와turn.failed의HTTP401/인증헤더누락은동일하게확인됐습니다. 시작전error item2개는 **tools.view_image의unrecognized/deprecated설정무시** 진단입니다. 나머지error item은WebSocket→HTTPS전환실패입니다. 세item은실제도구실행이아닙니다. 완료turn0/agent_message0이며실제추론성공은여전히없습니다.

## 승인CLI의 무호출 설정 비교

현재고정공식CLI **4fd2f057bad603bf53dc202457ffd0c3591de62e867a8b01e73816e3fbce5a73**를인증없는새Temp CODEX_HOME에서 **features list만2회**실행했습니다. 서비스로그인·auth.json·keyring내용·AppServer/usage·exec는호출하지않았습니다.

| 설정조회 | 관측값 |
| --- | --- |
| tools.view_image=false | **view_image stable true**. 이고정버전에서비활성화가적용되지않음 |
| features.view_image=false | **view_image false**, exit0·stderr비어있음 |
| 최종차단옵션집합 | shell_tool/apps/browser_use/computer_use/multi_agent/view_image/image_generation/sleep_tool/tool_suggest/hooks/memories 모두false |

현재[공식설정문서](https://learn.chatgpt.com/docs/config-file/config-reference)는tools.view_image를안내하지만, **해당고정바이너리의실제동작이문서와달랐습니다**. 실제서비스진단의무시경고와무인증config조회결과를함께근거로합니다. 새provider나버전교체없이기존CLI가인식하는features키를사용했습니다. features상태조회는실제exec의최종도구목록·추론성공까지증명하는실전검증은아닙니다.

## 격리 후보 변경

이전commit67216003의 **기존서비스config SHA일치gate+keyring선택명시**는유지합니다. 현재실제설정메타데이터가이gate와일치함을확인했습니다. ignore-user-config 때문에저장소선택이생략된설계차이는확인됐지만, keyring명시로401이해결되는지는새실전전까지미검증입니다.

추가수정은격리entry에한정합니다.

- 무시되는 tools.view_image=false를 **features.view_image=false**로대체. 옵션을없애서이미지읽기를허용하거나warning을숨기지않습니다.
- 지원되는 **features.image_generation=false / features.sleep_tool=false / features.tool_suggest=false**를추가하여불필요한도구를명시적으로차단합니다.
- read-only/approval never·기존shell/apps/browser/computer/multiagent/MCP/plugin/web차단·정확문장/JSONL/완료turn검증·승인SHA/소비marker gate를변경하지않았습니다. error item을허용목록에추가하지않았습니다.

인증설정·키링·토큰·CODEX_HOME·공식CLI·운영runtime44/기존43·봇/아트/부팅프로그램은수정하지않았습니다. 실제서비스config도읽기만했습니다.

## 검증 결과와 변경 SHA

**변경entry 회귀38 PASS / 0 FAIL**, 명령에새지원키들이들어가고구무시옵션이없음을확인했습니다. 기존정확응답/오류/도구/timeout/승인·SHA·keyring설정변경/usage실패차단도유지됩니다. 추가 **설정전용비교2건PASS**(구옵션무효/신옵션11개차단확인). 이전projector17·native7·예산1·22+12/제품전체검사는재실행하지않았습니다.

| Tools/DiscordAIServiceDiagnostics | 전SHA256 | 후SHA256 |
| --- | --- | --- |
| `inference-once90.py` | `058d6274cdc430eff86eaae7eccf33256681ec4175dc934e3290b3f869fdd2cc` | `b841cf6557046ada96b67564be7e093dcb0666980577de8ab37273dd0c22b1ab` |
| `test_inference_once90.py` | `880670aba528d3da354307077045e6fc262e06b9ebc78e1c5518df2fb7d02071` | `e8c61f571ac625f584cd35e4d47456bd04f99b01ff8bdb09e77ca55dfba42d81` |

상세원본정제projection/summary·두config결과·Assessment는 `Evidence/KeyringViewImageResolution20261026/`에보존했습니다. 이전진단·717a로그·소비marker·prepared ZIP/Manifest는덮어쓰지않았습니다. 이번새실전패키지/request/permit은발급하지않았습니다.

## 다음 단계와 승인 경계

추가모델/공식usage0, 이번실행한공식CLI는**인증없는features list2회뿐**입니다. 운영배포/새실전Task/모델승인소비/인증이전·재설정/기존ACL/봇·아트/OnBoot/SQLite/Unity·게임·Save/Push/main병합/PC종료·재부팅변경0. 기존진단summary의보호결과true·TaskREMOVED를근거로하며새실시간건강PASS를주장하지않습니다. MANUAL/AI_EXECUTION_HOLD유지.

현재후보는오프라인수정완료·미배포·실전미검증입니다. **이전모델명령을다시실행하지마세요.** 717a는소비완료이며기존prepared ZIP에는새후보가없습니다. 다음실전이필요하면현후보diff·새패키지/SHA·새요청·90초/사용량AppServer1+exec1/재실행0범위를구체화하고**별도신규승인**을받은경우에만진행합니다. 현지시로자동재시험하지않습니다.

1단계는아직NOT_VERIFIED이므로2단계미착수입니다. 향후별도승인실제1단계가exit0·정상turn·최종응답·JSONLPASS·읽기전용준수·기존보안보호6조건모두충족하면 **추가지시없이2단계오프라인연결구현·검증즉시착수**하는대표승인은계속유효합니다. 실제추가모델/A활성화/운영봇변경은포함하지않습니다.

브랜치 `codex/discord-usage-diagnostics-20261026`, 기반 `6721600339f08e657786693af8552c940cee2d62`. 최종로컬commit은Issue66/최종보고를따릅니다. **LOCAL_COMMITTED / REMOTE_NOT_PUSHED / CANDIDATE_NOT_DEPLOYED / NEW_LIVE_APPROVAL_REQUIRED**.
