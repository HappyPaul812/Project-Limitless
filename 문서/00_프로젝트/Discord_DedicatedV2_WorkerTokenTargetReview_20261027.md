# SID1010 기존 유효 토큰 대상 확정 조사 — 2026-10-27

판정 HOLD_NO_CONFIRMED_SID1010_PID_CREATION_TARGET.
브랜치 codex/discord-usage-diagnostics-20261026, 기준 f8f46a599df18a9c7dc34558908160b8e1012c70.
Issue #66 최신6441516356/6441521161 및 보존된 로컬 증거·로그만 재사용했습니다. 새 OS 프로세스/토큰/Task 조회는 하지 않았습니다.

## 조사 범위와 결과

1. 팀운영/Evidence의 JSON·log·txt에서 실제 SID1010 또는 LIMITLESSWorkV2D가 포함된 기존 파일9개를 확인했습니다. 계정·Manifest·설치 복구·Task/파일 감사 기록이며, 그 JSON 객체들에서 PID 필드가 있는 기록은0개였습니다. 파싱 실패0.
2. Temp/WorkerProcessGroupInvestigation20261027 및 Temp/V2Hold2Worker20261027의 기존 진단 JSON5개에서 SID1010/신규 계정명 일치 기록이 없었습니다.
3. 기존 f1f6f3991b5e4a78bffef16aac347c98.json의 PID·생성UTC·owner_sid 연결 프로세스11개는 모두 SID1007입니다. SID1010 대상에 사용하지 않습니다.
4. 최신 Task readback PASS는 Principal=SID1010 및 Worker Enabled를 증명하지만 프로세스 PID·생성UTC·실제 실행 토큰의 증거가 아닙니다.
5. 따라서 SID1010 소유의 확정 PID·생성UTC 쌍 및 현재 실행을 뒷받침하는 기록을 검토 범위에서 확보하지 못했습니다. Windows 전체에 해당 프로세스/토큰이 없다는 뜻은 아닙니다. 과거 기록만으로 현재 생존도 확정하지 않습니다.

검색은 기존 증거/진단 파일로 제한했습니다. 전체 Temp 재귀 파일목록 시도는 비대해 중단하고 명시된 진단2폴더로 범위를 좁혔습니다. 검색 결과를 전체 PC에 대한 부재 증명으로 확대하지 않습니다.
보호된 현장 Entry/Result의 새로운 관리자 열람은 하지 않았으며 기존 정제 결과만 재사용했습니다.
검토 파일별 SHA·범위·판정은 Evidence/WorkerTokenTargetReview20261027/ExistingEvidenceReview.json에 기록했습니다. 원본 수정0.

## 기존 읽기 명령 처리

입력할 유효 대상이 없으므로 준비된 기존 토큰 명령을 실행하도록 안내하지 않습니다. 추측 PID/시각, PID0을 형식적으로 넣은 반복, SID1007 토큰 대체를 하지 않습니다.
기존 문서 Discord_DedicatedV2_WorkerEffectiveAccess_Preparation_20261027.md의 명령/도구 SHA581d2c91…는 그대로 보존합니다. 새로운 모의 검증기나 실행 도구를 제작하지 않았습니다.
31·48·22 PASS는 기존 결과를 재사용했으며 이번 신규 시험0, 실권한검증 PASS0입니다.
Task descriptor PASS는 유지하고 유효토큰/Session0Batch/Job독점/1007충돌·V1/MANUAL/A/runtime HOLD도 유지합니다.

## 향후 별도 승인할 최소 FAKE_ONLY 격리 절차

현재 실행 승인이 아니며 다음 단계를 설계·검토하기 위한 최소안입니다.

1. **토큰 관측 전용 진단 실행 경로 준비:** SID1010만 사용하고 기존 Disabled/Trigger0 Task를 변경·시작하지 않는 별도 경로의 구현 가능성을 확인합니다. 최대1프로세스·90초 감독·전용 Job/고유 식별자·자동재시도0·네트워크/IPC/모델/Discord 호출0. 기존 인증/CODEX_HOME을 복사하거나 실제 로그인을 하지 않습니다. 안전한 Batch/Session0 생성이 기존 보호 정책과 양립하지 않으면 실행하지 않고 HOLD합니다.
2. **별도 실제 실행 승인 후에만 대상 확보:** 검토된 실행 도구와 SHA/정확 변경 범위·종료/정리 경계를 확정한 뒤, 대표 직접 단발 승인 절차로 신규 Batch 토큰·진단 프로세스를 생성하는 것을 검토합니다. 이때 OS 발급 SID1010, 실제 PID/생성UTC/Auth LUID/Batch4/Session0 및 이미지 SHA를 고정합니다. 기존 consumed 설치 승인을 재사용하지 않습니다.
3. **관측 및 종료:** 준비된 읽기 도구로 그 실제 토큰만 검사합니다. 단순 SID/그룹목록이나 any-job/빈lease만으로 통과시키지 않습니다. Job/자식 프로세스·90초 제한·동시실행/승인소비·정리 결과를 별도 판정하며, 기존 SID1007 프로세스는 건드리지 않습니다. 새 시험 소유 자산에만 종료·정리 범위를 제한하고 실패 증거는 보존합니다.
4. 이는 IPC 시험과 분리된 토큰 관측 시험입니다. FAKE_ONLY IPC는 이 단계가 통과하더라도 별도 승인 전 실행하지 않습니다. 공용 구독·비협조 실행기 경합을 제어하지 못하면 계속 HOLD합니다.

## 지금 필요한 사용자 승인 한 가지

**“SID1010 토큰 관측용 FAKE_ONLY 단발 격리 실행의 사전 준비(실행기·90초/Job·종료 절차와 보호 조건 구체화, 실제 실행 제외)” 승인**입니다.
이유: 기존 증거에 읽기 도구에 넣을 확정 대상이 없고, 새 로그온/프로세스 생성은 현재 금지되어 있습니다. 검증 가능한 대상의 생성 방법부터 보호 범위에 맞게 구체화해야 하며, 아직 검토된 실행 명령이 없으므로 지금 실제 실행을 승인받았다고 처리할 수 없습니다.
현재 요청을 추가 실전 실행 승인으로 해석하지 않습니다. 이번 턴에서는 사전 준비안 문서화까지만 완료했고, 위 승인에 따른 새 실행기 작업도 시작하지 않았습니다.

## 보존 및 중지

이번 live queries/test/mock tool/new logon/Task start/IPC/AI=0. 기존 계정·Task·ACL·승인소비·인증·OnBoot·게임/Unity/Save 변경0. 기존 원본과 HOLD 상태 보존.
Issue66/CURRENT_STATUS/CODEX_LATEST/담당인계 문서화 후 중지합니다. LOCAL_COMMITTED / REMOTE_NOT_PUSHED, 최종 로컬 커밋은 Issue 후속 보고.
