# Discord V2 Bootstrap 관리자 실행 준비 — HOLD

2026-10-31. 기준 `83b5d15e9be0158c13a2842476236f33fca97450`, 제품 코드 `5a105bcadedd8f75dfe0c864def4f42dfc921ecf`. 준비 문서만 변경했습니다. NO_GO / MANUAL / EXECUTION_HOLD.

## 이미지 및 신뢰

Bootstrap 원본: `E:\LimitlessBuildDependencies\NativeAOT20261028\BootstrapNative20261031-e9e95a17a3a0412db3b3a21aefa62d50\artifacts\Bootstrap\Limitless.ProtectedBootstrap.exe`.

- SHA256 `2FF5669350A6761A7EB979F6133A4B0A85B44F5440F96A05644A57601026349E`, 2,951,168바이트: 이번 읽기 대조 일치.
- 기존 동일 이미지 PE32+ AMD64, CLR 없음, 함수 35개·핵심 분기 10개 정적 검증 근거를 재사용합니다. 관리자 승인·생성·owner/DACL/readback 포함 근거이며 실제 Windows 동작 증명은 아닙니다. 빌드·시험 재실행 0회.
- 현재 E 원본은 개인 사용자 소유, 상속 DACL, Authenticated Users 수정권한이 있어 직접 관리자 실행용 봉인 이미지가 아닙니다. 별도 보호 staging 필요.
- V3 엔진 2026.10.31.03. 격리 DB 메타데이터는 12,288바이트/마지막 변경 10월28일 18:00:50.7177605 KST. 새 이미지 삭제·SHA 변경은 관측되지 않았으나 전체 탐지 부재를 입증하지 않습니다. 과거 탐지 및 안랩 #745042의 제출 Probe 제외 결정을 새 Bootstrap 정상 판정으로 전용하지 않습니다. 새 이미지 공식 정상 판정 NOT_VERIFIED.

## 생성 대상과 descriptor

아래 순서의 신규 디렉터리 6개만 대상입니다.

1. `C:\ProgramData\Limitless`
2. `C:\ProgramData\Limitless\DiscordV2`
3. `C:\ProgramData\Limitless\DiscordV2\Control`
4. `C:\ProgramData\Limitless\DiscordV2\Control\DeploymentTool`
5. `C:\ProgramData\Limitless\DiscordV2\Control\Approvals`
6. `C:\ProgramData\Limitless\DiscordV2\Control\DeploymentLedger`

예상 SDDL: `O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)`.
소유자 Administrators, 그룹 SYSTEM, 보호 DACL, 상속 ACE 없이 SYSTEM/Administrators만 FullControl. Worker 변경·삭제·소유권 취득 권한을 부여하지 않습니다. SECURITY_ATTRIBUTES로 생성 순간 적용하며 기존 항목 채택·기존 ACL 수정은 하지 않습니다. OwnerSid=SYSTEM 선택은 별도 명시 승인 대상입니다.

## 생성 직전 필수 조건

이번 읽기 확인에서 여섯 대상은 모두 부재이고 C:\ 및 C:\ProgramData의 SDDL은 기존 정적 PASS 스냅샷과 동일합니다. 숨김 ProgramData는 -Force로 확인합니다. reparse 없음. 이 결과는 미래 실행 시점의 HANDLE 검사 대체물이 아닙니다. 실제 Worker 토큰 전체 유효 권한은 UNKNOWN입니다.

실행 시 full 관리자·상승 토큰·비위장 상태, 정확한 관리자 SID, 시간·취소 상태, 이미지 SHA, 승인 원문 SHA, 정확한 범위와 부모 native security descriptor SHA/FileKey를 다시 확인해야 합니다. 부모 HANDLE pin 및 공유 쓰기/삭제 제한, 동일 볼륨·최종 경로·reparse·owner/DACL 검증이 필요합니다. 기존 항목·접근 오류·경합·불일치 발생 시 HOLD합니다.

EXE와 승인 JSON은 **여섯 대상 밖에 이미 안전하게 보호된 별도 staging**에 있어야 합니다. 생성 예정 DeploymentTool/Approvals 안에 Bootstrap을 먼저 배치하면 계약을 위반합니다. staging 선택·안전 생성·이미지 복사·봉인은 이번 승인에 포함되지 않았으며 위치 미확정입니다.

## 승인 자료 및 지원 명령

실제 생성 승인 JSON은 발급하지 않았습니다. 엄격한 10개 필드 계약입니다.

