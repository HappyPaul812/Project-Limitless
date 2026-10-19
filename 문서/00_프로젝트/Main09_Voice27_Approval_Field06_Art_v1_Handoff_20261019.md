# LIMITLESS / 2026-10-19 Main09 복구 음성 27개 사용자 청취 승인 + Field06 아트 ZIP 로컬 인계

- 발행: 기획 / 브랜치 `docs/planning`.
- 상태(분리): **MAIN09_27_USER_LISTENING_APPROVED / MAIN09_OFFICIAL_AUDIO_INTEGRATION_AUTHORIZED / MAIN09_ORIGINAL_UNITY_NOT_APPLIED / FIELD06_ZIP_PATH_USER_REPORTED / FIELD06_USER_ART_REVIEW_PENDING / FIELD06_UNITY_NOT_APPLIED**.
- 최신 선행: `Main09_Field03_DialogueVoice_Sequence_Bug_20261019.md`, `Main09_Quest_Clarity_Unity_QA_20261019.md`, `CURRENT_STATUS.md`, 리뉴얼 `docs/art-renewal`의 `Field06_ScorchedTrail_Renewal_Art_v1_Delivery_20261019.md`.
- 변경 범위: **문서화만**. 이 문서는 실제 Windows 로컬 파일 수정, Unity 실행, WAV 생성/교체, main Push/병합을 수행했다는 의미가 아니다.

## A. Main09 보이스 최종 사용자 청취 승인 (기획 수락 게이트 해제)

바울 대표는 Codex의 `codex/main09-dialogue-voice-fix-20261019` 최종 보고(원본 WAV 잘못된 분할/합본 발견, GUID↔Catalog/meta 27개 모두 일치, 새 음원 아닌 기존 발화 복구 후보 27개, 격리 Unity **550 PASS/0 FAIL**, 원본 4,180파일·UserData23 SHA 보호, TTS/API0, 원본 적용0, main병합0)를 받은 후 **“다 들어봤는데 다 맞아”**라고 답함. **27개 후보의 ID별 실제 문장 짝에 대한 사용자 청취 승인 전체 완료**로 해석하며, 우선 청취 보류7개 모두 USER_APPROVED로 승격. 기존 ASR 20개 동일/7개 우선 청취의 구분은 이력으로 보존. 각 후보의 exact WAV bytes/SHA/PCM 및 대응 CSV는 Codex 보고의 **최종 산출물**을 정본으로 사용하고, 이 문서에서 임의 SHA를 생성·재지정하지 않는다.

- **확정 서사 순서:** 폴 `main09_paul_002` “또 뵙네요”가 Step2 폴 첫 대면 첫 발화. 기존 `main09_paul_001` 길 안내는 Step1 선행에서 제거하고 Step2 인사 이후 정확히 한 번, 기존 원문/DialogueId 그대로. Step0~5 6목표/EXP35·탈렌트35/폴 합류/Save unchanged.
- **후속 실행 허가:** Codex는 승인된 정확한 후보 27개만 feature worktree에서 Unity VoiceCatalog 연결·GUID/meta·PCM 교정 및 코드 재회 순서 반영, delta QA, 원본디스크 비파괴 반영을 준비할 수 있다. 실제 원본 적용은 1) 사용자 dirty와 Source 충돌0 확인, 2) 기존 승인 WAV/meta/GUID와 VoiceCatalog 기존 entry 보호, 3) 백업·SHA 매칭, 4) 원본Editor/Save/포커스 무조작 조건 충족에 한정한다. 안전 조건 불충족 시 **ORIGINAL_APPLY_BLOCKED**로 보고; 해당 문제를 제외하고 리허설/격리 QA까지만 한다.
- **금지:** 27개를 재TTS하거나 API를 호출하지 않는다. 기존 WAV 소스 덮어쓰지 않는다. Main08/18/19/20/21 자산/다른 퀘스트, Field05, UserData, Scene 변경 금지. 원본 Editor Play·Refresh·OS 포커스 직접 변경 금지. main 직접 커밋/강제 Push 금지; 공식 원격 Push/PR/병합은 바울 대표의 별도 Git 통합 지시를 따른다.
- **검수:** 승인된 27개 `dialogueId↔speakerId↔text↔sourceSha↔finalClipGuid↔wav.meta` 1:1, 중복/무음/합본/말잘림0. Step0~5에서 대화 순서·자막/실발화·Player무음, 다음 페이지 중단/취소/빠른Next, Save/Continue, 기존 Main09 Quest Guide, 보상·로스터 한번씩, 기존 보호 WAV/Code 정적 및 필요한 delta Runtime QA. 기존 격리 **550 PASS** 전체 재수행 금지. 원본 인간 실플레이는 별도 확인으로 구분.

