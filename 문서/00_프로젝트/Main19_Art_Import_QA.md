# Main19 Art Import QA

## 구현 전 계약 — 2026-10-08

SOURCE_RECEIVED / AUDITED. ZIP14파일·PNG13·Manifest1, 예상 차이0·크기/투명도 일치13. 원본 바이트 유지·단일 PNG13, 임의 Slice/재생성/색변경0. 기술 Gate는 PASS_WITH_NOTE13/FAIL_BLOCKING0이며 최종 미술은 USER_ART_REVIEW_REQUIRED13. Alpha 경계 접촉은 잘림 판정 근거로 단정하지 않는다.

Main18과 같은 Resources/Main19/{Environment,Background,VFX}·Point/Uncompressed/128PPU/alpha transparency/NPOT none 정책으로 새 GUID 등록. Field10 Environment만 바꾸고 Quest12/위치/BGM/Monster/Save/기존 Sprite는 보존한다. 지면 단발 완만한 맥동, Witness 시작 전 실루엣 숨김·대화 시작 시 표시+먼 flare1회·철수 단계/완료 후 유지. Continue는 현재 목표11 또는 Completed이면 표시, 목표10 이전/직전은 숨김; 새 Save 필드 없음. 기존 정식 CameraFollow/Bounds 그대로, 강제 Focus/Shake 없음.

[원본 감사](Main19_Art_Source_Audit.json) · [Manifest](QA_증거/Main19/Art_Manifest.txt).

## 원본 시각 감사와 반영

13개 모두 PASS_WITH_NOTE, FAIL_BLOCKING0. USER_ART_REVIEW_REQUIRED13은 기술 Gate와 별도인 미적 승인이다. 자동 기술 PASS를 최종 Art 승인으로 승격하지 않는다.

원본 Contact Sheet와 각 중요 PNG/Runtime 캡처를 확인했다. 원본에서 눈에 띄는 텍스트·워터마크는 관찰되지 않았지만 암벽/능선/분출구/심부절벽에는 밝은 가이드 선이 남아 있다. 흰 fringe와 배경 사각형/장식 테두리 구분을 미적 승인 없이 확정하지 않는다. Ground 반복 경계, 균열 가장자리 접촉·밝기, 암벽/분출구의 용암·폭발 인상, 압흔의 크기, 원거리 인간형의 정보 노출/거리감은 사용자 검토 대상이다. 검정 사각형 불투명 Background로 판정할 Alpha 오류는 없으며 Ground 외12개는 투명 픽셀이 있다. PNG 재생성/Crop/Resize/Repaint/Recolor/Upscale0. 증거용 Contact Sheet만 축소 미리보기이며 Unity 등록 원본13의 byte는 그대로다.

Main18 선례의 Single Sprite·Point·Uncompressed·128PPU·max2048·NPOT None·AlphaTransparency·Mipmap off·Clamp·FullRect. 모든 Sprite null0, meta13, 새GUID 충돌0. 폴더 `Assets/_Project/Resources/Main19/{Environment,Background,VFX}`. Pivot은 기존 환경 선례와 같은 중앙이며 월드 Scale만 역할에 맞췄다. 모든13개 Scene에서 사용. 기존 Main18 AshPatch1만 입구 보조 재사용, 원본/meta 변경0.

지면21×15 반복, Track1 3.2×2.2/Track2 4×2.8·승인 조사 좌표 유지. PulseCrack01/02는 중반 node/corridor, DragScar/HeatVent는 중반 양옆, Ridge/DeepCliff는 후반 경계. 장식 Collider0·모든 Renderer sorting<0으로 Actor/Quest Label보다 뒤. 기존 Boundary만 통행을 차단하므로 조사 접근 동선을 막지 않는다.

GroundPulse2.4초·peak alpha0.55, DistantFlare2.8초·peak0.42: 단일 PNG sine² envelope1회 후alpha0. 고속 Flash/흰 Flash/Strobe/Shake0, 새 Material0·Frame Instantiate0·Frame Resources.Load0. Renderer2개 고정 캐시로 VFX 누적0. 실루엣은(-8.1,3.4) 4.6×2.3 원거리 배경, Flare는 실루엣 뒤로 배치해 몸을 덮지 않고 주변 열기만 보완. 능선은2.15높이에1.65높이로 배치, 상단 화면 잘림을 캡처로 찾아 보완했다. 카메라 코드/Quest 좌표/Dialogue 변경0.

[Importer13 실조회](Main19_Importer_Settings.txt) · [원본 표](Main19_Art_Manifest_Audit.csv) · [Contact Sheet](QA_증거/Main19/Art_Source_Contact.png) · [입구1280](QA_증거/Main19/Entry_1280.png) · [맥동1280](QA_증거/Main19/GroundPulse_1280.png) · [Witness1280](QA_증거/Main19/Witness_1280.png). 1280/1600/1920 RenderTexture 캡처이며 OS 해상도나 포커스를 바꾸지 않았다.
