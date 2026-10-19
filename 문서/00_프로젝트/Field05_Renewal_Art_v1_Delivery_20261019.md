# LIMITLESS / Chapter2 Field05 서부 외곽 환경 리뉴얼 아트 v1 — 제작·납품 (2026-10-19)

**상태:** `ART_DELIVERED / OFFLINE_PNG_QA_PASS / USER_ART_REVIEW_PENDING / UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`
**기준:** 원격 `main` `c263d9e36fc0570b16130d9c011abf3965def5f7` 및 [기획 발주서](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/Chapter2_Field05_Renewal_Art_Assignment_20261019.md).
**작업 브랜치:** `docs/art-renewal` (문서만 커밋, main 직접 변경·Push·병합 없음).

## 1. 기존 Scene 정본 감사

- 실제 `Unity/Client/Assets/_Project/Scenes/Field_05_WesternOutskirts.unity` YAML 조회: **환경 직렬화 SpriteRenderer 363개**, GameObject 374개. 환경 Renderer는 Grass315(기존3개 sprite local fileID 각각104·105·106참조), RoadLeft15 + RoadRight15(총30), Props18(울타리8/나무5/수풀3/바위2).
- Ground는 21×15의 x=-10~10, y=-7~7. 기존 Road30은 중앙에서 남북으로 굴곡진 계단형 동선. **v1은 길 위치·Road GameObject명·Collider·Scale을 바꾸지 않고 기존 위치에서 쓰는 Sprite-only 후보**.
- 근거 Sprite 참조: Ground/Road GUID `3ffe8024a48343dcbe3ee849067c02d3`의 local IDs `1606921373`(104), `405149151`(105), `1271049111`(106), `278596618`(15), `1662121770`(15). Fence `bfc29083d3224729a15d7658d6f6fbf1/1892116053`(8). 나머지 소품은 개별 `SpriteReferenceRecovery20261014` Single refs 21300000 외 기존 Legacy RockNorth Multiple ref (85×54, PPU100).
- 기존 Field05 theme `groundTint(0.65,0.52,0.33)`, `cameraColor(0.17,0.12,0.07)`. Scene 자동 Runtime tint와 합쳐 실제 화면은 이 합성보다 더 황토색일 수 있음.
- 실제 보호: Main14 마른 수로/관개 조사 **(5,2)**, 낡은 감시초소 조사 **(-2,2)**, 강한 지면 진동 **(-6,-2)**, 공식 세린 **(1,0)**. SootHound **(3,-3)**, FissureLizard **(-4,3)** 활동반경1/리스폰35초. Arbel 출입구 Spawn **(8,0)**, Exit/Trigger **(10.7,0)**.
- 현재 Field05 Scene에 신규 수로 메시·감시초소 건물 Renderer가 존재한다고 가정하지 않음. 아래 수로/초소 전용 후보는 **새 SpriteRenderer 필요, 승인 전 0배치**. 임의 포털·서쪽 출구 신설 금지.

## 2. 실제 제작 납품

- 파일명: **`Limitless_Field05_Renewal_Art_v1.zip`**, 이 대화에서 실제 생성한 다운로드 파일. 바울 대표 PC F드라이브에 직접 저장했다고 주장하지 않음.
- 크기: **3,275,850 bytes**. ZIP SHA256: **`a6646a676a619bc149798fe0f3909401b9c3b5a75f253cf6a1e61e21661b1258`**.
- **총 35파일, PNG26개**. PNG 중 **새 실사용 후보 19개** = 기존 환경 Sprite 교체역할 **12종**(Ground3 / Road2 / Props7) + **신규 배치 선택 7종**(마른 수로·낡은 초소·지면 진동·마커·갈대·갈라진 땅·다리 잔해). 나머지 PNG7개 = 실제배치/옵션합성/표시/ContactSheet/비교/타일 SeamQA 이미지.
- 기존 Renderer 363개의 `name/scene-x-y/original GUID/ fileID/ Sorting/new target path/PPU/Center pivot 제안`을 `Renderer_Mapping_363.csv` **363행**에 기록(Grass315/Road30/Props18; 유일한 대상명363/363). 기존 Scene YAML의 개별 GameObject fileID를 CSV에 추가했다고 주장하지 않음; 적용 전 Codex 재대조 필수.
- `Manifest.json`: 모든 실제 PNG19의 SHA256·Canvas·PPU/Center·Alpha BBox·kind(기존 교체/선택 추가)를 기록.
- `README.md`, `Rights_and_Provenance.md`, `Codex_Integration_Handoff.md`, **`Codex_ReadOnly_Source_Audit_Prompt.md`**, `Optional_Story_Placement_7.csv`, `QA/Alpha_Grounding_Audit.csv`, `QA/Offline_Validation_v1.json`, ContactSheet·Scene 비교 합성·장소표시 Preview를 포함.
- 원본 Field04 v2와 아르벨 v3는 팔레트/분위기 참고만 했고, 승인 PNG를 복사하거나 재제작하여 ZIP에 포함하지 않음. 신규 소품은 별도 절차로 만든 **오리지널 procedural pixel illustrations**. 이미지 제작 출처 선언은 독립된 상용권리 감사와 구분.

