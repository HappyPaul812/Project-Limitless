# LIMITLESS — Field06 v1.1 · Field07 v1 사용자 미술 승인 및 후속 Unity 검증 게이트 (2026-10-19)

- 작성/관리: 기획 `docs/planning`; 사용자의 실제 채팅 승인 `“코덱스 시켰고, 리뉴얼도 나왔고, 보니까 괜찮다.”`.
- 사용자 전달 로컬 ZIP 경로: `F:\Downloads\Limitless_Field07_AshenReach_Renewal_Art_v1.zip`, `F:\Downloads\Limitless_Field06_ScorchedTrail_Renewal_Art_v1_1.zip`.
- **상태:** `FIELD06_V1_1_USER_VISUAL_APPROVED / FIELD07_V1_USER_VISUAL_APPROVED / OFFLINE_ART_QA_PASS_REPORTED / ROAD_A_B_UNDECIDED / CODEX_UNITY_INTEGRATION_PENDING / ORIGINAL_EDITOR_RUNTIME_NOT_VERIFIED / MAIN_MERGE_NOT_AUTHORIZED`.
- 이 기획 세션은 사용자의 F드라이브를 직접 열거나 ZIP 자체를 검사하지 않았다. ZIP 내용/SHA 및 오프라인 검증은 리뉴얼 담당 `docs/art-renewal` 납품 정본 기준으로 기록한다. 사용자가 납품물을 보고 괜찮다고 한 것은 **현재 아트 스타일·시안의 미술 승인**, 기술적 적용/Runtime 완료/선택 옵션 전체 사용 승인으로 과장하지 않는다.

## A. Field06 — Scorched Trail 공식 승인 미술 후보 v1.1

정본 `docs/art-renewal:문서/00_프로젝트/Field06_ScorchedTrail_Renewal_Art_v1_1_TechFix_Delivery_20261019.md`.

- 파일명 `Limitless_Field06_ScorchedTrail_Renewal_Art_v1_1.zip`; SHA256 `d8551df7315495dcc6e4556ec33f30b711af8aeb85dc7b0a1c3612a5a6197f42`; 1,399,079 bytes/41 files/PNG29 (사용 후보22/프리뷰·QA7).
- v1 아트 승인 팔레트/스타일은 유지. v1→v1.1에서 **도로 Upper/Lower/WestEnd 4 PNG만** 도로 중앙 Alpha0 이음 및 상하 위상 교정. Ground3·Props7·선택Runtime6/Decal2 = PNG18 전부 원래 v1 SHA 불변. 기존 v1 ZIP 보존.
- 오프라인 ZIP CRC·Manifest22 SHA·PNG29 Decode·Road Seam6 PASS.
- **미해결/Unity 책임**: Field06 Scene 기존 Road30 남북형에서 동서 A30(재배치)/B42(추가12 Renderer) 중 선택 및 실제 Renderer/Importer 검증; `Chapter2Main15Flow.BuildEnvironment()`가 x<-3 등 49개 Grass를 비활성화해 만드는 Runtime 빈틈 위험; 일부 Props 실제 source rect/Collider/alpha-foot 정합. A30/B42 중 아직 최종 사용자 선택 미확정. v1.1 seam PASS를 실제 Play PASS로 바꾸지 않는다.
- 별도 Runtime6·Decal2 **선택 8종**은 `OPTIONAL_NOT_APPLIED` 유지, 본 승인으로 자동 설치하지 않음.

## B. Field07 — Ashen Reach 신규 환경 v1 사용자 미술 승인

정본 `docs/art-renewal:문서/00_프로젝트/Field07_AshenReach_Renewal_Art_v1_Delivery_20261019.md`.

- 파일명 `Limitless_Field07_AshenReach_Renewal_Art_v1.zip`; SHA256 `5d398295b6f98491a294628a18763b6d72e0dedfdf5f691ccd2e8d0539150403`; 477,759 bytes/27 files/PNG18 (실사용후보 15/프리뷰·QA3).
- 후보 PNG15: Ground3(동·중·서 색 전이), Road4(EW Upper/Lower/WestEnd), Props5, 균열/동물흔적 Decal2, Runtime용 `WindlessAsh` optional1. 총15 중 **Runtime optional1은 별도 사용 승인 전 HOLD**.
- Scene `Field_07_AshenReach.unity` 정적 SpriteRenderer372 (= AshSoil315/Road30/AshCrack8/BrokenTrack10/CanyonLip2/Tree5/Rock2); 오프라인 ZIP CRC, PNG decode18/18, Manifest15 SHA, Ground/Road seam24 PASS.
- **미해결/Unity 책임**: 기존 Road30 남북형 → 동서 A30/B42 선택(추가 Renderer12 여부 별도), Main16 `CreateAshMarks()`가 첫 `AshSoil_` Ground Sprite를 그대로 `WindlessAsh_0..6`에 복제하는 기능으로 신규 Ground 적용 시 재의 동적 표현이 부자연스러워질 위험, Scene Tint·Importer/Collider/접지/Camera·Main16 Story Wraith 12목표/Save/Continue/휠체어 접근. 정적 PNG만 바꾸고 Runtime7을 고쳤다고 주장하지 않음.
- `WindlessAsh` Runtime optional1은 별도 기술·사용 결정 전 적용하지 않는다. 공식 세린·불씨망령·Path Hearing 분기·TTS/BGM 및 Quest/Spawn 보존.

