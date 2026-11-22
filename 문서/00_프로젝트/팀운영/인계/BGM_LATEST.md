# BGM_LATEST — 2026-10-20

## 현재 역할
Chapter3 BGM 담당. 담당 브랜치 `docs/bgm`. main 직접 commit/Push/merge 금지.

## 최신 발주
Issue #31: F1 북풍·서릿길 마을 2곡 샘플.

## 현재 완료
- docs/planning Chapter3 병렬 제작 킥오프 확인.
- Main22 F1 / Main23~24 서릿길 마을 최신 기획 확인.
- 2곡의 제작 프롬프트와 QA 계약 확정.
- 제작 브리프: `문서/00_프로젝트/Chapter3_BGM_Pilot_Production_Brief_20261020.md`

## 상태
- F1 데모: PROMPT_LOCKED / AUDIO_GENERATION_PENDING
- 서릿길 마을 데모: PROMPT_LOCKED / AUDIO_GENERATION_PENDING
- 사용자 청취: PENDING
- SHA/decode/loop QA: PENDING
- Unity 적용: NOT AUTHORIZED / 0
- 설빙의 폭군 전체 보스곡: NOT AUTHORIZED / 0

## 보호
기존 Chapter1·2 공식 BGM, TTS, Unity Audio/Mixer/Scene/Save 원본 변경0.

## 다음 순서
1. 위 프롬프트로 각 1차 데모 생성(45~90초 권장).
2. MP3/WAV 수령 즉시 SHA256·decode·길이·루프·TTS 공간 검수.
3. 사용자 청취 승인.
4. 승인 후에만 Unity 적용용 별도 Handoff 작성.


## 2026-10-20 파일럿 2곡 실제 수령·1차 청취 QA

- `Where_the_Grass_Bends.mp3`: 74.239958초 / 44.1kHz Stereo / SHA256 `2bbbe6fe02b46c391653a5409802dde819ba64b5b5be99b20bbeafb4ad9b0fca`. Main22 F1 역할 **PASS 추천**, USER_REVIEW_PENDING.
- `Hearth_in_the_Low_Valley.mp3`: 72.620333초 / 44.1kHz Stereo / SHA256 `a8cae73d15d276ad5cf25415ee13bce6e9761b331822f8dcb5928da9b87c772b`. Main23 서릿길 마을 역할 **PASS 추천**, USER_REVIEW_PENDING.
- Raw MP3 direct Loop: 두 곡 모두 끝 무음(각 약2.36초/2.55초) 때문에 **LOOP_EDIT_REQUIRED**.
- 두 곡 역할 분리/길이/decode PASS. Unity 적용0, 기존 공식 BGM/TTS/Mixer 변경0.
- 세부: `문서/00_프로젝트/Chapter3_BGM_Pilot_2Track_Listening_QA_20261020.md`


## 2026-11-23 Issue #31 후속 — Loop Edit 준비 / 소형 A-B

- 최신 지시 확인: **신규 재생성 금지**, 기존 두 후보 보호, 편집 계획/A-B까지만.
- 원본 SHA 재확인 PASS:
  - `Where_the_Grass_Bends.mp3` = `2bbbe6fe02b46c391653a5409802dde819ba64b5b5be99b20bbeafb4ad9b0fca`
  - `Hearth_in_the_Low_Valley.mp3` = `a8cae73d15d276ad5cf25415ee13bce6e9761b331822f8dcb5928da9b87c772b`
- 11/19 복구 ref까지 대조했으나 두 곡의 **명시적 사람 청취 승인 문구는 복구하지 못함**. 따라서 최종본/공식 Master 아님. `APPROVAL_RECOVERY_NOT_FOUND / USER_LISTENING_REQUIRED`.
- Loop 후보:
  - F1: Intro 0–14.80s, Body 14.80–68.05s, 0.55s crossfade 후보.
  - 마을: Intro 0–4.32s, Body 4.32–60.72s, 0.55s crossfade 후보.
