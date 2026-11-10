# 2026-11-10 Codex — F2·맹독뱀 ZIP 검토 / 시작마을 상점 물약 아이콘 조사

- 작업 브랜치: `codex/20261110-main-renewal-prop-batch`
- 기준 local main: `6bebbfae6850aa60411feb6bb818523e8ef72397`
- 구현 커밋: **없음 — 적합한 내부 HP/MP 대체 자산을 확인하지 못해 제품 수정 HOLD**.
- 상태: `F2_BENDSNOW_REVIEW_DONE / VENOMSNAKE_REVIEW_DONE / STARTERVILLAGE_SHOP_ICON_REPLACEMENT_HOLD / REMOTE_NOT_PUSHED`.
- F2·맹독뱀 Unity 적용0, NPC 신규0, 원본 PNG/WAV/meta/GUID/Scene/Prefab/Save/Packages/BuildSettings 수정0.

## 확인한 최신 정본과 충돌 처리

GitHub main의 COMMON_RULES·GitHub 보고 정책, docs/planning의 PLANNING_LATEST, docs/art-new의 SPRITE_ART_LATEST, docs/art-renewal의 WORLD_ART_RENEWAL_LATEST, Issue #75·#43 최신 댓글을 먼저 대조했습니다. 원격 CODEX_LATEST는 10/24 기록이므로 로컬 11/09 인계·실제 HEAD와 최신 기획의 사용자 수용 기록을 우선 대조했습니다. 원격 main을 최신 제품으로 덮어쓰지 않았습니다.

