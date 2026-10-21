# LIMITLESS — Chapter3 Main23 서릿길 마을 첫 안전거점 병렬 제작 착수 (2026-10-21)

작성 브랜치: docs/planning. 바울 대표 지시: "챕터3 다음꺼 시키자."

## 확인된 이전 결과 및 중복 금지
- Main22 「북풍의 징후」 격리 Unity 5목표 구현 136 PASS / 0 FAIL, 기능 커밋 be7f64a3ff48a8848471166d4aefc4969080bd8b, 사용자 “잘된다” 긍정. 실제 원본 Unity 및 remote main에는 아직 반영되지 않음. 직전 Codex에 Main22 적용 준비 + 리뉴얼 Wolf Run 후보 QA를 이미 지시함. 작업 중복 금지.
- F1 실제 Unity 64 PASS / 0 FAIL, 서리들쥐 실제 Battle 40 PASS / 0 FAIL 재사용. Chapter3 신규 아트는 docs/art-new, Chapter1·2 Wolf 펫은 docs/art-renewal. 사용자가 직접 정리한 F: 다운로드 위치 추측 금지.

## Main23 기획 정본
- Main23 「따뜻한 땅을 떠난 자들」: F1 심부에서 남하 동물 흔적 2가지 조사 → 북쪽 서릿길 마을 남문에 도착 → 주민의 현상 증언 및 태온/하렌의 판단 장면 → 안전거점 Checkpoint/Save 등록 → 자유 탐색과 Main23 완료. F2 첫눈 및 최초 한기는 Main24, Main23 설원족제비 별도 지정 전투는 아직 보류(새 몬스터 아트/통합 없으므로 이번에 강제 추가 금지).
- 실제 거점 설계 정본: 문서/03_스토리/Chapter3_서릿길마을_상세마스터플랜_v1_20261019.md, Chapter3_Main23_24_서릿길마을_스토리시퀀스_v1_20261019.md, Chapter3_서릿길마을_NPC_서비스_서브퀘스트_v1_20261019.md.
- 마을 정체성: 따뜻한 목재·회갈색 돌기초·어두운 지붕·일부 얇은 서리·장작과 생활감, 새하얀 설원 금지. 남문 F1 / 북문 F2(현재 비개통), 중앙 스킬상점·보급점·여관·파티/펫 서비스 구역, 동쪽 미래 순간이동 7×7 WU 후보 빈 광장. 포탈은 현재 완전 비작동, 텔레포트 SceneTransition / VFX / 타깃 / 자동 활성화 금지. 후보 공간예산: 마을31×21 타일, 남북 주요길 3 WU, 포탈광장7×7 WU. 수치는 실제 Scene 물리계약 전 잠정값.
- 역할 NPC 5 가칭(오르나, 루덴, 마레, 도아, 베른) + 기존 펫 분양/관리 서비스2는 NPC ID/실제 인물 수·음성 확정이 아님. 스킬은 탈렌트 구매, 전투 외 장착/교체 무료, 최초 한기 대응 어묵탕 공급은 Main24 전 확보 규칙; 실제 가격/상품ID/아이콘은 미확정. 이번 Main23에서 가짜 완성 UI 만들지 않는다.
- 상점 구매·펫 구매, 스킬 교체를 Main23 필수 Quest 조건으로 하지 않는다. 기존 3인 파티·Save 정책, 태온/하렌 서사 및 메인23 시점 보스 진상 비공개 보호.