## 3. 오프라인 QA 결과

- ZIP CRC `testzip()==None`, 이미지 **26/26 PNG decode**, `Manifest`에 수록된 실제 원본 PNG **19/19 SHA 동일**, Renderer 대응 CSV **363행·중복이름0·원본 Grass 각 104/105/106참조 보존**.
- Ground 3종의 모든 짝 1px RGBA 경계 비교 **좌우0·상하0 mismatch**, Road 2종도 **좌우0·상하0 mismatch**. 반복 최외곽 경계 검사이며 Camera 또는 Scene 최종 결합부 전범위 가독성 확인은 아님.
- Ground/Road Sprite PNG 128×128 / PPU128 Center 제안. Props BigTree255×256 / Medium128×156 / RockNorth85×54·PPU100을 원본 회복 정보에 맞춘 후보로 냄. **실제 원본 Sprite Pixel foot와 Inspector Pivot·Physics collider 접지 검증은 NOT_VERIFIED**.
- 비교 `Preview/LL_C2_F05_BeforeAfter_Schematic_v1.png`의 왼쪽은 **기존 Source 역할·좌표에 따른 도식**이지 Unity 원본 screenshot이 아님. 오른쪽은 실제 제작 PNG의 *오프라인 합성*. 전체 Preview는 **실제 Unity RenderTexture가 아님**.
- 실제 Unity Import, Editor/Play/Runtime, Quest14, 세린·Monster/출구 왕복, Save/Continue, 도보·휠체어 이동, 35초 리스폰, 카메라 가독성은 별도 Codex 격리검증과 사용자 미술 승인 **이전이므로 아직 검증하지 않음**.

## 4. 이번 승인 범위 및 잔여 HOLD

**사용자 미술 검토 필요**: 기존 역할 교체 후보 PNG 12종의 스타일 승인; 별도 선택 소품 7종의 개별 채택/배치 승인. **신규 선택 소품 7종은 기존 Scene에 없으며, 이 ZIP을 Unity에 자동 적용하거나 Trigger/Collider를 만들면 안 됩니다.** 대형 초소·수로·지면 균열 시안은 Main14 위치 시각 설명용으로만 표기. 원본 Official Arbel/Field04·동료·몬스터·펫·Portal·Spawn·Collider·Save·Quest·BGM/TTS·Field03 Rock B(미술 승인 별도)는 모두 변경0.

1. 바울 대표가 PNG ContactSheet와 실제 Scene좌표 기반 Preview를 검토하여 아트 승인/수정 요청.
2. Codex 읽기전용 로컬 감사: 실제 Scene/Prefab Renderer id·PPU/Pivot·Source Sprite Rect/Foot·물리 접근성 충돌 여부 대조. `Codex_ReadOnly_Source_Audit_Prompt.md` 참조.
3. 승인 후에만 Codex 별도 feature worktree에서 새 GUID/Importer·Scene sprite 참조 최소 교체, 선택 장식은 개별 승인분만 배치. Quest/Monster/SceneExit/Save/Camera/휠체어 실제 회귀 및 Runtime RenderTexture 별도 보고.
4. **main 직접 커밋/Push/병합 금지**, 이 담당은 문서만 `docs/art-renewal`에 기록. Field04/Arbel 기 승인 결과는 최신 main 우선.
