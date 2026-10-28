# Discord V2 Catalog 최종 단발 검증 — 2026-10-28

기준 `b5e03a2a9a2d10df5857bbf4486bf37508bdcc76` / Issue66 댓글6459812141. **NO_GO / MANUAL / HOLD**. 승인된 동일 고정 Catalog GET1회만 수행했으며 추가 수신은 종료합니다.

## 실제 전송과 hash 비교

요청 URL과 관측 최종 URL은 동일한 [기존 고정 Microsoft Catalog URL](https://download.visualstudio.microsoft.com/download/pr/58aec969-7d60-47ab-a001-285ca0c69097/879bcae9d39b2812075ae8ce0cfb27c6755c6bced1063454425b2add533bea67/VisualStudio.vsman)입니다. HTTP1.1 **200 OK**, 리다이렉트0/Location 응답 없음, 전송 완료입니다. 자동 리다이렉트·재시도·압축 해제는 비활성화했으며 JSON 변환은 하지 않았습니다.

| 비교 | 크기 bytes | SHA256 |
|---|---:|---|
| 고정 기대값 — 변경0 | 31,191,413 | `879bcae9d39b2812075ae8ce0cfb27c6755c6bced1063454425b2add533bea67` |
| 최종 단발 수신 | 18,366,761 | `6e8522678632d309a7aab10883140ff42d1116f455c81e09378bd0181980c54b` |
| 보존된 첫 수신 | 18,366,761 | `6e8522678632d309a7aab10883140ff42d1116f455c81e09378bd0181980c54b` |

**기대 크기·SHA 모두 FAIL**. 첫 수신물과 새 수신물은 전체 바이트도 동일합니다. 응답 Content-Length는18,366,761이며 실제 저장 크기와 같습니다. Content-Type은application/octet-stream입니다. Content-Encoding 헤더가 없어 값은 **UNKNOWN**으로 남깁니다. 요청은Accept-Encoding identity, 클라이언트 AutomaticDecompression=None입니다. HTTP framing이 제거된 entity body를 원본 파일로 보존했으며 TLS packet capture를 의미하지 않습니다.

Date `Wed, 28 Oct 2026 03:28:18 GMT`, Last-Modified `Thu, 23 Jul 2026 13:49:44 GMT`. 전체 응답 헤더·최종 URL·수신 결과·승인 intent는 E드라이브 새 격리 폴더에 보존했습니다. ETag도 기록했지만 파일 hash나 서명 증거로 취급하지 않습니다. 정확한 로컬 경로와 절차는 Git 제외 자료에만 보존했습니다.

수신 실행 전후 C 여유는31,024,939,008bytes로 동일하고 E 여유는915,470,479,360→915,452,104,704bytes입니다. 전체 볼륨 변화는 동시 작업을 포함할 수 있습니다. 기존 Catalog·Bootstrapper·SHA 기록과 NuGet7종은 변경하지 않았습니다.

## 공식 신뢰 검증

Windows `Get-AuthenticodeSignature`1회 조회 결과 **UnknownError**, 메시지는 “지정된 트러스트 공급자가, 주제에 대해 지정된 형식을 지원하지 않거나 알 수 없습니다.”입니다. 서명자와 timestamp 서명자는 반환되지 않았습니다. 이는 JSON Catalog의 내장 서명이 무효라는 판정이 아니라 해당 Windows 조회로 형식을 검증하지 못했다는 결과입니다. **문서 서명·신뢰 체인·timestamp는 NOT_VERIFIED / UNKNOWN**입니다. 서명 구조 존재·TLS·hash를 신뢰 PASS로 대체하지 않았습니다.

Microsoft는 [setup engine의 신뢰 검사와 필요한 인증서](https://learn.microsoft.com/en-us/visualstudio/install/install-certificates-for-visual-studio-offline)를 설명하고, [Layout 검증](https://learn.microsoft.com/en-us/visualstudio/install/create-a-network-installation-of-visual-studio)에는 `--layout ... --verify`와 필요한 메타데이터 및 버전 제한을 명시합니다. 조사한 공개 문서와 설치 파일에서는 Layout 실행과 추가 다운로드를 금지한 이번 범위에서 단독 vsman을 검증하는 지원 CLI를 확인하지 못했습니다. 독자적인 서명 검증기·내부 비공개 API·인증서 설치로 우회하지 않았습니다. 따라서 요청한 공식 신뢰 검증은 완료하지 못했습니다.

## 원인 판정과 다음 승인

확정된 직접 불일치 지점은 **channel 참조의 고정 아티팩트 기준과 동일 공식 URL에서 실제 응답한 HTTP 본문 사이**입니다. 새 응답의 Content-Length부터18,366,761이므로 이번 차이를 저장 후 JSON 변환·클라이언트 자동 압축 해제·로컬 잘림으로 설명할 근거는 없습니다. 이전 조사에서 확인한 제품18.8.2/stable·132개 노드/359개 payload 일치 결과는 재사용했으며 반복 검증하지 않았습니다.

원본 배포·CDN·중간 전송 경로·표현 차이·계획 channel snapshot 중 어디에서 기준이 어긋났는지는 **미확정**입니다. Microsoft 서버 결함·손상·공격으로 단정하지 않습니다. 현재 수신물의 공식 신뢰도 미확정이므로 새 hash를 기대값으로 채택하지 않습니다. Manifest·SHA·URL 변경0입니다.

**다음 Layout 진행은 NO_GO**이며359개 payload 다운로드도 미승인입니다. 추가 직접 Catalog 재수신은 하지 않습니다. 다음 판단 후보는 Microsoft 공식 Installer/고정 버전 Bootstrapper가 메타데이터와 신뢰 검증을 관리하는 Layout 절차로 전환하는 것입니다. 문서화된 검증 경로를 사용한다는 장점이 있지만, 기존 Bootstrapper의 서명·신뢰 체인과 실제 고정 제품 버전은 먼저 확인해야 합니다. 공식 verify의 버전 제한과 불완전한 Layout 메타데이터 문제도 남아 있습니다.

전환 실행에는 별도로 Installer 실행·E격리Layout·필요한 메타데이터/engine 수신·PKI 조회와 가능한 update 동작 범위를 승인받아야 합니다. [공식 명령 문서](https://learn.microsoft.com/en-us/visualstudio/install/use-command-line-parameters-to-install-visual-studio)에 따르면 `--noWeb`만으로 Installer update 조회까지 차단되지는 않습니다. 이 범위에서359개 payload 다운로드를 분리할 수 있는지 확정하지 못하면 실행하지 않고 승인 범위 또는 Microsoft 지원 확인을 먼저 해결해야 합니다. 기존 VS·SDK 업그레이드, 설치, 인증서 변경은 포함하지 않습니다. 이번에 새 Layout 실행 명령을 발급하거나 실행하지 않았습니다.

이번 Catalog GET1회 외 NuGet 재다운로드·VS payload·Layout·설치·Restore·Publish·계정·Task·OnBoot·Unity 변경·Push·main 병합0. 보고 후 중지합니다. 2026-11-01 마감 유지, 이후 별도 연장 없이 증거·인계를 보존하고 게임 개발로 복귀합니다.

브랜치 `codex/discord-usage-diagnostics-20261026`. 이 보고와 상태/인계 문서만 로컬 커밋하며 `LOCAL_COMMITTED / REMOTE_NOT_PUSHED`. 최종 SHA는 완료 보고 및 Issue66 댓글에 기록합니다.
