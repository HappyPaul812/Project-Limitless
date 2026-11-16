> 2026-11-16 EOD 선별 인계: 출처 `codex/art100-101-min-review-20261116`, Git blob `59160a7fdc7e9eb8830d85e69f6e30900b99222b`. 이하 보고의 승인/미적용/PENDING 표시는 작성 당시 상태입니다. 최신 결재는 EOD 보고 우선: Stoat KeyPose·Idle4·Attack4v2·Hit4 미술 승인/Unity미적용, NPC02/17·10/07 로컬적용, Issue85 Import/GUID HOLD. NPC 손 한정 지시는 기획 해석 오류였으며 최신 정정대로 인계합니다. 기존 제품·미완료 자산을 새로 적용하지 않습니다.

# Issue #100·#101 최종 오프라인 실물 검수 — 2026-11-16

## 결론

| 대상 | 종합 | 기술 | 시각 의견 | 사용자 미술 승인 |
|---|---|---|---|---|
| 설원족제비 Attack4 MouthFix v2 | **PARTIAL** | 시트·CRC·정본SHA·SUMS·픽셀 수정범위 PASS, Manifest 메타데이터2건 불일치 | v1보다 입 과장 감소·연결 정상, PNG KEEP 권고.40px 물기/송곳니 세부 판독 제한 | **PENDING**, 자동 승인 아님 |
| OGA10·OGA07 v2 | **PASS / KEEP 권고** | 실제 PNG2 정본SHA·규격·손178/127px 국소변경 PASS | 금발 캐릭터 혼입 없음. 기존 디자인 유지·재작업 필요 없음 | **PENDING**, 자동 승인 아님 |

**OFFLINE_ONLY / UNITY_NOT_RUN / GAME_NOT_APPLIED / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**. PARTIAL은 오염된 시트라는 의미가 아니라 실제 Manifest 결함을 포함한 패키지 판정입니다. PNG를 고치거나 재제작하지 말고 담당자가 패키지 메타데이터만 정정하는 것이 적절합니다.

## 근거와 실제 입력 해시

