# Issue #124 승인 사수·투사·치유사 스킬 Sprite 연결

2026-11-27 · `codex/issue124-approved-skills-apply-20261127` · 기준 #132 `88a0214f395029b851dd930038516906d8bed590` · 제품 `7c05706ddbce17d3cc76299faf0d45f197276f34`

[사수·투사 최종 승인](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7208174629) · [치유사 최종 승인](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7207123331)

- 사수 정조준 v1·화살비 v1·동료의 습격 공통 v2, 투사 난도 v2·회심의 일격 v2·회오리 베기 v1을 적용했습니다.
- 원본 및 #132 기반에서 치유사 승인 PNG 매핑이 없음을 확인해 치유의 빛 v2·회복의 파동 v1·정화 v2도 함께 적용했습니다. 버튼과 스킬 상세 팝업이 같은 승인 Sprite를 사용합니다.
- 동료의 습격은 모든 펫에서 공통 Sprite를 표시합니다. UI 경로에서 늑대 Run 잘라내기를 제거했으며 전투·펫 시설의 야수별 Run/장착/표시 경로는 유지했습니다.
- 회심과 기세는 상수 하나로 같은 Resources 경로를 공유합니다. 정화가 정식 Sprite를 사용하게 되어 기존 감사의 런타임 Sprite 강제 파괴 대상에서는 정화만 제외했습니다.
- 승인 ZIP 3종 SHA256과 PNG 9종의 납품값, 커밋 내 PNG 9종 원본 바이트를 대조했습니다. 별도 GUID meta 9개. 제품 diff는 PNG·meta 18개와 카탈로그·기존 감사 2개입니다.

**Unity 6000.6.5f1 제품 worktree 표적 PASS 12건.** 클린 Import·스크립트 컴파일을 거쳐 제품 Battle 씬에서 승인 9종의 실제 버튼과 상세 팝업 Image가 카탈로그의 동일한 승인 Sprite를 참조했습니다. 기존 전투 런타임에 합성 기세 2를 넣고 정상 HUD 갱신 경로를 호출해 실제 `기세 2` 배지와 회심 버튼의 Sprite 객체 동일성을 확인했습니다. HUD 20/24px 및 버튼 30/36px 비교를 캡처했습니다. 합성 Save·설정 경로 사용. 별도 호환 복제본에서도 12 PASS·종료 0을 확인했습니다. 제품 배치는 12행 PASS와 정상 종료 로그를 남겼고 대기 명령의 제한 시간 뒤 종료되어 OS 종료 코드는 수집되지 않았습니다. 검수 임시 스크립트·Unity 자동 재직렬화는 원복했습니다. 전체 전투 플레이와 펫별 전투 재생은 재검수하지 않았습니다.

로컬 근거 `Temp/i124-approved-skills-qa/`에 `unity-product.log`, 사수/투사/치유사 실제 화면 4장 및 승인 PNG 해시 목록 `approved.txt`를 보존했습니다. `Fighter_36px_Momentum_24px.png` SHA256 `4cd60bb3b1af1104cc531ec254086eef989c45745c309d985aeec7963517497c`.

기존 5/7/6/20+5 PASS 전수 재실행 없음. 기능·수치·VFX·투사체·야수 애니메이션과 승인 자산·Save/UserData·#85/#131/#132 보호. 원본 main 동기화, main 병합, Push 없음. `LOCAL_COMMITTED / REMOTE_NOT_PUSHED`.
