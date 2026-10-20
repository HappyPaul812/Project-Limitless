# LIMITLESS — Chapter3 팀별 병렬 제작 착수 및 승인 게이트 (2026-10-20)

- 기획 브랜치: docs/planning. 바울 대표: “챕터3는 설계 끝이면 각 담당한테 작업 맡겨야지?”
- 상태: CH3_PARALLEL_PREPRODUCTION_AUTHORIZED / MAIN22_24_ID_LOCK_PENDING / USER_ART_APPROVAL_PENDING / CH3_FULL_RUNTIME_NOT_AUTHORIZED.
- 본 문서는 팀별 선행 파일럿 제작·기술 검토 작업의 공식 발주다. 전체 Chapter3 Unity 구현, 최종 음성 제작, 정식 PR/main 병합 허가가 아니다.

## 확정 골격과 미정 세부

- 기준 GitHub docs/planning: 문서/03_스토리/Chapter3_Main22_29_북부_온기의화로_스토리구성안_20261014.md, Chapter3_Main22_북풍의징후_상세기획_v1_20261014.md, Chapter3_Main23_24_서릿길마을_스토리시퀀스_v1_20261019.md, Chapter3_서릿길마을_상세마스터플랜_v1_20261019.md, 문서/08_몬스터/Chapter3_설빙의폭군_온기의화로_북부몬스터_상세기획_20261014.md, 문서/06_스킬/Chapter3_스킬상점_강화_냉기_동결괴사_20261012.md.
- 최신 연결: Chapter2 Main21 완료 → 초보 마을 펫 서비스 자유이용 → 자발적 북진 → Main22 F1 북풍과 서리들쥐 첫 지정 전투 → Main23 F1 조사와 첫 신규 안전지대 서릿길 마을 도착 → Main24 마을 스킬상점/한기 대응 공급 후 F2 첫눈·서리올빼미. 오래된 “Main23 F2에서 바로 종료” 구성은 최신 마을 설계로 대체.
- 북부 일반 필드 F1~F5, 보스전 1별도, 마을은 필드 수에 포함하지 않음. 몬스터 일반5(서리들쥐·설원족제비·서리올빼미·얼음갑충·눈보라 정령), 엘리트1(빙각 수호체), 보스1(설빙의 폭군). 보스 이름·웬디고 모티브·약해지는 단 하나의 마법 온기의 화로, 태온↔하렌 형제애와 마지막 “잘.했.어.”는 기존 사용자 정본.
- 한기2턴 행동불가, 동상=행동불가+HP감소, 동결괴사는 보스전 전용 대상3스택 전투불능(영구 저장삭제 아님), 3인 전투와 회피불가 소프트락 금지. 화로는 보스 전투의 대응 행동이며 일반 마을 모닥불로 복제 금지.
- 스킬: 직업별 기존3 무료·신규2와 강화판 탈렌트 구매·전투 최대6슬롯·비전투 무료 교체·강화판 기존 스킬 대체, 북부 마을 첫 스킬 구매처/Chapter3 완료 후 전국 안전지대로 확대. 구체 가격·효과 수치·ID 미정.
- 서릿길 마을(이름 가칭): 남문 F1, 북문 F2, 동쪽 장래 포탈용 빈 공간/안전 도착 위치 예약. 포탈 이동 기능/활성 FX/월드 트리거는 이번 구현 제외. NPC7안/서브퀘스트3안은 역할 기획이며 실제 사람 이름·경제·정식 Dialogue/Quest ID는 미확정.
- 정식 Scene/Quest/Dialogue/Monster/Skill/Item ID, 북부 맵 실제 좌표, 경제 수치, 장면별 본문, 보스 TurnOrder는 아직 production lock 전. 시안 제작의 승인과 게임 정식 적용 승인을 혼동하지 않음.

## 팀별 지금 할 일

### 기획 — docs/planning
- Main22~24 Production Lock 후보 문서 작성: Main21 펫 해금/자발적 출발, Main22 F1 5목표, Main23 마을 안전지대 개방, Main24 상점 자유 이용/한기 대응/북문 F2 첫 지정 전투. Quest/Dialogue/Scene/NPC/Monster IDs는 우선 후보로 만들고 Codex 충돌검증 뒤 확정.
- Main23 NPC7·서브퀘스트3의 필수/선택을 구분하고 가격·경제·레벨은 Chapter2 완료 Save/Balance 감사 뒤 결정.
- TTS 준비용 Main22~24 화자·원문·대사 순서·stable ID·페이지 경계를 확정하는 정식 대본 초안 준비, 대표 확인 뒤 SCRIPT_LOCK.
- F3~F5·보스 턴큐·최종 대사 계약은 후속 별도 게이트. 본 기획 문서로 자동 승인하지 않음.

### Chapter3 신규 아트 — docs/art-new
- F1 북풍의 들머리: 흙→얇은 서리 지면/전이/경로용 실제 PNG 소규모 파일럿. 초보 마을에 돌연 눈밭 금지.
- Main23 서릿길 마을: 생활감 있는 따뜻한 목재·돌기초 건축 소품, 남북 간선과 비작동 동쪽 포탈 광장 공간을 보여 주는 시안 및 소규모 독립 PNG. 코덱스의 Scene PPU/Pivot/Collider 계약을 먼저 반영.
- 몬스터 서리들쥐 1포즈 Key Art, 보스 설빙의 폭군 1포즈 실루엣 콘셉트까지 우선 시안; 몬스터7종 전체 애니메이션/NPC 대량 양산은 미술 승인 뒤. 기존 하렌/태온/폴/미엘/세린 및 Chapter1·2 공식 아트 수정 금지.
- 실제 PNG/별도 ZIP·Manifest SHA·출처·오프라인 QA·미술 승인 대기 보고를 docs/art-new에 기록. 합성은 Unity 실화면으로 주장하지 않음.

