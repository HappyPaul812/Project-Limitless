# LIMITLESS / Issue #117 — 아르벨 펫 관리 담당 독립 정지 원화 납품 (2026-11-19)

## 선행 확인
- [Issue #117](https://github.com/HappyPaul812/Project-Limitless/issues/117), 담당 인계 및 접근 가능한 Library 검색 확인. 분양 담당 #113 미술 승인([승인 기록](https://github.com/HappyPaul812/Project-Limitless/issues/113#issuecomment-6943908814)), 게임 미적용. 관리 담당은 #111 감사 및 기존 카탈로그 기준 OGA16_Bank 공유. 관리 담당 전용 승인 원화는 확인 범위에서 미발견. 사용자 Windows 및 전체 브랜치 전수 조사는 아님.
- 신규 관리 담당 외형 1종만: arbel-pet-management. 새 이름·신상·스토리·서비스 기능·동물 추가 없음.

## 산출물 및 QA
- `Arbel_PetManagement.png`, 512×512 RGBA 투명 정지 1포즈, 148207 bytes, SHA256 `62a771d90ccf27bc9754779c900178d22740dee041b66b1ec74172b00df285b9`. alpha>8 bbox (152,56,361,469), 투명 픽셀 205938, 외곽 알파 접촉0.
- 신규 AI 시안 `a_clean_high_detail_pixel_chibi_style_game_sprite.png` 1254×1254 RGBA SHA256 `363fe31366b6b735446abb9ccd6ab382f3245fabe282b6367eb116bd15fec379`. 주 연결영역 주변 보존·외부 고립 알파 제거·종횡비 유지 212×414 축소·중앙 및 바닥 기준 y470 정렬. 기존 아트 합성/재생성 없음.
- 짧은 회색빛 머리, 수염, 듬직한 체형, 황토 조끼/청회색 실용복, 빗과 관리 가방. 분양 담당의 묶은 머리/올리브 복장/환영 손짓, 은행 안내인의 안경·정장·장부와 구별. 레온과 일부 머리·목천·수염 모티프 유사하나 짧은 머리/조끼/소품/실루엣 차이. 기존 OGA16/17의 단순한 평면 화풍보다 명암과 장식이 세밀하므로 미술 승인 필요.
- 실제 512/128/40px 비교판 `Issue117_Actual_512_128_40.png` SHA256 `03e39bdcec5da64fd550199d3ed8b7c0fb0eefccb8d2ad2ae619aa0981b806cf`. OGA17, OGA16, 승인 #113 분양 담당, 레온 최근 납품본, 이번 신규를 실제 PNG로 비교. 참조 파일 해시 보존 확인 및 #113 승인 SHA 일치. 오프라인, 실제 Unity 배치·월드스케일 아님.
- 전체 ZIP `Limitless_PetManagement_117.zip`: 807823 bytes, SHA256 `72b7d5312097ad0599916f92f656cdfbe896742bfb9c07468c5adf97b448cf8d`, 9 files, CRC PASS / Manifest7/7 / SUMS8/8 PASS. 원화1·128/40 preview2·비교판1·README/SourceDeclaration/QA/Manifest/SUMS.
- PNG 전용 `PetManagement_117_PNG.zip`: 147204 bytes, SHA256 `ccd1845a320e313a2cbcaa92c82e77da2e71fb3fd4b2bea3a3bfaeb3bcde42d4`, CRC PASS. Library `/LIMITLESS_Art_Delivery/Issue117_Arbel_PetManagement_20261119/`에 PNG·ZIP2·비교판 총4파일 저장 성공. 사용자 F: 자동 복사 없음.

## 보호 및 승인
- FILE_TECH_PASS / USER_ART_REVIEW_PENDING / NOT_UNITY_APPLIED. 승인 분양 담당 및 은행/장비상인 등 기존 아트 BYTE_KEEP. 기존 단색 Placeholder 별도 결정 유지. 초상화·애니메이션·Unity·Codex 실행0. Unity6000.6.5f1·Issue85 HOLD·Save/UserData·Discord 보호. main 직접 commit/Push/Merge0.
- 사용자 미술 승인 후 별도 Codex 최소 QA에서 arbel-pet-management 및 관리 fallback 범위, PPU/Pivot·실제 월드 높이·접지·라벨 확인. 512 마스터의 PPU28 단순 직접 임포트 금지.