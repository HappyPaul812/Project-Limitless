# Discord V2 Bootstrap 단발 NativeAOT 빌드·토요일 마감 인계

2026-10-31. **BUILD PASS / STATIC_NATIVE_INCLUSION_PASS / NO_GO / MANUAL / EXECUTION_HOLD**.

기준 Issue #66 최신 요약 및 로컬 HEAD `48d3acc8d09688bdec0c3af28af4946edb29c5a0`. 소스 코드 `5a105bcadedd8f75dfe0c864def4f42dfc921ecf` 선행/제품 소스 diff0 확인. 브랜치 `codex/discord-v2-bootstrap-native-build-20261031`. **제품 코드·cutoff·binding을 이번에 변경하지 않았습니다.**

## 단발 발행 결과

| 항목 | 결과 |
|---|---|
| Bootstrap NativeAOT publish | **1회 / exit0 / 재시도0 / BUILD PASS** |
| 파일 | `Limitless.ProtectedBootstrap.exe` |
| SHA256 | **`2FF5669350A6761A7EB979F6133A4B0A85B44F5440F96A05644A57601026349E`** |
| 크기 | **2,951,168바이트** |
| PE | PE32+ AMD64 0x8664 / optional 0x20b / CLR directory 없음 |
| 경고·IL2091 | **0건 / 0건** |
| 실제 제품 실행·보호 폴더 생성·ACL 적용 | **모두 0회** |

출력은 기존 E 격리 build base의 새 루트 `E:\LimitlessBuildDependencies\NativeAOT20261028\BootstrapNative20261031-e9e95a17a3a0412db3b3a21aefa62d50\artifacts\Bootstrap\Limitless.ProtectedBootstrap.exe`에 보존했습니다. 실제 C 배포 경로에는 복사하지 않았습니다.

기존 공식 NuGet 7종 10.0.10의 현재 SHA/크기·기존 공식 서명 검증 기록을 대조했습니다. SDK 10.0.403·기존 MSVC/Windows SDK를 사용했고 설치/업데이트는 없습니다. 별도 E packages/cache/temp/home/obj/bin/output 및 offline feed만 사용했습니다. MSBuild 정적 평가로 StartupObject `ProtectedBootstrapProgram`, AssemblyName `Limitless.ProtectedBootstrap`, net10.0-windows / win-x64 / PublishAot=true, runtime·ILCompiler·ILLink10.0.10 고정을 확인했습니다.

제품 csproj와 C# 3개 **총 4개 입력**만 복제했고 봉인 SHA 및 발행 후 원본/복제 SHA 일치입니다. 원본 59 PASS/0 FAIL과 최종 코드/입력 SHA를 확인해 보존했으며 시험을 반복하지 않았습니다. 고정 invocation marker와 CreateNew/flush 발행 marker 후 단 한 번 publish했습니다. 실패 재시도/옵션 우회/소스 자동 수정은 없었습니다. 빌드 전 10월 31일 작업 가능 시간과 기존 cutoff도 검사했습니다.

## 최종 이미지의 코드 포함 정적 확인

dumpbin COFF 심볼·NativeAOT 역어셈블을 읽고 `.managedcode$I`의 함수 시작 코드(최대 96바이트, 짧은 함수는 실제 경계 길이)를 최종 PE의 실행 가능한 `.text`와 유일 일치 대조했습니다. 재배치 대상 바이트만 마스킹했으며 **35개 함수/Native wrapper 포함 PASS**입니다.

- 전용 Main/인자 검사, strict 승인 Parse/Authorization.
- 관리자 역할·TOKEN_ELEVATION·thread token 비위장 검사와 관련 API wrapper.
- 생성 gate/현재 시각/부모·대상 검사, 생성 후 Accept/Finish/VerifyNew.
- CreateProtected 및 SECURITY_ATTRIBUTES를 받는 CreateDirectoryW wrapper.
- owner/DACL AdministrativeSecurity, Security/Read/PinAndRead·최종 경로·file ID/volume·HANDLE readback wrapper.

최종 PE의 실제 상대 call/jmp를 해석한 **핵심 10개 분기 PASS**: Main→Controller, Controller→Validate, CreateProtected→BeforeNativeCreate 및 CreateDirectoryW, VerifyNew→AdministrativeSecurity, Accept→VerifyNew, Read→Security, 관리자 검사→OpenThreadToken/OpenProcessToken/GetTokenInformation입니다.

