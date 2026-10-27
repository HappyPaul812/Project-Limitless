# Issue #66 — 전용 Worker r1 보호 Staging 생성·검증 완료 / 미설치

## 관리자 실행 결과 후속 판독

[Issue #66 생성·검증 성공 실제 보고](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6435141316). 제공 JSON 재직렬화 저장본 SHA256 `03335ccc8a92e49a036ec01a0201a04e8f01eed970c9bd6c4f6fb2f5d595bd34`, 관리자 원본 파일 해시 아님. 최종 로컬 문서 커밋은 Issue 후속 참조 / REMOTE_NOT_PUSHED.

바울 대표 제공 JSON은 **STAGING_CREATED_VERIFIED_NOT_INSTALLED**, admin=true/Step=Stage/actual_creation_attempted=true, 내부 안전 검사10개 모두 PASS입니다. 아래 실행 대기 내용은 당시 이력으로 보존하며 최신 상태는 **보호 staging 생성·검증 성공 보고 수신 / 설치 미실행 / runtime HOLD**입니다. 승인된 생성1회는 완료됐으며 재실행하지 않습니다.

제공값은 [AdminStageUserProvided.json](팀운영/Evidence/DedicatedR1Preparation20261027/AdminStageUserProvided.json)에 값 동일하게 재직렬화했습니다. 원본 디스크 JSON 경로·실행 시각·도구 SHA·개별 파일 metadata/inventory가 제공 출력에 없어 독립 native 재조회나 원본 바이트 해시 검증을 수행했다고 표현하지 않습니다. 이번 판독에서는 생성/검사/151/Preflight를 재실행하지 않았습니다.

| 필수 보고 | 판독 결과와 근거 범위 |
|---|---|
| 생성·검증 상태 | PASS — 제공된 관리자 결과의 고정 native verifier 성공 반환. 설치 성공 아님 |
| 실제 반환 경로 | `C:\Program Files\LimitlessRelayDedicatedV2Staging20261027\a5af3087b78b45d1baae46067d5b7ac9\Bundle`, 같은 package의 `Bundle.zip`은 Create 계약에 포함 |
| Owner/Group/DACL | 성공 반환까지 고정 verifier가 BA Owner/SY Group·protected BA/SY-only DACL의 각 entry 일치를 검사하는 계약. 개별 관측 SDDL/파일ID 값은 JSON에 없고 임의로 추가하지 않음 |
| 파일 수·SHA | 고정 검증기 성공 계약상 Bundle 내부 payload766+Manifest1=767파일의 정확 entry·모든 SHA 및 별도 Bundle.zip SHA 검증 완료 보고. 전체 파일수는 Bundle.zip까지768개이며 디렉터리는 별도 |
| source/복사본 | 고정 생성기의 entry별 source 메모리 바이트와 복사 후 handle readback SHA·ZIP SHA 일치 성공 반환. 독립 두 번째 비교0 |
| 오류·부분 생성·복구 | 오류/HOLD/close-error 필드 없이 성공 반환, retry0. 보고된 실패/부분 실패 없음, 현재 복구 필요 증거 없음. 자동 삭제/복구 실행0 |
| 기존 운영 | 내부 계정 SID/state·신규 계정/CodeState/Task/pipe 부재 검사 PASS. Stage 분기에는 기존 운영/인증/Task를 수정하는 경로 없음. 11개 프로세스·OnBoot/봇아트/운영DB 전체를 새로 조사했다는 주장은 하지 않음 |
| 설치 전 남은 조건 | 별도 실제 설치 승인, 해당 시점 stage 전체 handle/FileID/Owner/DACL/SHA 재검증·외부 보호 승인/소비·계정2/권리·Task1 범위 확정. 새 SID·토큰/GROUP·설치readback·Job/IPC독점은 계속 deferred |

검증된 고정 계약의 디렉터리 SDDL은 `O:BAG:SYD:P(A;OICI;FA;;;SY)(A;OICI;FA;;;BA)`, 파일은 `O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)`입니다. 위 계약에 대한 관리자 검증 성공 보고로 기록하며 raw metadata 원본을 받은 것으로 표시하지 않습니다. 원본 r1 3SHA/후보 및 F: ACL 변경0, 코드/도구 변경0. 신규 계정/Batch권리/Task/설치승인JSON/설치/로그인/IPC/FakeWorker/AI/usage/Discord/Push 실행0. V1 REFERENCE_VERIFICATION_HOLD·MANUAL/A HOLD 및 runtime HOLD_NEW_TOKEN_GROUP_EXCLUSIVITY_NOT_VERIFIED 유지 후 중지합니다.

## 이하: 생성 명령 안내 당시 이력

[Issue #66 실제 상태 보고](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6435083422). LOCAL_COMMITTED / REMOTE_NOT_PUSHED, 최종 커밋은 Issue 후속 보고 참조.

2026-10-27 바울 대표가 **보호 staging 생성1회만 승인**했습니다. 관리자 PowerShell7에서 대표 직접 실행 지시이므로 Codex가 생성기를 실행하거나 관리자 권한으로 대리 생성하지 않았습니다. 현재 상태는 **WAITING_ADMIN_EXECUTION / 생성·검증 결과 미확정**입니다. PASS나 생성 실패 HOLD로 임의 판정하지 않습니다. 설치 승인·새 계정/권리·Task·IPC/AI는 이번 승인 밖입니다.

브랜치 `codex/discord-usage-diagnostics-20261026`, 시작 HEAD/기준 `af108ad73f5a3011b3e9eb91821086561e2414b0`. 기존 Preflight12PASS·오프라인151PASS 재사용, 독립 재실행0. r1/V1/r11·도구 코드 변경0입니다.

## 실행 전 확인

기존 문서의 Stage 전용 명령과 `Tools/DiscordAIServiceDiagnostics/R1Preparation/prepare-r1.ps1`의 scope 분기를 검토했습니다. 도구 SHA256은 `c6a4b7b6dec6158b7191bdecf120d0c9efb3bc2b668e009aeba104975337671d`로 일치합니다. `Stage`는 `STAGE_R1_ONLY_NO_INSTALL`만 받으며, 설치 승인 root/JSON·계정·LSA·Task 생성 분기로 진행하지 않습니다.

원본 `F:\study\codex\Project-Limitless\Temp\Session0DedicatedV2Candidate20261027-r1.zip`을 읽어 다음을 확인했습니다. payload766+Manifest1의 정확한 entry 집합·중복 부재·모든 payload SHA 일치입니다. 이는 승인된 생성의 source 확인이며 기존151시험 재실행이 아닙니다.

| 항목 | SHA256 |
|---|---|
| Installer | `83c97d781fb2d9aed797b41ea6bc799b83e939ad3ffa6f7af6a2d26f1080d68f` |
| Manifest | `43f6afcc91eae196e015393670cd9cd4ff37194099b313c2b901177226cd32e3` |
| ZIP | `be58f8cdbe0befdcc64cd67287865c6cd6fc2611b4a706d26cda022a8014ca25` |

대표의 관리자 실행에는 [기존 관리자 명령 문서의 2번 Stage 한 줄](Discord_DedicatedR1_AdminCommands_20261027.md)을 사용합니다. 새 NoProfile 자식 PowerShell7에서 도구 SHA를 확인한 메모리 바이트만 실행하고 기존 관리자 창을 닫는 exit 명령은 없습니다. 기존 helper에 포함된 실행 시점 namespace/부모/기존 계정 상태의 좁은 안전 읽기는 유지되며 별도의 `-Step Preflight` 12개 검사나151시험을 호출하지 않습니다. `-Step Install` 명령은 사용하지 않습니다.

## 고정 경로·검증 계약과 현재 미확인 사항

승인 대상 package 경로는 `C:\Program Files\LimitlessRelayDedicatedV2Staging20261027\a5af3087b78b45d1baae46067d5b7ac9`이며, 그 아래 Bundle 및 Bundle.zip만 배치합니다. 필요한 신규 staging root/package 디렉터리도 동일 보호 DACL로 생성하며 기존 Program Files 부모 ACL은 바꾸지 않습니다. helper는 staging root가 이미 있으면 HOLD합니다.

예정 Owner=BUILTIN\Administrators(S-1-5-32-544), Group=SYSTEM(S-1-5-18), protected DACL은 디렉터리 `O:BAG:SYD:P(A;OICI;FA;;;SY)(A;OICI;FA;;;BA)`, 파일 `O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)`입니다. 일반 사용자에게 쓰기·삭제·권한변경을 grant하지 않습니다. **실제 Owner/DACL은 아직 조회 결과가 없으므로 예정 계약이지 관측값이 아닙니다.**

고정 native Create는 source handle/SHA → 부모 Owner/writer·NTFS/경로 확인 → SECURITY_ATTRIBUTES 제한 DACL 생성 → 전체 entry별 handle/FileID/최종 경로/Owner/Group/DACL·reparse/hardlink/ADS·pending 상태 확인 → 복사본과 source 메모리 바이트의 SHA 비교 → Bundle767파일과 Bundle.zip·예상 밖 entry 검사 → handle metadata 재조회 순서입니다. source와 복사본 일치 및 실제767파일 검증은 **관리자 실행 결과 대기**입니다. 원본 F: 파일/ACL은 수정하지 않습니다.

## 결과 판독·부분 실패·다음 승인

- 정상 반환 기대값은 `STAGING_CREATED_VERIFIED_NOT_INSTALLED`와 고정 Bundle 경로입니다. 이를 받으면 실제 생성 실행의 정제 JSON을 보존하고 source와 생성 검증 계약에 맞춰 보고합니다. 개별 native 관측값이 출력에 없으면 임의로 관측값을 채우지 않습니다.
- `HOLD`, 부분 생성, 조회 불명, close 오류면 즉시 중지합니다. 같은 명령 재시도·자동 삭제·ACL 완화·기존 root 재사용0. 생성된 새 staging 자산과 정제 오류코드를 보존하고 복구 필요 여부를 판독합니다. 이1회 실행의 결과를 받기 전 두 번째 생성 명령을 안내하지 않습니다.
- 현재 실제 생성 시도는 Codex0이며 대표 실행 결과는 아직 제공되지 않았습니다. 부분 생성·복구 필요 여부도 UNKNOWN입니다. 계정·Task·기존 운영 프로세스·인증에 대한 변경 명령은 실행하지 않았습니다.
- 정상 생성 후에도 설치 직전 protected stage 전체 SHA/handle/Owner/DACL 재검증과 **계정2·권리·보호Code/State/Auth·Task1·외부 승인/소비 기록을 포함한 별도 실제 설치 승인**이 필요합니다. 신규 SID/유효Batch토큰/GROUP Task영향·Job·IPC 독점6개 deferred는 계속 HOLD입니다. 인증/IPC/AI/usage/Discord/A 승인과도 구분합니다.

V1 REFERENCE_VERIFICATION_HOLD·MANUAL/A HOLD, Worker1007/11개·OnBoot2·봇아트·인증/CODEX_HOME·운영SQLite·Unity/게임/Save 보호. Push/main병합0. 이번에는 관리자 Stage 명령 안내 후 결과를 기다리며, 실제 설치·시작으로 이어가지 않습니다.
