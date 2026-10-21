# LIMITLESS — 2026-10-21 Main23 서릿길 마을 기술 사전점검 완료 및 스킬 장착 영구저장 게이트

- 관리 브랜치: `docs/planning`. 바울 대표가 Codex 보고 이미지를 공유함. **이 보고는 Codex 격리 작업 결과이며 기획세션이 원본 Unity를 직접 실행해 확인한 결과는 아님.**
- 담당 브랜치 `codex/chapter3-main23-village-preflight-20261021`, 최종 커밋 **`74bd1de8558244405a2935e5fca96fcc2f0bb307`** (사용자 첨부 스크린샷 기준).
- `Main23` 관련 [Issue #45](https://github.com/HappyPaul812/Project-Limitless/issues/45), Scene/Quest/Dialogue/ID 기존 정본 [Issue #36](https://github.com/HappyPaul812/Project-Limitless/issues/36).
- 근거 설계 `20261021_Chapter3_Main23_Frostway_Village_Art_And_Tech_Parallel_Kickoff.md`, `Chapter3_서릿길마을_상세마스터플랜_v1_20261019.md`, `Chapter3_Main23_24_서릿길마을_스토리시퀀스_v1_20261019.md`.

## 1. 보고된 실제 사전점검 결과

- Main22 완료 후 Main23 사용 가능 상태 해금, Main23 **후보5목표**, F1 북쪽→서릿길 마을 남문 연결, Save/Continue·SafeZone 체크포인트 및 기존 서비스 재사용 방향 기술 계약 완료.
- 마을 북문→F2는 아직 막아둠; 동쪽 미래 포탈은 공간만 예약·이동 기능 비활성. Main23 시점에서 F2 첫 한기, 스킬 강제구입·신규 지정전투 만들지 않음.
- Codex가 임시로 관리 중인 **예약 ID 20개는 현재 Main22 기존 정의와 충돌 없음**으로 보고. 이는 **정본ID 고정 완료/상품ID 결정/기존 전역전체 등록 완료를 뜻하지 않음**. 최종 Scene/Quest/NPC/Skill/Item 별 기존카탈로그 대조와 진짜 Unity 등록 전 다시 유일성 확인.
- 기존 Main22 격리 QA136PASS, F1 Unity64PASS, Mouse Battle40PASS **재사용**, 전수QA 반복하지 않음. 이번 preflight는 Scene/Quest/NPC 실구현·미술 Import 없음. **기획 및 기술 계약 준비완료(`PREFLIGHT_READY`), 첫 플레이 구현은 아직 미착수**.
- 사용자 원본 4,351개 파일 SHA 및 Git status 보호, Editor 조작·Push·main 병합 없음.

## 2. 현재 독립 업무와 블로커

- `docs/art-new`: Main23 마을 Art Pilot v2 실제 환경 PNG 납품을 사용자 지시로 이미 수행 중. **다시 발주 금지**. 기존 Village4는 포맷QA 후보이며 미술 최종 승인 아니고, 승인 F1 환경6 PNG는 원본 보존. 마을 실제 PNG·배치/치수·건물/타일 접합·NPC 공간·동쪽 비작동 포탈 부지가 사용자 미술 승인 후 격리 Unity GameView QA 대상.
- `docs/planning`: Quest/Scene/NPC 최종 ID 잠금은 실제 catalog·feature branch와 대조해 순차 마감. 지역 상점의 **실제 상품·가격·보상/정식 대사·TTS**는 미정. 신규 스킬/강화 스킬/아이템 아이콘 및 한기 대응 따끈 어묵탕·잡템 Sprite 공급 누락은 내부 대장에 계속 관리.
- `codex/*`: **핵심 기술 리스크 = 기존 스킬 장착 상태 메모리 유지, 영구 Save/Continue 복원 미구현**. 이는 Main23의 **안전지역/마을 최초 소개 자체의 완료를 막는 기능은 아니지만**, 실제 구매한 스킬의 장착 상태 유지 및 Main24 스킬상점 정식 오픈을 막을 수 있는 기능적 결함. 메모리 전용인 현재 상태를 실사용 '상점 완료'로 표기 금지. 기존 저장 포맷/캐릭터별 장착6슬롯/기본 스킬3/강화 덮어쓰기/해금 탈렌트와 불일치가 생기지 않도록 **SaveService schema migration·구버전 세이브 하위호환** 설계 후 Codex 별도 구현·QA.
- 기술 우선순위: ① 이미 수행 중인 Main22 적용 준비와 Wolf Run QA 별도 유지(새로 중복 시키지 않음) ② Art Pilot v2 납품/대표 미술 승인 ③ 승인 마을 Art + 기술 계약 → 격리 Unity 실제 GameView/배치·휠체어 동선·문·체크포인트 QA ④ Main23 5목표 구현 ⑤ Main24 스킬상점 정식 기능 전에 **장착 영구저장/로드 회귀 PASS**. 단계④와 ⑤는 병렬 가능한 범위별 작업으로 분리.
- 수정 불가 범위: Main22/서리들쥐/승인 F1·Chapter1/2·Field09 B Glow·Save/UserData/Editor 원본/dirty/untracked 변경 없음. 임시 ID는 Scene·Quest·NPC와 충돌확인할 때만 lock; 허가없는 main 직접 Push/PR병합 금지.

## 3. 이번 현재 결론

`MAIN23_TECH_PREFLIGHT_COMPLETE / RESERVED_ID20_NO_CONFLICT_WITH_KNOWN_MAIN22 / VILLAGE_ART_DELIVERY_PENDING / VILLAGE_UNITY_NOT_BUILT / SKILL_EQUIPMENT_PERSISTENCE_BLOCKER_FOR_MAIN24_SHOP / ORIGINAL_UNITY_UNCHANGED / MAIN_NOT_MERGED`.

바울 대표에게 장황한 아이콘 자산 우선순위표나 동일 Codex 프리플라이트 지시문을 다시 보낼 필요 없음. 담당별 새 산출물을 받은 이후 딱 필요한 변경분 검수만 발주.
