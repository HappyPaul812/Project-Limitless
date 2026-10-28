# Discord V2 E드라이브 공식 공급 결과 — catalog 불일치로 중지

기준5ad31e13698862c629b91fd75630b295cc656260 / Issue66 최신6459162941. 승인 범위는 공식 다운로드·검증·Layout 준비뿐이며 설치·Restore·Publish는 제외했습니다.

**결과: 부분 공급 완료 / FAIL_STOP_HASH_SIZE_MISMATCH_NO_LAYOUT. NO_GO / MANUAL / HOLD 유지.**

## 완료한 공급

- 공식 NuGet7종10.0.10을 E의 별도 staging으로 수신했습니다. 각 고정 ID/버전/URL·크기·공식 SHA512 일치 후 `dotnet nuget verify --all`이 모두 exit0을 반환했습니다. 실제 SHA256을 기록하고 검증 파일만 E의 offline feed로 옮겼습니다.
- NuGet 검증 PASS7개, 합계 **134,765,337bytes**입니다. 파일별 SHA·서명 로그와 정확 경로는 로컬 증거로 보존합니다.
- Microsoft bootstrapper1개 **5,690,480bytes**, 고정 SHA256·크기는 PASS입니다. 다만 catalog 오류가 다음 검증 분기 전에 발생해 bootstrapper 서명 확인은 **NOT_REACHED**이고 실행하지 않았습니다. 전체 검증 PASS로 보고하지 않습니다.

## 중지 원인

고정 공식 URL에서 받은 VisualStudio.vsman의 **SHA256과 크기가 모두 승인 목록과 불일치**했습니다.

| 항목 | 계획 기준 | 실제 수신 |
|---|---|---|
| bytes | 31,191,413 | 18,366,761 |
| SHA256 | 879bcae9…33bea67 | 6e852267…80c54b |

전체 hash·URL·수신 bytes는 로컬 증거에 있습니다. 실제 hash가 기존 로컬 Installer 가공 catalog의 hash와 같다는 사실만으로 고정 기준을 교체하지 않았습니다. 전송·캐시·metadata 원인 중 무엇인지는 미확정이며 손상/공격/정상 변환으로 단정하지 않습니다.

바울 대표의 '해시나 서명 불일치 시 즉시 중지'에 따라 재다운로드·다른 URL·기대 hash 갱신·서명 우회·직접 payload 공급으로 계속하지 않았습니다. Layout 도구 실행0, 필수359 payload 수신0, 검증된 오프라인 Layout 없음입니다.

다운로드 파일9개(검증 nupkg7+bootstrapper1+불일치 catalog1), 합계 **158,822,578bytes**. 설치·캐시·증거·계획을 구분하며9개 전체가 PASS라고 쓰지 않습니다. 보조 자료 포함 E stage는 집계 시 약159.2MB였습니다.

## 경로·공간·환경 보호

다운로드 전 C/E 실제 여유를 기록했고 E의 별도 폴더에 staging/feed/cache/Layout/TEMP/TMP/CLI home을 만들었습니다. 정확 경로와 드라이브 bytes는 Git 제외 로컬 보고에만 보존합니다. C의 기존 Installer/Windows PKI 로그·캐시 가능성과 계획 여유0.5GB를 다운로드 전에 보고했습니다.
전후 전체 volume 변화에는 동시 다른 작업이 포함되므로 단순 차이를 전부 이 작업의 실제 쓰기로 귀속하지 않습니다. 실제 다운로드 payload는 모두 E에 있습니다. 시스템 환경변수·registry·기본 Program Files·기존 SDK/VS 위치·ACL·symbolic link는 변경하지 않았습니다. 프로세스 범위 환경만 E로 지정했습니다.

## 다음 실제 설치와 승인 경계

**현재 실제 설치 명령은 발급·실행 보류입니다.** 이전 modify 명령은 검증된 동일 버전 Layout이 전제인데 현재 충족하지 못했습니다. 먼저 보존된 수신물·승인 metadata와 공식 서명 체계의 불일치 원인을 읽기 분석하고 필요한 경우 공식 출처 재조회/재수신 범위를 별도로 확정해야 합니다. 임의 hash 교체나 재다운로드 승인을 추정하지 않습니다.

이후 고정 catalog·bootstrapper·모든 Layout payload 검증이 완료되고 별도 설치 승인이 있어야 기존 VS의두component 추가를 검토합니다. MSVC 기존 E 설치 위치 유지, 새32bit/공유SDK는 E 우선이나 실제 vendor root/C 필수 부분은 설치 승인 전에 확인·보고합니다. 전역 Program Files 변경·심볼릭 링크로 우회하지 않습니다.

이번 실설치0/Restore0/Publish0/진단생성이미지실행0/계정TaskOnBoot·ACL변경0/재부팅강제종료0/게임변경0/Push/main병합0. native 이미지0개. 원본 수신물·검증 로그·중지표시는 보존했고 공급을 중지했습니다. **2026-11-01 마감 유지.**
