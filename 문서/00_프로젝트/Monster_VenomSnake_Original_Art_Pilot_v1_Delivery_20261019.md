# LIMITLESS / Chapter1·2 몬스터 18종 감사 및 맹독뱀 오리지널 파일럿 v1 납품

**날짜:** 2026-10-19 / **기준 `main`:** `675988c9f2c5c507d9a696b0378be27c0bd3309b` / **담당:** `docs/art-renewal`.  
기획 정본: `docs/planning/문서/00_프로젝트/Chapter1_2_Monsters_And_Beasts_Original_Art_Renewal_Assignment_20261019.md`.  
**상태:** `MONSTER18_SOURCE_AUDITED / VENOM_SNAKE_ORIGINAL_PILOT_DELIVERED / OFFLINE_PNG_QA_PASS / USER_ART_APPROVAL_PENDING / UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`.

## 기존 정의 18종 + 지적재산권 경계

`Resources/MonsterDefinitions/01_GrassSlime.asset`~`18_VeinfireColossus.asset` 총 **18개 원격 YAML**을 각각 확인. 실제 Frame 배열 수, GUID, FieldSprite/TextureSheet/ResourcePath, Rights/승인 근거를 파일럿 ZIP의 **`Monster18_Source_Rights_Audit.csv` (18행)**에 기록.

| 대상 | 분류 및 보호/후속 방향 |
|---|---|
| `01 grass_slime` | 전용 Sprite/Controller 기등록; 승인/저작권 근거 추가 확인 전 **KEEP/HOLD** |
| `02 venom_bee` | 2D!PIXX PilotBee 제3자 CC BY. 공개 페이지 3.0 vs 원본 LICENSE 4.0 표기 불일치. **차기 오리지널 후보**, 원본 보존·권리 귀속 검토 |
| `03 forest_spider` | ethanf30games 2D Spider CC0, Runtime Resources 8개 프레임/동작별 사용. **차기 오리지널 후보**; Attack·Shoot 등 보존 |
| **`04 venom_snake`** | XenosNS Simple Green Snake CC0, 현재 128×32 / 4×32×32 8 FPS. **이번 최초 오리지널 시트 제작 대상** |
| `05~07 moss_beetle/shade_bat/grave_wight` | 현재 개별 전용 Battle/Field Sprite 및 Explicit 프레임이 있으므로 **재제작 HOLD**, 원본 권리·최종 미술 승인 근거 재확인 |
| `08~10 silent_echo/seal_guardian/silent_warden` | 기존 전용 Story/Boss Sprite·프레임. **보존 / 보스 불필요 재작업 금지** |
| `11~15 soot_hound/heatwind_hawk/fissure_lizard/ember_beetle/ember_wraith` | **Chapter2 공식 보호 5종, 재제작 0** |
| `16~17 obsidian_beetle/scorching_watcher` | Main18 등록 공식 시트, Imported Art 기술 PASS_WITH_NOTE와 최종 미술 검토 구분. **기존 등록품 보호** |
| `18 veinfire_colossus` | Main20 보스 공식 v3, 두 단계·Animator/Frame 계약 **엄격 보호** |

실제 원본 소스: `Assets/ThirdParty/XenosNS/SimpleGreenSnake/README.md`(CC0), `Assets/ThirdParty/ethanf30games/2DSpider/README.md`(CC0), `Assets/ThirdParty/2DPIXX/PilotBee/README.md`와 `Original/LICENSE.TXT`(CC BY 버전 차), `MonsterSpriteSheetAnimation.cs`, 실제 `04_VenomSnake.asset`.

기존 권리 자료의 **명시되지 않은 승인·권리 보유**를 임의 확정하지 않습니다. 출처가 충분히 확인되지 않은 기존 전용 시트를 무료/임시라고 부르지 않습니다.

## 제작 파일럿: `venom_snake` 새 PNG 원화·프레임

