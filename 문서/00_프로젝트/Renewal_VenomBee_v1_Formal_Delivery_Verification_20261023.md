# LIMITLESS / Issue #63 / VenomBee v1 정식 납품 재확인 — 2026-10-23

바울 대표가 Windows 로컬 전달 경로 `F:\Downloads\Limitless_Chapter1_VenomBee_OriginalKeyArt_Pilot_v1_20261023.zip`을 확인함. **로컬 F: 파일의 바이트를 이 세션에서 직접 읽지는 못했으므로 아래 SHA는 채팅 납품 원본 ZIP을 실제로 다시 계산한 결과**이며 F: 복사본의 별도 SHA 대조는 Codex 읽기전용 확인 대상이다.

## 납품 바이너리 실물 재검증
- ZIP: `Limitless_Chapter1_VenomBee_OriginalKeyArt_Pilot_v1_20261023.zip`
- 재검증 크기 **2,291,579 bytes**, SHA256 **`2e65662d68c77cfb90d7e39d9d40c2cd2816ee33882debe8a8e93ace782bd478`** (작업 컨테이너 /mnt/data 원본을 SHA256 계산)
- 앞선 zipfile 테스트 **8 entries / CRC PASS**. 동일 파일 SHA 재대조 일치.
- 원화: `VenomBee_OriginalKeyArt_Pilot_v1_512.png` **512×512 RGBA, 단일 1포즈**
- 게임 크기 확인용: `VenomBee_Preview_128.png` **128×128**, `VenomBee_Preview_40.png` **40×40**. Unity 실 게임 PPU·배치 시험은 아님.
- 참고 디자인 보드: `VenomBee_DesignBoard_Source.png`; 문서 `SourceAndRuntimeMapping.md`, `QA.md`, `Manifest.json`, `SHA256SUMS.txt` 포함.

## 비교판 정확도와 제한
- 포함 디자인 보드의 비교·스타일 설명은 **참고용 생성 콘셉트**이다. 실제 현행 Idle/Attack PNG 원본과 공식 Chapter2 원화 파일들을 옆에 놓고 픽셀 및 GameView 비교한 증거판은 **미제출 / NOT_VERIFIED**. 이를 '현행 원본 비교 PASS'로 표기하지 않는다.
- 현행 참조는 `02_VenomBee.asset`: Idle GUID `24000000000000000000000000000004` (238×215, 10프레임), Attack GUID `24000000000000000000000000000005` (315×253, 10프레임), 10FPS. 두 GUID의 실제 원본 PNG 경로·라이선스 역추적 **RIGHTS_SOURCE_PENDING**.
- 기존 외부 PilotBee OGA CC BY3/압축파일 CC BY4 충돌은 별도 근거 확보 전 미확정. 신규 후보는 기존 PNG 트레이싱/리컬러가 아니라 새로 생성된 이미지에서 자동 분리한 컨셉이다. 네이티브 512 수작업이 아니며 알파 마스크 경계는 시각 보완·GameView 확인이 필요하다.

## 승인·보호
- 상태 **`ART_CANDIDATE / USER_REVIEW_PENDING / NOT_UNITY_APPLIED`**. 바울 대표의 **시각 승인 필수**, Idle10+Attack10 **20프레임 제작 금지**, 원본 Sprite/GUID/meta/Scene, MonsterDefinition/독/드롭/Spawn·AI, Save/UserData 불변.
- Wolf·Bear·Fox, Chapter2 공식 몬스터/보스, OGA03·OGA09·OGA20 NPC 기존 시안, 숲거미 HOLD 및 기존 54행 감사 변경 없음.
- 기존 순차 기록: 원화 보고 커밋 `3840193957f8f83e67f820bd4065ee4519e00c50` → 인계 커밋 `76898099fe57872f88aa07d2c54f0cbef2b9a51e` → Issue #63 댓글 `6356050169`. 이번 기록은 **내용 정확도 보완이며 기존 세 단계를 대체하거나 기존 바이너리를 다시 제작한 것이 아니다.**
- 로컬 F: ZIP SHA 검증·현행 PNG의 실제 비교 자료는 추후 Codex 읽기전용 작업. 메인 병합·Unity 적용 0.
