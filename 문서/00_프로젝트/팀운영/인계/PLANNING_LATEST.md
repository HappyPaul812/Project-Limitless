## 2026-10-19 문서5 의미 병합 및 보호 경계

로컬 작업복사·local main 커밋·공통 조상·최신 원격 main을 대조했습니다. 최신 Main09(PR21/24)·Field05(PR22/23)·Main18(PR18/19)의 병합·원본 디스크 반영은 완료 상태로 유지합니다. 과거 기록의 승인 대기/미구현은 과거 당시로 구분합니다. 기획·리뉴얼 담당의 원격 전용 기록도 삭제하지 않았습니다.

충돌 원본 네 버전과 SHA256·줄바꿈·시각은 로컬 `Temp/Doc5Recovery20261019/Versions`, 전체 dirty·미추적·UserData·Settings 백업은 `Backup`/`BackupManifest.json`에 보존했습니다. 문서 정합과 원본 Git 동기화는 별개이며, HEAD/index/작업파일 동기화는 격리 리허설 후 실제 확인 결과로만 선언합니다. 원본 Editor 자연 Import·손플레이는 미검증이며 기존 Search/URP 환경 OPEN을 유지합니다.

# PLANNING 인계 — 2026-10-14 야간 음성007~011 최종 승인·오늘 통합·로컬 동기화 위임

**바울 대표 실제 발언:** 태온007~011 복구 후보 **모두 들어 보고 원래 짝 전부 확인**, 선행 무음 일부는 Codex 판단으로 최소 보정, 다섯 음성 버그의 공식 수정/검증/오늘꺼 통합/깃허브-로컬 동기화를 마치길 요청. 추가 청취 승인 반복요청 불필요하나 **원문·PCM 발화 경계·Catalog GUID/Meta·실제 Runtime 5페이지** 대조 및 편집 안전 QA를 반드시 실시. 잘못된 본문/샘플 손상이면 STOP, 새 TTS 호출·기존005/006/미엘/Player 변경 금지.

**지금 GitHub에서 확인된 부분완료:** Arbel v3 Codex 정식 적용 브랜치 `codex/arbel-v3-official-apply-20261014` HEAD `652867c988d015f0eb65ccb82591d2ef3a36551b`; 승인 23아트/국소 지붕·처마, **3953PASS/0FAIL(단위 assertion), 부분환경 미검증**, 사용자 PC 원본 디스크 적용/Feature Push라는 Codex 보고. 기획에서 **PR#12 생성→main 병합 완료**, merge SHA **`3aa66981f2545d463126e01a96c359c21ca804ea`**. 이 branch를 또 병합하지 않는다. 기존 Portal-나무/URP/Search/Editor 실환경 문제 OPEN; 승인 Optional11 배치0. 원본 PC·Git 로컬 `main`과 원격 동기화는 **Codex 이후 검증 필요**.

**새 실행 정본:** `문서/00_프로젝트/EndOfDay_Integration_LocalSync_Runbook_20261014.md` (`docs/planning`). 음성 수정 후 신규 후보WAV/007~011 Catalog만 Codex 전용 브랜치 Push/PR/QA PASS 시 main 병합, `docs/planning` 오늘 승인·기획 문서도 검증 후 PR 병합. 원격 main SHA와 **로컬 main/origin main 일치**는 사용자 dirty·Unity Editor/Play/Save 충돌을 보호하며 **비파괴적 fast-forward만 가능한 경우** 실행. `reset --hard/clean -fd/force push` 금지. 해당 작업 직접 완료한 척 보고하지 않음.

**Chapter3 기획 선행:** Main21 완료 `World_StarterVillage` 펫서비스를 자유롭게 이용해도 되고, 북쪽 F1부터 Main22 서리들쥐1 첫 전투. Main23 F1심부/첫눈, Main24 F2 한기2턴 사전 대응 안내. Main22~24 세부기획 `docs/planning` 작성 완료(DRAFT/미구현). 사용자 편안히 주무시는 동안 별도 비동기 작업을 자동 실행하는 기능이 여기 연결된 것은 아님.

---

# PLANNING 인계 — 2026-10-14 저녁 Main22 상세·Main23~24 연결 설계 진행

바울 대표가 저녁 식사 중 기획을 계속 진행해 달라고 요청, 기획 담당이 **Main22 상세 v1 + Main23~24 연결 v1**를 `docs/planning`에 작성. 새 정본:
- `문서/03_스토리/Chapter3_Main22_북풍의징후_상세기획_v1_20261014.md` — Main21 완료→초보 마을 **펫 분양/관리 NPC 자유 방문/새 서비스 소개**→북쪽 자율 출발→F1(녹지·얇은 서리) 조사2~3→**서리들쥐1기 지정전투**→F1 쉼터에서 형제/동료 대사. Main22 5단계 목표·안전 Spawn/포털 왕복·저밀도 몬스터·Save·접근성·Path 반응형 규칙·대사 Tone 제안 및 남은 승인 게이트 기록.
- `문서/03_스토리/Chapter3_Main23_24_북부탐험_첫눈_한기_연결기획_v1_20261014.md` — Main23 F1 심부 발자국·설원족제비1 조우 후보→F2 첫눈 경계. Main24 F2 진입 첫 한기 2턴 전체 행동불가, **최초 필수 한기전 이전 대응약/정화Ⅱ/온기의 불꽃 등 실사용 대응 경로 확보**(상품·가격·사전지급 방식 미정), 서리올빼미1 예고형 전투·파티 전원 행동불가 무대응 방지.
- 전체 정본 `문서/03_스토리/Chapter3_Main22_29_북부_온기의화로_스토리구성안_20261014.md`에 두 문서 연결. **실제 `main` Main21 QuestDefinition `nextMainQuestId` 빈 값 발견** → Main22 해금·Continue·일회 보상/북쪽 포털이 실제로 동작하는지 구현 승인 뒤 Codex 검증 필수.
- **스포일러·인물 경계:** 태온/하렌 반말·형제애 메인/결말 `잘.했.어.`, 폴·미엘 쌍방 Slow Burn, 세린의 독립 조사/미엘·세린 수다, 여성 플레이어의 추가 반응(동일 정보/보상), 미엘 과거는 Chapter5/6까지 비공개.
- **승인 상태:** 세부 Objective·대사·NPC 동작 제안은 **v1 DRAFT / USER_REVIEW_PENDING / UNITY_NOT_IMPLEMENTED**. 기존 Main21 13단계·챕터 전환·펫 서비스 구현과 User Save 보호. 스킬은 상점에서 탈렌트 구매하는 기존 정책 유지. Chapter3 아트/TTS/BGM·코드 구현 발주0.
- 다음: 바울 대표 귀환 후 이 세부 구성, 첫 조우 빈도/유저 부담, F2 한기 해제 수단 공급방법, 북쪽 원정 출발 동기, Chapter3 신규 Scene 구성의 중요한 설계 선택 검토. **사용자 식사 동안에도 메시지를 받지 않은 시간에 백그라운드 작업을 계속했다고 주장하지 않는다**. 이 턴에서만 GitHub 기획 진행.

