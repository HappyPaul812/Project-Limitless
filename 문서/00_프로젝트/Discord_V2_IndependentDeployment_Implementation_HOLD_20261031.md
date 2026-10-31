# Discord V2 독립 관리자 배포 도구 B안 — 구현 후보/컴파일 실패 HOLD

2026-10-31. **IMPLEMENTATION_CANDIDATE_UNVERIFIED / TEST_COMPILE_FAIL / SECURITY_HOLD / NO_GO / MANUAL / EXECUTION_HOLD**.

기준 Issue #66 최신 보고 및 `e9fbba36369babb7611eddac64fb5d233fdbe769`. 브랜치 `codex/discord-v2-independent-deployer-20261031`. 미검증 후보 코드 **`ac0fb5955bb79763dbe1817546ba1983fb780b02`**. 바울 대표의 **“실제 구현 또는 보안 검증에 실패하면 중지하고 보고하세요”** 조건에 따라 첫 테스트 컴파일 실패 후 수정·재시도·모의 실행 없이 중지했습니다. 완료/검증 PASS가 아닙니다.

## 추가한 독립 후보 — 원본 Supervisor/Probe 무변경

`Tools/DiscordV2/ProtectedDeploymentAdmin/`에만 새 코드를 작성했습니다.

| 파일 | 후보 역할 |
|---|---|
| Program.cs | 독립 AdminDeploymentProgram 진입점. 정확한 --review/--apply, 보호 요청 경로/원문 SHA만 입력. 비밀값·암호 입력/로그 없음 |
| DeploymentAdmission.cs | 독립 승인 요청 strict JSON, 정확 DeploymentId/TrialId·도구 SHA·manifest/승인/계약/검토 근거 SHA·이미지 상수·승인 범위·만료/cutoff 검증. 기존 ProtectedDeployment.Execute를 변경 없이 재사용한 읽기 계획 → 검증된 단계별 변경 경계 |
| AdminDeploymentWindows.cs | 기존 Windows 후보를 별도 어댑터로 복제·분리. 관리자 상승/비위장·자기 도구/상위 경로 pin, 초기 owner/D:P DACL, CREATE_NEW, 원본 pin, flush/SHA/소유권/DACL/최종 경로 readback 후보 |
| DeploymentSecurityPolicy.cs | 메모리 descriptor/readback와 도구 PE 형식 검사. PE 형식만으로 NativeAOT/코드 신뢰를 입증하지 않음 |
| DeploymentAdmin.csproj | 별도 StartupObject/AssemblyName, 향후 단일 NativeAOT 도구 프로젝트 정의. private reviewed binding 필요. **이번에 build/publish하지 않음** |
| Tests/Program.cs, Tests/DeploymentAdmin.Tests.csproj | 새 배포 경계만 메모리 host로 시험하도록 작성. 실제 도구 Main/Windows backend/기존 시험 실행 경로 없음. **컴파일 실패로 미실행** |

원본 `ProtectedDeployment.cs` 계약/계획 코드는 읽기 참조합니다. 원본 `ProtectedDeploymentWindows.cs`와 무조건 HOLD는 그대로 보존했습니다. 새 어댑터는 별도 `DeploymentWriteBoundary` 객체가 있어야 쓰기를 호출하도록 작성했으며 단순 bool/환경변수/force로 기존 guard를 여는 방식을 사용하지 않았습니다. 다만 **새 경계 자체가 검증 통과한 것은 아닙니다**.

## 실제 검증 결과 — 실패 및 중지

허용된 단위 테스트를 위해 테스트 프로젝트만 격리 복제해 관리형 테스트 DLL 컴파일을 1회 시도했습니다. 독립 도구 프로젝트는 빌드하지 않았고, 실제 도구/기존 이미지/Windows backend는 호출하지 않았습니다.

- 테스트 컴파일 **FAIL 1 / exit 1**, 경고 0 / 컴파일 오류 1.
- 오류 **CS0236**, `Tests/Program.cs:60`: `internal DateTimeOffset Clock=Now;`의 `Now`가 바깥 테스트 클래스의 static 고정 시각 대신 Fake의 인스턴스 `Now` 속성으로 결합됩니다.
- 신규 모의 테스트 **미실행**, PASS 0 / FAIL 0 / NOT_RUN_COMPILE_BLOCKED. 준비한 테스트 수를 PASS로 세지 않았습니다.
- 관리자 인증·승인/계약·DACL·readback·중복/실패 처리의 런타임/모의 검증 결과 **NOT_VERIFIED**. 컴파일 오류가 이 항목들의 안전성을 입증하거나 반증하는 것은 아닙니다.
- 기존 36·34 PASS 기록 보존, 재실행 0. 실패 시 중지 지시 때문에 fixture 수정 후 재시도도 하지 않았습니다. 후속 수정 시 고정 시각 참조를 명확히 한 뒤 새 경계 검증부터 수행해야 합니다.

## 추가 정적 보안 차단 사항 — 소비 원장 위치

`AdminDeploymentWindows.ConsumeApproval`의 marker가 `Path.GetDirectoryName(requestPath)` 아래 `<DeploymentId>.deploy-consumed`입니다. 요청 파일 위치가 CLI에서 달라질 수 있으므로 같은 승인/DeploymentId가 다른 보호 폴더에 복제되면 **폴더별로 소비 위치가 달라집니다**. 동일 위치의 CREATE_NEW만으로 전역 일회성을 보장할 수 없습니다. 실제 재사용/Windows 적용은 시도하지 않았습니다.

