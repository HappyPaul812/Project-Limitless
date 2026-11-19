# 2026-11-19 · 아르벨 주민·포털 관리인 승인 원화 적용

바울 대표 최종 미술 승인 및 게임 적용 지시. [임시 주민 #111](https://github.com/HappyPaul812/Project-Limitless/issues/111) · [포털 관리인 #119](https://github.com/HappyPaul812/Project-Limitless/issues/119)

| 대상 | 실제 위치 및 적용 경로 | 보호된 동작 |
|---|---|---|
| 마을 주민 | Arbel (1.5, -1.5)의 Scene `VillageNpc` 인스턴스에만 승인 스프라이트와 흰색 tint 오버라이드. 노란 placeholder tint가 PNG 색을 덮지 않도록 해당 인스턴스의 G/B만 교체. PPU 412, 피벗 가시 중심으로 기존 0.9 월드 단위 높이와 중심 유지. | 기존 이름·인사, transform 위치, 런타임 scale, 이름표, 트리거 및 공용 프리팹 유지. 별도 `arbel-resident-west`는 기존 연결 그대로. |
| 포털 관리인 | Arbel (-7, -3)의 `arbel-portal-keeper` 카탈로그 경로 한 줄을 독립 원화로 교체. PPU 360.5 및 발밑 피벗으로 기존 가시 높이 유지. | 기존 Resident 역할·좌표 및 ‘연결 기능은 아직 준비 중입니다.’ 유지. 포털 이동 비활성. 공유 OGA20 원화 및 다른 참조 유지. |

기존 [미술 납품 검수](https://github.com/HappyPaul812/Project-Limitless/blob/docs/art-renewal/문서/00_프로젝트/팀운영/인계/WORLD_ART_RENEWAL_LATEST.md)를 재사용했습니다. ZIP 각각 CRC 9/9, Manifest 7/7, SUMS 8/8, 512×512 RGBA와 승인 PNG SHA256 일치. GUID 충돌 없음. [개별 SHA 및 임포트 설정](Evidence/ArbelResidentPortalApply/ApplyQA.json)

Unity 6000.6.5f1 기존 어셈블리 참조 변경 C# 컴파일 및 Git diff 검사 통과. 기존 아르벨 Flow와 기존 승인 NPC 4종의 파일 SHA 불변. Scene diff는 지정 주민 인스턴스의 시각 속성 세 개, C# diff는 포털 관리인 Resources 경로 한 줄입니다. 격리 Editor에서 두 PNG의 Sprite 로드, 주민 Scene 참조·좌표·색·기존 Trigger 및 관리인 카탈로그 연결을 확인했습니다. 실제 Play 화면과 기존 상호작용은 미검증입니다. 원본 Editor 포커스·Save·UserData 조작 없음.

제품 커밋 `31a72cf42a890a7c43e32005ba583172a5733256`, 로컬 `codex/issue118-chapter3-balance`. 원본 작업 폴더에 제품 6경로 반영. 원격 Push 미실행.
