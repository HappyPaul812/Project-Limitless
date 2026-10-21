# LIMITLESS — 2026-10-21 맹독뱀·Wolf v3 및 NorthF1 3샘플 Codex 검수판정 / 생산 게이트

- 기획: `docs/planning`. 사용자 Codex 보고 이미지(2026-10-21, `codex/art-review-monsterwolf-f1-20261021`, 최종 검수 commit `5df5f74d7e9070544bb88aa92fbfe991972498cf`) 기준. 이 기획 세션에서 Windows 원본을 직접 Unity에서 실행하거나 별도 GameView를 검증하지 않았음.
- 이번 비교 자료는 **전부 오프라인 합성 시각 자료**, Unity GameView **아님**. 파일/PNG 기술 검사 ZIP SHA256·CRC·PNG decode PASS, 원본 Snake/Wolf PNG2 SHA2/2 동일, F1 기존6 SHA6/6 보존, Ground 두 후보 반복 경계 기술 PASS. **맹독뱀 SHA 누락 및 F1 QA JSON 내부 해시 충돌/정정 필요** 보고를 문서·Manifest QA 항목으로 별개 관리. 포맷 PASS를 실사용 미술/게임등급 PASS로 바꾸지 않음.
- 사용자가 상세 자산 우선순위표 전부를 매번 보고받는 것을 원치 않으므로 이 문서를 팀 내부 자산 추적 정본으로 사용, 사용자에게는 최종 승인 필요한 시각 증거만 간단히 제시.

## A. Chapter1·2 맹독뱀/펫 Wolf v3

- ZIP: `F:\Downloads\Limitless_MonsterWolf_KeyArt_v3_Review_20261021.zip`, 문서상 기존 납품 ZIP SHA `a5189bdb542988da69581135f137b39c98b3c5434272cb5020d13310a98fdae8`. 내용: 이전 승인 대기 KeyArt v3 원본2 + offline previews. 기획 담당 해시 원본/현재 동등성은 Codex 보고 참조.
- VenomSnake `VenomSnake_KeyArt_v3.png` PNG SHA `7001d199b279cb8e541fc603bd62351ab460d3b44da91a2d65dc099f8fd0`, 기존1254×1254 RGBA. Codex **`STATIC_1POSE_VISUAL_PASS_RECOMMENDED`**: 같은 전투 표시 칸에서 형태/독니 판독 양호, 주요 신체 잘림 없음. 다만 **낮은 알파 잔여 청소 권고**와 포맷 문서 SHA 누락 정정 필요. **바울 대표 최종 미술 승인 전**이므로 실사용 sprite/애니메이션/MonsterDefinition 등록 자동 허가 아님. **원본 v3 보존, 다시 그리지 말 것.**
- Wolf `Wolf_KeyArt_v3.png` PNG SHA `30b2d464fe2c456c0fa7d82f1fad51f64065b6e8379ea174548deec7d1b91b4e`, 952×1306 RGBA. Codex **`VISUAL_FAIL`**: **가슴 아래 회색 배경 매트와 뒷다리 배경 잔조각**(실제 털/발 끝이 아닌 배경제거 잔여물)이 남음. 해당 국소 alpha/edge-matte 제거만 표적 수행, 승인 후보 원래 늑대 해부학·털·발·색·크기/시선/실루엣을 바꾸지 말 것. 저해상 억지 축소/전체 재드로잉/Run 애니메이션 제작 금지. **원본 KeyArt SHA 보존**, 수정 PNG는 새 `Wolf_KeyArt_v3_MatteFix_v4.png` 같은 별도 파일명, original vs corrected alpha diff·128px 게임 크기 offline board·눈/다리/가슴 원본 마커 유지 확인, ZIP SHA/CRC/manifest 명확히 포함.
- 실제 Chapter2 공식 5종 PNG·PPU314 source와의 정적 같은 슬롯 비교는 Codex가 별도 수행한 **오프라인 비교**이고, Unity Actor actual worldScale / sorting / Runtime 아님. Snake/Wolf 각각 대표 최종 아트 승인 필요. 코드/씬, 펫 Wolf Run/Bear/Fox 및 Snake 전투행 시트는 대기.
- `docs/art-renewal` 담당 발주: **Wolf 배경 제거 최소 수리1종** 및 맹독뱀 PNG 원본 unchanged 표기·누락 SHA metadata만 보완. 고립 알파 미세 정리는 원본 승인까지 수정자산으로 임의 덮어쓰지 않고 별도 'ImportCleanup proposal'로 기록.

## B. Chapter3 첫 북부 F1 배경

