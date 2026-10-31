# Discord V2 보안 수정 Supervisor 단발 NativeAOT 빌드

2026-10-31. **BUILD PASS / NO_GO / MANUAL / EXECUTION_HOLD**. 실행·배포 승인이 아닙니다.

## 기준과 변경 범위

- Issue #66 최신 보고와 시작 HEAD `3c481bc462c69ce613d2c1c2d81b952e38757d26` 확인.
- 브랜치 `codex/discord-v2-supervisor-fixed-build-20261031`. 보안 수정 코드 `1717e443c9dfe89f2e82e7897d657dc99c7a0fa7`와 Supervisor 소스 diff 0. 이번 변경은 보고·증거 문서뿐입니다.
- 기존 표적 **34 PASS / 0 FAIL** 기록 보존. 기존 58 및 과거 시험·표적 34 재실행 0.
- 전역 `ReleaseAdmissionImplemented=false`, Main과 실제 Windows 배포 backend의 명시 HOLD 가드 보존. 새 이미지도 미승인 TrialId로 실행 불가합니다.
- 원본 main HEAD `ab47b931fec92976170f9f50aabde1822d78bb90` 유지. Unity·Save·Packages·BuildSettings 접근/수정 없음.

## 입력과 단발 발행

격리 루트: `E:\LimitlessBuildDependencies\NativeAOT20261028\SupervisorExpiryFix20261031-01748fd023c8406b9409e5aeaef24e1d`.

SDK 10.0.403 고정, 기존 MSVC/Windows SDK 사용. 공식 NuGet 7종 10.0.10의 기존 서명검증 결과와 현재 SHA256/크기 일치 확인. 격리 offline feed 외 restore source 없음, compiler/linker/runtime 10.0.10 고정. 프로젝트·전역 설치·기존 빌드 루트는 변경하지 않았습니다.

MSBuild 정적 평가 PASS: net10.0-windows / PublishAot=true / RuntimeFrameworkVersion=10.0.10 / TargetLatestRuntimePatch=false, compiler/linker 10.0.10. 과거 MSB4191을 일으킨 metadata 조건식을 사용하지 않았습니다.

Supervisor 소스 19개·projitems·프로젝트·비공개 binding 총 **22개** 복제 SHA 및 발행 후 원본/복제 SHA 일치. 보안 계약/승인/Task 경계 구현도 이 입력에 포함됩니다. 실제 실행용 계약·승인·소비 기록 생성/재사용/초기화 0. [공개 입력 SHA](Evidence/DiscordV2SupervisorFixedBuild20261031/BuildInputs.json)에는 비공개 binding을 제외했습니다. 비공개 전체 manifest SHA256: `21F6AC50E6C41457962D93E4B410AA39F6DFDF0BD572894734240CB420020308`.

CreateNew/Flush(true) 단발 marker 기록 후 14:48:38 KST 발행 시작. **Supervisor publish 1회, Probe publish 0회, 재시도 0회**. 발행 완료 후 결과 JSON 스크립트의 `true`/`$true` 표기 오류로 wrapper가 종료됐습니다. publish 종료코드는 파일로 보존되지 않았습니다. 재빌드 없이 별도 읽기 검증으로 완료 로그·생성 PE·SHA·경고 및 입력 보존을 확인했습니다. BUILD PASS는 이 증거에 근거하며 wrapper 무오류를 뜻하지 않습니다.

## 생성 이미지와 기존 이미지 보존

| 이미지 | SHA256 | 바이트 | 결과 |
|---|---|---:|---|
| 새 Supervisor | `34D2B90A0EDE2BBA1BB8A848A80F30890B80ABB7463EE0118A35395B8BA5AB16` | 3,172,352 | BUILD PASS |
| 기존 Probe | `0BABC05A2E0B24B1E6563D049B8FAABD7375F737B2ACD5AEDEAF840FC5E59322` | 1,892,352 | 보존 PASS |
| 기존 Supervisor | `CCA8202417DD4D3A1E4F0D301143E9960EEFC3B984EAC79FC92DC6FA08974ECD` | 7,536,640 | 보존 PASS |

새 파일: 격리 루트의 `artifacts/Supervisor/Supervisor.exe`. MZ/PE signature, **PE32+ AMD64 0x8664 / optional 0x20b / CLR directory 0** 확인. 발행 로그 경고 **0**, IL2091 **0**, 오류 **0**. 기존 실행 가드가 유지된 이미지이므로 크기 차이·경고 0을 실행 기능 검증으로 해석하지 않습니다. 새/기존 EXE 실행·이동·배포 0, 기존 이미지 수정 0.

## V3 관측과 한계

V3 엔진 **2026.10.31.01**로 공식 제외 예정 엔진 2026.10.30.00 이후임을 읽기 확인. 새 이미지 10초 간격 및 후속 SHA 재확인 일치, 삭제·변조 관측 없음. quarantine DB는 메타데이터만 확인: 12,288바이트, 마지막 쓰기 10-28 18:00:50. 변경 없음이 무탐지를 입증하지 않습니다. 과거 문의 #745042 판정은 당시 제출 샘플에 관한 것으로, **새 SHA의 공식 정상 판정/실시간 진단 결과는 NOT_VERIFIED**입니다. 백신 UI 조작·검사/예외·복원·비활성화·payload 조회 없음.

## 남은 차단 조건

1. 바울 대표의 새 이미지 SHA에 대한 명시적 승인과 새 이미지 신뢰 근거/V3 확인 필요. 이전 샘플 판정 자동 승계 금지.
2. 보호 CodeRoot/EvidenceRoot 생성·소유권/비상속 DACL·부모 교체방지·이미지 복제/readback은 별도 관리자 승인 후에만 가능합니다. 현재 수행 0.
3. 현 이미지의 명시 실행 HOLD는 유지됩니다. 이를 연결/변경하는 후속 코드·검증·빌드/새 SHA 승인은 별도이며, 이번 빌드 승인을 재사용할 수 없습니다.
4. 정확한 새 TrialId/FAKE_ONLY_EXECUTION_V1, 새 SHA/계정/보호 경로/신뢰자료 및 Register·Run·안전 정리 등 동작별 신규 승인이 필요합니다. 계정의 다른 사용/자격증명/보호자산 관리 및 Windows 전체 동일 SID 부재는 여전히 UNKNOWN입니다.
5. 실제 Batch4·Session0·Job90초·자식제한·실패정리/Task 생명주기는 NOT_VERIFIED. 실제 시험은 별도 승인 대상입니다. 실제 Task/계정/암호/ACL/OnBoot 변경, IPC·AI·Discord 업무 실행 모두 0.
6. cutoff **2026-11-01 23:59:59 KST**, 계정 만료 기존 확인값 **2026-11-02 00:00:00 KST** 유지. 신규 실행 승인/증거의 유효성은 실행 직전 다시 확인해야 하며 자동 연장·기존 소비 초기화는 금지합니다.

보고 후 중지합니다. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. GitHub Issue 댓글은 별도 실제 보고하며 Git push·main 병합·원본 동기화는 하지 않습니다.

증거: [Summary](Evidence/DiscordV2SupervisorFixedBuild20261031/Summary.json), [발행 로그](Evidence/DiscordV2SupervisorFixedBuild20261031/Publish.log), [정적 평가](Evidence/DiscordV2SupervisorFixedBuild20261031/StaticEvaluation.json), [빌드 설정 SHA](Evidence/DiscordV2SupervisorFixedBuild20261031/BuildSettingsSHA.json). 바이너리·비공개 binding·자격증명은 Git에 포함하지 않습니다.
