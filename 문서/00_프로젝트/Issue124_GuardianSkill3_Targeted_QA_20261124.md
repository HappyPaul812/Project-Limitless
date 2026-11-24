# Issue #124 수호자 기본 스킬 3종 표적 검수

2026-11-24 · 전체 **PARTIAL**

[기획 발주](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7123880804), [리뉴얼 납품](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7126000503), [기획 접수](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7126855514)를 확인했습니다.

## PASS · 파일 및 미술

- ZIP 4,991,933 B · SHA256 `8526cfc6e041022e0b1019535bd0350f9de9b69f5fec1b98f17232fdde93c5eb`. ZIP CRC 27/27, PNG 청크 CRC·디코드 23/23, Manifest 경로·크기·해시 25/25, SHA256SUMS 26/26.
- 원화 모두 128×128 RGBA이며 알파 범위 0–255, 네 모서리 투명, 반투명 가장자리를 확인했습니다. 동봉 공격·방어·스킬 3종은 승인 파일과 바이트가 일치합니다.
- 도발 `960bd0a3b87eadae637a1e2243eb3eab848b28963651ba328b01e699dba5687b`, 철벽 `76954eabf4557c6c4c44aacb8b6a0a9432d005373db159b5956d63c3d14a960e`, 수호의 맹세 `e6dd2c2516c07e8d3a415b04cb60599fbc50a62cba64a603ced686a8d4593906`.
- 승인 전투 버튼과 금속·금테·청색 톤이 맞습니다. 도발의 바깥 빨간 표식은 30/40px에서 다른 두 후보와 구분되며, 기존 흰 실루엣 `pawn_right` 도발 상태와도 구분됩니다. 수호의 맹세는 넓은 청색 보호막으로 철벽과 외곽이 구분됩니다.
- 기존 Kenney `pawn_left`, `structure_wall`, `pawns`, `pawn_right` 실물을 같은 픽셀 크기로 대조했습니다.

## PASS · 격리 Unity 표시 및 공유 계약

- Unity 6000.6.5f1 배치 Play Mode에서 후보 3종을 격리 복제본에만 임시 Import했습니다. 실제 Battle의 스킬 버튼 생성과 상태 배지 갱신 경로에 검수 입력을 넣어 세 스킬 Sprite를 확인했습니다. 스킬 시전·효과 발동은 검수 항목에서 제외했습니다.
- 철벽과 수호의 맹세는 각각 스킬과 상태 Image가 같은 Sprite 인스턴스를 참조했습니다. 도발 상태는 기존 `pawn_right`를 유지하고 도발 스킬은 후보 Sprite를 표시했습니다.
- 실제 UI 기본 크기는 스킬 18, HUD 12 캔버스 단위입니다. 캔버스 배율을 바꾼 Unity 화면에서 스킬/HUD 약 30/20px, 36/24px, 40/27px를 확인했고, 별도 픽셀 비교판은 요청한 30/40px 및 20/24px를 정확히 사용했습니다. 1280×720 기준 12px HUD에서는 라벨의 역할이 더 큽니다.

## PARTIAL · 작은 크기의 기능 판독

- **철벽**은 40px에서 성벽 상단이 보입니다. 30px부터 중앙 방패가 먼저 보이고, 20/24px에서는 성벽 윤곽이 뭉쳐 승인 기본 방어와 구분이 약합니다. 기존 `structure_wall`은 20px에서도 성벽 외곽이 남습니다.
- **수호의 맹세**는 20/24px에서도 보호막 외곽이 보입니다. 내부 아군 보호 관계는 작은 크기에서 라벨의 도움을 받습니다. 독립 아이콘만으로 기능을 판독하기에는 한계가 있습니다.

## HOLD · 본편 반영

본편 반영은 검수 범위 밖이며 미술 승인 대기입니다. Unity 원본, 기존 승인 자산, #85 `c5d9ed202a445ce3ef68c72f0b54cc47c1719b9c`, #131 `9d155ca635143b217a9cdd68a0c3e8be54106cb4`, 원본 main `65cceb2d6c3283d18abec6e27d8a6f1296411f17`, Save/UserData, Discord를 보호했습니다. 로컬 보고 커밋만 작성했습니다. `LOCAL_COMMITTED / REMOTE_NOT_PUSHED`.

증거 디렉터리 `Temp/i124-guardian-qa/`. 실제 크기 비교판 `GuardianSkill3_PixelComparison.png`의 SHA256은 `5cf4994fcb95c71a54fb6818d46ad7ba34a9acdf56221b37b1d3c8cfd76d6c59`입니다. Unity 원본 화면은 `Guardian_Unity_30skill_20hud.png`, `Guardian_Unity_36skill_24hud.png`, `Guardian_Unity_40skill.png`입니다.