## B. Field06 v1 사용자 다운로드 경로 인계 — 미술 승인은 별개

바울 대표는 로컬 파일을 **`F:\Downloads\Limitless_Field06_ScorchedTrail_Renewal_Art_v1.zip`** 경로로 제시함. 이는 사용자 제공 경로이며 **현재 이 기획 세션이 F드라이브 파일 존재·해시를 직접 검사한 것은 아니다**.

리뉴얼 `docs/art-renewal` 공식 납품 정본:
- `Limitless_Field06_ScorchedTrail_Renewal_Art_v1.zip`
- 보고된 예상 SHA256 `d01f797dd5566c33c6bf917a397c2032f5fc44fcf31a53dc328807e912a23b70`, 크기 **1,362,496 bytes**, ZIP38파일, PNG28개(신규 실사용후보22 + 프리뷰/QA6).
- 정적 대체 후보 **14 PNG** = Ground3 + 동서 Road4 + Props7; 선택 **8 PNG** = Main15 Runtime Quad 대응6 + 추가 Decal2 (**선택 승인/구현0**).
- Field06 현행 Static SpriteRenderer **363 = Grass315 / Road30 / Props18**. Runtime MeshRenderer Patch6은 Sprite PNG만 바꾼다고 자동 대체되지 않음. Runtime Grass 중 비활성 홀 발생 가능. 실제 거리/Collider/Scene 원본 그대로.
- 기존 Road30 **남북형**. `A30`(x=-4~10/y±0.5, 기존30재배치) 및 `B42`(x=-10~10/y±0.5, 신규12추가)의 **동서 Road 설계 후보만** 납품. Field05 B42의 사용자 선호는 Field06의 B42 공식 미술 승인으로 자동 승계되지 않음. B42를 시각 검토 우선 후보로 둘 수 있으나 최종 결정은 바울 대표 실제 화면 확인 후.
- **상태: ART_DELIVERED/OFFLINE_QA_PASS/LOCAL_ZIP_VERIFICATION_PENDING/USER_ART_REVIEW_PENDING/UNITY_NOT_APPLIED**.

**Codex 순서:** Main09 신규 검증을 반복하지 않고 공식 음성 연결 마감을 우선. 그다음 Field06 ZIP의 실제 존재·SHA/CRC/Manifest/PNG/권리 정보 **읽기 전용 확인**; 무결성 문제시 STOP. A30/B42 격리 Unity 미술 비교만 진행하고 Preview는 실제 Unity와 합성 문서를 구분. Main15 12단계/4 Encounter/세린 정식 합류/Arbel↔Field06/Save·Continue/휠체어/Camera·Runtime Grass/Tint/6 Patch 안정성 중 새로운 Road 위치로 인한 회귀만 우선 평가. 사용자 미술 승인 전 **원본디스크 Scene 변경, Runtime6/Decal2 삽입, B42 선택 확정, main 병합 금지**. 아직 승인되지 않은 범위가 있으면 검증만 하고 보고.

## C. 다음 단계 의사결정 게이트

1. **Main09**: 사용자 청취 승인 완료 → 승인 후보 SHA 기반 정식 연결+delta QA+비파괴 원본 적용(가능한 경우) → Git feature/PR/main 통합은 별도 지시 필요.
2. **Field06**: 로컬 ZIP 사용자 경로 확보 → Codex read-only checksum/격리 A/B 실제 Scene 뷰 → 바울 대표 **정적14/선택8 여부 및 A30/B42 미술 승인** → Codex 정식 Unity 구현/검증(별도 단계).
3. 완료보고는 작업별 `USER_APPROVED / ISOLATED_QA_PASS / ORIGINAL_APPLIED / REMOTE_PR_MERGED / ORIGINAL_EDITOR_RUNTIME_NOT_VERIFIED`를 분리하고 브랜치·커밋·백업SHA를 명시.
