# Issue #124 · 수호자 기본 스킬 승인 Sprite 연결

2026-11-24 · `codex/issue124-guardian-skill3-apply-20261124` · 제품 `931325391a3f5ae1da8c4e7f0ceb7f98a64519d9`.

[기획 채택](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7129561043) · [v2 표적 재검수](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7129457769)

- 도발 v1을 `skill.guardian.taunt`에 연결했습니다. `status.taunt`는 기존 `pawn_right`를 사용합니다.
- 철벽 v2는 `skill.guardian.iron_wall`과 `status.guardian.iron_wall`에, 수호의 맹세 v2는 `skill.guardian.cover_allies`와 `status.guardian.cover_allies`에 각각 공용으로 연결했습니다.
- ZIP SHA256 `9abaa07de47ee869f35570a7647aad70189eb660400a77c5332acdbb0d9dfb18`. 승인 PNG SHA256 도발 `960bd0a3b87eadae637a1e2243eb3eab848b28963651ba328b01e699dba5687b`, 철벽 `6bebeac7b03c6906b6f23adb822f6a05531c3fe49ea48b8452d44cf76ae5f08f`, 맹세 `3b66adf62bff00d86056f29564e7a885d0987b58812624c4bb8de03ba6e49977`과 일치합니다. PNG 3개와 고유 GUID를 가진 meta 3개, 카탈로그만 제품 커밋에 포함했습니다.
- 맹세의 20/24px 보호 관계 판독 **PARTIAL**은 이전 기술 판정을 유지합니다. 기획은 보호막 외곽과 기존 라벨 병행을 근거로 채택했습니다.
- **표적 PASS** · 제품과 동일한 파일 7개를 Unity 6000.6.5f1 호환 QA 복제본에 복사해 백그라운드 배치에서 카탈로그를 호출했습니다. 128×128 스킬 Sprite 3종 로딩, 철벽·맹세 스킬/상태 동일 Sprite 참조, 기존 도발 상태를 확인했습니다. 총 7 PASS(버전 포함), 종료 코드 0. QA 임시 파일을 원복했습니다. 증거 `Temp/i124-guardian-apply-qa/unity.log`. 이번 검증 범위는 Sprite 연결입니다.
- #131 검증 후보를 기반으로 별도 후속 worktree를 사용했습니다. 승인 자산·#85·#131·원본 main·Save/UserData를 보호했습니다. main 병합·원격 Push 없이 `LOCAL_COMMITTED / REMOTE_NOT_PUSHED` 상태입니다.
