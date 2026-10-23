# Phase3 대표 계정 일반 PNG 자동 수령 검증

2026-10-23 / Asia/Seoul / `codex/discord-ai-collaboration`

## 결과

**대표 계정의 일반 PNG 자동 수령 VERIFIED. 실제 변경분 기준9 PASS / 0 FAIL, 신규 로컬7 PASS / 0 FAIL.** 기존 봇 자체 업로드 시험을 대표 업로드로 간주하지 않았습니다.

기존 감시기가 **23:05:32 KST** 대표가 게시한 일반 첨부를 **23:05:34 KST**에 받아 업무를 자동 등록한 기록을 발견했습니다(약2.199초). 이번 요청에서는 그 Discord 원문·첨부·원래 자동 보고·저장 파일을 **23:26:56 KST**에 다시 대조했습니다. 새 업로드를 요청하거나 새 업무를 수동 생성하지 않았습니다. 검증 전후 전체 업무 수는21→21입니다.

이번 업무만 TEST_ONLY로 명시했고 BLOCKED/NOT_VERIFIED/USER_REVIEW_PENDING을 유지했습니다. 시험용 업무의 미술 승인 전환은 허용하지 않습니다. 실제 기술/Unity/권리/미술 검수 완료로 처리하지 않았습니다.

## 실제 파일·업무·Discord

| 항목 | 실제 확인 |
|---|---|
| 채널 | 아트-납품 / 1563179817715638403 |
| 작성자 | 허용 대표 숫자 ID와 일치·사람 계정·봇 아님 |
| 원문 메시지 | 1563191652171124807 |
| 첨부 ID | 1563191651848036453 |
| 기존 감시기의 자동 등록 업무 | **L-5546382550914017** |
| 형식/크기 | 실제 PNG·CRC/디코딩 정상 / **563,323 bytes** |
| 규격 | **677×626 RGBA**, 알파255~255로 불투명 |
| SHA256 | `b9a6be63ea275a8710191f1be050b44868ff55b5c59bfdf787986dbe92176006` |
| 중복 여부 | 해당 SHA의 수령 원본1·수령 이벤트1·검수 업무1, 기존3건과 다른 SHA |
| 원래 자동 보고 | 1563191666414723246 / 실제 Discord 원문 재확인 |
| TEST_ONLY 검증 보고 | **1563197037779099649** / SENT 및 실제 Discord 재조회 PASS |

저장 경로:

`F:\study\codex\Project-Limitless\Temp\DiscordAICollab20261023\Worktree\Tools\DiscordAICollab\data\phase3\quarantine\b9a6be63ea275a8710191f1be050b44868ff55b5c59bfdf787986dbe92176006\source.png`

Discord 첨부를 다시 읽은 바이트와 저장된 원본의 SHA·길이가 동일합니다. 이미지 바이트를 수정하거나 제품 폴더에 복사하지 않았습니다. 불투명 PNG라는 사실은 입고 시험 결과이며 정식 Sprite 적합/라이선스 PASS를 뜻하지 않습니다.

다음 기존3건은 payload/evidence/상태를 포함해 전부 동일합니다:

- L-bf8ebb36f38240c9 — 기존 Discord SELF_TRANSPORT_TEST
- L-169ba798cf6646cc — 기존 ZIP 사본 시험
- L-79a30df7eb18488c — 기존 Windows 백그라운드 PNG 시험

따라서 아트 파일 검수 대기 업무는 기존3+대표 일반첨부 시험1입니다. 시험 업무는 TEST_ONLY 표기·승인 불가로 구분합니다. 전체 장부21건은 기존 AI/MOCK 등의 다른 업무도 포함한 숫자입니다.

## 서비스·권한

현재 수령 서비스는 22:36:41 KST 시작한 Python 런처36296→자식50196 관계이며 정상 heartbeat가 갱신됩니다. 서로 다른 수령 서비스를2개 실행한 것으로 계산하지 않습니다. 이번에 봇/감시 서비스를 중지·재시작하지 않았습니다.

Discord 공식 SDK로 애플리케이션 Message Content Intent 활성 여부와 아트-납품의 유효 권한 보기·기록읽기·보내기·파일첨부를 확인했습니다. 4권한 모두true입니다. 다른 계정의 업로드나 봇 자체 업로드는 대표 일반 첨부 증거에 포함하지 않았습니다. 토큰은 기존 DPAPI wrapper가 메모리에만 로드하며 출력/근거/Git에 저장하지 않았습니다.

