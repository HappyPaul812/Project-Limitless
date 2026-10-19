# LIMITLESS / 기본 야수 동료 Wolf 오리지널 Run 시트 파일럿 v1 납품

- **날짜:** 2026-10-19
- **GitHub 기준:** `main` `675988c9f2c5c507d9a696b0378be27c0bd3309b`
- **기획 정본:** `docs/planning/문서/00_프로젝트/Chapter1_2_Monsters_And_Beasts_Original_Art_Renewal_Assignment_20261019.md`
- **담당 브랜치:** `docs/art-renewal`
- **상태:** `WOLF_ORIGINAL_PILOT_DELIVERED / OFFLINE_PNG_QA_PASS / USER_ART_APPROVAL_PENDING / BEAR_FOX_STYLE_SPECS_ONLY / UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`

## 1. 현재 게임의 실제 제3자 Run 연결 및 규격

실제 읽은 정본: `Scripts/Battle/BeastCompanionSystem.cs`, `Scripts/Core/BeastCompanionService.cs`, `Scripts/UI/PetFacilityPresenter.cs`, `Resources/CompanionAssaultValidation/README.md`, `Wolf_Run.png.meta`, `ThirdParty/ScratchIO/AnimatedWildAnimals/LICENSE.txt`, `PROJECT_USAGE.md`.

- 외부 원본 ScratchIO / **Animated Wild Animals / CC0 1.0**, OpenGameArt 공식 출처. `All.zip` 원본 보존. 현재 `CompanionAssaultValidation/{Wolf,Bear,Fox}_Run.png`는 **옛 README에는 '연출 미연결'이라고 쓰여 있으나 현재 C# 실사용**. 기존 원본에 의도적 삭제/바이트 수정0.
- 세 종의 **실제 기존 PNG header + 현 Catalog**:
  - **Wolf:** **384×40 = 64×40×6장**, `FrameWidth64, FrameCount6, FPS12, IconFrameIndex4(0-base), DisplayScale2, TravelSpeed570, 기본 LEFT, pivot(.5,0)`, source importer GUID `8c410e828c92433389e8f18e8536bb2e`.
  - **Bear:** **320×33 = 64×33×5장**, `FPS12 / DisplayScale2 / IconFrameIndex0 / TravelSpeed570 / LEFT / bottom pivot`.
  - **Fox:** **384×36 = 64×36×6장**, `FPS12 / DisplayScale2 / IconFrameIndex0 / TravelSpeed570 / LEFT / bottom pivot`.
- 기본 Wolf는 직접 피해5%(DoT 제외), Bear 최대HP10%, Fox 민첩+2. **BeastCompanionService**는 기본 세 종 무료, 장착 가능한 사수만 사용, 몬스터펫만 승리 이력 및 100탈렌트 구매를 요구. **세린은 기본 Fox**이며 주인공이 다른 직업이어도 세린이 사수로 참가·해금된 경우 관리 UI에서 야수를 사용 가능. 야수는 파티 슬롯/독립 턴/HP 없음. 몬스터펫은 원래 MonsterDefinition 시트를 활용하는 계약 유지.

## 2. 실제 Wolf 신규 오리지널 PNG ZIP

