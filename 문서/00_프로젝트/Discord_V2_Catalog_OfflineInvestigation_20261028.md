# Discord V2 Catalog 로컬 원인 조사 — 2026-10-28

기준 commit `f7155b7b675833cd58d60f9c960b71b8b2c7ea72`, Issue #66. **NO_GO / MANUAL / HOLD**. 기존 자료만 읽었으며 다운로드·Layout 재시도·설치·Restore·Publish·OS 변경은 수행하지 않았습니다.

## 확인된 사실

| 항목 | 기존 계획 / 실제 관측 |
|---|---|
| 고정 공식 Catalog 기대 | 31,191,413 bytes, SHA256 `879bcae9d39b2812075ae8ce0cfb27c6755c6bced1063454425b2add533bea67` |
| 실제 수신 Catalog | 18,366,761 bytes, SHA256 `6e8522678632d309a7aab10883140ff42d1116f455c81e09378bd0181980c54b` |
| Installer 캐시와 수신물 | 전체 바이트 동일. 크기·SHA도 동일 |
| 최초 구성 선택 자료 | 위 Installer 캐시. 다운로드 기대값은 별도 channel manifest의 Catalog 참조에서 취득 |
| 제품 / 채널 | 양쪽 메타데이터 VS18.8.2 / build18.8.12023.21 / VisualStudio.18.Release stable |
| 구성 비교 | 계획132개 노드 및359개 payload의 URL·SHA·크기 대조에서 누락·변경0 |
| 대상 도구 | MSVC14.51.36252, 표시 Windows SDK10.0.26100.8249 유지 |

공식 channel 참조는 `download.visualstudio.microsoft.com`의 고정 Catalog 아티팩트를 가리킵니다. 정확한 URI·원본 출처·증거 JSON은 Git 제외 로컬 조사 문서에 보존했습니다. 기존 설치 기록 VS18.8.12023.21과 제품 메타데이터는 일치하지만 실제 설치 호환성은 미검증입니다.

## 원인과 신뢰 판단

직접 중지 원인은 고정 아티팩트의 크기·SHA 불일치입니다. 최초 계획에서 구성 선택용 캐시와 다운로드 기대값의 출처가 분리되어 있었다는 사실은 확인했습니다. 제품 버전 변경 증거는 없으며, 기존 캐시와 전체 바이트가 동일하므로 임의 파일 손상을 뒷받침하는 증거도 없습니다. 다른 표현·스냅샷 선택 또는 서버·전송 처리 중 무엇이 실제 불일치를 만들었는지는 **UNKNOWN**입니다.

기존 수신 스크립트는 응답 객체를 저장하지 않았습니다. 요청 URL은 계획과 코드에서 확인되지만 실제 최종 URL·HTTP 상태·응답 헤더·Content-Encoding·ETag·redirect 이력은 기록되지 않아 복구할 수 없습니다. 파일 시간은 HTTP Last-Modified 증거가 아닙니다.

Catalog와 channel에 서명 구조 및 Microsoft 명칭은 있으나 문서 digest·서명·인증서 체인·시각을 검증한 증거는 없습니다. 서명 필드 존재를 서명 PASS로 취급하지 않습니다. **현재 수신물의 공식 신뢰 UNKNOWN / HOLD**. Bootstrapper는 기존 크기·SHA PASS만 유지하며 서명 미검증입니다. NuGet7종 기존 PASS는 재검증하지 않았습니다.

## 최소 해결안과 별도 승인 조건

현재 Manifest·SHA·URL 변경은0입니다. 수신 SHA를 새 기대값으로 대체하지 않습니다. 공식 검증된 다른 스냅샷이라는 전제가 충족되지 않아 새 고정 Manifest도 발급하지 않습니다.

다음 승인 범위는 **동일한 고정 공식 URL의 Catalog1개를 E드라이브 새 폴더로1회 재수신하고 전송 증거를 보존하는 것**만 제안합니다. 기존 수신물·로그를 덮어쓰지 않습니다. 요청/최종 URI·HTTP 상태·헤더·redirect를 기록하고 임의 redirect는 중지합니다. 원시 수신 바이트와 명시된 Content-Encoding에 따른 표준 디코딩 결과가 있다면 구분 보존하며 JSON 재직렬화는 금지합니다. 기존 기대 크기·SHA 그대로 대조하고 불일치 시 즉시 중지합니다.

Catalog 일치 및 공식 메타데이터·서명 검증, Bootstrapper 서명 검증과 구성 고정이 끝나기 전359개 payload 다운로드는 재개할 수 없습니다. 공식 검증에 추가 온라인 조회나 검증 도구 실행이 필요하면 해당 범위를 먼저 확정해야 합니다. NuGet 재다운로드·전체 Layout 재시도·설치 승인은 포함하지 않습니다.

이번 조사에서 신규 다운로드0, 기존 수신물 변경0, C드라이브 대용량 작업0, 계정·Task·OnBoot·Unity 변경0, Push·main 병합0. 로컬 보고만 커밋하며 `LOCAL_COMMITTED / REMOTE_NOT_PUSHED`. 브랜치 `codex/discord-usage-diagnostics-20261026`. 마지막 관련 commit은 위 기준이며 이 보고의 최종 SHA는 완료 보고에 기록합니다. 2026-11-01 마감 유지, 조사 후 중지합니다.
