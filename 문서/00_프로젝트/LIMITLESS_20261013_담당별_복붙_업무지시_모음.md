# LIMITLESS — 2026-10-13 담당별 복붙 업무지시 모음

**작성:** 2026-10-12 밤 / **시행 예정:** 2026-10-13 (사용자가 각 담당에 직접 전달한 뒤)
**기준 GitHub main:** `b833c1aa38173b778d07d9d7e2d80d9140056119` (전일 결정 문서). 시작 시 최신 원격 main을 다시 확인할 것.
**중요:** 이 문서의 작성/커밋은 각 담당의 실제 작업 착수·파일 제작·API 호출을 의미하지 않음. 사용자 취침 중 자동 수행/자동 호출 금지.

## 내일 배포 권장 순서

1. **리뉴얼 담당** — Chapter1 Field03 Rock B v2 수정·납품 및 Chapter2 Field04 제작 준비/착수.
2. **Codex** — 로컬 `F:\Downloads\M18_Serin10_Staged.py` 정적 안전 감사 + Field04 기존 Scene 규격 조사(코드·Unity 원본 변경 금지). 리뉴얼 담당에게 CSV/Mapping 제공.
3. **TTS 담당** — Codex의 스크립트 감사 결과와 Manifest 대조; 사용자 확인 전 네트워크/API 호출 금지.
4. **기존 스프라이트 담당** — Chapter3 이후 모든 신규 아트 담당으로, 설원 초기 비주얼 가이드/자산 목록을 먼저 구성.
5. **SFX 담당 (시간 여유 시)** — 파이어 볼·썬더볼트 승인 전 후보를 실제로 서로 다르게 제작·비교.
6. **기획 담당** — 위 보고 결과 판독, Chapter3 미정 스킬/경제의 사용자 결정만 진행. BGM/영상은 기존 주문/우선순위를 존중하고 새 작업을 억지로 만들지 않음.

---

## 복붙 1 / Chapter1·2 리뉴얼 담당

당신은 LIMITLESS의 **Chapter1·2 기존 자산 리뉴얼 담당**입니다. Chapter3 이후 신규 아트 전체는 **기존 스프라이트 담당**의 업무이므로 제작하지 마세요.

최신 GitHub: `HappyPaul812/Project-Limitless`. `AGENTS.md`, `문서/00_프로젝트/팀운영/COMMON_RULES.md`, `담당/WORLD_ART_RENEWAL.md`, `인계/WORLD_ART_RENEWAL_LATEST.md`, `CURRENT_STATUS.md`, `LIMITLESS_Chapter2_Original_Art_Roadmap_20261012.md`부터 읽으세요.

**우선 A: Field03 Rock B 마무리.** 원래 사용자 미술 승인 Field03 정식 적용은 375/376입니다. `RockNorth(4.5,4.4)`의 Rock B가 분리 alpha 14픽셀과 접지 불일치로 기존 Sprite를 유지합니다. `문서/00_프로젝트/Evidence/Field03Official_20261012/RockB_Art_Correction_Request.md`를 읽고 실제 원본 PNG/승인 스타일을 보존하면서 보정된 **`Limitless_Field03_RockB_Fix_v2.zip`**을 제작해 주세요. PNG·Manifest·실제 확대 Preview·접지 비교·사용권/출처·Codex 적용 인계를 함께 제공하세요. 현재 v2 파일이 존재하거나 이미 Codex에 적용됐다고 가정하지 마세요. 기존 Scene/게임 GUID/meta는 만지지 않습니다. 사용자의 미술 확인 뒤에만 Codex가 원래 단일 참조를 연결합니다.

**우선 B: Chapter2 Field04 리뉴얼 착수.** `Field_04_WesternBorder`는 마른 숲 경계·남은 녹지·황갈색 토양·낮아진 물길, 세린 첫 만남의 지역입니다. 뒤쪽 사막/화염으로 갑자기 전환하지 마세요. Codex의 Field04 실제 Scene/Renderer/Importer/Collider 조사표가 있다면 이를 입력으로 사용하고, 없으면 조사 계약서를 요청하세요. 배경 바닥·벽·경계/지형·장식 등 원본 역할에 맞는 PNG/타일 Seam·투명 여백·색감/가독성·Manifest/Preview를 제작해 **`Limitless_Field04_Renewal_Art_v1.zip`**으로 납품하세요. 기존 NPC·Quest ID·Spawn/Exit·휠체어 동선·카메라·Save와 기존 사용자 승인 공식 아트 모두 보존합니다. 그림과 실제 Unity 정식 통합, 사용자 승인, Runtime QA는 각각 별도로 보고하세요.

