# Issue #30 / 설빙의 폭군 Attack4 v3 국소 보완 — 2026-11-27

[기획 발주](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-7207087008), [Codex v2 검사](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-7206929072) 확인. v2 파일/336px 배율 PASS, 미술 PARTIAL. 승인 KeyPose SHA f1a3c5f91a5fef69cd881e27b578c32253d0124d5d8628a8ca748f7479333830, Idle4 SHA 9d9b8d05be1813c013a5d784d749ce2d8ef751541ac36ce90712f3255cb53f43, v2 시트 SHA addd5627ed7c854a9a95b95b66845b836a466da1cff4972e007e53c426fd95e9 실제 일치, Reference 원본 바이트 보존. 기존 화로/결빙 변경0.

## 국소 수정
- 신규 생성/전면 재생성0. 밝은 저채도 연결 영역의 알파 정리. 밝은 실제 서리에도 영향 가능하여 외곽 및 내부 빈 공간 시각 재검수 필요.
- 00: 승인 Idle00 실물 하체 국소 합성, 상체/뿔 유지(알파 정리 제외).
- 01: 알파 정리 후 하단 지지부 2px 국소 보간, y490 복원. 전역 위치·스케일 변경 없음.
- 02: 타격 자세/좌표/배율 변경0, 알파만 정리. 남은 가시 픽셀 116984/116984 RGB 일치.
- 03: 기존 앞팔 국소 변위장 최대55px 설정, 실제 Idle00 지지부 합성 및 과잉 발 제거. 02→03→Idle00 전환 개선 시도. 국소 합성 경계, 발톱/서리 디테일, 앞팔 형태와 모션 연속성은 미술/Codex 검수 필요. 완전 해결 판정 아님.
- 프레임 bbox 00[22,47,490,490],01[22,15,491,490],02[13,61,499,489],03[42,47,481,490), RGBA 동일 픽셀 180254/224682/231935/161484 (각262144). 체급 전역 축소0. 4셀 512×512, 가로 2048×512 RGBA, 우향, PPU512/Pivot(0.5,0)/VisualScale3 계약. Unity 미실행.

## 납품 및 QA
- 시트 SHA256 41bd76bf537e053cadc790939643259ba162d4bccb5653847fb47a770f42f132. 개별/셀4/4 픽셀 일치, 외곽 alpha0, 상이4개. APNG Attack4 및 Idle→Attack→Idle 12프레임 각각 정확125ms, 실물 RGBA 일치. GIF는 120/130ms 교대 평균8FPS, 어두운 매트 미리보기; 게임 one-shot 계약 유지.
- 실제 동일 배율/배경 336px 밝은/어두운 Idle/v2/v3 비교 및 512px 1:1 비교. 생성형 비교판 없음. 파일 QA와 미술 승인 구분.
- ZIP LIMITLESS_Issue30_FrostTyrant_Attack4_v3_Targeted_20261127.zip, 24234785 bytes/18 entries SHA256 07de830f1e299c7545121d5669d19e55cf2a9db028ede02bc6d491530ef7ba32. CRC PASS/SUMS17/17. 대화 다운로드 납품, GitHub 바이너리 미업로드.
**OFFLINE_FILE_QA_PASS / TARGETED_ART_REVIEW_REQUIRED / UNITY_NOT_APPLIED**. Hit/Defeat HOLD. #85/#131/#132, Unity6000.6.5f1, 기존 보스/화로/결빙/Save/UserData/main 보호. Unity 실행·적용/Scene/코드/main 변경0.