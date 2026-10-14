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
