# LIMITLESS — 2026-10-21 서릿길 마을 GameArt v3 · Wolf Run6 v4 합동 Codex 델타 검수 계약

기획 docs/planning. 바울 대표 2026-10-21 전달:
- `F:\Downloads\Limitless_Chapter3_FrostwayVillage_Environment_GameArt_v3.zip`
- `F:\Downloads\Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtRework_v4.zip`

이번 대화의 기획 담당은 **사용자 PC F:에 직접 접근하지 못함**. 아래 SHA는 docs/art-new 및 docs/art-renewal의 자체 납품보고 값이며 Codex가 로컬 ZIP readback 확인 전 제품 승인 아님. 두 ZIP을 **한 번에 격리 QA하되 결과는 A/B로 분리**, 서로 다른 아트 브랜치 / 게임 제품 범위를 섞지 말 것.

## A. Main23 Frostway Village GameArt v3

- art-new 정본 `문서/00_프로젝트/Chapter3_Main23_FrostwayVillage_GameArt_v3_ActualDelivery_QA_20261021.md`, GitHub [#45](https://github.com/HappyPaul812/Project-Limitless/issues/45), report ZIP SHA256 **`2a74677d5f62d7e13e64ea85ba62e12175be0be0e0d19864c23233f1bd84e79a`**, 17,311,281 bytes, **59 entries = 게임 후보 PNG 43 + Preview12 + 메타4**.
- Sprite43 구성: Tiles 기본4(256×256 RGBA), Transitions2(단방향 포함), Buildings 독립5, Buildings/Modular 지붕·벽·문9, Props14, Signs6, Portal_Inactive3. 기존 v2 콘셉트11 PNG를 다시 포장한 것이 아니라 독립 게임용 후보 PNG43. v2 자체 SHA 불일치: art-new retained ZIP2142fe.. vs Codex 사용자 전달 ZIP19dfad.., **그때 사용자 로컬 ZIP 엔트리 바이트 비교 못해 원인 미확정**; 신규 v3의 실제 F: ZIP SHA부터 우선 확인.
- **검수1 파일·보존**: ZIP SHA/압축CRC/Manifest JSON/SHA256SUMS/PNG43 RGBA decode/불투명 tile4 규격·개별 Sprite alpha·bbox·외딴픽셀·고립 알파·테두리 침범0·빈 셀0. README나 문서에 43 실제 독립 PNG가 있는지 파일명·역할 대조. 승인 F1 6 PNG/Chapter2 공식 원본 hash 및 기존 village v2 콘셉트11 바이트보호.
- **검수2 실제 사용성**: 승인 F1 Tile과 새 흙·자갈·돌길·서리 Tile4 및 전환오버레이2의 2×2/3×3 **시각 반복 패턴까지 확인**(엣지 픽셀 일치는 반복무늬 미술 PASS가 아님). 특히 방향지정 오버레이를 4방향 자동타일로 취급하지 말 것. NPC·휠체어 진입에 필요한 타일·길 노출, 건물5+모듈9(지붕·벽·문 정합), 외관 지붕 sorting/문 collider 후보, 소품14 alpha/접지/door blocking, 표지6에서 여관·일반·스킬상점 명확히 식별되는지, 비작동 포탈 돌기단3 **기능·Collider/Spawn/전송0**.
- **검수3 격리 Unity 시각**: Main23 기술 Preflight branch `codex/chapter3-main23-village-preflight-20261021`의 Scene/PPU/TileGrid/Spawn/Quest·IDs/안전지점 계약을 재활용. 기존 16:9 / 4:3 / 21:9 Offline Preview3와 **실제 격리 Unity GameView(가능하면 3비율)** 구분 표기. 43 PNG만으로 **마을 전역 완성 scene/동선 인증**을 강요하지 말고 격리 구성 가능한 기준 1개 거리 블록 또는 방대한 구조 없이 남문→중앙 서비스 앞→미래 북문/포탈 공터 미니 레이아웃 테스트. 오버랩·알파 프린지·타일 seams·보행/휠체어 Collider 클리어런스·문 앞 접근·서비스 글자/아이콘 가독성 확인. 아트 결함과 구현 계약 미확정 각각 분리.
- **판정 분리**: `ZIP_IDENTITY_PASS|FAIL`, `PNG43_TECH_PASS|FAIL`, `GAME_ART_READY_PASS|HOLD|FAIL`, `CONCEPT_VISUAL_PASS|HOLD|FAIL`, `ISOLATED_UNITY_TESTED|OFFLINE_ONLY`, `USER_FINAL_ART_APPROVAL_PENDING`. 부족 자산·정확 파일명·좌표/문/건물 접합·비주얼 결함만 보완 요청. 새로운 마을 전체 재작화·Main23 실제 Quest/NPC/SkillStore/Save 구현/본편 game scene 적용은 이번 검수로 허가되지 않음.

## B. Chapter1·2 Wolf Run6 v4

- art-renewal 정본 `문서/00_프로젝트/Wolf_Run6_ArtRework_v4_RealPNGs_DeltaQA_20261021.md`, [#43](https://github.com/HappyPaul812/Project-Limitless/issues/43). 신고 ZIP SHA **`3ab0b57d2389b55ed33f73c1d3d13862b9b595245e196d1f3a4c98f090b8a412`**, 3,024,517 bytes, **34 entries**. 6 poses 512×384 RGBA, 6-cell Strip 3072×384, 3×2 Atlas1536×768, 12FPS GIF/APNG·128/40px preview.
- 변경: Run **00/01/02/03 PNG v3 원본바이트 동일**, Run04 **4,368 pixel** 뒷다리 재구성, Run05 **7,542 pixel** 루프 앞발 가동 변경. 이전 통과 꼬리 Run04 지역 `(x346..485,y88..164)` Pixel identical 유지. KeyArt v5 원본 SHA `b770c73c36b9035a2bb0e70b8c126090b5fa248d27db542af077094d12ee2ea1` 보호. 현재 Run v3는 **기술·격리Unity12FPS PASS/미술 HOLD**, 아직 승인된 모션 아님.
- **검수1 파일·보존**: 실제 F: ZIP SHA (이전 v3 배포본 v3 report 171b954d.. vs actual f7eb4122.. 11Byte 불일치 전례 있어 최종 바이너리 동일성 PASS 없이는 승인 불가), CRC/Manifest/PNG6 SHA·strip/atlas Rect, 프레임 00~03 exact byte SHA, 04 꼬리 영역 pixel exact, GIF/APNG 프레임6/총0.5초 LEFT/다른 포즈6/셀 침범·alpha.
- **검수2 12FPS 실재생**: **새 격리 Unity harness에서 실제 12FPS 애니메이션**, 기존 v3에서 04 뒷다리 어색함, 새 v4 04 뒷다리/관절/발 회전·포즈·가슴·엉덩이 비율, 05 앞발→00 접지/회복, 02→03→04→05 중간 균형, 꼬리 유지, 루프 `05→00` 정지컷과 128px 실제 실크기 재생 비교. 바이트 변경 최소·발 최하단 좌표 3px 일치는 자연스러운 Run 예술 PASS 증거가 아니므로 해부학/지면 고정 여부와 루프 시각을 별도로 평가. 한도 내 동일 테스트 영상/GIF 캡처. Editor Search 예외 1건이 재발하면 별도 환경 이슈로 로그/영상 기록, Wolf 미술 FAIL이라고 오인하지 말 것.
- **판정 분리**: `ZIP_IDENTITY_PASS|FAIL`, `FILE_TECH_PASS|FAIL`, `UNITY_12FPS_PLAYBACK_PASS|FAIL|NOT_TESTED`, `MOTION_ART_PASS|HOLD|FAIL`, `USER_RUN_FINAL_APPROVAL_PENDING`. **기존 공식 Wolf 펫 Sprite/GUID/Meta/BeastCompanionSystem 교체·main 병합 금지**. 후보 v4가 PASS여도 반드시 바울 대표 모션 최종 승인 별도.

## 공통 실행 원칙/완료 보고

- Codex 별도 `codex/qa-village-v3-wolfrun-v4-20261021` clean worktree 사용. 이미 Main22 격리 Quest **136PASS**, F1 승인 환경 실제 Unity **64PASS**, 서리들쥐 Battle **40PASS** 및 Main23 기술 preflight 결과는 재사용·전수 재시험 없음. Main22 본게임 통합 준비는 완료했으나 실제 원본/main 적용 전에 별도 사용자 승인 필요, 해당 Unity 원본 파일 접근/설정/Save/UserData/Editor/dirty·untracked 보호.
- 변경 후보 ZIP2 자체는 읽기 전용. 예상 SHA가 실제와 다른 경우, 이전 v2/v3처럼 `WARN: NONIDENTICAL_PACKAGE` + 정확 ZIP 크기/entry metadata와 각 파일 SHA를 따로 기록하고 **사용자 파일 자동 이동·재압축·삭제하지 말 것**. 파일 포맷 자체 정상이나 패키지 동일성 FAIL인 상태에서 '모두 PASS' 주장하지 말 것.
- 보고: 실제 SHA·파일수/보존 증거, village PNG43 각 주요 TECH/Art/Scene usability 판정, Wolf 프레임 04/05 및 실 12FPS motion/loop 정지화면과 QA, 독립 `PART_A`,`PART_B` **각각** QA PASS/FAIL·HOLD/후속 미술 수정 3개 이내 정확히 표시, branch·commit, `original local changed0`, `main direct Commit/Push/merge0`, 사용자 미술 최종 승인 필요.