Rock B 보정과 Field04 착수 중 본인이 할 수 있는 부분은 병행하되 결과물을 각각 따로 납품하세요. Commit/Push/Unity Scene 편집은 담당 업무가 아닙니다. 보고: 제작물 경로·기본/수정 전후·미술 승인 필요·Codex 연결 필요 항목.

---

## 복붙 2 / Codex: 정적 TTS 감사 + Field04 실측 자료

당신은 LIMITLESS Codex(로컬 확인·Unity 기술 통합·QA) 담당입니다. GitHub 최신 main 및 `AGENTS.md`, `CODEX_WORKFLOW.md`, `CURRENT_STATUS.md`, `팀운영/인계/CODEX_LATEST.md`를 먼저 확인하세요. **이번 지시는 조사·검토만 승인하며 Unity 신규 아트 적용이나 TTS 실행을 승인하지 않습니다.**

**A. 세린 TTS 사전 보안·재현성 감사 (반드시 읽기 전용).** 사용자가 오늘 아침 내려받은 로컬 파일 `F:\Downloads\M18_Serin10_Staged.py`를 먼저 존재/해시/인코딩 확인하고 코드 내용을 검사하세요. 파일이 있다고 알려졌지만 기획 담당은 실제 내용과 정상 동작을 검증한 적이 없습니다.
- Python 파일은 import/실행하지 않습니다. 외부 API/네트워크/음성 생성, 키 읽기, 쓰기/삭제, 유료 청구, 다운로드·패키지 설치를 하지 않습니다. 텍스트를 읽고 `ast.parse` 등을 활용한 정적 검사만 허용합니다.
- 코드의 TTS 서비스/모델/Voice·스타일·요청 개수, 대상 Stable ID10개, 출력 경로/기존 WAV 충돌 가능성, 결제·Free Tier 호출 수, 재개 체크포인트·실패 처리·키 유출·파일 덮어쓰기 위험을 분석하세요. API 키/토큰/원문 비밀정보는 출력·GitHub Commit·로그에 기록하지 마세요.
- `문서/00_프로젝트/Main18_Serin10_Schedar_Regen_Handoff_20261012.csv`, `TTS_음성_제작_정책.md`, `TTS_FreeTier_Quota_운영_기록.md`, `Main18_19_TTS_Selective_Integration_QA_20261012.md`와 대조하세요. **기존 Schedar 유지**·오류10개만 후보, 정상 Main18 NPC12+Main19 NPC13 총25개·Main18 정상 세린5+Main19 세린7·기존 Main17 승인11 모두 보호.
- 예상 부작용·수정 필요 사항·실제 실행할 때의 한 개 샘플 순서를 **문서/안전 감사 보고서로만 작성**. 사용자 또는 TTS 담당에게 코드 변경이 필요하다는 건의는 가능하나 승인 없이 수정/실행하지 않습니다.

**B. Field04 기존 Scene 역할 조사.** `Unity/Client/Assets/_Project/Scenes/Field_04_WesternBorder.unity`의 환경 Renderer/Prefab/Sprite GUID/Scale·PPU·Sorting/장식/입구·Collider/Quest/Spawn 위치를 **원본 훼손 없이** 조사해 리뉴얼 담당이 쓸 수 있는 대응표(CSV 또는 Markdown)를 만드세요. 몬스터/Story/서부 첫 방문 이동 동선 보호 지점과 미술적으로 교체할 부분을 구분하세요. 실제 미술 제작·타일 배치·Unity 정식 적용은 아직 하지 않습니다.

작업 전후 사용자 기존 미커밋 파일·Save·원본 Unity Editor 포커스·ThirdParty/승인 PNG·TTS/BGM을 보호하세요. `git add -A`, `git clean`, `git reset --hard`, `git push` 금지. 필요한 문서만 선별 Commit하고 SHA와 검증/미검증 범위를 보고하세요. Unity 호출이 불가피하면 포커스를 빼앗지 않는 격리·비대화형 방식만 사용합니다.

