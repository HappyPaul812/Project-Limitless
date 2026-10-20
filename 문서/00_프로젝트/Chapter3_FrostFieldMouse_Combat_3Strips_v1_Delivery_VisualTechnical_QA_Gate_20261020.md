# LIMITLESS / Chapter3 서리들쥐 A Attack·Hit·Defeat 독립 Strip v1 납품 및 격리 검수 게이트 (2026-10-20)

- 사용자 제공 로컬 경로: `F:\Downloads\Limitless_Chapter3_FrostFieldMouse_Combat_3Strips_v1.zip`.
- 기획 담당 `docs/planning`, 아트 납품 `docs/art-new`, 작업 [Issue #30](https://github.com/HappyPaul812/Project-Limitless/issues/30). 기획 세션은 사용자 F: 로컬 ZIP을 직접 열람하지 못했으며 아래 구조/해시는 아트 담당의 `docs/art-new/문서/00_프로젝트/Chapter3_FrostFieldMouse_Combat_3Strips_v1_ArtDelivery_QA_20261020.md`에서 확인한 보고 내용.
- 기존 Codex 기술 계약 `codex/chapter3-mouse-combat-contract-20261020` 계약 commit `ad9e4b46`, 인계 `13178071`. 기획 발주 `문서/00_프로젝트/Chapter3_FrostFieldMouse_AttackHitDefeat_3Strip_ProductionOrder_20261020.md`.
- **현 상태:** `THREE_STRIPS_ACTUAL_ZIP_DELIVERY_REPORTED / OFFLINE_FORMAT_QA_PASS_REPORTED / USER_ART_APPROVAL_PENDING / CODEX_ISOLATED_UNITY_MOTION_QA_PENDING / ORIGINAL_UNITY_NOT_APPLIED / MAIN_NOT_MERGED`.

## 아트 담당 실제 납품 명세

- ZIP `Limitless_Chapter3_FrostFieldMouse_Combat_3Strips_v1.zip`, SHA256 `24ef8dddf76fb5843e8e7d3fac05942f1450c9f39cf366b2422a0ab82cba6741`, 8,688,417 bytes.
- Attack `Sprites/FrostFieldMouse_A_Attack_4Frames.png` SHA256 `08ed9ffb3d2807b0e1a3b27450f8f3bf1ff3e56f5f33e8b6607b2983db4e402e`.
- Hit `Sprites/FrostFieldMouse_A_Hit_4Frames.png` SHA256 `37c51117ae18f2fed2bda5a8cef6af3099812b5e0979e1368634172937568f1b`.
- Defeat `Sprites/FrostFieldMouse_A_Defeat_4Frames.png` SHA256 `bda2809d177d03088e85389ecea23815b92962d61c36cc68967cd0015c96dfcc`.
- 각 2048×512 RGBA, 가로 512×512 4셀, 8FPS(0.125s per frame), PPU512, Pivot bottom center `(0.5,0)`, 우향1, VisualScale1. Attack/Hit 후 Idle로 돌아가며 Defeat는 마지막 frame03 유지. 원본 승인 Idle `FrostFieldMouse_A_Idle_4Frames.png` SHA256 `5afebfa1f95ca5673162513fdea63b025546aa836550c0ceb17f0334659988f2`, 1포즈 승인 SHA `0516914ddad36af5854065d66d01b9664329aa89eea8b394480eed6af92e32c2`.
- PNG3, 모션별 4프레임 확인·APNG/GIF 8FPS 예시·128px QA·12개 개별 프레임 PNG·Manifest/OfflineQA/SHA256SUMS 포함, 아트 담당 ZIP CRC·PNG/프레임·Alpha bbox·발 접지선 y490 자체 PASS 보고.
- **시각품질 경고**: 이번 Attack/Hit/Defeat는 각각 새로운 2172×724 원화 4포즈 보드를 분할·일률적으로 512셀에 축소/재구성해 만들어졌고, 정적 승인/Idle의 **냉회백색 털**보다 새 전투 그림에서 **갈색 털이 강하게 나타남**. 애니메이션/Idle 전환 시 같은 몬스터로 보일지, 타격/피격/사망 실효 128px에서 디테일을 잃었는지 우선 확인. 규격 PASS가 미술 PASS를 보장하지 않는다.

## 다음 Codex 작업 — 이번에는 실제 ZIP의 격리 시각·기술 QA까지만

1. **원본 보호**: 현재 local main/원본 Unity Editor·Play·dirty·untracked/Save/UserData/승인 1포즈+Idle/다른 몬스터·Field09 작업 보호 SHA 및 별도 격리 worktree 확인. `git clean/reset/restore/stash` 강제 금지, 원본 Editor·실제 게임 Script/Scene 변경 금지.
2. 사용자가 제공한 실제 Windows ZIP SHA256→CRC/testzip→Manifest/SHA256SUMS→PNG3 실제 해시→분할12 frame/Alpha crop 고립 픽셀/foot-anchor 일치 확인. 파일이나 해시 불일치 발견 시 **검증 FAIL** 및 파일명을 그대로 보고하고 임의 수복·교체 금지.
3. 기존 승인 Idle과 **같은 128px 실효 표시 크기/게임 UI scale**로 Idle→Attack→Idle, Idle→Hit→Idle, Idle→Defeat(frame03 hold) 동작을 **독립 Unity 테스트 씬 또는 기존 격리 재생 harness**에서 재현. ExplicitAttack/Hit/Defeat 프레임 매핑/Import PPU512·pivot(0.5,0), 우향/좌우반전, collider/foot-anchor의 불필요 변경 및 이펙트 섞임도 점검. 실제 Main22 몬스터 Definition/Quest/전투 시스템 신설은 금지.
4. **표정·비율·팔/꼬리·털 색감**을 승인 Idle/1포즈와 각 Attack·Hit·Defeat 4프레임으로 직접 비교. 간헐적으로 **회백→갈색→회백으로 튀는 현상**이 있는지/다른 몬스터로 보이는지, 128px에서 Attack 할퀴는 순간·피격 회복·사망 쓰러진 모습이 읽히는지, 지나친 자산 downscale 과정으로 외곽 픽셀·꼬리·눈·앞니 뭉침이 있는지 보고. 경계/잘림/지면 미끄럼/03 최종 사망 유지도 확인.
5. 적어도 실제 **같은 크기 비교판(Idle/Attack/Hit/Defeat 각 대표 frame)**과 모션별 8FPS 재생 캡처/짧은 미리보기, 프레임03 사망정지 장면, 사용자 미술 승인을 위한 **조각별 문제점/검증** 제시. 기존 전체 QA 반복 없이 이번 파일3에 대한 Delta만. 기존 Idle 격리14PASS 재사용.
6. 결과를 **PNG 기술 PASS/FAIL**, Attack/Hit/Defeat **각 모션 시각 PASS/HOLD/FAIL**, **색/스타일 일관성 PASS/HOLD/FAIL**, Unity 격리 Runtime/재생 PASS/FAIL로 구분. 원본 승인 Idle·정적 시트가 수정되면 FAIL. 문제 발생 시 **다시 새로 그릴 대상 1~3종과 수정 위치/색만** 정확히 보고, 아트 담당에게 선조치시키지 않음.
7. 검수 report: branch/commit, ZIP/Sprite SHA, 테스트 수 PASS/FAIL, 변경 파일, 파일 경로/실제 Preview, 사용자가 최종 미술 승인할 게이트, 원본 disk/main Push/merge=0 표시. **격리 시각QA 합격만으로 공식 게임 구현/PR/main 병합 자동 승인 금지**.

## 제작 진행 순서

현재 Codex 델타 QA 먼저 → 바울 대표가 새 3모션의 직접 움직임·색 일관성을 판단 → 필요 시 `docs/art-new` 수정 PNG만 v2 재납품 → 다시 최소 Delta 검증 → 승인 시 실제 게임 등록/Chapter3 Main22 기능 구현은 다음 별도 Codex 작업. 리뉴얼 담당 Field09 B Glow는 전혀 다른 흐름. TTS/SFX/영상 대기.
