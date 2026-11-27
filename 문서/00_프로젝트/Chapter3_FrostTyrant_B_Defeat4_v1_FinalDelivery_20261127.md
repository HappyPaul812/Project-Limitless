# Issue #30 / 설빙의 폭군 Defeat4 v1 최종 납품 정합성 마감 — 2026-11-27

[기획 최종 발주](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-7209730687), [Codex v1 검수](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-7209211654) 확인. 기존 336px 외형/접지/자세 및 Unity 비반복 계약 PASS는 이전 검수 근거로 보존, 이번 Unity 실행0. 기존 v1 납품 PARTIAL의 파일 정합성만 수정. 미술 연출 유지, 신규 생성/재작화0.

## 원본 및 기술 수정
- 입력 v1 ZIP SHA256 0c7af59131557383359a819b4f5ccf738d767f47e416fbcb5512a7846101d1a4, 승인 Hit4 ZIP dfc12372ffcb5076eb2e1d4546e6407c00496b8d7080f9ffce23b9bc3a2eb29c 및 Attack4 v3 ZIP 07de830f1e299c7545121d5669d19e55cf2a9db028ede02bc6d491530ef7ba32 실물 확인/CRC PASS. 기존 ZIP 수정0.
- 참조 KeyPose/Idle4를 승인 Hit4 납품 ZIP의 실제 승인 바이트로 교체, Attack4 v3/Hit4 v1 유지. SHA256 4/4 일치: KeyPose f1a3c5f91a5fef69cd881e27b578c32253d0124d5d8628a8ca748f7479333830, Idle4 9d9b8d05be1813c013a5d784d749ce2d8ef751541ac36ce90712f3255cb53f43, Attack4 v3 41bd76bf537e053cadc790939643259ba162d4bccb5653847fb47a770f42f132, Hit4 v1 952e44855b7319c339a22b99fc35d8395d182f75b8d926be2de2f55b0e703713.
- 기존 하단 source-over 중복 합성으로 alpha 최대 +64 발생. y360..511을 각각 승인 Idle00/01/02/00의 정확한 RGBA로 복원, 77824픽셀×4 동일. 정정 픽셀 38691/39248/38881/38691. 완전 투명 RGB 0/126/175/186픽셀 정규화. 배열 직접 결합으로 시트와 개별 RGBA 4/4 일치. 상단 가시 RGBA 및 전체 가시 RGB와 nonzero-alpha mask 불변. alpha>8 bbox 01 우측 491→490(1px), 나머지 동일, y490 exclusive/체급/방향/자세 유지.
- 기존 APNG 02/03 디코딩 alpha 불일치 11023/10747픽셀, 최대255. 기존 PREVIOUS disposal 및 delta 영역 확인, 인코더 내부 원인 전체 확정 아님. 전체 512캔버스 SOURCE/NONE APNG로 재작성, Defeat4 4/4 및 Idle→Defeat 8/8 정확한 RGBA 일치, acTL plays=1, 125ms/cell 및 마지막 추가1000ms hold(1125ms). GIF는 어두운 불투명 매트/양자화, 120/130ms 교대 평균8FPS, 마지막 추가1000ms, 반복 확장 없음. 실제 뷰어의 최종 표시 유지 동작 차이는 가능. 게임 4셀/8FPS/PPU512/Pivot(0.5,0)/Scale3/우향/one-shot 및 마지막 셀 유지 계약 변경0.
- 512×512 RGBA4, 2048×512 시트 SHA256 feedd43f2f79486433f9e75b47ac5bc6af8f6626031178b3924e401b221adf16. 실제 336px 밝고 어두운 Idle/v1/최종 비교판. 새 Unity 캡처 아님.

## 정식 납품
LIMITLESS_Issue30_FrostTyrant_Defeat4_v1_FINAL_20261127.zip, 18398074 bytes/18 entries, SHA256 59e8854c0d1e50865e2bc51e49d189f773150c68f999009301c416bbca5a3d1e, CRC18/18, SHA256SUMS17/17 (Manifest 포함, 자기참조 제외). 실제 ZIP 대화 다운로드 제공, GitHub 바이너리 미업로드. 기존 v1 ZIP과 구분하여 FINAL 사용. QA/OfflineQA.json에 실측 보존.

**FINAL_DELIVERY_OFFLINE_QA_PASS / USER_DEFEAT4_ART_APPROVAL_PENDING / CODEX_FINAL_PACKAGE_RECHECK_PENDING / UNITY_NOT_APPLIED**. 이전 격리 Unity6.5f1 비반복 PASS 재사용, 실제 보스 종료 트리거/퇴장 미검증. KeyPose/Idle4/Attack4 v3/Hit4 v1/화로/결빙 및 #85/#131/#132/main/Save/UserData 보호. Unity 적용/실행/Scene/코드/main 변경0, 병합/Push0. 미술 승인 및 본편 적용 별도.