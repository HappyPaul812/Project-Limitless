# LIMITLESS / Chapter3 Main23 서릿길 마을 외형 보완 — Issue #59 정식 증거·납품 상태 보고 (2026-10-23)

> **현재 판정: `SCOPE_CONFIRMED / REUSE_BASELINE_AUDITED / NEW_ART_NOT_DELIVERED / USER_ART_REVIEW_PENDING / NO_UNITY_APPLY`.**
>
> 이 문서는 **실제 신규 아트 납품 완료 보고가 아니다.** 기존에 대화로 전달된 작업범위 요약과 신규 PNG/ZIP 납품 사실은 구분한다. 존재하지 않는 파일·SHA256·유니티 검수 결과를 만들지 않는다.

## 1. 이번 GitHub 정식 보고의 근거

- 신규 아트 지시: [Issue #59](https://github.com/HappyPaul812/Project-Limitless/issues/59) 및 [2026-10-23 기획 최종 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/59#issuecomment-6355299474). 이전 대화 보고는 제작 범위 요약이며 본인의 증거 있는 납품 보고가 아니라는 기획 판단을 그대로 수용한다.
- 기획 정본: [Main23 시각 플레이패스·Main24 사전 계약](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/LIMITLESS_20261023_Chapter3_Main23_PresentationPass_Main24_SavePrep.md).
- Codex 요청: [Issue #58](https://github.com/HappyPaul812/Project-Limitless/issues/58) — 실제 Scene/Pivot/PPU/Sorting/Collider/3비율/동선과 `Main23_Frostway_VisualGapList.csv` 요청. **2026-10-23 본 아트 보고 시점에 공개 Issue #58 댓글 0개이며, 실 CSV와 GameView가 본 세션에 전달되지 않아 검증 `NOT_TESTED`.** 임의 실측값/우선순위 판정 금지.
- 기존 Main23 격리 **148PASS/0FAIL**은 Codex가 보고한 *기능* QA로, 이번 신규 아트의 품질·Import·Runtime PASS가 아니다. main 정식 반영 및 원본 Editor 손플레이와 분리한다.
- 최종 승인 지면9 Earth·ThinFrost·Gravel은 main 정식 등록분으로 **수정·대체0**, Chapter1·2·승인펫3종·공식NPC, 기존 Save/UserData 및 개인설정 보호.

## 2. 실제 신규 납품 여부 — 제출할 것이 없으면 미납품으로 표시

| # | 목표 | 신규 실 PNG | 실제 비교판/사이즈 | 검수·승인 |
|---|---|---|---|---|
| 1 | 마을 안내 NPC 오르나 1종 | **`NOT_DELIVERED`**. PNG 파일·SHA 없음 | 128px/40px PNG·필드 Pivot·PPU `NOT_DELIVERED / NOT_VERIFIED` | 이름 가칭, 공식 외형·StableID·VoiceID 미확정, 최종 승인 없음 |
| 2 | 남문 목석 건물 정면·문·지붕 접합 1세트 | **`NOT_DELIVERED`**. 기존 재사용 원본만 확인 | 실제 남문 배치·문/지붕 접합 전후 `NOT_TESTED` | GapList·GameView 및 Collider/동선 미확인 |
| 3 | 스킬상점 또는 여관의 외형·간판 1세트 | **`NOT_DELIVERED`**. 기존 SkillShop/Inn/간판 원본 확인 | 대체 전후·16:9/4:3/21:9 실게임 조립 `NOT_TESTED` | 시설 선택은 Codex 부족 아트 우선순위 확인 후 결정 |
| - | 기타 | 새 게임아트0·새 Unity 적용0 | GIF·임시 목업을 QA로 대체하지 않음 | 사용자 최종 미술 승인0 |

**미술 후보 3종의 새로운 ZIP·각 PNG SHA·실제 Runtime PASS를 지금 주장할 수 없다.** 관련 원본 PNG가 ZIP에 보관되어 있다는 사실은 본게임 배치나 미술 승인을 증명하지 않는다.

## 3. 실물 기준 기존 아트 재사용 감사 — 새 납품과 구분

이번 보고에서 **실제로 읽어 바이트 SHA256·규격을 확인한 소스**는 과거 사용자 대화 납품 `Limitless_Chapter3_FrostwayVillage_Environment_GameArt_VariantTiles_v9.zip`이며 SHA256은 `afea1ed0b357a5bdce9e962c7969b81b77d29e1ed11b7b9dc170d57747fbeb79`이다. 이 파일에 게임용 기존 PNG 49종(기존 43 + 이전 신규 변형6)이 있으며 49/49 RGBA를 파일 자체에서 확인했다. **현재 Codex의 `FrostwayVillage v5` 로컬 ZIP 실물과 SHA 비교는 수행하지 않았으므로 v5와 43/43 바이트 일치한다고 별도 주장하지 않는다.**

아래는 새 제작물이 아니라 **재사용 검토 후보**다. Path는 해당 실물 ZIP 안의 상대경로이고, PPU·Pivot·footprint·Sorting·Collider는 실제 Unity Import/Scene 확인 전 전부 `NOT_VERIFIED`다.

| 재사용 후보 상대경로 | 이미지 크기·형식 | 원본 PNG SHA256 |
|---|---|---|
| `Buildings/Frostway_Residence_Building.png` | 502×374 RGBA | `4eb505aad2aefe90170a1d3e3c08990fe4587b3a91d2136dcdc1f0e4ee37fd8a` |
| `Buildings/Modular/Frostway_Door_StoneFrame.png` | 149×210 RGBA | `b341458803f0c65ea6c995594246767d182ccb0e044d1633878797274576c34e` |
| `Buildings/Modular/Frostway_Door_WoodFrame.png` | 148×210 RGBA | `4b359df1726465b0100e6b43611c67d8f0829e3ce35481ccfff857d6c4cea34f` |
| `Buildings/Modular/Frostway_Roof_Gable.png` | 175×219 RGBA | `37ea6a69844b662c40583cea3d457ff0c47344aef7054ff8e32b4d09870dceb0` |
| `Buildings/Modular/Frostway_Roof_Shingle_A.png` | 167×191 RGBA | `433b4c89e3717496d1496e951ef8d9da4f07634cd7686458e7b7a585b93dff5d` |
| `Buildings/Frostway_SkillShop_Building.png` | 511×418 RGBA | `ef34b49de4f807debf4a69e1947eb9b8e02a543ee117ca2867fe3aa06a5efb17` |
| `Signs/Frostway_Sign_SkillShop.png` | 125×173 RGBA | `f86c2645fa7b6ffd65893de58b75358de641b7f032f317e56f01f57da14ab263` |
| `Buildings/Frostway_Inn_Building.png` | 540×443 RGBA | `67b16d2ef8c3d3d99e066c5723bd5df9a917624c79faeaadf32a99dd4a94f14d` |
| `Signs/Frostway_Sign_Inn.png` | 110×144 RGBA | `c16624d46acdda522a42d6ebeed03bd81eb23053dd11b91fb4b69f307fc2cb75` |

### 이번 턴에서 새로 만든 **참고 증거물** (신규 아트 납품이 아님)

- 오프라인 보존자산 참고판: `Main23_Existing43_Reuse_Contact_OFFLINE.png` / SHA256 `588a1d5844b34c6ce9ad887d284f5956a9416902b54ab2884d20b844a62feff5`. 기존 주택·스킬샵·여관·문/지붕 모듈을 **카드용 개별 크기**로 배치했으며 실제 씬 비율, 건물 접합 QA, Unity GameView가 아니다.
- 49개 실제 보유 PNG의 경로/규격/SHA 전수 CSV: `Existing_Frostway49_PNG_Inventory.csv`.
- 참고 감사 ZIP: `Limitless_Main23_Issue59_ReuseAudit_PRELIMINARY_20261023.zip`, SHA256 **`83d621d8f32fde8f31b10f10e4fa5fb29c25ff2677b472e74862f9eccd93739d`**, **774,179 bytes**. ZIP 내부: CSV·기존자산 참고판·Manifest.json·SHA256SUMS.txt. **ZIP CRC PASS, 내부 SHA256SUMS 3/3 PASS**.
- 위 실물은 **현재 ChatGPT 대화에서 생성한 첨부 파일**이며 GitHub 바이너리 업로드0. 따라서 GitHub 문서만으로 해당 PNG/ZIP 실물을 다운로드할 수 있다고 표현하지 않는다. 대화 첨부 파일 이름으로 조회하여야 한다.
- 기존 사용 가능한 환경 PNG의 **재사용 가능성 제시**에 한정하며, 신규 3개 아트와 실제 남문 건물·문 접합 전후 비교는 별도 제작·시각 검증을 필요로 한다.

## 4. 미해결 검수 및 생산 게이트

1. Codex #58의 **실제 `Main23_Frostway_VisualGapList.csv` 및 16:9/4:3/21:9 GameView/RenderTexture**, 남문 건물 문/지붕 연결 XY, Sprite PPU/Pivot/footprint/Sorting/Collider 자료 확보. 현재 `NOT_RECEIVED`. 추정 3WU 남북 통행, 7×7WU 미래 비활성 포탈 광장 가림 검수는 `NOT_TESTED`.
2. 이 자료로 v5 43종 중 KEEP과 실제 부족 아트를 분리한 뒤 **오르나 단독 PNG, 남문 목석 건물 접합 시안, SkillShop/Inn 중 우선1세트**만 비파괴 제작. 오르나는 가칭·디자인 후보. 기존 43 전체 재제작 불가.
3. 신규 128px·40px 오르나, 남문 건물 실제 스케일/문/지붕 접합 전후, 시설/간판 전후 및 화면비3종 **오프라인** 비교; 파일별 RGBA/크기/해시/투명 연결성·ZIP CRC/Manifest, 실제 사용·재사용 경계를 제시. 유니티 실측 화면과 오프라인 합성은 서로 다른 것으로 표기.
4. 바울 대표 미술 승인과 Codex 기술 QA/Unity 정식 적용은 **별도 게이트**. 승인 전 정식 Scene·NPC/StableID·VoiceID·스킬상점 구매·포탈 Trigger·main 반영 금지. OPEN16은 근거 없이 해결0/잔여16.
5. **GitHub 완료보고 운영**: 담당 브랜치 `docs/art-new`에 보고서+최신 인계 문서 커밋 → Issue #59 본인 결과 댓글 → 사용자에게 문서 URL/브랜치/SHA/실물 ZIP SHA와 승인 대기상태 전달. `main` 직접 커밋/Push/병합·강제Push 금지.

## 5. 현재 결론

**정식 증거 문서/재사용 후보 인벤토리 감사 및 오프라인 참고판 제작은 완료.**
**Issue #59의 요청 신규 아트 3종 실물 납품·접합 QA·Unity 적용은 완료되지 않았으며 최종 사용자 미술 승인 요청 단계가 아니다.**
