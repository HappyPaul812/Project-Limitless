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


## 2026-11-23 Issue #31 최종 Intro·Loop Master 제작 완료

- Issue #31 최신 기록에서 바울 대표의 **B_LOOP_PLAN_USER_APPROVED** 확인.
- 두 승인 원곡 SHA 불변:
  - Grass `2bbbe6fe02b46c391653a5409802dde819ba64b5b5be99b20bbeafb4ad9b0fca`
  - Hearth `a8cae73d15d276ad5cf25415ee13bce6e9761b331822f8dcb5928da9b87c772b`
- 최종 납품 형식: WAV PCM16 / 44.1kHz / Stereo.
- Grass:
  - Intro 15.15s SHA `55443ce14f9d7fd86e53a95d03d04ab98eb2a70ce3f8ed260cba81e0e1713261`
  - Loop 52.95s SHA `eb49138171a85e95856c2d1d5f96ae21de92f4a5bafe4c5726c46f8f499e69cd`
- Hearth:
  - Intro 4.90s SHA `f6fb974b363bcb881cfbb7264a8a97c87c49fd4d6d298f11bc69b4a7329c2561`
  - Loop 55.90s SHA `0164f502afda08f17b4a13ec5922e8feadcef7aa5d932300fa097fb45bf747ce`
- 최소 QA: 4 WAV decode PASS, Loop 경계 무음0, hard-click 검사 PASS, 승인 B transition 보존, gain/EQ/compression0.
- 납품: `LIMITLESS_Issue31_FinalLoopMasters_20261123.zip`
  - 21,459,175bytes
  - SHA256 `3c9810962e1a1db11117d5a3fe6ee8ce5caa4fcc62cc6889656aff7ac7d39c94`
  - CRC PASS
- 원본 수정0 / 신규 음악 생성0 / 유료 API0 / GitHub binary0.
- Unity6000.6.5f1, BGM/TTS/Mixer/Save/UserData 변경0.
- 상태: **FINAL_MASTER_DELIVERED / UNITY_INTEGRATION_PENDING_CODEX / MAIN_MERGE_NOT_DONE**.
- 상세: `문서/00_프로젝트/Chapter3_BGM_LoopEdit_AB_QA_20261123.md`


## 2026-11-23 Issue #127 — BGM Gemini 제작 전담 수용 / F2·F3 착수

- 기획 정책 `BGM_WORKFLOW_ROLE_BOUNDARY_20261123.md` 확인 및 **ROLE_BOUNDARY_ACCEPTED**.
- 앞으로 Gemini/Lyria 생성 프롬프트 작성·수정·제작 진행은 BGM 담당 책임. 기획 담당에게 프롬프트 작성/도구 조작을 요청하지 않는다.
- Issue #127 범위:
  - F2 첫눈의 숲 신규 데모1
  - F3 눈 덮인 고갯길 신규 데모1
- 기존 #31 F1/서릿길 승인 원곡·최종 Intro+Loop Master 재생성/QA 반복0.
- F2/F3 BGM 최종 프롬프트 v1 작성 완료. 상세: `문서/00_프로젝트/Issue127_F2_F3_BGM_Production_Brief_20261123.md`
- 현재 ChatGPT BGM 세션에는 Gemini 음악 생성기를 직접 조작하는 연결이 없음. 플러그인 검색에서도 Gemini music 직접 연결 미발견.
- 따라서 정책에 따라 대표의 최소 수동 조작 필요:
  1. Gemini → Create music
  2. Short
  3. Instrumental/No vocals
  4. BGM 담당 프롬프트 붙여넣기
  5. 각 곡 1회 생성
  6. MP3 다운로드 후 이 세션에 업로드
