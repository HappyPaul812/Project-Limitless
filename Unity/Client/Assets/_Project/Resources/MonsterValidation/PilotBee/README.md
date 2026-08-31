# Pilot Bee 비교 검증 리소스

이 폴더는 `PilotBeeComparisonValidation` Scene에서 Pilot Bee와 현재 초원 슬라임의 크기·픽셀 밀도·
색감·Idle/Attack을 나란히 확인하기 위한 프로젝트 전용 복사본이다. 실제 Field·Battle에서 사용하지 않는다.

- Pilot Bee 원본: `Assets/ThirdParty/2DPIXX/PilotBee/Original/`
- 초원 슬라임 비교 원본: `Assets/_Project/Art/Monsters/01_GrassSlime/01_GrassSlime_Walk.png`
- Import: Point Filter, Mipmap 끔, 압축 없음, Alpha Transparency, 런타임 분할을 위한 Read/Write 허용
- 기본 권장 Scale: 공통 PPU 기준 `0.85`
- 조작: `Space` 일시정지, `A` Idle/Attack 전환, `+/-` 벌 Scale 조절

비교용 복사본은 검증 결과가 확정되면 삭제할 수 있으며, ThirdParty 원본은 계속 보존한다.
