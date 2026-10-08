# Main19 Art 통합 최종 QA — 2026-10-08

**SOURCE_RECEIVED → AUDITED → IMPORTED → RUNTIME_VERIFIED. 기술 PASS, 최종 미술 USER_ART_REVIEW_REQUIRED13.** 원본을 수정하거나 미적 승인을 자동 확정하지 않았다.

## Git 시작과 납품 감사

시작 HEAD `4161107193fd254df8e0bf2da0c4c2cd2399f193`, 로컬 추적 origin/main과 동일(0ahead/0behind). 사용자 이전 원격 기준4e54c3e보다 최신 LOCAL을 사용했다. 네트워크 fetch/push0. 기존 tracked75/미추적26 보호.

ZIP 실제 경로 `F:\Downloads\Limitless_Main19_BurningPulse_Art_Pack.zip`, 2305141bytes. SHA256 `28981bd07a5fb5a639cdad3a44c5b2c7a493539b0afa905c476f562682bdc33d`. 내부 파일14=PNG13+Manifest1, Environment10/Background1/VFX2, 예상PNG13 대비 누락0/추가0. Manifest 파일명·크기·투명도13/13 일치·PNG decode13/13·중복SHA0.

기술 Gate: PASS_IMPORT0 / PASS_WITH_NOTE13 / FAIL_BLOCKING0. 별도 미적 USER_ART_REVIEW_REQUIRED13(중복 집계 축). 원본 가이드 선·반복 지면 경계·실루엣 밝기/거리감은 미적 검토 전이며 기술 성공이 이를 닫지 않는다. [원본 감사](Main19_Art_Source_Audit.json) · [Import QA](Main19_Art_Import_QA.md) · [Manifest 비교13](Main19_Art_Manifest_Audit.csv).

## 배치와 연출

|대상|실제 결과|
|---|---|
|Unity 등록|Assets/_Project/Resources/Main19/Environment·Background·VFX, PNG13 원본 byte 동일·meta13·새GUID 충돌0·Null Sprite0/Missing Asset0|
|Ground|21×15 반복·짙은 화산암, Source 유지. 반복 경계의 실제 가독성 사용자 검토|
|Pulse Crack01/02|중반 Node/통로·alpha0.4/0.35·용암 강으로 만들지 않음|
|Melted Cliff|승인 조사점 아래 장식. 원본 가이드/열광과 용암 폭포 인상 사용자 검토|
|GiantTrack01/02|기존 Target좌표·3.2×2.2→4×2.8 확대·명확한 발톱/발가락 디자인 추가0|
|DragScar/HeatVent|중반 방향 흔적·열기 장식. 특정 무기/분화로 확정0|
|Ridge/DeepCliff|후반 전망·자연 심부 차단, 기존 Bounds/서쪽 안내 유지·추가Collider0·Main20 Scene0|
|Distant Silhouette|원거리1024×512 단일PNG·(-8.1,3.4)·4.6×2.3, Witness 전 숨김·시작 시 표시·이후 유지. Boss Sprite/최종 디자인 승인0|
|GroundPulse|조사 시작 때 단일PNG sine² envelope2.4초1회·peak alpha0.55·종료0·반복 입력중복0|
|DistantFlare|Witness시2.8초1회·peak0.42·실루엣 뒤 주변열광·종료0|
|Witness|기존 대화·Hook/철수 판단 유지. 화면 상단 잘림을 캡처로 찾아 배치 보완, 최종 Framing 검사PASS|
|Sorting/성능|장식Sorting<0·Actor/Marker앞노출 유지·Scene Renderer14 고정/VFX2·Frame Instantiate/Resources.Load0·새Material0·강제 Camera/Shake/Flash0|

[입구1280](QA_증거/Main19/Entry_1280.png) · [맥동1280](QA_증거/Main19/GroundPulse_1280.png) · [Witness1280](QA_증거/Main19/Witness_1280.png). 1600/1920도 같은 세 시점 캡처했다. RenderTexture는 Canvas 모드를 잠시 바꾸고 원상 복구하며 Game View/OS/화면 포커스를 조작하지 않는다. Dialogue·Objective·Lv/Monster Label을 렌더 증거로 확인했으며 목표 안내와 Player 이름이 가까운 기존 배치 체감은 사용자 확인 대상이다.

## Gameplay·Save·Audio 회귀

**314 PASS / FAIL0**(반복 Scene label 포함 기록 수). [전용 실행 결과](Main19_Field10_Art_Runtime_Results.txt).

