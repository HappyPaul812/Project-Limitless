# LIMITLESS / Chapter1 기존 몬스터 4종 제작 출처 기록 — 대표 진술·라이브러리 설계도 대조 (2026-11-10)

- 발단: [Issue #57 Codex 7몬스터+3기본펫 읽기 전용 감사](https://github.com/HappyPaul812/Project-Limitless/issues/57#issuecomment-6764431147). 실제 Unity PNG·GUID/fileID·Spawn·SHA 검사 결과 Chapter1 잔여 7종 중 3종 ORIGINAL_CONFIRMED, 4종 UNKNOWN, 기본 펫 Wolf/Bear/Fox 3종 ORIGINAL_CONFIRMED. 현재 실사용 THIRD_PARTY_ACTIVE로 **확정된 종은 0종**. 이는 UNKNOWN 4종의 독자 제작/법적 권리 전체 보증이 아니다.
- **사용자 2026-11-10 추가 직접 진술**: “UNKNOWN들 우리가 만들건데, 문서화가 안됐구나? UNKNOWN들 니 라이브리리에도 있으니까 시간 날 때 찾아봐.. 몬스터들 펫들 중에는 남의 애셋은 이제 없는거지?” 문맥상 기존 4종을 팀에서 만든 자산으로 지목한 출처 보강 요청. 작성자 의도(과거 직접 제작/협업 제작)는 **USER_STATED_TEAM_ART**로 기록하되, 최초 생성 파일과 현재 Unity PNG 사이의 SHA/수정과정 역추적 전에는 Codex 기술 감사의 **UNKNOWN → ORIGINAL_CONFIRMED 자동 승격 금지**. **신규 제작 필요 확정0, 기존 공식 PNG 보호**.
- **라이브러리에서 추가 확인**: “던전 B2 침묵의 파수꾼 설계도.png” (Dungeon_01 B2 설계도(안)) 이미지에는 *침묵의 잔영*, *봉인 수호체*, *침묵의 파수꾼* 3종의 콘셉트·전투 128x128 프레임 예시·스펙이 함께 포함됨. 파일은 기존 팀 설계·제작 과정의 **시각적 정황**으로 분류하며, 현행 Unity 3개 스프라이트시트의 **원본 SHA 동일성/출처 선언 증거 자체는 아님**. `Codex_Dungeon01_B2_Implementation_Prompt.txt`에도 세 종 관련 B2 구현 기획 기록 존재(원화 직접 저작 증명 아님). `grass_slime` 최초 독립 디자인 원본 파일 직접 일치 자료는 이번 라이브러리 검색에서 아직 확인 못함.
- 감사 최신 정본의 나머지 3종 `monster_moss_beetle`, `monster_shade_bat`, `monster_grave_wight`는 [문서/08_몬스터/README.md](https://github.com/HappyPaul812/Project-Limitless/blob/main/문서/08_몬스터/README.md)의 **사용자 직접 제작·수정/내부 공식** 명시 및 현행 SHA 일치로 ORIGINAL_CONFIRMED. Wolf/Bear/Fox는 기존 공식 Run6 v5/v4/v7과 Unity 실사용 SHA 일치 확인. 독침벌·숲거미·맹독뱀은 이후 공식 교체·Local Unity 적용 실적 존재(관련 #63/#90/#93/#97). Chapter2 11~18은 정식 보호자산(기존 #57 범위)이며 전체 외부 자산 저작권 법률 검증과 별개.

## 감사상 UNKNOWN4 상세 기록

| ID | 현재 Unity 실사용 PNG | 현재 파일 SHA256 (Codex 감사) | 출처 상태 |
|---|---|---|---|
| `grass_slime` | `01_GrassSlime_Walk.png` | `1c422147f0a8fe7ee234e0ab771ab098ac32c44338d5efecc4f3f08a3174de34` | 사용자 팀 제작 진술 / 생성본 1254→1256 padding 기록 / 최초 생성 경로·SHA 대응 미확인 |
| `monster_silent_echo` | `MonsterSheets/Silent_Echo_128x128_Sprite_Sheet.png` | `1097c2268baca8b51435edb845dcc2f459aee601a799c47c34e17c87a711ddb4` | 사용자 팀 제작 진술 / 라이브러리 B2 설계도 시각정황 / 직접 sprite sheet 원본·SHA 계보 미확인 |
| `monster_seal_guardian` | `MonsterSheets/Seal_Guardian_128x128_Sprite_Sheet.png` | `8ec559c56d2d814e5acb3dda45c928f1ad06302f7754b43fb5bc96e95e9b02fc` | 사용자 팀 제작 진술 / 라이브러리 B2 설계도 시각정황 / 직접 sprite sheet 원본·SHA 계보 미확인 |
| `monster_silent_warden` | `MonsterSheets/Silent_Warden_128x128_Sprite_Sheet.png` | `97ef560e9258cbc17345ae51c017e9e113c1d840515e205698214f444a53e656` | 사용자 팀 제작 진술 / 라이브러리 B2 설계도 시각정황 / 직접 sprite sheet 원본·SHA 계보 미확인 |

## 실행 정책

1. **몬스터·펫 외부 실사용 확정 목록: 없음** (`THIRD_PARTY_ACTIVE_CONFIRMED=0` in scoped audit). 외부 사용이 **전혀 없다**는 완전 출처 증명과 구별.
2. 위 4개 기존 PNG는 **교체·재생성 대상이 아님**. `OWNER_TEAM_CREATED_ASSERTED / ORIGINAL_SHA_LINEAGE_PENDING / CURRENT_GAME_ART_KEEP`. 신규 그림 생성보다 원본 아트 생성 채팅·라이브러리 파일·편집/시트 출력 과정을 추적하여 최종 SHA 계보를 증빙할 것.
3. 향후 Codex는 **읽기 전용**으로 원본 파일의 최초 작성·편집 경로, 원화 ZIP/개별PNG, SHA, 원본→최종시트 변환/수정기록을 대조한 뒤에만 4개 상태를 승격할 것. 마우스/유니티/Save/UserData와 실제 원본 편집 불필요. 라이브러리 설계도 그림만으로 원본 바이트 동일 주장 금지.
4. `ThirdParty` 폴더에 **보관만 된** 파일과 실제 런타임 참조는 구분. 기존 다른 비오리지널 자산(NPC/상점 아이콘/VFX)은 이번 몬스터·펫 범위 밖이므로 게임 전체 자산 100% 오리지널이라고 선언 금지.
5. 이 문서는 `docs/planning`에서 제작 출처 기록을 보강하는 **문서 전용**. Unity Scene/Prefab/MonsterDefinition/아트·GUID/meta/Save/UserData/원본 로컬 main/원격 main 변경 없음.

**현재 판정:** `THIRD_PARTY_ACTIVE_CONFIRMED_0 / CH1_ORIGINAL3_UNKNOWN4 / PET3_ORIGINAL_CONFIRMED / USER_TEAM_ORIGIN_DECLARATION_CAPTURED / 4_PROVENANCE_LINKS_PENDING / REMAKE_NOT_NEEDED`.