---

# PLANNING 인계 — 2026-10-14 Main21 초보 마을 귀환 이유: 펫 서비스 공개와 기후 전환

**바울 대표 직접 정정:** Main21의 마차 귀환은 Chapter1 **초보 마을 `World_StarterVillage`**에서 종료. 왜 다시 마을을 거치는가? **(1) Chapter2 클리어 뒤 기존 초보 마을에도 펫 분양·관리 서비스가 추가됐음을 플레이어에게 직접 보여 주기, (2) 뜨거운 서부→곧바로 얼어붙은 설원으로 넘어가는 기후 개연성 문제 방지**. 복귀 장소·목적을 혼동하지 말 것. 이미 `main`에 있는 `SafeZonePetServiceRollout`은 **Main21 완료 후** 새 펫 NPC 2명 활성화: 따라서 Main21 목표10 마차 도착 당시 NPC를 강제 보여주거나 목표13 전에 서비스 사용 강제하면 안 됨. **Main21 목표13/클리어 후 마을 자유 탐색(펫 NPC 자발적 확인/이용) → 북쪽 길로 자발적 Main22 출발 → 신규 북부 F1 몬스터/서리**. 펫 구매 의무0, Main22 즉시 Scene 강제 전환0, 기존 보유/직업·SafeZone·Save·13목표/미술 보호.

기후: **서부 열기→평온한 초보 마을(찬 북풍 정도)→F1 얇은 서리·서리들쥐·설원족제비→F2 첫눈→F3~F5 눈/동토**. 북행 탐사의 직접 동기는 기존 Main21의 북쪽 이상 징후와 F1 동물 남하 조사이며, 펫 시설은 마을 귀환의 게임플레이 허브 이유다. Main21의 기존 하렌 첫 만남/북쪽 준비를 Main22에서 반복 금지. 상세 Scene Portal/퀘스트/배치·새 대사는 기획/구현 미확정.

갱신 문서 (`docs/planning`):
- `문서/03_스토리/Chapter2_Main21_돌아온_온기.md`
- `문서/03_스토리/Chapter3_Main22_29_북부_온기의화로_스토리구성안_20261014.md`

---

# PLANNING 인계 — 2026-10-14 Main22 새 북부 필드·첫 몬스터 전투 포함

바울 대표 피드백 **“메인22 어디서 해? 필드, 몬스터 안들어?”** 반영. **마을 준비만 하는 Main22안 폐기**. Main22 `북풍의 징후`는 **아르벨 출발 → Chapter3 신규 북부 F1 ‘북풍의 들머리’ 입장 → 서리·남쪽으로 향하는 동물 흔적 2~3곳 조사 → 서리들쥐 1기 첫 지정전투 → 태온/하렌 의견차 → F1 내 안전지점 종료**로 수정. F1 일반2종은 기존 **서리들쥐·설원족제비** 그대로(한 화면 몹 최대2, 일반전1~2기). **Main23**은 F1 심부 탐색·동물 남하 흔적 확인 후 **F2 ‘첫눈의 숲’ 입구까지**, Main24에서 F2 첫 한기 예고/대응. 기존 일반5종/엘리트1/보스1·북부 일반필드5+보스전1 유지, 하렌은 자동 4인 전투 합류 금지. Main22 필드·전투는 기획안이며 신규 SceneId/QuestId·레벨·EXP·아트·구현은 미확정.

수정 정본: `문서/03_스토리/Chapter3_Main22_29_북부_온기의화로_스토리구성안_20261014.md` (`docs/planning`). 뒤편의 이전 Main22 '마을에서 준비만' 요약은 이 최신 정본에 의해 대체됨. 다음 Main22 실제 Objective/Dialogue·이동 연결 안전성 확정 필요.

---

# PLANNING 인계 — 2026-10-14 Chapter3 인물 관계 3축·미엘 과거 설정 확정

바울 대표가 Chapter3 **형제애 태온·하렌(메인) / 폴·미엘 Slow Burn / 미엘·세린 일상 수다(+여성 플레이어 선택적 참여)**의 3축을 요청·확정. **내부 스포일러 신규 사용자 확정:** 미엘은 과거 결혼을 약속한 사랑하는 남성과 함께 전쟁에 참전, 둘 모두 큰 부상, 남성이 미엘을 먼저 치료해 달라 했고 미엘 치료 중 그가 곁에서 사망. 이 경험이 **미엘의 마음의 상처와 타인을 먼저 치료하려는 경향**의 핵심 개인적 맥락. 본격 공개는 **Chapter5 또는 Chapter6**, **Chapter3 Main26 안전 쉼터에서 단 한 차례 짧은 말/표정으로 간접 암시 후보**. 지금 트라우마 관련 리서치·질환 명명/치료 묘사는 보류. 동료 본인의 자기결정권·중요 정보 성별 동등성·세린 독립적인 탐사/판단 보호. 폴은 과거 연인 대체/트라우마 치료사 역할이 아니며 새 고백·연애 확정 없음.

**내부 스포일러 정본:** `문서/03_스토리/Chapter3_형제애_폴미엘_여성동료수다_미엘과거복선_20261014.md` (`docs/planning`). `Chapter3_Main22_29_북부_온기의화로_스토리구성안_20261014.md`와 `문서/04_등장인물/폴_미엘_관계.md` 동기화 완료. Chapter3 Main22~29은 여전히 **1차 구조 가안**이고 아직 QuestID/Dialogue/TTS/BGM/아트/Unity 제작0. 태온·하렌 결말 정확 **“잘.했.어.”** 보존. 공개 영상·TTS 녹음용 대본으로 이 비공개 전쟁 사건을 미리 공유하지 말 것. 다음에는 Main22~24 자세한 목표/인물 대사 먼저 설계.

