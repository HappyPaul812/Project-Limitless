# LIMITLESS — Field05 동서 주도로 v2 수정 발주 (2026-10-19)

**상태: V1_ART_STYLE_LIKED / NORTH_SOUTH_ROAD_HOLD / EAST_WEST_REWORK_REQUIRED / USER_FINAL_APPROVAL_PENDING / ORIGINAL_UNCHANGED.**
바울 대표 실화면 피드백: “맘엔 드는데, 챕터2는 방향이 서쪽인데 왜 길이 자꾸 남쪽이야?”

## 실물 원인
- GitHub main Field05 Scene 환경 Renderer363 = Grass315, Road30, Props18. RoadLeft 및 RoadRight 이름의 기존 Road30은 y -7..7에 세로15행×2열이며 x는 -1.5~1.5에서 굽어진다. v1은 Sprite 교체만 하고 Transform을 보존하여 **세로도로가 그대로 남았다**.
- Chapter2 사건 전개는 아르벨 거점에서 서쪽 Field05 조사 방향. 기존 Field05 동쪽 Arbel 복귀 Spawn (8,0), Trigger (10.7,0). 현재 확인된 Field05 FieldConnection은 동쪽 아르벨 연결뿐으로, 새 서쪽 Portal/Field06 출구를 가정하거나 신설하지 않는다.
- Main14 메마른 관개수로 (5,2), 감시초소 (-2,2), 강한 지면 진동 (-6,-2), 세린 (1,0); SootHound (3,-3), FissureLizard (-4,3), 활동반경1 및 35초 Respawn은 고정 보호.

## 수정 방향
1. 주도로는 **동쪽 아르벨 출입구에서 서쪽 조사 지점으로 이어지는 동서 간선**. 현재 30종 Renderer를 15열×2행으로 이동하는 후보와, Field04처럼 전체 맵폭 21열×2행 42개로 연장하는 후보를 각각 격리 QA/실제 Unity 스크린샷 비교. 42개 확장은 추가 GameObject12를 필요로 할 수 있으므로 정식 적용 승인으로 오인하지 않는다. 구체 위치는 실제 Camera/Bounds/출구 연결 감사를 거쳐 결정.
2. 수로·초소·진동 조사점은 도로 옆 가까운 **짧은 지선/흙 흔적**으로 읽히도록 배치하되 필수 Collider/Quest/E키/내비 목적지/Spawn을 이동하지 않는다. 서쪽 출구가 없는 현재 Scene에서는 서쪽 길 끝을 실제 새 구역 Portal처럼 표현하지 않는다.
3. 기존 v1 황토·올리브 색채, Ground3, Props7, 독자 소품의 디자인은 **유저가 좋아한 방향이라 보존**. 동서방향 길에 적합하지 않은 Road2 PNG는 리뉴얼 담당이 최소 v2 수정(새 적합 타일/Endcap 필요성은 근거 제시). 기존 alpha seam 0 및 PPU/Pivot/foot 실제 검증은 다시 수행.
4. 선택 소품7은 미술 개별 승인 전 배치0, Field04/Arbel 및 Field03 Rock B 별도 문제를 건드리지 않음. 신규 Chapter3 아트 영역 아님.

## 역할·게이트
- **Codex**: ZIP F:\Downloads\Limitless_Field05_Renewal_Art_v1.zip 실제 SHA/Manifest 확인(보고 SHA a6646a676a619bc149798fe0f3909401b9c3b5a75f253cf6a1e61e21661b1258). isolated worktree/복사 Unity에서 남북원본 vs 동서 Road30 vs 동서 Road42 실제 Unity RenderTexture를 **동일 카메라**로 제작. Renderer 이름/ID·기존 Sprite meta/GUID·물리/Quest/Save/Monster/Arbel 복귀/도보·휠체어 QA. 추가 PNG 설계가 필요하면 정확히 Road 몇종인지 아트 담당에 기술 인계.
- **리뉴얼 담당**: Codex 시각·좌표 감사에 맞춰 Road2 v2 등 최소 변경, PNG ZIP/Manifest/Mapping/Preview/QA 정리. 임의 Scene/Collider/Save/Quest 수정 금지.
- **기획/사용자**: 이번 지적은 **V1의 컬러·무드 선호**만 의미, 공식 미술 전체 승인 아님. 수정본 실제 Unity 시각 승인 이후 Codex 정식 적용 QA/작업 브랜치.
- **로컬 원본 보호**: 이전 PR16에서 로컬 문서 커밋2개 원격 이력 보존, dirty 겹침464/내용차이5 경로로 local sync PARTIAL. 원본 Editor·Play·포커스/Save/dirty 파일 무접촉. reset-hard/clean/force-push/무단 stash 금지.

**수락 기준:** 서쪽 진행 축 명확, 동쪽 아르벨 출입구 자연 연결, 세린/조사 3점·몬스터·휠체어/귀환 경로 정상, 새 도로 seam/접지/화면비/Save Regression 통과, 실Unity 전후 이미지 제시. 신규 Portal 추가0. 원본 정식 적용·Git push/merge는 사용자 미술 승인 전0.
