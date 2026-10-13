> 2026-10-14 후속: 동일 원본 Sprite 복구 후 필수23 Scene 참조/Runtime 누락0 및 통합 회귀 통과로 Sprite 병합 게이트를 해소했습니다. 아래는 10월13일 당시 BLOCKED 기록입니다. 최신 결과는 [Sprite 복구 최종 QA](Sprite_Reference_Recovery_Final_QA_20261014.md)와 PR #10을 확인합니다.

# EOD 승인 작업 통합 및 최종 합본 QA — 2026-10-13

**최종 판정: BLOCKED / PR10_DRAFT_RETAINED / MAIN_NOT_MERGED.** 승인 기능 통합과 충돌 해결은 완료했으나 전체 Scene 참조 게이트가 FAIL입니다. 기존 main에서도 재현되는 아르벨 등 환경 Sprite 누락이며, 실제 합본 런타임에서 아르벨 배경이 비어 보임을 RenderTexture로 확인했습니다. 단순 신규 회귀로 오인하지 않되 전체게임 QA PASS로 승격하지 않습니다. 사용자 지시7에 따라 main 병합을 중단하고 안전한 통합 브랜치에 결과를 보존합니다.

## 기준 / 통합

- main `0e0476649b667129cb7aa6004f4b677335ed8be2`; PR10 출발 `6afe5a75e72f5b5c7253d0656c635be94d1dac00`.
- 브랜치 `codex/eod-integration-20261013`. Unity 합본 QA 코드 기준 `632343f005c9225d73df22eb3b4a70deb6ba6778`; 이후에는 보고서·상태·인계 및 공백 정리만 변경합니다. 최종 브랜치 SHA는 완료 보고와 PR10 head에서 확인.
- PR8 `ec469c0`를 merge 부모 보존 방식으로 병합. 기존 PR7 Field04 Legacy GUID 교정은 원래 main Scene과 Sprite 참조만 다르고 나머지 블록 동일함을 확인했습니다. 승인 v2 Scene의375 참조/42도로 결과로 해결하고 Legacy PNG7은 삭제하지 않았습니다.
- Field04 공식 Scene은 ec469c0 Git blob과 정규화 후 동일; PNG52 동일바이트. Main08 승인WAV6/메타/Catalog는79554069와 동일. 기존 A/B/C Runtime C# 전체는79554069와 동일. 기능 추가/미승인 스킬/Chapter3 상점/Save 규격 변경0.
- PR9 최신 docs/planning22e1344 병합. Main08 add/add 충돌의 양측 문단 전부 보존 검증. CURRENT_STATUS·CODEX_LATEST의 A/B/C/Importer/Main08/Field04 이력 유지.
- 리뉴얼/TTS 담당 문서5개만 선별 추가·갱신. 신규 Main18 WAV8/DSP Preview6/실행 스크립트·샘플 자산은 게임에 추가하지 않음. 최신TTS 기록은8개 모두 USER_LISTENED, 샘플1음색승인/heat_vent1불일치/6개음색변동·최종KEEP/REGEN미정. Story13 UNKNOWN/HOLD, Story14 로컬미호출 유지.

## 이번 새 합본 실행

| 검사 | PASS | FAIL | 판정 |
|---|---:|---:|---|
| Field04 | 253 | 0 | PASS (기존 예외 별도) |
| Main08 | 177 | 0 | PASS (기존 예외 별도) |
| Skill | 72 | 0 | PASS (기존 예외 별도) |
| PetService | 106 | 0 | PASS (기존 예외 별도) |
| Modal | 28 | 0 | PASS (기존 예외 별도) |
| PetContract | 64 | 0 | PASS (기존 예외 별도) |
| 기존15스킬 세부검사 | 154 | 0 | PASS |

주요 기능검사 총854 PASS/기능FAIL0, 정적검사83 PASS. **별도 전체Scene 참조 검사는 FAIL**이므로 이 수치를 전체게임 PASS로 해석하지 않습니다.

- Field04: Compile/환경375/Sprite0누락/도로42/고유PNG52/PPU128/Collider보존, Main12~13 조사·세린 임시합류·Story전투, 실제물리Trigger6방향 왕복, 휠체어147waypoint, 일반전투2종/35초리스폰, 실제Save/Continue, Camera16:9/4:3/21:9 PASS.
- Main08: Trace01~03의 자막·화자·Portrait·Clip/GUID/승인PCM6·빠른Next·Mixer·퀘스트·Save/Continue PASS. 초기 QA1회는 승인PCM/GUID TSV를 전달하지 않은 테스트 준비 누락으로 중단됐으며, 최초실패기록 보존 후 기존 승인fixture를 보완한 새 실행177PASS. 게임 코드 수정0.
- 스킬: 2×3/최대6/기본5명령/별도Back/빈슬롯미노출/전투외관리/전투중교체차단/다른슬롯Reset·36행보관함레이아웃·화면5비율 PASS. 기존15효과/MP/CD/대상취소154PASS. 장착영속저장 미구현 그대로.
- 펫: Main21전후3거점 조건/기존아르벨/중복0/사수/기본3·몬스터9/100탈렌트/구매·장착·Save/Continue/다른슬롯/실패롤백 PASS. Chapter2몬스터4효과64개계약검사 PASS. 스킬·펫Modal 상호점유/닫기/이동잠금해제28PASS. Chapter3상점은 설계만 유지.

