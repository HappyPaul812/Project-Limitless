# LIMITLESS / Issue #124 / 2026-11-27 기본 공격 투사체 3종 정식 규격 후보

- Codex 감사 https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7210907911 및 3종 정정 https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7210987313 확인. 기본 공격 3종만. 파이어 볼 전용 원천은 이번 ZIP 제외, 후속 마도사 스킬 VFX 이관. 기존 파이어 볼/화상/정조준/화살비/적 공유 경로 임의 교체 금지.
- Sharpshooter_Arrow_32px_8f_4x2_RGBA.png 128x64 SHA256 ea91f0f3e37687ce1d6626f98dcd9e4183e540a04a87929e9b3c6ff8026d9f3b, 80ms/12.5FPS/52 UI px.
- Healer_Light_96px_5f_5x1_RGBA.png 480x96 SHA256 97f55488a23939fbb5a1074350d945dd3ab9aef986d4b810a58723c290616751, 50ms/20FPS/72 UI px.
- Mage_BasicFire_32px_7f_4x2_RGBA.png 128x64 SHA256 758a768d2f42bf31717c22a3e2c85516a247423a2df8f97a2512f2bb471d8795, 60ms/약16.67FPS/52 UI px. 마지막 셀 전체 RGBA 0.
- 우향, PPU32, 중앙 Pivot(.5,.5), 개별 Single/Full Rect/Point 계약을 Docs/QA.json에 명시(실제 Unity import 미실행). 독립 생성 원천3 보존, 셀 경계 alpha taper/밝은 핵심 정렬 및 시퀀스별 고정 배율 LANCZOS 축소. PNG 20개/시트3/동일 픽셀 APNG3/밝고 어두운 배경 GIF6. GIF는 3배 최근접 확대, 실제 80/50/60ms 및 프레임수 검증. 네 모서리 투명, 시트와 개별 프레임 일치, 미사용 셀 투명 검증. Review/Class3_Actual_UI_1x.png 및 3x.png는 감사 UI 표시 크기 Point 근사 정적 합성. 자체 시각 검토상 화살/빛/불 구분되나 치유사 밝은 배경 대비 약함, 기본 불 꼬리/크기 맥동과 프레임 연속성은 독립 검수 필요. 실제 게임 렌더/이동/타격 미검증.
- 기존 GitHub Resources/BattleProjectiles/GoldenArrow, HealerMagicalProjectile, Fireball 폴더 존재 확인, 실제 원본 PNG 파일명/바이트 미확보. Codex 로컬 1:1 증빙 바이너리 접근 불가. 외부 원본과 신규 실물 비교 미완료; 이전 미술 파일럿 비교판은 명시적으로 NOT_External. 후속 Codex 실제 원본 동배율 대조 필요.
- ZIP LIMITLESS_Issue124_BasicProjectile3_Original_20261127.zip 2157209 bytes SHA256 b36c33570075c0ac84a83082330f07d4dbf9a311659bb6a6fe353b9fe3ef6fc6. CRC42/42, Manifest40/40 SHA/size, SUMS41/41 PASS. 대화 다운로드 실물, GitHub PNG 업로드/F: 자동복사 아님.
- 승인15 아이콘/기능/수치/타이밍/VFX/기존 투사체/코드/Unity/Save/UserData/#85/#131/#132/main 변경0. Unity 실행/적용·병합·Push0. BASIC3_CONTRACT_SIZED_CANDIDATE / FILE_QA_PASS / EXTERNAL_COMPARISON_PENDING / USER_ART_APPROVAL_PENDING / NOT_UNITY_APPLIED.
