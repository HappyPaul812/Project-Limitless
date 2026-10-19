# LIMITLESS / Field09 ZIP 헤더 손상 복구 + 맹독뱀·Wolf Chapter2 공식 아트 수준 품질 게이트 (2026-10-19)

- 담당: 기획 `docs/planning` / ZIP·Unity 기술 Codex / 오리지널 아트 리뉴얼 `docs/art-renewal`.
- 사용자 확인: Codex Field09 격리 검수가 ZIP 자체 포맷 오류로 중단된 결과를 전달하였고, 몬스터·펫 v2에 관해 **“너무 픽셀로 돼서 ... 챕터2의 우리가 만든 거 그 정도 퀄리티는 돼야 하지 않겠어?”**라는 미술 방향 수정 요구.
- **상태:** `FIELD09_ZIP_FORMAT_BLOCKER / FIX_PNG_BYTES_PRESERVED_REQUIRED / FIELD09_UNITY_QA_NOT_RUN / VENOM_SNAKE_V2_ART_HOLD / WOLF_V2_ART_HOLD / CHAPTER2_OFFICIAL_QUALITY_TARGET_SET / V3_FULL_ANIMATION_NOT_YET_APPROVED / NO_MAIN_PUSH`.
- 이 문서는 기획/승인 기준 기록이며 ZIP 바이너리 직접 수정, PNG 생성, Unity 원본 적용을 이 기획 세션이 직접 수행한 것은 아니다.

## 1. Field09 — 정상 ZIP 재포장만, 기존 PNG byte/Pixel/Manifest 내용 무단변경 금지

**Codex 최신 제보 (바울 대표 공유 화면):**
- `F:\Downloads\Limitless_Field09_ObsidianScar_Minimal_Fix_v1.zip` 및 GitHub `docs/art-renewal/Art_Deliveries/Limitless_Field09_ObsidianScar_Minimal_Fix_v1.zip` 파일 SHA 동일. SHA 자체가 일치해도 그게 **유효한 ZIP**이라는 뜻은 아님.
- 32비트 필드/헤더의 byte order 또는 순서가 올바르지 않아 Python `zipfile`, Windows Explorer 및 .NET 세 구현 모두 열기 실패. 조사 중 선별한 개별 바이너리 수기 해독에서 `Fixed_Candidates` PNG8의 기존 납품 Manifest SHA가 일치했다고 Codex 보고. **포맷 오류가 확정**되었으므로 기존 납품 문서의 `ZIP CRC PASS`를 현재 안전 적용 근거로 사용하지 않는다.
- 실제 새 후보 PNG8과 기존 공식8 PNG/meta/GUID, 총4,339 원본파일/UserData23 보호, 원본 Unity Editor 조작0, Push/merge0. Field09 Unity 전후화면/Runtime QA **미실행**. 작업 브랜치 `codex/field09-minimal-fix-unity-integration-20261019`, 오류·보호보고 커밋 `008d32f18e96c4f04b53edb6a0993b2b29958332`.

