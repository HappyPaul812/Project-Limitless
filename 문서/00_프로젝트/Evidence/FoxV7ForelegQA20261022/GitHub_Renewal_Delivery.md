# LIMITLESS — Fox Run6 v6 미술 반려 및 v7 Frame03 오른쪽 앞다리 국소 복원 (2026-10-22)

## 반려 원인
- 대표 피드백: v6의 화면 오른쪽 앞발이 어색하고, 납품만으로 게임에 쓸 수 없음.
- v6는 지적받은 Frame03의 흰색 쐐기형 접합부만 수정해야 했으나 실제로는 Frame03 전신 포즈를 새 생성 그림으로 변경하여 약 60,629 픽셀 변화; 이로 인해 발/발목 방향이 기존 동작과 달라짐. **v6 미술 FAIL / 본게임 사용 금지**.
- SHA/CRC는 파일 무결성일 뿐 동작 미술 승인 증거가 아님.

## v7 수리 방식 — 기존 실 PNG 보호
- 사용한 정본은 기존 Fox v5 ManifestFix와 동일한 v5 프레임. v6 전신 교체 그림을 폐기하고 기준으로 사용하지 않음.
- Frame03만 v5를 기반으로, **기존 v3에 실제로 그려진 연속된 앞다리·발목·발끝 원화 일부**를 가슴 아래 국소 폴리곤에서 feathering하여 복원. 떨어진 밝은 앞발 조각/쐐기형 접합부를 없애고 발을 몸통에 연결하는 것이 목적.
- v5 Frame00/01/02/04/05 **압축 PNG 바이트 그대로 유지 5/5**. v5 Frame03 외부의 머리·귀·몸통·꼬리·다른 다리는 다시 그리지 않음. 변경 Frame03 RGBA 6,275픽셀, 변경 bbox x159..255, y185..294 (max exclusive x256/y295).
- 유의: 기존의 원화 모션을 보존하면서 복구한 후보이고, 다리의 진짜 근육/무게·발 딛는 타이밍/실제 Unity 12FPS 검수는 미실시. USER_ART_APPROVAL_PENDING.

## 실물 ZIP / QA
- 파일: **Limitless_Beast_Fox_Run6_v7_Frame03_ForelegFix.zip**.
- 크기: **2,990,546 bytes**; 최종 SHA256 **de6139858b2f5de95c9e8256a350f0e45744f15e404efb00161688c88e1c0030**.
- 실제 대화 다운로드: sandbox:/mnt/data/Limitless_Beast_Fox_Run6_v7_Frame03_ForelegFix.zip. GitHub에는 문서만 커밋하고 ZIP 바이너리 업로드하지 않으며 대표 PC F: 자동 저장 아님.
- 독립 512×384 RGBA 6PNG/Strip3072×384/3×2 Atlas1536×768, 12FPS GIF(500ms loop)/투명 APNG, 128·40px contact, Frame03 v5↔v7·v6↔v7 4배 비교, 02→03→04와 05→00→01 연속 미리보기, per-pixel 변경 좌표 CSV, bbox CSV/검증 스크립트, README/QA, Manifest SHA·bytes·CRC, 외부 SHA256SUMS.
- 검증: ZIP testzip CRC PASS, ZIP 모든 entry 실제 바이트 대조 PASS, Manifest 자기 자신과 SHA256SUMS 제외 **26/26 PASS**, SHA256SUMS 자기 자신 제외 **27/27 PASS**, 개별 PNG 디코드·사방 알파 3px, Strip/Atlas 포즈 RGBA 픽셀 일치 6/6, 원본 보존 5/5, PNG alpha bbox CSV 6/6 PASS.
- 이전 Manifest 순환참조 결함 재발 방지: Manifest는 Manifest/SHA256SUMS를 제외하고 나머지 파일만 해시/크기/CRC 기록. 별도 SHA256SUMS에는 Manifest 포함하고 자기 자신 제외.

## 상태 및 후속
- 정확 상태: FOX_V6_ART_REJECTED / FOX_V7_REAL_PNG_AND_ZIP_DELIVERED / OFFLINE_INTEGRITY_PASS / CODEX_ISOLATED_12FPS_V7_PENDING / REPRESENTATIVE_FINAL_MOTION_APPROVAL_PENDING.
- Codex에게 필요시 격리 프로젝트에서 Frame03 앞다리 및 02→03→04, 05→00 루프만 Delta QA. Bear v4 미술 PASS 권고·Wolf 승인본·Unity 본게임/Save/Scene/GUID/.meta/기타 몬스터·펫/환경/프로그램, main commit/push/merge 모두 보호.
