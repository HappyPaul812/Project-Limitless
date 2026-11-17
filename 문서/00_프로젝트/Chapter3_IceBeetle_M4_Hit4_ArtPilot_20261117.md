# LIMITLESS / M4 얼음갑충 Hit4 제작 및 오프라인 검증 — 2026-11-17

**Issue:** [#103](https://github.com/HappyPaul812/Project-Limitless/issues/103)
**승인 및 발주:** [Attack4 최종 미술 승인 / Hit4 발주](https://github.com/HappyPaul812/Project-Limitless/issues/103#issuecomment-6938653973)
**담당:** `docs/art-new`
**상태:** `KEYPOSE_IDLE4_ATTACK4_USER_APPROVED / HIT4_ART_CANDIDATE / USER_HIT4_ART_REVIEW_PENDING / OFFLINE_FILE_QA_PASS / UNITY_NOT_APPLIED`.

## 1. 승인 원본 보호
ChatGPT Library 승인 PNG 3종을 실제 회수하고 작업 전후 SHA256 검증. KeyPose 512×512 RGBA/Idle4·Attack4 2048×512 RGBA 규격 확인. 기존 Idle4 첫 프레임 및 Attack4 마지막 프레임은 KeyPose 전체 RGBA와 동일. 원본 파일 변경0.
- KeyPose: `b7ff0592226c2a1f28270053cf03483b6051c69edb412b59a7965ccf8699b3ad`.
- Idle4: `bd06c6d9a901d4c3550882ad4ce061629195b60e75f04636ec4814ebb0967793`.
- Attack4: `d8dab5e411f7eeb0aa73b1e412b8d820ecfeb38d514c60cf5fef7eb29af4b236`.

## 2. 신규 Hit4 후보
- `Sprite/IceBeetle_M4_Hit4_2048x512_RGBA_CANDIDATE.png` 2048×512 RGBA, 가로 4셀×512², 우향. 893,849 bytes. SHA256 `cf1dd6c78ffeffd12f8d026dda7ae362e09ffd0a7a3dee536d86569df4f51a3c`.
- 피격 충격/후방 반동 → 갑각 움츠림 → 회복 → 대기자세. 승인 KeyPose에서 상부 갑각에 부드러운 역좌표 변형 적용(프리멀티플라이 알파 선형 보간, 기본 상부 이동 -16/-8, -9/+17, -3/+5, 0/0px 및 작은 기울기). y430..511은 승인 원본과 RGBA 동일. Frame03은 KeyPose 전체 RGBA 동일 및 분리 QA PNG 바이트 동일.
- 앞서 생성한 시트/보드/GIF 시안은 최종 파일에 사용하지 않음. 승인 KeyPose 기반 파생 제작, 기존 Idle4/Attack4 모션 재사용 아님. 보간으로 일부 픽셀 변화 가능. 새 VFX/갑각 파손/상태이상 없음.
- 4프레임 서로 상이, 처음 3셀은 승인 Idle4/Attack4의 어느 셀과도 전체 픽셀 동일하지 않음. 모든 셀 경계 alpha0, 분리 QA PNG 4개와 시트 셀 일치. alpha>8 bbox(반열림) [20,119,481,464], [25,139,483,464], [27,129,483,464], [29,125,484,464]. 바닥 위치 유지.
- 실제 시트에서 512/128/40px 비교판 및 축소 스트립 직접 생성, GIF는 동일 셀을 어두운 바탕에 합성/256색 팔레트 변환. 디코드한 4프레임 RGB가 GIF 제작용 팔레트 이미지와 일치. 실제 지연 90/160/110/190ms, 합계 550ms 반복, 가변 간격이며 고정 FPS 아님. 실제 Unity PPU/Animator/런타임 미검증.

## 3. ZIP 납품 및 검증
- `IceBeetle_Issue103_Hit4_20261117.zip`, 3,131,456 bytes/12 entries, SHA256 `982328b9a96f13330b8cdf9d8be6433abd9907fa1b1826dbec0ec352bc6bee55`.
- ZIP 재개봉 CRC PASS, Manifest payload 실제 파일크기/SHA/전체커버 10/10 PASS, SHA256SUMS payload+Manifest 11/11 PASS. Manifest 자기/SUMS 참조 제외 및 SUMS 자기 제외. 중복0. SourceDeclaration 포함.
- ChatGPT Library `/LIMITLESS/Issue103_IceBeetle_Hit4/`에 ZIP/시트/실축소 비교판/GIF 4개 업로드 성공. 사용자 PC F: 복사 및 GitHub 바이너리 업로드 없음.

## 4. 범위/승인
**사용자 미술 승인 필요: YES (Hit4만).** 오프라인 파일 QA 합격이며 실제 게임 적용/런타임 검증 아님. Defeat4·펫·정식 스킬/스탯/드롭/MonsterID/Spawn·Unity 적용0. Unity6000.6.5f1, Issue85 HOLD, 기존 승인 몬스터/환경/보스, Save/UserData/Discord 보호. 대형 QA 복제 및 main 직접 commit/Push/Merge0. 문서만 담당 브랜치 갱신.
