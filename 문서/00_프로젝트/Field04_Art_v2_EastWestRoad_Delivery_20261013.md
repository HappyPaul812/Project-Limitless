> EOD 기록 보존: 본 문서는 과거 제작·기획 이력을 포함합니다. 현재 승인/통합 상태는 `EOD_Integration_LocalSync_Closeout_20261019.md` 및 CURRENT_STATUS의 최상단을 따릅니다. 아래의 이전 계획/API 지시는 신규 실행 승인이 아닙니다.

# Field04 동서 진행로 아트 v2 납품 — 2026-10-13

**담당:** Chapter 1·2 리뉴얼 아트 / **브랜치:** `docs/art-renewal`

**상태:** `ART_DELIVERED / USER_ART_REVIEW_PENDING / UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`.

이 문서는 실제 다운로드 ZIP의 납품 기록이며 Unity 통합 완료를 뜻하지 않습니다.

## 확인한 원본과 설계 이유

- 2026-10-13 [Field04 v1 실제 검수](Field04_Art_v1_Inspection_20261013.md)에서 363환경 Renderer = Grass315 + Road30 + Props18 확인.

- 실제 진행은 **동쪽 Field03(Spawn8,0 / Exit10.7,0) → Field04 → 서쪽 Arbel(Spawn-8,0 / Exit-10.7,0)**. 과거 v1의 RoadLeft/Right 각 15행은 남북 진행로처럼 보였음.

- 사용자 지시대로 기존 Scene 전체 회전 및 길 PNG 일괄 90도 회전 없이 새 **동서 도로 128×128 RGBA 30장**을 제작. 기존 30 Road 각 GameObjectFileID에 일대일 대응하며, 새 제안 좌표는 **x=-7~7의 15열 × y=+0.5/-0.5의 2행**. 실제 Transform/Renderer 교체는 사용자 승인 후 Codex 전담.

- 30개만으로는 21열 월드의 양 출구까지 시각적으로 모두 닿지 않아 **연장용 12개 별도 PNG**(x=-10,-9,-8,+8,+9,+10 / 상하 2행)를 `OPTIONAL_NEW_RENDERER`로 포함. 연장용 추가 배치는 자동 허가 아님. 서쪽 TreeWest/Spawn(-8,0) 기존 콜라이더 겹침을 별도 기술 확인하기 전까지 연결 전체 통과를 주장하지 않음.

## v1 검수 보완

1. Grass: 원본 `1606921373@ecafa1446d0264141bb7b01e4005beff`의 실제 명칭은 `tiles_grass_6_0`이며 v1의 `tiles_grass_3_0` 라벨을 정정. 104개 참조 보존.

2. Tree: 원본 `21300000@5b888cd38e2bf8543a27554e427d128b`는 실제 Medium 3개 → 128×156 Medium PNG; `21300000@d06207b6940ba73478971dde7147ae92`는 실제 Big 2개 → 255×256 Big PNG로 이름/크기 정합 수정.

3. Field04 RockNorth: 원본 85×54 Slice/PPU100 대비 v1 크기·접지 문제가 있어 `LL_C2_F04_RockNorth_Grounded_128_v2.png` 재제작. 새 alpha bbox (11,31)-(117,98) / PPU128 Center 제안. 기존 Collider·Scale·좌표 변경 금지. **Field03 Rock B v2와 완전히 별개 자산**이고 새 RockNorth 실제 Runtime 접지 검사는 아직 미실시.

## 실제 ZIP 납품

