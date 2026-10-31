# Discord V2 독립 배포 도구 CS0236·원장 재사용 결함 표적 수정

2026-10-31. **CS0236_FIXED / TARGETED_24_PASS_0_FAIL / PRODUCTION_LEDGER_BINDING_HOLD / NO_GO / MANUAL / EXECUTION_HOLD**.

기준 Issue #66 최신 실패 보고 및 HEAD `6645e5c7ff7aed3a877f0117b89ef9e76879e444`. 동일 브랜치 `codex/discord-v2-independent-deployer-20261031`. 수정 코드 **`4117ca3670ac033b3bb88f226c610d5a78c60e5e`**. 기존 두 결함에만 변경했으며 Supervisor/Probe/NativeBatch 원본 소스는 변경하지 않았습니다.

## 1. 컴파일 오류 수정

`Tests/Program.cs`의 모의 host 초기 시각을 `Clock=DeploymentAdminTests.Now`로 명시하여 Fake.Now 인스턴스 속성과 구분했습니다. 변경 코드/테스트 DLL의 관리형 컴파일 **exit 0 / 경고 0 / 오류 0**, 기존 CS0236 해소입니다. 독립 도구 프로젝트/NativeAOT를 빌드한 결과가 아닙니다.

## 2. 요청 폴더 종속 소비 제거

- `DeploymentLedger.cs`: 요청 경로나 CLI/환경변수와 무관한 빌드 시 고정 `DeploymentLedgerBinding.Root`, 정확한 승인 `LedgerRoot`, 정규 DeploymentId로만 marker 경로를 만듭니다. 같은 ID는 요청 폴더/새 프로세스/실패 여부와 관계없이 동일한 파일을 가리킵니다. 요청과 binding의 경로가 다르거나 상대/UNC/정규화 불일치·CodeRoot/EvidenceRoot 중첩이면 HOLD합니다.
- 요청 JSON에 LedgerRoot를 필수 추가했습니다. 원문 SHA에도 포함되며 기존 요청을 조용히 변환하지 않습니다. 구 요청/소비 재사용은 허용하지 않습니다.
- `AdminDeploymentWindows.PinLedger`: 기존 고정 원장과 모든 부모를 pin하고 최종 경로/reparse/권한을 검사합니다. 원장이 없으면 실패하며 만들거나 기존 ACL을 바꾸지 않습니다. 원장/기록 owner는 BA 또는 SYSTEM, group SYSTEM, DACL protected, **상속 없는 SY·BA Full ACE 두 개만** 허용합니다.
- `ConsumeApproval`: 기존 `Path.GetDirectoryName(requestPath)` 사용을 제거했습니다. 공통 `DeploymentLedger.Consume`와 실제 어댑터의 CREATE_NEW(원자)/초기 명시 SD·공유 없음/write-through/최종 경로·owner/DACL·비상속 HANDLE readback을 사용합니다. 실제 변경 직전 현재 승인/권한/만료를 재검사합니다.
- 이미 있는 marker는 열어서 고치거나 삭제하지 않습니다. CREATE_NEW 이후 오류/취소/flush 실패 시 빈 기록도 보존합니다. 불확실한 소비를 다시 성공으로 처리하지 않습니다.

## 3. 실제 원장 위치는 추측하지 않음 — HOLD 유지

현 Native FAKE_ONLY 계약/최신 로컬 인계에는 **승인된 고정 ledger 절대 경로가 없습니다**. 다른 옛 V2/Dedicated 계약에 있는 ApprovalRoot는 현재 Native binding과 다르므로 가져다 쓰지 않았습니다. 실제 경로·소유권이 확인됐다고 보고하지 않습니다.

바울 대표 지시 4에 따라 production binding은 **미발급/빈 값이면 fail-closed HOLD**입니다. 선택적인 private binding `Supply(ref root)`가 승인된 고정값을 공급해야 하며, 실제 도구 프로젝트도 별도 private ledger binding 파일이 없으면 컴파일을 거부합니다. 이 binding은 런타임 인자/환경변수/요청 파일 위치로 설정할 수 없습니다. 실제 binding 파일/승인 원장/보호 폴더를 이번에 만들지 않았습니다.

이는 임의 경로로 바꿔 배포 가능 상태를 만드는 해결이 아닙니다. **원장 로직과 CS0236의 표적 검증은 통과했지만, 실제 원장 위치·보호 bootstrap 승인은 미완료**입니다. 같은 승인 원장을 이후 버전에서도 유지해야 하며 원장 이전/초기화/재지정 기능은 없습니다.

## 4. 변경 경계 검증 — 24 PASS / 0 FAIL

새 `--ledger-delta`만 실행했습니다. 기존 36·34 PASS와 이전 후보 전체 시험은 실행하지 않았습니다.

| 표적 | 결과/한계 |
|---|---|
| CS0236 정확한 고정 시각 초기화 | PASS |
| 서로 다른 요청 폴더의 동일 ID | 거부 PASS, 기존 bytes 불변 |
| 신규 고유 ID | 별도 기록 정상 생성 PASS |
| CREATE_NEW 후 실패/flush 실패 | 기록 유지·재사용 거부 PASS |
| 만료·manifest SHA 변조·비관리자·위장 상태 | 소비 전 거부 PASS |
| 실제 원장 binding 미발급/경로 재지정 | HOLD/거부 PASS |
| 약한 owner·group·미보호/상속/Worker ACE | 거부 PASS — 메모리 보안 descriptor 정책 시험 |
| 동시 동일 ID | 합성 메모리 원장 CREATE_NEW에서 성공 1/거부 1 PASS |
| 다른 관리형 프로세스에서 동일 ID/새 ID | 실제 테스트 DLL 자식 프로세스의 합성 파일 CreateNew: 동일 ID 거부(exit23), 새 ID 성공(exit0), 기존 파일 SHA 불변 PASS |
| 자식 프로세스 종료 후 동일 ID 재시도 | 반복 거부·기존 파일 bytes 불변 PASS |

