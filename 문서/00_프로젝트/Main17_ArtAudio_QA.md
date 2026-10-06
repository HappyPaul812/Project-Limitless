# Main17 외부 Art / Audio 최종 QA — 2026-10-06

## 결과

**고유 PASS270 / FAIL0 / NOT_VERIFIED0 / 진행 Blocker0**. [실제 케이스](Main17_ArtAudio_Cases.csv). 새 자동/정적 검사와 캡처 검토만 집계했고 기존309/180PASS, 기존44승리·KO0 밸런스는 별도 재사용한다. 실물입력3·사람청취2는 이 자동 PASS에 포함하지 않는다. Runtime 원시 PASS248(통합)+37(일반조우/Fade/HUD), 나머지는 Resolver/Import/SourceHash/물리/격리정리/보호/캡처 검토이며 같은 ID는 중복 제거했다.

FUNCTION_COMPLETE / ART_COMPLETE / BGM_COMPLETE / BALANCE_REPRESENTATIVE_PASS / SAVE_COMPLETE / PATH_COMPLETE. TTS_PENDING14 유지. 출시 전 수동·Standalone 검증을 완료로 간주하지 않는다.

## 실제 재검증

- Main16 기존 협곡 조사·Main17 잠금/개방·Field07→08 Spawn/Bounds/동쪽Exit·서쪽차단.
- Main17 조사→세린→Story Battle→일반 공격 승리→후속 조사→12목표 완료→완료 Continue. 새 Blade Battle곡과 Field08 탐색곡 복귀. entry/witness/패배복귀/승리뒤/completed의 격리 Continue에서 Quest/위치/Path/영구세린/Party/Formation/Fox 복원·사이트 중복0.
- 저장 Party 유지, 세린 사수/Fox 기본, 스킬/아이템 열림 때 기본 버튼 표시·입력·포커스 차단 및 취소 복구.
- 실제 Field07/08 일반 조우의 BattleSceneFlow 진입→Blade→일반 복귀. 일반전은 곡 전환 확인용으로 ReturnToField(false) 호출했으며 이번 일반전 승리 결과를 새로 주장하지 않는다. 실제 Story 승리 및 기존44회 밸런스 승리를 재사용한다.
- FadeOut 중간에는 Blade·gain0~1, 필드 FadeIn 중간에는 탐색곡·gain0~1, 완료 후gain1. Source1/Service1/Mixer BGM/LoopON/재생 상태 PASS. Field 왕복·격리 Continue 중복0. Victory 결과 화면은 기존 전투곡, 필드 복귀 후 순차 Fade로 전환한다.
- Field04~08 일반 Resolver Blade, Chapter1 Steel 유지, 파수꾼 전용 AudioClip 참조 최우선, 미지정 Boss에 공통곡 강제0.
- 최종 Source PNG9/Manifest/ZIP/MP3 hash 보호. 원본3075파일 누락0, 기존 PNG/WAV 변경0. 기존Transform425/Collider15 예상 밖 변경0. 깊은 틈 CircleCast 차단·북남 우회 PASS.

Reward EXP60/Talent50의 코드·Asset은 변경하지 않았다. 이번 실제 완료/완료 Continue와 기존180PASS의 실제 마지막 대화·중복지급0·레벨업 경계 검증을 연결해 보호한다. Main01~16의 전체를 재실행하지 않고 원본 hash와 기존 PASS 및 이번 Main16/Navigation 대표 경로로 보호한다.

## Art와 해상도

정확한 English.zip: PNG10/Manifest1, 손상0/중복0. Runtime 등록9, 실제 적용7, 불투명 지면 변형2는 보관/UNUSED, Preview UNUSED. READY Manifest1 / ADJUST_IMPORT_ONLY7 / UNUSED3 / INCOMPATIBLE0. [원본 감사와 매핑](Main17_Field08_Art_Applied.md).

