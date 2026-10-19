# Main18 정식 통합 및 Field05 승인 후보 마감 — 2026-10-19

## A — DONE / 원본 Editor 검증은 PARTIAL

바울 대표 통합 지시와 docs/planning 실행 정본을 적용했습니다. 신규 TTS/API·음성 재가공0. 전용 브랜치 `codex/tts-main18-serin10-integration-20261019`, 기능 `94a68cee54be397b16d8bacd4c3f930bad2eece3`, 기존 최종 `85633624f71bd18d5d79d495812980372c056975`. Push 및 [PR18](https://github.com/HappyPaul812/Project-Limitless/pull/18) main 병합 완료: **`6f988dad77316347be69ac3b0979237771c09ed9`**.

원격 main 기준은 기존 검증과 동일한 c263d9e였으므로 1,370 PASS/0 FAIL·Main18 실제22/22 결과를 재사용했습니다. 추가 실제 Unity Leon/현장 조사점 재생·Save/Bootstrap Continue·Main17세린11·PCM35 검증 **394 PASS/0 FAIL**, 신규 오류0(기존 Search 예외는 환경 PARTIAL). 기존 정상25 및 Catalog280 항목 불변, 승인10→Catalog290. 승인소스/원본4169 SHA 보호 재확인 후 원본 Unity 디스크에 **신규 WAV10/meta10 및 Catalog1개**만 정식 반영했습니다. 적용 전 원본Catalog와기준280의 LF동등 확인, 신규경로20개 부재확인, 적용후 worktree SHA21/21일치. 원본 Editor/OS focus 조작0, 원본 Editor Import/Runtime·물리스피커 청취는 미검증입니다. 원본 Scene·기존GUID/meta·정상WAV 보호.

| 승인10 ID | PCM/소스 | GUID/Catalog | 원본 디스크 |
|---|---|---|---|
| main18_leon_serin_01 | PASS | PASS | PASS |
| main18_ground_serin_01 | PASS | PASS | PASS |
| main18_heat_vent_serin_01 | PASS | PASS | PASS |
| main18_overheat_serin_01 | PASS | PASS | PASS |
| main18_overheat_serin_02 | PASS | PASS | PASS |
| main18_overheat_serin_03 | PASS | PASS | PASS |
| main18_overheat_serin_04 | PASS | PASS | PASS |
| main18_vibration_serin_01 | PASS | PASS | PASS |
| main18_watcher_witness_serin_01 | PASS | PASS | PASS |
| main18_watcher_witness_serin_02 | PASS | PASS | PASS |

## B — 기술 DONE / 최종 미술 승인 PENDING

다운로드 후 `F:/Downloads/Limitless_Field05_EastWest_Road_v2.zip` 589850bytes, SHA **3a0d3b91cd87a5362ebbe435c1e5acd06ffc74354eafaa41f02b4f181d27c3d3** 일치. v1 ZIP SHA/CRC 및 Ground3/Props7 재검증, v2 Road4 SHA/CRC/Manifest·알파경계 PASS. 실사용14 PNG,375 Renderer=315Ground+42Road+18Props, 신규도로12, 새Portal/선택7 추가0.

별도 브랜치 `codex/field05-eastwest-road42-v2-20261019`, 기능 **`ebb363567b3932cce8af12e315dda03362c8fd19`**, 최종 **`92746215df23e2fe57e3393a563c839dca66a29a`**. 로컬 worktree `Temp/Field05Road42Final20261019/worktree`. 실제 최종 기능416 PASS/0 FAIL + 저장Scene재로드84 PASS/0 FAIL. Main14 3곳/세린/2종 전투·35초 리스폰/도보·휠체어/Arbel물리왕복/SaveContinue/카메라Bounds16:9·4:3·21:9 PASS. 같은카메라 v1남북/회전B42/최종v2 B42 63장, 저장Scene추가21장,3단비교21구도.

갤러리 로컬경로: `Temp/Field05Road42Final20261019/Evidence/index.html`; 브랜치 내 `문서/00_프로젝트/Field05_Road42_v2_Final_Unity_QA_20261019.md` 및 `Evidence/Field05Road42Final20261019/index.html`. **Field05 원본 적용0 / feature Push0 / PR0 / main 병합0. 최종 실제 v2 화면 미술 승인 필요.** 서쪽 길끝·도로반복감·나무 겹침·기존 조사점 사각표식은 확대자료로 판단. 기존 Ground/Props/Collider/Spawn/Exit·Arbel·Field04 보호.

## C — LOCAL_SYNC_PARTIAL / 손실0

- 원본 local main `ff022e63dfe364f559fcc29819d27b123ad31127` 유지. 음성병합 origin/main/GitHub main `6f988dad77316347be69ac3b0979237771c09ed9` 확보; 이 보고 문서가 후속 병합되면 원격 SHA는 추가 변경됩니다.
- 시작시 tracked dirty319 / untracked files1001. 백업 **1368 고유파일** SHA검증, UserData **23개** 포함. 원본 Save/Settings 및 모든 기존 미커밋파일 내용 변경0(승인된 Catalog append만 예외). 실제 원본 Main18 신규20 및 Catalog1개 반영은 위 범위에 한정.
- HEAD→origin/main 변경과 사용자변경의 겹침 **464경로**, 내용다른 **5파일**. `CURRENT_STATUS.md`, `EndOfDay_Integration_Voice_LocalSync_QA_20261014.md`, `팀운영/인계/CODEX_LATEST.md`, `PLANNING_LATEST.md`, `WORLD_ART_RENEWAL_LATEST.md` (모두 문서/00_프로젝트 하위). 백업 `Temp/IntegratedCloseout20261019/Backup`, `BackupSHA256.json`, `ActualDirtySyncConflicts.json`, `ProtectionAfterIntegration.json`. 원본문서5개를 덮어쓰거나 main에 몰래커밋하지 않고 FF 중단 상태 유지. reset/clean/stash/force push0.
- 원격결과는 두 격리 worktree에서 확보. 원본 main==origin/main은 아직 아님. 이 보고/CURRENT_STATUS/인계 갱신도 원본 dirty문서가 아닌 전용브랜치에서 수행했습니다.

## OPEN / 다음 재개

1. 대표 Field05 v2 최종 갤러리 미술 승인 후 B브랜치 Push/PR/정식반영 재개(현 지시는 승인 전 중단).
2. 원본 문서5개는 사용자편집과원격본문의 의미 충돌을 병합 검토한 후 main 동기화. 원본상태는 이번에 강제로 정리하지 않았습니다.
3. 원본 Editor Import/Runtime은 포커스 보호로 미검증, 설치6000.6.5f1/저장소6000.5.7f1 차이·기존 Search/URP 오류는 환경 OPEN. 자산전수 감사의 기존 GoldenArrow/HealerMagicalProjectile GUID중복5쌍은 main bytes와동일, 신규Field05GUID충돌0; 범위밖원본보존.

이번 작업의 문서 보고만 main에 포함하며 Field05 Scene/PNG는 승인 후보 브랜치에만 있습니다.
