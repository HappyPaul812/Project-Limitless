# Sprite 참조 복구·최종 통합 QA — 2026-10-14

**필수 Sprite 게이트 PASS / 새 기능 회귀 FAIL0 / 기존 제약을 포함한 종합 PARTIAL.** PR #10 Ready와 정상 main 병합 조건을 충족했습니다. 원격 Push·최종 병합 여부와 SHA는 PR #10 및 완료 보고에서 확인합니다. 이 보고서를 이전 결함 전체가 해결됐다는 뜻으로 해석하지 않습니다.

기준 EOD `47b865e9c19c23d30c2c9eb5e14c79cddb79a52d`, main `0e0476649b667129cb7aa6004f4b677335ed8be2`, 최신 planning `1140dd5`. 새 브랜치 `codex/sprite-reference-recovery-20261014`, 구현 commit `5507f94e723f1d02c27f2a0839af2bdbac53bc5c`. 최신 planning의 복구 게이트/인계 2문서를 보존 병합했습니다. 이후 새 meta의 빈 값 뒤 공백을 제거했으며 Importer 값은 그대로입니다.

## 원인과 최소 복구

GUID 파일은 존재했지만 원격 ThirdParty meta의 Sprite 분할 등록이 Scene local fileID와 불일치했습니다. tiles_grass는 원격31/로컬60, fence_tiles는10/25, house_tiles_new는3/30 Slice입니다. grass의 Scene 사용 ID 405149151/1662121770/278596618/1271049111/1606921373 등은 로컬 등록에 있으나 원격 현재 subasset에 없었습니다. 소품8개는 Single Sprite 21300000 참조와 기존 Multiple 등록 불일치를 로컬 정상 Single Importer로 복구했습니다. 전체 GUID/fileID/Renderer와 PNG SHA·Importer 원문 SHA는 CSV/manifest에 기록했습니다.

기존 PNG11을 `_Project/Art/Environment/SpriteReferenceRecovery20261014`에 **동일바이트** 복사하고 신규 GUID를 등록했습니다. 새 Sprite subasset123개를 로컬 meta의 rect/pivot/PPU와 실제 Unity 객체로 대조하여123 PASS. PNG 픽셀 변경0, 사용자 meta 변경0. 새 meta는 로컬 원문에서 GUID와 빈 값 뒤 공백만 달라집니다. 5개 Scene의1747 Sprite GUID만 교체하고 local fileID를 유지했습니다. GUID 역치환 후 원래 Scene 전체 텍스트 동일: Transform/Collider/Sorting/Tint/NPC/Quest/Spawn/Exit/Camera/몬스터 변경0. ThirdParty 직접 변경·Scene 재생성·새 미술 제작0.

## 23개 활성 BuildScene 전수

모든 Scene의 실제 Sprite 참조3654개가 AssetDatabase의 Sprite local fileID까지 일치합니다. MissingScript0, 미해결 직렬화 Sprite 참조0. Runtime23개 모두 Sprite null0. Edit Mode의 VillageNpc2개는 직렬화 fileID0인 동적 항목이며 실제 초기화 후 정상 Sprite가 할당됨을 확인했습니다. Arbel·Field05~08의 실제 GPU RenderTexture 배경 출력도 직접 확인했습니다. 이미지의 잔디 패턴·기존 단색 표식·배치 등은 로컬 정상 설정에 따른 기존 화면이며 미술 리뉴얼이나 최종 미술 승인으로 보고하지 않습니다.

| Scene | 이전 EOD Edit null | 복구 Edit null | Runtime null | Runtime Renderer |
|---|---:|---:|---:|---:|
| Bootstrap | 0 | 0 | 0 | 0 |
| OpeningIntro | 0 | 0 | 0 | 0 |
| CharacterCreation | 0 | 0 | 0 | 0 |
| PathSelection | 0 | 0 | 0 | 0 |
| JobSelection | 0 | 0 | 0 | 0 |
| FinalConfirmation | 0 | 0 | 0 | 0 |
| World_StarterVillage | 1 | 1 | 0 | 313 |
| Field_01 | 0 | 0 | 0 | 372 |
| Field_02 | 0 | 0 | 0 | 380 |
| Battle | 0 | 0 | 0 | 0 |
| SampleScene | 0 | 0 | 0 | 0 |
| Field_03 | 0 | 0 | 0 | 387 |
| Dungeon_01 | 0 | 0 | 0 | 97 |
| Dungeon_01_B2 | 0 | 0 | 0 | 94 |
| Field_04_WesternBorder | 0 | 0 | 0 | 379 |
| Arbel | 285 | 1 | 0 | 295 |
| Field_05_WesternOutskirts | 362 | 0 | 0 | 367 |
| Field_06_ScorchedTrail | 362 | 0 | 0 | 320 |
| Field_07_AshenReach | 371 | 0 | 0 | 383 |
| Field_08_RedRift | 368 | 0 | 0 | 18 |
| Field_09_ObsidianScar | 0 | 0 | 0 | 14 |
| Field_10_BurningPulse | 0 | 0 | 0 | 19 |
| Field_11_DeepCore | 0 | 0 | 0 | 8 |

