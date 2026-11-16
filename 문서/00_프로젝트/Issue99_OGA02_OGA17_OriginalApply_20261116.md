# Issue #99 OGA02·OGA17 원본 게임 적용 기록 — 2026-11-16

## 승인과 결과

사용자 “게임에 넣자. npc.” 및 [Issue99 최종 적용 지시](https://github.com/HappyPaul812/Project-Limitless/issues/99#issuecomment-6906279696)에 따른 외형 적용입니다. #57·#95 적용 사례와 기존 카탈로그/Resources 방식을 재사용했습니다.

**LOCAL_ORIGINAL_APPLIED / APPEARANCE_DELTA_62_PASS / USER_REALPLAY_PENDING / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

- 시작 원본 main: `d360813667d2ee47515067b8dd07c1a599efec15`.
- 작업 브랜치: `codex/ch1-oga02-17-original-apply-20261116`.
- 제품 커밋: `7b84fa76e8948bb09febdf688898cdaa1ef231c8`。원본 FF-only/no-autostash 적용 완료.
- 본 보고·CODEX_LATEST·CURRENT_STATUS는 후속 Docs 커밋으로 동일 브랜치에서 커밋 후 원본 FF 동기화. 최종 실제 SHA는 Issue99 완료 댓글과 `Temp/Issue99NPC20261116/Final.json`에 기록.
- Unity Editor **6000.6.5f1**. 원본 버전·패키지 변경 없음, #85 패키지 오류에 의한 Import 차단 없음.

## 입력 검증

`F:\Downloads\LIMITLESS_ISSUE99_OGA02_OGA17_NPC_ORIGINAL_ART_PILOT_20261116.zip`

- ZIP 1,903,948 bytes / SHA256 `f10cb1f417f6bc03c64a05028f9549eacc23276e148c81136d5d922b2b4f1bdd`.
- 19 entries CRC PASS / Manifest 17개·SHA256SUMS 17개 일치.
- OGA02 원본 SHA256 `07bdb6368dcab01c85f551b71bd79ba391a3c3f621355664fc6453cb5da98cdf`,132616bytes.
- OGA17 원본 SHA256 `1282d90a7f89924e3efaad40679cbc272193bb9e05397765671549d9de646359`,137471bytes.
- 원본 PNG 무수정. 512×512 RGBA, resample·crop·alpha 정리·재생성 없음.

## 실제 제품 변경 5파일

PNG 경로 기준: `Unity/Client/Assets/_Project/Resources/VillageNpcSprites/Eldiran/`.

| PNG (+.meta) | GUID | PPU | 정규화 Pivot | 픽셀 Pivot | Stable ID |
|---|---|---:|---|---|---|
| OGA02_TrainingGuide_Original_Approved.png | 203587cfe9b34e5c9c7bbfc55117bf1f |357.875|(.5,.083984375)|(256,43)|starter-village-training-guide|
| OGA17_EquipmentShop_Original_Approved.png |5b473af875ba45b586eae7d500dbe10e|358.75|(.4990234375,.08203125)|(255.5,42)|starter-village-equipment-shop|

가시 영역 alpha≥8의 높이409/410px를 기존32/28=1.142857WU에 맞췄습니다. Pivot은 승인 가시 영역 하단에 맞추며 alpha1~7의 미세 halo는 보존(약0.0056WU 하단 확장). FullRect·Point·SingleSprite·mipmap없음. 독립 GUID 중복 없음.

나머지1파일 `Unity/Client/Assets/_Project/Scripts/NPC/VillageNpcAppearanceCatalog.cs`: 위 Stable ID 2개 Resources 문자열만 교체. 기존 OGA17 PNG/meta 및 `arbel-pet-adoption`의 구 리소스 연결은 유지. Scene·Prefab·Stable ID·Collider·가격·Quest·훈련/상점 로직 변경 없음.

## Delta QA

새 프로젝트 복제 없이 기존 `Temp/Art95_91Unity20261111/QA/Client` 재사용. 제품5파일·독립 QA Probe를 배치 Editor에서 확인했습니다. QA 환경에 누락된 기존 승인 OGA03/09 PNG/meta 4개만 원본과 동일하게 보완 후 재실행하여 최종62/62 PASS. 첫 실패는 해당 QA 누락으로 발생했으며 원본에는 영향 없음. Editor Search 색인 오류는 별도 환경 로그이며 신규 게임 Runtime 오류0.

- 960/1280/1680×720 실제 GameView 전후12장: 가시 높이/발 접지, 이름표·Collider·transform·sorting 보존, 두 NPC 카탈로그 자동 선택 PASS. 이미지 수동 육안 확인 완료.
- 가상 키보드 E로 실제 기존 안내 대화 열기·Esc 닫기, 패드 A/B 열기·닫기 PASS.
- **EquipmentShop 구매·판매·가격: NOT_IMPLEMENTED_BASELINE / NOT_RUN.** 기존 역할 구현이 “정식 거래는 다음 단계에서 열립니다” 안내뿐입니다. 새 거래 기능을 만들거나 거래 PASS로 표시하지 않았습니다.
- **TrainingGuide: 기존 안내 대화 PASS.** 별도 훈련 UI는 기존 미구현으로 추가하지 않았습니다.
- 월드 NPC 마우스 직접 상호작용은 기존 미지원(N/A). 실제 마우스 raycast/click은 기존 **GeneralShop 잡화점** 확인/구매/판매 UI에서 검증했습니다. 구매·판매 후 실제 소지금/수량·가격 변화 PASS. 이는 장비 상점 거래 검증이 아닙니다.
- 기존 전체 ItemDefinition 가격, 승인4종·다른 NPC 참조·Arbel 기존 OGA17·마을 Scene 재진입 PASS.
- QA Save/Settings는 `Evidence/SyntheticSave`, `Evidence/SyntheticSettings` 전용. 원본 저장 데이터 접근/수정 없음.
- 원본 Editor 표적 Import 후 실제 Resources.Load 성공, 두 신규 GUID·PPU·Pivot 확인. 컴파일 idle, Console error0. Scene/Play/selection 보존. 원본에서 강제 Play/Scene 전환 없이 실제 플레이 검증은 격리 QA에서 수행.

증거: `Temp/Issue99NPC20261116/Evidence/checks.txt` 62PASS, status.txt PASS, 전후/대화/상점 PNG15장, ScopeNotes.txt, Unity-rerun.log, original-resources.json, original-console.json, protection-after.json. QA 도구·합성 Save·전체 증거는 제품 커밋에 넣지 않았습니다.

## 보호·디스크·확인 위치

- 원본 Assets/Packages/ProjectSettings/UserData 기존4674파일(480804317bytes)의 SHA256 대조: 허용 카탈로그1파일 외 변경0. 신규 PNG SHA 원본 일치. 기존 사용자 dirty/untracked git status 바이트 동일.
- OGA03/06/09/16·HOLD OGA20·기존 Scene/Quest/가격/Stable ID/Collider/Save/UserData·Discord 파일 변경 없음. 외부 Save/Settings는 QA 경로로 격리해 사용하지 않았으며 외부 개인정보 전체 해시 감사는 하지 않음.
- F: 시작324418146304bytes → Import 후322490466304bytes 여유. 기존 Unity QA Import 캐시 재사용으로 약1.93GB 감소, 여유 충분. Library/Temp/기존 백업·Discord 삭제 없음. 작업 전체 프로젝트 복사 없음. 새 소형 입력/증거와 sparse worktree만 생성. 캐시 무차별 정리 없음.
- main 직접 commit·강제 reset·stash·병합 commit·원격 push 없음. 로컬 ff-only 반영만 수행.
- 바울 대표: **시작 마을 동쪽**, 훈련장 관리인(6.4,-2.1), 장비상인(5.8,1.2). E/F 또는 패드 A로 기존 안내 확인. 장비 거래 기능은 기존 미구현 상태임에 유의.
