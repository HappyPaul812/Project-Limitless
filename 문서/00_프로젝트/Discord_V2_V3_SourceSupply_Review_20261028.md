# Discord V2 V3 진단 후 읽기 전용 소스·공급망 검토

2026-10-28 / 기준966b3bdf20699fa0a605599d8c2bb0973d5047a4 및 기존 빌드 소스00141490b80c582509271b5f46801ef41a64cc37 / Issue #66. **NO_GO / MANUAL / HOLD 유지.** 재빌드·복원·예외·실행·신고 발송0.

## 1. 원인 판정과 출처

바울 대표가 확인한 공식 V3 진단: Trojan/Win.Overlord.C5916070, 2026-10-28 18:00:50 KST, 실시간 삭제 완료/처리코드1008, 클라우드미확정. 추가로 제공한 V3 화면 텍스트의 대상은 구분자 OCR(통화기호/W/w)을 정상화하면 기존 실패 로그의 **Probe native 중간 exe 경로**와 일치합니다. 최종 발행 폴더나 Supervisor 파일이 아닙니다. 원본 CSV는 확보하지 않았고 사용자 제공 공식 UI 전사라는 출처를 그대로 기록했습니다.

빌드 시작18:00:36, MSB3021 copy source access denied18:00:50, V3 동일 대상·동시각 삭제 및 기존 격리 메타데이터 갱신, 직후 원본/최종exe 부재가 정합적입니다. **발행 실패의 주요 직접 차단 원인은 V3 실시간 삭제로 판단합니다(사용자 제공 공식 로그 근거).** 단순 클라우드 분석 잠금만이었다는 이전 가설은 현 근거와 맞지 않습니다. 다만 syscall trace/원본CSV/샘플SHA가 없어 세부적인 삭제·접근거부 선후와 바이너리 바이트 동일성을 독립 입증한 것은 아닙니다.

V3의 악성 진단 사실과 실제 악성코드 여부는 다른 판정입니다. **악성/오진 최종 분류 UNKNOWN.** 클라우드미확정은 안전 판정이 아닙니다. 처리코드1008의 의미는 대표 제공 V3 로그에 따른 것이며 source의 Windows ERROR_NO_TOKEN(1008)과 번호가 같다고 연관짓지 않습니다. 진단명만으로 실제 원격제어/감염 행위를 역추정하지 않습니다.

## 2. 빌드 입력·공급망 읽기 비교

| 검토 | 결과 | 한계 |
|---|---|---|
| 원본 작업파일·E 빌드 복사본·발행 전 manifest |20/20 SHA256 일치 | 삭제된 exe의 내용·동일성을 대신하지 않음 |
| 그중 Git 추적 입력 |16/16이00141490 commit blob과 일치 | 나머지4개 비공개 binding/Probe source·프로젝트는 로컬 manifest 비교 근거 |
| 공식 NuGet7종10.0.10 | 현재 feed와 빌드 cache nupkg 각각 기존 승인SHA256과 일치, 기존 서명검증 exit0 기록 재사용 | 서명 검증/기존PASS 전체 재실행 아님 |
| 캐시의 빌드·런타임 code payload |exe/dll/targets/props/bat/cmd/obj/lib/h/a/o 대상855개를 공식 nupkg 내부와 SHA 비교, 불일치·미판독0 | 모든 형식/SDK·MSVC·OS 전체와 최종 PE 안전성을 인증한 것은 아님 |
| 복원/빌드 설정 | 로컬 feed1개, runtime/ILCompiler/ILLink10.0.10, 격리targets는 버전·속성 고정만. 새 실행hook 없음 | 원래 설치된 SDK10.0.403/MSVC/Windows SDK 전체의 독립적인 신뢰성 검증은 미수행 |