- Quest12 전체 순차 완료·기존 조사/Path5/Dialogue/EXP80·Currency70 유지. 순서 변경0·조기진행0·완료 중복보상0.
- A 갑충+도마뱀, B 갑충+망령 후열·원본HP/공격%/민첩·정상 Attack 승리. 각각 도망/전멸→재도전→승리단회, 실패 진행/보상0·중복 EndBattle0. 기존 과열/냉각/정화/Monster Art·수치 변경0.
- 실제 물리 Trigger Field09→10와10→09 왕복. Spawn·Quest·BGM·Field Art·Runtime Root1·Field09 연출 누출0, 서쪽 Boundary/안내 유지. 4:3/16:9/21:9 viewport 및 Spawn/Exit비중첩 PASS.
- 실제 Bootstrap 슬롯 버튼 Continue5: A전/A후/Witness전/Witness후/완료후. Quest/완료·좌표·Party/Formation/Unlock/Beast·HP/MP·Path·Inventory·Currency·Level/EXP 동일. 목격 전 숨김·목격 후/완료 표시·VFX재발생0/중복Root0. SaveVersion1·새 Save 필드0.
- Field Paths of Cracked Earth, Battle Blade and Gambit 실제clip참조 유지. 음악 파일/배정/볼륨/Source/Fade/Mixer 변경0. 새 BGM0·사람 청취는 이번 자동 PASS와 별도.
- TTS API0·Voice 변경0·Main19 NPC13 TTS_PENDING/Player·지문무음 유지. Main07 작별5/Main17 신규14 CLOSED와 Main18 TTS_PENDING22 보호.
- Compile Error0·Runtime Console Error0·신규 Warning0·Missing Script0. 기존 프로젝트 CS0618 컴파일경고16은 변경범위 밖. Unity6000.5.7f1·비포커스 PlayUnfocused·격리 Save/Settings. 종료 Bootstrap EditMode·Save/설정/Play복구, OS입력/Computer Use0.

보호 baseline3301중3299 byte 동일·예상 변경2(Field10Scene/Main19Flow)·예상밖 변경/삭제0. Scene Player prefab/Camera/WorldBounds 설정은 후행 공백을 제외한 YAML25블록 동일. 기존 Main18/Main17 Art·Monster/Player/Companion/Portrait·Voice/BGM·ThirdParty·사용자 수동 Asset·Save/설정/meta/GUID 보호. [보호 감사](Main19_Art_Protected_Files_Audit.json).

## 사용자 직접 Art 검토와 다음 작업

USER_ART_REVIEW_REQUIRED13. 특히 Ground의 Main18 대비 차이/반복 경계, GiantTrack 위압감, MeltedCliff 용암폭포 인상, PulseCrack 밝기, 실루엣의 정보 공개/식별성, 전체 붉은 비율, Witness 거리감7항목. 가이드 선/테두리 잔존도 함께 검토한다. 최종 시각 승인 전 Final Art APPROVED로 기록하지 않는다. 낮은 대비의 원거리 실루엣은 사람이 판단해야 한다. Optional/전직업·다른Party 조합·실물입력/청취 전체 QA는 이번 기술 범위 밖의 기존 후속 항목이다.

다음 권장: 위 렌더/원본 검토 의견으로 원본 Art 수정이 필요한지 판단→승인된 수정본만 별도 적용. 이어서 Main19 NPC13 TTS 제작·사용자 전체 청취 승인 후 별도 Unity 연결. Main20 신규설계/Boss구현은 이번 범위가 아니다.

## Commit·변경 파일

- `3d477d6` Feature: Main19 Burning Pulse Art Pack 원본13 정식 등록
- `294b81e` Feature: Main19 Field10 환경 및 Witness 단발 맥동 연출
- 이 문서를 포함한 최신 Docs commit: Art 통합 QA/현재 상태.

최종 예상 로컬3ahead/0behind·기존 tracked75/미추적26 유지·staged0. 최종 보고 시 Git 조회로 확정한다. unrelated Stage0·GitHub Push0.

