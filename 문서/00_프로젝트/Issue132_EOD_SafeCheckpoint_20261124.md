# Issue #132 · 2026-11-24 개발 마감

## 통합 제품 PASS

- 제품 `931325391a3f5ae1da8c4e7f0ceb7f98a64519d9`은 #131 제품 `5399c89b2be41c2c7ea0a0aedb36b16eb70f3120` 및 보고 `9d155ca635143b217a9cdd68a0c3e8be54106cb4`를 포함합니다. #85 호환 패키지와 치유사 GUID, 기본 전투 버튼과 잡템 4종, Main24·25와 승인 음성 19개, 수호자 3종을 재사용합니다. Unity 6000.6.5f1을 유지합니다.
- #131 클린 Import·컴파일·게임 137 PASS와 #124 수호자 로딩·공유 7 PASS를 재사용했습니다. Unity 제품 파일을 추가 수정하지 않았습니다. #85 Editor Search 초기화 예외는 미해결입니다.
- 기획·신규 아트·리뉴얼·TTS·BGM 최신 인계 5종과 관련 승인·검수 문서를 선별했습니다. 복구 전용 `Recovery/Issue132_20261124/ApprovedArt/`에는 F4 2종, F5 균열, 빙벽, 화로 v3 및 Flame8의 승인 PNG 16개와 출처·SHA256을 보존합니다. 미적용 아트는 Unity 경로 밖에 있습니다.

## 로컬 main 동기화 HOLD

- 원본 `65cceb2d6c3283d18abec6e27d8a6f1296411f17`과 후보 간 겹침 8경로를 다시 대조했습니다. 치유사 meta 5개는 줄바꿈 정규화 후 같으며, 원본 바이트는 보존했습니다.
- manifest와 lock의 원본 작업본에는 AI Inference, App UI 및 별도 패키지 버전 세트가 있습니다. BuildSettings에는 App UI 구성과 후보 F3 등록의 차이가 있습니다. F3 등록은 후보에 보존했으며, 사용자 측 패키지 세트의 의도·호환성을 확정할 근거가 없어 3개 설정의 혼합이나 원본 FF를 하지 않았습니다.
- 원본 미커밋 303·미추적 550 항목과 Save/UserData를 포함해 로컬 전용 바이트 백업을 생성하고 CRC·SHA256을 검증했습니다. 로컬 백업은 Git 제외 경로에 두었으며 전송 대상에 포함하지 않습니다.

## 복구 범위

- 11/23 원격 복구본을 부모로 새 스냅샷을 구성합니다. 이 방식은 승인 제품과 보고 파일을 보존하면서 로컬 작성자 개인 이메일이 있는 신규 이력의 전송을 피합니다. 기존 원격 ref의 변경은 없습니다. 결과와 독립 수신 검증 영수증은 [Issue #132](https://github.com/HappyPaul812/Project-Limitless/issues/132)에 기록합니다.
- ZIP 원본, 검수 GIF·APNG, 개발 QA 파일, Stage 원장, Save/UserData, Discord, 개인 파일과 비밀 정보는 이번 증분 복구에서 제외합니다. 승인 F4 BGM 마스터 납품 ZIP은 `F:/Downloads/bgm/LIMITLESS_Issue129_FinalLoopMasters_20261124.zip`에 남겨 별도 백업 대상입니다. 납품 기록 SHA256 `2ab6609247c0232565e3adf51c0654b9d417ce9300a7634bc4a42c69e9af26ac`. 승인 아트 원본 ZIP 이름과 해시는 복구용 README에 있습니다.
