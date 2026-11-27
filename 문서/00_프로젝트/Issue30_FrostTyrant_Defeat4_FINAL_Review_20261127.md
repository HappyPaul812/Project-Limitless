# Issue #30 설빙의 폭군 Defeat4 v1 FINAL 마감 검수

2026-11-27 · [정식 납품](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-7209775512) · [마감 지시](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-7209845637)

**FINAL 파일 정합성 PASS. 미술 승인 및 본편 반영 HOLD.**

- 실물 `F:/Downloads/LIMITLESS_Issue30_FrostTyrant_Defeat4_v1_FINAL_20261127.zip` SHA256 `59e8854c0d1e50865e2bc51e49d189f773150c68f999009301c416bbca5a3d1e`, CRC 18/18, SHA256SUMS 17/17. Manifest를 포함한 ZIP 내 모든 비-SUMS 항목의 SHA가 일치합니다. Manifest의 시트 SHA256 `feedd43f2f79486433f9e75b47ac5bc6af8f6626031178b3924e401b221adf16` 검증.
- KeyPose `f1a3c5f91a5fef69cd881e27b578c32253d0124d5d8628a8ca748f7479333830`, Idle4 `9d9b8d05be1813c013a5d784d749ce2d8ef751541ac36ce90712f3255cb53f43`, Attack4 v3 `41bd76bf537e053cadc790939643259ba162d4bccb5653847fb47a770f42f132`, Hit4 v1 `952e44855b7319c339a22b99fc35d8395d182f75b8d926be2de2f55b0e703713` 모두 승인 SHA 및 기존 Hit4 납품본의 실제 바이트와 일치합니다.
- 시트와 개별 512×512 프레임 RGBA 4/4 일치. 하단 152행(프레임당 77,824픽셀)이 각각 승인 Idle00/01/02/00과 RGBA 완전 동일합니다. 이전 v1의 알파 초과 최대 64가 복원됐고, 기존 v1과 가시 RGB·불투명도가 0보다 큰 영역의 마스크·상단 가시 RGBA가 동일합니다. 접지 y490 exclusive, 셀 테두리 알파 0. 336px 밝은 배경 비교에서 상체의 점진적 낮아짐과 광채 감소가 유지됩니다.
- APNG 단독 4/4·Idle→Defeat 8/8 디코딩 RGBA가 셀과 동일합니다. 전체 캔버스 SOURCE/NONE, 1회 재생, 125ms 및 마지막 1125ms를 확인했습니다. 두 GIF 모두 반복 확장 없이 120/130ms 교대와 마지막 추가 1000ms 홀드를 확인했습니다.
- [기존 Unity 검수](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-7209211654)의 6000.6.5f1 실제 336px 표시 및 마지막 프레임 유지 PASS를 재사용했습니다. FINAL에 대한 새 Editor 실행은 하지 않았습니다. 실제 보스 종료 트리거·퇴장 미검증을 유지합니다.

로컬 `Temp/i30-defeat4-final-qa/`의 `Idle_v1_Final_336_Light.png` SHA256 `b36a1a7c6023ffc9cbc48ae27b243b8a1dc6f5a93fb8c8808aa2342373f362eb`, 어두운 배경 `Idle_v1_Final_336_Dark.png` SHA256 `5a8cf0c71e597c27907292c95ae06ffba009c5523e4ba093d40b81d6e805b323`, 검증 로그 `file-qa.txt`. 비교판은 납품 PNG입니다.

승인 자산·화로·결빙 배경·Save/UserData 및 #85/#131/#132 보호. 본편 적용·main 병합·Push 없음.