- `Unity/Client/Assets/_Project/Resources/Main19.meta`
- `Unity/Client/Assets/_Project/Resources/Main19/Background.meta`
- `Unity/Client/Assets/_Project/Resources/Main19/Background/BurningPulse_DistantFlameSilhouette.png`
- `Unity/Client/Assets/_Project/Resources/Main19/Background/BurningPulse_DistantFlameSilhouette.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main19/Environment.meta`
- `Unity/Client/Assets/_Project/Resources/Main19/Environment/BurningPulse_DeepCliff_Blocker_01.png`
- `Unity/Client/Assets/_Project/Resources/Main19/Environment/BurningPulse_DeepCliff_Blocker_01.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main19/Environment/BurningPulse_DragScar_01.png`
- `Unity/Client/Assets/_Project/Resources/Main19/Environment/BurningPulse_DragScar_01.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main19/Environment/BurningPulse_GiantTrack_01.png`
- `Unity/Client/Assets/_Project/Resources/Main19/Environment/BurningPulse_GiantTrack_01.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main19/Environment/BurningPulse_GiantTrack_02.png`
- `Unity/Client/Assets/_Project/Resources/Main19/Environment/BurningPulse_GiantTrack_02.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main19/Environment/BurningPulse_Ground_Base.png`
- `Unity/Client/Assets/_Project/Resources/Main19/Environment/BurningPulse_Ground_Base.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main19/Environment/BurningPulse_HeatVent_01.png`
- `Unity/Client/Assets/_Project/Resources/Main19/Environment/BurningPulse_HeatVent_01.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main19/Environment/BurningPulse_MeltedCliff_01.png`
- `Unity/Client/Assets/_Project/Resources/Main19/Environment/BurningPulse_MeltedCliff_01.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main19/Environment/BurningPulse_PulseCrack_01.png`
- `Unity/Client/Assets/_Project/Resources/Main19/Environment/BurningPulse_PulseCrack_01.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main19/Environment/BurningPulse_PulseCrack_02.png`
- `Unity/Client/Assets/_Project/Resources/Main19/Environment/BurningPulse_PulseCrack_02.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main19/Environment/BurningPulse_Ridge_01.png`
- `Unity/Client/Assets/_Project/Resources/Main19/Environment/BurningPulse_Ridge_01.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main19/VFX.meta`
- `Unity/Client/Assets/_Project/Resources/Main19/VFX/BurningPulse_DistantFlare_VFX.png`
- `Unity/Client/Assets/_Project/Resources/Main19/VFX/BurningPulse_DistantFlare_VFX.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main19/VFX/BurningPulse_GroundPulse_VFX.png`
- `Unity/Client/Assets/_Project/Resources/Main19/VFX/BurningPulse_GroundPulse_VFX.png.meta`
- `Unity/Client/Assets/_Project/Scenes/Field_10_BurningPulse.unity`
- `Unity/Client/Assets/_Project/Scripts/Editor/Main19ArtContentBuilder.cs`
- `Unity/Client/Assets/_Project/Scripts/Editor/Main19ArtContentBuilder.cs.meta`
- `Unity/Client/Assets/_Project/Scripts/Editor/Main19ArtRuntimeAudit.cs`
- `Unity/Client/Assets/_Project/Scripts/Editor/Main19ArtRuntimeAudit.cs.meta`
- `Unity/Client/Assets/_Project/Scripts/World/Chapter2Main19Flow.cs`
- `Unity/Client/Assets/_Project/Scripts/World/Main19FieldArtPresentation.cs`
- `Unity/Client/Assets/_Project/Scripts/World/Main19FieldArtPresentation.cs.meta`
- `문서/00_프로젝트/CURRENT_STATUS.md`
- `문서/00_프로젝트/Main19_Art_Handoff.md`
- `문서/00_프로젝트/Main19_Art_Import_QA.md`
- `문서/00_프로젝트/Main19_Art_Manifest_Audit.csv`
- `문서/00_프로젝트/Main19_Art_Protected_Files_Audit.json`
- `문서/00_프로젝트/Main19_Art_Source_Audit.json`
- `문서/00_프로젝트/Main19_Design_QA.md`
- `문서/00_프로젝트/Main19_Field10_Art_Runtime_Results.txt`
- `문서/00_프로젝트/Main19_Final_QA.md`
- `문서/00_프로젝트/Main19_Importer_Settings.txt`
- `문서/00_프로젝트/QA_증거/Main19/Art_Manifest.txt`
- `문서/00_프로젝트/QA_증거/Main19/Art_Source_Contact.png`
- `문서/00_프로젝트/QA_증거/Main19/Entry_1280.png`
- `문서/00_프로젝트/QA_증거/Main19/Entry_1600.png`
- `문서/00_프로젝트/QA_증거/Main19/Entry_1920.png`
- `문서/00_프로젝트/QA_증거/Main19/GroundPulse_1280.png`
- `문서/00_프로젝트/QA_증거/Main19/GroundPulse_1600.png`
- `문서/00_프로젝트/QA_증거/Main19/GroundPulse_1920.png`
- `문서/00_프로젝트/QA_증거/Main19/Witness_1280.png`
- `문서/00_프로젝트/QA_증거/Main19/Witness_1600.png`
- `문서/00_프로젝트/QA_증거/Main19/Witness_1920.png`
- `문서/03_스토리/Chapter2_Main19_타오르는_맥동.md`
