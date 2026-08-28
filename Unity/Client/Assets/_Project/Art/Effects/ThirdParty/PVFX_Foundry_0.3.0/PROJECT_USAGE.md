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
- 마도사 `파이어 볼`: `solar-shrapnel` grid 원본에서 초기 Charge 2프레임을 사용하고 `warm-explosion` grid 15프레임을 별도 명중 폭발로 사용
- `warm-explosion`의 `marker.peak` index 4에서 170% 즉발 피해와 화상을 적용하며, 버튼·화상 상태 아이콘은 같은 index 4 고정 Sprite 사용
- 마도사 `썬더볼트`: `electric-impact` grid 96×96 14프레임을 20 FPS로 적 전체 위치에서 거의 동시에 재생
- `electric-impact`의 `marker.peak` index 1에서 전체 90% 피해와 감전 1을 적용하며, 버튼은 같은 index 1 고정 Sprite 사용
- 프로젝트 Resources에 보존한 원본 Grid 사본 SHA-256: `solar_shrapnel_charge_sheet.png`=`44CF68066822C60078D480B04AB8FA8E6662779CBACE4A9A4BA19E8C57A48425`, `warm_explosion_sheet.png`=`DEE1C564A92054F8765353900A16BA3FE92EA55EB0C8C411331E0152D097E6DC`
- 원본 ZIP과 sprite-sheet는 수정하지 않았으며 grid sheet SHA-256 `617348209199d3c1c037925fb23dd906b3ff4f643aefad65cdda67a1f69a97bf` 일치를 확인

ThirdParty의 ZIP, 문서, 선택 원본 SpriteSheet와 manifest는 수정하지 않습니다. 전투 런타임에 필요한 개별 프레임 또는 Grid 사본만 Resources 아래에 보관합니다.
