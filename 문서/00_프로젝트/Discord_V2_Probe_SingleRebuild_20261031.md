# Discord V2 Probe 추가 단발 NativeAOT 발행 결과

**Probe BUILD PASS / NO_GO / MANUAL / EXECUTION_HOLD / gate=false.** 바울 대표의 추가1회 빌드 승인만 사용했습니다. 생성 EXE 실행0, 실제 FAKE_ONLY 시험 미실시입니다.

## 기준과 준비 설정 검증

- Issue66 최신보고6538682825, 보고 commit `9227601ccc92b32db15e8a935a48bd64e954e8ee`, 영수증 HEAD `29b7a7e21f9744358c650d2aff96707043de5ad7`에서 `codex/discord-v2-probe-single-rebuild-20261031`로 분리했습니다. 구현/IL2091 소스 기준fe5b22e6는 무변경입니다.
- 최초 Probe 실패는 이전 준비targets의 `'%(KnownILCompilerPack.TargetFramework)'=='net10.0'` 조건 때문에 발생한 **MSB4191**이며 컴파일 전 실패입니다. 원래 실패 로그·targets·시작표식은 그대로 보존했습니다.
- 현재 targets는 Supervisor 성공 발행에 사용한 조건 없는 KnownILCompilerPack/KnownILLinkPack10.0.10 고정 구성과 SHA가 일치합니다. 원본 소스·P/Invoke·보안 계약·gate=false를 바꾸거나 경고를 숨기지 않았습니다.
- **실제 컴파일 전** `dotnet msbuild -getProperty/-getItem` 정적 평가exit0을 확인했습니다. TargetFramework=net10.0-windows, PublishAot=true, RuntimeFrameworkVersion=10.0.10, TargetLatestRuntimePatch=false, net10.0의 compiler/linker pack 각10.0.10입니다. 이 평가에는 compile/restore/publish target 실행이 없습니다.
- 기존 source20파일 봉인 SHA 및 공식 NuGet7종의 SHA/크기·기존 서명exit0을 재대조했습니다. 기존 E 격리루트/검증 offline feed/SDK10.0.403·기존MSVC를 사용했고 새 다운로드·설치·전역 설정 변경0입니다.
- V3 엔진2026.10.31.01 및 승인 cutoff2026-11-01 23:59:59KST 이전임을 재확인했습니다. 별도 CreateNew 표식으로 **이번 추가 발행 최대1회**를 강제했습니다. 기존 Probe 실패와 Supervisor 발행 기록을 덮어쓰지 않았습니다.

## 실제 이미지 결과

| 대상 | 결과 | SHA256 | 크기·PE |
|---|---|---|---|
| Probe | **추가1회 PASS**, exit0 | `0BABC05A2E0B24B1E6563D049B8FAABD7375F737B2ACD5AEDEAF840FC5E59322` | **1,892,352bytes**, PE32+ AMD64(0x8664), CLR directory 없음 |
| 기존 Supervisor | **재발행0·변경0**, 전후 SHA/크기 일치 | `CCA8202417DD4D3A1E4F0D301143E9960EEFC3B984EAC79FC92DC6FA08974ECD` | **7,536,640bytes**, 기존 PE32+ AMD64 |

Probe 실제 NativeAOT compile/link/publish 로그에서 **warning0·IL2091 0**입니다. 초기 MSB4191 근거를 지우거나 초기 실패를 PASS로 바꾸지 않았습니다. 이번 승인으로 누적 시도는 과거10/28 Probe1 +10/31 최초실패1 +이번추가1, Supervisor는10/31 성공1이며, 이번에는 Probe추가1/Supervisor0입니다. 기존52/17/Windows7/관리형3 시험 반복0입니다.

실물 루트: `E:/LimitlessBuildDependencies/NativeAOT20261028/NativeResume20261031-7ed1a2ca6f724242a49884cba87f5360`.
Probe는 `artifacts/ProbeAdditional20261031/Probe.exe`, Supervisor는 기존 `artifacts/Supervisor/Supervisor.exe`입니다. SHA·PE 확인은 파일을 데이터로 읽은 정적 검사이며 EXE 실행이 아닙니다.

## V3 관측과 한계

생성직후·10초후·최종13:04:57KST 확인에서 Probe와 Supervisor 파일/SHA/크기가 유지됐습니다. 관측 범위에서 출력삭제/변경이나 새 탐지·격리 통지는 확인하지 못했습니다. **새 두 바이너리의 공식 V3 정상/미탐지 판정은 NOT_VERIFIED**이며 파일 보존 사실과 구분합니다. 기존 공식회신은 제출 샘플에 대한 진단 제외 결정입니다. 새 바이너리 전반의 정상 인증으로 확대하지 않습니다.

V3 설정/예외/검사완화·새 수동검사/cloud분석요청/샘플전송0, 복원된 예전 Probe/제출ZIP 접근0입니다. 새 탐지·격리·삭제를 확인하면 즉시 중지하며 우회하지 않는 정책을 유지합니다. 이번 실제 build는 통과했지만 V3 독자 로그의 미확인 상태를 무진단으로 단정하지 않았습니다.

## 남은 실행 HOLD / FAKE_ONLY 승인 조건

두 이미지 빌드·봉인은 이제 확보됐습니다. **실행 승인·신뢰 근거는 별개**입니다. 기존 FAKE_ONLY 위험 수용 승인을 다시 요구하는 것이 아니라, 비협조 동일SID 코드 부재/자격증명 비공유/다른 실행경로 미사용·보호 자산 관리의 실제 사실 근거, 이미지별 V3 결과/보호 배포 신뢰를 확인해야 합니다. 운영 전역독점성은UNKNOWN입니다.

실제 FAKE_ONLY는 정확 이미지SHA·범위·유효기간에 묶인 새 단발 승인과 Task등록/시작/정리의 각각 별도 허가, Batch4/Session0·제한token HANDLE·Job단일대상/child제한/90초/취소/실패정리 검증이 필요합니다. 이번에는 Task·계정·ACL·OnBoot·Session0·IPC·AI·Unity·Save 변경/실행0, gate=false 유지입니다.

**2026-11-01 마감 이후 기존 승인 재사용·소비초기화·계정/승인 자동연장 금지**, 새 지시가 없으면 중지합니다. 빌드 성공을 gate 개방/실제 실행/운영 전환 승인으로 해석하지 않습니다. GitHub Push/main병합0, **LOCAL_COMMITTED / REMOTE_NOT_PUSHED**로 보고 후 중지합니다.

정확 source/private binding·MSBuild 평가·추가 발행 로그·새 시작표식/retention JSON은 기존 ignored worktree Temp 및 E루트에 로컬 보존합니다. 바이너리/PDB/source/원시로그는 외부 게시하지 않습니다. 정제 결과는 Evidence/DiscordV2ProbeAdditional20261031/Summary.json입니다. 보고 commit `233a64316b37080aa60357068e37e798b7cb788e`, [Issue66 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6538790069) 실제등록·전체본문재조회일치 확인. 후속영수증은문서만변경하며 LOCAL_COMMITTED/REMOTE_NOT_PUSHED·gate=false·실행HOLD를 유지합니다.
