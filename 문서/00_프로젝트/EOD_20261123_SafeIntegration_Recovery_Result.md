# 2026-11-23 마감 · Issue #128

- 통합 브랜치 `codex/eod-20261123-safe-integration`
- 제품 통합 `e5d58a840e1ee18524af9959ee84f195932d2542`
- 마감 전 원본 `736df561adb408c0225ed9052c37f2d45b7e24f5` 및 로컬 보호 ref `backup/pre-eod-2026-11-23-issue128`
- 동기화 및 새 복구 ref 영수증 [Issue #128](https://github.com/HappyPaul812/Project-Limitless/issues/128)

## 통합 범위

- #126 빙각 수호체 전투 표시와 KO 유지, 승인 PNG 5개. F5 씬·공식 조우·유니크 AI·드롭·Save·전체 승리 전환은 HOLD.
- #31/#127 승인곡 4곡과 Intro/Loop WAV 8개. F1·서릿길·활성 F2 후보 씬 배정. F3 정식 씬, 실제 장치 청취, 전체 씬/Save Continue, 상용 배포 권리는 HOLD.
- #122 승인 전리품 정의와 아이콘 각 6개, 100% 드롭 경계 수정 및 서리들쥐 정의 1/6. 나머지 5종과 Chapter 3 신규 Spawn·Drop·Save는 HOLD.
- #123 F3 아트는 기술 검수와 단일 오버레이 제한만 보존. #124 길 특성 출시 반영 및 독침·젤 후보 교체는 보류. #125 Main24 Stage 음성 10/10 청취 PASS, Runtime 연결 0/10. Stage 설치 및 카탈로그 반영 0.
- 기획·신규 아트·리뉴얼·TTS·BGM의 11/23 Markdown을 선별했다. 충돌한 담당 인계 5개는 기존 경로를 유지하고 `팀운영/인계/EOD_20261123_*_LATEST.md`로 원격 최신 스냅샷을 보존했다.

## 검증

- 정적 검증 3 PASS. 승인 소스와 Unity 변경 파일 64개 blob 일치, 신규 meta GUID 29개 고유, 기존 씬·Save·Voice 카탈로그·Unity 버전 변경 없음.
- Unity 6000.6.5f1 격리 전투·오디오 Play 11 PASS / 0 FAIL. 동일 실행에서 BGM DSP Loop와 KO 마지막 프레임 유지 확인. 최소 테스트 씬에서 실행했다.
- 커밋의 기존 Animation 15.1.0으로 시작한 전체 클린 컴파일은 기존 #85 패키지 CS1061로 실패했다. 호환 패키지 설정은 격리 QA에서만 사용했다. 호환 격리본 전체 컴파일 성공, 참조 검증 10 PASS / 0 FAIL. 새 Git 체크아웃의 출시 수준 컴파일은 #85 HOLD.
- #85 HOLD, 원본 사용자 패키지·meta 및 기밀 보호. Git 밖의 Stage WAV 10개, 제작 ZIP·원장·영상·개인 데이터 및 미추적 파일은 PC에 유지되며 이번 Git 복구 범위에서 제외된다.
