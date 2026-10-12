# 2026-10-13 Git 통합·Rock B v2·Field04 검수 총괄

> 최신 Git 규칙: 이전 main 직접 Push 명령은 [Codex 작업 브랜치 v2 안내](Git_BranchOnly_Integration_20261013.md)로 대체됩니다. 검수 결과/기존961c90d 이력은 보호하며 새 원격main73c37bf를 별도 worktree에서 보존 병합했습니다.

**Git 통합 준비 완료 / 두 아트 검수 완료 / 원본 정식 적용0 / TTS 호출0 / Push0.** Rock B v2는 기술 보완을 통과했고 최종 미술 승인 대기입니다. Field04는 납품표 보완·RockNorth 보류와 기존 경계/Spawn 문제가 있어 전체 PARTIAL입니다.

## Git 결과·Push 경로

- 시작 원본 main `ff022e63dfe364f559fcc29819d27b123ad31127`, 사용자337항목/822파일 미커밋. 원본 main·파일·index는 그대로입니다.
- 첫 fetch 원격 `927f519`(전용4커밋), 로컬 전용2커밋. 별도 `codex/integrate-main-20261013` / `Temp/GitIntegration20261013/main-merge`에서 원격 기반 A/B만 순서대로 cherry-pick, 충돌0/range-diff 동일.
- 진행 중 원격 `9256687f7f380e15ffb9fb75f37af9675a3d58cc`로 추가2커밋 전진. 이를 충돌 없이 merge, 원격 총6커밋 모두 보존. 최신 원격 main 조상 검사/Unity diff0 확인. 금지한 reset/stash/pull-rebase/force/push0.
- 마지막 fetch의 `405ce12b71a52cb94ce79dd313eed68861839537`(회복/부활 아이템 제안 문서1개)도 Chore merge `4446d787e7ccf6f073f4c912deb5e742ba923c4e`로 보존. **최종 원격 전용7커밋 포함**, 최신 origin/main 조상 검사 성공/Unity diff0입니다. 코드/아이템 구현0.
- A 재현 `93ccd6d3dfac9478016361882c65419588c93900`, B 재현 `a759f66e8880980b49b5b8bfc68f3a7ebe8fdc7f`.
- 원격 추가 보존 Merge `de55c2b` / Git 보고 `853a4fd`.
- Rock 검수 Commit `975fb636a7a7a7527bc704c21019853301d83bac`.
- Field04 검수 Commit `dcb1f14524de6cfb295cbf5e5d6cfb83d24229a5`.
- 본 총괄·CURRENT_STATUS·CODEX_LATEST를 포함하는 마지막 Docs Commit SHA는 최종 보고/로컬 Git 이력으로 확인합니다.

사용자가 **검증된 통합 worktree에서** 직접 실행할 명령:

```powershell
git -C "F:\study\codex\Project-Limitless\Temp\GitIntegration20261013V2\main-integration" push origin HEAD:refs/heads/codex/integrate-main-20261013-v2
```

원본 dirty checkout의 단순 git push와 다릅니다. **Codex 작업 브랜치만** Push하며 main은 변경하지 않습니다. 동일 작업 브랜치에 원격 신규 이력이 있으면 force 없이 거절됩니다. main 최종 병합 및 원본 main 동기화는 사용자 결정과 수정 보호를 포함하는 별도 작업입니다. Codex는 위 명령을 실행하지 않았습니다.

## 검수 결과·Preview

| 대상 | 완료한 확인 | 보류·미검증 |
|---|---|---|
| Rock B v2 | ZIP8/PNG3 CRC/decode, Manifest hash, 고립14 제거/본체3537 RGBA 보존 및 y=-19 이동, 실제 PPU128/Center·원래 배치/Collider/RenderTexture; alpha 접지차0.001875 | 사용자 최종 미술 승인/독립 권리 증빙/원본 Import·정식 적용·적용 후 전체회귀 |
| Field04 v1 | ZIP27/PNG21, 환경12 hash/Optional3 별도 QAhash, 타일27 edge 비교, 대응363/363, Import16 GUID충돌0, Literal/제안 실제 GPU Preview, 주요 Quest/이동/전투/Continue/Camera/휠체어 | 나무2그룹5참조의 Big/Medium 역대응, RockNorth 크기·발높이, 사용자 미술 승인/서쪽 건조 방향성/권리/손 플레이 |
| 기존 원본 문제 | 서쪽 Spawn native 겹침(-0.01631531)과 북쪽 non-exit Collider틈 관측. 5 Story지점·동서 주변 통로의 실제179waypoint, Camera3화면비×4방향 확인 | 정확한 서쪽 Spawn 무겹침은 FAIL 유지. 북쪽 틈의 실제 사용자 입력 Bounds이탈은 NOT_VERIFIED. 게임을 임의 수정하지 않음 |

