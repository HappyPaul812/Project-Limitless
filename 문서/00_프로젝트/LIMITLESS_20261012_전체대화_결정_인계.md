# LIMITLESS — 2026-10-12 대화 결정 및 인계 총정리

기록 구분: **사용자 결정 / Codex 보고서상 기술 PASS / 부분 QA / 후속 대기 / 미래 기획**. 실제 Unity 미검증 항목을 완료로 표시하지 않습니다. 최신 CURRENT_STATUS와 해당 QA가 세부 정본입니다.

## 사용자 최종 운영 규칙
- 상용 게임명 Limitless, 개발 프로젝트 Project-Limitless. 한국어 존댓말 사용. 게임의 장애·접근성은 실질적 이동·전투·상호작용 편의로 구현하고 일률적인 극복 서사로 소비하지 않습니다.
- **Chapter 1·2 기존 무료/공용 아트의 오리지널화는 리뉴얼 담당 전담**: 환경·몬스터·보스·펫·NPC·VFX.
- **Chapter 3 및 이후 모든 신규 아트는 기존 스프라이트 담당 전담**: 마을·필드·던전·환경·몬스터·보스·펫·NPC·초상화·캐릭터·VFX·필요 UI 이미지.
- 동일 자산의 중복 제작 금지. 이미 사용자 승인한 공식 캐릭터·몬스터 시트 및 수동 편집 원본은 명시적 교체 지시 없이 보호. Codex는 Unity 통합·기술 QA 담당, 기획은 계약·우선순위·문서화 담당.
- Codex Unity 검사는 백그라운드·비대화형·격리 Save로 진행하고 사용자 게임/Editor 포커스를 빼앗지 않습니다. 기존 미커밋 파일·GUID/meta·ThirdParty·Save·TTS·BGM 보호. **Git push는 사용자가 직접**.
- 모든 담당은 GitHub의 START_HERE·COMMON_RULES·담당 행동요령·CURRENT_STATUS를 읽습니다. 세션 용량이 차기 전 선제 보고·후임 세션 인계 문서 갱신.

## Chapter 1 환경 리뉴얼 기록 (기존 Codex 정식 QA 기준)

| 대상 | 확인 상태 |
|---|---|
| World_StarterVillage / Field_01 | 사용자 승인 리뉴얼이 기준. 최신 별도 QA에서 기능·가독성 미검증 구분 |
| Field_02 | 공식 12 PNG·373 참조 적용, Main06~07/연결 QA, 전체 PARTIAL |
| Field_03 | 공식 11 PNG·375/376 참조 적용, Main08~10·Main11 입구/35초 리스폰 QA, 전체 PARTIAL. 남은 1참조는 Rock B |
| Dungeon_01 B1 | 승인 PNG25 등록, 63 앵커 보호, 공통 DungeonEnvironmentRenderer의 Tiled20/소품8/중복33 숨김·기존 문2. Main11 B1·일반4전투·35초·휠체어/Continue QA, 전체 PARTIAL |
| Dungeon_01 B2 | 승인 PNG31 등록, 54 앵커 보호, 같은 Renderer의 Tiled18/소품8/중복28 숨김. Main11 B2·일반4전투·35초·보스 Phase2·석판 획득·완료/Continue QA, 전체 PARTIAL |

- **B2 봉인실 스포일러 수정 완료 보고**: 보스 승리 전 단서7개 숨김, 승리 후 표시, 석판 회수 후 석판 그림만 숨김. 기존 Quest·Inventory·장벽 조건 유지.
- **B1 봉인문**: 원래 대형 문 PNG는 벽 Collider/조사 지점과 맞지 않아 보류됐습니다. Codex 후속 보고에서는 기존 벽 안의 작은 **0.75×0.625 상태 표식/하강로 안내**로 안전하게 보완했다고 기록(구현 `6c78cdb`). 대형 문 PNG가 적용됐다고 말하면 안 됩니다.
- **Field03 Rock B**: 분리 알파 14픽셀/접지 문제. Codex가 v2 ZIP을 찾지 못해 `ART_FIX_PENDING`, 기존 RockNorth Sprite 1개 유지. “Codex 작업 완료”와 Rock B 수정 완료를 혼동하지 않습니다.
- 전체 PARTIAL은 기존 Unity Search 예외, 사용자 직접 조작·장시간 성능/가독성·독립 상업권 증빙 미검증 등을 포함하며, 개별 실제 UI/Runtime 검사 PASS는 별도로 인정합니다.

