# Issue #30 / 설빙의 폭군 Hit4 v1 — 승인 Idle 기반 2D 편집 (2026-11-27)
[Attack4 v3 대표 최종 승인](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-7208175259), [제작 방식 변경](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-7208484717) 확인. 신규 생성 재시도0, 생성 시안/차단 결과물 사용0. Library 제목 검색에서 기존 승인 Hit4 미확보(제한 검색).
## 제작
- 승인 KeyPose SHA f1a3c5f91a5fef69cd881e27b578c32253d0124d5d8628a8ca748f7479333830, Idle4 SHA 9d9b8d05be1813c013a5d784d749ce2d8ef751541ac36ce90712f3255cb53f43, Attack4 v3 시트 SHA 41bd76bf537e053cadc790939643259ba162d4bccb5653847fb47a770f42f132 실물 3/3 일치 및 작업 전후 원본 해시 불변. v3 최종 미술 승인 KEEP, 기존 앞팔 복귀 PARTIAL 수용 기록 유지. 화로/결빙 변경0.
- 승인 Idle00/01/02/00에서만 파생. 상체 y<=170에 각각 (dx,dy)=(-7,0),(-17,-5),(-8,1),(-2,0) px 이동, y170~360 완만한 가중치로 감소, y360~511 원본 픽셀 복사. Premultiplied alpha bilinear 보간. 최대 17px 후방/5px 상방의 작은 반동 후 안정. 새로운 외형/상처/효과 없음. 별도 수작업 신규 포즈가 아닌 기존 Idle 변위 기반 후보. 336px 밝고 어두운 비교판 육안에서 뚜렷한 체급·발톱 왜곡 미발견. 반응 강도가 작아 실제 게임 판독은 별도 확인 필요.
- Hit00..03 512×512 RGBA 4장, 2048×512 가로 시트 SHA256 952e44855b7319c339a22b99fc35d8395d182f75b8d926be2de2f55b0e703713. 우향/PPU512/Pivot(0.5,0)/Scale3/8FPS/0.5초 one-shot 계약 유지 제안. Unity 미검증.
## QA 및 납품
- 하단 152행=77824픽셀 각각 해당 Idle 소스와 RGBA 완전 동일, 프레임 간 하단 동일이라는 뜻은 아님. bbox (22,25,490,490)/(17,19,491,490)/(21,25,490,490)/(22,25,490,490), y490 exclusive(마지막 가시 행489), 외곽 alpha0, 시트/셀4/4 일치.
- 실제 Hit4 및 Idle4→Hit4→Idle4 4/12프레임 APNG 각각 125ms/원본 RGBA 픽셀 일치, GIF 120/130ms 교대 평균8FPS 어두운 매트 및 반복 미리보기. 밝은/어두운 336px 실제 픽셀 비교판.
- LIMITLESS_Issue30_FrostTyrant_Hit4_v1_20261127.zip, 18735238 bytes/17 entries, SHA256 dfc12372ffcb5076eb2e1d4546e6407c00496b8d7080f9ffce23b9bc3a2eb29c. CRC17/17, SHA256SUMS16/16 PASS. Manifest/QA JSON/참조 원본3종 포함. 실제 대화 다운로드 납품, GitHub 바이너리 미업로드.
**OFFLINE_FILE_QA_PASS / USER_HIT4_ART_REVIEW_REQUIRED / UNITY_NOT_APPLIED**. Hit4 미술 승인 후 Defeat4 별도 발주. 기존 승인 KeyPose/Idle4/Attack4 v3/화로/결빙, #85/#131/#132, Unity6000.6.5f1, main/Save/UserData 보호. Unity 실행/적용/Scene/코드/main 변경0.