**다음 실행 계약 (Codex):**
1. 이전 오류와 손상 ZIP 자체의 SHA/크기 및 `Manifest.json`, `QA/Original_Source_SHA256_and_Delta.csv` 원본→수정 SHA를 보존. 임의로 "다시 받으면 해결"로 넘어가거나 손상 archive를 원본에 Import하지 않는다.
2. 검증 가능한 후보 PNG8를 확보한다. 이전 Codex의 수기 복구 PNG가 실제 Manifest SHA/PNG decode·pixel RGBA와 각 항목 일치한다면 **해당 PNG byte를 그대로 사용**. 일치하지 않거나 복구가 불완전하면 리뉴얼 담당에게 **같은 PNG 원본 8개**의 개별 byte를 다시 받아 확인한다. PNG 재생성/리컬러/보정 금지.
3. Python 표준 `zipfile` 등 정상 writer로 **새 이름 `Limitless_Field09_ObsidianScar_Minimal_Fix_v1_repacked.zip`**를 만들고, Windows/.NET/Python 모두 읽을 수 있는 표준 ZIP 헤더·CRC·중앙 디렉터리·파일명 방식으로 재포장(필요시 ZIP_STORED, Zip64 불필요, 특수 extra fields 없게). 같은 구성을 유지한 `Fixed_Candidates` PNG8 + Manifest/QA/README/Codex 안내 **13 entries**. 비트·헤더 수기 패치만으로 성공이라고 주장 금지.
4. **기존 PNG8 각각 SHA256 == 납품 Manifest SHA, 내부 문서/JSON/CSV 의미 불변, 정상 ZIP testzip/CRC=PASS** 확인. 원래 ZIP의 SHA는 달라지는 것이 정상. 새 archive SHA256, byte count, PNG별 원래·복구 SHA, Windows Explorer/.NET/Python 세 방식 검증 증거 기록.
5. 그 다음에만 **기존 기획 정본** `Field09_MinimalFix_v1_IsolatedQA_And_OfficialApply_Authorized_20261019.md`의 3해상도 Ground 반복/이종 seam + 6개 가이드 제거/과열·Main18·Field08↔09 조건부 QA를 이어간다. 한 곳이라도 FAIL이면 원본 Scene 적용 보류. 정상 QA0FAIL/미술 확인/모든 원본 SHA 보호 후 안전한 원본 Unity 적용. 기존 공식 PNG/meta 보존을 기본, 신규 후보는 새 GUID Asset과 지정 Renderer만 교체. `main` 직접 commit/Push/merge 금지.
6. 리뉴얼 아트 담당은 GitHub `docs/art-renewal`의 기존 손상 파일을 무단 삭제·덮어쓰기 하지 않고 **별도 정상 ZIP**을 제공/등록하고 납품 문서에 "원본 납품 ZIP 잘못된 헤더, 재포장 정정" 추가. Codex가 이미 원본 후보 PNG8를 무손실 복구할 수 있다면 작업을 중복 발주하지 않고 검사 근거만 전달한다.

## 2. VenomSnake v2, Wolf v2 — '오프라인 무결성'과 '공식 일러스트 완성도' 분리

사용자 공유 로컬 ZIP:
- `F:\Downloads\Limitless_Monster_Renewal_Pilot_VenomSnake_v2.zip`
- `F:\Downloads\Limitless_Beast_Companions_Wolf_v2.zip`

리뉴얼 납품 정본 `docs/art-renewal/문서/00_프로젝트/Monster_Beast_Pilot_v1_Rejection_v2_Delivery_20261019.md`는 v2가 **독립 코드로 만든 픽셀 도형 시트**임을 밝힘. VenomSnake: `32x32px × 4` 5동작(Idle/Walk/Attack/Hit/Defeat), Wolf: `64x40px ×6` Run, 둘 다 선명한 Pixel/프레임/CRC 무결성 PASS. **이 것은 예술적 합격이 아님**. 사용자는 v2의 과도한 블록감/낮은 질감 완성도를 지적하며 Chapter2 공식 11~18 몬스터와 어울리는 높은 완성도를 요구.

**실제 비교 기준:** GitHub `Unity/Client/Assets/_Project/Art/Monsters/11_SootHound/Soot_Hound_Sprite_Sheet.png.meta` 및 `12_HeatwindHawk/...meta`에서 프레임 rect **314×314**, 4열 Sprite/PPU314/feet bottom pivot으로 확인. 이는 현재 32×32 뱀(단축 비율~9.8배)과 64×40 늑대의 현재 도트 도형 스타일이 고품질 공식 몬스터들과 다르다는 **기술·스타일 참고**, **모든 시트를 일괄314px로 강제하라는 뜻은 아님**. 실제 11~18 공식 게임 렌더 화면/색감/명암/포즈·질감과 렌더 배율을 사용해 질적 기준(고유 실루엣, 자연스러운 신체 구조, 레이어 명암/빛 방향, 자연스러운 털·비늘·눈·독니·동작, 애니메이션 프레임 일관성, 작은 화면에서 식별)을 충족시키도록 한다. 최근 32px 시트를 nearest-neighbor로 256/314에 확대해 "완료" 처리 금지.

