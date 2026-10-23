# Discord 예약·사용량 감시·조건부 종료 — 모의 검증 보고

2026-10-23 / Asia/Seoul / `codex/discord-ai-collaboration`

## 판정

**안전한 예약·보류·조건부 종료의 로컬 모의 구현 완료. 실제 종료·제품 통합·추가 AI 실행은 하지 않았습니다.** 기존 네 AI VERIFIED 및 실제 인계는 재실행하지 않았고, 원본 게임/Save/승인 아트도 변경하지 않았습니다.

이번 단계는 새 코드와 가짜 Discord 어댑터 검증입니다. 현재 봇·아트 수령 서비스를 재시작하지 않았으므로 새 6개 Slash 명령의 서버 등록/실제 응답, 실제 예약 운영은 **NOT_VERIFIED**입니다. 기존 연결을 새 기능의 실제 성공으로 집계하지 않습니다.

## 구현 범위와 제한

| 항목 | 완료 내용 | 실제 운영 제한 |
|---|---|---|
| 작업 예약 | 서울 시각·정리 여유·수정/취소·이벤트 중복 차단·영속 상태 | 자연어 전체 해석 대신 명시적 시각 입력 |
| 마감 | 공통 접수/claim/실제 CLI 게이트, 미실행 보류 인덱스, 실행중 결과 보존 | 실행중 외부 작업은 강제 종료하지 않음 |
| 다음 예약 | 다음 확인 시각 저장·주일 거부·재확인 알림 outbox | 시각 경과만으로 AI 재개하지 않음 |
| 사용량 | 공식 관측의 5시간/주간 창·회복 시각·만료/소진 HOLD | Discord 봇의 공식 자동 조회 브리지 미연결, 무인 자동 재개 불가 |
| Discord | 기존9+새6 명령 소스·대표/서버/채널 확인·한국어 비공개 응답 | 실제 서버 등록/새 명령 응답 미실행 |
| 자동 보고 | 마감·정리·다음 확인 outbox·영수증·불명 전송 자동 중복 방지 | 가짜 SDK로만 시험, 실제 새 알림 전송 미실행 |
| Windows | 공식 ScheduledTasks cmdlet의 고정 tick XML 생성·검사 | OS 등록 0, AI/종료 예약 동작 없음 |
| Git 통합 | 명시 승인 없으면 HOLD, 보호/백업/격리/충돌/HEAD 비교 계약 문서화 | 통합 실행기 연결·Push·병합 0 |
| PC 종료 | 13개 조건+최신 근거+취소 모의 판정, Discord 요청 항상 HOLD | 실제 로컬 권한/미저장 작업 검사·비강제 OS 종료 실행기 미연결 |

예약이 실행 승인, 미술 승인, Push/main 병합 승인으로 승격되지 않도록 했습니다. 정리 기본 10분, CLI90초/claim300초보다 시간이 부족하면 새 실행을 시작하지 않습니다. 기존 전역/역할 정지는 별도로 유지합니다. 동시 최대2개·기존 1회 슬롯·자동 재시도0 정책을 유지합니다. 주일에는 생산/검수 실행을 막습니다.

`next_start`는 다음 확인 예약이며 자동 AI 실행 허가가 아닙니다. 실제 자동 재개는 공식 조회 브리지, 새로운 시간창과 미소비 유효 실행 승인이 연결된 후에만 검토할 수 있습니다. 기존 소비된 슬롯과 결과불명 작업은 재시도하지 않습니다.

## 사용량 실제 근거

Codex 앱 공식 도구의 **2026-10-23 23:08:48 KST 관측**:

| 창 | 사용률 | 조회 당시 회복 시각 |
|---|---|---|
| 5시간(300분) | 22% | 2026-10-24 03:23:53 KST |
| 주간(10080분) | 3% | 2026-10-30 22:23:53 KST |

이는 해당 시점의 사용량이며 개발 대화 진행에 따라 변합니다. 출처·관측시각·두 창·일반 사용 허용 여부만 정규화해 [안전한 관측 근거](../Evidence/SafetyScheduling20261023/official-usage.json)에 기록했습니다. 인증/계정/재설정권/크레딧 원문은 저장하지 않았습니다. 실제 관측 값을 격리 DB에 재생하여 브리지 검증도 통과했습니다.

