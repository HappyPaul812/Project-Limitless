# LIMITLESS / Chapter3 빙각 수호체 M6 Hit4 파일럿 제작 및 QA — 2026-11-23

**Issue:** [#126](https://github.com/HappyPaul812/Project-Limitless/issues/126)  
**선행 승인:** [Attack4 v2 최종 미술 승인 및 Hit4 발주](https://github.com/HappyPaul812/Project-Limitless/issues/126#issuecomment-7091428809)  
**기술 선행 기록:** [Codex Attack4 v2 격리 재생](https://github.com/HappyPaul812/Project-Limitless/issues/126#issuecomment-7091633547)  
**담당 브랜치:** `docs/art-new`  
**상태:** `KEYPOSE_IDLE4_ATTACK4_V2_USER_APPROVED / HIT4_ART_CANDIDATE / USER_HIT4_ART_REVIEW_PENDING / OFFLINE_FILE_QA_PASS / HIT4_UNITY_NOT_APPLIED`

## 1. 기승인 원본 바이트 보존

아래 실제 게임용 승인 파일은 **읽기 전용**으로 확보했으며 SHA256은 기존 승인 기록과 정확히 일치하고 작업 완료 후에도 일치함. 기존 PNG 수정/덮어쓰기0.

| 원본 | SHA256 |
|---|---|
| `RimeSentinel_M6_KeyPose_512_RGBA_CANDIDATE.png` | `c34ea53a95aa6a1fdba4f5e355715fbc25797d6d51e4d544cb9e4dab52f7073c` |
| `RimeSentinel_M6_Idle4_2048x512_RGBA_CANDIDATE.png` | `0757ebedd37f24e7939d26f38be2efbf9560906b832fae187c61273a314b753e` |
| `RimeSentinel_M6_Attack4_v2_2048x512_RGBA_CANDIDATE.png` | `0649ef42da7aa628e471a49f4554ad150a0a5fc1b28235a5e9b42bf025030621` |

이전 대화에서 생성된 `빙결_암석_골렘_충격_반응_스프라이트.png`는 승인 원화와 얼굴·갑옷 차이가 있어 최종 스프라이트로 **사용하지 않음**. 승인 KeyPose를 픽셀 소스로 국소 2D 변형하여 한 시트만 신규 제작.

## 2. 신규 Hit4 실제 스프라이트

- `Sprite/RimeSentinel_M6_Hit4_2048x512_RGBA_CANDIDATE.png`: **2048×512 투명 RGBA, 가로 4셀×512²**, 파일 크기 **1,151,287 bytes**, SHA256 **`bd6d66be3b990d9bc7f33261f1d9835abd413c28b91a5842ac9800a5dec28496`**.
- Frame00 충격에 잠깐 상체가 움찔, Frame01 상체와 얼음 어깨가 강하게 후방 반동, Frame02 무게 회복, Frame03 승인 KeyPose 원형 복귀. 몸 전체를 단순 평면이동/회전시키지 않고 머리·상체·어깨·거대한 주먹 주변에 가우시안 공간 가중치의 국소 역좌표 remap 적용. 새 VFX/얼음 갑옷 붕괴/공격 기술/몬스터 수치 추가0.
- **Frame03 전체 RGBA 픽셀이 승인 KeyPose 512²와 정확하게 동일**, 프레임4개 각각 서로 다름. 승인 원화 아래 **y424..511 전 영역 RGBA가 4/4 픽셀 단위 동일**하여 양발·접지 불변. 프레임 셀 전체가 개별 QA PNG와 픽셀 일치, 셀 4변 외곽 알파0.
- bbox (Pillow 반열림) `[32,68,487,490], [34,59,491,490], [32,66,485,490], [31,65,481,490]`. 변형 픽셀 수는 [113051,117944,111597,0]이며 프리멀티플라이 alpha 보간에 따른 색상 변화도 포함하므로 신규 손그림 픽셀 수로 해석 금지. 본 파일은 승인 원화 유래 2D 보간 파일럿으로 얼굴/갑옷 개별 애니메이션 수작업 클린업을 완료했다는 뜻이 아님; 시각 승인 필요.

## 3. 실제 축소 비교판 및 피격 GIF

- `Preview/RimeSentinel_M6_Hit4_512_128_40_Review_OFFLINE.png`: 실제 최종 PNG에서 512·128·40px로 축소해 네 프레임 비교.
- `Preview/RimeSentinel_Hit4_Actual_128px_4Frame_RGBA_Strip.png`: 512×128 투명 RGBA (128²×4).
- `Preview/RimeSentinel_Hit4_Actual_40px_4Frame_RGBA_Strip.png`: 160×40 투명 RGBA (40²×4).
- `Preview/RimeSentinel_M6_Hit4_NonLoop_610ms_OFFLINE.gif`: 실제 4개 PNG 프레임을 어두운 배경 합성 후 팔레트 양자화, **nonloop**. 재개봉 실제 지연 **100/160/130/220ms**, 총 **610ms**. 정식 Unity Animator 타이밍·피격 데미지/이벤트 계약 미확정.

## 4. ZIP 재개봉 검증

- `LIMITLESS_Issue126_RimeSentinel_Hit4_20261123.zip`: **3,519,214 bytes / 13 entries**, SHA256 **`4ceb3996d1d602dcdccbf5980b7cd3423a2c9b1e509615c4c450c7ab4c642ea2`**.
- ZIP CRC `testzip() is None`: PASS. Manifest payload 실제 파일크기+SHA+전체커버 **11/11 PASS**, SHA256SUMS payload11+Manifest **12/12 PASS**. Manifest 자신·SUMS 상호/순환 SHA 참조 없음. SourceDeclaration/QA JSON 포함.
- 다운로드 실패 대비 ChatGPT Library `/LIMITLESS/Issue126_RimeSentinel_Hit4_20261123/`에 ZIP, Sprite PNG, 512/128/40 검토판, GIF 저장.
- Windows 사용자 PC `F:\Downloads` 자동 복사, GitHub PNG/ZIP 바이너리 업로드 하지 않음.

## 5. 승인 대기 / 보호

**Hit4 미술 승인 필요 YES.** 승인된 KeyPose·Idle4·Attack4 v2는 KEEP. Defeat4 미착수; 별도 예고 `팔 들어올리기`·`균열 강타` 신규 제작0. Hit4 Unity6000.6.5f1 임포트·Play/PPU/Animator/전투 GameView/상대 배율/Hitbox 실측은 **NOT_RUN**. 이전 Attack4 v2 격리 Unity PASS를 Hit4 PASS로 자동 승계하지 않음.
Unity 원본·main·Codex #122·#85 HOLD·기존 승인 공식 아트·Scene·Prefab·MonsterDefinition/Spawn/Drop/Quest/Save/UserData·Discord 변경0. 담당 문서와 Issue 댓글만 `docs/art-new`에서 갱신. main 직접 commit/Push/Merge 금지.