---

# PLANNING 인계 — 2026-10-14 아르벨 국소 투명·접합 최종 미술 승인

바울 대표가 이전 지붕·집 간격 문제를 수정한 Codex **국소 지붕 투명 + 처마 접합부 마감색** 3열 Unity 비교 화면을 직접 본 후 **“미술 승인은 내가 보는거면 봤으니까 된거지 뭐”**라고 명시 승인. **아르벨 기본 23종과 해당 국소 투명·접합 표현 USER_ART_APPROVED, 미술 재승인 질문 금지**. Codex 보고 **1,039PASS/0FAIL·실제 Unity 캡처38장**, 신규PNG0, 문·벽·처마 불투명 보호, 배경 건물 떠보임 보완.

기획 정본 `문서/00_프로젝트/Arbel_v3_Official_Apply_Push_Gate_20261014.md` 갱신. 남은 사항은 ZIP 검증본 SHA(`84e9707d...`) vs 현재 Downloads 문서차이(`58fb22...`) 소스 확정과 정식 디스크 적용/접근성·Quest·Save/Scene 회귀 QA, 작업 Codex feature Push. 기존 조건부 정식 적용/feature Push 승인 유효하되 **실제 적용·Push0, main 병합은 별도 승인**. 선택 장식11개 미배치, 기존 Portal-나무 OPEN 유지. 사용자의 원본 Unity Editor/Save/dirty 상태 보호. Codex 잔여 할당량을 고려하여 현재 새 작업을 시작했다고 주장하지 않음.

---

# PLANNING 인계 — 2026-10-14 Field02~Field03 태온007~011 음성 페이지 대응 오류 (최신)

바울 대표 실제 Unity Play 캡처5장·청취 재제보: Field02 `폭이 같습니다` 음성 미재생, Field03 `누군가` 화면에서 앞 `폭이` 재생, `너무 조용합니다` 화면에서 `누군가` 재생, `그 이유가 무엇인지는` 전에 앞 태온 음성 연결/혼재, `마법 같습니다` 내용은 정상이지만 시작 공백 길다. 코드 원문/Stable ID 실제 확인: `main08_taeon_supp_007`~`011`. 중간 미엘 대사/무음 Player, 5+3페이지 `DeepZone` 분할, 이미 main 병합 승인한 태온005/006 **절대 수정 금지**.

**기획 새 정본**: `문서/00_프로젝트/Main08_Field02_Field03_Taeon007_011_Voice_Content_Audit_20261014.md` (`docs/planning`). 코드 DialoguePresenter는 페이지마다 이전 음원을 Stop하고 선택 Clip Play, 따라서 **WAV 실제 내용·GUID 연결·합본/선행무음·현재 AudioSource timeSamples**를 Codex **원본 읽기전용** 우선 대조. 후보007~011은 기존 PCM 재사용/손실없는 분리/필요시 선행 여백 최소 보정 후 별도 임시 폴더와 개별 청취 자료만 납품. **새 TTS 0, 실제 Unity 음성 연결0, Commit/Push0, main병합0**. 청취 승인 후에만 정확 ID 대상 정식 연결/QA/브랜치 Push 지시. Main08 PR#11 merged/main SHA `314f7fbbd66799ff93ec71404bcf388023c655be` 보호. Arbel v3 지붕 깊이감/집-지붕 떨어져 보이는 미술 HOLD와 분리.

---

# PLANNING 인계 — 2026-10-14 아르벨 세린 투명 지붕 후보 기술 PASS, 미술 판정 대기

Codex 격리 새 후보: 기존 Y-Sorting + SortingGroup + **세린을 실제로 가리는 지붕만 투명 렌더링**. 지붕 불투명 영역이 세린을 약99.79% 가림, Order 값만으로 해결 불가능. **1,575PASS/0FAIL, 실제 Unity 캡처68장, 세린 위치변화/문·기타NPC 상호작용3/3PASS**, 새PNG0, 원본5223파일/Editor/Play/포커스·Save·Scene 변경0, 공식 적용·Push0. 기존 Portal-나무 겹침 OPEN·Optional 장식11 미배치. 보고서 우측 화면은 세린이 드러나지만 지붕 위에 서 있는 인상을 완전히 해소하는지는 사용자 시각 판정 필요. **USER_VISUAL_APPROVAL_PENDING / OFFICIAL_APPLY_BLOCKED**: 기술 QA를 최종 미술 승인으로 간주 금지. 다음 바울 대표에게 우측 투명 지붕 최종안 승인 여부 확인 후, 필요시 동적/국소 투명화 수정 제안·격리 검증. 기록 `문서/00_프로젝트/Arbel_v3_Official_Apply_Push_Gate_20261014.md` `docs/planning`.

---

# PLANNING 인계 — 2026-10-14 PR #11 태온 005·006 main 병합 확인

바울 대표가 Main08 음성 PR #11의 **main 병합을 명시 승인**하고 기획 담당이 정상 병합 수행. PR #11 = merged/closed, merge commit **`314f7fbbd66799ff93ec71404bcf388023c655be`**; GitHub 원격 `main` HEAD가 정확히 동일 SHA임을 다시 확인. 태온005·006(사용자 최종 청취 승인; 005 선행무음185.833ms 축소, 006 기존 PCM 보존), 음성 Catalog 2개 ID 연결과 QA 문서·증거 병합. 사전 224PASS/FAIL0, 별도 기존 Search·스피커·대화 중 조우 OPEN은 그대로.

**아르벨 v3 공식 적용은 세린 지붕 깊이감 FAIL로 여전히 BLOCKED**, 선택 장식11 미배치. TTS 병합을 아르벨 승인으로 오해 금지. 최신 확인은 `문서/00_프로젝트/Main08_Trace03_Taeon005006_Voice_Boundary_Followup_20261014.md`.

---

# PLANNING 인계 — 2026-10-14 태온005·006 최종 사용자 청취 승인 및 PR #11

바울 대표가 005 최종 공백 축소 WAV를 다시 듣고 **“들었어 좋아.”** 최종 청취 승인. 승인한 태온005 최종6.543625초/006 3.800542초, 기존 승인6·Stable ID 보호. 원본 게임 디스크 Catalog2참조 적용 보고, Field02 QA224PASS/FAIL0·전체 Search/실제 스피커 등 PARTIAL.