공식7종: Microsoft.DotNet.ILCompiler, Microsoft.NET.ILLink.Tasks, runtime.win-x64.Microsoft.DotNet.ILCompiler, Microsoft.NETCore.App.Runtime.NativeAOT.win-x64, Microsoft.NETCore.App.Runtime.win-x64, Microsoft.AspNetCore.App.Runtime.win-x64, Microsoft.WindowsDesktop.App.Runtime.win-x64. project.assets의 tool libraries3종과 runtime download 목록은 이 공급 범위와 일치하고 임의 공급원/패키지는 발견하지 못했습니다. 새 다운로드/restore/컴파일/verify 도구 실행0. 입력 hash 일치는 변조 징후 부재의 근거이지 malware-free 인증이 아닙니다.

## 3. 실제 source 동작과 승인 기능 대조

- **Probe의 실제 Main:** 인수0/고정 자기경로 확인→자기 thread token 비위장 확인→자기 primary token/SID/비관리자·Session0/Batch4/LSA 조회→자기 Job 포함 여부 확인→30초 이내 대기/종료. 네트워크·일반 파일 수집·다른 process 생성·권한 부여/지속성 기능의 실행 경로를 발견하지 못했습니다. 소스 주석의 유한 진단 목적과 맞습니다.
- **Probe 컴파일 입력:** Probe Program/비공개 binding/TokenPolicy/NativeInterop. Supervisor의 Task 관리·DACL 변경 어댑터는 Probe 프로젝트에 포함되지 않습니다. 공통 NativeInterop에는 CreateProcess/Job 종료 등의 P/Invoke 선언도 있지만 Probe Main에서 호출하지 않습니다. 삭제 PE가 없어 실제 trimming/import 결과는 확인하지 못했고, 이 선언을 V3 진단 원인으로 단정하지 않습니다.
- **Supervisor:** SHA/계약/관리자 동작승인·신뢰근거 검증, 허용 보호자산/상위경로 읽기와 pin·해시, 고정 증거 CREATE_NEW/영구소비, 실제 token/HANDLE/process 관측, 자기/생성객체 DACL 보호, 고정 Probe suspended 생성·Job 편입·resume·자기 Job 종료·정리입니다. 승인된 설계의 민감 API와 범위를 구현하고 있으나 실제 경로 검증 완료를 주장하지 않습니다. 일반 사용자 문서/브라우저/인증 저장소를 수집하거나 임의 외부 process를 종료·주입하는 경로는 발견하지 못했습니다.
- **관리자 CLI:** 로컬 Task Scheduler COM의 exact Task 등록·단발 시작·정확 instance 중지·삭제 기능이 존재합니다. 암호는 새 Task 등록을 위한 콘솔/SecureString→BSTR 전달 후 zero/free이며 저장 자격증명 추출 기능을 발견하지 못했습니다. 계정/그룹/Batch 권리 API는 읽기이고 계정 생성·권리추가·SeDebug 활성화 기능은 발견하지 못했습니다. Task 등록 자체는 고위험 권한 기능이지만 승인된 설계에 포함되고 source gate=false로 진입 차단됩니다. 이번 Supervisor Publish0회·이미지실행0.
- **통신/실행 검색과 흐름 검토:** 앱 정의 HTTP/socket/pipe/AI/Discord 호출, 다운로드·업데이트 실행, ShellExecute/셸·임의 명령 실행, Assembly.Load/원격 주입/WriteProcessMemory/CreateRemoteThread, 서비스 설치·레지스트리 지속성 경로를 발견하지 못했습니다. Task XML의 http URI는 스키마 namespace이며 접속 코드가 아닙니다. OS의 로컬 COM/LSA 호출은 존재하며 업무 IPC 금지와 구분합니다. source에 명시적 외부통신이 없다는 검토이지 OS/도구/백신의 전체 네트워크 차단 증명은 아닙니다.

**발견 사항:** 현재 검토 범위에서 승인 밖 악성 행위 경로는 발견하지 못했습니다. 자기 token/LSA/native API와 제한된 자체 제작 NativeAOT가 탐지 패턴과 충돌했을 가능성은 있으나 서명규칙이 알려지지 않아 가설입니다. 실제 V3 Trojan 진단은 반대 근거로 그대로 보존합니다. IL2091 constructor 보존 annotation 경고는 별도 구현 위험이며 Trojan 진단의 원인으로 연결할 근거가 없습니다. 이번 source 수정·경고 억제0.

