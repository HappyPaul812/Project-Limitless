# Field05 B42 최종 승인 정식 적용 QA — 2026-10-19

**최종 실행 완료:** PR22 main병합 `b95909387a7b6f597417041bfcd72f2b866d4e47`, 원본디스크 적용 완료. 원본 Editor Runtime은 포커스 보호로 미검증. 아래 준비 단계 기록과 구분합니다.

바울 대표의 최신 재개 지시로 미술 최종 승인 완료. 기존 `ebb363567b3932cce8af12e315dda03362c8fd19` 기능과 `f8afb80ce744fd1eef78ce1c5c7eb1d07187ebee` 검수 후보를 재사용했습니다. 별도 플레이 환경/빌드·새 비교화면·Field05 순간이동 테스트0. 추가 미술 승인 요청0.

## 결과

- **원본 Unity 디스크 적용 완료**: 승인PNG14(기존 Ground3/Props7 + v2 Road4), 새 Asset/meta32경로, Field05 Scene1개만 반영. Scene meta/GUID는 원본과 동일. 기존 원본 Scene이 HEAD 대비 clean임을 확인하고 백업한 뒤 적용했습니다. 사용자 Scene 편집 덮어쓰기0.
- 환경375=Ground315/Road42/Props18, 기존Road30이동+Collider없는12개추가, 선택소품7·서쪽Portal 추가0. 기존 Collider21개와 비도로 Transform 전부 동일. 새 GUID 충돌0. 신규 meta의 Unity 생성 끝 공백만 정리했고 Importer/PPU/Pivot/GUID 값은 유지했습니다.
- 기존 기능416+저장Scene84 PASS/0FAIL을 재사용. 이번 **66 PASS / 0 FAIL** 추가: 기존 검증 Save의 실제 Bootstrap Slot01 Continue→최종Scene375/PNG14/도로42 Z0FlipFalse/NoCollider/MissingScriptSprite0/Main14세곳/세린/몬스터2종35초데이터/Save→실제Continue 퀘스트·편성·위치 일치. 35초 리스폰·전투·이동·화면 수천개 검증은 불필요하게 반복하지 않았습니다.
- 사용자 원본4844파일 중 승인된 Field05 Scene1개만 변경, 나머지 보호. UserData23/Main08·Main18 승인WAV/meta/Catalog/Arbel/Field04/기존사용자meta 변경0. 원본 Editor/Play/포커스 조작0. 디스크 반영 후 원본 Editor의 자연 Import/Runtime은 미검증입니다.
- 기존 Search/URP 환경 예외와 Editor6000.6.5f1 대 main6000.5.7f1 차이는 환경 PARTIAL, 신규 Runtime Console0.

## 범위와 다음 단계

전용 브랜치 `codex/field05-eastwest-road42-v2-20261019`만 Push/PR/승인 범위 main 병합 대상으로 준비했습니다. Field05 기능 변경에는 C#·음성·다른 Scene 변경이 없습니다. 최신 main 문서 충돌2개는 격리 worktree에서 기존 완료 기록을 함께 보존해 해결했고, 사용자 원본의 dirty문서5개는 그대로 유지했습니다.

Main09는 우선 완료한 별도 PR21: 기능932c3573a0ad4a4d94c0b92c4d0bb932662ae419 / 최종27983bb3d5652dfd6fec0b837ad4a63e98fc5ed5, 실제427PASS/0FAIL. Main09 main병합/원본적용은 수행하지 않았습니다. Main18 PR18/19는 재작업하지 않았습니다.

최종 PR/병합 SHA는 병합 후 CURRENT_STATUS와 Codex 인계에 추가 기록합니다. 원본 local main은 ff022e63 유지이며 문서5개 충돌로 LOCAL_SYNC_PARTIAL입니다.