음성 전용 GitHub **[PR #11](https://github.com/HappyPaul812/Project-Limitless/pull/11)** OPEN, 최신 조회 **mergeable:true**, 비교 **main보다 ahead2/behind0**, 20파일(새음성2/meta3/Catalog/QA/문서). 원격 작업 HEAD `721602256ac4e8c4c7081e6b6b3f04122b266edd`, `main` 아직 `bddca540d2fdd9e93f642f8d0f387b34730bd2d8` 불변. 사용자 `좋아`는 **음성 청취 승인**으로 기록, 팀 운영 정책에 따라 **main 병합의 명시 승인 전 실행 보류**. 아르벨 세린 지붕 FAIL/정식 적용 차단은 별도 계속 유지. 최신 음성 정본 `문서/00_프로젝트/Main08_Trace03_Taeon005006_Voice_Boundary_Followup_20261014.md` (docs/planning).

---

# PLANNING 인계 — 2026-10-14 태온005/006 원격 Push 완료·최종005 청취 확인 권장

바울 대표 보고 + GitHub 직접 확인: 작업브랜치 `codex/main08-taeon005006-final-20261014` 원격 HEAD `721602256ac4e8c4c7081e6b6b3f04122b266edd` Push 완료. 원본 디스크 새로운 승인WAV2/meta3와 Catalog 참조ID005/006 등록, 기존 승인6·원본WAV/meta·Editor/Play/포커스·5,221파일 보호. 005 선행 무음185.833ms만 제거→6.543625s; 006 3.800542s 완전 동일. Field02 대상 224PASS/FAIL0, 기존 Editor Search 예외/물리스피커/정본 버전/대화중일반몬스터 정상조우는 PARTIAL·OPEN. main `bddca540...` 변화0.

**주의**: 실제 WAV 선행 약한 신호 0.14초, 강한 발화 신호 약2.33초(첫 음절 시점으로 단정 불가). 바울 대표가 처음 요청한 공백의 체감 개선을 재청취하면 최종 승인 판단에 유익. **PR/main 병합은 사용자 승인 전 대기**. 만일 여전히 공백 길면 Codex에게 보존된 숨/내부 여백 청취 기반 진단만 요청. 정본 `문서/00_프로젝트/Main08_Trace03_Taeon005006_Voice_Boundary_Followup_20261014.md`.

---

# PLANNING 인계 — 2026-10-14 Arbel 공식 적용 FAIL, 세린 깊이감 원인 진단

바울 대표 Codex 보고: **아르벨 정식 적용 필수 게이트 FAIL**, 원본 적용·Commit·Push 중단 완료. 격리 시험 기술 **905PASS**, Renderer284/284·Missing Sprite/Script0·캡처17장·후보23·PNG재제작0. 그러나 기존 지붕 Sorting 뒤=세린 거의 가려짐, 앞으로=세린 지붕 위에 선 것처럼 보임. Order30 등 단순 Sorting으로는 해결되지 않음. 원본5223 SHA/HEAD/dirty·Unity Editor/Play/포커스·Save·Scene 무변경. 기존 Portal/나무 겹침은 별도 OPEN.

정본 `문서/00_프로젝트/Arbel_v3_Official_Apply_Push_Gate_20261014.md` 후속 상태 갱신: **OFFICIAL_APPLY_BLOCKED / SERIN_DEPTH_VISUAL_FAIL**. 다음 지시는 **Codex 격리 깊이감 원인·실제 발앵커/Y-Sort/지붕 앞뒤 레이어/문·NPC 시야·적응형 roof transparency/mask 가능성 조사**, 객관적 Before/After 제시. PNG 지붕 분리 필요 시 Chapter1·2 리뉴얼 담당 `docs/art-renewal`에 정확 자산/렌더러/처마 마스크 범위를 요청. 정상 미술 승인 유지하되 전경 레이어 세부 보완. 23개 새 아트 전체 재제작 금지; Optional11 미배치; 원본 공식 적용 및 작업 브랜치 Commit·Push/PR/main 병합은 **시각 게이트 재PASS·사용자 비교 확인 전** 금지. 태온005/006 TTS 후속은 별도 작업.

---

# PLANNING 인계 — 2026-10-14 태온005·006 직접 청취 승인 + 005 공백 보정

바울 대표가 분리 두 WAV를 각각 청취하고 **“둘 다 정상인데. '초원에서는...' 앞이 좀 공백이 있네?”**라고 보고. **006은 내용·음색·경계 전체 승인**, **005는 내용·음색 승인, 첫 발화 이전 선행 무음만 짧게 보정 필요**. 005 6.729s와 006 3.801s는 기존 분리 후보 길이이며 최종 005는 조금 짧아질 수 있음. Codex는 원인이 WAV 선행 무음인지 런타임 시작 지연인지 확인 후, 발음/호흡을 살려 무음만 최소 Trim하고 006 PCM은 그대로 보존. 원본 PCM 전체 재결합 동일성은 최종 Trim 뒤 주장 금지; 살아 있는 발화 PCM·기존 승인6·다른 대사/Save/Editor 보호.

기획 정본 `문서/00_프로젝트/Main08_Trace03_Taeon005006_Voice_Boundary_Followup_20261014.md` 갱신. 사용자가 2문장 청취 승인했으므로 기술적으로 안전한 005 trim 확인 후 해당 2 Stable ID만 격리 Unity Catalog 연결/회귀 검증, 전용 codex 브랜치 Push **조건부 진행 가능**, `main` 직접 변경/병합 금지. **현재 Codex 적용 완료는 아님**.

---

# PLANNING 인계 — 2026-10-14 태온005·006 무손실 후보 청취 대기

바울 대표 전달 Codex 결과: 기존 태온006 합본 WAV에서 **`main08_taeon_supp_005` 6.729초, `main08_taeon_supp_006` 3.801초**를 무손실 분리하여 후보 제작 완료. 자동전사 정본문장 **2/2 일치**, 클리핑0, 샘플 삭제·중복0, 합친 PCM=원본 PCM 동일. 기존 승인6/원본 WAV·GUID/meta·Catalog/Unity·Scene·Save/기타대사 불변, 신규 TTS/API·정식 게임연결·Push0. **실제 바울 대표 개별 청취 승인 전**이며 후보 기술 PASS를 청취 승인으로 오해하지 않음. 다음: 후보 둘 다 재생·음색/문장·분리 경계 승인 받은 뒤 Codex에만 해당2 StableID 정식 연결/검증 지시. 최신 기록 `문서/00_프로젝트/Main08_Trace03_Taeon005006_Voice_Boundary_Followup_20261014.md`.

---

# PLANNING 인계 — 2026-10-14 Arbel v3 조건부 공식 적용·feature Push 승인

바울 대표가 아르벨 격리 Unity 3245 PASS 보고와 미해결 세린 지붕 깊이감/Portal 기존 겹침을 받은 뒤 **“넘어가자.”**고 지시. 기획은 이를 **세린 Sorting/가림 필수 시각검증 및 출처 ZIP SHA 확인 성공 시** 원본 Arbel v3 기본 23종 공식 디스크 적용과 전용 `codex/*` 브랜치 Commit·Push를 진행하라는 **조건부 승인**으로 관리. **main 직접 작업/PR 병합/Optional11 배치 승인 없음**.

작업 정본: `문서/00_프로젝트/Arbel_v3_Official_Apply_Push_Gate_20261014.md` (`docs/planning`). ZIP 검증본 SHA `84e9707df7f0409e0238781f0860499dd4344b79ec88b49964a7ce7d8e10633b` 사용 필수, Downloads 다른 SHA는 PNG43 동일하더라도 보존본과 문서 차이 확인 전 임의 혼용 금지. 기존 원본5223/사용자 dirty/Save·Editor/Play/포커스, Field04/Main08 TTS 및 다른 씬 불변. **세린 앞뒤 깊이감/양쪽 문 시야·휠체어 이동/Portal 사용 실질 장애를 검증 전 PASS 처리 금지**. 필수 게이트 불합격이면 적용/Push 중단. 현재 **지시 문서화 완료, Codex 실제 적용 미실행**.

---

# PLANNING 인계 — 2026-10-14 Arbel v3 격리 Sorting 보완 3245 PASS

사용자 전달 Codex 격리 보완 결과: **3,245 PASS / 0 FAIL**, 실제 Unity 캡처55, 교체후보23, Renderer284/284, Sprite/Script 누락0, Optional 장식0. **PNG 43개 픽셀 변경0**, SpriteRenderer Sorting만 격리 환경에서 조정해 세린을 화면에 드러내고 양쪽 건물 문 가림을 해결. Collider가 있는 소품9 배치 높이 검증 통과, 기존 Barrel·파티 안내판·Chest Collider 우회 경로 도보/휠체어5/5 지점 도달. **OPEN**: 세린의 지붕 앞뒤 깊이감 시각 판정, 기존 Portal-나무 겹침, 수동 실제 조작/전체 런타임 QA. 현재 Downloads ZIP SHA는 기대값과 달라 **기대 SHA `84e9707d...`의 이전 보존 검증본**으로 실험했고 PNG43 동일·문서11개 차이만 보고, 정식 적용 때 출처 확정 필요.

기획 상태는 `ISOLATED_TECH_PASS / VISUAL_PARTIAL / ORIGINAL_APPLY_NOT_AUTHORIZED`. 원본5223 SHA 불변·Editor/Play/포커스·Save 미변경·Push0. 다음 Codex는 **세린 지붕 앞뒤 위치별 화면 비교→가림 자연스러움 점검과 Sorting 범위 확정** 후 사용자 확인. Portal 겹침은 기존 이슈로 별도 관리. 승인 디자인 재제작 금지, 공식 Unity 적용·브랜치 Push/main 병합 미승인. 갱신 정본 `문서/00_프로젝트/Arbel_v3_User_Visual_Approval_And_Occlusion_Fix_20261014.md`.

---

# PLANNING 인계 — 2026-10-14 Field02 Main08 태온005·006 음성 불일치

바울 대표가 원본 Unity Field_02 캡처2장과 함께 “초원에서는…”부터 “이 흔적…” 직전까지 다른 대사라고 재제보. MainQuest08FieldFlow.GetLines 실제 대조: 정확히 Trace03의 **태온005** (`main08_taeon_supp_005`, “초원에서는 무언가를 피해 밀려오는 것처럼 보였습니다.”), **태온006** (`main08_taeon_supp_006`, “여기서는 그 방향이 조금 더 분명합니다.”), 다음 플레이어 무음 “그러면 우리가 찾던 방향은 맞는 것 같습니다.”까지. WheelTracks 첫 **미엘004** (“이 흔적… 폴 씨 것 아닐까요?”)은 **제외 경계**. 10/13 Main08 음성6개 승인 적용 당시 **태온005/006은 OPEN**이었고, 기존 ASR에서 기존005에는 태온004의 긴문장, 기존006에는 정본005+006 합본 발화 기록이 있다.

신규 기획 정본 `문서/00_프로젝트/Main08_Trace03_Taeon005006_Voice_Boundary_Followup_20261014.md` (docs/planning). 다음 Codex는 기존006 PCM에서 두 정확 문장의 무손실 분리 가능성 진단 후 **005/006 두 후보를 별도 폴더로 출력하고 인간 청취 승인 요청**. 분리 실패일 때만 TTS 담당에게 그 ID만 제작 검토. 원본 WAV·meta·Catalog·승인 6개·Unity Editor/Save 변경0, 신규 TTS/API0. 현 상태 **CANDIDATES_PENDING / USER_LISTENING_PENDING / NOT_APPLIED**. 대화 중 일반조우 OPEN은 별도 보호.

---

# PLANNING 인계 — 2026-10-14 Arbel v3 미술 방향 승인 / 화면 가림 보완 대기

사용자가 Codex 아르벨 격리 Unity 캡처 39장을 직접 보고 **“봤어. 좋아”**라고 답함. 따라서 **USER_VISUAL_DIRECTION_APPROVED**. 보고 수치: 교체 후보23 / Renderer284/284 / 선택장식0 / 전체 Scene 참조3654 정상 / Missing Sprite·Script0, 원본5223파일 변화0. **기술 PASS / 시각 PARTIAL**: 북서 지붕이 세린 가림, 양쪽 건물 벽이 출입문 가림. 소품9종 실제 물리접지/기존 Portal-나무 중첩/휠체어 6/8 및 미도달2 지점 HOLD. Downloads ZIP 기대 SHA 불일치라 시험은 기대 SHA와 일치한 기존 검증본으로 진행한 점 주의. 정식 적용 전에 후보 ZIP SHA를 확정해야 함.

정본: `문서/00_프로젝트/Arbel_v3_User_Visual_Approval_And_Occlusion_Fix_20261014.md` (docs/planning). **다음은 Codex 격리 환경에서 지붕/벽 가림만 최소 보정하고 실제 Unity 전후 비교 + 기능/접근성 재검증**, PNG 수정 필요 시 리뉴얼 아트 담당 개별 수정 요구. 사용자 재확인 전 원본 공식 적용/Push/main 병합 승인 없음. 선택 장식11종 계속 미배치.

---

# PLANNING 인계 — 2026-10-14 Arbel v3 격리 Unity 화면시험 승인 (최신)

바울 대표가 "그래."라고 명시하여 Arbel v3의 **원본 변경 없는 격리 Unity 시험적 Sprite 교체 및 실제 GameView/RenderTexture Before/After 캡처**를 승인함. 이는 전체 미술 승인/원본 Unity 공식 적용/feature Push/main 병합 승인 **아님**.

신규 기획 정본: `문서/00_프로젝트/Arbel_v3_IsolatedUnity_VisualTrial_Gate_20261014.md` (`docs/planning`). Codex에게 23개 직접교체 후보만 격리 Scene에 시험 적용, 바닥·도로·벽12, 지붕2 가림 HOLD, 소품9 접지 HOLD를 구별해 실제 Unity 렌더링 비교 제작 지시. Optional11 배치0/Portal(-7,-4) 겹침 무변경. 원본 Unity Editor/Play·Save/Scene/dirty·Assets/GUID/meta 전부 보호. 이후 바울 대표가 실제 게임 화면 직접 확인하고 23개 기본안 및 11개 선택 장식을 별도로 승인.

**현재: ISOLATED_TRIAL_REQUEST_READY / NOT_YET_TESTED / USER_ART_APPROVAL_PENDING.** GitHub 문서 작성만 완료, Codex가 실제 격리 Unity 시험/이미지 제작을 했다고 주장하지 않는다.

---

# PLANNING 인계 — 2026-10-14 북부 Main22~29 스토리 1차 구성

바울 대표 지시 “우리는 기획이나 하자”에 따라 Arbel v3 리뉴얼 확인/담당 문서 인계는 아트 담당에게 맡기고 기획에서는 Chapter3 북부 **온기의 화로·설빙의 폭군** 메인 시나리오를 진전. 신규 문서: 문서/03_스토리/Chapter3_Main22_29_북부_온기의화로_스토리구성안_20261014.md (docs/planning).

**Main22~29 8개 퀘스트는 기획 제안/미확정**: 출발→F1 피난동물→F2 첫한기→F3 세린이 화로 마력단서 발견→F4 피난처와 동상→F5 빙각 수호체 및 하렌·태온 신뢰→Main28 설빙의 폭군과 단일 화로 전투→Main29 서서히 돌아오는 온기와 전투 후 정확 대사 “잘.했.어.”. 북부 일반필드5+보스1, 일반몹5/엘리트1/보스1, 조우저밀도/냉기 대응 사전 공급/실시간 시간제한 없음, 하렌 자동 영구 합류·3인 초과 전투 금지. 세린/폴/미엘도 스토리 의사결정과 안전 확보에 기여. 확정된 보스명·화로 전제와 제안된 세부 원인/퀘스트 수 구분. 새 Quest/Scene/아트/TTS/코드 제작0. 다음 Main22~24 목표·대사 설계 및 Main28 턴큐 안정성 확인.

---

# PLANNING 인계 — 2026-10-14 Codex Arbel v3 보고 후 아트 담당 명시 확인 대기

Codex v3 메타데이터 정정은 사용자 전달 보고 기준 **QA PASS**: `Limitless_Arbel_Renewal_Art_v3.zip` SHA256 `84e9707df7f0409e0238781f0860499dd4344b79ec88b49964a7ce7d8e10633b`, 소품 Sprite.rect 8개 정정, 기존 PNG34/34 동바이트·284Renderer·도로 seam0·북서 십자, 검사2455+191PASS; 원본 Unity·Push0.

**중요한 부서 간 인계 조치:** GitHub `docs/art-renewal`의 `WORLD_ART_RENEWAL_LATEST.md`를 확인하니 아직 v2 상태임. 따라서 리뉴얼 담당이 v3 결과를 **직접 읽고 검토한 것은 미확인**. 기획 담당은 `Arbel_v2_Props8_Metadata_Correction_Gate_20261014.md`에 리뉴얼 담당의 **명시 검토·응답·문서 갱신 지시**를 기록함. 바울 대표가 담당 세션에 이를 전달한 뒤 담당의 리뷰와 `docs/art-renewal` 문서화 커밋을 확인할 필요가 있음. 자동 세션간 전달 완료로 주장하지 말 것.

현 상태: **Codex 기술 문서 QA 완료 보고 접수 / 아트 담당 v3 확인 미완 / 바울 대표 미술 승인 미완 / Unity 적용·런타임·Push 미진행**. 지붕/NPC 가림·소품 실제 접지·Portal 장식 충돌/선택장식11 HOLD.

---
# PLANNING 인계 — 2026-10-14 Arbel v2 재검수 / Props8 기록 오류 (최신)

바울 대표가 Codex v2 재검수 결과를 전달. v2 ZIP SHA256 17625d35c93599bdd74a4b8280c0416240c402be279815c8966199410bca97b7 일치, PNG/CRC/Manifest, 도로 Seam0, 북서 의료 십자 수정, Arbel Renderer284/284 전부 PASS. 다만 **소품9종 중 원본 크기 필드8종의 기존 시트/복합크기 fallback 오기**가 있음. Codex 정정 근거 CSV에 따르면 Medium Tree 125×146→128×156, Barrel64×82→128×128; 나머지는 임의 추정하지 않고 CSV 근거로 복구. PNG 자체 오류 아님.

최신 문서: 문서/00_프로젝트/Arbel_v2_Props8_Metadata_Correction_Gate_20261014.md (docs/planning). **권고: Codex가 기존 감사 근거 CSV로 문서8종만 고쳐 v2를 보존한 별도 v3 ZIP(문서정정판) 제작**, 실사용 PNG34 전부 동일 SHA, Renderer284/284+기존 QA 재검증. 선택 장식11종·지붕2종 가림·실제 Alpha foot/접지·Portal 충돌은 HOLD. 사용자 미술 승인·Unity 적용·main Push 전 단계이며 원본 Editor/Save 보호.

---
# PLANNING 인계 — 2026-10-14 Arbel v1 QA 미술 보완3항목 (최신)

바울 대표 제공 Codex 로컬 검수: `F:/Downloads/Limitless_Arbel_Renewal_Art_v1.zip` SHA256 `ec707452169aa99ec297a7ac23a14928d4a0123b4b7b051d17ddcb282953d626`; 47파일/PNG40/Arbel Renderer284/284, PNG CRC/Manifest·바닥6 seam 대부분 정상. **NEEDS_MINOR_FIX**: (1) 도로2장의 좌우 RGB·상하 Alpha 경계 불일치, (2) 소품 **9개**의 원본 크기/가시 alpha 접지 데이터 보완, (3) 북서 건물의 십자표지가 실제 치유사 위치와 달라 안내 오해. Preview5는 합성화면/실 Unity 미검증; 지붕 확대·선택장식11·Portal 기존 충돌 HOLD. **원본 Unity 적용0, Push0, 바울 대표 미술 최종 승인 대기.** 기술 대응 284/284 정상이어도 전체 교체 승인 아님. 리뉴얼 담당 `docs/art-renewal`에 `Limitless_Arbel_Renewal_Art_v2.zip` 최소 보완, 기존 정상 PNG/Asset/SHA·GUID/meta 보존, 변경부 비교 및 인계 문서 요청. Codex는 v2 읽기 전용 재검증 후 사용자 미술 승인 때만 통합.

정본: `문서/00_프로젝트/Arbel_Renewal_v1_MinorFix_QA_Handoff_20261014.md` (docs/planning). Chapter3 설빙의 폭군/신규 아트와 별도 업무.

---

# PLANNING 인계 — 2026-10-14 설빙의 폭군·온기의 화로 (최신)

바울 대표 직접 확정: Chapter3 최종보스 이름 **‘설빙의 폭군’**, 현대 판타지에서 알려진 웬디고 이미지에서 영감을 받는 **LIMITLESS 고유 보스**. **마법의 온기의 화로가 북부를 보호하고 있으나 남은 힘이 거의 없어 곧 영구 결빙 위기**. 보스 입김에서 눈발/눈보라가 뿜어 나오며 `동결괴사`를 부여한다. 보스 이름 가칭 `백빙의 심장`은 **폐기**. 화로는 기존 보스전 기능대로 `살아 있는 아군1명/행동1회/동결괴사 중첩 전부 제거/회복·부활0`이며 실제 화로 잔량/실시간 카운트다운으로 기존 공정성을 깨지 않는다.

신규 정본: `문서/08_몬스터/Chapter3_설빙의폭군_온기의화로_북부몬스터_상세기획_20261014.md`. 일반5종(서리들쥐·설원족제비·서리올빼미·얼음갑충·눈보라 정령)+엘리트 빙각 수호체+보스 설빙의 폭군, 일반 탐험 Field5→단독 보스전1. 모든 적의 화로 약화와의 연관, 전열·후열, 행동/상태, 전리품 후보, 설원족제비 한기 대응 펫 후보 1종, 낮은 화면 밀도/대응 선제 공급까지 기획. 1차 문서는 최신안 링크를 넣어 오래된 보스명 오인 방지.

**승인 경계:** 보스 명칭과 온기의 화로/입김 동결괴사 중심은 바울 대표 확정. 실제 화로 기원·마력 흐름 구조·필드 가칭·몬스터 이름과 전투 수치/패턴·보스 정체·펫 효과·Drop/EXP/Talent·Scene/Quest/Monster ID는 세부 제안이며 승인/검증 전 구현 금지. 상태 한기/동상/동결괴사 및 전투3인/온기의 화로 기존 정본, 마지막 **‘잘.했.어.’** 정확한 문자·후전투 연출 보존. Chapter3 신규 아트 담당 `docs/art-new`이며 현재 아르벨 Chapter1·2 리뉴얼 `docs/art-renewal` 작업과 무관. 다음: 실제 Chapter2 완료 Save·경제 표본 read-only 조사→정확 보스 턴큐/괴사 3중첩 공정성·몬스터 전투 밸런스/드롭 계약 설계→사용자 승인→신규 아트/코드 요청.

---

# PLANNING 인계 — 2026-10-14 Chapter3 북부 필드·몬스터 1차 구성

**최신 바울 대표 지시:** “북쪽으로 갈수록 얼어붙는 필드니까 알아서 해, 보스 전까지 필드가 몇개인지 계획을 세우고 몬스터를 기획하자. 몹이 많으면 정신 없어.”

기획 담당이 `문서/08_몬스터/Chapter3_북부5필드_몬스터7종_1차기획_20261014.md`를 `docs/planning`에 신규 문서로 작성. **일반 탐험 필드5개 + 보스 전용 전장1개, 일반5종+엘리트1+보스1** 1차 구성. 북진 단계: 북풍의 들머리→첫눈의 숲→눈 덮인 고갯길→백색 침엽림→동토의 균열→백빙의 심장(보스, 모든 이름 가칭). **한 화면 일반 적 최대2개체 목표**, 필드당 일반 종2이하, 일반 조우1~2마리/엘리트·보스 독립전투, 동결괴사 보스전용, 기본펫/기존펫 보호, 신규 펫 우선1종 설계 후보. 냉기 F2/동상 F4 도입은 대응법 선제 공급과 예고 필수. 기존 형제애 `잘.했.어.`와 전투3인 보호. 명칭/레벨/드롭/Exact IDs/전투 수치는 후속 사용자 검토·밸런스까지 **TBD**, 기획안·미구현.

이 문서는 최신 병합 main `bddca540d2fdd9e93f642f8d0f387b34730bd2d8` 정본 및 `Chapter3_초기_방향`, `Chapter3_냉기_보스기믹`을 존중. **Chapter3 새 환경/몬스터 이미지는 신규 아트 담당(`docs/art-new`) 업무**, 현재 진행 중인 아르벨 리뉴얼(`docs/art-renewal`)과 분리. 기존 Chapter1·2 Sprite 복구/Field04 통합 완료를 신규 구현으로 확대하지 않음.

**다음 순서:** 5개 필드 구조에 맞는 몬스터별 스탯/행동 예고·사전 대응·드롭과 펫 효과 상세 → 실제 Chapter2 완료 플레이 레벨·탈렌트·몬스터 보상 실측 분석 → 보스 화로 턴 큐 공정성 모델과 형제애 Main22 이후 사건 배치 → 사용자 최종 수치/미술 승인 → Codex 구현 요청. 문서화만 완료, Unity·TTS·Scene·Spawn 변경 없음.

---

# PLANNING 인계 — 2026-10-14 오전 재개

사용자 “굿모닝~! 이어서 하자”에 따라 GitHub 최신 상태 확인: 원격 EOD `codex/eod-integration-20261013` `47b865e9c19c23d30c2c9eb5e14c79cddb79a52d`; PR #10 OPEN/Draft/미병합; main `0e0476649b667129cb7aa6004f4b677335ed8be2` 변화 없음. 어제 기능854 PASS/0FAIL·정적83 PASS이나 전체 BuildScene Sprite 참조 FAIL 기존문제(Arbel·Field05~08). **신규 복구 수행/검증 완료된 원격 증거 없음.** `Sprite_Reference_Restore_Integration_Gate_20261014.md`에 정확한 Codex 복구 범위·검증·통합 PR 게이트를 신규 정본화. 다음: Codex 기술 Sprite 참조 복구(아르벨/서부 필드), GitHub 작업 브랜치 Push, 합본 QA 통과 시에만 PR10 Ready→main 병합. 기존 타 부서 아트 리뉴얼·TTS 승인 필요 사항과 명확히 분리. 원본 Editor/Save/dirty tree 보호. 본 문서 추가는 GitHub `docs/planning` 기록만 바꾸고 Unity 게임 파일을 수정하지 않음.

---

# PLANNING 최신 인계 — 2026-10-13 (퇴근 전)

## 최신 통합 판정 — BLOCKED / PR #10 Draft / main 미병합

바울 대표가 “오늘까지 한 것 모두 합쳐줘”를 승인하여 Codex가 완료·승인 범위만 `codex/eod-integration-20261013`에 합치고 원격 Push까지 수행했습니다. GitHub에서 원격 HEAD `47b865e9c19c23d30c2c9eb5e14c79cddb79a52d`, [PR #10](https://github.com/HappyPaul812/Project-Limitless/pull/10) **OPEN/Draft/미병합**, main `0e0476649b667129cb7aa6004f4b677335ed8be2` 불변 직접 확인. 충돌 PR #8/#9은 합본 작업 브랜치에서 해결됐으며 승인 Field04 Road42(도로42, Renderer375, PNG52), Main08 승인 TTS6, B/C 스킬UI·펫시설, Chapter3 기획, 최신 담당 인계 모두 포함됐습니다.

Codex 새 합본 QA **기능854 PASS·기능FAIL0, 정적83 PASS**지만 **23개 활성 BuildScene Sprite 참조 전체 검사 FAIL**. 기존 main에서도 동일 문제가 재현되어 `Arbel` Edit Mode 약285/Runtime284개, `Field_05_WesternOutskirts` Edit/Runtime362개의 Sprite null로 실제 배경 누락 확인; Field06~08에도 기존 null 다수. 전체 누락 main 2,111→통합1,749로 감소, Field04의362건이 해소되고 **추가 새 누락은0**. 기존 문제라도 시각 배경 결함을 PASS로 표시하지 않고 main 병합을 중단한 조치는 타당합니다. 기존 Search 예외·정본 Unity버전 차이·서쪽 Spawn 겹침·북쪽 Collider 틈·스킬 저장 영속 미구현·Main08 태온005/006 음성 문제·사람 플레이 미검증은 OPEN/PARTIAL로 유지합니다.

정본 보고서: [EOD_Integration_Final_QA_20261013.md](../../EOD_Integration_Final_QA_20261013.md) (통합 작업 브랜치), 증거 `문서/00_프로젝트/Evidence/EODIntegration20261013`. 원본 5,199파일 SHA256, 기존 사용자 dirty/HEAD, Save/Scene/음성/아트, Editor·Play/포커스 무변경 보고. **main 현재로서는 절대 Merge 금지**.

### 다음 1순위 — 기존 Arbel·Field05~08 Sprite Importer 참조 복구

Codex에게 원본 로컬에서 기존 PNG·사용자 수동 Importer/meta·GUID/local fileID/atlas subasset을 **읽기 전용** 대조시키고, 동일 바이트 자산을 `_Project`에 프로젝트 관리 방식으로 등록해 해당 Scene의 Sprite 참조만 교정하는 최소 수정안을 별도 worktree/브랜치에서 검증시킵니다. ThirdParty 원본/meta 임의수정, Scene 재생성, 위치·Collider·Quest 수정, 승인 아트 재제작 금지. 아르벨 리뉴얼 제작 자체와 이 기술적 참조 복구는 **서로 다른 작업**입니다.

수정 뒤 23 BuildScene 전체 Edit + 해당 Runtime 배경 참조 및 기존 합본 854검사 영향 범위 회귀, 카메라 실제 화면, 전체 원본 파일 보호 검증. 기존 main에서 발생한 문제임을 근거와 함께 남기되 누락 게이트 PASS 전 PR #10 Ready/main Merge 금지. 안전 QA 통과 시에만 사용자의 이전 최종 통합 승인을 실행할 수 있습니다.

---

# PLANNING 최신 인계 — 2026-10-12

**최종 사용자 결정(2026-10-12): 리뉴얼 담당은 Chapter 1·2 기존 아트 교체(환경·몬스터·보스·펫·NPC·VFX)만 전담합니다. 기존 스프라이트 담당은 Chapter 3 이후 모든 신규 아트(환경·필드·던전·몬스터·보스·펫·NPC·캐릭터·VFX·UI 필요 이미지)를 총괄합니다. 기존 공식 승인 자산은 별도 승인 없이 교체하지 않습니다. Codex는 구현·Unity 통합·QA 전담입니다.**

우선 참조: `../../LIMITLESS_20261012_전체대화_결정_인계.md`와 Chapter2 리뉴얼 로드맵, CURRENT_STATUS, CODEX_LATEST. Chapter1 B1/B2 환경 미술/통합 완료 보고, B1 문 작은 상태표시 보완 보고, Field03 Rock B v2 ZIP 없어서 ART_FIX_PENDING. Chapter2 지역/자산 리뉴얼은 Field04부터 시작. Chapter3 스킬 상점·강화판, 부활·정화Ⅱ, 한기/동상/동결괴사·온기의 화로 등은 기획 확정이나 미구현. Main18 세린 TTS10개 선별 재생성 대기. 사용자 직접 Git Push; 모든 담당 세션 용량 전에 사전 보고·후임 인계. 구체적 기획 수치를 임의 확정하지 않습니다.
