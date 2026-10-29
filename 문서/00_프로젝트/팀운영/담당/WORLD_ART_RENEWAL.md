# Chapter 1·2 기존 자산 리뉴얼 담당 행동요령

**최종 사용자 결정(2026-10-12): 리뉴얼 담당은 Chapter 1·2 기존 아트 교체(환경·몬스터·보스·펫·NPC·VFX)만 전담합니다. 기존 스프라이트 담당은 Chapter 3 이후 모든 신규 아트(환경·필드·던전·몬스터·보스·펫·NPC·캐릭터·VFX·UI 필요 이미지)를 총괄합니다. 기존 공식 승인 자산은 별도 승인 없이 교체하지 않습니다. Codex는 구현·Unity 통합·QA 전담입니다.**

## 즉시 적용할 최신 범위
- Chapter 2 환경: Field_04_WesternBorder → Arbel → Field_05_WesternOutskirts → Field_06_ScorchedTrail → Field_07_AshenReach → Field_08_RedRift → Field_09_ObsidianScar → Field_10_BurningPulse → Field_11_DeepCore.
- 환경 정식 적용 이후 Chapter 1·2의 실제 무료/공용 몬스터→펫→마을 NPC→VFX 순으로 조사·교체. 사용자 제공 공식 몬스터 시트나 승인 보스·캐릭터는 무단 교체하지 않습니다.
- [Chapter2 리뉴얼 로드맵](../../LIMITLESS_Chapter2_Original_Art_Roadmap_20261012.md) 및 [인계문](../../LIMITLESS_Renewal_Lead_Handoff_20261012.txt) 참조.
- Chapter 3 이후 신규 아트는 제작하지 않고 기존 스프라이트 담당에 맡깁니다. 겹치거나 같은 자산을 중복 주문하지 않습니다.

## 목적
기존 무료 에셋 인상이 강한 환경을 **LIMITLESS 고유의 따뜻하고 생활감 있는 안전지대 → 이상 징후가 스며드는 필드·던전** 방향으로 단계적 교체한다. 리뉴얼은 **게임 기능 보호가 최우선**이다.

## 시작 자료
[CURRENT_STATUS](../../CURRENT_STATUS.md) · [월드 제작 규칙](../../../20_월드/월드_제작_규칙.md) · [리뉴얼 마을 기준](../../StarterVillage_Renewal_Baseline_20261009.md) · [마을 v4 검수](../../StarterVillage_Renewal_Art_v4_Inspection_20261010.md) · [Field01 리뉴얼 QA](../../Field01_Renewal_Recovery_QA_20261012.md).

## 업무·보호 원칙
1. 기존 계획 순서는 **World_StarterVillage → Field_01 → Field_02 → Field_03 → Dungeon_01 → Dungeon_01_B2**이지만, 완료된 앞 단계 재작업을 자동으로 반복하지 않는다. CURRENT_STATUS에서 **이미 완료·사용자 승인된 부분과 아직 미검증인 부분**을 구분해 다음 대상을 결정한다.
2. NPC/퀘스트/상호작용/E키, Spawn, Exit·Scene 전환, 동선·장애물 Collider, 몬스터 배치/리스폰, Bounds/Camera, Save/Continue, 보행·접근성 기능을 보존한다.
3. 가능한 한 **환경 Sprite만 교체**하고 좌표·Scale·Pivot·Sorting·Trigger·Collider·Generator는 함부로 재생성하거나 변경하지 않는다. 필수 구조 변경은 별도 기획/검증 계약을 받는다.
4. 실제 맵/ZIP/Scene/Importer/기존 Renderer 대응은 **Codex가 로컬 조사**한 목록을 바탕으로 제작한다. 채워 넣지 않은 임의의 타일/맵 크기를 사실처럼 가정하지 않는다.
5. 아트 역할·층·렌더러 맵핑표, 필요한 투명 PNG, 반복 타일 이음, 원근·접지, 색상뿐 아닌 인지 가능성 및 QA 스크린샷 요구사항을 정리한다.
6. 사용자 미술 승인, Codex의 Import·GUID 검증, Runtime 이동·전투·저장 회귀 검증을 각기 다른 단계로 관리한다.

## 후임
인계/WORLD_ART_RENEWAL_LATEST.md에 **씬별 완료/부분완료/미시작, 승인 버전, 보존해야 할 기준 Scene, 다음 제작·검수 대상**을 기록한다.

## 2026-10-29 / 바울 대표 최신 리뉴얼 제작 정책 — 우선 적용

- **우선순위 정정:** Chapter1·2 실사용 **비오리지널 전투 몬스터의 고유 자산 전환**이 일반 마을/퀘스트 NPC의 고급화보다 먼저다. 기존 공식 승인 몬스터·펫·환경·보스는 손대지 않는다.
- **마을·퀘스트 NPC 최소 투자:** 초상화 없이 서 있는 NPC는 역할별 구분·LIMITLESS 화풍·자체 오리지널 자산·기능 보호만 요구한다. 상인·주민대표·남문 경비의 기존 시안은 보존하고 불필요한 반복 polishing/전면 재작업을 금지한다.
- **forest_spider는 오리지널 교체 확정(P1).** CC0이라 이용 가능하다는 점과 **우리 자체 자산이 아니라 리뉴얼 필요**하다는 점을 구분한다. 원본32프레임 현재 게임 사용/보존은 정식 교체 전 임시 KEEP이며 리뉴얼 큐 삭제 사유가 아니다.
- **단계:** (1) 오리지널 디자인 1포즈 파일럿 (2) 바울 대표 실제 시각 승인 (3) 승인 후 Idle/Walk/Attack/Shoot 애니메이션 제작·Codex 격리 QA·별도 본편 적용. 현재는 **문서 계획만 작성, 신규 이미지/애니메이션 제작 0**.
- **VenomBee #63** 1포즈 TECH PASS / 실전 교체 ART HOLD, CC BY3.0/4.0 라이선스 불일치 HOLD, 바울 대표 승인 전 원화 수정·키포즈·20프레임 제작 모두 금지.
- 기술 근거와 상세 큐: [2026-10-29 우선순위·숲거미 파일럿 계획](../../Renewal_20261029_Original_Asset_Priority_Spider_Pilot_NPC_Minimal.md). 이전 조사·진행상태는 보존. Unity 원본/Save/GUID/기승인 아트·main 직접 commit/merge 금지.

