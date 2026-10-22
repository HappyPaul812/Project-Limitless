# LIMITLESS — Fox Run6 v5 Manifest 무결성 수정 및 실제 ZIP 납품 (2026-10-22)

## 원인
- Codex v5 검사: ZIP CRC PASS, SHA256SUMS 38/38 PASS, Manifest 37/38 FAIL. Manifest가 기록한 자신의 크기는 9,184 bytes였으나 실제 9,345 bytes로 SHA/CRC도 불일치.
- 원본 ZIP 39개 항목 중 Manifest 자체만 SHA/크기/CRC 불일치였고 나머지 37개 항목은 전부 기존 Manifest 값과 일치. 자기참조 체크섬이 원인이므로 구조적으로 제거.

## 수정 내용 (오직 2파일)
- 기존 v5 ZIP: Limitless_Beast_Fox_Run6_v5.zip; SHA256 5f6da110fe64f280528d9768bc9154d4175c0724f8541d72aa9ebbecf1cd44df; 크기 3,131,578 bytes. 원본 보존.
- 수정한 내부 파일은 Manifest.json 및 SHA256SUMS.txt 두 개뿐. 그림 파일·GIF/APNG·Strip/Atlas·CSV·스크립트·설명 등 나머지 37개 파일은 원본 ZIP과 바이트 동일.
- 새 Manifest.json은 자기 자신과 SHA256SUMS.txt를 file_hashes에서 제외하고 정상 37개 항목의 SHA256/bytes/CRC32만 기록. checksum_policy에 자기참조 제외 규칙 명시.
- 새 Manifest 실제 크기 9,460 bytes / SHA256 f041e7d51c8ef1da79bed74a93b1ff684a25a5ffd3239f0a01a53d7edb0dae99.
- 새 SHA256SUMS.txt는 정상 37개 파일 + 최종 Manifest만 검증(38개). SHA256SUMS 자기 자신은 제외. 새 SHA256SUMS SHA256 242d039352e99bd90dd3743dbc2043263a3a0f547911a3e83e44906304fa9ed4.

## 실물 ZIP 납품 및 QA
- 파일: Limitless_Beast_Fox_Run6_v5_ManifestFix.zip (채팅 sandbox:/mnt/data/Limitless_Beast_Fox_Run6_v5_ManifestFix.zip).
- 실제 최종 ZIP 3,131,729 bytes / SHA256 b0968d4358c916057bd23734d426f6fa0e6548a0ad8880400e4d061413f2ae29 / 39 항목.
- ZIP CRC PASS. Manifest 37/37 SHA+크기+CRC PASS. SHA256SUMS 38/38 PASS. 37/37 일반 파일 원본과 완전 바이트 일치. 그중 이미지 31/31, Fox 독립 PNG6/6 원본 보존. bbox CSV와 PNG Alpha 6/6 일치.
- 모든 원본 ZIP의 내부 파일명/폴더명 유지. 파일 비교에서는 오직 Manifest.json과 SHA256SUMS.txt만 변경됨.
- 최종 외부 QA JSON: /mnt/data/Limitless_Beast_Fox_Run6_v5_ManifestFix_ReleaseQA.json 에 ZIP SHA와 37개 파일 각각 바이트/해시 증거 포함.
- 참고: 원본 Strip/Atlas는 일부 반투명 영역에서 포즈별 RGB 라운딩 차이(0/62/87/53/215/0픽셀)가 이미 존재. alpha 셀은 6/6 동일이며 원본과 수정본 두 ZIP에 공통으로 존재하는 사항이므로 이번 체크섬 작업에서 수정·재인코딩하지 않음.

## 보호 및 상태
- Bear Run6 v4 미술 PASS 권고, 승인 Wolf Run6 v5, 기존 Fox v5 아트, Unity 본편 및 main 원본 변경 없음.
- ZIP 바이너리 GitHub 업로드 또는 사용자 F: 로컬 자동 복사한 것이 아님. docs/art-renewal 문서만 커밋.
- 현재 상태: FILE_INTEGRITY_PASS / MANIFEST37_PASS / SUMS38_PASS / GAME_IMAGES_31_BYTES_PRESERVED / CODEX_FOX_V5_UNITY_MOTION_QA_PENDING / USER_ART_APPROVAL_PENDING.
- 다음은 Codex의 새 ZIP 체크섬 재검증 후 Frame03 관절 접합 및 02→03→04 실제 Unity 12FPS 미술 Delta QA. 미술 승인 전 게임 자동 적용 금지.