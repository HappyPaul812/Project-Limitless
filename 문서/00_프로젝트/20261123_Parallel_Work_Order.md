# LIMITLESS / 2026-11-23 / 병렬 부서 작업 지시

- 담당: 총괄 기획
- 발주 근거: 바울 대표 "굿모닝~ 오늘도 열심히 달려보자. 각자 일 시켜줘."
- 상태: **WORK_ORDER_GITHUB_POSTED / ROLE_EXECUTION_NOT_VERIFIED**. GitHub Issue 작성이나 댓글 게시만으로 타 세션의 자동 실행은 보장되지 않습니다.
- 작업 브랜치: `docs/planning` (원격 `main` 직접 변경 없음)

## 기준
- 마지막 실제 원본 로컬 `main`: `736df561adb408c0225ed9052c37f2d45b7e24f5` (2026-11-19 Issue #121 보고). 오늘 실제 PC 상태는 미조회.
- 검증된 원격 복구 ref: `backup/2026-11-19-safe-checkpoint` = `f64082914e114096b6b9393263e7a36f7d829a55`.
- 원격 `main` = `bfd281545632877325f5d21e04cefb924042c465` (과거 시점이며 로컬 최신 제품기준으로 착각 금지).
- 승인 Unity `6000.6.5f1`. 원격 main의 구 `6000.5.7f1`로 되돌리지 말 것.
- #85 HOLD, Discord #66 HOLD, 개인 Save/UserData/사용자 미커밋·미추적·기존 승인 PNG/WAV 및 원격 main 보호.
- 11/19 아르벨 NPC 50PASS, 불씨망령 전투30PASS/전투불능75PASS, 눈보라 KeyPose/Idle8v2/Attack4/Hit4/Defeat4 미술 승인·격리 임포트 PASS 재사용. 전리품 6개 아트 승인/본게임 미등록, 눈보라 F4/F5 정식 등록·실전 보류.

## 발주표
| 역할 | 이슈 / 작업 | 진행 및 승인 게이트 |
| --- | --- | --- |
| Codex | [#122](https://github.com/HappyPaul812/Project-Limitless/issues/122) 승인 전리품6 Item/Drop 계약 조사·조건부 최소등록 준비 | 정식 StableID 미확정이면 ID 제안 및 HOLD; 원본 적용 별도 승인 |
| 메인 신규 아트 | [#123](https://github.com/HappyPaul812/Project-Limitless/issues/123) F3 고갯길 지면/암반/길표식 최대3소량 파일럿 | F1/F2/눈보라·갑충 완료 자산 보존, 미술 판정 전 Unity 금지 |
| 리뉴얼 아트 | [#124](https://github.com/HappyPaul812/Project-Limitless/issues/124) NPC 종료 후 실제 사용 중인 외부 VFX·아이콘 최대3 감사 + 필요하면1파일럿 | OGA/NPC·몬스터·펫·물약/전투BG 중복 제작0 |
| TTS | [#125](https://github.com/HappyPaul812/Project-Limitless/issues/125) Ch3 Main24~29/서브6 신규 VoiceID 발주표 | Google/API 호출0, 기존162음성 보호 |
| BGM | [#31](https://github.com/HappyPaul812/Project-Limitless/issues/31#issuecomment-7086604560) 기수령 F1/서릿길 데모2곡 loop 비교/청취판정 준비 | 음원 승인·본편 등록 전에 Master 변경 금지 |
| SFX | [#33](https://github.com/HappyPaul812/Project-Limitless/issues/33#issuecomment-7086604972) 북풍·발걸음·온기 공간음2~3 파일럿 | 기존 수령 확인·원본 QA/승인 전 Unity 금지 |
| 개발기록 영상 | [#98](https://github.com/HappyPaul812/Project-Limitless/issues/98#issuecomment-7086605435) 11/16 Raw 대비 10/06 편집/하이서드 | 최종 시청·청취 승인 전 게시·업로드 금지 |
| 총괄 기획 | [#118](https://github.com/HappyPaul812/Project-Limitless/issues/118#issuecomment-7086605896) Lv22~23 유지 vs Lv19~21 보스진입 성장 비교 | +3500/+3500/+4000 EXP는 미승인; 기술실측 추가 반복발주0 |

## 보고 및 전달
각 역할은 START_HERE/COMMON_RULES/자신의 담당·인계·최신 Issue를 먼저 읽고, `docs/*` 혹은 Codex 분리 worktree에서만 작업. 검증 범위/새 파일/SHA/기존 보존/승인 필요 여부를 담당 브랜치 보고 및 인계, Issue 댓글과 사용자 보고로 분리. 담당세션끼리 실시간 실행을 확인할 수 없으므로 대표가 역할세션에 해당 이슈를 전달해야 합니다.

## 제외
새로운 main PR 병합/remote main Push, 새로운 Discord AI 무인실행, 11/19 이미 승인된 아트 재제작, #85 packages/meta 임의 수정, 라이선스 재시험 반복, 비Git 민감자료 공개 백업은 오늘 발주하지 않습니다.
