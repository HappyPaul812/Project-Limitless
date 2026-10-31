# Discord V2 C드라이브 대체 보호 경로 읽기 조사·마감 인계안

2026-10-31. **C조상 DACL 정적 PASS / 전체 배포 안전성 UNKNOWN / NO_GO / MANUAL / EXECUTION_HOLD**.

기준 Issue #66 최신 보고 및 HEAD `89f79b83f49668c52981411343e7b443fcd79ce4`. 브랜치 `codex/discord-v2-c-protected-path-survey-20261031`. 코드·binding·이미지·원장 변경 없이 ACL/경로/빌드 입력·파일 크기만 읽었습니다. 기존 E드라이브 ACL 검사는 반복하지 않았습니다.

## 1. 후보 및 정확한 조상 판정

후보: **`C:\ProgramData\Limitless\DiscordV2`**. C는 고정 NTFS입니다. `C:\ProgramData\Limitless`와 후보 디렉터리는 현재 없습니다.

| 기존 조상 | 소유자 | 현재 적용 비신뢰 허용 ACE의 DELETE/DELETE_CHILD/WRITE_DAC/WRITE_OWNER | ReparsePoint | 기존 Native 조상 정책 |
|---|---|---|---|---|
| `C:\` | TrustedInstaller | 발견 없음 | 미표시 | PASS_STATIC |
| `C:\ProgramData` | SYSTEM | 발견 없음 | 미표시 | PASS_STATIC |
| `C:\Program Files` (기존 Native CodeRoot의 조상) | TrustedInstaller | 발견 없음 | 미표시 | PASS_STATIC |

현재 적용 CommonAce를 소스의 조상 정책과 대조했습니다. 상속 전용(InheritOnly) ACE는 현재 조상 개체의 변경 권한으로 세지 않았습니다. SYSTEM/Administrators 및 조상에서만 허용되는 TrustedInstaller를 제외한 적용 허용 ACE에 DELETE·DELETE_CHILD·DACL/소유권 변경·적용 Generic 권한이 없습니다. callback/object 등 지원 불명 ACE로 통과시킨 항목도 없습니다.

`C:\`의 Authenticated Users 적용 mask는 `0x4`(디렉터리 생성), Users는 `0x1200a9`; `C:\ProgramData`의 Users는 `0x1200a9`와 **`0x116`**입니다. 파일 시스템 객체에서 `0x116`은 생성/EA·attribute 계열 권한이며 **DELETE_CHILD(`0x40`)가 아닙니다**. SDDL 약어 `DC`만 보고 파일 시스템 DELETE_CHILD로 해석하지 않았습니다. 상위에서 새 이름을 만들 수 있으므로, 다른 프로세스의 선점은 CREATE_NEW로 거부하고 기존 폴더를 채택하지 않아야 합니다.

**정확한 위험 조상:** 현재 C 조사에서 기존 Native 조상 정책을 위반한 조상은 발견되지 않았습니다. 기존 확인된 `E:\`, `E:\Program Files`의 Authenticated Users DELETE 위험은 계속 HOLD이며 [기존 보고](Discord_V2_Ledger_Candidate_Path_HOLD_20261031.md)를 보존합니다. 이번에 E ACL을 다시 읽거나 수정하지 않았습니다.

## 2. 유효 권한의 확인 범위 — 전체 PASS 아님

현재 DACL의 **모든 적용 비신뢰 허용 ACE**를 보수적으로 합쳐도 위험 비트가 없다는 상한을 확인했습니다(deny를 이용해 낙관적으로 통과시키지 않음). 이것은 일반 비특권 사용자·그룹에 대한 현재 조상의 DACL 근거입니다.

그러나 Worker로 로그인/위장하거나 실제 Worker 실행 토큰의 그룹·특권을 취득하지 않았고, 그 토큰의 Windows AccessCheck를 실행하지 않았습니다. **실제 토큰의 유효 권한/특권 우회 가능성은 UNKNOWN**입니다. Worker가 privileged 그룹/특권을 갖지 않는다는 운영·토큰 검증이 별도로 필요합니다. 현재 snapshot은 적용 시점 HANDLE pin·최종 경로/볼륨·실효 권한 readback을 대신하지 않습니다.

후보와 하위 자산은 아직 없으므로 owner/DACL·부모 교체 방지·실제 CREATE_NEW 경합·원장 영속성을 검증한 것이 아닙니다. 따라서 **전체 후보 안전성은 UNKNOWN**, 개발/운영 binding 발급과 실제 적용은 HOLD입니다. 코드 정책 완화나 기존 C/E ACL 변경은 필요하다고 확정하지 않았으며 수행하지 않았습니다.

## 3. 전체 보호 범위와 Native binding — 실제로는 이미 C 고정

현재 비공개 `NativeBinding.cs`의 **CodeRoot·EvidenceRoot 둘 다 C드라이브**입니다. E 고정이라는 가정은 실제 자료와 다릅니다. 두 경로는 새 후보 하위는 아닙니다. 기존 Probe·Supervisor 각각의 봉인된 SourceManifest에 있는 NativeBinding SHA가 현재 파일과 일치함을 확인했습니다. 실제 기존 private 절대 경로는 공개 문서에 복사하지 않았습니다.

| 보호 역할 | 최소 변경 검토안 A — 새 binding 발급 아님 | 필요한 보호 조건/현재 한계 |
|---|---|---|
| CodeRoot | 기존 C Native binding의 정확한 경로 유지 | 현재 부재. owner BA/SY·protected DACL·Worker 읽기/실행, 변경/삭제/ACL/owner 변경 불허. 부모 조상은 위 정적 PASS, leaf 미검증 |
| EvidenceRoot | 기존 C Native binding의 정확한 경로 유지 | 현재 부재. Worker에는 계약의 create-file 최소권한만, 기존 증거/ACL/owner/삭제 권한 불허. 실제 유효권한·생성 readback 미검증 |
| 관리자 공통 경로 | 새 후보의 Limitless→DiscordV2→Control | 관리자 전용, 명시 초기 owner/비상속 DACL. 현재 부재·미승인 |
| 배포 도구 | 후보 아래 `Control\DeploymentTool` 검토 | 독립 도구/부모 보호, 자기 이미지 SHA·elevation/비위장 확인. 도구 아직 미빌드 |
| 승인/신뢰 자료 | 후보 아래 `Control\Approvals` 검토 | SYSTEM/Admin 전용, 요청/manifest/계약/승인 원문 SHA·기한·동작 범위. 실제 파일 발급 없음 |
| 고정 원장 | 후보 아래 `Control\DeploymentLedger` 검토 | owner BA/SY·group SY·D:P·SY/BA Full 두 ACE만, Worker 권한 없음. CREATE_NEW·원장 재지정/초기화/이전 금지. 현재 binding 미발급 |

이 전체 구성은 기존 Native 보호 계약과 **구조적으로 양립 가능해 보이지만 실제 보호 완료는 UNKNOWN**입니다. 공통 관리자 경로와 leaf들은 초기 `SECURITY_ATTRIBUTES`로 보호 생성하고 readback해야 합니다. 상속을 받은 일반 mkdir 후 수정하는 방식은 제안하지 않습니다.

**이미지 재사용 판정:**

- A안: 기존 정확한 C CodeRoot/EvidenceRoot·Worker SID/기타 binding을 유지하고 새 후보는 관리자 도구/자료/원장에만 사용하면 **현재 cutoff 내 같은 계약에서 Probe·Supervisor의 경로 변경 재빌드는 불필요**합니다. 두 이미지는 해당 binding의 배포 원본 후보로 보존할 수 있습니다. 신규 원장 private binding과 독립 도구 NativeAOT 빌드/새 SHA·신뢰 검증은 필요합니다. 실제 사용 승인이 아니며 마감 후 실행에 대한 재사용 보장도 아닙니다.
- B안: CodeRoot/EvidenceRoot까지 모두 새 후보의 `Code`/`Evidence` 등으로 통합하려면 **Native binding 변경 및 Probe·Supervisor 모두 새 NativeAOT 이미지**가 필요합니다. Probe는 자기 CodeRoot 위치를 검사하고 Supervisor도 파일/계약/Task 경로를 엄격히 검사합니다. 단순 파일 이동으로 해결할 수 없습니다. 새 SHA/계약/동작 승인/V3 판정과 경로 관련 검증이 모두 달라집니다. 현재 두 이미지의 SHA 재사용은 불가합니다.
- B안의 공유 상위 폴더는 Worker 경로 통과/읽기 요구와 관리자 Control 영역을 분리해야 합니다. A안의 관리자 전용 공통 경로를 그대로 쓰고 모든 역할을 옮길 수 있다고 보지 않습니다. 범위를 줄이는 **A안 우선 검토**를 권고하며 최종 선택/발급은 하지 않았습니다.

## 4. C드라이브 실제 필요 용량

확정 측정값은 두 EXE **9,452,032바이트(9.014MiB)**입니다. 도구는 아직 빌드하지 않아 크기를 확정할 수 없습니다.

**추가 C 필요량 = 9,452,032 + T(독립 도구) + S(실제 승인·제어/신뢰 자료) + R(소비 원장·작은 내구 기록) + NTFS allocation/metadata**.

현재 Native 제어 자료는 계약/승인 JSON 4KiB, 신뢰 근거 16KiB 등의 읽기 제한을 갖고 있어 정상 단발 자료는 작습니다. 하지만 실제 승인 manifest와 도구가 없으므로 정확한 최종 합계를 만들 수 없습니다. 원장 보존 기간/승인 ID 수에 따른 증분도 자동삭제로 상쇄해서는 안 됩니다.

승인 검토용으로 **64MiB의 잠정 배포 여유 공간 예산**을 제안합니다(확정 최소치나 강제 코드 상한이 아님). 새 도구·실제 manifest·기록 보존량을 측정해 예산 이내인지 적용 전에 다시 확인해야 합니다. 조사 시 C 여유 공간은 약 **48.28GiB**로, 현재 확인한 배포량보다 충분하지만 숫자만으로 운영 승인을 내리지 않습니다.

빌드 캐시·NuGet 7종·일반 SDK/도구 설치·PDB/OBJ/소스·대용량 QA 증거를 C 보호 영역으로 복제할 필요가 없습니다. E의 기존 빌드/패키지/아카이브 위치는 유지하고 C에는 보호 실행 파일·필수 작은 제어/내구 기록만 두는 원칙입니다. 이번에 SDK를 신규 설치/이전하거나 캐시를 C로 옮기지 않았습니다. 실제 tool/file 크기 측정 없이 큰 증거까지 C 예산에 포함된 것으로 처리하지 않습니다.

## 5. 별도 관리자 승인과 마감 판단

현 상태의 실행 판단은 **NO_GO**입니다. C 조상의 정적 PASS만으로 실제 배포를 승인할 수 없습니다.

1. A/B 구성 선택, 정확한 새 관리자 root/원장·도구·승인 자료 위치, 기존 Native scope 보존 여부를 명시 승인해야 합니다. 이번에는 어떤 경로도 final binding으로 발급하지 않았습니다.
2. C 공통 부모·관리자 Control/각 leaf 및 기존 정확 Native CodeRoot/EvidenceRoot의 **부재·선점 없음 확인→초기 보안 descriptor/CREATE_NEW 생성→owner/DACL/최종 경로/HANDLE/실효 권한 readback**을 별도 승인해야 합니다. 기존 C/E 공유 ACL 변경·기존 폴더 채택/덮어쓰기·소유권 탈취를 포함하지 않습니다. 실패 시 부분 증거 보존, 자동 rollback/삭제 없음.
3. 독립 도구의 private ledger binding·관리형/표적 검증·별도 NativeAOT 빌드/새 도구 SHA/코드 포함/V3 신뢰 확인·관리자 보호 bootstrap을 구분 승인해야 합니다. 현재 도구 이미지가 없고 실제 Windows 검증도 미완료입니다.
4. 실제 계정/자격증명 비공유·다른 실행 경로/보호자산 관리·V3 이미지 신뢰 근거, 정확한 신규 DeploymentId/TrialId/계약/동작별 승인·기한·소비 부재가 필요합니다. 현재 UNKNOWN/NOT_VERIFIED를 PASS로 승격하지 않습니다. 실제 Task/FAKE_ONLY 시험 승인은 별도입니다.

**11월 1일 마감에 필수 조건이 남아 있으면 E 공유 ACL을 급히 수정하거나 검증을 생략하지 말고 HOLD 상태로 11월 2일 문서 인계를 권고합니다.** 이미 도달한 NO_GO를 자동 해제하지 않습니다. 11월 2일은 기존 승인으로 실행을 재개하는 날짜가 아닙니다.

### 11월 2일 안전 인계안

- 기존 E 위험 조상/원장 미발급 HOLD, C 조상 정적 snapshot·실제 토큰/leaf 보호 UNKNOWN을 함께 전달합니다.
- 두 원본 이미지와 SHA·현재 C Native binding의 원문 SHA·소비 기록을 그대로 보존합니다. 파일 이동/원장 이전/초기화/마감 연장 없이 검토 자료만 인계합니다.
- 권고 구성은 A안이며, 부모/부재 상태의 새 읽기 검증·경로 결정·보호 bootstrap·독립 도구 검증/새 승인부터 명시적 후속 지시를 받아야 합니다.
- cutoff **2026-11-01 23:59:59 KST** 및 기존 계정만료 인계값 **2026-11-02 00:00 KST**를 경고합니다(계정 만료는 이번에 재조회하지 않음). 현재 Supervisor의 AdmissionPolicy.Deadline도 기존 cutoff이므로 11월 2일에는 그 이미지를 그대로 새 실행 가능 상태로 볼 수 없습니다. 새 실행을 원하면 별도 새 지시/계약·계정/신뢰 검토가 필요하며, cutoff 코드를 변경하는 Supervisor·배포 도구는 새 빌드/새 SHA 검증 대상입니다. Probe 재사용도 당시 binding/SID/신뢰를 다시 판단해야 합니다. 자동 연장·옛 승인/소비 재사용 금지입니다.

## 6. 보호 범위/보고 근거

Supervisor `6DF94E47C301C20EAB537EB6AD49C680129B80787536358E6BDC78ED50A842E7` / 7,559,680바이트와 Probe `0BABC05A2E0B24B1E6563D049B8FAABD7375F737B2ACD5AEDEAF840FC5E59322` / 1,892,352바이트 무결성 보존. 코드·binding·이미지·소비 원장 변경 0, 폴더 생성/ACL·owner/Task/계정/OnBoot/V3 변경 0, NativeAOT 빌드/EXE 실행 0, 기존 시험 반복 0. E ACL 재검사 0. Unity·Save 접근/변경 0. 원본 main `ab47b931fec92976170f9f50aabde1822d78bb90` 유지. Push/main 병합 없음.

[읽기 조사 증거/ACL·용량·binding 입력 일치](Evidence/DiscordV2CPathReadOnlySurvey20261031.json). LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 문서만 갱신하고 **NO_GO / MANUAL / EXECUTION_HOLD**로 중지합니다.

### 실제 GitHub 보고 영수증

읽기 조사/인계안 로컬커밋 `f5daf3ffa19723ef760ab926f5886a03cebf4492`. [Issue #66 C 대체 경로 조사](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6541753116) 실제등록·전체본문재조회일치 확인. 후속영수증은 문서뿐입니다. C조상정적PASS/전체UNKNOWN·실Native이미지binding이미C·실제경로발급/코드/이미지/원장/폴더ACL/Task계정변경0·NO_GO/MANUAL/EXECUTION_HOLD·LOCAL_COMMITTED/REMOTE_NOT_PUSHED로 중지합니다.