### BGM — docs/bgm
- Chapter3 첫 샘플 두 곡: F1 약한 북풍/서리와 조사 긴장, Main23 서릿길 마을의 주민 생활·아직 남아 있는 온기. 서로 다른 오리지널 짧은 데모(권장 약 45~90초)/루프 설계·레벨과 오디오 간섭/권리/SHA 보고.
- 최종 설빙의 폭군 보스 음악은 짧은 분위기·큐 시안까지 허용, 정식 전투 패턴과 주요 연출 잠금 전 전체 사운드트랙 자동 완성 금지. 기존 Chapter2 음악·TTS 대비 과한 음량 금지. Unity 적용 별도.

### TTS — docs/tts
- 기존 인물 음색 정책/대사 오류 복구 이력 먼저 확인. Main22~24 신설 주민/화자 후보 캐스팅표와 단가·한도/제작 큐, 정확한 DialogueId·화자·문장·WAV 경계별 감사 준비.
- 정식 대사 및 ID는 아직 미잠금이므로 신규 TTS/API 호출0, 원문 없는 WAV 생성0. Main20/21 기존 등록 대기 22개와 작업 자산 보호. 확정 SCRIPT_LOCK 뒤 첫 배치만 생성하고 사용자 청취 승인 후 Unity Catalog 적용.

### SFX — docs/sfx
- 소규모 청취 파일럿 2~3개: 약한 북풍/서리 발소리/마을 온기 분위기 중 선택, 냉기 경고 및 화로 사용 효과는 사운드 큐 콘셉트 우선. 음성/자막 없는 필수 정보 전달 금지.
- 과도한 음량/연속 피크, 강제 점멸·청각 의존 금지. 오리지널·권리·루프 및 파일 QA docs/sfx 보고, Unity 자동 Import 안 함.

### 개발 영상 — docs/video
- Chapter2 실제 구현 화면과 Chapter3 북부 기획·콘셉트·미구현 시안을 명확히 구분한 개발기록의 챕터3 준비 편 스토리보드/샷리스트 작성. 아트 승인·Unity 실화면 확보 이전에 새 게임플레이처럼 영상 조작/배포 금지. 더빙이 묻히지 않게 BGM·자막 기준 포함.

### Codex — codex/chapter3-tech-preflight-20261020
- 지금은 읽기 전용 기술 감사만. 최신 main과 원본 local/사용자 dirty·UserData·Save 보호; 원본 Editor/Play/포커스 무조작.
- Main21 Quest asset의 nextMainQuestId 빈 값과 Main22 Available 연결 방식, Quest/Actor/Scene ID 충돌, 북부 신규 F1 Field/Spawn/Bounds/Collider/휠체어, 서릿길 마을 SafeZone 및 비작동 미래 포탈 안전구역, 기존 펫 해금, 스킬6슬롯 및 영속 구매·강화 계약, 냉기/화로 TurnOrder 갭 분석. 현재 Unity 프로젝트 원본 신규 Scene/Quest/Audio 구현 금지.
- 기술 규격/ID 후보·충돌/의존도·실제 PPU/Pivot·Asset Import 계약을 한 장씩 분리 보고. Main22 정식 ID/Dialogue ScriptLock 후만 별도 feature/worktree로 Main22 F1 vertical slice 1개부터 구현과 변경분 QA.

### Chapter1·2 리뉴얼 — docs/art-renewal
- 기존 Field09 신규 샘플3 미술 납품·Codex 검수/Slab 역할 확인, VenomSnake/Wolf 품질 보류 작업 유지. Chapter3 신규 월드/몬스터/보스는 전담 docs/art-new 관할이므로 중복 발주 금지.

## 진행 단계/보고
1. 즉시 병렬: 기획 ID/대본 작업 + Codex 읽기 감사 + 신규 아트 F1/마을 PNG·KeyArt + BGM 두 데모 + SFX 소형 파일럿 + TTS 준비 + 영상 스토리보드.
2. 첫 승인 게이트: ID/PPU 등 기술 계약, Main22~24 Script Lock, 파일럿 미술·음악 검수. TTS 신규 생성과 Codex Main22 기능 구현은 이 때 별도 허가/범위로 착수.
3. 이후 Main23 새 마을·서비스, Main24 한기 첫 전투 순으로 각각 안전 적용/검증. Main25~29 대사/보스 턴공정성 확정 전 대규모 구현 금지.
4. 담당 브랜치 docs/planning, docs/art-new, docs/tts, docs/bgm, docs/sfx, docs/video 및 codex/*만 사용. main 직접 commit/Push/강제 merge 금지, 원본 승인 자산/Save 보호.
5. 납품 보고에는 브랜치·커밋·실제 파일/ZIP/SHA·오프라인 QA·사용자 승인 여부·Unity 미적용 여부·후임 세션 인계를 구분. 담당 세션들은 자동으로 메시지를 전달받지 않으므로 이 문서와 사용자 전달용 지시문을 확인해야 시작 가능.
