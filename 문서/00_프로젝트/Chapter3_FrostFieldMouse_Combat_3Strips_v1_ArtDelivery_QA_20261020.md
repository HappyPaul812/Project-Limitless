# LIMITLESS Chapter3 서리들쥐 A — Attack/Hit/Defeat 3 Strip v1 실제 납품·QA (2026-10-20)

**담당:** Chapter3 이후 신규 아트 총괄 `docs/art-new` / [Issue #30](https://github.com/HappyPaul812/Project-Limitless/issues/30)

**상태:** `THREE_MOTION_STRIPS_DELIVERED / OFFLINE_FORMAT_QA_PASS / USER_COMBAT_ART_REVIEW_PENDING / UNITY_NOT_INTEGRATED`.

## 최종 발주와 기술 계약

- 기획 `docs/planning/문서/00_프로젝트/Chapter3_FrostFieldMouse_AttackHitDefeat_3Strip_ProductionOrder_20261020.md` 및 Codex 확정 계약 `ad9e4b46`.
- **세 시트 모두 2048×512 RGBA**, 수평 4셀(각 512×512), **8FPS**(각 125ms), **PPU 512**, Pivot **(0.5,0)** 하단 중앙, 우향 1방향, **VisualScale 1**.
- Attack 프레임 순서 `00 준비 → 01 할퀴기 → 02 반동 → 03 복귀` 후 **기존 Idle로 복귀**.
- Hit 프레임 순서 `00 피격 → 01 움찔 → 02 자세 회복 → 03 복귀` 후 **기존 Idle로 복귀**.
- Defeat 프레임 순서 `00 균형 상실 → 01 쓰러짐 → 02 바닥에 누움 → 03 사망 자세`, **03 프레임 유지**. Defeat GIF/APNG은 동작 검토용 반복 미리보기일 뿐 게임에서 Defeat를 Loop하는 규칙이 아님.
- 승인 1포즈 SHA `0516914ddad36af5854065d66d01b9664329aa89eea8b394480eed6af92e32c2`, 기존 Idle SHA `5afebfa1f95ca5673162513fdea63b025546aa836550c0ceb17f0334659988f2`. 모두 읽기 참조만 했으며 **원본 변경 없음**.

## 실제 작업 ZIP

- 영문 ZIP: `Limitless_Chapter3_FrostFieldMouse_Combat_3Strips_v1.zip`
- **ZIP SHA256 `24ef8dddf76fb5843e8e7d3fac05942f1450c9f39cf366b2422a0ab82cba6741`**
- **8,688,417 bytes**, 해당 ChatGPT 대화의 클릭 가능한 다운로드 첨부로 납품. GitHub에 바이너리를 Push한 것으로 해석하지 않는다.
- PNG SHA256:
  - `Sprites/FrostFieldMouse_A_Attack_4Frames.png`: `08ed9ffb3d2807b0e1a3b27450f8f3bf1ff3e56f5f33e8b6607b2983db4e402e`
  - `Sprites/FrostFieldMouse_A_Hit_4Frames.png`: `37c51117ae18f2fed2bda5a8cef6af3099812b5e0979e1368634172937568f1b`
  - `Sprites/FrostFieldMouse_A_Defeat_4Frames.png`: `bda2809d177d03088e85389ecea23815b92962d61c36cc68967cd0015c96dfcc`
- 패키지에 모션별 4프레임 접촉 미리보기, 각 8FPS 정확 재생 APNG 및 GIF, 128px 크기의 전 12프레임 비교판, 12개 개별 QA용 프레임 PNG, `Manifest.md`, `QA/Offline_QA.json`, `SHA256SUMS.txt` 포함.

## 제작방법·기술 검증

- 신규 그림 소스 3개는 각 **2172×724 RGBA**의 포즈 4개씩을 포함한 비정규 프레임 보드였음. 따라서 그대로 Unity 납품하지 않고 연결 컴포넌트 분석으로 네 독립 포즈를 식별·분리하여 잘림 없이 **동일한 스케일**로 512px 셀에 재배치함.
- 3×4 모든 셀은 정상 불투명 캐릭터 영역 존재, 외곽의 투명 여백 확보, 알파 bbox x>0 / x<512 / y>0 / y<512; 발 접지선 y=490으로 일치.
- 모션마다 네 프레임의 RGBA SHA가 모두 다르고, 동작의 형태 변화가 실제로 존재함. Attack/Hit/Defeat 모두 완전히 새 그림 포즈를 포함하며 전체 이미지를 복사/이동한 4프레임이 아님.
- 3개 메인 PNG **2048×512 RGBA**, 셀 4×512, 파일 디코딩 PASS. 128px 보드 표시 PASS. 8FPS APNG은 125ms/프레임; GIF는 타이밍 제한상 120/130ms 교대, 합계 500ms. ZIP `testzip()` CRC PASS, 파일별 SHA256SUMS 제공.
- **미술 중요 경고:** 새 원화는 승인 정적/Idle의 냉회백색보다 갈색 계열 털이 강조됨. 같은 공식 서리들쥐로서 색상·전체 체형/눈빛·장비 없는 야생성·128px 타격/피격/사망 가독성은 **사용자 검토 필수**. 수치적 규격 PASS만으로 공식 미술 PASS 아님. 새 원화 3개에서 512 셀로 일률 축소/재구성했으므로 미세 디테일의 픽셀아트 수동 정리도 별도 검토 필요.
- 실제 Unity PPU/Pivot Import(수치는 Manifest 참조용), 전투 표시, Attack/Hit→Idle 복귀, Defeat 03 유지, 기존 스크립트 호환/Compile/Runtime은 **검증하지 않음**. 원본 Scene/코드/Animator/Save/UserData, F1·서릿길 마을 환경, 기존 1포즈·Idle, 설빙의 폭군 등 기존 자산 변경 없음.

## 승인 및 다음 단계

1. 바울 대표는 동작과 승인 Idle 대비 외형·색상 일관성 판단. **USER_COMBAT_ANIMATION_APPROVAL_PENDING**.
2. 미술 지적 발생 시 해당 세 PNG만 별도 수정·재검수. 현재 완성본을 승인 전 공식 Game-ready로 자동 승격하지 않음.
3. 승인 후 Codex는 격리 Unity에서 3개 Strip의 PPU512·Pivot(0.5,0), Attack/Hit 복귀·Defeat 최종 고정·UI 128px 가독성·미술 연속성/실제 Import 검증. 정식 Unity 적용/PR/메인 병합은 별도 승인 게이트.
4. 이 문서는 `docs/art-new`에만 기록, `main` 직접 커밋·Push·병합 없음.
