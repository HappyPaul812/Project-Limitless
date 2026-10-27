## 2026-10-27 Issue66 Task GR/FR 검증기 격리 후보 — 신규48PASS / 실제Windows재검증0

기준97028645/기획6440747926·고정Admin JSON/기존2PASS 재사용. 새 TaskSecurityCandidate C# Task전용 계약: 객체/정확Task/SID/고정snapshot 근거 필요·GR만 파일읽기0x120089로정규화/추가비트삭제0·exact OwnerGroup/보호DACL/ACE3·순서·SID·기존FA유지. 쓰기삭제실행/WDAC/WOWNER/unknown/추가ACE등거부. 신규48PASS0FAIL·snapshot후보PASS(LiveVerified/RuntimeAdmittedfalse), 원설치HOLD/HRESULT별도보존. 기존 DedicatedContract/r2 ZIP/설치계정TaskACL/Stage/승인소비 불변·실콜사이트연결0·관리자조회/설치/IPC0. 문서 Discord_DedicatedV2_TaskSecurityCandidate_20261027.md에파일SHA/근거/후속Task재등록없는읽기검증절차/HOLD조건. V1/MANUAL/A/runtimeHOLD·Push0, 최종로컬SHA Issue후속.
## 2026-10-27 Issue66 r2 관리자 진단 판독 완료 — Task PRESENT / SDDL 재현불일치 / HOLD 중지

대표 Admin-33a677… JSON 확인(SHA4de232b3…): query45=READ_OK44/UNKNOWN1(Operational Disabled), payload766 SHA/CodeACL 모두일치·파일768/누락예상외0/Auth빈상태. Bot1009Disabled·Worker1010Enabled/Users·직접Batch+deny4, OnBoot2 pin일치/기존1007Enabled1008Disabled. Task PRESENT/Disabled/Trigger0/90초/Principal정상·Action비교true. 기대GR0x80000000 vs 실제FR0x120089, header일치/right불일치. 고정Contract에 관측SDDL 넣어 D3_SD_RIGHTS와 원설치예외체인 재현(신규오프라인2PASS). 과거throw태그/실등록시각은 로그미활성으로 미확정. 설치/복구/재시도0·소비증거보존, Task전용권리표현계약 수정방향만 기록. 상세 Discord_DedicatedV2_R2_Readonly_Review_20261027.md/AdminReviewSummary.json. 실제토큰/GROUP/IPC/Job·V1/MANUAL/A HOLD 유지, 전체운영보존감사주장0/Push0. 최종 로컬SHA Issue후속.
## 2026-10-27 Issue66 읽기 진단 ScopeFix — ScriptBlock 경로21PASS / 관리자 결과 대기

대표 실행에서 $script:r 미정의로 첫 읽기query 누적 실패·Task/계정/최종JSON 미도달. 원인 -File과 &ScriptBlock의 범위차이, 도구 결함. 구도구/hash보존·구명령사용중지. 별도 probe-r2-partial-v2.ps1 결과/기록/시험변수 script초기화 일치, 실제 EncodedCommand→ScriptBlock SelfTest21PASS0FAIL(OS조회변경0). 새명령/SHA Discord_DedicatedV2_R2_Readonly_ScopeFix_20261027.md. 관리자Collect/설치실행0·소비승인보존·Task/설치원인UNKNOWN·V1/MANUAL/A/runtimeHOLD. Issue후속SHA / REMOTE_NOT_PUSHED.
## 2026-10-27 Issue66 r2 관리자 읽기 진단 준비 — 17 PASS / Collect 미실행

기준 f6b61621, 최신Issue6439920038 재사용·보호Entry/Result Codex열람거부로 원본판독미완료. 새 R2Readonly/probe-r2-partial.ps1(기본SelfTest/명시Collect) 준비, 신규17PASS0FAIL/C#컴파일·명령구문PASS. Task COM 단계별 GetTask/숨김목록/SD·Action비교·정제이벤트, NetAPI/LSA 신규계정권리, 새설치766 SHA/ACL/Lease/승인소비, OnBoot2·기존SID·보존루트metadata 읽기만. 실제관리자Collect0/설치재실행0/변경0. Task/직접원인/권한·부분파일은 UNKNOWN 결과대기, existing_preserved 보고와 독립감사 분리. 명령·SHA·한계·복구조건 Discord_DedicatedV2_R2_Readonly_20261027.md. 재시도·복구·설치·IPC금지, V1/MANUAL/A/토큰GROUPJob HOLD. LOCAL_COMMITTED / REMOTE_NOT_PUSHED, 최종SHA Issue후속.
## 2026-10-27 Issue66 r2 부분 설치 HOLD — 승인 소비됨 / 재실행 금지

대표 결과 install1/consumedtrue/account2(Bot1009·Worker1010)/Task시도1, CREATE_ONLY_DISABLED_TASK에서 MethodInvocationException→InvalidOperationException(-2146233079). Task 등록 및 SDDL/readback 중 직접 실패 위치 미확정·Task 존재/Disabled/그룹권리/파일 최종감사 미완료. Codex 읽기조회 Task AccessDenied(-2147024891), 계정조회 NullReference(-2147467261) → UNKNOWN. 실패를 부재/PASS로 추정하지 않음. 기존보존true는 native 보고·최종감사완료 아님. 설치/Stage/Resume 재시도·삭제·권한완화0, IPC/model/authimport0. 보호 Entry/Result 및 부분자산 보존 후 관리자 읽기전용 감사 필요. 상세 Discord_DedicatedV2_R2_Install_Hold_20261027.md. V1/MANUAL/A/토큰GROUPIPCJob HOLD 유지, 기존 운영변경/Push0. 최종 로컬 SHA Issue 참조.
## 2026-10-27 Issue66 r2 설치 전용 실행 준비 — 20 PASS / 직접 관리자 실행 대기

r2 고정 Package3eff…/3SHA 유지. 새 R2Install 실행기는 고정 core/native API 직접 사용·consumeDirectory/Consume 객체 구성 검증, 신규 승인/epoch/단발소비·Stage Handle/metadata/전체 SHA·과거증거 fingerprint·생성 직전 재검증 유지. 신규20PASS/0FAIL + 실제 Inspect13메서드/766payload PASS. 계정/Task/승인/암호/설치 실행0. 대표 직접 명령은 Discord_DedicatedV2_R2_Install_20261027.md. 실패 시 HOLD/증거보존/재시도삭제0. r1/r2Stage 재실행금지. 실제SID/권리/Task는 결과대기, 유효토큰/GROUP/IPC/Job 및 V1/MANUAL/A HOLD 유지. 기존 운영 보호·Push0. 최종 커밋은 Issue 참조, LOCAL_COMMITTED / REMOTE_NOT_PUSHED.
## 2026-10-27 Issue66 r2 보호 Stage 성공 수신 — 생성·검증PASS / 미설치

[Issue 실제 보고](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6438760726), 결과·문서만 갱신 / REMOTE_NOT_PUSHED.

대표JSON R2_STAGING_CREATED_VERIFIED_NOT_INSTALLED/installedfalse, 3eff…Bundle/Zip·767파일/생성시도768·전체metadataSHA/exactbytes true·고정3SHA일치. 실제OwnerBA/GroupSY·protected SDDL·FileID00730000000311c1/volume ca54aa62 제공, r1 metadata보존/r2쓰기제한true. 오류/부분실패증거없음·계정Task승인IPC/model/retry0. AdminStageUserProvided.json 값동일재직렬화·독립native재조회/전체raw목록주장0, Stage/시험재실행0·같은명령반복금지. 상세R2_Staging보고. 다음계정2/Task1/승인소비·설치별도승인/직전Stage재검증필요·신규SID토큰GROUPIPCJob/V1/MANUAL/A/runtimeHOLD·Push0.

## 2026-10-27 Issue66 r2 Stage 준비PASS — 신규25PASS+실제Inspect / 관리자실행대기

고정r2 3SHA/package3eff…/766+Manifest1·별도R2Staging도구준비, 새보호대상 공통부모아래3effGUID/Bundle·Bundle.zip. 공유부모ACL변경0·r1/a5af…쓰기는C# 경계가거부, 모든생성경로r2하위만. 메모리25PASS/0FAIL(정확byte·r1sentinel/보호DACL·범위/링크/오류), 실제전체InspectPASS·Native컴파일/정확entry/SHA검증·creationfalse. 도구SHA87509891…/boundary7203eec5…·실제privateStage/OwnerDACL 관측아직없음. 대표관리자한줄직접1회안내·계정Task승인JSON/설치0·기존r1명령재실행/자산수정0. 정본 Discord_DedicatedV2_R2_Staging_20261027.md, V1/MANUAL/A/runtimeHOLD·운영보호·Push0.

## 2026-10-27 Issue66 r1 native 원인 확정·r2 오프라인 후보 — 8PASS+Inspect / 설치HOLD

대표readonly5PASS/실콘솔정상·계정Task소비없음·갱신승인만845f3f78… expired, Worker암호까지입력확인. 고정r1의 consume note property와 Consume ScriptMethod 대소문자충돌을실제adapter생성에서같은InvalidOperation/-2146233079/MemberAlreadyExists로재현. consumeDirectory저장속성최소rename, backend호출0·기존fake의native생성경로누락보완. r2 payload변경1개·766+1·새package3effaac3…/승인/epoch/pipe, 8PASS/0FAIL+InspectPASS. Manifest7c841014…/ZIP4efa0aa8…·Installer불변, r1/staging/기록보존·r2실staging/승인/설치0. r1명령재실행금지·새SHA후보는별도r2승인필요. 상세 Discord_DedicatedV2_R2_AdapterFix_20261027.md·V1/MANUAL/A/runtimeHOLD·Push0.

## 2026-10-27 Issue66 r1 native 설치 HOLD — guard·최신 생성상태 읽기 대기

대표두JSON nativeHOLD/NATIVE_PROTECTED_DEDICATED_STAGING/InvalidOperation(-2146233079), supervisor원본보존/새승인/markertrue·자식1회·retry0. CHILD_RETURNED는성공아님, 같은재개명령재실행금지. stage라벨은ReadHost/core뒤recheck까지같아계정미생성·정확실패지점단정금지. 새 probe-native-guard-readonly.ps1 SHA1d8cde5d…: 고정source guard상수allowlist만출력·stage/승인권한/정의로드/콘솔메타·소비/계정/Task현황읽기, 암호/설치/변경0·구문/guard/민감문자비출력PASS. 정본Recovery보고한줄·대표진단대기, 기존12/10/151재실행0·r1불변·V1/MANUAL/A/runtimeHOLD·Push0.

## 2026-10-27 Issue66 r1 재개 bootstrap 수정 — H alias충돌 재현·28PASS

대표READ_ONLY_BOOTSTRAP/ParameterBindingValidationException HOLD는해시함수H가Get-History기본alias에가려진wrapper결함, 비밀없는0/1/2입력동일오류FQID재현. Get-R1ResumeSha256로수정·단계태그세분화·alias/프로필변경0. 실제wrapper bootstrap ZIP/Manifest/assembly경로까지신규28PASS/0FAIL·승인/설치/암호실행0·151/12/10재실행0. 새wrapper SHA cc6357c8e071ebad6109d51a6b8f9c6f3d08d9aa177b0b6e15c15d057ac589a4/core/r1불변. AdminCommands4번핀교체, 실제관리자재개결과대기·원본승인보존/단발/오류HOLD·V1/MANUAL/A/runtimeHOLD·Push0.

## 2026-10-27 Issue66 r1 재개 — 관리자 미소비/무계정 확인·보존형 콘솔 도구24PASS

대표관리자진단: 만료승인2ee97768…만PRESENT·BA/SY DACL/동일관리자/고정r1일치, consume/lease/계정2/Task/CodeState없음·Result0. 원본값재직렬화AdminRecoveryUserProvided.json, 별도앞선재개요청에보존형단발도구준비. 만료승인원본SHA/DACL확인이동보존→보호ResumeMarker CREATE_NEW→새15분승인→수정없는r1Installer 콘솔직접1회·stdout수집0. 새fake24PASS/0FAIL·실제승인이동/발급/설치0·12/10/151재실행0. wrapper SHA2fdf7a49…/core01fa6f5f…·콘솔/소비/자산불명HOLD·중복차단. 정본Recovery보고·AdminCommands4번, 대표직접결과대기·envelope exit0는설치PASS아님·V1/MANUAL/A/runtimeHOLD·Push0.

## 2026-10-27 Issue66 r1 재개 요청 — 관리자 최소 읽기 감사 대기

대표재개요청에기존Install재실행0·소비삭제0. 실제approvalroot존재/내부접근거부확인, 새 probe-install-hold-readonly.ps1 SHA c34be6d7…·구문PASS·비관리자UNKNOWN9/CodeState부재2·changes0. 보호부모미확인은자식ABSENT로오인금지. 진단은승인binding/만료·소비/lease·신규계정/Task/CodeState/Result개수만읽고암호/생성/수정/설치0. 관리자JSON필요·HOLD_REQUIRES_RECOVERY_REVIEW, source/helper/r1불변·기존12/10/151재실행0. 정본 Discord_DedicatedR1_Recovery_20261027.md 한줄진단, 결과이후안전재개검토·V1/MANUAL/A/runtimeHOLD·Push0.

## 2026-10-27 Issue66 r1 설치 HOLD — 자식 IOException 0x80070006

대표최신JSON FIXED_INSTALLER_FRESH_CHILD/actual_creation_attempted=true·내부HOLD/NATIVE_PROTECTED_DEDICATED_STAGING·IOException -2147024890=0x80070006 invalidhandle. 암호오타증거아님, stdout수집자식의Read-Host 콘솔handle가능성·정확throw위치UNKNOWN. stage라벨이암호전후갱신되지않아staging실패로단정금지. helper순서상신규보호approval root/JSON도달, 소비/계정/Task실제상태는결과없어읽기감사필요. 같은설치명령재실행/승인삭제/암호변경0·HOLD, 이전reverify생성falseJSON과구분. 상세 Discord_DedicatedR1_Install_20261027.md. 코드/후보/운영변경0·V1/MANUAL/A/runtimeHOLD·Push0.

## 2026-10-27 Issue66 r1 계정2·Task1 설치 1회 승인 — 대표 관리자 실행 대기

기준93d9a702…·Install도구SHA c6a4b7b6…/고정r1 3SHA·보안코드확인, 기존Preflight12/Stage10/151재실행0·코드/후보수정0. 대표직접실행전Codex생성/설치0, 현재WAITING_ADMIN_INSTALL_RESULT·실제SID/계정/Task상태미확인. 내부stage767+ZIP/OwnerDACL/FileID/SHA·보존/충돌/승인소비검사는유지. 로컬암호SecureString2회·BotDisabled/새WorkerBatch-only/TaskDisabled Trigger0/FAKE_ONLY EMPTYauth 계약, 성공/HOLD 후중지·재시도/자동삭제0. 상세 Discord_DedicatedR1_Install_20261027.md·관리자문서3번만안내, 계정실행토큰/GROUP/Session0IPC/Job독점·V1/MANUAL/A HOLD유지·Push0.

## 2026-10-27 Issue66 r1 관리자 Staging 성공 보고 — 생성·검증 완료 / 미설치

[Issue 실제 보고](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6435141316), 로컬 문서·제공 JSON만 갱신 / REMOTE_NOT_PUSHED.

대표 제공 Stage JSON admin=true/actual_creation_attempted=true·STAGING_CREATED_VERIFIED_NOT_INSTALLED·내부10PASS·retry/IPC/model0. 고정 Bundle경로 확인, native 성공계약상 payload766+Manifest1=767/별도Bundle.zip·SHA·BA/SY DACL/owner·handle/FileID/경로/링크검증·복사본SHA일치 완료 보고. raw개별metadata/원본JSON경로·시각·도구SHA미제공·독립재조회0을명시. AdminStageUserProvided.json 보존, 상세 Discord_DedicatedR1_Staging_20261027.md. 실패/복구필요증거없음·생성재실행/삭제0, 151/Preflight재실행0. 다음 실제설치 별도승인·직전stage재검증필요. 계정/권리/Task/설치승인JSON/설치/로그인/IPC/AI/usage/Discord/Push0·신규SID/토큰/GROUP/Job독점6개deferred·V1/MANUAL/A/runtime HOLD 유지.

## 2026-10-27 Issue66 r1 보호 staging 1회 승인 — 관리자 직접 실행 대기

대표가Stage만1회승인, 기존문서2번명령의도구SHA c6a4b7b6…·코드분기확인. 원본r1의3SHA·payload766+Manifest1/모든SHA확인, Preflight12/151시험재실행0·후보/도구수정0. Codex생성실행0·대표결과미수신으로WAITING_ADMIN_EXECUTION, 실제Owner/DACL/767복사파일·source일치·부분생성/복구필요여부는미확정. 고정경로/기대SDDL·보고계약은 Discord_DedicatedR1_Staging_20261027.md, 관리자문서2번한줄안내. 실패시HOLD·재시도/삭제0. 계정/권리/Task/설치승인JSON/설치/IPC/AI/usage/Discord/Push0·V1/MANUAL/A HOLD·기존Worker11/운영보호, 설치별도승인필요.

## 2026-10-27 Issue66 r1 관리자 Preflight 제공 결과 — 12PASS / runtime HOLD

[Issue 실제 보고 완료](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6434820211), 로컬 문서·제공 증거만 반영 / REMOTE_NOT_PUSHED.

대표 제공 JSON admin=true·12개 고유 검사 모두PASS·PASS_READONLY_SNAPSHOT/PREFLIGHT_PASS_POSTINSTALL_CHECKS_PENDING. 값 동일 재직렬화 AdminPreflightUserProvided.json 보존, 원본디스크경로/실행시각/도구SHA 미제공·독립실행주장0. 기존비관리자5/7은이력보존. 실제생성시도false·IPC/AI/retry0, r1/151/도구변경·진단재실행0. 다음은staging생성만별도승인, 설치승인아님. 신규SID/Batch토큰/GROUP/설치readback/Job독점은deferred·runtimeHOLD, V1/MANUAL/A HOLD유지. 실제staging/승인/계정/ACL/Task/설치/로그인/IPC/Push0. 상세 Discord_DedicatedR1_InstallPreparation_20261027.md.

## 2026-10-27 Issue66 전용 r1 설치 사전 준비 — 비관리자5PASS/7HOLD, 실제 변경0

사전 준비 커밋 `862473f7e66f010acafbe7a99750200fa55a2a64`, [Issue 실제 보고](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6434754454). LOCAL_COMMITTED / REMOTE_NOT_PUSHED.

r1 Installer/Manifest/ZIP 고정, payload766+Manifest1 읽기 검증. 기존151 재실행0·후보/기존DedicatedV2 수정0. 외부 R1Preparation helper/관리자 한줄3개/설치전·설치후·실IPC전 계약을 준비했습니다. 새 절차 정적10PASS/0FAIL·PS/명령 구문PASS. 현재 좁은 읽기 사전검사5PASS/7HOLD(admin=false, 계정provider NullReference·Task접근거부·native부모admin guard), 미확인PASS처리0. 새 SID/그룹/Batch 유효토큰은 설치 후 필수이며 실행 가능한 GROUP에 대한 r1의HOLD guard 유지. 설치완료는 IPC가능 판정이 아닙니다.

다음 최소행동: 관리자 PowerShell7에서 읽기전용 Preflight 한줄만 실행·정제JSON 판독. Stage/Install은 각각 후속승인, 현재 실행0. 상세 Discord_DedicatedR1_InstallPreparation_20261027.md·Discord_DedicatedR1_AdminCommands_20261027.md, Evidence/DedicatedR1Preparation20261027/. 실제 계정/권리/ACL/Task/staging/승인파일/설치/IPC/AI/usage/Discord/Push0, Worker1007/11개·V1/r11·기존운영/인증/OnBoot보호, V1/MANUAL/A HOLD. 로컬커밋·Issue실제보고는 후속 연결, REMOTE_NOT_PUSHED.

## 2026-10-27 Issue66 전용 Worker V2 후보 — 신규151 PASS / 실제 설치·독점성 HOLD

구현·검증 로컬 커밋 `60fc6484e9a77b12f52b527b3b18eeb5161d6113`, [Issue 실제 보고](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6434471862). LOCAL_COMMITTED / REMOTE_NOT_PUSHED.

기존 SID1007/11개/Task4·OnBoot2/r11을 수정하지 않고 DedicatedV2 신규 소스와 별도 schema3 후보 r1을 구현했습니다. Bot LIMITLESSBotV2D·Worker LIMITLESSWorkV2D는 OS 신규 SID만 허용, 전용 Code/State/Auth/CODEX_HOME/Task/IPC/승인/consume/lease, FAKE_ONLY/approved=false/auto_start=false. payload766+Manifest1·소스동일성·전체 SHA/ZIP·통합C# 컴파일/PS·Python 구문 확인. fake 설치/staging71+fake runtime/Job67+패키지13=151PASS/0FAIL. 상세 Discord_V2_DedicatedWorker_20261027.md 및 Evidence/DedicatedV2_20261027/에 전체SHA·r11차이·시험·인계 기록.

새 Worker는 미래 설치시 Disabled 생성→최소권리/보호파일→별도 승인된 ENABLED_BATCH_ONLY, Bot과Task는Disabled/Trigger0 유지. 네이티브 설치/권한·실제Job·PIN 전 부팅·유효GROUP/비협조launcher 통제·동일구독 충돌은 미검증/HOLD. 기존 인증 복사0, 기기코드 로그인은 별도 승인/Batch enrollment 도구 설계 필요. 실제 staging/계정/ACL/Task/설치/IPC/AI/usage/Discord/A/Push0. V1 REFERENCE_VERIFICATION_HOLD·MANUAL/A HOLD 유지. 로컬 커밋·Issue 보고 링크는 CODEX_LATEST 후속에 기록, REMOTE_NOT_PUSHED.

## 2026-10-27 13:34KST Worker 관리자 판독 — 동일성/owner11·GroupSID성공 / 역할·독점HOLD

f1f6f399 원본SHAfdcf6351873fe5eeb999381f0b5c9ccab96c5abd84fcbe8ec94ededeb74c6200. 11/11 sourceWMI정밀도 생성시각MATCH·실제ownerSID1007·Session0·Security BATCH4·기본token조회성공, LUID2개(4+7). Python6356/28788/34416 서명cache유효; 같은이름정제서명파일7432/41880 역할미확정. 나머지6개 IMAGE_HANDLE_SHA_SIGNATURE InvalidOperation/-2146233079·가드종류미기록→UNKNOWN, 완화0. Job포함3/구체Job미확인. 실제AI/CLI부재·작업내용증명0.

관련96행=GROUP92+Worker user4, resolver오류0. 실제Group Everyone S-1-1-0 / AuthenticatedUsers S-1-5-11 / Users S-1-5-32-545 활성토큰소속, Administrators S-1-5-32-544·INTERACTIVE S-1-5-4 관측토큰부재. 이전길이11오류객체와동일성은옛ID누락으로미확정. GROUP4와Security BATCH4분리. 직접Worker4개Enabled/Ready/instances0(OnBoot2포함), GROUP실행상태를WorkerAI실행으로단정금지. 알려진V2메타ABSENT=전역lease/독점증명아님.

상태 HOLD_ROLE_AND_EXCLUSIVITY_UNKNOWN, r11/helper/패키지/보호정책수정0·새진단재실행0. 상세 Discord_Worker11_AdminReadout_20261027.md, Evidence/WorkerInvestigation20261027/원본+AdminMatrix+AdminInterpretation. 다음은필요시이미지가드6개상수tag·정제식별최소진단/독립Worker설계검토, 같은전체명령재요청0·프로세스중단/운영변경0. V1/MANUAL/A HOLD·실제staging/설치/IPC/승인생성/Push0. LOCAL_COMMITTED/REMOTE_NOT_PUSHED.
## 2026-10-27 Issue66 Worker11·GroupPrincipal 전용 조사 — 56PASS / 실제 HOLD

원본76eec5af(SHA82237991…)의PID/생성시각11개고정. 새도구WorkerInvestigation20261027/만추가, 기존V2/r11코드변경0. OS계정Lookup으로PAUL-PC\LIMITLESSRelaySvc SID1007/LOCAL_USER확인. 실제비관리자최종48106246… 결과는11개모두OpenProcess QUERY_LIMITED Win325로현재동일성·소유·역할UNKNOWN, TaskCOM UnauthorizedAccess/-2147024891로GroupId/SID·멤버십UNKNOWN. 이미지·서명·부모/자식·Job/실제AI충돌미확정, 빈배열을0건판정금지. 알려진V2 lease/consume/approval메타ABSENT는전역배타증명아님.

새Native읽기(TokenQuery/그룹속성/LUID/파일handleSHA+WinVerifyTrust cache-only/인접관계)·OS GroupId LookupAccountNameW/SidW·GROUP4 vs Security BATCH4분리. 정상서명/이름도후보일뿐safe_to_exclude=false, r11 foreign_zero변경0. 신규fake56PASS/0FAIL(원시비밀비출력·PID재사용·그룹거부/불명·deny/restricted·Job/lease오판/날짜소수초포함). 실제서명/토큰그룹/GroupPrincipal성공아직미검증. 새도구SHA955a1d717ba42da7a9439946da8970d7976bc0adeb395eb91fc6b717c655fd38. 관리자PowerShell7한줄과11개역할표/공유실행기경합·별도WorkerSID/VM설계권고는 Discord_Worker11_GroupPrincipal_Investigation_20261027.md.

Evidence/WorkerInvestigation20261027/FinalProof·원본결과·ProcessMatrix·SourceSHA·OfflineTests. r11 Installer/Manifest/ZIP 실제SHA불변확인. V1/MANUAL/A HOLD·기존Worker/인증/CODEX_HOME/OnBoot2/봇아트/운영SQLite/Unity/Save보존, 종료/재시작/권한/계정/Task/staging/설치/IPC/AI/usage/Discord/Push0. 진단·오프라인검증후HOLD중지. branch codex/discord-usage-diagnostics-20261026, 기준8b137161; LOCAL_COMMITTED / REMOTE_NOT_PUSHED.
## 2026-10-27 Worker 관리자 단발 결과 — 11개 확인 / HOLD_NOT_IDLE

76eec5af 원본SHA8223799133dea9736f9dbe341d4498c0a020ec1880a2cb1797fea1cb7e86ccbe, 11:39:40KST. process_query_known=true/PID+생성시각11개→정책상유휴아님/HOLD. V2 lease ABSENT/known=true. Task조회는GroupNotFoundException(-2146233088), group String길이11/UserId null/LogonType4, task_query_known=false/다른작업수null. 실제GroupId미기록으로그룹정체추측금지, 로컬SAM조회한계·비-SAM/도메인여부미확정. 오류해결해도11개유휴차단은별개. 원본Evidence/Session0V2R11_20261027/WorkerAdminResult_76eec… 보존, 상세Discord_V2_NewPackage_Staging_20261027.md후속판독. 같은진단재실행/프로세스종료/Task·권한수정요청0. r11/SHA/78PASS불변·실측안전승격0, 실제staging/설치/IPC/승인생성0. V1/MANUAL/A HOLD·운영보존. LOCAL_COMMITTED/REMOTE_NOT_PUSHED.
## 2026-10-27 Issue66 native staging 구현·새 V2 r11 — 78PASS / 실제 Worker HOLD

원본Workerprobe SHA0fa7933f… 및 helperd6471b2b… 불변 확인. 현재admin=false·관리자단발결과미수신으로유휴HOLD, 기존PASS6/855/19전수/성공suite반복0. Worker전용관리자한줄명령은 Discord_V2_NewPackage_Staging_20261027.md. 기존Worker종료/인증/권한변경0.

SECURITY_ATTRIBUTES 생성DACL+Owner/Group/DACL·부모handle/finalpath/fileID/NTFS/reparse/hardlink/ADS/정확집합·SHA/readback·설치직전재검증 native구현. 같은Create/Verify fakeFS60PASS, Worker공통조회결합11PASS, 새실물패키지7PASS=78/0FAIL. Native보호staging생성/검증 호출0·실측미검증. 최종새후보Temp/Session0V2Candidate20261027-r11, package13f746811f8a4b7da4c1bc3402c67f41, payload769+Manifest1/ZIP770/15,270,008bytes. r9기존766유지(3수정)+3추가, 삭제0. r10중간후보superseded/보존. schema2/approved=false/auto_start=false/FAKE_ONLY. Installer eb321fc5ee0426cdd7e1c8f989f05a5e2ec55e6d1887789f0e16490598311bc2 / Manifest e8d6ed623934a577f88a5bb99ebafc78a2a469c10485a0c648c40dd457356dc0 / ZIP 0250f736b44798d3ba73b5d9674fc154fa233bc74e3b972f85931c2aa59a41d0.

실제staging경로예정 C:\Program Files\LimitlessV2Staging20261027\13f746811f8a4b7da4c1bc3402c67f41\Bundle. 실제생성·배치·계정/Task/ACL·설치/IPC/승인파일0. V1참조HOLD/MANUAL/A·Worker1007/인증/CODEX_HOME/OnBoot2/봇아트/운영SQLite/Unity/Save보존. 다음은Worker관리자JSON판독+정확r11SHA의별도staging생성승인/실측readback→다시단발설치승인→별도IPC승인. 부분실패보존/HOLD/자동재시도·삭제·권한완화0. Evidence/Session0V2R11_20261027/FinalProof·전체Manifest/파일별SHA·r9diff. branch codex/discord-usage-diagnostics-20261026 / 기준124be621, 실제마감commit Issue66후속참조. LOCAL_COMMITTED / REMOTE_NOT_PUSHED.
## 2026-10-27 Issue66 HOLD2 원인 수정·계획 완료 — 실측 HOLD 유지

기존3a5803a2 재사용·PASS6재검사0. 빈UserId NTAccount(string)에서 동일ArgumentException/-2147024809 합성재현; 당시Task/인수는원본누락으로확정불가. 원래조회함수 빈인수HOLD/함수·자료형·길이기록/counter범위수정. 별도Worker전용query가process/task/lease를분리, 실제SID1007유휴는관리자결과대기. 신규46PASS/0FAIL(fake)·native0. staging새경로 ProgramFiles/LimitlessV2Staging20261027/R9-01f8… 설계·protected생성DACL/owner/readback/SHA766+1/link/ADS거부모델, native생성기미구현·실제배치미승인. 원본r9ZIP불변·수정소스미포함, 실제설치전별도패키지/SHA결정필요. 최소관리자Worker전용한줄명령·SHA5개는 Discord_V2_Hold2_Resolution_20261027.md. V1/운영보존·MANUAL/A/HOLD유지·계정/ACL/Task/IPC/승인생성0. 원인수정/검증계획범위에서중지. LOCAL_COMMITTED/REMOTE_NOT_PUSHED.

## 2026-10-27 V2 r9 관리자 결과 수신 — HOLD / 설치0

3a5803a2 원본SHAfc7085b0… 관리자PASS6/HOLD2. Worker연결11개집계이나known=false·ArgumentException(-2147024809)로유휴미확정·타작업0확정불가. 후보ACL 조회배열 root/root/.zip 결함수정·2경로회귀PASS, 별도읽기root/ZIP owner1001·AuthenticatedUsers write로실제보호조건미충족. 도구SHA1fe25f911859c534b7e2785a763d80028871aca244719b3e0c7fc2fe4541bf42. 상세AdminPreparation문서/증거 참조. 같은전체검사재요청0, 설치명령·승인발급0. 다음은Worker조회오류분리·정상유휴확보 및 별도승인보호staging설계. V1 HOLD/운영보존, 실제계정/ACL/Task/IPC변경0, LOCAL_COMMITTED/REMOTE_NOT_PUSHED.

## V2 r9 사전검사 메모리 범위 오류 수정
## 관리자 화면 오류 수정 (후속 정본)

바울 대표 화면의 checks 속성 오류는 메모리 ScriptBlock 실행에서 script:result/script:m이 다른 범위를 가리킨 도구 결함입니다. 결과 객체를 상속 범위에서 수정하고 Manifest는 공유 객체에 담아 두 참조를 제거했습니다. r9 패키지·SHA·설치기·보안조건 변경0. 위 명령의 도구 SHA를 새 값으로 교체했습니다. 이전 SHA 명령은 폐기합니다.

동일 메모리 호출로 실제 비관리자 package PASS/admin HOLD·JSON9fc1203a387a40d09dd49b416c7d0b4b 저장 확인. 메모리 범위 회귀 PASS/false HOLD/Win325 오류 보존 3 PASS, AST parse0/script범위참조0. 관리자 분기는 여전히 미검증, 설치0. 대표가 새 명령을 관리자에서 실행한 결과만 판독하며 자동 재실행하지 않습니다.

## 2026-10-27 Issue66 V2 r9 설치 준비 — WAIT_ADMIN_READONLY / HOLD

r9 SHA3·payload766+Manifest1·ZIP767 bytes 대조 PASS. 현재 토큰 admin=false라 Windows 경계 NOT_VERIFIED. 읽기 전용 prepare-r9-readonly.ps1 SHA8193c5a121082866ec5473c6d99d4ca025bfbc8b84737a13af591a845a2d51a8, parse/변경호출allowlist/비관리자HOLD 확인. 관리자 직접 SHA검증 메모리 실행 명령은 Discord_Session0_V2_R9_AdminPreparation_20261027.md. 결과 Temp/V2R9Preflight20261027/<GUID>.json. 모든 관리자 조건 PASS 전 설치 명령·승인 파일 발급0, 설치/계정/ACL/Task/IPC0. V1 REFERENCE_VERIFICATION_HOLD·기존운영 보존, 권한거부/불명/Worker busy/후보owner·writer불일치 HOLD, 기준완화/자동재시도/삭제0. 기준구현62578e5, 직전53fb709; 실제마감commit/Issue댓글 후속기록. LOCAL_COMMITTED / REMOTE_NOT_PUSHED.

## 2026-10-27 Issue66 V2 독립 후보 구현 완료 — NOT_APPROVED / NOT_INSTALLED

V1 코드/guard·1008Disabled/기존소비·Worker1007·인증/OnBoot/게임 불변, REFERENCE_VERIFICATION_HOLD 유지. 필수8개구성요소+shared계약/원자publisher/ledger 구현. 최종 로컬 Temp/Session0V2Candidate20261027-r9, payload766/ZIP767·약15.25MB, schema2/approved=false/auto_start=false/FAKE_ONLY, 임의newSID없음. Installer1a1d671d1cf86aab56c9c2392ed4ba35a8240e348134e6a467f8eb8ec81f17a0 / Manifest250bab2d46010841353a319fbcf34581784366ac3042e6bc0fdc97aadd17c09e / ZIPa4a652bdf73c9a1007bd1e761ae539d3459896b46d8a021fa1459012a1d15c41.

신규 코어84/Python18/교차9/Builder2 PASS·0FAIL, 실제계정/ACL/Task/LSA/IPC/Worker/AI/usage/Discord0. Python18은최종5모듈SHA동일증명으로재사용. 원본소스13/JSON3 SHA불일치0, 기존전수QA·19프로세스재검사0. Worker직접Batch·Users확인(없으면기존권한수정없이HOLD), SID세션관계·다른launchable task·lease·PID생성시각·250ms감시; VM격리/실전충돌없음증명은아님. 검증된메모리bytes만설치/compile·consume분리·TaskDisabled/trigger0/flags50/90초readback·UTF16계약·정제stage/raw코드보존. 실제WindowsNative/Session0/logon4미검증.

상세 Discord_Session0_V2_Candidate_20261027.md, Evidence/Session0V2Candidate20261027/ 전체Manifest·파일별SHA·비교·시험·FinalProof. 다음은대표의정확r9 SHA별도설치승인후관리자사전검사/Disabled설치만, 설치판독후다시별도IPC1회승인. 기존Cleanup/Install/신규실행자동재개금지, 부분실패는consume·제한된신규자산보존/HOLD/자동삭제·복구0. branch codex/discord-usage-diagnostics-20261026, 코드기준63b97539… / 실제마감commit Issue66참조. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 후보검증후중지.

## 2026-10-27 Issue66 HOLD 대안 — 신규 합성72PASS / V2 설계 권고

대안모델·검증 commit `63b97539ae4e1655d9f36e905b1cb2e3601b22d7`, 실제 Issue66 보고 https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6429561131 . LOCAL_COMMITTED / REMOTE_NOT_PUSHED.

기존15개메타데이터/종료객체1/누락3·참조0/비참조확정0 재사용, REFERENCE_VERIFICATION_HOLD 유지. RM4함수는 등록한855파일의 현재사용 보조근거일 뿐 빈목록도HOLD·내부등록쓰기동반. 실제RM호출0. V2는 기존1008Disabled/Code/소비/승인 보존·별도SID/root/State/Task/pipe/epoch·FAKE_ONLY/Disabled/trigger0·생성시점ACL의 별도후보를 구현하는 안을 권고. 현패키지V2설치불가·기존fresh guard완전보존·실제설치기/Manifest/ZIP미생성. 최소8구성요소 파생+결합시험 필요, 미래SHA없음(null).

신규 C# 상태모델/가짜파일·Scheduler·Worker호출차단72PASS/0FAIL, 실제Windows/RM/계정/Task/권한/IPC/FakeWorker/AI/usage/Discord/기존운영/Unity/Save변경0. 855전수/기존성공suite반복0. sourceSHA4개·보존source13/JSON3 SHA와 정확한 제약은 Discord_ReferenceHold_Alternatives_20261027.md 및 Evidence/TrialAlternatives20261027/. 실제커널/원자ACL/Session0미검증. 실제V2구현→새후보SHA/오프라인결합→별도관리자설치승인→다시별도IPC1회승인 순서. 기존Cleanup/Install자동재실행금지. 로컬브랜치 codex/discord-usage-diagnostics-20261026, 마지막기준d6de3befd4449cb733ce23c9ed64f17068fb7e8a. 실제신규commit Issue66마감참조, LOCAL_COMMITTED/REMOTE_NOT_PUSHED. 설계검증후HOLD중지.

## 2026-10-27 Issue66 관리자 프로세스 결과 수신 — HOLD 유지

판독 commit `a6ad6760040684402c7f692b54a6095f5cabff30`, 실제 Issue66 보고: https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6429320304 . LOCAL_COMMITTED / REMOTE_NOT_PUSHED.

f5c37d29… SHA1b981f432d715025724d94381e97c993532bacd773ad2a6de9b16389b2a65a18 / 관리자 / 07:55KST. 현재15개 이름·생성시각·소유SID·Session0 확인, 이미지12·명령줄0. PID32660 조회객체 생성/종료시각+CIM부재로 PROCESS_EXITED 근거이나 과거동일성UNKNOWN. 원본 current_status UNKNOWN은 문서 해석으로 보완, JSON수정0. 원본PID누락3·과거생성시각부재로 과거19개 참조판정UNKNOWN, 무참조확정0/참조발견0. 이미지오류 PID288 code0은 진단한계, 328/3360/32660 code31. 보호수준만으로 무참조/Windows파일인증 금지.

추가 관리자 반복실행 요청0·진단기변경0·전체검사/cleanup/설치/시스템변경0. 이후는 기존 당시증거 또는 별도 현재진단범위 검토, 관련참조 해소 및 바울 대표 후속승인 전 정리불가. 상세 Discord_UnknownProcesses_AdminResult_20261027.md. LOCAL_COMMITTED / REMOTE_NOT_PUSHED, 실제이번commit Issue66 후속참조.

## 2026-10-27 Issue66 미확인19개 읽기 전용 조사 — WAIT_ADMIN_READONLY / HOLD

진단·결과 commit `80776f95df9b77b2f8efab1c8b29b48630c0b99c`. 실제 GitHub 보고 댓글: https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6429198580 . 보고서·인계는 로컬 커밋이며 원격 미Push, GITHUB_REPORT_COMPLETE 아님.

정체확인0/19. 보존PID16개는 현재 OpenProcess Win325 ACCESS_DENIED 및 CIM NativeErrorCode2/HRESULT-2146233088/StatusCode0. 과거19개는 생성시각·개별실패정보 부재로 동일성UNKNOWN, 누락3개는 기존 First16 잘림으로 PID복구불가. 참조발견0이나 무참조확정0, Windows보호시스템판정0. 기존 Task282/service-driver792/process334 성공검사반복0. Cleanup/설치/계정/권한/프로세스/Unity변경0.

전용 probe-unknown-processes-readonly.ps1 SHA33bc59120213a49f1e27a9e96f25cb872db566783d8eed46c3595d168d28aaf1, 신규합성9PASS/0FAIL·parsePASS·exit0. 관리자용 SHA검증 bytes/NoProfile 자식명령과 결과경로는 Discord_UnknownProcesses_ReadOnly_20261027.md. 관리자결과도 과거생성시각/누락3개를 자동복원하지 않음. 경로문자열 불일치를 NO_REFERENCE_VERIFIED로 승격하지 않으며 간접참조/별칭 한계 유지. 안전하게 해소불가면 HOLD, 정리재개는 바울 대표 후속승인 필수. 같은 Cleanup 자동재실행/새LSA설치 금지. branch codex/discord-usage-diagnostics-20261026, 기준 a8ec860e922d088a5f4699157ac957aa441204e0, 실제후속commit은 Issue66 마감 참조. LOCAL_COMMITTED / REMOTE_NOT_PUSHED.

## 2026-10-27 Issue #66 실제 관리자 정리 사전검사 — 참조 확인 HOLD / 삭제0

관리자 요청39d34e64… 판독: 계정1008 Disabled/Users/직접권한[]·profile/logon0 및855개 검증 통과. Task282/서비스·드라이버792/프로세스334의 확인참조0이나 미확인프로세스19로 CHECK_REFERENCES HOLD. 필요한경로/명령행 부재 원인은 아직 미확정, 정상OS라고추정하지 않음. cleanup소비false·백업단계미도달·파일/폴더/계정/Task삭제0·설치0.

기존Install1/실패·관리자증거/부분자산/승인후보 보존, MANUAL/A HOLD. 이번에코드·권한기준변경/재실행/프로세스중단0. 동일명령반복금지, 새LSA설치불가·미승인 유지. 다음필요조사는미확인process의 생성시각/SID/가용경로/조회불가원인 읽기전용 분류이며 PID재사용주의. 상세 cleanup문서후속판독 및 Evidence/Session0TrialCleanup20261027/39d34e6461e94a97b0ccac01b611ef8f.json(SHA23798422…) 참조. 실제보고commit Issue66 마감 참조.

## 2026-10-27 Issue #66 부분 자산 정리 승인 — WAIT_ADMIN_CLEANUP

구현 commit `82e41f64e85e57bc3675c7e4413c6f4857023caa` (LOCAL_ONLY), 기준ae163034…/격리branch. 대표가 옛 Code root C:\Program Files\LimitlessRelayTrial 및 Disabled Bot SID1008 정리만 승인. 현재 Codex Session9 비관리자라 실제 정리/백업0, FIXED_ADMIN_REQUIRED. 신규 정리 도구/Native helper 준비·합성 **43 PASS/0 FAIL**(실제삭제/LSA/Scheduler0), 기존 성공 suite 반복0.

Admin/SID/owner·DACL·855 SHA·예상밖파일/ADS/reparse·전체Task/서비스·드라이버/프로세스 참조/보존검사→관리자보호855백업 및 evidence→재검사→handle파일별 삭제/빈디렉터리 비재귀→정확 SID계정삭제→최종검사. 참조/조회불가/불일치면HOLD, task등록삭제/권한완화/exit/설치경로 없음. 실제정리 대상·단일 관리자 명령·실패대응은 DiscordSession0_PartialCleanup_AdminPending_20261027.md. 현재 신규설치가능성 미확정, Install1/실패증거/LSA후보 그대로 보존. 정리PASS 후에도 새패키지 별도승인 전 미설치 유지. 관련commit Issue66 마감 참조.

## 2026-10-26 Issue #66 LSA 최소 수정 후보 — 신규36 PASS / 미설치

구현 commit `b2ae449450c7042bda413fe07427e402babb0480` (LOCAL_ONLY), 기준d41ceaf1…/격리branch. UTF-16 길이동적계산·34/36·종료NULL/범위/메모리 검증, 실제 LSA 오류NtStatus/Win32·정리오류 보존, installer 단계/InnerException 정제·원자 protected private log, runner exit제거/JSON logical_exit_code·LASTEXITCODE 분리 구현. 관리자/SID/권한정책/원자SDDL4함수 보존. **신규36 PASS/0 FAIL**, 기존29·35·42 전체재시험0, 실제LSA/Scheduler/설치본문0.

새 Temp/Session0LsaRepairCandidate20261026: 855중 Policy.cs+Installer2 변경/853동일/추가삭제0. Installer79194ad6…/Manifest33ed9a83…/ZIPd678e760… 전체값은 DiscordSession0_LsaRepair_Candidate_20261026.md 및 Evidence/Session0LsaRepair20261026/Candidate.json. 이전Atomic/Ready/Install1 증거 보존. 부분Code/Bot1008 Disabled·task부재 자산 수정0·자동재사용없음/부분환경설치거부. 실제 복구·재설치는 새SHA와부분자산정리 범위 별도승인 필요. 운영/인증/부팅/Unity/Save/Push/main 변경0, MANUAL/A HOLD. 다음 승인 전 중지. 관련commit Issue66 마감 참조.

## 2026-10-26 Issue #66 관리자 진단 판독 — LSA 길이 결함 확정 / 복구 전 HOLD

관리자 a11561f5…: SID ADMIN1001/Worker1007/Bot1008 분리, Bot Disabled/Users만/만료10-27 22:41, 직접권한[]; task 현재부재·Manifest/record/State부재 확인. Code/root BA 소유·SY/BA만 Full, 855 SHA 불일치0. OnBoot2개/boot approval/승인3SHA 및Install1 원본증거 보존.

설치 사본과SHA동일한 TrialAccountPolicy.cs21행 Length32/Max34 결함 발견: SeBatchLogonRight는17글자/UTF-16 34byte+null2=36인데 마지막t가 잘림. 순수byte연산 증거 LsaUnicodeLengthProof.json, native호출0. 코드결함 확정이나 당시 원시예외는 catch로 소실되어 실제실패 nativecode/해당호출 도달은 NOT_VERIFIED. 4104 ACCESS_DENIED_TEXT는 소스본문 단서이며 실행오류 증거 아님.

기존코드수정/권한변경/재설치/삭제0. 다음 후보는 길이 계산·단계/InnerException 보존·대화형exit제거 최소안, 새SHA/오프라인검증 뒤 별도 복구·재설치 승인 필요. 현재 HOLD로 중지·MANUAL/A HOLD. 상세 DiscordSession0_PartialInstall_ReadOnly_20261026.md 관리자판독부, Evidence/a115…json 및 proof 참조. 관련 보고commit Issue66 참조.

## 2026-10-26 Issue #66 부분 설치 읽기 전용 조사 — NOT_VERIFIED / 관리자 진단 대기

진단 구현 commit `6d037658420e4f36121a1e3555d6b72a9efba0ea` (LOCAL_ONLY). 이전 Install1 소비 유지·추가설치0. 현재 root존재/State부모목록상부재/승인3SHA 및 원래 receipt SHA보존 확인. 계정·그룹·배치권한·task SD·Code/Manifest·로그는 비관리자 토큰 조회 실패(부재 아님). 설치 직접원인은 고정throw로 원문 소실되어 NOT_VERIFIED. 현재 진단 접근거부를 과거 설치 원인으로 해석하지 않음. 창 종료는 기존wrapper 대화형 exit2로 별도 확정.

신규 readonly 진단 SHA2e53e489…/합성16 PASS·기존29/35 반복0. 설치기 호출/exit/권한변경/암호입력 없음. 관리자PowerShell7 한줄 진단 명령은 DiscordSession0_PartialInstall_ReadOnly_20261026.md. 최종 일반토큰 증거328ab780…; 기존 승인코드/소비/부분자산 보존, MANUAL/A HOLD·운영/Unity/Save 변경0. 다음: 관리자 정제 JSON 판독 후 중지; 복구·재설치는 별도 승인 필요. 관련 commit은 Issue66 마감 기록 참조.

## 2026-10-26 Issue #66 관리자 설치 실제1회 — 부분 설치 HOLD

대표 로컬 실행 요청0f8e15c9… 결과 확인: 승인SHA/855/854 PASS, 실제 관리자 Install1회 후 고정 INSTALL_HOLD 오류. 새 Bot SID 끝1008 생성 후 Disabled, 코드폴더 존재·상태폴더 미생성, task UNKNOWN_OR_ABSENT. 실제 SD/완전 설치 검증 미완료. OnBoot2개/boot approval 전후SHA 동일, Host/IPC/Fake/AI/usage/Discord0. 자동재설치·삭제·권한완화0, MANUAL/A HOLD.

HOLD시 wrapper exit2를 현재 메모리ScriptBlock에서 실행하도록 안내해 PowerShell 창까지 종료됐습니다. 설치 실패의 직접원인은 기존 installer catch의 원문 소실로 미확정. 현재 승인파일 수정/재실행0. 다음은 관리자 읽기전용 부분상태 진단, 기존 Install/Remove 재실행 금지. 상세 DiscordSession0_AtomicInstall_PartialHOLD_20261026.md 및 Evidence/Session0AtomicInstall20261026/0f8e15c951a144efa79f5ccdbeed9c55.json. 관련 설치 준비 commit30cc720d…/이번 실제 보고 commit은 Issue66 참조.

## 2026-10-26 Issue #66 새 원자 SDDL 후보 설치 승인 — 관리자 로컬 입력 대기

실행 준비 commit `30cc720d59396409b76ee4642ec56d16fdb79816` (LOCAL_ONLY). 대표가 Installer117065bb…/Manifest6efbd783…/zip4b2c56ed… 새 후보의 최소권한 계정·Disabled/trigger0/FAKE_ONLY 작업 설치를 승인. 세 SHA·855개 hash·854개 보존 사전검사 PASS. 현재 Codex Session9/비관리자이므로 **WAIT_ADMIN_INSTALL / 설치 시도0**. 신규 SID/실제 task SD/운영 OnBoot 전후 PASS는 아직 미확정. 기존29/35 재시험0.

신규 run-atomic-trial-install.ps1은 승인 설치기 bytes만 한 번 호출하고 SID3개/Users/배치 직접권한/24시간 만료/설치855 SHA/실제 SDDL/고정 action/Disabled/trigger0/기존 OnBoot SHA 전후를 검사하는 보조 명령. 승인 payload 수정0. 관리자 PowerShell7의 로컬 보안 입력이 필요하며 정확 명령은 DiscordSession0_AtomicInstall_AdminPending_20261026.md. HOLD면 재등록/자동삭제 금지. Host/IPC/FakeWorker/AI/usage/Discord0, MANUAL/A HOLD 및 기존 운영·Unity/Save 보호. 설치 PASS 후 다음 IPC 승인 전 중지. 실제 관련 commit은 Issue66 마감 기록 참조.

## 2026-10-26 Issue #66 예약 작업 원자 보안 등록 후보 — 신규35 PASS / 설치 대기

구현 commit `2ef4012fb9a963a80de5a9910b8853917fd870fd`, 기준 `5d259e2738768dd0816540b9a31228049db70a56`, LOCAL_ONLY. COM RegisterTaskDefinition CREATE-only(flags50)에 SDDL 동시 전달, 실제 GetSecurityDescriptor(7) owner/group/보호DACL/정확ACE3 검사·실패HOLD/재등록0 구현. 기존29 재시험0, 신규 가짜Scheduler **35 PASS/0 FAIL**. 실제 Windows 등록·IPC 검증 아님. 알 수 없는 실제 mask 정규화도 HOLD.

새 Temp/Session0TaskSecurityAtomicCandidate20261026: 855 payload 중 설치기1 변경/동일854/추가삭제0, metadata 의미차이 hash1개. Manifest `6efbd7839bd4e452d9fbca8a465290533472e87cde71845a73bd37bdb21493ab`, Installer `117065bbfca686474f9104bc0dff20fa6528dfb9c204e5a2f2720d91994958e9`. 원본 Ready Manifest9aa4…/Installer337a…/855개 전부 보존. 상세 DiscordSession0_TaskSecurity_AtomicCandidate_20261026.md 및 Evidence/Session0TaskSecurityAtomic20261026/.

계정/task/service/시스템ACL/부팅/Host/Fake/AI/usage/Discord0. MANUAL/A HOLD, 기존 봇·인증·SQLite·Unity/게임/Save/Push/main/PC 보존. **실제 설치 명령 실행 금지**, 새 SHA 별도 승인 후 관리자 로컬 보안 입력→실제 SD/ACL→FAKE_ONLY IPC 순서. 현재 Unity 사용자 확인사항 없음. 원본 main bfd28154…/tracked dirty308/untracked36 동일. 상세 Result.json 및 Issue66 완료 보고 참조.

## 2026-10-26 Issue #66 실제 설치 승인 후 사전검사 — 설치 전 SECURITY HOLD

HOLD 기록 commit `f230d0dbf9920dac2c619f318841a94f665921a0` (LOCAL_ONLY). 기준 `a8b0be3971aa461d09432348b309179cade27981`. 대표의 새 계정/FAKE_ONLY IPC 승인은 확인했습니다. Manifest9aa4a60d…/Installer337ad613…/정확855개(Code66 Runtime741 Legacy47 Deployment1) SHA PASS·불일치0. 그러나 승인 Installer112행 작업 등록→114행 DACL 적용 구조이며 실제 SD readback이 없습니다. 생성 시점 권한 검증 미충족으로 **HOLD_BEFORE_INSTALL**. 실제 권한 유출을 재현/확정한 것은 아님.

이전29 PASS가 이 설치 권한 경계까지 검증하지 못한 점을 정정합니다. 이전 설치 문서 상단에 실행 금지를 표시했습니다. 승인 후보/855파일은 수정하지 않았고, 읽기 전용 Preflight만 추가했습니다. 등록 시 SDDL 동시 전달/0x10 flag/owner·DACL readback의 별도 수정 후보·새 SHA 검증이 먼저 필요합니다. 이번 지시의 검증 실패 시 중단을 따르며 현재 관리자 설치 명령이나 암호 입력을 요청하지 않습니다.

계정/task/서비스/ACL/Host/Fake/모델/usage/Discord/암호입력/인증복사0. 신규 BOT SID 없음·서비스 Session0 실행 없음·실제 IPC 미검증. 현재 조사자는 Session9 비관리자이며 OnBoot2개 export 조회는 실패해 실제 정의 SHA는 확인보류(수정/실행0). 기존MANUAL/A 미활성·운영/Unity/게임/Save/Push/main/PC 보호, 운영 rollback0. 기존 성공시험 재실행0.

상세 DiscordSession0_Install_Preflight_HOLD_20261026.md, Evidence/Session0InstallPreflightHOLD20261026/. 다음: 최소 작업 원자 등록 수정안 별도 후보→가짜Scheduler 경계 검증→새 Installer/Manifest SHA 승인→그 후 실제 새 계정/fake IPC. 기존 운영 환경 재설치·기존 소비 초기화 금지. 실제 commit은 마감 기록 참조.

## 2026-10-26 Issue #66 Host↔Worker 접합 완료 — 오프라인29 PASS / 설치 전 중지

구현 commit `5f1dad2284d7c2cf78c4d80d451555c3619eb60a` (LOCAL_ONLY). 기준 `ef1dec16bf285024c9caa733845ff93985e07d7f`. 인증된 pipe→고정 C# Host→익명 stdin Job→서비스 broker→고정 Worker wrapper/sidecar→기존 frozen 추론/JSONL→정제 결과→단발 Discord callback까지 코드 접합을 완료했습니다. CODE_JOIN_COMPLETE / OFFLINE_VERIFIED / NOT_INSTALLED. 실제 Windows 계정 IPC·실제 Discord→AI 성공 아님.

신규29 PASS/0 FAIL: 중단 전 통합17/native5 보존·재개 후 callback4/사용자 소유 코드 거부1/원자 게시기2 추가. 완료 성공 시험 재실행0, 이전42 전체 반복0. 패키지 기초4 PASS 재사용, 최종855파일 SHA/변경분/동일 Runtime·Legacy 무결성3 확인. 실제 모델/usage/Discord/계정·task·서비스/시스템 ACL/운영 DB/토큰 복사0, MANUAL/A 미활성.

최종 Ready bundle은 Temp/Session0HostBundle20261026Ready, Manifest SHA `9aa4a60d6dd57c320f2d268ef96dcf86694e53ca9dfdcbb6a3e97d1f33dfe7ec`. 기본설치 FAKE_ONLY/비활성/trigger0·새 LIMITLESSBotTrial(Users/Batch/24시간)만 준비. 계정/설치 명령은 문서에만 제공, 실행하지 않았습니다. BOT/ADMIN/WORKER SID 분리·새 Host identity-query 최소권한·원자 BA 소유 게시·실제모드 사용자-writable CLI 거부. 기존 CLI 경로/ACL/인증은 변경하지 않습니다.

다음: 별도 설치 승인 후 문서의 SHA 검증 메모리 설치 명령→Verify→별도 승인된 실제 fake-only SID/IPC/종료 검사. 그 다음 새 state epoch/요청/manifest·CLI 신뢰·시험앱/token/권한과 실제 모델1건 승인을 별도로 확인. 소비 초기화/자동 모델/A 활성화 금지. 현재 제한 토큰 LSA C0000022는 실제 서비스 계정 검사 아님. 상세 DiscordSession0_HostWorker_Joined_InstallReady_20261026.md, Evidence/Session0HostWorkerJoined20261026/. 실제 구현 commit은 마감 기록 참조.

## 2026-10-26 Issue #66 Session0 보안 중개기 후보 — 신규42 PASS / SECURITY HOLD

구현 commit `82af13e15469ae23d8ca13e96ac6b651f3160a9f` (LOCAL_ONLY). 기준 `f93d13e79b336453711a69ea5de34be2035b1d99`. 봇/관리자/Worker SID를 모두 분리해야 하는 fail-closed 후보를 구현했습니다. UAC 일반/관리자 토큰만으로 동일 SID 문제를 해결했다고 주장하지 않으며 같은 SID는 상승 토큰도 거부합니다. 로컬 인증 pipe(arm/work/control)·고정 JSON schema·서비스 별도 소비장부·정제 reader·LiveSession0Bridge IPC 경로·취소 Job 파생본을 추가했습니다.

**PARTIAL / SECURITY_DEPLOYMENT_HOLD**. 실제 Host와 고정 Worker backend 접합 및 서비스 계정 간 IPC는 아직 미완/미검증입니다. 현재 별도 최소권한 시험 봇 계정과 보호된 설치가 없으므로 활성화하지 않습니다. 새 계정/서비스/task/ACL/토큰 복사0. 이 조건은 후보가 보안 요구를 만족하지 못하면 HOLD하라는 대표 지시에 따릅니다.

새 정책/전달34 + Windows native8 = **42 PASS / 0 FAIL**. 네이티브 fake 정상/timeout/cancel 모두 active0, cancel tree total4. 기존 성공35/전체 검증 재실행0. 실제 모델/usage/Discord/운영 DB0, MANUAL/A 미활성. 기존 runner4 SHA·boot approval SHA 보존. 기존 봇/부팅/인증/Unity/Save/Push/main/PC 변경0, 현재 운영 건강/동적 DB 재조회 검증은 하지 않았습니다.

다음: 별도 봇 보안 주체·보호 설치/고정 task 범위를 승인·확정한 뒤 최종 Host/고정 Worker backend 조립 및 실제 계정 fake-only 권한 검증. 그 뒤 대표의 별도 실전 승인으로만 Planning 고정 응답1건. 지금 모바일/관리자 실행 명령을 요청하지 않습니다. 상세 DiscordSession0_SecureBroker_HOLD_20261026.md, Evidence/SecureSession0Broker20261026/{Tests.txt,Native.json,SourceSHA256.json,Result.json}. 원본/이전 소비/이전 PreparedPlan 보존. 실제 구현 commit은 Issue66 완료 기록 및 아래 갱신값 참조.

## 2026-10-26 Issue #66 모바일 버튼·Worker 오프라인 후보 — 35 PASS / 실전 배포 HOLD

구현 commit `a61d8c17a8414335830939d7c1ec159ab1d8a023` (LOCAL_ONLY). 기준 commit `243bf0bab0841b545f5ff9cc4f187f2880826b4c`. 새 버튼5개와 전체 SHA/대표/역할/상위범위/절대만료/승인소비/단발 인계·보고를 격리 구현했습니다. 새 합성 Session0→기존 strict JSONL→격리 SQLite→Discord 보고 callback 연결 등 **35 PASS / 0 FAIL**. 기존 성공 전체 재시험0. 실제 모델/usage/Discord/운영 DB/서비스 task/active permit0, MANUAL/A 미활성.

**실제 Worker 연결은 미완료이며 배포 HOLD**입니다. live adapter는 무조건 거부, fixture 결과는 live 주입 금지. 다음에는 관리자 전용 arm/결과 ACL·보호 reader·고정 서비스 task 인계·전달/준비/정리 포함 전체90초 Supervisor·실행 중 취소를 구현/오프라인 검증한 뒤 별도 승인받아야 합니다. 현재 일반 봇과 관리자 동일 SID의 FullControl/사용자 소유 파일은 보호 경계로 인정하지 않습니다. 기존 계정/ACL/auth/boot 변경0. 기존 서비스 실행기4 SHA와 boot approval SHA 일치.

준비 ID `1a2ffc6823db49fd8fe25a86ad72f51b`는 approved=false인 계획만, permit/소비 아님. 새 실전 ID/SHA는 이후 실행 전 다시 바인딩. 지금 대표에게 실제 모바일 명령/관리자 명령을 실행하도록 안내하지 않습니다. 준비 후 `/limitless-trial task`→내용→실행 승인→실행→진행 확인, Planning 고정 문장1건만. 후속 역할 자동 승인/미술 승인 변경 없음. 운영/Unity/Save/Push/main/PC 변경0. 현재 live 봇 건강/운영 DB 내용 재검증을 한 것은 아님.

상세 DiscordMobile_Buttons_Worker_Offline_20261026.md, Evidence/DiscordMobileButtonsWorker20261026/{PreparedPlan.json,Tests.txt,Result.json}. SHA는 PreparedPlan 전체64자리 참조. 기존 후보/마커 보존·운영 배포0이므로 현재 원상복구 작업 없음. 구현 commit은 아래 또는 Issue66 완료 댓글에 기재합니다.

## 2026-10-26 Issue #66 미진행 웹 시험 완료 — Fake4역할 PASS / 정상 정리 PASS

마지막 관련 구현 commit: `341f6a5168a4e14b89f417de4089b5a59167d88e` (LOCAL_ONLY).

대표 새 지시로 이전 NEW/grant0 기획 업무를 원본 불변 격리 snapshot으로 재개했습니다. 휴대전화 status/task 재시험0, 웹에서 승인→기획→두 아트→Codex→기획 회신 실제 확인. Fake정확4·승인소비4·TECH_PASS4·신규 SENT11(과거2 별도), 실제 모델/usage/운영 SQLite0. USER_REVIEW_PENDING 유지·미술 승인/게임 적용0.

16:53:56 KST 완료 후 Host exit0/active0·부모/worker 모두 종료, 반환 command1564182523351662602만 DELETE1·guild/global inventory 복원 CLEANUP_PASS. 이전 snapshot/마커 보존, 완료된 재개 Queue에는 소비4가 있으므로 옛 NEW copy를 다시 자동 실행하지 않습니다. MANUAL/A 미활성·운영/게임·Save/Push/main/PC 보호. 모바일 전체 승인 UX/비대표/실제 취소·만료 live검증은 남음. 상세 DiscordMobile_RemainingWeb_FourRole_PASS_20261026.md 및 CompletionSanitized.json. 실제 AI/A 별도 승인 필수.

## 2026-10-26 Issue #66 실제 시험 종료 — 모바일 상태/접수 PASS · 승인/실행 미시험 · 정리 PASS

대표 휴대전화 status/task 응답 확인·실제 SENT receipt2·기획 task NEW 확인. 64자리 SHA 수동 입력 UX가 길어 컴퓨터 웹 사용 가능 안내. 승인 grant0/Fake0, 승인/실행/인계는 미시험으로 PARTIAL. 새 Host·만료 연장·소비 초기화0.

16:34:38 KST EXPIRED, Host exit0·active0·cleanup_verified, 반환 command1564175559733551186 단일 DELETE1·guild/global 전후 inventory 일치 CLEANUP_PASS. 현재 봇/명령은 종료·삭제됐고 아래 READY는 역사입니다. 모델/usage/운영 SQLite0, MANUAL·A 미활성/운영·게임·Save/Push/main/PC 보호. 상세 DiscordMobile_LiveTrial_PARTIAL_Cleanup_20261026.md 및 CompletionSanitized.json. 다음은 웹 시험 또는 버튼/짧은 코드 UX 개선 검토, 자동 재시험/추가 AI 금지. 구현95a4c60c, 완료 문서 commit은 Issue66 참조.

## 2026-10-26 Issue #66 실제 시험 Host READY — 모바일 확인 대기 / 16:34:38 KST 자동 종료

시험 앱/대표 소유 token 저장·현재 사용자 읽기 PASS. 별도 앱1564156403571105906에 단일 guild 명령 CREATE1, 반환 ID1564175559733551186, Gateway/대표·채널 권한 PASS. 운영 app1563102982386749440과 분리, 기존 명령0/global0 백업. `/limitless-trial status` 모바일 첫 응답 확인 대기. root1/Fake최대4/동시1, 실제 AI/usage/운영 DB0, MANUAL/A 미활성.

한시 Host Session9(PAUL-PC\windows), PID26424/부모34112, 자동 만료2026-10-26 16:34:38 KST. 새 Native Job active1/강제정리+원격 반환 ID만 DELETE/전후 목록 대조. 새 경계8 PASS+Native/구문 PASS, 기존 전체 테스트 반복0. 아직 모바일 결과/최종 정리 완료 아님. 상세 `DiscordMobile_LiveHost_READY_20261026.md`, Evidence/DiscordMobileLiveHost20261026/. 기존 운영/부팅/인증/게임·Save/Push/main/PC 보호, 추가 AI 금지. 기존 ID/token HOLD는 해소됐습니다.

## 2026-10-26 Issue #66 — 두 앱 분리 정정 / 시험 token 로컬 입력 대기

최신 대표 확인: 시험1564156403571105906 / 운영1563102982386749440으로 서로 다름. 이전 동일 앱 HOLD 정정·새 앱 생성 불필요. 기존 대표/guild 승인 대조 및 로컬 공개 사전 점검 PASS. 별도 setup-mobile-trial-token.ps1 준비: 숨김 token 입력→고정 시험 앱/대표 소유 REST 검증→새 현재 사용자 DPAPI 저장/읽기. 기존 저장소/ACL/운영 token 변경·복사0.

구문/새 fixture8 PASS, Inspect PASS(token/네트워크0). 대표가 `DiscordMobile_TrialToken_Input_20261026.md`의 일반 PowerShell 명령으로 직접 입력 필요. 현재 token 저장/실API 검증/Host/등록/모바일/모델/usage/운영 SQLite0. Host 진입점은 token 결과 확인 뒤 준비할 단계이며 READY가 아닙니다. MANUAL/A 미활성·기존 패키지/source·운영/게임/Save/Push/main/PC 보호. 아래 동일 앱 판정은 역사이며 현재 적용하지 않습니다.

## 2026-10-26 Issue #66 Phase3 — 운영/시험 앱 동일 확인으로 실행 전 HOLD

대표가 시험/운영 Application ID `1564156403571105906`을 동일하게 제공한 뒤 “같은 앱”이라고 직접 확인했습니다. 별도 시험 앱 조건 FAIL이므로 token 입력·저장/Host/명령 등록/Discord 요청0으로 HOLD했습니다. 기존 공개 운영 설정에서 대표 승인 ID와 guild 일치는 확인. 새 application을 만들고 다른 새 ID/소유 확인만 제공해야 재개할 수 있습니다. 서버/ai-시험실 채널은 유지 가능합니다.

코드/패키지 변경0·기존 ZIP/Manifest/adapter/Phase2/boot approval SHA 보존. Fake/모델/usage/운영 SQLite0, 기존 성공 전체 반복0. MANUAL·A 미활성·운영/인증/게임·Save/Push/main/PC 변경0. 상세 `DiscordMobile_Phase3_AppIdentity_COLLISION_20261026.md`, Evidence/DiscordMobileAppCollision20261026/. 시작 commit4f944707, 실제 문서 commit은 Issue66 참조. 새 실행 승인이 부족한 것이 아니라 대상 앱 충돌로 중지합니다.

## 2026-10-26 Issue #66 Phase3 실제 시험 승인 — 앱 설정 미확인 HOLD

대표의 별도 시험 앱 등록/Fake 왕복/정리 승인은 확인했지만 전용 application·대표 소유권·guild/channel·로컬 시험 token 저장소를 확인하지 못해 실행 전 HOLD했습니다. 외부에 앱이 없다고 단정하지 않으며 운영 인증을 읽어 대체하지 않았습니다. Portal 최소 절차와 모바일 순서는 `DiscordMobile_Phase3_LiveTrial_HOLD_20261026.md`에 기록했습니다.

Host/등록/삭제/모바일 실행/Fake/실제 모델/공식 usage/운영 SQLite0, 새 permit·소비 없음. 기존 ZIP/Manifest/source/boot approval SHA 및 Phase1/2/3 증거 보존, 기존 성공 테스트 반복0. MANUAL/실업무 HOLD·A 미활성, 운영 bot·아트·부팅·인증/게임·Save·Push/main·PC 변경0. 현재 모바일 시험 승인은 미실행이며 앱/ID/권한/보안 설정 준비 후 같은 승인 범위에서 재개할 수 있습니다. 시작/source commit6fcaafa0, 문서 commit은 Issue66 완료 보고 참조.

## 2026-10-26 Issue #66 Phase3 — Discord SDK 어댑터 준비 / 실연결 미실행

Phase3 implementation commit: `8cbf90ba84c93f9c45f4979a65a605fda727d464` (LOCAL_ONLY).

Phase1 VERIFIED·Phase2 24 PASS 보존. 새 `/limitless-trial` SDK 그룹과 기존 FakeExecutor 연결, owner/app/guild/channel/권한·SHA/만료/취소·interaction/followup receipt·한국어 모바일 응답을 구현했습니다. 기존 미술 승인·mobile_offline.py·운영 bot 변경0. 신규 **18 PASS/0 FAIL**, 패키지 smoke5/5. 기존 성공 전체 재실행0.

별도 시험 앱·단일 guild 등록 방식 제안. 승인 bytes13+Phase1 roles1+신규 모듈2=정확16파일 ZIP/비활성 Manifest 준비. CRLF 차이는 승인 raw bytes 사용으로 해결. approved=false·permit/배포/실제 등록0. 실제 ID/권한/ACL/inventory와 한시적 시험 host는 다음 승인 후 확인 필요, 현재 운영 배포 불가.

모델/공식 usage/Discord 게시/운영 SQLite 업무0, MANUAL/실업무 HOLD·A 미활성. 합성 소진은 checkpoint 보존, reset만으로 재개하지 않으며 새 관측·유효 승인 확인. 운영/인증/게임·Save/Push/main/PC 변경0. 상세 `DiscordMobile_Phase3_Adapter_Preparation_20261026.md`, Evidence/DiscordMobilePhase3Prepared20261026/. 시작 commit d2db1e47; 최종 commit은 Issue66 참조. 실제 모바일 왕복은 별도 승인 전 실행 금지.

## 2026-10-26 Issue #66 — Session0 추론 VERIFIED / 2단계 오프라인 연결

실제 request `d56fdba9618044b98f98fe98e4cff211`의 로컬 summary 대조: 서비스 계정 Session0/logon4, CLI exit0·완료 turn1·정확한 한 문장 응답·JSONL PASS. AppServer1+exec1, 응용 재실행0, 전체23.170초, stderr0. permit 비활성·임시 작업 REMOVED·Job 자식0. 운영 보호 및 이전 marker SHA 확인. 추론 토큰 input9934/output22/cached8192. 실제 서버 요청/내부 retry 수 UNKNOWN.

조건부 승인에 따라 2단계 오프라인 구현 착수: 새 합성 DB, 기존 Store/4역할 표/검증기/체크포인트 재사용, SHA 바인딩 실행 승인 후보, 기획→두 아트→Codex→기획 기록, 중복/실패 HOLD/outbox. 운영 `/approve`는 미술 승인용 유지, 새 approve-execution은 오프라인 후보이며 Discord 등록 없음. 새 연결 검증 **24 PASS / 0 FAIL**. 프로세스/네트워크 시도0·scratch 밖 Python 쓰기0. Evidence/DiscordMobileOffline20261026/OfflineTests.json 참조.

추가 실제 모델/usage/Discord/운영 DB 업무0. MANUAL/신규 실업무 HOLD, 운영 bot·아트·부팅·인증·CLI·게임·Save·Push/main 변경0. 상세 `DiscordMobile_Offline_Integration_20261026.md`. 실운영 연결·추가 모델·A 모드는 별도 승인 필요. 마지막 관련 구현 commit `0dffe9738eb8e56fe06af413e19411ab87f72b25`. 최종 문서 commit은 Issue66 완료 보고 참조.

## 2026-10-26 Issue66 인증수정본 신규1회 명시승인 — 새request d56fdba9618044b98f98fe98e4cff211 / 관리자실행대기

대표가d9fa4f7c 후보(b841cf65)의새실전1회를승인. config818c/keyring/ChatGPT증거20b0확인·features.view_image등11제어false/기존read-only검증gate유지. 새ZIPe5315fcc/최종Manifest5de623d3, 47파일runtime44/기존40보호·CLI전체45/운영43SHA일치. 이전usage marker2+717a추론marker총3/이전ZIP·Manifest보존.

새전용run-approved-inference-authfix-once90.ps1·기존90초Job/worker예산/Session0-logon4/usage AppServer1후exec1/자동재실행0/내부builtin retry허용. 신규wrapper구문·변수/비관리자가드/새package집합SHA/marker PASS·기존38/native7전체재실행0. 현재비관리자S9·로컬서비스암호필요로대표직접실행대기. 신규model/usage/configCLI/실배포/activepermit/소비0.

정확관리자명령 Inference90_AuthFix_NewApproval_Ready_20261026.md. 실제결과 ProgramData/LimitlessRelay/InferenceTrials/d56fdba9618044b98f98fe98e4cff211/Output/summary.json대조. **실제1단계6조건VERIFIED면추가지시없이즉시2단계오프라인연결구현·가짜검증시작**. 추가실model/A/운영bot변경금지. 현재1단계미시작·2단계미착수,실패HOLD/자동재시험금지. MANUAL·운영/게임/Push/main/PC변경0. 후보d9fa·준비최종commit은Issue66참조.

## 2026-10-26 Issue66 현재keyring/무효view_image확인 — 지원제어후보갱신 / 새실전승인필요

review93fa150e서비스진단성공·projection20b0e1b0대조. 현재서비스config818c2e7b과거승인일치·keyring/ChatGPT확인. 직접실패401인증누락유지. 시작전error2는tools.view_image무시경고로확인. 승인CLI의인증없는features list2회에서구tools옵션에도view_image=true, features.view_image=false는false(exit0/stderr없음)확인.

격리entry에서구옵션을지원키로대체하고image_generation/sleep_tool/tool_suggest도false추가. 기존keyringSHA gate/명시·Sandbox/검증기/승인/도구차단유지. 회귀38PASS/0FAIL+config비교2PASS·이전정제기17/native7/예산/제품전체재실행0. 실제모델/usage0·config전용CLI2만. 원격추론해결은미검증.

상세 Keyring_ViewImage_Candidate_Resolution_20261026.md / Evidence/KeyringViewImageResolution20261026/. 새후보미배포·새모델request/permit/패키지발급0·기존소비marker/ZIP불변. 이전모델명령재실행금지·새실전은현후보범위/새패키지SHA/신규승인필수. 1단계여전히NOT_VERIFIED·2단계미착수; 향후별도승인실전6조건VERIFIED시추가지시없이2단계오프라인즉시착수승인유효. MANUAL/HOLD·A/운영/게임/Push/부팅변경0. 기반67216003·최종commit은Issue66참조.

## 2026-10-26 Issue66 717a직접원인HTTP401확인 — keyring선택후보55PASS/미배포/1단계미성공

서비스로그review2a87401a성공·projection SHA2adef4fd대조. error10+turn.failed모두HTTP401 Unauthorized/Missing authentication. item3은error/error/error(초기2개설정진단+HTTPS전환오류)·실제도구항목아님. CLIexit1/완료turn0/agent_message0. QUOTA휴리스틱429부분문자열오판가능수정·실제429확인없음. 기존로그/boot/DB/소비marker보호·진단TaskREMOVED/Jobexit0자식0.

과거서비스config818c2e7b=keyring/forcedChatGPT확인·exec ignore-user-config의keyring재선택누락발견. 격리entry에기존configSHA일치gate+cli_auth_credentials_store=keyring명시후보추가, 인증/저장소/토큰/운영/검증기/Sandbox전환완화0. 직접401확정이나선택누락이유일원인·실전해결은미검증. 현재service configSHA/시작전설정경고키는추가정제자료필요.

55PASS(38+17)/0FAIL·새모델/usage/CLI0·후보미배포/새실전permit발급0. 이전ZIP/Manifest/717a소비marker보존·기존모델명령재실행금지. 상세 Inference401_KeyringSelection_Candidate_20261026.md에업데이트된로그전용관리자명령. 서비스계정현재config enum/SHA·기존경고키만정제읽기, 새모델조회아님. 실제재시험은별도신규승인필수.

현재1단계NOT_VERIFIED·2단계미착수. 향후별도승인실전6조건VERIFIED시추가지시없이2단계오프라인연결구현즉시착수승인유효·추가실제모델/A/운영봇변경금지. MANUAL/HOLD·Push/게임/부팅변경0. 기반446b62d1·최종commit은Issue66참조.

## 2026-10-26 Issue66 서비스기존로그진단 준비13PASS — 대표직접실행대기 / 조건부2단계승인기록

실패717a의원인미확정. agent비관리자S9·보호로그는서비스SID로만읽어야하므로새로그전용wrapper/projector준비. 서비스자기DPAPI복호화·원본SHA대조·error10/turn.failed/item3유형·순서·stderr/실행metadata를엄격정제출력, 원본로그전후SHA보호. 모델/usage/CLI실행경로없음·새AIpermit/slot없음. 합성13PASS·PS구문/변수/비관리자가드PASS. 운영/인증/ACL/게임/Push변경0·MANUAL/HOLD.

정확관리자명령 Existing_Inference_LogReview_Ready_20261026.md. 승인부족이아니라서비스계정로컬암호입력필요로대표실행대기. 정제projection수신후원인확인→최소수정/오프라인검증. 기존소비marker보존·추론재시험은별도신규승인필수.

**대표조건부지속승인:** 향후별도승인된실제1단계가exit0/정상turn/최종응답/JSONL PASS/읽기전용준수/기존보안보호모두VERIFIED이면추가지시없이즉시2단계(Discord모바일→승인큐→실행기→결과보고)오프라인구현·가짜검증시작. 실제추가모델/A/운영봇변경금지·MANUAL. 현재조건false로2단계착수0. 새로그projection성공은실제AI성공이아님. 기반9b46a03f·로컬최종commit은Issue66참조.

## 2026-10-26 Issue66 Session0 실제추론 단발 NOT_VERIFIED — 소비완료/종료/재실행금지

request 717a0afa735d485daf790579b964e8c2 실제summary대조. 서비스SID일치·Session0/logon4·usage AppServer1성공후exec1시작, CLIexit1/turn.failed/완료turn0/최종응답없음·Worker/Taskexit2. error10≠retry10, forbidden3실제유형미확인·도구실행단정금지. 내부retry/원격성공·소비미확인. 전체28.758초·Job자식0/정리true·permit비활성/활성경로부재·TaskREMOVED·소비marker존재. 이전승인/마커/CODEX_HOME/부팅/장부보호결과true·MANUAL/HOLD.

상세서비스프로필은현계정OS접근거부로error.code/정제stderr직접원인미확정. ACL/권한우회/검증기완화/새모델·usage/시험재실행0. 필요시다음은기존서비스로그만정제투영하여원인확인; 새실전은별도신규승인필수. 과거prepared미소비상태는이결과로갱신됨. 상세 Inference90_Actual_NOT_VERIFIED_20261026.md / Evidence/Inference90ActualNOTVERIFIED20261026/. 실행source adbcddbd·이번docs-only최종commit은Issue66참조. Push/main/A/게임변경0·중지.

## 2026-10-26 Issue66 내부retry 명시승인 반영 — 90초 독립추론 준비44PASS / 실제0

대표가 app재실행0·추론exec1·builtin HTTP/stream기본retry허용·원격요청/구독정확1회보장불가 승인. 추가답변으로 usage AppServer1+추론exec1 허용확인. 신규request 717a0afa735d485daf790579b964e8c2 미소비, prepared approved=false/expiry0·실전permit/marker/trial없음. 구e0문서예약/사용량소비증거보존.

새Job/entry/core/관리자wrapper/준비도구/테스트7파일만추가. 기존20초Job/운영runtime/인증/CLI/HOME/부팅/DB/봇·아트/게임불변. 서비스90초/부모대기80초/준비차감Job75초·worker70초/로그3초예산. 정확한국어고정응답·도구차단·JSONL/토큰·보호DPAPI로그·kill-on-close/permit/task정리. 준비/보안입력대기는서비스실행과구분·OS90.000초/원격취소보증아님.

신규Python36+native7+예산1=44PASS/0FAIL·최종PS/비관리자가드/47파일SHA PASS. 실제모델/공식usage/Discord/SQLite업무/운영배포0·MANUAL. 상세 Inference90_ReadOnly_Prepared_20261026.md / Evidence/Inference90Prepared20261026/. 관리자정확명령은보고서참조; agent실행금지·다음대표직접실행후summary로판정. A/모바일연결미검증·미승인. 기반67669b9b·로컬최종commit은Issue66/최종보고. Push/merge0·준비후중지.

## 2026-10-26 Issue66 공식 builtin 재시도 정책 검토완료 — POLICY_APPROVAL_PENDING

공식문서/기존exit1증거대조: model_providers는custom정의·openai예약ID 덮어쓰기금지. 실패한retry0두줄은 이전Temp features-list의 -c목록에만존재·운영config/phase2미적용. builtin내부retry0 공식지원법미확인. HTTP4/stream5 문서참고값을 고정CLI의 전체요청상한으로단정금지.

제안만: app/Scheduler재실행0·CLI프로세스최대1/완료턴1·전체90초, builtin HTTP/stream기본retry는대표별도승인필요. 프로세스1≠원격요청1, 재시도는구독한도추가소비가능·로컬kill은서버취소/비용상한보장못함. 기존Job최대20초/구AI작업6분·hardkill금지여서그대로사용불가. 승인후새격리90초Job/entry·정리예산분리·변경분오프라인검증필요, 이번미구현.

모델/usage실조회/CLI/기존QA재실행/운영설정/slot실행소비/permit/배포0. MANUAL/AI_EXECUTION_HOLD·기존운영/인증/부팅/SQLite/게임보존. 과거request e0d2e6fb9a6e48708b35052c02ad133c 미소비유지. 상세 Official_OpenAI_RetryPolicy_Review_20261026.md / Evidence/OfficialRetryPolicyReview20261026/Review.json. 기반b5cd8d9a·이번docs-only commit은Issue66참조. Push/merge0·검토후중지.

## 2026-10-26 Issue66 실제AI 단발 신규승인 — 추론 전 RETRY_POLICY_HOLD

바울 대표가 고정한국어 요청 모델최대1회/90초/자동재시도0 승인. 실제 모델/공식사용량/Discord/SQLite업무0, 새permit/작업/소비marker0. 새request `e0d2e6fb9a6e48708b35052c02ad133c`는 문서예약만·미소비. 현재 agent 비관리자 Session9, 서비스Session0/logon4는 이전PASS이며 이번새실행없음.

기존 phase2는 응용retry0·builtin bounded기본값. 승인CLI의 빈Temp CODEX_HOME `features list`에서 openai request/stream_max_retries=0 override는 reserved builtin provider 오류 exit1. 내부retry0 제어 미확인으로 HOLD, custom provider/보안우회/조건완화0. package45/runtime43 SHA불일치0·기존승인/마커보존. 운영코드/봇/아트/부팅/DB/MANUAL/인증/게임/Save 변경0, 새실시간건강PASS주장금지.

상세 Inference_Session0_RetryPolicy_HOLD_20261026.md / Evidence/InferenceRetryPreflightHOLD20261026/Preflight.json. 다음은 공식builtin retry0지원확인 또는 대표의retry범위결정 후에만 새90초격리runner 준비. 구Discord/SQLite runner사용금지. 기반862b8600·이번docs commit은Issue66참조. Push/merge0·실제AI/A/모바일체인미검증으로중지.

## 2026-10-26 Issue66 공식사용량 단발 실제성공 — Session0/logon4 / 종료완료

request773d9ecb7d944d9783c57ead117f471c 실제로컬summary대조PASS. PAUL-PC\LIMITLESSRelaySvc SID일치·Session0/logon4·격리배포44/기존40보존·account/rateLimits/read id3 PASS/에러없음·appserver/Job/task exit0. launch1/공식사용량1·모델0/Discord0/자식SQLite업무0/재시도0.

관측10/26 10:58:46 KST: 5시간사용81%(잔여19%,10/26 12:19회복), 주간사용13%(잔여87%,11/2 07:19회복), ordinary_allowed=true. 재조회하지않은당시값입니다. permit비활성·활성경로부재/TaskREMOVED/Job자식0/정리확인·소비marker및이전marker보존·OnBoot/boot승인/장부/ManuAL/CODEX_HOME보호PASS.

단발성공후추가조회/모델/A/배포/재시작0으로중지. 과거OFFICIAL_READ_REJECTED/turn.failed원인소급확정금지·실제AI/A는여전히미승인·부팅직후무인AI체인전체검증아님. 상세 UsageOnly_Session0_Observed_PASS_20261026.md / Evidence/UsageOnlySession0PASS20261026/. 실행source b44da900, 완료docs commit은Issue66/최종보고참조. Push/main병합/Unity/게임/Save/종료재부팅0.

## 2026-10-26 Issue66 변수수정본 신규1회 명시승인 — 관리자 실행대기

대표가 e10d02cb 수정본의 새trial1회를승인. serviceCodexHome/childSummaryPath 유지·부모/자식AST충돌0. 이전987a request/launch marker62a867/배포본/ZIP 보존, 새 bindingfix-e10d02cb 전용marker와 실행시새Guid만허용. 새permit은정확trial만·44runtime맵/기존40보존불변.

같은1회응답의안전한사용량숫자를보고하도록entry2줄/PS엄격숫자선택추가, 추가조회0. 표적binding3+보고경계3PASS·구문/단일read정적/비관리자가드/diff PASS·기존44/22+12/Job4전체반복0. launcher7fe0b17f/entry670c922e/새ZIP55241c88. 현재새배포/permit/marker/request/실조회/AI0.

정확한관리자명령 UsageOnly_BindingFix_NewApproval_20261026.md / Evidence/UsageTrialBindingFix20261026/. 현재비관리자라대표직접로컬암호입력대기·새승인재요구아님. 결과후실제계정/S0-logon4/조회수/공식결과/RPC코드/정리/기존보호확인하고성공실패무관중지. 이전marker초기화/자동재시도/모델/A/운영변경금지. 최종commit은Issue66/최종보고참조.

## 2026-10-26 Issue66 단발실패 원인확정·격리변수수정 — 실조회0/재실행금지

987a8f42 배포PASS44/서비스Session0-logon4/SID일치 뒤 PowerShell변수충돌로실패. 자식 home변수→읽기전용HOME 쓰기, 부모결과경로 child변수→switch Child충돌. 실제로Job/Python/appserver시작전차단·후속child-run-marker없음·공식RPC0/AI0/Discord0. 기존OFFICIAL_READ_REJECTED와별개이며환경/인증오류로오인금지.

격리후보에서 serviceCodexHome/childSummaryPath로만rename, 신규binding회귀3PASS·parser/diff PASS. live배포본ea7a 보존·수정후격리3a57. 기존44/22+12/Job4 재실행0. permit비활성/TaskREMOVED·boot승인/OnBoot/장부불변. 날짜별launch fuse62a867 보존·소비슬롯삭제/재발급/새request/permit/실행0. 실제조회0이라도자동재시도하지말고대표새재개지시전중지.

상세 UsageTrial_LauncherVariableFailure_20261026.md / Evidence/UsageLauncherVariableFailure20261026/. source작성오류이며관련로컬fix만완료·미배포. 기존CLI/CODEX_HOME/인증/ACL/게임/Push변경0. 선행e6b24a0b·최종commit은Issue66/최종보고참조.

## 2026-10-26 Issue66 서비스환경 실제PASS 대조·사용량 단발 최종명령 준비

대표전달c332610e와로컬summary일치: 관리자parent Session9/서비스childSession0-logon4·SID/CLI·CODEX_HOME/보호I/O·20초/재시도0·task종료0·REMOVED·부팅/승인/장부불변PASS. 실제usage0/AI0로기존1회승인미소진.

run-approved-usage-once.ps1/UsageOnlyJob.cs 준비: 별도ProgramData trial/정확44허용범위·전체단발fuse/CreateNew/20초Scheduler+Job kill-on-close/남은시간최대17초Python/필수배포SHA/기존entryf176유지. permit비활성·해당임시작업제거·보호로그보존. 가짜Python새Job4건PASS·구문/비관리자가드PASS·기존44/22+12재실행0. 현재새배포/활성permit/실조회/모델0.

정확한관리자직접명령은 ServiceEnvironment_PASS_UsageOnce_LocalRun_20261026.md. 기존승인범위이며새승인요구아님; 이전암호입력을저장하지않아로컬Get-Credential만한번더필요. 대표실행결과후최대1회여부/RPC코드/정리/보호확인하고중지. 지금자동실행/추가조회/AI/A금지. Evidence/ServiceEnvironmentPassUsageOnce20261026/. commit은Issue66/최종보고참조.

## 2026-10-26 Issue66 안전 배치 환경 확인 명령 준비 — 관리자 직접 실행 대기

과거3483af06 서비스Session0/logon4 PASS의 Password/Limited 등록·정리 방식과 기존 BatchReadOnlyMetadata 모듈을 재사용한 restore-service-environment.ps1 준비. 새 parser/비관리자가드/readonly장부도우미/사용량없는명령범위 PASS. 실제관리자/서비스실행은대표직접실행대기이며 기존44·22+12재검증0.

관리자PowerShell에서로컬Get-Credential 후 새20초임시환경작업만실행·정리하도록설계. 실제SID/Session0/logon4/ProgramData I/O/CLI·CODEX_HOME/OnBoot·승인·SQLite·MANUAL보호확인. 이스크립트에는사용량/모델/Discord/업무DB처리경로없음. 현재ProgramData/작업/permit미생성·실조회0·AI0. 기존계정암호/인증/ACL/부팅/봇/게임/Push변경0.

정확한직접실행명령과결과경로는 ServiceEnvironment_LocalAdmin_Wait_20261026.md, 증거 Evidence/ServiceEnvironmentLocalAdmin20261026/Preparation.json. 대표실행완료후 summary와보호/정리조건전부PASS인지확인한뒤기존승인usage1회20초절차계속. 지금자동실행하지말고로컬실행결과대기. 선행84486500/패키지0f98620b보존·최종commit은Issue66/최종보고참조.

## 2026-10-26 Issue66 격리배포·실조회 승인 실행 — 선행환경 HOLD / 실조회0

바울 대표의 배포+조건부사용량1회 승인은 유효합니다. 기준0f98620b·패키지44SHA/기존40·변경3+신규1·PSM1/entry/roles/CLI/boot승인 점검PASS. 현재토큰은비관리자·서비스계정아님·Session9, sandbox밖읽기에서도Task API CimJobException/HResult-2146233087. Session0/logon4와20초supervisor검증불가로 ProgramData배포/permit활성화 전에 지시대로중지했습니다. 공식RPC실패가아닌실행환경게이트입니다.

배포0/서비스시험0/실조회0/AI0/Discord0/trial SQLite업무0/재시도0/시험프로세스0. permit/requestID 미생성·활성permit부재·소비marker/증거보존. 운영52SHA일치·MANUALreadonly확인·원본/운영상태동일. 인증/ACL/CODEX_HOME/OnBoot/봇/아트/게임/Push변경0. 제품코드수정0·문서만기록.

상세 Discord_UsageTrial_ExecutionGate_HOLD_20261026.md 및 Evidence/UsageTrialExecutionGateHold20261026/ExecutionGateHold.json. 재개전기존안전한관리자/서비스배치실행환경·실제Session0/logon4·20초supervisor가필요합니다. 암호/토큰복사·일반사용자시험대체·기존부팅변경금지. 지금자동재시도/대기예약하지말고중지. 마지막진입점commit0f98620b, 문서commit은Issue66/최종보고참조.

## 2026-10-26 Issue66 배포 사전점검·usage-only 준비 — 신규12PASS / 미승인·미배포

970a7247 기존5파일+테스트2SHA동일·기존22검증반복0. 승인목록은42Python+PSM1=43항목, 후보는43Python+PSM1=44. 의미변경3+신규진단모듈1만필요하며 미변경40항목승인SHA보존. checkout의26Python+PSM1 줄바꿈차이는검토패키지에서운영원본bytes로보존했습니다.

새 collab밖 usage-only-once.py: 별도보호permit/정확root·집합SHA·entry/roles/기존CLI·1회20초/소비marker/HOLD를검사하고 provider.read만1회. 모델/Discord/SQLite업무/모드변경0. 기본/비활성초안HOLD. 신규진입점12PASS/0FAIL, 실제AI·실조회·재시도0. ReviewPackage와approved=false초안만준비, 실제ProgramData승인/배포/OnBoot/봇/ACL/인증/게임/Push변경0. 운영52SHA/DB8테이블/MANUAL/원본상태보호.

상세 Discord_UsageOnly_Deployment_Preflight_20261026.md / Evidence/UsageDeploymentPreflight20261026/. 다음은대표의별도trial패키지·정확SHA/44항목/신규permit·실행문맥및20초supervisor검토와실조회1회명시승인. 기존모델runner사용금지·소비marker초기화금지. 지금배포·실조회·AI/A하지말고중지. 최종commit은Issue66/최종보고참조.

## 2026-10-26 Issue66 오류 진단 보존 수정 — 격리22PASS / NOT_DEPLOYED

codex/discord-usage-diagnostics-20261026 격리 브랜치에서 RPC method/id/code·정제message/data·실제종료code·단계/위치·제한stderr를 서비스자기프로필 전용으로 기록하도록 수정했습니다. HOLD/승인/응답검증/Sandbox/인증정책 유지. stderr 동시drain32KiB·정제4096자/로그24KiB·최대64파일·RPC18초+종료정리1.8초. 로그준비/저장실패·비정상종료도HOLD.

신규 가짜서버22PASS/0FAIL·Python7구문/diff-check PASS. 실제AI0/실조회0/재시도0. 운영52파일SHA동일·DB핵심8테이블동일·MANUAL·원본main/status동일. 운영배포/승인SHA갱신/ACL/인증/OnBoot/봇재시작/A/Unity/Push/merge0. 새모듈로43개이므로 기존42개검증기는HOLD가정상; 승인목록은 다음 별도리뷰 없이 갱신금지.

상세 Discord_UsageDiagnostics_Preservation_OfflineQA_20261026.md 및 Evidence/UsageDiagnosticsPreservation20261026/. 마지막관련 기반fcd0488c, 최종commit은Issue66참조. 다음은 격리diff/배포·SHA항목승인과 모델없는 usage-only 진입점검토 후 대표별도승인 최대1회20초재시도0 검증. 과거추론runner 재사용금지. 실제조회·AI/A를 실행하지 말고 중지합니다.

## 2026-10-24 서비스 공식 CLI 실제 추론1회 승인 — 준비완료/로컬 입력 대기 / 실제AI0

- 새 SERVICE_FINAL_PLANNING_20261024_01·별도out/marker 단발실행기준비. 기존42Python/12CONSUMED/과거실패/사용자Worker슬롯재사용0. 실제서비스문맥에서CLI승인SHA/45파일/구독인증/직전공식usage/읽기전용/90초1회/응용재시도0 gate.
- 기존검증기·DPAPI로그·실패투영·Discord게시재사용, 게임전후해시·게시실패NOT_VERIFIED·두마커재실행차단. 순수mock8 PASS·PS구문0·비관리자가드PASS. 실제CLI추론/Discord0·trial소비마커아직없음.
- 현세션비관리자라대표관리자PowerShell의로컬서비스암호입력필요. 이미승인된1회실행이며추가승인요청아님. [한줄명령·보존경로·판정](ServiceCLI_FinalInference_Ready_20261024.md). Bot/Art RUNNING·MANUAL유지·운영참조/부팅/인증/게임/Push변경0. A활성화는후속승인. 선행75117899.

## 2026-10-24 공식 CLI 운영 참조 전환 PASS — MANUAL / 추론0

- 대표로컬실행984adccf… 결과·현설정·백업대조PASS. service_codex.path/sha256 두값만 공식전체패키지 bin/codex.exe /4fd2f057…로변경, 그밖JSON값·기존42Python보존. 승인JSON SHA 0cd2ce55…→3e032ebc… 확인. 원본백업 before-cli-reference-984adccf569441ee9248ff0934af56e5.json SHA검증.
- 전환후서비스 PAUL-PC\LIMITLESSRelaySvc/Session0/logon4에서version/help/taskexit0·45파일PASS·CODEX_HOME유지·임시작업REMOVED·부팅2개불변. 추가읽기대조45SHA/42SHA PASS·SQLite읽기전용MANUAL·Bot/Art RUNNING·AI Worker비활성.
- 실제추론/재시도/A/재시작/인증복사/Unity/Push/merge0. 전환완료이며실제읽기전용1회는별도승인, 새서비스슬롯/직전구독usage/로그보존/90초1회조건필요. 원래turn.failed원인UNKNOWN유지. 선행fd77ce30. [완료근거](Evidence/ServiceCLI_ReferenceSwitch_PASS_20261024.json).

## 2026-10-24 서비스 CLI 운영 참조 전환 승인 — 로컬 관리자 실행 대기 / 미전환

- 승인된두값변경·정확백업/원자적교체·서비스version/help재확인·실패자동복원 단발전환기준비. 기존검증기/42Python/boot실행코드변경0. 합성검사4 PASS(필드보존/교체/실패복원/ACL)·실제운영참조미변경·승인JSON SHA0cd2ce55…유지.
- 공식45 SHA/RX재확인·Bot/Art RUNNING·실제AI0. 현세션비관리자/서비스로컬자격증명필요로전환/사후서비스재확인미실행. 대표관리자PowerShell1줄실행이필요하며암호는로컬보안입력만. [실행명령·전후값·복원](ServiceCLI_ReferenceSwitch_Ready_20261024.md).
- 실제추론/A/서비스재시작/SQLite/인증·CODEX_HOME/Unity/Push/merge0. 전환PASS와실제추론은별개, 다음읽기전용1회는후속승인. 선행e6325932.

## 2026-10-24 서비스 Session0 격리 CLI 실행 PASS — 운영 전환0 / AI0

- 대표의관리자로컬입력실행결과를로컬 run-3483af069a9c4f1387e2fec54f15dd65.json과대조PASS. PAUL-PC\LIMITLESSRelaySvc/Session0/logon4·45SHA·격리공식CLI --version/--help각exit0·taskexit0·CODEX_HOME불변.
- 임시작업REMOVED·부팅2XML/승인목록불변·실제AI0/자동재시도0. 승인JSON SHA0cd2ce55…재확인·Bot/Art RUNNING·DB읽기전용MANUAL확인. 운영CLI전환/실제추론은별도승인, 최초turn.failed원인UNKNOWN·Sandbox/추론성공/PIN전end-to-end미검증.
- 다음승인: service_codex.path/sha와전체45파일무결성관리전환검토, 이후새서비스전용슬롯/90초1회/공식구독usage/로그보존검증. 선행80845ea7. [실행PASS근거](Evidence/ServiceCLI_ServiceBatch_PASS_20261024.json).

## 2026-10-24 서비스 격리 CLI 배치 검증 준비 — 로컬 관리자 입력 대기 / AI0

- 기존성공 Password/Batch/Limited 임시작업+Get-Credential 절차확인. 독립 verify-isolated-batch.ps1 준비: 45SHA/서명·SID/Session0/logon4·고정version/help만·부팅XML/승인SHA전후·임시작업제거, 암호/원문로그0·재시도0.
- 구문0오류·비관리자가드변경없이HOLD검증. 실제서비스실행은대표의관리자PowerShell/로컬보안입력대기. PASS로선언하지않음. 운영참조/42Python/승인목록/SQLite/CODEX_HOME/Task2/모드변경0·AI0. 선행1742abe5. [명령·결과위치·후속승인](ServiceCLI_BatchVerification_Ready_20261024.md).

## 2026-10-24 공식 CLI 격리 배치 — 45SHA PASS / 서비스 실행 HOLD / AI0

- 기존검증공식패키지를 data/runtime/codex/official-0.164.0-alpha.11.1-x64에전체45파일배치, 재다운로드0·전후SHA45일치·본체/보조3개OpenAI서명Valid. 새루트에만보호DACL/서비스RX·현재소유자SYSTEM관리자Full, 기존ACL변경0.
- 현재windows/Session8에서격리CLI --version/--help각exit0, CODEX_HOME변경0. 서비스batch는현계정비관리자/서비스자격증명부재로미실행HOLD. ACL확인과서비스실행성공구분·turn.failed원인UNKNOWN유지. 실제추론/운영참조전환은각각후속승인.
- 42PythonSHA/승인JSON불변·MANUAL·Bot/Art RUNNING·사용자slot승인대기보존. 운영CLI/Task2/SQLite/인증/Sandbox/게임/Push변경0·AI0. 선행0a71e56d. [배치·권한·미완검증·복원](ServiceCLI_IsolatedDeployment_20261024.md).

## 2026-10-24 공식 전체 CLI 격리 검증 — PACKAGE PASS / SERVICE HOLD / AI0

- 공식0.164.0-alpha.11.1 x64전체tar.gz 실제확보·게시SHA41052059…일치·안전추출45파일. manifest버전/레이아웃·본체/host/runner/setup OpenAI Authenticode Valid. rg NotSigned는별도명시. 전체파일SHA근거JSON보존. 새바이너리실행/설치/운영교체0.
- 현재windows비관리자·두task조회거부·서비스config/log접근거부를sandbox밖에서도확인. 기존batch경로는관리자+로컬서비스자격증명입력필수, 새서비스자체진단실행못함. 우회/인증복사0·과거원인UNKNOWN. 정확경로/전후SHA·전체공식배포·참조최소변경·정밀복원·별도1회시험안제출.
- 42PythonSHA일치·승인JSON SHA보존·MANUAL·Bot/Art RUNNING·사용자slot승인대기1건. 실제AI/Worker/Discord/부팅작업/게임/Unity/Push/merge/재부팅0. PIN전추론NOT_VERIFIED. 선행834d785c. [무결성·서비스차단·복구안](ServiceCLI_IsolatedIntegrity_20261024.md).

## 2026-10-24 서비스 CLI 공식 복구 사전 검토 — 배포0 / AI0

- 공식문서에서 단독exe는 완전standalone패키지가 아님 확인. 현재사용자보조3exe 정확SHA/역할·두CLI동일표시버전/서로다른SHA·유효OpenAI서명 확인. helper누락은원인후보이며 최초원본유실로turn.failed직접원인UNKNOWN.
- 공식배포metadata HTTP200/tag rust-v0.164.0-alpha.11.1 및 Windows완전package/SHA 확인. installer는manifest/rg/resources까지검사하므로3exe수동추가불가. 별도공식설치→전체SHA/서비스문맥검증→참조변경별도승인·정밀복원안만제출. 복사/설치/교체/setup0.
- 42PythonSHA모두일치·승인JSON SHA동일·MANUAL·Bot/Art RUNNING·사용자slot1승인대기보존. 서비스PATH/현재config/제한자식프로세스·추론네트워크/오류로그는서비스자체진단필요. 신규Worker실행/12slot재사용/AI/Discord/Task/게임/Push/merge0. 선행2704d1a4. [정확해시·최소복구안](ServiceCLI_OfficialRecovery_Review_20261024.md).

## 2026-10-24 PIN 없는 무인 AI 우선 — 원인 UNKNOWN / 실제 추론0

- 최종 목표는 잠금화면 부팅→Discord 승인→구독 AI→검수·보고. 로그인 후 Worker는 예비로 유지하며 승인 대기 슬롯/기존CONSUMED12개 변경0. PIN전 end-to-end NOT_VERIFIED, Session0 불가로 단정하지 않음.
- CLI SHA/배포 구성 차이 재확인: 사용자 공식4exe vs 서비스 단독exe. 서비스 로그 접근권한 없음·최초 stderr/JSONL 유실로 직접원인 미확정. 완전배포본/서비스내부 설정·sandbox 로그 무호출 수집을 우선, 실제 설치/배포/승인목록 변경0.
- 공식 headless ChatGPT 인증·영속runner 안내 확인. Linux VM/별도장치/VPS 검토안, 현재 PC Pro 확인·관련서비스 조회 미발견. VPS 예시 월US$12(2GiB), 설치/성능/인증검증 전. 기존토큰복사·API전환0. 실제 재시험은 별도 서비스용 승인 및 로그보존 사전검사 후 최대1회/90초만.
- Bot/Art 상태 RUNNING·Worker비활성 확인, MANUAL/부팅작업/42파일SHA정책/SQLite/게임·Save/인증 보호. 문서만 변경·기존테스트 반복0·AI/Discord/Push/merge/설치/재부팅0. 선행8a0191c4. [진단·비용·재시험조건](NoPIN_Unattended_AI_Diagnosis_20261024.md).

## 2026-10-24 Planning 사용자 Worker 실전 준비 — 승인 대기 / AI0

- PAUL-PC\windows/Session8 자기CODEX_HOME에서 공식구독인증·AppServerusage PASS(16:19:44KST,5h68%/주간26%,사용허용). exec/로그인변경/API fallback0. 등록51 SHA동일·Bot/Art RUNNING/잠금유지·MANUAL/claim0. 실제실행직전usage다시확인필요.
- 신규1슬롯 USER_WORKER_PLANNING_READONLY_20261024_01을 WAITING_FOR_OWNER_APPROVAL로 등록. plan42b0861a…/contract정규화8fc083d9…/역할정본ad23c0ca…(docs/planning d73bdbf9). 기존CONSUMED12변경/재사용0·기존task추가0·outbox10유지.
- 기존 실행코드/승인목록은 미변경, real_roles와Worker 두 후보/diff만 별도준비: 단일Planning슬롯·정확승인·5문장계약·추가인계금지·PASS만기존outbox게시. 신규후보6모의테스트PASS·실제AI/Discord0. 후보반영/무결성갱신/실행허가도 대표의 신규슬롯·계획·후보SHA 명시승인 후에만 진행. 자동시작/A/게임/Save/Push/merge/종료0. 선행cd1b3327. [준비보고·정확SHA·차이](PlanningWorker_Trial_Preparation_20261024.md).

## 2026-10-24 SHA 등록·운영 Worker 오프라인 연결 완료 — 실행 비활성 / AI0

- 검토된4파일 내용/SHA 재확인4PASS, ProgramData 별도 Worker 무결성 목록 등록(execution_permission=false/auto_start=false). 기존 부팅검사는 그대로 두고 호환SHA4개만 추가. 기존Python38·원래승인map39항목·그밖모든승인필드·ACL/소유자 보존. collab42 SHA불일치0, 별도진단4/기존38/Worker5/사용자CLI묶음4 총51 SHA검사PASS. OnBoot작업변경0·XML재열람권한불가/실재부팅미검증.
- 기존relay.sqlite 온라인백업 후 Worker전용표4개 연결. 같은transaction에서 기존표12개 모든행SHA동일(tasks21/checkpoint14/outbox10포함). 운영Worker업무0·모의승인/보고삽입0·MANUAL유지·Bot/Art RUNNING heartbeat증가. 로컬 data/user-worker-registration.json/connection.json/runtime-binding.json 근거.
- 인증/usage/승인/checkpoint/정본 gate·중복claim·역할auto_handoff·실패HOLD/기존봇outbox 연결. 후속AI새승인필요. 기존4역할run_once/JSONL검증 그대로, 별도adapter가 자기계정DPAPI원본/timeout부분로그연결. 실제auth/usage/AI/Discord호출0·자동시작/A/유료/아트/Unity/Save/Push/merge/종료0.
- 신규연결8+회귀20+로그3 고유31오프라인PASS, 영향있는4역할전달은최종선별재검증. 실제1회조건: 허용12슬롯모두CONSUMED이므로 새slot지원·변경코드SHA 별도검토가선행, 이후 정확한단일계획/실행허가/사용자SID·CLI·로그검증/공식인증·두usage·정본·예약gate필요. 기존slot재사용/새실행안함. 선행fe0f9865. [완료근거·남은조건](UserWorker_SHA_OfflineConnection_20261024.md).

## 2026-10-24 신규 진단4파일 SHA 검토 — 제안서만 / 적용0

- 4개 원문·SHA·생성branch/reflog/commit·기존코드 참조 검토 완료. 기존승인Python38 SHA일치/변경0, 미등록4개는 이전제안SHA·현재commit blob과4/4일치. 실비밀하드코딩 발견0(전체읽기+알려진키/JWT/PEM패턴0), runtime출력 비밀없음까지 보증하지 않음.
- 네 파일 모두 실행코드(라이브러리2/수동진입점2)이며 자료파일 아님. failure_evidence 낮음/offline_batch_diagnosis 중간/private_execution_log 중간~높음/service_batch_once 높음. 정상부팅·현재UserWorker 기능의존성 필요0, 자동실행/배포대상에서모두제외. 단 현재위치+전체SHA검사유지 전제면4개모두 조건부무결성등록검토대상이며 일부등록으로HOLD해결불가.
- 제안JSON을 파일별 절대경로·SHA·생성/current commit·용도·위험·조건부필요·실행승인false로 보강. ProgramData승인/OnBoot2/실행코드/ACL변경0·이동/우회0·MANUAL유지·AI/auth/usage/Discord0·Bot/Art RUNNING확인. 새승인은 제안작성만, 실제등록/배포/재시험은 별도검토. 전체테스트반복0, 정적JSON/SHA/commit내용일치PASS. 선행eb08148d. [파일별검토](Boot_SHA_FourFile_Review_20261024.md).

## 2026-10-24 이중 실행 구조 준비 — PREPARED_NOT_DEPLOYED / AI0

- 새 Tools/DiscordAIUserWorker에 같은 SQLite 승인 큐/사용자 Worker 상태머신 준비. 승인된 계획만 로그인 전 보관, 로그인 후 실제SID·자기HOME·CLI SHA·ChatGPT인증·공식usage·정본·예약·정지·미소비승인 재검증. 기존4역할 real_roles.run_once 그대로 재사용. 전용claim 중복차단/중단claim 자동재획득0·실패재시도0·기본released=false/자동시작·실행CLI없음.
- 격리SQLite/모의callback 20 PASS. 운영상태 읽기전용 확인: MANUAL·worker_enabled=false·새 schema 미배포. 운영DB migration/새승인/실제AI/auth/usage/Discord게시0, 기존Bot/Art RUNNING heartbeat 유지. 실제 연결·사용자ACL/완전runtime pinning/private로그/승인slot 확장·실증은 배포 전 필요. 기존승인TTL10분·소비된 과거slot 재사용금지 유지.
- 새로 발견: 앞선 진단 추가collab4파일이 ProgramData 부팅승인에 미등록. 실행중 정상과 다음 시작SHA통과는 별개, 다음 시작HOLD 가능. 파일이동 제안은 자동검토가 무결성우회 위험으로 거부하여 이동0/우회0. 부팅작업·승인·검사·기존runtime수정0. 검사유지+정확한4SHA 추가등록 제안서만 준비, 대표 별도검토 필요.
- PIN없는 공식대안 조사: workspace Codex access token(Business/Enterprise)·headless service accounts(pay-as-you-go) 문서 확인, 현재계정 자격/비용 미확인·도입/유료전환0. Session0 원인UNKNOWN·CLI/SHA/모델/Sandbox/프록시차이는 이전보고 보존. Unity/Push/merge/PC종료0. 선행f32d4f04. [구조·가능/불가·검토안](../../Tools/DiscordAIUserWorker/README_KO.md).

## 2026-10-24 서비스 AI 환경 최종 조사 — 원인 UNKNOWN / 추론0

- Operational 로그 비활성화(IsEnabled=false) 확인·변경0. 두 CLI는 같은 버전0.164.0-alpha.11.1이나 SHA/배포구성 다름: 일반 사용자 설치는 동반실행파일3개, 서비스 고정폴더는 codex.exe만. 정상 사용자 model=gpt-6.1-sol, 과거 서비스 config SHA는 keyring/chatgpt 두 설정 템플릿과 일치·명시모델없음. 이 차이는 원인 확정이 아님.
- 현재 사용자 프록시없음/WinHTTP직접·DNS/TCP443 성공. 서비스 당시 전체 환경/추론통신/기존 Codex·sandbox 로그는 미기록 또는 권한부족으로 미확인. Session0·keyring 조합 안정성은 공식 문서만으로 보증 못함. 일반 비대화형 ChatGPT 실행은 공식 안내되므로 서비스 불가로 단정하지 않음.
- 향후 기록 보완: 원본 JSONL 바이트는 서비스 자신의 .codex/limitless-diagnostics/request에 CurrentUser DPAPI 암호화, 비밀제거stderr/전체이벤트·최초error/turn.failed·종료코드·계정/세션/설정 보존. 공용결과로 원문 복사0, timeout partial 보존. 도구/부작용보고와 미지원item 분리 기록하되 양쪽 거부 유지. 새 슬롯/재시험 추가0.
- 합성DPAPI roundtrip 포함 오프라인36 PASS, PS구문/Python compile/diff PASS. 실제AI/로그인/usage조회/Discord게시0·MANUAL/소비슬롯 유지·Bot/Art RUNNING heartbeat 증가·OnBoot수정0. 다음: 같은 계정에서 기존로그/완전런타임 무호출 확인 후 원인 수정 또는 새 단발 진단계획 별도 승인. 사용자 로그인 후 자기 인증CLI AI Worker 분리 대안은 제안만. Unity/아트/Push/merge/종료0. 선행8dbe4076. [최종환경보고](ServiceBatch_Environment_FinalDiagnosis_20261024.md).

## 2026-10-24 서비스 배치 첫 추론 실패 분석 — NOT_VERIFIED / 추가 AI 0회

- 첫 실제 결과 CLI1/배치2·error10·forbidden item2·완료턴0·최종응답없음. Discord는 검증 실패로 NOT_SENT이며 전송 실패 근거 아님. 인증/공식조회 PASS와 추론 성공은 분리. 원본 JSONL/stderr는 최초 구현에서 저장하지 않아 첫 오류/10개 원인/두 item type/turn.failed 직접 원인 UNKNOWN. 서비스 프로필 로그는 현재 Windows 권한으로 접근 불가.
- 확인된 근거 손실만 수정: 단발 실패 시 고정 오류 분류·알려진 item type·내용 제거 projection·고정 실행 manifest 보존. 검증기/금지도구/Sandbox/인증·모델/provider 변경0. 기존 소비 marker·DB CONSUMED 유지, 재실행 안 함.
- 오프라인30 PASS + 과거 합성 replay3 재생(정상2 PASS/금지도구1 거부). 첫 실패 원본 replay는 UNAVAILABLE이며 재생 PASS로 간주하지 않음. 기존 로그만 읽는 추가 진단기 준비·실행 결과 ACCESS_DENIED, CLI/네트워크/AI/Discord 호출0.
- MANUAL·Bot/Art RUNNING heartbeat 증가·OnBoot/서비스코드 수정0. 첫 summary에서 OnBoot2 보존/임시삭제 확인, 현 세션 XML 읽기 권한 없어 새 대조 미검증. 다음은 관리자/기존 서비스 계정에서 오프라인 로그만 확인. 실제 재시험은 원인 또는 진단 계획 검토·별도 새 승인/슬롯 필요. Unity 확인사항 없음. 선행 commit293db5d6. [진단 보고](ServiceBatch_FirstFailure_Diagnosis_20261024.md).

## 2026-10-24 서비스 배치 인증 PASS / 실제 추론 1회 실행 준비

- 대표 실행 결과와 로컬 evidence `service-codex-auth-46bee54f62714b42838b595c731da1b2.json` 대조: 서비스 Codex 인증·공식 5시간/주간 조회 각각 PASS. 대표 전달 결과에서 OnBoot2 보존·임시 작업 제거 확인. 실제 추론0·MANUAL 유지.
- 대표의 별도 실제 AI 1회 승인으로 `service-batch-once.ps1` 및 `collab/service_batch_once.py` 준비. 기존 Password/Limited/비대화형 Session0, 실행 직전 공식 사용량·기존 마감 gate, 최신 GitHub docs/planning PLANNING.md 단일 입력, 추론1회/90초, 기존 출력·JSONL 검증기 및 5문장 검사를 통과한 결과만 지정 Discord 채널에 게시. 1회 소비 마커/DB 슬롯으로 재시도 차단.
- 현재 **PREPARED / NOT_RUN**. 현 Codex 세션은 비관리자이고 이전 Windows 암호는 폐기되어 관리자 PowerShell의 로컬 보안 입력이 필요함. 구문·출력 제한/도구 거부 focused6 PASS, GitHub 실제 단일문서 fetch PASS(d73bdbf9), 운영DB MANUAL/정지0/실행중0. AI/Discord 게시 미실행.
- 다음: 대표가 관리자 PowerShell에서 `Tools/DiscordAICollab/service-batch-once.ps1` 1회 실행 후 `data/service-batch-once-20261024/batch-summary.json` 확인. 실패 자동재시도·A 활성화 금지. Unity 확인사항 없음, 게임/아트/기존 서비스/Push/merge 변경0. 선행 관련 commit a1c5a7bc.

## 2026-10-24 Windows 1326 최소 복구 — 안전 배치 PASS

- 대표 로컬 입력 후 Windows 배치 인증1회 PASS(0), 임시 Password/Limited 작업1건 등록·실행·삭제 PASS. 실제 PAUL-PC\LIMITLESSRelaySvc/일반계정/SID 일치/로그온4/세션0/비대화형/종료0, 14:19 KST. 이전1326 재현 안 됨, 과거 오입력 여부 단정 안 함.
- OnBoot2 XML SHA·배치 권한 전후 동일, 저장 인증/암호 재설정0·AI/Codex/네트워크 호출0·MANUAL 유지·A/게임/Push/merge0. 기존 테스트 반복0, Unity 확인사항 없음.
- CODEX_HOME 환경null·서비스 프로필 정상. 배치에서 Codex 인증/keyring/공식 사용량 접근은 이번 시험 범위 밖이며 별도 지시 시 읽기 검증 필요. 실제 추론/A 활성화 미진행. 구현 a1c5a7bc, [성공근거](Evidence/BatchEnvironment20261024/recovery-pass.json).

## 2026-10-24 Windows 1326 최소 복구 재개 — 로컬 보안 입력 대기

- 계정 LIMITLESSRelaySvc 활성·기존 SID 일치 재확인. 기존 Password/Limited 등록 방식 유지, 선행 배치 권한 보유 근거 유지·관리자 실행 때 새로 점검.
- 등록 전 LogonUserW 배치 인증1회 및 SID 확인 추가. 실패 시 등록0/중단, 성공 시 AI 없는 임시 환경 작업1건만 검증·정리. 구문/네이티브 컴파일 PASS, 실제 인증/등록 재시도는 대표 로컬 입력 대기.
- 기존 부팅2개/저장 Codex 인증/권한 변경0·MANUAL 유지, AI/A/게임/Push/merge0. Unity 확인사항 없음. 선행 commit e41fe62a. 다음은 로컬 실행 결과 확인이며 2번/3번 진행 금지.

## 2026-10-24 1번 배치 오류 진단 — 원인 1326 확인 / 등록 FAIL 후 중단

- 실제 관리자 진단1회: Register-ScheduledTask에서 Windows 자격 증명 거부. 외부 CimException HRESULT0x80131500이 아니라 ErrorId/CIM MessageID의 **0x8007052e / 1326 / ERROR_LOGON_FAILURE**가 실제 원인 계층. 입력 암호 오류/다른 인증 조건의 세부 구분은 비밀 검사 없이 단정하지 않음.
- LIMITLESSRelaySvc 활성·일반계정·SID 일치, SeBatchLogonRight 직접 보유·직접 및 알려진 공통 그룹의 배치 거부0. 실행/작업 경로 확인. 권한/암호/정상 부팅 작업을 임의 수정하지 않음.
- 등록 FAIL, 자식 실행 NOT_RUN, 임시 작업 미등록/정리대상0. 기존 Bot/Art OnBoot2 XML SHA 전후 동일·권한 전후 동일·MANUAL·SQLite 보존, Bot/Art heartbeat 유지. Codex/AI 호출0·2번/3번 미진행.
- 최소 수정: probe-service-ai 오류 진단이 현지화 메시지 대신 ErrorId/CIM MessageID에서 원래 HRESULT를 표시. 안전 배치 전용 진단/계정환경 자식/읽기 LSA 메타데이터 도구 준비. 새 회귀(구문4·암호제거·1326 매핑) PASS, 기존 전체QA 반복0.
- [원인·판정](../../Tools/DiscordAICollab/BATCH_ENVIRONMENT_CHECK_KO.md), [증거](Evidence/BatchEnvironment20261024/summary.json). 다음은 대표의 로컬 서비스 Windows 자격 증명 확인 후 별도 재개 지시로 1번 안전 검증. 아직 AI 추론·A 활성화 단계 진행 불가.
- 원본게임/Unity/Save/아트/기존인증/재부팅·종료/유료API/Push/main병합0. 마지막 관련 선행commit64a0fd81, 이번 관련 commit은 codex/discord-ai-collaboration 후속 기록 참조. Unity 사용자 확인사항 없음.

## 2026-10-24 서비스 계정 Codex 공식 구독 인증·사용량 VERIFIED — MANUAL 유지

- 실제 LIMITLESSRelaySvc 일반계정 콘솔에서 공식 device-auth로 로그인 성공. login status ChatGPT 확인·같은 SID/프로필/CODEX_HOME/실행파일·설정SHA 전후 동일. Win32 DACL 실제 오류0, 소유자/그룹/SACL/계정권한 변경0. keyring에 자기 신규 인증 저장, 개인 계정 인증/설정 복사0.
- 공식 App Server 조회 AUTH_AND_USAGE_VERIFIED/READONLY_LINK_READY. 로그인 직후 관측은 5시간72% 사용/28% 남음·초기화 10/24 14:09:57 KST, 주간11% 사용/89% 남음·초기화 10/31 09:09:57 KST. 과거 관측으로 실행하지 않으며 실제 시작 전 새 공식 확인 필수.
- 서비스 계정 CLI0.164.0-alpha.11.1·공유 동일SHA 실행파일·device-auth/App Server 지원 PASS. 개인 설치 폴더 ACL은 그대로. 런타임39/인증도구4 SHA 고정, 기존 Bot/Art/부팅 미재시작·MANUAL·업무/승인/usage/체크포인트 보존. 실제AI/유료API/제품·Save·아트원본/Push/main병합0.
- 관리자 창의 실제 VerifyAuthAsService도 REGISTER_SERVICE_LOGON_TASK/CimException으로 등록 단계 HOLD(native_status_hex 없음). 인증 RPC에 도달하지 않았으며 콘솔 인증 성공과 분리. 배치 keyring/프로필 읽기 미검증, 기존 OnBoot2종 수정/재등록0·Bot/Art 유지. 최초 비관리자 창 거부도 정상 보호 게이트로 구분.
- 다음: Windows 단발 배치 등록 오류 별도 확인 후 동일 keyring/프로필/login status/사용량 검증 → 새 연결 코드 정상 로드 → 대표 명시적 A 활성화 및 새 정확한 읽기1건 계획/슬롯 승인 → 별도 승인된 추론1회 실증. 이번에는 승인/큐 삽입·A 자동전환0.
- [성공근거](Evidence/ServiceCodexAuth20261024/login-success.json)·[상태](Evidence/ServiceCodexAuth20261024/summary.json)·[절차/대안](../../Tools/DiscordAICollab/SERVICE_CODEX_AUTH_KO.md). 인증수정 a0eaae4f·배치검증 도구/완료근거485e4665. Discord 최종 인증성공/별도배치HOLD 보고 SENT(영수증1563374682206503015). 기존 전체QA 반복0, Unity 직접 확인사항 없음.

## 2026-10-24 A 서비스 실행기 연결 준비 — 16 PASS / 실제 서비스 인증 HOLD

- 부팅 Bot의 동일 lock/DB 경로에 서비스 SID/일반계정/자기 프로필 확인·공식 구독/사용량 읽기 검사 연결. 성공 시 기존 UsageMonitor→ResumeQueue→네 역할 공용 VERIFIED run_once 재사용, 실패 시 중계만 유지. MANUAL 자동 변경0. 구현 commit `94a0f739`.
- 새/선별14+Bot연결2=16 PASS/0 FAIL, PS구문0오류. 기존28·전체QA 반복0. 부팅 승인39 SHA 및 C:bootstrap/wrapper/supervisor 일치·ACL/소유자 보존. 기존업무21/usage13/설정28/승인0/재개큐0/체크포인트14 및 MANUAL 보존, 보호7파일 동일. Bot/Art 재시작·중복기동0.
- 현재 실제 프로세스는 기존 relay_only AI HOLD 유지·새 코드 로드 전. 실제 서비스 계정 단발 점검은 SERVICE_CODEX_PROFILE_NOT_INITIALIZED HOLD. SID/일반계정/자기 프로필 검사 후 .codex 초기화 확인 불가로 공식 인증·사용량 RPC 시작0, 5시간/주간 실제 회복시각 UNKNOWN. 다른 계정 인증 복사·우회·로그인/설치0.
- 대표가 pwsh -File 방식으로 서비스 단발 점검 완료(request_id4858235fe10c40bea75ccc55f35521df). 결과·현재 장부 지문 동일 근거 저장. 최종 Discord HOLD 보고 SENT(영수증1563356946449301625). 최초 local 결과와 실제 서비스 결과를 구분했으며 초기화 불가 시 AI HOLD 지시 준수.
- 다음: 별도 서비스 계정 정식 Codex 프로필/구독 로그인 준비·필요 시 CLI 접근 확인 → 단발 읽기 재검증 → 성공 시 기존 서비스 정상재시작/동일 lock 확인 → 최신 정본의 새 읽기1건 계획·슬롯 승인 → 대표 명시적 A 활성화 → 승인1건 실증. 승인 TTL600초는 연장하지 않으며 장시간 소진 후 만료 시 재승인 필요.
- [절차·검증](팀운영/Service_AI_Bridge_Readiness_20261024.md), [근거](Evidence/ServiceAIBridge20261024/summary.json). 실제AI/유료API/원본게임·Save/미승인아트제품적용/Push/merge0, Unity 확인사항 없음. 실제 PIN전부팅/USB재연결은 기존 미검증 경계 유지. 마지막 관련 구현 commit `94a0f739`.

## 2026-10-24 주일 승인 AI 업무 허용 — 코드·부팅 SHA 반영 / MANUAL·AI HOLD 유지

- 기존 Discord 협업 실행기 네 파일의 주일 차단만 제거. 일요일 승인 업무·활성 A의 사용량 회복 재개 허용, 승인/모드/한도/미술 최종 승인·제품 적용·Push/main 병합 제한 유지. 구현 commit `9cf84605`.
- 격리28 PASS/0 FAIL, 실제 AI 호출0. 부팅 승인38 SHA 및 C:bootstrap/wrapper/supervisor 일치, 승인파일 ACL/소유자 보존. 부팅/Wi-Fi 보호7파일·기존업무21/usage13/설정28/승인0/재개큐0/실제MANUAL 지문 동일. Bot/Art heartbeat 유지·서비스재시작0.
- 현재 Bot은 부팅전용 중계(AI worker/재개큐 미연결)이므로 **실제 AI 자동 실행 불가**. 새 코드는 다음 정상 시작 때 로드되며 BootRelay AI HOLD는 유지. A 가능으로 오보고하지 않음.
- Discord 완료/보류/승인필요 보고 SENT 영수증1563350905108168806. 태스크 경로는 저장된 관리자 검증+현재 C:승인파일 대조 일치, 현재 태스크원문/프로세스명령줄 직접 조회는 접근 제한으로 NOT_VERIFIED.
- 다음: 서비스계정 Codex 인증/공식사용량·AI 실행경로·실제quota회복 재개 검증 후 대표 명시적 A 활성화. 현재 AI범위는 읽기전용 검토이며 일반 개발/Unity worker 미연결. 실제 PIN전부팅/USB재연결 미검증. Unity 사용자확인 없음, 원본게임/유료API/Push/merge0.
- [정책·미검증 조건](팀운영/DISCORD_SUNDAY_AUTOMATION_POLICY_20261024.md), [근거](Evidence/DiscordSundayPolicy20261024/summary.json). 마지막 관련 구현 commit `9cf84605`; 마감문서는 동일 브랜치 후속 commit 참조.

## 2026-10-24 부팅 중계 등록·비대화형 시작 시험 완료

LIMITLESS-Bot-OnBoot / LIMITLESS-Art-OnBoot 등록2/2 및 대표 실행 결과 PASS_NONINTERACTIVE_BATCH_ONLY 확인. 관리자 검증 기록상 서비스 일반계정 활성·관리자 아님·Password/Limited·BootTrigger 유지. 09:10 KST 후속 읽기 조회에서 Bot/Art RUNNING·heartbeat 3초 이내, MANUAL·재개큐0·기존업무21/usage13 확인. 이번 재시작·AI·재부팅·Unity·원본게임변경·Push/merge0. 일반권한의 태스크 직접 조회는 기존 ACCESS_DENIED이므로 관리자 결과 파일 근거와 현재 heartbeat를 구분합니다. 실제 PIN 전 부팅/USB Wi-Fi 재연결은 NOT_VERIFIED이며 별도 재부팅 승인 때 확인합니다. Unity 사용자 확인 사항 없음. 마지막 관련 commit c8965d33(등록 안내), 이번 마감 commit은 해당 브랜치 로그 참조. [완료근거](Evidence/BootRelay20261024/resume-verification.json), [등록 후속](팀운영/Boot_Relay_PreRegistration_20261024.md).

## 2026-10-24 실제 부팅 등록 승인·로컬 입력 대기

대표의 OnBoot2종 실제등록/필요최소권한 승인 유지. 관리자 로컬 등록기·서비스계정 보안입력창 준비(6721ed4b0fc1d0839610b82530922c5073bc8501). 구문2·배치권한보조코드컴파일PASS, 전체QA반복0. 관리자 PowerShell 실행 명령을 안내했고 암호/토큰 직접입력 대기 WAITING_FOR_LOCAL_SETUP. 마지막확인 계정없음/작업0/2·기존프로세스유지·추가AI/재부팅/종료/제품변경/Push/merge0. 실제PIN전부팅 NOT_VERIFIED. 다음은 추가승인요청 없이 대표 실행 후 registration-result.json과 실제태스크/일반계정/트리거 검증. [실행절차](../../Tools/DiscordAICollab/BOOT_REGISTER_KO.md)

## 2026-10-24 PIN 없는 부팅 중계 등록 전 준비

전용 일반계정 LIMITLESSRelaySvc·OnBoot+30초·Password/LeastPrivilege·C:bootstrap·기존단일SQLite/lock·새계정DPAPI·MANUAL/AI HOLD 경로 준비. OS등록/계정생성/C배포0. 새13 PASS·호환선별3 PASS/0 FAIL·PS구문2 PASS·승인없음HOLD·현재Discord HTTPS PASS. 실제 로그인전/USB재연결/새계정권한·Codex인증/F·납품함접근/재부팅 NOT_VERIFIED(계정아직없음). 기존서비스미재시작·MANUAL/큐0·원본7356변경/누락0·기존DB동일·AI/유료API/종료/Push/merge0. 구현be274f93842ac726f959afbe50d6f6b1bddd70eb. 다음은 등록안·제한권한 별도승인 후 계정/토큰/비대화형 실증, 실제재부팅은 별도승인. Unity 사용자확인 없음. [등록안](팀운영/Boot_Relay_PreRegistration_20261024.md)

## 2026-10-24 운영 실사용 준비 최종 점검

실제 mode/usage/schedule-status 한국어 조회3종 대표확인·Gateway영수증PASS, MANUAL/재개큐0 유지. 추가 격리선별8 PASS/0 FAIL, 기존36/27 전체재실행0. A서버/합성응답PASS이나 이전화면오류 해결확정 NOT_VERIFIED. OS XML2종 로그인30초/동일사용자/최소권한/IgnoreNew/재시작3/강제종료금지/SHA일치·비활성/미등록 확인. 실제quota회복AI/재부팅/로그아웃/통합/종료 미검증. 원본7356변경/누락0·기존21업무/13usage/승인/설정/아트5동일. 실행코드변경/서비스재시작/AI/유료API/OS등록/종료/Push/merge0. 관련검증996f8888. 다음은 별도승인 후 자동시작 등록 또는 실제 재개 실증, Unity 사용자확인 없음. [최종보고](팀운영/Operations_Readiness_Final_20261024.md)

## 2026-10-24 Discord A·B 운영 모드 구현

A/B 영속선택·배타전환·체크포인트·주일접수/재개금지·정본SHA 재검증 HOLD·B finish 예약/범위/취소 구현. 변경분36 PASS/0 FAIL; Discord17명령 실제등록/재접속 PASS, 대표 전체정상 확인·장부 B/MANUAL 실제응답PASS, A서버전송PASS/대표오류보고로화면 NOT_VERIFIED·새finish예약실응답 NOT_VERIFIED. 로컬시험정리MANUAL복귀. 실제 quota회복 AI재개/재부팅/PC종료 미검증. 실제 통합 실행기 미연결·승인/미저장/동기화 불명시 HOLD. 기본MANUAL/대기0·추론/유료API/종료/OS등록/Push/merge0. 원본7356변경/누락0·기존21업무/13usage/승인/설정/아트5동일. 구현1e794393. 다음: 대표 새 Slash 응답 확인, 별도 승인 후 실제 운영 검증. Unity 사용자확인 없음. [보고](팀운영/OperatingModes_AB_20261024.md)

## 2026-10-24 운영 자동화 실연동 VERIFIED

기존9+새6 Slash 실제등록·대표계정6종 Gateway/비공개응답 확인. 시험SCH-79e9740a0440 취소/접수보류해제. 공식Codex App Server 사용량120초 자동조회·소진/실패보류·회복후재검증·기존미소비승인 단발재개연결, 실제 재개대기0/추론0. 신규모의27 PASS/0 FAIL·정상중지exit0/재접속/중복차단PASS. Windows Art/Bot 동일사용자로그인30초·최소권한·재시작3회 비활성XML 준비·등록0. PC종료 실제비활성·동기화/미저장작업 미확인HOLD. 원본7356변경/누락0·기존21업무/13usage/승인/설정/아트입고동일·격리SHA일치·TEST_ONLY재처리0·AI/Unity/제품/Push/merge0. 구현7ef5495312ab6d986a646f8ee32d3656e823c2bd. [완료보고](팀운영/Operations_Automation_Live_Verified_20261024.md). 봇·수령서비스 정상재접속 후 실행중. 다음은 별도 승인 후 자동시작 등록/활성화, 실제 한도소진/AI재개·재부팅·로그아웃 미검증. Unity 사용자확인 없음.

## 2026-10-23 Phase3 대표 일반 PNG 자동 수령 VERIFIED

대표23:05:32 일반첨부→감시기23:05:34 자동등록 기록을23:26:56 실제 Discord/저장 SHA 대조. L-5546382550914017, PNG677×626 RGBA/563323bytes/SHA b9a6be63…176006. 기존3대기업무와 별도 TEST_ONLY·BLOCKED/NOT_VERIFIED 유지·승인전환 차단 보강. 신규로컬7 PASS/0 FAIL·실제9 PASS/0 FAIL, TEST_ONLY Discord보고1563197037779099649 SENT/재조회PASS. 이번재업로드/수동업무생성0·전체21→21·원본7356변경/누락0·다른업무/승인/사용량/설정/수령이벤트동일·AI/Unity/제품/Push/merge0. 구현51a60ca38c07357032b7901e64afdca45e0d8f01. [실제 보고](팀운영/Phase3_Owner_Attachment_Verified_20261023.md). 수령서비스 정상·미재시작, 자동시작 조사/설계만·OS등록0. 다음은 별도 승인 후 같은 사용자 로그인 기반 자동시작 구현/등록 검토, 실제 재부팅/로그아웃 검증과 Unity 사용자확인 없음.

## 2026-10-23 예약·사용량 보류·조건부 종료 — 모의 검증 완료

서울 마감/정리/수정/취소·영속 보류·6개 Discord 명령 소스·공식 사용량 UNKNOWN/소진 차단·고정 Windows 점검 XML·종료 조건 모의 평가 구현. 신규48 PASS/0 FAIL(47+레거시1), 기존 납품 선별3 PASS/0 FAIL·구문 PASS. 원본7356 SHA변경/누락0·기존21업무/13사용량/승인/28설정 내용동일. 추가 외부AI/Unity/제품/OS등록/실제종료/Push/merge0. 구현08f6973ab91f22efe58bedba4006568be9ea22f0. [완료 보고](팀운영/Scheduling_Usage_Shutdown_Simulation_20261023.md). 기존 봇/수령 서비스 미재시작·새 Slash 실제 등록/응답 NOT_VERIFIED. 공식 사용량 자동 브리지/무인 재개/실제 통합·종료 실행기는 미연결, 기본 HOLD. 다음은 별도 활성화 후 새 명령 실연결 및 공식 관측 브리지 검토, Unity 사용자확인 없음.

## 2026-10-23 Phase3 아트 자동 납품 — 수령 감시 실행 중

Discord 아트-납품 및 Windows 납품함 연결·격리 PNG/ZIP 검사·SHA 중복 차단·Codex BLOCKED 업무 등록·Discord 보고 구현. 신규 로컬45 PASS/0 FAIL, 실제 파일 기준8 PASS/0 FAIL·보고5건·HOLD 업무3건. Discord 실제 첨부는 봇 SELF_TRANSPORT_TEST이며 대표 신규 첨부 일반 수령은 NOT_VERIFIED. 원본7356 SHA변경/누락0·기존17업무/승인장부보존·추가AI/Unity/제품/Push/merge0. 코드 be8441e4bdb4769b67b9e925a981bcc4ccc9a283. [완료 보고](팀운영/Phase3_Art_Delivery_Verified_20261023.md). 수령 감시만 RUNNING·AI/Unity worker 비활성·Windows 자동 시작 미설치. 다음은 대표 첨부 관찰 및 별도 승인 후 격리 Unity Worker 구현·실행 검토이며 이번 Unity 사용자 확인 없음. 아래 과거 단계의 미구현 표기는 당시 이력입니다.

## 2026-10-23 네 역할 실제 자동 인계 파일럿1건 VERIFIED — 종료 후 중지

FOUR-43a6eb4146c0: Planning1→NewArt1/Renewal1→Codex1→원래Planning기록자동연결 성공. 실제CLI4회/각90초읽기전용/순차동시1·최대2유지/재시도0·수동중계0. 입력73024/출력1190, 네출력JSONL/최종검증PASS·Discord5영수증PASS. 실제인계9PASS/0FAIL·신규fake10PASS/0FAIL·구문PASS, 기존QA전수재실행0. 최신59HOLD/58남문신규자산0·63단일포즈PASS/교체HOLD·대표승인대기·CC BY충돌유지. 기존13업무evidence불변/승인장부동일·원본7356 SHA변경/누락0·git상태동일·Unity/PNG/ZIP/Save/제품/미술자동승인/Push/merge0. 구현3406801d. [성공보고](팀운영/Four_Role_Real_Handoff_Verified_20261023.md), [파일납품자동화조사](팀운영/Art_Delivery_Automation_Feasibility_20261023.md). 인증Slash ZIP검역은코드존재·폴더감시/UnityQAworker/비교자료승인요청연결은미구현·이번미활성. 다음권장작업은명시적납품함/완료Manifest계약정의후별도지시. 이번슬롯재개/추가AI금지·Unity사용자확인없음.

## 2026-10-23 RenewalArtAgent 실제1회 VERIFIED — 네 역할 개별 확인 후 중지

대표 승인 Renewal만 공식ChatGPT구독CLI 읽기전용1회/90초상한. exit0/15.313초/turn1/최종파일·JSONL검증PASS, 입력17737/캐시0/출력335토큰. Discord [REAL AI / VERIFIED / RenewalArtAgent] 1563169806088142859 게시PASS. Ch1·2범위/승인원본보호·단일포즈TECH_PASS/애니메이션교체HOLD/대표미술승인대기/CC BY3·4충돌미해결 인식 확인. 다른AI/재시도/API전환0·네AI전체자동협업미실행. 신규fake4PASS/0FAIL·구문PASS, 기존12업무evidence불변·원본7356 SHA변경/누락0·git상태동일·Unity/PNG/Save/제품/미술승인/Push/merge0. 구현84c72a1d. [성공보고](팀운영/RenewalArtAgent_Real_Verified_20261023.md). 이번시험종료후중지, 다음업무인계는별도지시필요. Unity 사용자확인 없음.

## 2026-10-23 NewArtAgent 실제 재시험 VERIFIED — 1회 성공 후 중지

대표 새 승인으로 NewArt만 공식ChatGPT구독CLI 읽기전용1회/90초상한 실행. exit0/13.641초/turn1/최종파일·JSONL·엄격검증PASS, 입력18134/캐시0/출력378토큰. Discord [REAL AI / VERIFIED / NewArtAgent] 1563167449531154594 게시PASS. Chapter3범위·승인43/지면9보호·Orna visible80/canvas84.63·KEEP/HOLD·최신58 NEW_ART_REQUIRED_0·대표최종미술승인대기 인식 확인. Renewal/다른AI/재시도/API전환0. 새슬롯CONSUMED/과거실패보존·기존11업무evidence불변·원본7356 SHA변경/누락0·git상태동일·Unity/PNG/Save/제품/미술승인/Push/merge0. 새경계fake4PASS/0FAIL·구문PASS, 기존9및전수QA반복0. 구현fcb9167e. [성공보고](팀운영/NewArtAgent_Retest_Verified_20261023.md). 이번1회종료후중지, Renewal등다음단계별도승인필요. Unity 사용자확인 없음.

## 2026-10-23 NewArt 실행 실패 조사 — 추가 AI0·진단 누락 보완 후 중지

프로세스 exit1 반환·첫 JSONL 전 종료 확인, timeout/OSError 생성실패 아님. stderr 원문 폐기로 내부 초기화/모델요청 도달 여부·근본 원인 미확정. 성공 Planning/Codex와 공통CLI/ChatGPT/옵션/schema 동일, 입력·폴더만 다름; 역사적 실행 환경값/바깥권한은 미기록. argv/cwd/경과시간/관측단계·비밀 없는 오류분류만 추가, 보안/파서/PASS판정 유지. 신규fake8+기존자동인계선별1=9PASS/0FAIL·구문PASS, 실제AI추가0/usage증가0. 기존10업무+NewArt실패evidence불변·원본7356 SHA변경/누락0·git상태동일·제품/Unity/PNG/Save/미술승인/Push/merge0. 구현1129756c. [조사보고](팀운영/NewArt_Exec_Failure_Forensics_20261023.md). 실제재시험 미실행·성공미검증, 별도승인과새계획/슬롯 전 재개금지. Unity 사용자확인 없음.

## 2026-10-23 아트 AI 실제 연결 — NewArt 첫 실패로 중지

대표 승인 각1회 범위에서 NewArt CLI1회 시도 exit1/JSONL0/최종응답0/NOT_VERIFIED, Renewal0회. 토큰 미수신·추론 성공 미확인·자동 재시도/API전환0. Discord 실패보고1563161292213063801. 새 fake 경계8PASS/0FAIL·Python구문PASS, 실제 연결 실패1/후속미검증1. 기존 Planning/Codex 업무10개 불변·원본7356 SHA변경/누락0·git상태동일·제품/Unity/아트/Save/미술승인/Push/merge0. 구현85e1e243. [보고](팀운영/Art_Roles_ReadOnly_Stopped_20261023.md). 두 담당 성공조건 미충족으로 다음 납품 자동화 조사는 보류. Unity 사용자확인 없음. 다음 실제 재시험은 별도 승인 필요, 이번 소비슬롯 재사용금지.

## 2026-10-23 첫 실제 AI 자동 인계1건 VERIFIED — 성공 후 중지

인계AUTO63-85cd0486e4e5: Planning1회→Codex1회→원래Planning업무기록수신 자동완료. 기획L-37f83d21152d4df8 REVIEW_PENDING·Codex L-85bcfa2bec254668 DONE, 요청해시/실제Codex입력해시동일·답변원업무evidence연결. Discord요청/답변/수신3영수증PASS. 기획수신은추가AI추론아님·중간수동복사0·재시도0·다른AI0.

Planning 입력16914/출력283, Codex 입력15110/출력305, 합계입력32024/출력588. 각exit0/turn1/검증PASS·각90초읽기전용. 실제성공기준7PASS/FAIL0·새fake테스트7PASS/FAIL0(구분기록). 구독인증/추가유료API전환/결제0, 호출별구독금액은독립확인불가. 기존VERIFIED evidence불변·슬롯2개CONSUMED·원본7356 SHA변경/누락0·git상태동일·Unity/Save/제품/아트/미술승인/Push/merge0·SFX PAUSED. 구현fcb2aba2. [성공보고](팀운영/Real_Auto63_Handoff_Verified_20261023.md). 이번한건종료후중지. 아트AI연결은다음별도승인 전실행금지. Unity추가확인없음.
## 2026-10-23 CodexAgent Issue63 실제1회 VERIFIED — 성공 후 중지

바울 대표 승인으로 CodexAgent만 공식ChatGPT구독CLI 읽기전용1회/90초상한 실행. AGENTS/CODEX원문blob검증·Issue63본문/최신댓글입력, 실제Unity/PNG/권리검수0. exit0·completed_turns1·수정검증기PASS·도구/오류0, 입력16928/캐시0/출력316토큰. Discord [REAL AI / VERIFIED / CodexAgent] 1563152371192827905 게시PASS. 재시도/유료API전환/아트AI실행0.

Planning VERIFIED파일/업무상태보존. Codex업무L-eae92127f4d442f4 DONE, 후속기획L-ad951113bdea49f0 NEW큐만생성·Planning추가추론0. 실제상호AI회신자동협업은다음별도승인범위. 역할별승인슬롯모두CONSUMED. 변경분fake역할/중복/아트차단2PASS·기존39PASS보존. 원본7356 SHA변경/누락0·git상태동일·Unity/아트/Save/제품/미술승인/Push/merge0·SFX PAUSED. 구현c8724c0c. [성공보고](팀운영/CodexAgent_Issue63_Real_Verified_20261023.md). 이번시험완료후중지, 다른AI실행금지. Unity추가확인없음.
## 2026-10-23 Planning 최종 승인 재시험 VERIFIED — 성공 후 중지

바울 대표 재시험명시승인으로 PlanningAgent만 공식ChatGPT구독CLI1회/읽기전용/90초제한 실행. exit0·completed_turns1·수정파서/최종파일대조PASS·도구/오류0. 입력15747/캐시0/출력371토큰, 실제summary Discord [REAL AI / VERIFIED] 1563149428318015663 전송PASS. 다른담당추론0·자동재시도0·유료API전환/추가결제0. 호출별구독금액은독립확인불가.

계획f19fd546.../16687bytes. 옛슬롯CONSUMED보존·이번별도슬롯CONSUMED·usage1행COMPLETED. 텍스트검토대기추가로APPROVAL_PENDING2/DONE2/NOT_VERIFIED2, 미술승인아님. 기존39PASS보존·슬롯분리신규1/기존회귀1PASS. 원본7356 SHA변경/누락0·git상태동일·Unity/아트/Save/제품/Push/merge0·SFX PAUSED. 구현bfe387a6. [성공보고](팀운영/Planning_Final_Retest_Verified_20261023.md). 이번시험완료후중지. CodexAgent등확장은별도승인필요. Unity추가확인없음.
## 2026-10-23 출력 거부 오프라인 조사·최소 수정 완료 — 추가 AI0 / 중지

과거a59b5783 real_roles.py:107 통합거부 위치 확인. 원문JSONL/최종파일/진단미보존·ephemeral 저장스레드0이라 실제세부거부조건확정불가. 중간agent_message를최종JSON으로오해하는 결함은 합성재현해수정했으나 과거실제원인으로단정하지않음. JSONL정책검사와공식output-last-message 대조 분리·개별거부사유/turn/카운터보존·한국어오류·임시원문정리. 필수필드/역할/단일turn/도구차단등유지.

로컬39 PASS/FAIL0(파서22·입력선별4·fake실행기13), 실제추가호출0·유료전환0·슬롯CONSUMED/기존업무상태유지·Discord연결유지/기존오류재게시PASS. 입력후보25497→16687bytes/34.6%감소, AGENTS/역할전체blob검증, 실제토큰절감미검증·미승인후보만. 원본7356 SHA변경/누락0·git상태동일·제품/미술승인/Unity/Push/merge0·SFX PAUSED. 구현5d2e4241. [보고](팀운영/Planning_Output_Rejection_Offline_Fix_20261023.md). 실제CLI end-to-end 재시험은 별도승인필요하며 이번작업중지. Unity추가확인없음.
## 2026-10-23 PlanningAgent 승인 실제1회 — 결과 검증 NOT_VERIFIED / 중지

바울 대표 별도승인으로 계획e9e0e683...의PlanningAgent만 공식구독CLI1회/90초제한 실행. exit0·입력18075/캐시0/출력385토큰 수신, 최종구조/정책검증 CLI_OUTPUT_REJECTED_NO_RETRY로NOT_VERIFIED. 원문미보존이라 상세거부조건확정불가. 실제호출완료와요약채택실패를구분합니다. 승인슬롯CONSUMED·재호출0·다른역할추론0·API전환0. Discord [REAL AI / READ ONLY] 실패/사용량보고1563141744839888897 전송PASS.

기존DONE2/모의승인대기1/과거미검증1보존, 새미검증1추가. SQLite정상·원본7356 SHA변경/누락0·status동일·게임/미술승인/Unity적용/Push/merge0·SFX PAUSED. 추가유료API비용0, 구독호출별금액/차감정산미확인. 구현908d738d. [결과보고](팀운영/PlanningAgent_First_Real_Trial_Result_20261023.md). 이번시험중지. 후속실제재시험/Codex연결은별도승인필요, 장부초기화/자동재시도금지. Unity추가확인없음.
## 2026-10-23 PlanningAgent 첫 시험 준비·한국어 Discord 조회 완료

status/pending을 한국어로 표시하고 바울 대표 실제확인 PASS. 기존DONE2/APPROVAL_PENDING1/NOT_VERIFIED1·SQLite정상·MOCK업무보존. 모의/텍스트연결결과를 실제미술승인에서 제외. 첫실제시험은 PlanningAgent 최신GitHub정본요약1회만 허용, Codex우선권고는 최신지시로대체·확장은별도승인. CLI구독인증/설정PASS, 실제추론0·승인슬롯미등록·새실제업무0·비용0.

새 계획SHA e9e0e683f43456f67b7468e014699221e3c563eb63f9ba4257a466b3dbba0b38, 입력25497bytes·읽기전용1회/90초/Discord보고에 별도승인필요. 변경분19테스트PASS/FAIL0, 기존전수QA반복0. 원본7356 SHA변경/누락0·status동일·게임/미술승인/유료API/Push/merge0·SFX PAUSED. 사용량조회시계정전체5시간96%·주간71%잔여, 실제호출별구독차감은미검증. 구현ca356af7. [보고](팀운영/Discord_Planning_First_Approval_Pending_20261023.md). Unity추가확인없음. 다음은 계획검토후별도승인대기이며 실제AI자동호출없음.
## 2026-10-23 Discord 실제 AI 준비 — 별도 승인 대기

실제 status2/pending1/handoff1 응답 영수증으로3명령 PASS 확인. MOCK 보존·별도 구독 CLI 실행기/4역할 SHA고정 입력 준비, 실제 AI0·승인슬롯미등록. 변경분 가짜subprocess12 PASS/FAIL0, 기존QA반복0. GitHub main/기획/신규/리뉴얼 역할·최신인계 발췌 조회, 실제4AI 연속실행은 미검증. 공식 CLI구독인증·설정검사PASS이나 실제추론/구독차감은 승인후1회까지NOT_VERIFIED.

다음: CodexAgent 계획SHA d572d0ea8154abbf3af159b3c02c39c5d419d84404f7c24d541aab3a837e4988·4역할합계읽기전용1회·90초·Discord결과보고에 별도 승인 필요. 입력25~31KB/600자응답요청은 과금토큰 강제상한 아님. 사용량조회시5시간5%/주간72%잔여·유료크레딧없음, 리셋/구매0. 원본7356 SHA변경/누락0·status동일·게임/미술승인/유료API/Push/merge0·SFX PAUSED. 구현1d0ab0e6. [상세보고](팀운영/Discord_AI_Real_Connection_Readiness_20261023.md). Unity추가확인없음. 기존실패단발장부 보존·다음AI자동호출금지.
## 2026-10-23 Discord 실연결 후속 — 모의 1건 승인 대기

실제 Gateway·명령9등록·메시지 전송 PASS. 바울 대표가 status/pending/handoff 응답 확인, 자동 영수증은 status/handoff PASS·pending NOT_VERIFIED. 모의1건 리뉴얼→Codex→기획 APPROVAL_PENDING 완료, art USER_REVIEW_PENDING 유지. 로컬8점검+회귀1 PASS/FAIL0. 메인 아트는 Codex 라우팅 설정 확인만, 실제 업무/비소유자 Discord 거부 미검증. 실제 AI0·유료API0·SFX PAUSED·Push/merge0.

CLI 구독인증 가능 및 추론 없는 설정검사 PASS, 실제 추론 성공은 별도 승인 후 읽기전용1회까지 미검증. 원본7356 SHA변경/누락0·status동일. 구현040224ee. [보고](팀운영/Discord_AI_Collab_Live_Followup_20261023.md). 다음: pending 자동영수증/비소유자 실증, 메인아트 실업무는 별도 승인된 범위에서만. Unity 추가 확인 없음. 검증용 Run 프로세스는 자동 재시작/AI실행 없이 연결 유지 중이며 필요 시 로컬 Run으로 재실행합니다.
## 2026-10-23 Discord 협업 Phase2 — 설정 대기 / 실제 단발 실패 보존

기존de619848을 재사용해 숨김 토큰 설정·Discord 영수증·구독 단발 게이트를 보완했습니다. 변경분22 로컬/모의 PASS, 기존57·UnityQA 반복0. Discord는 WAITING_FOR_USER_SETUP. 실제CLI exec1은 reserved openai provider override로 bootstrap 실패/완료turn0, 자동 재호출0. 수정 준비안은 추론 없는 설정검사만PASS. 큐에 NOT_VERIFIED·미술승인대기로 보존했습니다.

원본Save/UserData 포함7356 SHA변경0·원본status/HEAD불변·제품0·SFX PAUSED·Push/main병합0. 구현1390086a. 다음: 대표 Portal/로컬설정 후 실제Discord 확인, 새추론1회는 별도승인 필요. [Phase2 보고](팀운영/Discord_AI_Collab_Phase2_Report_20261023.md).

## 2026-10-23 Discord 협업 로컬 파일럿 — LOCAL_ONLY

별도 codex/discord-ai-collaboration에서 SQLite 큐·역할8·최대동시2·SHA/범위 승인·예산/일요일·ZIP/PNG 검역·Discord slash9 모의 어댑터를 구현했습니다. 신규 로컬57 PASS/0 FAIL, 실제 별도 프로세스 재시작 중복0. 설계 ba82c396·구현094ea98d. [완료 보고](팀운영/Discord_AI_Collab_Local_Pilot_Report_20261023.md).

실제 Discord/AI/Unity 미연결·추가 AI 비용0, SFX PAUSED. 실제 AI runner·Unity 검수기·자동 게시·GitHub 쓰기·제품 적용은 미완료이며 별도 승인 단계입니다. 원본7343 SHA/858상태 불변, 관련 기존worktree4 clean, 기존 게임QA 재실행0·원본 적용/Push/merge0. 다음: 테스트 guild 실연결 승인과 제한된 공식 Codex 추론·권한/한도 실증. 이 도구 작업 때문에 Unity에서 추가 확인할 변경은 없습니다. 과거 EOD와 Main23/아트 별도 worktree 이력은 유지합니다.

## 2026-10-22 EOD 마감 — PR54 병합·원본 SYNC_COMPLETE 확인

[PR54](https://github.com/HappyPaul812/Project-Limitless/pull/54) merge `e2801286ccd26fe2060d9fee658ac272da4b4f2b`: 승인 Bear v4·Fox v7 공유 코드 양쪽 보존6파일 + 마을 최종승인9 Sprite/Tile 신규등록39파일 = 제품45파일, 담당 문서·근거152파일, 총197파일 통합. 신규 정적10/격리Unity15 PASS, 기존154/162/Wolf/Main22 전체QA 재실행0.

실제 GitHub main=origin/main=원본local main=e2801286, index=HEAD/staged0 확인. 백업912파일190,738,290bytes SHA불일치0, 보호7179파일 중 승인문서6개 외 변경0. Save/UserData/개인설정/Editor·미커밋 작업 보존. 이미 원본적용 Bear/Fox6파일 재작성0. dirty310→308/untracked554→550은 승인6경로 Git 편입이며 그 밖 상태 동일. 원본Editor/Play/포커스0, 자연Import·손플레이는 미검증.

이 문서는 PR54 완료 시점 스냅샷입니다. 문서 후속PR 이후 최종3자 SHA는 로컬 `Temp/EOD20261022/OriginalLocalSync_Proof.json` 및 Issue #53 완료기록을 정본으로 확인합니다. 마지막 관련 제품통합 commit e2801286. 추가제품 변경0.

OPEN16 미해결16 유지, Main23 전체·Main24 Save #46 HOLD. 미승인TTS READY104/152·Main21 KEEP WAV7·미완BGM/SFX 게임 적용0. 다음은 별도 승인 후 해당 범위만 진행합니다. 기존 승인기능 재구현·전체QA 반복 금지.

## HISTORY — EOD 실행 전 기록 보존

## 2026-10-22 EOD 승인분 통합 — 신규 delta PASS / PR·원본 동기화 대기

Issue #53: Bear Run6 v4 + Fox Run6 v7 공유 펫 정의를 모두 보존하고 승인 제품6파일을 선별했습니다. 기존154/162 전수QA 재실행0, 원본 이미 적용된 파일 재복사0. 마을 최종승인9 PNG는 별도 FrostwayVillageV10 Sprite9/Tile9 및 meta39파일로 등록, 기존49/Scene/Sorting/통행 변경0. 제품 총45파일. 격리 Unity15 PASS/0FAIL, 신규 정적10 PASS. 기존 Wolf/Main22/F1/서리들쥐 재검수0.

담당 문서는 오늘 변경분만 파일별 병합·고유 HISTORY 보존. TTS READY104/152는 청취승인이 아니며 Main21 KEEP WAV7 및 미완 BGM/SFX 제품 적용0. OPEN16 미해결16, Main23 전체·Main24 Save #46 HOLD.

원본912파일190,738,290bytes SHA백업, 총7179 SHA 보호. 원본Editor/Play/포커스0. 브랜치 codex/eod-integration-20261022. 마지막 승인 제품 commit cad73927(선행 Bear 9cf0e569). 최종 PR/merge/동기화 증거는 후속 마감 기록으로 확정합니다.

## HISTORY — 실행 전 기존 기록

## 2026-10-21 EOD 완료 — PR48 병합 / 원본 SYNC_COMPLETE

[PR48](https://github.com/HappyPaul812/Project-Limitless/pull/48) 승인 Wolf6+Main22/F1/Mouse52 제품58 및 담당 문서 선별통합 **`c2750d1faea7ef9d54df1b4fcd7d6efb2bd49ac8`**. 이 완료증거 작성시 GitHub main=origin/main=원본local main, index=HEAD/staged0 실측 **SYNC_COMPLETE**. 후속문서 PR 이후 최종SHA는 `Temp/EOD20261021/OriginalLocalSync_Proof.json` 및 Issue47 완료기록을 따릅니다.

기존 Wolf106/Main22 136/F1 64/Mouse40 재사용·전수재실행0. 신규 정적38/격리Unity14 PASS. Main22는 PROTOTYPE 대사/수치·보상·경제 미확정. Village v5 타일3 ART_HOLD/OPEN16 해결0·Main23/24미구현, 청취미승인TTS READY64/152·미완BGM/SFX/영상은 제품제외 유지.

957파일196,911,769bytes 백업·6758SHA 보호. 승인범위밖 SHA/사용자상태차이0, 이미승인50파일 무재작성. BuildSettings 개인config·원본문서고유이력 보존. dirty314→308/untracked595→550/staged0, 승인경로 편입에 따른 변화. 원본Editor/Play/Refresh/포커스0, 자연Import·손플레이 미검증·URP/Search OPEN. reset/clean/stash/force/main직접Push0.

내일: 대표 평소 자연Import·기존 Main21완료 캐릭터 북문→Main22 시작 확인, 마을타일 수용/OPEN16 처리범위·정식대사/TTS·보상 설계·Main24Save #46 별도 결정. 완료 기능 재구현/전체QA 반복 금지. [완료 보고](EOD_ApprovedIntegration_LocalSync_20261021.md).

## HISTORY — 아래는 실제 EOD 실행 전 기록

## 2026-10-21 EOD 승인분 선별 통합 — 신규 최소검증 완료 / PR·원본동기화 진행 중

Wolf Run6 v5 승인6파일 + Main22/F1/서리들쥐52파일 선별, 제품58파일. Main22 대사·TTS·전투수치·경제·보상은 PROTOTYPE/DESIGN_NOT_LOCKED. 기존 Wolf106/Main22 136/F1 64/Mouse40 각 QA 재사용·전수재실행0. 새 정적38+격리 Unity14 검증 PASS, Scene/Quest/리소스·Save 선택필드 연결 확인.

Village v5 타일3 미술HOLD·OPEN16 해결0·본편미구현, 청취 미승인TTS READY64/152·미완BGM/SFX/영상 제품제외. 담당 문서는 파일별 선별·의미병합/고유HISTORY 보존. raw 진단로그는 개인경로/라이선스정보 비공개로 로컬보관, 공개본에는 판정마커만 유지.

새 실측 dirty314/untracked595/staged0, 사용자957파일196,911,769bytes 백업·보호6758SHA. 원본Editor/Play/Refresh/포커스 조작0. branch `codex/eod-integration-20261021`, 제품 commit `b53344ce`. 아직 PR병합·원본SYNC 완료 선언 아님. [EOD 보고](EOD_ApprovedIntegration_LocalSync_20261021.md).

## 2026-10-21 서리들쥐 Fix v2 최종 미술 승인·정식 외형 연결 완료

- 바울 대표 최종 승인 반영. 승인 Idle 원본바이트+Fixv2 Attack/Hit/Defeat PNG4·16프레임을 `Resources/MonsterVisuals/FrostFieldMouse` 외형 구성에 연결, PPU512/Pivot하단/8FPS/scale1. ID·수치·보상은 TBD이므로 표준 몬스터목록·새 Field/Quest/조우 추가0.
- 실제 격리 Battle 기본공격·HP감소/Hit·Idle복귀·Defeat03유지·기존두종회귀 **40PASS/0FAIL**. Hit직후 Attack이 가려지는 기존 플래그 문제 최소 보완. 기존36건 전수반복0. [상세·GameView·동작](Chapter3_FrostFieldMouse_Fix_v2_Official_Apply_QA_20261021.md).
- 제품13경로 원본 디스크·대상6경로 Editor Import 완료, 각모션4프레임 로드·컴파일종료·Console조회오류0 확인. 원본4,339SHA 중 승인코드1외 변경0/신규12만. Save/UserData·기존아트·Settings 보호, Bootstrap/PlayOFF/비포커스 유지.
- 브랜치 `codex/chapter3-mouse-combat-v2-apply-20261021`, 제품 commit `2201da328e481b83f397860ea22e584b88f1da11`. 원본main HEAD e4e9891f 유지·main직접commit/Push/PR/병합 미실행. 다음: 공식ID/밸런스/Main22 조우 별도 확정 후 이 외형 재사용. 동일 미술 재승인 불필요.

## 2026-10-20 EOD 완료 — PR41 병합 / 원본 SYNC_COMPLETE

[PR #41](https://github.com/HappyPaul812/Project-Limitless/pull/41) 승인 통합 main **`db242b77377b7c2ec08664f102b1a6bab0b3ffb3`**, 통합 head `ccad0344dacf647bd2c3530d2284783e24e7f784`. 완료 증거 작성 시 **GitHub main=origin/main=원본 local main**, index=HEAD 및 stage0 직접 확인. 후속 완료문서 PR 이후의 최종 SHA는 원본 `Temp/EOD20261020/OriginalLocalSync_Proof.json`에서 재확인합니다.

Field09 승인5종+B파생Glow 제품14파일 정식 통합·원본 디스크 동기화. Scene Sprite참조6개 외 차이0, 공식Glow PNG/meta/GUID·Transform/Sorting/Collider·Main18/Save코드 보존. Chapter3 승인1포즈·Idle PNG4는 Unity밖 승인참조로만 보존, 게임연결0. 담당 문서 파일별 선별·의미병합/기존 main 이력 유지. EOD최소 정적13+격리Unity11=24PASS0FAIL, 기존39/14/34/과열UI10 재사용·전체회귀0, 원격CI등록0·충돌0.

904파일188,454,129bytes 검증백업, 보호기준6,558SHA 중 승인된기존파일8개갱신 외 비인가변경0·사용자904SHA불일치0. tracked dirty308/untracked548/staged0 및 status바이트 그대로. Save/UserData/개인Settings·원본Editor Bootstrap/PlayOFF/비포커스 보호. reset-hard/clean/stash/force/main직접Push0. 원본 자연Import·손플레이 NOT_VERIFIED, Search/URP OPEN.

**내일:** 서리들쥐 전투Fixv2(518d210a)는 별도보관·QA/대표미술승인대기, 변경셀/Idle연결만 검수. 미승인F1/마을환경·BGM미완료루프·TTS/SFX/영상·Field09실패자산 제품제외 유지. BGM루프·권리확인, 잔여Field09/Chapter3정식구현은 별도승인.

[완료 보고](EOD_ApprovedIntegration_LocalSync_20261020.md) · [동기화 증거](Evidence/EOD20261020/OriginalLocalSync_Proof.json) · [보호 증거](Evidence/EOD20261020/UserOriginalProtectionProof.json)

## HISTORY — 아래는 EOD 실행 전 준비 기록

## 2026-10-20 EOD 승인분 통합 — 최소 QA 완료 / PR·원본 동기화 진행 중

Field09 승인 환경5종+B파생Glow 제품14파일만 선별: PNG6 신규/meta/폴더meta +Scene Sprite참조6개. 공식Glow PNG/meta/GUID 및 Transform/Sorting/Collider/Quest/Save코드 불변. 기존39PASS·Idle14PASS 재사용, EOD 정적13+격리Unity11=24PASS0FAIL. Chapter3 승인1포즈·Idle4PNG는 Unity밖 `아트/몬스터/Chapter3/ApprovedReference`에 참조보존·게임연결0. 담당 문서는 파일별 선별/의미병합, 기존 main 고유 이력 보존.

사용자 변경904파일188,454,129bytes 백업/보호기준6,558SHA 기록. Combat Fix v2 ZIP518d210a는 CRC/SHA 확인 후 별도Temp아카이브, QA/미술승인대기. F1/마을미승인환경·BGM미완료루프·TTS/SFX/영상·Field09실패자산 제품제외. 내일 우선: 전투Fixv2 변경셀·Idle전환 검수→대표 미술승인; BGM루프/권리확인. 원본Editor 조작0, 자연Import/손플레이 미검증. 브랜치 `codex/eod-integration-20261020`, 기준main12c69c72. 아직 원격병합/원본Sync 완료 선언 아님.

[EOD 실행 보고](EOD_ApprovedIntegration_LocalSync_20261020.md) · [선별/검증 근거](Evidence/EOD20261020/IntegrationDeltaQA.json)

## 2026-10-19 EOD 승인 통합 완료 / 원본 SYNC_COMPLETE

승인 기능 [PR #27](https://github.com/HappyPaul812/Project-Limitless/pull/27) main 병합 **`f7a82907221e322a29b0b4cab24f7a4b4a0d197a`**, 통합 head `57f438ab3c72fabf91c127ff52398b3b9e17997d`. 이 완료 증거 작성 시 GitHub main=origin/main=원본 local main, index=HEAD 실제 확인 **SYNC_COMPLETE**. 후속 문서 전용 PR 병합의 최종 SHA는 원본 `Temp/EOD20261019/OriginalLocalSync_Proof.json`을 최종 재검증하며, 아래 SHA는 기능 통합 확정점입니다.

Main09 승인27·재회순서/Main10 승인18·조사NPC 우선권/Field06 v1.1 B42·Ground49·안전Spawn/Field07 v1 B42·투명Ash7 공식 통합. 게임168파일 원본 승인본과 동일하여 재기록0, 기존 Main18·Field05·Main09 UX 중복0. 정적8+실제Unity10=18PASS0FAIL, 기존 대규모 QA 반복0. 기획/리뉴얼/TTS 문서75파일 선택·의미 병합 및 충돌문서8개의 main/담당 고유 문단 보존 검사 PASS.

1,059파일 SHA백업(203,125,963bytes)·39worktree 기록·보호4,339SHA/백업1,059SHA 불일치0. 추가 ZIP4·고유커밋2 patch 아카이브 보존. 최종 사용자 tracked dirty308/untracked548/staged0 유지. UserData·Save·Settings 보호, 원본Editor/Play/Refresh/포커스 조작0. 기존 로컬 문서2커밋 main 조상 유지.

Field09 시각FAIL PNG/Scene/ZIP, 미승인 독사·늑대/TTS, Chapter3 구현은 제외. 원본 Editor 자연Import·손플레이/Main15·16 전주행 NOT_VERIFIED, Search/URP OPEN. 다음: 대표 평소 자연Import·승인 화면 확인 → Field09 수정 납품/미승인 TTS 청취 승인 대기 → Main22 기획·ID·아트 게이트. 완료 기능 재구현/전체 QA 반복 금지. [마감 보고](EOD_Integration_LocalSync_Closeout_20261019.md)

## HISTORY — 이전 완료·승인 단계 기록

## 2026-10-19 로컬 main 문서5 충돌 복구 완료 및 다음 단계

문서5 의미 병합 PR #25 기능 `bc0ffac095746a499a31792aea2bef0a7e39691b` → main `30954827ebbe9d2f2fd4e6bca22abd2f50e6acc7` 완료. 격리 리허설 후 **원본 HEAD/index/작업파일 SYNC_COMPLETE** 확인. dirty·미추적·UserData·Settings 1,430파일 백업, 문서5개 의미 병합 외1,425파일 SHA 불일치0. 기존 문서 커밋2개 이력 유지. 남은 tracked dirty308/untracked548은 사용자 변경으로 보존하며 staged0입니다. 최신 main은 후속 보고 PR 이후 최종 검증 SHA를 따릅니다.

Main09/Field05/Main18 재구현·대규모 재QA0, 원본 Editor/Play/Refresh/포커스0. 자연 Import와 실제 Main09/Field05 화면은 바울 대표의 평소 사용 시 확인 사항입니다. Search/URP 및 로컬 Unity 버전 차이 기존 OPEN 유지.

Main20 세린5/Main21 17 총22음성 Unity미등록. 기존 로컬 후보17개 PCM/SHA/원문 PASS이나 음색 오류 보고9개·나머지8개 청취 승인 미확인. 후보 없는5개(태온1/세린1/Voice미정 하렌3) 별도. API/생성/Unity음성변경0. [정확한22개 원문 CSV](Main20_Main21_TTS_Missing_ReadOnly_20261019.csv)와 [복구·보호·다음 실행 보고](LocalMain_Doc5_Recovery_Closeout_20261019.md) 참고. Main22는 기획 승인·정식ID·북부 아트가 아직 필요하며 기존 북문 동선/Save 연결 읽기 감사부터 진행합니다.

## 2026-10-19 문서5 의미 병합 및 보호 경계

로컬 작업복사·local main 커밋·공통 조상·최신 원격 main을 대조했습니다. 최신 Main09(PR21/24)·Field05(PR22/23)·Main18(PR18/19)의 병합·원본 디스크 반영은 완료 상태로 유지합니다. 과거 기록의 승인 대기/미구현은 과거 당시로 구분합니다. 기획·리뉴얼 담당의 원격 전용 기록도 삭제하지 않았습니다.

충돌 원본 네 버전과 SHA256·줄바꿈·시각은 로컬 `Temp/Doc5Recovery20261019/Versions`, 전체 dirty·미추적·UserData·Settings 백업은 `Backup`/`BackupManifest.json`에 보존했습니다. 문서 정합과 원본 Git 동기화는 별개이며, HEAD/index/작업파일 동기화는 격리 리허설 후 실제 확인 결과로만 선언합니다. 원본 Editor 자연 Import·손플레이는 미검증이며 기존 Search/URP 환경 OPEN을 유지합니다.

## 2026-10-19 Main09 최종 승인·main 병합·원본 디스크 적용 완료

- 바울 대표의 명시적 승인 후 [PR21](https://github.com/HappyPaul812/Project-Limitless/pull/21) **main병합 `f3196b6b4fc4dfaf8a901a6a29ea07254ae70325`**, 승인PR head `ed8efda34902f374a92f78e98cb5dd6d5af1b8fd`, 기능 `932c3573a0ad4a4d94c0b92c4d0bb932662ae419`. 기존427PASS/0FAIL을 재사용했습니다.
- 원본 Main09 기능9파일(기존7+신규helper/meta2) 적용 완료. 적용 직전 기존파일과 기준main 일치/충돌0 확인·백업, 적용후 검증브랜치 SHA9/9일치. 기존Quest ID/대사/폴합류/보상/위치/Collider/Save/승인음성 보호. [원본적용증거](Evidence/Main09Clarity20261019/OfficialApplyResult.json).
- 원본Editor/Play/포커스 조작0. 원본 Editor 자연Import·Runtime은 미검증, 기존 Search/URP·Editor버전차이·물리OS입력은 환경PARTIAL. 전체6단계·입력·SaveContinue의 실제 격리Unity427PASS와 구분합니다.
- 승인Field05 B42도 원본디스크 적용·PR22 main병합 b9590938 완료 상태 유지. Main18 PR18/19 재작업0. UserData23/Main08·Main18 WAV/meta/Catalog 변경0, 승인범위 외 원본 변경0.
- 원본 local main은 ff022e63 유지. 문서5개 내용충돌로 LOCAL_SYNC_PARTIAL이며 reset/clean/stash/forcePush 없이 사용자편집과 기존문서2커밋을 보존했습니다. 다음: 원본 Editor가 자연 Import한 후 정상 플레이 확인, 사용자문서5개 의미병합 후 로컬main동기화. Main09 승인 대기는 해제됐습니다.

## 2026-10-19 Main09 퀘스트 안내 수정 — 427 PASS / 원본 미적용

- `codex/main09-quest-clarity-20261019`, 기능 `932c3573a0ad4a4d94c0b92c4d0bb932662ae419`. 6단계 행동/조작·바닥 링·화살표·플레이어XY거리·폴 재대화·로그/토스트 동기화. 여관NPC와 조사 입력 충돌을 현재 Main09/1.6반경 한정 우선처리로 해결했습니다.
- 실제 키보드도보/게임패드휠체어 전체6단계348 + 최종UI/추적/전투Scene회귀79 PASS/0FAIL. 16:9/4:3/21:9 화면, 폴합류/보상/SaveContinue PASS. 대사·Scene·WAV·Collider 불변, 원본4844SHA/UserData23보호.
- 기존Search/URP·Editor버전·물리OS입력은환경PARTIAL. [실제 QA/화면](Main09_Quest_Clarity_Unity_QA_20261019.md). main병합·원본적용0, 전용PR검토대기. 다음: 승인된Field05 B42는 기존결과 재사용통합(추가미술승인 불필요), local main문서충돌5개보존.

## 2026-10-19 재개 작업 완료 — Main09 PR21 / 승인 Field05 PR22 병합·원본 적용

- 우선순위1 Main09: 실제 도보/키보드·휠체어/게임패드 6단계348 + 최종UI/추적/Scene회귀79 = **427PASS/0FAIL**. 기능 `932c3573a0ad4a4d94c0b92c4d0bb932662ae419`, QA commit `27983bb3d5652dfd6fec0b837ad4a63e98fc5ed5`. [PR21](https://github.com/HappyPaul812/Project-Limitless/pull/21) Push/검토대기. **Main09 main병합·원본디스크적용0**, UX개선화면 검토후 다음 반영 대상. 기존여관과조사입력충돌도 현재Main09/1.6반경한정 해결. 대사/좌표/Collider/승인음성 보존.
- Field05: 대표 최종승인 범위 PNG14/Road42/환경375 정식 반영. [PR22](https://github.com/HappyPaul812/Project-Limitless/pull/22) **main병합 `b95909387a7b6f597417041bfcd72f2b866d4e47`**, PR head `81b1c82df4fb0203d2958fc90804fc4956fb3cc5`. 원본디스크Scene1+신규32경로 일치/SceneGUID보호. 기존416+84검증재사용+실제Continue/Quest/Meta66PASS/0FAIL. 새Field05플레이환경/빌드/화면/텔레포트테스트0. [정식QA](Field05_B42_Official_Apply_QA_20261019.md).
- 원본4844SHA 중 승인Field05Scene1만변경, UserData23/Main08·Main18음성/Arbel/Field04/기존meta보호. Main18 PR18/19 재작업0. 원본Editor/Play/포커스조작0; 원본 자연Import/Runtime·물리OS입력은미검증, 기존Search/URP·Editor버전은환경PARTIAL.
- local main `ff022e63dfe364f559fcc29819d27b123ad31127` 보존. tracked dirty320/미추적1053, 원격겹침517중512동등/문서5내용충돌로 **LOCAL_SYNC_PARTIAL**. 기존로컬문서2커밋 ff022e63/3a37aa2e의 main조상이력 보존확인. reset/clean/stash/forcePush0. [최종보호](Evidence/Field05Official20261019/FinalLocalProtection.json).
- 다음 재개: Main09 PR21을 최신 main과 충돌없는 상태로 유지하고 대표 UX확인 후 main·원본 반영. 사용자 원본문서5개는 양쪽내용 의미병합 검토 후 로컬main동기화. 기존검증/아트/음성을 재제작하거나 수천건 QA를 반복하지 않습니다.

## 2026-10-19 Field05 B42 원본 디스크 정식 적용 — 66 추가 PASS / Push·PR 준비

- 바울 대표 최종 미술 승인에 따라 기존PNG14/환경375/Road42 후보를 원본 Scene+신규32경로로 반영했습니다. 기존Collider21·비도로Transform·SceneGUID·UserData23·승인음성 보호. 원본Editor조작0/자연ImportRuntime미검증.
- 기존416+84 PASS 재사용 + 실제SaveContinue/Scene/Quest/Meta 연결66 PASS/0FAIL. 새화면/별도플레이환경/빌드/텔레포트테스트0. [정식적용QA](Field05_B42_Official_Apply_QA_20261019.md).
- Main09 PR21은 실제427PASS/0FAIL로 Push·검토대기, main병합/원본적용0. 다음: 승인된Field05전용PR병합 후 정확SHA인계, 원본문서5충돌은보존해 local sync PARTIAL 유지.

## 2026-10-19 Field05 B42 최종 미술 승인 — 정식 통합 준비

- 바울 대표의 최신 재개 지시로 최종 미술 승인 완료. 기존 기능 ebb363567b3932cce8af12e315dda03362c8fd19/최종후보 f8afb80ce744fd1eef78ce1c5c7eb1d07187ebee와 416+84 PASS를 재사용합니다. 추가 비교 화면/플레이 검수 빌드/강제 Field05 입장 도구는 만들지 않습니다.
- 원본 Scene은 HEAD 대비 미커밋 변경 없음, 기존 Collider21개와 비도로 Transform 동일, 새 Asset32경로 충돌0을 확인했습니다. 승인PNG14/도로42/환경375만 정식 적용 대상으로 준비합니다.
- Main09는 먼저 완료: 기능932c3573a0ad4a4d94c0b92c4d0bb932662ae419, 최종27983bb3d5652dfd6fec0b837ad4a63e98fc5ed5, PR21 Push/검토 대기, 실제427 PASS/0FAIL. Main09 main병합/원본적용0. Main18 재작업0.
- 다음: Field05 Meta/GUID/Quest/Save 변경분 회귀→원본 디스크 안전적용→전용 Push/PR/승인범위 main 병합. 사용자dirty문서5개와Save/Editor/포커스보호 유지.

## 2026-10-19 Field05 v2 Road4 / B42 최종 후보 — 기술 PASS / 미술 승인 대기

- 전용 `codex/field05-eastwest-road42-v2-20261019`, 기능 `ebb363567b3932cce8af12e315dda03362c8fd19`. 새 v2 SHA/CRC/PNG4 Manifest·Ground3 Props7 원본SHA PASS. 환경375=315/42/18, PNG14, Road12추가, 선택7/새Portal0.
- 실제 최종 Unity 기능416 PASS/0 FAIL, 저장Scene재로드84 PASS/0 FAIL, 16:9·4:3·21:9 3안 비교63장. 기존 Search/URP/Editor버전차이만 환경 PARTIAL. [검수자료](Field05_Road42_v2_Final_Unity_QA_20261019.md).
- **원본 Field05 정식 적용·Push·PR·main 병합0. 새 v2 화면 미술 승인 필요.** 기존 Collider/좌표/Spawn/Save/Arbel·Field04 보호. 다음: 대표 갤러리 승인 후 통합.

## 2026-10-19 통합 마감 — Main18 원격 병합·원본 디스크 적용 / Field05 최종 미술 승인 대기

- Main18 승인10 Push→PR18→main 병합 `6f988dad77316347be69ac3b0979237771c09ed9`, 기능 `94a68cee54be397b16d8bacd4c3f930bad2eece3`. 기존1370 PASS/0FAIL·22/22 재사용 + 실제재생/SaveContinue delta394 PASS/0FAIL. 정상25/Main17세린11/PCM35/기존Catalog280 보호. 원본Unity WAV10/meta10/Catalog만 정식반영, 원본Editor미조작/Import Runtime미검증.
- Field05 새 v2 Road4 B42, PNG14·환경375·기능416+저장Scene84 PASS/0FAIL. 세화면비63장 비교, 최종미술승인PENDING. `codex/field05-eastwest-road42-v2-20261019` 최종 `92746215df23e2fe57e3393a563c839dca66a29a`. Field05 원본적용/Push/PR/main병합0.
- 원본dirty319+미추적1001,1368고유파일백업·UserData23 보호. 겹침464/문서충돌5로 local main ff022e63 보존·동기화PARTIAL. Search/URP/Editor버전차이·기존meta중복은환경OPEN. [실행결과·재개점](Main18_Field05_Integrated_Closeout_QA_20261019.md).
- 다음: 대표 Field05 최종화면 승인, 원본문서5개 의미병합 후 로컬main동기화. 주간재설정권 사용은 실행하지 않았습니다.

## 2026-10-19 Main18 승인 세린10 worktree 통합 — Runtime PASS / 환경 PARTIAL

- 이슈17·docs/tts 승인에 따라 ground샘플1+DSP PREVIEW6+수동복구3을 소스bytes 그대로 새 WAV/meta로 추가했습니다. 불합격remaining9/옛음색사용0·추가가공/TTS API0. Main18 Manifest는10개 Status만 RUNTIME_VERIFIED로 갱신했습니다.
- Catalog280→290/기존280항목 불변. Main18 NPC22/22 실제 페이지·자연종료·Next/빠른Next·Player/지문무음·Mixer/Mute·Scene/SaveContinue PASS, 정상25회귀·Main17세린11(청각분기포함)·PCM35최대오차0 PASS. 최종1370assertion PASS/FAIL0.
- 전체QA_PARTIAL: 기존Search예외, 정본6000.5.7f1/OS손입력·물리스피커미검증. 실제6000.6.5f1 격리Batch. 원본/체크포인트4169SHA변경0·기존WAV/meta/GUID·Save보호·원본Editor/포커스조작0.
- 브랜치 `codex/tts-main18-serin10-integration-20261019`, 마지막 기능 commit `94a68cee54be397b16d8bacd4c3f930bad2eece3`. 원본 main/Unity미적용·Push/main병합0. [최종QA](Main18_Serin10_Final_Unity_Integration_QA_20261019.md)·[10개소스매핑](Main18_Serin10_Final_Source_Matrix.csv).
- 다음: 바울 대표 통합결과 검토 후 별도 승인으로 Push/main 반영. 승인10 재청취·재생성 불필요.

## 2026-10-14 최종 마감 — 음성 PR14 병합 완료 / 로컬 main 보존 중단

- 바울대표전후보청취승인·오늘병합지시로 007~011 정식연결 완료. 음성구현 `2ff11baa878156f9b7c537d9580e70cd1f5568e4`, [PR14](https://github.com/HappyPaul812/Project-Limitless/pull/14) merge `d05a325b05f2eab79ee18c5ca65e9ee523a5e5ab`. PR12Arbel/PR13기획 중복적용0·TTSAPI0·C3DRAFT구현0.
- 최신통합본 음성250+Arbel1729+Main21펫106+기존전투/Field04회귀232+Scene26=2343PASS/FAIL0. 승인8/미엘/Player무음·Quest/Save보호, 본문/호흡PCM동일·clipping0. 최종WAV007 3.267208/008 4.495667/009 3.017417/010 4.130042/011 2.550625초. 원본디스크 Catalog5GUID만변경+새11파일·기존WAV/meta불변.
- 전체PARTIAL: 기존URP/Search환경OPEN·원본liveImport/실물입력/스피커미검증·local同期STOP. 원격/추적main은음성병합시점 `d05a325b05f2eab79ee18c5ca65e9ee523a5e5ab`, localmain `ff022e63dfe364f559fcc29819d27b123ad31127`. 로컬전용문서2커밋으로ff-only불가를격리복제에서확인, 강제이동/reset/clean/stash/forcePush0. SYNC_COMPLETE아님. 미커밋1274경로/5851SHA와Save백업보존.
- 원본EditorBootstrap·PlayOFF·PausedOFF·Focusedfalse·Compilingfalse전후유지,강제Refresh0. [최종QA](EndOfDay_Integration_Voice_LocalSync_QA_20261014.md). 다음:2로컬문서커밋+기존dirty의보존/통합을별도로검토후로컬동기화재개. 음성재승인·재제작요청불필요.

## 2026-10-14 마감 음성007~011 정식 연결·통합 QA

- 바울대표전후보청취승인에따라5WAV복구/앞저음량여백최소축소·Catalog5GUID교정. 신규TTS0·승인005006+기존6/미엘/Player/QuestSave보호. 최종007 3.267208·008 4.495667·009 3.017417·010 4.130042·011 2.550625초.
- 최신main b5174f2f(PR12Arbel/PR13기획)기준 음성250+Arbel1729+Main21펫106+전투회귀232+Scene26=2343PASS/FAIL0. 기존환경/원본liveImport미검증은PARTIAL. Arbel중복적용0·C3DRAFT구현0.
- 음성전용codex/main08-taeon007011-final-20261014, 구현2ff11baa878156f9b7c537d9580e70cd1f5568e4. [마감QA](EndOfDay_Integration_Voice_LocalSync_QA_20261014.md). QA통과후이브랜치PR병합예정이며실제병합SHA는최종인계에추가기록합니다.
- localmainff022e63 대 originmain 이력분기(로컬문서2커밋). 격리ff-only실패로원본main동기화STOP/PARTIAL,강제이동0·1274dirty백업·Save보호. 승인Catalog1만디스크수정+새11파일·원본HEAD/Editor제어0. 다음:로컬2커밋/dirty비파괴정리후동기화재개.

## 2026-10-14 아르벨 v3 승인 아트 정식 디스크 적용

- 아트23종·기존환경Renderer284·SortingGroup·발기준건물주변깊이·국소지붕투명/처마접합 적용, 선택장식0/PNG신작0. 세린·양쪽문·접합/실제서비스·도보휠체어·SaveContinue·Main12/13전투·Field04회귀 필수PASS.
- main1765+228, 원본5파일1706+228, BuildScene26 =3953PASS/0FAIL. 전체PARTIAL: 원본대조재현URP/Search·정본Editor버전/OS입력·원본Editor실시간Import미검증. Portal↔나무는 별도OPEN(관리인실제접근PASS, 텔레포트미구현).
- 원본4000파일동일/허용Scene1만변경+신규55, Sprite/WAV/meta/Save/Settings/Field04/Chapter3보호. 원본Bootstrap/PlayOFF/비포커스, 강제Refresh0/main병합0.
- 다음: 정상Import 후 원본화면확인, Portal OPEN 별도기획. 자세한QA/인계는 Arbel_v3_Official_Apply_QA_20261014.md. 마지막 관련Feature commit 648b1603c7d79803fdfa54fd0c2222e3ef8cc201; 전용브랜치 codex/arbel-v3-official-apply-20261014.

## 2026-10-14 Main08 태온005·006 최종 승인 음성 연결

- 바울 대표의 두 후보 청취 승인에 따라 새 ApprovedTaeon00500620261014 WAV2/meta3 등록, Catalog005/006 GUID2행만 교정. 기존 승인6·미엘004·Player무음·기존 원본 WAV/meta 모두 보존.
- 005 시작 배경 여백185.833ms(4460프레임)만 제거, 남은 PCM 원본 suffix100% 동일·zero/zero 절단·clipping0. 최종6.543625초. 약한 활동 시작 약0.14초, 강한 발화 신호 약2.33초(호흡/문장 내부 여백 포함); 숨소리 후보를 삭제해 더 짧게 만들지 않음. 006 승인본3.800542초/WAV전체동일.
- 격리Unity6000.6.5f1 224PASS/FAIL0: 승인8 PCM/GUID·Trace01/02/03→Player무음→WheelTracks·Next/취소/자연끝·Quest·실제BootstrapContinue. Source시계 시작005약13ms/006약40ms. 원본 Editor Bootstrap·PlayOFF·비포커스 유지, 강제Refresh0.
- 전체PARTIAL: 기존Search인덱스예외·물리스피커/OS손입력·main정본6000.5.7f1 미검증, 대화중일반조우는기존OPEN(이번억제). 다음:바울 대표 원본 작업 재개 후 디스크 변경을 정상 Import하여 청취·페이지 경계 확인. 이번 아트/Quest/게임플레이 변경0.
- 브랜치codex/main08-taeon005006-final-20261014. 마지막 기능Commit `fe588f9af1369ec825a9b9820ebfb0a92dc9499a`. 상세QA: [Main08_Taeon005006_Final_Integration_QA_20261014.md](Main08_Taeon005006_Final_Integration_QA_20261014.md). 승인된 작업브랜치만Push, main직접Commit/Push·병합0.

## 2026-10-14 Sprite 참조 복구 — 필수 게이트 PASS

- 새 codex/sprite-reference-recovery-20261014: 동일PNG11·Sprite123 등록, Arbel/Field05~08의1747 Sprite GUID만 복구. Scene 역치환 동일/ThirdParty·원본미커밋·5221SHA/HEAD/status 보호.
- 실제23 BuildScene 참조3654/MissingScript0/Runtime null0. Edit null2는 VillageNpc 자동할당 정상. 배경5 RenderTexture 확인, 로컬 rect/pivot/PPU123 PASS.
- 이전854범위 새855 PASS(waypoint 추가계측1)/FAIL0, 서부85·의뢰589·Quest 기반/침묵 ALL PASS, 기존정적83·확대322 PASS. Field04 42/375/PNG52·Main08 승인6·B/C/15효과/Save/Chapter3 불변.
- 종합PARTIAL: 기존Search·정본6000.5.7f1·손플레이/가독성 미검증과 기존Spawn/북쪽틈/음성005~006/장착영속 미구현 유지. 원본Editor·포커스/Save/TTS/BGM 조작0.
- PR10 Ready/정상 main merge 조건 충족. 실제 원격 Push/통합HEAD/mainSHA·병합 여부는 PR10 최신 결과와 완료 보고를 확인하며 이전BLOCKED 기록은 역사로 보존합니다. 다음: 남은 기존 결함을 별도 작업으로 처리.
- 마지막 관련 복구 commit `af1c9cfddbf8e431a32d78d5a643d91a7a099da0` Push 완료. EOD merge `359b0622376c24d78caa025ec33df5fef71ccaa7`·충돌0/동일Unity트리/통합23참조·Runtime·정적83 재PASS. 최종 통합 문서 commit·원격 병합 SHA는 PR10 최신 결과 참조. [상세 QA](Sprite_Reference_Recovery_Final_QA_20261014.md).

## 2026-10-13 Main08 승인6개 정식 음성 연결·원본 디스크 반영

## 2026-10-13 EOD 최종 합본 — BLOCKED / MAIN 미병합

PR8/9충돌해결·Field04공식42/375/PNG52·Main08승인6·B/C·최신기획·담당문서 통합. 새합본 주요기능854PASS/FAIL0, 별도전체Scene참조FAIL: 기존main에서도Arbel/Field05~08환경Sprite누락 재현, 실제Arbel배경누락 확인. Field04362→0, 신규참조회귀증거0이나 전체Sprite0게이트미충족으로PR10Draft유지/main병합중단. 원본5199파일SHA/HEAD/dirty불변. 상세 `EOD_Integration_Final_QA_20261013.md`. 다음: 동일바이트Project등록·SpriteImporter복구안 검토 후 전체참조QA 재실행. 스킬영속Save·Chapter3상점·Main18추가TTS 미구현/HOLD보호. 최종브랜치SHA는PR10head·완료보고 참조.


- codex/main08-approved-voice-apply-20261013:바울대표직접청취승인6WAV를Main08/ApprovedRecovery20261013에새GUID등록/Catalog6참조교정. 원음WAV/GUID/meta그대로·목적지/출처16백업·승인입력6보관. 원본프로젝트디스크에도새13파일+Catalog반영완료,원본mainHEAD/기존미커밋/Save/설정/Scene/BGM/TTS보호.
- 최종등록Runtime177PASS/FAIL0,승인ClipPCM/GUID/경로6/6·Trace01~03/초상화/Next/Mixer/Quest/실제BootstrapContinuePASS. 정본6000.5.7f1미검증/실제6000.6.5f1·기존Search로PARTIAL,조우억제대화fixture·실물입력미검증. 원본Editor강제Refresh/Play/포커스전환0,즉시메모리반영미검증.
- 승인밖태온005/006기존발화·합본문제2건과대화중일반조우사례OPEN;전체Main08음성내용PASS아님. 새TTS/API·Push·mainCommit/병합0. Main08_ApprovedVoice_Integration_QA_20261013.md 참조. 마지막Commit은 `Fix: 승인 Main08 음성을 원음 보존 등록으로 연결` 이력확인.

## 2026-10-13 Main08 기존 음성 복구 후보6개 납품 — 청취 승인 대기

- codex/main08-voice-recovery-candidates-20261013: 태온002←003/003←004·미엘001←002 재사용, 태온004 긴정본은기존005에서발견해복사. 미엘합본003의450ms저신호간격에서0샘플분리해002/003 후보작성. 후보6decode/정본ASR일치·복사4byte동일·분리2PCM합산원본동일 PASS.
- 후보폴더/6재생기HTML/ID별CSV·길이RMS/Peak/원본SHA/경계보고제공. 사용자청취6대기·재제작현재확정0, 부적합ID만후속분류. 원본WAV/GUID/meta/Catalog/Unity/Save/설정·TTS/API/화면/Push변경0. 새RuntimeQA실행0.
- Main08_ExistingVoice_Recovery_Candidates_20261013.md 참조. 마지막관련Commit은 `Docs: Main08 기존 WAV 복구와 청취 후보 납품` 이력확인. 승인전정식적용금지.

## 2026-10-13 Main08 Field02 음성 오류 — 원본 내용·분할 문제 후보 확인

- codex/main08-field02-voice-20261013: docs/planning 사용자QA와 로컬 제작/WAV/Catalog 대조. ID/GUID·화자·자막/Manifest와 원본PCM 동일, 태온002 1.03초/RMS-48.98dBFS·발화검출없음, 미엘001 Main05문장/002 앞문장/003 정본문장+앞문장 ASR검출. 원인층은제작원본 내용↔ID/분할경계, 원생성내부와인간청취승인은미검증.
- 실제격리6000.6.5f1 Trace01~03/Clip/Next/Mixer/Quest/Save·BootstrapContinue129PASS/FAIL0, 기존Search로PARTIAL/정본6000.5.7f1미검증. 기존다른ID 재사용·두문장분리 후보만문서인계, 새TTS/음성교체/Catalog·게임코드/원본변경0. 원본오류수정완료로보고하지말것.
- 다음: 바울 대표/TTS담당이정확한ID·원문별재사용/분리후보청취; 불가시에만재생성준비·승인후적용. Main08_Field02_Voice_Technical_QA_20261013.md 참조. 마지막관련Commit은 `Docs: Main08 음성 원본 내용과 재생 경계 감사` 이력확인. Push0.

## 2026-10-13 CURRENT_STATUS 문서 충돌 해소·최신 기획 정합

- 로컬 미커밋 Unity6000.6.5f1 MCP 복구 기록(2026-10-10)을 원문 그대로 보존하고 최신 origin/main의 통합·미술 검수 기록 및 PR7의 A/B/C·Importer 기록을 합쳤습니다. 과거 기록은 당시 상태이며 기존 승인·PASS/PARTIAL/미검증 문구는 변경하지 않았습니다. 날짜가 있는 최근 기록은 최신 날짜부터 표시하고 기존 역사 순서는 유지합니다.
- 확인 기준: main `0e0476649b667129cb7aa6004f4b677335ed8be2`, docs/planning `612ff531f7c067f87441c9f6c2614089a5751c8c`, PR7 `7f493c69080047a06d34b96b71ea57ae4b409e30`. PR7의 B/C·Importer 구현 기록을 main에 이미 반영된 기능으로 해석하지 않습니다. 원본 Unity/Save/아트/TTS/BGM 변경0, 새 Unity 실행0, Push/main Commit0.
- Chapter3 다섯 직업의 신규·강화 스킬은 기획 1차 승인 상태입니다. 기존 시작3개 무료, 신규/강화판 저가 탈렌트 구매, 강화판 원슬롯 승계, 액티브 최대6개·전투2행×3열 규칙 유지. 상세 수치·실제 상품/ID·영속 학습/강화/장착 Save 계약은 별도 확정·구현 대상이며 이번에 생성하지 않았습니다. [기획 정본](../06_스킬/Chapter3_스킬상점_강화_냉기_동결괴사_20261012.md).
- Chapter2 Main21 완료 후 펫 서비스 확대와 Chapter3 진행 중 최초 구매처/클리어 후 전체 안전지역 스킬 상점 확대를 구분합니다. 미정 Chapter3 완료 Quest/플래그는 만들지 않습니다. 각 담당 작업브랜치·Codex 별도 worktree, main 직접 Commit/자동Push 금지 규칙을 유지합니다. 이전 문서 동기화203파일은 그대로이며 기존 CURRENT_STATUS 충돌1개를 이번 보존 병합으로 해소했습니다.
- 검증은 원문 문단 보존·날짜/Markdown/UTF-8·diff --check·보호파일 SHA 검사입니다. 이번 문서 작업을 새 게임 Runtime QA로 보고하지 않습니다. 병합 보고서는 `Temp/CurrentStatusReconcile20261013/worktree/문서/00_프로젝트/CURRENT_STATUS_Reconcile_20261013.md`, 원본 바이트 백업은 `Temp/CurrentStatusReconcile20261013/local.md`입니다. 마지막 관련 Commit은 `Docs: CURRENT_STATUS 로컬 기록 보존과 최신 정본 병합` 이력에서 확인합니다.

## 2026-10-13 PR7 Importer 동일성 검증·로컬 반영

- 후보 d57ff66 → PR7 로컬 통합 codex/integrate-abc-planning-20261013 / 코드병합 2fbe9577b212ee46f4525a597c3bcd4d7833f055. 정상 로컬 설정 대비 2,016,000 픽셀 차이0·363 누락0·환경 Collider/Importer 동등·PNG7 바이트 동일. 새 Field04 228 PASS/FAIL0, 실제6000.6.5f1/정본6000.5.7f1 미검증·기존Search PARTIAL.
- 별도 로컬 codex/local-pr7-importer-20261013 / Temp/PR7Apply20261013/local-working 사용. 원본 main/337항목·822파일/4317SHA 보호. 자동Push/main병합0; 원격PR7 Draft 유지. 서쪽Spawn 겹침·북쪽틈 OPEN, v2 적용0·장착 영속 Save 미승인. [새 상세보고](PR7_Importer_Apply_Verification_20261013.md).

## 2026-10-13 PR7 기존 Sprite 등록 복구 후보 — 원본 미적용

- PR7 3aca기준별도codex/pr7-importer-fix-proposal-20261013. ThirdParty원본변경0, _Project/Art/Environment/LegacySpriteRegistration20261013에기존PNG7byte동일복사+새meta/GUID, Field04362참조GUID만교정. 역치환시기존Scene텍스트동일/Collider·좌표·Quest·Spawn·localID보존/v2미사용.
- 실제격리누락362→0/363, 기존Road30남북유지·Collider동일·수정후228회귀PASS, 기존Search예외로PARTIAL. 다른Scene전역이관/정본6000.5.7f1/손입력미검증. 기존서쪽Spawn/북쪽틈은미해결.
- PR7_Importer_Fix_Proposal_20261013.md와Evidence/PR7_Importer_Proposal_20261013의새등록manifest·실제Preview/회귀참조. 수정후보는main/PR7/사용자원본에반영0, 원본4,317SHA/822dirty/status/HEAD보호/Push0/TTS0.
- 마지막수정후보Commit `d9ea3be7f0a0ab6e361b679a71f6e1bb4c59e611`. 다음:별도수정PR검토·통합후새HEAD재QA, 현재원PR3aca는BLOCKED/Draft유지. 미술v2적용·Save스키마·신규상품을이교정과혼동하지않음.

## 2026-10-13 A — Field04 v2 동서 도로 격리 후보

- codex/field04-v2-road-20261013 / Temp/ABC20261013/A. 검수 Commit `f6222caacea500dfd71e9d9537cb2f9eca9396e9`. 최신 main0e04766·planning c0f1d7c 기준; 기존 통합 PR#2는 main에 반영되어 이력을 보존합니다.
- 실제 Road30을 x=-7..7/y±.5로 재배치한 후보와 Optional Endpoint12 포함42안 제작. 대응363/363, 나무Medium3/Big2 수정, RockNorth alpha foot차-.005625, PPU128/Center/GUID55충돌0. 원본 게임 Scene/PNG/meta 변경0; 정식 적용은 미술 승인 대기입니다.
- 최종 A674PASS/FAIL0 + 별도Preview244PASS/FAIL0. Main12→13/세린Story·일반2전투/35초/Continue2/147물리waypoint/Camera12/실제 동서 Physics Exit3 통과. 최초 QA helper·Importer 경로 누락은 보완 후 전체 재검증, 이전 결과는 Temp 보존. 기존 Editor Search예외·서쪽 Spawn 겹침·북쪽 틈/손플레이·원격6000.5.7f1 미검증으로 종합PARTIAL입니다.
- 현재 보호 snapshot4,318파일 SHA 불변, 원본822미커밋/status/HEAD ff022e6 동일. 원본Editor포커스·Save·TTS/BGM 보호, TTS실행0/Push0/main병합0. 검수 보고서 Field04_v2_Road_Candidate_QA_20261013.md 및 Evidence/Field04_v2_20261013 참조.
- 다음: 바울 대표의 v2 미술·도로30/42안 선택 승인 후 별도정식통합·동일회귀. B/C는 별도브랜치에서 관리하며 이 A 브랜치가 B/C 코드 통합을 포함한다고 간주하지 않습니다.

## 2026-10-13 작업 C — Chapter2 안전지역 펫 서비스 확대 (격리 구현/QA PARTIAL)

- 브랜치 `codex/safezone-services-20261013`, 기준 main `0e04766`. Main21의 기존 Quest 완료 기록으로 StarterVillage·지하묘지 입구에 분양/관리 각1명, Arbel 기존2명 보존. 공용 소유/100탈렌트/장착/SaveVersion1 유지, 미완료 슬롯은 추가NPC 제거.
- 기존 사수0명도 분양되던 정본 불일치를 사수목록 안내/구매 경계로 보완. 타 직업 주인공+보유 사수 동료는 이용 가능. Scene·기존NPC/meta·승인PNG·TTS/BGM 직접변경0.
- `SafeZone_PetService_Rollout_Implementation_QA_20261013.md`에 후보4좌표·기존Sprite재사용·C3 최초상점과 전체확대 분리설계 기록. C3 완료조건/상품/가격/Save확장은 미정이며 코드 생성0.
- 최종 격리 Unity6000.6.5f1 Compile·Runtime 오류0·3거점UI/100구매/공유/완료·미완료Continue/타슬롯/Save실패rollback/지체의길 Collider통행 PASS106, 기존Editor Search예외1로 전체PARTIAL. 원본6000.5.7f1·Main21전체재플레이·손플레이/이름표가독성/신규NPC공식미술·대사는 미검증/검토대상. 다음: 후보NPC 확인과 브랜치 검토/승인. Push/main 반영0. 마지막 구현 Commit `0d151e3f0222dac992145e69717f7607158b8106`.

## 2026-10-13 B — 6칸 임시 스킬 장착·2행×3열 전투 메뉴

- codex/skill-loadout-ui-20261013 / Temp/ABC20261013/B에서 구현·격리 QA 완료. 캐릭터ID/직업별6칸+독립보관함, 전투시 칸위치 스냅샷, 빈칸버튼0·뒤로분리·K/패드Menu·Start 무료관리. 시작3개/직업·기본5명령·기존15효과·Quest/NPC/펫/Scene/Save schema 보호.
- 실제Unity6000.6.5f1: 새UI/장착72 PASS, 기존15스킬154 PASS(effect15), C+B 3안전지역 공존28 PASS, 기존침묵감사ALL PASS. 5해상도/16:9·4:3·21:9 RT 및36행Mask/focus/6칸geometry PASS. 초기 명단호출누락/21:9팝업겹침 FAIL을 수정하고 원시실패 보존.
- 전체PARTIAL: 기존UnityEditor.Search 예외1/신규Runtime0. 정본6000.5.7f1·실물입력/손플레이/장시간가독성 미검증. 장착은 세션메모리만 유지, 새게임/Save로드/재시작시 시작3개 복원(화면직접명시). 영속학습/장착Save계약·상품/가격/Chapter3ID 결정대기, 실제신규스킬/상점0.
- 원본4,318파일SHA·822미커밋파일/status/HEAD불변. 원본Editor·Focus·PNG/meta/Save/TTS/BGM/ThirdParty 무변경, Push/main병합0. [보고·Preview·실패근거](Skill_Loadout_UI_Implementation_QA_20261013.md).
- 다음: 대표님 관리/전투UI 확인 및 Save 영속계약 결정, 승인된 작업브랜치만 사용자Push/PR 검토. 마지막 구현·QA Commit: `c4ed7c9fd2ed9b3fe5cf3ea1c57387ef51d5e816`.
## 2026-10-13 담당별 브랜치 규칙·통합 v2 준비

- 기존 통합961c90d/이전worktree·원본main ff022e6 보존. 새 codex/integrate-main-20261013-v2 / Temp/GitIntegration20261013V2/main-integration에서 최신origin/main73c37bf를 merge, 충돌0/이전통합·원격둘다조상/Unity변경0. Merge SHA `71f71bb934bfc2ea5542b0a96d957410c35e1f7d`.
- 이제 AI main직접커밋금지, 기획docs/planning·Codex codex/주제 작업브랜치. 기획정책문서는docs/planning74c5a0b에서읽었고다른담당브랜치변경0. 사용자Push는Codex작업브랜치만, main병합은별도결정. 이전HEAD:main안내대체/Git_BranchOnly_Integration_20261013.md참조.
- 원본337항목/822미커밋·4,750파일전후SHA/status/HEAD 보호. 원본Editor/Unity/TTS/아트적용/Push0. 기존QA·미술승인대기·Field04보완상태유지.

## 2026-10-13 Git 안전 통합·Rock B v2/Field04 격리 검수

- 원본337항목/822미커밋·4,750보호파일 SHA 불변. 원본main ff022e6/Editor Bootstrap·비포커스/Save/TTS/BGM 보호. 최종원격405ce12(원격전용7커밋)+로컬A/B를 별도 codex/integrate-main-20261013 worktree에서 보존 통합, 충돌0/Unity diff0/Push0. 정확한 사용자 Push 명령은 ArtInspection_20261013_Summary.md.
- Rock B v2: 고립14 제거/본체3537 동일RGBA·19px상향, 실제Unity/PPU128/Center·접지차0.001875 PASS. USER_ART_REVIEW_PENDING, 공식375/376·기존RockNorth1 유지/신규 적용0. 검수Commit `975fb636a7a7a7527bc704c21019853301d83bac`.
- Field04: CRC/PNG/hash·타일27·대응363/363·실제RenderTexture/Importer PASS. 나무Big/Medium5참조역대응·RockNorth발높이보완 ART_MAPPING_FIX_REQUIRED. QA제안362/363·Optional0, 정식 적용0. 검수Commit `dcb1f14524de6cfb295cbf5e5d6cfb83d24229a5`.
- Main13전체/Continue2/Field03·Arbel·Field05왕복/Story+일반2 UI승리·35초/휠체어179waypoint/Camera12검사PASS. 최초QAJson오류·정확서쪽Spawn경로FAIL보존, 마지막재개177PASS/FAIL0. 원본서쪽Spawn겹침·북쪽Collider틈/기존Search예외로전체PARTIAL. 사람미술·OS입력/권리/원본정식Import미검증.
- 다음: Rock v2 사용자미술승인, Field04나무표/RockNorth보완·색감검토. 기존경계/Spawn수정은별도기술범위. TTS실행0·투사스킬구현0. 원본dirty CURRENT_STATUS는 수정하지 않고 통합worktree문서만 갱신.

## 2026-10-12 Field03 Rock B v2 자료 대기

- ART_FIX_PENDING. 지정경로와 F:/Downloads·사용자Downloads 재귀 ZIP 검색에서 Rock B v2 미발견. 발견된 Field03 v1은 수정본으로 사용하지 않았습니다.
- 신규 적용0, 공식375/376과 기존RockNorth1·좌표/Scale/Collider/GUID 유지. v2 PNG/Manifest/분리픽셀 제거/실제 접지 QA NOT_VERIFIED. 다음: 실제 수정본 납품 후 개별 검증.
- 별도 QA 문서 Field03_RockB_Fix_v2_Followup_QA_20261012.md. 문서 Commit은 Git 이력 참조. Push 미실행.

## 2026-10-12 B1 봉인문 후속 시각 보완

- B2 완료 Push47a65e2 확인 후 진행. 기존 벽 내0.75×0.625 닫힘/열림 상태 표식1개와 기존 하강로 B2이동 안내 적용. TECH_HOLD_RESOLVED_STATE_PLAQUE, 전체 PARTIAL(Search/사람 가독성 미검증).
- Main11 해금·전후Continue·B1/B2/Field03왕복·휠체어·B2단서 회귀 PASS. 모든Scene/Collider/PNG/meta·Save·사용자337/822 보호. 전투/35초 검증 재사용.
- 다음: 실제 화면의 작은 표식 가독성 확인. 마지막 관련 Commit: `6c78cdb833e873639b0a027d5cfdc7efb5a83b3f`. Push 미실행.

## 2026-10-12 Dungeon B2 정식 통합·봉인실 스포일러 수정

- USER_ART_APPROVED. PNG31 등록, 기존54 앵커 보존, B1 렌더링 코드 그대로 재사용: Tiled18/원크기소품8/중복표시숨김28/생성26.
- Main11 B2·일반4 UI승리/35초·보스 Phase2/소환/도망금지·석판1/정상귀환완료·Continue/재진입 표시 복원 PASS. 단서7 처치전숨김/후표시, 석판 회수후숨김.
- 전체 PARTIAL: 기존 Search 예외, 사람 직접 플레이·장시간 가독성/권리 NOT_VERIFIED. B1 전체 바이트와 문 TECH_HOLD·사용자337/Save/GUID 보호. 다음: 실제 화면·가독성 사용자 확인.
- 마지막 관련 Commit: `2babb13c41bd1c2c75ad69bb93381dc3ed71547a` (정식 통합·스포일러 수정·QA). Push 미실행.

## 2026-10-12 Dungeon B1 승인 아트 정식 통합

- USER_ART_APPROVED. PNG25 등록, 기존63 앵커 보존, Tiled20/원크기 소품8/중복표시 숨김33/기존 문2. 생성28개 재사용.
- Main11 B1·4개 실제 전투/35초 리스폰·B1/B2/Field03 왕복·Continue·휠체어 경로 통과. 전체 PARTIAL: 기존 Search 예외, 신규 문 시각 TECH_HOLD, 사용자 손 플레이/장시간 가독성/권리 NOT_VERIFIED.
- 기존 사용자337항목·B2·Save·GUID 보호. 다음 권장: 사용자 실제 화면 확인과 문 비율/상호작용 정합 설계. Git Push 미실행.
- 마지막 관련 Commit: `c47806bee9213775b98fb1b7b17b67b3f57f1c54` (정식 구현 및 QA 근거).

<!-- DUNGEON01_B2_INSPECTION_20261012_BEGIN -->
## 2026-10-12 Dungeon01 B2 리뉴얼 실제 검수 — 사용자 미술 승인 대기

- **USER_ART_REVIEW_PENDING / 기계검사PASS / 전체QA_PARTIAL / 원본영구적용0**. ZIP51/PNG44(실사용31·Preview12·QA1), CRC/decode/Manifest/alpha/31Importer·신규GUID충돌0, Floor4종16쌍좌우/상하Seam정확일치. Scene54공용Sprite1/기존복제0+Main11단서Runtime7. 기존형상불변, 격리환경자식26/단서자식7/석판그림1만QA제안.
- 실제B2순차조사·일반4UI승리/35초리스폰·B1왕복·보스24정상공격승리/Phase2소환2·장막·충격준비·도망거절·봉인실조사·석판1/정상귀환/Main11완료PASS. PrimaryContinue2회+실제완료Save별도뷰복원, 휠체어8지점244waypoint/Camera16:9네방향BoundsPASS. B1구간은기존PASS재사용/완료checkpointfixture, 재실행0.
- **기존처치전단서7/7 enabled 시각적선노출위험**: 장벽/순차조건은차단PASS. 후보QA에서만처치전숨김/후표시·석판회수후숨김제안, 원본게임수정0. 보조Gate/장식미배치, 얇은푸른홈·작은문양·벽비율·B1색감·Marker가독성검토필요. 초기QA컴파일3진단은QA2오류수정후Compile0; raw보존.
- 기존Search예외로PARTIAL. 세부침묵CD/DoT·모든직업/밸런스·OS직접플레이·사람최종가독성·독립상업권증빙NOT_VERIFIED. 원본3738SHA불변/사용자337·822보호, Save/meta/승인아트/TTS/BGM/Editor포커스무접촉·Push0.
- [정본보고/실제Preview](Dungeon01_B2_Renewal_Art_v1_Inspection_20261012.md). 다음: 사용자미술검토, 승인후별도역할/반복/단서노출최소통합설계. 기준395dd5d, 검수Commit `4e30cd89f2ed9b2a2cefa4dcfd96c37e2062a012`.

<!-- DUNGEON01_B2_INSPECTION_20261012_END -->

<!-- DUNGEON01_B1_INSPECTION_20261012_BEGIN -->
## 2026-10-12 Dungeon01 B1 리뉴얼 v1 실제 검수 — 미술 승인 대기

- **USER_ART_REVIEW_PENDING / 기계검사PASS / 전체QA_PARTIAL / 원본영구적용0**. ZIP44/PNG38(실사용25·Preview11·QA이미지2), CRC/decode/Manifest해시·크기·alpha PASS. 격리25Single/PPU128 Import, 실제Scene63Renderer/공용Sprite1/원본환경복제0 확인. Floor3종9쌍좌우/상하Seam정확일치.
- 전역1:1교체는실제왜곡FAIL. 기존형상/Collider불변의격리자식표시30개·반복타일 후보와단순교체를구분. 줄눈29/뚜껑4 후보표시제외, 봉인문상태바인딩은QA제안/게임미적용. 실제전체/부문/닫힘열림/B2연속성/Overlay/63CSV 납품. 미술검토:밀도·B2색감·작은소품·문Collider/상호작용관계.
- Main11 B1순차조사/역순·중복차단/해금/B2진입index5, Continue3회, 일반4조우정상UI승리·현행35초·최종4중복0, B2복귀(0,6.8)/Field03실제물리출구(.55,-2.65)·재진입, 휠체어6지점233waypoint+실제출구PASS. QA옛출구명/Scene교체뒤Rigidbody계측오류만수정,기존PASS반복0/실패로그보존.
- Compile/Missing0, Search기존예외로PARTIAL. 사람가독성·OS직접입력·전직업/연속사냥·B2보스전체/봉인실/Main11완료·독립상업권증빙은NOT_VERIFIED. 과거B1/B2의Main11미구현표기는현행스토리/코드와다른초기기록.
- 원본3738게임파일SHA불변·사용자337항목/822파일보호·Save/설정/GUID·승인Field03/TTS/BGM변경0. 원본Editor/포커스무접촉·Push0. [검수보고/실제Preview](Dungeon01_B1_Renewal_Art_v1_Inspection_20261012.md). 다음: 사용자미술검토와승인후별도통합설계. 기준7f30f60,검수Commit `dce84d58cdf361dba145e8f1c22b66cabf639ff8`.

<!-- DUNGEON01_B1_INSPECTION_20261012_END -->

<!-- FIELD03_OFFICIAL_20261012_BEGIN -->
## 2026-10-12 Field03 사용자 승인·정식 통합 완료

- **USER_ART_APPROVED / 신규11종 / 375 of 376참조 / QA_PARTIAL**. Scene362+Theme복제13 신규적용, Rock B1종/Scene1개는 분리alpha14픽셀·접지불일치로 기존Sprite 유지. 원본PNGbyte/기존GUID·좌표·Scale·Flip·Sorting·Collider·Tint(0.46,0.61,0.46)/CameraColor 보호. 신규26파일·GUID15개/충돌0, 보조8종 배치0·게임C#변경0.
- 실제 Main08전체/Main09폴해금/Main10입구개방·Continue3회 PASS. Main11은 Field03자동시작이 아니라 Dungeon01진입시시작 확인. QA의잘못된가정만수정하고 Main10완료Save에서재개, 이미통과한Story반복0. 모델용량중단동안남은QA완료되어 재실행0.
- 일반6Spawn 정상UI전투·30초미생성/현행35초리스폰·최종6/중복0, Field02/Dungeon01왕복, 휠체어11지점/112물리waypoint, Field01/02/마을회귀 PASS. Compile/Missing Sprite/Script0. 기존Search예외로전체PARTIAL, 폴주변이름/안내겹침은기존검수에서도관찰. OS직접조작·Main11전체던전·장시간가독성/밸런스·외부소스권리 NOT_VERIFIED.
- 사용자337항목/822파일 보호, 게임기존3712파일중Scene1만변경/3711SHA불변. CURRENT_STATUS 사용자8줄은Commit제외. 원본Editor/포커스무접촉·Push0. 다음: Rock B수정본 재검수/1참조연결, 사용자실제플레이, 기존Search별도조사.
- [정식QA·Preview·증거](Field03_Renewal_Official_Integration_QA_20261012.md), [Rock B수정요청](Evidence/Field03Official_20261012/RockB_Art_Correction_Request.md). 기준Commit `9c7457b`; 정식통합Commit `dcd913e4dcb870528206623120a0d2d1af0059d5`. Push는사용자직접수행.

<!-- FIELD03_OFFICIAL_20261012_END -->

<!-- FIELD03_INSPECTION_20261012_BEGIN -->
## 2026-10-12 Field03 리뉴얼 v1 검수 — 미술 승인 대기

- **USER_ART_REVIEW_PENDING / QA_PARTIAL / 원본 영구적용0**. ZIP34항목/PNG29(실사용20=직접12+보조8/Preview7/QA2), CRC·decode·Manifest20해시/크기/로컬GUID·수량 일치. 실제Scene363+Theme복제13=376개 대응CSV/RenderTexture/Overlay 확보. 기존Tint(0.46,0.61,0.46) 유지한 격리메모리 교체만 수행.
- 보완 권장: Rock B 상단 분리14alpha픽셀·중앙아래 본체/접지, 나무 밝은 외곽·바닥 반복밀도·Field02연속성/암부. 보조8종 미배치/입구단계 보호, 출처·권리 독립감사 미완료. [검수보고/실제Preview](Field03_Renewal_Art_v1_Inspection_20261012.md).
- Main08 Field03 실제진행/Main09 6목표·폴해금/Main10 7목표·입구0~3숨김/4이후표시·Continue3회·Dungeon B1진입/귀환·Field02왕복 PASS. 일반6Spawn 정상UI승리·재생성 PASS. **현행Field03는 전부35초**: 30초 미생성/35초생성 확인, 요청30초와 불일치 기록·게임설정변경0.
- 실제Player반경0.5+margin/NPC Collider 포함11지점 물리접근·16:9카메라4방향viewport Bounds PASS. QA의30초기대값/0.4반경·NPC제외 경로실패 보존 및 검사만 보정. 기존Search예외 유지. OS직접키/패드·사람가독성/조작감·전직업/레벨/연속사냥밸런스 NOT_VERIFIED.
- 원본3712파일 SHA256불변·사용자337항목/822파일 보호, Field02 정식적용 재작업0·TTS/BGM/Save/설정/meta/Scene 변경0. 원본Editor/포커스 무접촉, Push0. 다음: 미술보완/사용자승인 이후 별도 Sprite-only 통합. 기준 `e31d8e3`, 검수Commit `80d998a`. Push는 사용자 직접 수행.

<!-- FIELD03_INSPECTION_20261012_END -->

<!-- FIELD02_OFFICIAL_20261012_BEGIN -->
## 2026-10-12 Field02 최종 미술 승인 및 정식 통합

- **USER_ART_APPROVED / IMPLEMENTED / QA_PARTIAL**. 신규12 PNG(원본byte 동일)+meta/GUID16개 고유. Scene363개 참조와 Theme복제10개=373개 공식아트 연결. GroundTint(0.62,0.76,0.60), 배치/Scale/Flip/Sorting/Collider 보존, 보조5종 추가0·게임 C# 변경0.
- 실제 격리 Main06 조사·Main07 폴/정상3대3전투/미엘·폴귀환/완료·Bootstrap Continue·Main08 정상자동시작·Field01/02/03왕복 PASS. 일반6Spawn 정상승리·원래30초 만료전 생성0/만료후각1개·최종6개 PASS(관찰프레임 지연2.4~2.9ms). 휠체어외형/환경반경0.4 실제물리87waypoint·Field01/마을 회귀 PASS.
- 기존 Search예외 별도미해결로 전체 PARTIAL. QA 출구좌표 오류/재개, 연속무회복 기본공격 승리FAIL 이력 보존. 개별 스폰 테스트의 기본HP fixture를 연속사냥 밸런스 승인으로 확대하지 않음. 사람 키보드/패드 플레이·가독성/조작감·전직업/레벨 밸런스 NOT_VERIFIED.
- 원본3684파일 중 Field02 Scene1만 허용변경, 다른3683해시 불변. 사용자337항목/822파일 보호, 기존TTS25·오류10 무음·Save·메타/승인아트 유지. 원본Editor/포커스 무접촉. 기존 통합worktree 유지·main55ac89f 안전FF·Push0.
- [정식 통합 QA/증거](Field02_Renewal_Official_Integration_QA_20261012.md), [승인 및 기존 검수](Field02_Renewal_Art_v1_Inspection_20261012.md). ZIP현재경로 `F:/Downloads/Limitless_Sprites/Limitless_Field02_Renewal_Art_v1.zip` 재확인. 다음: 사용자 실제플레이와 기존Search 별도복구.
- 관련 기준Commit `55ac89f`, 정식통합 Commit `7b75d00`. Push는 사용자가 직접 수행.

<!-- FIELD02_OFFICIAL_20261012_END -->

<!-- URGENT_20261012_BEGIN -->
## 2026-10-12 긴급 묶음 작업 — 3단계 처리 및 검증 체크포인트

- Field02 ZIP: CRC/PNG25개·실사용17개·Manifest 해시/크기 일치. 실제 Scene363+Theme복제10=373개 대응표, 격리 Import/RenderTexture 확보. 배치·Scale·Flip·Sorting·Collider 보존. 원본 영구 적용0.
- **ART_REVIEW_PENDING / 기술 PARTIAL**: 바닥 Theme 색조, Rock B의85×54/PPU100→128×128/PPU128 표현 차이, 보조5개 배치 승인 필요. Main07 폴 실제 대화/3대3 UI정상승리/미엘·폴 귀환/완료·Continue와 Field01/03 양방향 Trigger/Spawn PASS. Main08 자동시작을 QA 기대값에 반영, 게임 수정0. 연속 도보/휠체어 체감·일반6마리30초 리스폰은 미검증. [검수 및 통합 인계](Field02_Renewal_Art_v1_Inspection_20261012.md).
- Chapter2 종합 QA **PARTIAL**: Main20 Attack19회/Phase2 정상 승리, Main21 A/B Attack각7회 정상 승리·마차12초/Skip/하렌/완료Continue PASS. Main12~21 순차 서비스·서브9종/전리품7종 회귀 PASS. Search Console assertion FAIL 원문 유지. Main12~19 실제 UI 전투/전체 수동 플레이 미검증. [종합 보고](Chapter2_Main12_21_Integrated_Regression_QA_20261012.md).
- Chapter3 스킬 상점·강화/부활/정화Ⅱ·냉기/화로·몬스터/전리품/펫/경제 설계3문서와 기존 정본 교차 링크 완료. 구현0·미정 수치/ID 추가0. 기존 TTS 정상25개 재통합0·오류10개 의도적 무음 유지.
- 관련 Commit: Field02 최초검수 `47566ff`, Chapter2 QA `2eaf2e9`, Chapter3 문서 `ef49afd`. Field02 보충 최종 `91ee5a0`. 원본3661파일 SHA256 불변·사용자337개 상태 동일·staging 비움·격리 QA 종료/원본PID31832 유지 확인. Push0, 사용자 직접 Push.

<!-- URGENT_20261012_END -->

## 2026-10-12 Main18·19 TTS — 정상25개 통합 / 세린10개 재제작 대기 / QA_PARTIAL

[선택적 통합·원인 감사](Main18_19_TTS_Selective_Integration_QA_20261012.md)·[개별35상태](Main18_19_TTS_Selective_Applied_Matrix_20261012.csv)·[Schedar10개 인계](Main18_Serin10_Schedar_Regen_Handoff_20261012.csv). Main18 정상12/12·Main19 정상13/13 원본 WAV byte 그대로 연결하고 실제 Story/DSP/자연 종료를 확인했다. Main18 오류10개는 REGEN_REQUIRED·Unity미연결·실제무음이며 Dialogue ID/화자/자막을 유지했다. Manifest는 검증25개만 RUNTIME_VERIFIED, Player/지문과 Status 외 모든 필드 불변이다.

오류 원인은 미확정. 코드/Review는 Schedar·gemini-3.8-flash-tts를 가리키지만 성공한 원본10개의 실제 요청 Voice/SDK/응답 정보는 없어 UNKNOWN이다. 구 제작 style에 여성 명시가 없고 정상본 생성 후보에는 여성 명시가 있는 차이는 발견했으나 인과를 단정하지 않는다. 공식 요청 형식과 코드 형태는 일치한다. Schedar 유지·자동 Voice 교체0, TTS API/재생성0. 요청한 Schedar_v2.py/수정ZIP은 미발견; 발견한 이전 Staged 도구의 Erinome/Kore 후보는 현행 정책과 달라 실행하지 않았다.

정상 WAV25 PCM 원본↔Unity 최대 오차0·새AudioGUID 중복0. 실제 Catalog255→280, 기존255 entry를 포함한 byte prefix 전체 보존. 오류10 entry0·WAV Import0. 조건부 폴/미엘/태온·Mixer/Mute·빠른Next/Scene잔류0·Save/Continue2지점·기존255개 조회/짧은재생·Main18/19 지정전투 진입/승리처리/목표1회 진행 PASS. 기존255개 전체 의미 청취/정상 전투 입력을 재검사한 것은 아니다.

격리6000.6.5f1 CompileError0·MissingAudio/Script0·신규Runtime오류0, 기존Search예외 재발로 전체PARTIAL. 사용자 정상 청취 의견과 의미별 승인/ASR/Runtime 결과를 분리했다. ASR의 단어 차이5개는 추가청취 권장이고 자동 오발화 확정은 아니다. 원본 Editor 강제Import/Play/포커스조작0으로 원본창 Import 완료는 미검증. 다음: 오류10개만 Schedar 재제작·샘플검토/요청로그 확보, 정상본 단어 청취 확인은 상세보고 참조.

기존3632파일 중 Catalog1개 외3631개 불변·삭제0, 원본/체크포인트/제작스크립트50개 보호. 신규52파일, Runtime C#·Quest/전투/Save/기존Main01~17 WAV/meta/GUID 변경0. 사용자337개 Git항목·CURRENT_STATUS 별도MCP기록 보존·원본Editor PID31832 유지·Push0. 마지막 기능관련 Commit `e83393ceabc74c38dc49661969c1dd5e68bf06c0` (`Feature: Main18 세린 오류10 제외와 정상 Main18 Main19 음성25 통합`).

---

## 2026-10-12 하렌·세린 공식 아트 v1 — 통합 완료 / QA_PARTIAL

[검수·통합 QA](Haren_Serin_Official_Art_v1_Integration_QA_20261012.md): ZIP10항목 CRC·PNG8디코딩 PASS. 본자산5개를 원본 바이트 그대로 등록했다. 하렌 배경 초상화는 Manifest의 RGBA 표기와 달리 RGB이며 투명형은 RGBA다. Field512×512/128셀16프레임·PPU128/하단중앙·Idle/Walk8Clip, 하렌·세린 Battle1254×1254/209셀30프레임씩·PPU100/하단중앙·6Clip/6상태/5Trigger 생성. 신규meta GUID38개 충돌0.

세린 companion_serin 전투 외형 연결, 사수3스킬에 Skill 외형 호출만 추가했다. 실제 Battle 등장·기본/정조준/화살비/야수/피격/전투불능 PASS. 기본·정조준 화살 이동과 화살촉이 왼쪽 적을 향함을 실제 경로·확대 RenderTexture로 확인했다. GoldenArrow·투사체·피해/쿨타임/파티3슬롯 불변. 기존 폴·미엘·태온 공용 attachment와6Trigger 재생/종료 PASS.

하렌 Main21 첫 만남 index11에만 공식 Field Idle과 story_haren 초상화를 연결했다. 실제 하렌 화자 페이지·대화 종료 후 목표12 진행 PASS,13목표·대사·전투·퍼즐·보상·Save 보호. 하렌 CompanionDefinition/명단/능력치·스킬/Chapter3 추가0. 하렌 Battle은 ART_READY / RUNTIME_PENDING이며 합류 기획 확정 후 연결한다.

격리6000.6.5f1 Compile오류0·Missing Sprite/Script0·신규Runtime오류0. 기존 Search 예외가 재발해 전체 PARTIAL. 원본 Editor 강제 Import/Play/포커스 전환0으로 원본 Import/recompile 완료는 NOT_VERIFIED; 동일 소스·자산의 격리 검증을 사용했다. 생성 아트 포즈별 장식·활·망토 변화의 최종 미술 체감, 전체 기존 스킬/수동 입력·Main21 전체 재주행은 미검증이다. 다음: 자연스러운 원본 Import 완료와 새 포즈 연속성·실제 입력 확인, 하렌 합류는 별도 기획.

기존3564파일 중 허용 C#3개 외3561개 불변·삭제0, 신규68파일. 기존 사용자335개 Git항목과 CURRENT_STATUS 별도 MCP기록 유지. 기존 초상화/세린Field/플레이어Animator/마을v4/Field01/Save·설정 보호. 원본Editor PID31832 유지·화면조작/Push0. 마지막 기능관련 Commit `3674add11e8cc153ffb91dc8ebaecb5d1b7fbbbe` (`Feature: 하렌 세린 공식 아트와 Main21 첫 등장 외형 통합`).

---

## 2026-10-12 Field_01 리뉴얼 v1 — USER_ART_APPROVED / 통합 완료 / QA_PARTIAL

[복구·최종 QA](Field01_Renewal_Recovery_QA_20261012.md): 기존 환경12종 Commit `40f9517` 보존. PNG/ZIP일치12·GUID충돌0·실제Importer PASS, Field_01 환경363개 Sprite만 교체·그 외 Scene 불변, Generator12역할 전용매핑 완료. Scene재생성/PNG/meta GUID/Spawn정의/리스폰설정/게임Runtime C#수정0.

리스폰 실패는 World_StarterVillage에서 Field01 몬스터를 센 QA오류. 입력·복귀위치/Scene집계만 통제해 실제8전투 승리반환 뒤 기존30초 만료+1.6~2.2ms 생성8/8 PASS(조기/중복/누락0). Main02~06 실제Trigger·대화·Story2/3인 진입/승리반환·태온/미엘 해금·마을/Field01/02 왕복·별도Save/Continue좌표·Quest복원 PASS. 실제Direct3D12 RenderTexture/WorldSpace Quest Marker 확인. QA의 Main03 대화종료·Main05 의뢰선택/프레임대기도 보완하고 체크포인트부터 재개.

Compile오류0·새Runtime오류0, 기존Search 내부예외2건 미해결(최종격리실행 SearchDatabase1건 재발)로 전체PARTIAL. 정상공격/스킬로 적전멸하는 전체전투, ScreenSpace Overlay 이름표 가독성·미엘주변 전체조작화면 및 전체Party/Battle회귀는 NOT_VERIFIED. 다음: Search별도진단·사용자편한시점 실제전투/전체이름표 확인.

원본3564파일 변경/삭제0, 기존사용자335개 Git항목과 별도MCP기록 보존. 기존QA복사본 재사용·원시로그/실패증거 보존·테스트Save별도. 원본Editor PID31832 유지·Scene/Play/포커스/강제종료조작0, GitHub Push0. 마지막 기능관련 Commit `37e4e3dc0573dc19d1ce142d9b87c77cb90cf6e9` (`Feature: Field01 오리지널 환경 통합 복구와 리스폰 QA 완료`).

---

## 2026-10-10 Main21 「돌아온 온기」 — DESIGN_CONFIRMED / IMPLEMENTED / QA_PARTIAL

[상세 정본](../03_스토리/Chapter2_Main21_돌아온_온기.md)·[격리 QA](Main21_Implementation_QA_20261010.md): 승인13목표/A·B 기존몬스터3인 전투/자유순서3단서 안전통로/직접주민지원/동일접근가능마차 Yes·No/12초·Skip/안전Spawn·중단Continue/하렌텍스트첫등장/Chapter2완료 보상100EXP·60Currency한번 구현. Main22진입0. 선택Save플래그 추가,SaveVersion1·슬롯5 유지. 원문21+Path대사2 Manifest/신규WAV0.

기능213·마을335·서브/메인서비스588·Main18기능467·Main19기능169·Main20기능150 검사 PASS,격리Search 내부예외로Console FAIL/전체QA PARTIAL. 기존Search예외2건 이슈 유지. 실제 전체일반전투 난이도·30~45분·사용자플레이감각 NOT_VERIFIED/USER_REVIEW_PENDING. 하렌아트·말정밀프레임·내부가림 ART_PENDING. 다음: 사용자플레이검토·아트/TTS납품·Search별도복구.

보호3466파일 중 공통C#5개 외3461불변/삭제0,기존Git335항목 보존. Scene/NPCPrefab/사용자Save·설정/공식아트/음원 보호. 원본Bootstrap/Edit/포커스false 유지,원본Play·Scene전환·화면조작/Push0. StarterVillage v4 승인 Scene+NPC는 `7309f772`에서 이미Commit완료(아래 과거 미커밋 기록을 대체). 설계Commit `6ce4fd3`; 구현Commit `546ade248a33f14a4a62bcfcdc3c6b0d1411c715`.

---

## 2026-10-10 StarterVillage v4 — 기술 PASS / LOCAL 적용 / 미술 승인 완료

[최종 검수](StarterVillage_Renewal_Art_v4_Inspection_20261010.md): 문 상·하단 및 지붕 접합/환경15종 PASS. 전용 V4 GUID·실제 Importer 검증, Scene284+남문 활성16+비활성2 Sprite만 교체. Generator 전용 로더 연결·ThirdParty 원본 불변. 격리 실제 RenderTexture·NPC12·남문 왕복/Spawn/저장 검사 PASS; 서브9종 Runtime/Continue 및 Main01~20 서비스 검사 PASS. 격리 Unity Search 내부 예외2건 미해결로 Console 검사 FAIL/전체 QA PARTIAL, 전체 메인 전투 회귀는 미검증. 사용자 기존 Scene 변경과 분리할 수 없어 실제 적용 Scene는 미스테이징 보존하고 LOCAL Sprite-only 패치를 Commit 증거로 남김. 보호1257개 중 허용3개 외1254개 불변. USER_ART_APPROVED. 마지막 관련 Commit은 이 항목을 포함한 `Feature: StarterVillage v4 전용 환경 통합 및 격리 QA`; 마차 보완 문서 Commit `1121cec`. Push0.


최종 승인 재검증: LOCAL 사본 대비 Sprite284개만 변경·그 외 불변. HEAD Renderer500/LOCAL284·공통 ID0으로 사용자 변경 없이 Scene Commit 분리 불가, LOCAL 미커밋 유지. Unity Search 예외2건 미해결·QA PARTIAL 유지. 통합 Commit `9af4e688`; 승인 문서 Commit `edb603e`.

---

## 2026-10-10 Main21 마차 Supplement v1 — 참고 검수 / PARTIAL·미구현

[보완 검수](Main21_Accessible_Wagon_Supplement_v1_Inspection_20261010.md): CRC/PNG4·Manifest PASS, RGB 참고 도해만 제공되어 신규 Sprite/Animation Import0. 휠체어2+좌석3·한 명씩 램프 탑승·가림 구역 개념 보완, 실측 치수/실제 회전 동선·기존 PNG Seat3/짐 대응·가림 레이어는 PARTIAL/USER_REVIEW_PENDING. 말 Rect/Pivot/접지점/FPS는 미제공, 기존 Grid 위험 미해결. 구현 준비 정본에 Anchor/Sorting/Scale TBD·추가 납품 요구 연결. 자동 귀환/Quest/Controller/Save 구현0. 마지막 관련 commit은 이 항목을 포함한 `Docs: Main21 마차 보완 도해 검수와 미결 계약 기록`; 이전 아트 `ebf3872`. A 작업과 독립, Push0.

---

## 2026-10-10 Main21 접근 가능한 마차 v1 — 독립 아트 등록 / 이벤트 미구현

[검수 정본](Main21_Accessible_Wagon_Art_v1_Inspection_20261010.md): CRC/PNG5개·Manifest/정지 Import PASS. 전용 Art/Story/Main21Carriage/V1에 마차 Sprite2/미분할 참고 Texture2 신규 등록·원본해시 일치·실제 Importer/Console Error0 확인. 휠체어2대/벤치 구조는 보이나 승객5명·실제 치수 USER_ART_REVIEW_PENDING. 말3프레임 Rect/접지점·Scale·Anchor·가림 레이어는 후속 확정, Quest/Controller/Scene Transition/Save 구현0. 보호437파일 해시 불변·Bootstrap/Edit Mode/포커스 비활성 유지. 다음: 수용 배치 도해/Anchor·프레임 납품정보 및 최종 미술 검토 후 별도 구현·격리 QA. 하렌 이름 확정 commit `0ae6b3b`. 마지막 관련 commit은 이 항목을 포함한 `Chore: Main21 마차 v1 독립 아트 등록 및 검수`. Push0.

---

## 2026-10-10 하렌(Haren) — 정식 이름 확정 / Character 미구현

사용자 승인으로 태온 친형의 정식 이름을 하렌(Haren)으로 Story·팀 공통 Character 정본에 반영했다. 시각의 길/투사·제한적 시각 보조 판타지 바이저·형제애·Chapter3 마지막 “잘.했.어.” 유지. Character ID·연령·정확한 합류 시점/조건·현재 시력·눈 부상 정도는 계속 TBD. 기존3인 전투·폴/플레이어 구분·폴/미엘 Slow Burn·세린 역할 보호, Unity/Save/Character 아트 변경0. 마지막 관련 commit은 이 항목을 포함한 `Docs: 태온 친형 하렌 이름 정식 확정`. 아래 과거 기록의 이름 승인 대기는 이 확정으로 대체한다. Push0.

---

## 2026-10-10 StarterVillage v3 — NEEDS_ART_CORRECTION / 미통합

[v3 기술 검수](StarterVillage_Renewal_Art_v3_Inspection_20261010.md): ZIP CRC/PNG/환경15종 규격·매핑 PASS, v2 대비 문1종 변경/14종 동일. 상단 y10/x53~75 틈은 해결됐지만 하단 문 y114/x52~76의25×1px 투과 공백이 문 우선/벽 우선 모두 남아 FAIL. Unity 환경 교체0/300·비활성0/2, Scene/Prefab/Generator/Save/기능 수정0, 런타임 QA NOT_VERIFIED. 다음: 하단 접합 아트 수정 후 재검수; 최종 미술 USER_ART_REVIEW_PENDING. 마지막 관련 commit은 이 항목을 포함한 `Docs: StarterVillage v3 하단 접합 결함 검수 기록`; 이전 관련 `c3722da`. 기존 변경 보호·화면 조작/Push0.

---

## 2026-10-10 Main21 「돌아온 온기」 — DESIGN_DRAFT / IMPLEMENTATION_PENDING

[정식 기획 초안](../03_스토리/Chapter2_Main21_돌아온_온기.md)에 사용자 제공13목표·30~45분 목표(미검증)·게임플레이 중심·기존몬스터 전투A/B·세 지점 근거 판단 퍼즐·스토리5/자유편성 전투3·동료별 활약을 기록했다. 접근 가능한 보급 마차1종(접이식 램프/고정 공간/동료 좌석·휠체어2대)·자동 귀환/Skip·아트 미제작을 명시했다. 형 이름 하렌(Haren)은 가칭/USER_APPROVAL_PENDING, 형제 갈등 해결0·잘.했.어.는 Chapter3 결말 전용이다.

GitHub main/LOCAL HEAD 관련10문서 blob 일치 확인. 기존 형 등장→북쪽 징후 순서는 최신 초안의 징후11→형 첫만남12→북쪽준비13으로 참조 정리하고 Main20 구현 완료를 우선했다. ID/목표유형/좌표/조합/보상·형 이름 승인/정확 합류·대사/음악/최종 아트는 TBD. 팀별 후속 목록·문서 연결/검수 완료, 실제 Unity/Play/C#/Quest/Prefab/Sprite/BGM/TTS/Save 변경·제작0. 다음: 상세 사용자 승인 후 아트·구현·별도 QA, 승인 없이 구현 시작0. 마지막 관련 commit은 이 항목을 포함한 `Docs: Main21 돌아온 온기 정식 기획 초안 기록`; 이전 구현준비 commit `6ba8f49`. 기존 변경 보호·Push0.

---

## 2026-10-10 Main21 자동 귀환 마차 — 기획 전제 확정 / 구현 준비 완료·미구현

아르벨→초보 마을 Main21 전용 일회성 자동 귀환·Yes/No·Skip·항상 접근 가능한 마차1종/최대 휠체어2대·스토리5명/전투3명 유지 전제를 [구현 준비 문서](../03_스토리/Main21_자동귀환_마차_구현준비_20261010.md)에 정리하고 Chapter2/메인 Story 정본에 연결했다. 기존 Scene/Dialogue/Quest/Spawn/Save/Input/BGM을 파일 대조했으며 Arbel 전용 연출 구간→안전한 마을 Spawn 전환을1순위 권장했다.

제공자는 레온1순위 후보, 도착은 기존 남문 복귀 좌표를 참고한 전용 Spawn 후보, 주행10~15초 목표. 정확 NPC/좌표·Objective/Dialogue·최종 아트/방식은 미정이다. 수락 체크포인트·도착 배치 뒤 진행·중복 Skip·저장 실패/Continue 복구·아트/placeholder·격리 QA 계획까지 기록. Unity/Script/Scene/Asset/Save/음원/placeholder 수정·생성0·화면 조작0. 링크/문서 차이 검증 완료, Runtime/Compile/RenderTexture NOT_VERIFIED. 다음은 미정 계약과 마차 아트 확정→실제 구현/QA. 마지막 관련 commit은 이 항목을 포함한 `Docs: Main21 접근 가능한 자동 귀환 마차 구현 준비`이며 이전 관련 정본은 `0278d41`·`c3722da`. 기존 사용자 변경 보호·Push0.

---

## 2026-10-10 StarterVillage v2 — NEEDS_ART_CORRECTION / 미적용

ZIP CRC·PNG20 디코드·환경15종 치수/SHA/Manifest PASS, v1의 약14px 지붕 틈·길 Left/Right 매핑 해결. 그러나 실제 벽·문64px 겹침에서 문 상단23×1px/하단25×1px 배경 투과가 양 건물·양 그리기 순서에 남아 필수 접합 FAIL. [수정할 PNG·정확한 픽셀·확대 증거·전체 합성](StarterVillage_Renewal_Art_v2_Inspection_20261010.md). 기술 PASS/최종 미술 승인은 대기(`USER_ART_REVIEW_PENDING`).

Unity Import/Scene/Prefab/Generator 변경0·무료 Sprite 교체0. 기존 Scene284/남문 활성16+비활성2 유지, Unity/Tools/UserData3537파일·Save/설정·원본 ZIP SHA 동일. MCP Bootstrap/EditMode/포커스false·Compile/Import idle·ConsoleError0 읽기 확인, 화면 조작/Play/추가 Editor0. 새 Import/RenderTexture/NPC 가독성/남문·Save·Quest Runtime 회귀는 NOT_VERIFIED. 다음: 문PNG 조립공백 수정본 재검수→PASS 후 전용Art/Generator 통합과 별도 회귀. 구현 commit 없음; 마지막 관련 commit은 이 항목을 포함한 `Docs: StarterVillage v2 접합 재검수와 적용 중단 기록`. 기존 Git 변경 보존·Push0.

---

## 2026-10-10 팀 공통 인물·형제애·바이저 — 기획 보완 / 미구현

플레이어와 고유 동료 폴 구분·Main21 일행5/전투3·세린의 지속적인 탐험/판단 역할·태온의 경험 기반 자기 판단과 패턴 익히기·형의 제한적 마력 바이저·마지막 **잘.했.어.**를 [팀 공통 정본](../04_등장인물/동료_형제애_바이저_팀공통정본_20261010.md)에 기록하고 기존6정본을 연결했다. 형 합류/현재 시력/바이저 작동·외형, Main21 상세 목표/협동 전투, 세린 개인 서사 시기는 미정이다.

CompanionDefinitions4의 ID/Path/Job·동료 편성 최대2·패턴 익히기 구현 대조 및 링크49 검증 PASS. 이번 서사 기획은 미구현이며 Dialogue/TTS/Character/Save 변경0. 다음은 미정 상세 설계 확정. 마지막 관련 commit은 이 항목을 포함한 `Docs: 동료 형제애 바이저 팀 공통 정본 보완`.

---

## 2026-10-10 Chapter 3 형제애 — 기획 확정 / 미구현

Chapter3 Main22 시작·태온 친형 시각의 길+투사·과거 보호 중 눈 부상(길 자동 결정 아님)·가족식 반말·보호 책임감과 태온 자기결정권의 갈등을 확정했다. 형이 태온의 판단을 믿고 맡기게 되는 감정 변화와 마지막 직접 칭찬 **“잘.했.어.”**를 정본화했다. Main21 형제1:1 대련안 폐기·기존3인 전투 유지·협동 전투는 검토 방향. Main21 초보 마을 귀환/폴·미엘 Slow Burn/형 등장 유지. [담당별 공유·결말 연출·미정 항목 정본](../03_스토리/Chapter3_태온_형제애_서사_20261010.md).

문서 연결·확정/미정 경계 검토 완료. Unity 구현/Quest·Character Asset/음성·음악 제작0, 기존 변경 보존. 형 이름·나이·현재 시력·정확한 합류 조건·Chapter3 보스/퀘스트 수·Dialogue ID/TTS 제작은 미정. 다음은 별도 상세 설계 확정이며 이번 구현 승인은 없다. 마지막 관련 commit은 이 항목을 포함한 `Docs: Chapter 3 태온 형제애 서사 정본 확정`.

---

## 2026-10-10 개발환경 — Unity 6.6.5f1 MCP 복구

사용자 업그레이드 `6000.6.5f1` 확인. 최초 서버 로그의 `uvx` 명령 미발견으로 HTTP/WebSocket 실패; 기존 실행 스크립트에 DryRun·uvx 검사·UV_PYTHON·Editor 중복 실행 방지 추가. Python3.12.14/uv0.12.18/MCP10.2.0 유지, 기존 바로가기 정확 인수 DryRun Exit0. 사용자가 닫은 Editor를 명시적 허락 후1회 실행해 HTTP8080/Plugin35도구/MCP 상태·Scene·Console 왕복 PASS. Bootstrap clean EditMode·포커스false·컴파일/Import 종료·현재 Error0/Warning0. 게임 보호3457파일 SHA 동일·Save/설정 동일·기존 Git변경 보존, 게임/아트/음성 변경0·Play0·화면조작0·commit/push0.

최초 업그레이드 로그 C#Error0/CS0618 20건(FindObjects계열18·spritesheet2)/구형Importer경고3 기록; 이번에 API/meta 대량 수정하지 않음. [정확한 원인·검증·경고·보호 보고서](Unity_665_MCP_Recovery_20261010.md). 다음: 기존 개발 시작 바로가기 사용, 별도 Unity6.6 Main20/SideQuest Runtime 회귀. 네이티브 Codex 도구 표시 갱신과 게임전체 호환은 미검증. 시작 관련HEAD `485138b`, 이번 변경 미커밋.

---

## 2026-10-09 최신 상태 — Main20 「심부의 거신」 Unity 구현 / 격리 Runtime PASS

**Quest9/9·Field11·Boss2Phase·승리 후 잔해/냉각·귀환 구현 완료.** Main19 완료 후 Field10 서쪽→Field11 진입, 현재 목표 조사/세린 텍스트 대화→거신 전투→9목표 완료까지 연결했다. Boss Lv15/HP1200/Attack30/AG10/BaseEXP140(기존 레벨 차 배율)/Talent40·Loot0, Quest EXP100/Talent80 별도1회. HP600 즉시 전환1회·기존 파동 예고 이행 후 Phase2 주입·용융 강타MeleePhysical 계약 반영. 도망/야수 습격 차단, 기존 Party/Formation 유지.

승인 v3 게임PNG17·기본시트+동기화Overlay16·정적VFX7·환경8(Trace 포함), Crowns 보스/Paths Field11·복귀를 연결했다. 원본17PNG/MP3 byte 동일·신규GUID. 원본Field10/StarterVillage·Side9/Loot7·Save/WAV/TTS 보존. 세린5 TTS_PENDING·무음3, API/WAV 생성0.

검증: 격리 Unity6000.5.7f1 Batch Main20 151·일반 Turn Queue 승리12·Main18 467·Main19 169·Side9 574 assertions PASS(합1373/FAIL0). 실제물리 출구 왕복·Quest9·패배/재도전·보상EXP/Talent 중복 차단·Continue3시점(Party/Inventory/HPMP/Beast/Path/좌표)·5슬롯Version1·카메라3비율/절벽동선 PASS. Loot7 실제 보상 확률경계·수량/판매/중첩, 파수꾼60%/장막 모델 PASS. 최종 격리 compile/Runtime Console Error0·로드Scene MissingScript0. 원본MCP Refresh/compile 후 ConsoleError0, Bootstrap/Edit Mode/포커스false 유지.

[최종 구현·QA·미검증 사항](Main20_Implementation_Final_QA_20261009.md) · [1373 assertion 증거](Main20_Runtime_Results_20261009.txt). 일반전투는Lv15 전사+세린+미엘 구성의 실제Attack19회 승리(HP160/100/56)이며 전직업 난이도 승인은 아니다. 구형Main18/19 검사기의 NPC메뉴/출입구 가정만 격리복사본에서 보정했다. 예상 밖 보호파일 변경0.

다음: 사용자 실제 화면 Ground/Arena·Boss/KO/Overlay/VFX/HUD·충돌 체감, 키보드/마우스/패드 입력, Crowns 최종 청취·전직업 난이도 검토. 기존 파수꾼 전체UI완주 재실행·7종 모든자연접촉 드롭통계는NOT_VERIFIED. v3 미술/Overlay 채택 승인은 완료·추가 아트 재제작 요구0, Main21 상세 구현0. 구현 commit `eaefb0c8b11596a50bc0e81ffa46c8582ad92ccc`; QA 문서 commit은 이 항목을 포함한 `Docs: Main20 최종 백그라운드 QA와 현재 상태 기록` 참조. 이번 GitHub Push0.

---

## 2026-10-09 최신 상태 — Main20 아트 v3 기술 PASS / 사용자 미술 검토 대기

**READY_FOR_USER_ART_REVIEW**. v2의 마지막 FAIL인 Cliff 회색 사각 가이드선 제거 PASS(문제3구간 alpha0·Field11 지면 합성 확인). 게임 PNG17 무결성 PASS, 실제 변경은 Cliff1개·나머지16 byte 동일. Boss1256/314/4×4·공용3행·Overlay16프레임 정렬/KO 소거·VFX7 기술 PASS 유지. 추가 Trace 보존·정본16역할 대응(이름15+Overlay대안1). v3 PNG22=게임17+QA5, text3·CRC/decode 손상0.

사용자 미술 승인/Overlay 최종 채택 대기. Ground/Arena·Idle/KO·VFX 실제 가독성 PARTIAL 유지, 필수 추가 아트 재제작 요구0. [v3 최종 검사·권장 기술 계약·승인 후 구현 순서](Main20_ArtPack_Inspection_v3_20261009.md). 기존 v1/v2 기록 보존. Phase 예고 이행/첫행동·용융강타 분류·EXP배율 권장안을 제시했으나 임의 확정0.

다음: 사용자 미술/Overlay·음악 검토→미결 기술 계약 결정→별도 Main20 구현 지시→신규 Import/Field11 왕복/Quest9·Boss·VFX·Save/BGM 연결→격리 QA. 이번 Unity Import·코드/Scene·Side9/잡템7·Save/PNG/meta/Audio 변경0, 포커스 조작/Push0. LOCAL 시작e75e7b7, 마지막 관련 commit은 이 항목을 포함한 `Docs: Main20 아트 v3 최종 기술 검수와 구현 준비 기록` 참조.

---

## 2026-10-09 최신 상태 — Main20 아트 v2 재검수 / NEEDS_ART_CORRECTION

LOCAL 시작 cad75de. v2 PNG21=게임 RGBA17+QA RGB4, text2·CRC/decode 손상0. 정본 이름15/16(Phase2는 Overlay 대안), 역할 대응16후보+추가 Trace 보존. Boss 공용3행10/11 방출 제거 PASS, Overlay16셀 정렬·KO overlay14/15 소거 PASS. 기술적 채택 가능하며 사용자 대안 승인 대기. VFX7 byte 보존·static 단일512 매핑 PASS. Ground 경계/Arena/Trace 완화 확인, 실제 화면은 PARTIAL.

**NEEDS_ART_CORRECTION**: Cliff_Boundary에 회색 사각 가이드선 잔존(Handoff 충돌), 수정 재납품 필요. Ground 반복 띠·Idle/KO 흐름·전체 Boss 열빛 소거·미술 승인은 별도 검토 대기. [v2 검수·정본16/목표9·후속 기술 미결](Main20_ArtPack_Inspection_v2_20261009.md). v1 검수 이력 보존. Phase 예고 경계/용융 강타 분류/EXP140 배율/Field10↔11/Crowns/Quest·Save 연결을 정리했으며 임의 확정·구현0.

원본 ZIP/PNG·Unity/Side9·Save·WAV/TTS/Audio 변경0, Import/Play/포커스 조작/Push0. 다음: Cliff 수정→재검수→사용자 미술 검토·Overlay 승인→미결 계약 결정→별도 Main20 구현 지시. 마지막 관련 commit은 이 항목을 포함한 `Docs: Main20 아트 v2 재검수와 통합 준비 기록` 참조.

---

## 2026-10-09 최신 상태 — Chapter2 잡템7 등록 / 사용자 미술검토 대기

## 2026-10-09 최신 상태 — Chapter1·2 서브 퀘스트 9종 구현

Chapter1 5/5 · Chapter2 4/4 등록, 현재 인벤토리 수집·확인 납품·정확 차감·EXP440/Talent173·동시 의뢰·NPC 명시 선택·HUD/QuestLog/추적 연결 완료. Arbel QuestLog 연결 보완. 기존 Main 완료 로직·몬스터·잡템7·Main20·사용자 Save/아트/Scene 보존.

검증: 격리 Unity6000.5.7f1 Batch Runtime9/9 실제 NPC 수락·거절·납품·Bootstrap Continue, 일부 수량/판매 감소/보상 실패·중복 차단/5슬롯 Version1 PASS. 최종 compile error0·Runtime console error0·로드3Scene Missing Script0. Main01~19 목표 서비스와 Main18 규칙 PASS; 전체 실제 전투·이동 완주 및 물리 입력/시각 승인은 미검증. 격리 검색 인덱스 초기화 오류는 격리 검색 시작 옵션 해제 후 재검증 통과.

[정식 기획·EXP 가정](Chapter1_Chapter2_SideQuest_Design_20261009.md) · [최종 QA·증거·미검증](Chapter1_Chapter2_SideQuest_Final_QA.md). 다음: 사용자 UI/키보드·패드/전투 수집→납품 확인, Main 완주 회귀와 실제 EXP 동선 측정. Chapter2 종료Lv16~17 보장 아님. 관련 commit 7a78aa8 / 14327fd, 마지막 QA commit은 이 항목을 포함한 `Chore: 서브 퀘스트 9종 격리 Runtime QA 기록` 참조. GitHub Push0.

---

아이콘7 기술PASS, ItemDefinition7/7·몬스터LootEntry7/7. 확률/성공1개/중복입력보상방어/stack99/판매/구매차단/저장JSON 서비스검증 PASS, Console error0. 기존3315파일 중 몬스터7 lootEntries만 변경, Save/기존PNG/meta·사용자변경 보존. [상세 정의·검수·구현파일·검증](Chapter2_Loot_Items_20261009.md).

USER_ART_REVIEW_PENDING. 실제UI·Continue·Main17~19 Runtime 회귀 미검증. 별도 잡템Handoff 원문 미발견으로 Manifest설명 사용·대조 후속. 시작commit0483167; 마지막 관련commit은 `Feature: Chapter2 잡템 7종 및 몬스터 드롭 등록` 참조. Push0·Main20 구현 미착수 유지.

---

## 2026-10-09 최신 상태 — Main20 아트 납품 검수 / NEEDS_ART_CORRECTION

ZIP PNG17+Manifest1, 정본16개 basename 정확일치7·이름/방식 대응후보9·추가Trace1. PNG decode/CRC PASS, 기본Boss1256/314/4×4/16셀·셀경계 PASS. Skill/Hit 공용3행 frame10/11 방출표현은 정본 충돌로 수정 필요. Phase2 완성시트 미납품·overlay 대안/동작 정합 확인 필요, VFX7은512px static1frame 매핑 대상. Trace는 흔적목표 대응 가능, cooled는Rift01 대응·Ground 반복경계는 사용자 검토 대기.

**NEEDS_ART_CORRECTION / USER_ART_REVIEW_REQUIRED / Unity 통합 미착수**. 검수보고서·CURRENT_STATUS만 갱신, 원본 ZIP/PNG·Unity Asset/Scene/C#/meta/GUID/Save 변경0·Import/Play/API/Push0. 다음은 공용3행 수정·Phase2/파일별칭 매핑 확인→재검수/사용자 시각 승인→별도 Unity 구현. [전체 검사·정본16매핑·목표9·통합 준비](Main20_ArtPack_Inspection_20261009.md). 시작/직전 관련commit9920f5c; 이번 마지막 관련commit은 이 항목을 포함한 `Docs: Main20 아트 납품 검수와 Unity 통합 준비 기록` 참조.

---

## 2026-10-09 최신 상태 — Main20 정식 설계·Art/Audio/TTS Handoff 완료 / 미구현

Main20 「심부의 거신」 / main_20_colossus_of_the_depths / Field_11_DeepCore 「열맥 심부」 / Main19 선행·목표9. 열맥 거신 Veinfire Colossus / veinfire_colossus: Lv15 HP1200 Attack30 Agility10·Boss EXP140/Talent40·Quest EXP100/Talent80·IsBosstrue·Flee/Pet불가·Loot없음. HP50% 첫 전환1회·고정4/5행동·공용 Burn/과열 유지. 세계 최종 원인 확정0·Main21 상세 확정0.

DESIGN_CONFIRMED / IMPLEMENTATION_PENDING. 정적 전체 NEEDS_IMPLEMENTATION_EXTENSION: Phase/예고/HUD·새Scene/Quest/Encounter·BGM 매핑 필요. 예고 중 Phase 전환 cursor·용융 강타 사거리·기본EXP/최종지급 경계를 임의 결정하지 않음. [정식 설계](../03_스토리/Chapter2_Main20_심부의_거신.md) · [QA](Main20_Design_QA.md).

[Art Handoff](Main20_Art_Handoff.md): Field11 환경·Boss4×4/16프레임·Phase2 대응안·VFX7·KO, ART_PENDING/USER_ART_REVIEW_REQUIRED. [Audio Handoff](Main20_Audio_Handoff.md): Paths 재사용·Crowns 원본 실재/177.815458초/MP3/44100Hz/stereo·decode PASS·Unity 동일byte0,IMPORT_PENDING/USER_LISTENING_REQUIRED. [TTS Manifest](Main20_TTS_Manifest.csv): 세린5 TTS_PENDING·Player2/지문1 무음·Schedar 유지.

이번 문서9개만 변경. 게임코드0·Unity Asset0·Audio Import/MP3복사0·TTS API/PNG/WAV생성0·SaveVersion변경0. 보호: Main18 성공15·잔여7·전체청취 미완료·UNITY_APPLIED0·성공15 재생성 금지, Main19 TTS_PENDING13·생성0 유지. Unity 실행 검증은 이번에 수행하지 않았으며 실제 Art/음악 청취·Boss 난이도/Runtime QA는 후속 구현 후 확인한다.

다음 권장: Main20 Art Handoff를 아트 담당 세션에 전달하여 정식 Boss/Field11/VFX 제작→미결 기술 계약 확인→별도 구현·백그라운드 QA. TTS 제작 우선순위 Main18→Main19 보호. 시작/직전 관련 commit6a4ccc3, 이번 마지막 관련 commit은 이 항목을 포함한 `Docs: Main20 심부의 거신 정식 설계 및 제작 Handoff` commit 참조. GitHub Push0.

---

## 2026-10-09 최신 상태 — Main18 TTS Day1 quota 중단 / 잔여7

전체22·성공15(Story01~14/16)·실패2(Story15/17)·미호출5(Story18~22)·API시도17. **UNITY_APPLIED0 / 사용자 전체청취 미완료 / 성공15 재생성 금지 / 다음 재개 정확히7 / Main19 TTS 생성0·미시작**. 원본 제작 단계이며 Unity Voice 복사·Catalog·Dialogue 연결은 전체22 완성과 사용자 직접 청취 승인 전 금지.

관측 quotaId `GenerateRequestsPerDayPerProjectPerModel-FreeTier`, quotaValue10·retryDelay 반환. Key 개수가 아닌 Project + Model의 실제 quota 상태로 계획한다. Story15 quota 실패 뒤 Story16/17 시도 이력은 보존하며 앞으로는 첫 quota 실패 즉시 중단. reset 시각·Key 소속 Project·현재 남은 quota는 미확정.

문서 검증: Manifest22 / checkpoint17=성공15+실패2 / WAV15 hash·ID 대응 / 잔여7·15+7=22 확인. 기존 Manifest·checkpoint·성공WAV 보존, 이번 API0·WAV변경0·Unity변경0·코드변경0. [재개7 Dialogue ID·증거·운영기록](TTS_FreeTier_Quota_운영_기록.md#다음-재개-대상--정확히-7개) · [장기 정책](TTS_음성_제작_정책.md#free-tier-quota-운영-규칙).

다음 권장: 실제 quota/checkpoint 확인→Main18 잔여7→전체22 존재 확인→사용자 전체 청취→필요한 재녹음→승인 후 별도 Unity 적용→quota에 맞춰 Main19 TTS13. 원본·checkpoint는 현재 LOCAL 미추적 자료이므로 후임에게 별도 전달 필요. Unity 사용자 확인은 승인 후 적용 작업에서 수행. 시작/직전 관련 commit `528f205`; 이번 기록의 마지막 관련 commit은 이 항목을 포함한 `Docs: TTS Free Tier quota 운영 규칙과 Main18 재개 상태 기록` commit 참조. GitHub Push0.

---

## 2026-10-08 최신 상태 — Main19 Art13 정식 반영·연출 기술 PASS

납품ZIP13PNG+Manifest 일치·원본13 byte 보존·새GUID/meta13·Null0. Field10 Ground/흔적/암벽/능선/심부 차단 정식 배치·조사GroundPulse1회·Witness 전 실루엣숨김/목격표시+먼flare1회·Continue 복원. **SOURCE_RECEIVED/AUDITED/IMPORTED/RUNTIME_VERIFIED**, 기술PASS_WITH_NOTE13·FAIL_BLOCKING0·미적 **USER_ART_REVIEW_REQUIRED13**. ART_PENDING 납품 대기는 해소, 최종 미술 승인 대기는 유지. 가이드 선/반복 경계/실루엣 식별·거리감/전체 색감 사용자 검토.

비포커스 격리 Play **314PASS/FAIL0**: Quest12·A/B 정상승리·각도망/전멸재도전·중복보상0·실제Trigger Field09↔10·Continue5시점·Silhouette Gate/VFX중복0·BGM유지·Bounds. CompileError0/RuntimeConsoleError·신규Warning0/MissingScript0. SaveVersion1·Party/Formation/Beast·Main18과열/기존Monster/Voice/BGM/Sprite·사용자Save/meta보호. baseline3301중3299byte동일·승인Scene/Flow2만변경. Main19 TTS_PENDING13·Main18 TTS_PENDING22, Main07/Main17 Voice CLOSED 유지·API0. Bootstrap EditMode/비포커스복구.

다음 권장: [원본/배치 Art 검토](Main19_Art_Import_QA.md)→승인 의견에 따른 후속 수정, NPC13 TTS 제작·직접 전체 청취 승인 후 별도 적용. 마지막 관련 구현commit `294b81e`, 원본등록 `3d477d6`; QA는 이 항목 포함 최신Docs commit. [통합QA/변경목록](Main19_Final_QA.md)·[Witness](QA_증거/Main19/Witness_1280.png). GitHub Push0.

---

## 2026-10-08 최신 상태 — Main19 정식 설계·기초 구현 PASS

Main19 「타오르는 맥동」 / `main_19_burning_pulse`, Main18 선행·12목표·EXP80/Currency70. Field10 「맥동의 열맥」 21×15 임시환경·Field09 왕복·서쪽 Main20 폐쇄. Story A 갑충+도마뱀, B 갑충+망령(후열), 기존 Monster 수치/과열/냉각/정화 보호. 탐색 Paths of Cracked Earth·전투 Blade and Gambit 기존곡 연결. Witness/Hook·Path5 정의. 신규 NPC13 **TTS_PENDING**, Player9+지문1 무음; API0. Art8종(균열 장식2변형)+VFX2 **ART_PENDING / USER_ART_REVIEW_REQUIRED**, 기존 그림 임시 재사용.

비포커스 격리 Play **169 PASS/FAIL0**: 실제12목표·A/B 정상승리/도망/전멸 재도전·보상 중복0·실제 Continue4시점·Party/Formation/Beast/소지품/HP·MP/성장 보존·3화면비 Bounds·MissingScript0. CompileError0/RuntimeConsoleError·신규RuntimeWarning0. SaveVersion1·사용자 저장/설정·기존 Main01~18/Voice/Sprite 보호, 기존3275파일 중3270 byte 동일·승인5만 변경. Main07 작별5/Main17 신규14 CLOSED, Main18 TTS_PENDING22 유지. clean Bootstrap EditMode 복구.

사용자 확인/다음 권장: 최종 Art 전달·실루엣/동선/난이도 체감 검토, NPC13 TTS 제작→직접 전체 청취 승인→별도 Unity 적용. Optional 실제 전투/다른 Path·Party·직업 전체 Play는 후속 QA. 마지막 관련 구현 commit `52259d5`; 정식 설계/최종 QA는 이 항목 포함 최신 Docs commit. [정식설계](../03_스토리/Chapter2_Main19_타오르는_맥동.md) · [최종QA/변경파일목록](Main19_Design_QA.md) · [Art](Main19_Art_Handoff.md) · [TTS23](Main19_TTS_Manifest.csv). GitHub Push0.

---

## 2026-10-08 최신 상태 — 폴 작별 Voice5 CLOSED

사용자 직접 청취 USER_LISTENED_PASS5/5·승인원본5 byte복사·신규GUID5·Catalog250→255/기존prefix보호·Source SHA/PCM/Unity전sample5/5오차0. 실제작별6페이지/Voice5·Player무음·모든5Clip자연종료후page유지·명시Next1page·같은프레임E/A/E재열림0·Main07완료·기존Main08Trace01 “잠깐만요.” 실제재생·BootstrapContinue2회·VoiceMixer100/40/0/Mute복원 **143PASS/FAIL0**. 전체40페이지/Character38resolve/Player2무음, 기존33Voice변경0/Main08변경0/API0. **TTS_REQUIRED5→0 / USER_LISTENED_PASS5 / UNITY_APPLIED5 / RUNTIME_VERIFIED5 / 이번 작별 누락 CLOSED**. 기존 별도Main07 청취 대기는 승격하지 않았다.

CompileError0/RuntimeConsoleError·신규Warning0/MissingScript0, 기존CS0618컴파일경고16 별도. 사용자Save·Sprite·Portrait·Battle·Growth·Retry·Main17/18 byte보호. cleanBootstrap EditMode·비포커스·격리Save/Settings/Input/Listener복구. 추가 작별Voice 청취요청0. 다음권장: 별도 요청의 기존 미확인목록, Main18 TTS_PENDING22 유지. 마지막구현commit `4c9a889`; QA는 이항목포함최신Docs commit. [최종QA](Main07_Paul_Farewell_Final_QA.md) · [적용표5](Main07_Paul_Farewell_Final_Matrix.csv). GitHub Push0.

---

## 2026-10-08 Main07 폴 작별 ID 누락 수정 / TTS_REQUIRED5

사용자 Play의 작별 Character Voice 무음 원인은 폴5줄 Stable ID 누락. main07_paul_supp_001~005 추가·Player “혼자 가시려고요?” 무음 유지. 전수40페이지/Character38/기존resolve33/추가ID누락0. **WAV 미제작·Unity Catalog 미적용·TTS_REQUIRED5, 실제 Voice 문제 OPEN**. [정본Handoff5](Main07_Paul_Farewell_TTS_Handoff.csv)·[QA](Main07_Paul_Farewell_QA.md).

실제6페이지 text fallback/명시Next1page/같은프레임E/A재열림0·Main07완료·기존Main08재진입/Trace01 “잠깐만요.” 실제재생·자연종료page유지·BootstrapContinue2회 73PASS/FAIL0. CompileError0/RuntimeConsoleError·신규Warning0/MissingScript0. 기존WAV·Catalog·Main08변경0/API0/사용자변경byte보호. 사용자 다음 확인은 TTS담당 제작 후 신규5 전체 청취 승인, 이후 별도 Unity 적용·검증 후 CLOSED. 마지막 구현commit `4a9e4c7`, QA는 이 항목 포함 최신Docs commit. GitHub Push0.

---

## 2026-10-08 Main17 승인 TTS14 적용·Runtime 검증 완료

사용자 직접 청취 **USER_LISTENED_PASS14/14**. 원본 byte 복사14·Catalog236→250(prefix보호)·SHA/PCM/Unity전sample14/14오차0. Default/Hearing 실제12목표·조건부Paul/Miel/Taeon 유무·자연종료14 후page유지·Next/Scene/Battle잔류0·정상승리/afterVoice·실제Continue5지점·Mixer100/40/0/Mute/복원 PASS. 기술고유646PASS/FAIL0, CompileError0/최종ConsoleError·Warning0/새Warning0/MissingScript0. **Main17 TTS_PENDING14→0, USER_LISTENED_PASS14·UNITY_APPLIED14·RUNTIME_VERIFIED14**. Main18 TTS_PENDING22·생성/적용/변경0, API0·가공0·기존Voice예상밖변경0.

기존3253파일 중3252byte보호·의도한Catalog1수정·삭제0, 사용자 미커밋 변경 보호. 추가 Main17 Voice 청취 요청0; 기존 음악/실물입력 및 Main18의 별도 미확인 항목 유지. 다음권장: 사용자 지정 후속 확인 또는 별도 승인 Main18 음성 작업. 마지막 관련 구현 commit `f0f44b3`, QA는 이 항목 포함 최신Docs commit. [최종TTSQA](Main17_TTS_Final_QA.md) · [적용14표](Main17_TTS_Applied_Matrix.csv). cleanBootstrap EditMode·비포커스·격리저장/설정/Listener복구. GitHub Push0.

---

## 2026-10-08 Main18 「검은 열기」 구현·자동 QA 완료

정식13목표/Field09 흑요석 상흔21×15/보고 냉각약3·아르벨 판매/독립 과열·HUD·VFX/갑충·감시자 Elite/BGM/무음 Player·NPC TTS_PENDING22 구현. Main19·Cleanse II·새 음성 생성0. 실제13목표 연속 진행·두 전투 도망/패배 재도전·정상승리·중복0, BootstrapContinue7지점 보존, 5Job×2전투·5Path관찰 PASS. Runtime 고유 500PASS/FAIL0, CompileError0/최종ConsoleError·Warning0/새Warning0/MissingScript0, Source17PASS_WITH_NOTE·핵심FAIL_BLOCKING0. 기존3156파일 중3144byte 동일·의도한12수정·삭제0, 기존 사용자 변경 보호.

사용자 직접 확인: **Art17 USER_ART_REVIEW_REQUIRED**(흰 원본 guide/반복 지면 경계 포함), **BGM2 USER_LISTENING_REQUIRED**, 실물 입력1. 자동 시각/청취 PASS로 승격0. 다음 권장: Art 검토→승인 원본 교체·BGM 청취→별도 승인 TTS; Main19 별도 요청. 마지막 관련 구현 commit `6361b00`(Art `543e82b`, 과열 `f72d907`, 설계 `2e6e86c`); QA는 이 항목을 포함한 최신 Docs commit. [최종QA](Main18_Final_QA.md) · [Art](Main18_Art_Import_QA.md) · [Balance](Main18_Balance_QA.md). 사용자 화면 보호, clean Bootstrap Edit Mode·비포커스 복구. GitHub Push0.

---

## 2026-10-07 Main05 태온001 WAV 혼입 복구 / 재감사 완료

사용자 실제Play의 무입력 추가발화는 기존001 WAV 안에 대표보고002 문장이 섞인 원인. 원본PCM 6.60초 무음경계에서001만 복구(10.55→6.60초), 정상5/원본/GUID/Catalog/게임Dialogue·Quest·Input 보호·TTS0. Voice6 새전사/nativePCM6/6, 실제Main04→Guard6→독립대표8→정식해금→Main06첫경계 304PASS/FAIL0, page2자연종료후5초무입력 유지/002자동재생0/1입력미엘page3/대표자동연결0/실제Continue3회 PASS. CompileError0/최종RuntimeConsoleError·Warning0/신규Warning0/MissingScript0·기존컴파일Warning16. QA첫Scene위치누락오류2건은 별도보존 후fixture수정, 최종오류0.

복구001 사람청취 **USER_CONFIRMED_REPAIRED**(사용자: “해당 두 문장만 나오고 끝도 자연스러움”), Main05 USER_LISTENING_REQUIRED0. 다음권장: 보호된별도후속요청. 보호baseline3497중2개변경/3495byte동일·Main07재도전/동료성장/미엘/Early7/Paul20와사용자Save보호. 이전Main05CLOSED는 당시이력이며001anchor판정을이번의미PASS로재사용0. [정본QA](Main05_Guard_Voice_Reaudit.md) · [Voice6](Main05_Guard_Voice_Reaudit_Matrix.csv) · [검증JSON](Main05_Guard_Reaudit_Verification.json). 마지막 관련WAVcommit `b96e8b3`, QA는 이항목을포함한최신 `Docs: Main05 태온001 혼입 재감사 및 Runtime QA` commit 참조. GitHub Push0.

---

## 2026-10-07 Main07 재도전·동료 성장·미엘 PCM 복구 완료

Main07 전투목표에서도 폴 위치의 E/F/A 재도전을 허용, 첫대화 반복0·승리후차단. 필수DefeatEncounter10 전수 재도전/단일승리/Continue PASS(Main11 보스도망금지). 동료 EffectiveLevel=PlayerLevel, 고유값+기존직업식/수호자·치유사 동료만 승인된 공통+1 적용, 일반·임시Story공유·동료레벨Save0·Player성장보존. 미엘기존3 rawPCM복구(6.94/3.63/4.72초), 태온3변경0·신규TTS0.

Main06→07 Early7/PaulFirst→도망·패배재도전→정상승리→MielMeeting→완료Continue, 별도도망재도전정상승리 PASS. 고유label431PASS/FAIL0(반복기록617), 실제33PCM오차0, CompileError0/RuntimeConsoleError·Warning0/기존컴파일Warning16/새Warning0/MissingScript0·관련9Scene0. Lv1/3/5/10/20/50×4, EXP레벨업/실제BootstrapContinue/수동Party·Formation·Beast·HP/MP보존 PASS. 기존3274중6변경/3268byte보호, Early7/Paul20/Main03~06/Main17/244/원본/사용자Save보호. cleanBootstrap EditMode·비포커스·격리Save/Settings/Input원복.

사용자 확인: 복구미엘3 청취PENDING(자동의미청취PASS로승격0), 기존실물입력/물리Collision 별도. 다음권장: 보호된미청취목록·별도후속요청. [통합QA](Main07_Integrated_Retry_Growth_Voice_QA.md) · [전수Retry10](Story_Battle_Retry_Matrix.csv) · [동료24행](Companion_Growth_Runtime_Matrix.csv) · [Voice3](Main07_Miel_Recovery_Matrix.csv). 마지막 관련구현commit `ec6d3e5`(재도전)·`74bea14`(성장)·`907b2e4`(Voice), QA문서는 이 항목을 포함한 최신 Docs commit. GitHub Push0.

---

## 2026-10-07 Main07 Early7 적용 / Paul20 PCM 복구 완료

- Early7 정식Asset/Catalog 연결·RuntimePCM7PASS. Paul20 전수에서묶음/단편/번호밀림확인후기존PCM분리·재배치·Paul008단편결합. 원본/meta/GUID보존. **TTS_REGEN_REQUIRED0 / TTS API0 / 잔여3 소비0**.
- 실제Main06완료→Main07초반연속+Paul20Voice fixture **227PASS/0FAIL**,27PCM오차0,Paul0무입력10초유지/002재생0/Next1회page1.Compile/ConsoleError0·MissingScript0.
- 사용자복구001/002청취PASS. Early7+Paul18 사람청취PENDING25(기술검증과구분), 과거생성API코드미확인. 다음권장: 나머지25청취,동일구batchPaul후속Main09별도감사. 기존Main03~06/Main17/244/UserSave보호. [정본QA](Main07_Early7_Paul_Recovery_QA.md).
- 마지막관련commit `c231565`(Early7),`5ed20c2`(Paul20). QA/문서 최신Docs commit 참조. GitHub Push0.

## 2026-10-07 Main07 초반 ID·입력 경계 수정 / TTS 대기

- NPC7 Stable ID 추가, **TTS_REQUIRED7** handoff/manifest 준비. 사용자 첫 자막 유지 오발화 확인으로 **Paul001 TTS_REGEN_REQUIRED1** 분리. 기존WAV 변경0; 전체 Voice CLOSED 아님.
- 실제 Main06완료→Field02재진입/Main07시작→흔적4→부상자4→Paul0/1 연속 84PASS/FAIL0. 무입력 18.42초page0 유지/002재생0, 명시Next1회page1. custom E/A 같은프레임 재열림 수정. Compile/ConsoleError0·MissingScript0.
- 사용자 확인/다음작업: TTS 담당이 신규7 및 Paul001 재생성 후 전체 청취·승인하고 별도 반영. Paul002 본문청취 미검증1 유지. 기존Voice/244/Main17/사용자Save 보호. [상세QA](Main07_Early_Dialogue_Voice_QA.md).
- 마지막 관련 commit `c8de0ec`(ID7), `89ff828`(Main07입력경계). QA/문서 최신 Docs commit 참조, GitHub Push0.

## 2026-10-07 Main05 승인 신규 Voice5 및 연속 QA 완료

- Main05 신규WAV5 적용, 최신 사용자 청취/자동QA5/5, 전체6매핑·PCM PASS. 문제 **CLOSED**.
- Main04후속9→Guard6→독립대표8→공식해금→Main06Field02첫001, 입력 중복/재열림/경계 및 실제 Save/Continue3회 PASS. Runtime 239PASS/0FAIL, CompileError0, ConsoleError0, MissingScript0. 기존 CS0618경고16 별도.
- 다른Voice·Main17·Battle·Beast·원본·Save·GUID 보존, NEEDS_LISTENING244 유지. [상세QA](Main05_FinalVoice_QA.md).
- 사용자 직접 추가 확인 필수 없음(승인 원본 및 자동 검증 범위). 다음 권장: 보호된 기존 미청취 목록을 별도 작업에서 진행.
- 마지막 관련 구현 commit `c67d632`(WAV5), `f2d7e0c`(기존 입력수정). 최신 QA/문서 commit은 이 항목이 포함된 `Docs: Main05 Voice Dialogue 최종 QA 상태`를 참조. Push0.

## 2026-10-07 Main05 귀환 보고 감사 / 입력 결함 수정 / Voice 의미 대기

같은프레임 Enter+Space page0→2 및 A 종료→NPC 재열림을 실제 가상장치 입력으로 재현·수정했다. DialoguePresenter 실제입력용 Advance/열림·닫힘 프레임 경계와 InteractionSystem 입력진입점만 최소수정, 대사배열/StableID/Objective/Catalog/WAV 변경0. Main04전투후9→GuardReport6→독립대표Report8→정식합류→Main06첫조사001 한 Play 연속 **221PASS/FAIL0**, 24페이지 기록·Main05 6PCM 원본오차0·단일Source·조기진행/자동다음NPC0. Programmatic Advance 유지.

**전체 Voice 의미 해결은 아직 아니다.** 사용자 Main05의 Main04문장 오발화 증거 유지. 현재 Mapping6PASS와 개별 WAV 의미는 구분: 의미NOT_VERIFIED6, 후속태온2/미엘3 **재생성후보5**, 개별Wrong/확정TTS_REGEN_REQUIRED0(어느 미엘 자막인지 사용자가 기억하지 못함). 후보를 확정 재생성목록으로 자동승격하지 않았다. TTS생성/Clip교환0·NEEDS_LISTENING244 marker 유지·Main04late5/Main06이후/Main17Voice 변경0. [최종 감사·한계](Main05_Return_Voice_QA.md) · [Voice Matrix6](Main05_Voice_Matrix.csv) · [정확본문 후보5](Main05_TTS_Regen_Candidates.csv). 최신 근거는 이 문서이며 이전 일반metadata PASS는 의미판정이 아니다.

현재검증: 최종Compile Error0·기존CS0618 Warning16·이번파일새Warning0, 최종 Runtime Console Error/Warning0·MissingScript0, Bootstrap EditMode/is_focused=false·격리Save/Settings 해제·InputSettings native 정상 및 두 설정값복원. 기존3135파일 중 게임C#2만 변경/3133 byte보호. QA 초기 환경실패는 최종PASS와 구분해 상세문서에 남겼다.

다음권장: Main05 후보5 개별원본청취로 Wrong ID 특정 후 정확본문TTS 재생성/승인원본반영. 실물Keyboard/Gamepad와 물리출구접촉은 이번가상입력/Scene Load 검증과 구분한다. 마지막 관련 수정commit `f2d7e0c`; QA문서commit은 최신local log. GitHub Push0.

---

## 2026-10-07 Main04 후반 음성 5개 최종 반영 완료

현재 Main04 Late Sequence의 재생성 대기 **5→0**. 미엘009/010/011·태온006/007 모두 **REGENERATED / USER_LISTENED_PASS / UNITY_APPLIED / RUNTIME_VERIFIED**. 사용자 전체 원본 청취PASS와 이번 Runtime 기술 검증을 구분한다. 실제 Actor 9페이지·7Voice 전체재생/Next/Portrait/화자/본문, 새5개 PCM 오차0, 완료→Main05/완료전후 Bootstrap Continue, 동일프레임 Next2/단일Source, Player·지문 무음 PASS. **148PASS/FAIL0**, Compile Error0·최종Console Error0/Warning0·Field Runtime Missing Script0. 원본·meta·GUID·Catalog·대사 변경0; WAV 정확히5만 교체. 기존 NEEDS_LISTENING244 marker/Queue219 및 Main17 TTS_PENDING14 유지·일괄PASS승격0. [최종 QA](Main04_Late5_Final_QA.md)와 [현재5개 Mapping](Main04_Late5_Source_Mapping.csv)이 우선하며 아래 이전 Wrong/Suspect/대기5 표기는 역사 기록이다. 음성commit `0e7809a`; Push0.

현재 검증: clean Bootstrap EditMode / is_focused=false / 격리Save·Settings 해제, 기존2356파일 byte보호. 새 검증 관련 Editor helper만 추가. 이번5개에 사용자 추가청취 요구 없음; 실물 입력·Standalone 등 기존 별도 미검증은 유지. 다음 권장: 기존 Queue219 사용자 청취 또는 별도 요청 작업(Main17 TTS14 별도). 마지막 관련 구현commit `0e7809a`, QA/문서commit은 최신local log 참조.

---

> 미래 설계 참고(2026-10-06): [Chapter2 Main12~21·후반 방향](../03_스토리/Chapter2_서부_방향.md), [Chapter3 Main22 시작 방향](../03_스토리/Chapter3_초기_방향.md), [폴–미엘 관계](../04_등장인물/폴_미엘_관계.md)를 문서화했다. **Main18~22/Chapter3 미구현**이며 아래 실제 구현·QA 상태는 유지한다. 코드/Asset/Audio 변경 없음.

## 2026-10-06 Main17 실제 Art / Battle BGM 최종 반영

FUNCTION_COMPLETE / ART_COMPLETE / BGM_COMPLETE / SAVE_COMPLETE / PATH_COMPLETE. 정확한 English.zip 원본9PNG 등록·7실제적용(지면변형2보관), Preview미사용. PNG/ZIP/MP3 원본 수정0. Blade and Gambit는 Chapter2 서부 일반/Story에 적용, 기존 단일Source/Mixer Fade0.20/0.75/0.9·LoopON, Field07/08 탐색곡 복귀·Boss예외 유지. Quest12/보상60·50/Serin·Fox/Party/5Path 및 기존44승리·KO0 보호.

이번 고유 PASS270 / FAIL0 / 자동 NOT_VERIFIED0 / 진행 Blocker0. 기존309/180PASS 별도 재사용. 실제 통합 Story 승리/완료Continue 및 Field07/08 일반 BGM·Fade·3해상도 HUD 캡처 PASS. Transform425/Collider15 예상 밖 변경0, 원본3075 누락0·PNG/WAV 보호. Compile완료/Error0, 기존 CS0618 Warning16·이번 클래스 Warning0, MissingScript0. Bootstrap 비포커스 EditMode·격리Save/설정 해제.

남은 사용자/외부: TTS_PENDING14, 실물 Keyboard/Gamepad/Dungeon USER_INPUT_REQUIRED3, Field08/Battle 음악 USER_LISTENING_REQUIRED2. 기존 장비DF1/Standalone DR1 분리. 다음 작업은 청취·실물/Release 검증이며 TTS/Main18은 별도 요청 전 작업하지 않는다. 마지막 구현 commit Art3b4820a / Audio53ef42c, 상세 [최종 QA](Main17_ArtAudio_QA.md). GitHub Push하지 않음.

---

## 2026-10-06 Main17 TTS 제외 최종 마감 완료

기능·Story·Battle·Save·Path 완료, 확인된 진행 Blocker0. Main17 보상은 사용자 승인 EXP60·탈렌트50으로 최종 확정, 실제 마지막 대화/중복지급 방지/완료Save·Continue/레벨업경계 PASS. 5Job×Lv4/9/12/16×2조우40회+치유4회 모두승리·KO0, 몬스터 수치변경0. 이번 고유180PASS/FAIL0/NV0, 기존309PASS 별도 재사용. Compile완료·ConsoleError0/Warning0·MissingScript0·비포커스Bootstrap/격리Save해제.

외부대기: TTS_PENDING14·환경 ART_WAITING_EXTERNAL·Chapter2 Battle BGM AUDIO_WAITING_EXTERNAL. Main17에 신규Elite 불필요, Main18/Field09/Overheat 생성0. 실제 Scene/import 기반 Art 및 Cue/Mixer/Loop/Fade 계약 Audio Handoff 완료. 사용자확인: Keyboard/Gamepad/Dungeon접촉 USER_INPUT_REQUIRED3, 탐색곡 적합성 USER_LISTENING_REQUIRED1. 일반장비DF1/StandaloneDR1 별도 유지. 다음: 외부납품 이후 해당 교체 영향 QA·청취/실물검증, Main18은 요청 전 시작하지 않는다. 마지막 구현·검증 commit `42640f4`, 상태기록 commit은 최신log 참조. 기존103상태·PNG/WAV/Save 보존·Push0. [최종QA](MAIN17_FINAL_QA.md) · [Balance](Main17_Balance_QA.md) · [Art 인계](Main17_Field08_Art_Handoff.md) · [Audio 인계](Main17_Audio_Handoff.md).
## 2026-10-06 Main17 대형 개발 — Phase1~9 완료

main_17_red_rift / 붉은 균열 / Field_08_RedRift 12목표, Main16 이후 연결·조사·Story Battle·승리·Save/Continue5지점·BGM·Main18 Hook 구현. LOCAL Main15 세린 정식 해금과 저장 Party/Formation/Beast 유지. 통합246PASS/Path120PASS, 고유 CSV PASS309/FAIL0/NV0/USER4. 최종 Console Error0/Warning0·Missing Script0·비포커스 Bootstrap EditMode 복귀. 지형 기본 도형 Placeholder, NPC14 TTS_PENDING. 사용자 확인: 실물 Keyboard/Gamepad/Dungeon/BGM. 일반장비DF1/StandaloneDR1 유지. 다음: TTS 제작·청취 또는 사용자 지정 후속 개발. 기존103작업/PNG/WAV/Save 보호·Push0. 선행 관련commit1783cee, Phase9 hash 최신local log/최종보고 참조. [설계](../03_스토리/Chapter2_Main17_붉은_균열.md) · [최종QA](MAIN17_FINAL_QA.md) · [체크포인트](MAIN17_RESUME_CHECKPOINT.md).
## 2026-10-06 전투 하위 메뉴 개선·통합 영향 회귀 완료

BattleSceneController에서 스킬/아이템 열기 때 기본5명령 GameObject를 비활성화하여 표시·interactable·Raycast·Navigation을 함께 차단, 메뉴/대상/행동동안 유지·취소/뒤로/다음조작명령에서 복원한다. 기존Esc와같은 취소에 패드B도 연결했다. 기존812PASS 재사용(817 CSV/JSON 변경0), 이번고유검증484: **PASS482/FAIL0/NV0/USER2**이며 서로중복합산하지 않는다.

5직업 Skill/Item/무효대상/사용/포커스·뒤로/5회표본/3해상도6캡처, 정상일반공격·Guard·Flee·Victory·Continue, Main03/04저작Dialogue/Portrait/정본Clip→Story2/3인정상승리·복귀·Roster보존, 전멸SafeZone/Continue PASS. 자연Main전체/음성의미청취는재실행하지 않았다. Compile0/ConsoleError0·기존CS0618 Warning16·이번 helper 신규Warning0·cleanBootstrapEditMode/비포커스/격리Save해제. 사용자확인:실물Keyboard/Gamepad/Dungeon접촉 USER3, 일반장비DF1/Standalone재시작DR1·Voice219/Main045유지. 다음권장:잔여실물확인 또는 다음개발, Blocker0. 게임코드1외QA·문서만변경·PNG/WAV/Save/Settings보존·기존103작업유지·Push0. 선행관련5a4d05a, 이번commit최종보고. [계획·전체QA](LIMITLESS_BATTLE_SUBMENU_QA.md) · [고유CSV](LIMITLESS_BattleSubmenu_Cases.csv).

## 2026-10-06 2차 QA 후속 분류 완료

총817 유지·기존812PASS/FAIL0 재사용(재실행0). 남은NV3의 실제상태를 코드/Scene/설정/로컬산출물로 확인해 **DEFERRED_FEATURE1(일반장비장착 미구현), DEFERRED_RELEASE_VALIDATION1(Standalone 재시작), USER_INPUT_REQUIRED3(Dungeon물리접촉·실물Keyboard·Gamepad), NOT_VERIFIED0**로 분리했다. 상태 재분류이며 새 PASS가 아니다. 장비의 Inventory 수량저장/Beast장착저장과 일반장비 장착Save를 구분한다.

Dungeon의 Collider/Default·Untagged/비Trigger/Collision→Battle 연결 및 승리·도망 grace2초·복귀이격1.2 정적 확인, 실제물리접촉 미실행. 저장소 안에 게임Standalone산출물없음·Scene19존재/중복0·현재ConsoleError0. 전체Build/Play/포커스전환0. 사용자 직접확인: 접촉→Battle/도망후재접촉, 실물키보드/게임패드. 확인된개발Blocker0·다음개발가능, Standalone Build/실행/OS재시작 및 실물검증은 Release전필수. Voice219/Main04재생성5 제외·Asset/게임C#/PNG/WAV/Save변경0. 마지막관련QA `feeaa08`, 이번문서commit 최종보고참조·Push0. [잔여·다음개발3단계](LIMITLESS_SECOND_QA_DEFERRED.md).

## 2026-10-06 2차 종합 회귀 RESUME 완료

중단 전750 PASS를 재사용하고 MP0/침묵/Guard·전멸 보존·안전지역 Party·Main16 Hearing/Default·남은 전투 화면을 완료했다. 최신세부체크817: **PASS812 / FAIL0 / NOT_VERIFIED3 / USER_INPUT_REQUIRED2**. Main16 각75내부체크는 총수 중복 합산하지 않는다. 기존 전체QA/15 Skill/8 Dungeon/50 Mapping/Quest103을 처음부터 반복하지 않았다.

실제결함1: Field03/Arbel Party 확정 제한 및 Field03 다음 프레임 닫힘. 기존 허용지역3 정책을 Open/Update/Confirm에 공유하도록 최소수정, 실제 NPC 적용→Save→Bootstrap Continue/유지·취소 및 Field/Battle 차단 PASS. 13대표UI×3해상도39PNG 주요 글자·버튼 잘림 없음. Compile0/ConsoleError0·기존CS0618 Warning16·이번 helper 신규Warning0·종료 cleanBootstrap EditMode/비포커스/사용자Save·Settings보존. 보호3075파일 중Party코드1만변경, 기존PNG/WAV/TTS0.

사용자확인: 실물Keyboard/Gamepad 입력2. 미검증: 실제 장착 상태(미구현), Dungeon물리접촉, standalone OS재시작3. 다음권장: 해당 미검증만 보완, 별도Voice청취219·기존Main04재생성5 유지. 마지막관련Fix `adbbfbb`, 이번QA commit 최종보고 참조. 기존103작업보존·Push0. [전체 QA](LIMITLESS_SECOND_REGRESSION_QA.md) · [Resume](LIMITLESS_SECOND_REGRESSION_RESUME.md) · [체크 CSV](LIMITLESS_SecondRegression_Cases.csv).

## 2026-10-06 전체 Story Voice Semantic Risk 사전 감사

현재 Flow 재추출331 Dialogue / Voice247(Intro18+Story229) / Player Silent42. 최신 사용자·Runtime 의미PASS23(Main03 10·Main04 13)을 SHA/GUID/path 보호하고 Matrix의 태온005 경계PASS 누락을 보완했다. 기존 NEEDS_LISTENING244는 PASS20/기존재생성5/새청취219가 겹치는 관리 범위로 유지한다. 기존 Main04 Wrong1·Suspect4 재생성필요5 변화0, 신규 Wrong/Partial/재생성확정0.

Risk HIGH35/MEDIUM129/LOW83, 새Queue219는 HIGH30/MEDIUM129/LOW60. 저음량18·극단짧은길이12·다문장짧은음성15, PCM/교차Main/교차화자/ID중복·Missing·본문/Mapping drift0. Risk는 청취 우선순위이며 의미판정이 아니다. [전체 감사/임계값/Main별표](StoryVoice_SemanticRisk_Audit.md) · [자연 진행 Queue219](StoryVoice_ListeningQueue.csv) · [위험 구간 우선 Queue219](StoryVoice_PriorityListeningQueue.csv) · [보호PASS23](StoryVoice_Protected_PASS.csv). 다음 권장: Main09(27Voice/HIGH7)→Main11(28/HIGH7) 실제 전체발화·음량 청취, 결과ID를 모아 별도 재생성 요청. 기존Main04 5개는 기존Handoff 별도 유지.

새 Editor 감사529검사/합성10case/독립247WAV·과거PASS23해시 검증PASS. Compile0/Console Error0. Bootstrap Edit Mode/is_focused=false 유지, 포커스/Play전환0. 보호 기존2260파일 변경0; TTS생성·WAV/meta/Catalog/Story/Registry/Mixer/Portrait/게임C# 변경0. 사용자 직접 확인: Queue 음성 의미 청취 미수행. 기존 작업103개 유지·직접 감사/CSV/문서만commit·Push0. 마지막 선행 관련commit `85592e54d3109b1ccbb031c9deba7d1d9509b2bc`, 이번commit 최종보고 참조.

## 2026-10-06 수정 완료 Voice 개발 기록 MP4

새 `F:\Downloads\Limitless_DevJourney_2026-10-06\Limitless_DevJourney_FixedVoice_2026-10-06.mp4` 완료. **5분28.13초 / 1920×1080 / 30fps / H264 / AAC48kStereo / 49.42MiB**. 공식 Recorder GameView+GameAudio 원본으로 Title·Intro·성별/길/직업 및 Sprite Preview·Main03후반·Main04첫조우·실제 전투 승리·결과 버튼 Field복귀를 담았다. 정상 새 격리 게임으로 진행했으며 미검증 구간은 녹화 밖 정상 진행했다. 첫 Main04 패배 후 정상 회복/재도전 승리를 사용하므로 구간 컷이 있으며 무편집 연속 플레이가 아니다.

**Intro001~006 6개 + Character16개(Main03태온008~012, Main04미엘001~007·태온001~004)** 최종MP4 AAC 원본 일치22/22. 실제 전투 BGM도 정합. 기존 사용자 Runtime 청취 PASS 원본만 사용했고 신규 사람청취를 주장하지 않는다. Subtitle/Portrait/Player무음/UI/Voice순서·cutoff 검사 PASS, 전체decode오류0/검은화면0/최종A/V길이차4.33ms/끝Field정상. 실제 스킬·적 행동·정상 명령11회 승리/버튼 복귀, 이후 NPC대화0. 미엘008은 전투후대화라 제외, 미해결5Voice 진입0 및 TTS_REGEN_REQUIRED5 유지.

게임 기능/수치/Quest/강제승리 조작0. 기존Unity관련3,048파일 해시 전부 동일, 임시 도구 제거 및 Package원래바이트복원, 사용자Save/Settings·Sprite·WAV보존. 종료 cleanBootstrap EditMode/컴파일오류0/포커스전환0. 정리 중 ImportWorker충돌2건은 동기Refresh 후 재컴파일완료·ConsoleError0으로 회복 확인했다. GitHubPush/기존영상덮어쓰기 없음.

[전체 녹화 QA](LIMITLESS_DevJourney_FixedVoice_2026_10_06.md) · [최종 AAC23행](LIMITLESS_DevJourney_FixedVoice_2026_10_06_AAC_QA.csv). 사용자 직접 확인: 최종MP4 시청. 다음 권장 작업: Main04전투후5Voice 재생성·별도Runtime청취. 마지막 선행 관련commit `162ae1eeb36f03589306bddadb73891291450954`; 이번문서commit은 최종보고 참조.

## 2026-10-06 Main04 대기 지문 수정 및 전투 후 Voice 5건 재생성 전달

대기 문장 “상황을 살피고 있습니다.”는 `MainQuest04FieldFlow.CreateStoryActor`의 공통 NpcController 설정으로, Dialogue ID·Sequence index·Manifest·Voice entry가 없다. 태온 기본 상호작용에서 이름/Portrait가 붙던 상태 설명을 **`<지문> 상황을 살피고 있습니다.`**로 수정했다. DialogueLine의 명시적 prefix 규칙으로 Character Speaker·Portrait·Voice를 제거하며, 잘못된 Voice ID가 전달돼도 Resolve하지 않는다. Player와 기존 Character 대사는 유지한다. 331 authored Dialogue에는 이 대기 설정이 원래 포함되지 않아 개수는 유지한다.

Main01~16 Matrix 및 Flow의 행위 표현 자동 검색 후보 **6행(고유본문5개)** 중 확정 지문은 **고유본문1개 / NPC설정2곳**이다. 나머지4행은 실제 발화 문맥으로 유지했다. [정식 작성 규칙](../03_스토리/Story_대사_지문_작성_규칙.md)과 [후보 감사](Main01_16_Direction_Candidates.csv)에 근거를 기록했다.

미엘 `main04_miel_supp_009` / AfterBattleConversation index2의 정본은 “저도 볼 겁니다.\n이번에는 순서대로요.”다. Runtime·Manifest·정본 부록 일치. 사용자 Runtime 및 직접 원본 WAV 청취 “원본도 자막과 다름”으로 **WRONG_AUDIO_CONTENT 1건**을 확정했다. 추가 청취 답변 “순서가 뒤죽박죽이다”에 따라 인접 태온006·미엘010·태온007·미엘011 **4건은 SEMANTIC_INTEGRITY_SUSPECT**로 등록했다. 정확한 오발화 문장/개별 Wrong·Partial은 미확정이며 metadata로 의미 PASS를 추정하지 않는다. 실제 Clip/PCM 정합으로 Mapping·Cache 문제 재현0. **TTS_REGEN_REQUIRED 0→5**, 단일5행(Miel3/Taeon2) 전달. TTS 생성·WAV 수정·기존 파일 재배열0, **NEEDS_LISTENING244 유지**.

백그라운드 실제 대기 표시 및 Character→지문→Player 전환, 첫 조우10→전투 전3→Story Battle/QA 승리/실제 결과버튼 복귀→전투 후9 전체 **335개 기술 검사 PASS / FAIL0**. 전투 후7Voice 전체 원본PCM오차0/Player Voice·Portrait0/Next·Portrait 전환·Voice cleanup·UI Wrap/동적높이/본문·Footer 비중첩 PASS. 실제 지문 스크린샷도 prefix 전체·이름/Portrait 없음 확인. 기술 PASS와 위5건 의미 문제를 구분한다. 초기 QA의 단일 Show 창을 Next로 닫는 잘못된 기대는 기존 Esc/Hide 정책에 맞게 수정해 전체 재실행했고 초기 실패는 최종 PASS 수에 포함하지 않았다.

보호 대상 기존 3,046파일 중 변경은 Presenter 및 Main04 Flow 코드2개뿐이다. 새 Editor QA와 meta 추가, Main03/정상Main04첫9/후속004·007·008/태온005/전체WAV·meta·Catalog·Registry·사용자Save·설정 변경0. Compile Error0/Console Error0, 기존 CS0618 경고2개는 이력으로 구분한다. 종료 clean Bootstrap Edit Mode/격리Save 해제/is_focused=false/포커스전환0. 전략전투 조작 검증은 생략했다.

[13페이지 인접 순서](Main04_Direction_Miel009_Adjacent_Sequence.csv) · [전투 후9페이지 WAV/PCM 감사](Main04_Direction_Miel009_Sequence_Audit.csv) · [5행 TTS Handoff](Main04_Direction_Miel009_TTS_Handoff.csv) · [실제 Runtime 로그](Main04_Direction_Miel009_Runtime_Results.txt). 다음은5개 정식본문 새원본 제작 및 청취 후 별도 Unity 교체/연속검증이다. 선행 관련 commit `a5460c8f9843c9a800d628915e1da9872ed67285`, 이번commit은 최종보고 참조. GitHub Push 없음. 아래는 준비 당시 이력이다.

## 2026-10-06 Main04 지문 및 미엘009 원본 불일치 수정 계획

“상황을 살피고 있습니다.”는 MainQuest04FieldFlow.CreateStoryActor의 NpcController 공통 대기 설정이다. Dialogue ID·Sequence index·Manifest·Voice entry가 없으며 태온/미엘 두 Actor가 같은 문장을 공유한다. InteractionSystem→VillageNpcRole의 기본 대화 표시에서 태온 이름과 Portrait가 붙었다. Story 설계 Main04의 현장 관찰 문맥과 Flow의 공통 대기 분기를 근거로 상태 설명 지문으로 분류한다. 정식 표기는 `<지문> 상황을 살피고 있습니다.`이며 공통 DialogueLine 규칙으로 Character Speaker/Portrait/Voice를 제거한다. 문장 형태만으로 다른 대사를 자동 전환하지 않는다.

미엘 `main04_miel_supp_009`는 AfterBattleConversation index2이며 Runtime·Manifest·정본 부록의 “저도 볼 겁니다.\n이번에는 순서대로요.”가 일치한다. 사용자 Runtime 불일치와 이번 직접 WAV 청취 “원본도 자막과 다름”으로 WRONG_AUDIO_CONTENT를 확정했다. 틀린 실제 문장의 정확한 전사는 미확정이다. WAV/Mapping 재배열 없이 TTS Handoff를 작성하며 TTS 생성은 하지 않는다. 전투 후9페이지 전체와 대기 지문을 백그라운드 QA로 검사한다. 기존 정상 Main03·Main04첫9개·후속004/007/008 및 태온005를 보호한다. NEEDS_LISTENING244 유지. 선행 관련commit `a5460c8f9843c9a800d628915e1da9872ed67285`.

## 2026-10-06 Main04 후속 Voice 3개 최종 반영 및 연속 Runtime PASS

`Limitless_TTS_Regen_Main04_PostFirstEncounter_3`의 태온004·미엘007·008을 기존 Unity WAV 경로에 반영했다. 사용자 최종 지시 “맡겠습니다 로 해.”에 따라 태온004 본문은 “또 옵니다.\n제가 앞을 맡겠습니다.”로 유지했다. Runtime·Manifest·Handoff·제작 보고서가 일치하며 본문/Registry/Catalog 변경은 없다.

실제 첫 조우10페이지 종료 → 태온004 → 미엘007 → Player “갑시다.” → Story Battle → QA 전용 승리 처리/실제 결과 버튼 → Field 복귀 → 미엘008 → 태온005 경계까지 연속 실행했다. **221개 기술 검사 PASS / FAIL 0**, 새3개 전체 PCM 원본 오차0. 사용자 이번 답변 **“다 잘 들린다”**로 새3개와 태온005의 의미·전체발화·화자·음량 PASS를 확인했다. **TTS_REGEN_REQUIRED 3 → 0**, **NEEDS_LISTENING 244 유지**. 경계005 WAV는 교체하지 않았으며 NEXT_AUDIT_BOUNDARY_FAIL은 없다.

Player Voice/Portrait0, 태온·미엘 Portrait 복원, UI Wrap/동적높이/본문·Footer·Portrait 비중첩, 자연 종료 후 Next, Battle 전후 잔류 제거와 최종 Cleanup PASS. 전략 전투 조작 및 Main04 전체 Quest 완료 검사는 수행하지 않았다. 경계 페이지의 로그 NEXT 표시는 공통 QA 기록이며 실제 Advance는 호출하지 않고 검증 후 Hide로 종료했다.

보호 대상 기존 3,044개 파일 중 변경은 대상 WAV3개뿐이다. 정상 첫 조우9개/Main03/태온005/meta/GUID/Importer/Catalog/게임 C#/설정/사용자 Save 변경0. 새 Editor QA와 meta만 추가했다. Compile Error0 · Console Error0, 기존 CS0618 경고2개는 별도 이력이다. 종료 clean Bootstrap Edit Mode/격리 Save 해제/is_focused=false, 포커스 전환0.

[Source 및 Unity PCM 감사](Main04_PostFirstEncounter_Final_Source_Audit.csv) · [연속 Runtime 결과](Main04_PostFirstEncounter_Final_Runtime_Results.txt) · [전체 Sequence](Main04_PostFirstEncounter_PreSelfCare_Sequence_Audit.csv). 다음 권장 작업은 범위 밖 Voice의 별도 청취 감사다. 선행 관련 commit `83d0b4fd684434a219d1c1a6f23d9445f6e4a0fa`, 이번 완료 commit은 최종 보고에 기록한다. GitHub Push 없음. 아래 준비 단계는 당시 이력이며 현재 판정은 이 단락을 따른다.

## 2026-10-06 본문 충돌 해소 및 새 Voice 3개 적용

사용자가 최종 답변 “맡겠습니다 로 해.”로 태온 004의 기존 정본 “또 옵니다.\n제가 앞을 맡겠습니다.” 유지를 지시했다. 중간 “막겠습니다” 의도는 이 지시로 대체되었다. Runtime·Manifest·Handoff·새 제작 보고서 본문이 일치하며 본문 변경은 없다. 새 WAV 3개를 기존 경로에 반영했고 연속 Runtime 검증 대기다. 의미 해결 및 TTS_REGEN_REQUIRED 3→0은 이번 Runtime 청취 확인 이후 판정한다. 첫 조우 9개/Main03/태온 005/meta/GUID/Catalog/Registry 보호.

## 2026-10-06 Main04 후속 Voice 3개 적용 준비

새 폴더 `Limitless_TTS_Regen_Main04_PostFirstEncounter_3`에 대상 3개가 제작되어 있다. 미엘 007·008은 Unity 반영 대기다. 태온 004는 사용자가 이번 답변에서 정식 본문을 “또 옵니다.\n제가 앞을 막겠습니다.”로 변경하는 의도를 확인했다. 현재 로컬 Flow·Manifest·Handoff와 새 제작 보고서는 “맡겠습니다”이므로 MANIFEST_RUNTIME_DRIFT에 앞서 사용자 최신 의도와 기존 정본의 충돌이 확인되었다. “막겠습니다” 원본을 준비하기 전 태온 004를 적용하거나 의미 PASS 처리하지 않는다.

문서화 → 구현 → 검증 순서로 진행한다. 첫 조우 정상 Voice 9개, Main03 전체, 종료 경계 태온 005 WAV, GUID·meta·Importer·Catalog·Registry를 보호한다. 연속 Runtime 완료 전 TTS_REGEN_REQUIRED 3 및 NEEDS_LISTENING 244를 유지한다. 선행 관련 commit `83d0b4fd684434a219d1c1a6f23d9445f6e4a0fa`.

## 2026-10-06 Main04 첫 조우 이후 SelfCare 경계 직전 Voice 3건 감사

직전 FirstConversation 10페이지 / 9 Voice의 사용자 Runtime PASS를 유지한다. 감사 시작은 `main04_taeon_supp_004` (EncounterConversation index 0)이다. 종료 경계는 `main04_taeon_supp_005` (AfterBattleConversation index 1), 태온의 “본인부터 보셔야 하는 것 아닙니까?”이며 **HANDOFF_INCLUDED=NO**이다. 범위는 EncounterConversation 3페이지 → Story Battle → AfterBattleConversation index 0까지다. 총 Dialogue 4개, Voice 3개(미엘 2 · 태온 1), Player Silent 1개이며 전투는 Dialogue 수에 포함하지 않는다.

사용자가 실제 Runtime에서 이 범위의 전반적인 자막/발화 불일치를 확인했다. Voice 3개를 **SEMANTIC_INTEGRITY_SUSPECT / TTS_REGEN_REQUIRED 후보**로 일괄 등록했다. 각 WAV의 정확한 오발화 내용과 Wrong/Partial 개수는 개별 청취 증거가 없어 미확정이다. Metadata 정상만으로 의미 PASS를 확정하지 않는다. 기존 WAV를 재배열하지 않고 현재 Runtime 본문으로 새 원본을 제작한다. 재생성 목록은 **0 → 3**, 기존 NEEDS_LISTENING 관리 범위는 **244**로 유지한다.

[단일 3행 TTS 전달](Main04_PostFirstEncounter_PreSelfCare_TTS_Handoff.csv) · [전체 4행 Sequence 및 원본 감사](Main04_PostFirstEncounter_PreSelfCare_Sequence_Audit.csv) · [해시/PCM 교차 비교](Main04_PostFirstEncounter_PreSelfCare_CrossComparison.csv). 이번 작업의 WAV, Unity 코드, Catalog, 본문, Main03 및 정상 Main04 Voice 변경은 모두 0이다. TTS API 실행, Editor/Play 재실행, 포커스 전환도 없다. 마지막 관련 commit은 `ca103570983e18711184f4d63eaa5e8e396a2056`이며 이번 문서 commit은 최종 보고에 기록한다. 다음 작업은 이 3개 원본 제작 → 청취 채택 → 별도 반영 및 연속 Runtime 검증이다.

## 2026-10-06 Main04 첫 조우9개 새Voice 반영·전체Runtime 의미PASS

원본 `Limitless_TTS_Regen_Main04_FirstEncounter_9` 미엘6 Sulafat·태온3 Gacrux,제작원본9건USER_LISTENING_PASS.기존WAV9개전체바이트교체/Path·meta·GUID·Importer·Catalog·Speaker·본문유지. **사용자 이번 “9개 모두 Runtime 청취 PASS”** 확인으로전체9건 RUNTIME_LISTENING_PASS 확정.001 두문장 같은Line/다른위치이동0/한칸밀림0/Main03재등장0/다른화자0/부분누락0. **TTS_REGEN_REQUIRED9→0**,NEEDS_LISTENING244관리범위유지.

실제Main04 MielActor.TryInteract→FirstConversation10페이지(Voice9/Player1)전체연속검증 149 assertions PASS/FAIL0,TRACE9/NEXT10별도.9Clip전PCM원본오차0/SourceUnitySHA동일·Registry/Speaker/Manifest일치·Catalog247 ID/Clip중복0/Missing·Null0.각Clip자연종료후수동Next/Source정리·정확한다음Clip·Miel/TaeonPortrait/Player Voice0·Portrait0/Wrap·동적높이·본문/Footer/Portrait비중첩·최종Panel/Source/Portrait종료PASS.다음Main04전투Objective·격리자동Save읽기/첫대화완료countPASS.첫대화완료이지Main04전체Quest완료검사는아님.

[최종QA](Main04_FirstEncounter_Final_적용_QA.md)·[9개Source/Unity/PCM감사](Main04_FirstEncounter_Final_Source_Audit.csv)·[이번연속Runtime로그](Main04_FirstEncounter_Final_Runtime_Results.txt).보호 3044파일중변경은WAV9+기존EditorQA1(결과경로/캡처프레임대기/격리Save진행검사)뿐.게임C#/Main03전체12WAV/meta/Catalog/Save/Settings/Scene/Packages불변.CompileError0·최종ConsoleError0/Warning0,재컴파일기존CS0618경고2건은이력보존.종료cleanBootstrapEditMode/audit해제/Play옵션·설정복원/is_focused=false,포커스전환0.

다음권장:이번9건추가TTS불필요,244관리범위나머지Voice별도청취.마지막선행관련commit `4b1d2e1a8b3722e7a226add5862756ac2485a978`,이번완료commit은최종보고참조.원본폴더Stage0/직접변경만commit·GitHubPush0.아래는이전이력이다.

## 2026-10-06 Main04 첫 조우 Voice9개 전체 재생성 전달 확정

사용자 실제Runtime:001 첫문장만정상/두번째문장이다른위치에서발화,이후미엘·태온Voice전반적자막순서불일치/Main03태온처럼들리는발화재등장.9건모두 **SEMANTIC_INTEGRITY_SUSPECT / TTS_REGEN_REQUIRED**,001은기존직접WAV청취로 **PARTIAL_AUDIO_CONTENT** 추가확정.나머지각파일의틀린정확한문장/Character를추측하지않는다.이전Runtime Playback/PCM/148검사PASS는기술증거로유지하고의미PASS로사용하지않는다.

LOCAL FirstConversation10페이지/Voice9(미엘6 Sulafat·태온3 Gacrux)/Player1 정상무음.원문을Flow에서재추출하고현재Manifest와줄바꿈포함9건일치,본문Drift0.기존WAV재배열/Mapping수정없이9개전체정식본문재제작방향확정. **현재LOCAL 재생성필요1→9**(사용자요청의0→9는이전001등록전기준;001중복등록없이전체9). NEEDS_LISTENING244관리범위유지/Main03해결상태보호.

정본전달 [9행 Handoff](Main04_FirstEncounter_TTS_Regen_Handoff.csv),[WAV9개감사](Main04_FirstEncounter_WAV_Audit.csv),[Main03비교원본목록](Main04_FirstEncounter_Main03_Comparison_Inventory.csv),[교차비교](Main04_FirstEncounter_CrossQuest_Comparison.csv),[Main04내부비교](Main04_FirstEncounter_Internal_PCM_Comparison.csv).현재Main03 WAV12개와9×12=108쌍/첫조우내36쌍비교.동일SHA/PCM cross duplicate 0,내부PCM duplicate 0.내용이유사한발화는해시불일치여도배제할수없어사람청취증거와구분한다.

WAV/TTS API/Unity코드/Catalog/정식본문/Player/Main03Voice변경0.이번Editor/Play재실행0/포커스변경0,문서화→감사·전달생성→정적검증.다음:이9행으로온전한원본제작→개별청취→별도승인된반영작업에서기존GUID보존교체·연속Runtime검증.마지막관련commit `3ab8e67a71fc684d55796b012aade22ea943ad07`,이번문서commit은최종보고참조.아래는이전감사이력이다.

## 2026-10-06 미엘 첫 조우 전체10페이지 감사 / 원본001 누락 확정

[전체 QA](Miel_First_Encounter_Voice_QA.md)·[순서10행](Miel_First_Encounter_Sequence_Audit.csv)·[확정 TTS전달](Miel_First_Encounter_TTS_Handoff.csv).Main04 FirstConversation,미엘6/태온3/Player1.문제 main04_miel_supp_001 정식본문 `조금만 참으세요. / 출혈은 멎었습니다.` 중사용자Runtime와직접WAV모두첫문장만청취:CaseA PARTIAL_AUDIO_CONTENT.2초원본전체PCM48000오차0/자연종료/자동Next·조기Cleanup재현0.이번TTS생성/Playback수정/WAV수정0,정확한전체본문으로재생성전달.

연속148assertions PASS/FAIL0,9Voice전PCM정합/Player Voice0·Portrait0/MielPortrait·UI/최종Cleanup·첫대화종료→다음Encounter Objective·격리Save읽기PASS.의미정상확정0/Partial확정1/나머지8 USER_LISTENING_REQUIRED;Metadata정상으로의미PASS추정금지. **TTS_REGEN_REQUIRED0→1**,NEEDS_LISTENING244기존관리범위유지,Main03태온008~012완료보호.Compile/최종ConsoleError0·Warning0.3042기존파일해시변경0,새EditorQA만추가/cleanBootstrapEditMode/포커스전환0.

다음:미청취8건전체발화확인→확정누락001원본재제작→사용자청취/기존GUID보존교체·회귀.마지막관련commit `5e660dba9fac846932eb3e075735a22e19ff6fa9`,이번감사commit은최종보고참조.직접변경만커밋·GitHubPush없음.아래는이전이력이다.

## 2026-10-06 Main03 008~012 최종 반영·연속 Runtime 사람 청취 PASS

008/010/011 새 원본·009 **NeFix 최종본**을 기존 Unity WAV 내용만 교체.012는 지정 원본과 이미 동일하여 재작성0/해결유지.5건 Source 사용자청취PASS + 이번 사용자 **“5개 모두 Runtime 청취 PASS”** 확인: 의미·전체문장·음량PASS,009 `네.` 포함.**TTS_REGEN_REQUIRED4→0**, NEEDS_LISTENING244 기존관리범위유지.331 Matrix에 완료flag 기록.005/006/007/003/004 기존PASS와001/002 보호,Player 정상무음.

LOCAL Runtime/Manifest 본문5건 줄바꿈 포함일치·Source/Unity WAV SHA동일·실제Unity PCM5개 전샘플오차0.실제 첫Battle→QA전용승리종료→정식복귀→008→Player→009→Player→010→011→012→단서001→002→Main03종료 연속PASS.전략전투/물리입력검증 아님.기존 QA 도구로 PlayUnfocused·격리Save/Settings 사용,포커스전환0.기술검사 227 PASS/FAIL0(7 TRACE·9 AUDIO는별도),Player Voice0/Portrait0·태온복원·UI Wrap/동적높이/본문잘림0/Footer·Portrait비중첩·끝까지Next/빠른Next/최종정리/Quest완료/격리자동Save읽기PASS.

[최종 적용QA](Main03_Supp008_012_Final_적용_QA.md)·[5건 Source/PCM 감사](Main03_Supp008_012_Final_Audit.csv)·[이번 연속 실행로그](Main03_Supp008_012_Final_Runtime_Results.txt).보호 3042파일 비교:변경은 Unity008~011 WAV4개뿐,meta/GUID/Importer/Catalog/게임·Editor C#/Save/Settings/Scene/Packages불변.종료cleanBootstrap EditMode/audit해제/격리설정·Play옵션복원.CompileError0/ConsoleError0·Warning0 조회;C#변경없어강제재컴파일0.초기 준비스크립트 실행실패 후 조기QA를중단했고 WAV교체후전체재실행PASS,조기실행은최종증거에포함하지않음.

다음 권장:244 관리범위의미확인Voice를별도청취.이번5건추가TTS불필요.마지막선행관련commit `e355b44c9b2a87b7072d504d95db33560ebf7b7c`,이번완료commit은최종보고참조.직접변경만Stage,원본TTS폴더/다른WAV Stage0·GitHubPush0.아래는이전이력이다.

## 2026-10-06 Main03 008~011 사용자 Runtime 의미 불일치 확정·TTS 전달

사용자 실제 Runtime 청취에서 008~011 모두 자막과 발화 불일치 확인. 네 건 모두 **WRONG_AUDIO_CONTENT / RUNTIME_SEMANTIC_MISMATCH**이며 Metadata 정상 여부로 의미 판정을 되돌리지 않는다. **TTS_REGEN_REQUIRED 1→4**, NEEDS_LISTENING **244는 기존 관리 범위**로 유지(이번 확정 문제/이미 해결된 항목을 포함하므로 미확인 244건이라는 뜻 아님). 012는 새 WAV·사용자 직접 청취·Unity Runtime 의미/전체 문장/정상 음량 PASS 해결 유지. 005/006/007/003/004 PASS와 001/002 기존 상태, Player 정상 무음 보호.

LOCAL MainQuest03FieldFlow.AfterBattleConversation 및 DialogueLine의 구형 화자 접두어 제거 규칙에서 전체 표시 본문을 추출한다. 현재 Manifest와 네 건 모두 줄바꿈까지 일치. 실제 AfterBattle 페이지 순서는 1/3/5/6(index 0/2/4/5); Player 페이지를 제외한 전달 4행이다. 009는 `네.` 뒤 줄바꿈을 보존한다. Actual Spoken Text는 **USER_CONFIRMED_MISMATCH**로 기록하며 잘못된 문장을 추측하지 않는다. 이전 008 직접청취의 별도 증거는 보존한다.

정본 전달: [4행 Handoff](Main03_Taeon_Supp008_011_TTS_Handoff.csv), [원본/참조 감사](Main03_Taeon_Supp008_011_Voice_Audit.csv), [공통 연기/설정/검증](Main03_Taeon_Supp008_011_TTS_Handoff_QA.md). 기존 7행 Remaining Handoff는 원본 청취 감사 목록이며 새 재생성 전달에는 이 4행만 사용한다. 문서화→감사/전달 생성→백그라운드 정적 검증 순서. WAV/meta/Unity 코드/Catalog/설정 변경 및 TTS API/Push 0. 이번 Editor/Play 재실행 없음; 새 음성 제공 후 직접/Runtime 의미 청취가 다음 작업이다.

마지막 선행 관련 commit: `bd4bffeca1efb1789079197f3b215e19ddb0fe48`. 이번 문서 commit은 최종 보고 참조. 아래 기존 기록은 당시 이력이며 현재 판정은 이 단락과 최신 Matrix/재생성 목록을 따른다.

## 2026-10-05 Main03012 최종 반영·후반부 연속 QA

012 새원본7.76초/RMS−16.16dBFS를기존WAV에교체,GUID/meta/import/Catalog보존. 사용자원본/이번Unity Runtime **의미·전체문장·음량PASS** 확인,PCM186240개오차0.012저음량해결.008은직접WAV/Runtime의006계열발화로 **WRONG_AUDIO_CONTENT** 확정,이번실제008 Resolve/Source도008이며MappingFix없음. **TTS_REGEN_REQUIRED2→1(008)**,244 NEEDS_LISTENING원래관리범위유지,후반미청취5종유지. 기존005006007003004보호.

008→Player→009→Player→010→011→012→단서001→002→Main03완료를격리연속검증:무음213/실제재생227 assertions PASS/FAIL0,각AUDIO9/TRACE7별도. Player Voice0/Portrait0,태온Portrait/UI/Next/최종Voice·Portrait정리/Quest완료/격리자동Save읽기PASS. 사용자Save/Settings/포커스변경0. CompileError0/최종ConsoleError0·Warning0;기존deprecated2/MCP연결경고1이력보존. 새기능/게임C#변경0.

상세 [적용·최종 QA](Main03_AfterBattle_FinalVoice_적용_QA.md). 다음작업:008정확한 “역시 이상합니다.” 원본제공→청취/교체/회귀,009010011001002는일괄의미청취. 마지막관련구현commit `06666354eeaf9e2c328b058895751872b405ef88`. 최종문서commit은보고참조. 아래는이전감사이력이며최신판정은이단락을따른다.

## 2026-10-05 Main03 008 의미 오류 원인 확정

008 WAV 직접청취와 실제 Runtime 모두 사용자 확인 “그냥 돌아다니는 것 같지만…” 계열. 기대 “역시 이상합니다.”와 불일치: **Case B WRONG_AUDIO_CONTENT / RUNTIME_SEMANTIC_MISMATCH**. 실제008 페이지 Resolve/AudioSource는008이며006과 Path/GUID/reference/hash가 모두 다르다. 기존 재생PASS는 기술 결과이며 의미PASS 해석을 철회한다. C#/Mapping/WAV 변경0. 올바른008 새 TTS 대기. 확정 재생성 **2건(008 의미 오류 +012 저음량 별도)**; 의미 청취 대기6종. 기존244 관리범위 유지. 정상005/006/007/003/004 보호. 기존 Player Voice0/Portrait0 결과 유지. 사용자 Play/Scene/Save/포커스 변경0. Console Error0/Warning0 조회 확인, C# 변경 없어 강제 컴파일 없음. 상세: [008 원인 QA](Main03_008_VoiceIdentity_원인_QA.md). 이전 관련 commit9a82ff8, 이번 commit은 최종 보고 참조.

아래 이전 감사는 당시 기술 검증/미청취 이력이며008 최신 판정은 위 결과를 따른다.

# Project-Limitless 현재 개발 상태

## Main03 후반부 전체 Voice 감사·원본012 저음량 전달 (2026-10-05)

- 문서화→집중 QA 구현→백그라운드 검증. [후반부 감사QA](Main03_RemainingVoice_감사_QA.md)·[순서9행CSV](Main03_Taeon_Remaining_Sequence_Audit.csv)·[일괄Handoff7행](Main03_Taeon_Remaining_TTS_Handoff.csv). “역시 이상합니다.”=008/AfterBattle index0. LOCAL 실제순서는005~007→003/004→첫Battle→후속008~012/Player→단서001/002→Main03완료이며 요청의 “008이후 첫전투” 순서와 차이를 보고/기존Story 유지했다.
- 후속9페이지(Voice7/Player2) 전수: Mapping/Missing/재현PlaybackBug/자동TransitionCut0.008 Clip/IsPlaying/IsAudible/PCM/정상RMS−21.63 확인, 사용자 보고무음은 격리조건에서 미재현·근본원인 미확정.012 원본RMS−51.90/Peak−32.62dBFS·1.45초 저레벨 결함 확정, **TTS_REGEN_REQUIRED0→1**. 모든WAV/meta/Catalog/게임code수정0·TTSAPI0.
- 사용자 이번 묶음 재생은 듣지못함.7개 USER_LISTENING_REQUIRED, 다른6개 Wrong/Truncated 추정확정금지. Handoff는012확정재생성1+다른6의원본 의미확인으로 구분.005/006/007/003/004 이전사람 RuntimePASS 유지, NEEDS_LISTENING244 관리범위 유지.
- 실제Main03 전체16페이지: 최종무음202assertions/실제재생214assertions PASS·각PCM측정9/FAIL0. 실제Battle진입→QA승리종료/정식결과버튼복귀→후속전체/단서/Quest완료·Player Voice0/Portrait0·태온Portrait·UI/Footer비중첩·빠른Next/끝까지Next정리PASS. 전투조작/전략 검증 아님. 후반부7WAV 모든PCM샘플오차0.
- Compile Error0/최종 Console Error0·Warning0, 기존CS0618 2건이력보존. 보호3040기존파일해시변경0·사용자419Git항목보존·cleanBootstrap EditMode/격리설정복원/포커스전환0. 다음권장: Handoff7개 원본을한번에청취하여006같은오발화 여부확정→012정상레벨 온전한원본제공→기존ID/meta보존교체·회귀. 직접변경만커밋·GitHubPush없음. 아래는이전이력이다.

위 후반부 감사 helper/증거 보고 관련 commit `d743227`. 후반부 최종 문서는 별도 Docs commit으로 기록한다.

## 태온 후속003/004 Unity 반영·최종 사람 Runtime PASS (2026-10-05)

- 문서화→기존 WAV2개 전체바이트 교체→격리 검증. 입력 `Limitless_TTS_Regen_Main03_Taeon_Supp003_004/`·[적용/집중 QA](Main03_Supp003004_Regen_적용_QA.md).003 0.96초/RMS−19.73dBFS·004 4.36초/RMS−18.95dBFS, Unity PCM 모든 샘플 원본과 오차0. 기존 GUID/meta/Import/Catalog/Registry/Story/게임 C# 유지.
- METADATA_PASS·원본 USER_LISTENING_PASS·실제 Unity **RUNTIME_LISTENING_PASS** 모두확정. 사용자 “003·004 모두 의미 일치·끝까지 재생 PASS” 답변. **확정 TTS_REGEN_REQUIRED2→0**,005~007 기존 해결3→0 유지. NEEDS_LISTENING244 관리범위 유지(이번 해결2건은 원래 그부분집합이며 완료 flag 추가, 미확인 전체244라는 뜻 아님). 다른 음성 청취/재생성0.
- 무음83assertions/실제재생88assertions PASS·각 PCM측정2·FAIL0. 실제005→Player→006→Player→007→003→004→Battle, 끝까지 대기후 수동Next·자동발화절단0·Player Voice0/Portrait0·태온복원·현재UI 원문/Portrait/Footer 비중첩·Battle 잔류0. 명시적 조기Next는 기존skip정책대로 정리하며 Timing/Quest/Save 수정0.
- Compile Error0/최종 Console Error0·Warning0, 기존 CS0618 2건 재컴파일 이력 보존.331 Matrix/Catalog247·Missing0/중복0/Speaker·Manifest본문 mismatch0. 보호3041중 기존변경은 WAV2뿐·다른WAV/meta/005~007/Catalog/Mixer/Save/Settings/Scene/Packages/사용자419Git항목(새Source4포함)보존. clean Bootstrap EditMode·격리 설정복원·포커스전환없음.
- 다음 권장: 기존244청취 관리범위에서 이미PASS한003/004를 구분하며 나머지 원본을 별도 순서로 청취. 이번2건 추가TTS 필요없음. 직접변경만 커밋·GitHub Push 없음. 반영/집중 QA commit `1f9fa8f`, 최종 문서는 별도 Docs commit. 아래는 이전 이력이다.

## 태온 후속 Voice 원본 결함 확정·장문 Dialogue 수정 완료 (2026-10-05)

- 문서화→공통 Presenter 최소 수정→백그라운드 검증. [적용 계획/Voice·UI QA](Main03_FollowupVoice_DialogueLayout_계획_QA.md). 사용자005/006/007 실제 Runtime 의미 청취 PASS로 기존 TTS_REGEN_REQUIRED3→0 해결. 세 WAV 재수정 없음. 새 전투 전003/004 두 줄 모두 문제라는 추가 확인을 반영했다.
- 003(“옵니다.”) 원본1.10초 RMS−61.60/Peak−39.04dBFS,004(“제가 앞을 막겠습니다. 뒤를 부탁드리겠습니다.”) 원본1.65초는 사용자 청취에서 다른/부분 문장. 모두 G. OTHER 원본 결함이며 Manifest/Catalog/Clip/GUID/Speaker 정상. 실제 재생 완료 전 자동 전투 전환 없음. 새 TTS 생성/가공/Mapping·Quest·Save 수정0, [다음 제작 전달2건](Story_Dialogue_TTS_REGEN_REQUIRED.csv)을 기록했다. 현재 재생성2/NEEDS_LISTENING244(새2건 포함), 아직 음성 해결 완료 아님.
- 기존 Legacy Text/Font/CanvasScaler 유지. Speaker/Body/Footer 분리, Wrap·내용 기반 높이(최소23%/최대40%)·본문28~24·NPC Portrait/Player 전체 폭 정상. 상한을 넘는 본문은 Mask와 스크롤로 끝까지 접근하며 원문 축약0/Player Portrait 생성0이다.
- 격리 PlayUnfocused550 assertions PASS/FAIL0 + PCM측정2건(로그 count552): 실제005→Player→006→Player→007→003→004→Battle/Voice 정리와 Player Voice0/Portrait0,3해상도(1920×1080/1600×900/1280×720)×22 Layout·선택창 비중첩, 현재 Story 최장10개·24개행 스크롤 마지막줄 접근 확인. 대표 배경 캡처 시각 검수. 실제 물리 입력장치는 미검증. Compile Error0/최종 Console Error0·Warning0, 기존 CS0618 2건·MCP 연결 경고 이력 보존.
- 보호3041기존파일 중 변경은 Presenter/기존 Editor QA2개뿐. WAV/meta/Catalog/Mixer/Save/Settings/Scene/Packages/사용자415Git항목 보존. clean Bootstrap EditMode·격리 설정/해상도 복원, 화면 포커스 전환 없음. 다음 작업은003/004 온전한 원본 제공→실제 청취→기존 ID/meta 보존 교체·회귀. GitHub Push 없음. 관련 커밋은 완료 기록 참조. 아래는 이전 이력이다.

마지막 재컴파일: Compile/Console Error0·기존 CS0618 Warning2(ExternalAssetImportEditor 53/59행). QA 종료 당시 Warning0과 구분한다. UI/집중 QA commit `284cbf2`.

## 태온 첫 조우 Voice3개 교체 진행 (2026-10-05)

- 입력 `Limitless_TTS_Regen_Main03_Taeon_005_007/005.wav·006.wav·007.wav` 원본/사용자 실제 청취 PASS, PCM24kHz/mono/16bit·4.04/7.16/6.52초. [적용 계획과 집중 QA](Main03_Taeon_Voice_Regen_적용_QA.md)을 먼저 기록하고 기존 Unity WAV3개만 전체 바이트 교체했다. GUID/meta/Importer/Catalog/게임코드/Mixer/다른Voice 유지, 실제 Unity PCM과 새 원본 모든 샘플 차이0. 백그라운드57checks/실제 재생45checks PASS, Compile/최종Console Error0/Warning0. **사용자 Runtime 의미 청취 확인 대기**, TTS_REGEN_REQUIRED3/NEEDS_LISTENING244 유지. 격리 QA는 Bootstrap으로 종료했고 이후 사용자 Field01 Play/Save 진행은 중단·되돌림 없이 보존한다. GitHub Push하지 않는다.
- 관련 WAV교체/집중 QA commit `191e817`. 남은 완료 조건은 사용자 Runtime 청취 결과 확인이며 확인 후에만 재생성 필요3→0으로 해결 기록한다. 기존 전체331개Runtime 증거와 이번 집중57/45검사를 구분하며 전체Story를 다시 재생하지 않았다.

## Story Dialogue 전수 감사·Player 중앙 정책 적용 (2026-10-05)

- 문서화→구현→격리 백그라운드 검증. [전수 QA](Story_Dialogue_Consistency_QA.md)·[Matrix](Story_Dialogue_Audit_Matrix.csv)·[LOCAL 원문 부록](../03_스토리/LOCAL_Story_Dialogue_원문_부록.md). 전체331페이지(Intro18/Main01~16 313), Player42, Voice247(Story229+Intro18). 기존 문서 exact23/본문미기재308을 구분해 구현 원문을 명문화했으며 기존 게임 기획/발화 본문 변경 없음.
- 구형 Main03의 Player4페이지가 태온 ID/Portrait를 사용하던 원인을 중앙 DialogueLine/Presenter/PortraitCatalog에서 수정. 모든 Player stable ID player·Voice NONE·Portrait NONE, 화자 mismatch4→0·잘못된 Player Portrait4→0. Named Portrait 태온/미엘/폴/세린 정상, 레온 등 미제작은 기존 text-only 유지. 개별 Flow/Save 구조 변경 없음.
- 실제 사용자 청취로 **태온 첫 만남 005/006/007 음성 불일치3** 확인. 006은005의 문장을 말하고 007은006 첫 문장만 말하고 종료. 005 실제 발화 원문은 미확정. Manifest/파일명/GUID/해시는 맞아도 원본 발화가 틀린 상태다. [TTS_REGEN_REQUIRED3개](Story_Dialogue_TTS_REGEN_REQUIRED.csv)와 사용자 증거를 기록. 나머지244개 NEEDS_LISTENING. **음성 정합성 해결 완료 아님**: 이번 TTS 생성 금지 조건에 따라 WAV/Catalog를 그대로 보존했으므로 현재 잘못된 원본 연결은 남아 있다.
- 기술 Mapping247/247·Manifest본문 mismatch0·Missing/Unexpected Audio0·Duplicate ID/Clip/Branch Collision0. 격리 PlayUnfocused **2574checks PASS/FAIL0**: Story313개 자막/ID/Clip/Portrait 전수, Player42개 정리, NPC↔Player/연속Next/Scene전환/VoiceVolume/Mute, Intro18개참조·Next8회/Skip정리. 전체 Story 수동 플레이/나머지244개 실제 발화 청취는 미실행. CompileError0·최종ConsoleError0/Warning0, 기존 CS0618 이력2건 유지.
- 사용자 승인으로 이전 Play 종료 후 격리 QA, 종료 clean Bootstrap EditMode·is_focused false. 보호3032파일 중 기존 변경은 중앙 UI코드2개만, WAV/meta/Catalog/Mixer/Save/Settings/Packages/Scene 및 기존410Git항목(94그룹)보존. 직접 변경만 Stage/commit, 작업 diff--check 검사, GitHub Push 없음. 계획 commit `af48c2e`, Runtime/QA commit **`08b07ad`**.
- 다음 권장: 정확한 현재 본문으로3개 원본 재제작/제공 → 실제 청취 채택 → 기존 ID/meta를 보존하여 교체 → 격리 회귀 QA. 아직 듣지 않은244개는 별도 청취. 이번 감사에서 잘못된 Voice에 자막을 맞추거나 추측 Shift Mapping하지 않았다. 아래는 이전 이력.

## LOCAL 전체 회귀검증 완료 (2026-10-05)

- [종합 QA](LIMITLESS_CURRENT_FULL_REGRESSION_QA.md)·[보존 실행 로그](LIMITLESS_FullRegression_Runtime_2026_10_05.txt): **21범주 PASS18/FAIL0/NOT_VERIFIED3**. 신규 격리244체크 PASS, Main09 내부115/Main10 내부63 PASS는 wrapper와 중복합산하지 않음. Title 수정 회귀11PASS. 기존 Main15/16·Audio/Voice·Dungeon/Boss QA는 현재 Runtime/데이터 SHA 보존을 확인하고 범위를 명시해 재사용.
- PlayerSprite **READY50/BLOCKED0**, 50 Mapping/Job Preview·800 Sprite·대표남녀 Confirm/World/실제 Bootstrap Continue/Battle Left Idle 정상. PNG/meta/ID/Audio/Catalog/사용자Save/Settings 불변. Main01–16 총16 Quest/103 Objective 참조·순수 규칙 감사와 Main09/10 실제 단계/편성/Continue/Battle/Dungeon 왕복 정상. Main13–15 Path 반응형 retrofit는 계속 설계후보이며 Main16 실제 Path 분기는 구현 상태 유지.
- 실제 최소수정: Renderer2D의 활성 Missing 디버그 리소스6개 정상복원(`1e0a957`), Title 다시보기 버튼 하단 Outline 여백(`80d2f0b`, 1920×1080/1600×900/1280×720 경계·Replay/Next/Skip 정리 PASS). Editor QA의 오래된 미래Quest Seed/석재 재사용/던전 미구현 기대값만 보정(`deb181c`). Quest/Save/Party/Battle 기능·수치 변경0.
- Serialized 검사12242객체/31Scene: Missing Script0, 현재 Runtime 활성 Missing Sprite/Audio/Material/기타 참조0. Build Scene19 Missing0. 과거 Milestone 백업Scene의 카메라 target 누락5와 제거된 Renderer 필드 YAML1행은 실행 범위 밖 이력으로 보존. 컴파일Error0/최종ConsoleError0·Warning0. 재컴파일에서 기존 ExternalAssetImportEditor의 CS0618 경고2건은 별도기록/원본보존.
- 남은 확인: J 스킬15종 실제UI/취소/타깃 전체, N Dungeon B1/B2 일반8조우 전승리, T Party/Pet/Settings 포함 주요 모든UI 다중해상도 시각 QA. 사람 음성청취 품질·모든 물리 입력장치·배포 Player 저장은 미검증. 정식 BGM/Voice 기술연결 정상, SFX 재생은 미구현이며 Chapter2Battle BGM TBD 정책 유지.
- clean Bootstrap Edit Mode·Audit Save/Settings/Play옵션/runInBackground 복원. 임시 Recorder/녹화스크립트 제거 및 manifest/lock 원본복원. 사용자410 Git항목(기존94그룹)보존·직접변경만 커밋·Push 없음. 다음권장작업은 위 J/N/T의 남은 검증이며 새 게임기획/Art 재가공 없음. 아래는 이전 이력.

## Player Sprite low-alpha QA 정책 정정/전체READY50 (2026-10-05)

- 문서화→QA helper구현→재판정/검증. [정책](../11_UI/Player_Sprite_QA_Policy.md)·[Mobility개별QA](../11_UI/Mobility_LowAlphaPolicy_QA.md). 이전non-zero Alpha 일괄차단은가시적오류와저Alpha노이즈를구분하지못한오탐. 가시적Crop/분리조각/침범/방향/장비손실은계속BLOCKED, 작은비가시Alpha1–16노이즈는비차단경고. 큰면적/긴선/반복/실루엣영향은별도검토.
- Mobility9종만재판정READY9·LOW_ALPHA_NOISE186성분(최대Alpha1–16/최대33px/bbox변14px). **전체READY41/BLOCKED9→READY50/BLOCKED0**. HealerFemale 포함기존READY41종 재판정없음/PNG/meta/Entry/QA/기존328Clip동일. PNG50수정0/Stage0·기존144SpriteID/AppearanceID/Save/fallback유지. 기존9Entry에만72Clip추가, 전체400Clip.
- Tools/SpriteQA helper: MaxAlpha/성분면적/bbox/본체거리/반복·경계후보 기록, 시각검수 없는자동PASS금지. 회귀11PASS/0FAIL(A-E,셀내절단/큰면적/긴선/반복/실루엣/빈Frame/검수누락). 게임/Editor C#변경0.
- 이전85PASS Runtime 재실행없음. 최소회귀81Mapping/Clip참조PASS·Preview/World Frame0참조9정상·PlayUnfocused BattleLeftIdle9PASS. Save→Continue는이전실행이력+StableID/정본Mapping/코드불변으로보호하며이번실제전체흐름반복없음. MissingSprite0·현재컴파일Error0/최종ConsoleError0/Warning0.
- 검증중RuntimeBattle해석기를EditMode호출해Destroy오류9건발생/기록. 임시객체9개정리후properPlay검사통과, 게임코드변경없음. cleanBootstrap EditMode·Save/Settings/Play설정복원·포커스전환없음. 사용자410Git항목보존. 직접변경diff--check확인·GitHubPush없음.
- 계획commit `d7b830f`, 반영commit `aabfe3d`. 다음권장: 별도요청시 실제걷기/입력 통합QA(이번미검증). 더필요한Mobility Art수정목록없음. 아래는정정전이력.


## Mobility v2 redownload9종 반영/엄격QA 완료 (2026-10-05)

- 입력 `F:/Downloads/Limitless_Mobility_Path_9_Restored_v2_redownload.zip`. 문서화→원본9PNG교체→검증. PNG9/Male5/Female4/정확9조합·중복0/누락0·512×512RGBA/4×4/16Frame/Cell128. 이미지수정0. [계획](../11_UI/Mobility_RestoredV2Nine_적용_계약.md)·[개별QA/최소수정리스트](../11_UI/Mobility_RestoredV2Nine_QA.md).
- 144Frame 전체시각/Alpha검사: 이전가시적검/장비/바퀴아래조각 모두해소, 머리/휠체어/바퀴/방향/디자인 정상. 하지만사용자StrayPixel0을Alpha>0 고립성분까지적용하면9종에최대Alpha1-16의미세잔여픽셀이남음. **대상READY0/BLOCKED_ART9·전체READY41/BLOCKED9 유지**. 다른41종/HealerFemale은재판정하지않음. 상세Frame/좌표/픽셀수/Alpha를다음이미지세션최소수정리스트로기록.
- 기존9AppearanceID/144SpriteID/meta/import/Gender+Path+Job/Save/fallback보존. 다른41PNG/CatalogEntry/QA·기존328Clip 동일, 신규Clip0. 사용자410Git항목보존. 게임기능C#변경없음, QAHelper대상/대표만조정.
- 격리PlayUnfocused85PASS/0FAIL: 대상9Mapping/Preview, 대표남녀Fighter World/BattleLeftIdle/Save→BootstrapContinue의기존성별fallback유지. BLOCKED원본의실행직접표시는승격전보류. 전체800Sprite Missing0·대상import/rect/pivot 정상. 옛ID재계산/기본fallback 유지.
- 컴파일Error0·최종ConsoleError0/Warning0, 기존CS0618경고2건별도기록. cleanBootstrap/EditMode·Save/Settings/Play설정복원·포커스전환없음. 실제걷기/키보드/게임패드미검증. 작업diff--check확인, GitHubPush없음. 계획commit `e2d3620`, 반영commit `b6b5d35`. 아래는이전이력.

## Mobility 최종 복원10종 반영/QA 완료 (2026-10-05)

- 입력 `F:/Downloads/Limitless_Mobility_Path_10_Restored_Final.zip`. 문서화→원본10PNG교체→검증. PNG10/Male5/Female5/Job5×Gender2·512×512RGBA/4×4/16Frame/Cell128·중복0/누락0. 이미지수정0. [계획](../11_UI/Mobility_RestoredFinal10_적용_계약.md)·[개별QA/최소수정리스트](../11_UI/Mobility_RestoredFinal10_QA.md).
- 160Frame 실제검수: Mobility READY1(HealerFemale)/BLOCKED_ART9, 전체READY40/BLOCKED10→**READY41/BLOCKED9**. 잔여검/장비/바퀴아래 조각은0-basedFrame/셀좌표로기록. 다음이미지세션은해당9종의최소수정리스트만처리.
- 기존10AppearanceID/160SpriteID/meta/import/Gender+Path+Job/SaveMapping/fallback 유지. 통과Entry에8Clip추가, 다른40PNG/Entry/QA 및기존320Clip 동일. 게임기능C# 변경없음, QA helper 대상Path만Mobility로 변경. 사용자411Git항목 보존.
- 격리PlayUnfocused88PASS/0FAIL: 10Mapping/Preview·대표MaleFighter 기본fallback 및FemaleHealer 원본Preview/World/BattleLeftIdle/Save→Continue 통과. 전체800Sprite Missing0. 옛ID재계산/기본fallback 유지. 컴파일Error0/최종ConsoleError0/Warning0. 기존CS0618 2건/MCP재연결경고1건 별도기록.
- cleanBootstrap EditMode·Save/Settings/Play설정복원·포커스전환없음. 실제걷기/키보드/게임패드 미검증. 직접변경diff--check 확인·GitHubPush없음. 계획commit `32f74ef`, 반영commit `e83df1b`. 아래는이전이력.

## Hearing v3 복원10종 READY 반영 완료 (2026-10-05)

- 입력 `F:/Downloads/Limitless_Hearing_Path_10_Restored_v3.zip`, 문서화→구현→검증. 원본10PNG 바이트교체, Male5/Female5/Job5×Gender2·중복0/누락0·512×512RGBA/4×4/16Frame/Cell128 확인. Sharpshooter파일은기존Marksman ID/경로에연결. 이미지/C#기능수정0. [계획](../11_UI/Hearing_RestoredV3Ten_적용_계약.md)·[개별QA](../11_UI/Hearing_RestoredV3Ten_QA.md).
- 새160Frame검수 **Hearing READY10/BLOCKED0·전체READY30/BLOCKED20→READY40/BLOCKED10**. v2 검/지팡이/장식/발/머리위 조각 및SharpshooterFemale Up12/14 머리절단해소, 귀장치/방향정상. 낮은Alpha미세가장자리성분은가시분리파츠와구분하며세부QA에공개. 다음이미지세션최소수정리스트 없음.
- 기존10AppearanceID/160SpriteID/meta/Import·Gender+Path+Job/SaveMapping/fallback 보존. 기존10Entry에만80Clip추가. Mobility 포함다른40PNG/Entry/QA·기존READY30/240Clip동일. Assets/Settings/UserData 변경은10PNG+Catalog+Inventory12파일뿐·모든C#/Save/Settings불변, 사용자94항목(410Git 파일)보존.
- 격리PlayUnfocused88PASS/0FAIL: 10Mapping/JobPreview 및대표MaleFighter/FemaleHealer의최종Preview/World/BattleLeftIdle·실제Battle/Save→BootstrapContinue v3Sprite직접표시통과. CharacterCreation선택전기본Preview·구버전ID재계산/기본fallback통과. 전체800Sprite Missing0·대상160Sprite rect128/pivot64,0/Multiple/PPU128/Point/Uncompressed 정상.
- 컴파일요청완료·Error0/최종ConsoleError0/Warning0, 이전CS0618이력보존. cleanBootstrap EditMode·Save/Settings/Play설정복원·포커스전환없음. 실제걷기/키보드/게임패드미검증. 직접변경diff--check통과, 기존사용자whitespace보존. GitHubPush없음.
- 다음대상준비: 남은Mobility10종 BLOCKED 그대로보존. 원본ZIP 수령후적용계획/QA를시작할준비상태이며이번작업범위에포함하지않았다. 계획commit `dac6a9a`, 마지막 v3 원본반영·READY승격·검증commit bb0c8e8. 아래는이전이력.

## Hearing v2복원10종 원본 반영·QA 완료 (2026-10-05)

- 입력 `F:/Downloads/Limitless_Hearing_Path_10_Restored_v2.zip`. 문서화→구현→검증 순으로Hearing10PNG 원본바이트교체. Male5/Female5/Job5×Gender2·중복0/누락0·512×512RGBA/4×4/16Frame/Cell128 확인. 이미지수정0. [계획](../11_UI/Hearing_RestoredV2Ten_적용_계약.md)·[개별QA/Frame/좌표](../11_UI/Hearing_RestoredV2Ten_QA.md).
- 새160Frame 실제검수에서 **READY승격0/BLOCKED_ART10, 전체READY30/BLOCKED20 유지**. 이전큰발조각/일부머리위선개선. v2잔존/신규: Fighter발밑/셀옆조각, Guardian검파츠, Healer지팡이/머리위선, Mage장식/지팡이조각, Sharpshooter발/머리위선·FemaleUp12/14 머리수평절단. 귀보조장치존재/방향유지확인. 정상음파효과와구분했으며수정판명칭으로승격하지않음. 다음작업은개별QA 좌표의원본Art보정.
- 기존Hearing10AppearanceID/160SpriteID/meta/Import·Gender+Path+Job/SaveMapping·fallback 보존. Mobility 포함다른40PNG/Entry/QA·READY30/240Clip동일. 새Clip0, 사용자94항목(410Git 파일)과Save/Settings 보호. Assets/Settings/UserData 변경은10PNG+Catalog+Inventory12파일뿐.
- 격리PlayUnfocused 88PASS/0FAIL: 10Mapping/JobPreview·대표MaleFighter/FemaleHealer의CharacterCreation/최종Preview/World/BattleLeftIdle·실제Battle/Save→Continue·구버전ID재계산/fallback 통과. BLOCKED이므로표시는기본성별fallback이며신규HearingArt직접표시검증완료로보고하지않음. 전체800Sprite Missing0·대상160Sprite rect128/pivot64,0/Multiple/PPU128/Point/Uncompressed 정상.
- 컴파일요청완료·현재Error0/Warning0·직전CS0618 2건이력보존, 최종Console Error0/Warning0. cleanBootstrap EditMode·Save/Settings/Play설정복원·포커스전환없음. 실제걷기/키보드/게임패드미검증. 직접변경diff--check통과, 기존사용자whitespace보존. GitHubPush없음.
- 계획commit `d9eb01e`, 마지막 v2 원본반영·QA·검증commit c568299. 아래는이전이력.

## Hearing 최종복원10종 원본 반영·QA 완료 (2026-10-05)

- 입력 `F:/Downloads/Limitless_Hearing_Path_10_Restored_Final.zip`. 문서화→구현→검증 순으로Hearing10PNG 원본바이트교체. Male5/Female5/Job5×Gender2·중복0/누락0·512×512RGBA/4×4/16Frame/Cell128 확인. 이미지수정0. [계획](../11_UI/Hearing_RestoredFinal10_적용_계약.md)·[개별QA/Frame/좌표](../11_UI/Hearing_RestoredFinal10_QA.md).
- 새160Frame 실제검수에서 **READY승격0/BLOCKED_ART10, 전체READY30/BLOCKED20 유지**. Fighter발밑/머리위 조각, Guardian셀옆검은파츠, Healer발/이웃지팡이 조각, Mage이웃지팡이/장식조각, Sharpshooter발/머리위/이웃파츠잔존. 정상음파효과와구분했으며수정판명칭으로승격하지않음. 다음작업은개별QA 좌표의원본Art보정.
- 기존Hearing10AppearanceID/160SpriteID/meta/Import·Gender+Path+Job/SaveMapping·fallback 보존. Mobility 포함다른40PNG/Entry/QA·READY30/240Clip동일. 새Clip0, 사용자94항목(410Git 파일)과Save/Settings 보호. Assets/Settings/UserData 변경은10PNG+Catalog+Inventory+QAhelper13파일뿐.
- 격리PlayUnfocused 88PASS/0FAIL: 10Mapping/JobPreview·대표MaleFighter/FemaleHealer의CharacterCreation/최종Preview/World/BattleLeftIdle·실제Battle/Save→Continue·구버전ID재계산/fallback 통과. BLOCKED이므로표시는기본성별fallback이며신규HearingArt직접표시검증완료로보고하지않음. 전체800Sprite Missing0·대상160Sprite rect128/pivot64,0/Multiple/PPU128/Point/Uncompressed 정상.
- 컴파일Error0·기존CS0618경고2건보존, 최종Console Error0/Warning0. cleanBootstrap EditMode·Save/Settings/Play설정복원·포커스전환없음. 실제걷기/키보드/게임패드미검증. 직접변경diff--check통과, 기존사용자whitespace보존. GitHubPush없음.
- 계획commit `5130489`, 마지막 원본반영·QA·검증commit 6b4699b. 아래는이전이력.

## Vision512 최종 복원3종 반영 완료 (2026-10-05)

- `F:/Downloads/Limitless_Vision_3_Restored_512_Final.zip` 지정 Vision Fighter Male·Mage Female·Sharpshooter Male 원본3PNG 교체. 문서화→구현→검증, PNG3/중복0/누락0·512×512 RGBA·4×4·16Frame/Cell128 확인. 이미지 추가 수정0. 직전1254입력 보류 기록과 구분하여 새48Frame 실제QA. [계획](../11_UI/Vision_Restored512Final3_적용_계약.md)·[최신 QA](../11_UI/Vision_Restored512Final3_QA.md).
- **3종 모두 READY 승격**, Fighter 검 끝·Mage Up 머리/지팡이 접합·Sharpshooter Up12/15 머리·Up13 망토 복원 통과. 남은문제Frame 없음. 전체 **READY27/BLOCKED23 → READY30/BLOCKED20**. 이번3종 추가Art 수정 필요 없음, 다음 권장작업은 범위밖 나머지BLOCKED20의 기존QA 수정.
- 기존3 Appearance ID/48Sprite ID/meta/Import·Gender+Path+Job/Save Mapping·fallback 유지. 기존3Entry에만24Clip 연결. 다른47PNG/Entry/QA·기존READY27/216Clip 보존. 사용자 기존94항목(410Git 파일) 보호, Save/Settings/ProjectSettings 불변. gameRuntime 기능/SaveMigration 변경0, QA helper의 대상 범위만3종으로 조정.
- 격리PlayUnfocused90PASS/0FAIL: 3종Mapping/Preview/World/Battle Left Idle/실제Battle Scene/Save→Bootstrap Continue·CharacterCreation 성별기본Preview·구버전ID 재계산/기본fallback 통과. 전체800Sprite Missing0·48Sprite rect128/pivot64,0·Multiple/PPU128/Point/Uncompressed 확인.
- C# 컴파일 완료·Error0. 기존 CS0618 경고2건 보존, 최종Console Error0/Warning0. cleanBootstrap EditMode·Save/Settings/Play 설정 복원·포커스 전환 없음. 실제걷기/키보드/게임패드 미검증.
- 이번 변경 diff --check 통과, 기존 전체작업트리 whitespace는 수정하지 않음. GitHub Push 없음. 계획commit `30a3419`, 마지막 원본 반영·READY 승격·검증commit `39220c2`. 아래는 이전이력이다.

## Vision 복원 최종3종 입력 규격 불일치로 보류 (2026-10-05)

- 입력 `F:/Downloads/Limitless_Vision_3_Restored_Final.zip`, Vision Fighter Male·Mage Female·Sharpshooter Male 지정3PNG/중복0/누락0/RGBA 확인. 문서화 선행 후 검사했으나 **모두1254×1254**로 필수512×512·4×4·Cell128 규격 실패. [계획](../11_UI/Vision_RestoredFinal3_적용_계약.md)·[최종 QA](../11_UI/Vision_RestoredFinal3_QA.md).
- 게임 자산 교체0/승격0, **대상3종 BLOCKED_ART 유지·전체 READY27/BLOCKED23 유지**. 기대 READY30/BLOCKED20 미달성. 기존 문제 Frame: Fighter Male5/7/9/11 검 끝, Mage Female12-15 접합, Sharpshooter Male12/13/15 머리/망토. 새 입력 정식48Frame QA는 규격 불일치로 보류.
- 모든50PNG/meta/Entry/기존216Clip·다른47종·READY27·사용자 기존94항목(410개 Git 상태) 보존. Assets/ProjectSettings/UserData 해시 변경0. 기존 Appearance/Sprite IDs·Import·Mapping·Preview·Save/fallback 코드 변경0.
- Unity 읽기검증 전체800Sprite Missing0, 기존3종16Sprite씩/rect128/Multiple/PPU128/Point/Uncompressed. 새 복원본 Preview/World/Battle/Save Play QA·새 전체 컴파일 미수행. C# 변경0·현재 Console Error0/Warning0·clean Bootstrap Edit Mode, 포커스 전환 없음. 직전 CS0618 2건 보존.
- 다음 필요한 작업: 동일3종512×512 RGBA·4×4·Cell128 원본 ZIP 확보 후 교체/재검증. 직접 문서 diff --check 통과, 전체 diff --check는 기존 사용자 whitespace로 실패하며 보존. GitHub Push 없음.
- 관련 계획 commit `e58f432`, 마지막 게임 반영 commit `dbbfb54`. 이번 형식 불일치 기록은 후속 문서 commit에 포함한다. 아래는 이전 이력이다.

## Intellectual/Vision Restored8 원본 반영 완료 (2026-10-05)

- `Limitless_IntellectualVision_8_Restored.zip` 지정8PNG 원본 바이트 교체. PNG8/중복0/누락0·512×512 RGBA·128px×16 확인, 이미지 수정 없음. [계획](../11_UI/Intellectual_Vision_Restored8_적용_계약.md)·[8종 개별 QA](../11_UI/Intellectual_Vision_Restored8_QA.md).
- **READY승격5 / 대상 BLOCKED_ART3, 전체 Ready22/Blocked28 → Ready27/Blocked23**. READY: Intellectual Fighter Female·Mage Female/Male, Vision Guardian Male·Sharpshooter Female. 잔존: Vision Fighter Male5/7/9/11 검 끝, Vision Mage Female12–15 머리/지팡이 접합, Vision Sharpshooter Male12/13/15 Up 머리/망토. 다음 권장 작업은 이3종의 정확한 QA 문제Frame Art 보정.
- 다른42종 PNG/meta/Entry/QA/기존176 Clip 보존. Intellectual Fighter Male READY·Hearing/Mobility/EmotionalScar 불변. 기존8 Appearance ID/128 Sprite GUID/fileID·Import·Gender+Path+Job/Save Mapping 유지. READY5종 기존 Entry에만40 Clip 추가. 사용자 Save/Settings 보존, Runtime 기능/Save Migration 없음.
- 격리 PlayUnfocused86PASS/0FAIL: 대상8 Mapping/Preview·READY 대표 Intellectual Mage Male/Vision Guardian Male의 실제 Restored World/Battle Left Idle/실제 Battle Scene/Save→Bootstrap Continue 통과. 전체800 Sprite Missing0·geometry 오류0. QA helper 대표 조합3줄만 변경, C# 컴파일 완료·Compile Error0. 기존 CS0618 경고2건, 최종 Console Error0/Warning0.
- clean Bootstrap Edit Mode·Save/Settings/Play 설정 복원·포커스 전환 없음. 실제 걷기·키보드/게임패드 미검증. 직접 변경 staged diff --check 통과·기존 사용자 변경/whitespace 보존. GitHub Push 없음.
- 관련 commit: 계획 `24e72d0`, 마지막 원본 반영·READY 승격·검증 `dbbfb54`. 이 상태 기록은 후속 문서 commit에 포함한다. 아래는 이전 이력이다.

## Intellectual/Vision 8종 셀 픽셀 정밀 수정 완료 (2026-10-05)

- Cleanup v2 원본 기준8종만128px 셀 단위로 정밀 수정. 생성형 재작업/보간/리사이즈/방향 변경 없음. 정상84셀 RGBA Hash·Pixel Diff0, 허용44셀 중41셀 수정·34셀 정수1–2px 이동, 전체87셀 그대로 보존. [계획](../11_UI/Intellectual_Vision_CellCleanup_적용_계약.md)·[개별 QA/남은 프레임](../11_UI/Intellectual_Vision_CellCleanup_QA.md).
- 분리 성분341px 제거 및 이동 밖 Alpha1–2 잔여124px 별도 기록. 원본의 머리·리본·검·망토 절단 단면은 이동만으로 복원되지 않아 **READY승격0/BLOCKED_ART8·전체 Ready22/Blocked28 유지**. 다음 권장 작업은 잘리기 전 완전한 원본 파츠 확보와 해당 문제 셀 복원. 남은 프레임·좌표는 QA 참조.
- 다른42종 PNG/meta/Entry/QA/Clip 및 Intellectual Fighter Male READY 보존. 기존8 ID/128 Sprite GUID/fileID·Import·Gender+Path+Job/Save Mapping 유지. Assets/ProjectSettings/UserData 시작 해시에서8PNG+Catalog+Inventory 총10파일만 변경, 사용자 Save/Settings 보존. C#/Runtime 기능 변경 없음.
- 격리 PlayUnfocused86PASS/0FAIL: 대상8 Mapping/Preview·대표 Intellectual Mage Male/Vision Mage Female World/Battle Left Idle/실제 Battle Scene/Save→Bootstrap Continue 통과. BLOCKED 기본 성별 fallback 검증. Missing Sprite0·전체800 Sprite geometry 오류0, 최종 Console Error0/Warning0·현재 Compile Error0. 새 전체 C# 컴파일 미수행.
- clean Bootstrap Edit Mode·격리 Save/Settings/Play 설정 복원·포커스 전환 없음. 실제 걷기·키보드/게임패드 미검증. 직접 변경 staged diff --check 통과, 기존 사용자 whitespace 보존. GitHub Push 없음.
- 관련 commit: 계획 `c3a58e1`, 마지막 Art 수정·검증 `5ac5abb`. 이 상태 기록은 후속 문서 commit에 포함한다. 아래는 이전 이력이다.

## Intellectual/Vision Cleanup v2 8종 반영 완료 (2026-10-05)

- `Limitless_IntellectualVision_8_Cleanup_v2.zip` 지정8PNG만 원본 바이트로 교체. PNG8/중복0/누락0·512×512 RGBA·128px×16 확인. [적용 계획](../11_UI/Intellectual_Vision_CleanupV2_적용_계약.md)·[개별 QA/남은 프레임](../11_UI/Intellectual_Vision_CleanupV2_QA.md).
- 기존 큰 조각은 제거됐지만 머리·리본·망토 crop와 일부 인접 조각 잔존으로 **READY 승격0/BLOCKED_ART8, 전체 Ready22/Blocked28 유지**. Vision Fighter Male 방향 정상 유지. 다음 권장 작업은 QA 표의 원본 crop 윤곽 복구와 잔여 조각 제거.
- 다른42종 PNG/meta/Entry/Clip·판정과 사용자 변경 보존. Intellectual Fighter Male READY·Hearing/Mobility 불변. 기존8 ID/128 Sprite GUID/fileID·Import·Gender+Path+Job Mapping/Save/fallback 유지. Assets 전체에서8PNG+Catalog+Inventory 총10파일만 변경. Runtime 기능/C# 변경 없음.
- 격리 PlayUnfocused86PASS/0FAIL: 대상8 Mapping/Preview·대표 Intellectual Mage Male/Vision Mage Female World/Battle Left Idle/실제 Battle Scene/Save→Bootstrap Continue 통과. BLOCKED는 기존 기본 성별 임시 fallback. 첫 AssetDB 갱신시각 불일치2건은 동기 Import 후 전체 재검증으로 해소. 최종 Console Error0/Warning0·Missing Sprite0·전체800 Sprite geometry 오류0. 새 전체 C# 컴파일 미수행·현재 컴파일 오류0.
- clean Bootstrap Edit Mode·격리 Save/Settings/Play 설정 복원·포커스 전환 없음. 실제 걷기/키보드/게임패드 미검증. 직접 변경 파일의 staged diff --check 통과; 전체 diff의 기존 사용자 whitespace는 보존. GitHub Push 없음.
- 관련 commit: 계획 `4c9563a`, 마지막 Sprite 반영·검증 `501c0c8`. 이 상태 기록은 후속 문서 commit에 포함한다. 아래는 이전 작업 이력이다.

## Intellectual/Vision Cleanup8 반영 완료 (2026-10-03)

- 지정8종만 Cleanup ZIP 원본 반영. [계약](../11_UI/Intellectual_Vision_Cleanup8_적용_계약.md)·[QA/문제 프레임](../11_UI/Intellectual_Vision_Cleanup8_QA.md). 문서 `b73d949`, 반영 `745a913`; 검증 기록은 이 항목을 포함한 후속 commit.
- 대상 READY0/BLOCKED_ART8, 전체 **Ready22/Blocked28 유지**. crop/분리 조각이 남아 승격 없음. 다음 권장 작업은 QA의 문제 프레임 원본 미술 수정.
- 기존42종 PNG/meta/참조/판정 및 사용자94항목 보존. 기존8 ID/128 Sprite ID/import/Mapping/Save/fallback 유지. Runtime·다른 기능 변경 없음.
- 격리 PlayUnfocused86PASS/0FAIL, 대상8 Preview/Selection·대표 World/Battle Left Idle/Save→Continue·save-restore 통과. Console Error0/Warning0, Bootstrap Edit Mode·설정 복원. 새 C# 컴파일 미수행, 기존 deprecated 경고는 이전 이력 유지.
- Foreground/실제 걷기·키보드/게임패드 입력 미검증. 작업 diff --check 통과; 전체 diff에는 기존 사용자 whitespace가 남아 수정하지 않았다. GitHub Push 없음. 아래는 이전 이력이다.

## Intellectual3 / Vision5 V2 스프라이트 반영 (2026-10-03)

- `Limitless_IntellectualVision_8_Fixed_v2.zip`의 지정8종만512×512 RGBA 원본 그대로 교체했다. Target8/Duplicate0/Missing0·추가PNG0. 원본ZIP/추출PNG·이전8PNG/Git 이력 보존. 문서 선행 `6def064`, 반영 commit `35ea4ab`. [계약/기존8 ID](../11_UI/Intellectual_Vision_V2Eight_적용_계약.md)·[8종 개별 QA](../11_UI/Intellectual_Vision_V2Eight_QA.md).
- V2 READY0/Blocked_ART8·전체 **Ready22/Blocked28 유지**. Intellectual Mage Male 큰 조각은 제거됐으나 frame9/10에2–3px 잔여 조각. Vision Fighter Male Left 방향 일치, 잔여 조각으로 Art 보류. 다른6종도 머리 절단/인접 조각/Cell끝 잔여픽셀이 남는다. 다음 작업은 QA 표의8종 문제Frame 원본 미술 수정이다.
- 기존8 Appearance ID·128 Sprite GUID/fileID·meta/Import·Gender+Path+Job Mapping·Save/fallback 유지. 다른42종 PNG/meta·Catalog Entry/Sprite/Clip·Inventory/QA 동일. 특히 Intellectual Fighter Male의 READY·PNG/meta/8 Clip은 교체/재판정하지 않았다. 기능/퀘스트/UI/음성/BGM/전투/Save 변경 없음, 기존 QA 도구의 대상/대표만8종으로 보정.
- 격리 PlayUnfocused86 PASS/0 FAIL: 대상8 UI Mapping/Preview, Intellectual Mage Male·Vision Mage Female 대표 생성/World/Battle Left Idle/Save→Bootstrap Continue 통과. Blocked의 기본 성별 임시 fallback 검증이며 Ready Art 재생은 아니다. Compile/Console Error0·최종 Warning0(컴파일 직후 기존 deprecated 경고2). clean Bootstrap Edit Mode·격리 Save/Settings·Play 옵션 복원.
- Foreground/GameView 선택/포커스 전환/물리 입력 없음, 실제 걷기 품질 미검증. 관련 diff 검사 통과, 기존 사용자 변경 보존. GitHub Push 없음. 아래 기록은 이전 작업 이력이다.

## Intellectual4 / Vision5 Revised9 스프라이트 반영 (2026-10-03)

- 지정9종만 `Limitless_Intellectual4_Vision5_Revised.zip`의512×512 RGBA 원본 바이트 그대로 교체했다. 원본ZIP/추출PNG와 이전9PNG·Git 이력 보존. 문서 선행 `c66b43c`, 반영 commit `d4e0093`. [적용 계약](../11_UI/Intellectual_Vision_Revised9_적용_계약.md)·[9종 개별 QA](../11_UI/Intellectual_Vision_Revised9_QA.md).
- Intellectual Fighter Male READY 승격, 다른8종 BLOCKED_ART. Vision Fighter Male Left5/7 방향 오류는 해결됐으나 잔여 조각으로 Art 보류. 전체 **Ready22/Blocked28**. 남은8종의 문제 Frame·절단/조각을 QA 표에 기록했으며 다음 작업은 해당 원본 미술 수정이다.
- 기존9 Appearance ID·144 Sprite GUID/fileID·meta/Import·Gender+Path+Job 자동 Mapping·Save/fallback 유지. 승격1종의 Idle/Walk8 Clip만 Catalog에 추가. 다른41종 PNG/meta·Catalog Entry/Sprite/Clip·Inventory/QA 동일. 기능/UI/퀘스트/음성/BGM/전투/Save 코드 변경 없음; 기존 QA 감사만 승격 대상과 기대 집계 검사 보정.
- 이번 승인된 격리 PlayUnfocused89 PASS/0 FAIL: 대상9 UI Mapping/Preview, 수정판 Intellectual Male Fighter와 fallback Vision Female Mage의 생성/World/Battle Left Idle/Save→Bootstrap Continue 통과. Ready8 Clip key/16Frame·6fps/loop 검사. Compile/Console Error0·최종 Warning0(컴파일 직후 기존 deprecated 경고2). clean Bootstrap Edit Mode·격리 Save/Settings·Play 옵션 복원.
- GameView 선택/창 활성화/포커스 전환/물리 입력 없음. 실제 걷기 품질은 미검증. 관련 diff 검사 통과, 기존 사용자 변경 보존, GitHub main 조회·Push 없음. 아래 기록은 이전 작업 이력이다.

## Intellectual4 / Vision5 Player Sprite 부분 수정 (2026-10-03)

- 정확히9종 교체: Intellectual Fighter/Mage 남녀4개는512×512 원본 그대로, Vision Fighter Male/Guardian Male/Mage Female/Marksman 남녀5개는 이번 사용자 승인으로1254×1254 원본을 보존한512×512 최근접 변환본 적용. 문서 선행 `5b1de7e`, 마지막 관련 기능 commit `7c65c78`. [입력·9조합 ID 계약](../11_UI/Intellectual_Vision_Fixed9_적용_계약.md)·[QA](../11_UI/Intellectual_Vision_Fixed9_QA.md).
- 기존9 Appearance ID·144 Sprite GUID/fileID·meta/Multiple/PPU128/Pivot/Point/Uncompressed·자동 Mapping/Save 스키마 유지. Duplicate0/Missing0. 다른41종 PNG/meta·Catalog/Sprite/Clip·Inventory/QA 동일. Hearing10/Mobility10 BLOCKED_ART 유지, Voice/BGM/Main16 변경 없음.
- 144Frame 관찰: Empty0·시트별 Duplicate0, Intellectual PASS0/Blocked_ART4, Vision PASS0/Blocked_ART4/Blocked_DIRECTION1(Fighter Male Left5/7이 Right 방향). 머리 절단·인접 잔여 조각이 남아 **전체 Ready21/Blocked29 유지**. 다음 작업은9종 QA의 문제 프레임 원본 미술 수정이다.
- 격리 PlayUnfocused89 PASS/0 FAIL: 대상9조합 실제 UI 자동 Mapping/Preview, Intellectual Male Fighter·Vision Female Mage 생성/World/Save→Bootstrap Continue/Battle Scene 전달 및 이전 ID 재계산 통과. 미술 Blocked의 기본 성별 임시 fallback 검증이며 Ready Art 재생은 아니다. 실제 걷기·물리 입력·Foreground는 미검증이다.
- Compile/Console Error0·최종 Warning0, 보호880파일 중 의도한11개만 변경. clean Bootstrap Edit Mode·격리 Save/Settings·Play 옵션/백그라운드 설정 복원, 포커스 조작 없음. 관련 diff 검사 통과, 기존 사용자 변경 보존, GitHub Push 없음.

## Mobility Player Sprite 수정판 교체 (2026-10-03)

- 제공 ZIP의512×512 RGBA 캐릭터 시트10개(Male5/Female5·5Job×2)를 기존 Mobility PNG 경로에 원본 바이트 그대로 반영했다. 참고 Preview.png는 제외, ZIP·원본 미술 변환/복원/재생성 없음. 문서 선행 `8cb64bb`, 마지막 관련 기능 commit `3179685`. [계약](../11_UI/Mobility_Player_Sprite_Fixed_적용_계약.md)·[10종 QA](../11_UI/Mobility_Player_Sprite_Fixed_QA.md).
- 기존 Appearance ID10개·Catalog/자동 Gender+Path+Job Mapping·Save 구조와160 Sprite GUID/fileID·meta/Import 설정 유지. Duplicate0/Missing0. 다른40종의 PNG/meta Hash·Catalog Entry/Sprite/Clip/QA·Inventory 동일, Hearing 재판정 및 Voice/BGM/Main16 변경 없음.
- 160Frame 관찰: Empty0·시트별 Duplicate0·본체4방향/휠체어 디자인 확인. 10종 모두 인접 머리/바퀴/무기 조각 또는 Up 머리 상단 절단이 남아 **Mobility Ready0/Blocked_ART10, 전체 Ready21/Blocked29 유지**. 원본 미술에서 각 Cell128px 안에 완전한 캐릭터를 배치하고 잔여 조각을 제거하는 것이 다음 권장 작업이다.
- 격리 PlayUnfocused 감사86 PASS/0 FAIL: 남녀10조합 실제 Path/Job Preview/Mapping/fallback 표시, Male Fighter·Female Sharpshooter 생성/World/Save→Bootstrap Continue/Battle Scene 전달 통과. Blocked의 기본 성별 임시 fallback 검증이며 수정판 Ready Art 재생은 아니다. 사람의 걷기 품질·키보드/게임패드·Foreground는 미검증이다.
- Compile Error0·최종 Console Error0/Warning0(컴파일 직후 기존 deprecated 경고2). 보호880파일 중 의도한12개만 변경. clean Bootstrap Edit Mode·격리 Save/Settings/Play 옵션/백그라운드 설정 복원, 포커스 전환 없음. 관련 diff 검사 통과, 기존 사용자 변경 보존, GitHub Push 없음.

## Hearing Player Sprite 수정판 변환·교체 (2026-10-03)

- 사용자 승인으로 원본 ZIP1254×1254 PNG10개를 보존하고 별도512×512 최근접 변환본을 기존 Hearing10종 경로에 반영했다. 원본 미술 재생성·복원·재배치·Alpha 보정 없음. 문서 선행 `f412331`, 교체·감사 `961e3a7`. [계약](../11_UI/Hearing_Player_Sprite_Fixed_적용_계약.md)·[QA/10종 문제 프레임](../11_UI/Hearing_Player_Sprite_Fixed_QA.md).
- Male5/Female5·각5Job×2, Duplicate0/Missing0. 기존10 Appearance ID·Gender+Path+Job Mapping·Save 구조와160 Sprite GUID/fileID·Import 설정 유지. 다른40종 PNG/meta Hash와 Catalog Entry/Sprite/Clip/QA 상태 동일, BGM/Voice/Main16 변경 없음.
- 변환본160프레임 관찰 결과 Hearing Ready0/Blocked_ART10. 인접 셀 조각·Right 발 아래 다음 행 머리·Up 머리 잘림·일부 무기/망토 절단이 남아 **전체 Ready21/Blocked29 유지**. 수정판이라고 PASS하지 않았으며 Runtime은 명시적 기본 성별 임시 fallback을 유지한다. 다음 작업은128px Cell마다 캐릭터/장치를 온전히 배치하는 원본 미술 수정이다.
- Hearing 전용 격리 PlayUnfocused 감사96 PASS/0 FAIL:10조합 실제 UI Mapping/Preview/Story, 대표 남녀 생성/World/Save→Bootstrap Continue/Battle Scene 전달 통과. 수정판 Art의 Runtime Ready 재생은 하지 않았다. 사람의 실제 걷기 품질·물리 입력·Foreground는 미검증이다.
- 최종 Compile/Console Error0·기존 deprecated 경고2, clean Bootstrap Edit Mode·격리 Save/Settings·Play 옵션 복원. 보호880파일 중 의도한12개만 변경했다. 관련 diff 검사 통과, GitHub Push 없음.


## Player Sprite 자동 조합 매핑 설계 정정 (2026-10-03)

- 정식 규칙을 **Gender + Path + Job → 대응 Sprite 자동 결정**으로 정정했다. 성별/이름→Path→Job→Confirm 흐름을 유지하며 수동 Appearance 필터·이전/다음·목록·확정·번호 UI를 제거했다. Preview는 결과를 자동 갱신한다. Path Theme는 실제 Path 디자인이고 Job 추천은 선택 제한이 아니다. 아래 독립 Appearance 선택 기록은 폐기한 이전 구현 이력이다.
- Male25/Female25·Path별10·Job별10, Stable ID Mapping50 / Duplicate0 / Missing0. Marksman→sharpshooter 등 메타데이터 이름만 대응한다. [50조합 Matrix](../11_UI/Player_Sprite_Combination_Matrix.md)·[정식 계약](../11_UI/Player_Appearance_선택_Save.md).
- RuntimeReady21 / ArtBlocked29 유지. Blocked도 정확한 조합 ID를 기록하며 기본 성별 Sprite의 명시적 임시 fallback을 사용한다. 다른 Path/Job 또는 공통 Mobility Sprite로 대체하지 않는다. PNG50·Sprite800·Catalog·기존 Animator 재Import/재Slice/재생성 없음, 보호282파일 SHA256 동일. [기존 Blocked 프레임/사유](../11_UI/Player_Appearance_Foreground_QA.md).
- Save 정본 Gender/Path/Job/Name을 유지하고 AppearanceId는 계산 결과다. Continue는 누락·invalid·이전 수동 ID 불일치도 정본에서 재계산한다. Path/Job 없는 Version1은 기본 성별 fallback, 사용자 Save 삭제·초기화 없음. World Clip Override와 Battle Left Idle은 같은 결과를 사용한다.
- PlayUnfocused 격리 감사 **346 PASS/0 FAIL**, 실제 UI50조합·대표5Path 생성/World/Save→Bootstrap Continue/Battle Scene 전달·구버전 복원·Story 실제 Path 판정 통과. 백그라운드 UI Render **7 PASS/0 FAIL**, 1280×720·800×600 기본 정보/Job/Confirm 확인. Compile/Console Error0, 기존 deprecated 경고2. [검증·한계](../11_UI/Player_Sprite_Combination_QA.md).
- 사용자 직접 확인: 물리 키보드·게임패드/foreground 미검증. 기존800×600 최종 확인 중앙/능력치 패널 겹침과 직업명/상세 간격은 후속 UI 작업 대상. 다음 권장 작업은 Blocked29 원본 미술 수정·재검수와 최종 확인 반응형 레이아웃 정리다.
- 마지막 관련 commit: 문서 `98dd23a`, 자동 Mapping/UI `dc9feff`, Save/Continue `6102952`. clean Bootstrap Edit Mode·격리 Save/Settings·Play 옵션 복원. BGM/Voice/Main16 및 기존 사용자 변경 유지, GitHub Push 없음.


## Main16 Audio 마무리 — Field07 탐색 BGM (2026-10-03)

- Where the Earth Breathes를 원본 MP3(44.1kHz stereo/192kbps/180.17초) 그대로 `Audio/Music/`에 Import하고 기존 Catalog에 Field07 한 항목만 추가했다. 탐색·조사·불씨망령 전투 후 잔향·협곡 입구에 같은 곡을 쓴다. 정책 `f0a7129`, 마지막 관련 기능 commit `012be7d`.
- 전투 적용 최초 승인 응답은 사용자가 철회했고, 재확인은 **기존 유지: Field07 전투 null/TBD**다. Field07 일반/Story Battle은 음악을 정리하며 복귀는 새 탐색곡이다. Steel and Sunlight는 기존 Chapter1 일반전에 유지한다. 후보 Trail of the Ember Wraith·Beneath The Cracked Earth는 Downloads 보관·Runtime 미적용, 기존 Chapter2 소개/서부/Deep West 정책 유지.
- Main16 전용 격리 Audio 감사 최종217 PASS/0 FAIL: Title·Arbel·Field06↔07 실제Exit·단일 Source/Listener·Save→Continue·일반 Battle 진입/복귀·실제 Story 공격 Victory/복귀·패배 복귀 API·잔향 대사·Hearing/Default44 Dialogue/28 Voice 재생·Next/종료·Loop 순환. 전체 Quest428/507 검사나 Appearance QA는 반복하지 않았다. [방법·Fixture 수정 이력·한계](Main16_Audio_마무리_QA.md)·[로그](Main16_Audio_Runtime_Results.txt).
- Voice100/BGM80 유지, BGM0에서도 Voice 출력0.5678347·Mute 전체0·해제 복원 통과. 임시 메모리 SFX 제거, 원본 Gain/Mixer/전역 기본값/Ducking/Runtime 게임 코드 변경 없음. 보호750파일 해시 동일, Voice28 Mapping·Appearance21/29·사용자 변경 유지.
- 실제 사람 귀의 청취는 **0/28**다. 발음·감정·호흡·속도·Segment boundary·Speaker 음색·Voice/BGM balance/Masking·BGM Loop 경계·장면 적합성은 미검증이며 TTS 재생성 필요 목록은 미정이다. [개별28개 청취 체크리스트](Main16_Audio_청취_체크리스트.md). 다음 권장 작업은 사람이 기본 Voice100/BGM80으로 청취해 실제 문제만 기록하는 것이다.
- Unity 컴파일 오류0, 최종 Console Error0/Warning2(기존 ExternalAssetImportEditor의 deprecated spritesheet API 경고). clean Bootstrap Edit Mode, 격리 Save/Settings·Play 옵션·runInBackground 복원. 직접 변경만 로컬 커밋, GitHub Push 없음. 아래 기록은 당시 이력이다.

## Appearance Foreground·Main16 Voice 중단 작업 이어서 완료 (2026-10-03)

- 직전 완료된 Main16 WAV28개·Mapping28개와 외형50종 실제 화면 판정부터 재개했다. ZIP 재Import·Sprite 재Slice·기존 Voice Import·Quest 재구현·TTS 생성은 반복하지 않았다. 이번 continuation의 남은 범위 Foreground 승인을 별도로 받았다.
- Appearance 총50 / Ready21(남10·여11) / Blocked29 / 시트 육안 미판정0. Mobility10 전부 보류, Vision Alpha 경고10은 PASS5·보류5. Hearing10·Intellectual Fighter/Mage4를 추가 보류했다. [파일·Frame·사유](../11_UI/Player_Appearance_Foreground_QA.md). 원본 PNG50·Catalog800 Sprite GUID/fileID 동일, 준비 Clip168개. 기존 준비 Clip을 재사용하고 보류 정책만 반영했다.
- 최종 정책의 성별·5테마·이전/다음·Preview·선택·확정·Navigation·World·Save/Continue 실제 감사803 PASS/0 FAIL. 직전 Hearing 외형+Vision 실제 Path 독립·대표 World32상태를 재사용했다. 대표 Vision 여성의 실제 Battle Left Idle도 확인했다. 800×600 단계 표시 겹침을 최소 수정하고 같은 해상도 화면·경계로 재검증했다.
- Main16 Manifest28 / Source Audio33(segment28·합본5) / Import28 / Mapping28 / 기존201+28=Story229. Player 무음9·관찰7, Unmapped/Text/Speaker mismatch/Missing/Duplicate 모두0. Hearing·Default 실제 Scene/NPC/Story Battle/귀환·완료와 격리 Save/Continue428 PASS/0 FAIL. 연속 Next·화자 fallback·전투 정리·Voice100/40/0·Mute·retry 출력 통과. 기존201 Catalog prefix와 WAV 원본28개 동일. [적용·검증 상세](Main16_Voice_Foreground_QA.md).
- 컴파일 오류0, 최종 Console Error0/Warning0. 컴파일 시 기존 ExternalAssetImportEditor deprecated 경고2건은 별도 확인했다. clean Bootstrap Edit Mode, 임시 해상도·Play 옵션·격리 Save/Settings 복원。 기존 보호 대상211파일 해시 동일, 기존 작업 트리를 보존하고 직접 변경만 커밋했다.
- 사용자 직접 확인: 실제 청취0/28로 발음·감정·호흡·속도·Segment boundary·Text Audio 일치·음량 취향은 미검증이며 재생성 필요 수는 미정이다. OS Tab 전달 후 실제 이동을 확인하지 못해 키보드·게임패드 물리 입력은 미검증으로 남긴다. 다음 권장 작업은 보류29개 원본 그림 수정과 청취 품질 검수다.
- 마지막 관련 기능 commit: Main16 `b957aa7`, Appearance/UI `3953e49`. GitHub Push 없음. 아래 기록은 당시 상태를 보존한다.

## Player Appearance 선택·Save/Continue (2026-10-03)

- CharacterCreation에 성별·전체/5테마 필터, 이전/다음 외형 목록 탐색, 큰 Preview, 체크/이름/번호 선택 표시와 기존 Navigation을 연결했다. Catalog50종 중 **35 ready / 15 보류**(남17/여18 선택 가능)이며 기존 기본 외형도 유지한다. Vision 경계 경고10종157셀 중5종은 낮은 Alpha/장치 윤곽 접촉으로 합격,5종은 인접 행 조각으로 보류했다. Mobility10종은 분리된 본체·다른 Character·방향 오류 의심으로 보류하고 원본은 수정하지 않았다. [선택 계약](../11_UI/Player_Appearance_선택_Save.md)·[전수 QA](../11_UI/Player_Appearance_선택_QA.md).
- Stable Appearance ID를 Session/Version1 Save에 선택적 필드로 추가하고 Continue에서 복원한다. 유효한 ID는 공용 성별 Animator의 Sprite Clip만 Override하며 World와 기존 Battle Left Idle에 반영한다. 구버전·invalid·blocked ID는 기존 Male/Female+Path Variant를 유지한다. 외형 테마는 실제 Path/Job/Story와 독립이며 미용실/진행 중 변경 NPC는 미구현이다. 마지막 관련 기능 commit **`cc3aa90`**, 선택/Runtime `0ad3cc5`, 문서 선행 `eeb48b2`.
- Background PlayUnfocused **1197 PASS / 0 FAIL**: 실제 생성→World,35종4방향 Idle/Walk·Battle용 Sprite,35종 Save/Restore, 신규 남/여 Bootstrap Continue, 구버전/invalid 남/여 Continue, 기존 외형/Path fallback30조합과 Path×Job25조합. Catalog50/800·ID/meta/성별/테마 정상, Stable ID·Sprite 참조 유지·PNG50 원본 해시 동일·보호798파일 변화0. 컴파일 오류0·최종 Console Error0/Warning0(재컴파일 당시 구형 편입 도구 CS0618 경고2개 별도). Bootstrap clean Edit Mode·Editor unfocused·격리 경로/진입 옵션 복원, foreground/OS 입력 없음. 관련 diff 검사 통과, GitHub push 없음.
- 사용자 확인/다음 권장 작업:35종의 발 미끄러짐·방향 전환·프레임 흔들림·크기와 장치 표현, 실제 키보드/컨트롤러 입력을 육안/기기로 확인한다. 보류15종은 사용자 원본의 프레임/Alpha 문제를 먼저 해결한 뒤 재검수한다. Foreground Visual QA와 Battle 전체 playthrough는 미완료다. 아래50종 등록 항목의 미구현 설명은 등록 당시 이력이며 현재 선택/Save 상태는 이 항목을 따른다.

## 검증본 Player Appearance 50종 등록 (2026-10-03)

- 사용자 `Limitless_Player_Sprites_Validated_50.zip`의 PNG50개를 원본 바이트 그대로 `Art/Characters/Player/Validated50/`에 편입했다. 5테마 각10개·남25/여25·512×512 RGBA·4×4/128px16프레임이며 정식 Player의 PPU128·Point·무압축·하단 중앙 Pivot을 적용했다. [등록 계약과 검증 결과](../11_UI/Player_Appearance_Validated50.md)를 따른다. 문서 선행 commit `00aaaef`, 마지막 관련 기능 commit `d6937e4`.
- 기존 External50 Catalog GUID 및 stable ID50개를 유지하고 새 sheet/800 Sprite를 연결했다. 테마/직업은 메타데이터이며 선택 제한이 아니다. 이전 PNG·Animator·Prefab·Scene·Male/Female+Path fallback·Save 형식은 유지했다. Character Creation50종 UI·appearanceId Save·실제 게임 외형 Override는 미구현이며 `readyForSelection=false`다.
- 백그라운드 Edit Mode 검사:50/800 참조·Rect/Pivot/설정·유일 ID·meta 정상, 재편입 Sprite 참조800개 동일, 원본 ZIP/PNG 해시 동일. 보호 파일212개 중 의도한 Catalog만 변경, 기존 사용자 변경 보존. 컴파일 오류0·Console 오류0, 기존 외부 편입 도구의 구형 spritesheet API 경고2건. Bootstrap clean·Editor unfocused, foreground 조작 없음.
- 사용자 확인/다음 권장 작업:10시트157 Cell의 Alpha 경계 접촉·전체50종 방향 행/발 위치를 육안 검수하고 행 계약을 확정한다. 그 후 공용 Animator Clip Override와 선택/Save ID를 별도 연결한다. GitHub push 없음.

## Main16 재 속의 형상 (2026-10-03)

- **구현 및 범위 내 Runtime QA 완료**: `main_16_shape_in_the_ash`의 12개 순차 목표, Field07 재바람 황야·Field06 서쪽↔Field07 동쪽 연결·지정 불씨망령1체·협곡 입구·아르벨 보고를 기존 Quest/World/Battle/Save에 연결했다. 계약은 [Main16 Runtime](../03_스토리/Main16_Runtime_구현_계약.md)을 따른다. 마지막 관련 기능 commit `2ef6690`.
- 정식 `path.hearing` 분기에서는 Player가 최초 관찰하고 세린이 확인한다. Default는 세린이 먼저 공유한다. 분기별 stable Dialogue ID·현장 세린 NPC를 사용하며 저장 Party/Formation과 실제 전투 편성은 변경하지 않는다. 지정 승리 이후 일반 불씨망령 배치와 협곡 발견은 Quest count에서 복원한다.
- 격리 Save/Settings의 PlayUnfocused에서 Hearing/Default × 세린 편성/미편성 네 실행 모두 완료, PASS 체크507개. 실제 NPC·순차 조사·명령 공격 Victory·도망/재도전·Defeat 중앙 Arbel 복귀·Bootstrap Continue17회·Navigation·펫 미해금을 확인했다. 자세한 방법/Fixture와 한계는 [Main16 QA](../03_스토리/Main16_Runtime_QA_2026_10_03.md)를 따른다.
- 21:9 변경 직후 카메라 보간이 맵 밖을 보여 주는 오류를 수정하고 3비율×4모서리의 첫 프레임 viewport 제한을 검증했다. 목격 취소 후 자동 재열림도 방지했다. 최종 컴파일/Console 오류·경고0, Bootstrap clean Edit Mode·격리 경로/진입 옵션 복원·Editor unfocused. 관련 diff 검사 통과, 전체 트리는 기존 사용자 공백429건으로 exit2다.
- 사용자 확인/다음 권장 작업: 조사 표식·한국어 TextMesh·재/균열/협곡 배치 시각 QA와 실제 이동/입력 검수. Foreground/OS 입력 없음. Voice 미적용·Field07 BGM TBD이며 별도 확정 뒤 제작/배정한다. Main17/협곡 내부/Overheat/Boss는 구현하지 않는다. 직전 Asset/BGM/Background/Player 작업과 기존 사용자 변경을 보존했다. GitHub push 없음.

## 외부 Asset 정식 편입 (2026-10-03)

- 확정 MP3 7곡, Chapter1 배경 PNG 6종, Player 시트 50종을 원본 바이트 그대로 편입했다. Chapter1 일반전=Steel and Sunlight, B1=Stone Without Memory, B2=Beneath The Forgotten Hall, 파수꾼=The Warden’s Final Stand, Field04~06=Paths of Cracked Earth. 기존 Mixer/BGM 서비스·Scene 복귀 정책을 재사용했다. Intro 곡은 시작/종료 Hook만 준비했고 Deep West 곡은 최초 Scene 미확정으로 Catalog에 보관한다. 후보4곡은 Downloads에 보존하며 Chapter2 일반 전투 변주는 TBD다.
- 배경은 Field01 초원, Field02/03 숲, Dungeon B1/B2 지하묘지, 파수꾼 Boss 전용으로 연결했다. 독성 늪/고대 성소는 대응 Scene 미정으로 미배정이다. 원본1672×941 RGB를 기존 전체 화면 Background Image에 비율 유지 Fit하고 기존 HP/명령 영역을 유지했다.
- Vision/Hearing/Intellectual/Mobility/Emotional Scar 각10종(각 Male5/Female5)을 실제 집계했다. 고정 Appearance ID·원본 Texture 참조 Catalog를 준비했으며 Vision512×512만 160프레임 Slice했다. 나머지40종1254×1254는 128px Cell 규칙과 불일치해 Slice를 보류했다. Mobility10·Intellectual4종에는 Alpha가 없다. 전체50종 방향/발 Pivot 검수 전으로 선택 가능=false이며 Character Creation 신규 외형 선택과 appearanceId Save는 **미구현**이다. 기존 Male/Female·Path Variant·Save fallback은 유지했고 새 Animator Controller를 복제하지 않았다.
- 검증: Unity6000.5.7f1 Runtime/Editor 컴파일 완료·Console 오류/경고0, Edit Mode 최종216항목 실패0, 신규63파일 원본/SHA256 일치·GUID 유일, 배경6 Sprite·Audio7 Clip 참조 정상, 직접 변경 staged diff --check 통과. Bootstrap dirty=false 유지, Scene 저장·Play Mode·foreground 조작 없음.
- 사용자 직접 확인: 실제 음악/Loop 경계·일반전/Boss 후 탐색곡 복원·화면 비율별 배경/HP 가독성. 다음 권장 작업: 1254px 시트 Cell/Alpha 원본 확인과 50종 방향/발 위치 검수 후 공용 Animator Override·독립 외형 선택·Stable Appearance ID Save 연결. Intro 첫 진입 연출 종료 계약·Deep West 최초 Scene 확정도 별도 작업이다.
- [편입 정본·Inventory·검증](외부_Asset_정식_편입_2026_10_03.md). 마지막 관련 기능 commit: `e8603df`, 정책/검증 문서: `2b7218b`. GitHub Push 없음.

## Missing Story Voice Supplement Pack (2026-09-30)

- [보충팩 QA](Missing_Story_Voice_Supplement_QA.md)의 정식 CSV58행을 LOCAL 기존 무음 대사와 대조하고 사용자 허용에 따라 안정 ID를 지정했다. Main03 12/Main04 18/Main05 6/Main08 19/Main12 3개를 원본 WAV 그대로 적용했다. 입력61 WAV 중 Manifest 미참조 합본3개는 제외, Main01/02 신규 음성은 미제공이다. 기존 Registry의 태온 Gacrux34/미엘 Sulafat22/폴 Achird2개, gemini-3.8-flash-tts·Voice Design 사용 안 함을 기록했다.
- Existing Voice Wins 원칙으로 기존143+신규58=Story Catalog201개. 기존 ID 삭제/Clip 교체/GUID 변경/Character 변경0, Intro18개·Main06/07/09/10/11 기존102개·Main13~15 기존41개·BGM 보존. 기존 관련353파일 및 사용자 수정73파일 해시 동일. 모든 기존 대사 화자/본문/순서 보존, 불일치/누락/중복/범위 밖 Row0. Main01~12 정적228쪽 중 Voice160쪽(70.18%), Voice 없음68쪽이다.
- Background 실제 Play Mode에서 신규58개 참조/디코딩/비무음/58회 연속 Next/종료/fallback, 실제 Main03/04/05/08 대사 함수와 Main12 조사 Line/VoiceId 샘플의 Voice 출력 확인. Main01은 기존 무음5쪽 진행을 확인했다. Intro Storyteller·Main07/09/11/13 회귀, Voice100/40/0·Mute/Unmute 출력, 신규 음성 중 Scene 전환 후 Source0 통과. Main12 전체 Quest 상호작용/전체 playthrough·물리 입력·시각 검증은 미완료다.
- 컴파일 오류0·최종 Console Error0/Warning0·이번 변경 diff 검사 통과. 격리 Save/Settings와 백그라운드 실행/진입 옵션 복원, Bootstrap clean Edit Mode, Foreground 조작 없음. 정책 계획 commit `5814d34`, 마지막 관련 기능 commit `8756399`. 실제 WAV/Meta·Catalog·직접 연결 C#5개·문서만 커밋, 제작 Output 전체 Stage 및 GitHub push 없음.
- 사용자 확인/다음 권장 작업: 실제 청취로 발음·감정·호흡·캐릭터 취향·음량 균형을 검수한다. Main01/02 일반 NPC/Player를 포함한 남은 무음 대사는 별도 제작 범위를 확정한 뒤 보충한다. Main16은 기존 문서 설계 상태이며 이번에 구현하거나 Voice를 만들지 않았다. 세부 Mapping/GUID/해시/Runtime 출력은 [감사 JSON](Missing_Story_Voice_Supplement_Audit.json)을 따른다.

## Path 반응형 Story와 Main16 설계 (2026-09-30)

아래는 당시 문서 설계 작업 기록이다. 현재 Main16 Runtime 상태는 상단 2026-10-03 항목을 따른다.

- [Path 반응형 Story 정본](../03_스토리/Path_반응형_Story_연출_규칙.md)에 Player Agency First·공통 핵심 정보/Story State·Hearing Player 우선 관찰/세린 교차 확인·다른4개 Path 방향을 확정했다. [Story Scene 템플릿](../03_스토리/Story_Scene_설계_템플릿.md)에 Path-Reactive Check를 추가하고 Path/세린/Story/TTS 문서는 정본을 참조한다.
- Main13 첫 만남/장치 인식, Main14 반복·강한 진동, Main15 짧아진 진동·비교·서쪽 방향을 DESIGN RETROFIT CANDIDATE로 기록했다. 기존 대사/ID/Voice/Runtime은 유지하며 향후 별도 분기 확정·구현·검수를 해야 한다.
- [Main16 「재 속의 형상」](../03_스토리/Chapter2_Main16_재_속의_형상.md)의 정식 Story 설계 완료. Field07 재바람 황야/Scene 가칭·12 Objective·움직이는 재의 Hearing 분기·불씨망령 단독 Story Encounter·처치 후 남은 진동·지하 열기 가설·협곡 입구/Arbel 보고·세린 미편성 시 진행/편성 보존·Main17 후보를 기록했다. 실제 Stable ID/Scene 배치/보상 TBD, Main16 Runtime/Quest/Scene/Voice/BGM 미구현, Overheat 미사용·Boss 미노출이다.
- 이번 작업은 Markdown 문서15개만 변경했다. 링크/참조 경로 및 새 중복 제목 검사 통과, 이번 변경 diff 검사 통과. C#/Scene/Asset 수정·Quest/Dialogue 구현·TTS 생성·Unity 실행/Play Mode 없음. 기존 사용자 변경92항목 보존, 설계 commit `5a92b72`, GitHub push 없음.
- 다음 권장 작업: Main16 구현 전 체크리스트의 LOCAL API/ID·World 왕복/Bounds·순차 목표/Save·Story NPC/Override·분기별 Dialogue를 확인하고 확정한 뒤 구현한다. Voice는 Story→Branch→ID→Manifest 확정 후 제작하며 Field07 BGM은 별도 확정한다. 현재 Unity에서 확인할 신규 구현 사항은 없다.

## Main13~15 Voice 및 확정 BGM 3곡 (2026-09-30)

- 정식 `dialogue_manifest_main13_15.csv`41행과 LOCAL 화자/본문을 대조해 Main13 8/Main14 11/Main15 22개를 원본대로 Import/연결했다. 세린 Schedar15개·레온 Orus5개와 기존 태온 Gacrux6/미엘 Sulafat4/폴 Achird11개, 제작 모델 gemini-3.8-flash-tts/ko-KR를 Registry에 기록했다. Voice Design 사용 안 함, Tone/속도/Batch TBD. PCM24kHz/mono/16bit·253.40초·원본 바이트/신규 GUID41개 정상, 중복/누락/본문·화자 mismatch0. 기존102개를 유지한 Story Catalog143개, Intro18개 유지, Main16 이후 미적용.
- 정식 BGM3곡을 원본 MP3 그대로 Audio/Music에 Import했다. Before the First Light→Bootstrap Title, Morning at the Gate→StarterVillage/Arbel/Field03 던전 입구 안전 반경, Morning Over the Ridge→Field01/Chapter2 첫 서부 Field04. Field05/06·다른 Field·Dungeon/Battle/Boss·Intro는 TBD/이전 음악 정리. 단일 지속 Source·기존 BGM Mixer·Loop·같은 곡 재시작 방지·Voice 독립, Ducking/Crossfade 없음.
- 배경 실제 Play Mode에서41 ID/화자/Next·실제 NPC/조사 factory·인물5명 출력·Voice/BGM/SFX 독립 출력·전체 Mute0/설정 복원, 최종12 Scene 전환의 곡 선택/정리/서비스1·Listener1·세 곡 출력, 안전 반경 진입/이탈을 통과했다. Intro/기존Story120개 참조와 실제 Next/Skip 회귀도 확인했다. Intro→Title의 Listener 수명 누락 및 보충 중 Intro 중복을 수정해 재검증했다. 상세는 [Voice/BGM QA](Main13_15_Voice_BGM_QA.md).
- 최종 컴파일 오류0·Console Error0/Warning0·이번 변경 diff 검사 통과. 기존 사용자 변경/기존음성/입력원본 등370파일 SHA-256 동일. 격리 Save/Settings·Game View 진입 동작·백그라운드 설정 복원·Bootstrap clean Edit Mode 종료. Foreground 검증 없음. 계획 `5f3dca9`, Voice 기능 `6818b42`, 마지막 BGM 기능 `ea97a2d`. 관련 파일만 커밋, GitHub push 없음.
- 사용자 확인/다음 권장 작업: 실제 청취의 취향·감정·발음·호흡·Voice/BGM 균형·Loop 경계·Scene 감정 적합성을 검수한다. 전체 Main13~15 Quest playthrough·실제 이동/물리 입력/시각 QA는 미검증이다. 후속 Field/Dungeon/Battle/Boss 전용 BGM과 Main16 이후 Voice는 별도 확정 후 도입한다.

## Main01~12 Story Voice 적용 (2026-09-30)

- 외부 PreSerin 출력107 WAV 중 manifest104쪽을 LOCAL 화자/본문으로 대조해102개550.97초를 원본 그대로 Import/연결했다. 태온 Gacrux28개·미엘 Sulafat22개·폴 Achird52개. PCM24kHz/mono/16bit, 원본 바이트 동일·GUID/참조102개 정상·전체 입력 디코딩107개. 모델/Tone/Batch는 근거가 없어 TBD. Narrator/Intro18개 유지, 세린 미적용.
- Main06 3개/Main07 26개/Main09 27개/Main10 18개/Main11 28개 적용. Main01~05·Main08·실제 Main12는 음성 미제공으로 텍스트 진행 유지. CSV Main12 두 항목은 실제 Main14이라 제외했다. 정적 대화228쪽 중 Voice 없음126쪽, TEXT_AUDIO_MISMATCH0·중복0·범위 제외 Unmapped2. 기존 대사/Save/수동 Next 정책 보존.
- 기존 VoiceClipCatalog/VoicePlaybackSource/Voice Mixer를 재사용하고 선택적 Dialogue ID·화자 확인을 추가했다. 실제 백그라운드 Play Mode에서102 ID/Clip/화자·연속 Next·null/wrong-speaker fallback·실제 Main07/09/10/11 factory·Main01/02/05/12 무음 진행·Hide/단일 Show/확인창/비활성화·Scene 제거 정리를 확인했다. 인물별 실제 출력, Voice0/Mute/채널값 복원, Intro18 참조/8연속Next/Skip→CharacterCreation·잔류Source0 통과. 상세는 [Story Voice QA](Story_Voice_Main01_12_QA.md).
- 검증 상태: 컴파일 오류0·최종 Console Error0/Warning0·이번 변경 diff 검사 통과. Bootstrap clean Edit Mode·격리 Save/Settings·Game View 진입 동작·백그라운드 설정 복원. Foreground/Game View 활성화 없음. 계획 Docs `e4a495c`, 마지막 관련 기능 commit `ea776a9`. 이번 변경만 커밋, GitHub push 없음.
- 사용자 확인/다음 권장 작업: 실제 청취로 발음·감정·호흡·문장/자막 일치·컷 경계·음량 균형을 검수한다. 전체 Quest playthrough·물리 입력·시각 배치는 미검증이다. 미제공 Main01~05/08/12와 Main14로 분류된2개 제작 메타데이터를 후속 확인한다. Main13 이후·세린 Voice 도입은 별도 작업이다.

## 정식 Audio Volume Settings (2026-09-30)

- Master→BGM/SFX/Voice AudioMixer, 전체 음소거·채널별0–100 Slider/숫자·실시간 적용, Scene 공용 설정 화면을 구현했다. 버튼/F10/패드 Start 진입, 기존 Navigation과 모달·이동 잠금 사용. 기본값 Voice100/SFX100/BGM80/Mute OFF, 로그 dB 변환·0은-80dB, Mute에서도 채널값 보존. Intro Source는 Voice Group·volume1로 연결해 중복 감쇠 없음.
- 기존 사용자 JSON을 Version2로 확장해 슬롯과 독립적으로 저장한다. 구버전 Skip 보존·Audio 기본값 이행, 범위 제한, Scene 유지·재Load·Play 재시작에서63/47/72 및 Mute 복원 확인. 정식 BGM/SFX Source는 현재 없으며 신규 Source는 공용 Route API로 용도별 Group에 연결한다.
- 실제 백그라운드 Play Mode에서 Intro와 임시 BGM/SFX 신호의27개 출력 검사, UI 숫자·방향 이벤트·모달 정리, Intro18개 대응/Next/연속Next/Skip/전환 후 Source0 회귀 통과. TTS 원본·사용자 저장·기존 변경109파일 해시 동일. 상세는 [Audio QA](Audio_Volume_Runtime_QA_2026_09_30.md). 컴파일/게임 Error0, 재컴파일 MCP Warning1 별도 기록. Bootstrap clean Edit Mode·검증 경로/진입 동작 복원.
- 남은 확인/다음 권장 작업: Game View 시각 배치·물리 키보드/패드/마우스·청취 밸런스·배포 Player 저장 검수. BGM/SFX 실제 음원 도입 시 정식 Group 연결. 기존 Intro 발화·분할 경계 청취 검수도 계속 남아 있다. 계획 Docs `f628e11`, 마지막 관련 기능 commit `cd656e0`. 이번 변경만 커밋, GitHub push 없음.
- 최종 Console Error0/Warning0 재조회. 이번 변경 diff 검사 통과, 전체 작업 트리의 기존 사용자 변경에 남은 공백 오류는 보존했다. 검증용 설정 JSON·임시 신호 정리 완료.

## Intro Storyteller_4 음성 적용 (2026-09-30)

- 외부 생성 WAV 18개를 기존 Intro Voice 경로에 원본 그대로 적용했다. 24kHz/mono/PCM16, 합계83.20초, SHA-256 동일·GUID/Catalog 유지. Narrator 표시명 Storyteller_4와 제작 메타데이터의 Voice ID를 [TTS 정책](TTS_음성_제작_정책.md)에 기록했다. 다른 캐릭터 Voice는 TBD. API/SDK/Key/HTTP 추가 없음. 계획 Docs `602b1b8`, 마지막 관련 기능 commit `7e60218`.
- 안정 Dialogue ID→Clip 연결 유지, Next 즉시 중지·다음 블록 시작, Skip/정상 종료/외부 전환·비활성화에서 Coroutine/음성 정리. 기존 자동 최소 시간과 Clip+0.5초 규칙 유지, 독립 Voice Volume 기반 추가. Story·설정 UI 변경 없음.
- 백그라운드 실제 Play Mode 전체19블록/156.47초·18개 순서/자막/Clip·중간 Next·같은 프레임8회 Next·Skip→CharacterCreation/잔류 Source0·정상 Replay→Bootstrap·재진입001·복제 Catalog null 자동 진행·외부 전환 정리 통과. Import18개/디코딩18개/컴파일 오류0. 세부 결과·QA 도우미 예외와 의도한 null 경고는 [Intro QA](Intro_Storyteller4_적용_QA.md)에 구분 기록한다.
- 사용자 확인/다음 권장 작업: 실제 청취로18문장 발화 일치·자동 분할 경계·음질/음량을 검수한다. 자막/ID/Clip의 구조적 대응과 실제 발화 청취를 구분하며, 체감 BGM 밸런스 및 실제 물리 입력은 미검증이다. 원본 재가공이나 새 음성 생성은 하지 않았다. GitHub push 없음.
- 최종 Console Error0/Warning0, Bootstrap clean Edit Mode, 포커스 전환 없음·Save 격리 해제·사용자 저장 해시 동일. 18개 모두 실제 음성 출력 신호 확인. 이번 변경 diff 검사 통과, 전체 작업 트리는 기존 사용자 Animation/Scene/meta trailing whitespace로 exit2(기존 변경 보존).

## Main15 Runtime QA와 TTS 정책 (2026-09-30)

- TTS 정본 [음성 제작 정책](TTS_음성_제작_정책.md)을 추가했다. 2026-09-29 개발 방침으로 Gemini 3.8 Flash/Flash-Lite TTS, Free Tier 우선, Voice Design/Scene Style 분리·Voice Registry TBD, 사전 생성 Audio Asset·Client Key 비포함, 다음 Intro 제작을 기록했다. 기존 MeloTTS 문서는 제작 이력으로 연결했다. API/음성/Cloud/Key/Audio Asset 작업 없음. 문서 commit `d9bfc68`.
- 격리 Play Mode에서 Main15 시작→12개 순차 목표→정식 세린 합류를 완료했다. 실제 왕복 Collider·Arbel Safe Zone 유지·적 AI에 의한 전멸 중앙 복귀·HP/MP 회복·회복약 소비 유지, 4조우 실제 전투 명령 승리·4종 분양 목록·그을음들개100 구매, 세린 사수/청각/Fox·기존 미엘/폴 편성 보존·Party Manager 동료2명 제한을 확인했다. 목표 근처 배치는 QA가 했으며 전체 수동 이동 입력 검증은 아니다. 세부 증거·한계는 [Main15 QA](../03_스토리/Main15_Runtime_QA_2026_09_30.md)를 따른다.
- **실제 버그 2건 수정/재검증**: Arbel 런타임 Bounds 초기화 순서로 저장 위치가 Spawn에 덮이는 버그를 수정해 완료 전 `(2,-4)`/후 `(1,-4)` 및 Field06 `(5,-2)` Continue를 확인했다(`cfadcc8`). Field06 북쪽의 복제 원본 출구 틈으로 맵 밖 이동되는 버그를 Collider1개로 막아 실제 물리 이동이 y14.5 대신 y6.7에서 차단되는 것을 확인했다(`1e5cb54`). 동쪽 왕복·중앙 Spawn 유지. 마지막 관련 기능 commit `1e5cb54`.
- **검증/남은 항목**: Unity6000.5.7f1 컴파일 오류0, 기능 검증 중 게임 Error/Warning0. 3비율 viewport Bounds 계산 통과. 이번 사용자 승인으로 1016×569 Game View 배치·Navigation·공식 외형·최종 KO 유지 일부 시각 QA를 수행했다. 초기 직사각형 환경 Patch, 작은 조사 TextMesh 가독성, 세린 Battle Portrait fallback/미엘 후열 겹침은 후속 검토이며 미술·기획을 임의 변경하지 않았다. 전체 이동 경로·NPC 겹침·KO 전환·모든 비율 UI·Monster 정밀 QA는 미검증이다. MCP 재컴파일 WebSocket Warning1과 캡처 도우미 PlayerLoop Error5를 게임 코드와 구분해 기록했고 캡처를 중단했다. 기록 후 Console 정리·Edit Mode 재조회, 사용자 슬롯 SHA-256 동일·QA JSON 삭제·캡처만 보존·Bootstrap clean 종료. 관련 diff 검사 통과, GitHub push 없음.
- 다음 권장 작업: Field06 미술/표식·세린 전투 외형/배치의 별도 검토와 실제 이동 경로/KO 전환 시각 검증. TTS 제작은 정식 정책대로 Intro Free Tier Pipeline을 별도 작업으로 시작한다. 기존 BeastCompanion/안전지대 문서의 과거 미구현 설명은 별도 문서 감사 대상으로 남긴다.

## Main15 「타오르는 흔적」 구현 당시 기록 (2026-09-29)

- `main_15_burning_traces` 12개 순차 목표와 레온 의뢰·세린 조사/자발적 합류 대화를 구현했다. Main14 완료 뒤 레온 대화로 시작하며, 서쪽 첫 그을음들개 승리만 지정 목표를 진행한다. Quest Navigation은 레온, Arbel 북서쪽 출구, 조사 지점, 첫 조우, 귀환 지점과 세린에 연결했다. 완료 시 기존 명단에 세린을 영구 해금하되 선택된 동료 2명과 진형은 바꾸지 않는다. 진행 중 세린은 Story Temporary이며 기존 사수·청각의 길·Fox를 사용한다. 영구 해금과 Quest·Beast·편성·위치·안전지대는 기존 Save 구조를 사용한다.
- `Field_06_ScorchedTrail` Scene과 Arbel 북서 출구↔Field06 동쪽 출구의 별도 왕복 Spawn을 등록했다. Spawn과 Exit은 떨어져 있으며 Arbel Safe Zone을 유지하고 Field06에는 새 Respawn 거점을 두지 않는다. 기존 Field Scene을 바탕으로 서쪽의 풀 감소·고사목 색·황토·균열·그을음 표식을 단계적으로 더한다. 일반 Spawn M15-01 그을음들개, M15-02 그을음들개+열풍매, M15-03 균열도마뱀, M15-04 화열딱정벌레+열풍매를 기존 몬스터 정의·Battle·Victory→Arbel 펫 분양 경로에 연결했다. 불씨망령·Boss·Dungeon·과열·Fast Travel·Main16 Quest는 구현하지 않았다. Main16의 방향만 설계 문서에 남겼다.
- **검증 상태**: Unity 6000.5.7f1 Edit Mode 컴파일 오류 0. 정적 Asset 조회에서 Main15 목표 12개, Serin 사수/청각의 길 Definition, Field06 Scene Build 등록, 4개 Spawn의 Monster 참조, Arbel 왕복 Connection을 확인했다. Console에는 기능 오류 없이 MCP WebSocket 초기화 경고 1건이 남았다. 요청에 따라 Play Mode·Game View·실제 Main15 진행·전투/분양·Save/Continue·전멸 복귀·화면 비율·시각 QA는 **미수행**이다. 다음 작업에서 이 경로들과 Field06 배치/Bounds/카메라를 실제 Runtime으로 확인해야 한다. 관련 설계 commit `d656f15`, 기능 commit `aea23bc`. GitHub push 없음.

## Chapter 2 도입부 핵심 Runtime QA (2026-09-29)

- Unity 6000.5.7f1 격리 Play Mode에서 Main11 완료·석판 보유 상태로 Main12를 시작해 석판→잡화 상인→기록→서쪽 결정→Field03 서쪽 출구 순서를 진행했다. Field04 도착으로 Main12가 완료되고 Main13이 시작됐다. Main13의 마른 토양·물길·세린 첫 대화 4페이지를 확인했다. 세린의 청각 보조 장치 설명 문구, 임시 동행 상태, 전투 참가자의 `companion_serin`/사수/`path.hearing` 및 초기 Fox를 런타임에서 확인했다.
- 지정 Story Battle 두 건은 실제 Battle Scene 진입과 stable encounter ID를 확인하고, QA에서 승리 결과 함수를 호출해 목표 진행을 검증했다. 수동 전투 명령으로 승리한 검증은 아니다. Field04→Arbel 최초 진입에서 Main13 도착·`safezone_arbel` 자동 활성화·고정 NPC 8명 및 PortalAnchor를 확인했다. 레온에게 보고해 Main13을 완료했다.
- Main14는 레온 시작→우물→관개수로→세린 재합류 대화→감시초소→지정 조우 승리 처리→강한 진동 4페이지→레온 보고까지 진행해 완료를 확인했다. Arbel 상점 UI, 치유사 치료 대화, 편성 UI, 펫 분양·관리 UI, Portal 관리인 준비 안내를 확인했다. 파티 안내인의 Arbel Scene 제한과 분양 확인 대화의 모달 소유권 버그를 최소 수정하고 재검증했다.
- 격리 슬롯 5의 Bootstrap Continue에서 완료된 Main14·세린 임시 상태·Arbel Safe Zone을 복원했다. 진행 중 Main14 4단계 저장 자료를 구성해 Continue 후 목표 ID, 세린 임시 상태와 안전지대 복원도 확인했다. Field05 전투에서 QA가 전멸 결과 함수를 호출하자 Arbel `Spawn_Arbel_Center`로 복귀하고 저장됐다. 실제 적 공격에 의한 전멸은 미검증이다. 코드 재컴파일 후 Unity Console Error/Warning 0건. 화면 배치·NPC 위치 미관·실제 전투 명령 승리/전멸·도망·전체 전투/Quest/Pet 회귀는 이번 범위에서 검증하지 않았다. 다음 권장 작업은 별도 시각 QA와 실제 전투 입력 확인이다. GitHub push 없음. 마지막 관련 수정 commit `7387023`.

- 2026-09-29 **Chapter 2 도입부 구현**: Main12 「남겨진 기록」·Main13 「메마르는 땅」·Main14 「땅 아래의 울림」의 순차 Quest Definition과 현장 조사·NPC 대사·지정 Story Battle 승리 목표·Navigation을 연결했다. Field_03 서쪽 출구, `Field_04_WesternBorder`→`Arbel`→`Field_05_WesternOutskirts` 왕복 Scene/Spawn/경계 개구부를 추가했다. 새 Field와 Arbel Scene은 기존 Scene 복제 기반 초기 배치이며 환경·NPC 위치는 시각 QA 전이다. 기존 Chapter 2 몬스터 Definition을 새 Field 일반 Spawn과 지정 조우에 재사용했다. 아르벨 NPC의 기존 Sprite 매핑 누락은 정적 확인 중 보완했다. 설계 commit `c25d319`, 기능 commit `019454b`, 외형 연결 fix commit `f67a548`.
- 세린 `companion_serin`은 공식 방향 Animator와 초상화, `path.hearing`/사수 Definition을 연결했다. Main13 공동 전투~아르벨 도착, Main14 재합류 뒤 전투 참가 여부는 Quest 단계로 복원하며 영구 명단에 해금하지 않는다. 초기 Beast는 Fox다. 임시 세린은 현재 Pet 관리 화면의 정식 사수 목록에서는 제외된다. 아르벨에는 레온·상점·무료 치유·파티 안내·펫 분양/관리·주민·Portal 관리인과 준비 상태 상호작용을 연결했다. 정상 도착 시 기존 Safe Zone을 `safezone_arbel`/`Spawn_Arbel_Center`로 활성화하고 슬롯에 저장하도록 했다. 실제 Fast Travel과 Main15는 만들지 않았다.
- **검증 상태**: 구현 직후에는 정적 확인만 수행했으며, 이후 핵심 Runtime QA 결과와 남은 항목은 이 문서 맨 위에 기록했다.

- 2026-09-29 **Chapter 2 Monster 5종 Runtime 및 Pet 4종 연결**: 수정 공식 PNG 5개 모두 1256×1256 RGBA, SHA-256 원본/복사본 일치. Unity Importer에서 각 314×314·16프레임·314 PPU·Point·하단 중앙 Pivot을 확인하고, 명시적 Idle/Basic/Skill·Hit/KO 프레임과 5종 Definition을 연결했다. KO는 1회 재생 뒤 마지막 Down 프레임 유지, 별도 Walk 원본이 없어 Field 이동에는 Idle을 사용한다. 5종의 확정 Level/HP/Attack/Agility/EXP/Talent·전후열과 행동 배율, 개체별 예고/방어/재사용 순서, 공용 Burn을 연결했다. Loot·Chapter 2 Field/Spawn/Scene·Overheat·Serin Runtime은 추가하지 않았다. 기능 commit `d8df236`.
- **검증**: 포커스 없는 Unity 6000.5.7f1 Edit Mode에서 5종 Sprite 참조 16개씩·누락 0·수치/Formation/공격 범주, 5종 행동 순서와 균열 돌진·열핵 분출 예고 소비, 갑각 수축 직접 피해 100→60/DoT 100/다음 자기 행동 100을 확인했다. 사수 펫은 그을음들개 습격 36+Burn 2회, 열풍매 후열 습격 36→40, 균열도마뱀 첫 직접 피해 100→80·다음 100·DoT/0 미소비·새 전투 충전, 화열딱정벌레 첫 유효 Burn 무효·둘째 적용·새 전투 충전을 확인했다. 불씨망령은 펫 목록에서 제외된다. 포커스 없는 격리 Play Mode에서 `RecordVictory` 서비스로 승리 종을 기록한 뒤 4종 각 100 탈렌트 분양(400→0), 불씨망령 구매 거절, 장착, 저장 파일 불러오기·이어하기 복원을 확인하고 임시 슬롯 폴더를 삭제했다. 기존 독침벌 습격 36+독, Chapter 1 펫 5종 카탈로그, Wolf/Bear/Fox 기본 이용권·Bear HP 100→110·Fox 민첩 10→12를 자동 회귀 검사했다. 최종 Edit Mode, Bootstrap Scene, Editor 비활성 포커스, Console Error/Warning 0. 기능 스테이징 `git diff --check` 통과.
- **남은 확인**: 실제 Chapter 2 Field/Spawn이 없어 5종의 전체 Battle Scene 연출·사망/승리/Flee/Defeat 분기와 펫 외형 이동을 실전 조우로 확인하지 않았다. Game View의 KO 순서·Down Hold 시각 품질, 신규 분양소 스크롤/여러 화면 비율, 전체 기존 스킬 회귀도 미검증이다. 화면 포커스 전환이 필요한 시각 QA는 이번 작업에서 실행하지 않았다. 다음 권장 작업은 별도 승인된 격리 시각 QA 또는 Chapter 2 Field 설계 확정 후 실제 조우 연결이다.

- 2026-09-29 Chapter 2 일반 몬스터 5종의 사용자 확정 1차 Level/HP/Attack/Agility/EXP/Talent·스킬 배율·AI 방향·펫 효과를 설계 정본에 반영했다. 열풍매 펫은 기존 원거리 피격 -5% 안을 폐기하고 후열 대상 습격 최종 직접 피해 ×1.10으로 교체했다. 제공된 공식 PNG 5개는 모두 존재하고 1254×1254 RGBA이지만 양 축이 4로 나누어지지 않아 요청된 동일 크기 4×4 Slice 조건을 충족하지 않는다. 원본 변경·강제 Crop/Resize·대체 없이 5종 Asset Import와 이에 종속된 MonsterDefinition/AI/Pet Runtime·검증을 중단했다. 원본 SHA-256은 그을음들개 `459D3F08`, 열풍매 `ED35ECBA`, 균열도마뱀 `DF6AC182`, 화열딱정벌레 `F3859DDB`, 불씨망령 `975B13F0` 접두다. 문서 링크·staged `git diff --check` 통과, Unity Play Mode 미실행. 다음 작업은 4×4로 정확히 분할 가능한 공식 수정 시트 확보 후 구현 재개다. 관련 문서 commit `8ab63ed`.

- 2026-09-29 Chapter 2 몬스터 5종의 구현 전 파이프라인을 LOCAL 코드·Asset 기준으로 조사하고 종별 준비 체크리스트를 문서화했다. 기존 10종 Definition/Formation/Spawn/Animation, 공용 Burn, AI·보스 예고, BeastCompanion 승리→분양→장착 경로를 대조했다. 열풍매 원거리 직접 피해는 현 피격 API에서 안정 분류 불가로 구현 보류가 필요하다. 상대 링크·주요 코드 경로·staged `git diff --check` 통과. Unity Play Mode는 실행하지 않았으며 Chapter 2 최종 Sprite/수치/Definition/AI/Spawn/Pet Runtime은 미구현이다. 다음 작업은 최종 Asset·수치 확정 후 준비 문서의 순서대로 별도 구현·검증하는 것이다. 관련 문서 commit `74e5f37`.

- 2026-09-29 몬스터 설계 정본에 신규 종의 전투 특징·펫 가능 여부·계승 효과를 함께 정의하는 영구 규칙과 템플릿을 추가했다. Chapter 2 일반 몬스터 5종(그을음들개·열풍매·균열도마뱀·화열딱정벌레·불씨망령), 작열 감시자 Elite 후보, 기존 Burn 활용과 Overheat 미구현 후보를 문서화했다. 상대 링크와 staged `git diff --check` 통과. Unity는 실행하지 않았으며 Asset·MonsterDefinition·AI·Spawn·Pet Runtime은 미구현이다. 다음 작업은 별도 승인된 Chapter 2 구현에서 미확정 수치·피해 분류를 검토하는 것이다. 관련 설계 commit `3745968`.

- 2026-09-29 세린 공식 시각 자료 준비: 사용자 확정 Portrait(1374×1145 RGB)와 4×4 Sprite Sheet(512×512 RGBA)를 원본과 동일한 SHA-256으로 등록했다. Sheet는 128×128 16프레임, 128 PPU, Point, 하단 중앙 Pivot이며 방향별 Idle 4개·Walk 4개와 기존 플레이어 방향 상태 구조의 Animator를 준비했다. Unity Editor에서 Portrait/UI Sprite Import, 16프레임 크기·Pivot, 8개 상태의 Sprite 참조 누락 0건을 확인했고 Console Error/Warning 0, staged `git diff --check` 통과했다. Story/Companion/Battle Runtime·Main12/13·Chapter 2 Scene·BeastCompanion 세린 연결은 미구현이며, 실제 Scene 등장/화면 미관은 검증하지 않았다. 다음 작업은 별도 Chapter 2 기획·구현 시 이 자료 연결이다. 관련 기능 commit `9177f28`.

- 2026-09-29 BeastCompanion 실제 Play Mode QA: 사용자 승인 범위의 격리 슬롯과 검증용 임시 사수 Asset을 사용했다. 실제 전투 명령으로 도망 시 승리 기록·분양 후보 0과 필드 스폰 유지를 확인했고, 전멸은 검증 입력으로 아군 HP를 0으로 만든 뒤 Battle 종료 경로를 실행해 기록 0·안전지대 복귀·스폰 유지를 확인했다. 적 HP를 0으로 만든 실제 Battle 승리 종료 경로에서는 `venom_bee` stable ID 기록·분양 목록 등장·결과 화면 귀환 후 스폰 처치가 확인됐다. 분양 버튼으로 독침벌 100 탈렌트 구매(260→160), 관리 화면 장착, Bootstrap `이어하기` 후 승리/해금/잔액/장착 복원을 확인했다. 검증용 사수 A=Wolf/B=Bear는 같은 전투에서 별도 장착·A 기본 HP/B 120→132 HP·직접 피해 A 20→21/B 20 유지·각자 Wolf/Bear 습격·이어하기 후 별도 ID 유지를 확인했으며 정식 동료는 추가하지 않았다. 8종 모두 장착 후 실제 습격에서 자기 ID의 Image 생성, 최소 2개 Sprite 프레임 교체, 위치 이동, 종료 뒤 오브젝트 정리를 확인했다. 독침벌 습격은 저레벨 대상 HP 70→1, 생존 대상 일반 독, CD3을 확인했다.
- 전투/UI 회귀: 정조준 후열만 선택·적중·쿨타임, 화살비 후열 적중과 후열 부재 시 행동/CD 미소비, 기본 공격 대상 선택·종료, 회복약 대상 선택 취소 시 2개 유지와 사용 후 1개·HP 회복, Guard 피해 10→5 및 다음 자기 차례 종료, 일반전 Flee를 실제 Play에서 확인했다. 분양·관리 Game View는 1016×569(약 16:9), 1280×800(16:10), 800×600에서 목록·설명·조작 버튼 접근성을 확인했다. 고정 5종 목록은 ScrollRect 없이 패널에 들어간다. 관리 사수 선택 단계의 빈 Image 흰 사각형을 수정해 선택 전/복귀 시 숨김·상세 선택 시 재표시를 Play Mode에서 재검증했다(Fix commit `6661f59`). 최종 Editor 컴파일 대기 없음·Console Error/Warning 0. 검증용 Asset, 격리 저장, 화면 크기 및 임시 Screenshot은 정리했다. 도발 강제 대상의 실제 재현, 8종 외형의 세밀한 미관 판단, 모든 지역·모든 전투 스킬의 전체 회귀는 이번 QA 범위에서 미검증이다. `문서/10_전투/전투시스템.md`의 Beast 패시브/Wolf 전용 서술은 현재 설계·코드와 충돌해 별도 문서 정리가 필요하다.

- 2026-09-29 BeastCompanion 잔여 백그라운드 검증: Unity 6000.5.7f1의 포커스를 받지 않은 Edit Mode에서 `TurnOrderQueue`를 실제 생성하여 동일 우선도 민첩 10 사수/11 적의 순서가 Wolf일 때 적→사수, Fox일 때 사수→적으로 바뀜을 확인했다. 8종 Definition ID와 사용 가능한 원본/생성 프레임 수는 Wolf 6, Bear 5, Fox 6, 독침벌 10, 맹독뱀 4, 숲거미 8, 이끼딱정벌레 6, 그늘박쥐 6이었다. 격리 서비스 상태 검사에서 승리 종 중복 제거, 승리 후 분양 후보 표시, 구버전 Beast 필드 누락 및 잘못된 장착 ID의 Wolf fallback, Beast 저장 DTO 왕복과 Guard 직접 피해 20→10을 확인했다. Console 오류·경고 조회는 0건이다. 코드 수정은 없었다. 실제 Battle의 Victory/Flee/Defeat 기록 분기, 구매/Continue 최종 회귀, 두 사수 독립 장착, 8종 습격의 화면 재생·종료 정리, Skill/Item/Attack/Flee 전체 행동 회귀, UI 화면 비율별 시각 QA는 이번 백그라운드 검사에서 미검증이다. 이전 격리 Play 검증 결과는 아래 기록과 같다. 다음 권장 작업은 포커스 전환 없는 Play Mode 테스트 하네스 확보 후 남은 전투·저장 회귀와 별도 시각 QA다.

- 2026-09-29 사수 전용 BeastCompanion 공용 상태, 무료 Wolf/Bear/Fox 장착, 몬스터 펫 5종의 승리 기록→100 탈렌트 분양→영구 해금, 캐릭터별 장착 Save, 분양·관리 UI 호출 지점, 패시브와 `동료의 습격` 외형·독 효과를 연결했다. Chapter 2 시설·NPC·Scene과 세린은 만들지 않았다. 설계 commit `8ee189f`, 기능 commit `fe0ce94`, 검증 수정 commit `106bae1`.
- 검증: Unity 6000.5.7f1 컴파일 및 최종 Console 오류·경고 0. 격리 계산에서 Wolf 100→105/DoT 100, Bear 80↔88 및 1~100 HP 왕복 무증가, Fox 민첩 +2, 독침벌 180 피해·일반 독 3회, 뱀·박쥐 첫 유효 상태 1회 차단과 새 Battle 충전, 거미 광역 100→90·Guard 45/단일·DoT 100, 딱정벌레 직접 100→95/DoT 100, 기본 3종 사용권·구버전 Beast 누락→Wolf·중복 승리/잔액 부족 구매를 확인했다. 5종 원본 프레임 10/8/4/6/6개를 읽었다. 사용자 승인 후 격리 Play Mode에서 분양 숨김→승리 후 표시, 100 탈렌트 구매·중복 거절, Bear/Fox/벌 장착, 150 탈렌트·장착·승리 기록 Save/Continue, 분양·관리 UI 표시와 Bear 버튼 장착을 확인했다. 화면 전환 직후 옛 행 비활성화도 확인했다. 두 사수 독립 장착, 실제 전투에서 8종의 등장 애니메이션, 전투 명령·상태 전체 회귀와 여러 화면 비율의 미관은 미검증이다. 다음 권장 작업은 이 격리 전투 회귀와 Game View 비율별 QA다.

- 2026-09-28 Main11 `main_11_silent_catacomb` 「침묵의 지하묘지」를 Main10 완료 후 기존 Field_03 입구에 연결했다. B1/B2 조사·하강로·침묵의 파수꾼·보스 뒤 봉인실·부서진 석판 조각·귀환의 순차 목표 13개, 대사와 Navigation 지점, 입구 안전지대 완료 보상 EXP60/탈렌트50을 구현했다. Quest Item은 판매·사용 불가이며 완료 뒤에도 유지된다. Main12 Definition/Scene은 만들지 않았다. 정식 설계 commit `02beeed`, 기능 commit `fdb90e7`.
- 검증: Unity 6000.5.7f1 백그라운드 Play에서 Main10 완료 상태의 Field_03 실제 입구→Main11 시작, B1 목표 1~5 순차 진행·B2 진입, B2 목표 6~9 순차 진행, 보스 승리 시 10번 목표만 완료, 봉인실 조사·석판 1개 획득·`아직 조사한다` 선택 유지·`돌아간다` 선택 후 안전지대에서만 전체 완료를 확인했다. 보스 전멸은 목표 미진행·입구 안전지대 복귀·재도전 가능이었다. 13번 목표에서 저장/복원해 석판·보스 완료가 유지됐고, 완료 뒤 재알림·재저장/복원에서도 EXP 180·탈렌트 85(보스+Quest)가 늘지 않았다. Quest Item 판매 거절 및 B2 남쪽 출구→B1 북쪽 Spawn 복귀를 확인했다. 컴파일 오류 0; 슬롯 없이 출구만 확인한 테스트의 자동 저장 건너뜀 경고 3건, 최종 재컴파일 후 Unity MCP WebSocket 경고 1건은 기능 오류와 구분한다. 이번 기능 스테이징의 `git diff --check` 통과. 실제 Game View 화면 비율별 미관·글자 배치, B1/B2 일반 조우 전체와 기존 Skill 전체 회귀는 미확인이다. 다음 권장 작업은 이 시각 QA와 남은 회귀 검사다.

- 2026-09-28 `Dungeon_01_B2`의 봉인 구역 공간, 일반 조우 B2-01~04, 침묵의 잔영·봉인 수호체·첫 보스 침묵의 파수꾼을 구현했다. 최종 PNG 4개를 원본 그대로 등록하고 128×128 Sprite Slice 및 전투 아이템·해제약 아이콘을 연결했다. 보스는 표식·예고형 충격과 HP 60% 이하 1회 수호체 2기 소환·직접 피해 장막을 사용하며, Main11과 보스 뒤 스토리 단서는 아직 구현하지 않았다. 문서 commit `0440cf2`, 기능 commit `a393e5c`.
- 검증: Unity 6000.5.7f1 컴파일 성공. 격리 Play Mode에서 B2-01 접촉·침묵·전투 아이템 침묵 해제·도망·복귀, 보스 접촉·도망 차단·첫 공격·표식·충격 예고→다음 행동 전체 공격과 Guard 피해 31→16, 60% 전환·수호체 2기·장막 직접 피해 100→70/DoT 100 유지·수호체 전멸 시 장막 해제, 보스 승리·복귀·스폰 비재생성을 확인했다. 순수 로직 검사에서 잔영의 자기 행동 2회 재사용 제한, 수호체의 직접 피해 감소·DoT 제외·KO 해제를 확인했다. 감사 캐릭터에 지정한 유효하지 않은 `path_intellectual` 때문에 저장 경고가 21건 발생했고, 재컴파일 후 Unity MCP WebSocket 경고 1건이 남았다. 일반 `git diff --check`는 Unity 생성 YAML의 빈 값 뒤 공백을 지적하지만 이를 제외한 검사에는 오류가 없다. B2-02~04 실제 전투 승리, B1·기존 스킬 전체 회귀, Game View 시각 QA는 아직 확인하지 않았다. 다음 작업은 올바른 경로 ID의 격리 슬롯으로 나머지 조우와 시각 QA를 확인하는 것이다.

- 2026-09-28 전투 공용 아이템 명령과 생존 아군 대상 선택·취소·성공 후 공용 Inventory 소비, HP/MP 회복 및 Poison/Burn/Shock/Silence 전용 해제약을 구현했다. 4종 가격은 각 구매 40/판매 20 탈렌트다. Field03 지하묘지 입구에 전투 없는 안전지대, 보급 상인(침묵 해제 물약 최초 판매), 파티 안내인, 기존 무료 완전 회복 치유사, 상주 묘지 감시인을 런타임 설치한다. 전멸은 최근 활성 안전지대 Spawn으로 복귀하고 HP를 회복하며 소비품·EXP·탈렌트·장비·영구 진행은 되돌리지 않는다. Boss는 아직 없고 전투 임시 상태는 Battle Scene 종료로 초기화된다. 문서 commit `e150349`, 기능 commit `94dd402`.
- 검증: Unity 6000.5.7f1 컴파일 및 최종 Console 오류·경고 0. 격리 검사에서 전용 해제약 4종의 개별 상태 제거·무효 사용 미소비, HP 회복, 안전지대 저장 JSON 왕복과 Version1 누락 필드를 확인했다. 사용자 허락을 받아 격리 저장 슬롯의 실제 Play Mode에서 Player·태온·미엘·폴의 아이템 명령, 전열↔후열 대상, 취소·무효 사용 미소비, 성공 사용 후 행동 진행, 침묵 중 스킬 차단과 자가 해제, 정화의 4종 동시 제거를 확인했다. Field03의 NPC 4명 생성, 보급 상점·파티 UI·무료 치유사·감시인 대화, 몬스터 활동 중심의 안전지대 비침범, Main10 입구 진입을 확인했다. Dungeon 일반 조우 전멸 후 Field03 안전지대 복귀, HP 회복, 소비품·EXP·탈렌트 보존, 일반 몬스터 재전투 가능, 저장·복원된 안전지대와 공격·방어·도주 기본 회귀를 확인했다. 보급 상점과 파티 UI는 직접 열어 확인했으나 해당 NPC의 상호작용 버튼 경로, Bootstrap Continue UI, 전용 해제약 3종의 실제 Battle UI, 기존 스킬 15개 전체 회귀, Boss Play와 화면 비율별 시각 QA는 후속 확인 사항이다. 다음 권장 작업은 남은 UI 경로와 화면 비율 확인이다.

- `Dungeon_01` 「침묵의 지하묘지 B1」의 입구 홀·중앙 회랑·서쪽 안치실·동쪽 무너진 묘실·북쪽 봉인실 Scene과 기존 몬스터 일반 조우 4개를 구현했다. Main10 완료 격리 슬롯의 Field03 입구 상호작용→Dungeon Spawn, B1-01 실제 충돌·UI 공격/스킬/방어·승리(EXP16·탈렌트9·드롭 없음, Lv12), 결과 버튼 복귀·해당 스폰 제거, B1-02 UI 도망·스폰 유지·복귀 직후 1.99초 재조우 유예, B1 남쪽 출구 Trigger→Field03 복귀를 Play Mode에서 확인했다. Bootstrap Continue의 던전 좌표 복원과 Bounds 밖 좌표의 Spawn fallback도 확인했다. 기능 버그·코드 변경은 없었다. 감사 중 예상된 좌표 fallback/슬롯 해제 경고 2건을 기록했고 최종 Console 오류·경고 0건이다. 전용 묘지 Prop이 없어 기존 Kenney 회색 석재 Sprite로 단순하게 표현했으며 B2·Main11·새 몬스터·보스는 없다. 다음 작업은 별도 Main11 기획 전 실제 이동 난이도·석재 미관 확인이다. 기능 commit: `c72d771`.

- 공용 해로운 상태 `침묵 1`을 구현했다. 다음 성공한 실제 행동까지 Skill 명령만 차단하고 공격·방어는 허용하며, 완료 때 제거한다. 재적용은 중첩 없이 갱신하고 정화는 독·화상·감전·침묵을 한 번에 제거한다. 침묵 치유사는 자기 정화를 사용할 수 없다. HUD·상세에는 아이콘 없이 한글 fallback을 표시한다. 새 몬스터·부여 스킬·Dungeon·Quest는 추가하지 않았다. Runtime/Editor 백그라운드 컴파일 오류 0, Scene·저장 비변경 Editor 감사 통과, Unity Console 오류·경고 0. 실제 Battle Game View의 배지 가독성과 입력 포커스는 사용자 확인 사항이며 다음 작업은 별도 기획에 따라 침묵 부여 수단을 연결하는 것이다. 기능 commit: `3c8de16`.

- World EXP HUD를 좌상단 Anchor `(0,1)`, 좌·상단 24px 여백, 320×72 패널과 280×9 EXP Bar로 조정했다. 아이콘·레벨·이름·EXP 숫자를 첫 줄에 분리하고 EXP/레벨/저장 로직은 변경하지 않았다. 좁은 화면의 Quest Toast는 위쪽 24px을 유지하면서 오른쪽으로 옮겨 가로 간격 16px을 확보하고, Quest Navigation 마커는 EXP 패널과 겹칠 때만 아래로 피한다. 향후 EXP 위치 프리셋은 Anchor·Pivot·여백으로 확장할 수 있다. 백그라운드 Runtime/Editor Roslyn 컴파일 오류 0, 사용자 승인 후 1016×569 Game View 배치와 Play Mode HUD 감사·마커 회피 검사 통과, Console 오류·경고 0. `AGENTS.md`는 백그라운드 우선과 foreground 사전 승인 원칙으로 보완했다. HUD commit: `a4e5a63`, Toast 충돌 수정: `1fd64f3`, Navigation 충돌 수정: `96b90f9`, 규칙 commit: `046ca33`.

- 2026-09-24 유지보수: 실제 `BattleSkillCatalog`·상태 런타임·길 특성과 문서를 대조해 스킬/전투 정본, 미구현 후보, 과거 프리뷰를 분리했다. 프로젝트 전용 핵심 C#의 Quest·Save·Battle 길 특성·Companion·Navigation 설명 주석을 보강하고, `AGENTS.md`에 비전공자용 주석과 백그라운드 검증·화면 포커스 보호 규칙을 추가했다. 기능 동작은 변경하지 않았다. 백그라운드 Roslyn 전체 Runtime 컴파일 오류 0, 변경 Markdown 상대 링크와 관련 파일 diff 검사는 통과했다. Unity Editor가 실행 중이지 않아 Test Runner·Console·Main10 최종 Game View는 이번 세션에 재확인하지 못했다. 입구 시각 확인은 사용자 직접 확인 사항이다. 공식 `마음의 상처의 길`과 현재 Path Asset 표시명 `마음의 상처`의 불일치는 별도 Asset 수정 사항이다. 관련 commit: `d6e2fb2`(문서), `95ae1df`(규칙), `39ca7bd`(주석).

- Main10 `main_10_center_of_silence` 「침묵의 중심」의 7개 순차 조사 목표를 Field03에 구현했다. Main09 석재를 재사용하고 입구를 단계별로 공개한다. 완료 보상은 EXP40·탈렌트40·아이템 없음이며 Dungeon_01 Scene이 없을 때 입구는 안내 대사로 안전하게 끝난다. 원인·배후·빛의 정체는 공개하지 않았다.
- 어제 Unity Play Mode의 격리 저장 감사에서 61개 검사가 통과했고, 입구 위치를 위로 조정한 뒤 재실행한 감사도 통과했다. 이번 세션에는 Unity MCP와 Editor가 연결되지 않아 조정 후 최종 Game View 시각 확인 및 Console 재확인은 수행하지 못했다. 사용자 확인: Field03 실제 화면에서 입구와 HUD의 겹침, 조사 표식의 가독성을 확인한다. 다음 권장 작업은 이 시각 확인 후 Dungeon_01 별도 기획이다. 기능 commit: `91eee5f`.

- Main09 `main_09_reunion_in_silence` 「침묵 속의 재회」 구현: Field03의 푸른 빛 방향→평행 바퀴 자국→폴 재회→정보 교환→가공된 석재→폴의 합류 결정, stable Objective 6개. 원인·배후·빛의 정체는 미확정이다. 완료 보상 EXP35·탈렌트35·아이템 없음, `companion_paul` 1회 영구 해금. 이후 Main10도 위에 기록한 범위로 구현되었다.
- Main05 완료 후 시작 마을의 기존 파티 안내인에서 Player 고정+해금 동료 최대 2명 편성, 전열/후열, 초상화·직업·길·선택 상태, 확정/취소, 공격 역할 부족 경고를 제공한다. Guardian는 폴 최초 해금 시 미엘+폴, Healer는 태온+폴, Sharpshooter/Fighter/Mage는 태온+미엘을 기본 선택하며 수동 편성은 보존한다.
- 일반 Battle은 저장한 동료·행을 사용한다. Main03/04/07 Story 임시 편성은 독립적으로 유지된다. Save Version1에 선택적 Formation·수동 편성 플래그를 추가했고 구버전 누락 필드는 기본값으로 복원한다. `UserData/Main09Audit` 격리 폴더에서 A~E Save→Bootstrap→Continue, 세 일반 전투 조합 승리, 폴 마도사 스킬, Main07 Story 참가 구성, Main08→09 같은 Scene 연결을 Unity Play Mode 115개 검사로 확인했다. 상점·인벤토리 및 기존 퀘스트 서비스 감사도 통과했다.
- 사용자 확인: 마을 파티 화면의 실제 해상도별 글자·초상화 배치와 Field03의 조사 표식이 지형과 겹치지 않는지 Game View에서 시각 확인. MCP 화면 캡처가 오래된 프레임을 반환하고 외부 패키지의 PlayerLoop 진단을 남겨 자동 시각 QA는 제외했다. 이 문단의 다음 작업 제안은 Main10 구현 전 기록이며 현재 계획은 문서 상단을 따른다.
- 관련 기능 commit: `a93fef8`(파티), `6b9829d`(Main09). GitHub push는 하지 않았다.

- `Grave_Wight_Battle_Final.png` 사용자 최종 원본을 수정 없이 정식 등록하고 `monster_grave_wight` 묘지 망자를 구현했다. Lv7·HP125·공격20·민첩8·EXP32·탈렌트9의 느린 전열형 언데드이며 향후 `Dungeon_01` 「침묵의 지하묘지」에서 사용한다.
- 실제 1254×1254 RGBA의 Alpha 배치를 분석해 209×220px 6프레임씩 Idle/Walk/Attack/Hit/Defeat를 연결했다. Defeat는 왼쪽→오른쪽 F00→F05로 한 번 재생한 뒤 마지막 프레임을 유지한다. Point·Clamp·Mip Map Off·sRGB/Alpha On·Uncompressed·PPU 209로 Import했다.
- `material_grave_wight_fragment` 망자의 파편(판매 8 탈렌트, 드롭 35%, 아이콘 null/fallback)을 추가했다. 격리된 Battle 검증에서 묘지 망자 1마리와 전열 2마리 구성, 우향, 애니메이션 전환, HP/공격/민첩, 2마리 EXP64·탈렌트18·드롭 집계를 확인했다. 기존 6종 Definition과 이끼갑충·그늘박쥐 프레임 연결을 회귀 검사했으며 최종 Console Error/Warning 0이다. 기능 commit: `e80943f`.

- Main 02 지정 전투 진행을 `FieldMonsterSpawnDefinition` 객체 참조가 아니라 전투 진입 시 보존한 stable encounter ID로 판정하도록 수정했다. `field01_main02_investigation_encounter` 승리만 Objective 2→3을 진행하며 다른 일반 조우·도주·패배는 진행하지 않는다.
- 추적 중인 Quest Objective의 실제 Transform을 stable ID Registry로 연결하고, 화면 안 `◆ 현재 목표 + 대상명`, 화면 밖 방향 화살표·거리 Indicator를 표시하는 공용 Quest Navigation을 추가했다. Quest Log의 선택과 추적을 분리하고 `추적` 버튼·T·게임패드 Y를 지원하며 Modal 중에는 안내를 숨긴다.
- Main 01~08의 현재 Scene에서 안전하게 식별되는 NPC, 조사 지점, 흔적, 지정 Story Encounter를 연결했다. Scene 밖 목적지로 향하는 출구 안내는 후속 확장 대상으로 남겼다.
- Unity Play Mode에서 Main 02 지정/일반 encounter 분리, Objective 1/2/3/완료 JSON 저장·복원, 화면 안 Marker, 화면 밖 Clamp Indicator, Quest Log 추적 표시를 검증했다. 기능 commit: `ba9ef7c`.

- Field 03 `침묵의 숲길`과 Main 08 `main_08_what_they_avoid` 「피하고 있는 것」을 구현했다. 회피 흔적 3개, 폴의 안정적인 바퀴 자국, Field 03 조사 흔적, 깊은 구역의 푸른 빛까지 stable ID 7개로 진행하며 보상은 EXP 30·탈렌트 30·아이템 없음이다.
- 사용자 최종 원본 `Moss_Beetle_Battle_Final.png`, `Shade_Bat_Battle_Final.png`를 수정 없이 등록하고 이끼갑충·그늘박쥐의 명시적 Idle/Walk/Attack/Hit/Defeat, 민첩·보상·재료 Loot를 연결했다. Field 03 조우 수는 초입 3 > 중간 2 > 깊은 곳 1이다.
- Unity 정적 로드와 Field 03 Play Mode에서 Scene·Player·전환 1개·몬스터 6개·프레임 애니메이터 6개, 그늘박쥐 일반 전투 진입과 화면 방향을 확인했다. 두 몬스터의 Attack·Hit·Defeat 최종 프레임 전환과 Encounter A/B/C 구성을 런타임 검사했다. 직접 Scene 진입 검증에서는 저장 슬롯이 없어 자동 저장 생략 Warning 2건이 발생했으며 기능 오류는 없었다. Save A~G의 실제 Bootstrap 왕복과 모든 승패/도주 조작은 후속 수동 확인이 필요하다. 기능 commit: `1c10492`.

- Main 07 `main_07_deep_tracks` 「깊게 패인 흔적」을 구현했다. Main 06 완료 뒤 `Field_02`에서 두 줄 바퀴 자국 조사 → 부상자 확인 → 자국 추적 → 폴 첫 대화 → 지정 전투 → 미엘에게 복귀 → 미엘·폴 첫 만남 → 폴 작별 순서로 진행하며, stable Objective/Actor/Encounter ID 8개로 저장한다.
- 폴(`companion_paul`)은 28세 남성 마도사·지체의 길 휠체어 사용자다. 공식 `Portrait_Paul.png`와 `Paul_Battle_Final.png`를 연결했고, 전투 시 후열에서 기존 파이어볼·썬더볼트·가이아 웰과 지체의 길 보정을 사용한다. Main 07에서는 Player·Taeon·Paul 대 숲거미 2·맹독뱀 1의 Story Temporary Companion으로만 참가하며, 미엘은 전투에서 제외되고 폴은 영구 해금/기본 파티에 추가되지 않는다.
- Main 07 보상은 EXP 30·탈렌트 25·아이템 없음이며 완료 신호 반복에도 한 번만 지급된다. 임시 슬롯 5에서 A~G(Available, 각 핵심 진행 단계, Completed)를 실제 저장→로드→복원해 Scene·Objective·파티·보상과 중복 방지를 확인하고 삭제했다.
- Unity MCP에서 폴 초상화 stable ID 조회, 전투 시트 33프레임, Idle·Attack·Guard·Skill·Hit·Defeat Animator, 후열 마도사/지체의 길, 마도사 3스킬, Main 07 참가자 구성을 확인했다. Main 07 필드 진행·대화 Modal·보상도 Play Mode에서 검증했으며 최종 Console Error/Warning 0이다. 기능 commit: `48420ea`. 이 문단의 다음 작업 제안은 당시 기록이다.
- Main 06 `main_06_into_the_forest` 「숲으로」를 구현했다. Main 05 완료 직후에는 Available을 유지하고 정상 동선으로 `Field_01`에 나서면 시작된다. `Field_02` 진입 → `field02_main06_anomaly_trace` 조사 두 목표를 stable ID로 진행하며, 흔적은 몬스터들이 평소 길을 벗어나 한 방향으로 넓게 퍼진 사실만 전달한다.
- 조사 대화는 태온 2쪽·미엘 1쪽·플레이어 1쪽으로 구성했다. 태온·미엘 공식 초상화와 플레이어 null-safe 배치를 기존 `DialoguePortraitCatalog`로 사용하고, 대화 완료 뒤에만 퀘스트를 완료한다. 보상은 EXP 20·탈렌트 20·아이템 없음이며 반복 신호에도 한 번만 지급된다.
- Unity MCP Play Mode에서 Main 05 완료→Main 06 Available, Field_01 시작, Field_02 진입 목표 갱신, 조사 대화 Modal 열기/닫기, 완료와 EXP/탈렌트 20 지급, 중복 방지를 확인했다. 임시 슬롯 5를 재사용해 A~E 상태를 각각 저장→로드→복원하고 Scene·Objective·태온/미엘 정식 동료·보상을 확인한 뒤 삭제했다. Field_02 기존 몬스터 6개 로드와 최종 Console Error/Warning 0을 확인했다. 기능 commit: `a4b50a5`.
- Main 05 `main_05_return_of_three` 「돌아온 세 사람」을 실제 구현해 Main 01~05의 1장 도입부를 마무리했다. Main 04 완료 뒤 `Field_01`에서 시작해 시작 마을 귀환 → 남문 경비병 보고 → 주민 대표 최종 보고의 stable Objective 3개로 진행한다.
- 보고 대화는 Main 04의 다중 화자 구조를 재사용한다. 남문 경비병·주민 대표는 Sprite 미연결 시 null-safe 글자 배치, 태온은 `Portrait_Taeon`, 미엘은 `Portrait_Miel`, 플레이어는 초상화 숨김으로 전환된다. 결론은 초원 너머 숲 `Field_02`에서도 비슷한 일이 있다는 방향까지만 제시하고 이상 이동의 원인은 확정하지 않는다.
- Main 05 완료 보상은 기존 `RewardBundle`의 EXP 40·탈렌트 40·`item_healing_potion_small` 회복약 1개다. 보상 지급 뒤 `companion_taeon`·`companion_miel`을 stable ID 기반 정식 동료로 해금하고 기본 파티를 Player·Taeon·Miel로 저장한다. 기존 Version 1 Save에 동료 필드가 없으면 미해금 상태로 호환하며 Main 03/04 Story Encounter의 임시 참가 구조는 유지한다.
- Unity MCP Play Mode에서 Quest Log 목표·보상, Field 귀환, 경비병/주민 대표 Marker, 두 보고 대사와 Portrait 전환, Field_02 대사, Quest 완료, EXP 40·탈렌트 40·회복약 1 지급, 합류 알림 큐, 정식 동료·기본 파티 저장을 확인했다. 임시 슬롯 5로 A~E 진행 복원과 실제 Bootstrap Continue, 반복 주민 대표 대화·Scene 왕복의 보상 중복 방지, Field_01 Story Actor 비활성, 레벨업 시 새 MaxHP 완전 회복을 확인한 뒤 슬롯을 삭제했다. 최종 Console Error/Warning 0. 기능 commit: `b47c2da`. 다음 작업은 Field_02 후속 Main Quest 설계다.
- Main 04 `main_04_three_people` 「세 사람」을 실제 구현했다. Main 03 완료 뒤 `Field_01`에서 앞서간 흔적 도달 → 부상자를 치료 중인 미엘과 첫 다중 화자 대화 → 플레이어·태온·미엘의 지정 3대3 Story Encounter → 전투 후 단서 결합·마을 귀환 결정 순서로 진행한다. stable Location/NPC/Encounter ID 4개로 저장하며 Main 04 자체 완료 보상은 없다.
- 미엘은 구조 대기 대상이 아니라 도착 전부터 부상자를 치료하는 능동적 치유사로 등장하며, 전투 뒤 자기 상처도 확인하겠다고 말한다. 세 사람의 관찰을 합쳐 초원 안쪽의 이상만 추론하고 원인·배후·오염은 확정하지 않는다. 태온·미엘은 기존 공식 초상화와 전투 애셋을 사용하지만 아직 영구 파티가 아닌 Story Temporary Companion이다.
- 지정 전투 `field01_main04_three_people_encounter`는 초원 슬라임 1·독침벌 2와 싸우며 승리만 진행한다. 도주·패배는 같은 목표에서 재도전하고 기존 몬스터 EXP·탈렌트·Material Loot 정책을 유지한다. 완료 뒤 최소 데이터만 준비한 Main 05 `main_05_return_of_three` 「돌아온 세 사람」이 Available이 되며 실제 Main 05 내용과 정식 동료 합류는 구현하지 않았다.
- Unity MCP Play Mode에서 목표 4개·Quest Log, 화자별 태온/미엘 초상화와 플레이어 null-safe 배치, 실제 3대3 참가자·Formation, 미엘 회복/MP, 태온·미엘 공식 전투 애니메이션, 패배·도주 무진행, 승리 보상과 후속 대화, Main 04 Completed/Main 05 Available을 확인했다. 임시 슬롯 5에서 A~E 저장/이어하기를 모두 확인하고 삭제했으며, C/D 마을 왕복 재진입 시 대화·전투가 반복 실행되지 않고 승리 신호 중복도 진행을 건너뛰지 않음을 확인했다. 최종 Console Error/Warning 0. 기능 commit: `d0dd615`. 다음 작업은 Main 05 「돌아온 세 사람」이다.
- Main 03 `main_03_unfamiliar_companion` 「낯선 동행」을 실제 구현했다. Main 02 완료 뒤 `Field_01`에서 태온 첫 발견 → 첫 대화 → 지정 Story Encounter → 전투 후 단서 결합 → 태온 임시 동행 → 다음 조사 지점 순서로 진행한다. 목표는 stable Location/NPC/Encounter ID 5개로 저장하며 Main 03 자체 보상은 없다.
- 태온은 Main 02 흔적보다 안쪽 `(7.5, -0.8)`에서 공식 Field Sprite와 `Portrait_Taeon.png`를 사용한다. `관찰 → 이해 → 보호`를 중심으로 플레이어의 흔적과 태온의 반복 행동 관찰을 결합하고, 원인·배후·오염은 확정하지 않는다. 미엘은 등장하지 않으며 Main 04 「세 사람」에서 처음 등장한다.
- `field01_main03_taeon_encounter`는 일반 Field Encounter와 분리된 플레이어+태온 대 초원 슬라임+독침벌 2대2 전투다. 태온은 아직 정식 동료가 아닌 Story Temporary Companion이며 기존 수호자·지적의 길·공식 Left 전투 애니메이션을 그대로 쓴다. 승리만 Objective를 진행하고 도주·패배는 같은 목표에서 재도전한다.
- Unity MCP Play Mode에서 Quest Log/목표 5개, Marker, 태온 초상화·공식 Sprite, 실제 2대2 참가자, Idle·Attack·Guard·Skill·Hit, 기존 몬스터 EXP 20·탈렌트 7 지급, 두 번째 대화, 안전한 다음 지점 재배치, 치료 흔적, Main 03 Completed와 Main 04 Available을 확인했다. 임시 슬롯 5에서 A~E 저장/불러오기를 모두 확인한 뒤 삭제했고, 마을 왕복 재진입과 도주·패배 무진행도 확인했다. 기능 commit: `372d1fe`. 다음 작업은 Main 04 「세 사람」이다.
- 월드 상시 Quest Tracker를 정식 Quest Log로 교체했다. 키보드 `Q`와 게임패드 View/Back 계열 `<Gamepad>/select`로 열고 닫으며, `Esc`/게임패드 Cancel로 닫는다. 요청의 `<Gamepad>/selectButton`은 현재 Input System 장치에 없는 경로여서 실제 유효한 `select`를 사용했다.
- Quest Log는 왼쪽 ScrollRect에 `[메인 퀘스트]`/`[서브 퀘스트]`와 동적 제목 행, 오른쪽에 QuestDefinition 기반 이름·유형·설명·현재 Objective·단계·보상을 표시한다. Active Main을 기본 선택하고 Main이 없으면 첫 Side, 0개면 `진행 중인 퀘스트가 없습니다.`를 표시한다. 선택은 색상과 `▶`를 함께 쓰며 방향키·D-pad·Left Stick·Tab·Submit을 지원한다.
- 기존 Quest HUD는 상시 내용을 제거하고 Quest 시작·Objective 갱신·완료 시 4초 Toast만 표시한다. Quest Log는 `WorldModalState`를 재사용해 Dialogue·Shop·Inventory와 상호 배제되고, 열림 중 Player 이동과 World HUD를 잠근 뒤 닫을 때 Focus·잠금·Modal 소유권을 해제한다. Quest Save 구조와 NPC Quest Marker는 변경하지 않았다.
- Main 01 목표를 `주민 대표와 대화하세요.` → `남문 경비병과 대화하세요.`, Main 02 목표를 `초원 안쪽의 조사 지점으로 이동하세요.` → `조사 지점 주변의 몬스터 무리를 물리치세요.` → `전투 지점 너머의 흔적을 조사하세요.`로 명확히 했고 기존 스토리 문서 범위 안의 Quest 설명을 Definition에 추가했다.
- Unity MCP에서 Bootstrap 정상 시작, Q Toggle/Esc, 게임패드 Select Open/Cancel Close, Main 기본 선택, Main/Side 3개와 임시 Side 10개 동적 목록·Scroll, 0개 상태, 방향키·D-pad Focus, Tab Close Focus, Dialogue/Shop/Inventory 양방향 상호 배제, 이동 잠금/복구를 확인했다. Quest System Audit로 Main 01/02 진행·Save/Continue·구버전 Save·Toast 시작/갱신/완료/자동 숨김을 재검증했고 임시 Side 데이터는 저장하지 않았다. Console Error/Warning 0. 마지막 기능 commit: `b63d6b4`.
- 다음 직접 확인은 실제 1280×720 및 다른 화면 비율에서 긴 한글 설명 줄바꿈, 실제 연결 게임패드의 View/Back·D-pad·Left Stick 감각, Quest Toast의 4초 체감 시간이다.
- 대화 UI에 150×150 초상화 슬롯을 추가했다. `DialoguePortraitDefinition`/`DialoguePortraitCatalog`가 표시 이름과 분리된 stable ID로 Sprite를 조회하며, Sprite가 null이거나 정의가 없으면 프레임과 왼쪽 여백을 함께 숨겨 기존 글자 배치를 유지한다. 기존 ID 없는 `Show` 호출도 호환 오버로드로 유지한다.
- 주민 대표 `starter-village-main-guide`, 남문 경비병 `starter-village-gate-guard`, 태온 `companion_taeon`, 미엘 `companion_miel`의 Resources 정의를 준비했다. 태온 `Portrait_Taeon.png`와 미엘 공식 초상화 `Portrait_Miel.png` 적용을 완료했으며, 나머지 두 정의는 Sprite를 연결하기 전까지 기존 null-safe 글자 배치를 유지한다. 콘텐츠 생성 메뉴는 기존 연결 Sprite를 보존한다. 마지막 기능 commit: `ee9efcd`.
- 주민 대표·남문 경비병을 포함한 `VillageNpcRole`과 Main 01 연속 대화가 stable NPC ID를 전달하도록 연결했다. 대화/확인/연속 대화, 모달 소유권, 거리 이탈 종료, World HUD 숨김·복귀 경로는 유지했으며 Save 스키마는 변경하지 않았다.
- Unity 6000.5.7f1에서 컴파일 Error 0, 핵심 정의 4개 로드와 null Sprite를 확인했다. Play Mode에서 네 ID에 임시 Sprite를 각각 주입해 슬롯 표시·본문 여백 190을 확인하고, null 자동 숨김·본문 여백 28 복귀·모달 획득/해제·거리 이탈 자동 종료·HUD owner 1→0을 확인했다. 검증 Sprite는 저장하지 않았고 최종 Console Error/Warning은 0이다. 마지막 기능 commit: `f9d1437`.
- Unity에서 최종 초상화 Sprite를 연결한 뒤 실제 다양한 해상도에서 얼굴 크롭·150×150 프레임 가독성·긴 대사 줄바꿈·선택지와의 겹침을 직접 확인해야 한다. 다음 권장 작업은 태온·미엘 초상화 → 주민 대표·남문 경비병 초상화 → 태온·미엘 전투 애셋 순서다. 일반 주민·생활 NPC 초상화는 후순위다.
- 태온 공식 전투 Sprite `Taeon_Battle_Final.png`를 `companion_taeon` 전용 전투 비주얼에 적용했다. 기본 방향은 Left이며 Idle·Attack·Guard·Skill·Hit·Defeat 6개 애니메이션을 사용한다. 실제 Battle Scene Play Mode에서 재생·복귀와 Guardian 스킬 연결을 확인했고 최종 Console Error/Warning은 0이다. 마지막 기능 commit: `4854591`.
- 미엘 공식 전투 Sprite `Miel_Battle_Final.png`를 `companion_miel`에 적용했다. 태온과 같은 데이터 기반 Battle Animation 구조와 기본 Left 방향을 재사용하며 Idle·Attack·Guard·Skill·Hit·Defeat 6개 애니메이션을 연결했다. 실제 Battle Scene Play Mode에서 기본 공격·방어·치유사 3스킬·피격·전투불능, MP·회복·정화 수치와 태온 회귀를 확인했고 최종 Console Error/Warning은 0이다. 마지막 기능 commit: `34b7e06`.
- Main 02 `main_02_grassland_anomaly` 「초원의 이상」을 실제 구현했다. Main 01 완료 뒤 자동 시작하며 `Field_01`의 넓은 조사 구역 도달 → 지정 `grass_slime_01` 조우 승리 → 흔적 상호작용 순서로 진행한다. 일반 Kill Count가 아니며 다른 Encounter·도망·패배는 진행시키지 않는다.
- 조사 구역은 `(.4, 1)`, stable ID `field01_main02_investigation_area`이고, 지정 조우는 `field01_main02_investigation_encounter`, 흔적은 `(5.1, -1.2)`의 `field01_main02_tracks`다. 사용자 수정 Field Scene과 Monster Spawn/Respawn 데이터는 바꾸지 않고 런타임 연결로 분리했다.
- 흔적에는 `◇ 흩어진 흔적`과 `[E/F] 조사`를 함께 표시해 색상만으로 찾지 않게 했다. 다섯 Path별 1문장 반응은 표현만 다르고, 모두 "몬스터들이 마을을 공격하러 온 것이 아니라 무언가를 피해 밀려온 것 같다"는 같은 결론으로 합류한다. 원인·배후·오염 여부는 미확정이다.
- Main 02 자체 보상은 0이고 지정 전투의 기존 EXP·탈렌트·Material Loot는 유지한다. 완료 뒤 Main 03 `main_03_unfamiliar_companion` 「낯선 동행」이 Available이 되는 최소 데이터만 추가했으며 실제 내용은 아직 없다.
- Unity MCP Play Mode와 임시 슬롯 5에서 시작 전/조사 구역 후/지정 조우 후/완료 네 상태 저장·복원, 다른 Encounter 무시, 지정 Encounter 진행, 반복 조사 중복 완료 방지, Main 03 해금을 확인했다. 임시 슬롯과 캡처는 정리했다. 마지막 기능 commit: `d5e7417`.
- Main 01 `main_01_call_reaches` 「부름이 닿은 곳」을 실제 데이터로 추가했다. 시작 마을 주민 대표(`starter-village-main-guide`)에게 말을 걸면 별도 수락 창 없이 Active가 되고, 주민 대표 대사를 끝까지 진행하면 목표가 남문 경비병(`starter-village-gate-guard`)으로 바뀐다. 경비병 대사를 끝까지 진행하면 Completed가 되며 보상은 EXP 0·탈렌트 0·아이템 없음이다.
- Main 01 완료 뒤 Main 02 `main_02_grassland_anomaly` 「초원의 이상」이 Available이 되는 최소 해금 데이터만 추가했다. Main 02의 실제 목표·대사·Field 조사 내용은 아직 구현하지 않았다.
- 범용 NPC Quest Marker를 추가했다. `Available=✦ 새 이야기`, `ActiveObjective=◆ 현재 목표`, `ReadyToTurnIn=✓ 완료 보고`, `None=숨김`이며 색상 외에도 서로 다른 기호와 텍스트로 구분한다. stable NPC ID와 QuestDefinition의 Start/Objective/TurnIn ID를 연결하고 `QuestService.Changed` 때만 갱신한다.
- Marker는 기존 Nameplate Y 1.02, Interaction Prompt Y 1.48 위인 Y 1.92에 World Space uGUI로 배치했다. Main 01 시작 전 주민 대표만 표시되고, 첫 대화 뒤 경비병으로 이동하며, 완료 뒤 모두 사라진다.
- Unity MCP Play Mode에서 Main 01 전/주민 대표 후/완료 상태를 임시 슬롯 5에 실제 저장·불러오기해 `Available → ActiveObjective → Completed`, Main 02 Available, 반복 NPC 알림 중복 완료 방지를 확인했다. Available Marker Game View와 1.92 배치를 확인했으며 기능 코드 컴파일 Error 0이다. 프로젝트 기존 deprecated API Warning 6건은 이번 변경과 무관하게 남아 있다. 마지막 기능 commit: `30d0064`.
- 데이터 기반 퀘스트 기반을 구현했다. `QuestDefinition`/`QuestObjectiveDefinition`/`QuestCatalog`/`QuestRuntimeState`/`QuestService`를 분리했고, stable QuestId·ObjectiveId와 `TalkToNpc`·`ReachLocation`·`DefeatEncounter`·`Interact`·`GenericSignal` Notify로 Scene 검색 없이 순차 목표를 진행한다.
- 메인은 한 개만 Active인 직렬 진행과 선행 완료 해금을 사용하고, 서브는 여러 개를 동시에 진행할 수 있다. Active Main, Active Side, Completed 목록과 추적 Quest 변경 API를 제공하며 길은 진행·보상 우열에 관여하지 않는다.
- 마지막 목표 완료 시 기존 `RewardBundle`로 EXP·탈렌트·아이템을 지급하고 Completed를 기록해 반복 Signal의 중복 보상을 막는다. Save Version 1의 선택 `QuestProgress` 필드에 Active/Completed/Objective/Tracked 상태를 저장하며 구버전 null 필드는 빈 퀘스트 상태로 복원한다.
- World Overlay Canvas 왼쪽 위에 `[메인]`/`[서브]`, 퀘스트명, 현재 목표 하나를 표시하는 `QuestHudPresenter`를 추가했다. 메인 시작을 기본 추적하고 Dialogue·Shop·Inventory가 공유하는 `WorldModalState`가 열리면 HUD를 숨긴다. Main 01에서 주민 대표 목표가 경비병 목표로 즉시 바뀌고 완료 뒤 숨는다.
- Unity MCP에서 빈 Scene Play Mode 감사로 시작, 잘못된 ID 무시, 순차 목표 이동, 완료, Reward 1회, 반복 Signal 중복 방지, 서브 2개 동시 Active, 메인 선행 해금, JSON 저장/복원, 구버전 null 호환, HUD 텍스트와 Modal 숨김을 확인했다. `QUEST_SYSTEM_AUDIT Play Mode PASS`/`ALL PASS`, 최종 Console Error 0·Warning 0이다. 기능 commit은 `80227e6`이다.
- 몬스터 처치 보상을 기존 승리 EXP 흐름에 통합했다. 실제 전투불능 `Combatant`의 stable `MonsterDefinition`을 한 번 집계해 EXP·탈렌트·개체별 전리품 Roll을 `RewardBundle`로 만들고, 실제 적용 결과를 Victory UI와 Save가 함께 사용한다. 도망·패배는 세 보상 모두 0이며 EXP 레벨 차이 배율은 Currency/Loot에 적용하지 않는다.
- 초원 슬라임 3/점액 50%, 독침벌 4/침 45%, 숲거미 5/거미줄 40%, 맹독뱀 6/비늘 35%를 데이터로 추가했다. 네 Material은 MaxStack 999·SellPrice 2/3/4/5·사용 불가이며 ShopDefinition에는 넣지 않았다. 기존 Game-icons.net White 원본의 `dripping-goo`/`wasp-sting`/`spider-web`/`dorsal-scales`를 ItemDefinition.Icon에 연결하고 Material별 tint를 적용했다.
- Victory UI는 EXP, CurrencyIcon+탈렌트 텍스트, ItemId별 합산 이름·수량 또는 `없음`, 레벨 진행을 620×390 패널에 표시한다. Inventory 수용 실패 시 EXP/탈렌트는 유지하고 들어가는 전리품만 실제 지급 결과에 포함한다.
- Unity 6000.5.7f1 응답 파일 기준 Runtime/Editor 전체 정적 컴파일은 오류 0개, 기존 deprecated API 경고 Runtime 5개·Editor 1개다. Economy Inventory Audit에 조우 탈렌트 9/10/13/16, deterministic 0/1/동일 Item 3 Drop, Reward 데이터와 Material/Shop/부분 수용 검사를 추가했다. 실제 Audit/Play Mode 실행은 두 차례 시도했으나 이 PC의 Unity Licensing Client IPC가 반복 단절되어 Domain Reload 도중 진행되지 못했으며, Unity MCP 도구도 현재 세션에 노출되지 않았다. 따라서 실제 Victory UI·Save/Continue·Console 런타임 검증은 완료로 표시하지 않는다.
- 시작 마을 일반 주민 4명에 stable ID 기준 OGA-07/11/13/19 외형을 연결하고 주황 Placeholder를 제거했다. 일반 주민은 상시 이름표 없음, 기능 NPC·주민 대표는 역할명 한 줄 정책이며 Interaction Prompt는 별도로 유지한다.
- 표시되는 시작 마을 NPC 이름표는 TextMesh 폰트 40, 기본 character size 0.075(7자 이상 0.065), Y 1.02, 가운데 정렬, 검은 그림자를 적용한다. 상호작용 안내는 Y 1.48로 분리했고 플레이어 이름표·레벨 HUD는 변경하지 않았다.
- 시작 마을 역할 NPC 8명에 Eldiran CC0 외형을 stable ID 기반으로 연결했다: 잡화상 OGA-03, 장비상 OGA-17, 치유사 OGA-06, 은행 OGA-16, 동료 편성 OGA-10, 훈련장 관리 OGA-02, 남문 경비 OGA-20, 주민 대표 OGA-09.
- 원본 시트는 `Assets/ThirdParty/Eldiran/RPGCharacters32/Original`에 보존하고, 선택된 정면 32×32 셀의 정확한 마젠타만 투명화한 파생본은 `Assets/_Project/Resources/VillageNpcSprites/Eldiran`에서 관리한다. Point·PPU 28·발 기준 Pivot을 사용하며 태온·미엘 등 핵심 캐릭터 외형은 변경하지 않았다.
- 2026-09-15 Unity 6000.5.7f1에서 ScriptAssemblies 빌드·Domain Reload와 Unity MCP Play Mode 검증을 완료했다. C# 컴파일 오류와 기능 관련 런타임 Error는 없다.
- NPC 대화·치유 확인 등 상호작용 UI가 열리면 World EXP HUD를 CanvasGroup으로 숨기고 입력 간섭도 차단하며, 종료·비활성화·Scene 전환 시 복귀한다. 소유자별 공통 API라 향후 상점·은행·동료 편성·퀘스트 UI도 같은 규칙을 재사용할 수 있다.
- NPC 대화는 실제 Player/NPC Transform을 추적하며 상호작용 거리 2.0보다 넓은 3.0을 넘으면 이동을 막지 않고 자동 종료한다. 자동 종료도 공통 Hide 경로라 EXP HUD가 복귀하고 범위 밖 Prompt는 숨겨진다.
- 경제·인벤토리 최소 기반을 추가했다. Currency는 음수/overflow/실패 지출을 방어하고 Inventory는 stable ItemId+Count 배열로 저장한다. ItemDefinition/Catalog와 원자적 RewardBundle 연결점을 제공하며 상점·은행·장비·실제 아이템은 아직 구현하지 않았다.
- Save Version 1 선택 필드로 Currency/Inventory를 추가해 구버전 누락 필드는 0/빈 목록으로 호환한다. Inventory Capacity는 미확정·무제한이며 패배 화폐 손실과 휴식 EXP는 없다. 상세: `문서/12_시스템/경제와_인벤토리.md`.
- 사용자 표시 화폐명은 `탈렌트`로 확정했다. `CurrencyPresentation`이 표시명, 금색 tint와 Kenney Board Game Icons CC0 원형 토큰 Sprite를 공통 제공하며 World EXP HUD에는 추가하지 않았다.
- 시작 마을 `GeneralShop` 역할에 실제 잡화상 Shop을 연결했다. `ShopDefinition`/`ShopService`/공용 Modal UI를 분리하고 회복약 Buy20·Sell10, 마력 회복약 Buy30·Sell15를 ItemDefinition 데이터로 제공한다. 거래는 1개 단위이며 성공 시 현재 슬롯 저장, 상점 중 World EXP HUD·플레이어 이동 억제와 키보드/게임패드 Focus를 지원한다. 장비상·아이템 사용은 아직 미구현이다.
- Unity MCP Play Mode에서 100→80(회복약 0→1)→50(마력 회복약 0→1)→60(회복약 1→0), 잔액 0의 재구매 실패와 보유 0 판매 실패 무변경을 확인했다. Open/Close 5회, NPC 비활성화 자동 Close, HUD/이동 잠금 복귀, `Field_01` 왕복 재진입, 실제 빈 슬롯 5 Save/Restore 60 탈렌트·마력 회복약 3개 복원을 확인하고 검증 슬롯은 삭제했다. Audit ALL PASS, 최종 Console Error/Warning 0이며 기능 commit은 `c0a5b6a`다.
- 잡화상 거래 수량을 구매/판매 모드별 `TradeQuantity`로 일반화했다. 기본/아이템 전환 시 1, `-`·`+`·`최대` 조작, 선택 수량 총액과 거래 가능 문구를 표시하며 구매 최대는 자금·남은 MaxStack, 판매 최대는 보유량이다. `ShopService`는 Shop 포함 여부와 long 총액 overflow를 사전 검증하고 `InventoryService`도 MaxStack을 강제한다.
- Unity MCP Play Mode에서 회복약 3개 구매 200→140·0→3, 마력 회복약 4개 구매 140→20·0→4, 마력 회복약 구매 최대 0, 회복약 2개 판매 20→40·3→1과 수량 2→1 Clamp를 확인했다. MaxStack 98/99에서 추가 2개와 Shop 2개 구매가 모두 거부됐고, 빈 슬롯 5 Save/Restore 40 탈렌트·회복약 1·마력 회복약 4 복원 후 검증 슬롯을 삭제했다. Open/Close 5회와 아이템 전환 수량 초기화, HUD/이동/Focus 복귀, Audit ALL PASS를 확인했으며 기능 commit은 `e741bda`다.
- 잡화상 판매 탭을 `ShopDefinition`이 아니라 현재 Inventory 기반으로 전환했다. Count 양수·SellPrice 양수인 Consumable/Material만 이름과 수량으로 표시하고 Quest/KeyItem은 제외한다. 전량 판매 행 제거·다음 선택 이동·빈 목록 안내, 모드/아이템 전환 수량 1 초기화, `[구매]`/`[판매]` 텍스트 상태를 지원한다.
- Unity 6000.5.7f1의 열린 Editor와 Unity MCP에서 컴파일 오류 0, Economy Inventory Audit ALL PASS를 확인했다. Play Mode에서 점액 3개 판매 10→16·3→0과 품절 행 제거, 거미줄 2개 판매 16→24·4→2, 반복 모드 전환 수량 1 복귀, 빈 판매 목록 안내·조작 비활성, 테스트 슬롯 Save/Restore의 Currency·Inventory·Level/EXP·PartyResources 유지를 확인했다. 당시 Material 아이콘 미연결 상태도 기능 오류 없이 동작했다.
- Material 아이콘 연결 뒤 Unity MCP Play Mode에서 Inventory와 잡화상 판매 탭의 4개 Sprite 참조·동일 tint·`preserveAspect`를 짧게 확인했다. 기존 null Icon fallback과 판매/보상 로직은 변경하지 않았다.
- 월드/필드 소지품 UI와 소비 아이템 실제 사용을 완료했다. `ItemUseService`가 데이터 기반 HP +30/MP +12, 최대치 Clamp, 풀 자원·HP 0·MP 미사용 대상 실패 시 무소비를 공통 처리하고 `PartyResourceService`에 반영한다. `I`로 열며 아이콘·이름·수량·설명·효과·현재 파티 대상·탈렌트를 표시한다.
- Dialogue·Shop·Inventory는 `WorldModalState`의 단일 Modal 정책을 공유한다. Inventory 중 World EXP HUD와 이동을 억제하고 닫기·Scene 전환 시 복귀한다. Unity MCP에서 1280×720 Game View, 3회 반복, 상호 차단, `Field_01` 재설치, Audit ALL PASS 및 사용 뒤 Inventory/PartyResources 복원을 확인했다. 기능 commit은 `c5fe8fd`, UI commit은 `c089443`이다.

## 기준

- 갱신일: 2026-09-22
- 기준 브랜치: `main`
- 마지막 기능 관련 commit: `0454af8` (`Feature: 시작 마을 NPC 외형 적용`)
- 마지막 기능 수정 commit: `ac11356` (`Fix: 시작 마을 NPC 외형 런타임 설치 수정`)
- 마지막 NPC UI 수정 commit: `41df219` (`Fix: 시작 마을 NPC 이름표 가독성 개선`)
- 마지막 NPC 외형 마무리 commit: `23281a5` (`Feature: 시작 마을 일반 주민 외형 완성`)
- 마지막 상호작용 UI 우선순위 commit: `fcf5cf9` (`Fix: 상호작용 중 월드 EXP HUD 숨김`)
- 마지막 NPC 거리 대화 종료 commit: `6b42c26` (`Fix: NPC 거리 이탈 시 대화 종료`)
- 마지막 경제·인벤토리 기반 commit: `dcb76e6` (`Feature: 경제와 인벤토리 최소 기반 추가`)
- 마지막 인벤토리 기반 상점 판매 commit: `f6dbcca` (`Feature: 인벤토리 기반 상점 판매 목록`)
- 마지막 몬스터 Material 아이콘 commit: `82d60fe` (`Feature: 몬스터 Material 아이콘 연결`)
- 마지막 몬스터 보상 commit: `298f023` (`Feature: 몬스터 탈렌트와 전리품 보상 추가`)
- 마지막 퀘스트 시스템 기반 commit: `80227e6` (`Feature: 데이터 기반 퀘스트 시스템 기반 추가`)
- 마지막 탈렌트 표시 참조 commit: `0277317` (`Feature: 탈렌트 화폐 표시 참조 추가`)
- 마지막 오류 수정 commit: `486ff5a` (`Fix: 월드 경험치 바 채움 비율 수정`)
- 마지막 관련 문서 commit: `ace782e` (`Docs: 사수 전용 야수 동료 설계 확정`)
- 마지막 전투 UI 관련 commit: `7de1918` (`Refactor: 전투 스킬 설명 UI 정리`)
- 마지막 태온 전투 애니메이션 commit: `4854591` (`Feature: 태온 공식 전투 애니메이션 적용`)
- 마지막 미엘 전투 애니메이션 commit: `34b7e06` (`Feature: 미엘 공식 전투 애니메이션 적용`)
- 마지막 Main 03 구현 commit: `372d1fe` (`Feature: Main 03 낯선 동행 구현`)
- 마지막 Main 04 구현 commit: `d0dd615` (`Feature: Main 04 세 사람 구현`)
- 마지막 Main 05 구현 commit: `b47c2da` (`Feature: Main 05 돌아온 세 사람 구현`)
- 마지막 몬스터 후보 에셋 commit: `849bc12` (`Chore: 독 몬스터 후보 에셋 보존`)
- 마지막 Pilot Bee 검증 오류 수정 commit: `7a07f2a` (`Fix: Pilot Bee 검증 Scene 입력과 Camera 수정`)
- 마지막 음성 제작 도구 commit: `272b473` (`Chore: MeloTTS 오프라인 제작 도구 추가`)

이 문서는 완료된 기능과 미구현 범위를 빠르게 파악하기 위한 상태 요약이다. 세부 설계는 각 시스템 문서를 따른다.

## 최근 시작 마을 1차 허브

- 일반 주민 stable ID 01~04에 OGA-07/11/13/19 정면 Sprite를 연결했다. 기존과 같은 32×32, Point, PPU 28, Mipmap Off, 발 Pivot, 정확한 마젠타 Alpha 0 파생 규칙을 사용하며 원본 PNG는 변경하지 않았다.
- 이름표 정책은 `VillageNpcRoleType`으로 공통 처리한다. 일반 주민 Label은 비활성, 기능 NPC와 주민 대표는 역할명 한 줄이다. `PlaceholderVisual`은 복제된 기존 Label을 재사용하고 카탈로그도 중복 Label을 정리해 `주민`/`마을 주민` 동시 표시를 막는다. Interaction Prompt와 주민 대화는 독립 유지한다.
- 역할 외형 적용 경로에서 전용 Sprite 유무와 관계없이 주민·상인·안내인 전원의 기존 `Label` TextMesh를 공통 정규화한다. 기존 폰트 48/character size 0.12/Y 0.65를 폰트 40/기본 0.075/Y 1.02로 줄이고 올렸으며, 7자 이상 이름은 0.065로 축소한다. 중앙 한 줄 정렬과 검은 그림자를 사용하고 `[E] 대화하기`는 Y 1.48로 올려 이름표와 분리했다. `PlayerNameplate`와 World EXP HUD는 변경하지 않았다.
- `World_StarterVillage`의 기존 19×13 환경·Player·Camera·남문 `Field_01` 연결을 보존하고, `Chapter01_StarterVillageHub` 아래에 중앙 광장, 시장, 치유소/여관, 은행·파티 관리 안전 서비스, 훈련 구역, 주민 생활 구역, 남문 역할을 구분했다.
- `VillageNpcRole`이 표시 이름과 분리된 stable NPC ID·역할 데이터를 제공한다. 메인 안내, 잡화점, 장비점, 은행, 파티 관리, 훈련, 주민, 남문 경비는 후속 시스템의 진입점/중립 대사만 제공하며 퀘스트·거래·보관·편성 내용을 확정하지 않았다. Taeon/Miel은 배치하지 않았다.
- 치유사는 시간 제한 없는 `치료한다`/`괜찮습니다` 확인 UI를 제공한다. 확인하면 `PartyResourceService.HealPartyFully()`로 등록된 파티 HP/MP를 무료 완전 회복하고 `GameSaveService.SaveCurrentSession()`으로 즉시 저장한 뒤 중립 완료 메시지를 표시한다.
- Unity 컴파일 오류 0개를 확인했다. Unity MCP Play Mode에서 역할 12개가 모두 2m 대상 탐색으로 선택되고 NPC Collider 겹침 0건, 7개 Zone 런타임 설치, 19×13 Bounds·남문 Trigger·복귀 Spawn을 확인했다. 치유는 검증값 HP/MP 35/12→100/50, 슬롯 저장 100/50, Continue 복원 100/50이었고 마을→`Field_01`→마을 왕복 뒤에도 100/50을 유지했다. 최종 Console은 Error 0개·Warning 0개다. 실제 화면에서 NPC 간 시각 간격과 1280×720 선택지 가독성은 사용자가 확인하면 좋다. 상세: `문서/20_월드/시작_마을.md`.
- 이름표·주민 외형 변경은 Unity 6000.5.7f1 전체 응답 파일의 `Assembly-CSharp`·`Assembly-CSharp-Editor` 별도 출력과 Unity Editor ScriptAssemblies 컴파일에서 오류 0개를 확인했다. 신규 경고는 없고 기존 CS0618 경고는 Runtime 5개·Editor 1개이며 관련 파일 `git diff --check`를 통과했다.
- Unity MCP Play Mode에서 주민 1~4 Sprite OGA-07/11/13/19, 주황 Placeholder·마젠타 배경 0개, 주민 Label 비활성 4/4, 기능 NPC·주민 대표 단일 활성 Label 8/8, 주민 대사 호출 4/4, BoxCollider2D `(0.50, 0.34)`와 Circle Trigger, `Field_01` 왕복 재설치를 확인했다. 전체 화면에서 일반 주민 텍스트가 제거되어 혼잡도가 줄고 PlayerNameplate·EXP HUD는 기존 상태를 유지했다. 스크린샷은 `Unity/Client/ValidationCaptures/`에 보존한다.
- 기능 관련 Console Error는 0개다. 저장 슬롯 없이 World Scene을 직접 실행한 검증 경로에서 기존 `선택된 저장 슬롯이 없어 자동 저장을 건너뜁니다.` 경고가 3회 발생했으며 NPC 외형·이름표 신규 경고는 없다.
- World EXP HUD는 탐험 중 표시하고 대화·확인 UI가 열리면 소유자별 억제 상태로 숨긴다. HUD CanvasGroup은 alpha 0·interactable false·blocksRaycasts false이며 모든 상호작용 UI 종료 후 alpha 1로 복귀한다. DialoguePresenter는 Hide, OnDisable, OnDestroy에서 상태를 해제한다.
- 대화 Canvas sortingOrder 10을 World HUD Canvas 5보다 높게 두고 Panel을 열 때 마지막 sibling으로 올렸다. Unity MCP Play Mode에서 일반 주민·잡화 상인·주민 대표, 3회 반복, `StarterVillage→Field_01→StarterVillage` 왕복 후 재대화를 검증했고 모두 숨김/복귀가 정상이다.
- 전체 Assembly-CSharp·Assembly-CSharp-Editor 정적 컴파일 오류 0개, 기존 CS0618 경고 Runtime 5개·Editor 1개다. Play Mode 기능 관련 Error 0개이며 직접 World Scene 실행으로 인한 기존 자동 저장 건너뜀 Warning 1개만 확인했다. 상세: `문서/11_UI/월드_레벨과_EXP_HUD.md`.

## 최근 전투 간 파티 HP/MP 지속

- `PartyResourceService`가 플레이어와 동료의 CurrentHP/CurrentMP를 `BattleParticipantSetup.Id` 기준으로 관리한다. Battle 생성은 저장값을 HP 1~MaxHP, MP 0~MaxMP로 Clamp해 적용하고, 값이 없는 신규 캐릭터·구버전 Save는 현재 최대치로 시작한다.
- 일반 승리와 도망은 실제 종료 자원을 유지한다. 승리 시 전투불능 캐릭터는 HP 1로 복귀하며, 실제 레벨업한 플레이어만 연속 상승 후 최종 Level의 새 MaxHP/MaxMP까지 완전 회복한다. 패배는 파티 전체를 완전 회복한다. 휴식 경험치는 없다.
- Version 1 Save에 `PartyResources(CharacterId, CurrentHp, CurrentMp)` 선택 배열을 추가했다. 옛 JSON의 누락 배열은 호환 기본값으로 처리하며 게임 재실행은 무료 회복 수단이 아니다. 전투 상태이상·도발·방어·쿨타임·기세·Path Runtime은 저장하지 않는다. 향후 치유소는 `HealPartyFully()`를 재사용할 수 있지만 NPC와 아이템은 아직 구현하지 않았다.
- 현재 프로젝트에는 요청에 언급된 별도 최근 거점/체크포인트 패배 복귀 시스템이 없고 조우 Field 복귀가 구현되어 있어, 기존 복귀 흐름은 변경하지 않고 자원 완전 회복만 연결했다.
- 신규 소스를 포함하도록 현재 Bee 응답 파일을 보완한 전체 정적 컴파일에서 Assembly-CSharp 오류 0개·기존 CS0618 경고 4개, Assembly-CSharp-Editor 오류 0개·기존 CS0618 경고 1개를 확인했고 관련 `git diff --check`를 통과했다. 실제 승리·도망·전투불능 승리·레벨업·패배·저장/Continue 시나리오는 Play Mode에서 직접 확인해야 한다. 마지막 기능 commit: `9e36ec0`.

## 최근 World EXP HUD PathSymbol 표시

- 화면 하단 `WorldExperienceHud`의 `Lv.n 이름` 왼쪽에 현재 세션 PathId의 공식 PathSymbol을 32×32px, 원본 비율·색상 유지로 표시한다. 전투 기능용 TraitIcon과 Path 이름은 추가하지 않았고 머리 위 PlayerNameplate도 변경하지 않았다.
- `PathPresentationResolver`가 저장에서 복원된 안정적인 PathId로 `PlayerPathDefinition.PathSymbol`을 찾는다. 이름·레벨·EXP와 함께 PathId 변경만 감시하며 Scene 전환, 이어하기와 레벨업 시 새 세션 상태로 갱신된다. 잘못된 PathId나 누락 Sprite는 Image만 숨겨 기존 HUD를 유지한다.
- 5개 PathDefinition의 PathSymbol fileID가 모두 유효하고 각 GUID가 실제 Sprite `.meta` 하나로 해석됨을 확인했다. 전체 Assembly-CSharp 컴파일 오류 0개·기존 CS0618 경고 4개, Assembly-CSharp-Editor 오류 0개·기존 CS0618 경고 1개이며 관련 `git diff --check`를 통과했다.
- 실제 Play Mode에서 1280×720 크기·텍스트 겹침, EXP 획득·레벨업, StarterVillage/Field_01/Field_02 전환, 저장 후 Continue, Battle 진입 시 HUD 정리와 Console 오류를 직접 확인해야 한다. 상세: `문서/11_UI/월드_레벨과_EXP_HUD.md`. 마지막 기능 commit: `6b52c74`.

## 최근 FinalConfirmation UI 폴리싱

- 중앙 패널을 큰 PathSymbol(64px)·길 이름, 작은 TraitIcon(26px)·특성 이름·설명, 직업 역할·보너스·패시브·시작 스킬 순서로 분리했다. 길 표현은 `PathPresentationResolver`와 `PlayerPathDefinition.PassiveDescription`을 재사용하며 Path별 문자열 분기는 없다.
- 오른쪽 최종 능력치는 이름과 숫자를 별도 고정 열로 정렬하고 `기본 10 + 직업` 정책을 유지했다. 길 능력치 보너스와 기존 계산·저장·Scene 전환은 변경하지 않았다. `게임 시작`은 이전보다 큰 Primary Action으로 구분하되 기존 키보드 Focus·Enter·Space 입력을 유지했다.
- 5개 PathDefinition의 PathSymbol·TraitIcon GUID가 각각 실제 Sprite `.meta` 하나로 해석되고 JobDefinition 5개가 존재함을 확인했다. 전체 Assembly-CSharp 컴파일 오류 0개·기존 CS0618 경고 4개, Assembly-CSharp-Editor 오류 0개·기존 CS0618 경고 1개이며 관련 `git diff --check`를 통과했다.
- 1280×720 기준 RectTransform 영역상 중앙 패널·능력치·버튼은 겹치지 않는다. 실제 Play Mode에서 5개 길과 5개 직업의 텍스트 렌더링, 긴 특성 설명, 캐릭터 Sprite, 이전·게임 시작과 Console 오류를 직접 확인해야 한다. 상세: `문서/11_UI/최종_확인.md`. 마지막 기능 commit: `88965c6`.

## 최근 MeloTTS 오프라인 음성 제작 시험

- 2026-09-09: OpeningIntro Scene과 런타임 생성 Camera에 AudioListener가 0개여서 AudioSource가 재생되어도 최종 출력이 무음이던 문제를 수정했다. Main Camera 생성 시 AudioListener를 함께 만들고 기존 활성 Listener가 있으면 재사용한다. AudioSource는 enabled, volume 1, mute false, 2D, Mixer 미연결을 명시하며 Listener volume 1·pause false로 시작한다.
- 기존 Editor 로그에서 `There are no audio listeners in the scene` 경고가 반복된 직접 증거를 확인했다. 첫 Clip은 외부 원본과 Unity 파일의 SHA256·크기가 같고 44.1kHz·모노·16비트 실제 신호다. 두 Assembly 컴파일 오류 0개이며 실제 소리, isPlaying, Enter/P/Esc는 수정 후 Play Mode에서 직접 확인해야 한다. 마지막 수정 commit: `a8dca02`.
- 사용자가 선택한 speed 1.00 기준으로 현재 OpeningIntro 19개 Slide 중 실제 내레이션 문장 18개를 생성했다. `opening_001.wav`~`opening_018.wav`는 44.1kHz·모노·16비트이며 외부 검증 후 `Assets/_Project/Audio/Voice/Opening`에 복사했다. 마지막 `LIMITLESS` 제목 장면은 무음이다.
- `VoiceClipCatalog`의 직접 AudioClip 참조로 문장 ID와 WAV를 1:1 연결하고 `VoicePlaybackSource`가 재생·일시정지·재개·정지를 맡는다. 자동 진행은 `max(기존 시간, Clip 길이 + 0.5초)`이고 수동 진행·Esc·건너뛰기는 즉시 정지한다. 자막은 항상 유지하며 다시 보기에서도 음성을 재생한다.
- Catalog 18/18 참조와 WAV 신호·형식을 확인했다. Unity Editor가 Assembly-CSharp와 Assembly-CSharp-Editor를 오류 0개로 컴파일했고 OpeningIntro 진입과 CharacterCreation 전환 로그에 누락 참조 경고나 런타임 예외가 없었다. 실제 청취 품질, 전 문장 순서, Enter/P/Esc, 다시 보기와 자동 Skip은 Play Mode에서 직접 확인해야 한다. 마지막 관련 commit: `47d8594`.
- 공식 MeloTTS `main` commit `209145371cff8fc3bd60d7be902ea69cbdb7965a`, 패키지 0.1.2를 Unity 밖 `F:/study/tts/MeloTTS`에 두고 전용 Conda `melotts` Python 3.9.25 CPU 환경에서 한국어 `KR` 모델을 로드했다. 기존 `py3_12` Python 3.12.0에는 설치하지 않았다.
- `태초에, 신은 세상을 창조했다.`를 speed 0.85/0.95/1.00으로 각각 44.1kHz·모노·16비트 WAV로 생성했다. 길이는 4.111/3.687/3.603초이며 RMS가 모두 0보다 커 실제 신호가 있다. 시험 WAV와 모델·캐시는 저장소 및 Unity Assets에 넣지 않았다.
- `Tools/TTS/melotts_batch.py`가 UTF-8 CSV/JSON의 `id,text,speaker,speed`를 읽어 모델 한 번 로드 후 `id.wav`를 만든다. Colab Notebook은 시스템 Python 대신 `uv`의 격리 Python 3.9를 사용하고 Drive mount는 선택 셀로 둔다.
- Windows에서는 `g2pkk`의 `eunjeon` 빌드가 MSVC 부재로 실패해 전용 KR 환경에 `python-mecab-ko` wheel과 최소 import shim을 사용했다. Notebook JSON·Python 구문·CSV/JSON 입력은 검증했으나 실제 Colab 런타임 실행은 별도 확인이 필요하다. 상세: `문서/00_프로젝트/MeloTTS_오프라인_음성_제작.md`. 마지막 관련 commit: `272b473`.

## 최근 정식 시작 이야기 구현

- 2026-09-09: 고정 1000×220 크기와 알파 0.86의 `CaptionShade`를 문장 실제 크기+좌우 42px·상하 20px로 자동 조절하도록 바꿨다. 화면 위 기준 약 69% 지점으로 내리고 알파 0.68의 방사형 배경을 사용해 중앙 Glow와 PathSymbol을 가리는 큰 사각 패널 인상을 줄였다. 렌더 순서는 배경→Glow→CaptionShade→자막→조작 UI다.
- 방사형 Glow Sprite·색·알파·확대와 기존 서사·입력·설정·Scene 흐름은 유지했다. Assembly-CSharp 오류 0개·기존 CS0618 경고 4개, Assembly-CSharp-Editor 오류 0개·기존 CS0618 경고 1개이며 관련 diff 검사를 통과했다. 실제 Play Mode에서 문장별 배경 크기, Glow 원형, PathSymbol·하단 UI 간격과 자막 가독성을 확인해야 한다.
- 2026-09-09: 스프라이트가 없는 단색 uGUI Image가 RectTransform 전체를 칠해 `AbstractLight`가 노란 사각형으로 보이던 문제를 수정했다. 런타임 방사형 알파 Sprite를 바깥 빛과 중심 빛 두 겹에 적용해 가장자리 알파가 0이 되도록 했으며, 광원을 `SlideContent` 아래로 옮겨 자막과 같은 Fade를 적용한다. 은은한 알파 변화와 확대만 사용하며 자막 패널 뒤에 배치한다.
- 기존 19개 Slide 데이터, PathSymbol, `LIMITLESS`, 자동 진행, 클릭·Enter·Space, P, Esc, 체크박스 저장, 새 게임·이어하기·다시 보기 정책은 변경하지 않았다. Assembly-CSharp 오류 0개·기존 CS0618 경고 4개, Assembly-CSharp-Editor 오류 0개·기존 CS0618 경고 1개로 별도 컴파일했다. 실제 Play Mode의 광원 외형과 입력은 직접 확인해야 한다.
- 플레이어 표시명을 `LIMITLESS`로 확정하고 Bootstrap·CharacterCreation·JobSelection·FinalConfirmation의 기존 제목을 변경했다. 저장소·namespace·Editor 메뉴의 개발명 `Project Limitless`는 유지한다.
- 빈 슬롯 새 캐릭터 흐름에 `OpeningIntro`를 추가했다. 확정 서사 19개 Slide는 `OpeningIntroSequence` 한 곳에서 문장·시간·연출을 관리하며 Fade, 느린 확대, 추상 빛과 공식 PathSymbol 5개를 사용한다. 클릭·Enter·Space 진행, P 일시정지, Esc·버튼 건너뛰기를 지원하고 중복 종료를 막는다.
- 슬롯과 분리된 `UserSettingsService`가 `SkipOpeningIntro`를 별도 JSON에 원자적으로 저장한다. 파일 누락·손상·버전 불일치는 false, I/O 실패는 경고 후 계속 진행한다. Esc는 설정을 바꾸지 않는다.
- Bootstrap에 `시작 이야기 다시 보기`를 추가했다. 설정을 무시하고 재생하며 완료·건너뛰기 뒤 Bootstrap으로 복귀한다. Editor 메뉴 `Project Limitless/Test/Play Opening Intro`도 다시 보기로 안전하게 진입한다.
- OpeningIntro를 Build Settings와 저장 금지 Scene에 등록했다. 기존 Continue, 선택 슬롯, 캐릭터 생성 이후 흐름과 Path·전투·성장 수치는 변경하지 않았다.
- 전체 Assembly-CSharp 정적 컴파일 오류 0개·기존 CS0618 경고 4개, Assembly-CSharp-Editor 오류 0개·기존 CS0618 경고 1개. 실제 Play Mode의 자동/수동 진행, 설정 재실행 유지, 새 슬롯·다시 보기 복귀와 화면 배치는 직접 확인해야 한다.
- 상세: `문서/11_UI/시작_이야기.md`. 마지막 관련 commit: `cec069f`.

## 최근 메인 스토리와 1장 도입 설계

- 하나의 공통 메인 스토리를 중심으로 선택한 길에 따라 장면·대사·개인 에피소드가 달라지는 구조를 확정했다. 길별 메인 캠페인 다섯 벌은 만들지 않으며, 길에 따른 정보·진행·경제 보상의 우열도 두지 않는다.
- 플레이어는 구체적인 신의 명령이 아니라 설명하기 어려운 `부름`을 따라 시작 마을에 도착한다. 부름의 이유와 신의 의도는 장기 미스터리로 남긴다.
- 1장 초반 Main 01~05를 시작 마을 적응 → 초원의 이상 조사와 첫 전투 → 태온과의 동행 및 미엘 조우 → 첫 3인 파티 전투 → 마을 보고와 정식 합류 흐름으로 설계했다. 몬스터가 무엇에게 쫓기는지는 확정하지 않았다.
- 태온은 수호자+지적의 길의 `관찰 → 이해 → 보호`, 미엘은 치유사+마음의 상처의 `회복탄력`을 이야기에서도 드러내되 장애·상처를 성격이나 초능력으로 단순화하지 않는다. 다음 작업은 실제 퀘스트 시스템 구현 전 세부 스토리·대사·퀘스트 데이터 설계다.
- 상세: `문서/03_스토리/메인_스토리와_퀘스트_설계.md`.

## 최근 길 공식 표시와 동료 길 적용

- 2026-09-09: `PlayerPathDefinition` 표현 자료를 자체 제작 공식 `PathSymbol`과 Game-icons.net 전투 `TraitIcon`으로 분리했다. 새 청각·시각·지체 심볼과 기존 마음의 상처·지적 심볼로 5개 공식 문장을 완성했으며, 캐릭터 Variant 외형을 공식 문장으로 사용하지 않는다.
- PathSelection 카드와 상세는 큰 PathSymbol·길 이름·작은 TraitIcon·특성 이름·추천 순서로 표시한다. 전투 상세는 두 아이콘과 두 이름을 분리하고, HUD의 `path.*` 상태는 TraitIcon과 한글 상태명을 함께 표시한다. 플레이어와 태온(지적/패턴 익히기), 미엘(마음의 상처/회복탄력)은 같은 Resolver를 사용한다.
- SaveData는 기존 PathId만 유지한다. `traitStatusMarkerIds`로 런타임 상태와 TraitIcon을 연결하며 Sprite 누락 시 Image만 숨기고 텍스트는 유지한다. Path 전투 수치와 NPC Runtime은 변경하지 않았다.
- 새 자체 제작 원본 3개는 `Assets/_Project/Art/Characters/PathVisuals/Symbols/`, 기존 2개는 원래 위치를 유지한다. Game-icons.net White/Black 10개와 라이선스도 원본 그대로 유지한다.
- 위치: `Assets/ThirdParty/GameIconsNet/Path/{White,Black}/`, 라이선스: `Assets/ThirdParty/GameIconsNet/license.txt`. 저작자·정확한 압축 내부 경로는 `외부에셋.md` 참조. 원본 PNG 10개 및 두 압축 라이선스의 SHA256 일치, 5개 Sprite GUID 연결을 확인했다. Sprite/Single·Bilinear·무압축·Alpha·비율 유지와 기존 Null 방어를 확인했다.
- 새 PNG 3개가 다운로드 원본과 SHA256 일치하며 5개 PathSymbol·5개 TraitIcon GUID가 각각 한 Sprite `.meta`로 해석됨을 확인했다. 전체 Assembly-CSharp 정적 컴파일 오류 0개, 기존 CS0618 경고 4개이며 관련 diff 검사를 통과했다. 실제 Unity Play Mode의 다섯 카드, 상세, HUD는 수동 확인한다. 마지막 기능 commit: `00f4810`.
- 다음 확인: Bootstrap 새 캐릭터 → PathSelection의 서로 다른 공식 심볼 5개와 작은 TraitIcon → 임의 직업 → Battle 플레이어 상세 → 태온(수호자/지적/Companion Emblem/Mesh Network) → 미엘(치유사/마음의 상처/Heart/Heart Shield) → 런타임 상태 TraitIcon과 Console 오류.

- `PathPresentationResolver`가 PathId로 공식 PathSymbol·길 이름·TraitIcon·특성·추천·설명을 같은 `PlayerPathDefinition`에서 제공한다. PathSelection과 전투 상세가 같은 Resolver를 사용한다.
- PathSelection을 상단 안내, 가로 5개 카드, 하단 상세 패널과 `이 길을 선택` 버튼 구조로 개편했다. 아이콘·이름·특성·한 줄 추천을 함께 표시하고 선택 시 배경, 굵은 테두리, 1.03배 확대, `✓ 선택됨`을 함께 사용한다.
- 공식 PathSymbol은 자체 제작 5종이며 Heart Shield, Sound Waves, Eye Target, Cog, Mesh Network는 White 기본의 TraitIcon이다. 누락 시 텍스트를 유지하는 방어도 유지한다.
- `BattleParticipantSetup.PathId`를 추가해 플레이어는 저장 PathId, 태온은 지적의 길, 미엘은 마음의 상처를 데이터로 전달한다. 이름 문자열 비교와 추천 강제는 없다.
- `PathCombatTraitRuntime`은 PathId가 있는 여러 실제 Combatant를 한 전투에서 독립 관리한다. 태온의 패턴 익히기와 미엘의 회복탄력·행동 2회 소비가 플레이어와 동일한 피해·치유 경로에 실제 연결된다.
- 전투 상세 팝업은 PathSymbol·길 이름과 TraitIcon·특성 이름을 분리하고, 집중·잔향·패턴 등 임시 상태는 TraitIcon을 쓰는 기존 상태 영역에 따로 표시한다. Field 이름표 `Lv.n 이름`은 변경하지 않았다.
- 전체 Assembly-CSharp 응답 파일 별도 컴파일 오류 0개, 기존 deprecated API 경고 4개. 관련 파일 `git diff --check` 통과. 실제 Game View의 5카드 배치와 태온·미엘 패시브 발동, 저장 이어하기는 수동 확인이 남았다.
- 상세: `문서/02_세계관/Path_시스템.md`, `문서/11_UI/길_선택.md`, `문서/10_전투/전투시스템.md`. 마지막 기능 commit: `a77c17d`.

## 최근 길 전투 특성 구현

- `SelectedPlayerPathId`를 전투 시작 때 한 번 읽는 `PathCombatTraitRuntime`을 추가했다. 플레이어에게만 적용하고 태온·미엘 및 추천 외 직업에는 별도 제한이 없어 5×5 조합이 모두 같은 경로를 사용한다. 전투 종료 뒤 상태는 저장하지 않는다.
- 마음의 상처 `회복탄력`은 아군 HP가 처음 50% 이하로 실제 내려갈 때 전투당 1회 발동해 다음 플레이어 성공 행동 2회의 직접 피해·치유를 10% 높인다. 청각 `잔향 포착`은 공격을 마친 실제 적별 잔향과 소비·다음 행동 시작 만료를 처리한다.
- 시각 `집중`은 같은 실제 적 단일 공격을 최대 3중첩으로 추적해 현재 공격에 0/3/6/9%를 적용한다. 광역은 기존 집중 대상만 보정하고 중첩을 만들지 않는다. 지체 `굳건한 자리`는 전열 직접 피해 -5%, 후열 직접 피해·치유 +5%다.
- 지적 `패턴 익히기`는 실제 적·아군 참조 쌍을 추적해 반복 직접 공격 뒤 다음 해당 피해를 10% 줄이고 소비한다. 패턴 보유 아군에게 플레이어가 주는 직접 치유는 대상별 10% 증가하며 패턴을 소비하지 않는다.
- 길 배율은 기존 방어·철벽·가이아·수호의 맹세 앞에서 한 번만 적용한다. 화상·독 DoT에는 길 배율을 적용하지 않고 회복탄력의 실제 HP 경계 통과만 관찰한다. HUD와 상세 팝업은 회복탄력·잔향·집중·굳건한 자리·패턴 및 대상 정보를 텍스트로 표시한다.
- 길 데이터의 직접 능력치 보너스를 비우고 캐릭터 생성 계산과 화면을 `기본 10 + 직업`으로 통일했다. 추천 직업 설명은 실제 전투 시너지를 안내하지만 선택을 잠그지 않는다.
- 전체 `Assembly-CSharp` 응답 파일 별도 컴파일 오류 0개, 기존 deprecated API 경고 4개. 관련 파일 `git diff --check` 통과. 실제 Play Mode에서 다섯 길의 수치·HUD·새 전투 초기화와 기존 스킬/DoT/Save 회귀를 직접 확인해야 한다.
- 상세 규칙: `문서/02_세계관/Path_시스템.md`, `문서/10_전투/전투시스템.md`. 마지막 기능 commit: `c86e931`.

## 최근 월드 레벨·EXP UI 구현

- EXP Fill에 Source Sprite가 없어 uGUI가 `Image.Type.Filled`와 `fillAmount` 대신 전체 사각형을 그리던 문제를 수정했다. 1×1 흰색 런타임 Sprite를 연결하고 왼쪽 시작 Horizontal Filled 방식 하나로 실제 길이를 갱신한다.
- 기존 Screen Space Overlay `PlayerNameplate`에 현재 슬롯의 Level을 연결해 머리 위에 `Lv.n 이름`을 흰색으로 표시한다. 위치·피벗·Camera 추적은 유지하고 몬스터 난도 색은 적용하지 않는다.
- 같은 Overlay Canvas 하단 중앙에 `Lv.n 이름`, `EXP 현재 / 필요`, 금색 진행 Bar를 표시하는 얇은 World HUD를 추가했다. 필요 EXP와 만렙은 기존 중앙 성장 API를 사용하며 Lv50은 `MAX LEVEL`과 Bar 100%로 표시한다.
- 이름·Level·CurrentExperience 값이 실제로 달라졌을 때만 갱신한다. Scene별 Player/Canvas 생명주기와 이름 기반 재사용으로 World 전환 중 중복을 막으며 Battle에는 생성하지 않는다.
- 격리 Unity 6000.5.7f1 빈 Scene Play Mode에서 0/24/50/75/99%, Lv3 85/170=50%, 레벨업 후 Lv2 20/130≈15.4%, Lv50=100%의 fillAmount와 실제 렌더 메시 폭 검사를 통과했다. 실제 Game View의 배치와 저장 슬롯 이어하기·SceneTransition·정상 전투 복귀는 수동 확인이 남았다.
- 상세: `문서/11_UI/월드_레벨과_EXP_HUD.md`. 마지막 기능 commit: `abd0175`, Fill 수정 commit: `486ff5a`.

## 최근 초반 성장과 공용 독 구현

- MaxLevel 50, 다음 레벨 요구 EXP `100 + 25*(L-1) + 5*(L-1)^2`(Lv1~49). CurrentExperience는 현재 레벨 진행량이며 초과분 이월·다중 레벨 업·Lv50 EXP 0을 처리한다.
- 슬라임/독침벌/숲거미/맹독뱀의 Level은 1/2/3/4, Base EXP는 8/12/16/20, HP는 60/70/80/95, 기본 공격은 10/12/14/16이다. Field_01 권장 Lv1~3, Field_02 Lv3~5.
- 몬스터와 플레이어의 레벨 차이로 이름색과 EXP 배율을 공용 판정한다. 회색/녹색/노랑/주황/빨강은 각각 0/50/100/125/150%이며 몬스터 이름에 Lv를 표시한다. 흰색은 플레이어/NPC용으로 유지한다.
- Field_02 기존 거미 4개를 유지하고 맹독뱀 2개를 추가했다. 원본 CC0 이동 4프레임을 프로젝트 전용 복사본에서 재사용하고 전투에서는 오른쪽을 향한다. 뱀 조우는 거미 2+뱀 1, 처치 후 30초 리스폰이다.
- 공용 PoisonDefinition으로 일반 독 최대 HP 5%×3회·맹독 7%×3회를 처리한다. 강한 독은 교체, 약한 독은 거절, 동급은 지속시간만 갱신하며 중첩하지 않는다. 정화·방어 무시 Tick·개체별 부여 대기시간을 유지한다. 향후 투사 독칼/사수 독화살도 같은 구조를 사용할 예정이다.
- 승리 시 실제 처치 개체별 EXP를 합산하고 결과 패널에 EXP/레벨 업/진행량을 표시한다. 현재 슬롯에 Level/EXP와 마지막 월드 위치를 즉시 저장하며 다른 슬롯과 전투 중간 상태는 저장하지 않는다.
- 정적 Assembly-CSharp 컴파일 오류 0개, 기존 CS0618 경고 4개. 격리 Unity 프로젝트에서 성장/배율/독/5개 슬롯/15개 버튼 감사와 실제 Field→Battle→승리→Field→31초 리스폰 자동 Play 검증 통과. Editor Search 패키지 시작 예외 1건은 별도로 관찰됐으며 게임 감사 실패는 없었다.
- 실제 사용자 Editor의 정상 전투 입력·시각적 애니메이션·전 직업 스킬/VFX/Timeline/Targeting 회귀는 수동 확인이 남았다. 자동 전투 검증은 적을 테스트 코드로 처치했다. 레벨 업 HP/MP 완전 회복은 미확정이며 기존 새 전투 시작 시 최대 HP/MP 생성 규칙을 변경하지 않았다.
- 상세 규칙·수정 파일·검증 범위: `문서/08_몬스터/초반_성장과_공용_독.md`. 마지막 관련 commit: `5c5a34c`.

## 최근 스킬 선택 버튼 아이콘 수정

- 파이어 볼 Warm Explosion index 4·썬더볼트 Electric Impact index 1·정화 Spectral Bloom index 5를 버튼 전용 고정 Sprite로 생성하고 null/파괴된 캐시는 다시 읽는다. 상세·상태·VFX 로더와 전투 계산은 변경하지 않았다.
- 기존 캐시는 파괴된 Sprite도 그대로 반환해 버튼 자식 Image가 생성되지 않았고, Rebuild로 복구되지 않았다. 수정 전 파괴 캐시 재현 실패를 확인했다. 실제 사용자 세션에서 최초 무효화를 일으킨 이벤트는 미확정이다.
- 추가로 Play 진입 때 동료의 습격 Wolf 버튼의 무효 캐시 재사용을 확인·복구했다. Wolf Run index 4와 나머지 정상 매핑은 유지한다. 현재 코드/문서 기준 도발은 pawn_left, 치유의 빛은 suit_hearts다.
- 15개 스킬의 실제 버튼 생성 감사: Edit Mode 및 빈 Scene Play Mode 2회, 각각 3회 재생성/부모 닫기·열기에서 Sprite 할당·enabled·activeSelf·alpha·캐시 재사용 통과. 세 Grid 버튼의 파괴/null 캐시 복구 후 Image 재할당도 통과했다.
- 전체 Assembly-CSharp 정적 컴파일 오류 0개, 기존 deprecated API 경고 4개. Unity 배치 Editor 컴파일 및 관련 staged diff 검사 통과. 상세 매핑/검증 범위는 `문서/10_전투/전투_UI_아이콘_에셋.md` 참조.
- 실제 Battle의 마도사 → 치유사 → 수호자/사수/투사 각 3개 버튼을 눈으로 확인하고, `열기 → Esc → 다시 열기` 반복·다음 턴·다음 전투 유지 여부를 수동 확인해야 한다. 자동 감사는 전투 진행을 실행하지 않았다.
- 마지막 관련 commit: `c870aaa`.

## 구현 확인된 항목

### 캐릭터 생성과 선택

- `Level + JobId` 기반 결정적 6능력치 성장과 Lv50 중앙 상한 구현. 딜러 주 스탯 평균 +1.5 정수 패턴, 체력 HP, 직업별 HP 성장, 수호자/치유사 의지 파생 수치를 계산하며 Path는 성장 계산에서 제외. 수호자·치유사의 미확정 6능력치 레벨 성장은 적용하지 않음
- 파생 능력치는 저장하지 않고 각 슬롯의 Level/JobId로 복원. 경험치 곡선·몬스터 EXP·현재 캐릭터 승리 보상을 구현했으며 향후 파티 EXP 분배 정책은 미확정

- Bootstrap의 5개 캐릭터 슬롯 목록과 슬롯별 `이어하기 / 새 캐릭터` 런타임 UI. 새 캐릭터는 기존 4단계 생성 흐름 유지
- `GameSaveData` Version 1 JSON을 Unity Editor 프로젝트 로컬 `UserData/Saves/save_slot_01.json`~`05.json`에 독립 저장
- 선택 슬롯만 자동 저장하며 손상 슬롯은 다른 슬롯에 영향을 주지 않고 사용 불가로 표시
- 기존 `Application.persistentDataPath/project_limitless_save.json`은 슬롯 1이 비었을 때만 검증·복사하고 원본 유지
- FinalConfirmation 확정, 정상 마을/Field 전환 완료, Battle 종료 뒤 Field 복구 시 자동 저장하며 Battle 중간 상태는 제외
- 손상 JSON·Version 불일치·잘못된 Visual/Path/Job/Scene을 삭제·덮어쓰기 없이 거부하고 새 게임으로 안전 복귀
- Editor 전용 `Project Limitless/Test/Manage Local Saves` 창의 슬롯별/전체 삭제 기능
- WorldBounds가 있는 마을/Field에서 5초 주기·Pause/Quit·Scene 도착·Battle 복귀 시 실제 float 좌표를 선택 슬롯에 저장
- 이어하기는 같은 Scene의 Bounds 안 유효 좌표를 우선하고 좌표 없음·NaN/Infinity·Bounds 밖이면 기존 SpawnPoint로 fallback
- SceneTransition 시작 시 이전 Scene 좌표를 무효화하고 목적지 Spawn 배치 뒤 새 좌표 저장
- Bootstrap 저장 슬롯의 `삭제` 버튼, 캐릭터 정보·복구 불가 경고 확인창, 해당 슬롯만 삭제 후 즉시 빈 슬롯 갱신

- `Bootstrap → CharacterCreation → PathSelection → JobSelection → FinalConfirmation → World_StarterVillage` Scene 흐름
- 이름 입력과 검증
- Male/Female 기본 외형 선택과 세션 유지
- 5개 길 선택 및 길별 전투 패시브 프리뷰, 길 직접 능력치 보너스 없음
- 길별 추천 직업 안내와 모든 5×5 조합 선택 허용
- 5개 직업 선택
- 직업 능력치, 패시브, 시작 스킬 3개 프리뷰
- FinalConfirmation의 선택 결과와 최종 능력치 표시

### 플레이어 외형과 월드 표시

- Player Nameplate
- Path Visual 데이터와 UI Preview
- 청각·시각·지체 길의 Male/Female Character Variant
- 지체 길의 전투형 전동 휠체어 Variant
- 지적 길의 동행의 문장 Symbol
- 마음의 상처 길의 이어진 심장석 Symbol
- 기본/Variant 공통 Player 이동과 방향 Animation 구조

### 마을과 필드

- `World_StarterVillage`의 Player 이동, Camera 추적, NPC E 대화
- `Field_01` Scene과 야외 환경
- 마을 남문 → Field_01 북쪽 입구 이동
- Field_01 북쪽 입구 → 마을 남문 복귀
- `SceneTransitionService`, `SceneTransitionTrigger`, `SceneSpawnPoint`
- 공통 `WorldBounds2D`와 `WorldBoundaryGeneratorUtility`
- Orthographic viewport 크기를 고려한 Camera Bounds
- Exit Opening을 제외한 월드 외곽 Collider
- 마을 외곽 울타리의 Tile 영역 밖 돌출 방지

### 첫 필드 몬스터

- 모든 Field 출입구에 데이터 기반 원형 몬스터 안전지대 적용. 생성점과 활동 반경 전체를 보정하며 2초 재조우 유예와 별도로 유지
- Field_01 북/남 안전지대와 Field_01↔Field_02 데이터 기반 양방향 연결
- Field_02 숲길 기본 Scene·그늘 테마·World/Camera Bounds 재사용, 숲거미 4개 스폰
- CC0 Spider Idle/Walk/Attack/Shoot 선별 프레임 연결, `독액 분사` 생존 아군 전체 직접 피해 20 구현
- Field_02 거미 조우를 전열 독침벌 2·후열 숲거미 1로 변경하고 숲거미 기본 공격 14, 독액 분사 20을 분리 유지
- 신규 Field/같은 Field 신규 몬스터가 직전 최강 일반 몬스터와 함께 등장하고 약 10~15%를 출발점으로 Play Mode에 맞춰 데이터 조정하는 공통 성장 규칙 문서화
- 같은 MonsterDefinition을 공유하는 복수 몬스터도 실제 Combatant 참조별로 감전·독·화상·독침 대기시간을 독립 저장하며, 썬더볼트 피해 처리 뒤 살아남은 EnemyAll 각 대상에 감전 1을 별도 단계로 적용

- 데이터 기반 `MonsterDefinition`과 Scene별 `FieldMonsterSpawnDefinition`
- Field_01 여러 빈터의 `초원 슬라임` 5마리 데이터 기반 런타임 배치
- 스폰별 시작 위치와 활동 반경 안의 독립적인 느린 무작위 배회
- Rigidbody2D 기반 장애물 Collider 충돌
- 플레이어 접촉 시 이동 정지와 `MonsterEncounterService.EncounterStarted` Event 발생
- 사용자 제공 4×4 Sprite 시트 기반 초원 슬라임 방향별 Idle/Walk Animation
- 배회 방향·이동/대기 상태와 `GrassSlime.controller` 동기화
- 초원 슬라임 데이터에 실제 Sprite/Animator 연결 및 녹색 Placeholder 미사용
- 승리한 스폰만 제거하고 데이터 기본값 30초 후 원래 위치에 독립 리스폰, 도망 시 스폰 유지
- `MonsterDefinition.DisplayName`을 표시하는 재사용 가능한 필드 몬스터 Overlay 이름표
- 레벨 차이별 이름색·Lv 표기·검은 외곽선의 이동 추적 이름표, 필드 HP Bar 미포함
- 독 몬스터 원본 보존: Pilot Bee(CC BY, 라이선스 버전 표기 충돌 기록)·2D Spider(CC0)·Simple Green Snake(CC0). 벌은 Field_01, 거미와 맹독뱀은 Field_02에 구현
- Pilot Bee 검증 Scene: Idle 238×215×10·Attack 315×253×10·기본 우향 구조를 런타임 분할하고 현재 초원 슬라임과 나란히 비교. Point Filter·무압축·투명·Read/Write 검증 복사본과 기본 Scale 0.85 제공, Field/Battle 미연결
- Pilot Bee 검증 Scene 입력을 새 Input System의 null 안전 `Keyboard.current` 방식으로 수정하고 메인 키보드·Numpad +/-를 지원. Scene 전용 직교 Main Camera를 연결해 `No cameras rendering` 표시 제거
- Field_01에 데이터 기반 독침벌 3개 스폰(`venom_bee_01`~`03`)을 추가해 기존 초원 슬라임 5개와 총 8개 배치. 공용 설치기·배회·접촉 조우·개별 30초 리스폰·도망 유지·2초 재조우 유예 재사용
- `02_VenomBee` MonsterDefinition이 Pilot Bee Idle/Attack 시트 구조와 Scale 0.85를 Field/Battle에 공통 제공. 런타임 분할 재생으로 원본 PNG를 수정하지 않으며 공용 일반 독을 부여

### 1차 턴제 전투

- 치유사 MP 숫자를 상단 HP HUD에서 제거하고 `UsesMp` 참가자의 Hover/Focus 상세 팝업 HP 다음 줄로 이동. 다른 직업은 빈 MP 줄을 표시하지 않음
- 모든 스킬 Tooltip·재사용 중 UI 용어를 `재사용 대기시간`으로 통일하고 공통 Tooltip 생성 경계에서 `구현: 사용 가능/미구현` 개발 상태 문구를 제거. MP 사용 스킬은 고정 비용을 함께 표시
- 화살비·회오리 베기·썬더볼트 광역 Target과 실행기에서 실제 Combatant 참조 중복을 제거해 대상당 피해 1회를 보장. 빈 슬롯·VFX 수는 피해/기세 횟수에서 제외하며 독침벌 A/B는 별개 인스턴스로 유지. 숲거미 독액 분사도 같은 참조 안전장치 적용
- 적대 광역 스킬은 스킬 메뉴를 닫기 전에 유효 Target을 검사한다. 화살비 후열 0명·회오리 베기 전열 0명·썬더볼트 전체 0명이면 범위별 경고 후 스킬 메뉴·취소·키보드 포커스를 즉시 복구하며 행동·턴·자원·쿨타임·VFX를 시작하지 않음
- 치유사 전용 MP 런타임 자원과 `MP 현재/최대` HUD 구현. MaxMP는 `임시 기본값 + (Level-1)×1 + 지능×2`로 재계산하며 레벨 성장 +1과 지능 계수 +2는 확정. 자기 행동 종료 회복 및 치유의 빛/회복의 파동/정화 선검사·성공 차감 적용
- 치유사 스킬은 고정 비용으로 `정화 < 치유의 빛 < 회복의 파동`을 유지한다. 치유의 빛은 쿨타임 없음, 회복의 파동 3턴·정화 2턴은 유지하며 기본 MP·회복량·실제 비용 숫자는 임시값, 레벨업 시 MP 완전 회복 여부는 미확정
- 행동 우선도→민첩→전투 시작 1회 Tie Break 순서 구현. 실제 Combatant 참조별 값을 전투 동안 유지하고, 민첩 동률 전투에서만 Overlay를 열어 전투 RNG와 분리된 지역 `System.Random`으로 1~6 눈을 0.09초 간격으로 1.2초간 변경. 최종 눈도 직전 값과 다르게 별도 추첨하며 입력 잠금과 실제 첫 행동/Timeline 판정 유지
- 회복의 파동·정화·철벽·수호의 맹세·가이아 웰 VFX는 캐릭터 본체와 독립된 Image만 사용한다. Image는 생성 즉시 숨기고 첫 유효 Sprite·Tint 설정 뒤 표시하며, null 프레임은 숨기고 종료 즉시 비활성화·파괴한다. CharacterSprite 뒤 강제 배치를 제거해 원본 후광의 안쪽이 가려져 바깥 색 레이어처럼 분리되는 현상을 방지했고, 수호의 맹세 이전 섬광의 의도적 null Sprite도 유효 Orb Sprite로 교체

- 재사용 가능한 `Battle` Scene과 Build Settings 연결
- `Combatant`, 전열/후열 2×3 `Formation`, `TargetResolver`, `TurnOrderQueue` 분리
- 근거리 전열·직선 후열 보호, 원거리 후열 우선, 마법 자유 대상 판정
- 플레이어와 몬스터 공통 대상 판정 및 단일 행동 도발 강제 대상 구조
- 민첩과 행동 우선도 기반 순서 및 다음 행동 타임라인
- 플레이어 이름·현재 외형, 초원 슬라임 Sprite와 양측 HP 표시
- 마우스·키보드 지원 남색/금색 전투 UI와 공격 가능·불가능 텍스트 구분
- 기본 공격, 다음 행동까지 피해 50% 감소 방어, 일반전 도망
- HP 0 전투불능, 적 전멸 승리, 승리·도망·패배 후 `Field_01` 복귀
- 복귀 시 전투 상태 초기화, 접촉 위치 이격과 2초 조우 유예
- 스킬 버튼과 NPC 직접 지시 확장용 `IsPlayerControlled` 구조
- 기본 공격 사거리: 수호자·투사 근거리, 사수 원거리, 마도사·치유사 마법. 치유사는 낮은 피해의 마법 공격
- 논리 2×3 Formation을 숨기고 적 왼쪽·아군 오른쪽의 사이드뷰 전장 좌표로 Sprite 배치
- 타임라인 아래·전장 위의 상단 HP 전용 HUD와 적군 3열×최대 2줄·아군 3열×1줄 이름/비율 HP 항목
- 캐릭터 근처 이름·HP·상태 텍스트 제거, Sprite 가로 중앙 위 대상 화살표와 행동 중 표시만 유지
- 상단 HP 항목의 행동 중·방어·도발·구현 스킬 재사용 턴 한 줄 요약과 상세 팝업 역할 분리
- 전투불능 시 HP HUD·상세 팝업의 행동 상태/쿨타임 제거와 회색 `전투불능` 단일 표시
- Hover/마우스 선택/키보드 포커스 공용 단일 상세 상태 팝업과 포커스 캐릭터·고정 HP 행 동시 강조
- 평상시 회색 발판 제거, 대상 선택 중 공격 가능 대상만 얇은 금색 선과 선택 화살표로 표현
- 상단 한글 `전투` 제목과 현재 행동자 강조·이후 순서 타임라인, 하단 전용 명령 패널
- 조작 안내를 13px 글씨로 명령 패널 내부 하단에 배치하고 버튼과 하단 padding을 확보
- 약 17% 축소한 공용 명령 버튼·축소 명령 패널과 대상/스킬 선택 중 Esc와 같은 흐름을 실행하는 마우스 `취소` 버튼
- Kenney Game Icons·Board Game Icons CC0 실제 Sprite와 한글을 함께 사용하는 공격·스킬·방어·도망·취소 버튼 및 HP HUD 상태 배지
- 역할 ID와 Resources 경로를 한곳에서 연결하는 `BattleUiIconCatalog`, Sprite 누락 시 문자 기호 없이 한글만 남기는 fallback
- HP HUD 도발은 `pawn_right`, 3턴 재사용은 표시값 3=`hourglass_top`·2=`hourglass`·1=`hourglass_bottom`으로 구분
- 직업 스킬 데이터의 `IconId`를 통해 수호자 도발=`pawn_left`, 치유의 빛=`suit_hearts`, 정조준=`target` Sprite를 스킬 이름 왼쪽에 표시
- 스킬 버튼은 아이콘·이름 중심으로 단순화하고, Hover와 키보드 포커스가 공유하는 팝업에서 설명·대상·효과·재사용·구현 여부를 표시
- 도발·치유의 빛·정조준의 현재 구현 규칙과 일치하는 설명 데이터를 `BattleSkillDefinition`에 연결하고 취소 아이콘을 `arrowLeft`로 교체
- 외부 신규 에셋 없이 남색·금색 하늘·원경·지면 층의 임시 전투 배경 구성
- 플레이어·NPC·몬스터가 공통 사용 가능한 `BattleActionPresenter`와 전투 계산 분리
- 근거리 기본 공격의 짧은 전진·타격 대기·원위치 복귀, 피격 좌우 흔들림·점멸, 떠오르는 피해 숫자
- 연출 중 명령·대상 선택·취소 입력 잠금과 연출 완료 후 다음 턴 진행
- 사수 기본 공격의 짧은 조준, 재사용 가능한 UI Projectile 이동, 도착 시 피해·피격 연출과 다음 턴 연결
- 마도사·치유사 기본 공격의 짧은 캐스팅과 Projectile, Fireball·밝은 금빛 구체 시각 구분
- 필드 Animator 현재 상태와 분리된 전투 Sprite 해석기, 아군 Left Idle·적 Right Idle 진입 및 공격 후 복구
- 전투 적 구성을 전열 초원 슬라임 A/B와 후열 독침벌 1로 변경. 독침벌은 원본 우향 Idle을 반복하고 기본 공격 중 Attack 10프레임으로 전환한 뒤 Idle 복귀, 피해 계산은 기존 적 기본 공격 유지
- 독침벌 기본 공격은 MonsterDefinition의 120% 데이터로 슬라임 10 대비 12 피해를 매 행동 적용. 독 부여 가능 공격만 독 3 부여·갱신 후 공격자별 독침 대기 2를 시작하고, 이후 해당 벌의 행동 종료에만 `2→1→0` 감소하여 네 번째 행동부터 다시 부여
- 대상의 독은 자신의 행동 종료마다 최대 HP 5% 올림·최소 1 피해로 `3→2→1→제거`되고 재적중은 독 3 갱신이다. 대상 독 지속시간과 벌의 독침 대기시간은 별도 저장되어 정화·자연 종료가 공격자 대기시간을 초기화하지 않음
- 독 HUD·상세 팝업에 PVFX Venom Ward index 6 아이콘과 `독 n`, `행동 종료 시 최대 HP 5% 피해` 표시. 틱은 작은 Acid Splash index 3~8과 피해 숫자만 사용하며 화상과 함께 있으면 두 틱 후 다음 턴 진행
- CC0 Polar_34 - Projectiles 원본 GIF 보존, 32×32 PNG Sprite 프레임 변환 및 사수 golden arrow·마도사 fireball 기본 공격 적용
- 시작·목표 X 좌표 비교 기반 공용 Projectile 좌우 반전과 GIF 프레임 지연 재생
- PVFX Foundry 0.3.0 CC0 원본·라이선스 보존, Magical Projectile travel 5프레임을 치유사 기본 공격에 적용
- 치유사 치유의 빛: 자신 포함 살아 있는 단일 아군, 최대 HP 35% 올림 회복, 최대 HP·전투불능 안전 처리
- 치유사 회복의 파동: Formation의 살아 있는 아군 전체(자신 포함)를 치유의 빛 기본 회복량의 60%로 동시 회복, 전투불능 제외·최대 HP 상한·회복 대상 없음 행동 미소비·치유사 행동 기준 3턴 쿨타임
- 회복의 파동 연출: 치유사 중심 Arcane Parry 1회와 대상별 작은 Radiant Heal 병렬 재생, Radiant Heal peak에서 전체 HP·회복 수치·HUD 동시 갱신, peak 고정 프레임 버튼/상세 아이콘 분리
- 치유사 정화: 살아 있는 아군 1명의 독·화상·감전을 `HarmfulStatusType` 공통 분류로 모두 제거, 상태 없음 행동 미소비, 치유사 행동 기준 2턴 쿨타임
- 정화 연출: PVFX spectral-bloom 96×96 전체 16프레임을 대상 위치에서 20 FPS로 재생하고 release index 7에서 상태·HUD 갱신, peak index 5 고정 버튼/상세 아이콘 분리
- PVFX Radiant Heal 96×96 14프레임을 대상 위치에서 재생하고 peak 7프레임에 회복·`+회복량`·HUD 갱신
- 치유 대상 선택 중 Esc/취소는 스킬 메뉴로, 스킬 메뉴 취소는 기본 명령으로 돌아가는 단계별 입력 흐름
- 사수 정조준: 기존 원거리 TargetResolver 후열 우선, 기본 공격력 160% 정수 올림, 사수 행동 기준 2턴 쿨타임
- `정조준!` 강조와 기존 golden_arrow 0.42초 이동, 도착 순간 피해·HP HUD·피격 연출 적용
- 사수 `화살비`: `EnemyRearRowAll`의 살아 있는 적 후열 전체에 일반 공격 120% 피해, 후열 0명에서는 전열 전환 없이 행동·쿨타임 미소비, 사수 행동 기준 2턴 쿨타임
- 화살비 `bow` 버튼 아이콘과 기존 golden_arrow 기반 조준→3발 상승→공중 대기→대상별 3발 낙하→공유 타격·동시 피격의 약 0.75초 연출
- 다중 Projectile은 순수 연출로 관리하고 실제 피해는 마지막 낙하 시점에 후열 대상마다 한 번만 적용한 뒤 전체 피격 완료 후 다음 턴 진행
- 화살비 다중 후열 실검증용 프로토타입 배치: 초원 슬라임 A는 전열 0열, B·C는 후열 0·1열에 배치하여 전열 1명+후열 2명 유지
- 정조준과 화살비 쿨타임은 같은 `BattleSkillCooldowns`에서 참가자·Skill ID별로 독립 관리하며, 화살비는 모든 피격 반응 완료 후 성공 확정 시 2턴 등록
- 사수 `동료의 습격`: 전후열 자유 단일 적, 도발 강제 대상 우선, 일반 공격 180%, 사수 행동 기준 3턴 쿨타임
- 별도 Combatant가 아닌 기본 Wolf가 매 사용 시 현재 사수 `ActionRoot` 위치·크기로 계산한 근처 지점에서 출발한다. 384×40 시트를 64×40 Sprite 6개로 나눈 12 FPS Run Coroutine과 570 UI 단위/초 Transform 이동 Coroutine이 독립적으로 동시에 실행되며, 대상 바로 앞 도착 시 Run을 멈추고 타격 후 0.12초 뒤 제거된다. `BeastCompanionDefinition` 경계로 Bear/Fox 교체 가능
- 모든 스킬 설명 팝업은 메뉴 Hover/키보드 포커스 중에만 표시하고, 스킬 확정 즉시 공통 경계에서 닫아 대상 선택·연출·행동 종료 뒤 기본 명령 화면까지 숨김 유지
- 모든 Battle Action의 `actionPlaying` 중 스킬 설명과 캐릭터/상태 상세 팝업을 함께 억제하고 기존 Hover·포커스 대상을 비워, 마우스가 HUD 위에 남아 있어도 공격·회복·Projectile·VFX를 가리지 않음
- 동료의 습격 버튼은 작은 버튼에서 실루엣이 선명한 Wolf Run 다섯 번째 프레임(index 4) 기반 실제 동물 아이콘을 사용하고 정조준은 기존 Kenney `target.png` 유지. Wolf 경로와 아이콘 프레임 번호는 `BeastCompanionDefinition`에서 제공
- 마도사 `파이어 볼`: 전후열 자유 단일 마법·도발 우선, 즉발 170%, 명중 당시 Attack의 30% 화상 2회, 마도사 행동 기준 3턴 쿨타임
- 화상은 대상 행동 종료 시 작은 fireball 불꽃과 함께 저장 피해를 한 번 적용하며, 재적중 시 중첩 없이 새 명중 피해·2회로 갱신. HUD와 상세 팝업에 Warm Explosion peak 아이콘+`화상 n` 표시
- 파이어 볼 연출은 Solar Shrapnel 초기 Charge 2프레임→기본 공격보다 큰 기존 fireball→Warm Explosion 15프레임이며 index 4에서 즉발 피해·화상 적용
- 마도사 `썬더볼트`: `EnemyAll`로 살아 있는 적 전열·후열 전체에 90% 피해, 대상별 감전 1, 마도사 행동 기준 3턴 쿨타임 구현. 광역이라 단일 도발 강제 대상은 적용하지 않음
- 감전은 다음 행동의 주는 피해를 15% 감소시키고 공격하지 않는 행동도 종료 시 제거되며, 재적중은 중첩 없이 감전 1 갱신. 전투불능 시 저장소와 HUD에서 제거
- PVFX electric-impact 96×96 14프레임을 청백색 예고 뒤 적 전체에서 거의 동시에 재생하고 peak index 1에서 피해·감전을 함께 적용. 스킬 아이콘은 peak 고정 Sprite, 상태 아이콘은 Kenney `power.png`
- 마도사 `가이아 웰`: 자기 자신에게 받는 피해 60% 감소를 부여하고 현재 행동을 제외한 자신의 다음 2회 행동 종료에서만 감소, 마도사 행동 기준 4턴 쿨타임 유지
- 가이아 웰 활성 중 공용 방어 입력은 지정 안내 후 행동·턴 미소비로 차단하고 버튼을 사용 불가 색상으로 표시. 계산에서도 방어 단계를 건너뛰어 두 효과가 절대 중첩되지 않음
- PVFX arcane-parry 96×96 16프레임을 자신 위치에서 20 FPS 재생하고 peak index 8에서 상태 적용. 실제 `dice_shield.png`로 `가이아 2/1` HUD·상세 상태 표시
- 수호자 `철벽`: 자신에게 받는 피해 70% 감소를 부여하고 사용 행동을 제외한 자신의 다음 2회 행동 종료에서만 `철벽 2→1→제거`, 수호자 행동 기준 4턴 쿨타임 유지
- 철벽은 도발과 동시에 유지하지만 공용 방어 50%와는 입력·피해 계산 양쪽에서 중첩을 차단한다. 활성 중 방어 시 지정 안내 후 행동·턴을 소비하지 않고, 원시 피해 10은 기존 올림 규칙으로 3이 됨
- PVFX `earth-rupture` 96×96 20프레임을 발밑에서 20 FPS로 한 번 재생하고 peak index 9에서 상태를 적용한다. Kenney `structure_wall.png`를 스킬 버튼과 `철벽 2/1` HUD·상세 상태에 함께 사용
- 수호자 `수호의 맹세`: 수호자의 다음 행동 시작 전까지 자신을 제외한 같은 진영 생존 아군 전체의 직접 공격·공격 스킬 피해를 최대 50% 감소시키고 감소량 절반을 수호자에게 이전. 고정 3인 목록 없이 진영·생존 상태를 피격 순간 판정
- 수호의 맹세 이전 예산은 사용 시 수호자 최대 HP 40%를 올림 계산하고 철벽 적용 전 이전 예정량으로 소비. 예산 부족 시 실제 이전량의 두 배까지만 감소하며 0 또는 수호자 전투불능에서 즉시 종료. 발동 시 대상별 `수호의 맹세 -감소량` 한 줄과 금색 이전선·섬광 표시
- 이전받은 실제 피해는 철벽 70%·공용 방어 등 수호자의 기존 개인 방어를 정상 적용하며, 화상·독 같은 DoT는 `BattleDamageOrigin` 분류로 보호에서 제외. 도발·철벽과 동시 유지하고 수호자 행동 기준 4턴 쿨타임
- PVFX `frost-nova` index 4~10을 금백색·낮은 알파의 짧은 파티 범위 VFX로 사용하고, 실제 이전 때 피격 아군→수호자 금색 선·섬광 표시. Kenney `pawns.png`를 버튼·수호자 HUD에 사용하고 상세 팝업에 남은/최대 이전 예산 표시
- 투사 `난도`: 기존 근거리 TargetResolver로 적 1명을 선택하고 전진 타격 순간 일반 공격 150% 피해와 자신 기세 +1 적용
- 기존 근거리 기본 공격 Presenter를 재사용하는 전진→타격·피해 숫자·피격 반응→원위치 복귀→다음 턴 흐름
- `난도`는 스킬명, `기세`는 Combatant별 0~3 개인 자원이며 난도 직접 세 번째 사용 기준 2턴 쿨타임과 독립 관리
- 투사 `회심의 일격`: 근거리 단일 공격, 기세 0/1/2/3에 일반 공격 100/130/160/190%, 적중 후 기세 전부 소비, 자체 쿨타임 없음
- `GetMomentum`·`AddMomentum`·`ConsumeAllMomentum` 구조로 회오리 베기 명중당 획득 확장 준비와 회심 소비 연결
- 난도 버튼 `cross`, 기세 HUD·회심의 일격 버튼 `skull`, 상단 `기세 n`·상세 `기세 0/3~3/3`, 전투불능 시 숨김
- 회심의 일격 설명 팝업에 데이터 기반 현재 기세와 현재 예상 피해 배율 표시
- 투사 `회오리 베기`: 살아 있는 적 전열 전체에만 일반 공격 80% 피해, 실제 적중한 전열 적 1명당 기세 +1(최대 3), 자체 쿨타임 없음. 전열이 비면 행동 미소비로 사용 불가
- 스킬 전용 광역 범위 `EnemyFrontRowAll`·`EnemyRearRowAll`·`EnemyAll`과 공용 해석기 구조. 회오리 베기는 전열, 화살비는 후열, 향후 썬더볼트는 적 전체 범위로 확정
- 회오리 베기는 `spinner` 버튼 아이콘과 투사 중심 약 0.3초 코드 기반 회전 참격을 사용하며, 동시 피해 숫자·피격 반응·HUD 갱신 후 다음 턴 진행
- 회오리 베기의 기세 획득은 공용 `AddMomentum`만 사용하고 난도 직접 사용 기록을 변경하지 않아 난도 쿨타임과 독립
- JobDefinition 프리뷰를 사용하는 재사용 가능한 전투 스킬 카탈로그·실행기·참가자별 쿨타임·상태효과 런타임
- 실제 스킬 메뉴와 Esc 복귀, 미구현 스킬 비활성 표시, 수호자 도발 제자리 강조 연출
- 수호자 도발의 적 전체 적용, 적별 다음 2회 행동 소모, 수호자 행동 기준 3턴 쿨타임과 적 HUD 상태 표시
- 참가자 목록 기반 `BattleEncounterSetup`과 3대3 프로토타입 Factory, 기존 2×3 Formation을 사용하는 실제 N대N 전투 생성
- 플레이어·태온(수호자)·미엘(치유사)의 플레이어 직접 조작과 독립 HP 미니 HUD
- 전열 슬라임 2명·후열 슬라임 1명의 독립 Combatant·턴·HP·도발 상태와 6명 전체 행동 타임라인
- 적·아군 후보를 공통 처리하는 대상 선택 UI 기반과 태온·미엘 코드 생성 임시 Visual
- 현재 HP·직업/몬스터 분류·방어·도발·구현 스킬 쿨타임을 계산 코드 변경 없이 조합하는 `BattleCombatantStatusViewModel`
- 방어·도발 표식을 안정적 ID와 수치로 분리해 향후 무료 아이콘 Asset으로 교체 가능한 표시 구조

## 데이터만 있고 실행 로직이 없는 항목

- 길 능력치와 고유 패시브의 전투 효과
- 직업 능력치, 패시브, 시작 스킬 프리뷰
- AP, 상태이상, 협동 기술, 보스 패턴의 초안 방향

이 항목들은 캐릭터 생성 UI에서 표시되지만 실제 전투 계산이나 효과로 실행되지 않는다.

## 문서화된 전투 설계

- 아군·적 공통 전열 3칸과 후열 3칸의 2×3 진형
- 근거리·원거리·마법 사거리와 바로 앞 전열의 후열 보호
- 단일 적대 행동에 일반 사거리보다 우선하는 도발 강제 대상
- 민첩·행동 우선도 기반 솔로 순서와 시간 제한 없는 직접 행동 지시
- 공용 공격·스킬·방어·도망 명령 및 승리·도망 필드 복귀
- 멀티플레이는 동시 입력, 기본 45초 제한, 남은 시간 10초 경고, 시간초과 시 방어로 설계되어 있으나 네트워크는 미구현
- AP, 상태이상, 행동·협동 기술, 보스 패턴은 미확정이며 구현하지 않음

## 미구현

- 투사 시작 스킬 3종을 제외한 나머지 직업별 스킬 효과와 길 패시브
- `BeastCompanion` 선택·장착·저장 UI와 Wolf/Bear/Fox 패시브 수치·능력치 계산
- NPC 동료 정식 CompanionDefinition·파티 편성·최종 Sprite
- 여러 몬스터 배치 전투와 보스전 실제 콘텐츠
- AP와 상태이상, 행동·협동 기술, 보스 패턴
- 멀티플레이 네트워크 전투
- 인벤토리·아이템·아이템 보상·파티 EXP 분배 정책
- 퀘스트와 영구 저장·불러오기
- 인스턴스 던전과 Field_02 이후 지역

## 현재 검증 상태

- MP 상세 팝업 이동·Tooltip 공통 문구 정리·광역 참조 중복 제거는 관련 파일 `git diff --check`와 Unity 6000.5.7f1 배치 실행 종료 코드 0을 확인했다. Presenter의 화살비·회오리 베기·썬더볼트·독액 분사 계산 콜백이 연출당 한 번인 구조와 Executor의 대상당 단일 피해를 정적으로 교차 확인했으며, 실제 1/2/3명 피해 숫자와 HUD 배치는 Play Mode 확인이 필요하다.
- 치유사 MaxMP 공식 변경은 관련 파일 `git diff --check`와 Unity 6000.5.7f1 배치 실행 종료 코드 0을 확인했다. 지능 12 기준 Lv1 44·Lv2 45·Lv3 46·Lv50 93, 마도사 0을 정적으로 교차 확인했으며 실제 MP 소비·행동 종료 회복·쿨타임·저장 슬롯별 재계산은 Play Mode 확인이 필요하다.
- 성장 능력치·치유사 MP·민첩 Tie Break 변경은 관련 파일 `git diff --check`를 통과했고 Unity 6000.5.7f1 배치 실행이 종료 코드 0으로 완료됐다. 실제 Play Mode의 Lv별 표시, MP HUD/회복/부족 거절, 동률 유무별 Overlay와 Timeline 일치는 사용자가 직접 확인해야 한다.
- Unity 6000.5.7f1의 전체 `Assembly-CSharp` 참조 응답과 Roslyn으로 새 전투 코드를 포함해 컴파일했으며 Compiler Error 0개를 확인했다.
- 치유사 기본 공격을 마법 사거리로 매핑하고 기존 검증용 공격력보다 4 낮게 적용한 뒤 동일한 Unity 참조로 Compiler Error 0개를 재확인했다.
- 임시 실행 테스트로 근거리 보호·후열 개방, 원거리 후열 우선, 마법 자유 대상, 도발 우선·2회 지속, 방어 50%, 행동 우선도·민첩 정렬의 12개 검증을 모두 통과했다.
- `Battle.unity` GUID와 Build Settings GUID 일치, Battle 항목 1개, 새 C# meta 3개와 `git diff --check` 통과를 확인했다.
- 사용자가 Unity Play Mode에서 `Field_01 → 초원 슬라임 → Battle` 진입, 기본 공격, 적 자동 공격, 턴 순환을 정상 검증했다.
- 사용자가 방어 시 받는 피해가 10에서 5로 감소해 50% 방어 규칙이 정상임을 확인했다.
- 사이드뷰 UI 변경 후 Unity 전체 `Assembly-CSharp` 참조로 Compiler Error 0개를 확인했다. 실제 사이드뷰 화면 배치와 입력 회귀는 사용자가 다시 확인해야 한다.
- 조작 안내를 명령 패널 내부로 옮긴 뒤 동일한 Unity 전체 참조로 Compiler Error 0개를 확인했다. 16:9에서의 하단 padding과 버튼 간격은 사용자가 직접 확인해야 한다.
- HP Fill의 실제 Rect 폭 갱신과 스폰 ID별 처치·30초 리스폰 구조를 적용한 뒤 Unity 전체 `Assembly-CSharp` 참조로 Compiler Error 0개를 확인했다.
- 사용자 Play Mode 확인에서 한 마리만 생성되는 문제를 재현했고, Unity Editor 로그에서 `grass_slime_02`~`05`의 meta YAML 마지막 줄바꿈 누락으로 GUID가 무효 처리되어 Asset import가 제외된 원인을 확인했다.
- 새 4개 meta를 정상 형식으로 수정하고, Installer의 실제 로드 개수·고유 ID 로그와 Field01SceneGenerator의 5개 ID·30초·최소 3유닛 간격 검증을 추가했다. Runtime/Editor C# 컴파일 오류 0개를 확인했으며 Play Mode 재검증이 필요하다.
- `BattleCore.cs`가 변경되지 않았으며 공격 10, 방어 50%, Formation·TargetResolver·TurnOrderQueue 계산 규칙을 유지했다. 실제 HP Bar 비율과 필드 리스폰 시간은 Play Mode에서 사용자가 확인해야 한다.
- 이전 사이드뷰 UI 작업에서는 `BattleCore.cs`와 조우/복귀 코드가 변경되지 않았음을 정적으로 확인했다. 이번 작업은 `BattleSceneFlow.cs`의 승리·도망 결과 전달만 확장했으며 전투 계산 규칙은 변경하지 않았다.

- 월드 전환 Scene/Spawn ID, Missing Script, Bounds 참조와 viewport 계산은 정적으로 확인했다.
- 초원 슬라임 Script GUID, Monster/Spawn Asset 연결, Field_01 대상 Scene과 조우 Event 구조를 정적으로 확인했다.
- 초원 슬라임 PNG를 1256×1256, 314×314 Cell의 4×4 구조로 확인하고 Sprite 16개, Animation Clip 8개, Animator와 데이터 참조를 정적으로 교차 확인했다.
- `MonsterNameplate`가 몬스터별 Text를 분리하고 `DisplayName`을 받으며 Camera 이동 뒤 화면 좌표를 갱신하는 구조를 정적으로 확인했다.
- Unity 6.5에서 오류가 된 `GetInstanceID()`를 권장 API인 `GetEntityId()`로 교체했다.
- Unity 6000.5.7f1이 사용하는 Roslyn과 전체 `Assembly-CSharp` 응답 파일로 재컴파일하여 Compiler Error 0개와 종료 코드 0을 확인했다.
- 최신 Unity Editor 로그에서 `MissingReferenceException`과 `NullReferenceException` 기록이 없음을 확인했다. 실제 Play Mode 기능 검증은 사용자가 직접 확인해야 한다.
- 관련 C# 변경은 `git diff --check`를 통과했다.
- 기본 공격 액션 연출 코드를 Unity 6000.5.7f1의 전체 `Assembly-CSharp` 참조와 Roslyn으로 컴파일해 오류 0개를 확인했다. 기존 API deprecation 경고만 남아 있다.
- `BattleCore.cs`, `TargetResolver`, `Formation`, 피해·방어 계산은 변경하지 않았다. 근거리 연출을 유지하고 사수 원거리 기본 공격만 Projectile 연출에 연결했으며, 마법 기본 공격은 기존 즉시 처리 흐름을 유지했다.
- 사수 Projectile 코드를 Unity 6000.5.7f1의 전체 `Assembly-CSharp` 참조와 Roslyn으로 컴파일해 오류 0개를 확인했다. 조준 시간·Projectile 위치와 방향·입력 잠금은 Play Mode 확인이 필요하다.
- 기본·Path Variant·휠체어·초원 슬라임 Animator Controller에서 `Idle_Left`·`Idle_Right` 상태를 확인하고, `BattleVisualResolver`를 포함한 전체 `Assembly-CSharp` 컴파일 오류 0개를 확인했다. 실제 방향과 첫 프레임은 Play Mode 확인이 필요하다.
- `AnimationClip.SampleAnimation`이 Sprite PPtr 곡선을 적용하지 못하던 경로를 임시 Animator 상태 평가로 교체하고, Path Variant 기본 Sprite fallback과 null UI 투명 처리를 추가했다. 전체 `Assembly-CSharp` 컴파일 오류 0개이며 흰 사각형·경고 제거는 Play Mode 확인이 필요하다.
- 마도사·치유사 기본 마법 Projectile과 런타임 Orb Graphic을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 피해·치유사 감소값·마법 사거리 계산은 변경하지 않았으며 색상·도착 시점·Idle 복구는 Play Mode 확인이 필요하다.
- golden arrow 8프레임(80ms), fireball 7프레임(60ms)을 원본 32×32 크기로 추출하고 복사 전후 SHA-256 일치를 확인했다. 실제 에셋 연결 후 전체 Assembly-CSharp 컴파일 오류 0개이며 표시 크기·방향·프레임 재생은 Play Mode 확인이 필요하다.
- 사수 golden arrow와 마도사 fireball 이동시간을 각각 0.35초로 조정하고, 프레임 간격·도착 후 피해 적용·치유사 Projectile 0.24초·근거리 속도는 유지했다. 전체 Assembly-CSharp 컴파일 오류 0개이며 체감 속도는 Play Mode 확인이 필요하다.
- 치유사 임시 Orb를 PVFX Magical Projectile의 96×96 travel 프레임 5개로 교체했다. manifest 픽셀 해시 일치, Point Filter·투명 Sprite·50ms 프레임·0.24초 이동 유지와 전체 Assembly-CSharp 컴파일 오류 0개를 확인했다.
- 수호자 도발 스킬 구조를 Unity 전체 Assembly-CSharp 참조로 컴파일해 오류 0개를 확인했다. Combatant·TargetResolver 기존 도발 우선 판정을 재사용하고 BattleCore·Formation·TurnOrderQueue는 수정하지 않았다. 메뉴 조작·HUD 배치·2회 소모·3턴 쿨타임은 Play Mode 확인이 필요하다.

- 3대3 Encounter와 공용 대상 선택 변경을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. `BattleCore`·기존 스킬 런타임·ActionPresenter·BattleVisualResolver는 변경하지 않았으며 실제 3대3 UI·입력·도발 분산은 Play Mode 확인이 필요하다.
- 전투 상태 UI와 ViewModel을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 전투 계산·N대N·대상 판정·연출 코드는 변경하지 않았으며 16:9 팝업 위치와 마우스/키보드 동작은 Play Mode 확인이 필요하다.
- 고정 HP 목록 변경을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 별도 출력 컴파일해 오류 0개를 확인했다. `BattleCore`·Formation·TargetResolver·TurnOrderQueue·도발·피해 계산과 하단 명령 패널은 변경하지 않았으며, 좌우 목록 배치·실제 HP 비율·포커스 행 연동은 Play Mode 확인이 필요하다.
- 상단 HP HUD 재배치를 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 별도 출력 컴파일해 오류 0개를 확인했다. HP HUD·전장·상태 Anchor·팝업 안전 영역만 변경했으며 N대N·Formation·Combatant·대상/턴/도발/피해 계산·명령·연출 코드는 변경하지 않았다.
- 대상 화살표 정렬·HP HUD 상태 요약·명령 버튼 축소·공용 취소 흐름을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 별도 출력 컴파일해 오류 0개를 확인했다. `BattleSceneController`의 UI와 입력 단계만 변경했으며 전투 계산·N대N·도발/방어 판정·행동 실행은 변경하지 않았다.
- 전투불능 상태 정리·회색 발판 숨김·단색 버튼/상태 아이콘·죽은 대상 선택 정리를 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 별도 출력 컴파일해 오류 0개를 확인했다. ViewModel과 UI 갱신만 변경했으며 Combatant·Formation·TargetResolver·TurnOrderQueue와 피해/도발/방어 계산은 변경하지 않았다.
- Kenney 실제 Sprite 카탈로그·명령 버튼·HP HUD 상태 배지 변경을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 별도 출력 컴파일해 오류 0개를 확인했다. 기존 deprecated API 경고 4개만 유지되며 `BattleCore`·Combatant 계산·Formation·TargetResolver·TurnOrderQueue·Projectile/VFX·취소 흐름은 변경하지 않았다. 실제 import와 화면 정렬은 Play Mode 확인이 필요하다.
- 도발 pawn_right와 재사용 3단계 모래시계 표시를 Unity 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. `BattleSkillCooldowns`의 시작·감소 계산은 변경하지 않고 ViewModel이 남은 턴과 총 턴을 UI에 전달하며, 전투불능 목록 제거와 텍스트 fallback을 유지한다.
- 치유의 빛 회복 API·아군 대상 선택·Radiant Heal Presenter 확장을 Unity 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 원본 grid sheet SHA-256 일치와 96×96 프레임 14개를 확인했으며, `TakeDamage`·방어 50%·TargetResolver·Formation·TurnOrderQueue·도발·기존 Projectile/VFX는 변경하지 않았다. 실제 회복 시점과 화면 위치는 Play Mode 확인이 필요하다.
- 사수 정조준 데이터·원거리 대상 연결·golden_arrow 도착 피해를 Unity 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 정수 퍼센트식은 Attack 12→20, 15→24, 20→32를 확인했으며 TargetResolver·Formation·BattleCore·Presenter·기본 공격과 기존 Projectile 에셋은 변경하지 않았다. 실제 후열/전열 선택과 2→1→사용 가능 흐름은 Play Mode 확인이 필요하다.
- 투사 난도의 근거리 대상 선택·150% 타격·적중 후 자원 증가·직접 사용 3회 쿨타임·HUD 표시를 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 공격력 12→18, 15→23, 20→30의 정수 올림 계산을 확인했고 기존 deprecated API 경고 4개만 있었다. Formation·TurnOrderQueue·BattleCore·Projectile/VFX와 기존 세 직업 스킬은 변경하지 않았으며 이후 사용자 Play Mode 확인을 통과했다.
- 사용자가 Unity Play Mode에서 기존 난도 150% 근거리 공격·기세 획득 전 동작이 정상임을 확인했다.
- 기세 용어·API 리네임과 회심의 일격 데이터·근거리 타격·기세 소비·skull UI를 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 공격력 10 기준 기세 0/1/2/3 피해 10/13/16/19와 원본 ZIP·런타임 skull PNG SHA-256 일치를 확인했고 기존 deprecated API 경고 4개만 있었다. 실제 연출·HUD·쿨타임 독립은 Play Mode 확인이 필요하다.
- 회오리 베기 데이터·광역 실행·동시 피격 Presenter·spinner 아이콘 연결을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 기존 deprecated API 경고 4개만 있었고 Formation·TargetResolver·TurnOrderQueue·BattleCore·기존 Projectile/VFX는 변경하지 않았다. 실제 1~3명 대상 피해·기세와 연출은 Play Mode 확인이 필요하다.
- 회오리 베기 대상을 `EnemyFrontRowAll` 데이터와 공용 스킬 대상 해석기로 전열에 제한한 뒤 Unity 전체 `Assembly-CSharp` 참조 컴파일 오류 0개를 확인했다. Formation·TargetResolver·기존 VFX는 변경하지 않았으며, 후열 제외·전열 0명 행동 미소비는 Play Mode 확인이 필요하다.
- 화살비 `EnemyRearRowAll` 대상·120% 광역 실행·다중 golden_arrow Presenter·bow 아이콘을 Unity 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. Kenney 원본과 bow 복사본 해시 및 golden_arrow 8프레임 유지를 확인했고, 기존 deprecated API 경고 4개만 있었다. 실제 타이밍·후열 0명·대상당 단일 피해는 Play Mode 확인이 필요하다.
- 화살비 다중 후열 실검증용 프로토타입 적 배치를 전열 1명+후열 2명으로 바꾼 뒤 Unity 전체 `Assembly-CSharp` 참조 컴파일 오류 0개를 확인했다. 전투 계산·범위·연출 코드는 변경하지 않았으며 실제 동시 타격과 후열 전투불능 제외는 Play Mode 확인이 필요하다.
- 사용자가 Unity Play Mode에서 화살비가 후열 여러 명을 정상 공격하는 것을 확인했다.
- 화살비 성공 완료 후 2턴 쿨타임 등록과 설명 데이터를 Unity 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 120%·EnemyRearRowAll·golden_arrow 연출은 변경하지 않았고 기존 deprecated API 경고 4개만 있었다. 2→1→사용 가능과 정조준 독립 표시는 Play Mode 확인이 필요하다.
- **동료의 습격 구현**: `BeastCompanionDefinition`·카탈로그, 자유 단일 대상+도발 우선, 180% `TakeDamage`, 3턴 쿨타임과 Wolf Run+이동+도착 타격 구조를 Unity 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 기존 deprecated API 경고 4개만 있으며 실제 화면 위치·속도·입력 잠금·쿨타임 흐름은 Play Mode 확인이 필요하다.
- **동료의 습격 Wolf 연출 조정**: 선명한 Wolf Run index 4 실물 프레임 아이콘, 사수 근처 출발, 최초 이동 속도의 3/4, 대상 바로 앞 정지·도착 타격·0.12초 여운 뒤 제거를 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 원본 SpriteSheet는 수정하지 않고 야수 정의가 경로·아이콘 프레임을 제공하며, 대상·180% 피해·도발·3턴 쿨타임 코드는 변경하지 않았다. 실제 아이콘 가독성·간격·체감 속도는 Play Mode 확인이 필요하다.
- **동료의 습격 Run 프레임 수정**: 원본 Wolf Run의 64×40 프레임 6개가 모두 서로 다른 이미지임을 확인하고, 전투 `Image.sprite`를 0→1→2→3→4→5→0 순서로 12 FPS 교체하는 Coroutine을 570 UI 단위/초 Transform 이동 Coroutine과 분리했다. 출발·도착점은 매 사용 시 현재 사수·대상 `ActionRoot` 위치와 표시 크기로 계산하며, 아이콘용 index 4 고정 Sprite와 전투용 6프레임 배열은 분리했다. 전체 `Assembly-CSharp` 참조 컴파일 오류 0개(기존 deprecated API 경고 4개)를 확인했으며 실제 다리 움직임과 접촉 위치는 Play Mode 재확인이 필요하다.
- **마도사 파이어 볼 구현**: Magic 자유 단일 대상·도발 우선, 170% 즉발 피해, 명중 당시 Attack 30%를 저장한 행동 종료 화상 2회와 비중첩 2회 갱신, 마도사 행동 기준 3턴 쿨타임을 공용 스킬·상태·쿨타임 구조에 연결했다. PVFX Charge·Warm Explosion과 큰 기존 fireball, 작은 화상 틱을 새 시각 정의 경계로 연결했으며 전체 `Assembly-CSharp` 참조 컴파일 오류 0개(기존 deprecated API 경고 4개)를 확인했다. 실제 크기·타격 프레임·행동 종료 틱은 Play Mode 확인이 필요하다.
- **마도사 썬더볼트 구현**: EnemyAll 생존 적 전체 90% 동시 피해, 대상별 감전 1의 15% 주는 피해 감소·행동 종료 제거·비중첩 갱신, 마도사 행동 기준 3턴 쿨타임을 기존 런타임에 연결했다. electric-impact 14프레임과 peak index 1 동시 타격, VFX/상태 고정 아이콘 분리를 적용했고 전체 `Assembly-CSharp` 참조 컴파일 오류 0개(기존 deprecated API 경고 4개)를 확인했다. 실제 다중 대상 크기·동시성·상태 수명은 Play Mode 확인이 필요하다.
- **마도사 가이아 웰 방어 규칙 수정**: 자신 전용 받는 피해 감소를 60%로 상향하고 공용 방어 50%와 중첩되지 않게 입력·피해 계산 양쪽에서 차단했다. 활성 중 방어 버튼은 사용 불가 색상으로 보이지만 클릭·키보드 시 지정 안내를 제공하고 행동을 소비하지 않는다. 전체 `Assembly-CSharp` 참조 컴파일 오류 0개(기존 deprecated API 경고 4개)를 확인했으며 실제 버튼 상태·피해 10→4는 Play Mode 확인이 필요하다.
- **수호자 철벽 구현**: 참가자별 철벽 상태·70% 받는 피해 감소·자신의 다음 2회 행동 지속·4턴 쿨타임과 공용 방어 중첩 차단을 기존 상태·쿨타임 구조에 연결했다. Earth Rupture 20프레임과 `structure_wall.png` 연결 후 전체 `Assembly-CSharp` 참조 컴파일 오류 0개(기존 deprecated API 경고 4개)를 확인했으며 실제 크기·피해 10→3·HUD·입력 흐름은 Play Mode 확인이 필요하다.
- **수호자 수호의 맹세 구현**: 직접 행동 피해 분류, 수호자별 최대 HP 40% 이전 예정 예산, 예산 비례 감소, 철벽 적용 후 실제 이전 피해, 다음 수호자 행동 시작·전투불능·예산 소진 종료를 공용 상태 경계에 연결했다. Frost Nova 중간 7프레임·금색 이전선과 실제 `pawns.png`, 대상별 감소량 피드백을 연결했으며 실제 VFX 범위·홀수 피해·예산 소진·복합 방어는 Play Mode 확인이 필요하다.
- **공통 스킬 설명 팝업 흐름 조정**: 스킬 확정 시 정보 UI를 닫는 공통 경계와 대상 선택·행동 완료 안전 숨김을 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 스킬별 설명 내용·효과·대상·쿨타임은 변경하지 않았으며 실제 Hover/키보드 포커스와 취소 복귀 흐름은 Play Mode 확인이 필요하다.
- **전투 연출 중 정보 팝업 억제**: `actionPlaying` 화면 갱신에서 스킬 설명·캐릭터 상태 상세 팝업과 기존 Hover/포커스 대상을 함께 정리하고 두 표시 함수의 재오픈을 차단하는 코드를 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. Wolf 570·Run 12 FPS·출발/정지 위치와 모든 전투 계산은 변경하지 않았으며 실제 마우스 잔류·새 Hover 복귀는 Play Mode 확인이 필요하다.
- **치유사 회복의 파동 구현**: 단일힐 35%에서 파생한 대상별 21% 올림 광역 회복, Formation 생존 아군 동적 목록, 치유사 행동 기준 3턴 쿨타임과 Arcane Parry+Radiant Heal 병렬 연출을 연결했다. 전체 `Assembly-CSharp` 참조 컴파일 오류 0개(기존 deprecated API 경고 4개)를 확인했으며 대상 수·동시 HP 갱신·팝업 억제·쿨타임은 Play Mode 확인이 필요하다.
- **치유사 정화 구현**: 기존 독·화상 Dictionary와 감전 HashSet은 유지하고 `HarmfulStatusType`·공통 조회/단일 제거/전체 제거 API를 추가했다. 살아 있는 아군 1명의 세 해로운 상태를 모두 제거하며 상태가 없으면 행동·턴·쿨타임을 소비하지 않는다. spectral-bloom 16프레임·release index 7 제거·peak index 5 아이콘과 치유사 행동 기준 2턴 쿨타임을 연결했다. Unity 6000.5.7f1 자동 컴파일 오류 0개를 확인했으며 실제 대상 선택·복합 상태 제거·VFX·팝업 억제·쿨타임은 Play Mode 확인이 필요하다.
- **독침벌 독 부여 대기시간 구현**: `BattleMonsterAbilityRuntime`이 공격자 Combatant별 독침 대기 상태를 독 상태 저장소와 분리해 관리한다. 첫 공격은 120% 피해+독 3 뒤 대기 2, 다음 두 벌 행동은 120% 피해만 주고 행동 종료마다 1·0으로 감소하며 네 번째 행동부터 다시 독을 부여한다. 정화·독 자연 종료·다른 참가자 행동은 대기시간에 영향을 주지 않는다. Unity 6000.5.7f1 자동 컴파일 오류 0개를 확인했으며 4행동 순서와 복수 벌 독립성은 Play Mode 확인이 필요하다.
- **독 몬스터 후보 에셋 보존 및 Pilot Bee 검증 준비**: F:\Downloads 원본과 프로젝트 복사본 SHA-256 일치를 확인하고 벌·거미·뱀을 제작자별 ThirdParty 폴더에 분리했다. Pilot Bee 검증 스크립트를 포함한 전체 `Assembly-CSharp` 참조 컴파일 오류 0개(기존 deprecated API 경고 4개), Point·무압축·투명 Import 설정을 정적으로 확인했다. Unity 프로젝트가 이미 열려 있어 두 번째 배치 인스턴스는 안전하게 중단했으며 실제 화면의 Scale·픽셀 밀도·Idle/Attack 자연스러움은 전용 Scene Play Mode 확인이 필요하다.
- **동료의 습격 Run 검증 환경 유지**: ScratchIO `Animated Wild Animals` CC0 원본 ZIP과 Wolf/Bear/Fox 자산을 보존하고, 실제 Battle과 분리된 `CompanionAssaultRunValidation` Scene을 유지한다. 원본/복사본 SHA-256, 64px 프레임 구조, Point·무압축·투명 Import와 통일된 아래 중앙 Pivot은 정적으로 확인했으며 Unity Play Mode 직접 확인은 남아 있다.
- Working Tree에는 이번 문서 작업과 무관한 사용자 Asset·Scene·ProjectSettings 변경이 남아 있으며 이 상태 문서는 해당 미커밋 변경의 완성 여부를 판단하지 않는다.

## Unity에서 사용자가 직접 확인할 사항

1. Play Mode를 종료해 Asset 재import를 완료한 뒤 다시 시작하고, Console에 `필드 몬스터 배치 로드: Field_01, 5개 [grass_slime_01, ..., grass_slime_05]`가 출력되는지 확인한다.
2. HP가 100%에서 50%가 되었을 때 숫자와 함께 HP Bar 길이도 정확히 절반이 되는지 확인한다.
3. Player와 초원 슬라임 양쪽 HP Bar가 피해 직후 정상 감소하고, HP 0에서 완전히 비는지 확인한다.
4. 슬라임 처치 후 `Field_01` 복귀 시 조우했던 해당 슬라임만 사라져 있는지 확인한다.
5. 처치하지 않은 다른 슬라임들은 그대로 존재하며 계속 배회하는지 확인한다.
6. 약 30초 후 처치한 스폰의 시작 위치에서 슬라임이 다시 나타나는지 확인한다.
7. 도망한 경우 조우했던 슬라임이 사라지지 않고, 기존 2초 재조우 유예 뒤 정상 배회하는지 확인한다.
8. `Field_01`에 총 5마리의 초원 슬라임이 서로 다른 활동 반경 안에서 독립적으로 배회하는지 확인한다.
9. Console에 Compiler Error, MissingReferenceException, NullReferenceException이 없는지 확인한다.
10. 근거리 직업으로 플레이어 기본 공격 시 대상 앞까지 짧게 전진하고, 타격 후 원래 자리로 복귀하는지 확인한다.
11. 초원 슬라임 공격에도 같은 전진·복귀가 적용되고, 양측 피격 시 좌우 흔들림·짧은 점멸과 `-피해량` 숫자가 표시되는지 확인한다.
12. 연출 중 공격·스킬·방어·도망·대상 선택·Esc 취소가 중복 실행되지 않고, 연출 종료 뒤 다음 턴 입력이 정상 복구되는지 확인한다.
13. 공격 10과 방어 중 피해 5, HP 숫자·Bar, 행동 순서 타임라인, 승리·도망·패배 및 Field_01 복귀·리스폰이 기존과 동일한지 확인한다.
14. 사수 직업으로 기본 공격 시 제자리에서 짧게 조준하고 golden arrow 애니메이션이 약 0.35초 동안 왼쪽 적을 향해 이동하는지 확인한다.
15. Projectile 도착 순간에만 HP와 피해 숫자가 갱신되고 기존 흔들림·점멸 뒤 다음 턴으로 정상 진행되는지 확인한다.
16. Projectile 이동과 피격 연출 중 명령·대상 선택·Esc가 중복 실행되지 않는지 확인한다.
17. 필드에서 플레이어가 걷거나 상·하·우 방향을 보는 중 조우해도 Battle에서는 기본·Path Variant·휠체어 모두 Left Idle 첫 프레임인지 확인한다.
18. 초원 슬라임이 필드에서 이동하거나 다른 방향을 보는 중 조우해도 Battle에서는 Right Idle 첫 프레임인지 확인한다.
19. 근거리·Projectile 공격 종료 후 양측이 각자의 Left/Right 전투 Idle Sprite로 복귀하는지 확인한다.
20. 남자 지체의 길 수호자와 초원 슬라임이 흰 사각형 없이 표시되고, `Battle Visual` Warning·NullReference·Compile Error가 없는지 확인한다.
21. 마도사 기본 공격 시 제자리 캐스팅 후 fireball 애니메이션이 약 0.35초 동안 왼쪽의 전열 또는 후열 대상까지 이동하고, 도착 순간에만 HP·피해 숫자가 갱신되는지 확인한다.
22. 치유사 기본 공격 시 PVFX Magical Projectile이 0.24초 동안 왼쪽 대상을 향해 재생되고, 도착 순간 피해와 기존 낮은 피해량·마법 자유 대상 규칙이 유지되는지 확인한다.
23. 두 마법 연출 중 입력이 잠기고 완료 후 Left Idle과 다음 턴이 복구되며, 치유사 Magical Projectile과 근거리 연출도 정상인지 확인한다.
24. 테스트용으로 시작 X보다 목표 X가 큰 배치를 구성할 수 있을 때 golden arrow와 fireball이 원본 오른쪽 방향으로 표시되는지 확인한다.
25. 수호자로 Battle에 진입해 스킬 → 도발을 마우스와 키보드로 선택하고, Esc로 명령 메뉴에 복귀되는지 확인한다.
26. 도발 사용 시 수호자가 제자리에서 강조되고 도발! 텍스트 뒤 슬라임 HUD에 도발 2가 표시되는지 확인한다.
27. 슬라임 첫 행동 완료 후 도발 1, 두 번째 행동 완료 후 표시 제거를 확인한다.
28. 수호자 다음 행동 차례의 메뉴에 도발 [재사용 2턴], 이후 1턴, 종료 후 다시 사용 가능 상태가 표시되는지 확인한다.
29. 쿨타임 중 도발 버튼이 실행되지 않으며 철벽·수호의 맹세와 다른 직업 스킬 버튼의 구현 상태·재사용 표시가 실제 규칙과 일치하는지 확인한다.
30. 공격·방어·도망과 기존 근거리/Projectile 연출, HP Bar, 승리·패배·Field_01 복귀·30초 리스폰을 회귀 확인한다.
31. Console에 Compile Error, NullReferenceException, MissingReferenceException이 없는지 확인한다.

32. Battle 진입 시 플레이어·태온·미엘과 초원 슬라임 A·B·C가 3대3으로 겹치지 않고 표시되며 각 HUD에 이름·직업·HP가 보이는지 확인한다.
33. 타임라인에 여섯 참가자의 민첩 기반 순서가 표시되고 플레이어·태온·미엘 차례마다 시간제한 없이 직접 명령할 수 있는지 확인한다.
34. 전열 슬라임 A·B와 후열 슬라임 C에 근거리·원거리·마법 기본 공격 대상 규칙이 기존대로 적용되는지 확인한다.
35. 슬라임 하나의 HP가 0이 되어도 다른 두 슬라임의 HP·행동이 유지되고, 세 마리 전멸 때만 승리하는지 확인한다.
36. 태온 또는 플레이어 수호자의 도발 후 세 슬라임 HUD가 모두 도발 2가 되고, 각 슬라임 행동 때 자기 표시만 1로 감소한 뒤 두 번째 행동 후 사라지는지 확인한다.
37. 도발 중 세 슬라임의 단일 공격이 시전자 수호자에게 집중되고, 시전자 행동 기준 재사용 2턴→1턴→사용 가능인지 확인한다.
38. 아군 한 명 전투불능 시 그 참가자만 행동에서 제외되고 나머지 전투가 계속되며, 아군 세 명 전멸 때만 패배하는지 확인한다.
39. Console에 Compile Error, NullReferenceException, MissingReferenceException이 없는지 확인한다.

40. 16:9 Battle이 제목 → 타임라인 → 상단 HP HUD → 실제 전장 → 하단 명령 패널 순서로 표시되고 각 영역이 겹치거나 잘리지 않는지 확인한다.
41. 현재 3대3에서는 상단 HUD의 적군 첫 줄과 아군 한 줄에 각각 3개 HP 항목만 표시되고, 캐릭터 주변 이름·HP Bar가 없는지 확인한다.
42. 참가자 수를 줄인 Encounter에서는 빈 HP 항목이 생기지 않고, 적 4~6명 구성에서는 적군이 한 줄 최대 3명·최대 2줄로 배치되는지 확인한다.
43. 피해 직후 해당 HP 항목의 Bar 길이가 현재/최대 HP 실제 비율만큼 줄고, HP 0에서 완전히 비며 이름에 `[전투불능]`이 표시되는지 확인한다.
44. 대상 선택을 방향키로 전환할 때 역삼각형이 적/아군 전열·후열 각 Sprite의 가로 중앙 위로 정확히 이동하는지 확인한다.
45. 캐릭터 주변에는 대상 화살표와 `행동 중`만 보이고, 상단 HP 항목에 실제 Sprite와 함께 `행동 중`, `방어`, `도발 2/1`, 구현 스킬 `재사용 n턴`이 한 줄로 표시되는지 확인한다.
46. 참가자가 전투불능이 되는 즉시 화살표·행동 중·도발·방어·재사용 표시가 사라지고 HP HUD에는 회색 `전투불능`만 남는지 확인한다.
47. 전투불능 참가자 상세 팝업에는 이름·분류·HP 0/최대 HP만 표시되고 방어·도발·쿨타임이 남지 않는지 확인한다.
48. 평상시 모든 캐릭터 아래 회색 발판이 보이지 않고, 대상 선택 중 유효 대상에게만 얇은 금색 선이 표시되는지 확인한다.
49. 공격=sword, 스킬=star, 방어=shield, 도망=exitRight, 취소=cross Sprite와 한글이 함께 보이며 비율·정렬이 깨지거나 버튼 크기가 커지지 않았는지 확인한다.
50. 대상 선택 중 포커스된 참가자가 전투불능이 되면 강조가 제거되고 다음 유효 대상으로 이동하거나 후보가 없을 때 기본 명령으로 복귀하는지 확인한다.
51. 대상/스킬 선택의 취소 버튼·Esc와 공격·방어·도발 수치, Projectile·승패·필드 복귀가 기존과 같고 Console 오류가 없는지 확인한다.
52. 행동 중 arrowRight, 방어 shield와 재사용 단계별 hourglass가 상태와 함께 갱신되는지 확인한다.
53. 전투불능 즉시 모든 상태 Sprite와 글자가 사라지고 회색 `전투불능`만 남으며 Console에 MissingReference·NullReference가 없는지 확인한다.
54. 도발 상태에 pawn_right가 표시되고 기존 target은 나오지 않으며, 도발 2→1→제거가 유지되는지 확인한다.
55. 도발 사용 직후 재사용 3턴=hourglass_top, 다음 자기 차례 2턴=hourglass, 마지막 1턴=hourglass_bottom, 0턴=아이콘·텍스트 제거인지 확인한다.
56. 미엘 또는 플레이어 치유사 차례에 스킬→치유의 빛이 활성화되고 자신·플레이어·태온·미엘 중 살아 있는 아군만 선택되는지 확인한다.
57. 피해를 받은 아군에게 사용하면 최대 HP의 35% 올림 값만큼 회복하되 최대 HP를 넘지 않고, peak 순간 HP Bar·상세 HP와 `+회복량`이 함께 갱신되는지 확인한다.
58. 최대 HP 아군 선택 시 `이미 HP가 가득 찼습니다` 안내 후 스킬 메뉴로 돌아가며 행동이 소비되지 않는지 확인한다.
59. 전투불능 아군과 적은 대상이 아니며, 대상 선택 중 Esc/취소는 스킬 메뉴로, 스킬 메뉴 Esc/취소는 기본 명령으로 돌아가는지 확인한다.
60. Radiant Heal이 대상 위치에서 14프레임으로 재생되고 peak 뒤 연출 종료 시 다음 턴으로 진행하며, 연출 중 중복 입력과 Console 오류가 없는지 확인한다.
61. 플레이어 사수 차례에 스킬→정조준이 활성화되고 `강한 원거리 · 160% · 2턴` 안내가 보이는지 확인한다.
62. 후열 슬라임이 살아 있으면 후열만 선택되고, 후열 전멸 뒤 전열만 선택되며 전투불능 적은 후보에서 빠지는지 확인한다.
63. 정조준 선택 후 즉시 HP가 줄지 않고 `정조준!`→golden_arrow 0.42초 이동→도착 순간에만 피해·HP Bar·상세 HP·피해 숫자·피격 연출이 갱신되는지 확인한다.
64. 기본 공격력 12 기준 정조준 raw 피해가 20이며 방어 중 대상에는 기존 50% 감소가 적용되고 방어 무시·치명타·상태이상이 없는지 확인한다.
65. 사용 직후 HUD에 재사용 2턴/hourglass_top, 다음 사수 행동에 1턴/hourglass_bottom, 그다음 사용 가능 및 표시 제거인지 확인한다.
66. 정조준 대상 선택 중 Esc/취소는 스킬 메뉴, 스킬 메뉴 Esc/취소는 기본 명령으로 돌아가며 연출 중 입력이 잠기는지 확인한다.
67. 수호자·치유사·사수 스킬 메뉴에서 각각 pawn_left·suit_hearts·target 아이콘과 한글 스킬명이 함께 보이고, 적 도발 HUD에는 기존 pawn_right가 유지되는지 확인한다.
68. 스킬 버튼 Hover와 방향키 포커스에서 하나의 상세 팝업이 즉시 갱신되고, 설명·대상·효과 수치·재사용·미구현 여부가 현재 규칙과 일치하는지 확인한다.
69. 스킬 메뉴를 닫거나 대상 선택으로 이동하면 팝업이 사라지고, 16:9에서 HP HUD·하단 명령 패널과 겹치거나 화면 밖으로 잘리지 않는지 확인한다.
70. 취소 버튼에 arrowLeft와 `취소`가 함께 표시되고 cross가 나오지 않으며, 취소 버튼과 Esc의 단계별 복귀가 기존과 같은지 확인한다.
71. 투사 스킬 메뉴에 cross 아이콘+난도, skull 아이콘+회심의 일격이 함께 표시되는지 확인한다.
72. 난도 1회 적중 시 기존 150% 피해와 skull 아이콘+`기세 1`이 표시되는지 확인한다.
73. 기세 1에서 회심의 일격이 130% 피해를 주고 타격 후 기세 0이 되는지 확인한다.
74. 난도 2회 후 기세 2가 되고 회심의 일격이 160% 피해를 준 뒤 기세를 0으로 소비하는지 확인한다.
75. 난도를 직접 세 번째 사용할 때 기세 3과 난도 재사용 2턴이 표시되는지 확인한다.
76. 기세 3 회심의 일격이 190% 피해를 주고 기세만 0으로 만들며 난도의 남은 쿨타임은 유지하는지 확인한다.
77. 기세 0에서도 회심의 일격을 사용할 수 있고 일반 공격 100% 피해를 주는지 확인한다.
78. 상단 HUD는 기세 1/2/3만 표시하고 0에서 숨기며, 상세 팝업은 기세 0/3~3/3을 계속 표시하는지 확인한다.
79. 회심의 일격 Hover·키보드 포커스 설명에 100/130/160/190%, 전부 소비, 재사용 없음과 현재 기세·예상 배율이 표시되는지 확인한다.
80. 난도 설명이 적중 후 기세 +1·기세 최대 3·세 번째 직접 사용 후 2턴으로 통일됐는지 확인한다.
81. 회심의 일격 대상 선택 중 Esc/취소가 스킬 메뉴로 복귀하고 선택만으로 피해나 기세 소비가 발생하지 않는지 확인한다.
82. 방어 중 적에게 회심의 일격을 사용하면 기존 50% 피해 감소가 적용되는지 확인한다.
83. 투사 전투불능 시 기세·쿨타임 표시가 숨겨지고, 기존 도발·치유의 빛·정조준·기본 공격·승패·도망·Field 복귀와 Console이 정상인지 확인한다.
84. 투사 스킬 메뉴에 spinner 아이콘+회오리 베기가 표시되고 Hover·키보드 포커스 설명에 적 전열 전체·80%·적중당 기세 +1·최대 3·재사용 없음이 보이는지 확인한다.
85. 살아 있는 전열 적이 1/2/3명일 때 회오리 베기가 전열에만 각각 80% 정수 올림 피해를 동시에 적용하고 기세가 각각 +1/+2/+3 되는지 확인한다.
86. 기세 2에서 적 2명 이상을 맞혀도 기세가 3을 넘지 않고, 기세 3에서도 회오리 베기를 계속 사용해 피해는 주되 기세는 3으로 유지되는지 확인한다.
87. 회오리 베기로 기세 3을 만든 뒤 난도에 2턴 쿨타임이 생기지 않고, 회심의 일격이 190% 피해 후 기세를 0으로 소비하는지 확인한다.
88. `회오리 베기!` 뒤 투사 중심 회전 참격→모든 대상 피해 숫자·피격 반응→기세 HUD 갱신→다음 턴 순서와 연출 중 입력 잠금이 정상인지 확인한다.
89. 전열 2명·후열 1명에서 전열만 피해를 받고 기세 +2인지, 전열 1명·후열 2명에서는 기세 +1인지 확인한다.
90. 전열이 전멸하고 후열만 살아 있을 때 `회오리 베기로 공격할 전열 적이 없습니다.` 안내 후 행동과 턴이 소비되지 않는지 확인한다.
91. 사수 스킬 메뉴에 target+정조준과 bow+화살비가 함께 표시되고, 화살비 설명에 원거리 물리·적 후열 전체·120%·재사용 없음이 보이는지 확인한다.
92. 후열 생존자가 1/2/3명일 때 후열만 각각 120% 피해를 거의 동시에 한 번씩 받고 전열 HP는 변하지 않는지 확인한다.
93. 대상마다 golden_arrow 3발이 떨어져도 피해 숫자와 실제 HP 감소는 대상당 한 번이며, 방어 중 후열은 기존 50% 감소가 적용되는지 확인한다.
94. 후열이 전멸하고 전열만 남았을 때 `화살비로 공격할 후열 적이 없습니다.` 안내 후 Projectile·행동·턴이 시작되지 않는지 확인한다.
95. 도발 상태와 관계없이 후열 전체가 유지되고, `화살비!`→상승→공중 대기→다중 낙하→동시 피격의 약 0.75초 순서와 연출 중 입력 잠금·완료 후 단일 턴 진행이 정상인지 확인한다.
96. 화살비 설명에 재사용 2턴이 보이고, 성공 연출 완료 후 화살비 재사용 2턴/hourglass_top이 표시되는지 확인한다.
97. 다른 아군·적 행동에는 2턴이 유지되고 다음 사수 행동에 1턴/hourglass_bottom, 그다음 사수 행동에 0으로 사라져 다시 사용 가능한지 확인한다.
98. 화살비 쿨타임 중 정조준이 사용 가능하면 정상 사용되고, 정조준 사용·쿨타임이 화살비 남은 턴을 변경하지 않는지 확인한다.
99. 후열 0명에서 화살비 거절 후 쿨타임 표시가 생기지 않고 행동이 그대로 유지되는지 확인한다.
100. `Assets/_Project/Scenes/Validation/CompanionAssaultRunValidation.unity`를 열고 Play 시 Wolf/Fox/Bear가 같은 속도로 오른쪽→왼쪽을 반복하며 픽셀 흐림이나 발 기준점의 불필요한 흔들림 없이 보이는지 확인한다.
101. `Space` 일시정지, `+/-` Run FPS 조절, `F` 좌우 Flip이 동작하고 Wolf/Fox/Bear의 속도감·타격감·현재 Battle Sprite 표시 크기와의 조화를 직접 비교한다.
102. 검증 Scene 실행 뒤 기존 `Battle.unity`의 사수 기본 공격·정조준·화살비와 다른 직업 스킬, Formation·대상·턴·승패/도망/Field 복귀에 회귀가 없는지 확인한다.
103. 사수 스킬 메뉴에서 Wolf 실제 Sprite 아이콘+동료의 습격이 표시되고, 정조준은 기존 `target.png`를 유지하며 설명의 야수/단일 물리·적 1명·전후열 자유·180%·3턴이 보이는지 확인한다.
104. 전열과 후열 생존 적을 모두 선택할 수 있고, 사수에게 도발 강제 대상이 있으면 그 적만 선택되는지 확인한다.
105. 사수는 제자리에 있고 Wolf가 현재 행동 중인 사수의 실제 `ActionRoot` 위치 근처에서 나타나는지 확인한다. 전열/후열 등 사수 표시 위치를 바꿔도 고정 화면 좌표가 아니라 새 사수 위치를 따라 출발하며, 6개의 서로 다른 다리 자세가 0→1→2→3→4→5→0 순서로 12 FPS 반복되는 동안 Transform은 독립적으로 570 UI 단위/초로 대상 바로 앞까지 이동해야 한다.
106. Wolf가 대상을 관통하지 않고 바로 앞에 정지하며, 도착 전 HP가 줄지 않고 도착 순간 한 번만 180% 피해·피해 숫자·피격 반응이 발생하고 방어 중 대상에는 기존 50% 감소가 적용되는지 확인한다.
107. Wolf가 타격 위치에서 짧게 멈춘 뒤 제거되고 피격·제거가 모두 끝날 때까지 입력이 잠긴 뒤 다음 턴이 한 번만 진행되는지 확인한다.
108. 사용 직후 재사용 3턴/hourglass_top, 다음 사수 행동마다 2/hourglass→1/hourglass_bottom→0/사용 가능 순서이며 정조준·화살비 쿨타임과 독립인지 확인한다.
109. 대상 선택 중 Esc/취소가 스킬 메뉴로 복귀하고, 기존 도발·치유의 빛·투사 스킬·Formation·턴 순서·HP HUD·승리/도망/Field 복귀에 회귀와 Console 오류가 없는지 확인한다.
110. 모든 스킬 메뉴에서 Hover/키보드 포커스 중 설명이 보이고, 스킬 선택 즉시 대상 선택 또는 연출 전에 사라지며 연출 종료·기본 명령 복귀 뒤에도 숨겨졌다가 다음 스킬 메뉴의 새 Hover/포커스에서만 다시 표시되는지 확인한다.
111. 캐릭터/상태 상세 팝업을 연 채 기본 공격·도발·치유의 빛·정조준·화살비·동료의 습격·난도·회심의 일격·회오리 베기를 시작하면 즉시 닫히고, 마우스를 기존 HUD 위에 계속 둬도 연출 중 재오픈되지 않으며 연출 종료 뒤 새 Hover/포커스에서 다시 열리는지 확인한다.
112. 마도사 스킬 메뉴의 파이어 볼에 Warm Explosion peak 아이콘과 단일 마법·전후열 자유·170%·화상 30%×2·재사용 3턴 설명이 표시되는지 확인한다.
113. 전열과 후열 적을 자유롭게 선택할 수 있고 마도사에게 도발 강제 대상이 있으면 그 한 명만 선택되는지 확인한다. 대상 선택 Esc/취소는 스킬 메뉴로 돌아가며 행동·쿨타임을 소비하지 않아야 한다.
114. `파이어 볼!`→Solar Shrapnel 초기 Charge→80×80 fireball 이동→192×192 Warm Explosion 순서이며, 폭발 index 4 전에는 HP가 줄지 않고 index 4에서 한 번만 170% 피해·화상 2·피격 반응이 발생하는지 확인한다.
115. 화상 대상이 행동을 마칠 때마다 46×46 작은 불꽃과 30% 저장 피해가 한 번 발생해 `화상 2→1→제거`되는지 확인한다. 큰 Warm Explosion이나 새 Projectile은 틱 때 나오지 않아야 한다.
116. 화상 1회가 남았을 때 다시 맞으면 합산되지 않고 2회로 갱신되며, 명중 당시 마도사 Attack 기준의 새 틱 피해로 교체되는지 확인한다. 화상 종료 후 재적중하면 새 화상 2가 생겨야 한다.
117. 사용 직후 파이어 볼 재사용 3턴/hourglass_top, 다음 마도사 행동마다 2/hourglass→1/hourglass_bottom→0/사용 가능 순서이고 다른 참가자 행동에는 감소하지 않는지 확인한다.
118. 화상 틱과 VFX 중 입력·두 정보 팝업이 억제되고 피격 완료 뒤 다음 턴이 한 번만 진행되는지, 화상으로 전투불능/승리가 발생해도 턴·승리·Field 복귀가 정상인지 확인한다.
119. 마도사 스킬 메뉴에서 썬더볼트에 electric-impact peak index 1 아이콘과 `대상: 적 전체`, 90%, 감전 1, 다음 행동 주는 피해 15% 감소, 재사용 3턴 설명이 표시되는지 확인한다.
120. 전열·후열에 적을 나누어 배치하고 전투불능 적을 섞었을 때 살아 있는 적 전체에만 짧은 청백색 예고와 144×144 electric-impact가 거의 동시에 나타나는지 확인한다.
121. electric-impact index 1 전에는 HP가 줄지 않고 index 1에서 모든 대상이 각각 일반 공격 90% 피해와 감전 1을 동시에 받으며, 방어 중 대상에는 기존 50% 받는 피해 감소가 유지되는지 확인한다.
122. 적에게 도발 상태가 있어도 썬더볼트는 한 명으로 좁혀지지 않고 EnemyAll 전체를 유지하며, 전열 전용 회오리 베기와 후열 전용 화살비 범위는 그대로인지 확인한다.
123. 감전된 서로 다른 적의 HUD와 상세 팝업에 각각 Kenney power 아이콘+`감전 1`이 보이고, 한 대상이 다시 맞아도 감전 2가 아니라 1로 갱신되는지 확인한다.
124. 감전된 대상의 다음 기본 공격·단일/광역 스킬 피해가 기존 값의 85%로 적용되고, 여러 대상을 공격해도 그 행동의 모든 피해가 감소하는지 확인한다.
125. 감전된 대상이 방어·회복·도발처럼 공격하지 않는 행동을 해도 행동 종료 후 감전이 제거되며 다른 대상의 감전은 독립적으로 남는지 확인한다.
126. 감전 대상이 전투불능이 되면 power 아이콘과 상세 상태가 즉시 사라지고, VFX·피격 완료 전 입력과 두 정보 팝업이 억제된 뒤 다음 턴이 한 번만 진행되는지 확인한다.
127. 썬더볼트 성공 뒤 재사용 3턴/hourglass_top, 다음 마도사 행동마다 2/hourglass→1/hourglass_bottom→0/사용 가능이며 파이어 볼 쿨타임과 독립인지 확인한다.
128. 기존 파이어 볼·화상·도발·치유의 빛·사수/투사 스킬·Formation·대상 선택/Esc·턴 순서·HP HUD·승리/도망/Field 복귀와 Console에 회귀가 없는지 확인한다.
129. 마도사 스킬 메뉴의 가이아 웰에 `dice_shield.png` 아이콘과 자기 보호·자신·받는 피해 60% 감소·자신의 다음 2회 행동·재사용 4턴 설명이 정확히 표시되는지 확인한다.
130. 가이아 웰 선택 시 대상 선택이나 Projectile 없이 마도사 위치에서 150×150 arcane-parry 16프레임이 20 FPS로 재생되고 peak index 8에서 HUD에 `가이아 2`가 나타나는지 확인한다.
131. 가이아 웰 사용 행동이 끝난 직후에도 `가이아 2`가 유지되고, 다른 아군·적의 여러 행동에는 감소하지 않는지 확인한다.
132. 마도사가 다음 행동으로 공격·파이어 볼·썬더볼트 등을 정상 사용한 뒤 `가이아 1`, 그다음 자기 행동 종료 뒤 상태와 아이콘이 제거되는지 확인한다.
133. 가이아 웰 활성 중 원시 피해 10이 4로 줄어드는지 확인하고, 방어 버튼이 사용 불가 색상으로 보이는지 확인한다. 클릭·키보드 Submit 시 `더 강한 방어 효과가 이미 적용 중이라 방어를 사용할 수 없습니다.` 안내 후 행동·턴이 유지되어야 한다.
133-1. 가이아 웰과 방어가 어떤 경로에서도 중첩되지 않고, 가이아 종료 후 방어 버튼 색상과 기존 10→5 피해 감소가 정상 복구되는지 확인한다.
134. 가이아 웰이 다른 아군 피해를 줄이거나 도발 대상을 바꾸지 않고, 마도사 전투불능 시 `가이아 n` HUD·상세 상태가 즉시 제거되는지 확인한다.
135. 사용 직후 재사용 4턴 표시가 생기고 다음 마도사 행동 시작마다 3→2→1→0으로 감소하며, 쿨타임 중 다시 사용할 수 없고 다른 참가자 행동에는 감소하지 않는지 확인한다.
136. 연출 중 입력과 두 정보 팝업이 억제되고 완료 후 다음 턴이 한 번만 진행되며, 기존 파이어 볼/화상·썬더볼트/감전·도발·치유·사수/투사 스킬·승리/도망/Field 복귀에 회귀가 없는지 확인한다.
137. Field_01에서 기존 초원 슬라임 5마리와 독침벌 3마리, 총 8마리가 서로 과도하게 겹치지 않고 Bounds 안에서 배회하며 독침벌 Idle 날갯짓과 Scale 0.85가 자연스러운지 확인한다.
138. 각 독침벌과 접촉해 Battle로 진입하고 전열 초원 슬라임 A/B·후열 독침벌 1 배치, 이름·HP HUD·대상 판정과 독침벌 우향 Idle 반복을 확인한다.
139. 독침벌 기본 공격에서 실제 Attack 시트가 재생되고 공격 완료 뒤 Idle로 복귀하며, 피해량과 턴 진행은 기존 적 기본 공격 규칙이고 독 상태가 생기지 않는지 확인한다.
140. 독침벌 조우 승리 시 접촉한 필드 스폰만 사라졌다 30초 뒤 자기 시작 위치에 리스폰하고, 도망 시 제거되지 않으며 복귀 직후 2초 재조우 유예가 유지되는지 확인한다.
141. 독침벌 직접 공격이 슬라임 10 대비 12 피해이며 방어 중에는 기존 방어 규칙이 유지되고, 정상 적중 대상 HUD에 Venom Ward index 6 아이콘과 `독 3`이 표시되는지 확인한다.
142. 독 대상 자신의 행동 종료에만 최대 HP 5% 올림·최소 1 피해와 작은 Acid Splash가 발생해 `독 3→2→1→제거`되고 다른 참가자의 행동에는 감소하지 않는지 확인한다.
143. 독 1/2/3에서 다시 독침벌 공격을 받으면 더해지지 않고 독 3으로 갱신되며, 완전 종료 뒤 재적중하면 새 독 3이 생기는지 확인한다.
144. 상세 팝업에 `독 n`과 `독: 행동 종료 시 최대 HP 5% 피해`가 보이고, 독 피해로 전투불능이 되면 독 아이콘·문구가 즉시 사라지는지 확인한다.
145. 화상과 독이 같은 대상에 있으면 행동 종료에 두 작은 틱 연출이 순서대로 한 번씩 발생하고 모두 끝난 뒤 다음 턴이 한 번만 진행되는지 확인한다.
146. 치유사 스킬 메뉴에서 spectral-bloom peak index 5 아이콘과 `정화` 이름이 함께 보이고 상세 팝업에 상태이상 해제·살아 있는 아군 1명·독/화상/감전 모두 제거·재사용 2턴·구현 사용 가능이 표시되는지 확인한다.
147. 정화 대상 선택에 자신을 포함한 살아 있는 아군만 포함되고 전투불능 아군은 선택할 수 없으며, Esc/취소 시 행동 없이 스킬 메뉴로 돌아가는지 확인한다.
148. 상태가 없는 살아 있는 아군을 선택하면 `정화할 해로운 상태가 없습니다.` 안내 후 VFX·행동·턴·쿨타임 없이 스킬 메뉴가 유지되는지 확인한다.
149. 독 3 대상에게 정화를 사용하면 spectral-bloom 전체 16프레임이 20 FPS로 재생되고 release index 7 전후에 독 HUD·상세 상태가 즉시 사라지며 HP는 변하지 않는지 확인한다.
150. 독+화상 및 독+화상+감전 대상에게 각각 한 번 사용해 걸린 해로운 상태가 모두 동시에 제거되는지 확인한다.
151. 정화가 대상의 방어·가이아 웰·도발 관련 상태·기세·각 스킬 쿨타임을 제거하거나 변경하지 않는지 확인한다.
152. 정화 연출 중 입력과 스킬/상태 상세 팝업이 억제되고 연출 종료 뒤 다음 턴이 한 번만 진행되는지 확인한다.
153. 성공 직후 정화 재사용 2턴이 표시되고 다른 아군·적 행동에는 유지되며, 다음 치유사 행동에 1턴, 그다음 치유사 행동에 사용 가능으로 돌아오는지 확인한다.
154. 기존 치유의 빛·회복의 파동·독 틱·화상 틱·감전 피해 감소와 모든 직업 스킬, 방어·도망·승패·Field 복귀에 회귀가 없고 Console에 Compile Error·NullReference·MissingReference가 없는지 확인한다.
155. 독침벌의 첫 행동이 12 피해와 독 3을 함께 적용하고, 두 번째·세 번째 행동은 각각 12 피해만 적용하며 독을 새로 부여하거나 갱신하지 않는지 확인한다.
156. 두 번째 독침벌 행동 종료에 내부 대기 2→1, 세 번째 행동 종료에 1→0이 되고 네 번째 행동에서 다시 12 피해+독 3을 적용하는지 확인한다.
157. 첫 독 부여 직후 치유사 정화로 독을 제거하고, 이어지는 벌의 두 공격에서는 12 피해만 받고 독이 즉시 재적용되지 않으며 네 번째 벌 행동에서만 다시 독이 생기는지 확인한다.
158. 독이 정화 없이 자연 종료되어도 해당 벌의 독침 대기는 별도로 유지되는지 확인한다.
159. 독침이 사용 가능한 상태에서 이미 독인 대상을 공격하면 독이 4 이상 중첩되지 않고 3으로 갱신되며 해당 벌의 대기가 다시 2로 시작하는지 확인한다.
160. 향후 검증용으로 독침벌을 2마리 이상 배치했을 때 한 벌의 독 부여와 행동 종료가 다른 벌의 독침 대기시간을 시작하거나 감소시키지 않는지 확인한다.
161. 매 행동 120% 직접 피해, 독 5%×3, 독 HUD 3/2/1, 정화·화상·감전·모든 직업 스킬과 승리/도망/Field 복귀가 유지되고 Console 오류가 없는지 확인한다.
162. 수호자 스킬 메뉴에서 `structure_wall.png` 아이콘과 `철벽` 이름이 함께 보이고, 상세 팝업에 자기 보호·자신·70% 감소·자신의 다음 2회 행동·재사용 4턴·공용 방어 중첩 불가가 표시되는지 확인한다.
163. 철벽 사용 시 대상 선택·Projectile 없이 수호자 발밑에서 Earth Rupture 전체 20프레임이 짧게 한 번 재생되고 peak index 9 부근에 `철벽 2` HUD가 나타나며, 연출 종료 후 암석이 캐릭터를 가리지 않는지 확인한다.
164. 철벽 사용 행동 종료 직후 `철벽 2`가 유지되고 다른 아군·적 행동에는 감소하지 않으며, 수호자의 다음 공격·스킬·방어 외 행동 종료 후 `철벽 1`, 그다음 자기 행동 종료 후 제거되는지 확인한다.
165. 철벽 중 원시 피해 10이 3으로 적용되고, 방어 버튼이 사용 불가 색상이며 클릭·키보드 Submit 시 `더 강한 방어 효과가 이미 적용 중이라 방어를 사용할 수 없습니다.` 안내 후 행동·턴이 유지되는지 확인한다.
166. 철벽 종료 후 공용 방어가 다시 가능하고 피해 10→5가 유지되며, 철벽과 공용 방어가 어떤 입력·피해 경로에서도 중첩되지 않는지 확인한다.
167. 철벽과 도발을 동시에 유지한 상태에서 적 단일 공격이 수호자로 향하고 70% 감소가 적용되며 두 상태의 남은 횟수가 각자 규칙대로 독립 감소하는지 확인한다.
168. 철벽 성공 직후 재사용 4턴/hourglass_top이 표시되고 수호자의 다음 행동 시작마다 3→2→1→0으로 감소하며 다른 참가자 행동에는 변하지 않는지 확인한다.
169. 수호자 전투불능 시 철벽 상태·아이콘·상세 문구가 즉시 제거되고, 연출 중 스킬/상태 팝업 억제와 완료 후 입력 복구·단일 턴 진행이 정상인지 확인한다.
170. 기존 도발·공용 방어·가이아 웰·모든 직업 스킬·독/화상/감전/정화·Formation/TurnOrder·승리/도망/Field 복귀에 회귀가 없고 Console에 Compile Error·NullReference·MissingReference가 없는지 확인한다.
171. 수호자 스킬 메뉴에서 실제 `pawns.png`와 `수호의 맹세`가 보이고 상세 설명에 광역 보호·자신 제외 생존 아군 전체·직접 피해 최대 50%·감소량 절반 이전·최대 HP 40% 보호 예산·DoT 제외·다음 행동 전·4턴 재사용이 표시되는지 확인한다.
172. 사용 시 Frost Nova 중간 테두리가 금백색·낮은 강도로 파티 전체에 짧게 한 번 재생되고 공격용 얼음 폭발처럼 과도하지 않으며 지속 VFX가 남지 않는지 확인한다.
173. 원시 피해 20을 보호 아군이 받으면 아군 10, 이전 예정 5가 되고 수호자에게 철벽이 없을 때 실제 5 피해가 적용되며 금색 선·섬광과 HUD 예산 감소가 함께 보이는지 확인한다.
174. 최대 HP 100 수호자의 이전 예산이 40으로 시작하고 철벽 중에도 이전 예정량 기준으로만 40까지 소비되며, 각 실제 이전 피해에는 70% 감소가 적용되어 예정 5가 실제 2가 되는지 확인한다.
175. 남은 예산 3에서 큰 직접 피해를 받으면 수호자 이전 예정량 3, 아군 감소량 최대 6만 적용되고 예산 0과 동시에 수호의 맹세 HUD가 사라지는지 확인한다.
176. 화상·독 행동 종료 틱은 아군에게 그대로 적용되고 수호자 HP·이전 예산·금색 이전 연출이 변하지 않는지 확인한다.
177. 수호자의 다음 행동 시작 직전에 수호의 맹세가 종료되고 쿨타임이 3으로 감소하며, 다른 참가자의 행동 시작·종료에는 보호와 쿨타임이 유지되는지 확인한다.
178. 수호자 전투불능 즉시 보호가 종료되고 도발·철벽과 동시에 유지 가능하며, 수호자 자신의 직접 피격은 이전 없이 기존 철벽·방어 규칙으로만 처리되는지 확인한다.
179. 단일 기본 공격·파이어 볼 등 단일 공격 스킬·화살비/회오리 베기/썬더볼트 같은 광역 공격의 아군 대상마다 보호 예산이 순서대로 소비되고, 대상 수를 4명 이상으로 늘려도 고정 3인 제한이 없는지 확인한다.
180. 기존 공용 방어·가이아 웰 대상이 보호받을 때 수호의 맹세 감소 뒤 각 대상의 기존 개인 방어가 유지되고, 도발·정화·감전·회복·승패·도망·Field 복귀에 회귀 및 Console 오류가 없는지 확인한다.

기존 Male/Female, Path Visual과 Wheelchair Variant, 이름표, 월드 경계·전환·초원 슬라임 필드 Animation도 회귀가 없는지 함께 확인한다.

181. 한 번의 Play Mode에서 회복의 파동·정화·철벽·수호의 맹세·가이아 웰을 각각 3회 이상 사용해 캐릭터 본체 색 변화·흰 네모·단색 사각형·잔존 Image·반복 누적이 없고 원본 VFX 후광만 정상 표시되는지 확인한다. 이어서 Play→Stop을 최소 3회 반복해 같은 항목을 재확인한다.
182. 파이어 볼·썬더볼트·화살비·동료의 습격·독/화상/감전·숲거미 독액 분사의 기존 색과 피해 횟수가 유지되는지 확인한다.
183. 민첩 동률 전투를 여러 번 새로 시작해 주사위가 순차 반복이 아닌 1~6 랜덤 눈으로 바뀌고 최종 눈도 항상 1이 아닌지, 결과 문구·Timeline·첫 행동자가 일치하고 다음 라운드에는 재표시되지 않는지 확인한다.
184. 민첩 동률이 없는 전투에서는 Overlay가 없고 바로 정상 입력으로 시작하는지 확인한다.
185. 화살비 후열 0명과 회오리 베기 전열 0명에서 범위별 경고 뒤 같은 스킬 메뉴가 즉시 조작되고 Esc·취소로 상위 명령에 복귀하며, 행동·턴·쿨타임·기세·VFX가 변하지 않는지 확인한다.
186. 기본 공격과 단일 스킬 Target Selection에서 화면 취소·Esc가 각각 같은 이전 메뉴로 돌아가고, 대상이 있는 화살비·회오리 베기·썬더볼트의 대상당 1회 피해와 감전·기세가 유지되는지 확인한다.

VFX PNG 전부가 RGBA Alpha 0~255이고 Sprite Import의 Alpha Is Transparency가 켜져 있으며, Arcane Parry 16·Radiant Heal 14·Spectral Bloom 16·Earth Rupture 20·Frost Nova index 4~10의 선언 범위가 실제 시트 크기 안에 있음을 확인했다. 회복의 파동 중앙 VFX에 남아 있던 구형 `CreateEffectImage` 일곱 번째 인수를 제거했고 프로젝트 내 15개 호출이 현재 5/6개 인수 시그니처와 일치한다. 광역 스킬 0 Target 입력 복구까지 Unity 6000.5.7f1의 전체 `Assembly-CSharp` 응답 파일로 별도 출력 컴파일해 오류 0개, 기존 deprecated API 경고 4개만 확인했으며 관련 파일 `git diff --check`를 통과했다. Tie Break와 수호의 맹세 계산·피드백·VFX 코드는 변경하지 않았다. 실제 Play Mode의 화살비·회오리 베기 Invalid Action 메뉴 복구와 Esc·취소, 수호의 맹세 및 위 171~186 항목은 직접 확인해야 한다.

## 다음 권장 작업

길별 새 캐릭터로 Battle에 진입해 회복탄력 50% 통과와 2회 소비, 적별 잔향 소비·만료, 집중 0/3/6/9%와 광역 비증가, 굳건한 자리 전열/후열, 적·아군 쌍별 패턴 피해 감소·치유 증가를 확인한다. 같은 전투에서 화상·독이 길 배율을 받지 않는지, 전투 종료 후 새 전투와 저장 이어하기에서 길 런타임이 초기화되는지도 확인한다.

우선 실제 정상 입력으로 Field_01/02 전투 승리·레벨 업·즉시 저장/재실행 복원·맹독 교체/정화·뱀 이동/공격/리스폰을 확인한다. 이어 실제 Battle에서 위 15개 스킬 버튼의 가독성·Esc 재진입·다음 턴/새 전투 아이콘 유지를 확인한다. 썬더볼트 버튼 Electric Impact와 감전 상태 power.png의 구분을 유지한다.

Unity Play Mode에서 월드 안쪽으로 이동 후 5초 자동 저장과 Stop/재실행 뒤 실제 위치 복원, Bounds 밖 좌표의 SpawnPoint fallback, Field 전환 뒤 새 Scene 좌표 저장, Battle 중 종료 시 마지막 안전 좌표 유지를 확인한다. Bootstrap 삭제 확인의 취소·단일 슬롯 삭제·즉시 빈 슬롯 갱신과 Editor 관리 창도 함께 확인한다. 이후 Field_02 감전 독립 적용과 기존 전투·외형·리스폰 회귀도 확인한다.

## 갱신 규칙

기능 작업 완료 시 완료 기능, 검증 상태, Unity 직접 확인 사항, 다음 권장 작업과 마지막 관련 commit hash를 갱신한다. 긴 작업 로그는 기록하지 않는다.


## 2026-10-13 Field04 v2 42도로 정식 적용

승인된 Road30 재배치+연장12 및 Grass315/Props18 적용. 새 격리 QA 253 PASS, 기존 Search/서쪽Spawn/북쪽경계/정본Unity버전 차이는 PARTIAL·OPEN. 원본 디스크 APPLIED. Main08/사용자Save/TTS/BGM 보호. 브랜치 codex/field04-v2-official-42-20261013; 관련 SHA는 해당 브랜치 최종 커밋 및 완료 보고 참조. 상세 `Field04_v2_Official42_Apply_QA_20261013.md`. 사용자 직접 플레이 확인 권장.

## 로컬 고유 이력 보존 — 과거 당시 기록

다음은 2026-10-19 의미 병합에서 보존한 로컬 작업복사·로컬 커밋의 고유 기록입니다. 과거의 미완료·승인 대기·재개 지시는 당시 상태이며, 현재 상태는 문서 상단과 최신 main 완료 기록을 따릅니다. 기존 수치·승인·OPEN·근거 경로는 변경하지 않았습니다.

### [과거 당시] 2026-10-14 최종 마감 확인 — 음성 완료 / 로컬 Git 동기화 PARTIAL (local_working)

- 태온007~011 승인후보를 정식 연결하고 앞 저음량 공백만 보정했습니다. 음성 PR14 merge d05a325b05f2eab79ee18c5ca65e9ee523a5e5ab, 마감문서 PR15 merge 9110bc7624e08ddbdd93b96510e8fafc5fbe341c. 신규TTS0·기존WAV/meta덮어쓰기0·승인005006/기존6·미엘/Player/Quest/Save 보호.
- 격리 최신통합본 음성250+Arbel1729+Main21펫106+전투/Field04회귀232+23Scene26=2343 PASS/0 FAIL. 기존 URP/Search·원본liveImport/물리스피커/OS입력 미검증은 별도 PARTIAL. Arbel/기획중복적용0·Chapter3DRAFT구현0.
- 최종 원격main/originmain 9110bc7624e08ddbdd93b96510e8fafc5fbe341c; 사용자 local main/HEAD ff022e63dfe364f559fcc29819d27b123ad31127. 로컬 문서전용2커밋 이력분기로 격리ff-only실패, 원본main이동/reset/clean/stash/forcePush0. **완전동기화 미완료**. 1274 dirty·미추적 백업/5851 SHA/UserData backup: Temp/EndOfDay20261014.
- 원본 EditorBootstrap·PlayOFF·PausedOFF·Focusedfalse·Compilingfalse 유지·강제Refresh0. [최종QA](EndOfDay_Integration_Voice_LocalSync_QA_20261014.md). 다음: 로컬전용3a37aa2e/ff022e63 문서커밋과dirty보존 통합방식 검토후동기화재개. 원본Play/포커스강제조작 금지, 음성재승인 요청불필요.

### [과거 당시] 2026-10-14 아르벨 v3 정식 적용·전용 브랜치 Push 완료 (local_working)

- 승인PNG23·SortingGroup2·건물 발 기준 뒤1/앞5·현재Sprite 국소지붕alpha0.18·불투명 처마/접합 보완을 원본디스크56경로에 최소반영. 신규PNG제작0/선택장식11배치0/기존환경Renderer284/284/런타임MissingSpriteScript0/BuildScene23유지.
- main core1765+회귀228+원본5파일 core1706+회귀228+BuildScene26=3953PASS/0FAIL. 세린 뒤/측면/문앞/접근·양쪽문·접합·도보/휠체어·실제NPC/shop/healer/pet/Portal관리인·SaveContinue·Main12/13실전·Field04 375/42회귀 PASS.
- 전체PARTIAL: 원본대조에서도 재현한URP/Search환경오류,원본liveImport/OS입력/정본다른Editor버전미검증. VillageNpc초기fileID0은 PlaceholderVisual의도값(런타임정상). Portal-나무OPEN유지·실Teleport기존미구현, 실제우회/관리인대화PASS.
- 초기4001중4000 SHA동일·승인Arbel.unity1만변경, 신규55경로·GUID충돌0. 추가TTS/Story392동일, rootHEADff022불변. 기존NPC좌표/Collider/Quest/Save/Settings/Field04/승인음성8/WAV/GUID/meta/Chapter3/복구Sprite보호. EditorBootstrap·PlayOFF·PausedOFF·Focusedfalse·Compilingfalse전후동일·강제Refresh/Scene전환0.
- 브랜치codex/arbel-v3-official-apply-20261014, Feature648b1603c7d79803fdfa54fd0c2222e3ef8cc201, 최종Push652867c988d015f0eb65ccb82591d2ef3a36551b(원격동일). main직접Commit/Push/PR병합0. [QA](Arbel_v3_Official_Apply_QA_20261014.md)·[A인계](Arbel_v3_Official_Apply_Handoff_20261014.md). 다음: 원본Editor정상작업재개시자연Import후실제화면확인, main병합은별도사용자지시만.

### [과거 당시] 2026-10-14 Main08 태온007~011 원본 조사·청취 후보 납품 (local_working)

- 원본007 비발화·008=007·009=008·010=009+010합본·011선행저음량 구간을 WAV/PCM/GUID/실제Unity로 확인. 기존 WAV 재사용 후보5 제작, 신규TTS/API0·정식Catalog연결NO·B Commit/Push0.
- 009/010 3.849417초 zero/zero 경계 분리·PCM재결합원본100% 동일. 011 선행1.079958초만 줄여 최종2.800042초, 마지막흡기0.19초/강한발화1.21초 남김·원본suffix동일·clipping0. 후보007 3.530초·008 4.810초·009 3.849417초·010 4.400583초.
- 격리Unity6000.6.5f1 Field02 WheelTracks→Field03 Investigation→DeepZone5+3→실제BootstrapContinue 97PASS/FAIL0. 원본Clip/자막/PCM·미엘·Player무음·Next·SaveQuest보호. 전체PARTIAL: 기존Search startup 예외,007/008 ASR어휘차이,청취승인대기. 원본4,001파일SHA 변경0(B납품시).
- 청취자료 [후보페이지](Evidence/Main08Taeon00701120261014/index.html), [QA·재개지점](Main08_Taeon007011_Content_Recovery_Candidates_QA_20261014.md). 다음: 바울 대표 개별청취승인 후 해당ID만 별도음성브랜치 연결·회귀QA. 최신확인 main314f7fbb(PR11완료), main직접변경0.

## V2 후보 구현 마감 기록 (2026-10-27)

- 구현·증거 로컬 커밋: 62578e5174d208dd4e83ebe23c2ac438303eb478
- 실제 Issue #66 보고: https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6430994946
- LOCAL_COMMITTED / REMOTE_NOT_PUSHED. Issue 댓글만 원격 반영, 코드·문서는 로컬입니다.
- V1 REFERENCE_VERIFICATION_HOLD 유지. V2 r9 미승인·미설치이며 별도 실제 설치 승인 전 중지합니다.

마감 근거: 사전검사 도구/증거 커밋 b08fb5c00661318d5c30f64749f4563c84297311. 실제 Issue66 보고 https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6431260341 . LOCAL_COMMITTED / REMOTE_NOT_PUSHED, WAIT_ADMIN_READONLY/HOLD/설치0.

마감: 구현/증거 커밋124be621ce1883b79f4659769bde74b6bdb153c3. 실제 Issue66 보고 https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6431728309 . LOCAL_COMMITTED / REMOTE_NOT_PUSHED, 실제Worker/staging HOLD·실제생성/설치0.

마감 근거: 구현·증거 커밋98e7787fe0e4f653f64cb9fb6380900e0caf0e44, 실제 Issue66 보고 https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6432178495 . 최종후보r11·78PASS/0FAIL, Worker실측HOLD/관리자결과대기, 실제staging·설치·IPC0, LOCAL_COMMITTED / REMOTE_NOT_PUSHED.

마감: 구현·증거 로컬 커밋07ef386bd150af8b2f81ae77edd35722e3870dc1. 실제 Issue66 보고 https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6433810712 . 신규56PASS/0FAIL, 실제11개ACCESS_DENIED/역할UNKNOWN·GroupPrincipal UNKNOWN·독점실행HOLD. r11불변/운영변경0, LOCAL_COMMITTED / REMOTE_NOT_PUSHED.

마감: 관리자판독·증거 로컬커밋d2102bb9eca1d0e0826cbcb6b3ae4f8f35deb3e3. 실제 Issue66 보고 https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6433935615 . 실제11개동일성·owner/GroupSID조회성공, 역할·AI·독점HOLD. 코드/r11/운영불변, LOCAL_COMMITTED / REMOTE_NOT_PUSHED.
