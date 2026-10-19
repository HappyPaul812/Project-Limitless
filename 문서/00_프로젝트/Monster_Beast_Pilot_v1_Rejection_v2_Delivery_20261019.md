# LIMITLESS — 맹독뱀·Wolf 파일럿 v1 반려 및 v2 재작업 납품 (2026-10-19)

- 담당: Chapter 1·2 기존 아트 리뉴얼 `docs/art-renewal`.
- 요청 승인: 바울 대표의 **v1 명시적 반려 및 v2 제작 지시**. v2 자체에 대한 최종 미술 승인은 **별개이며 대기**.
- 기준 GitHub main: `675988c9f2c5c507d9a696b0378be27c0bd3309b`.
- 상태: `V1_REJECTED / V2_ART_DELIVERED / OFFLINE_FILE_QA_PASS / USER_V2_ART_REVIEW_PENDING / UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`.

## 1. v1 반려 사유 및 v2 수정 방향 — 확정

| 자산 | v1 반려 사유 | v2 수정 방향 |
| --- | --- | --- |
| `Limitless_Monster_Renewal_Pilot_VenomSnake_v1.zip` | 뱀이 무섭지 않음 | 독사다운 날카로운 쐐기형 머리, 위험한 눈매, 독니/열린 입, 긴장된 S자 목과 똬리, 공격 직전 및 타격 프레임에서 적극적인 위협감. 귀여움/둔한 실루엣 금지 |
| `Limitless_Beast_Companions_Wolf_v1.zip` | 늑대가 '보노보노에 나오는 강아지 캐릭터'처럼 만화적임 | 지나친 큰 눈·짧은 주둥이·동글동글한 유아 비례를 피하고 길고 좁은 주둥이, 선 귀, 좁고 탄탄한 체형, 길게 뻗는 다리, 현실적인 판타지 야수 실루엣 |

v1 ZIP/문서/권리 기록은 역사 원본으로 보존. v1을 미술 승인 완료 혹은 Unity 적용으로 승격하지 않으며 승인된 기존 공식 몬스터/펫/스킬/기획/권리/Source/Scene/GUID/meta/Save는 재제작·수정하지 않습니다.

## 2. v2 실제 새 PNG/시트 납품 — ChatGPT 아티팩트

**A. `Limitless_Monster_Renewal_Pilot_VenomSnake_v2.zip`**
- SHA256: `b4d2c023c312893bb5035620bcfa049131b25b9a471920e4da16c60481ef4e38`
- 82,330 bytes / ZIP 엔트리39 / PNG26.
- **실사용 PNG**: `Art/LL_VenomSnake_{Idle,Walk,Attack,Hit,Defeat}_128x32_v2.png` 5종 (각 128×32 RGBA, 32×32 4프레임, 8fps, LEFT, PPU32, Center Pivot/Point/NoMip 제안), 개별 PNG20장.
- `Preview/LL_VenomSnake_v2_ContactSheet.png`, 동작별 GIF 5개, `Manifest.json` 개별 SHA256, `Source_Mapping.csv`, `Animation_Contract.csv` 20행, `QA/Offline_Validation.json`, `Generator_Source.py`, 출처/인계 문서.
- 기존 `04_VenomSnake.asset` Idle/Attack TextureSheet 연결과 호환 방향이나 **Walk/Hit/Defeat 12개 Explicit 프레임을 Codex가 승인 후 새로 연결해야 Runtime에서 실제 실행**. 기존 CC0 ThirdParty 원본 수정/복제0.

