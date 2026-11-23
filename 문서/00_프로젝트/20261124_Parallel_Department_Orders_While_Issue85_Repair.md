# LIMITLESS / 2026-11-24 / Codex #85 복구 집중 기간 — 다른 담당 병렬 제작 6건

**대표 직접 지시**: “코덱스는 열심히 하고 있고, 딴 담당자들 일 하자.” `#85` 패키지/메타 복구는 Codex 단독 진행. **다른 부서는 GitHub 문서 + 독립 미술·오디오·편집 납품만**, Codex 추가 요청/Unity 본편 변경/신규 Play QA/원본 main 병합·원격 Push 금지.

기준 원격 안전 체크포인트 `backup/2026-11-23-safe-checkpoint` = `65cceb2d6c3283d18abec6e27d8a6f1296411f17`, 원본 Unity 6000.6.5f1. 원격 `main`은 오래된 `bfd281545632877325f5d21e04cefb924042c465`으로 현재 프로젝트 정본이 아님. 11/23 마감 승인 아트·TTS Stage10·BGM4·빙각·드롭 아이콘6·서리들쥐1/6 / 개인 Save/UserData·Discord·키 보호.

## 1. 신규 아트 담당 — #123 / `docs/art-new`

- 어제 F3 첫 소량 납품(눈-암석 접합 오버레이, 낮은 빙설 암반, FlushStone EdgeMarker) **3/3 파일/Unity Import PASS**, 대표 최종 미술 `PENDING`. PNG 재제작·기술QA 반복 금지.
- **새 납품**: 기존 3종의 원본 PNG와 F1/F2 승인 지면/소품을 **동배율 비교하는 대표 최종 미술 판단용 단일 리뷰 보드 + F3 진입 구도(오프라인 개념 배치 1개)**. 암반의 길 바깥 배치/FlushStone 바닥 경계/Transition의 단일-placement만 표시. 걷기·휠체어 길폭4.5WU 실제 통행과 Sorting/Collider/GameView는 `NOT_VERIFIED`.
- **별도 기획 자료**: F4 눈보라 정령 구간으로의 색조·날씨 전이 1페이지 컨셉/필요 자산 후보 목록만 작성, **F4 신규 PNG 제작/타일 양산·Scene 생성 금지**.
- `docs/art-new` 실제 커밋/SPRITE_ART_LATEST.md/#123에 결과 보고.

## 2. Chapter1·2 리뉴얼 아트 — #124 / `docs/art-renewal`

