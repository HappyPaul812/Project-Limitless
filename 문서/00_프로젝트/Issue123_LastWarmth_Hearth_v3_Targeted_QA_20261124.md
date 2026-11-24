# Issue #123 · 마지막 온기의 화로 v3 표적 검수

2026-11-24 · 승인 원화 검수 · `codex/issue123-hearth-v3-review-20261124`

[최종 승인·납품](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7126702622) · [docs/art-new 최신 인계 커밋](https://github.com/HappyPaul812/Project-Limitless/commit/19bf79d3edf0b243a6d2aa522603dffd9f07f4e5)

- **파일·내보내기 PASS** · 로컬 `LIMITLESS_Issue123_LastWarmth_Hearth_v3_20261124.zip`, 3,806,588바이트, SHA256 `1f4caab7cd712fc325450b8644e727dd6fc4719e876810d0c370fd5ffbc4e676`. ZIP CRC 6/6, SHA256SUMS 5/5, Manifest 규격/해시 대조, PNG 청크 CRC 및 디코딩 통과. 게임용 PNG SHA256 `3a3094749ba49c6b7139b9638a61d543fa903d7ec214209f2b51a43985c1a5d2`, 256×256 RGBA, 외곽 알파 0, 범위 0–255, bbox [10,8,245,248), 최소 비영 알파 9. 승인 1254 마스터에서 Manifest대로 축소·알파 정리한 결과와 알파 및 가시 픽셀이 모두 일치했습니다.
- **동배율·가독성 PASS** · 승인 보스 KeyPose 512×512, Idle4, 결빙 배경 512×384의 ZIP 실물 해시 3/3 일치. 화로는 원본 픽셀 1:1에서 폭이 절반인 소품으로 읽힙니다. 회청색 고드름과 돌 기단이 보스·빙벽에 어울리고, 중앙의 강한 금색 핵과 고리가 일반 불꽃과 구별됩니다. 밝고 어두운 배경 모두 40px에서 금빛과 외곽이 읽히며 30px에서는 세부 고리보다 금빛 핵과 전체 윤곽이 남습니다.
- **가림·동선 개념 PASS / 제품 검증 HOLD** · 전투 합성에서 화로는 좌측에 놓여 보스, 아군 3인 표시 영역, 명령·상태 안내를 가리지 않습니다. 화로·보스와 뒤쪽 빙벽이 맞닿는 부분에서도 핵과 보스 얼굴이 구분됩니다. 아군과 UI는 도식이므로 실제 배율·Sorting·가림은 미검증입니다. 이전 결빙 파일럿과 탐험 지도 내부를 픽셀 대조해 남쪽 F5 입구 → 북쪽 보스 목표, 북쪽 화살표와 세로 통행로 유지 확인. 4.5 WU는 미실측 목표입니다.
- **이전 화로 원본 미확보** · 로컬 프로젝트 Assets·Recovery와 Downloads 파일명, 관련 납품 ZIP 내부를 화로/Hearth/Brazier 이름으로 읽기 검색했습니다. 이전 원화로 식별할 실물은 확인하지 못했습니다. 이번 승인 마스터는 이전 원본과 별개입니다.

오프라인 증거 `Temp/i123-hearth-v3/`. 승인 원화·#85·#131·원본 main 보호. Unity 6000.6.5f1, 본편 적용·Scene 생성·main 병합·원격 Push 없음.
