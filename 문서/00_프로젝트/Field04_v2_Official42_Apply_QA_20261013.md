# Field04 v2 42도로 정식 적용 및 QA — 2026-10-13

## 결과

- 승인 ZIP SHA256: `178ad6b235c65848cce48f4e2cfe86ff77c36857a12663237f7d7afa561f969d` 일치. CRC/PNG52/Manifest52 해시 PASS.
- 브랜치 `codex/field04-v2-official-42-20261013`, 기준 `origin/main 0e0476649b667129cb7aa6004f4b677335ed8be2`. 다른 작업 브랜치/PR7 병합 없음.
- 원본 디스크 반영: **APPLIED**. 원본 main HEAD 변경/커밋/병합 없음. Push 결과와 최종 SHA는 완료 보고에 기록.
- 정식 Scene 1개: `Assets/_Project/Scenes/Field_04_WesternBorder.unity`.
- 기존 Road30 ID 유지, x=-7…7/y=±0.5로 변경; 연장12 x=±8,±9,±10/y=±0.5 추가. 총42.
- Grass315 + Props18 + Road42 = 환경 Renderer375. 승인 PNG52를 `_Project/Art/Environment/Field04_V2_Official42_20261013`에 동일 바이트 등록. Optional Story3 미등록·미배치.
- 원래333 비도로 좌표/Scale/Sorting/Tint와 모든 원래Collider/Spawn/Exit/Prefab/Camera/Quest 구조 유지. 1,145 원래 YAML 블록의 허용 변경을 역변환하여 원문 동일성 확인. 기존 도로30 Transform 좌표와 Sprite363 및 부모children목록만 수정; 새로운12는 Collider 없음.

## 새 격리 실행 결과

**PASS 253건**, 자동 기능 FAIL0. 상세 [checks.txt](QA/Field04_Official42_20261013/checks.txt).

- Compile 성공, 환경375 Sprite 누락0, 고유Sprite52, PPU128, 새GUID충돌0, Center Pivot/Point/FullRect/투명도 Importer 정적 확인.
- Main12 실제 진입 완료, Main13 토양/하천 조사·세린 임시 합류·Story 승리·아르벨 도착·레온 대화 완료. 영구 합류 시점은 변경하지 않음.
- 실제 전투 UI 기본 공격으로 Story전투와 일반 몬스터2종 승리. 몬스터30초 미출현/35초1개 재출현/최종2개 중복없음.
- Field03↔Field04↔Arbel↔Field05 실제 Rigidbody2D 이동/물리 Trigger 왕복6개 및 기존 Handler/Spawn 연결 검사.
- 지체의 길 실제 원형Collider 반경0.5와 여유0.03의 접근경로147 waypoint 통과. 기존 서쪽Spawn 정확좌표는 겹침이므로 안전한 y=0.25 경로를 검사; Spawn 자체를 PASS 처리하지 않음.
- 실제 Bootstrap Continue 버튼, 조사 전·Story승리 후 저장/복귀 위치·Quest 동일. 모든 QA Save/설정은 격리 폴더에 기록.
- 16:9/4:3/21:9 화면과 동서남북 Camera viewport가21×15 Bounds 내부. RenderTexture 전체/도로/양끝 이미지 제작·육안 검토.

## PARTIAL / OPEN / 미검증

- **PARTIAL**: 정본 Unity6000.5.7f1, 실제6000.6.5f1. 격리 복사본에서 패키지 자동 호환 갱신이 있었으며 원본/브랜치 Packages·ProjectSettings 미변경. 정본버전 직접 실행은 NOT_VERIFIED.
- **PARTIAL**: 기존 UnityEditor.Search.SearchDatabase Index 예외 재현. 신규 Runtime 예외는0. 사람의 직접 플레이·입력감·청취/최종 화면 확인은 NOT_VERIFIED.
- **OPEN 기존**: 서쪽 Spawn(-8,0)/TreeWest 겹침 distance=-0.01631531. 북쪽(0,7.4) Collider 틈. 이번 작업에서 수정하지 않음.
- **OPEN 기존**: main의 GoldenArrow00/HealerMagicalProjectile00~04 기존 GUID 중복5쌍. 새 Field04 GUID에는 중복0. Unity가 격리 복사본에서 기존중복meta를 자동 보정했으나 해당 수정은 납품·원본 반영하지 않음.
- 미술의 서쪽 건조화는 승인 PNG/좌표 그대로 적용. 기존333 환경에 임의 색보정/장식 이동 없음. 배포권리는 납품 출처 문서 범위만 확인.

## 보호 및 반영

- 원본 Unity는 적용 직전 readonly 조회로 Play/변경/컴파일/갱신false, Field04 미로드, dirty Scene없음 확인 후 디스크만 복사. Play/Refresh/Scene전환/포커스 조작0.
- 기존 보호파일 **5551개 SHA256 불변** (허용된Field04Scene 제외). Main08 승인6 WAV/Catalog, 기존PNG/meta/Scene/Save/TTS/BGM/문서 유지. TTS API0.
- 원본Scene+meta 백업: `Temp/Field04Official42_20261013/OriginalBackup`. 원본Scene meta/GUID 불변. 기존 사용자 미커밋 상태 유지에 승인된Field04Scene 변경과 새아트파일만 추가.
- 원본 작업 트리에 브랜치의 전체Unity를 복사하지 않음. Field04 Scene과새52PNG/meta/5폴더meta만 반영.
- 기능코드 변경0, ThirdParty 변경0. 보고서/현재상태/인계는 작업worktree에서만 갱신.

## 직접 확인

안전하게 게임을 재시작하거나 Field04를 새로 진입한 뒤 42도로/양끝나무/세린·조사 지점의 가독성과 실제 조작감을 확인. 원본 Editor는 자동 Refresh/Play 재시작하지 않았음. 서쪽Spawn/북쪽틈은 별도 후속수정 대상.

[전후 비교](QA/Field04_Official42_20261013/before_after_contact_sheet.png)는 이전 native QA 캡처와 이번 새실행을 구분 표시. 이번 자동PASS 근거는 새 official42 실행만 사용.

실행 코드 검증: 납품 Runtime C# 전체는 QA복사본과 SHA256 동일. 기존 Editor QA helper의 출력경로만 격리 결과 폴더로 보정되었으며, 해당 보정은 브랜치에 미포함. Scene SHA256도 QA복사본·worktree·원본 디스크 모두 일치합니다.
