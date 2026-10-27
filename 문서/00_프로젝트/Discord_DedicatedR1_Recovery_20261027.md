# Issue66 — r1 설치 재개 요청 / 최소 읽기 감사

## 관리자 읽기 결과 수신 / 콘솔 재개 도구 준비

[Issue #66 실제 보고](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6435886677), 최종 로컬 커밋은 Issue 후속 참조 / REMOTE_NOT_PUSHED.

대표 제공 [AdminRecoveryUserProvided.json](팀운영/Evidence/DedicatedR1Preparation20261027/AdminRecoveryUserProvided.json)은 admin=true, BA/SY protected approval root·정확 r1 승인파일만 PRESENT, fixed_r1_binding/same_admin=true/expired=true입니다. 승인 SHA는 `2ee977688b2cd0d11590aff0232318f8bf936f2f1f25e3a9dace64e2669e9cdf`. 소비 디렉터리/마커/lease·Code/State·Bot/Worker·Task 모두 ABSENT, protected Result디렉터리 ABSENT/count0입니다. changes/install/password/IPC/model0. 원본 경로·시각 미제공인 대표 JSON을 값 동일 재직렬화했으며 독립 관리자 재조회 완료로 표현하지 않습니다.

읽기 진단은 retry_authorized_by_probe=false를 유지합니다. 별도의 앞선 대표 **“다시 해줘” 재개 요청**에 따라, 아래 보존형 단발 재개 도구를 준비했습니다. **Codex의 실제 승인 이동/재발급/설치 실행은0**이며 대표 직접 실행 전까지 설치 HOLD입니다. 고정 r1 Installer/Manifest/ZIP·기존helper·staging은 변경하지 않았습니다.

새 `resume-r1-console.ps1` + `ResumeCore.ps1`은 다음 순서입니다.

1. 관리자 PS7 x64·stdin/stdout 실제 콘솔·기존 r1 stage 전체 handle/SHA/권한·approval root/file BA/SY DACL·기존 승인 SHA/동일관리자/만료·소비 없음·신규 계정/Task/CodeState 없음·재개 marker 없음 확인. 하나라도 불명/기존 자산이면 실제 변경 전 HOLD.
2. 보호 approval root 아래 `Resume-2ee977688b2cd0d1` 디렉터리를 CREATE_NEW로 1회 예약·lease. 이전 승인 파일을 그 안의 `ExpiredApproval.json`으로 **덮어쓰기 없이 이동**하고 원본 SHA/DACL을 재검증. 원문 바이트/승인 근거는 보존하며 삭제하지 않음. 이 기록은 소비 marker가 아니고 단발 재개 증거입니다.
3. 고정 schema3/동일 package/epoch·approval_id·3SHA·동일 관리자·동일 최소 설치 범위를 검증해 **새 15분 승인**을 원래 승인 파일명에 CREATE_NEW. 만료 승인을 유효 승인으로 간주하거나 기존 소비를 재사용하지 않음. 미사용 소비 키는 원래 고정 r1 계약 그대로이며 새 marker/보존 증거로 승인 갱신을 별도 추적.
4. 수정되지 않은 protected r1 Installer를 **Start-Process -NoNewWindow -Wait**로 동일 콘솔에 연결, stdin/stdout 파일·파이프 수집 없이 1회만 실행. 기존 `$json=& pwsh...` 수집은 이 재개 경로에서 사용하지 않음. 두 암호는 고정 설치기의 로컬 SecureString만 사용. [Microsoft Start-Process](https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.management/start-process?view=powershell-7.5)는 NoNewWindow의 현재 콘솔 사용과 기본 키보드 입력/콘솔 출력을 설명합니다. 실제 관리자 콘솔에서 기존 오류가 해소되는지는 아직 미검증입니다.
5. Native Installer JSON은 콘솔에 직접 표시. 뒤의 재개 envelope `CHILD_RETURNED_REVIEW_INSTALLER_JSON_NOT_SUCCESS_PROOF`는 **설치 PASS가 아님**. 자식 exit0만으로 PASS 판정 금지, 실제 Installer status/SID/보호 Result를 별도로 판독. `ResumeOutcome.json` 보호 기록, 자동재시도0·오류/불명은 부분승인/자산 보존HOLD.

새 결합 fake 검사 [ResumeFakeTests.json](팀운영/Evidence/DedicatedR1Preparation20261027/ResumeFakeTests.json)은 **24 PASS/0 FAIL**. 콘솔/권한/소비/네임스페이스/만료/SHA/동일관리자·역할/epoch 실패는 변경0, 승인 보존→새발급→최대1회 자식 순서·중복/오류/정리실패 HOLD·비밀문자 비출력·구문을 검증했습니다. 실제 승인/계정/Task/설치/암호 입력0, 기존12/10/151 재실행0. 코드 역할/배포 계약을 바꾸지 않은 별도 외부 복구 도구이며 실제 OS 동작은 결과 수신 전 PASS가 아닙니다.

SHA256: 재개 wrapper `2fdf7a494fa3b7e0d82cf4592568b36ad562678581015f1f31f7d099d91c010e`, core `01fa6f5f51f5a3a269d149be74ccb540ab0422a18ad8b1089490a6af6bf57785`. wrapper가 core SHA를 검사합니다. 정확한 한 줄 명령은 관리자 명령 문서4번입니다. **기존 Install/Stage/Preflight 명령은 재실행하지 않습니다.** 새 재개 도구도 최대1회 직접 실행하고 결과 JSON만 전달합니다. 입력 리디렉션 감지 시 승인 변경 전 HOLD이며 암호를 파일/인수로 우회하지 않습니다.

재개 실패 시 기존 승인 archive/새 승인/marker·lease/소비/신규 자산을 보존하고 자동 삭제·재발급·권한완화0. 승인 이동 후 새발급 실패나 자식 실패도 marker를 남겨 같은 재개 명령 반복을 막습니다. 성공해도 신규 Task/IPC/FakeWorker/AI/로그인/usage/Discord/A는 실행하지 않습니다. V1/MANUAL/A/runtimeHOLD·기존 Worker11/운영/인증 보호·Push0. 관리자 직접 재개 결과 대기입니다.

## 이하: 앞선 읽기 진단 준비 이력

바울 대표의 “다시 해줘” 요청에 따라 실패 설치의 재개 준비를 진행했습니다. 기존 설치 명령을 그대로 반복하거나 승인/소비를 삭제하는 승인이란 뜻으로 처리하지 않습니다. 대표 직접 관리자 실행·기존 자산 보존·단발/오류HOLD 조건을 유지합니다. 현재는 **HOLD_REQUIRES_RECOVERY_REVIEW**, 설치 재시도0입니다.

실제 읽기에서 `C:\ProgramData\LimitlessRelayDedicatedV2Approvals` 폴더 존재를 확인했으나 내부 열거는 access denied입니다. 비관리자 권한으로 승인/소비/계정/Task를 확정하지 않습니다. 별도 비밀 없는 child 메타 조회에서 stdin/stdout redirected=true를 관측했으나 이 비대화형 환경 결과를 대표 관리자 창의 정확한 오류 원인으로 단정하지 않습니다.

새 `Tools/DiscordAIServiceDiagnostics/R1Preparation/probe-install-hold-readonly.ps1`은 승인 root/고정 승인JSON binding·만료·관리자일치, 소비 디렉터리/마커/lease, 신규 Code/State, 계정2 SID/활성 상태, 고정 Task의 metadata 및 protected Result 개수만 읽습니다. 암호/Token/환경/명령줄/인증/Result본문을 읽지 않고 생성·수정·삭제·설치·LSA·Task실행 경로가 없습니다. 보호 부모를 읽지 못한 경우 자식의 ItemNotFound를 ABSENT로 오인하지 않고 UNKNOWN으로 보존합니다.

도구 SHA256 `c34be6d7bd023c3575b2cf16a2b53f4a06c3225209fcfff42edf8916ffad5874`. PowerShell 구문 PASS, 비관리자 신규 진단 결과는 UNKNOWN9개·Code/State 부재2개, changes/install/password/IPC/model0입니다. [정제 결과](팀운영/Evidence/DedicatedR1Preparation20261027/InstallRecoveryReadonlyNonAdmin.json). 이는 기존12/10/151 재실행이 아니며 protected 상태 PASS가 아닙니다.

대표가 관리자 PowerShell7에서 아래 읽기 한 줄만 실행하고 JSON을 전달합니다. 암호를 묻지 않으며 창을 닫는 exit가 없습니다.

```powershell
$r1ProbeBytes=[IO.File]::ReadAllBytes('F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\R1Preparation\probe-install-hold-readonly.ps1');if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($r1ProbeBytes)).ToLowerInvariant() -cne 'c34be6d7bd023c3575b2cf16a2b53f4a06c3225209fcfff42edf8916ffad5874'){throw 'R1_PROBE_SHA_HOLD'};& ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($r1ProbeBytes)))
```

읽기 도구 자체는 retry_authorized_by_probe=false이며 어느 결과도 자동 설치를 실행하지 않습니다. 관리자 JSON 수신 후 consumed 여부·기존 계정/Task·approval 만료와 정체를 분리해 판단하고, 입력 handle 경계의 필요한 수정/복구 범위를 검토합니다. 현재 고정r1/기존helper/설치기/승인/소비/staging 불변, 암호변경/재발급승인/자동삭제/계정설치0입니다. 유효 토큰·GROUP/Session0IPC/Job독점·V1/MANUAL/A HOLD·기존 운영/인증 보존, Push0. 브랜치 codex/discord-usage-diagnostics-20261026, 시작HEAD88e2c71bbec6165b3c29747149cfcaf4b55f4574.
