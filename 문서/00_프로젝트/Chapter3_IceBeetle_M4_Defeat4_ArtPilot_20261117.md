# LIMITLESS / M4 얼음갑충 Defeat4 제작 및 오프라인 검증 — 2026-11-17

- Issue: [#103](https://github.com/HappyPaul812/Project-Limitless/issues/103)
- 승인/발주: [Hit4 최종 미술 승인 → Defeat4](https://github.com/HappyPaul812/Project-Limitless/issues/103#issuecomment-6940446478)
- 담당 브랜치: `docs/art-new`
- 상태: `KEYPOSE_IDLE4_ATTACK4_HIT4_USER_APPROVED / DEFEAT4_ART_CANDIDATE / USER_DEFEAT4_ART_REVIEW_PENDING / OFFLINE_FILE_QA_PASS / UNITY_NOT_APPLIED`

## 승인 원본 보호
이전 실제 납품 PNG 4종을 읽어 SHA256 작업 전후 비교, 모두 일치. 원본 수정0.
- KeyPose: `b7ff0592226c2a1f28270053cf03483b6051c69edb412b59a7965ccf8699b3ad`
- Idle4: `bd06c6d9a901d4c3550882ad4ce061629195b60e75f04636ec4814ebb0967793`
- Attack4: `d8dab5e411f7eeb0aa73b1e412b8d820ecfeb38d514c60cf5fef7eb29af4b236`
- Hit4: `cf1dd6c78ffeffd12f8d026dda7ae362e09ffd0a7a3dee536d86569df4f51a3c`

## 신규 게임용 시트
- `Sprite/IceBeetle_M4_Defeat4_2048x512_RGBA_CANDIDATE.png` **2048×512 RGBA, 가로4셀(512×512), 우향**, 912,389 bytes, SHA256 **`9cd3fd4c1925f9953830d9d857b96f11f8bb754dc7b57fe34640936f4cb1c5fb`**.
- 힘 빠짐 → 다리 접힘 → 쓰러짐 → 최종 패배 자세 유지. 폭발·갑각 파괴·VFX 추가 없음. 이전 생성 시안(2172×724 RGBA, SHA256 `7f5c3d62d6e1ac10ddf432d910d5b65be09c1a92f830cd62153d5fd5fbeaf316`)의 각 프레임에서 최대 연결 알파 영역 추출 후 동일 비율(455/484), Lanczos 축소, 공통 하단 y464/가로 중심으로 정렬. 저알파/고립 잡픽셀 제거. **승인 KeyPose 픽셀을 직접 변형한 동작은 아니며 세부 묘사 차이가 있을 수 있어 사용자 시각 승인 필요.**
- 4셀 모두 서로 다르고 KeyPose와 동일한 셀 없음. 경계 alpha0, 분리 QA4 PNG와 시트 셀 전RGBA 일치. bbox(반열림) [28,126,483,464], [28,151,484,464], [21,163,491,464], [20,178,491,464]. 하단 동일/상단 단계적으로 낮아짐. 다리 접힘은 허용된 패배 동작이므로 기존 접지 영역 픽셀 동일을 주장하지 않음.
- 실제 최종 시트에서 512/128/40px 비교판·축소 PNG 직접 제작. 검토 GIF는 동일 셀을 어두운 배경에 합성해 256색 양자화. 디코드 RGB 4/4 일치, 실제 지연 **160/190/220/1600ms**, 총 2170ms, **loop 확장 없음**, 마지막 자세 유지 1600ms. Unity 타이밍/PPU/Animator 미검증.

## ZIP 및 저장
- `IceBeetle_Issue103_Defeat4_20261117.zip` 3,134,616 bytes/12 entries, SHA256 **`c04ad4e6528f3ea9aa36971b8ef9ce033711d2e96ba0d13589f3adb0fb676bbc`**.
- 재개봉 CRC PASS, Manifest payload 10/10 크기 및 SHA/전체 커버 PASS, SHA256SUMS payload+Manifest 11/11 PASS, 자기/순환참조 및 중복 없음. SourceDeclaration 동봉.
- ChatGPT Library `/LIMITLESS/Issue103_IceBeetle_Defeat4/`에 ZIP·시트·비교판·GIF 실제 업로드. 사용자 PC F: 복사나 GitHub 이미지 바이너리 업로드는 하지 않음.

## 승인 및 보호
**Defeat4 미술 승인 필요 YES.** Unity6000.6.5f1, Issue85 HOLD, 승인 원본, Save/UserData, Discord 및 기존 프로젝트 보호. Unity 실행/Import/적용/MonsterID/Spawn/스탯/펫 구현, 대형 QA 복제, main 직접 commit/Push/Merge0. 담당 브랜치 문서만 기록.
