# Hearing 최종 복원10종 적용 계획

입력 `F:/Downloads/Limitless_Hearing_Path_10_Restored_Final.zip`. LOCAL 정본·문서화→구현→검증. 현재READY30/BLOCKED20, 기대40/10은 실제검수 전 미확정. Hearing Fighter/Guardian/Healer/Mage/Sharpshooter(Marksman)×Male/Female만10종 대상. Mobility 포함 다른40종 PNG/meta/Entry/QA/기존240Clip·READY30·사용자94항목(410Git 파일) 보호.

PNG10/Male5/Female5/Job5×Gender2·중복0/누락0·512×512 RGBA/4×4/16Frame/Cell128 확인. 원본바이트만교체하며 이미지수정/리사이즈/crop/픽셀이동/삭제/생성 없음. 기존10 AppearanceID/160SpriteID/meta/Import/PPU/Pivot/Filter/Compression·CatalogEntry/성별/Path/Job/SaveMapping 유지. 전체Catalog 재생성 금지, 통과한 대상Entry에만8Clip 연결.

새160Frame 전체 머리/귀장치/발/무기/망토 crop·조각/인접셀침범·CharacterSwap/방향/Empty/Alpha/Scale 검사. 파일명이복원본이라는 이유로READY 처리하지 않으며 BLOCKED시0-basedFrame/좌표/파츠/정확사유 기록. 이전사유는비교자료일뿐 새판정에복사하지 않는다.

10Mapping/JobPreview 및 대표Male/Female CharacterCreationPreview/World/BattleLeftIdle/Save→Continue·구버전Save/기본fallback를 격리PlayUnfocused로검증. cleanBootstrap/설정복원·컴파일/Console조회·백그라운드·포커스전환 없음. 실제걷기/물리입력 미검증. 지정변경만stage/commit, ZIP/unrelated/GitHubPush 금지.

이전QA:
- Hearing_Fighter_Female.png: Right 발 아래 인접 Up 머리 조각, Up 머리 상단 잘림, Left 일부 머리카락 경계 절단.
- Hearing_Fighter_Male.png: Left 망토 절단·인접 조각, Right 발 아래 머리 조각과 Up 머리 상단 잘림.
- Hearing_Guardian_Female.png: Right 발 아래 다음 행 머리 조각과 Up 머리 상단 잘림, 일부 무기·머리카락 경계 절단.
- Hearing_Guardian_Male.png: Right 발 아래 다음 행 머리 조각과 Up 머리 상단 잘림.
- Hearing_Healer_Female.png: 인접 셀 머리카락 조각, Right 발 아래 잔상과 지팡이 경계 절단, Up 머리 상단 잘림.
- Hearing_Healer_Male.png: 분리된 지팡이·망토 조각, Right 발 아래 머리 조각, Up 머리 상단 잘림.
- Hearing_Mage_Female.png: 분리된 빨간 지팡이 장식·인접 조각, Right 발 아래 잔상과 Up 머리 상단 잘림.
- Hearing_Mage_Male.png: Right 인접 지팡이 장식·머리 조각, Up 머리 상단 잘림.
- Hearing_Marksman_Female.png: 인접 머리카락·무기 조각, Right 발 아래 다음 행 머리 조각, Up 머리 상단 잘림.
- Hearing_Marksman_Male.png: Left 망토 절단·인접 조각, Right 발 아래 머리 조각과 Up 머리 상단 잘림.
