# LIMITLESS Chapter3 — 서리들쥐 A / 설빙의 폭군 B 픽셀아트 1포즈 시안 및 QA (2026-10-20)

**담당:** Chapter3 이후 신규 아트 총괄 · 브랜치 `docs/art-new` · [Issue #30](https://github.com/HappyPaul812/Project-Limitless/issues/30)

**상태:** `CONCEPT_DIRECTION_USER_SELECTED / PIXELART_1POSE_CREATED / SPRITE_ART_APPROVAL_PENDING / NOT_UNITY_INTEGRATED`.

## 1. 사용자 선택 기록

- **서리들쥐: 고해상도 콘셉트 A안 채택**. 기존 콘셉트에서 야생 들쥐의 거친 털·앞니·발톱·긴 꼬리·북부 초입 생존감 유지.
- **설빙의 폭군: 고해상도 콘셉트 B안 채택**. 두개골형 얼굴, 뿔처럼 뻗은 구조, 날카로운 앞발/발톱, 눈과 얼음에 덮인 압도적인 보스 분위기 유지.
- 사용자 선택은 **디자인 방향 채택**으로 이해하며 아래 1포즈 픽셀아트 이미지 자체에 대한 **최종 미술 승인으로 해석하지 않는다**. 전투/필드 시트 애니메이션 확대도 별도 승인이다.
- 이전 1차 반려 콘셉트는 REJECTED 상태 보존. 고해상도 v2 A/B 콘셉트 원본은 변경하거나 삭제하지 않음.

## 2. 실제 신규 산출물 (이번 ChatGPT 대화 다운로드)

- ZIP: `Limitless_Chapter3_ApprovedAB_PixelArt_1Pose_v1.zip`.
- ZIP SHA-256: `4bd67210e4d813b1ceb5c22cd76a35af577837b63068af57392c41226da968fc`.
- GitHub 브랜치에는 **이 문서 및 후임 인계만 기록**; 바이너리 ZIP/PNG 파일은 대화 첨부로 납품했으며 GitHub repo·사용자 Windows F: 로컬에 업로드했다고 단정하지 않음.

| 구분 | PNG | 검사 규격 | Alpha bbox (left,top,right,bottom) | SHA-256 |
|---|---|---|---|---|
| F1 서리들쥐 1포즈 | `Sprites/FrostFieldMouse_A_PixelArt_1Pose.png` | 512×512 RGBA | (143,327,368,474) | `868dbfa4f7be623509f844c5dd10fc0a51ebe690b4aa2ca0c2853cf6f671d5a3` |
| 설빙의 폭군 1포즈 | `Sprites/FrostTyrant_B_PixelArt_1Pose.png` | 512×512 RGBA | (43,111,469,474) | `52c26b9359665255620addd21ed9c0f07d8bbb03923753c3ae580cd768adc251` |

### ZIP 내부 6항목

1. `Sprites/FrostFieldMouse_A_PixelArt_1Pose.png`
2. `Sprites/FrostTyrant_B_PixelArt_1Pose.png`
3. `Preview/ApprovedAB_PixelArt_1Pose_Comparison.png`
4. `QA/SmallSize_128px_Review.png`
5. `QA/Offline_QA.json`
6. `Manifest.md`

## 3. 제작·검증 및 한계

- 새로 생성한 픽셀아트 디자인 참고 보드에서 승인된 A/B 외형을 골라 **각각의 단독 캐릭터 부분만 분리**. 원본 포함 텍스트·격자·배경은 게임 후보 PNG에서 제거. 분리한 색은 56색 한정 팔레트로 정리하고 nearest-neighbor 정수 3배 배율로 각 512×512 투명 캔버스에 배치.
- **오프라인 검증 PASS**: 2장 PNG 디코딩, 모두 512×512 RGBA, alpha=0/255, 캐릭터 실루엣 8-connected 주요 컴포넌트 1개/장, 외곽 2px 이상의 투명 여백, ZIP 무결성 PASS. 128px 축소 미리보기 제공.
- **미술 QA 별도**: 실제 Chapter2 몬스터/보스와의 스타일·체급 비교, 날카로운 윤곽의 가독성, 휠체어/캐릭터와의 크기 관계, 의도된 원본 디자인 보존은 시각 승인 대기. 자동 픽셀 규격 PASS를 미술 승인으로 주장하지 않음.
- **기술 미확정**: 512×512는 **시안 출력 캔버스**일 뿐 Unity PPU/Pivot/Monster prefab/실제 Battle Sprite 크기/6행 시트/콜라이더/Frame 규격은 Codex Issue #35 기술 계약에 따름. 실제 게임용 프레임 수나 화로 기믹을 정하지 않는다.
- **미제작**: 방향 4개, Idle/Attack/Hit/Defeat, 전용 VFX, 판정용 Collider/Scene·Runtime 전체.
- **보호**: F1/서릿길 마을 파일럿 환경 10종, Chapter1·2 리뉴얼, 공식 캐릭터 하렌·태온·세린·폴·미엘·플레이어 원본, Unity/Save/GUID/meta 수정0. `main` 직접 커밋/Push/병합 없음.

## 4. 후속 승인·작업 순서

1. 바울 대표에게 **이번 픽셀아트 1포즈 2종의 미술 적합성** 확인받기 (고해상도 A/B 콘셉트 선택 자체는 완료).
2. 그림이 승인되면 Codex [Issue #35](https://github.com/HappyPaul812/Project-Limitless/issues/35)의 PPU/Pivot/Collider/배치 실측 계약 및 기획 Production Lock을 대조한 뒤 실제 몬스터 Sprite Keyframe 제작 범위 확정.
3. 그 다음 사용자 범위 승인 후에만 해당 몬스터/보스의 Idle/Attack/Hit/Defeat 시트를 제작하고 Codex에 Unity Import/Animator/Runtime QA 위임.
4. 다른 다섯 몬스터·기존 공식 자산을 이번 시안 제작 때문에 임의 생성·변경하지 않음.