Preview:

- [Rock 실제 기존/v1/v2 비교](Evidence/ArtInspection_20261013/rock_unity_comparison_contact_sheet.png)
- [Field04 실제 원본/Literal/제안 비교](Evidence/ArtInspection_20261013/field_unity_comparison_contact_sheet.png)
- [Field04 개별 Contact Sheet](Evidence/ArtInspection_20261013/field_individual_contact_sheet.png)

세부 정본: [Git 통합](Git_Main_Integration_PrePush_20261013.md), [Rock B v2](Field03_RockB_v2_Inspection_20261013.md), [Field04](Field04_Art_v1_Inspection_20261013.md).

## 실제 QA 경계

- Main12는 앞4목표 fixture 후 실제 마지막 Field04 도착만 확인. Main13은 토양→물길→세린→Story UI승리6공격→Arbel→레온 완료 PASS.
- Continue2회는 실제 Bootstrap 버튼·좌표/Quest 복원. Field03↔Field04↔Arbel↔Field05 handler/Spawn 왕복 PASS. Field04→Field05 직접 연결을 새로 만들지 않음.
- 일반 열풍매7공격/그을음들개8공격 UI승리, 30초 부재/35초1개 재생성/중복0 PASS. Battle은 level30 격리 fixture·TimeScale8, 게임 밸런스/실제 OS 입력/전직업은 미검증.
- 첫 QA 자체 JsonUtility 오류와 서쪽 정확한 Spawn 경로 실패를 보존. 이미 통과한 Rock/이미지/5 Story 경로를 반복하지 않고 남은 단계만 재개. 실행별 PASS62/107/177, 최초 FAIL 보존, 마지막 재개 FAIL0. 원시 PASS346/중복 제외279; 단일 전체PASS로 주장하지 않음.
- 세 실행 compile error0, 마지막 신규 Runtime오류0. 기존 UnityEditor.Search 예외가 남아 전체 PARTIAL. 원시 Unity 로그는 Temp에 보존, 문서용 checks/console은 의미·순서 유지하면서 LF/후행공백만 정리.
- 원본 Editor Bootstrap/Edit/비포커스 상태 유지. 기존 격리 Client 재사용, 원본 창/Scene/Import/Play/OS 포커스 조작0. 별도 AuditSave/Settings·음소거 사용.

## 보호·다음 작업

[보호 JSON](Evidence/ArtInspection_20261013/protection.json): 사용자822파일/보호그룹3929, 중복 제외 **4,750파일 전후 SHA 동일·삭제0**. 원본 status 원시 byte 동일, HEAD ff022e6 유지. 원본 Field03 375/376·RockNorth 기존1, 모든 원본 Scene/meta/ThirdParty/승인PNG/Save/TTS/BGM·TTS 감사 보호. 두 납품 ZIP도 해시 유지. 이번 커밋은 문서/검수 Preview/Evidence만입니다.

**다음:** 사용자가 Rock v2 실제 비교를 검토해 최종 승인하면 단일 Sprite 참조 교체를 별도 작업으로 진행할 수 있습니다. Field04는 나무표 수정·RockNorth 보완/유지안·실제 색감/가독성 검토가 먼저이며 제안362개도 자동 채택하지 않습니다. Optional3은 별도 승인 전0입니다. 기존 서쪽 Spawn/북쪽 경계 수정은 원본 보호를 지키는 별도 기술 범위로 분리합니다.

Chapter3 투사는 최신 원격9256687의 **공격형 직업 컨셉 확정 / 분쇄 베기·맹공 이름·효과·수치·ID 미승인**을 확인했습니다. 공격80%·공격적 유틸20%는 감각적인 방향이며 게임 배율이 아닙니다. 파고들기 미채택, 기존 난도/회심의 일격/회오리 베기·기세·전열 보호 유지. **코드/SkillDefinition/UI/Save 추가0**입니다.

TTS `M18_Serin10_Staged.py` SHA `e358511d852a014f0c2196754debe523cd55336bda8a11d18717a3278276b10f` 불변. 실행/새 API/음성 생성0, 기존 정상25와 Main17 세린11을 포함한 승인 음성 byte 보호. 작업 완료가 아트·TTS 실행 승인으로 확대되지 않습니다.