새 ZIP 다운로드명 **`Limitless_Monster_Renewal_Pilot_VenomSnake_v1.zip`**  
**크기 81,582 bytes / 35개 파일 / PNG26개 / ZIP SHA256 `8eb8d512b1ec1249f18f4770616e988acbee449dfc74a63678eb5916fb37d303`**.

- 완전히 새로운 뱀의 청록색 코일·독 보라색 관/황금 눈/노란 배, **원본 제3자 이미지 픽셀 복사·추적·단순 리컬러0**. 소스 코드는 PIL 기반 신규 픽셀 도형 렌더링.
- 실제 납품: **Idle/Walk/Attack/Hit/Defeat 동작 5개 × 4프레임 = 20프레임**. 128×32 RGBA 시트 **5장**과 32×32 단일 PNG **20장**, Contact Sheet PNG **1장** 및 모션 Preview GIF **1개**.
- 원본 MonsterDefinition: `fieldSprite={fileID:0}`; Idle/Attack가 원래 **동일 원본 128×32**(GUID `4eebc05f001d494daecb632a39bcfb95`, fileID2800000)을 참조, `sourceFacesRight=0`, `FPS=8`. 신규 파일은 **새 UID로 별도 Import할 후보**일 뿐 원본을 교체한 것은 아님.
- 제안 설정: Cell32×32, 4열1행, PPU32, CenterPivot(0.5,0.5), Point, Mipmap OFF. 기존 `MonsterSpriteSheetAnimation`가 Idle/Attack을 `Texture2D`로 프레임 추출. **Walk·Hit·Defeat는 12개 개별 Sprite를 Explicit 배열에 별도 연결해야 실제 Runtime에서 작동**(현재 게임 자동 연결 기능 없음). 새 Animator·콜라이더·MonsterID·전투 능력·HP·DoT·AI 수정은 포함하지 않음.
- Package includes: `Manifest.json` PNG26개별 SHA256/Canvas/Alpha bbox/PPU/Pivot, `Monster18_Source_Rights_Audit.csv` 18행, `Source_Mapping.csv`, `Animation_Contract.csv` 20행, `README.md`, `Rights_and_Provenance.md`, `Codex_Integration_Handoff.md`, `QA/Offline_Validation.json`, `Preview/LL_VenomSnake_ContactSheet_v1.png`, 애니메이션 GIF.
- QA: ZIP CRC `testzip()==None`, PNG26 Decode PASS, Manifest PNG26 SHA PASS, 20개 프레임 비어 있음 0, 프레임 네 변의 불투명 픽셀 0, 같은 동작의 4프레임 byte 동일 중복0, 시트/분리 PNG 각 프레임 일치. **실제 Unity/OS/Play/Focus/화면·방향·전투·Save/MonsterPet QA는 NOT_VERIFIED**.

## 보호 및 진행

- Chapter1·2 18종 정의/Legacy/ThirdParty/Chapter2 공식 Sprite/Mesh/Animator/Scene/MonsterSpawn/MonsterDefinition/Boss/Save/GamePlay 변경0. 신규 PNG는 채팅 다운로드용 격리 ZIP에만 제공; 사용자 F드라이브 로컬에 자동 저장한 것 아님.
- **바울 대표의 파일럿 미술 승인 필요**: 독립적인 실루엣·다섯 동작·작은 크기 32px 읽힘. 승인 후 Codex가 별도 worktree에 신규 GUID로 Import하고 `venom_snake` 시트와 Explicit Walk/Hit/Defeat 12개 연결 및 Source Sprite/Save/Poison/Field·Battle 실제 검증. 승인 전 Unity 적용0.
- Fox·Bear 등 펫은 별도 파일럿 [Wolf 기록](Beast_Wolf_Original_Run_Pilot_v1_Delivery_20261019.md). 그다음 몬스터 확대 제작은 별도 재발주·개별 미술 승인 후.
- GitHub는 `docs/art-renewal` 문서만 Commit, **main 직접 Commit/Push/병합0**.
