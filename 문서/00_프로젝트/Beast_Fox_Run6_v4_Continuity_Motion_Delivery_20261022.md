# LIMITLESS — Fox Run6 v4 실제 연속 모션 후보 납품 (2026-10-22)

## 주문 / 이전 검수 / 보존
- Codex v3 기술+alpha bbox CSV PASS; 이번에는 bbox CSV 헤더/열 오류를 재수정하거나 정상 PNG를 재제작하지 않음.
- Target 01→02→03→04 paw lift/recovery/landing, preserve 05→00.
- 승인 Wolf Run6 v5, 기존 게임 Sprite/Animator·Save·Scene·GUID/meta, main 보호. 본 게임 자동 적용 없음.
- v3 ZIP SHA256: `d4727d0e5fedbb2db7d224f0684fc6c80c78221aa120da3bef356ab43b800696` — 바이트 보존.
- v4 변경 프레임: **01, 02, 03, 04**; 바이트 보존 프레임 **00, 05**.

## 실제 아트 수정 방식 및 한계
- Frame01 and04 local head/shoulder body-bob continuity adjusted without replacing tail; frame02 leading paw lift and toe-off locally articulated; frame03 near forepaw recovery with contrasting bent far foreleg derived from painted, separately angled foreleg of v3 Frame04. Preserves overall fox torso/tail shape and original 00/05 exact bytes. Not newly hand-painted all six frames; actual kinetic realism requires review.
- 모든 6프레임은 LEFT, 512×384 RGBA, 12FPS, 투명. 한 프레임 단순 이동만으로 전체 6프레임을 생성하지 않았으며, 변경되지 않은 프레임은 압축 PNG 파일 SHA까지 동일하게 보존.
- **파일 QA는 실제 Unity 모션 미술 PASS가 아님**: 마루 접지/속도/체중이동·다리/꼬리 실루엣은 Codex 격리 12FPS와 대표 개별 미술 심사 필요.

## 실제 파일 납품
- ZIP: **`Limitless_Beast_Fox_Run6_v4.zip`**
- 실제 크기: **3,698,704 bytes**, 최종 실물 ZIP SHA256 **`9c46764df7f88194b1d15663e4df34ecdb24367f7e65eed298dfc089221b5827`**, 엔트리 **35개**.
- 현재 대화 실물: `sandbox:/mnt/data/Limitless_Beast_Fox_Run6_v4.zip` (별도 대화 샌드박스, GitHub 바이너리 업로드/대표 F: 자동 저장 아님).
- Assets/Poses: `Fox_Run_00_v4.png`~`05_v4.png` 개별 PNG6.
- Assets: `Fox_Run6_v4_Strip_3072x384.png`, `Fox_Run6_v4_Atlas3x2_1536x768.png`.
- Preview: 12FPS 128px GIF + transparent APNG(각 6프레임 500ms); 40px/128px contact sheets; v3 vs v4 12FPS 비교 GIF; 관심 프레임 고해상도 상하 비교; 05→00→01 정적 연결 비교.
- Metadata: `Manifest.json` (개별 파일 SHA256/크기·프레임 규격·변경범위), `SHA256SUMS.txt`, `Frame_Descriptions_KO.md`, `QA/Frame_Gait_And_Contact.csv`, `QA/verify_bbox.py`, `QA/Offline_Validation.json`, `README.md`.

## 실제 파일 QA (완료)
- Pillow PNG 6/6 512×384 RGBA decode PASS; 모든 6 프레임 고유 SHA.
- 외곽 3px alpha bleed 0; Strip 6-cell→frame pixel identical 6/6, Atlas 6-cell→frame pixel identical 6/6.
- GIF 6프레임 (80+90+80+90+80+80ms=500ms), APNG 6프레임 (83+83+84+83+83+84ms=500ms), 평균 12FPS.
- bbox CSV는 기존 정확한 규약 그대로: 좌상단 (0,0), x→오른쪽, y→아래; alpha A>0; min_x,min_y,max_x_exclusive,max_y_exclusive. 새 PNG6 전부 실제 픽셀에서 재산출하여 Python 재계산 대조 6/6 PASS. 이전 메타데이터의 통과상태를 왜곡하지 않음.
- zipfile.testzip()==None CRC PASS; ZIP 35개 엔트리 재열어 내부 바이트와 외부 작업 파일 정확히 동등, Manifest 멤버 SHA 대조 PASS.
- 최종 생성 ZIP의 SHA는 **압축 후 재변경 없이** 재열어 산출하여 `/mnt/data/Limitless_Beast_Fox_Run6_v4_ReleaseQA.json`에도 기록.

## 후속 승인 게이트
- **상태**: `FOX_RUN6_V4_PNG6_DELIVERED / ZIP_PNG_META_TECH_PASS / CODEX_UNITY_12FPS_V4_PENDING / USER_RUN_ART_APPROVAL_PENDING / MAIN_UNCHANGED`.
- Codex는 Fox v3/v4 동일 크기·12FPS 변경분만 비교해 적정 체중중심·발 접지·관절 회수·05→00 루프 확인. 메타데이터 전수 QA 불필요. Bear/Fox 사용자 승인 전 자동 교체 금지.