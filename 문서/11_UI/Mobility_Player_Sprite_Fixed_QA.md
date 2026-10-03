# Mobility Player Sprite 수정판 QA

2026-10-03. [적용 계약](Mobility_Player_Sprite_Fixed_적용_계약.md)대로 기존10개 PNG를 ZIP 원본 바이트 그대로 교체했다. 시트10개·Male5/Female5·Job5×2, Duplicate0/Missing0. 참고 Preview.png는 제외했다. 전체512×512 RGBA/Alpha0–255,4×4/128px/16Frame. 원본 ZIP과 사용자 그림은 변환·복원·재생성하지 않았다.

## 10종 미술 판정

160개 Cell을 배경 합성 확대 이미지로 직접 관찰했다. 모두 nonempty이며 시트별16프레임 픽셀 Hash가 서로 다르다. Down0–3/Left4–7/Right8–11/Up12–15의 본체 방향 구분과 휠체어·파랑/금색 장비를 확인했다. 관찰 범위에서 캐릭터 교체·심각한 본체 내부 Alpha 구멍·본체 Scale 급변은 보이지 않았다. 그러나 분리 조각과 머리 절단 때문에10종 모두 **BLOCKED_ART**, Mobility Ready0/Blocked10이다. Idle은 각 행 첫 Frame이고 Walk는 행4장이다. 실제 걷기 품질·물리 입력·Foreground는 미검증이다.

|파일명|Job Stable ID|Gender|0-based 문제 Frame|판정·구체적 사유|
|---|---|---|---|---|
|Physical_Fighter_Female.png|fighter|Female|4–15|BLOCKED_ART: Left/Right 발 아래 머리 조각, Right/Up 머리 상단 절단, 검 끝 조각 분리|
|Physical_Fighter_Male.png|fighter|Male|4–15|BLOCKED_ART: Left/Right 발 아래 머리·무기 조각, 셀 옆 검/망토 조각, Up 머리 상단 절단|
|Physical_Guardian_Female.png|guardian|Female|4–15|BLOCKED_ART: Left 머리 위 바퀴 조각, Right 발 아래 머리 조각, Up 머리 상단 절단|
|Physical_Guardian_Male.png|guardian|Male|8–15|BLOCKED_ART: Right 발 아래 머리 조각, Up 머리 상단 절단|
|Physical_Healer_Female.png|healer|Female|4–15|BLOCKED_ART: Left/Right 발 아래 금발 머리 조각, Up 머리 상단 절단|
|Physical_Healer_Male.png|healer|Male|4–15|BLOCKED_ART: Left 머리 위 바퀴 조각, Right 발 아래 머리·장비 조각, Up 머리 상단 절단|
|Physical_Mage_Female.png|mage|Female|4–15|BLOCKED_ART: Left/Right 발 아래 머리 조각, Up 머리 상단 절단|
|Physical_Mage_Male.png|mage|Male|4–15|BLOCKED_ART: Left 머리 위 바퀴 조각, Right 발 아래 머리 조각, Up 머리 상단 절단|
|Physical_Marksman_Female.png|sharpshooter|Female|4–15|BLOCKED_ART: Left/Right 발 아래 금발 머리 조각, Up 머리 상단 절단|
|Physical_Marksman_Male.png|sharpshooter|Male|0–15|BLOCKED_ART: Down 발 아래 잔여 조각, Left/Right 발 아래 머리 조각, Up 머리 상단 절단|

CSV의 cell_boundary_hits0은 실제 검사와 일치하지만, 경계에서 이미 잘린 그림을 셀 안쪽으로 옮긴 듯한 절단 형태가 남아 있다. 처리 과정 자체는 추정이며, 잘림/분리 조각은 실제 관찰이다. 수정 권장: 완전한 머리·무기·휠체어를 각128px Cell 안에 여백을 두고 배치하고, 인접 행/열의 잔여 조각을 제거한다. 잘린 원본을 단순히 Cell 안으로 옮기는 방식으로는 복원되지 않는다.

## Identity·Import·보존

기존 ID10개는 `appearance.external.v1.mobility.`에 다음 suffix를 붙인다: `fighter.female`, `fighter.male`, `guardian.female`, `guardian.male`, `healer.female`, `healer.male`, `mage.female`, `mage.male`, `marksman.female`, `marksman.male`. 새 ID0. Catalog50개·Mapping50개·Sprite800개 유지. Mobility160개 Sprite GUID/fileID와 meta 전체 Hash 동일. Import는 Multiple/PPU128/Point/Uncompressed, Rect128×128·Pivot(64,0)이다.

시작 시 보호880파일 중 Mobility PNG10개·External50.asset·Validated50.json만 변경했다. 다른40종의 PNG/meta·Catalog Entry/Sprite/Clip/QA 상태와 Inventory 정보는 동일하다. Hearing 재판정 없음. BGM/Voice/Main16 변경 없음. [JSON](Mobility_Player_Sprite_Fixed_QA.json)에 각 PNG Hash/16Frame bbox/문제/판정을 기록한다. 확대160Frame 증거는 `Unity/Client/Temp/MobilityFixedAudit/Physical_*_Frames.png`에 있다.

## Runtime 및 전체 상태

격리 PlayUnfocused 감사 **86 PASS/0 FAIL**. 남녀10조합 실제 Path/Job UI 자동 Mapping/Preview/fallback 표시, 대표 Male Fighter·Female Sharpshooter의 CharacterCreation→Confirm→World→Save→Bootstrap Continue→Battle 실제 Scene 전달을 통과했다. 이전/불일치 Appearance ID도 정본 조합으로 복원된다. Runtime 결과는 [감사 출력](Mobility_Player_Sprite_Fixed_Runtime_Results.txt)을 따른다. 미술 Blocked는 기존 정책대로 기본 성별 Sprite를 사용하며 자동 계산 ID는 Mobility의 정확한 조합을 유지한다. 수정판 시트 참조 검사와 fallback Runtime 검증을 구분한다. 수정판 Art의 Ready 애니메이션 재생을 완료했다고 간주하지 않는다.

Unity Compile/Console Error0, 최종 Console Warning0. 컴파일 직후 기존 ExternalAssetImportEditor.cs의 deprecated spritesheet 경고2개는 확인했다. clean Bootstrap Edit Mode로 복귀했으며 격리 Save/Settings·Play 진입 옵션·백그라운드 실행 설정을 복원했다. 창 활성화/OS 입력/Foreground 검증 없음. 이번 변경 diff 검사 통과.

작업 전 Ready21/Blocked29 → 작업 후 **Ready21/Blocked29**. Hearing10·Mobility10·Vision5·Intellectual4가 계속 Blocked다. EmotionalScar10·Vision5·Intellectual6의 PASS는 유지했다. 저장 스키마/마이그레이션/수동 Appearance 선택 UI 변경 없음. GitHub Push 없음.
