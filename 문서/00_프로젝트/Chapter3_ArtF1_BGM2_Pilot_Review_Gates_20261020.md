# LIMITLESS / 2026-10-20 Chapter3 신규 아트 F1 파일럿·BGM 2곡 1차 보고 판정

- 담당: 기획 docs/planning.
- 사용자 제공 **로컬 파일 경로** `F:\Downloads\Limitless_Chapter3_ArtNew_F1_Frostway_Pilot_v1.zip`. 본 기획 ChatGPT 환경에서는 사용자 Windows F: 경로를 직접 열람할 수 없고 현재 sandbox에서 ZIP 실물을 보유하지 않음. 별도 Chapter3 메인 아트 세션이 작성한 GitHub `docs/art-new` 보고/인계에서 납품 명세 확인. **실제 ZIP 재다운로드·바이트 검사 및 화면 시각 품질은 본 기획 세션에서 독립 검사하지 못함.**
- BGM은 사용자 “bgm 보고서도 봐” 요청. `docs/bgm` `Chapter3_BGM_Pilot_2Track_Listening_QA_20261020.md`와 `BGM_LATEST.md` 독해. **실제 MP3는 본 기획 세션에 없고 GitHub에는 보고서만 있으므로 직접 청취한 결과라고 주장하지 않음.**

## A. Chapter3 신규 아트 Issue #30

- `docs/art-new` 최신 납품 명세 커밋 `5b46e3d6bed2`, 인계 `06e928b02889`.
- ZIP `Limitless_Chapter3_ArtNew_F1_Frostway_Pilot_v1.zip`: 담당 보고 SHA256 `6a22ac4e9c4d4f4f9e37c2ac8270b145d5483676d00f83c320cdf0e6925382b4`. 21항목, 실제 PNG12(F1 독립6, 마을4, 서리들쥐/설빙의폭군 key art2), preview3, 반복QA3 및 Manifest 등. 개별 지면/props 256×256 RGBA, 일부 Village Building/KeyArts 512×512 RGBA.
- 담당 보고 파일decode/PNG SHA/ZIP testzip/타일 base2×2 끝점 양축 일치 PASS. **이는 실제 월드 PPU/Collider/Sorting/스폰·경로 편의·그림완성도 PASS가 아님.**
- 제작법이 서드파티 복제가 아닌 독립 **Pillow 결정론적 픽셀 드로잉**이라고 명시. 이전 Chapter1/2 미술에서는 규격 PASS와 사람의 시각 만족이 달랐기 때문에, **무조건 정식 승인하지 말고** 실제 PNG/3개 프리뷰를 Codex가 로컬 ZIP에서 읽고 Chapter2 승인 공식 배경/몬스터와 실월드 렌더 스케일/화면비 나란히 비교한 Preview Gallery를 우선 마련. 2D 탑다운 월드와 전투 몬스터/보스는 **서로 다른 표현 맥락**이므로 각자 비교.
- Codex Issue #35 기술 프리플라이트(실사용 PPU/Pivot/Collider/맵 Tile seam, 새 Scene·QuestID)와 연결. 이미 진행 중인 Codex 업무/Field09 시각 QA와 충돌하지 않게 **신규 Unity 통합·전체 신규 Scene 구현 지시 금지**. User의 첫 공식 미술 승인 전 추가 F1/마을/몬스터 풀 시트 대량생산 금지.
- 현 상태: `PNG12_DELIVERY_REPORTED / OFFLINE_QA_REPORTED_PASS / GITHUB_BINARY_NOT_UPLOADED / USER_ART_APPROVAL_PENDING / TECH_IMPORT_CONTRACT_PENDING / UNITY_NOT_APPLIED`.

## B. BGM Issue #31

- `docs/bgm` 브리프 `Chapter3_BGM_Pilot_Production_Brief_20261020.md`, QA `Chapter3_BGM_Pilot_2Track_Listening_QA_20261020.md`, 마지막 인계 `BGM_LATEST.md`. 최신 QA/인계 commit `122682e8b345` / `e44aee0ecd8b`. 담당 세션에 **실제 MP3 2개 업로드**됐다고 기록, GitHub에는 오디오 등록 0.
- F1 `Where_the_Grass_Bends.mp3`: 74.239958초/44.1kHz Stereo/1,787,885B/SHA256 `2bbbe6fe02b46c391653a5409802dde819ba64b5b5be99b20bbeafb4ad9b0fca`. 담당 **배경 역할/곡 구성 PASS 추천**. 약71.88초부터 마지막 2.36초 무음 → **raw direct loop FAIL**.
- 서릿길 마을 `Hearth_in_the_Low_Valley.mp3`: 72.620333초/44.1kHz Stereo/1,749,014B/SHA256 `a8cae73d15d276ad5cf25415ee13bce6e9761b331822f8dcb5928da9b87c772b`. 담당 **분위기·구분 PASS 추천**. 시작 무음0.45초/끝 무음2.55초·종지/Loop 접속 부적합 → **raw direct loop FAIL**.
- 원본 Integrated loudness 각 -11.1/-11.4 LUFS는 오디오 자체 값이지 인게임 적정 레벨이 아님. 기존 `BgmPlaybackService`/Mixer 및 TTS 우선 정책에 맞춰 향후 음량 낮추기/ducking·대화와 동시 청취를 검증해야 한다. 생성도구/상업 이용 및 원본 보존 증빙을 메타에 포함. 원본 MP3 직접 수정을 금지하고 **사용자 청취로 곡 후보 승인 후에** 루프 전용 편집본을 생성해 음악적 crossfade/end→intro 시청감 테스트.
- 현 상태: `COMPOSITION_PASS_RECOMMENDED_BY_BGM_TEAM / LOOP_EDIT_REQUIRED / USER_LISTENING_PENDING / AUDIO_BINARY_GITHUB_0 / UNITY_AUDIO_NOT_APPLIED`.

## 다음 단계 의존관계
1. 바울 대표가 BGM 2곡 원본을 **직접 듣고 곡 자체 적합 여부** 결정. 곡을 승인하면 담당이 각 파일의 시작/끝 무음·종지 보완 루프 버전을 재납품하고 Codex가 실제 Unity 루프/TTS 혼합검증. 미승인 곡은 폐기/교체, 루프 수리로 곡미술 승인했다고 오해 금지.
2. 메인 아트 로컬 ZIP은 Codex가 안전 격리에서 SHA/Manifest/PNG12/preview 및 기존 F1·마을 실규격·Chapter2 공식 스타일을 실제로 비교한 **Visual Preview**를 사용자에게 제시. 그림 스타일 반려 시 PNG 수가 아닌 시각적 비율/실루엣·명암 결함을 정확히 특정하고 작은 샘플만 재제작.
3. 바울 대표 **현재 병렬 실행 승인 담당은 Chapter3 메인 아트·BGM만**. TTS/SFX/개발 영상 업무는 대기; 기존 발주 이슈는 기록·future queue 유지하되 추가 진행·API 호출 금지. Chapter1·2 `docs/art-renewal` Field09 수정과 분리.
4. 메인 아트가 사용자 스타일 승인·Codex 기술/PPU 확인되면 그때 별도 범위의 Game-ready 통합을 결정. `main` 직접 Push/Unity 원본 수정 금지.