CreateDirectoryW 실패 시 Win32Exception/throw 분기와 Main의 HOLD 안내 literal도 최종 이미지에서 확인했습니다. Main은 Controller로 이어지며 예전 작은 Supervisor의 무조건 throw/필수 경로 제거 문제를 보이지 않습니다.

이는 **코드 포함과 정적 분기 확인**입니다. 전체 함수의 의미적 동작, 실제 Windows API/관리자 권한/owner·DACL·HANDLE readback, 보호 디렉터리 안전 생성은 **NOT_VERIFIED**입니다. Native wrapper를 실행한 결과나 폴더 생성 PASS로 보고하지 않습니다.

## V3 관측

설치 엔진은 **2026.10.31.03**입니다. 생성 직후·10초 후·후속 SHA가 일치하며 새 파일 삭제·변조는 관측되지 않았습니다. quarantine DB는 메타데이터만 확인했으며 12,288바이트/10-28 18:00:50.717 유지입니다. 이를 무탐지/격리 부재의 증명으로 사용하지 않습니다.

**새 Bootstrap 이미지의 공식 정상 판정·V3 실제 진단 결과는 NOT_VERIFIED**입니다. 과거 Probe 제출 판정은 이 새 파일에 자동 승계하지 않습니다. 수동 검사·샘플 실행·백신 예외/종료/복원/우회는 하지 않았습니다.

## 실제 보호 폴더 생성에 남은 승인·차단 조건

1. 새 Bootstrap 이미지 SHA/신뢰·V3 확인과 운영자 검토가 필요합니다.
2. 생성 대상 밖의 **기존 관리자 전용 보호 staging** 및 도구/승인 파일의 안전 봉인 배치를 확정해야 합니다. 현재 E build 원본을 바로 관리자 실행할 수 있다는 뜻이 아닙니다. 이 staging 위치·파일 배치는 미완료/미승인입니다.
3. 정확한 Bootstrap 승인 ID·도구 SHA·관리자 SID·owner BA/SYSTEM·고정 6개 경로·C루트/ProgramData의 최신 descriptor SHA/file key·기한을 결합한 보호 승인 자료가 필요합니다. 실제 승인 파일 발급/소비와 운영 원장 binding은 수행하지 않았습니다.
4. 명시적으로 승인된 관리자 컨텍스트에서만 `--review`와 별도 실제 `--create` 단계를 실행해야 합니다. 실제 API 실행/readback·부모 SDDL 무변경·기존 프로그램 영향은 아직 미검증입니다. 기존 대상/권한/조회 불명/경합이면 HOLD하며 기존 자료를 고치지 않습니다.
5. 실제 C 대상 6개 디렉터리 생성·검증, 독립 배포 도구의 실제 빌드/배포·원장 binding·승인 소비, Probe/Supervisor 배포, Task 및 FAKE_ONLY 단발 시험은 모두 미완료·별도 승인 대상입니다. Bootstrap 빌드 성공으로 이 단계를 자동 진행하지 않습니다.

## 기존 자산 보존

현재 Probe(1,892,352바이트 / `0BABC05A2E0B24B1E6563D049B8FAABD7375F737B2ACD5AEDEAF840FC5E59322`)와 Supervisor(7,559,680바이트 / `6DF94E47C301C20EAB537EB6AD49C680129B80787536358E6BDC78ED50A842E7`) 보존을 확인했습니다. 이전 7,536,640/3,172,352바이트 Supervisor도 원위치 SHA/크기 불변입니다. 기존 Native binding·제품 소스도 불변입니다. Probe·Supervisor 빌드/실행/복사 0회입니다.

실제 Windows 보호 폴더/ACL·계정/암호/Task/OnBoot/V3·Unity/Save 변경 0, 기존 승인/소비 기록 재사용·삭제·초기화 0입니다. 원본 main `ab47b931fec92976170f9f50aabde1822d78bb90` 유지. Push/main 병합은 하지 않았습니다.

## 토요일 운영 규칙과 11월 7일 재개 순서

