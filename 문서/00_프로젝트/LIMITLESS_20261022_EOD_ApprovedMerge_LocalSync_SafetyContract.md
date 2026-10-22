# LIMITLESS — 2026-10-22 승인 작업 통합·원본 로컬 무손실 동기화 계약

**사용자 직접 지시:** “오늘은 여기까지 하자. 오늘 한 작업들 합치고, 로컬 동기화까지 해줘..”  
작성 주체: 기획 / 브랜치 docs/planning. **상태: 사용자 마감 통합 승인·Codex 실제 실행 대기.** 이 문서 발행 자체는 GitHub main 병합이나 원본 Windows 로컬 동기화의 완료 증거가 아님.

## 0. 통합 사전 검증
- 2026-10-22 기획이 확인한 원격 main: **510c021b4159167e211be60ca90e1cb7328bafe9**. 공개된 열린 PR **0건**. 최근 Bear/Fox Codex 로컬 제품 브랜치는 원격 목록에 없어, 로컬에서만 존재하는 커밋을 원격에서 바로 병합할 수 있다고 가정 금지. Codex 작업 직전에 main HEAD·PR·로컬 HEAD를 새로 확인한다.
- 어제 EOD PR #48 기능·#49 문서 및 로컬 SYNC_COMPLETE 기존 기록/백업을 먼저 확인. 전날 Wolf Run6 v5, Main22/F1/서리들쥐, 음성/필드 등 이미 병합한 변경을 또 가져오지 않는다.
- 임시 작업브랜치/격리 worktree: **codex/eod-integration-20261022**. main 직접 commit/Push·force/clean/reset 금지.

## 1. 이번 EOD 제품 allowlist — 승인·검증된 것만

