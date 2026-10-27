# Issue #66 — 전용 Worker r1 설치 1회 승인 / 관리자 직접 실행 대기

[Issue #66 실제 상태 보고](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6435281477). 최종 로컬 문서 커밋은 Issue 후속 참조 / REMOTE_NOT_PUSHED.

2026-10-27. 바울 대표가 **신규 시험 계정2·예약 작업1의 설치1회만 승인**했습니다. 기준 문서 커밋 `93d9a7029e04d9fd1b6bd6921961dd214392c77a`, 브랜치 `codex/discord-usage-diagnostics-20261026`. 현재 **WAITING_ADMIN_INSTALL_RESULT**이며 설치 PASS/HOLD 및 실제 생성 SID·자산 목록은 미확인입니다. 대표 직접 실행 전 Codex 생성/설치 실행0입니다.

## 명령·고정 바이트 검토

기존 관리자 문서3번 Install 명령과 도구SHA `c6a4b7b6dec6158b7191bdecf120d0c9efb3bc2b668e009aeba104975337671d` 일치를 확인했습니다. 고정 ZIP을 읽어 다음3SHA와 실제 포함된 설치 보안 코드를 확인했습니다. Preflight12·Stage10·151시험을 재실행하지 않았고 후보/도구/설치 코드를 변경하지 않았습니다.

| 항목 | SHA256 |
|---|---|
| Installer | `83c97d781fb2d9aed797b41ea6bc799b83e939ad3ffa6f7af6a2d26f1080d68f` |
| Manifest | `43f6afcc91eae196e015393670cd9cd4ff37194099b313c2b901177226cd32e3` |
| ZIP | `be58f8cdbe0befdcc64cd67287865c6cd6fc2611b4a706d26cda022a8014ca25` |

승인된 보호 package는 `C:\Program Files\LimitlessRelayDedicatedV2Staging20261027\a5af3087b78b45d1baae46067d5b7ac9`입니다. 도구는 Install 분기에서 Bundle767파일·별도ZIP·고정3SHA·Owner/DACL·handle/FileID/경로/위험 링크·정확 entry를 재검증하고 handle을 설치 종료까지 유지합니다. 이것은 유지해야 하는 설치 내부 검사이며 별도 Preflight/Stage 명령 재실행이 아닙니다.

실행 관리자/새 namespace 충돌·기존 Worker/V1 계정 상태를 읽고 protected staging을 검증한 뒤, 신규 보호 승인 root/15분 승인 JSON을 CREATE_NEW로 생성합니다. 승인에는 실제 관리자SID·3SHA·schema3/epoch·두 계정명·보존·새 Worker batch-only 활성화 범위를 바인딩합니다. 기존 승인 root/소비 마커의 재사용은 거부합니다. 고정 Installer는 기존 V1 파일855·OnBoot pin/기존 Worker 보존 snapshot과 fresh namespace·승인을 검증하고 소비/lease 후에만 계정을 생성합니다. 미확인/권한거부/충돌은 HOLD입니다.

## 승인된 설치 범위

- 신규 `LIMITLESSBotV2D`, `LIMITLESSWorkV2D`: 둘 다 Disabled 생성·Windows 발급 신규 SID를 기존 모든 local SID/관리자/Worker1007/V1과 비교, Users-only·새SID에만 제한 권리·계정만료24시간·설명 package 결합 readback.
- 신규 Code/State/Auth/CODEX_HOME: 보호 DACL 생성·실제 Owner/Group/DACL·SHA/readback, 기존 인증/Token 복사0·Auth EMPTY. 신규 Worker만 사전 승인된 `ENABLED_BATCH_ONLY`, Bot Disabled 유지. Batch 권리 직접 readback은 유효 그룹/GPO/실제 Batch 토큰 검증과 구분합니다.
- 신규 Task `LIMITLESS-DedicatedV2-20261027-Host`1개만 원자적 SDDL 등록, Owner/DACL·Principal=실제 새Worker/PASSWORD1·RunLevel0·Disabled/Trigger0/Restart0/IgnoreNew/PT90S·고정 action/readback. FAKE_ONLY/real_ai_enabled=false·auto_start=false 유지.
- 단발 승인/소비/설치lease·보호 Result 증거. 실제 계정/Task 생성과 실제 Worker Session0 실행은 별개이며 Task 시작/IPC/FakeWorker는 승인 밖입니다.

대표는 [관리자 명령 문서3번](Discord_DedicatedR1_AdminCommands_20261027.md)의 한 줄만 관리자 PowerShell7에서 **최대1회** 직접 실행합니다. 도구 SHA를 확인한 메모리 바이트를 새 NoProfile 자식에서 실행합니다. Preflight/Stage 명령을 다시 실행하지 않습니다. 로컬 SecureString prompt에서 새 Bot/Worker 암호를 각각 입력하고 암호를 대화/파일/CLI인수에 보내지 않습니다. 기존 관리자 PowerShell을 닫는 exit 명령은 없습니다.

## 결과 수신 후 감사·정지 계약

정상 helper 기대 상태는 `INSTALL_REPORTED_COMPLETE_POSTINSTALL_AUDIT_REQUIRED`, 내장 설치 결과는 `DEDICATED_INSTALLED_BOT_DISABLED_WORKER_BATCH_ONLY_TASK_DISABLED`입니다. 이 값을 받아도 세부 evidence를 판독하기 전 실제 SID/Owner/DACL/권한을 추측하지 않습니다.

결과 수신 후 실제 생성 SID/계정 상태·그룹/권리·CodeStateAuth ACL·파일SHA·Task Owner/DACL/Principal/Disabled/Trigger0·기존 V1/Worker/OnBoot snapshot·소비/생성 자산/부분 실패 목록을 제공 JSON과 보호 Result 증거 기준으로 확인합니다. 출력에 없는 개별 raw metadata는 독립 조회값으로 기입하지 않습니다. 관리자 읽기 감사가 더 필요하면 새 읽기 도구/최소 범위로 처리하고 설치를 재실행하지 않습니다.

오류/부분 설치/불명은 HOLD, 재시도·자동 삭제·권한 완화0입니다. 승인 root만 생성되거나 Worker enable 뒤 Task 등록에 실패할 수 있으므로 생성된 SID/파일/권리/상태·정제 오류·소비 여부를 보존합니다. 신규 자산 비활성화/정리도 자동 수행하지 않고 별도 복구 승인을 받습니다. 기존 V1 Cleanup/Install·Worker/운영 Task는 사용하지 않습니다. 정상 또는 HOLD 판정 후 즉시 중지합니다.

현재 실제 SID·계정 활성 상태·Task보안/배포 무결성 readback·부분 생성/복구 필요 여부는 **관리자 실행 결과 대기**입니다. 신규 Worker의 유효 토큰·GROUP 영향·Session0 IPC·Job 독점성은 설치 후에도 별도 HOLD입니다. V1 REFERENCE_VERIFICATION_HOLD·MANUAL/A HOLD, 기존 Worker1007/11개·봇아트·OnBoot2·인증/CODEX_HOME·운영SQLite·Unity게임Save 보존. 설치승인만으로 로그인/AI/usage/Discord/A·자동시작·Push/main병합/종료·재부팅을 실행하지 않습니다.
