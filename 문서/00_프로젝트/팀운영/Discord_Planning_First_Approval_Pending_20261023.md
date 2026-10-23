# PlanningAgent 첫 실제 시험 준비·한국어 조회 응답

현재 단계는 **WAITING_FOR_OWNER_APPROVAL**입니다. 실제 모델 호출0·승인슬롯미등록·신규실제AI업무0. 이전 Codex 우선 권고는 바울 대표의 최신 지시에 따라 대체합니다. 첫 시험은 PlanningAgent의 최신 GitHub 정본 요약만 허용하며 Codex/메인아트/리뉴얼 실행은 코드에서 차단했습니다. 첫 성공 후 확장은 별도 승인 단계이고 자동 연쇄 호출하지 않습니다.

## Discord 실제 표시 확인

status/pending을 JSON 원문 대신 한국어로 표시합니다. 완료/검토대기/미검증 건수, 담당, 업무ID, MOCK/실제응답/과거연결시험을 구분합니다. MOCK에는 ‘실제 미술 승인에 사용하지 않습니다’를 표시하고 읽기전용 AI 요약도 실제 아트 승인으로 사용할 수 없도록 approve 검사를 보완했습니다. 원래 상태값/명령9개/SQLite 스키마/작업 레코드는 유지합니다.

검증용 봇 프로세스만 재시작해 동일 DB로 접속했습니다. 바울 대표가 두 명령 모두 한국어 정상 표시를 확인하셨습니다. 접속 READY와 기존 실제3명령 응답 영수증도 보존합니다. 준비 보고는 Discord message1563139223006216203에 게시했습니다. 일반 채팅에 고정 준비 문구를 보내는 것은 실제 AI 응답이 아닙니다.

SQLite integrity_check=ok. 기존 DONE2 / APPROVAL_PENDING1 / NOT_VERIFIED1 보존. 모의 승인대기는 그대로 남기되 실제 미술 승인 대상이 아닙니다. 과거 bootstrap 실패는 삭제/재시도하지 않았습니다.

## 승인할 구체적 시험

- 역할: PlanningAgent만. 최신 GitHub 정본 발췌로 현재 업무·제약·다음 검토를 한국어 요약하며 모순/오래된 문서를 보고합니다. 이미지 제작/Unity/파일수정/계정조작/미술승인 없음.
- main47ad2eecb40135b2ee28e4947d642ebdbf0bf68a와 docs/planning d73bdbf9a3162132b959f4672e88c3975b361c3c를 GitHub에서 재조회해 기존 준비 스냅샷과 일치함을 확인했습니다. 전체 연결 문서를 읽었다고 주장하지 않으며 입력 전문은 근거 PlanningAgent.plan.json에 있습니다.
- 새 계획SHA256: **e9e0e683f43456f67b7468e014699221e3c563eb63f9ba4257a466b3dbba0b38**. sha256 필드를 제외한 정렬JSON 해시입니다. 이전 계획SHA를 승인으로 재사용하지 않습니다.
- 읽기전용1회·90초·구독 CLI·600자응답 요청·결과 Discord 보고. 입력25497bytes이며 실제 tokenizer 미측정, 사전 보수 추산은 최대약2.6만입력토큰+프로토콜 오버헤드 수준입니다. 글자/시간 제한은 과금토큰 강제상한이 아닙니다.
- 별도 승인 전 grant/run 호출 없음. 정확한 역할/계획SHA/구독 사용/보고 범위에 승인한 뒤 같은 phase2 DB에10분짜리 승인1회만 기록합니다. 계획24시간 경과·정본 변경 시 다시 준비합니다. 실패/timeout도 공통1회 슬롯을 소진하고 자동 재시도0. 새DB/승인ID 재발급으로 우회하지 않습니다.

## 공식 인증·사용량·비용

CLI0.163.0-alpha.11.2, ChatGPT 구독 인증 가능=True, 추론 없는 features 설정 검사PASS 재확인. [공식 인증](https://learn.chatgpt.com/docs/auth)·[비대화형 실행](https://learn.chatgpt.com/docs/non-interactive-mode)의 저장 인증 재사용/exec JSON/schema 경로를 사용합니다. built-in provider 내부 설정 덮어쓰기0·API키전환0·계정/인증 원문 출력0. shell/apps/browser/computer/multi_agent/MCP/웹/플러그인/훅 비활성화, 자식프로세스에 Discord/GitHub/API 비밀을 전달하지 않습니다.

현재 실제추론0이므로 해당 호출 사용량0·추가유료API비용0. 준비 과정의 이 대화 사용량은 별개입니다. 계정조회시5시간96%/주간71%잔여·유료크레딧없음(계정전체 스냅샷). 한도리셋/구매0. 실제 요청 수락/모델 접근/호출별 구독 차감은 승인 후1회까지 NOT_VERIFIED이며 인증만으로 실제 과금을 입증하지 않습니다. 현재 호출 실패는 없고 과거 실패 원인은 provider override bootstrap 제한으로 보존했습니다.

## 변경분 검증과 원본 보호

변경분19 PASS/FAIL0: 한국어조회/빈대기/출처미확인표시/MOCK승인제외/실제readonly승인차단/SDK callback·queue보존6개와 실행기 가짜subprocess13개. 기존57/22/모의8점검 및 Unity전수QA 반복0. Python컴파일PASS. 실제 AI 실행·연속 역할 추론은 미검증입니다. 실제응답은 REAL_CLI_READONLY, MOCK는 별도 표시하고 텍스트를 실행명령/승인으로 해석하지 않습니다.

원본7356개 SHA변경0·누락0·git상태동일. 게임/Save/UserData/승인자산/원본Scene 변경0·SFX PAUSED·Push/main병합0. 아트 제작→Codex 검수→바울 대표 최종 미술 승인→승인범위 정식 적용을 유지합니다. 원격 GitHub에는 반영하지 않고 codex/discord-ai-collaboration 로컬 커밋만 남깁니다.

[근거 폴더](../Evidence/DiscordPlanningFirst20261023/)에 계획·재확인설정·장부요약·한국어출력예시·사용량·보호 결과를 기록합니다. 상세 실행기 설계는 [이전 준비 보고](Discord_AI_Real_Connection_Readiness_20261023.md)를 참고하되 첫 역할/계획SHA/사용량 수치는 본 문서가 최신입니다.
