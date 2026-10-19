# LIMITLESS / Chapter2 Field06 「불탄 흔적의 길」 환경 리뉴얼 v1 제작 지시 — 2026-10-19

- 담당: Chapter1·2 리뉴얼 아트 (`docs/art-renewal`).
- 발주/기획: `docs/planning`.
- 대상 Unity Scene: `Unity/Client/Assets/_Project/Scenes/Field_06_ScorchedTrail.unity`.
- 상태: **ART_V1_PRODUCTION_AUTHORIZED / ART_NOT_YET_DELIVERED / USER_ART_APPROVAL_PENDING / UNITY_NOT_APPLIED**.
- 전제: Field04 공식 동서 Road42, Arbel v3 공식 환경23종, Field05 B42 공식 PNG14/375Renderer는 2026-10-19 `main` 기록상 정식 완료. 지난 인계문에서 Field05 미술검토 대기라고 쓴 문장은 옛 기록이다. 이를 반복 제작·재승인 요청하지 않는다.
- 근거: `문서/00_프로젝트/LIMITLESS_Chapter2_Original_Art_Roadmap_20261012.md`; `문서/03_스토리/Chapter2_Main15_타오르는_흔적.md`; `Unity/Client/Assets/_Project/Scripts/World/Chapter2Main15Flow.cs`; `문서/00_프로젝트/Field05_B42_Official_Apply_QA_20261019.md`; `CURRENT_STATUS.md`.

## 목표 — 서쪽으로 깊어지는 작은 화재 흔적, 아직 용암은 아님

Field05의 메마른 황토/올리브와 아르벨 보급 거점 톤에서 자연스럽게 서쪽으로 이어지는 **Field06 고유 오리지널 환경 아트 v1** 제작. 동쪽 입구는 여전히 마른 풀·작은 잔존 나무가 있고 중앙에는 황토 및 붉은 갈색 건조 흙길, 서쪽으로 갈수록 가뭄/검게 탄 돌/고사목/균열이 선명해진다. 다만 **용암, 폭발, 거대한 산불, 완전한 사막, 보스 거점 느낌은 금지**. 화면 전체를 붉은색으로 뒤덮지 말고 이후 Field07 재바람 황야/Field08 붉은 균열로 갈수록 강해질 연출 여지를 남긴다. 절대로 Field05 B42의 도로 서→동 진행 방향이 다시 수직 중심 길로만 바뀌지 않게, **주 진행 방향 동↔서**를 한눈에 읽히게 한다. 실제 Scene 좌표/길 형상은 읽기 전용 점검으로 확정할 것.

## 선행 읽기 전용 소스 감사

1. 원격 GitHub `main` 최신 / `docs/art-renewal` 최신 확인 후 현행 Scene Renderer(정적/Runtime), Sprite GUID+fileID, Importer PPU/Pivot, Transform/Scale/Rotation/Flip, Tint, Sorting, Ground/Path/Rock/Tree/Grass 계열, World Bounds/Camera, Collider, Spawn/Exit를 목록화한다. `Chapter2Main15Flow.BuildEnvironment()`가 Runtime에서 추가하는 `DrySoilEast`, `OchreGround`, `CrackedGround`, `CrackBranch`, `ScorchedRock`, `DryGrass` 패치와 static Scene 그림을 구분한다. 실제 렌더러 개수는 조사 전 임의 확정하지 않는다.
2. `Field_06_ScorchedTrail`은 Arbel 서쪽 연결의 왕복 필드. `Chapter2Main15Flow`에 현행 필드 입장 `field06_east_entry` (7,0), `field06_main15_soot` (5,-2), `field06_main15_tracks` (2,2), `field06_main15_pulse` (-1,-2), `field06_main15_crack` (-4,2), `field06_main15_heat` (-7,-2), `field06-serin` (-6,1)이 런타임으로 설치된다. **이 좌표를 아트에서 옮기거나 지우지 않는다.**
3. Main15 의뢰는 레온→서쪽 입구→그을음→짐승 이동흔적→그을음들개 지정전투→짧아진 지면 울림→균열→검은 바위→세린 보고→Arbel 귀환→세린 정식 합류 순서다. 4개 실제 encounter `Field06_M15_01~04`·기존 MonsterSpawn/Battle/Save·세린 스프라이트/음성/펫·아르벨 NPC/마차 등을 보호.
4. 사용자의 Windows F드라이브 Unity/ZIP·실제 카메라 실측을 담당 세션이 직접 확인했다고 주장하지 말 것. GitHub Scene YAML/코드로 가능한 읽기 전용 분석은 자체 수행. 추가 로컬 실측이 반드시 필요할 때만 **Codex용 독립 read-only 조사 프롬프트**를 따로 납품하되 현재 Main09 음성 버그 수정과 섞거나 Codex 작업을 선점하지 않는다. 처음부터 로컬을 요구하느라 아트 콘셉트/오프라인 준비를 멈추지 않는다.

