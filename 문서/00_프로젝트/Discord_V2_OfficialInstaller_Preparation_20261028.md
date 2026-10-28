# Discord V2 공식 Installer 경로 준비 — 2026-10-28

기준 `d7a19985734309ba39af59609bef586e4373ed01` / Issue66 댓글6459952452. **공식 Installer 실행 준비 판정 NO_GO / MANUAL / HOLD**. Bootstrapper 자체 실행·Layout·다운로드·설치는 하지 않았습니다.

## 실제 확인한 신뢰와 버전

기존 Bootstrapper 전체 파일 SHA256 `f26687367d48386ce8400de02d99e264163ce9654c87cf43e24c4b8ec9c11723`, 크기5,690,480bytes 일치. Windows Authenticode=Valid, 설치된 Microsoft SignTool의 Authenticode 정책 검증은 exit0/성공서명1/경고0/오류0입니다. Microsoft Corporation→Code Signing PCA2024→Microsoft Root CA2011 및 Microsoft Time-Stamp Service→Time-Stamp PCA2010→Microsoft Root CA2010 체인이 검증됐습니다. 타임스탬프는2026-07-23 19:40:55 KST(10:40:55Z)입니다. [Microsoft SignTool 문서](https://learn.microsoft.com/en-us/windows/win32/seccrypto/signtool)의 검증 방식을 사용했습니다. 로그의 PE Authenticode digest와 전체 파일 SHA는 계산범위가 다르므로 혼동하지 않습니다.

FileVersion18.8.12023.21은 기존 VS18.8.2 build와 일치하지만 표시 ProductVersion은 **Visual Studio2026**입니다. 이는 SKU·채널·실제 다운로드 제품 버전 고정 증거가 아닙니다. [공식 명령 문서](https://learn.microsoft.com/en-us/visualstudio/install/use-command-line-parameters-to-install-visual-studio)는 일반 연도 ProductVersion의 Bootstrapper가 최신 제품을 선택할 수 있음을 설명합니다. 현재 파일이 공식18.8.2 고정 Bootstrapper라는 근거는 **NOT_PROVEN**입니다.

[공식 Release History](https://learn.microsoft.com/en-us/visualstudio/releases/2026/release-history)에18.8.2 Stable/build12023.21은 있으나 Community용 고정 링크는 제공되지 않고 최신Stable만 지원한다고 명시합니다. 따라서 기존 Community18.8.2 보존 조건에서 제품 고정을 PASS로 볼 수 없습니다. 다른 SKU·새 Bootstrapper·최신 VS로 임의 전환하지 않았습니다.

## 최소 Layout안과 남은 확인

선택안은 VC.Tools.x86.x64 및 Windows11SDK.26100 컴포넌트2개, en-US1개 언어, Recommended/Optional 제외입니다. 전체Workload나 ARM64 제품 Layout은 선택하지 않습니다. 다만 VC의 x86/x64 묶음과 SDK 내부 필수 아키텍처까지 x64 전용이라고 보장하지 않습니다.

과거 모델132node/359payload의 약1.139GB, 메타데이터 포함 참고 약1.176GB를 재사용합니다. 당시 두 언어 모델이며 이번 Installer의 실제 dependency closure·제품 core·engine 범위와 같다는 뜻은 아닙니다. **실제 Installer 의존성·다운로드 용량은 미관측**입니다. E권장 여유10GB는 계획 예산이며 확정 상한이 아닙니다. 새 수동 payload 로직이나 검증기를 만들지 않았습니다.

E내 별도60자 Layout 경로를 정했으나 폴더는 만들지 않았습니다. 관측 여유는 C30,923,726,848bytes / E915,452,080,128bytes입니다. 기존 VS·Installer·SDK 설치 위치를 보존합니다. TEMP/TMP의 E지정은 향후 자식 프로세스에만 적용할 계획입니다. 기존 Program Files 기본값·레지스트리·환경변수·ACL·cache/shared 위치를 변경하지 않습니다.

C의 기존 Installer/ProgramData 메타데이터·PKI cache·로그 쓰기 가능성은 남습니다. **필수 추가량·최대량은 UNKNOWN**이며 이전 예비0.5GB를 확정 상한으로 재사용하지 않습니다. E임시폴더만으로 C쓰기0을 보장할 수 없어 사전 조건은 HOLD입니다.

공식 문서상 최근 Bootstrapper는 최신Installer를 기본 사용합니다. `--quiet --noUpdateInstaller`는 update가 필요하면 실패하도록 하는 옵션이지만 모든 update 조회·engine 수신·C쓰기를 막는다는 보장은 아닙니다. `--noWeb`만으로 update 확인 차단을 가정하지 않습니다. `--keepLayoutVersion`은 기존 Layout 유지용으로 새 Layout의18.8.2 고정을 대체하지 않습니다.

## 준비 명령과 실패 계약

정확한 경로와 관리자 PowerShell7 한 줄 준비안은 Git 제외 로컬 문서에 보존했습니다. 공식 `--layout`·컴포넌트 `--add`·단일언어·quiet/wait/norestart/noUpdateInstaller 조합입니다. **고정 버전과 C예산이 미확정이므로 첫 throw로 차단한 검토용 명령이며 실행 가능 명령을 발급한 상태가 아닙니다.** 현재 파일로 안전한18.8.2 Layout 실행을 보장할 수 없어 조건 해소 뒤 다시 확정해야 합니다.

[공식 Layout 검증](https://learn.microsoft.com/en-us/visualstudio/install/create-a-network-installation-of-visual-studio)의 `--layout … --verify`를 사용할 계획이며 필요한 메타데이터와 오래된 minor release의 검증 제한이 남습니다. exit0만으로 PASS하지 않습니다. nonzero·신뢰/제품/채널/버전 불일치·자체update 요구·C예산 초과는 HOLD, retry/fix0, 실패물·로그 보존입니다. 실행 중 안전취소와 C쓰기 관찰·중단 상한도 다음 실행 승인 전에 확정해야 합니다. 이번 강제종료·정리 삭제는 없습니다.

다음 최소 선행은 Community18.8.2의 공식 고정 경로 근거 또는 Microsoft 지원 확인, 자체Installer 업데이트 금지 계약, C추가쓰기 예산·취소 계약, 실제 의존성과 구버전 verify 지원범위입니다. 어느 하나도 추측으로 PASS 처리하지 않습니다. 그 후 Bootstrapper 실행·E Layout 다운로드·검증만 별도 승인받아야 하며 설치·Restore·Publish는 계속 별도입니다.

NuGet7 PASS·Catalog/HTTP/hash 실패 근거·기존 Bootstrapper 보존. Catalog 수동 재수신0, Bootstrapper 실행0, Layout0, 새 패키지 다운로드0, 설치/Restore/Publish0, 계정/Task/OnBoot/Unity/main 변경0, Push0. 2026-11-01 마감 유지, 준비 후 중지합니다.

브랜치 `codex/discord-usage-diagnostics-20261026`. 로컬 보고/상태/인계만 커밋하고 `LOCAL_COMMITTED / REMOTE_NOT_PUSHED`. 최종 SHA는 완료 보고와 Issue66 댓글에 기록합니다.