원본1254×1254, Sprite Single/PPU128/FullRect/Point/Uncompressed/Max2048/NPOTNone/중앙Pivot/Alpha. Shader Sprites/Default, Renderer별Sorting과 Collider 유지. Source repaint/crop/resize/색상변경0. Material 밝기와 Scene mesh vertex alpha만 사용해 불투명 틈 외곽을 배경에 섞는다. Placeholder 선/발자국/grass 균열 Renderer는 비활성화, 정식 텍스처로 대체한다. Art 생성기 보호 분기로 향후 PolishField가 완성 Art를 단색 도형으로 덮지 않는다.

3해상도 모두 실제 Runtime Camera+기존 Overlay HUD를 임시 ScreenSpaceCamera로 렌더링한 백그라운드 캡처다. 캡처 뒤 Canvas 설정을 복구하며 Game View/창 활성화/OS 입력은 사용하지 않았다. UI 검토는 AI의 이미지 검토이며 사용자 실물 조작 검증과 구별한다.

| 해상도 | 캡처 검토 |
| --- | --- |
| [1920×1080](QA_증거/Main17_Final_ArtAudio/Field08_1920.png) | Player/Serin/몬스터·목표·HUD 식별, 빈영역/잘못된 Filter/외곽사각 seam 없음 |
| [1600×900](QA_증거/Main17_Final_ArtAudio/Field08_1600.png) | 동일 PASS |
| [1280×720](QA_증거/Main17_Final_ArtAudio/Field08_1280.png) | 동일 PASS |

Exit/Interact/Quest 표시는 원래 전환·목표 Registry와 실제 진입/왕복·Prompt 검증을 유지한다. 원본 고해상도 그림을 옛128px 타일이라고 판정하지 않는다. Sorting 유지, Sprite 잘림/녹색 빈 영역 없음. 렌더 배치에 따른 장면 크기 맞춤은 원본 PNG 편집과 구별한다.

## 발견과 수정

첫 실제 Spawn 검사는 Sprite 교체 시 자동 크기 보정이 Rock/Canyon root Scale에 남아 실패했다. Scale1을 명시 복구하고 Collider 비교/통합 Runtime 재실행으로 해결했다. 남은 결함0. 중간 감사의 결과 파일 동시 읽기 때문에 Windows IO1224가 발생했으나 게임 결함은 아니며, 읽기 경쟁 없이 재실행 PASS. 보조 helper의 Resource 경로와 Warden filename oracle 오류도 현재 LOCAL 실제 값/AudioClip 참조로 수정했다. 최종 검증 파일에는 성공한 현재 결과를 기록한다.

## Compile / Console / Missing

Unity6000.5.7f1 컴파일 완료, Error0. 마지막 Refresh의 기존 CS0618 Warning16(ExternalAssetImportEditor/기존 회귀 helper 등), 이번 변경 클래스 Warning0. 새 코드와 관계 없는 기존 경고를 숨기거나 대량 수정하지 않았다. Runtime 검사 중 Error0/Warning0. Field07/Field08/Battle MissingScript0. Bootstrap EditMode·비포커스·AuditSaveDirectory=null·slot0·runInBackground=false 복구 PASS. 실제 사용자 Save/음량 설정은 격리했다.

## 남은 항목

| 분류 | 수 | 내용 |
| --- | ---: | --- |
| TTS_PENDING | 14 | 이번 WAV 생성/수정/임시연결0 |
| USER_INPUT_REQUIRED | 3 | 실물 Keyboard, Gamepad, Dungeon 실제 접촉 |
| USER_LISTENING_REQUIRED | 2 | Field08 탐색곡·새 Battle곡의 음악적 적합성과 루프 청취 |
| DEFERRED_FEATURE | 1 | 기존 일반 장비 저장 미구현, Main17 Art/Audio 범위 밖 |
| DEFERRED_RELEASE_VALIDATION | 1 | 기존 Standalone 빌드/실제 재시작, 이번 Editor 검증과 분리 |

Main18/Field09/Elite/Overheat 구현0. 몬스터 balance/reward 변경0. GitHub Push0.

## Git

Art **3b4820a**, Audio **53ef42c**. 최종 QA 문서·helper·검증 증거는 별도 Docs 커밋. 이번 파일만 Stage한다. 관련 Stage/commit diff --check PASS; 전체작업트리 기존 unrelated whitespace는 보존한다. 시작 상태103개 보존 여부와 Stage0은 최종 git status로 확인한다.