## Chapter 2 현황 및 새 리뉴얼 계획
- Chapter 2 범위 Main12~Main21, Chapter 3 시작 Main22. Main20 거신과 Main21 전투/마차·하렌 첫 등장·Save/Continue 중심 자동 Runtime QA는 통과 범위가 있으나 Chapter 2 모든 구간의 사람 직접 플레이 완료는 아님.
- **다음 리뉴얼 순서**: Field_04_WesternBorder → Arbel → Field_05_WesternOutskirts → Field_06_ScorchedTrail → Field_07_AshenReach → Field_08_RedRift → Field_09_ObsidianScar → Field_10_BurningPulse → Field_11_DeepCore.
- 환경 교체를 먼저 완료한 뒤 **기존 몬스터→사수 펫→마을 NPC→공용·직업·보스·환경 VFX**로 확장. 실제 Scene/Renderer/Importer·동선을 Codex가 조사하고, 리뉴얼 담당은 실사용 PNG/Manifest/Preview/ZIP을 제작. 사용자 미술 승인 이후 Codex가 통합합니다.
- 기존 Chapter 2 다섯 종(그을음들개/열풍매/균열도마뱀/화열딱정벌레/불씨망령)은 사용자 제공 수정 공식 시트가 있으므로 무조건 무료 애셋으로 분류하거나 재제작하지 않습니다.
- 첫 제안/제작 지시물은 `Limitless_Field04_Renewal_Art_v1.zip`. 대화에서 지시한 사실만 확정하며 실제 납품·통합 완료로 간주하지 않습니다.

## Main18~19 세린 TTS 결정
- 사용자 직접 청취로 Main18 세린10개 **남성처럼 들리는 오류**·재제작 필요, Main18 다른 세린5개와 Main19 세린7개는 정상. Main17 세린11개 기존 승인 원본 보존.
- 목소리 설정 `Schedar` 유지. 실제 API 음성 성별·WAV 원인 및 전수 청취 결과를 추정하지 않습니다.
- Codex가 정상 Main18 NPC12 WAV + Main19 NPC13 WAV, **합계25개**를 연결했다고 기록. 오류10개는 ID/자막 보존, 무음.
- 오류 WAV10개만 다음 무료 할당량에서 안전 재생성: 우선 1개 샘플 사용자 청취 승인 후 나머지9개. 승인 전 TTS 유료 API 사용 금지. 이미 확인된 정상 음성 변경 금지.

## Chapter 3 기획 정본 (구현과 구분)
- 레벨 상승 자동 강화 대신 **탈렌트로 스킬 상점에서 강화판 구매, 기존 슬롯 대체**. 직업당 신규 스킬2개 목표(사수는 펫 전술로 예외 검토).
- 치유사 신규 **부활**(전투불능 아군1명, 냉기 보스 이전 획득 가능); 기존 정화 강화판 **정화Ⅱ**는 기존 해제 범위+과열/한기/동상 제거, **동결괴사 제거 불가**.
- 마도사 한기 전용 신규 해제 스킬(가칭 **온기의 불꽃**)은 동상·과열·동결괴사를 치료하지 않습니다.
- **한기**: 행동 불가2턴. **동상**: 행동 불가+HP 감소. **동결괴사**: 보스 전용3중첩 즉시 전투불능(영구 사망 아님). 보스는 대응 가능한 예고 기회를 제공해야 합니다.
- 전투 UI의 **온기의 화로**는 아군 행동1회를 소비해 살아 있는 동료의 동결괴사 중첩을 전부 제거. **따끈 어묵탕**은 한기 전용, **해빙 연고**는 동상 전용. 마법·정화·아이템 역할 혼동 금지.
- 신규 스킬 이름/수치·스킬 가격·Save 스키마·부활 HP·동상 피해·새 몬스터/아이템 등은 미정. 구현됐다고 쓰지 않습니다.

## 각 담당 다음 작업
1. 리뉴얼 담당: Chapter 2 Field04 실사용 아트 v1 제작, 이후 Arbel~Field11 환경, 기존 몬스터·펫·NPC·VFX. Chapter 3 신규 아트 제작 불참.
2. 스프라이트 담당: Chapter 3 이후 **모든 신규 아트**. 기존 공식 인물과 승인 PNG 보존, Chapter 1·2 리뉴얼 중복 불참.
3. Codex: 승인 자산 실제 Import·Unity QA; B1 작은 문 상태 표시 기통과 보고, Field03 Rock B v2 납품 전 기존 참조 유지; 원본 Editor/사용자 작업 보호.
4. TTS 담당: Main18 오류10개만 선별 재생성, Main17/정상18·19 보호.
5. 기획 담당: Chapter3 미정 스킬·상점·상태이상·전리품 경제 수치 사용자 확정 후 별도 문서 반영.

문서 정본: `LIMITLESS_Chapter2_Original_Art_Roadmap_20261012.md`, `LIMITLESS_Renewal_Lead_Handoff_20261012.txt`, `팀운영/담당/WORLD_ART_RENEWAL.md`, `팀운영/담당/SPRITE_ART.md`, `문서/06_스킬/Chapter3_스킬상점_강화_냉기_동결괴사_20261012.md`, `문서/00_프로젝트/팀운영/인계/CODEX_LATEST.md`.
