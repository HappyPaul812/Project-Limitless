# Field03 Rock B v2 후속 확인 — 2026-10-12

**ART_FIX_PENDING / 수정본 기술검사·접지 QA NOT_VERIFIED / 신규 적용0.**

B2 통합·QA·Commit 및 사용자 Push(원격47a65e2)를 확인한 뒤 조사했습니다. 지정 파일 `F:/Downloads/Limitless_Field03_RockB_Fix_v2.zip`은 존재하지 않습니다. F:/Downloads와 C:/Users/windows/Downloads 아래 ZIP을 재귀 검색했지만 Field03/Rock/Fix에 해당하는 Rock B v2 수정본은 발견하지 못했습니다. F:/Downloads/Limitless_Sprites/Limitless_Field03_Renewal_Art_v1.zip은 v1 원본으로 확인되는 파일명이며 수정본으로 사용하지 않았습니다. 다른 캐릭터 Fixed ZIP도 적용 대상이 아닙니다.

기존 v1의 분리 alpha14픽셀과 접지 불일치 문제는 해결됐다고 주장하지 않습니다. PNG를 수정·재생성하지 않았고 기존 RockNorth Sprite1개를 유지합니다. 신규 수정본 PNG/Manifest/CRC/분리픽셀 제거/Runtime 접지 검사는 자료 부재로 NOT_VERIFIED입니다. 기존 공식375/376 환경 참조와 RockNorth 좌표·Scale·Collider·GUID/meta는 변경하지 않았습니다. Field03 Scene SHA256 `cf9fac91a5cc0f49c58f07c15fcea1ee1c27de5bb64daa8e53c1727ad69551d1`가 작업 시작과 동일합니다.

## 다음 아트 납품·검수 조건

- 실제 v2 ZIP/Manifest와 PNG 해시·크기·alpha bbox를 제공해야 합니다.
- 상단 분리 alpha14픽셀 제거를 기계 검사하고 본체의 손상·잘림이 없는지 확인합니다.
- RockNorth의 기존 좌표(4.5,4.4), Scale 및 BoxCollider(size0.4×0.22/offset0,-0.22)를 보존한 실제 Unity 확대 화면에서 접지를 비교합니다.
- 기술 문제가 해결된 경우에만 해당 Sprite 참조1개를 교체하고 나머지375개를 보호합니다. v1을 임의 보정하여 대신 연결하지 않습니다.

Main08~11·TTS/BGM·몬스터·리스폰·Save 변경0입니다. Field03 연결 경계는 이번 B1 봉인문 QA에서 왕복·Missing 검사로 확인했고 과거 통과 스토리 전체를 재실행하지 않았습니다. 기존 Search 예외는 별도 유지합니다. 이 문서는 자료 미납과 보호 상태의 기록이며 미검증 항목을 PASS로 처리하지 않습니다.

사용자337항목(822파일) 보호, 원본 Editor와 화면 무접촉, Git Push 미실행. B1 봉인문 보완 Commit `6c78cdb833e873639b0a027d5cfdc7efb5a83b3f`과 별도로 이 자료 대기 상태를 문서 Commit합니다.