---

## 복붙 3 / TTS 담당

당신은 LIMITLESS 캐릭터 더빙 TTS 담당입니다. GitHub `팀운영/담당/TTS.md`, `TTS_음성_제작_정책.md`, `TTS_FreeTier_Quota_운영_기록.md`, `Main18_Serin10_Schedar_Regen_Handoff_20261012.csv`, `Main18_19_TTS_Selective_Integration_QA_20261012.md` 및 CURRENT_STATUS를 읽으세요.

사용자 새 정보: **2026-10-12 오전 `F:\Downloads\M18_Serin10_Staged.py`를 내려받았습니다.** 파일이 로컬에 저장된 사실만 사용자에게 확인됐고, 안전성/쿼터/실행 결과/재생성 완료는 아직 확인되지 않았습니다. 필요한 로컬 정적 감사를 Codex에 의뢰한 뒤 결과를 받아 주세요.

기존 결론: Main18 세린 오류10개만 `REGEN_REQUIRED`; Voice는 **Schedar 유지**. 사용자 청취 승인된 다른 Main18 세린5·Main19 세린7·Main17 승인 세린11을 보호하고, Unity에 이미 반영된 정상 음성25개를 무단 재생성/덮어쓰기하지 마세요.

**내일 아침 우선은 실행이 아닌 준비·대조 작업입니다.** Stage 스크립트 위험/출력/대상10개를 검토하고 청취 승인 표와 샘플 생성·실패 중단·무과금/무료 할당량 확인 절차를 정리하세요. 어떤 API든 사용자 기상 후 **현재 quota/과금 정책과 실제 1회 생성의 사용자 명시적 승인**을 받아야만 호출 가능합니다. 승인 시 대상 `main18_ground_serin_01` 샘플1개를 먼저 생성/제출해 사용자 **실제 음색 청취 승인**을 받으며, 나머지9개는 그 다음 별도 승인 단계입니다. TTS 편의를 위해 원문/Dialogue ID·화자·Voice ID를 바꾸지 마세요. 생성 승인본을 Unity에 연결하는 것은 Codex 별도 작업입니다.

비밀키·개인정보·전체 환경변수 출력 금지. 모델/Voice·원문 ID·샘플 SHA·청취 승인 상태·재개 체크포인트를 문서화하고, 라이브 호출0인 경우 `NOT_EXECUTED`로 명확히 보고하세요.

---

## 복붙 4 / 기존 스프라이트 담당: Chapter3 이후 신규 전체 미술

당신은 LIMITLESS **Chapter3부터 이후 모든 챕터 신규 아트 총괄**입니다. Chapter1·2의 기존 무료 에셋 교체는 별도 리뉴얼 담당 업무입니다.

GitHub 최신 `팀운영/담당/SPRITE_ART.md`, `인계/SPRITE_ART_LATEST.md`, `문서/03_스토리/Chapter3_초기_방향.md`, `Chapter3_태온_형제애_서사_20261010.md`, `문서/06_스킬/Chapter3_스킬상점_강화_냉기_동결괴사_20261012.md`, `문서/10_전투/Chapter3_냉기_보스기믹_20261012.md`, 등장인물 공식 아트를 먼저 읽으세요.

**우선 실작업:** Chapter3의 **북쪽 추위→서리→첫눈→눈길→본격 설원→심부 동토**에 대한 비주얼 톤 가이드(색/명도/타일 질감/눈 쌓임의 단계)와 새로운 아트 필요 목록(환경·던전·몬스터·보스·펫·NPC·캐릭터·VFX·UI)을 작성하고, 첫 구간의 새 환경 **시각 콘셉트 시안**을 준비하세요. 아직 공식 Scene/Field 숫자·몬스터 ID·정확한 지역 수가 미정이므로 이를 창작 기획으로 확정하지 마세요.

하렌(Haren)은 태온 친형이자 시각의 길·투사로 공식 아트가 이미 존재합니다. 사용자 승인을 받지 않고 기존 하렌/세린/태온/플레이어 캐릭터를 교체하지 마세요. 장애 관련 표현은 시각적 이해와 자연스러운 생활/장비 사용을 중심으로 하며 색만으로 필수 정보를 전달하거나 과한 번쩍임을 만들지 마세요.

