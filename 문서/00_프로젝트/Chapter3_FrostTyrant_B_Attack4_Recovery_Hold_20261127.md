# Issue #30 / 설빙의 폭군 Attack4 복귀 국소 편집 검토 — 2026-11-27
[최신 기획 지시](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-7207776987), [Codex v3 검수](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-7207318417) 확인. v3 파일·매트·Idle 진입·체급/접지/우향/격리 배율 PASS, 앞팔 02→03→Idle00 PARTIAL. 신규 생성 재시도0.
## 실물 검토
v3 ZIP SHA256 07de830f1e299c7545121d5669d19e55cf2a9db028ede02bc6d491530ef7ba32, CRC 18/18 정상. 로컬 추출 프레임4/시트/KeyPose/Idle4 모두 ZIP 수록 바이트와 일치(7/7).
- Attack00 332e0bfe53a6ee6c36c0a712c83e66a601969ee2309695cf574825409cde8b15
- Attack01 b3f25db3f0075ea35f6bb1fcbd3403ede5f9033f5497bb708dd9b0769c3c27c0
- Attack02 2b69147031dbe31d0237cb340d61ad5757ac746ed00b0c708dd7c7f28d61b587
- Attack03 9a06ac61727a9ce9b120657a0ad0dc18a1a3b47550e2653becc10e268360741d
- 시트 41bd76bf537e053cadc790939643259ba162d4bccb5653847fb47a770f42f132
- KeyPose f1a3c5f91a5fef69cd881e27b578c32253d0124d5d8628a8ca748f7479333830 / Idle4 9d9b8d05be1813c013a5d784d749ce2d8ef751541ac36ce90712f3255cb53f43.
실제 Attack02/03/Idle00 좌표 비교 후 Attack03 복사본에만 수평 역매핑·완만한 어깨/하단 가중치·premultiplied alpha 보간 적용, 강도 0.65/1.0/1.25 세 시험. 원본 파일 수정0. 세 시험 모두 bbox (42,47,481,490) 유지했으나 실제 어두운 배경 1:1 비교에서 앞팔·손목 서리 늘어짐, 흉곽 압축, 하체 경계 왜곡 확인. 통합 래스터에서 팔과 몸통/지지부가 겹쳐 있어 이 방식으로 자연스러운 회복을 얻지 못함. 다른 편집법의 가능성까지 부정하는 판정은 아님. 미세 수정만으로 해결됐다고 주장하지 않음.
## 판정
**V3_KEEP / RECOVERY_CONTINUITY_PARTIAL / V4_HOLD / NO_NEW_DELIVERY / UNITY_NOT_APPLIED**. 세 시험 모두 미채택, v4 공식 PNG/비교판/GIF/APNG/ZIP 납품0. v3 4×512/2048×512 RGBA/8FPS/PPU512/하단 중앙 pivot/Scale3/우향 기존 계약 유지. 기존 파일/매트/진입/체급/접지 PASS 보호. 미술 최종 승인 및 본편 적용은 HOLD. 기존 KeyPose/Idle4/화로/결빙 변경0, Hit/Defeat 제작0, Unity 실행/적용/Scene/코드/main 변경0. #85/#131/#132/Save/UserData 보호. 차후 회복 개선은 별도 승인 및 팔 레이어 분리 등 안전한 방법 검토 시에만.