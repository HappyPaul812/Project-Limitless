# 2026-10-21 승인분 EOD 통합·원본 안전 동기화

## 실행 상태

**PRE_MERGE_QA_PASS / PR_PENDING / ORIGINAL_SYNC_PENDING**. Issue #47 및 `LIMITLESS_20261021_EOD_ApprovedIntegration_LocalSync_Contract.md`에 따라 실행합니다. 이 단계에서는 병합과 로컬 동기화 완료를 주장하지 않습니다. 실행 후 실제 PR·최종 SHA·보호증거를 후속 기록합니다.

## 선별 제품과 재사용 검증

- Wolf `f0dd59dd010759760811a7a1b516eab3969d500c`:6파일. 신규 Run PNG/meta와 최소C# 연결4파일. 기존 아이콘/KeyArt/Animator/Controller/Bear/Fox 보존. 원본 디스크에 이미 있는6파일은 재복사하지 않습니다. 기존106PASS·후속최소Import PASS 재사용.
- Main22 `be7f64a3ff48a8848471166d4aefc4969080bd8b` 및 적용준비 `2631e875`:58경로 원본 allowlist에서 격리 Editor/QA 제작도구6개 제외,52파일만 제품 통합. 선행 서리들쥐13경로 포함. F1 승인PNG6/Tile3/저장Scene·5목표Quest·북문 명시선택·지정전투·안전쉼터·Save 선택필드. 기존 Main22 136/F1 64/MouseBattle40 별도재사용(중복합산 없음).
- Main22 정식 대사/DialogueID/TTS, 수치·공식 몬스터ID·EXP/drop/완료보상·경제는 **PROTOTYPE / DESIGN_NOT_LOCKED**. 새 정식 설계나 음성을 확정하지 않았습니다. Main23/24 본편/스킬저장/경제 추가0.
- 제품 commit `b53344ce`, 실제58파일·SHA·원본 source는 allowlist 참조. root BuildSettings의 개인app-ui config는 제품에 섞지 않고 기존main에 F1Scene3줄만 추가했습니다. 원본에선 개인config를 보존한 의미병합을 리허설해야 합니다.

## 문서 통합·공개 경계

docs/planning·art-new·art-renewal·tts 및 신규 기록이 있는 담당의 오늘 문서를 파일별 선별했습니다. TTS 다중프로젝트quota·429Failover·HOLD·체크포인트 재사용 표준/정직한READY64/152 현황은 문서만 통합합니다. READY는 청취/게임반영 승인이 아닙니다. 담당 전체브랜치 merge/cherry-pick0. 기존 main 고유 기록과 원본CURRENT_STATUS의 승인 서리들쥐 문단을 보존했습니다.

Codex Main22/F1/Mouse/Wolf/Village QA·OPEN16 원본CSV/좌표JSON·128px/3화면비 비교는 문서Evidence로만 보존합니다. Village의 PNG가 **Unity/Assets에 들어간 것은0**입니다. 진단 raw 로그12개는 로컬 `Temp/EOD20261021/PrivateRawLogs`와 원 소스 커밋에만 보존하고 공개본에는 합격마커/비공개 사유만 남겼습니다. 사용자 API 키/실제 Project ID/Save/UserData/백업원문/기기·라이선스정보는 공개하지 않습니다.

## 제외·내일 미결

- Village v5 기술PASS / Earth·ThinFrost·Gravel **ART_HOLD**, OPEN16 원본 확보·해결0. 타일변형 파일럿은 제안이며 승인/제작0. 본편자산/Scene/Quest 연결0.
- 청취 미승인 재캐스팅TTS/WAV/Catalog·BGM/SFX/영상·독사/Bear/Fox 신규아트·Field09잔여HOLD는 제품제외. 추가API0·TTS프로세스조작0.
- Main24 스킬Save persistence #46 및 Main23 본편 미구현. 기존 완료Field09/Chapter2/음성 중복적용0.

## 검증과 보호

실제 새 검증은 **정적38 PASS / 격리 Unity14 PASS / 신규기능 FAIL0**. 전체 기존106/136/64/40 반복0. 승인source→제품58 SHA, GUID중복/직렬화closure, 미승인제품0, 문서충돌마커/실제API키패턴0. Unity14는 저장Scene로드/스크립트누락0/Bounds·Spawn2/Tilemap·Quest5목표·Mouse16프레임참조·Wolf아이콘·이전Save 선택필드만 확인합니다. 실손플레이 재검증은 아닙니다.

승인 신규 YAML/meta에는 Unity가 생성한 빈필드 후행공백이 있습니다. 원본 승인 바이트를 바꾸지 않기 위해 그대로 보존하고 해당 출처목록을 로컬에 기록했습니다. C# 및 문서 diff검사는 일반 설정으로, Unity직렬화 파일은 blank-at-eol을 제외한 검사로 구분했습니다. 새 코드 컴파일 오류0. 기존 URP/Search/버전차이는 환경 OPEN입니다. Batch6000.6.5f1, 추적 ProjectVersion6000.5.7f1/기존 Packages 보존, 해당 구버전 Editor 재검증0.

원본 현재314 tracked dirty/595 untracked/staged0 실측. **957파일196,911,769bytes 복구백업**,6758SHA 보호 manifest/index/refs/worktree/patch 보존. 백업원문은 `Temp/EOD20261021`에만 있습니다. EditorLockfile은 시작 시 없었고 원본 Editor 조작/Refresh/Play/포커스 이동0. 원본 자연 Import·손플레이 NOT_VERIFIED.

## 통합·동기화 순서

기준 main e4e9891f →별도 codex/eod-integration-20261021 →승인제품/문서 선별→최소검증→PR→예상head 고정 병합→origin 갱신→복사본 리허설 통과 시에만 원본비파괴fast-forward. 원본에 이미 승인제품과 같은 bytes가 있는 경로는 파일을 재작성하지 않고 Git index에 승인분만 인식시키는 방식을 리허설합니다. BuildSettings 개인config와 원본문서의 고유문단은 보존해야 합니다. 보호 위반·불명확 충돌 시 해당 범위 STOP이며 강제reset/clean/stash/checkout/forcePush0입니다.

완료 판정은 후속 `OriginalLocalSync_Proof.json`의 GitHub main=origin/main=원본 local main, index=HEAD 및 사용자보호 검사로만 확정합니다. `SYNC_COMPLETE`는 아직 출력하지 않습니다.

[허용목록](Evidence/EOD20261021/SelectedChangesAllowlist.json) · [기능출처](Evidence/EOD20261021/FeatureCommitMap.csv) · [새 검증](Evidence/EOD20261021/IntegrationDeltaQA.json) · [백업요약](Evidence/EOD20261021/BackupSummary.json) · [문서선별](Evidence/EOD20261021/DocumentMergeInventory.json).


## 원본 진단로그 경로 충돌의 선별 제외

원본의 미추적 MouseCombatV2Apply_20261021/Unity.log와 공개 최소화 로그가 같은 경로를 사용하여, 해당 로그1개는 PR에서 완전히 제외했습니다. 원본 raw 로그는 백업 및 기존 경로 그대로 보존하고 기능/QA 판정자료 통합은 계속합니다. 원본 로그 덮어쓰기0.