- Discord 개발은 **토요일만** 진행합니다. 이번 2026-10-31 작업 이후 미완료는 NO_GO/HOLD로 보존합니다.
- **2026-11-02부터 평일 Limitless 게임 개발로 복귀**합니다. Discord 다음 작업일은 **2026-11-07 토요일**입니다. 예약 작업/자동 재개는 설정하지 않았습니다.
- 기존 cutoff **2026-11-01 23:59:59 KST**는 상한일 뿐, 11월 1일 Discord 작업을 자동 허가하지 않습니다. 계정 만료의 기존 인계값은 **2026-11-02 00:00 KST**이며 이번에는 재조회/변경하지 않았습니다.
- **11월 7일에는 현재 Bootstrap/Supervisor의 코드 cutoff가 이미 만료됩니다. 오늘 빌드한 SHA를 그대로 실행 가능 상태로 재사용하면 안 됩니다.** 새 운영 지시/기한·계정/신뢰 검토 없이 날짜를 자동 연장하거나 옛 승인을 사용하지 않습니다.

재개 순서:

1. 최신 Issue/인계·브랜치/소스·기존 이미지 SHA·소비 기록을 읽기 확인합니다. 만료 상태와 기존 HOLD를 먼저 확인하고 실행하지 않습니다.
2. 바울 대표가 새 작업 범위/새 시간 창·계정/신뢰 조건을 명시 결정해야 합니다. 계정 만료 연장·기존 승인 재사용은 자동 조치가 아닙니다.
3. 승인된 새 cutoff를 코드에 반영해야 한다면 Bootstrap 및 영향 Supervisor/배포 도구의 표적 검증→각각 별도 빌드 승인/새 SHA·정적 포함/신뢰 확인이 필요합니다. 오늘 결과는 보존 근거이며 새 이미지 승인을 대신하지 않습니다. Probe도 당시 SID/경로/신뢰 조건으로 재판단합니다.
4. 보호 staging·봉인 배치·새 승인 자료·최신 부모 snapshot/대상 부재와 관리자 실행 경로를 확정합니다.
5. 별도 승인 후 실제 Bootstrap 생성/readback, 이후 또 별도 승인으로 배포/Task/FAKE_ONLY 검증을 진행합니다. 조건 미충족이면 계속 HOLD하며 게임 개발 우선 방침을 깨지 않습니다.

이번 빌드·정적 검증 목표는 완료했으며 잔여 실제 적용은 **NO_GO / MANUAL / EXECUTION_HOLD**로 인계하고 중지합니다.

증거: [Summary](Evidence/DiscordV2BootstrapNativeBuild20261031/Summary.json), [35개 코드/10개 PE 분기](Evidence/DiscordV2BootstrapNativeBuild20261031/NativeInclusion.json), [선택 역어셈블](Evidence/DiscordV2BootstrapNativeBuild20261031/SelectedDisassembly.txt), [publish 로그](Evidence/DiscordV2BootstrapNativeBuild20261031/Publish.log), [MSBuild 평가](Evidence/DiscordV2BootstrapNativeBuild20261031/MSBuildEvaluation.json), [입력 SHA](Evidence/DiscordV2BootstrapNativeBuild20261031/BuildInputs.json). LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 바이너리·비공개 binding·비밀정보 업로드 없음.

### 실제 GitHub 최소 보고 영수증

보고/증거 로컬커밋 `366c8bb1ccaec9c03cf0da0703f6bb3247bd6b24`, 제품코드 `5a105bcadedd8f75dfe0c864def4f42dfc921ecf` 무변경. [Issue #66 비민감 빌드 결과 요약](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6542713992) 실제등록·전체본문재조회일치 확인. 원격에는 단발빌드성공/경고0·정적포함확인/런타임미입증·실적용제품실행없음·상세로컬보존 요약만 게시했습니다. 내부 경로/보안 구조·상세 운영/인계 자료는 로컬에만 보존했습니다.

후속 영수증은 문서뿐입니다. BUILD PASS/1회·최종PE35함수/10분기정적PASS·실Windows/제품EXE실행0·NO_GO/MANUAL/EXECUTION_HOLD·토요일운영/11월2일게임복귀/11월7일재개인계·cutoff자동연장없음·LOCAL_COMMITTED/REMOTE_NOT_PUSHED로 중지합니다.
