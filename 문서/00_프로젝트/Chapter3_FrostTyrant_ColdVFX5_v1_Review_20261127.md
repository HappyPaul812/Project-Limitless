# 설빙의 폭군 전용 냉기 VFX5 v1 검수 후보 — 2026-11-27

대표 직접 발주: 브레스 발사/피격, 한기, 동상, 동결괴사. 밝은 파랑/청백/회백, 바람/눈발/서리, 불꽃/번개/레이저/전면 연막 금지, 캐릭터/HUD 보호. #30 기존 보스 전투 아트 최종 승인 및 Battle 연결 PASS 기록 유지. [#88 공용 냉기 기획](https://github.com/HappyPaul812/Project-Limitless/issues/88), [#89 기존 승인 공용 6종 KEEP](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6787123247). 이번 5종은 보스 전용 후보이며 공용 상태 VFX/아이콘 교체 승인 아님.

## 납품
LIMITLESS_FrostTyrant_ColdVFX5_v1_REVIEW_20261127.zip, 21870816 bytes/67 entries, SHA256 35c653ef579d0a720606491b27548dcfce247ecb5f0cb9c9ef2305e46e094424, CRC 전체 PASS, SUMS66/66. 실제 대화 다운로드 제공, GitHub 바이너리 미업로드. 5종 40개 256×256 RGBA PNG, 1024×512 4×2 row-major 시트 5장, 8FPS 제안. APNG 정확125ms, GIF120/130ms 교대 어두운 매트. 발사/피격/괴사 one-shot, 한기/동상 loop 제안(상태 수명이나 발동 규칙 확정 아님).
- BreathCast 시트 SHA bd4352f49f93f820602f4a9754001e7c888d590dfd1014a06c0d0ea8da20a83f.
- BreathHit 2a818db064405cf53f145bd5160451d9329c04bc75d4463244aecc30caf9aa88.
- Chill 14b6cc30f006739e650daab64d909f713fa46572d5c3c5f82f562e1ca32441d4.
- Frostbite 0e32cc10883f7dc9d6c2145095f33bed7672fadce02a1372f79755ab4a81534d.
- Cryonecrosis 4a31bcb68aaf82388fc5a698b56025d73c4b462fa9c64cfac16d3260d4682282.

## 제작 및 제한
생성 소스 1774×887 5종 실제 바이트 보존. 균등 분할 경계 반올림, 240px 비례에 가까운 축소, 256px 캔버스 여백8, 16px 내부 feather, 광량 기반 alpha 감쇠, 상태 중앙부/한기 외곽 매트 감쇠 및 청회색 대비 보완. 원시 시안의 셀 경계에서 이미 잘린 내용 복원 아님. 밝은 배경 대비/소스 경계 자연스러움/프레임 전환은 미술 표적 검수. 외곽 alpha0, 상이8/8 각5종, 시트/개별40/40, APNG40/40 실제 RGBA 및 재생 메타 검증. Manifest 및 QA JSON 수록. 실제 승인 Idle4 참조 ZIP 바이트 SHA 9d9b8d05be1813c013a5d784d749ce2d8ef751541ac36ce90712f3255cb53f43 일치. 실제 160px 밝고 어두운 비교판, 보스 배치 목업(실제 보스, 도식 대상/HUD, Unity 증거 아님). 최대 알파 197/184/141/167/209, 상태 중앙 96×80 평균 알파 최대11.65/21.46/40.46. 실제 게임 가림 PASS 주장 금지.

기존 KeyPose/Idle/Attack/Hit/Defeat, 화로/결빙, 기존 #89 승인 상태 VFX/아이콘 보호. 신규 후보 PPU/Pivot/Sorting/실제 배율/트리거 미확정, 8FPS는 아트 검수 제안. 전투 상태/수치/판정/기믹/코드 변경0, Unity 실행/적용0, main 변경0. 실제 게임 검수 및 적용 별도 승인.

**OFFLINE_FILE_QA_PASS / USER_ART_REVIEW_PENDING / TARGETED_CODEX_REVIEW_REQUESTED / RUNTIME_SPEC_PENDING / UNITY_NOT_APPLIED**.