# LIMITLESS / Issue #123 / 마지막 온기의 화로 Flame8 파일럿 — 2026-11-24

[기획 발주](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7126854316), [v3 Codex 표적 PASS](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7126793451) 확인. 승인 원본 KEEP, 신규 애니메이션만 사용자 미술 검토 필요.

## 실물 제작
- 승인 v3 ZIP SHA 1f4caab7cd712fc325450b8644e727dd6fc4719e876810d0c370fd5ffbc4e676, CRC PASS. 게임 PNG SHA 3a3094749ba49c6b7139b9638a61d543fa903d7ec214209f2b51a43985c1a5d2 및 master SHA 2403ef51efbc105276f3136fc7f5cb9f913468da556fea49dc4085f3f0745358 바이트 보존, Reference 복사. 승인 보스 1포즈/결빙 원본 ZIP 실물 SHA f1a3c5f91a5fef69cd881e27b578c32253d0124d5d8628a8ca748f7479333830 / 5abfe30fcb1a09579a7617fe1df08079c0b83eeb982677cf57ec2bfc5166b6d5 일치, 원본 수정0.
- 파생 고정 베이스 Art/Hearth_v3_FixedBase_256.png 256×256 RGBA SHA ac19e6460e3565f97aa9a4b37fc7b487581df76145581af880e47fbc5a470c2b. 원화에 합성된 중앙 광휘를 분리한 것이므로 **단독 표시하지 않음**, 원본 대신 파생 베이스 + Flame 레이어를 normal source-over 합성. 원본 v3 위에 중복 합성 금지. 불꽃 레이어에는 재구성된 국소 내부 공간 색/반투명 광휘도 포함. 별도 additive VFX가 아님.
- Art/FlameFrames/Flame00..07 8 PNG 각각 256×256 RGBA, Art/Flame8_4x2_1024x512.png SHA 55b41d6eb29db1d62b246982d9b00a0ad6737427251bfe93f317a7adb1bee7bb, 행 우선 0..3/4..7. 동일 정렬점, 제안 PPU128/하단 중앙 pivot; 실제 Import/Sorting 미확정. 8FPS·125ms·1초 무한 반복. 합성 8프레임 PNG 및 4×2 시트 검토용 별도. 원본 마력 리본 국소 주기 변형·광량 완만한 맥동, 결정체 외형/구조 고정. P2 및 별도 승리 후 애니메이션 제외.

## 오프라인 QA
- 고정영역 59529/65536 픽셀 모든 8프레임 RGBA 완전 동일, 동적 ROI6007픽셀, 합성 전체 알파 차이0, Frame00 RGBA 전체 승인 v3 동일, 8프레임 상이. 개별 레이어+베이스 실제 합성 및 시트 cell 8/8 일치. 강한 금빛 픽셀 수 동적영역 기준 2426~2787, 핵 평균 RGB 180.2~199.21; 소등 없음. 프레임 연속 평균 절대 RGBA 차 0.658~0.9701, 07→00 0.9637; 실제 시각 루프 승인 별도.
- 실제 합성 GIF 8프레임/120·130ms 교대/총1000ms/loop0, GIF는 어두운 배경 매트. APNG 8프레임/125ms 정확/loop0/투명 RGBA, 8/8 원본 합성 픽셀과 동일. 실제 PNG 1:1 및 40/30px 밝은 배경 비교, 승인 보스/빙벽/화로 비교. 생성형 가짜 비교판 사용0.
- 이전 북진 개념 배치도 바이트 보존 SHA 16641e5e8c9b0b5f69f00c90db1c6fb9d080c6b764af97aba2516b3e3fef6239. 남쪽 F5 입구→북쪽 보스 목표, 북쪽 화살표, 4.5WU 설계 목표 미실측, 실제 Scene/통행/배율/UI 가림 미검증. 화로 전투 명령/직접 이동 없음 유지.

## 납품
LIMITLESS_Issue123_LastWarmth_Flame8_20261124.zip, 5873898 bytes, 29 entries, SHA256 5bb82e0a505314c8b30d05938399d0c899677eb481274ec89142801860ec9e78, ZIP CRC PASS, SHA256SUMS 28/28. 실제 대화 다운로드 제공, GitHub 바이너리 미업로드. Manifest.md, QA/OfflineQA.json, DynamicRegionMask.png 포함.
**ANIMATION_ART_REVIEW_PENDING / OFFLINE_QA_PASS / CODEX_TARGETED_QA_PENDING / UNITY_NOT_APPLIED**. 기존 v3 승인 유지. #85/#131/Unity6000.6.5f1/Save/UserData/Discord/main 보호. Unity 실행/적용/Scene/코드 변경0, main 직접 commit/Push/Merge0. 사용자 미술 승인 후 별도 Codex 표적 검수, 본편 적용 별도 승인.