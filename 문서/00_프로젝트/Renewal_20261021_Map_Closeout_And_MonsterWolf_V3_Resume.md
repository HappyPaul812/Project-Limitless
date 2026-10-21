# LIMITLESS — 2026-10-21 환경 리뉴얼 현황 정확 판정 및 Chapter1·2 몬스터·펫 작업 재개

- 기획 담당: `docs/planning`, 환경·몬스터·펫 아트 담당: `docs/art-renewal`; Codex는 실제 Unity/로컬 아트 QA. 사용자 지시: “리뉴얼 쪽은 맵은 끝난거 맞으면 어제 이어서 하자.”
- 정확 판정: **핵심 맵 아트 제작/통합은 상당 부분 완료했으나 Chapter1·2 모든 Scene이 `ART_FINAL_100_PERCENT`는 아님**. 그래서 기존 승인·적용 맵을 전면 재제작하지 않고, 미결 환경을 독립 backlog로 남긴 상태에서 **이미 발주됐던 몬스터·펫 오리지널 미술 파일럿 작업을 재개**한다.

## 맵/환경 단계 분리

- StarterVillage/Field01~03/Dungeon01 B1·B2: 기존 Chapter1 리뉴얼·기술 QA 이력 유지. 필요하면 최신 개별 공식 QA로 실제 승인 범위를 확인하고 남은 결함만 추적; 전체 Chapter1 품질 최종 무결 선언을 이번 한 번의 감사로 만들지 않음.
- Chapter2 Arbel/Field04~07: 사용자 승인/코덱스 원본 적용·PR 통합 이력 있음. 재제작하지 않음.
- Field08 RedRift: 현행 공식7 활성 PNG·Material 유지, 신규 PNG 필요 0. 이웃 Field07 연결 외관 QA가 미래 필요할 수 있으나 지금 새 그림 발주0.
- **Field09 ObsidianScar**: 2026-10-20 [PR41](https://github.com/HappyPaul812/Project-Limitless/pull/41)로 **사용자 승인 5 PNG + 별도 B 파생 Glow 총6 PNG/14 제품 파일 Scene Sprite 참조6종** 공식 `main`에 통합, EOD Unity 24PASS/0FAIL + 기존 격리39PASS/0FAIL 재사용. **핵심 지면·균열·절벽·석판·열기 문제 해결**, 기존 Glow 원본 GUID 보호. **잔여 HeatVent/Obsidian Spire 미술 갱신 및 AshPatch KEEP 후보/Cracked02 미사용 등은 [Issue38](https://github.com/HappyPaul812/Project-Limitless/issues/38) 계속 OPEN**. 이를 근거 없이 전체 100% 완료라 하지 않음. 이미 합격5+B를 다시 그리게 하거나 v1/v2 실패 ZIP 병합 금지.
- **Field10 BurningPulse**: Main19 공식 ArtPack13 PNG는 이미 Runtime 등록/기술 QA(과거314PASS)지만 문서 `Main19_Art_Handoff.md`의 최신 첫 줄에도 **`USER_ART_REVIEW_REQUIRED13`** 남아 있음. 사용자 최종 미술 승인 없이 새 아트가 필요하거나 '미술 완료'로 간주하지 않음. 지금은 기존 공식13 KEEP, 추후 사용자 해당 화면 QA 후 실제 불량만 갱신.
- **Field11 DeepCore**: Main20에서 v3/Phase2 Overlay 아트 채택 후 환경8 + Boss 등 게임 PNG17과 QA 통합 기록. 사용자 자연 손플레이·원본 Editor Import 등은 따로 검증 필요; 신규 미술 자동 발주0.
- 결론: `MAP_CORE_PRODUCTION_SUBSTANTIALLY_COMPLETE / FIELD09_REMAINING_PROPS_OPEN / FIELD10_FINAL_ART_REVIEW_PENDING / NO_AUTOMATIC_MAP_REMAKE / MONSTER_PET_ART_RESUME_AUTHORIZED`.

## Chapter1·2 몬스터/펫 이전 계약 이어가기 — 신규 최종 승인 아님

- 공식 선행 정본 `문서/00_프로젝트/Chapter1_2_Monsters_And_Beasts_Original_Art_Renewal_Assignment_20261019.md`. 실제 MonsterDefinitions18/펫 Wolf·Bear·Fox 분류 및 권리·PNG vs ThirdParty 구분이 우선. Chapter2 공식 몬스터5+Boss·기존 주요 캐릭터는 **KEEP**, 18종 일괄 재제작 금지.
- 과거 **VenomSnake v1·Wolf v1 미술 반려**, v2 32px/64px 단순 확대·완성도 저하로 HOLD. 이전 리뉴얼 담당 보고 `MonsterWolf_v3_KeyArt_Actual_PNG_And_Field09_Blocker_20261019.md`에는 **키아트 v3 독립 PNG2 제작 및 해시만 확인됐고 대표/공식5 실제 동배율 미술 승인은 없었음**:
  - `VenomSnake_KeyArt_v3.png`: 1254×1254 RGBA / SHA256 `7001d199b279cb8e541fc603bd62351ab460d3b44da91a2d65dc099f8fd0`.
  - `Wolf_KeyArt_v3.png`: 952×1306 RGBA / SHA256 `30b2d464fe2c456c0fa7d82f1fad51f64065b6e8379ea174548deec7d1b91b4e`.
  - 이 PNG2는 이전 해당 ChatGPT 세션 sandbox 산출물로만 보고, **현재 사용자의 정리된 F: 경로나 GitHub에 있다고 추측 금지**. 먼저 이미지 실물 보유 여부를 확인해 있으면 **재생성 말고** 실제 2장과 투명 PNG Alpha 경계 검사, 채택 스타일 QA에 활용.
  - 기존 Chapter2 공식 PNG 5종 비교는 **단순 이름/metadata/꾸며낸 시각 콜라주가 아닌 실제 공식 Monster Sprite 실물과 Unity 동일 표시 배율**을 기준. 아트 담당이 raw PNG 접근 불가능하면 즉시 LIMITATION 보고 + **기존 Codex QA 증거/로컬 읽기전용 자료 요청**, 미검증 비교 PASS 주장 금지.
- 이번 리뉴얼 담당 1차 업무는 **v3 맹독뱀1+늑대1 포즈를 대표가 실물 확인할 수 있게 납품/동일게임 스케일 비교판 준비**, 현재 소스가 없으면 이미 보존된 ZIP/이미지를 탐색 후 대체 제작 여부 제안. 실제 Render 비교가 불가능한 경우 그림 콘셉트 승인용 preview와 Runtime 실물 검수 대기를 구분. **Snake 전체 모션, Wolf Run/Bear/Fox 애니메이션, 실제 Unity/PR/공식 MonsterDef 변경 자동 허가 안 됨**.
- Chapter3 신규 서리들쥐/폭군 작업은 `docs/art-new`가 전담, Chapter1·2 리뉴얼 범위와 혼동 금지. Codex는 현재 Mouse Combat Fix v2 격리 QA 수행 중이므로 리뉴얼 담당이 Codex에 별도 대규모 검사를 중복 요구하지 않음.
- 제작/보고: `docs/art-renewal` 브랜치, 실제 PNG/미리보기/Manifest/SHA/출처·권리/기존 공식5 비교 근거, `USER_VISUAL_APPROVAL_PENDING`, branch/commit/납품 실제 경로 기록. `main` 직접 commit·Push/병합 금지, 사용자 F: 다운로드 정리된 위치 변경·파일 자동 추측/삭제 금지.

## 다음 승인 게이트

리뉴얼 담당 이미지2 실물 및 정확 QA 보고 → 기획/바울 대표 단일 포즈 미술 판정 → Codex 실제 스프라이트/PPU 비교·엔진 연결 계약 → 승인된 종만 실사용 전투/펫 Run 단계 제작. Field09 잔여 2소품과 Field10 미술 최종검토는 병렬 후속 backlog로 유지, 환경 결함 발견 시 별도 승인 후 최소 보완.
