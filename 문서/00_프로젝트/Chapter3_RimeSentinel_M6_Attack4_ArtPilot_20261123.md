# LIMITLESS / Chapter 3 F5 빙각 수호체 Attack4 단일 파일럿 — 2026-11-23

**Issue:** [#126](https://github.com/HappyPaul812/Project-Limitless/issues/126)  
**발주/선행 승인:** [대표 Idle4 조건부 승인 충족 및 Attack4 발주](https://github.com/HappyPaul812/Project-Limitless/issues/126#issuecomment-7089455631)  
**담당 브랜치:** `docs/art-new`  
**현재 상태:** `KEYPOSE_IDLE4_USER_APPROVED / ATTACK4_ART_CANDIDATE / USER_ATTACK4_ART_REVIEW_PENDING / OFFLINE_FILE_QA_PASS / UNITY_NOT_APPLIED`

## 1. 승인 원본 및 Codex 기존 검증 보존
- 승인 KeyPose PNG 512×512 RGBA **SHA256 `c34ea53a95aa6a1fdba4f5e355715fbc25797d6d51e4d544cb9e4dab52f7073c`**.
- 승인 Idle4 PNG 2048×512 RGBA **SHA256 `0757ebedd37f24e7939d26f38be2efbf9560906b832fae187c61273a314b753e`**.
- 두 파일 현재 세션 실물 SHA 검사 PASS. 원본 파일 재저장·변경0. [Codex 격리 260ms 검증](https://github.com/HappyPaul812/Project-Limitless/issues/126#issuecomment-7089428112)과 Idle4 최종 승인 조건을 수용. **Idle4 실제 260ms×4=1.04초를 Attack4 타이밍으로 무단 전용하지 않음**. 기존 승인 원본이 Unity에 본편 적용됐다고 주장하지 않음.

## 2. 이번 신규 게임용 Attack4 PNG 한 시트
- **`Sprite/RimeSentinel_M6_Attack4_2048x512_RGBA_CANDIDATE.png`**, **2048×512 투명 RGBA**, 4×512셀 가로, **1,130,329 bytes**, SHA256 **`7a44d60d540176d06c96ee3ff221db5ebbde40077977d8de8762a714ff480e62`**.
- 사용자 승인된 KeyPose의 팔/주먹/어깨 **국소 2D 비강체 역좌표 변형**으로 다음 4단계만 제작: ① 짧은 준비(관객 오른쪽 주먹 약9px 되감기) → ② 전방으로 +11px 밀기 → ③ 타격 정점(+26px 전진, 주먹 부위 최대14% 국소 원근 확대) → ④ 자세 회복. **Frame03 전체 512×512 RGBA가 승인 KeyPose와 완전히 동일**.
- 전체 이미지 평면 회전·점프·양쪽 주먹 교대 공격 없음. 승인된 얼굴·갑옷·얼음 어깨·주먹·다리 디자인은 동일 출처 유지. y424..511 하단 접지 영역 **4/4 승인 KeyPose 원본 RGBA와 픽셀 완전 동일**. 프레임별 가시 bbox 반열림: `(30,65,475,490), (30,64,489,490), (30,64,500,490), (31,65,481,490)`. 모든 셀 외곽 알파0. 네 프레임 상이, 시트 각 셀과 독립 QA PNG 4종 RGBA 완전 동일.
- 이번 대화 직전에 생성된 `리메_센티넬_빙결_주먹_연속기.png`(2172×724 RGBA, SHA256 `7b53e5c6c62f639de7f15f31abf37365434018b3eaaca99c042d59e10808e46f`)은 **서로 다른 주먹을 번갈아 내미는 결함으로 최종 납품 미사용**. 승인 원본에서 직접 파생한 실제 게임용 시트만 ZIP에 포함. 이 방식은 새 인체 포즈 수작업 원화가 아닌 국소 변형 파일럿이므로 공격의 박력·사실성에 대한 사용자 미술 검토 필요.

## 3. 비교판 및 검토 GIF
- `Preview/RimeSentinel_M6_Attack4_512_128_40_Review_OFFLINE.png`: **실제 납품 시트**의 4셀에서 직접 만든 512/128/40px 비교판(오프라인, Unity GameView 아님).
- `Preview/RimeSentinel_Attack4_Actual_128px_RGBA_Strip.png` (4×128)과 `Preview/RimeSentinel_Attack4_Actual_40px_RGBA_Strip.png` (4×40).
- `Preview/RimeSentinel_M6_Attack4_NonLoop_660ms_OFFLINE.gif`: 비루프 검토 GIF, 실제 디코드 **170/120/150/220ms = 총660ms**, 반복 확장 없음. 배경색은 미리보기용, 게임용 PNG 투명 배경 유지. GIF 타이밍은 **정식 Unity Animator/전투 판정 시점 계약 아님**.
- 예고 행동 `팔 들어올리기`, 별도 `균열 강타` 및 VFX·타격 이펙트 등은 **이번 일반 얼음주먹 Attack4에 미포함**.

## 4. ZIP / Manifest / SHA256 QA
- **`LIMITLESS_Issue126_RimeSentinel_Attack4_20261123.zip`**: **3,781,167 bytes**, 13 entries, SHA256 **`32bb44b25a8af81692bfa7f505d815236362be159cbbf2e0989af167a4ab5e1d`**.
- ZIP 재개봉 **CRC PASS**, Manifest **11/11 payload 경로·크기·SHA256/파일 커버 PASS**, SHA256SUMS **12/12** (11 payload+Manifest) PASS. Manifest 자기참조·SUMS 참조 제외, SUMS 자기참조 제외, 순환해시 없음.
- `Docs/SourceDeclaration_And_QA.md`, `QA/attack4_file_validation.json` 포함.
- ChatGPT Library **`/LIMITLESS/Issue126_RimeSentinel_Attack4_20261123/`**에 ZIP·게임용 PNG·비교판·GIF 저장. 사용자 Windows `F:\Downloads` 자동 복사 및 GitHub PNG/ZIP 바이너리 업로드 없음.

## 5. 승인 대기 및 보호 범위
**바울 대표 Attack4 시각 미술 승인 필요 YES**. KeyPose와 Idle4 최종 승인 보존; Hit4·Defeat4 및 예고/균열강타 모션 제작0. 실제 Unity6000.6.5f1에서 본 Attack4를 Import/재생·GameView·Animator·PPU·Collider·Hitbox·전투 타격 타이밍 QA **NOT_RUN**. 기존 Codex #126 Idle4 기술검수 반복0.
Unity 원본·Scene/Prefab/.meta/GUID·전투데이터/MonsterDefinition/Spawn·Drop·Save/UserData·Discord·#85 HOLD, M1~M5/Chapter1~3 승인 아트 보호. main 직접 커밋·Push·Merge 금지. `docs/art-new` 문서 갱신/Issue 댓글만 수행.