후속 구현에서는 **독립 도구 binding에 결합된 단 하나의 고정 보호 소비 원장**과 승인에 명시된 ledger 경로/DeploymentId를 검사해야 합니다. 이 원장은 새 CodeRoot/EvidenceRoot나 요청 파일 위치에 따라 바뀌어서는 안 되고, 다른 요청 위치·새 프로세스·실패 후 재시도에도 동일 ID를 거부해야 합니다. 요청/승인/도구 SHA·원장 상위 경로 보호를 함께 검증하고 기존 기록 삭제/초기화를 금지해야 합니다. 이 결함이 남아 있으므로 fixture 오류를 고쳐 컴파일만 성공해도 빌드/배포 GO가 아닙니다.

## 특권 주체 위험과 남은 검증

새 도구는 기존 두 이미지와 별개인 **새 특권 실행 주체**입니다. 관리자 역할/상승·비위장, 보호된 요청/부모 경로, 자기 도구 SHA와 초기 배포 보호, 이미지/계약/신뢰 검토의 원문 SHA, 정확한 단계/대상, 소비 직전 및 실제 쓰기/복사 chunk 직전 만료 재검사를 후보에 넣었습니다. 요청의 검토 bool은 기본 false이며 실제 관리자 진술/근거를 대신할 수 없습니다. 도구 자체가 변조됐거나 관리자가 잘못된 진술을 승인한 경우를 코드만으로 해결했다고 주장하지 않습니다.

소유권·비상속 DACL·부모 교체 방지·SHA readback은 기존 후보를 재사용했지만 **실제 Windows 검증 미실시**입니다. 요청/원문 HANDLE 보호·실효 권한·CREATE_NEW 경합·부분 복제/실패·만료·같은 ID의 다른 경로 재사용·새 프로세스 소비 유지 등에 대한 새 모의/정적 검증도 완료해야 합니다. 실계정 자격증명 비공유/다른 launcher 미사용/이미지 V3 공식 정상 여부는 **UNKNOWN / NOT_VERIFIED**로 유지합니다. Windows 전체 동일 SID 부재 입증도 하지 않았습니다.

## 기존 이미지와 원본 보호

| 이미지 | SHA256 | 바이트 | 결과 |
|---|---|---:|---|
| Supervisor | `6DF94E47C301C20EAB537EB6AD49C680129B80787536358E6BDC78ED50A842E7` | 7,559,680 | 무결성 PASS |
| Probe | `0BABC05A2E0B24B1E6563D049B8FAABD7375F737B2ACD5AEDEAF840FC5E59322` | 1,892,352 | 무결성 PASS |

두 이미지와 기존 NativeBatch 소스 diff 0·복사/이동/실행/재빌드 0. 새 도구 product build/NativeAOT 발행/실행 0, 실제 승인 생성/소비·폴더/ACL/Task/계정/암호/OnBoot/V3 설정/IPC·AI·Discord 업무/Unity·Save 변경 0. 원본 main `ab47b931fec92976170f9f50aabde1822d78bb90` 유지.

## 후속 작업 및 실제 적용 승인

1. 이번 실패/중지 결과 검토 후 fixture 오류와 고정 소비 원장 결함을 수정하는 후속 작업이 필요합니다. 새 경계의 컴파일·모의/정적 검증을 통과해야 합니다.
2. **실제 독립 도구 빌드 필요**: 검증 통과 후 별도 승인으로 격리 NativeAOT 발행·새 도구 SHA/PE/코드 포함/경고/V3 신뢰 확인. 현재 도구 EXE/SHA는 없습니다. 기존 Supervisor/Probe의 재빌드는 필요하지 않습니다.
3. 도구와 승인 자료/고정 ledger의 관리자 보호 bootstrap 위치·소유/DACL·부모 경로 보호 readback에 대한 별도 승인이 필요합니다. 아직 bootstrap을 적용하지 않았습니다.
4. 실제 배포 승인은 정확한 도구/두 이미지 SHA·DeploymentId/TrialId·manifest/계약/승인 SHA·고정 ledger·대상 CodeRoot/EvidenceRoot/Worker SID·유효기간·허용 생성/복제 목록·실패 증거 보존에 결합해야 합니다. 기존 경로 존재/보호 실패면 중지, 부모 ACL 수정·기존 증거 삭제·승인 재사용 허가는 아닙니다.
5. 실제 배포/도구 실행 및 Task/FAKE_ONLY 시험은 별도 승인 대상입니다. cutoff **2026-11-01 23:59:59 KST**, 만료 후 옛 승인 재사용/자동 연장 금지. NO_GO/MANUAL/EXECUTION_HOLD로 중지합니다.

[실패 로그](Evidence/DiscordV2IndependentDeploy20261031/TestCompile_FAILED.log), [상태/원본 보존](Evidence/DiscordV2IndependentDeploy20261031/Summary.json), [미검증 후보 입력 SHA](Evidence/DiscordV2IndependentDeploy20261031/CandidateInputsSHA.json). 로컬 커밋만 보존/REMOTE_NOT_PUSHED. Push/main 병합/원본 적용·비공개 binding/바이너리/비밀정보 업로드 없음.