## 4. 안랩 공식 판정 요청과 공개 범위

**공식 오진 가능성 검토 요청을 권고합니다.** [안랩 공식 오진 안내](https://ask.ahnlab.com/hc/ko/articles/900002412366-%EC%A0%95%EC%83%81-%ED%8C%8C%EC%9D%BC%EC%9D%84-V3%EC%97%90%EC%84%9C-%EC%95%85%EC%84%B1%EC%BD%94%EB%93%9C%EB%A1%9C-%EC%98%A4%EC%A7%84%ED%95%A9%EB%8B%88%EB%8B%A4)는 의심 사례를 신고센터에서 분석하도록 안내하고, 진단 기준은 비공개라고 설명합니다. 이번 금지 범위상 검사 예외는 사용하지 않습니다.

우선 제출 후보는 진단명·시각·처리/클라우드상태·제품 및 엔진버전(확인된 경우만), Probe.exe라는 파일명, 경로 정제한 MSB3021 발췌, 실행하지 않고 자체 NativeAOT 빌드 중 삭제된 설명, toolchain/공식패키지 버전과 읽기 검토 요약입니다. 기존 클라우드 분석 항목을 vendor가 특정할 수 있는지 먼저 문의하는 내용의 로컬 초안만 준비했습니다. **샘플SHA는 없으며 source/package hash를 샘플 hash로 제출하면 안 됩니다.**

[공식 신고 절차](https://ask.ahnlab.com/hc/ko/articles/900003237626--%EA%B8%B0%ED%83%80-%EC%9C%84%ED%98%91%EC%A0%95%EB%B3%B4-%EC%8B%A0%EA%B3%A0%EC%84%BC%ED%84%B0-%EC%9D%B4%EC%9A%A9-%EC%95%88%EB%82%B4)는 일반적으로 의심 파일 ZIP 또는 ARC 로그첨부 중 하나를 요구합니다. 현재 파일 부재·복원금지이므로 metadata 문의만으로 정식 접수가 완료된다고 약속하지 않습니다. 샘플/ARC가 필요하면 새 수집·첨부 범위에 대한 대표 검토와 별도 승인 전까지 HOLD합니다. ARC 수집 프로그램 실행도 이번 범위 밖이라 하지 않았습니다.

민감정보: native 이미지/PDB/source/원시로그/ARC에는 비공개SID·보호경로·사용자명·프로젝트/빌드 메타데이터가 포함될 수 있습니다. 고정 binding에는 자격증명 상수가 없었지만 실제 삭제 PE/전송 payload 전체를 보지 못했으므로 자격증명 부재/외부전송 완료를 인증하지 않습니다. 원시 binding/전체 source ZIP/PDB/계정·인증자료/Save를 기본 제출 대상으로 삼지 않습니다. 기존 클라우드 전송 상세도 미확정입니다. 대표의 공개 범위·발송 승인이 없으므로 문의/파일/로그 자동 제출0, 다중백신 업로드0.

## 5. 다음 최소 조치와 중지

원본 V3 진단/클라우드 보고를 로컬 보존하고, 샘플 재생성·복원 없이 기존 항목/로그 기반 안랩 재검토가 가능한지 대표가 승인한 최소 문의 범위를 먼저 결정합니다. 공급망/소스 근거만으로 오진 확정·예외·재빌드로 진행하지 않습니다. 실제 배치 신뢰 근거·Job90초/제한token·동작별 실행 승인은 별도 HOLD입니다.

이번은 읽기 검토와 문서화만 수행했습니다. 백신 종료/예외/복원/샘플실행/재빌드/외부제출/계정ACLTaskOnBoot게임/Push main 변경0. 기존 Probe Publish1/Supervisor0·source gate=false 유지. 브랜치 codex/discord-usage-diagnostics-20261026, 마지막관련966b3bdf20699fa0a605599d8c2bb0973d5047a4;이번 최종SHA 완료보고/Issue66. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 2026-11-01 마감 유지·보고 후 중지.