## 전체 참조 FAIL — main 기준본 비교

23개 활성 BuildScene에서 MissingScript0, GUID본체는Assets/UPM에서 모두확인. 그러나 아래 SpriteRenderer의 실제Sprite null이 검출됐습니다. 이 표는 **Edit Mode 원시 null 개수**이며 동적NPC는 초기화 후 구분합니다.

| Scene | 기존main null | EOD null |
|---|---:|---:|
| World_StarterVillage | 1 | 1 |
| Field_04_WesternBorder | 362 | 0 |
| Arbel | 285 | 285 |
| Field_05_WesternOutskirts | 362 | 362 |
| Field_06_ScorchedTrail | 362 | 362 |
| Field_07_AshenReach | 371 | 371 |
| Field_08_RedRift | 368 | 368 |

- 총2111→1749로 Field04362개만 복구됨. 나머지Scene YAML과 ThirdParty 파일은 main 대비 변경0. Scene별 누락 증가0, **신규 참조 회귀 증거0**. 동일6000.6.5f1의 별도 main 기준본에서 전체검사를 다시 실행하여 기존문제로 분류했습니다.
- StarterVillage의VillageNpc1은 실제Runtime 초기화 후0으로 확인되어 동적할당 항목입니다. 이를 실제게임 누락1로 단정하지 않습니다.
- 기존 main의 실제 Runtime 추가비교에서도 Arbel284/Field05 362가 동일 재현됐습니다. 해당 배경누락은 신규회귀가 아니라 원격기준본의 기존문제입니다.
- 실제 합본Runtime: Arbel284, Field05 362, Field06 313, Field07 371, Field08 8개 null검출. 일부Scene은런타임재구성/동적Sprite가 있으므로 Edit null수와구분. Arbel/Field05는 배경누락 RenderTexture 증거 확인. 원본사용자Editor가같이깨졌다고단정하지 않음.
- 원인범위: GUID파일은존재하나 Scene Sprite subasset참조/Importer 조합의 문제로 좁힘. 정확한기존slice/PPU/피벗/사용자수동meta와Importer버전 대응은 후속복구검토 필요. ThirdParty원본과 사용자미커밋meta를 임의 교체하지 않았습니다.
- EOD정본의 “Missing Sprite/Script/Reference0” 게이트 미충족. 기존문제임을 기록해도 실제 배경누락을 PASS로 승격하지 않고 main병합 보류.

## 보호 / 기존 PARTIAL·OPEN

- 원본 프로젝트 보호파일5199개SHA256, HEAD ff022e63dfe364f559fcc29819d27b123ad31127, 사용자dirty status 불변. 원본Scene/Main08승인WAV6/Settings/PNG/TTS/BGM/문서 디스크 변경0. 원본Unity MCP/Play/Scene/포커스 조작0.
- 모든 실제Save/Settings 테스트 경로는새Temp QA에강제지정. 외부 사용자Save 폴더 전체의 별도사전hash는이번snapshot에포함되지 않아 별도SHA전수증명은NOT_VERIFIED; 원본Save 접근·수정 코드실행0, 격리경로검사PASS.
- Canonical Unity6000.5.7f1, 실제6000.6.5f1. 격리캐시와패키지호환갱신은테스트복사본한정. 브랜치Packages/ProjectSettings변경0, 정본버전직접실행NOT_VERIFIED.
- 기존UnityEditor.Search 예외 PARTIAL, Field04서쪽Spawn/TreeWest겹침·북쪽Collider틈 OPEN, Main08태온005/006발화·대화중일반조우 OPEN, 스킬영속Save/신규상품/Chapter3클리어·상점 계약미구현 보호.
- 기존전투투사체GUID중복5쌍은격리Unity자동보정했으나그meta를납품하지않음. 새승인아트/음성/Legacy등록GUID충돌0. 실제사람입력감·손플레이NOT_VERIFIED. TTS/API/재시도0, 미승인WAV/Preview적용0.

## 안전한 후속

1. PR10은Draft유지, main반영0. 이브랜치에통합코드/승인자산/QA자료만일반Push하여보존. main직접Push/force/reset/clean/원본pull·stash·rebase0.
2. 아르벨·Field05~08의 기존정상PNG와 원본사용자수동Sprite meta를 읽기전용으로 대조하고 `_Project` 동일바이트등록/Scene Sprite참조만의 복구안을 별도Codex브랜치에제안합니다. ThirdParty직접수정·Scene재생성·NPC/Collider/Quest배치변경 금지. 미승인미술교체로해결하지않음.
3. 복구안이승인범위와동일비주얼을보존함을확인한뒤 전체Scene·실제Runtime배경/기존854검사영향범위QA를통과하면 PR10 Ready→사용자승인조건에따른main Merge를재개합니다.
