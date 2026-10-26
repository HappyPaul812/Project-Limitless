# Issue66 관리자 미확인 프로세스 진단 결과 판독 — 2026-10-27

**REFERENCE_VERIFICATION_HOLD / NOT_CLEANED.** 관리자 결과 수신·판독만 완료했습니다. 진단기·Cleanup·설치 재실행0, 시스템 변경0입니다.

- 결과 f5c37d29d1d1448d8bc296e801a72ecb.json, SHA256 **1b981f432d715025724d94381e97c993532bacd773ad2a6de9b16389b2a65a18**.
- 결과 내 is_admin=true, 관측 UTC 2026-10-26T22:55:28.4954493Z = **2026-10-27 07:55:28 KST**.
- 원본 판독 SHA23798422… 일치. 원본19개 중 보존된PID16개만 대상, 원본PID누락3개 그대로입니다.
- **현재 메타데이터 부분확인15/19**, 추가1개는 조회 객체 종료 근거 확보. **과거 동일성 확정0/19·NO_REFERENCE_VERIFIED0·REFERENCE_CONFIRMED0**. 종료시각이 있는 현재 객체를 과거 대상으로 자동 연결하지 않습니다.
- 살아 있는15개: 이름·native/CIM 생성시각·소유 계정/SID·Session0 확보. SYSTEM(S-1-5-18)13개, LOCAL SERVICE(S-1-5-19)2개. 시험계정SID1008 소유 일치0.
- 이미지 확보12개(Windows System32 경로9개, 정제 비시스템 경로3개), 이미지와 시험별칭 일치0. 이미지 미확보3개: Secure System/Registry/Memory Compression.
- **명령줄 확보0/15**, CIM ExecutablePath 제공0/15. 공백은 오류 코드가 아닙니다. Windows 미제공 이유를 보호/권한거부 하나로 단정하지 않습니다. 비보호 수준으로 보고된3개도 명령줄은 공백입니다.

## 각 대상
| PID | 현재 이름 | 생성시각 UTC | Session / 소유 SID | 보호수준 원시값 | 현재 판독 |
|---|---|---|---|---|---|
| 288 | Secure System | 2026-10-14T11:17:02.7668752Z | 0 / S-1-5-18 | 5 | UNKNOWN(참조 미확인) |
| 328 | Registry | 2026-10-14T11:17:02.8062118Z | 0 / S-1-5-18 | 5 | UNKNOWN(참조 미확인) |
| 940 | smss.exe | 2026-10-14T11:17:04.8317071Z | 0 / S-1-5-18 | 0 | UNKNOWN(참조 미확인) |
| 1316 | csrss.exe | 2026-10-14T11:17:07.6993378Z | 0 / S-1-5-18 | 0 | UNKNOWN(참조 미확인) |
| 1452 | wininit.exe | 2026-10-14T11:17:09.1024874Z | 0 / S-1-5-18 | 0 | UNKNOWN(참조 미확인) |
| 1532 | services.exe | 2026-10-14T11:17:09.2838813Z | 0 / S-1-5-18 | 0 | UNKNOWN(참조 미확인) |
| 1552 | LsaIso.exe | 2026-10-14T11:17:09.3143806Z | 0 / S-1-5-18 | 4294967294 | UNKNOWN(참조 미확인) |
| 1588 | lsass.exe | 2026-10-14T11:17:09.3169367Z | 0 / S-1-5-18 | 4 | UNKNOWN(참조 미확인) |
| 3360 | Memory Compression | 2026-10-14T11:17:09.8946858Z | 0 / S-1-5-18 | 5 | UNKNOWN(참조 미확인) |
| 5000 | MpDefenderCoreService.exe | 2026-10-14T11:17:10.2103532Z | 0 / S-1-5-18 | 3 | UNKNOWN(참조 미확인) |
| 5128 | ASDSvc.exe | 2026-10-14T11:17:10.2171645Z | 0 / S-1-5-18 | 3 | UNKNOWN(참조 미확인) |
| 10376 | NgcIso.exe | 2026-10-14T11:17:21.1754762Z | 0 / S-1-5-19 | 4294967294 | UNKNOWN(참조 미확인) |
| 19404 | svchost.exe | 2026-10-14T11:17:37.2732033Z | 0 / S-1-5-19 | 2 | UNKNOWN(참조 미확인) |
| 4912 | SecurityHealthService.exe | 2026-10-14T11:17:42.1175418Z | 0 / S-1-5-18 | 2 | UNKNOWN(참조 미확인) |
| 28124 | nossvc.exe | 2026-10-15T07:51:49.8172439Z | 0 / S-1-5-18 | 4294967294 | UNKNOWN(참조 미확인) |
| 32660 |  | 2026-10-24T13:10:29.7810553Z |  /  | 0 | PROCESS_EXITED(현재 조회 객체) |
위 이름은 Windows 조회값이며 파일 서명·제품 정체 인증이나 안전 판정이 아닙니다. 특히 ASDSvc.exe/nossvc.exe의 공급업체·제품을 이름만으로 추측하지 않습니다. 전체 계정명·경로·정밀 생성시각은 비밀정보 없는 원본JSON의 허용 필드에 보존돼 있습니다. 원시 명령행은 없습니다.

