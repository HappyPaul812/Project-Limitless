# LIMITLESS / Issue #119 — 아르벨 포털 관리인 독립 정지 원화 납품 (2026-11-19)

## 기획 및 범위
- 대표 직접 지시: 포털 관리인 독립 원화 512×512 RGBA 정지 1포즈. [Issue #119](https://github.com/HappyPaul812/Project-Limitless/issues/119) 신규 추적 이슈 개설(기존 전용 이슈는 검색 범위에서 미발견).
- 11/17 복구본 ba59838bf923832362f3b1fccf4897864cc678fa의 Chapter2IntroFlow 및 VillageNpcAppearanceCatalog 확인: arbel-portal-keeper, (-7,-3), Resident, 기존 “연결 기능은 아직 준비 중입니다.”, 승인 OGA20 남문 경비 공유. 기존 담당 인계/Library 및 복구본 범위에서 독립 승인 원화 미발견. 로컬 전체 및 다른 브랜치 전수는 아님. 최초 그림 생성이 조사보다 먼저 이루어졌으며 조사 후 최종 후보를 납품.
- 기획 정본 Chapter3_서릿길마을_NPC_서비스_서브퀘스트_v1_20261019.md, #45/#56: 북부 동쪽 미래 포털 부지, 현행 포털 OFF. Main21 이후 펫 서비스 및 향후 안전거점 서비스 확장 방향만 시각에 참고. 포털 활성화/이동 기능/북부 신규 배치/ID, 대사·설정 변경 없음.

## 산출물 및 검수
- 최종 후보 `Arbel_PortalKeeper.png`, 512×512 transparent RGBA 정지 1포즈, 140331 bytes, SHA256 `31c063f5a6d80491d74cd8207b670e8340cd624c21f0af200d8911171ce1b4cf`. alpha>8 bbox (149,57,361,469), 투명 픽셀211979, 외곽 alpha 접촉0.
- 직전 생성 시안 `포털_관리자_여행_안내원.png` 1254² RGBA, SHA256 `68621d295859d1d96c07bf2826198e1bbfde46402f5c1e7acb4a0078bc89d523` 사용. 고립 알파 제거, crop(247,14,868,1220), 비율 유지 213×414 리사이즈/하단 y470 정렬. 이전 청색 복장 시안은 납품 제외, 최종 자주색 복장 후보 1장만 패키징. 기존 아트 합성/수정0.
- 모자·옆땋은 머리·자주색 짧은 망토·지도 두루마리·작은 원형 표장/낮은 채도 발광으로 길안내 역할을 표현. 은행의 안경/장부, 장비상인의 고글·공구, 펫 관리인의 수염/작업복 및 현행 경비의 갑옷/장병기와 차별화. **펫 분양 담당과 갈색 머리·웃는 얼굴·환영 손짓·복장/소품 모티프 유사성이 남아 있으므로 차별화 최종 PASS로 단정하지 않음.** 40px 도구 식별 및 기존 평면 NPC 대비 세밀한 명암도 대표 시각 검토 필요.
- 실물 OGA16·17·펫 분양/관리·레온·OGA20·아르벨 임시 주민 후보·신규 8종 512/128/40px 비교판 `PortalKeeper_Actual_512_128_40.png`, SHA256 `6dd2edabe93a6cec569e4a0da5ea523c69e3be10854b7f2d42f3e50f5556d99b`. 실제 PNG 기반 LANCZOS 축소, 참조 전후 SHA 일치. 북부 공식 NPC 원본 PNG 미확보로 직접 비교 NOT_VERIFIED, Unity 화면 아님.
- 전체 ZIP `Limitless_PortalKeeper.zip`, 1215647 bytes, SHA256 `4cab4373d0b8d2a1efddb0b38230aa459a5848d8149ef4d1a34233a8ab6f14cd`, 9파일 CRC PASS / Manifest7/7 / SHA256SUMS8/8 PASS. PNG 전용 ZIP `PortalKeeper_PNG.zip` 139766 bytes, SHA256 `aa720ef47900f0e99e767949bcbb90622355d550d55227608a8cf8623ee2928c` CRC PASS. /mnt/data 작업환경 파일, Windows 자동 복사 없음.

## 승인 및 보호
- FILE_TECH_PASS / USER_ART_REVIEW_PENDING / NOT_UNITY_APPLIED. 사용자 미술 승인 전 제품 적용 금지. 기존 승인 원화, 임시 주민 시안, 펫 분양/관리 승인 후 미적용 상태, 남문/카타콤 경비 그대로 보호. 후속 별도 Codex 승인 시 arbel-portal-keeper만 정확한 매핑, PPU/Pivot/발접지·기존 상호작용 검증. 북부 적용 범위/포털 기능은 별도 기획 결정.
- 초상화·애니메이션·Unity 적용0. Unity6000.6.5f1, #85 HOLD, 불씨망령 HOLD_UNTIL_NPC_RENEWAL_COMPLETE, Save/UserData/Discord/main 보호. main 직접 commit/Push/Merge0.
