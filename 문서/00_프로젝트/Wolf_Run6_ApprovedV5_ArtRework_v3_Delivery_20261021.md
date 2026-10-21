# LIMITLESS — Wolf Run6 v2 미술 FAIL 대응 실제 v3 보완 아트·검수 ZIP 납품 (2026-10-21)

## 근거 / 역할 / 정확한 승인 상태

- GitHub [Issue #43](https://github.com/HappyPaul812/Project-Limitless/issues/43) 최신 Codex 댓글 #issuecomment-6309566186와 `docs/planning/문서/00_프로젝트/20261021_Main22_ApplyReady_WolfRun6_ArtFail_NextActions.md` 직접 확인.
- 이전 `Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtPilot_v2_CANDIDATE.zip`: **ZIP/PNG 기술 PASS, 미술 FAIL** (03·04 잘린 꼬리, 프레임 간 몸 비율과 접지, 05→00 전환 미승인). 실제 Unity GameView/Runtime 검증 아직 없음.
- 승인 단일 Wolf v5 Key Art `Wolf_KeyArt_v3_AlphaFix_v5.png` SHA256 **`b770c73c36b9035a2bb0e70b8c126090b5fa248d27db542af077094d12ee2ea1`** 현재 실제 PNG 검사 일치. **v5·v3·v4 원본 바이너리 변경0**. v2 Candidate ZIP SHA256 **`c69ca17e8ebe2b8591c4dc77ebc6531b60dbe8a44ee3475d39d4cc6159f8f2ad`** 확인, 변경0.
- **현재 결론 `ART_REWORK_V3_6_PNG_DELIVERED / TECH_OFFLINE_PASS / USER_RUN_MOTION_APPROVAL_PENDING / CODEX_VISUAL_DELTA_QA_PENDING / UNITY_NOT_APPLIED`**. 형식이 통과해도 6프레임 Run 전체가 예술 합격했다는 뜻이 아님.

## 실제 v3 제작 변경점

- v2는 6마리가 한 장에 겹쳐 생성된 원화를 잘라낸 이력. v3에서 00·01·02·05는 이미 명확한 개별 포즈이므로 **그대로 유지**, 불량 03·04에만 품질 우선 수정.
- 03: 오른쪽 캔버스 경계에서 잘린 꼬리를 별개 Run05 원본에 있는 **같은 늑대의 실제 털 페인팅**으로 재구성·회전해 긴 공중도약의 자연스러운 뒤쪽 꼬리 윤곽을 복원. 마찬가지로 합성 컷 때문에 잘려 있던 들린 뒷발/발가락을 동일 디테일의 Run05 발·발톱에서 국소 복원. 원형 퍼지/단순 투명 픽셀만 삭제하지 않음.
- 04: 잘린 듯 짧게 끝나는 꼬리 줄기 영역만 정리하고 같은 늑대의 페인팅된 꼬리 소스를 포즈에 맞게 위로 들어 올린 흐름으로 재구성. 00/01/02/05 원본 프레임 바이트 그대로. 단순 6장 이동/크기 확대/축소로 Run 가짜 제작한 것이 아니며 원래 독립 자세6개 보존.
- **소스 고지:** 6프레임 전체가 새로 손그려진 것은 아님. v2의 다른 4포즈는 유지하고 03/04 핵심 결함을 표적 복구. 등신 비율·신체 접지 및 반복 동작은 새 GIF/프레임별 내부 QA를 만들었지만 **Codex·대표 모션 검수 전까지 미술 PASS 선언 금지**.
- **방향 LEFT**, RGBA PNG 6개 각 **512×384**, 가로 **3072×384** 6-cell Strip, 3×2 **1536×768** Atlas, **12FPS 6프레임 0.5초 루프**, 128px/40px 게임 표기 후보. Unity PPU/Import 월드 사이즈는 별도 Codex 기술계약.

## 실제 신규 ZIP 및 검증

- **파일명** `Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtRework_v3.zip` (이 대화 `sandbox:/mnt/data/Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtRework_v3.zip`).
- **2,788,009 bytes / 36 ZIP entries / SHA256 `f7eb4122b150e351900a4ab7ea82c802809605bd0444be89e36f6bf167dcd008`**.
- `Assets/Poses/Wolf_Run_{00..05}_v3.png` 6 PNG, `Assets/Wolf_Run6_v3_Strip_3072x384.png`, `Assets/Wolf_Run6_v3_Atlas3x2_1536x768.png`.
- `Preview/Wolf_Run6_v3_12FPS_128px.gif`와 투명 APNG, `Wolf_Run_v2_vs_v3_12FPS.gif`(동일 크기·동일 시간 좌우 비교), `Run_03_Tail_BeforeAfter_2x.png`, `Run_04_Tail_BeforeAfter_2x.png`, 128px·40px 콘택트시트, 프레임별 표면·접지점 요약, 05→00 정적 연결 비교.
- `Manifest.json`: 패키지 내부 **각 파일 개별 SHA256/byte**, 승인 v5 SHA 및 v2 후보 SHA, fps/cell/direction; `QA/Frame_Gait_And_Alpha_Checks.csv/json`, `QA/Anatomy_Proportion_Visual_Gate.csv/json`. 신체 비례/어깨→엉덩이 추정은 아트 검토용 수동 앵커(엄밀한 뼈 길이·Unity 실측 아님).
- 결과: **표준 ZIP `testzip()==None` CRC PASS**, PNG 6/6 재해독, Strip 6셀↔PNG 프레임 바이트 동일6/6, Atlas 6셀↔PNG 동일6/6, 각 프레임 3px 테두리 알파 잔여0, 여섯 서로 다른 이미지, v5/v2 원본 SHA2/2 정합. 각 프레임의 가장 낮은 유효 알파 접지 위치 y=322,316,322,281(공중),330,325. 마지막 05(y325)→첫00(y322)의 최소발 하한선 차이는3px. 이 지표만으로 발 관절·달리기 모션 PASS 주장할 수 없음.
- v2 대비 수정 이미지 픽셀 수 03 **8,198**, 04 **7,615**; 00/01/02/05 0. 03·04 꼬리 복원이 실제로 뚜렷한지 2x 전후 확대 제공. 05→00 전환은 실제 12 FPS 루프를 시청하여 동작의 자연스러움 판정 필요.
- 패키지 ZIP에는 **승인 v5 키아트 파일을 변형/복제 수록하지 않았고**, 정본 SHA만 명시. 사용자 PC F: 파일을 직접 이동/덮어쓰지 않고, GitHub에는 바이너리 ZIP 업로드하지 않고 기록 문서만 담당 브랜치에 커밋.

## 후속 및 보호
- **Codex** 별도 `codex/*` 격리 환경에서 v3 신규 PNG6/strip/atlas SHA 대조 후, v2와의 동등 배율 GIF·실제 GameView 모션(3화면비) 변경분(03/04 꼬리·발/몸비례·접지·05→00 루프)을 검증. 기존 Main22/Main23 등 기합격 QA 전부 재실행하지 않는다.
- 꼬리/발 복원만으로 Run6 전체 미술 FAIL을 완전히 해소했다고 가정하지 말고 필요 시 **해당 문제 포즈 독립 신규 그림 제작**을 승인 받아 재작업할 것.
- **최종 모션 미술 승인 바울 대표 대기.** 허가 전 구형 ThirdParty Wolf_Run 교체·PPU 강제변경, Unity 원본 Editor/Scene/Meta/GUID/Save/UserData·게임 코드 수정0.
- Bear/Fox·VenomSnake 전체 애니메이션·Chapter3 신규 아트 제작0. `docs/art-renewal` 작업, `main` commit/Push/merge0.