공식 앱 도구가 Python 봇에 자동 노출되는 API라고 가정하지 않았습니다. [공식 Codex 안내](https://developers.openai.com/codex/pricing)는 대시보드와 CLI `/status`를 안내합니다. 새 관측이 없거나 5분이 지나면 UNKNOWN/HOLD입니다. 소진 후 회복 시각 경과는 RECHECK_REQUIRED이며, 새 공식 관측 없이는 호출하지 않습니다. 5분은 서비스의 보수적 캐시 정책입니다.

이번 외부 AI 실행기 추가 호출 **0회**, 유료 API 전환·추가 크레딧 구매·재설정권 사용 **0회**입니다. 이 개발 대화 자체는 구독 사용량을 사용하므로 “구독 사용량 0”이라고 주장하지 않습니다.

## 검증

신규 모의 테스트 **48 PASS / 0 FAIL**(전체47건+레거시 단발 경로 추가 차단1건), 영향 범위 기존 납품 테스트 선별 **3 PASS / 0 FAIL**. Python compileall·PowerShell 문법·git diff 검사 PASS. 기존 전체 AI/Unity QA 반복 없음.

새 테스트는 예약 등록/수정/취소·잘못된 시각·정리 여유·주일·중복/동시 요청·재시작 복원·기존 수동 정지 보존·기존 RUNNING lease 보존·마감 후 결과 저장·동시2 제한·불명/만료/소진 사용량·양쪽 창 회복 시각·새 관측 전 차단·CLI 호출 전 차단·모의 종료 모든 조건·동기화 충돌·Unity/복사/미저장 작업·취소·권한 없는 요청·한국어 비공개 응답·outbox 중복 및 전송 불명 처리를 포함합니다.

Windows 공식 cmdlet으로 2026-10-24 20:00 KST의 **미등록 점검 XML**을 생성했습니다. `-m collab.safety_tick`, InteractiveToken, LeastPrivilege, IgnoreNew, AllowHardTerminate=false를 확인했습니다. 시간값은 시험용이며 실제 등록 예약이 아닙니다. tick은 별도 시험 DB에서 실행하여 AI·종료·Git 변경0을 확인했습니다. [XML 검사 요약](../Evidence/SafetyScheduling20261023/windows-task-preview.json).

[신규 테스트 로그](../Evidence/SafetyScheduling20261023/local-tests.txt), [레거시 추가 차단 시험](../Evidence/SafetyScheduling20261023/legacy-gate-test.txt), [기존 선별 테스트](../Evidence/SafetyScheduling20261023/selected-regression.txt). 실제 PC 종료·실제 병합·새 Slash 실연결은 테스트하지 않았습니다.

## 보호·승인·운영

[보호 근거](../Evidence/SafetyScheduling20261023/protection.json): 원본 **7,356파일 SHA 변경0/누락0**, 원본 git 상태 동일. 작업 시작 시 기존 **21업무·13사용량 행·승인 장부·28설정** 내용 해시 동일. 게임/Unity/Save/PNG 수정0, 외부AI0, OS 작업등록0, 실제종료0, Push/merge0. 기존 작업 장부에는 모의 예약을 넣지 않았고 별도 임시 DB로 시험했습니다.

아트 제작 → Codex 검수 → 바울 대표 미술 승인 → 승인 범위 내 적용 순서 유지. 통합 승인/검증 부족은 HOLD입니다. 컴퓨터의 미저장 사용자 작업 여부는 원격 추측으로 확인됐다고 선언하지 않습니다.

**PC가 완전히 종료되면 자동 재개할 수 없습니다.** 사용자가 부팅·로그인하고 서비스를 다시 실행해야 합니다. OS 예약 등록·서비스 재시작·실제 종료 활성화는 이번에 하지 않았습니다. 새 명령 실연결·공식 사용량 자동 브리지·실제 통합/종료 실행기는 후속 단계입니다.

[운영·Discord 조작·Windows 방식 안내](../../../Tools/DiscordAICollab/SCHEDULE_OPERATIONS_KO.md), [설계 계약](../../../Tools/DiscordAICollab/SCHEDULE_SAFETY_CONTRACT_KO.md). Microsoft 공식 [ScheduledTasks](https://learn.microsoft.com/en-us/powershell/module/scheduledtasks/register-scheduledtask?view=windowsserver2025-ps)와 [shutdown](https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/shutdown) 동작을 확인했습니다. shutdown.exe의 양수 timeout이 강제 종료를 암묵 적용하므로 사용하지 않았습니다. 이번 소스에는 OS 종료 실행 경로와 임의 명령 실행 기능이 없습니다.

구현 커밋: `08f6973ab91f22efe58bedba4006568be9ea22f0`. 문서·근거는 후속 로컬 커밋에 기록합니다. 원격 Push 및 main 병합 없음.