마지막 두 시험은 Windows 배포 도구가 아니라 **관리형 테스트 DLL의 격리 QA 경로**를 실행했습니다. 숨김/no-window .NET 테스트 peer **5회**, 합성 QA marker 파일 **3개**만 생성했습니다. 모두 ignored `Temp/FixedLedgerDelta20261031/.../SyntheticLedgerQA` 아래이며 실제 원장/보호 경로가 아닙니다. 테스트 원장 IO의 owner/DACL 보호는 모의 정책입니다. **실제 Windows 보안 descriptor 적용/readback·승인된 원장 경로 유효권한 검증은 NOT_VERIFIED**입니다.

## 5. 보호 범위

| 항목 | 결과 |
|---|---|
| Supervisor | `6DF94E47C301C20EAB537EB6AD49C680129B80787536358E6BDC78ED50A842E7`, 7,559,680바이트 보존 PASS |
| Probe | `0BABC05A2E0B24B1E6563D049B8FAABD7375F737B2ACD5AEDEAF840FC5E59322`, 1,892,352바이트 보존 PASS |
| 원본 NativeBatch 소스 | 시작 HEAD 대비 diff 0 |
| 독립 도구 product/NativeAOT build·Main 실행·Windows backend 실행 | 모두 0 |
| 실제 보호 폴더/ACL/Task/계정/암호/OnBoot/V3 설정 변경 | 모두 0 |
| 실제 승인 생성/소비·EXE 복사/실행·IPC·AI·Discord 업무·Unity·Save | 모두 0 |
| 원본 main | `ab47b931fec92976170f9f50aabde1822d78bb90` 유지 |

관리형 테스트 컴파일/합성 QA 파일 생성은 위의 “실제 Windows 보호 설정 변경 0”과 구분합니다. SDK 최초 실행의 워크로드 안내가 있었지만 설치/업데이트는 하지 않았습니다. 테스트에 사용한 복제 소스와 현재 입력 SHA 일치를 확인했습니다.

## 6. 빌드 승인 가능 여부 및 잔여 조건

**현재 실제 도구 빌드 승인 준비 완료로 볼 수 없습니다: LEDGER_BINDING_HOLD.** 바울 대표가 현재 Native 계약에 사용할 정확한 고정 ledger 위치·관리자 보호 bootstrap 범위를 결정/승인해야 합니다. owner/DACL/상위 경로 검증 방법과 원장 영구 보존·재지정 금지를 확인한 뒤 검토된 private binding 및 정확한 도구 빌드 입력을 확정해야 합니다. 다른 계약의 승인 경로나 임의 새 위치를 대신 선택하지 않았습니다.

이번 24건은 두 결함의 표적 검증이며 독립 배포 도구의 전체 상태기계·실제 Windows 동작·NativeAOT를 검증한 결과가 아닙니다. 기존 미검증 부분을 PASS로 승격하지 않습니다. 이후 별도 승인으로 도구 NativeAOT 발행/새 도구 SHA·코드 포함·경고·V3 신뢰 확인이 필요합니다. 실제 도구 실행/보호 bootstrap·배포·Task/FAKE_ONLY 시험은 각각 별도 승인입니다.

자격증명 비공유·다른 실행 경로 미사용·새 이미지 V3 정상 판정은 여전히 **UNKNOWN / NOT_VERIFIED**, Windows 전체 동일 SID 부재 입증 주장은 없습니다. cutoff **2026-11-01 23:59:59 KST** 유지·만료 후 자동 연장/과거 승인 재사용/원장 초기화 금지. **NO_GO / MANUAL / EXECUTION_HOLD**로 보고 후 중지합니다. Push/main 병합·원본 적용 없음.

증거: [Summary](Evidence/DiscordV2FixedLedgerDelta20261031/Summary.json), [24PASS](Evidence/DiscordV2FixedLedgerDelta20261031/Delta.log), [관리형 컴파일](Evidence/DiscordV2FixedLedgerDelta20261031/ManagedCompile.log), [입력 SHA](Evidence/DiscordV2FixedLedgerDelta20261031/InputsSHA.json), [합성 파일 SHA](Evidence/DiscordV2FixedLedgerDelta20261031/SyntheticQARecords.json). LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 실제 바이너리·private binding·비밀정보 업로드 없음.

### 실제 GitHub 보고 영수증

수정코드 `4117ca3670ac033b3bb88f226c610d5a78c60e5e`, 보고/증거 `0e4f45d16e86c6b103da24e6ec50572463c343ee`. [Issue #66 표적 수정 보고](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6541481822) 실제등록·전체본문재조회일치 확인. 후속영수증은 문서뿐입니다. CS0236해소·24PASS/0FAIL·production ledger위치/binding미승인HOLD·실도구빌드/Main/Windowsbackend/보호설정변경0·NO_GO/MANUAL/EXECUTION_HOLD·LOCAL_COMMITTED/REMOTE_NOT_PUSHED로 중지합니다.