- 파일명: `Limitless_Field04_Renewal_Art_v2.zip`. 채팅 다운로드 제공, 사용자가 `F:\Downloads\`에 보관하는 방식. 이 세션이 사용자 PC F드라이브에 직접 저장했다고 주장하지 않음.

- 크기: **2,741,669 bytes**, SHA256: `178ad6b235c65848cce48f4e2cfe86ff77c36857a12663237f7d7afa561f969d`.

- **68파일 / PNG60개**: Road30, 추가 Endpoint12, Ground3(기존동일), Props7(기존6+수정 RockNorth1), Optional Story3(기존동일), 합성 Preview5, 기타 QA·CSV·문서.

- 기존 v1 ZIP SHA256 `60f4c51d5dba1ad3bae862db033d36ccaae2cf8dac5b34b65762667874441be3`, 변경 없이 별도 보존.

- `Manifest.json`, `RoadPlacement_30.csv`, `Optional_EndpointPlacement_12.csv`, `SpriteMapping_Corrected_v2.csv`, `Codex_Integration_Instructions.md`, `QA/Validation_v2.json`, 비교 Preview 및 실제 개별 PNG 포함.

- ZIP CRC `testzip()==None` / 30 Road + 12 Optional 수량 / 매핑 333기존 비-Road 참조 + 30 Road = 363 정적 일치. PNG 크기/해시/알파는 오프라인 자체 검사.

- Preview는 제작 환경에서 합성한 **SCHEMATIC COMPOSITE**, **실제 Unity RenderTexture가 아님**. 기존 GroundTint(0.69,0.61,0.42) 곱셈을 비교 그림에 모사함. 사용자 미술 검토·Unity 실제 Import 후 확인 필요.

## 보존·남은 리스크·인계

- 이번 납품에서는 원본 Scene·Renderer Transform·기존 meta/GUID·Collider·NPC·공식 세린·몬스터·Main12~13 Quest/Marker·Save·TTS/BGM·ThirdParty 자산 수정 **0**.

- 보호 지점: (7,0) Main12 도착, (5,-2) 토양, (2,2) 낮아진 물길, (-1,0) 세린, (-3,0) 공동전투. 휠체어 반경0.5 및 동서 연결부 접근성/가독성은 **v2 정식 회귀 미검증**.

- v1 기존 QA의 서쪽 Spawn과 TreeWest 콜라이더 겹침·북쪽 non-exit 경계 틈·Search 예외는 해결됐다고 주장하지 않는다.

- **다음 순서:** (1) 사용자 v2 비교 이미지 및 연장 12장 별도 미술 승인 여부 결정 (2) 승인된 범위만 Codex 격리 배치·Sprite Import/새 GUID 및 RockNorth 확대접지 QA (3) Main12~13/동서왕복/휠체어/Continue/Camera/35초 리스폰 실제 회귀 (4) 사용자 실제 화면 최종 승인.

- **Git 정책:** 이 문서는 `docs/art-renewal`에만 기록, `main` 무수정. Unity Scene이나 ZIP을 Git에 등록한 것이 아님. main 병합은 사용자 승인 후 별도 처리.

## 2026-10-19 담당 인계 추가 기록 (origin/docs/art-renewal)

**담당:** Chapter 1·2 리뉴얼 아트 / **브랜치:** `docs/art-renewal`
**상태:** `ART_DELIVERED / USER_ART_REVIEW_PENDING / UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`.
이 문서는 실제 다운로드 ZIP의 납품 기록이며 Unity 통합 완료를 뜻하지 않습니다.

- 2026-10-13 [Field04 v1 실제 검수](Field04_Art_v1_Inspection_20261013.md)에서 363환경 Renderer = Grass315 + Road30 + Props18 확인.
- 실제 진행은 **동쪽 Field03(Spawn8,0 / Exit10.7,0) → Field04 → 서쪽 Arbel(Spawn-8,0 / Exit-10.7,0)**. 과거 v1의 RoadLeft/Right 각 15행은 남북 진행로처럼 보였음.
- 사용자 지시대로 기존 Scene 전체 회전 및 길 PNG 일괄 90도 회전 없이 새 **동서 도로 128×128 RGBA 30장**을 제작. 기존 30 Road 각 GameObjectFileID에 일대일 대응하며, 새 제안 좌표는 **x=-7~7의 15열 × y=+0.5/-0.5의 2행**. 실제 Transform/Renderer 교체는 사용자 승인 후 Codex 전담.
- 30개만으로는 21열 월드의 양 출구까지 시각적으로 모두 닿지 않아 **연장용 12개 별도 PNG**(x=-10,-9,-8,+8,+9,+10 / 상하 2행)를 `OPTIONAL_NEW_RENDERER`로 포함. 연장용 추가 배치는 자동 허가 아님. 서쪽 TreeWest/Spawn(-8,0) 기존 콜라이더 겹침을 별도 기술 확인하기 전까지 연결 전체 통과를 주장하지 않음.

1. Grass: 원본 `1606921373@ecafa1446d0264141bb7b01e4005beff`의 실제 명칭은 `tiles_grass_6_0`이며 v1의 `tiles_grass_3_0` 라벨을 정정. 104개 참조 보존.
2. Tree: 원본 `21300000@5b888cd38e2bf8543a27554e427d128b`는 실제 Medium 3개 → 128×156 Medium PNG; `21300000@d06207b6940ba73478971dde7147ae92`는 실제 Big 2개 → 255×256 Big PNG로 이름/크기 정합 수정.
3. Field04 RockNorth: 원본 85×54 Slice/PPU100 대비 v1 크기·접지 문제가 있어 `LL_C2_F04_RockNorth_Grounded_128_v2.png` 재제작. 새 alpha bbox (11,31)-(117,98) / PPU128 Center 제안. 기존 Collider·Scale·좌표 변경 금지. **Field03 Rock B v2와 완전히 별개 자산**이고 새 RockNorth 실제 Runtime 접지 검사는 아직 미실시.

- 파일명: `Limitless_Field04_Renewal_Art_v2.zip`. 채팅 다운로드 제공, 사용자가 `F:\Downloads\`에 보관하는 방식. 이 세션이 사용자 PC F드라이브에 직접 저장했다고 주장하지 않음.
- 크기: **2,741,669 bytes**, SHA256: `178ad6b235c65848cce48f4e2cfe86ff77c36857a12663237f7d7afa561f969d`.
- **68파일 / PNG60개**: Road30, 추가 Endpoint12, Ground3(기존동일), Props7(기존6+수정 RockNorth1), Optional Story3(기존동일), 합성 Preview5, 기타 QA·CSV·문서.
- 기존 v1 ZIP SHA256 `60f4c51d5dba1ad3bae862db033d36ccaae2cf8dac5b34b65762667874441be3`, 변경 없이 별도 보존.
- `Manifest.json`, `RoadPlacement_30.csv`, `Optional_EndpointPlacement_12.csv`, `SpriteMapping_Corrected_v2.csv`, `Codex_Integration_Instructions.md`, `QA/Validation_v2.json`, 비교 Preview 및 실제 개별 PNG 포함.
- ZIP CRC `testzip()==None` / 30 Road + 12 Optional 수량 / 매핑 333기존 비-Road 참조 + 30 Road = 363 정적 일치. PNG 크기/해시/알파는 오프라인 자체 검사.
- Preview는 제작 환경에서 합성한 **SCHEMATIC COMPOSITE**, **실제 Unity RenderTexture가 아님**. 기존 GroundTint(0.69,0.61,0.42) 곱셈을 비교 그림에 모사함. 사용자 미술 검토·Unity 실제 Import 후 확인 필요.

- 이번 납품에서는 원본 Scene·Renderer Transform·기존 meta/GUID·Collider·NPC·공식 세린·몬스터·Main12~13 Quest/Marker·Save·TTS/BGM·ThirdParty 자산 수정 **0**.
- 보호 지점: (7,0) Main12 도착, (5,-2) 토양, (2,2) 낮아진 물길, (-1,0) 세린, (-3,0) 공동전투. 휠체어 반경0.5 및 동서 연결부 접근성/가독성은 **v2 정식 회귀 미검증**.
- v1 기존 QA의 서쪽 Spawn과 TreeWest 콜라이더 겹침·북쪽 non-exit 경계 틈·Search 예외는 해결됐다고 주장하지 않는다.
- **다음 순서:** (1) 사용자 v2 비교 이미지 및 연장 12장 별도 미술 승인 여부 결정 (2) 승인된 범위만 Codex 격리 배치·Sprite Import/새 GUID 및 RockNorth 확대접지 QA (3) Main12~13/동서왕복/휠체어/Continue/Camera/35초 리스폰 실제 회귀 (4) 사용자 실제 화면 최종 승인.
- **Git 정책:** 이 문서는 `docs/art-renewal`에만 기록, `main` 무수정. Unity Scene이나 ZIP을 Git에 등록한 것이 아님. main 병합은 사용자 승인 후 별도 처리.
