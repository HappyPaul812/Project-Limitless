# LIMITLESS / 2026-10-23 Chapter3 후속 — Main23 실제 화면 완성도 + Main24 Skill Save 선행설계

바울 대표 지시 “됐어 지금 열심히 일 하고 있어. 넵둬. 우리는 챕터 3 계속하자.”에 따라 **리뉴얼 담당 작업은 건드리지 않는다**. 기존 `docs/art-renewal` 지시·이슈는 변경·재발주·진척독촉하지 않음. 최신 공식 `main`은 `47ad2eecb40135b2ee28e4947d642ebdbf0bf68a` (10/22 EOD), 다음 Codex 실제 작업 시작 전 HEAD 재확인.

## 1. 현재 성공의 정확한 범위
- 바울 대표 제공 Codex 실제 결과: Main23 첫 플레이 **신규 QA 148 PASS / 최종 FAIL 0**. 다섯 목표 F1 심부→서로 다른 동물/짐수레 흔적→서릿길 남문→주민 증언·태온/하렌 대화→SafeZone/Checkpoint 등록 및 완료. Save/Continue, 남문 F1↔Village, 패배 복귀, 키보드/가상 마우스/휠체어 경로, 3화면비 격리 Unity 검증.
- Codex 로컬 격리 `codex/chapter3-main23-first-playable-20261023`, 제품 단축SHA `dcf9d6af`, 보고SHA `2979e327`; **GitHub Push/main merge/원본 제품경로 적용 0**. 사용자 실제 Editor 자연 Import·손플레이 아직 미검증, 기존 Search 환경 예외 OPEN.
- 사용자 최종 승인 지면 **FrostwayVillageV10 Sprite9/Tile9**는 이미 main 등록. 그러나 `World_FrostwayVillage.unity`는 main에 없고 격리 코드에만 Main23 구현. Chapter3 마을 기반 게임아트 **기존 43(+Variant6)의 건물·지붕·문·간판·소품**은 기술/미술 일부 PASS이지만 **전체 후보/서릿길 마을 정식 환경 미술 승인 완료 아님**. 기존 ZIP `F:\Downloads\Limitless_Chapter3_FrostwayVillage_Environment_GameArt_TargetedFix_v5.zip`에는 43 게임 PNG, 건물5/모듈9/소품14/표지6/비활성 포탈3/Transitions2/Tiles4가 있음. v10 승인9는 별도, 이 ZIP의 땅3개를 도로 격상하지 말 것.
- OPEN16 유지16/해결0; 원본 CSV·좌표 JSON은 Codex 로컬에 있으나 담당에게 원문 인계가 필요. 불명확한 16개를 상상하여 스스로 해결했다고 보고 금지.

## 2. 오늘 Code Phase: Main23 '시각/접근성 플레이 패스' — 격리 임시 마을 화면 개선

- 로컬 기존 Main23 격리 worktree를 **재사용**. 새 전체 구현/148 건 재검증 금지. 승인 지면9 **불변**.
- 현재 임시 텍스트 표지·건물 박스·NPC 임시 표시와 실제 Chapter3 v5 기존 개별 PNG43의 **역할↔Renderer 맵**을 대조. 건물·문·지붕·상점 표지·장작·랜턴·벤치·미래 포탈 비활성 돌기단 중 실제 사용가능한 **기존 후보만 격리 씬에서 배치/시연**, `ART_CANDIDATE_UNAPPROVED`로 태깅. 아트 부족은 빈 곳을 가리려고 코드가 가짜 Sprite·새 자산을 생성하지 말고 **정확한 부족PNG, 규격, Pivot, Sorting, 건물 연결 좌표**로 기록하여 `docs/art-new`에 넘긴다.
- 승인 지면과 기존 개별 후보의 PPU/Pivot/색감/스케일(256×256 tile), 건물문·지붕/Collider, 동·서 보조길 및 남북 문, F1 사전 쉼터에서 마을 안내 및 마을 도착 경로, 16:9/4:3/21:9 실제 Camera/RenderTexture·같은배율 전후. NPC 원화는 새로운 의존성. NPC 사용 가능 원본은 임시일 경우 화면 내 `시안` 표시와 Scene만, 제품 main 적용 금지.
- Quest UX: 플레이어가 **다음 조사 위치/상호작용 방법**을 모르면 막힘. 실제 5목표 매 단계 텍스트 목표, 지도/마커+선택 타깃, 마우스 클릭 후 이동·E/F 및 취소, 단계 완수시 UI 갱신, 중복보상 0, 스토리 진행 시 남문/저장 안내. 접근성: 큰 글씨·충분한 명암, 색/음성만으로 필수 정보 전달 금지, 자동 클릭대체/손발 조작을 방해하는 타이머 금지. 컨트롤 원래 매핑 유지, UI 전체 리뉴얼/입력 리팩터 금지.
- SafeZone과 기존 펫·파티 NPC의 기능과 판매 예고는 구분. Main24 신규/강화 스킬상점과 방한용 어묵탕 **실판매·아이템 사용** 구현·자동결제 금지. 북문 F2 막힘 안내, 포탈 동쪽 돌기단은 비작동·Quest마커/Trigger/Teleport0, 파티3인 기존 정책.
- 기존 Main23 148 PASS를 기준으로 **변경된 시각·입력·Collider·씬표시만 Delta QA**. 원본 Editor/Save/UserData/dirty/HEAD/index 보호, main 직접 commit/Push/merge 금지, 제품디스크 반영은 사용자 실제 시각확인 후 별도 지시.
- 납품: 실제 Unity 3비율 Canvas+Camera/RenderTexture(오프라인 조합과 구분), F1→Village 5목표 한 화면 흐름/다음목표 읽힘, 실제 임시·후보·승인 Sprite 분류, **부족아트 파일당 우선순위/스펙** `Main23_Frostway_VisualGapList.csv`, PlayMode PASS/FAIL 새 항목만, 브랜치·커밋·원본보호 증거.