최신 [#100 지시](https://github.com/HappyPaul812/Project-Limitless/issues/100#issuecomment-6911678456)·[입 수정 납품](https://github.com/HappyPaul812/Project-Limitless/issues/100#issuecomment-6911522213)·[공식 수정 보고](https://github.com/HappyPaul812/Project-Limitless/blob/docs/art-new/문서/00_프로젝트/Chapter3_SnowStoat_M2_Attack4_Mouth_TargetedFix_v2_20261116.md), [#101 지시](https://github.com/HappyPaul812/Project-Limitless/issues/101#issuecomment-6911679237)·[실물/표시 오류 정정](https://github.com/HappyPaul812/Project-Limitless/issues/101#issuecomment-6911482906)·[손 수정 보고](https://github.com/HappyPaul812/Project-Limitless/blob/docs/art-renewal/문서/00_프로젝트/Renewal_20261116_Issue101_OGA10_OGA07_HandOnly_TargetedFix_v2.md)·[이미지 감사 보고](https://github.com/HappyPaul812/Project-Limitless/blob/docs/art-renewal/문서/00_프로젝트/Renewal_20261116_Issue101_Wrong_Image_Identity_Audit.md)를 먼저 확인했습니다.

| F:/Downloads/ 실제 ZIP | 바이트 / entries | 실측 SHA256 | CRC |
|---|---|---|---|
| Stoat_Attack4_MouthFix_v2.zip |852864 /11|76e27b0c8d4fcfab09a11f857235755ce759a97572ae96bbb8641ea990388cb4|PASS|
| SnowStoat_Issue100_Attack4.zip |1092522 /12|2683a8bdb84595e648f56914944441b856986efee8647c0d2b92a7e5f9b13daf|PASS|
| npc101_fix.zip |287887 /2|ee98f2bde1062193f3b9100d9af67ca3b9fc2f17342c5ee30449193d20b2be3e|PASS|
| Limitless_NPC_101.zip |1188626 /13|0ec80e1fa546f1ce9cde560bea34f07b3359a0678565d72daec3f711bc232ca5|PASS|

입력 ZIP4개의 실제 값을 기록했습니다. 모두 각 최신 납품 보고의 대응 SHA와 일치합니다. 재포장 NPC ZIP은 과거 full 납품 ZIP과 구분합니다.

## A. 설원족제비 Attack4 v2

- v1 시트 SHA256 `a5f5856115473329d1b1a00a5db1022f1831ec48955e5e34f29b711dfb12141a`.
- v2 시트 SHA256 **`5175483e486889736f4f89402ec8a04595491c5c587e51a3f6bbd1365391364a`**, 요청 정본과 일치.
- 두 시트2048×512 RGBA/4×512셀. 수정 QA 독립 PNG2는 실제 시트2·3셀 RGBA와 동일.4프레임 서로 구별·4면 alpha0 PASS.
- 프레임 번호는 **1기준**. v1/v2 비교 결과:

| 프레임 | 변경 RGBA 픽셀 | bbox [x1,y1,x2,y2), 셀 내부 | 지정 입 ROI 밖 변경 | 접지 y396~511 변경 |
|---|---:|---|---:|---:|
|1|0|없음|0|0|
|2|1015|[449,307,490,345)|0|0|
|3|575|[456,312,494,340)|0|0|
|4|0|없음|0|0|

몸·귀·꼬리·입 외 얼굴·발은 각 v1 대응 프레임과 완전 동일. 프레임 순서/원본 자세 유지, 모든 프레임 alpha>8 하단455고정. 이는 v1→v2 보존 검사이며 모든 프레임 몸통이 서로 동일하다는 의미가 아닙니다.

### 시각 판독

직접 생성한512/128/40 동배율 비교판의 4프레임과 실제 입 crop×3을 확인했습니다.512px에서 v1의 크게 아래로 늘어난 붉은 입과 큰 삼각 송곳니가 v2에서는 얕은 입안·작은 송곳니로 줄어 부리처럼 보이는 과장이 완화됩니다. 위턱/코는 유지되고 아래턱이 주둥이 아래에 붙어 보입니다. alpha>8 입 ROI의 모든 전경은 몸통 주 연결영역과 연결되어 분리된 턱 조각이 없습니다. 프레임2 벌림→3 닫는 스냅→4 회복 순서와 몸/귀/꼬리의 기존 연속성 유지.

128px에서는 작은 입 벌림이 구별되지만40px에서는 송곳니/위아래턱 각각을 식별하기 어렵고 공격 변화도 약합니다. 추가 재작화를 자동 권고할 치명적 새 결함은 찾지 못했습니다. **PNG KEEP 권고**, 공격 전달력·입 모양 선호의 최종 미술 판단은 대표 확인 사항입니다. 실제 타격 타이밍/Animator/게임 표시 검증은 하지 않았습니다.

### Manifest 불일치 — 반드시 분리 보고

- SHA256SUMS **10/10 PASS**, 자기 자신을 제외한 모든 ZIP 파일 커버. 실제 ZIP 전체 SHA도 공식 납품값과 일치하여 로컬 사본 오염 증거 없음.
- Manifest11항목 중9개 크기·SHA 일치. **Manifest 자체 항목**은 expected3347bytes/SHA `656e101e4b31ba328f107a8c253251a057132056735c86a79ec05338c9d472f9`, actual3644bytes/SHA `14639934f3c5c553b17e69e43a36d3097d29d5a193d9c0a637ffb6d1ddf34715`로 불일치.
- **SHA256SUMS 항목**은 길이1091bytes 동일하나 expected SHA `6eb56851cf0a722bc014b661d3558ba270c030698b30f8a97cd5a0efdbeb0be4`, actual SHA `783fea9b67fd893c3e55c58edf076edf898493b10c7971673ddb58f51b01ec47`로 불일치.
- 자기참조/상호참조 해시 작성 순서에서 갱신되지 않은 메타데이터로 보이나 제작 코드 원인까지 확인한 것은 아닙니다. 담당자에게 Manifest 자기 자신·SUMS 제외 등 비순환 구성으로 문서 재패키징을 권고합니다. 이번 검수에서는 ZIP·Manifest·PNG를 수정하지 않았습니다.
- v1 Manifest10/10·SUMS11/11 PASS. 따라서 **v2 이미지 기술 PASS / 패키지 Manifest FAIL / 종합 PARTIAL**.

## B. NPC #101

| NPC | v1 PNG SHA256 | v2 PNG SHA256 | 변경 RGBA / alpha 픽셀 | 변경 bbox |
|---|---|---|---|---|
|OGA10|7a53cbb71544b69e996a06c7b576c9498d0565cc701a9123a6ad9485cd1d18a1|6be1a2d777a312dedd392b8ad6fbb2ba77b350d46d7bd2113d9d11bf09c7e6f3|178 /27|[152,232,366,329)|
|OGA07|940aa98590333b1423ed5f49101c9cf5a146a0079c8e354fc33e0ce943dff798|640b82a236afd5ca6025d5eb28970009f40941d39eadedeeeea0e180d5034d28|127 /20|[179,263,304,329)|

- 실제 `npc101_fix.zip`은 OGA10.png/OGA07.png만 포함한 재포장본입니다. Manifest/SUMS 미동봉(N/A)이며 원래 full 납품 ZIP의 누락/손상으로 취급하지 않습니다. PNG2의 SHA는 지정 v2와 완전 일치합니다.
- 512×512 투명 RGBA/외곽alpha0. bbox는 두 손 영역을 둘러싼 사각형이며 사각형 내부 전체를 수정했다는 의미가 아닙니다.
- 독립 검사용 손 ROI를 실제 손 위치에 맞춰 지정: OGA10 `[145,280,190,337) ∪ [295,225,375,280)`, OGA07 `[170,295,205,337) ∪ [265,255,315,300)`. 각 ROI 밖 변경0. 담당의 원래 세밀한 피부 마스크는 재포장 ZIP에 없으므로 동일 마스크를 실행했다고 주장하지 않으며, 실제 변경 픽셀과 손 위치를 비교판으로 확인했습니다.
- 얼굴·머리·복장·손 외 몸통·발·원래 포즈는 그대로입니다. 실제 PNG는 갈색 머리의 초록 조끼/붉은 스카프 파티관리인, 머릿수건/생활복 주민1의 기존 디자인입니다. **금발 여성 캐릭터는 PNG2에 포함되지 않았습니다.**
- 리뉴얼 담당은 예전 채팅의 별도 검색형 이미지 표시 오류라고 설명했습니다. F: PNG의 동일성과 미혼입은 직접 검증했으나 과거 채팅 UI나 금발 그림의 원출처까지 직접 복구/검증하지는 않았습니다. **BINARIES_CLEAN / WRONG_PREVIEW_ONLY(담당 설명과 데이터 부합)**.
-512/128px에서 환영 손짓·파티 표식/옷색과 머릿수건/천을 든 일상 주민의 역할 구분 유지.40px는 전체 인물 실루엣/색은 유지되나 세 사람 표식/천 무늬 및 손의 미세 변화는 작게 보여 판독 제한. 기존6종보다 세밀한 눈/머리/옷 질감은 v1 디자인 특성이며 v2가 새로 바꾼 문제가 아닙니다. 대표의 기존 긍정평가를 존중하여 **불필요한 재작업 없이 두 NPC KEEP 권고**.
- v1 ZIP의 Manifest11/11·SUMS12/12 PASS. 재포장 v2 ZIP 전체SHA는 과거695127bytes full ZIP `acbc523c6de831bc0d6b6556c36cd28cce297858218e38ca61e5414387063a64`와 달라도 정상입니다. 개별PNG를 기준으로 PASS 판정.
- 기획의 '손볼 것이 있으면 손보라'를 손가락 보정 발주로 해석한 오류는 최신 정정대로 인계. 손 한정 수정이 대표의 직접 지시였다고 인용하지 않습니다.

## 직접 만든 비교판과 보호

기존 원화 재제작0. 실제 ZIP PNG를 바이트 그대로 `Temp/Art100101Final20261116/Inputs/`에 추출, 두 비교판만 별도 생성했습니다. 512은1:1,128/40은LANCZOS,입/손 crop은NEAREST3배로 명시. 검색/생성 참고 인물 사용0. 출처 경로·원본SHA는 비교판에도 표기.

- `F:/study/codex/Project-Limitless/Temp/Art100101Final20261116/Issue100_Actual_Attack_v1_v2_512_128_40.png`
- `F:/study/codex/Project-Limitless/Temp/Art100101Final20261116/Issue101_Actual_NPC_v1_v2_512_128_40.png`
- 수치 원본 `audit.json`, 비교판SHA `boards-sha256.json`, 재현 도구 `audit.py`/`boards.py`는 같은 Temp 폴더. PNG/ZIP 바이너리 원격 업로드0.

이전 소형 문서 worktree `Temp/Art100101Review20261116/DocsWorktree` 재사용. 브랜치 `codex/art100-101-min-review-20261116`에 본 보고와 CODEX_LATEST만 로컬 커밋. 신규 worktree/대형QA/프로젝트 복제0. 최종 SHA는 Issue 댓글·Final.json에 기록하며 **LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

원본 main 사용자 상태 바이트 일치 및 입력ZIP4 SHA 불변을 최종 확인합니다. Unity 실행/명령·게임 적용·승인PNG 쓰기·Save/UserData/Discord 접근·패키지 변경·Push·Merge0. Unity6000.6.5f1·Issue85 HOLD 유지. CURRENT_STATUS는 기능 변경이 없어 수정하지 않았습니다. 사용자 최종 미술 승인·게임 적용 승인으로 확대 해석하지 않습니다.