- [#75 최신 발주](https://github.com/HappyPaul812/Project-Limitless/issues/75#issuecomment-6755493865), [실물 납품·방향 수용](https://github.com/HappyPaul812/Project-Limitless/issues/75#issuecomment-6756122488).
- [#43 단일포즈 마감 납품](https://github.com/HappyPaul812/Project-Limitless/issues/43#issuecomment-6755910265), [#57 비고유 자산 감사](https://github.com/HappyPaul812/Project-Limitless/issues/57).
- 최신 기획의 NPC 2종 발주가 있으나 이번 바울 대표 지시는 **뱀 우선·NPC 신규 금지**이므로 이번 작업에 적용하지 않았습니다.
- F2는 이미 “후보가 괜찮네”라는 방향 수용 기록이 있습니다. 이를 게임용 규격/본편 적용 승인으로 확대하지 않았습니다.

## A. F2 BendSnow — 부분 수정 필요

입력: `F:/Downloads/LIMITLESS_Issue75_F2_BendSnow_ArtCandidate_20261110.zip`

- ZIP SHA256: `151094aacd847ee7b4338ca08a827d0edaa21a1122df357379f0bd493afabd8a`
- 3,435,324 bytes / 10 entries / 실제 전체 CRC PASS / SHA256SUMS **8/8 일치**.
- Manifest 존재. F2 Manifest는 파일별 전체 목록이 아니라 후보 규격/상태 설명입니다. 후보 SHA 일치. SHA256SUMS에는 QA_Result.json 및 자기 자신이 없으며, 전체 ZIP 해시와 CRC는 두 파일도 포함합니다. 독립 파일 목록/해시는 Evidence에 기록했습니다.
- 신규 후보: `Candidate/F2_SnowPath_BendEdge_ArtApproved_CANDIDATE_1774x887_RGBA.png` 1774×887 RGBA, SHA `1db66eb4f92931318d5fafd46b2cd337fb9d47033537b428e21c1471d71490dd`.

### 실제 시각 판단

원화·Before/After 콘셉트 비교판·128×64와 40×20 실제 축소 PNG를 직접 확인했습니다. 길 중앙의 흙 노출이 연속적이고 굽음의 안쪽/바깥쪽 경계가 둥글어 직각 인상이 줄었습니다. 눈 덩어리의 크기·간격이 달라져 정렬된 눈꽃 패치 반복보다 자연스럽습니다. 눈과 흙이 재질·실루엣으로도 구분됩니다. 기존 첫눈 숲 방향과 색조는 무리 없으며 바위·관목이 이미 포함되어 있다는 점은 중복 배치 위험입니다.

**남은 문제:** 비교판은 생성된 OFFLINE 콘셉트이며 기존 After_Bend_1280 실제 화면의 지정 ROI 전후가 아닙니다. 따라서 실제 맵 반복 해결, 휠체어 4.5WU, 960/1280/1680 화면 점유, HUD 뒤 나무 가림 해결을 PASS로 판정할 근거가 없습니다. 그림상 통행 가능한 넓은 흙길 인상은 있으나 WU는 PNG 화살표로 측정할 수 없습니다. 전체 PNG를 바로 바닥에 쓰면 바위·관목과 눈 경계가 기존 F2와 중복될 수 있고 PPU/Pivot/타일 접합도 미정입니다. 40px에서는 굽은 띠는 남지만 잔설의 개별 모양은 뭉칩니다.

**판정: 부분 수정 필요.** 미술 방향 검토에 올릴 수 있으며 기존 방향 수용은 유지합니다. 다음 납품에서는 실제 기준 화면 ROI 위 기존 소스 재배치 제안, 게임용 부분 추출/접합 범위와 규격을 구분해야 합니다. 이번에는 후보 원본을 수정하지 않았습니다. 실제 배치·HUD·통행은 미검증, 사용자 최종 미술/본편 적용 승인 필요. 기존 Ground9/F2 P1·P2/Owl/Stump/환경43은 수정하지 않았습니다.

## B. VenomSnake — PASS (단일포즈 미술 승인 상정용)

입력: `F:/Downloads/LIMITLESS_VENOMSNAKE_SINGLEPOSE_CLOSEOUT_20261110.zip`

- ZIP SHA256: `5c708234539a7751c1646508feb5fc66e8a5e11439b222494145e99923980b8b`
- 3,619,505 bytes / 14 entries / 실제 전체 CRC PASS / SHA256SUMS **13/13** / Manifest **12/12 bytes·SHA 일치**.
- 후보 `Candidate/VenomSnake_KeyArt_v3_FINAL_CANDIDATE_SOURCE_KEEP.png`: 1254×1254 RGBA / 2,285,408 bytes / SHA `7001d199b279cb8e541fc603bd62351ab460d3b44da91bc91a2d65dc099f8fd0`, 이전 v3 정본 SHA와 동일합니다. 새 v4가 아닙니다.

### 실제 시각 판단

512/128/40 PNG 및 3종 비교판을 직접 확인했습니다. 우향 머리·들어 올린 목·바닥 코일의 S자 연결이 명확하고 밝은 배 비늘이 몸통과 분리됩니다. 금갈색/검은 비늘과 녹황 무늬, 큰 송곳니로 독성·위협감이 전달됩니다. 과거 v2 32px 후보보다 실루엣·입·곡선의 볼륨이 명확합니다. 기존 정식 Obsidian Beetle 시트도 확인했으며 어두운 윤곽·명암으로 볼륨을 주는 판타지 방향은 맞지만, v3의 비늘은 더 조밀합니다. 동일 Unity 월드 크기/PPU의 화풍 일치까지 검증한 것은 아닙니다.

128px에서 눈·입·코일이 읽히며 40px에서도 우향 공격 자세와 코일 실루엣은 남습니다. 다만 40px에서는 독니·비늘·독 무늬가 뭉쳐 세부 식별은 제한됩니다. 유혈·상처 표현은 없고 약해 보이지 않습니다. 크게 벌린 입과 긴 독니의 공포감 수위는 최종 미술 판단 사항입니다.

**남은 문제:** 원본 왼쪽 경계 alpha>0 **25px**를 독립 재측정했습니다. 기존 미세 고립 알파 경고를 삭제/수정한 것으로 주장하지 않습니다. 실제 배경 위 가장자리와 PPU/축척 정리는 게임 적용 전 별도 필요합니다. 미술 원화의 최종 승인 여부와 Unity import 기술 승인은 별개입니다.

**판정: PASS — 단일포즈 사용자 최종 미술 승인에 상정 가능.** 원본 보존 조건으로 후보 유지. 게임용 alpha/import·Idle/Attack·실전 접점 PASS는 아닙니다. **NPC 착수는 현재 보류: 바울 대표의 뱀 최종 미술 마감 판단 후 다음 업무로 가능**하며, 이번 작업에서는 NPC 제작을 시작하지 않습니다. 사용자 최종 미술 승인 필요, Unity 적용0/모션 양산0.

## C. 시작마을 물약 — 거래창 아이콘으로 대상 확정 / 교체 HOLD

바울 대표가 이번 대화에서 **“상인 거래창의 물약 아이콘”**이라고 확정하셨습니다. 처음의 월드 소품 조사와 구분합니다. 외부 물약 선반을 임의 추가·교체하지 않았습니다.

### 실제 참조와 외부 출처 확인

`StarterVillageGeneralShop.asset`의 `shop_general_starter_village`가 `item_healing_potion_small`·`item_mana_potion_small`을 판매합니다. `StarterVillageHubInstaller`의 `starter-village-general-shop` → `VillageNpcRole` → `ShopPresenter.OpenStarterGeneralShop` → `BuildItemRows`가 ItemDefinition.Icon/IconTint를 **64×64** Image에 표시합니다.

| 아이템 | 실제 PNG (Resources/ItemIcons/) | GUID | 확인한 외부 원본 |
|---|---|---|---|
| 회복약 | HealingPotionSmall.png | a9ac67de65b88774ea6e90e22805af16 | Kenney Board Game Icons `PNG/Double (128px)/flask_full.png` |
| 마력 회복약 | ManaPotionSmall.png | c062cadfd7ff1b94e8c89e77f74cf048 | 동일 ZIP `PNG/Double (128px)/flask_half.png` |

회복약 PNG SHA `9cc986f90aefa417e174ba2eeeb6f6f82c25e1bb4156b120ae8cdf273d96577f`, 마력 PNG SHA `cd2331d9e743e29373933ed35fc0594d57e7419d244eb1da65ebdd4b7f529b07`. 둘 다 로컬 원본 `kenney_board-game-icons.zip`의 해당 파일과 **바이트 SHA 정확 일치**. `_Project` 아래 저장되어 있어도 내부 제작물은 아닙니다. 각각 ItemDefinition만 직접 직렬화 참조하나 공용 ItemCatalog를 통해 다른 상점/인벤토리/전투에서도 공유됩니다. 따라서 공용 PNG/GUID 교체는 이번 범위를 넘습니다.

### 내부 대체 자산 탐색과 보류 근거

Assets/_Project 전체 아이콘 경로, 기존 시작마을 리뉴얼 Phase1A·v1~v4 ZIP, Downloads/Limitless_Sprites ZIP 파일 목록, 기존 내부 아이템 시트·관련 문서를 확인했습니다. 내부 `Battle_Item_Icons_128x128_Sheet.png`는 정본상 **아이템 명령 가방 / 해독제 / 화상 연고 / 절연 물약 / 침묵 해제 물약**입니다. HP/MP 회복약 전용2종이 아닙니다. Main18의 Cooling Remedy도 다른 기능의 아이템입니다. 다른 효과 그림을 임의 재지정하거나 재채색·새로 제작하지 않았습니다.

**HOLD: 조사한 위치에서 HP/MP 회복약에 적합한 기존 내부2종 및 승인된 대응 관계를 찾지 못했습니다.** 내부 원본2종의 위치/역할이 확인되면 시작마을 상점 한정 표시 연결로 진행할 수 있습니다. 이는 신규 아트 요청이 아닙니다. 현재 새 코드·설정·PNG 추가0, 실제 교체 파일0, 사용한 대체 스프라이트0. 제품 적용·구현 commit 없음.

### 월드 조사·캡처·QA 경계

- World_StarterVillage 직접 Sprite 참조284개: 내부 환경15종. 실제 Unity AssetDatabase 의존성50개에 ThirdParty0. Editor Preview Scene은 prefab 포함 SpriteRenderer286개(환경284+Player1+null NPC1). 물약 선반 A/B/C의 GUID를 Assets의 Scene/Prefab/asset 전체에서 검색한 참조0. 런타임 상인 외형은 별도 OGA03 NPC이며 이번 NPC 금지 범위로 유지했습니다.
- Unity **6000.6.5f1** 실제 연결 확인. 원본 Play OFF/Focused false/compiling false. 현재 active Scene은 별도 과거 worktree의 Bootstrap이며 이 상태를 임의 전환하지 않았습니다.
- `Evidence/BatchArtShop20261110/StarterVillage_Baseline_EditorPreview.png`: Unity에서 별도 Preview Scene으로 렌더한 **변경 전 월드 참고 캡처**입니다. 실제 PlayMode/상점 거래창 캡처가 아니며 임시 카메라 크기도 QA 전용입니다. 활성 Scene 변경0·dirty=false 유지·Preview Scene 종료.
- 거래창 전/후 캡처: **미확보**. 제품 교체를 하지 않았으므로 After를 만들거나 오프라인 합성을 실제 화면으로 표시하지 않았습니다.
- 신규 QA: ZIP·GUID·출처·참조 감사 PASS. 실제 교체 Delta QA/도보·휠체어·NPC 상호작용·구매/판매/메뉴 복귀는 **NOT_RUN (교체 HOLD)**. FAIL을 숨긴 것이 아니며 이전 대규모 QA 재실행0. MCP 임시 실행 코드 첫 시도에서 Object 이름 모호성으로 메모리 컴파일1회 실패 후 UnityEngine.Object로 수정해 성공했습니다. 제품 코드 작성0·현재 Console Error0.

## 보호·로컬 반영·다음 작업

원본 tracked dirty302·untracked550와 Save/UserData·이미지/음성/meta·설정 포함 보호3921개 파일의 SHA 기준을 보관했습니다. 원본 파일에 대한 Codex 쓰기0. 보호 전후 재대조 결과는 같은 Evidence의 protection-summary.json을 따릅니다. Save 내용과 개인 보호 목록은 Git에 넣지 않았습니다.

이번 로컬 main 반영은 안전한 경우 **검토 보고·인계 문서만 ff-only**로 전달합니다. 제품 교체의 local main 적용은 HOLD입니다. main 직접 commit/원격 Push·병합0. 실제 문서 커밋과 최종 local main SHA는 Git HEAD 및 #75/#43/#57 결과 댓글에 기록합니다. 로컬 보고서는 원격 Push하지 않으므로 원격 파일 링크가 존재한다고 주장하지 않습니다.

다음: ① HP/MP 기존 내부 원본2종 경로/역할 확인 후 시작마을 거래창 한정 연결 ② 해당 변경만 Unity6000.6.5f1 숨김 Play·실 거래창 전후/구매판매·이동잠금 해제 Delta QA ③ 충돌0·보호 검증 후 별도 브랜치 commit→local main 안전 FF. F2·뱀은 사용자 미술/본편 게이트를 계속 유지합니다.

## ZIP 내부 검토 파일 전체 목록

아래 목록은 실제 로컬 ZIP에서 읽은 entry 전체이며 PNG 원화/비교판은 직접 시각 확인, 텍스트/JSON은 무결성·규격·출처·검증 경계를 확인했습니다.

### F2

- `Candidate/F2_SnowPath_BendEdge_ArtApproved_CANDIDATE_1774x887_RGBA.png`
- `Docs/DesignNotes.md`
- `Docs/SourceDeclaration.md`
- `Manifest.json`
- `Preview/F2_BendSnow_ConceptComparison_OFFLINE.png`
- `Preview/F2_BendSnow_Width128_RatioPreserved.png`
- `Preview/F2_BendSnow_Width40_RatioPreserved.png`
- `QA_Result.json`
- `README_KR.md`
- `SHA256SUMS.txt`

### VenomSnake

- `Candidate/VenomSnake_KeyArt_v3_FINAL_CANDIDATE_SOURCE_KEEP.png`
- `DesignNotes.md`
- `Evidence/QA_Results.json`
- `Evidence/V2_Idle00_REFERENCE_SHA.json`
- `Manifest.json`
- `Preview/VenomSnake_v3_128px_OFFLINE_RGBA.png`
- `Preview/VenomSnake_v3_40px_OFFLINE_RGBA.png`
- `Preview/VenomSnake_v3_512px_OFFLINE_RGBA.png`
- `README_KO.md`
- `Review/VenomSnake_v2_vs_v3_OFFLINE.png`
- `Review/VenomSnake_v3_512_128_40_OFFLINE.png`
- `Review/VenomSnake_v3_Dark_Light_Forest_128_40_OFFLINE.png`
- `SHA256SUMS.txt`
- `SourceDeclaration.md`