- 신규 유료 가입/결제/API/키조회0.
- 상업 이용 근거: Google이 생성 원본 콘텐츠 소유권을 주장하지 않는 일반 약관 확인. 단, 무료 Gemini/Lyria 음악의 유료 게임 사용을 명시적으로 보장하는 별도 상업 라이선스는 현재 공식 문서에서 확인하지 못해 `COMMERCIAL_GAME_USE_EXPLICIT_LICENSE_NOT_VERIFIED` 유지.
- 상태: **PROMPTS_LOCKED / GEMINI_MANUAL_STEP_REQUIRED / AUDIO_NOT_YET_GENERATED / UNITY_NOT_APPLIED**.
- Unity6000.6.5f1, 기존 승인 BGM/TTS/Mixer/Save/UserData, Issue #85 HOLD 보호.


## 2026-11-23 Issue #127 — F2/F3 1차 데모 실제 수령

- F2 `Beneath_the_Frozen_Boughs.mp3`
  - 76.4865s / 44.1kHz Stereo
  - SHA256 `c3f1339e30d8973ad467e30650072f9fc2360ea1f90627a3570bfcaeb384e3b1`
  - Size 1,843,393bytes
  - 체감 약78.3BPM, -11.33LUFS, peak -0.09dBFS
  - 끝 무음 약1.91s
  - F2 역할 **PASS 추천 / USER_LISTENING_PENDING**
- F3 `Steps_on_Frozen_Stone.mp3`
  - 92.4996s / 44.1kHz Stereo
  - SHA256 `ce3aa05a5235791892dd222bb8e958084634c085f69edfae0ec31850ec6d20e7`
  - Size 2,227,707bytes
  - 체감 약80.7BPM, -11.65LUFS, sample peak 약-0.006dBFS, clipping0
  - 끝 무음 약2.67s. 실음악 약89.83s → 재생성 없이 향후 꼬리 제거로 길이 규격 충족 가능.
  - F3 역할 **PASS 추천 / USER_LISTENING_PENDING**
- 두 곡 decode PASS, 역할 분리 PASS 추천, 원본 gain/EQ/compression 변경0.
- tentative loop 후보:
  - F2 12.8→62.4s
  - F3 24.5→84.1s
  - 아직 Loop 편집/마스터 제작0.
- 기존 F1/서릿길 재생성0, Unity6000.6.5f1/TTS/Mixer/Save/UserData 변경0.
- 상용권: `COMMERCIAL_GAME_USE_EXPLICIT_LICENSE_NOT_VERIFIED` 유지.
- 상세 QA: `문서/00_프로젝트/Issue127_F2_F3_BGM_FirstDemo_QA_20261123.md`
- 다음: 바울 대표 두 곡 실제 청취 승인 → 승인곡만 Loop A/B 편집 단계.


## 2026-11-23 Issue #127 — F2/F3 사용자 청취 승인

바울 대표 직접 피드백 “나도 좋아.”를 두 데모의 실제 청취 승인으로 접수.
- `Beneath_the_Frozen_Boughs.mp3` → **F2_USER_APPROVED**
- `Steps_on_Frozen_Stone.mp3` → **F3_USER_APPROVED**
- 재생성0, 원본 SHA 변경0.
- 다음: 승인된 두 곡만 Loop A/B 편집·검증.
- Master/Unity 적용/main 병합0.
- 상업권 `COMMERCIAL_GAME_USE_EXPLICIT_LICENSE_NOT_VERIFIED` 유지.


## 2026-11-23 Issue #127 — F2/F3 Loop A/B 준비

- 두 곡 자체 사용자 승인 완료.
- 원본 SHA 보호:
  - F2 `c3f1339e30d8973ad467e30650072f9fc2360ea1f90627a3570bfcaeb384e3b1`
  - F3 `ce3aa05a5235791892dd222bb8e958084634c085f69edfae0ec31850ec6d20e7`
- B Loop 후보:
  - F2 13.560→62.775s / xfade0.55s
  - F3 24.102→81.107s / xfade0.55s
- 두 B preview 경계 near-silence 최대0.02s, hard-click delta가 내부99.9 percentile 이하 → 기술 PASS.
- Review ZIP `LIMITLESS_Issue127_LoopReview_20261123.zip`
  - 1,168,179bytes
  - SHA256 `71cd20113303053b74ed77230089d2e48bfd232f117e9c78c638555d85954b8e`
