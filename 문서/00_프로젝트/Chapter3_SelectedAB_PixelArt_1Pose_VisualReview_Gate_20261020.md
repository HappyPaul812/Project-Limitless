# LIMITLESS / Chapter3 서리들쥐 A·설빙의 폭군 B 1포즈 픽셀아트 검수 게이트

작성: 2026-10-20 / docs/planning. 사용자 제공 로컬 경로: `F:\Downloads\Limitless_Chapter3_ApprovedAB_PixelArt_1Pose_v1.zip`.

## 증거와 상태

`docs/art-new/문서/00_프로젝트/Chapter3_ApprovedAB_PixelArt_1Pose_v1_QA_20261020.md` 및 최신 인계 확인. ZIP SHA256은 담당 보고상 `4bd67210e4d813b1ceb5c22cd76a35af577837b63068af57392c41226da968fc`, 내부 6파일. 대상 PNG는 `Sprites/FrostFieldMouse_A_PixelArt_1Pose.png` (512x512 RGBA, SHA256 `868dbfa4f7be623509f844c5dd10fc0a51ebe690b4aa2ca0c2853cf6f671d5a3`) 및 `Sprites/FrostTyrant_B_PixelArt_1Pose.png` (512x512 RGBA, SHA256 `52c26b9359665255620addd21ed9c0f07d8bbb03923753c3ae580cd768adc251`). 사용자 고해상도 컨셉 A/B 선택은 **디자인 방향 선택**이지 1포즈 픽셀아트 자동 미술 승인 아님.

제작 보고: 참조 이미지에서 독립 캐릭터 분리 후 제한된 팔레트와 최근접 3배 스케일링; 512 RGBA/Alpha 외곽 여백/ZIP 무결성 PASS. 실제 Ch2 공식 전투 몬스터와 동일 월드 렌더 스케일 비교/체급·실루엣/디테일·게임용 PPU/Pivot·Animation 전부 미검증. `OFFLINE_QA_REPORTED_PASS / USER_PIXART_VISUAL_APPROVAL_PENDING / UNITY_NOT_APPLIED / ANIMATION_NOT_AUTHORIZED`.

## 후속 검토 지시 (Codex 최소 안전 검수 또는 대표 직접 ZIP 열람)

- Codex가 필요하면 로컬 ZIP을 **읽기 전용**으로 열고 SHA/Manifest/PNG를 검증하고, 기존 Chapter2 승인 몬스터 5종과 신규 A/B 몬스터/보스를 같은 **게임 내 표시 크기** 기준의 비교판을 만든다. 원본 512 캔버스 전체 확대가 아닌 실제 Alpha bbox 및 현행 Battle Sprite PPU/렌더링 크기 기준으로 비교. 보스는 Chapter2 기존 Boss와 체급 비교.
- 실루엣 식별성, 야생 생물의 위험·질감, 설빙의 폭군의 웬디고 모티브/압박, 픽셀 블록·장난감/마스코트 느낌 잔존 여부, 외딴 픽셀, 원본 컨셉 A/B 대응 여부를 **사람이 검토할 수 있는 시각 자료**로 출력. 가능하면 PNG2 단독/축소128px/공식 Ch2 실제 렌더 배율 3안 제시. 파일 미존재/해시 불일치면 FAIL·재확인, 임의 교체 금지.
- 아직 애니메이션/몬스터 전체 양산/Unity Scene Import/Code/Collider/전투 구현은 하지 않는다. 사용자 이미지 미술 최종 승인 후 Codex Issue #35의 기술 계약에 맞게 프레임 제작 범위를 별도 승인.
- `docs/art-new`는 신규 북부 F1/서릿길 마을 환경 파일럿과 Chapter1·2 리뉴얼 공식 파일 보호. `main`에 직접 Push/병합 없음. BGM 승인과 본 아트 승인 별개.

## 승인 경계

바울 대표는 이전 아동용 서리들쥐/마스코트 보스 **반려**, 이후 고해상도 컨셉 **서리들쥐 A/폭군 B 디자인 방향 선택**. **이번 ZIP의 1포즈 PNG 자체는 아직 승인 요청 전/대기**. Pixel art 품질과 기존 게임 자산 스타일을 사용자에게 실제로 보여 준 후 승인/수정 결정을 받는다. 로컬 F: ZIP은 기획세션 컨테이너에는 자동 탑재되지 않았으므로 PNG 자체를 보았다고 주장하면 안 됨.