**새 제작 계약 (리뉴얼 담당):**
1. **뱀v2·Wolfv2 미술 승인 보류**(이것이 v2 사용자의 전 프레임 QA 지적을 모두 기계적으로 Fail했다는 주장이 아니며, 바울 대표의 직접 실물 미술 보완 의견). 기존 v1·v2 원본 ZIP/Manifest·Source 보존하고 신규 v3 full sheet 자동 생성 금지.
2. 실제 Chapter2 `SootHound, HeatwindHawk, FissureLizard, EmberBeetle, EmberWraith`의 미술 완성도를 **사진 그대로 복사·복제하지 않는 참고 규격**으로 삼고, 먼저 **VenomSnake와 Wolf의 고품질 단일 Key Art/대표 1포즈** 2장을 Chapter2 공식 아트와 나란히 비교 가능한 동일 World Size/실게임 렌더 배율 mockup으로 제시. 최소 무늬·빛/그림자·해부학/얼굴·실루엣 품질을 확인. 여기서 필요하다면 신규 마스터 원화를 충분한 해상도로 새로 그리고 실제 Unity 시트 변환시 크기/PPU/FPS·Collider는 Codex가 검토. **제작만 급히 완료하고 전체20+6프레임을 반복 만들지 말 것**.
3. 뱀 = 귀여움/완만한 원통 모양 X, 위협적인 쐐기형 머리·독니·S자 공격 준비·비늘 음영·몸통 접지, 과도한 고어 없이 Chapter1 적 레벨에 맞는 포식자. Wolf = 보노보노풍 유아표정/짧은 주둥이/큰 둥근 눈 X, 판타지 늑대다운 주둥이·귀·근육·다리 관절/털 방향/그늘, 가혹한 공포가 아닌 사수의 신뢰 가능한 야수 동료.
4. 초반 몬스터는 배경(녹색 Field)과 겹쳐도 실루엣/공격 준비·독속성 읽힘, 늑대는 40px UI 및 전투 돌진에서 늑대로 식별. 직접 높은 해상도의 PNG를 새로 만든 뒤 게임 DisplayScale/PPU를 검증; 인위적 선명도 필터/텍스처 확대만으로 퀄리티 충족 주장 금지.
5. 대표가 **2장 핵심 아트의 스타일**을 승인한 뒤에야 실제 Idle/Walk/Attack/Hit/Defeat 뱀20프레임·Wolf 6프레임 애니메이션 시트 제작/기술 규격 및 ZIP QA 진행. 필요하면 Unity Runtime 수용 규격(원본 Snake128×32 / Wolf384×40, Source Script 경로)을 Codex에게 **읽기 전용 조사 프롬프트**로 요청하고 원본 Scene/meta/AI/펫 규칙 자체는 미변경. Bear/Fox는 Wolf 기준 승인 전 추가 제작 중단.
6. 리뉴얼은 `docs/art-renewal`에 `V2_USER_QUALITY_HOLD / CHAPTER2_STYLE_REFERENCE / KEYART_REVIEW_PENDING` 기록 후 프리뷰·실제 화면에서의 예상 크기/단일 원화 권리/제작 방식 보고. Codex는 미술 승인 전 v2를 게임에 연결하거나 Shader·스케일 조절로 v2를 "고품질"로 포장하지 않는다.

## 3. 분리 운영 및 완료 보고

Field09 ZIP 문제는 **Codex 기술/포맷**(필요시 리뉴얼 재포장만), 뱀·늑대 품질 문제는 **리뉴얼 담당 미술**이며 동시 작업 가능. Codex 한도 절약: 3 구현으로 실패된 파일을 반복 읽지 않고 표준 ZIP 정상화→매니페스트/세 플랫폼 검증→Field09 원래 최소 Delta Unity QA만 실행. `CURRENT_STATUS`와 각 부서 `*_LATEST.md`에 새 상태를 기록하고 Branch/Commit/승인 여부를 남길 것. 이미지 소스·완성 ZIP은 실제 사용자 미술 승인 전 공식 Scene에 적용하지 않는다.

