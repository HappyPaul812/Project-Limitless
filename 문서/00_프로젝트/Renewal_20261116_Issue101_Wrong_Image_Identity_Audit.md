# LIMITLESS / Issue #101 / 잘못된 비교 이미지 노출 경위 및 실물 PNG 무결성 재확인

**작성:** 2026-11-16 / 담당 브랜치 `docs/art-renewal`. **범위:** 읽기 전용 실물 감사·표시 오류 정정. Unity/원화 수정 없음.

## 상황과 결론
바울 대표가 이전 **손 보정 v2** 안내 화면에 대상이 아닌 금발 여성 캐릭터가 노출된 사실을 지적했습니다. 실제 납품 ZIP과 원본·수정본 PNG를 직접 대조한 결과 **실물 OGA10·OGA07 캐릭터는 정상**이며 금발 타인 캐릭터 파일 혼입이 없습니다.

### 표시 오류의 원인
이전 채팅 답변에서 **실제 비교 PNG를 지정하지 않고 이미지 검색형 `AsyncImageGroup`에 관련 문자열을 넣어 별도의 비교 이미지가 표시되도록 작성**했습니다. 이 이미지 조회 결과는 ZIP에서 추출한 픽셀 증거가 아닌 **무관한 이미지 표시**였습니다. 또한 검증과 무관한 설명용 이미지 생성 결과도 있었으므로 **그림 기반 설명과 실제 파일 증거를 구분하지 않은 화면 제시 오류**입니다. 외부 이미지가 원본 PNG로 들어간 것은 아닙니다. 문제 이미지를 유효한 전후 비교 증거로 사용하면 안 됩니다. 금발 이미지 자체의 정확한 출처/원본 파일명은 앱 UI로부터 회수하지 못했으므로 알 수 없음.

## 실제 검사 범위
- 사용자 요청 경로: `F:\Downloads\npc101_fix.zip`. **현재 대화 도구는 사용자 Windows F: 드라이브를 읽지 못합니다.** 따라서 해당 로컬 사본에 대한 SHA 검증이라고 주장하지 않습니다.
- 대신 **이전 납품과 동일 이름의 현재 실행환경 실제 파일** `/mnt/data/npc101_fix.zip`을 `zipfile`로 열어 확인. **ZIP CRC PASS**; 항목은 정확히 `OGA10.png`, `OGA07.png` 2개. 해당 PNG 바이트가 같은 실행환경에서 보존된 손 보정 v2 후보 파일과 **완전 일치**.
- 직접 비교한 원본: `/mnt/data/LIMITLESS_Issue101_OGA10_OGA07_20261116/Candidates_512`, v2: `/mnt/data/LIMITLESS_Issue101_HandFix_v2_20261116/Candidates_512`.
- Python/Pillow/NumPy로 **동일 512×512 RGBA 좌표별 직접 비교**, 변경 마스크와 원래 보정 허용 손 부위의 범위를 대조. 기존 승인 NPC 6종 전혀 수정하지 않음.

| 구분 | OGA10 파티 관리인 | OGA07 주민1 |
|---|---|---|
| 원본 PNG SHA256 | `7a53cbb71544b69e996a06c7b576c9498d0565cc701a9123a6ad9485cd1d18a1` | `940aa98590333b1423ed5f49101c9cf5a146a0079c8e354fc33e0ce943dff798` |
| 손 보정 v2 PNG SHA256 (ZIP 내부도 동일) | `6be1a2d777a312dedd392b8ad6fbb2ba77b350d46d7bd2113d9d11bf09c7e6f3` | `640b82a236afd5ca6025d5eb28970009f40941d39eadedeeeea0e180d5034d28` |
| 변경된 픽셀 | 178 (RGBA 중 27개 alpha 변경) | 127 (RGBA 중 20개 alpha 변경) |
| 손 허용 영역 밖 변경 픽셀 | 0 | 0 |
| 모드/크기 | RGBA 512×512 | RGBA 512×512 |

### 실물 ZIP SHA
- 실물 `/mnt/data/npc101_fix.zip`: **287887 bytes**, SHA256 `ee98f2bde1062193f3b9100d9af67ca3b9fc2f17342c5ee30449193d20b2be3e`, ZIP CRC PASS, PNG2. 기존 `/mnt/data/Limitless_NPC_101_HandFix_v2.zip`: SHA256 `acbc523c6de831bc0d6b6556c36cd28cce297858218e38ca61e5414387063a64`, ZIP CRC PASS. 각각의 PNG는 같은 v2 해시.
- **새로운 실제 파일 기반 비교판:** `/mnt/data/Issue101_Actual_PNG_Verified_20261116/Actual_PNG_Original_vs_v2_512_128_40.png`; SHA256 **`412f87683ce62ccac49103bd466b6280836ea233ff2bce24fc15a1bdbc4ad817`**. Pillow가 실제 저장된 원본·v2 4PNG에서 512 원본 그대로 + 동일 LANCZOS 128·40 실축소판을 만들었음. **검색 이미지, 생성된 합성 인물 이미지 사용0.**
- QA 보관 ZIP `/mnt/data/npc101_verified_actual_pngs.zip` **913342 bytes / SHA256 `4c19c12c27cd387c2443e3179bef0788e3aef3fb94fea12fdb6d6ee12a5b8646` / ZIP CRC PASS**. 원본 2PNG + 손 보정 v2 2PNG + 정확한 비교판 + JSON/SHA 증거. F: 자동 복사·GitHub 바이너리 업로드0.

## 정정 및 보호
- 이전 대화의 출처 불명의 금발 인물 이미지는 **무효 비교 자료**로 배제; 실제 원본 PNG를 직접 읽은 비교판으로 교체. 캐릭터 재제작·전면 변경·손 추가 수정 모두 **0**.
- **`ORIGINAL_V2_MATCH_VERIFIED / WRONG_COMPARISON_DISPLAY_CONFIRMED / FILES_OK / NO_REART / USER_ART_REVIEW_PENDING / NOT_UNITY_APPLIED`**.
- OGA02·03·06·09·16·17 기승인·로컬 반영 상태, OGA20 남문경비 HOLD, 기타 주민, Unity6000.6.5f1, Issue85 HOLD, Save/UserData, Discord/원본 파일 및 main 전부 보호. 사용자 PC F: 파일 직접 확인은 현재 **NOT_VERIFIED**, 필요시 Codex가 **로컬 ZIP SHA 읽기 전용 검사만** 수행하면 됨. main 직접 Commit/Push/Merge0.