- ZIP: `F:\Downloads\Limitless_Chapter3_NorthF1_ArtReview_Samples_v1.zip`, 문서상 SHA `dd9963d2a2ce442ad12ecccc8c3ae74edbfb7421eb838003d21ae2df767bd086`.
- `docs/art-new` 기존 6종 256×256 RGBA 전부 원본 보존, 3 후보별 Codex 시각 판정:
  - **F1 흙 후보 `HOLD`**: 커다란 얼룩/동일 패턴 반복, 토양 질감 평면적. 변형/흙층/작은 자갈·풀 결을 통해 눈에 띄는 패턴 재현 억제; 2×2/3×3 반복에서 과한 'stamp' 금지.
  - **F1 얇은 서리 `HOLD`**: 흙이 우세한 방향은 개선, 그래도 가장자리 표면 재질·반복무늬/얇은 서리 분포 보완. 전체 설원처럼 칠하거나 전면 화이트 마스크 금지.
  - **F1 전환부 `HOLD`**: 단방향 오버레이 구조(현재 우측 가중)는 기술 허용, 하지만 재질(흙/서리) 밀도 연결·모서리 경계 개선 필요. 기존 실제 Source와 alpha foot/tile seam 시각 보존.
  - **F1 전체 `HOLD`**. 기존 `NorthF1_Path_WornTrail_Overlay.png`: 토양과 명도 비슷해 길 가독성 불량. `NorthF1_Prop_FrostShrub_01.png`: 단순한 직선관목/명암 부족. `NorthF1_Prop_Waymarker_01.png`: 도식적 표지, 정보·접근성/방향표시 유지하되 깊이 부족.
- F1이 Chapter3 Main22 첫 플레이에서 직접 필요하므로 추가 `3지면 샘플 → 작은 보완 → 재보완` 끝없는 루프 대신 **기존 F1 6종의 결함을 전부 반영한 개선 art PILOT v2**를 한 번에 제작 발주. 단, 수리 범위는 기존6 대상/생태 환경 최초 얇은서리·녹색흙길만, 새 적·F2~F5·서릿길마을·설빙의 폭군 전투·퀘스트/스크립트·씬 추가 불허.
- **실제 F1 PPU/TileGrid/Collider 계약은 여전히 TBD**이므로 v2 PNG는 `ART_PILOT_V2`이지 `UNITY_READY` 아님. 기반 샘플3 SHA 및 Original6 PNG6 그대로 보호해 별도 새 v2 파일명 저장, 기존 256×256 RGBA 및 원본 Source 스타일을 기본으로 유지(실제 Unity contract 확인 없이 512·PPU 강제 변환 금지). 2×2/3×3 repeating preview, 지면/길/서리 전이 조합의 방향성·패턴/명도 대비, 128px UI 크기 미리보기, Chapter2 실제 환경 참고 보드, 접근 가능한 길/표지 인지성 시각 비교. 시안이므로 GameView라고 표기하지 말 것.
- `docs/art-new` 납품: F1 개선6 PNG, Original6 대비 전체/국소 changed pixel/alpha/가독성, manifest JSON 충돌/잘못된 SHA 메타 교정(실제 원본 PNG 변경 금지), ZIP SHA·CRC 및 화면 16:9/4:3/21:9의 **오프라인 합성** 보드. Codex 기술검수 후 사용자 미술 승인, 그 뒤 Main22 본게임에 연결.
- **지금은 아트 수정 지시, Codex 재검수 중복 발주 아님**. 기존 Codex `FrostFieldMouse` Combat Fix v2 정식 로컬 연결 완료40PASS/0FAIL은 별개.

## 팀별 다음 실행과 보호

1. `docs/art-renewal`: Wolf 원화 색·형태 보존 매트 최소 수정, Snake 원화 미술 PASS 후보 그대로 보존·문서 SHA 보강, 두 원화 완제품/최종 미술 승인 별도.
2. `docs/art-new`: F1 기존6 HOLD를 개선한 F1 6종 Art Pilot v2 한 번에 제작, 게임화/Unity 적용하지 말 것.
3. `docs/planning`: Main22용 북부 몬스터/드롭·잡템/아이템·스킬 상점·소모템/상태이상 아이콘 매핑·새 Sprite 전체 체크리스트 별도 내부 관리, ID·스탯/보상 임의 확정 금지.
4. `codex/*`: 위 실물 납품 후에만 **각 새 후보 ZIP SHA/CRC+동배율 시각 delta QA**. Main22 Actual Scene/Quest 및 Mouse Combat 연결이 이미 40PASS인 항목 재실행/중복 구현 금지.
5. 모든 역할: main 직접 커밋·Push·병합 금지. 승인된 Chapter1/2 Field04~11·Field09 5+B PNG 및 기존 Glow/meta/GUID, Chapter3 Mouse Idle/3 strips, Boss `설빙의 폭군` 승인 Idle/1pose, Save/UserData/원본 Editor 변경 금지. 사용자 Downloads 재정리·삭제 금지.