- 소형 A/B 4개 생성. 원본 수정0, GitHub binary 등록0, 배포 Master0.
  - Grass A `30a415e9bd9c0fbf98593dcf4bb5ad7f4227cf312ef9ead4597e8e89fa680735`
  - Grass B `d369f299f2da794ce3ff4921fb3a402ca450bd426872483eb8bbfd40ea46877a`
  - Hearth A `1f7ce6597f2fe10a2918a74cebfa3aadbaeb1df2baceaa85969e9ab1e00267c7`
  - Hearth B `e6c1cc562d7d4a6da328749a063a3ac37a9de0e4703136801c63d189d1120e17`
- 4개 decode PASS. 추가 gain/EQ/compression0. 사람 청취의 박/프레이즈 및 실제 TTS 동시재생은 PENDING.
- Unity 6000.6.5f1 / Mixer / TTS / Save / UserData / 승인 자산 변경0. Issue #85 HOLD 유지.
- 상세 QA: `문서/00_프로젝트/Chapter3_BGM_LoopEdit_AB_QA_20261123.md`
- 다음 게이트: 사용자 A/B 청취 승인 후에만 별도 Master/Loop 제작. Unity 적용은 별도 Codex 계약.


## 2026-11-23 승인 상태 정정 — 2026-10-20 대화 근거 복구

- 이전 GitHub 인계의 `USER_REVIEW_PENDING`은 **기록 누락**으로 정정한다.
- 2026-10-20 BGM 대화에서 바울 대표가 두 곡을 **“둘 다 좋고”**라고 직접 승인한 기록을 다시 확인했다.
- 따라서:
  - `Where_the_Grass_Bends.mp3` = **USER_LISTENING_APPROVED / Main22 F1 승인곡**
  - `Hearth_in_the_Low_Valley.mp3` = **USER_LISTENING_APPROVED / Main23 서릿길 마을 승인곡**
- 원곡 사용 승인 재확인 불필요.
- 다만 Raw MP3의 종료 무음 문제는 별도 기술 이슈로 `LOOP_EDIT_REQUIRED` 유지.
- 2026-11-23 생성한 소형 A/B는 **새 루프 편집안**이므로 `LOOP_EDIT_APPROVAL_PENDING`.
- 배포 Master / Unity 적용은 여전히 0, 별도 승인 전 보류.


## 2026-11-23 Issue #31 LoopPlan 기술 검증 완료

- 두 원곡은 이미 **USER_LISTENING_APPROVED**. 재승인 요청 금지.
- 원본 SHA 재확인 및 보호:
  - Grass `2bbbe6fe02b46c391653a5409802dde819ba64b5b5be99b20bbeafb4ad9b0fca`
  - Hearth `a8cae73d15d276ad5cf25415ee13bce6e9761b331822f8dcb5928da9b87c772b`
- B 방식 경계 미세 조정:
  - Grass Intro 0–14.60s / Body 14.60–68.10s / xfade0.55s
  - Hearth Intro 0–4.35s / Body 4.35–60.80s / xfade0.55s
- 기술 QA:
  - B 두 곡 경계 연속 무음 0초.
  - hard click spike 기계검사 PASS.
  - 자동 onset/beat continuity PASS. Grass는 crossfade 내부 약한 완급 변화가 있으나 누락 beat/gap 없음.
  - 4개 비교 MP3 전체 decode PASS.
- Review ZIP: `LIMITLESS_Issue31_LoopReview_20261123.zip`, 2,199,087bytes, SHA256 `65a37dc8322a0eb5ed6e52527de1ac0754a5bd3252e108dd6dd56bd1406a3484`.
- GitHub binary 등록0. 원본 수정0. 배포 Master0. Unity6000.6.5f1/Mixer/TTS/Save/UserData 변경0.
- 상태: **B_LOOPPLAN_TECH_PASS / LOOP_EDIT_USER_APPROVAL_PENDING / MASTER_NOT_BUILT / UNITY_NOT_APPLIED**.
- 상세: `문서/00_프로젝트/Chapter3_BGM_LoopEdit_AB_QA_20261123.md`
