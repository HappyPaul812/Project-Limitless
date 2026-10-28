# Supervisor 핵심 구현 중간 보존 — 2026-10-28

기준6f9e81cd/Issue66최신6460822429. 코덱스격리브랜치에서기존Supervisor프로젝트를재사용했습니다. **WIP / NO_GO / MANUAL / HOLD**. 5시간사용률98%를도구로확인해사용자지시대로안전중지하며재설정권사용0.

구현코드는 Tools/DiscordV2/NativeBatch/Supervisor 아래Program.cs·AdmissionPolicy.cs·ReleaseAdmission.cs·AdmissionWindows.cs·HandleCensus.cs 및기존프로젝트연결용Supervisor.Core.projitems에보존합니다. 보호경로/SID가있는기존Shared.cs와csproj는기존Git제외준비폴더에유지하고csproj에서projitems를Import하도록변경했습니다. 원Program백업도동일로컬준비폴더에보존합니다. 원Temp Program.cs는현재컴파일입력이아니며tracked Program.cs가컴파일입력입니다.

실제구현추가: 계약필드중복/누락·승인/만료/SHA검사, 신원/그룹/위험특권검사, 경로reparse/finalpath/파일handle pinning·DACL정적검사+AccessCheck, process/thread/token/job보호DACL과noninherit handle, CREATE_NEW+flush+영구marker/owner-rights보호, 중복소비거부, 외부handle관측·동일SID process독점검사, child생성보호descriptor와resume전재검사연결. ACL변경API는프로그램내실행경로구현이며이번실제로호출하지않았습니다.

**완료아님:** ReleaseAdmissionImplemented=false유지, 실제경로전에HOLD_NATIVE_ADMISSION_VALIDATION_PENDING으로차단. 단순true전환0. Windows보안경계는아직단위/실Windows검증미완료이며실행안전PASS아님. NtQuerySystemInformation class64는공개안정ABI가아니며주소redaction/관측실패는HOLD; 보호process접근불가도UNKNOWN/HOLD. 해당방식의지원성·실행가능성은남은차단입니다. 보수적외부HANDLE거부가Scheduler정상handle까지거부할수있으며임의예외허용0.

일반컴파일: Supervisor 기존 프로젝트 --no-restore/PublishAot=false/UseSharedCompilation=false, 현재기본SDK10.0.403에서오류0/경고0. NativeAOT빌드0·프로그램실행0. 첫호출은사용자기본CLI홈sentinel쓰기권한거부로컴파일전실패했고, 기존격리CLI홈을지정한호출에서컴파일성공했습니다. 테스트미작성/미실행(5시간안전중지). 기존PASS재실행0.

**부수효과관측:** SDK10.0.403최초사용출력에'ASP.NET Core HTTPS 개발 인증서를 설치했습니다'메시지가발생했습니다. --trust나인증서설치명령을명시적으로실행한것은아니지만SDK의자동동작입니다. 실제인증서신규여부/저장소추가상태는이번중지로확인하지않았고임의삭제0. 환경변경전체0이라고주장하지않습니다. 후속dotnet호출시DOTNET_GENERATE_ASPNET_CERTIFICATE=false/DOTNET_ADD_GLOBAL_TOOLS_TO_PATH=false등최초사용부수효과차단환경을먼저확정해야합니다.

다음구현: (1) 이번실제정책/파서/원자소비를직접테스트하는최소단위테스트추가및보안코드검토·오류수정 (2) HANDLE관측지원성/독점·token권리/owner-rights/교체차단실경계검증준비, gate유지 (3) 관리자등록·시작·정리CLI미구현명시, 후속별도작업. 테스트/검토후에만별도NativeAOT빌드요청단계로진행. 이번진단Probe프로젝트는변경하지않았습니다.

Task/계정/OnBoot/Session0/Job/IPC/AI/Unity/Save운영변경0, 추가다운로드/도구설치명령0, Push/main병합0. 2026-11-01마감유지. branch codex/discord-usage-diagnostics-20261026, 소스/보고localcommit/REMOTE_NOT_PUSHED. 최종SHA완료보고.