## TEST_ONLY 보호와 검증 범위

`collab/owner_receipt.py`는 정확한 원문/첨부/event/SHA/task ID를 결합하여 검증합니다. 기존 source=DISCORD_ATTACHMENT·BLOCKED·NOT_VERIFIED 업무만 표기할 수 있습니다. 파일 경로·SHA·기존 보고가 다르거나 자체 봇 업로드이면 거부합니다. 원래 수령 이벤트 결과는 과거 이력으로 보존합니다.

TEST_ONLY를 해당 업무의 제목/payload/evidence와 파일 검사 보고에 기록했습니다. `Store.approve`는 TEST_ONLY이면 승인 대기·기술PASS로 상태가 바뀌어도 명시적으로 거부하도록 보강했습니다. 로컬 테스트에서 승인 장부가 생성되지 않음을 확인했습니다. 실제 Discord의 approve 명령이나 미술 승인은 실행하지 않았습니다. 실행 중 기존 봇은 재시작하지 않았으며 실제 업무는 계속 BLOCKED라 기존 실행기/승인 경로의 대상이 아닙니다. 보강 소스는 향후 정상 재시작 시 로드됩니다.

신규 로컬7건: 정상 대조/원본·업무 수 보존, owner·원래 보고 증거 누락 차단, SHA/저장 파일 변조 차단, 자체 업로드 대체 차단, 재대조 중복 업무/보고 방지, 상태가 바뀌어도 TEST_ONLY 승인 차단, 다른 수령 기록 보호. [테스트 로그](../Evidence/Phase3OwnerReceipt20261023/new-tests.txt).

실제9기준: 서비스 활성, 채널/Intent, 대표 일반 첨부, 실제 PNG/SHA, 단일SHA/업무, 기존 감시기 자동 생성, TEST_ONLY·승인불가 대기, 원래/최종Discord보고, 기존3건/원본보호. [실제 근거](../Evidence/Phase3OwnerReceipt20261023/actual-verification.json), [기준별 판정](../Evidence/Phase3OwnerReceipt20261023/checks.json). 기존45/8 테스트 결과를 새 결과에 더하거나 전체 재실행 결과로 보고하지 않습니다.

## 보호

[보호 근거](../Evidence/Phase3OwnerReceipt20261023/protection.json): 원본 **7,356파일 변경0·누락0**, 원본 git 상태 동일. 기존3건과 나머지 모든 업무·승인·사용량·설정·원래 수령 이벤트·다른 파일 수령 기록 동일. 대상1건의 시험 분류 및 별도 보고만 추가했습니다. 파일 원본 SHA 동일.

추가 실제 AI0·Unity0·게임/Save/승인 아트 변경0·미술 자동 승인0·Push/main 병합0·자동 시작 등록0·서비스 재시작0입니다. 기존 예약/사용량/안전 종료 모의 기능과 네 AI VERIFIED 기록도 보존했습니다.

## 재부팅·로그아웃 및 후속 설계

[읽기 전용 조사](../Evidence/Phase3OwnerReceipt20261023/startup-readonly-inspection.json)에서 해당 명령을 참조하는 작업 스케줄러·Run 레지스트리·시작 폴더 LNK 등록은 각각0입니다. 재부팅/로그아웃 실험은 수행하지 않았으며 현재 방식의 자동 시작·로그아웃 중 지속 운영은 보장되지 않습니다.

[자동 시작·안전 중지 설계](Phase3_Autostart_Stop_Design_20261023.md): 동일 Windows 사용자 로그인 트리거·최소 권한·CurrentUser DPAPI·영속 비활성 게이트·중복 방지·제한된 재시작을 권장합니다. 현재 수동 Phase3Run/Phase3Stop 절차를 문서화했습니다. 로그아웃 후에도 계속 운영하는 Windows 서비스는 별도 요구·권한/보안 설계가 필요합니다. **자동 시작 등록은 승인 전 수행하지 않았습니다.**

브랜치 `codex/discord-ai-collaboration`. 구현 커밋 `51a60ca38c07357032b7901e64afdca45e0d8f01`. 보고서/설계/근거는 후속 로컬 문서 커밋에 기록합니다. 원격 Push 및 main 병합 없음.
