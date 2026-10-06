# Main17 Field08 실제 환경 Art 적용 기록

Source: `F:\Downloads\Limitless_RedRift_Environment_Assets_English.zip`
ZIP SHA256: `a238bc2eca0450b95fb5a61cd8cc655018bff82e54096fa91cfb2f7f424e19fb`

PNG10 / Manifest1 / README0. 실행용9PNG 등록, Preview는 Runtime에 넣지 않았다. 이미지10개 모두 decode 정상·파일명 중복0·손상0. RGB 불투명 Terrain4, RGBA Prop/Glow5, RGB Preview1. 투명 Props는 실제 alpha0 영역이 있고 일부 반투명 픽셀이 있다. 1254px 원본은 옛128px 타일과 다르므로 PNG를 Resize/Crop하지 않았다.

| 파일 | 실제 규격 | 판정 | 등록본 SHA256 |
| --- | --- | --- | --- |
| Manifest.txt | —×— / text | READY | 동일 |
| RedRift_AshPatch_01.png | 1254×1254 / RGBA | ADJUST_IMPORT_ONLY | 동일 |
| RedRift_AssetSheet_Preview.png | 1448×1086 / RGB | UNUSED | 동일 |
| RedRift_Canyon_Edge_01.png | 1254×1254 / RGBA | ADJUST_IMPORT_ONLY | 동일 |
| RedRift_DeadShrub_01.png | 1254×1254 / RGBA | ADJUST_IMPORT_ONLY | 동일 |
| RedRift_Fissure_Edge.png | 1254×1254 / RGB | ADJUST_IMPORT_ONLY | 동일 |
| RedRift_Fissure_Glow.png | 1254×1254 / RGBA | ADJUST_IMPORT_ONLY | 동일 |
| RedRift_Ground_Base.png | 1254×1254 / RGB | ADJUST_IMPORT_ONLY | 동일 |
| RedRift_Ground_Cracked_01.png | 1254×1254 / RGB | UNUSED | 동일 |
| RedRift_Ground_Cracked_02.png | 1254×1254 / RGB | UNUSED | 동일 |
| RedRift_Obsidian_01.png | 1254×1254 / RGBA | ADJUST_IMPORT_ONLY | 동일 |

ADJUST_IMPORT_ONLY7 / UNUSED3(Preview·균열 지면 변형2), Manifest READY1. 변형 지면은 불투명 사각 경계를 만들기 때문에 이번 단일 전체 바닥 대신 임의 Overlay하지 않았다. 원본은 등록·보관한다. INCOMPATIBLE0은 파일 손상/Import 불가능이 없다는 의미이며 시각 QA는 별도로 판정한다.

Import: Sprite Single / PPU128 / FullRect / Point / Uncompressed / Max2048 / NPOT None / 중앙Pivot / AlphaIsTransparency. Ground계열 Mirror, 나머지 Clamp. 원본 크기 유지. 미리보기는 QA 참고용이며 게임 Sprite로 사용하지 않는다.

Ground 실제 텍스처(-20), Fissure_Edge 깊은 틈(-10), Glow(-9), Ash(-7), Obsidian(-6 및 기존 Rock3), Canyon 기존 경계3, DeadShrub2. 과거 단색 Branch/Track와 grass를 늘린 AshCrack Renderer는 끈다. 콜라이더 없는 재의 표시 크기만0.5×0.5로 맞춘다. 죽은 식생3개는 비충돌 시각 장식이다. 구조 재생성 없이 Field08 시각 참조를 교체한다.

깊은 틈의 기존 BoxCollider(-5,0.5),0.7×7, 북/남 우회로를 유지한다. 전체 Fissure_Edge UV0~1을 유지한 Mesh 배치로 대각선 원본 균열을 기존 직선 충돌 슬롯에 맞춘다. PNG 수정이나 Sprite Rect crop은 없다. Renderer 밝기0.55와 원본 사각 외곽의 Scene vertex alpha 연결을 적용한다. Collider 내부 폭0.7은 불투명하고 바깥 절벽 그림만0.85unit까지 점차 바닥과 섞는다. Rock/Canyon Sprite 교체가 자동 Transform 크기 보정을 유발해 최초 Spawn QA가 실패했으며 Scale1 복구 후 재검증한다. Collider size/offset/trigger 원본값 유지.

검증은 MAIN17_FINAL_QA의 최신 외부 Art/Audio 구간을 따른다. 화면 포커스를 바꾸지 않고 실제 Runtime Camera를 RenderTexture로 캡처한다. Overlay HUD는 Camera-only 이미지에 포함되지 않으므로 별도 확인 없이는 그 캡처만으로 HUD PASS를 주장하지 않는다.

최종 확인: 기존Transform425/Collider15 비교의 예상 밖 변경0, 깊은 틈 CircleCast 차단/북남 우회 PASS. 최초 자동 크기 보정 결함은 수정 후 통합 Runtime PASS. 3해상도 실제 HUD 포함 캡처에서 Player/Serin/몬스터/목표/HUD 식별, 빈 영역/잘못된 Filtering/사각 틈 외곽 seam 문제 없음. 원본 비율을 전역 Scene footprint에 맞춘 렌더링이며 원본 pixel-art128px 타일로 판정하지 않는다.