## 작업 A — 신규 메인 아트 담당 docs/art-new 즉시 제작
- 기존 Chapter3_ArtNew_F1_Frostway_Pilot_v1.zip의 Village4 PNG는 오프라인 파일QA만 PASS, 미술 최종승인/실제 GameView 없음. 기준으로 비교하되 4개만 최종 마을 미술 합격으로 주장하지 않음. 기존 원본 SHA 보존.
- 제작: 서릿길 마을 최초 플레이용 실물 Environment Art Pilot v2 PNG 세트. 필요한 역할: 지면(자갈흙·돌길·경계/얇은서리)3, 목석 집/지붕/문·상점/여관 배치 가능한 building 프레임2종 이상, 장작/보급상자·랜턴 등 생활소품2종 이상, 남북 방향표지/스킬·일반상점 간판2종 이상, 미래포탈 완전 비작동 돌기단·빈 부지용 바닥1종 이상. 정확 PNG 수/PPU/TileGrid는 Codex 확정 전 ArtPilot 후보이며 임의 강제 규격화하지 않는다.
- Chapter2 공식 맵·승인 F1 6PNG와 동일 게임 배율로 질감·건물/지붕/경계/소품을 실제 비교. 얕은 서리로 F1→마을 연결, 2×2 타일 반복 seam, 걸어서·휠체어 남북 통로와 각 서비스/미래 포탈 접근 가능성(예비 시각 가이드) 확인.
- 16:9/4:3/21:9 합성 마을 레이아웃 그림, PNG 실물/각SHA, Manifest/CRC, v1과 미술 차이/중복패턴 리포트, 마을 실제 입구·서비스·동쪽 비작동 포탈 광장 간선 안내. 오프라인 합성은 Unity GameView 아님 명시. 실제 ZIP 납품 및 docs/art-new 브랜치/commit 기록.
- 사용자/기획이 미술 승인 전 Unity 정식 적용/신규 NPC portrait/SkillItemIcon 전부까지 확대 제작 금지. 원본 Main22, 서리들쥐, F1 승인본, Chapter1·2 및 원본 Editor/Save 보호.

## 작업 B — Codex 별도 Main23 기술계약 사전 점검
- 현재 Codex Main22 원본 통합 준비 + WolfRun QA와 충돌하지 않을 별도 worktree/branch codex/chapter3-main23-village-preflight-20261021. 기존 Main22 feature branch/main 차이, Scene/ID/Quest/World Bounds/Camera/PPU·Collider/Save/Checkpoint, SafeZone/상점·파티·펫서비스 실제 code/asset 리소스 감사. 이미 확인한 F164/Mouse40/Main22 136 전수 QA 반복 없음.
- 실제 Scene/Spawn/Quest ID 충돌 후보표 및 남문 F1↔마을, 북문 F2 미래 문(비개통), 동쪽 포탈 광장(비작동)과 돌기단/Spawn 후보 구획, 폭3WU 길·카메라3비율·E마우스 NPC 선택 우선순위/상점 문앞 포켓 기술 계약 산출. Main22 완료→Main23 Available, Main23 5개 후보 목표 및 Main24 Available 해금/Save 보존 계약.
- Main23 Quest 5 목표 후보: ①F1 안쪽 이동 ②동물·장작 흔적2 조사 ③마을 남문 진입 ④주민의 기온·남하 증언 및 태온/하렌 대화 ⑤체크포인트 등록/자유 탐색 뒤 완료. Main23 신규 족제비 지정전투·F2 한기·소모템/SkillShop 가격은 이번에 구현하지 않는다.
- 이번 Codex는 읽기 전용 기술 계약/문서만. Main23 실제 Scene/Quest Script나 User원본 Game/Editor/Save·Push/main merge 수정 금지. 명확한 제품구현은 Village 미술 승인 및 별도 구현 지시 이후.
- 보고: 브랜치/commit, 기술 계약/정식·임시 IDs 구분, 기존 Main22 의존관계, Asset 부족 목록, Save/원본 보호, 본편 미적용 상태.

## 다음 게이트 및 팀 자산대장
- 1차: ArtPilot v2 실 PNG/비교샘플+Codex 마을 Scene 기술계약 → 바울 대표 미술 승인.
- 2차: Codex 격리 실제 마을 GameView/통행/서비스 QA → Main23 5목표 first playable → 바울 대표 직접 플레이 → 별도 원본 동기화/PR 통합.
- 내부 관리(사용자에게 불필요한 대장 반복 금지): 신규/강화 스킬 아이콘, 따끈 어묵탕·냉기 대응 소모품, 몬스터 잡템/인벤토리/상점 아이콘 공급계약. 고유 아이템ID/가격·판매카탈로그 임의 확정 금지.
