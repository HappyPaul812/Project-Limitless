# LIMITLESS / TTS Issue #125 / 2026-11-24 — Main24 Stage10 보호·증거표 / Main25 Stage9 사전 준비

**상태:** `MAIN24_STAGING_10_USER_LISTEN_PASS_FILE_PROOF_PARTIAL / MAIN25_9_STAGING_PREP_ONLY / NEW_API_POST_0 / NO_UNITY_CHANGES`  
**GitHub 브랜치:** `docs/tts` (main 직접 수정/병합·원격 main push 없음)  
**최신 발주:** [Issue #125 11/24 기획 지시](https://github.com/HappyPaul812/Project-Limitless/issues/125#issuecomment-7115940550) 및 [11/24 타부서 병렬 발주](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/20261124_Parallel_Department_Orders_While_Issue85_Repair.md)  
**정본:** [Chapter3 72행 승인 큐](https://github.com/HappyPaul812/Project-Limitless/blob/docs/tts/Tools/TTS/Chapter3_Protagonist72_ApprovedQueue_20261123.json) (`docs/tts` blob SHA `93c6920ae7ea31312044333deae82a97375cc67a`). 11/19 승인 문장·5인 캐스팅 변경 금지.

## 1. Main24 10개 — 정확 원문/화자/성우/StageID 대응과 증거 등급

- 대표 직접 제작·청취 승인 **10/10 USER_PASS**(11/23). **재합성 금지**, 기존 생성 STAGE WAV 유지.
- 1~3번은 **사용자가 제공한 기존 생성기의 `[CREATED]` 콘솔 출력에 Stage ID와 SHA256 있음**. 아래의 SHA들은 *당시 콘솔값*이지 TTS 담당이 현재 사용자 `F:` 파일을 재해시한 결과가 아님.
- 4~10번은 직전 TTS 담당이 만든 배치7 실행기의 `ROWS`에서 규정한 **예상 StageID** + 사용자 제작·청취 승인으로 연결. 7개 실제 파일명/크기/SHA256/sidecar/PT 원장은 이 세션에서 직접 접근할 수 없어 **NOT_VERIFIED**.
- Stage ID와 `candidate_id` 모두 **Unity Runtime Dialogue ID가 아니다**. 최신 read-only Runtime 감사에서 Main24 정식 대사 연결 **0/10 HOLD**. 새 구현·가짜 ID 승격 금지.

| 순서 | 정본 Candidate ID → Stage ID | 화자 / Voice | 승인 원문(구두점 보존) | 파일 증거 수준 |
|---:|---|---|---|---|
| 1 | `main24_serin_001` → `ch3stage_main24_serin_001` | 세린 / Erinome | 첫눈이네요. 장갑을 더 챙기길 잘했어요. | 콘솔 CREATED+SHA(로컬 실파일 재해시 미확인) |
| 2 | `main24_miel_001` → `ch3stage_main24_miel_001` | 미엘 / Sulafat | 저는 어묵탕부터 생각나는데요? | 콘솔 CREATED+SHA(로컬 실파일 재해시 미확인) |
| 3 | `main24_serin_002` → `ch3stage_main24_serin_002` | 세린 / Erinome | 그건 비상용이에요! | 콘솔 CREATED+SHA(로컬 실파일 재해시 미확인) |
| 4 | `main24_paul_001` → `ch3stage_main24_paul_001` | 폴 / Achird | 두 분 다 준비는 확실하시군요. | 배치7 예상 ID / 사용자 청취PASS; 실파일SHA 미확인 |
| 5 | `main24_serin_003` → `ch3stage_main24_serin_003` | 세린 / Erinome | 날개를 펼칠 때 주변 눈이 한곳으로 모여요. 조심하세요. | 배치7 예상 ID / 사용자 청취PASS; 실파일SHA 미확인 |
| 6 | `main24_haren_001` → `ch3stage_main24_haren_001` | 하렌 / Alnilam | 태온, 네가 괜히 앞에 나설 필요 없잖아. | 배치7 예상 ID / 사용자 청취PASS; 실파일SHA 미확인 |
| 7 | `main24_taeon_001` → `ch3stage_main24_taeon_001` | 태온 / Algieba | 형, 먼저 어떤 공격인지 보고 대비하자는 거야. | 배치7 예상 ID / 사용자 청취PASS; 실파일SHA 미확인 |
| 8 | `main24_haren_002` → `ch3stage_main24_haren_002` | 하렌 / Alnilam | …그래. 계속 말해 봐. | 배치7 예상 ID / 사용자 청취PASS; 실파일SHA 미확인 |
| 9 | `main24_miel_002` → `ch3stage_main24_miel_002` | 미엘 / Sulafat | 한기에 걸린 동료는 다른 사람이 도와야 해요. | 배치7 예상 ID / 사용자 청취PASS; 실파일SHA 미확인 |
| 10 | `main24_paul_002` → `ch3stage_main24_paul_002` | 폴 / Achird | 추위가 더 깊은 곳에서 흘러오는 모양입니다. | 배치7 예상 ID / 사용자 청취PASS; 실파일SHA 미확인 |

기존 콘솔 SHA256(임의 변경 금지):
- `ch3stage_main24_serin_001`: `6bf00d66d0b230cb339d0a68079272a9fc2f1510f254a0cff365751b53f14464`
- `ch3stage_main24_miel_001`: `1097dabc14a3958f51b97e2b7e72eef3751782725b018a9ef1e70806ef1ca7dd`
- `ch3stage_main24_serin_002`: `20d863e8a6888ef25bc793c3aeba608b6f7d5dcc20053905f10191582ed3eeca`

**파일 검증 상태와 필요 증거**: 10개 사용자 청취 PASS와 별개로, ① 실제 10개 WAV 존재·SHA256·형식·길이, ② batch7 progress 7건의 기록과 현 파일 SHA 대조, ③ sidecar 원문/Voice/파일 SHA 스키마 대조, ④ 공유 PT 시도장부/프로젝트·과금 사실, ⑤ 기존 승인 WAV162 보호 확인은 **로컬 미검증**. 배치7 progress는 내부 장부일 뿐 외부 독립검증이 아니다. 기존 WAV941중 메타 부족 **590경로 중복 미확인** 유지. 기록을 채우려고 WAV를 재합성하거나 기존 Unity 프로젝트를 열지 않는다.

**구조화 증거표:** [Main24 Stage10 JSON](https://github.com/HappyPaul812/Project-Limitless/blob/docs/tts/Tools/TTS/Issue125_Main24_10_StageProof_20261124.json) — 10개 StageID·화자·Voice·원문·콘솔 SHA·Runtime null·증거등급·사용자PASS·파일존재 미확인 필드를 명시.

## 2. Main25 정본 9행 — STAGING_PREP_ONLY

- 72행 정본에서 **9/9 원문·구두점·화자·VoiceID·순서를 그대로 추출**. 세린2(Erinome), 폴3(Achird), 태온2(Algieba), 하렌1(Alnilam), 미엘1(Sulafat). 승인 캐스팅 외 신규 인물/음성/감정 강제 설정 없음.
- `main25_*`는 *Candidate ID*, `ch3stage_main25_*`는 *제안 Stage ID*일 뿐, 실제 Stage-only 생성기 계약이나 Runtime ID로 검증된 적 없음. **Runtime ID 9/9 UNKNOWN**, 실 WAV0, API0, 사용자 실행승인 대기.
- 동일 화자+동일 원문 기준 Main25 9개끼리 및 전체 승인 72행 내 **중복 0건**. 기존 승인 WAV162와 **590 미확인 WAV 경로**의 실제 원문/화자/해시 일치 여부는 별도 읽기 전용 검증 전까지 `UNVERIFIED`로 유지. 기존 보관물과 중복0을 주장하지 않음.

| # | 장면 | Candidate ID | 화자 / Voice | 원문 |
|---:|---|---|---|---|
| 1 | 표식1 | `main25_serin_001` | 세린 / Erinome | 이쪽만 서리 모양이 다르네요. 바람 방향과도 맞지 않아요. |
| 2 | 표식1 | `main25_paul_001` | 폴 / Achird | 마력 표식입니다. 이곳에서 흐름이 끊긴 모양이에요. |
| 3 | 표식2 | `main25_paul_002` | 폴 / Achird | 원래 이쪽으로 흘렀다면… 지금은 반대로 끌려가는군요. |
| 4 | 표식2 | `main25_taeon_001` | 태온 / Algieba | 돌아갈 수 있게 길부터 표시하자. |
| 5 | 표식2 | `main25_haren_001` | 하렌 / Alnilam | …그래. 어디로 가는 게 좋겠어? |
| 6 | 갑충 예고 | `main25_serin_002` | 세린 / Erinome | 갑각을 닫으면 공격이 잘 통하지 않을 것 같아요. |
| 7 | 갑충 예고 | `main25_taeon_002` | 태온 / Algieba | 몸을 움츠릴 땐 다른 행동으로 대비하죠. |
| 8 | 승리/다음 | `main25_miel_001` | 미엘 / Sulafat | 북쪽에 쉬던 사람들이 있을지도 몰라요. |
| 9 | 승리/다음 | `main25_paul_003` | 폴 / Achird | 이 흔적을 따라가면 피난처에 닿을 겁니다. |

**구조화 제작 준비 큐:** [Main25 Stage9 JSON](https://github.com/HappyPaul812/Project-Limitless/blob/docs/tts/Tools/TTS/Issue125_Main25_9_StagePrep_20261124.json). `production_status=PREP_ONLY_NO_POST`, `runtime_id=null`, `proposed_stage_id_status=PROPOSAL_NOT_RUNTIME_NOT_GENERATOR_CONTRACT`. 원래 승인 72큐를 수정하지 않음.

## 3. 사용자가 직접 실행할 수 있는 **읽기 전용** 증거 검사기 — API0

TTS 담당이 기존 소스를 복제하거나 유료 모델로 바꾸지 않고 직접 Python `Main24_Main25_ReadOnly_Preflight.py`를 작성·오프라인 테스트함. **2026-11-24 대화 첨부** `LIMITLESS_TTS_Issue125_StageProof_Main25_Preflight_20261124.zip`(13,716B; SHA256 `e33779f881b25517f5d59a0572801e518c83bb5e0e88df6ec9e759985b04c069`). 소스 SHA256 `1d64549de68a7d05006739d1dfcb3ebfa6b3fab1758e9f9891748073938343bd`. Python·Main24 JSON·Main25 JSON·한글 안내·fixture 테스트·오프라인 자체검사 출력 동봉. ZIP 자체는 **GitHub 저장소에 커밋한 파일이 아니라 이 대화에서 납품한 자료**임.

- 실 PC의 `F:\Downloads\Voice`를 TTS 담당이 직접 열 수 없으므로 **사용자 PC WAV 열람0, 실제 API 요청0, Unity 적용0**.
- PowerShell에서 ZIP 안 **3개 파일을 기존 Voice 루트에 추가만** 하고 선택적 로컬 검사:
```powershell
conda activate py3_12
cd F:\Downloads\Voice
python .\Main24_Main25_ReadOnly_Preflight.py --root F:\Downloads\Voice
```
- 파일 읽기/해시/manifest 교차만 수행. `API.txt`, Google, 원래 생성기, Unity 경로 접근 없음. 기존 파일/장부/sidecar **수정하지 않음**.
- 별도 임시 fixture/가짜 바이트 검사 **8/8 PASS**: 로컬WAV부재=미확인, 승인72큐없음=미확인, Main25 9행+POST0, batch7 progress 해시 대조, WAV손상 차단, 72큐 정확 19행 교차, 대사 변조 차단, CandidateID 중복0. **실 음성 파일 통합 검증은 NOT_VERIFIED**, 가짜 byte fixture는 TTS 녹음물 아님.

## 4. Main25 실제 음성 생성 이전 게이트 — 현재 전부 HOLD

1. **제작기 적합성**: 기존 `Chapter3_Principal72_STAGING_ONLY.py`는 Main24 Stage-only 구현으로 검증되었으며 Main25 9개에 대한 실제 `run`/StageID·저장위치·새 대사 원문 계약은 **아직 없음**. 임의 `--max-calls 9` 실행 금지. Main25 독립 실행기는 **별도 구현·오프라인 mock 검증** 후 사용자에게 제공.
2. **중복감사**: 기존 WAV162 및 부족 메타 WAV590에 대한 읽기 전용 대조 필요. 확인 불가면 `UNVERIFIABLE` 기록 및 중복 위험을 사용자에게 명시. Main24 10개 재생성0.
3. **무료/과금 안전**: 해당 실행 시점 미국 PT 날짜·기존 승인 모델·4키의 실제 Google 프로젝트별 소속·무료 잔량·타 스크립트 외부 사용·유료 요청 차단을 **사용자가 직접 확인**해야 함. 예전 로컬 40회/프로젝트10회는 원격 무료 quota 증거 아님. 이번 작업에서 실제 잔량 확인 또는 JSON 임의 갱신 없음.
4. **별도 실행 승인**: 사용자의 Main24 합격은 Main25 합성 실행 승인으로 확대 해석하지 않음. 사용자가 승인하고 무료조건 검증된 다음에만 사용자 직접 실제 합성. 429 / 결과불명은 즉시 STOP, 자동 재시도·키전환0.
5. **Unity 보호**: 6000.6.5f1, 11/23 백업 `65cceb2d6c3283d18abec6e27d8a6f1296411f17`, main·Packages·.meta/GUID·Save/UserData·Discord·#85 HOLD·기존 BGM/아트 원본 변경0. Codex #85 작업 중 추가 Codex 호출/원격 원본 Push/Unity Play QA 없음.

**작업 수치**: Main24 사용자 제작·청취10/10 보호, Main25 정확9행 제작준비, Chapter3 전체 72행 중 실제 Stage 사용자 승인10행/신규 예정9행/이후53행. 이 세션 추가 API POST0·유료 호출0·실 신규 WAV0·Unity 편집0·원본 WAV/162 변경0.

**대표의 추가 승인 게이트**: 실제 새 Main25 합성 실행 및 차후 Unity Runtime 연결 별도 승인 필요. 이번 자료 확인 자체는 사용자 승인 대기 아닌 요청된 준비 업무 완료.