## 보호 수준과 실패 근거

현재 살아 있는15개 중 API가 NONE 이외의 보호수준을 반환한 것은12개, NONE(4294967294)은3개(LsaIso/NgcIso/nossvc)입니다. 원시0을 비보호/false로 해석하지 않습니다. 조회한 보호수준은 무참조·Windows 정식 파일 인증을 의미하지 않습니다. `protected_windows_system=UNKNOWN`은 그대로 유지합니다.

공식 구조 문서: https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/ns-processthreadsapi-process_protection_level_information . 보호수준의 NONE 및 다른 수준 구분과 공급업체 진위를 별개로 다룹니다.

- PID288: native.image_error=0이지만 이미지 실패 분기 및 경로null입니다. **실패 시 유효한 오류 코드가 보존되지 않은 모순/진단 한계**로 기록하며 성공/무오류로 판정하지 않습니다. 과거 실패 원인도 아닙니다.
- PID328/3360/32660: native.image_error=31. 이미지 조회 실패의 실제 저장 숫자이며 보호 때문이라고 단정하지 않습니다.
- 나머지12개 이미지 성공. GetOwner/GetOwnerSid 반환0(성공), native와CIM 생성시각 비교15개 true. 이는 **이번 조회 내** 일치이며 과거 Cleanup과 동일성 비교가 아닙니다.
- 관리자 결과에는 OpenProcess 접근거부 오류가 없습니다. 이전 Codex 비관리자16개 ACCESS_DENIED와 이번 결과를 혼합하지 않습니다.

## PID32660의 종료 판정 — 원본 분류 보완

조회 핸들의 생성시각 **2026-10-24T13:10:29.7810553Z**, 종료시각 **2026-10-26T16:02:17.2555524Z**(2026-10-27 01:02:17 KST), CIM NO_INSTANCE_DURING_QUERY입니다. 이 **조회 객체**에는 PROCESS_EXITED 판정 근거가 있습니다. 다만 이름/소유자/Session/이미지/명령줄은 복구되지 않았습니다.

진단기 schema1은 OpenProcess87+CIM부재 조합만 PROCESS_EXITED로 분류하여 이 행의 current_status는 UNKNOWN으로 남겼습니다. 원본 JSON을 수정하지 않고 문서에서 `현재 조회 객체 PROCESS_EXITED / 과거 대상 UNKNOWN`으로 해석을 보완했습니다. 종료시각이 있다고 과거 PID32660과 같은 프로세스였다고 단정하지 않습니다. 이번에 진단기 수정·재실행은 하지 않았습니다.

## 미결과 다음 조건

과거 원본은 생성시각·개별 원인 미저장, PID3개는 First16으로 잘려 **과거19개 완전 복원 불가**입니다. 현재15개 이름 확인과 종료 객체1개 확인으로 이 결손이 해소되지는 않습니다. 현재 관측의 이미지/소유자 문자열 비일치는 명령행·환경리다이렉트·junction·간접 파일 접근까지 증명하지 않습니다. 따라서 **과거19개 참조 판정은 UNKNOWN19**, 현재 관측은 UNKNOWN15/PROCESS_EXITED1/식별자누락UNKNOWN3으로 구분합니다.

**동일 관리자 명령 재실행은 권하지 않습니다.** 관리자 권한에서도 명령행이 제공되지 않은 현재 결과를 보존합니다. 이미 존재하는 당시 비밀 없는 PID/생성시각 증거와 대응이 가능하면 먼저 검토합니다. 없으면 과거대상복구불가를 명시하고, 필요한 경우 별도 승인된 현재 미확인대상 읽기 전용 조사 범위/자산 사용 검증 계획을 수립해야 합니다. 기존282/792/334 전수검사 자동반복0, 서비스 시작/디버그/ACL완화/보호우회/메모리·토큰·환경 수집0입니다.

부분 정리 재개는 관련참조와 현재상태를 안전하게 해소할 증거 및 원본보호·범위검토 후 **바울 대표 후속 명시승인**이 필요합니다. 새 LSA 설치는 여전히 별도승인 대상입니다. 이번은 판독 후 HOLD 중지입니다.

## 검증·보존·Git

원본JSON SHA·schema/행19/보존PID16/누락3·집계·시간·명령줄공백/참조값과 종료시각 교차검토, git diff --check. 기존 성공시험·진단 전체 재실행0. 원본JSON은 바이트 그대로 추가보존, 진단코드 변경0. 파일·계정·Task삭제/설치/IPC/Fake/AI/usage/Discord/A/운영봇·아트·OnBoot/Unity·Save/Push/main/PC재부팅0.

branch codex/discord-usage-diagnostics-20261026, 선행진단80776f95df9b77b2f8efab1c8b29b48630c0b99c, 선행마감bcafbb2f0b262295e21e525c8ddcb7d502fa99c6. 이번 실제 판독 commit은 Issue66 마감댓글에 기록합니다. LOCAL_COMMITTED / REMOTE_NOT_PUSHED.