## v1 납품물 — 실사용 가능한 역할별 PNG

- 실제 원본 Sprite/Renderer 대응을 검토한 다음 **Ground/Path/Scorched soil / Dry grass / Dry shrubs / Burned tree / Charred rock / Small cracks / Quest-safe scenery** 중 사용되는 역할만 개별 투명 PNG로 제작. 정확한 자산 개수·canvas·PPU/Pivot은 실물 매핑 후 제안. 매핑이 검증되지 않은 장식은 `OPTIONAL_NOT_PLACED`로 분리.
- 아트 연결은 동쪽 Field05의 황토·올리브→중앙 황갈색/마른 갈색→서쪽 어두운 열 흔적의 **3구역 점진적 색 전이**로 설계. 반복타일 seam/alpha, 접지/Foot, 지나친 반복 패턴, 큰 나무/바위가 플레이어·NPC·조사표식을 가리는 문제를 검사.
- 큰 균열/가공 오브젝트에 시각적으로 진입 금지라고 혼동할 정도의 장식이나 새로운 Collider를 부여하지 않는다. 특수 Quest 표시와 진행 방향이 지형 배경에 묻히지 않는 고대비를 확보한다. 도보/휠체어 경로·Exit/Spawn·오른쪽 귀환 구역·몬스터 이동 구역은 깨끗하게 남긴다.
- ZIP 파일명 예시 `Limitless_Field06_ScorchedTrail_Renewal_Art_v1.zip` (영문 파일명). 구성: 사용 실 PNG, `Manifest.json`(SHA256/Canvas/Alpha/권장 Import/역할/Target), `Renderer_Mapping.csv`(source Sprite GUID+fileID/Transform/target/STATIC-vs-RUNTIME 여부), `README.md`, `QA/Offline_Validation.json`, `Rights_and_Provenance.md`, `Preview`/Contact Sheet, Codex 통합 인계. 프리뷰가 모식/합성이라면 실제 Unity 캡처처럼 표시하지 않는다.
- Field05처럼 주 방향이 동서로 읽히는 **화면 전체 시안+중요 조사점 확대 시안**, v1 후보의 각 장면에서 고정 Quest Mark·세린·몬스터 스폰을 읽을 수 있는지 QA. 전후 비교는 실제 캡처/모식도를 명확히 구별한다.

## 승인·검증·Git 경계

- **이번 요청은 아트 제작/오프라인 검수 권한**, 최종 미술 승인/실제 Scene 교체/신규 기능 구현/Unity Editor 조작/main 병합 권한은 아니다. 먼저 완성 아트 ZIP과 화면 시안을 바울 대표에게 보고하고 승인 이후 Codex가 별도 `codex/*` 브랜치에서 Import/배치/실제 이동·퀘스트·전투·Continue를 검증한다.
- 완료됐던 Field04/Arbel/Field05, 기존 Main15 Quest·Dialogue/TTS·세린 공식 아트/레온/펫·Field07~11·Story, Save/UserData/Scene meta/GUID/ThirdParty와 원본 작업트리 보존. 특히 Codex는 **현재 Main09 대사/음성 버그 우선**이므로 Field06 Unity 통합을 동시 강행하지 않는다.
- 리뉴얼 담당은 `docs/art-renewal`에 v1 제작 내역/미검증·보류/산출물 SHA/기존 완료 자산 보호를 문서화하고 자신의 `WORLD_ART_RENEWAL_LATEST.md` 인계를 갱신한다. **main 직접 커밋·Push·병합 금지**. 작업 마감 보고는 Branch/Commit/ZIP/검증 경계/대표 미술 승인 필요 여부를 표기.
- 합격 판정은 `ART_DELIVERED / OFFLINE_QA_PASS / USER_ART_REVIEW_PENDING / UNITY_RUNTIME_NOT_VERIFIED`처럼 분리하고 정식 적용 완료로 과장하지 않는다.

## 이후 예정

Field06의 미술 방향과 개별 역할 PNG를 승인받은 뒤 다음 환경 제작은 `Field_07_AshenReach`이다. `Field_08_RedRift`는 기존 공식 아트/스토리 연출이 있으므로 소스부터 재확인하고 별도 보완이 필요한 경우에만 갱신한다. Chapter3부터의 신규 북부 아트는 `docs/art-new` 담당이 맡는다.
