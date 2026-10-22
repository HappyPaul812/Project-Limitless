https://github.com/HappyPaul812/Project-Limitless/issues/52

## 2026-10-22 바울 대표 최종 승인: Fox Run6 v7 게임 제품 연결

**사용자 직접 승인 문구:** “Fox Run6 v7도 이대로 최종 승인..”
**현재 판정:** `FOX_RUN6_V7_USER_ART_MOTION_APPROVED / GAME_APPLICATION_PENDING / REMOTE_MAIN_UNCHANGED`. 승인은 **미술·모션** 최종 승인으로, 실게임 런타임 반영·GitHub 병합 완료가 아님.

### 승인 실물·근거
- ZIP: `F:\Downloads\Limitless_Beast_Fox_Run6_v7_Frame03_ForelegFix.zip`
- Codex 실 ZIP SHA256: `de6139858b2f5de95c9e8256a350f0e45744f15e404efb00161688c88e1c0030` (납품값 일치)
- CRC/Manifest **26/26**, SHA256SUMS **27/27**, PNG 알파 bbox CSV **6/6**, Strip/Atlas 소스 픽셀6/6 PASS.
- Fox v5 ManifestFix Frame00·01·02·04·05 **PNG 바이트5/5 보존**; Frame03 국소 6,275 RGBA픽셀 복원. **v6 전신포즈 교체 미술반려본은 절대 채택하지 않음**.
- 격리 Unity **12FPS, 실측 11.95FPS, 128px/40px, 양방향4루프**. Frame03 앞다리 흰 쐐기/분리 접합 제거·02→03→04 동작 자연스러움 개선 **Codex 미술 PASS 권고**, 바울 대표 **전체 Run6 모션 최종 승인**. 4배 확대의 미세 밝은 털띠는 확인된 잔여 관찰이지만 대표가 현 상태로 수용.

### Codex 실행 — 별도 안전 worktree
1. GitHub `main`, 담당 인계, 기존 Wolf Run6 v5·Bear Run6 v4 실제 게임 연결 방식을 우선 확인. `codex/fox-run6-v7-apply-20261022` 별도 worktree를 사용하고 로컬 원본 Unity, HEAD/index, dirty/untracked(최근 Codex 보고 862건이나 반드시 작업시점 새 실측), Save/UserData, Editor 포커스와 Scene 보호 상태를 읽기 전용으로 기록.
2. 승인 ZIP 실 SHA256/CRC만 필수 재확인한 뒤, 미술 표적 QA는 이미 완료됐으므로 **6프레임 새 아트를 다시 수정하거나 전체 검수를 반복하지 말 것**. 정확한 게임용 Run6 신규 PNG/meta·필요한 리소스 등록·동물 펫 호출 연결만 최소화하여 기존 Fox 공식/임시 관련 참조를 검토. 기존 Wolf/Bear Run6·아이콘·KeyArt·GUID/meta/Animator·Controller·장착/세이브 기능 보호.
3. Unity 변경분 검사: Fox 장착·호출·저장 복원, 6프레임 루프/12FPS, 좌우 이동, 128px/40px 가독성, 이동/공격/전투 종료의 **Fox 영향 부분만**. Bear 154PASS, Wolf 106PASS, Main22/F1 전수회귀 반복 금지.
4. 기존 원본의 dirty 파일과 충돌하면 억지 덮어쓰기/clean/reset/stash·강제 Push 금지. 안전한 clean worktree 검증 및 복원 가능한 SHA 백업 후 허가된 변경분만 사용자 **원본 게임 제품경로**에 적용. 이미 적용된 Bear/Wolf 파일 재복사 금지. 위험 충돌이면 부분 중단·구체적 blocker 보고.
5. 검증 완료 시 파일 SHA/변경 개수, 기존 참조·GUID/meta 보존, 실제 Unity QA, 원본 보호검사, branch/commit, 제품로컬 적용 여부, `Push/PR/main 병합` 여부를 **분리 보고**. `main` 직접 commit/push/강제 merge 금지. 원격 GitHub PR/병합·전체 로컬 동기화는 별도 마감 승인 절차.

관련 기록: [Fox 미술 Issue #50](https://github.com/HappyPaul812/Project-Limitless/issues/50), [Fox v7 리뉴얼 제작 보고](https://github.com/HappyPaul812/Project-Limitless/blob/docs/art-renewal/문서/00_프로젝트/Fox_Run6_v6_VisualReject_v7_Frame03_LocalForeleg_Repair_20261022.md).