**B. `Limitless_Beast_Companions_Wolf_v2.zip`**
- SHA256: `cbcb62a766ae044bf4b1ca549eeb16027f91938aab5044eb7d701a574ef106cd`
- 47,659 bytes / ZIP 엔트리21 / PNG12.
- **실사용 PNG**: `Art/LL_Beast_Wolf_Run_384x40_v2.png` (384×40 RGBA, 64×40 6프레임, LEFT, 12fps, Pivot bottom-center). 6개 개별 프레임, `Art/LL_Beast_Wolf_IconFrame04_64x40_v2.png` (Run 0-based index4 exact crop), 40px 원본 크기 판독성 검토용 PNG 및 확대 검토본.
- `Preview/LL_Beast_Wolf_v2_ContactSheet.png`, 움직임 GIF, `Manifest.json`, `Source_Mapping.csv`, `Animation_Contract.csv` 6행, `QA/Offline_Validation.json`, `Generator_Source.py`, 권리/인계 문서.
- 기존 ScratchIO/CC0 Run PNG, `BeastCompanionCatalog.DefaultWolf`, `RunResourcePath`, PetFacility, 세린 기본 Fox, Wolf/Bear/Fox 능력 및 저장/전투 모두 **현 프로젝트 원본 변경 없음**.
- Bear·Fox v2 신규 그림 제작0. **Wolf v2 자체 미술 승인 후** 동일한 현실적 판타지 톤으로 제작 검토.

두 ZIP의 픽셀 아트는 기존 제3자 이미지를 자르거나 추적·단순 리컬러한 것이 아닌, 별도로 작성한 코드의 새 픽셀 도형 산출입니다. `Generator_Source.py` 포함. 생성 과정에서 만든 참고 컨셉 이미지 자체를 Sprite Sheet로 사용하지 않았습니다.

**납품 장소 명확화:** 두 v2 ZIP은 이번 ChatGPT 세션에서 다운로드 가능한 **실제 파일**이며, **GitHub 저장소나 바울 대표 F: 드라이브로 자동 업로드·복사하지 않았습니다.** 후임은 위 정확한 ZIP 이름과 SHA256으로 로컬 수령 후 대조합니다. GitHub에는 이 내용과 인계만 기록합니다.

## 3. 오프라인 자체 QA

- 각 ZIP `zipfile.testzip() == None` (CRC), 모든 엔트리 검출, 모든 PNG Decode와 Manifest SHA256 불일치0.
- VenomSnake: 5 sheets×4 frames와 개별 20프레임 이미지가 각 픽셀 단위 동일; 빈/중복 프레임0; 32×32 프레임 네 변 Alpha 비투명0.
- Wolf: 384×40 sheet의 6 frame과 개별 프레임 픽셀 동일; Icon index4는 다섯 번째 Run 셀과 픽셀 동일; 빈/중복 프레임0; 64×40 네 변 Alpha 비투명0.
- 오프라인 시트 무결성은 **새 Unity Import/실제 게임 화면/레이어/입력/Collider/Quest/Save/전투·펫 연출 검증을 대체하지 않습니다**.

## 4. 승인 게이트 및 Codex 후속

1. 바울 대표의 v2 **뱀 위협감/공격 5동작 + 늑대 비유아적 얼굴·6프레임·40px UI** 각각 미술 검토/승인. **반려 시 v3 국소 개선**, 기존 v1 재승격 금지.
2. 승인 후 Codex `codex/*` 독립 worktree에서 로컬 ZIP SHA 대조, 현재 main asset/animation meta/GUID audit, 승인 PNG만 **새 별도 경로 + 새 GUID**로 Imported 후보 작성.
3. VenomSnake Idle/Attack frame slicing와 Explicit Walk/Hit/Defeat 12, poison/AI/Spawn/Quest/Save, Wolf `Resources/CompanionAssaultOriginal/Wolf_Run`과 RunResourcePath/12fps/IconFrameIndex4/LEFT·2x 크기/Travelspeed570·`동료의 습격`, 세린/Fox와 PetFacility/MonsterPet/Save, 화면비16:9/4:3/21:9로 테스트.
4. 기술 결과·대표 실제 미술 화면 검토 뒤 *별도* 정식 Unity 적용/PR/main 병합 승인. 기존 v1·Chapter2 공식 몬스터11~18·제3자 소스·사운드·게임 밸런스·입력과 접근성 규칙 보호.

**Git:** 이번 문서 커밋은 `docs/art-renewal`에만. `main` 직접 Commit/Push/병합0.
