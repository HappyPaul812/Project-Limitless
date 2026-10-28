# Discord V2 기존 Community Modify 최종 검토·인계 — 2026-10-28

기준 `a813901eae26f465df34b0b64dc3dbd5d7b7fdf3` / Issue66 댓글6460067324. **최종 판정 NO_GO / MANUAL / HOLD**. 사용자 조건 전부를 만족한다고 입증하지 못해 추가 설치 경로 개발 없이 중지합니다.

## 읽기 전용 확인 결과

등록된 설치 Manifest에서 product `Microsoft.VisualStudio.Product.Community`, installationVersion18.8.12023.21, productDisplayVersion18.8.2, Stable/`VisualStudio.18.Release`, x64를 확인했습니다. 기존 설치는 E드라이브, 공유 도구도 E드라이브입니다. 등록 다운로드 캐시는 **F드라이브**로 확인됐습니다. 기본 C의 instance 폴더가 없다는 결과를 설치 부재로 단정하지 않고 등록 CachePath를 따라 실제 instance 기록을 읽었습니다. 정확 경로·instance 식별·Manifest5개 크기/SHA는 Git 제외 로컬 관측 JSON에 보존했습니다.

기존 Installer 버전은4.8.60.302448642 / installerVersion4.8.60.55523입니다. 설치 Manifest requiredEngineVersion도4.8.60.55523으로 **현재 보존 Manifest와 engine 요구가 일치**합니다. 따라서 자체업데이트가 이미 필수라고 단정하지 않습니다. 실제 Modify에서 새 메타데이터나 engine을 요구할지는 실행하지 않아 미확정입니다.

선택 기록24개, 설치 패키지 기록568개에서 대상 두 root ID의 직접 일치 항목은 없습니다. 일부 C++ 공용 유틸리티는 기록되어 있어 C++ 관련 파일 전체가 없다는 뜻은 아닙니다. 기존 includeRecommended 설정은1로 기록되어 있으며 변경하지 않았습니다.

## 공식 Modify 적용 범위

[공식 명령 문서](https://learn.microsoft.com/en-us/visualstudio/install/use-command-line-parameters-to-install-visual-studio)의 기존 Installer `modify`는 제품 업데이트 명령과 구분되며 설치 경로·채널을 지정하고 두 `--add`로 VC.Tools.x86.x64 및 Windows11SDK.26100을 선택할 수 있습니다. 필수 의존성이 함께 추가되므로 실제 변경 패키지가 두 개뿐이라고 보장하지 않습니다. 기존 Installer에는 Bootstrapper 전용 `--wait`를 적용하지 않습니다.

`--quiet --noUpdateInstaller`는 자체업데이트가 필요하면 실패하도록 하는 조건이며 제품18.8.2를 고정하는 옵션은 아닙니다. [공식 Modify 안내](https://learn.microsoft.com/en-us/visualstudio/install/modify-visual-studio)는 Installer 업데이트 요구 가능성을 명시합니다. 현재 engine 일치와 Modify 구문 지원은 긍정적 근거지만 실제 트랜잭션의 제품 버전·설정·패키지 변화까지 입증하지는 못했습니다. 업데이트가 반드시 발생한다고 단정하지도 않습니다. 제품 또는 Installer 업데이트가 강제되면 계속하지 않고 NO_GO입니다.

## 저장 공간과 최종 조건

현재 여유 C30,895,202,304bytes / E915,452,080,128bytes입니다. E임시자료를 계획하더라도 기존 등록 F다운로드 캐시가 자동으로 E로 바뀌지 않습니다. [공식 설치 위치 문서](https://learn.microsoft.com/en-us/visualstudio/install/change-installation-locations)는 cache/shared 경로 변경 제한과 일부 system drive 설치를 설명합니다. 기존 설정·registry·경로를 바꿔 우회하지 않습니다.

C쓰기 후보는 Installer/ProgramData 상태, Windows Installer cache·rollback, PKI/로그, 구성별 시스템/SDK 고정 위치입니다. **추가 필수량과 peak는 UNKNOWN**입니다. 과거 전체 설치 모델의 약1.139GB 다운로드나3.617GB 설치량은 기존 설치의 Modify delta가 아니며 C추가량으로 쓰지 않습니다. SDK목적지·이미 설치된 공유분·실제 의존성·임시/rollback을 해결하지 못해 확정 용량을 제시할 수 없습니다.

| 요구 조건 | 판정 |
|---|---|
| Community18.8.2 유지 | 현재 값 확인, 실제 Modify 후 불변은 미입증 |
| 기존 VS 설정 보존 | 이번 변경0, Modify 후 전체 불변은 미검증 |
| 필요한 두 구성 요소만 추가 | 두 root 선택 지원, 필수 의존성·실제 변경 범위 미확정 |
| C추가 사용량 사전 확정 | 불충족 / UNKNOWN |
| 사용자 승인 후 실제 설치 | 준수, 이번 실행·설치0 |

모든 조건 충족이 필요한데 C용량 사전 확정부터 실패하므로 **GO를 발급하지 않습니다**. 다른 설치 경로·새 검증기·실행 명령·Catalog 수신을 추가하지 않습니다.

## 최종 인계

담당 Codex. 현재 조사 단계는 종료하며, 작업은 실행 승인 대기보다 **미충족 조건으로 HOLD** 상태입니다.

- NuGet7 PASS, Bootstrapper 공식 신뢰 결과, Catalog 두 수신물·HTTP/hash 실패 근거와 원본 로그를 보존합니다. 과거 PASS는 반복하지 않았습니다. 정확 보존 위치와 instance 관측은 로컬 보호 문서에 기록했습니다.
- native 실행 이미지2개 미확보, NativeAOT 도구 공급/빌드·실제 Session0/Batch·90초Job·단일프로세스·실패/취소 정리 검증은 미완입니다. 공급/서명 PASS를 실제 실행 PASS로 확대하지 않습니다.
- OS와 기존 VS에 변경을 가하지 않아 되돌릴 설치 트랜잭션은 없습니다. Manifest hash 기록은 시스템 전체 백업이나 자동 복구 보증이 아닙니다. 임의 rollback/repair/삭제를 하지 않습니다.
- 다음 담당은 이 보고·CURRENT_STATUS·CODEX_LATEST·Issue66을 먼저 확인하고 반복 검증·설치를 시작하지 않습니다. 별도 대표 지시가 있을 때만 Microsoft 지원에 버전 보존 Modify와 C추가량 사전 산정 가능 여부를 전달할 자료로 사용합니다. 이번 외부 문의 발송0입니다.
- 2026-11-01까지 별도 승인과 실동작 검증이 성립하지 않으면 추가 재시도·자동 연장 없이 증거·준비본·인계를 보존하고 게임 개발로 복귀합니다. 아직 마감 도래나 복귀 완료라고 주장하지 않습니다.

Installer/Bootstrapper 실행·Modify·Layout·다운로드·VS/SDK 설치·업데이트·Restore/Publish0. 환경변수/registry/기존 경로·계정/Task/OnBoot/Unity/Save 변경0. Push/main 병합0. 브랜치 `codex/discord-usage-diagnostics-20261026`, 문서만 로컬 커밋 / `REMOTE_NOT_PUSHED`. 최종 SHA는 완료 보고와 Issue66 댓글에 기록합니다.