### A. Bear Run6 v4 — 사용자 미술 최종승인·제품 원본 디스크 반영 완료, 원격통합 대기
- 승인 ZIP: F:\Downloads\Limitless_Beast_Bear_Run6_v4.zip ; SHA256 **4a9fedd54d58999def05b92839a1f00bc1d001375746e572f5d88bcade44977a**.
- Codex 제품 브랜치 **codex/bear-run6-v4-apply**, 제품 커밋 약식 **5b89ac92**, 검증문서 **6e86d05d**. 실제 원본 게임 파일에 이미 안전 적용·격리 Unity **154 PASS/0 FAIL**. 기존 Bear 파일 재복사·154건 전체 재QA 금지.
- 실제 커밋 diff에 있는 Bear PNG/meta·펫 소환/장착/저장 등 필요한 승인 범위만 새 main 기반 통합으로 반영. 기존 Bear 아이콘·KeyArt·GUID/meta/Animator/Controller·승인 Wolf는 변경금지. [Issue #51](https://github.com/HappyPaul812/Project-Limitless/issues/51).

### B. Fox Run6 v7 — 사용자 미술 최종승인·제품 원본 디스크 반영 완료, 원격통합 대기
- 승인 ZIP: F:\Downloads\Limitless_Beast_Fox_Run6_v7_Frame03_ForelegFix.zip ; SHA256 **de6139858b2f5de95c9e8256a350f0e45744f15e404efb00161688c88e1c0030**. v6 전신 교체 반려본은 사용 금지.
- Codex 로컬 브랜치 **codex/fox-run6-v7-apply-20261022**, 제품 커밋 약식 **332d9932**, QA 문서 약식 **5167841a**. 제품 파일 4개: 신규 Fox_Run6_v7.png + .meta, BeastCompanionSystem.cs (Fox 소환·셀규격 코드), BattleActionPresenter.cs (설명주석). 원본 게임 파일 이미 반영. 격리 Unity **162 PASS/0 FAIL**: 장착·호출·6프레임/양방향 이동·저장복원. 162건 전체 재QA/재복사 금지.
- **주의:** Bear와 Fox의 공통 펫 소환·저장 코드가 겹칠 가능성이 크다. **최신 main 위에 Bear 안전 변경을 먼저 반영하고 그 결과에 Fox 변경을 의미 병합**하여 양쪽 Bear/Fox와 기존 Wolf의 소환/저장 ID가 동시에 유지되는지 검사. 충돌 C#를 한쪽 버전 전체로 덮어쓰지 말 것. 필요없는 주석 변경은 생략 가능. [Issue #52](https://github.com/HappyPaul812/Project-Limitless/issues/52).

### C. 서릿길 마을 v10 Candidate9 — 사용자 미술 최종승인, 게임 자산 승격은 안전 기술 게이트
- 승인 ZIP: F:\Downloads\Limitless_Chapter3_FrostwayVillage_Environment_GameArt_VariantTiles_v10.zip ; SHA256 **f50e312eea52a6bca8272d78532224e2c66a6b40c4b1c8be5cd164b52b2bcee5**.
- Earth 01·02·03 / ThinFrost 01·02·03 / Gravel 01·02·03 **9종만 최종 미술 승인**. PNG9 변경금지. 기존 게임 PNG **49종** 및 Earth/ThinFrost 후보6 SHA 보존 완료. 실 격리 Unity 5x5/8x8·3화면비·통행 부분 QA PASS.
- 이번 마감에서 허용: 기존49 원본 PNG/Scene/Tilemap/GUID/meta 무변경 원칙 아래 **승인 Candidate9을 별도 게임용 새 Sprite/Tile asset 경로로 안전 등록하는 최소 범위**. 필요한 PPU/Pivot/alpha/seam/GUID/Sorting·리소스 참조의 변경분 QA 통과한 파일만 PR에 포함.
- **금지:** 승인 후보를 기존 Tiles 원본 49파일에 무단 덮어쓰기, 기존 Scene·Tilemap 전역 치환·자동 배치, Main23 NPC/퀘스트/SafeZone·접근성 마차 신규 구현. 기존 **OPEN16 미해결16/완화0/해결0** 유지, 문제분류·근거 없이 해결 선언 금지. 승인된 새 자산 등록이 안전하지 않다면 **Village 제품 변경만 이번 EOD에서 보류**하고 승인 상태·원본 ZIP·문서는 통합. [Issue #45](https://github.com/HappyPaul812/Project-Limitless/issues/45).

### D. 부서별 문서 선별·의미 통합
- docs/planning: 오늘 Fox·Bear 승인/제품QA와 Village Candidate9 사용자 미술승인, OPEN16 및 Main23/24 게이트, 본 EOD/최신 인계·HISTORY.
- docs/art-renewal: Bear v4·Fox v7 납품QA, Fox v6 반려 사유, SHA/Frame03 보호/ManifestFix 기록. 공식 승인 최종본만 현재 상태 표시.
- docs/art-new: v9→v10 Earth·ThinFrost·Gravel 후보9 제작·검수·보존 기록, 사용자 승인·OPEN16 잔여 문서.
- docs/tts: 최근 TTS 제작 사실과 운영 표준/사용자 KEEP WAV7 상태를 **기술 문서로만** 선별. 최신 TTS 정본 보고에 따르면 세린67/67·태온37/85 = 104/152 READY이며 전량 청취승인 아님. 사용자 KEEP Main21 WAV7은 재생성·Unity반영 금지.
- docs/bgm, docs/sfx, docs/video 등은 당일 확정·비기밀 변경문서가 있으면 선별, 미완 자산·음원 자체는 자동 제품 반영 금지. 각 브랜치를 통째로 main에 merge하지 말고 **원본 고유 변경/이미 반영된 문단을 지키는 파일별 의미 병합**.

## 2. EOD 제품 제외
- Main23 실 Scene·퀘스트·NPC·새 맵 전면 적용, Village OPEN16, Main24 6칸 Skill Save persistence #46, 임시 Main22 대사/경제/보상 확정, 미승인/미청취 TTS·7 KEEP WAV, BGM/SFX 샘플, 미술 반려 Fox v6.
- 사용자 개인 데이터/실험로그·API 키·고유 파일·개인 로컬 경로가 포함된 자료 GitHub 공개 금지.
- 승인 Wolf/이전 EOD PR48에 이미 들어간 제품58파일 중복 cherry-pick·전수검증 금지.

## 3. 사용자 원본 완전 보호 실행 순서

1. **읽기 전용 인벤토리**: AGENTS, COMMON_RULES, CODEX_WORKFLOW, CURRENT_STATUS, CODEX_LATEST, PLANNING_LATEST, 전날 EOD 계약·proof, 본 계약을 읽고 실제 Windows 원본 **F:\study\codex\Project-Limitless** 의 Git HEAD, index/staged, dirty tracked/untracked/ignored, 기존 로컬만의 commit, worktree 및 Unity Editor/Play/포커스를 확인. 사용자가 보낸 마지막 보고 원본4402파일·미커밋860개는 과거 시점이며 이번 EOD는 새로 실측.
2. **무손실 SHA 백업**: 원본 외 복원 가능 Temp/EOD20261022 위치에 모든 기존 사용자 수정·미추적·Save/UserData·개인 Editor/Settings 및 Bear/Fox 이미 적용된 파일의 사전 SHA/backup/manifest 저장. .git·비밀값의 외부 전송 금지. 이후 예상 충돌 경로를 분류. 원본에 강제 checkout/rebase/stash/clean/reset --hard/Editor 자동 Play 금지.
3. **격리 통합**: 최신 remote main에서 별도 worktree/브랜치 만들고 실제 Bear/Fox 커밋 계보 및 제품 파일 diff·바이트 확인 후 Bear→Fox 순으로 공통 펫 코드 의미 병합. 기존 game-approved 파일·GUID/meta 참조 보호. Village는 새로운 승인9 tile asset 안전 등록 가능한지 확인하고 조건 미충족 시 그 부분만 EOD 보류. 각 담당 문서 고유 최신 기록을 선택 반영, 완결된 과거 문서 역사 삭제 금지.
4. **새 변경분만 QA**: git diff --check, C# 컴파일·참조, Fox/Bear 공통 코드 merge로 생긴 상호 영향과 새 Village tile assets만 최소 격리 Unity 점검. Bear 기존154/Fox 기존162/Wolf106·Main22/F1 등 전체 회귀 반복하지 않는다. Search 초기화 환경 예외1은 기존 이슈로 별도 표기. 검증되지 않은 원본 Editor 자연Import/직접 손플레이 PASS 주장 금지.
5. **원격 PR 방식**: 정상 변경분을 codex/eod-integration-20261022 branch에 commit/push → PR(기능 및 문서 분리 가능) 생성 → 파일 allowlist·비인가 diff0·QA 결과·기존 main 최신 반영 확인 → **오늘 바울 대표 EOD 병합 승인 범위에서 PASS한 PR만** main으로 병합. main 직접 commit/push 또는 force merge 금지. 문제가 있으면 해당 범위를 HOLD, 나머지 안전 승인을 계속 진행.
6. **실제 로컬 동기화**: 최종 GitHub main SHA를 fetch한 뒤 깨끗한 별도 복사본에서 fast-forward 리허설. 기존 local main 독자 commit, dirty/index, 승인 Bear/Fox 실제 적용파일과 새 main의 파일바이트 충돌이 없는 것 확인 후에만 사용자 원본 local main을 **비파괴 FF 방식**으로 최신 origin/main과 맞춤. 불가능하면 임의 reset/checkout/overwrite 금지, 원본 현상 보존하고 LOCAL_SYNC_PARTIAL/SYNC_SAFE_BLOCKED 보고. 이미 원본에 있는 동일 바이트를 불필요하게 재복사하지 말 것.
7. **완료 증명**: GitHub main == origin/main == 사용자 원본 local main의 세 SHA, staged/index 변화, dirty/untracked 실측 전후 및 허용 변동 원인, 사용자 SHA불일치0, Save/UserData·Editor·승인Wolf/Bear/Fox·TTS WAV·기존 자산 무변경, 신규 코드/자산 Unity delta PASS, PR URL과 merge SHA를 최종 확인. 변경원인 없는 사용자 데이터 1바이트라도 바뀌면 STOP/복원, 완료 주장 금지. CURRENT_STATUS·CODEX_LATEST·EOD 실행보고에 완료 사실·미완·증거/HISTORY 보존.

## 4. Codex 필수 증거
- Temp/EOD20261022/BackupManifest.json
- Temp/EOD20261022/ApprovedChangeAllowlist.json
- Temp/EOD20261022/SharedCompanionCodeMergeProof.md
- Temp/EOD20261022/IntegrationDeltaQA.json
- Temp/EOD20261022/UserOriginalProtectionProof.json
- Temp/EOD20261022/PRMergeCloseout.md
- Temp/EOD20261022/OriginalLocalSync_Proof.json

**SYNC_COMPLETE 조건:** 오늘 실제 통합 가능한 승인제품·담당문서를 PR 방식으로 main 병합 + 원본 local SHA 3자 일치 + 사용자 원본/Save/Editor 보호 SHA 차이0 + 실 QA·PR 근거 확보. 일부 Village 자산 등록을 충돌/기능미확정으로 보류한 경우 **PARTIAL_APPROVED_SCOPE**로 분리 기록. 원본 sync 미완은 **REMOTE_MERGED_LOCAL_SYNC_PARTIAL** 또는 **SYNC_SAFE_BLOCKED**로 정직하게 보고. 작업이 막힌 부분은 복구 가능한 상태 그대로 보존하고 내일 인계.