|필드|확정값 또는 필요한 자료|
|---|---|
|Approved|별도 실제 생성 승인 이후에만 true|
|Profile|DISCORD_V2_PROTECTED_BOOTSTRAP_V1|
|ApprovalId|새 lowercase GUID N, 미발급|
|ToolSha256|위 SHA의 lowercase 표현|
|AdministratorSid|실제 UAC 상승 계정 SID, 미확정|
|OwnerSid|제안 S-1-5-32-544|
|Targets|위 순서의 정확한 여섯 경로|
|AncestorSecuritySha256|C:\, ProgramData의 GetSecurityInfo(flags 7) 원시 descriptor SHA 두 개, 미발급. SDDL 문자열 hash로 대체 금지|
|AncestorFileKeys|동일 부모의 native volume/file ID 두 개, 미발급|
|ExpiresUtc|신규 승인 시 확정. 현재 시각 이후이며 기존 cutoff 이내|

JSON은 8,192바이트 이하이고 unknown/중복/누락 필드는 거부됩니다. 봉인된 최종 원문 SHA256(lowercase)이 명령 인수입니다. 다음은 **실행 불가 템플릿**이며 실제 경로·승인 원문 SHA가 없는 상태에서 실행하지 않습니다.

```powershell
& '<승인된 보호 staging의 절대경로>\Limitless.ProtectedBootstrap.exe' --review --authorization '<봉인된 승인 JSON 절대경로>' --authorization-sha '<원문 SHA256 lowercase 64자리>'
& '<승인된 보호 staging의 절대경로>\Limitless.ProtectedBootstrap.exe' --create --authorization '<봉인된 승인 JSON 절대경로>' --authorization-sha '<원문 SHA256 lowercase 64자리>'
```

정확히 5개 인수만 지원합니다. **--review도 제품 실행이며 동일한 유효한 생성 승인 자료가 필요합니다.** 읽기 전용 별도 승인 프로필이 아닙니다. 이번에는 둘 다 실행하지 않았습니다. 따라서 실제 실행 가능한 최종 literal 명령 확정은 HOLD입니다.

## 대표 직접 GUI 작업과 별도 승인

보호 staging·공식 신뢰 근거·부모 native binding 자료를 확보하고, staging 준비/봉인과 Bootstrap 실제 생성의 정확한 범위를 별도 승인한 후에만 진행합니다. 대표는 시작 메뉴에서 PowerShell의 ‘관리자 권한으로 실행’을 선택하고 UAC의 프로그램·계정을 직접 확인합니다. 의도한 관리자 계정이 아니거나 경고·출처를 확인할 수 없으면 취소합니다. 암호는 Windows 보안 UI에만 입력하고 채팅·로그에 남기지 않습니다. UAC 동의는 작업 범위 승인이나 바이너리 안전 판정의 대체물이 아닙니다. 보안 경고 우회·V3 예외는 허용하지 않습니다.

상승 후 계정 SID를 읽기 확인하여 승인 자료와 대조하고, 승인된 시간창 안에서만 봉인된 명령을 사용합니다. 현재 Codex 프로세스의 관리자 역할은 활성화되지 않았습니다. 제품을 통한 실제 생성은 별도 승인 전 금지입니다.

성공은 exit 0 및 `BOOTSTRAP_CREATED_AND_VERIFIED=6`과 여섯 경로 owner/DACL/최종 경로/HANDLE readback으로 확인합니다. review 성공 문자열은 생성 성공이 아닙니다. 실패는 exit 70/`HOLD_BOOTSTRAP_INCOMPLETE_PRESERVE_PARTIAL_STATE_NO_RETRY`: 즉시 중지, 부분 생성과 기존 자료를 보존하고 삭제·ACL 수선·재시도·승인 재사용을 하지 않습니다. 도구에는 영구 승인 소비 원장이 없으므로 실패 전후의 일회성 운영 기록과 재사용 금지는 별도 절차로 관리해야 합니다. 자동 원복이나 소비 기록 초기화를 주장하지 않습니다.

## 범위와 마감

실제 폴더/ACL/이미지 복사/EXE 실행/Task/계정/OnBoot/백신/Unity/Save 변경 0회. 제품 소스·기존 이미지·Native binding 변경 없음. 기존 59 PASS 및 다른 완료 시험 반복 없음. 기존 Native CodeRoot/EvidenceRoot는 유지하며 운영 원장 binding 및 승인 소비는 발급하지 않았습니다.

기존 cutoff 2026-11-01 23:59:59 KST 자동 연장 금지. 토요일 운영 방침상 오늘 미완료는 HOLD 인계합니다. 다음 Discord 작업일 11월7일은 기존 이미지의 cutoff 이후이므로 옛 승인 재사용 불가이며 신규 시간창·영향 코드/빌드/이미지 승인 검토가 필요합니다. 11월2일부터 평일 게임 개발 복귀 방침 유지.
