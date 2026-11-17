# LIMITLESS / Issue #113 — 아르벨 펫 분양 담당 독립 정지 원화 납품 (2026-11-17)

## 선행 확인
- [Issue #113](https://github.com/HappyPaul812/Project-Limitless/issues/113), 담당 최신 인계와 접근 가능한 ChatGPT Library의 기존 원화 검색, main의 VillageNpcAppearanceCatalog.cs를 확인. arbel-pet-adoption은 OGA17_EquipmentShop, arbel-pet-management는 OGA16_Bank를 공유한다. 현재 자료에서 분양 담당 전용 기승인 독립 원화는 확인되지 않음. Windows 로컬 및 모든 브랜치 전수검사는 아님.
- 최초 생성 시안이 선행 조사보다 먼저 나왔으나, 기존 승인 자산 존재 여부를 확인한 뒤에만 해당 시안을 납품 후보로 패키징했다. 후속 재생성 호출은 실패했으며 최초 시안만 사용. 외부 참조 해소가 아닌 역할별 독립 외형 제작, 기존 서비스 기능 변경 없음.

## 실제 납품
- 원화 `Arbel_PetAdoption.png`, 512×512 RGBA 투명 정지 1포즈, 134346 bytes, SHA256 `d64c342bd4bd8953bafce1feb8949d3cb5ce1a4b67ecf88cda3a6757beb4e47d`. Alpha>8 bbox (155,59,357,471), 캔버스 테두리 알파 접촉0, 투명 픽셀 211984.
- AI 생성 시안 `반려동물_입양_안내원_sprite.png` SHA256 `1bf5d4a7eac4a6843e44a3195263a23fb21ab63382bab595f2f8cf9f6b826b3a`. 주요 알파 연결영역 주변을 보존하면서 외부 고립 픽셀 제거, 종횡비 유지 축소 및 접지 정렬. 기존 NPC 픽셀 합성 없음.
- 머리를 높게 묶고 천 리본, 올리브색·크림색 실용복, 작은 빗·돌봄 주머니·줄고리, 환영 손짓. 장비 상인의 고글/공구 및 은행·관리인의 안경/장부와 차별. 별도 동물, 이름·신상·서사 추가 없음. 발바닥 그림자 포함. 기존 평면 화풍보다 명암이 세밀하고 발자국 문양과 소품은 다소 많으며 40px에서 세부 및 역할 판독은 사용자 미술 검토 필요.
- 실제 OGA17·OGA16·OGA03 원본과 레온 최근 납품 후보 및 신규 원화 512/128/40px 직접 비교, `Issue113_Actual_512_128_40.png` SHA256 `8e7c89198f096844131c1558282ab216f7ce63a9afa26e1ccc189c1ae860b53a`. 비교 기준 파일 SHA QA.json 기록, 원본 변경0. 레온은 최근 납품 후보로 표시하며 이 문서에서 승인 여부를 확정하지 않음. 동일 픽셀 캔버스 오프라인 비교이며 실제 Unity 월드 배율이 아님.
- 전체 ZIP `Limitless_PetAdoption_113.zip`, 722753 bytes, SHA256 `b99c853fb4c92f8f8f1a7b6ef10c6c0b83a18d2759f565b5387e20d309bdc307`, 9 entries CRC PASS, Manifest 7/7, SHA256SUMS 8/8 일치. 원화1/128·40 미리보기2/비교판1/README/SourceDeclaration/QA/Manifest/SUMS.
- PNG 전용 ZIP `PetAdoption_113_PNG.zip`, 133365 bytes SHA256 `55be5daee8f224391878e5f404e9f8b11030b6c495405d059654a72dcb2175a1`, 1 PNG CRC PASS. 사용자 F: 자동 복사 없음.

## 보호 및 게이트
- FILE_TECH_PASS / USER_ART_REVIEW_PENDING / NOT_UNITY_APPLIED. 승인 후 별도 Codex 최소 QA에서 arbel-pet-adoption 및 이후 분양 fallback 실제 범위 확인. 관리인 및 장비상인, 기존 원본, 레온, Main04/07 보호. 원본 PPU28에 512 마스터 그대로 임포트 금지. 월드 스케일/접지/라벨/기능 실게임 미검증.
- 초상화·애니메이션·Unity/Codex 적용0, Scene/Quest/StableID/GUID/meta/Save/UserData/Discord 변경0. Unity6000.6.5f1 및 Issue #85 HOLD 유지. 단색 Placeholder 변경0. main 직접 commit/Push/Merge0.
