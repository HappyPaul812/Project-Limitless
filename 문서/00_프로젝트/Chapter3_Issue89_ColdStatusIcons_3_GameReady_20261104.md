# LIMITLESS / 신규 아트 / Issue #89 냉기 정식 상태 HUD 아이콘 3종 (2026-11-04)

**담당 브랜치**: `docs/art-new`  
**상태**: `OFFICIAL_ICON_ART_CANDIDATE / OFFLINE_FILE_QA_PASS / USER_ICON_ART_REVIEW_PENDING / UNITY_NOT_TESTED / NOT_ORIGINAL_APPLIED`.

## 1. 최신 지시·승인 경계

- [#89 총괄 기획 냉기 정식 아이콘3 소량 제작 지시](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6624809345) 및 [Codex B안 A/B 격리 50PASS](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6624142188), [Codex B안 시각 검토](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6624594309) 확인.
- 기승인 냉기 VFX3 PNG와 B 레이아웃(40px 가시알파 경계 8px 분리, 128px VFX 뒤·아래24px, 960 HUD 2행, 1680 HUD 경계8px) **전혀 변경하지 않음**. 기존 승인 VFX를 상태 배지 정식 아이콘으로 승격하지 않음.
- 14UI ≈ 실제 화면 약 12.12px (이전 Codex 실제 시험기준)인 작은 배지를 최우선 기준으로 **작은 실루엣을 처음부터 독립 제작**. 엔진 PPU/Pivot/Atlas는 최신 정본 실제 규격의 별도 읽기 전용 확인 전 `SPEC_PENDING`.

## 2. 실제 납품 PNG 3개

- **ZIP**: `LIMITLESS_Cold_Status_Icons_3_GameReady_20261104.zip` (**165,753 bytes**, **25 entries**).
- **ZIP SHA256**: `f758b59b73847f18c13e0d48aafee2f58eac33802ec597a1c739078335b2baa3`. `zipfile.testzip PASS`, 내부 `SHA256SUMS.txt` **24/24 PASS**.
- **실물 파일 라이브러리**: `/LIMITLESS/Chapter3_Issue89_ColdStatusIcons_20261104/LIMITLESS_Cold_Status_Icons_3_GameReady_20261104.zip` 업로드 성공. GitHub에 ZIP/PNG 바이너리 업로드0; 사용자 PC `F:\Downloads` 자동저장·로컬 해시 미확인.
- 본 납품의 게임용 신규 아이콘은 **128×128 투명 RGBA PNG 3장**. 원본 64×64 2배 NEAREST 기반으로 제작되어 과도한 텍스처 노이즈보다 픽셀 실루엣을 우선. 추가 **64×64 투명 RGBA 보조 3장**, 실제 12·14·20·40px 투명 PNG **12장**은 HUD 테스트용 파생물이며 원본 게임용 3종과 구분함.

| 상태 | 신규 `Icons/` 128px PNG | SHA256 |
|---|---|---|
| 한기 | `ColdStatus_Chill_128_RGBA_CANDIDATE.png` | `1f3ac724590a2d490cbe4a7cd28856c8ce5d888b130dde46a7d0008e6a3d0a16` |
| 동상 | `ColdStatus_Frostbite_128_RGBA_CANDIDATE.png` | `b814b1df3b0a5dbba46205d3b6edf91339cc4f45d118ec19ac53afab43c89a03` |
| 동결괴사 | `ColdStatus_FrostNecrosis_128_RGBA_CANDIDATE.png` | `da2f175d50c47961df4c2fe6a227219a594ac9c3b0874c2c50e9edf39644be9d` |

### 형태 기준

- **한기**: 6갈래 눈송이 중심 + 비대칭 냉기 흐름 2개, 넓은 둥근 형태.
- **동상**: 크기가 다른 3개 **붙어 있는 얼음결정 기둥**과 하부 결빙띠. 눈송이 중심 구조를 사용하지 않음.
- **동결괴사**: 좌우 비대칭 검푸른 **갈라진 얼음덩이**와 중앙 큰 균열, 하부 톱니 모양. 색상을 흑백으로 바꿔도 다른 윤곽을 목표.
- **문자·상태명·턴·중첩 수/KO 경고**는 아이콘 원본 PNG에 구워 넣지 않았고 기존 HUD 텍스트 독립 표시 계약 유지.

## 3. 실제 크기 비교/파일 검증

- `Preview/ColdStatus_12_14_20_40_HUD_Sizes_OFFLINE.png`: **12/14/20/40px 모두 실제 투명 PNG를 원본 크기로 배치**한 후 관찰 확대는 NEAREST로 명시. 단순 원화 축소 추정이 아니라 출력된 개별 이미지 실물 사용.
- `Preview/ColdStatus_Snow_DarkHUD_SameSize_OFFLINE.png`: 설원 밝은 배경/어두운 HUD 모의 패널 위 실제 14px 상태 아이콘 + 별도 문자, 오른쪽 실제 40px 아이콘. 이 비교판은 **OFFLINE 생성**, Unity A/B 캡처·실제 코드 적용 자료 **아님**.
- `Preview/ColdStatus_14px_GrayscaleShape_OFFLINE.png`: 흑백 상태 14px 형태만 따로 비교.
- 3×128 PNG/3×64 PNG/12 크기 PNG 모두 RGBA·실제 픽셀·투명 배경, ZIP CRC PASS, 내부 SHA24/24 PASS, UTF-8 `Manifest.json`, `DesignNotes.md`, `README_KR.md` 포함.
- 12px 실제 화면의 즉각 3상 구분성과 다른 체형/도보/휠체어/960·1280·1680 정식 HUD 접근성은 **Unity 실증 미실행**, 따라서 `UNITY_TECH_PASS`나 `USER_ART_APPROVED`로 표시하지 않음. B 레이아웃·기승인 VFX 그대로 보존.

## 4. 승인 다음 단계

바울 대표 아이콘3 미술 승인 → Codex가 별도 격리 Unity 6000.6.5f1에서 **기승인 B HUD의 임시 아이콘 참조만 3 후보로 바꾼 비파괴 Delta 비교**, 960×720 한기·동상·괴사1/3·2/3 실제 12.12px, 1280/1680 14UI/20px 및 색 없이 인지, 텍스트/중첩/턴, 클릭·휠체어 접근성을 검수 → 사용자 별도 원본 적용 승인 시까지 원본 Sprite/Scene/Prefab/BattleUI/Save/UserData/main 변경 금지.

**최종:** `OFFICIAL_ICON_ART_CANDIDATE / OFFLINE_FILE_QA_PASS / USER_ICON_ART_REVIEW_PENDING / UNITY_NOT_TESTED / NOT_ORIGINAL_APPLIED`. 전체 애니메이션·Particle·추가 VFX·기존 승인 Owl/Spider/BG/Save·원본 Unity·main 직접 commit/Push/merge 전부 0.