- 상태: **LOOP_AB_READY / B_LOOP_TECH_PASS / LOOP_USER_APPROVAL_PENDING / MASTER_NOT_BUILT / UNITY_NOT_APPLIED**.
- 상세: `문서/00_프로젝트/Issue127_F2_F3_LoopReview_QA_20261123.md`


## 2026-11-23 상업 사용권 상태 정리

- 초기 안내에서 “상업 사용 가능”을 너무 단정적으로 표현한 점을 BGM 담당 기록상 정정.
- 현재 공식 문서 기준:
  - Google이 생성된 원본 콘텐츠에 소유권을 주장하지 않음: **CONFIRMED**
  - 상업 사용 금지 조항: **NOT FOUND**
  - 무료 Gemini/Lyria 음악의 유료 게임 사용을 명시적으로 보장하는 전용 라이선스 문구: **PENDING_FURTHER_CONFIRMATION**
- 상태 표기는 `COMMERCIAL_USE_EXPLICIT_LICENSE_PENDING`.
- 이는 `COMMERCIAL_USE_FORBIDDEN`이 아님.
- 기존 승인 BGM 및 Issue #127 F2/F3는 계속 보존·제작/검수 진행. 폐기/재생성 지시 없음.
- 상용 출시 전 권리 근거를 추가 확인하는 별도 게이트만 유지.


## 2026-11-23 Issue #127 — F2/F3 Final Intro+Loop Master 완료

- 바울 대표의 B LoopPlan 최종 승인 확인.
- 승인 원본 SHA 보호:
  - F2 `c3f1339e30d8973ad467e30650072f9fc2360ea1f90627a3570bfcaeb384e3b1`
  - F3 `ce3aa05a5235791892dd222bb8e958084634c085f69edfae0ec31850ec6d20e7`
- 최종 형식: WAV PCM16 / 44.1kHz / Stereo.
- F2:
  - Intro 14.110454s SHA `090bf9417981173fb9cfc5fd998786413aa8b06c58c7e5b3157bb4e45b050504`
  - Loop 48.664694s SHA `8cefeeea2735d28efa796b987254ab6af96be1cbe297062622f029529a55327f`
- F3:
  - Intro 24.652313s SHA `9b157be7560f30c5401a7a225431cc791214d1933e4154355e6fca6cb8296036`
  - Loop 56.454989s SHA `09609b95da9f907a98079ad659c4ef8b559e9324b410d22e0e9c96df0662c1f8`
- Master QA: 4 WAV decode PASS, Loop 경계 무음0, hard-click 검사 PASS, Intro→Loop handoff PASS, 추가 gain/EQ/compression0.
- 납품 ZIP:
  - `LIMITLESS_Issue127_FinalLoopMasters_20261123.zip`
  - 24,120,430bytes
  - SHA256 `0d0a3a098440edd9f8805cff02891db67530561d63b715bf4d46288d9fe32c2b`
  - CRC PASS
- ZIP 포함: WAV4 + MANIFEST.json + SHA256SUMS.txt + README.txt.
- 신규 Gemini 생성0 / 재편곡0 / 원본 수정0 / GitHub binary0.
- 상태: **FINAL_MASTER_DELIVERED / MASTER_QA_PASS / UNITY_INTEGRATION_PENDING_CODEX / MAIN_NOT_MERGED**.
- 상업권은 `COMMERCIAL_USE_EXPLICIT_LICENSE_PENDING`으로 출시 전 별도 관리.
- 상세: `문서/00_프로젝트/Issue127_F2_F3_FinalLoopMasters_QA_20261123.md`


## 2026-11-24 Issue #129 — F4 눈보라 구간 BGM 브리프/프롬프트

- 최신 Issue #129 및 `20261124_Parallel_Department_Orders_While_Issue85_Repair.md` 확인.
- 기존 완료 보호:
  - F1 `Where the Grass Bends`
  - 서릿길 마을 `Hearth in the Low Valley`
  - F2 `Beneath the Frozen Boughs`
  - F3 `Steps on Frozen Stone`
  - 승인 Intro/Loop WAV8