- **영문 파일명:** `Limitless_Beast_Companions_Wolf_v1.zip` (이 대화에서 제작; 대표님 F: 디스크 자동 기록 아님).
- **55,181 bytes / 20파일 / PNG10 + 애니메이션 Preview GIF1.**
- **ZIP SHA256 `d6c490df6a458c95ad55e2f0eb1b902c4db5768aee529184a335545b00bdb56b`.**
- 필수 실사용 후보 `Art/LL_Beast_Wolf_Run_384x40_v1.png`, **384×40 RGBA / LEFT 6프레임×64×40 / 12FPS**. 기존 ScratchIO 원화 픽셀/실루엣 추적·재색칠하지 않고, 완전히 새 Slate/Gray·Ivory Chest 색과 털·꼬리·네발 달리기 애니메이션을 신규 pixel geometry로 작성.
- `Art/LL_Beast_Wolf_IconFrame04_64x40_v1.png` (실제 Run의 5번째 프레임 원본 영역의 동일바이트 Pixel Crop, **선택 UI 참조**, 기존 UI에서 Run을 자동 잘라 쓰므로 별도 설치 필수 아님).
- `Frames/Wolf_Run_00..05_64x40.png` **6개** 개별 프레임, `Preview/LL_Wolf_Run_ContactSheet_v1.png`, `Preview/LL_Wolf_Icon40_Preview_v1.png`, `Preview/LL_Wolf_Run_Animated_v1.gif`.
- `Manifest.json` PNG10의 SHA256·Canvas·Alpha BBox·PPU와 bottom-center Pivot 후보, `Animation_Contract.csv` 6행, `Source_Mapping.csv`, `Beast3_Source_Rights_Audit.csv`, `Bear_Fox_Style_Contract.md`, `README.md`, `Rights_and_Provenance.md`, `QA/Offline_Validation.json`, `Codex_Integration_Handoff.md`.
- 오프라인 ZIP CRC PASS, PNG10 decode PASS, manifest PNG10 SHA 일치, Run6 전부 비어 있지 않음, **프레임 네 변 비투명0·동일 프레임 중복0**, 개별6 PNG·5번째 Frame4 Icon이 실제 Run 원본 pixel과 일치 PASS. **실제 Unity Sprite.Create의 PPU/스케일 영향과 화면·접지 및 스킬 타이밍은 미검증**.

## 3. Bear / Fox는 그림 제작 대기

- Bear 기존 5×64×33, Fox 6×64×36 **각기 다른 원본 높이·프레임 수를 보존하는 것**이 우선. Wolf6에 맞춰 강제6프레임으로 일괄 변환하지 않습니다.
- Bear는 굵은 전신·무거운 착지, Fox는 짧은 몸·긴 붓꼬리·경쾌한 점프. Wolf처럼 색상만이 아니라 전체 실루엣·동작으로 구별.
- **두 종 신규 PNG 제작0**, 이번 ZIP에는 **스타일 및 실제 Animation Contract만** 포함. **Wolf 파일럿을 바울 대표가 미술 승인한 후** 두 종류 제작 요청 수용.

## 4. 이후 Codex 전용 통합 게이트

1. 바울 대표가 Wolf 6동작 프레임 흐름, 5번째 프레임 아이콘, 40px UI 판독성 미술 승인.
2. Codex가 격리 worktree에서 NEW `Resources/CompanionAssaultOriginal/Wolf_Run.png`로 **새 GUID**를 발급해 Import(원본 `CompanionAssaultValidation/Wolf_Run.png`는 보존), `BeastCompanionCatalog.DefaultWolf.RunResourcePath`만 신규 경로로 변경 검토. 현재 소스의 `Resources.Load<Texture2D>` 프레임 런타임 crop / Sprite.Create Pivot(.5,0) 확인 필수. 내부 PixelPerUnit 및 2배 표시/속도 영향은 Unity 실측 검증.
3. `동료의 습격` 좌우 돌진·피격 타이밍/거리(TravelSpeed570), 12fps·Run6·기존 IconFrameIndex4·PetFacility 작은 아이콘, 세린 기본 Fox, Bear HP, Wolf DMG, 100 Talent monster-pet 해금, Save/Continue/접근성 회귀 검사.
4. 본 문서 기록은 **공식 적용·게임 Runtime PASS가 아님**. 미술 승인 전 Import/Scene/BeastService/MonsterDefinition/C#/원본 Source 변경 금지. Chapter2 공식 몬스터5·보스·ThirdParty 라이선스 보존.
5. 담당자는 `docs/art-renewal`에만 문서 기록, **main 직접 Commit/Push/병합 없음**.

상세 Monster18 감사 및 동시 제작 [VenomSnake 파일럿](Monster_VenomSnake_Original_Art_Pilot_v1_Delivery_20261019.md).