## C. Codex 인수 절차 — Main10 우선, Field06·07 분리

1. **이미 바울 대표가 Codex에게 Main10 버그 조사·수정 지시를 완료했다고 직접 보고.** 현재 Main10 치유사 NPC 입력 방해(P1) 및 18개 음성 내부 발화 감사가 우선. Codex 실행 진행/결과를 본 기획 세션이 별도로 직접 확인한 것은 아님. 이 지시에 Field06/07 Unity 작업을 끼워 넣거나 원본 Editor/Save 조작하지 않는다.
2. Main10 진행 후 별도 기능 브랜치 각각 `codex/field06-v11-unity-integration-...` / `codex/field07-v1-unity-integration-...`를 사용. 원본 dirty/Untracked/UserData·승인 Main09 27 음성·이전 필드 아트/GUID/meta 안전 보호, 사용자의 원본 Editor/포커스/Play 무조작.
3. Field06: SHA/CRC/Manifest 재검증(한 번), 필수 정적 미술 후보만 격리 Scene에 Import, 도로 A30/B42를 실제 동서 UX/Collider·Spawn 기준으로 비교하고 **A/B 선택이 실물 Scene에 미치는 영향이 남을 경우 사용자에게 좁혀 보고**. Runtime Grass49 빈틈/Props footprint를 국소수정, Main15 12퀘스트/몬스터 4 Encounter/세린/Arbel↔Field06/Save/휠체어·3화면비 delta QA. 이전 v1 717 PASS 전체 재실행 금지. GUI 원본 수동 검증은 별도.
4. Field07: 현행 source sprite과 새 후보 Mapping/GUID/Tile 변화를 읽기 전용 감사 후 격리 Unity Import. A30/B42 후보만 비교/사용자 배치 결정. `AshSoil_`가 Runtime WindlessAsh7에 복사되는 문제는 실제 화면을 계측하고 별도 결론/필요한 최소 Code 수정만 제안(선택1 PNG 자동 적용 금지). Main16 12단계·Story Wraith/지문·세린·각 조사점·Path Hearing, Save/Continue, Field06↔07↔08 및 휠체어/우회 경로 QA. v1 오프라인 Seam PASS를 Unity PASS로 승격 금지.
5. **최종 적용 게이트:** `USER_VISUAL_APPROVED`는 두 지역 모두 충족. `ROAD_LAYOUT_APPROVED`, `ISOLATED_RUNTIME_QA_PASS`, `ORIGINAL_DISK_SAFE_APPLY`, `GIT_PR_MAIN_MERGED`, `ORIGINAL_EDITOR_USER_PLAY_PASS`는 각각 실제 결과로만 승격. 승인 전 `main` 직접 커밋/Push/강제 병합 금지.
6. 작업을 위해 새로운 아트가 필요하면 v1/v1.1를 덮어쓰지 말고 리뉴얼 아트 담당에게 **정확한 PNG·배치·접지 정보**만 최소 추가 요청. Field08 기존 공식 아트는 그대로 보호.

## D. 리뉴얼 담당 다음 업무 — Field08 우선 정본 감사, 자동 재제작 없음

Field07 사용자 미술 승인 후 로드맵의 다음 지역은 `Field_08_RedRift`이나 **Main17 공식 환경 아트가 이미 승인·Unity 연결됐다는 기록**이 있으므로 Field06/07처럼 통째 재제작하지 않는다. 담당은 최신 `main` `CURRENT_STATUS`, `문서/03_스토리/Chapter2_Main17_붉은_균열.md`, `Main17_ArtAudio_QA.md`, `Main17_Field08_Art_Handoff.md`, 현행 `Field_08_RedRift.unity`의 실제 적용된 official PNG/GUID/Renderer를 읽기 전용 감사하여:
- 기존 공식 아트/외부보완/Placeholder/Road 및 실제 화면 연결/접근성을 분류;
- Field07 색상 접속과 새 교체가 반드시 필요한 항목만 별도 개선 **후보 목록** 작성;
- 기존 공식 PNG/작열 감시자·세린·몬스터/Quest12/State/Save/TTS·BGM 무단 변경 없음.
- **Field08 신규 실물 PNG·ZIP 제작은 중복 여부와 필요한 범위를 바울 대표가 승인한 후에만 시작**. 필요 없으면 SKIP_APPROVED_EXISTING.
- 이 감사는 리뉴얼 담당의 GitHub 문서 작업/자산 조사로 진행하며 Codex Main10의 5시간 한도를 사용하지 않는다.

## 문서·Git

기획 `docs/planning`에서만 승인·인계를 기록. 리뉴얼 담당은 자신의 `docs/art-renewal` `WORLD_ART_RENEWAL_LATEST.md`에 Field06 v1.1·Field07 v1 **USER_VISUAL_APPROVED**와 보류 A/B·optional, Unity Pending을 업데이트. Codex는 각 `codex/*` 격리 worktree 및 안전 승인 절차 준수, main 직접 커밋 금지.