- 기존곡 재생성/재편집/기술 QA 반복0.
- F4 방향:
  - F3보다 더 춥고 위험한 탐험
  - 눈보라/노출/정령성 또는 이상현상 감각 강화
  - F3의 low strings + forward pulse 계보는 유지
  - 화성/고역 texture는 더 차갑고 불안정하게
  - F5/최종 보스 대비를 위해 대형 brass/drum/choir/climax 금지
  - 80~90BPM, 60~85s 권장(45~90s 허용), TTS midrange 공간 확보
- Gemini/Lyria 생성 프롬프트 v1 작성 및 잠금.
- 상세: `문서/00_프로젝트/Issue129_F4_Blizzard_BGM_Production_Brief_20261124.md`
- 상태: **BRIEF_LOCKED / GEMINI_PROMPT_LOCKED / AUDIO_GENERATION_NOT_AUTHORIZED / AUDIO_FILE_0 / UNITY_CHANGE_0**
- 상업권: `COMMERCIAL_USE_EXPLICIT_LICENSE_PENDING` 유지.
- 실제 생성은 바울 대표 별도 승인 후 Gemini Short / Instrumental / 1회만.
- F4 실제 Scene/정식 런타임 배정은 NOT_VERIFIED. 새 적명/Scene ID 임의 확정 없음.
- Unity6000.6.5f1, 기존 승인 BGM/TTS/Mixer/Save/UserData, #85 HOLD 보호.


## 2026-11-24 Issue #129 — F4 데모 수령

- Breath_Upon_The_Pass.mp3 / 73.168917s / 44.1kHz stereo / 1,763,772bytes.
- SHA256 f0142ee5e0c7a30c24aead0c483aa9a3b10de97e1d1c0e6318c039157f8a3b47.
- 전체 decode PASS, 길이 PASS, -11.5 LUFS, true peak +0.2 dBTP, decoded sample peak +0.157 dBFS(19 channel samples > 1), 끝 저레벨 약2.48s.
- F4 탐험곡 적합 PASS 추천. 원곡 청취 승인 대기. 피크 여유 및 루프 편집은 승인 후 별도 파일로 진행. A/B0 / Master0 / Unity0 / main0.
- COMMERCIAL_USE_EXPLICIT_LICENSE_PENDING. 기존 승인곡4/WAV8 및 #85 HOLD 보호.
- 보고: 문서/00_프로젝트/Issue129_F4_BGM_FirstDemo_QA_20261124.md


## 2026-11-24 Issue #129 — F4 원곡 승인 및 Loop A/B

- 대표 직접 “승인.” → Breath Upon The Pass 원곡 USER_APPROVED. 원본 SHA f0142ee5e0c7a30c24aead0c483aa9a3b10de97e1d1c0e6318c039157f8a3b47 전후 일치.
- B 후보 522444→2611336 samples @44100Hz (11.846803→59.213968s), xfade 24255 samples/0.55s, A/B 동일 -2dB gain. A 저레벨 2.73s, B 0s, true peak A -2.2/B -1.9dBTP. decode 및 기계적 경계 검사 PASS.
- ZIP LIMITLESS_Issue129_LoopReview_20261124.zip / 3843795 bytes / SHA256 6c565077e19c3281bba835aec720ddfa013d73441f9808aa1106a86cbb5feac1 / CRC PASS.
- 상태: ORIGINAL_USER_APPROVED / LOOP_AB_READY / B_LOOP_TECH_PASS / LOOP_USER_APPROVAL_PENDING / MASTER_NOT_BUILT / UNITY_NOT_APPLIED.
- 다음: B 및 -2dB gain 청취 승인 후 Master. 상업권 COMMERCIAL_USE_EXPLICIT_LICENSE_PENDING. 기존 승인곡4/WAV8 및 #85 HOLD 보호. Unity/main 변경0.
- 상세: 문서/00_프로젝트/Issue129_F4_LoopReview_QA_20261124.md
