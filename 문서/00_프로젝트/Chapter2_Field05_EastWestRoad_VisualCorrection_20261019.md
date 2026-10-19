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


## 2026-10-19 A/B 실제 Unity 비교 보고 및 신규 Road v2 납품 확인 — Codex Main18 작업 우선

바울 대표가 Codex **실제 Unity A/B 화면**과 결과를 전달함. 이 시각 자료는 로컬 격리 Unity의 출력이며 기획 담당이 직접 파일로 Unity를 실행한 것은 아님.

- **A30:** 기존 Road30(15열×2행) 동서 배치, 서쪽으로 갈수록 길이 중간에 끊겨 보이고 짧음.
- **B42:** 21열×2행 동서 간선, 서쪽 진행 방향이 더 분명해 보임. **기획 검토의 우선 시안은 B42**로 한다. 이것은 **사용자의 최종 B 구조 승인이나 실제 아트 v2 미술 승인 선언이 아님**. User art review pending. B 적용은 기존30+신규Renderer12 구조/Scene 변경을 요구함.
- 기존 v1 색감/소품은 그대로. Codex 보고 격리 자동 검사 **1,582 PASS/0 FAIL**, Main14/세린/두 몬스터/Arbel 왕복/Save/Continue PASS. **Console gate FAIL = Search·URP가 기존 교체 전 기준선에서도 재현됨**, 신규 Console fail로 단정/은폐하지 않고 baseline 분류, 전체 QA 판정은 **PARTIAL**. 원본 **4,067파일 변경0**, 공식 Unity 버전 직접 Import·수동 조작·최종 시각 승인 미검증(검사 환경 Unity **6000.6.5f1**; 보고된 정본 **6000.5.7f1**은 버전 진위 및 실제 사용 버전 재확인 필요).
- **리뉴얼 아트 담당이 이미 정확한 신규 동서 Road PNG 4종을 제작·별도 ZIP 납품**했다고 GitHub `docs/art-renewal`에서 확인함. 자료: `문서/00_프로젝트/Field05_EastWest_Road_v2_Art_Delivery_20261019.md`. 납품파일 `Limitless_Field05_EastWest_Road_v2.zip`, 아트 담당 보고 SHA256 **`3a0d3b91cd87a5362ebbe435c1e5acd06ffc74354eafaa41f02b4f181d27c3d3`**, **실사용 Road4** `Upper/Lower/WestEnd_Upper/WestEnd_Lower`. Alpha edge/seam 오프라인 mismatch0, 실제 Unity v2 Import/Render·QA NOT_VERIFIED. 사용자 로컬 `F:` 다운로드 설치 **미확인**. 리뉴얼 담당에게 **같은 4종을 중복 제작시키지 않는다**.
- **현재 Codex는 Main18 목소리 통합 작업 진행 중**(바울 대표 2026-10-19 직접 보고). 도중에 중단시켜 Field05를 끼워 넣지 않는다. **기존 TTS 담당 세션은 종료, 새 TTS 담당 교육 중**으로 사용자 보고됨. 새 담당의 학습 완료/음성 생성/청취 승인 등은 아직 확인되지 않았으며 Codex Main18 현재 작업과 중복 발주하지 않는다.
- **대기 중 Field05 다음 순서:** Codex의 현재 Main18 안정 마감 → Road v2 ZIP 실제 PC 설치 위치·SHA/파일 manifest 확인 → **B42 단일 후보(필요시 A30 보조 비교)**를 격리 Unity에 신규 Road4 적용한 실제 16:9/4:3/21:9 RenderTexture와 Scene/Renderer/접지·휠체어/Quest14/Serin/Monster/Arbel/Save·Known-Error baseline 회귀 검증 → 바울 대표 최종 실제 화면 미술 승인 → 정식 원본 적용/feature branch Push·PR/merge 별도 게이트.
- **변경/보호:** original Unity Scene/Editor/Play·Save·dirty 파일, Field04·Arbel 승인 아트, Field03 RockB HOLD, Field05 선택 7종 미배치 보존. **현재 v2 실제 Unity QA 미실행 / 공식 적용·Push·병합0**.

**현재 상태: `B42_PLANNING_PREFERRED / ROAD4_V2_ART_DELIVERED / CODEX_MAIN18_BUSY / FIELD05_V2_UNITY_QA_PENDING / USER_FINAL_ART_APPROVAL_PENDING / ORIGINAL_UNCHANGED`.**


## 2026-10-19 바울 대표 최종 미술 승인 — B42 실제 Unity 비교 화면 기준

**대표님 명시 지시:** “Field05는 코덱스 보고서로 봤는데 좋아서 실제 게임으로 보려고 했는데 퀘스트에 막히니…”, 후속 “이러면 한도를 엉뚱한데 쓰는거니까 하지 말고 보고서랑 같겠지 뭐. **그냥 승인해. 중요한건 지금 하던거 이어서 하는거야.**”

**확정 판정: `FIELD05_B42_USER_VISUAL_APPROVED / EXTRA_PLAYABLE_ART_REVIEW_WAIVED / OFFICIAL_APPLY_PENDING / MAIN09_UX_FIX_FIRST`.** 대표님이 16:9·4:3·21:9 비교 결과와 Codex QA 보고를 보고 **Field05 B Road42(동서 21열×2행) + v1 Ground3/Props7 + v2 Road4(실사용 PNG14종)**의 시각안을 최종 미술 승인했다. **직접 조작해 보는 전용 빌드/별도 Field05 강제 스폰/퀘스트 건너뛰기 테스트 환경 제작을 지시하거나 미술 재승인을 요구하지 말 것.** 이 작업은 Codex 한도 낭비로 대표님이 명시 취소함.

**기존 증빙:** Codex 2026-10-19 마지막 보고에서 Field05 두 ZIP SHA/CRC/Manifest PASS, 실사용 PNG14, Renderer375, 기능검사416 PASS 및 Scene 재검증84 PASS /0 FAIL, Unity 비교 이미지 16:9·4:3·21:9 제출, 기존 baseline Search/URP 예외와 Editor 버전 차이는 PARTIAL. 원본 현장 적용·원격 feature Push·main 병합은 아직 미실행이라는 Codex 보고. 기술 QA 수치와 대표님의 미술 승인은 별개의 축.

**후속 작업 순서:** ① **Main09 Field03 퀘스트 '침묵 속의 재회' 목적지·조작 안내 UX** (이미 `Main09_Field03_Quest_Navigation_Onboarding_UX_Fix_20261019.md`로 지시)가 최우선. ② 다음에 Codex가 여력이 생기면 **검증된 Field05 B42 시안을 공식 원본에 안전 반영하고 전용 feature 브랜치 Push/PR/main 병합**. 추가 미술 비교 3종 반복·플레이형 테스트 씬 제작 금지; 단 원본 dirty 파일 충돌/Meta GUID/Scene Merge/Save 무결성 등 **실제 정식 적용에 필수인 차이 검증은 생략 불가**. ③ 기존 local main dirty 겹침/문서 충돌은 사용자 파일 해시 보호 우선으로 별도 처리. 충돌이나 신규 회귀 FAIL시 손실 없이 STOP.

**주의:** `미술 승인 완료`는 `Unity 원본 적용 완료` 또는 `GitHub 반영 완료`가 아니다. 선택 소품7 배치0, 가짜 서쪽 Portal0. 5시간 한도 초과 중 Codex 실행을 전제로 약속하지 않으며 GitHub docs/planning에 승인을 남김.