## 3. 동시 진행: 신규 아트 표적 샘플 (리뉴얼이 아니라 Chapter3 담당 `docs/art-new`)

- 기존 `v5` 아트43을 재생성/일괄리뉴얼하지 말고, **현재 격리 Main23 화면과 v5 아트 후보 기준으로 실제 실패 또는 미흡한 지점만** 새 시안.
- 1차 아트 파일럿은 ① 남문 중심 안내/주민 증언 NPC **오르나 1종**(가칭 이름/VoiceID 미확정), ② 마을 분위기를 좌우하는 **목석 주택 정면·문/지붕 접합1세트**, ③ 스킬상점/여관 중 **임시 박스가 눈에 잘 띄는 시설의 입면/간판1세트** 이내. Codex VisualGapList에서 우선순위가 확인되면 실제 구조에 맞게 교체. 승인 Earth/ThinFrost/Gravel9 Sprite 수정 금지.
- 각각 독립 투명 PNG, 원본 v5 비교·실제 고정 PPU/Pivot·128px/40px/다른 3비율 조립 이미지, SHA256/Manifest/CRC/역할표를 제공. 가짜 Unity GameView 주장 금지. 특히 북문/F2 비개통·동쪽 포탈 비작동/7×7WU 빈 공간 보존, 남북길3WU 및 휠체어 동선 확보.
- 아트는 사용자 미술승인 전 격리 파일럿/시안으로만, Unity 본게임 등록·main 병합 없음. 제작 중 43종 전체 재작업 금지.

## 4. 다음 챕터 준비 — Main24 6스킬 슬롯 Save 계약 강화 (Codex read-only preflight만)

- 기존 [Issue #46](https://github.com/HappyPaul812/Project-Limitless/issues/46)은 `SkillLoadoutService` 6슬롯 구성 메모리전용, Save/종료/Continue 영속화 **미구현**. Main24 최초 스킬상점 결제 기능의 기술 게이트. 이제 본편 구현을 시작하기 전 **차후 독립 worktree에서 실제 DTO/SaveService·6슬롯·신규구매/강화·Talents 소비 구조**를 읽기 전용으로 감사하고 기존 저장과 호환되는 필드 이름/버전·마이그레이션·소유권·중복장착·강화 슬롯승계·손상 세이브 fallback 계약 및 최소 QA 케이스 작성. **현재 Main24 스킬 판매·영구 Save 코드 구현이나 주어진 6칸을 무단 main에 병합 금지**, 후속 구현 지시는 이 설계 근거로 별도 부여.
- Chapter3 진행 선후: Main23 시각/프레젠테이션 QA → 바울 대표 플레이/미술 피드백 → Main23 원본 제품 반영+PR 승인; 별도 Main24 Save 계약 → 스킬영속 코드 → Main24 상점+F2 첫 한기2턴 대처 테스트. 메인23 완성을 Main24 상점 구현 때문에 묶어두지 않는다.

## 5. 역할/규칙
- Codex 구현·격리 Unity·VisualGap 증거/고유 SHA 보호. 신규 아트는 `docs/art-new` 독립 시안. **리뉴얼은 사용자 지시 따라 그대로 두고 추가 연락/재발주/Issue변경하지 않는다.**
- `docs/planning`만 기획 본계약/인계 갱신, `main` 직접 commit/Push/merge 금지. 모든 이슈·작업물에 승인 vs QA권고, 격리 vs 본게임, placeholder vs 정식 공식 원화/런타임 구분.
