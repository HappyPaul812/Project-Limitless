# LIMITLESS — 2026-10-21 서릿길 마을 v4 + 늑대 Run6 v5 최소 변경분 격리 Unity QA 계약

- 기획: `docs/planning`. 바울 대표가 실제 Windows 경로 2건 제시:
  - `F:\Downloads\Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtRework_v5.zip`
  - `F:\Downloads\Limitless_Chapter3_FrostwayVillage_Environment_GameArt_TargetedFix_v4.zip`
- 이 기획 세션은 F: 원본 파일 바이트에 접근할 수 없으므로 아래 SHA/용량은 **각 아트 담당의 GitHub 실물 납품 보고값이며 Codex F: 실제파일 readback 이전에는 미검증**. 예전 ZIP SHA mismatch 이력이 있으므로 이번 신규 ZIP도 실제 해시 일치가 검수 첫 조건.
- 실 QA는 **기술 / 시각 / 실제 격리 Unity / 사용자 최종 승인** 별도 판정, 두 모듈 A/B 분리. 사용자 원본 Unity·Editor/Save/UserData/Git dirty/untracked/main SHA·이미 승인 아트 전부 불변, 모든 직접 Push/merge 금지.

## A. Main23 FrostwayVillage Environment TargetedFix v4

- `docs/art-new` 정본 `문서/00_프로젝트/Chapter3_Main23_FrostwayVillage_GameArt_TargetedFix_v4_QA_20261021.md`, [Issue #45](https://github.com/HappyPaul812/Project-Limitless/issues/45).
- **담당 신고**: ZIP SHA256 **`ee25baef91ec740b85dd3bb62b32583292d703f9261863bd534d0eb8241d1896`**, **19,767,770 bytes / 70 ZIP entries**, CRC PASS, SHA256SUMS **69/69**, 실 GameArt PNG 43종.
- 사용자에게 앞서 지적된 visual FAIL 2파일만 v3→v4 실 수정:
  1. `Buildings/Frostway_SkillShop_Building.png` 511×418 RGBA: detached alpha component bbox **x12..28 / y196..376**, 2,496 픽셀 제거. v3 SHA `7e3562c22f00a07a85f0ab0349b8119dff924f3ef4dccf199cccdc9ec367be89` → v4 SHA **`ef34b49de4f807debf4a69e1947eb9b8e02a543ee117ca2867fe3aa06a5efb17`**.
  2. `Buildings/Modular/Frostway_Roof_Gable.png` 175×219 RGBA: detached alpha component bbox **x100..144 / y183..206**, 712 픽셀 제거. v3 SHA `4971eea0e473ec7c7c1579001a454be52417fd9f3f38b9e223b00e0deaaf71ec` → v4 SHA **`37ea6a69844b662c40583cea3d457ff0c47344aef7054ff8e32b4d09870dceb0`**.
- **기존43 PNG 중 나머지41개 파일 바이너리 SHA v3 동일 보고**, 기존 PNG 본체의 정상 픽셀 RGBA 변화 0; clipping 잔조각만 8-connected component mask로 제거. 이전 2종 visual FAIL 수정에 해당, 전체 Village 미술 최종 PASS로 자동 승격 금지.
- **Codex 최소 델타 QA**: 새 ZIP 실제 SHA/CRC/manifest/43 PNG 경로/41 PNG 동일 SHA/2PNG before-after bbox 및 정상 지붕·벽 보존; 격리 실제 Unity GameView 16:9·4:3·21:9의 조립·Sorting/문·벽·지붕 접합/단일 거리 블록의 이동·서비스 접근만 변경부 회귀 확인. 과거 3화면비 실제 GameView 증거를 재사용하되 새로운 2PNG를 실제 Sprite 참조 후 전후 화면 증거 확보. 전체 Main23 씬·퀘스트 구현 금지.
- **별도 타일 미술 HOLD 유지**: 파일 기술 pixel edge PASS != 반복무늬 미술 PASS. 기존4 tiles v3 byte 동일(v4), 담당이 3×3 주기 패턴 표시자료를 새로 제공:
  - `Tiles/Frostway_Ground_Earth_01.png` x9..94/y120..252
  - `Tiles/Frostway_Ground_Gravel_01.png` x23..130/y26..120
  - `Tiles/Frostway_Ground_ThinFrost_01.png` x9..94/y120..252
  - `Tiles/Frostway_Path_Stone_01.png` x60..215/y94..240
  위 4종 자체 수정 금지. Codex는 실제 게임 표시 배율에서 3×3 반복문양이 허용 가능한지 **'PASS 권고' / 'HOLD 유지' + 비교 화면만** 보고, 기존 Asset 미술 유보를 데이터 기술 PASS로 덮지 않음. 대표님이 보기에 충분히 자연스러우면 수정 없이 사용자 수용 가능, 아니면 문제 tile만 차기 별도 개선.

## B. Wolf Run6 From Approved v5 — ArtRework v5

- `docs/art-renewal` 정본 `문서/00_프로젝트/Wolf_Run6_ArtRework_v5_Frame05_Hindleg_Delivery_20261021.md`, [Issue #43](https://github.com/HappyPaul812/Project-Limitless/issues/43).
- **담당 신고**: ZIP SHA256 **`16ba7923771c98f05b0f041b5c5a63857cdecfcdfd2e1f7768d9f24d1af0e351`**, **2,663,380 bytes**, ZIP/Manifest/PNG6/Strip/Atlas/6frameGIF/APNG 자체 파일 QA PASS.
- 기존 Run6 v4에서 **Frame05 하나만** 수정, 총 **174 RGBA 픽셀**, 실제 bbox **x318..333/y245..280**, 기존 결함 범위 x315..338/y235..280 내부. 새 `Wolf_Run_05_v5.png` SHA **`d3d53b94be9084bfd9b2a0bbe6fa492b903138b37c86e7068c9eaefbe8b1194c`**. Frame00..04는 기존 v4 개별 PNG 파일 **바이트 동일5/5**, 512×384 RGBA LEFT, 12FPS/500ms 6frame, strip3072×384/Atlas1536×768. 승인 정적 **Wolf KeyArt v5 SHA `b770c73c36b9035a2bb0e70b8c126090b5fa248d27db542af077094d12ee2ea1`** 변동0.
- **Codex 최소 델타 QA**: 신규 실제 ZIP SHA/CRC/Manifest·frame00..04 byte-identity·05 변경 범위174px+다리 제외 RGB 변동0·strip/atlas RGBA exact6/6 확인. 격리 실제 Unity에서 **12FPS Wolf Run6 v4 vs v5 같은 표시 크기(128px) 동시재생**, Frame05의 후지 관절·발목 곡선·발 접지/04→05→00 부자연스러운 '직각 끊김' 개선, 불필요 몸길이 점프/다리 미끄러짐/꼬리 절단/05→00 loop 평가. 40px icon preview는 참고. **실제 움직임이 부자연스러우면 미술 FAIL/HOLD 유지**, 픽셀 변경량 적다는 이유만으로 PASS 금지.
- 실제 펫 시스템 `BeastCompanionSystem`·기존 ThirdParty Wolf 원본 PNG/GUID/Meta/Unity 씬 적용 금지. User RUN Motion 승인은 Codex 실제 12FPS Art PASS 권고 후 별도.

## 완료 및 보호
- 공통 Codex 격리 브랜치 제안 `codex/qa-frostway-v4-wolfrun-v5-20261021`; 2ZIP READ-ONLY, clean worktree에서 실제 Unity 검증 only, 원본 변경0. Main22 136PASS/F1 64PASS/MouseBattle40PASS, Main23 기존 preflight/이전 Village43 기술 전수QA 재수행 금지.
- 결과: **각 ZIP 실제 SHA 및 납품 보고 일치 여부** 먼저 명시, Village 2PNG 수정 PASS/FAIL + other41 SHA + 4Tiles Pattern 미술 HOLD 명시; Wolf Frame05 모션 PASS/HOLD/FAIL·00..04 동일·실 12FPS 비교. 두 결과 독립. 실제 GameView & 모션 비교자료, SHA/기술/미술/대표 승인 분리, branch/commit/file diff/UserData·Editor 보호 SHA, main 직접 Push·병합 없음.
- Art approval이 나오기 전 기존 마을·펫 전체 정식 Unity 통합 하지 말 것. 전담 art-new/renewal은 이번 원본사본/디자인 재작업을 Codex가 대신 수행하지 않음.
