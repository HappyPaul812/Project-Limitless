# LIMITLESS 2026-10-13 퇴근 전 안전 통합 — 기획 인계

**상태: INTEGRATION_STAGING_CREATED / MAIN_NOT_MERGED / CONFLICTS_REQUIRE_CODEX / QA_GATE_PENDING.**

사용자 지시: **“나 퇴근하게 지금까지 한거들 다 합쳐줘.”** 이 지시는 오늘 완료·사용자가 승인한 기능/아트/음성과 관련 기획 문서의 안전 통합을 허가한다. 단, 어떤 변경이든 기존 정상 내용을 잃거나 회귀 검증을 생략하라는 뜻은 아니다.

## 원격 기준

- 저장소: HappyPaul812/Project-Limitless
- main은 사전 검사 시 `0e0476649b667129cb7aa6004f4b677335ed8be2`. **직접 커밋·Push·강제 업데이트 금지**.
- 퇴근 전 임시 통합 브랜치 `codex/eod-integration-20261013` 생성. `codex/main08-approved-voice-apply-20261013`의 `79554069d010115a5427eedfc1c109994737f4fb`를 기준으로 시작하여 **PR7 A/B/C + Chapter3 1차 설계/Importer 교정 + Main08 음성6개** 이력을 보존함.
- 최종 통합 대상 Draft [PR #10](https://github.com/HappyPaul812/Project-Limitless/pull/10): `codex/eod-integration-20261013` → main.
- [PR #8](https://github.com/HappyPaul812/Project-Limitless/pull/8): **공식 Field04 Road42** `codex/field04-v2-official-42-20261013` (`ec469c0aed66b58e8a8ee9e842c47c3d78c7b675`) → 통합 브랜치. GitHub mergeable=false 확인; **충돌 내용별 재검증 후 통합 필요**.
- [PR #9](https://github.com/HappyPaul812/Project-Limitless/pull/9): 최신 `docs/planning` (`22e134473e96847acbf86cfff3482e7958a75671`) → 통합 브랜치. GitHub mergeable=false 확인; **양측 기획 기록 보존 후 통합**.
- 기존 [PR #7](https://github.com/HappyPaul812/Project-Limitless/pull/7)은 여전히 Draft/미병합이며, #10의 시작 이력 안에 포함됨. **#7을 별도로 main에 중복 병합하지 말 것**.

## 합칠 완료 범위

1. **스킬 UI B**: 기본5명령 보호, 전투 스킬 선택2×3·장착6칸, 관리 UI. 실제 현재 Save/Continue 후 영속 장착 미구현 — 이를 완료로 표기 금지. 격리 QA 72 + 기존15스킬154 PASS, Search예외 PARTIAL.
2. **안전지역 펫 C**: Main21 완료 후 StarterVillage·Catacomb Entrance 신규 펫 분양/관리, Arbel 기존 보존; 100탈렌트, 사수 존재 판정. 격리 QA106 PASS/0FAIL, 기존Search예외 PARTIAL. Chapter3 스킬 상점 실제 구현 금지.
3. **Chapter3 5직업 기획 D**: 1차 설계 승인(각 신규2종+기본3종 II), 저렴한 탈렌트 개별 구매·최대6칸·전투 밖 무료 교체. 최종 수치/런타임/상품·영속 저장 미승인. 문서만 통합.
4. **Main08 승인 TTS 6개**: `codex/main08-approved-voice-apply-20261013` `79554069d010115a5427eedfc1c109994737f4fb`. WAV 원본 보존, 승인 분리/재활용 WAV와 StoryVoiceCatalog6 변경, 원본 디스크 적용 및 feature Push 완료. QA177PASS/0FAIL·종합PARTIAL, 태온005/006 기존 문제 및 대화 중 반복 몬스터 조우 OPEN.
5. **Field04 공식 리뉴얼**: `codex/field04-v2-official-42-20261013` `ec469c0aed66b58e8a8ee9e842c47c3d78c7b675`. 사용자가 ZIP과 **Road42=30+12 최종 승인**, 환경 Renderer375, 승인 PNG52·Optional Story3 미배치. 원본 디스크 적용/feature Push 완료. 253PASS/0FAIL·종합PARTIAL.
6. **최신 기획/아트·TTS 담당 문서**: `docs/planning`의 42안 승인·Main08 청취/원격 상태·팀 운영/세션 인계. 리뉴얼 `docs/art-renewal` 및 TTS `docs/tts`의 승인된 진행 이력은 기획과 정합 검토하여 **완료 기록만** 선별 병합. 다른 아직 미승인 자산·소스 코드·샘플 생성을 끌어오지 않는다.

## 병합 충돌 처리 지시

- 별도의 격리 worktree에서 `origin/codex/eod-integration-20261013`를 기반으로 준비. checkout 전 원본 dirty tree/HEAD/Save/Scene/승인WAV/meta SHA baseline 캡처. 사용자 원본 Editor 및 GameView/Play/OS 포커스 조작 금지.
- **Field04** 동일 Scene의 LegacySpriteRegistration 변경과 공식 v2 Road42 변경은 해당 **Scene 기능/트리/Renderer GUID 병합**이 필요하다. 정식 결과는 42도로/375Renderer/PNG52, Scene GUID/meta 보존. Legacy 구형 PNG7개를 무리하게 삭제하거나 다른 Scene 참조를 파괴하지 않는다. 공식42 변형으로 다른 이전 Sprite 누락362건을 재도입하지 않도록 Unity 실제 렌더러참조 검사. 42안의 기존 Spawn/북쪽 경계 OPEN은 유지.
- **공통 문서** `CURRENT_STATUS.md`, `CODEX_LATEST.md`, 기획 문서는 로컬 보존 이력/PR7/B/C/Importer/Main08/Field04 모두 읽어 한쪽 덮어쓰기·기계적 ours/theirs 전체 채택 금지. 최신 상태를 맨 위에 기록하고 역사적 완료/미완 로그 유지. 문서 충돌은 코드 성공으로 간주하지 않는다.
- 아래 메타데이터·바이너리는 안전한 **정확한 SHA에 해당하는 정식 자산만** 유지한다. Approval 없이 TTS API 재생성, 아트 재제작, Asset GUID 변경/삭제, 기존 다른 담당 변경 혼합 금지.
- GitHub PR #8·#9의 충돌은 각 소스 브랜치를 force-push하지 않고 통합 브랜치에서 해결하고, 중복 PR은 완료 후 별도 정리한다. 가능한 경우 Git merge 부모를 보존. `main`에 직접 Commit/Push 금지.

## 최종 합본 QA / main 반영 조건

- 통합된 결과 전체에서 Unity 격리 Batch Compile/Runtime 및 Search 예외 원문 보존, Missing Script/Sprite/GUID/Scene0, Field04 Road42/Renderer375, Main08 Voice6 실제 연결/PCM 검사, 기존 B+C 스킬/펫 공존, 기존 15스킬 및 Dialog Next, Main08·Main12~13 Quest·세린·전투·지체의 길 동선·Trigger·35초 리스폰·Save/Continue, 주요 거점/월드 UI, 새 생성 GUID 충돌 검사 및 `git diff --check` 확인.
- 보호: 원본 5천여 SHA baseline과 사용자 dirty, 원본 ProjectVersion, Unity Settings, Save, TTS/BGM, 공식 승인 PNG, 원본 Editor/OS 포커스·Play 상태 불변.
- 모든 기존 미해결은 **PARTIAL/OPEN**: Field04 서쪽Spawn·TreeWest 겹침/북쪽 Collider틈, UnityEditor.Search 기존예외, Unity 6000.5.7f1 정본 vs 6000.6.5f1 실제 QA 차이, 실물 사람 입력/가독성, 스킬 Save 영속 저장 미구현, 태온005/006 음성 문제.
- **합본 검증 전 PR #10은 Draft 그대로**. 검증 증거·병합 후 파일 구성과 알려진 예외가 정확히 기록되었을 때만 사용자의 이번 "합쳐줘" 승인을 토대로 PR #10을 Ready→Merge하여 main 원격 SHA 확인. 막히면 무리하게 병합하지 않고 남은 항목을 보고. 기존 PR #7 등의 중복병합 금지.
- 실제 `main` 병합 성공을 선언하기 전에 원격 main 새 HEAD와 대상 음성 WAV6/필드Scene Road42/B/C코드·기획문서 존재를 GitHub에서 확인할 것.

## 퇴근 직전 현재 사실

- GitHub 브랜치/PR10 생성 완료, 직접 main 작업0.
- **PR8/PR9는 실제 mergeability=false**이므로 기획 담당 GitHub 연결만으로는 현재 충돌 해결 및 합본 런타임 QA를 완료할 수 없음. 위 검증이 끝날 때까지 안전을 위해 main은 변경하지 않았다.
