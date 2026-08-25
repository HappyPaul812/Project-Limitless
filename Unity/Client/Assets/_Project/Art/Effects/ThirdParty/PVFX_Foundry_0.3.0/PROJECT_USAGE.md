# PVFX Foundry 적용 기록

- 원본 팩: `pvfx-foundry-thirteen-spritesheets.zip`
- 팩 버전: `PVFX Foundry 0.3.0`
- 원본 SHA-256: `CF52AFAEAFF593CD800C29A36179175ED50F8CA1ECDDE12069AED06B2FDD5BDA`
- 라이선스: 포함된 `LICENSE.txt` 범위에 따른 CC0 1.0 Universal
- 권장 출처 표기: `VFX from PVFX Foundry by Pixel VFX Studio`
- 치유사 기본 공격 선택: `magical-projectile`의 `phase.travel` 프레임 2~6
- 프레임 규격: 96×96, 20 FPS(프레임당 50ms), 투명 PNG
- `radiant-heal`: grid 원본의 96×96 14프레임을 `BattleSkillEffects/RadiantHeal`로 추출해 치유사 `치유의 빛`에 사용
- 프레임 간격은 manifest의 50ms, 실제 회복 적용은 가장 밝은 `marker.peak` 7프레임에 연결
- 원본 ZIP과 sprite-sheet는 수정하지 않았으며 grid sheet SHA-256 `617348209199d3c1c037925fb23dd906b3ff4f643aefad65cdda67a1f69a97bf` 일치를 확인

ThirdParty의 ZIP, 문서, 선택 원본 SpriteSheet와 manifest는 수정하지 않습니다. 전투용 개별 PNG만 Resources 아래에 원본 96×96 크기로 추출했습니다.