- 기존 VenomBeeStinger Organic v2와 GrassSlimeGel 원본, TipOnly 59px 후보, 길 Trait5, M1~M6와 NPC/펫/지형 **KEEP**. 사용자 최종 미술 승인이 없는 독침/젤 PNG·ItemDefinition·Tint 교체 금지. 숲거미 실·맹독뱀 비늘 파일럿은 기존 승인 게이트를 임의로 넘지 않는다.
- 신규 안전 작업: **실사용 Kenney 전투 UI 아이콘 21종** 정적 감사(#124)에 근거하여 **공격 버튼 아이콘 1종만 신규 오리지널 미술 후보**로 제작. 원본 `BattleUiIconCatalog` 외부 공격 아이콘의 기능과 크기를 검토해 128×128 투명 PNG, 실제 40/30px 대비, UI 어두운/밝은 배경 및 기존 승인 인벤토리 아이콘 화풍 비교. **원본 직접 트레이스·수정 금지**; 원본 실물 접근불가 시 `EXACT_STYLE_COMPARE_NOT_VERIFIED`, 다른 아이콘20종 양산 금지. 기존 버튼 원본 GUID/meta/크레딧·게임 로직 그대로.
- 비교/Manifest/SHA/ZIP 제출, user art approval `PENDING`, Unity 적용0. `docs/art-renewal`, `WORLD_ART_RENEWAL_LATEST.md`, #124 보고.

## 3. TTS 담당 — #125 / `docs/tts`

- Main24 Stage WAV10/10 사용자 생성·청취 PASS 보존. 실제 파일·원장/manifest SHA 전부 실물 확인은 미완이며 Unity Runtime Dialogue ID 매핑0/10 `HOLD`. **Main24 재합성0, 기존162 승인 WAV 변경0**.
- 오늘 **비API 준비**: Main24 Stage10 파일/sidecar/PT ledger 확인표(실물 못 보면 사용자 보고와 `NOT_VERIFIED` 분리) 및 `Stage Candidate ID → 승인 speaker/voice/text → 정식 Unity Runtime ID(UNKNOWN)` 교차표 작성. 현장 PC 직접 접근 불가 시 사실대로 보고.
- **Main25 다음 9개** 정본 72큐에서 원문/캐스팅/VoiceID 정확히 추출, 중복·대사 누락·기존 음성162 및 WAV590 중복 위험 플래그, 실행 전 체크리스트/오프라인 stage-only 생성 배치 사전검증 작성. 새로운 합성 POST/유료 API 호출/무료 한도 추정·비공식 음성파일 생성 **금지**(대표 별도 실행 게이트), 대사 임의 변경 금지. 사용자 실제 생성 전 `STAGING_PREP_ONLY`.
- `docs/tts`, `TTS_LATEST.md`, #125 보고.

## 4. BGM 담당 — F4 신규 기획 / `docs/bgm`

- F1/서릿길/F2/F3 승인 원곡 + Intro/Loop WAV8·11/23 Unity 연결 24PASS **LOCK/KEEP**, 어제 BGM을 재생성·오디오 QA 반복·Unity 변경 금지.
- 다음 지형 F4의 **눈보라 정령 조우/차가움 강화** 역할에 맞는 **오리지널 인스트루멘털 BGM 1곡의 기획 브리프 및 Gemini/Lyria 수동 생성용 프롬프트 1개** 작성. 템포·악기·감정선·F3와 F5/최종 보스 대비, TTS 공간, 45~90초 샘플 목표(음악은 아직 없음). F4 실제 Scene/정식 배정은 미검증, 이름/조우를 새로 확정하지 않음.
- 기존 Gemini 무료 생성 음악의 유료 게임 활용 권리 `COMMERCIAL_USE_EXPLICIT_LICENSE_PENDING`; 권리 조사/문서화를 계속하되 부당하게 상업사용 가능·불가능을 단정 금지. **새 생성 호출0/실제 MP30/Unity0**, 대표가 Gemini에서 1회 생성·청취 후 후속 판단. 다른 F5/빙각·설빙보스곡 자동 양산 금지.
- `docs/bgm`, `BGM_LATEST.md`, 새/관련 Issue에 실제 커밋 SHA 보고.

## 5. SFX 담당 — #33 / `docs/sfx`

- 먼저 #33의 기존 북풍/눈·서리 발걸음/서릿길 온기 환경음 파일이 실제 제작·사용자 청취 PASS까지 됐는지 확인, **기완료 파일 재제작 금지**. 분명히 미완료인 항목만 **2~3종** 오리지널 파일럿 준비.
- 짧은 북풍 loop(고음·peak 억제), 걸음 단발(눈·서리 질감), 온기거점 ambient(대사 주파수 마스킹 억제). 기존 BGM4의 Intro/Loop 영역/전투 후 복귀에 겹칠 때 과도하게 시끄럽지 않도록 low-gain 비교, TTS 동시공간 목표. WAV/상업권/출처/SHA/재생 길이/peak/loop boundary 최소 QA 및 대표 청취용 ZIP.
- 음원 생성 도구가 연결되지 않으면 파일 납품한 척하지 말고 설계/저작권 허용 오리지널 제작 경로·발주 프롬프트로 분리. Unity AudioMixer·트리거 연결·본편 등록0.
- `docs/sfx` 브랜치가 인계 구버전/부재일 수 있으니 Issue #33 정본으로 보고 후 `SFX_LATEST.md` 존재/생성 여부를 기록.

## 6. 개발기록 영상 담당 — #98 / `docs/video`

- 실제 기존 `Limitless_DevJourney_2026-11-16_Raw.mp4` (335.500초)와 `Comparison_20261006_vs_20261116.md`, 10/06 Final을 **원본 그대로**. 어제 신규 아트·BGM/빙각을 11/16 영상에 이미 촬영한 장면처럼 넣지 않음.
- **원본 실파일 접근 가능하면** 10/06 대비 진전 화면만 정직하게 비교 편집하는 최종 시청 후보 1개. 오프닝/엔딩·하이서드·자막/대사 시점/장소·실제 적용 vs 데모 표시. 기존 녹음 더빙 실제 컷만 사용, 라이선스·음량·접근성 확인. **Raw 재녹화/원본 덮어쓰기/YouTube 공개 금지**. 실파일 없으면 사용자가 이미 제출한 영상·비교 문서 확인 후 `FILES_NOT_ACCESSIBLE`과 편집 계획만 제시, 임의 합성 금지.
- `docs/video` 인계/Issue #98 보고, 대표의 최종 시청·청취 전 업로드·공개 금지.

## 보고·분업 공통

- 각 팀은 담당 최신 정본·GitHub Issue를 직접 확인하고 **본인 docs 분기**의 신규 문서/인계를 기록하며 완성물의 실제 경로/해시/QA/대표 승인 필요 여부, `main`·Unity 적용 여부를 구분한다.
- **Codex #85 수리 중 간섭 금지**. 타 부서는 Unity 6000.6.5f1을 직접 열거나 Packages/manifest/lock/.meta/GUID를 건드리거나 새 Codex 작업을 요청하지 않는다. 필요하면 `HOLD`/문서 검토만.
- 본 기획 GitHub 문서/댓글 자체는 **타 담당 ChatGPT 대화의 자동 실행을 뜻하지 않는다**. 바울 대표가 각 담당 세션에 실행 지시문을 전달할 수 있게 간결한 작업 스크립트를 별도 제공한다.
