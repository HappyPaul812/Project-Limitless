# NewArt CLI 실패 조사·진단 보완 (2026-10-23)

**실제 AI 추가 호출0 / 과거 실패 미검증 유지 / 내부 근본 원인 미확정 / 진단 누락만 수정 / 중지**

## 확인된 실패 지점과 미확인 사항

NewArt 업무 `L-cf2b95ce985346d8`는 subprocess가 생성되지 않아 OSError가 난 경우가 아닙니다. 기존 실행기가 CompletedProcess를 받아 **종료 코드1**을 기록했고 stdout0자/stderr330자/JSONL0/완료turn0/최종응답 없음으로 거부했습니다. 응답 파서 이전의 실행 실패가 선행했으며, 첫 JSONL 이벤트가 관측되기 전에 프로세스가 종료됐습니다. CLI 내부 초기화와 네트워크 요청 중 어느 지점인지는 **미확정**입니다. 모델 요청이 서버에 도달했다거나 추론이 전혀 발생하지 않았다고 단정하지 않습니다.

TimeoutExpired 처리가 아닌 EXIT_NONZERO 경로이므로 **90초 강제 종료 실패는 아닙니다**. 스키마·진단 파일 시각이 모두 21:01:59 KST로 나타나 매우 이른 종료와 부합하지만 정확한 경과 시간은 과거 기록에 없습니다. 입력29067bytes는60KB 상한 안이고 stdin으로 전달되어 Windows 명령줄 길이 제한의 입력 인수로 쓰이지 않았습니다. 문서 경로도 모델에 전달한 참고 텍스트이며 CLI가 그 GitHub 경로를 파일로 여는 구조가 아닙니다.

stderr 원문은 기존 비밀 보호 정책으로 폐기돼 복구할 수 없었습니다. 실패 시각 12:01:56~12:02:02 UTC의 기존 로컬 로그 DB를 **읽기 전용·고정 유형 집계**로 확인했으나 해당 child CLI의 오류에 귀속할 근거는 없었습니다. 로그 원문/인증정보/개인정보를 추출·게시하지 않았습니다. 이번 로그 DB 일반 sandbox 읽기는 실패하고 승인된 읽기 경로에서는 가능했지만, 이는 현재 조사 환경 차이일 뿐 **과거 NewArt 원인이 권한 문제라는 증명은 아닙니다**. 직전 NewArt의 바깥 실행은 기본 sandbox였고 과거 성공 실행의 바깥 권한·정확한 환경값은 child 증거에 기록되지 않아 동일성을 확증할 수 없습니다. 앞선 진행 안내의 권한 차이 언급 역시 원인 확정이 아닙니다.

따라서 인증 문제·quota·네트워크·파일 권한·스키마 로딩·설정 문제 중 하나로 임의 결론내리지 않습니다. 실제 원인이 해결됐다고 선언하지 않습니다.

## 성공·실패 실행 비교

|항목|Planning 자동 인계 성공|Codex 자동 인계 성공|NewArt 실패|
|---|---|---|---|
|공통 실행기|real_roles.run_once|동일|동일|
|CLI 사전 버전/인증|0.163.0-alpha.11.2 / ChatGPT|동일|동일|
|입력 UTF-8|23040bytes|16742bytes|29067bytes|
|종료/출력|exit0 / stdout974자 / stderr0|exit0 / 정상 JSONL·최종응답|exit1 / stdout0 / stderr330|
|완료turn|1|1|0|
|검증|PASS|PASS|NOT_VERIFIED|
|timeout/종료|90초 상한·정상 종료|동일|90초 상한·비정상 반환, timeout 아님|
|작업폴더|data/ai-ready/auto63/planning-empty|auto63/codex-empty|art-roles/NewArtAgent-empty|
|정확한 실행 당시 경과 시간·환경값|미기록|미기록|미기록|

각 폴더는 `F:/study/codex/Project-Limitless/Temp/DiscordAICollab20261023/Worktree/Tools/DiscordAICollab/` 아래입니다. 보호된 실제 게임 프로젝트를 실행 cwd로 사용하지 않습니다. register-discord.ps1이 tool root로 이동하며, subprocess의 launcher cwd는 별도 지정 없이 이를 상속하는 코드입니다. CLI `-C`는 safe_root가 정규화한 **절대 격리 폴더**입니다. 스키마·최종응답 경로도 절대 경로입니다. 세 response-schema.json SHA는 동일합니다.

공통 인수는 `codex exec --json --ephemeral --ignore-user-config --sandbox read-only --skip-git-repo-check -C <격리 절대경로>` + 동일한 `-c` 보호 설정 + `--output-schema <절대경로> --output-last-message <절대경로> -`입니다. 입력은 stdin, timeout90, CREATE_NO_WINDOW를 사용합니다. 강제 ChatGPT 로그인/openai provider/approval never, shell·snapshot·apps·browser·computer·multi_agent·MCP·plugins·hooks·web 비활성, project_doc_max_bytes0/reasoning low가 같습니다. **명시적 모델 인수는 없고 과거 실제 해석 모델명은 미기록**입니다.

