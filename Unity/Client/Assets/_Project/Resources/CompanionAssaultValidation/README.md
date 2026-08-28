# 동료의 습격 Run 후보 검증 리소스

이 폴더는 Wolf/Fox/Bear 중 최종 동물을 고르기 위한 임시 검증 전용입니다.
실제 전투 스킬과 연결되어 있지 않으며 최종 결정 뒤 폴더 단위로 제거할 수 있습니다.

- 원본: `Assets/ThirdParty/ScratchIO/AnimatedWildAnimals/All.zip`
- 각 Run 시트: 가로 64px 프레임, 투명 배경, 기본 방향 왼쪽
- Import: Sprite, Point Filter, Mipmap 끔, 압축 없음, Alpha Transparency
- 실행 중 64px 단위로 프레임을 나누며 Pivot은 모든 프레임 `(0.5, 0)`(아래 중앙)로 통일
- 검증 기본값: 12 FPS, 화면 표시 배율 2배, 오른쪽→왼쪽 Transform 이동

비교 Scene: `Assets/_Project/Scenes/Validation/CompanionAssaultRunValidation.unity`