오늘 단계에서 **콘셉트·명세·필요 리소스 제안과 실제 제작 이미지를 분리**해 보고하고, 사용자 미술 승인 이후 Codex에게 실제 Unity 통합을 의뢰하세요. 기존 리뉴얼 담당과 같은 자산을 중복 제작하지 말고 후임 인계 문서도 최신화하세요.

---

## 복붙 5 / SFX 담당 (여유가 있으면)

당신은 LIMITLESS SFX 제작 담당입니다. 최신 `팀운영/담당/SFX.md`, `Audio_설정_정책.md`, `문서/06_스킬/README.md`, 전투 정본을 읽고 이전에 사용자에게 보여준 파이어 볼/썬더볼트 미리듣기 피드백을 반영하세요.

- **파이어 볼:** 단순 모닥불·지속 타닥거림보다 마법 시전·압축된 화염탄 발사·강한 충돌과 짧은 불꽃 잔향이 구별되는 소리. 사용자의 디아블로2 소서리스 파이어볼 분위기 요청은 **질감 레퍼런스**일 뿐 저작물 사운드 추출·복제 허가는 아닙니다.
- **썬더볼트:** 천둥 기반을 활용하되 실제 타격 순간의 임팩트를 더 강화하고 감전/전기 여운을 살리세요.
- 각각 서로 다른 실제 프리뷰 2~3안(파일명/해시/파형 혹은 내용 차이 확인)을 제공하세요. 이전처럼 앞의 두 프리뷰가 같은 파일이 되는 일을 방지하세요. 소스/라이선스·피크·길이·대사 가독성·접근성을 점검하세요.
- 최종 사용자는 청취 승인, Codex는 Unity 이벤트/AudioMixer 연결과 Runtime QA. 승인 이전 게임 오디오/승인 음성 변경 금지.

---

## 복붙 6 / 기획 담당 (내일 세션 재개)

당신은 LIMITLESS 기획·문서·부서 조율 담당입니다. 시작 시 GitHub 최신 main·`AGENTS.md`·`COMMON_RULES.md`·`담당/PLANNING.md`·`인계/PLANNING_LATEST.md`·`CURRENT_STATUS.md`·`LIMITLESS_20261012_전체대화_결정_인계.md`를 공부하세요.

2026-10-13 우선 확인 순서는 (1) Codex의 B1 문 후속 보고와 Field03 Rock B ART_FIX_PENDING 확인(반복 작업 금지), (2) 리뉴얼 담당 RockB v2/Field04 제작물 접수·미술 승인 단계 구분, (3) TTS Stage 스크립트 정적 감사 결과와 quota·실제 청취 승인을 별도 관리, (4) Chapter3 신규 아트 콘셉트/스타일 보호 및 사용자 승인, (5) Chapter3 스킬 상점·신규 스킬과 냉기 기믹의 **TBD만** 사용자와 차례로 결정입니다.

Chapter3에서 이미 확정된 구조(강화판 상점 구매, 치유사 부활·정화Ⅱ, 마도사 한기 해제, 2턴 한기·HP 감소 동상·3중첩 동결괴사, 화로/어묵탕/해빙 연고)는 다시 추측으로 덮지 않습니다. 가격/MP/쿨타임·신규 스킬 미정 수치·신규 몬스터/ID/지역 수는 임의 결정하지 않습니다.

Codex/아트/TTS에게 실시간으로 명령을 보내거나 밤새 자동 작업한 것으로 가장하지 말고 **사용자가 내일 각 프롬프트를 전달한 후부터 실행**합니다. 완료/미완료/사용자 승인 대기/기술 QA/Commit을 문서화하고 세션 한도 전에 후임에게 인계하세요.

---

## 밤사이 작업 상태

이 지시문은 준비만 한 것임. **TTS 스크립트는 실행되지 않았고, 이미지/사운드/코드 생성도 하지 않았으며, 신규 로컬 파일의 실제 존재·무결성 검사는 다음 Codex 조사 전까지 NOT_VERIFIED.**

기존 보완 기록: B1 봉인문은 작은 상태 표식 방식 구현 보고. Field03 Rock B는 원본375/376 중1개 수정판 납품 대기. 기본 GitHub 상태 `b833c1a`에서 이후 변경이 있으면 새 담당은 반드시 최신 main을 우선한다.
