# LIMITLESS / 2026-11-16 / Chapter3 설원족제비 첫 공식 픽셀아트 1포즈 파일럿 발주

**발주:** 바울 대표 “메인 쪽 다음 작업 가자 그럼.”
**담당:** Chapter3 이후 신규 아트 `docs/art-new`.
**기획 판정:** `SNOW_STOAT_M2_ONE_POSE_PILOT_REQUESTED / ART_NOT_YET_DELIVERED / USER_ART_REVIEW_REQUIRED`.

## 작업 선택 근거

- [최신 신규아트 인계](팀운영/인계/SPRITE_ART_LATEST.md)에서 FrostFieldMouse A 1포즈·Idle4/Attack4/Hit4/Defeat4 16프레임 아트 `ART_COMPLETE`, 2026-11-16 문서 `Chapter3_FrostFieldMouse_A_FinalArtCloseout_20261116.md` 참조. **승인 서리들쥐 모션 재작업 금지.**
- [북부 일반5·엘리트1·보스1 상세 기획](../08_몬스터/Chapter3_설빙의폭군_온기의화로_북부몬스터_상세기획_20261014.md)의 **M2 설원족제비 (Snow Stoat)**는 F1~F2 연결 일반몹: 몸이 긴 하얀 털, 검은 꼬리 끝, **전열/상태이상 없음**, 빠른 단일 근접 물기. 생존 때문에 남하한 야생 개체이며 사람친화 마스코트가 아님.
- M2는 **유일한 북부 펫 후보**지만 분양/한기1회 방어 효과는 수치 미확정·향후 기획 게이트. 신규 펫 Battle/Run/Skill 시트 제작은 지금 **NOT_APPROVED**.
- #74/#85 이미 승인된 서리올빼미 정지·Idle8/Attack4, F2 환경 및 #91 Ch3 전투배경2 제품은 중복 제작하지 않음. 설빙의 폭군은 후반 보스이므로 기존 승인 1포즈·Idle4 보호, 공격/피격/죽음 제작 후순위라는 기획 규칙 유지.
- GitHub 이슈 및 원격 문서 검색에서 M2 **독립 제작 완료 보고를 확인하지 못함**. 담당은 실제 예전 ZIP/로컬 파일/후임 인계를 다시 확인해 중복 제작 가능성을 배제할 것.

## 이번 신규 제작 1포즈만

1. **설원족제비 정지 Pixel Art KeyPose 1장** (512×512 RGBA 투명, 우향 3/4 옆모습을 1차 디자인 가이드로 사용; 실사용 PPU/Pivot/Scale/Animator는 기존 공식 몬스터 비교와 추후 Codex 계약 전 `SPEC_PENDING`). 기존 서리들쥐 1포즈와 Ch2 공식 몬스터 화풍(광원·화소 밀도·윤곽·접지) 일치.
2. 실루엣: 길고 유연한 흰 몸, 짧은 다리, 작은 머리·귀, 검은 꼬리 끝, 황갈/회청 음영 소량. 눈 배경에서 흰 털이 사라지지 않도록 **명암/외곽/바닥 접지 명확**, 기존 여우·늑대·들쥐/장난감·마스코트와 혼동 금지. 눈 위 출현과 짙은 숲 배경 양쪽 모두 40px에서 식별.
3. `512 / 128 / 40px` 실축소 시각 보드와 **승인 서리들쥐/서리올빼미/Ch2 공식 몬스터**와 동일 표시 배율 비교판. 실물 원본이 접근 불가하면 기존 승인 이미지/실제 측정 근거만 사용하고 추정 비교는 `NOT_VERIFIED`로 표기.
4. 1포즈 PNG + 실제 실물 preview + Design Notes/Source Declaration/Manifest/SHA256SUMS/ZIP CRC. 실물 파일 경로/정확 SHA 및 GitHub 문서 보고. HTML/PDF만 만들고 PNG를 납품했다고 주장하지 말 것.
5. 별도 애니메이션 전체(Idle/Attack/Hit/Defeat/Run), 필드타일·배경, BattleUI, 신규 펫 시트 제작 **금지**. 사용자 **최종 시각 승인** 전 Unity/게임에 적용하지 말 것.

## 미구현 수치/행동 보호

- 빠른 단일 물기와 `자세 낮추기→달려들기` 예고는 **기획 후보**. 정확한 레벨/HP/Attack/Agility/EXP/골드/전리품 `설원 솜털` 아이템ID/드롭률·MonsterID·Spawn 좌표와 펫 효과는 전부 `TBD`. 스킬·상점·한기·정화·펫 슬롯·가격을 임의 확정하지 않음.
- 유일 펫 후보라는 디자인 역할을 위해 생물/야생성이 식별되는 원화만 제작; 펫 시스템 구현은 별도 지시.

## 안전/보고

- 메인 아트 `docs/art-new` 작업보고서와 `SPRITE_ART_LATEST.md` 인계, 담당 이슈 결과 댓글. 사용자 승인 필요 표시.
- Unity **6000.6.5f1** 고정 / Issue #85 Import·Compile 및 Clean Release Push HOLD 유지.
- 기존 승인 Game/Scenes/Sprites/GUID/meta/Packages/Save/UserData/Discord 모두 변경 금지. 원격 main 직접 commit/Push/Merge 금지.
- 대형 신규 QA worktree 생성·기존 합격 Unity QA 전수재실행/디스크 중복 복제 금지. 아트 ZIP 소량 납품, 장기 보관·캐시 관리 규칙 준수.