이전 Arbel/Field05/Field06/Field07/Field08 Runtime null284/362/313/371/8 → 모두0. 전체 Edit null1749→2(모두 동적), 실제 누락1747→0. Field06/08은 Runtime 환경 재구성으로 Renderer 수가 달라지므로 Edit와 Runtime 개수를 동일시하지 않습니다.

## 새 백그라운드 검증

| 검사 | PASS | FAIL |
|---|---:|---:|
| Field04 | 254 | 0 |
| Main08 승인6 | 177 | 0 |
| 스킬 장착·2×3 | 72 | 0 |
| 기존15스킬 | 154 | 0 |
| 펫 서비스3거점 | 106 | 0 |
| Modal 공존 | 28 | 0 |
| 펫 계약/효과 | 64 | 0 |
| 서부5 Scene 출구·Camera·Save/Continue·지체의길 | 85 | 0 |
| 실제9개 의뢰·완료/미완료 Continue·중복 보상 방지 | 589 | 0 |
| 실제 Sprite123 Importer | 123 | 0 |

기존854검사 범위를 새로 실행하여 **855 PASS/기능FAIL0**. Field04의 동일 waypoint (8,.25)가 한 번 더 계측되어 이전253보다1개 증가했으며, 일반전투 승리 공격횟수는7→8로 변했습니다. 이는 새 스킬/효과 변경이 아니며 기존 UI 승리·35초 리스폰·실제 물리 출구·Main12~13·휠체어·Camera·Save/Continue를 통과했습니다. 퀘스트 기반 Audit ALL PASS, BattleSilence ALL PASS. 원래 정적83개 재실행 PASS, 복구 확대 정적322 PASS. 전체 Main01~21을 처음부터 새 손플레이로 재현한 것은 아닙니다. Quest/Story/Battle/Save 런타임 코드와 데이터는 EOD와 동일하며 이전 검증 근거와 이번 영향 범위 회귀를 함께 적용합니다.

Field04 Scene·도로42·Renderer375·PNG52는 승인 ec469c0와 동일. Main08 승인WAV6/PCM/GUID/meta/Catalog는79554069와 동일. B/C 런타임·15개 효과·SaveVersion1·Chapter3 승인 문서 불변. 새 장착 영속 저장·상품·스킬·Chapter3 완료 flag 생성0.

## 보호·남은 제약

원본5221파일 SHA256/HEAD ff022e6/전체dirty status 불변. 현재 원본 status385항목을 보호했으며 어제337항목과 혼동하지 않습니다. 원본 PNG/WAV/meta/Scene/Save/설정/TTS/BGM/문서 변경0. 테스트 Save·설정은 이 Client 하위 새 경로로 강제 지정했습니다. 외부 LocalLow/DefaultCompany/Client에는 확인 시 사용자 Save/설정 파일0이며 Unity 자체 Analytics 파일은 보호 SHA 대상에 포함하지 않았습니다. 원본Editor는 Bootstrap/Edit/비포커스 상태를 읽기 전용 조회했으며 Play·Scene·OS포커스 조작0. 새 TTS/API/음원 제작 호출0.

실제 QA Unity6000.6.5f1, 브랜치 정본6000.5.7f1 직접 실행 미검증. 기존 UnityEditor.Search 예외는 새 Runtime 오류와 분리했으며 기능FAIL0. 초기 격리 batch 동시 실행에 따른 프로젝트 점유 종료1 및 초기 Import 대기를 보존하고, 격리 프로세스만 경로 확인 후 재시작하여 최종 검사 전부 정상 종료0. 브랜치 Packages/ProjectSettings 변경0. 기존 Field04 서쪽 Spawn/TreeWest 겹침·북쪽 Collider 틈, Main08 태온005/006 내용·대화중일반조우, 스킬 영속 Save 미구현, 실물입력·장시간 가독성은 OPEN/PARTIAL 그대로입니다.

최종 원격 처리는 복구 브랜치 Push→EOD 안전 반영→동일 게임 트리 확인/필수 참조 QA→EOD Push→PR10 Ready→승인 범위 정상 merge 순서입니다. 필수 게이트 실패 시 main 병합을 보류합니다. main 직접 commit/Push·force·원본 pull/reset/stash/clean은 수행하지 않습니다.

[전수표·Importer·회귀·실제 화면 근거](Evidence/SpriteRecovery20261014/)
