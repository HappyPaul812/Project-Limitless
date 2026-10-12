# 월드·환경 리뉴얼 아트 담당 행동요령

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