phase2.py의 성공 commit fcb2aba2 대비 원문이 그대로여서 명령 생성·환경 필터는 동일합니다. 자식은 허용된 OS 환경 이름만 전달받으며 Discord/GitHub/API 키와 URL override를 받지 않습니다. 과거 정확한 실행파일 경로와 환경값은 저장되지 않았으므로 비교 JSON의 argv는 **공통 코드와 보존 파일로 재구성한 명령**이며 당시 원문 캡처라고 주장하지 않습니다. 현재 CLI 버전·ChatGPT 인증 종류만 비추론 방식으로 다시 확인했습니다. auth 파일 내용은 읽거나 출력하지 않았습니다.

공식 [비대화형 실행 문서](https://learn.chatgpt.com/docs/non-interactive-mode)의 JSONL·최종 메시지 파일 방식을 유지합니다. 옵션 우회나 다른 provider/API로 변경하지 않았습니다.

## 확인된 진단 결함의 최소 수정

실패 시 stderr 길이와 제한된 분류만 남고 argv/cwd/경과시간/생성 단계가 누락됐던 **관측 정보 부족**을 수정했습니다.

- `Tools/DiscordAICollab/collab/exec_diagnostics.py`: 내부 생성 argv, launcher cwd, timeout, stdin 바이트, CLI 버전·인증 종류 확인 여부, 환경 **이름만** 기록. stderr는 메모리에서 고정 오류 코드로 분류하고 원문·환경값·prompt는 진단 manifest에 저장하지 않음.
- `collab/real_roles.py`: 기존 실행기에서 사전 검사 결과를 재사용하고 launch-manifest/경과시간·관측 단계를 추가. 로컬 준비 실패, 프로세스 생성 실패, 이벤트 전 반환, thread/turn 관측, timeout을 구분. 진단 분류는 PASS/FAIL에 영향을 주지 않음.
- `tests/test_exec_diagnostics.py`: fake subprocess·임시 DB만 사용. 실제 CLI/모델/Discord 호출 없음.

기존 parser/SCHEMA/승인 슬롯/역할 경계/환경 필터/sandbox/timeout/자동 중지/자동 재시도 금지는 변경하지 않았습니다. 이전 성공 자동 인계는 재구현하지 않았고 과거 NewArt NOT_VERIFIED·CONSUMED 슬롯도 그대로입니다. 권한을 확대하거나 보호 검증을 완화하는 수정은 하지 않았습니다.

## 로컬 검증·보호·재시험 가능 여부

**신규 진단8 PASS / FAIL0**, 기존 자동 인계 핵심 회귀1 PASS / FAIL0 = **선별9 PASS / FAIL0**, Python 구문검사 PASS. 기존 전수8/39/Unity QA를 재실행하지 않았습니다. 빈 stdout·exit1과330자 모의 stderr, NewArt 실패 시 Renewal 중지, Renewal 동일 처리, 정상 두 담당·manifest, 권한 유형, OSError 생성 실패, timeout, thread만 관측, 원문/환경값 미노출을 확인했습니다. 모의 재현은 과거의 **관측된 실패 형태**를 재현한 것이며 실제 내부 근본 원인을 재현한 것이 아닙니다.

이번 실제 AI 추가 호출 **0회**, 유료 API 호출/전환/재시도/추가 결제0. 운영 usage 신규0행, 기존10업무·NewArt 실패 evidence 동일, NewArt 슬롯 CONSUMED 보존/Renewal 슬롯 미생성. 원본 **7356파일 SHA변경0/누락0**, 원본 git상태 동일. Unity·Save/UserData·PNG·승인 자산·게임제품·미술승인·Push/main merge 변경0. 기존 기획→Codex VERIFIED 및 MOCK 보존, SFX 비활성 유지. Discord 추가 게시·봇 재시작0.

**실제 재시험은 아직 성공 가능성을 검증하지 못했습니다.** 향후 새 승인을 받으면 보완 진단으로 제한 재시험을 준비할 수 있으나, 현재 ArtRolesOnce 재실행은 기존 JOB_KEY/CONSUMED 슬롯으로 차단됩니다. 이번에는 새 슬롯 생성·소비 상태 초기화·자동 재시도를 하지 않았습니다. 바울 대표의 별도 실제 재시험 승인 이후 새 계획/별도 슬롯을 마련해야 하며, 실패 시 다시 중지합니다.

브랜치 `codex/discord-ai-collaboration`, 구현 commit `1129756c517539fbdb12730c6d915b2a7791bd29` (로컬). 근거 `Evidence/NewArtExecForensics20261023/comparison.json`, `protection.json`. **DIAGNOSTIC_GAP_FIXED / ROOT_CAUSE_UNCONFIRMED / REAL_RETEST_NOT_RUN / REMOTE_NOT_PUSHED**.
