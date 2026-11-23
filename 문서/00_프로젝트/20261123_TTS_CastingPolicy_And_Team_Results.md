# LIMITLESS / 2026-11-23 / 주요인물 음성 보호·일반 NPC 무료 TTS 정책 및 부서 보고 접수

## 사용자 신규 확정 정책
바울 대표: "tts는 주요 인물만 기존대로 하고 나머지는 꼭 필요하면 우리가 지금 쓰는 구글 tts 말고 구글 무료 tts 또 있지? 그걸로 하자고 전달해줘."

- **주요인물 5인** 폴·미엘·세린·태온·하렌의 기존 승인 목소리/캐스팅/기존 162 WAV는 변경·재합성 금지. 새 주연 대사도 동일 음성 계열이 기본이지만 실제 합성은 별도 승인.
- **기타 NPC는 기본 음성 없음**. 무음 자막으로 전달이 어려운 꼭 필요한 일부 배역만 저비용 후보 검토. 신규 23개 NPC 발화를 일괄 합성하지 않음.
- **보조 후보 제품:** Google Cloud Text-to-Speech, 한국어 `ko-KR-Standard-A/B/C/D` 및 `ko-KR-Wavenet-A/B/C/D`. 기존 Gemini TTS와 구분. 2026-11-23 공식 표준 가격표상 해당 계열 월 400만 자 무료 사용 한도, 단 Google Cloud 청구 계정 **필수**, 초과 시 자동 과금. 비용 발생 방지 수단, 자수 계산/사용량, 상용사용 약관 확인/대표 승인 전 API 호출·결제 설정·키조회/음성 생성0. ‘완전 무료’라고 호칭하지 않음. Google Translate 비공식 gTTS 엔드포인트 사용 금지.
- 공식 Google Cloud 설명: https://cloud.google.com/text-to-speech/pricing 및 https://docs.cloud.google.com/text-to-speech/docs/list-voices-and-types / https://docs.cloud.google.com/text-to-speech/docs/basics ; 비공식 gTTS: https://github.com/pndurette/gTTS .
- [TTS #125에 2026-11-23 정책 직접 전달](https://github.com/HappyPaul812/Project-Limitless/issues/125#issuecomment-7087343324). 담당은 docs/tts에서 NPC 23행 '무음 기본 / 필요 발화 후보' 분류표와 소형 무호출 캐스팅 계획을 작성할 것. **신규 원문·ID/합성·Runtime 등록은 사용자 별도 승인**.

## 오늘 접수된 각 결과 (GitHub 기록)
- [#122 Codex](https://github.com/HappyPaul812/Project-Limitless/issues/122#issuecomment-7086767830): 승인 전리품 PNG6/6 복구 SHA 일치, ItemId 정식0/6, MonsterDefinition/Drop target0/6, DROP_LINK_HOLD, 격리.NET60PASS, 100% 난수 edge에서 누락 재현. 단회 엘리트 완료키 및 가방 초과 처리/실전 Save 미검증. 브랜치 codex/ch3-loot6-contract-20261123, 로컬 SHA 0863351efc349a5cf4857f5d61b65f51e9355aad. 대표 미승인 정식ID/본편 연결 금지.
- [#123 메인 아트](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7086796003): F3 눈·암반 접합256², 저상 암반320×192, 바닥경계256×128 세 PNG 기술오프라인QA. Library 실물 ZIP SHA256 5704d754355d7a4d353be8324c0c442e0b8672e623fe7492849e40c49c4e8be4, CRC10/10 PASS를 기획에서도 재확인. 기존 F2 화풍에 근접하며 길 밖 배치 콘셉트는 양호. 다만 첫 '지면 접합'은 높게 쌓인 바위처럼 보여 **단일 길가 overlay 후보로만**, 반복타일 seam·PPU/Pivot/Sorting/Collider/실제4.5WU 휠체어 경로 NOT_VERIFIED. 사용자 최종 시각 승인/Unity 실제배치 별도. docs/art-new final 00ebb454e20024bbbaad023b29a6e4958c30ad2f.
- [#124 리뉴얼](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7086771177): 정적 참조로 Game-icons.net White 9종(길5+전리품4) 실제 GUID/런타임 표시경로 실증, CC BY3.0 저작자표시 필요; 정화 PVFX SpectralBloom 및 Kenney BattleIcons 21은 CC0, GameView 노출/기존 대체본 조사 미완. 신규 VFX 파일럿 제작0. docs/art-renewal 최종 33a770b1bc03a18ce31b3d46e15078011f5cbb83. 제안: 비고유 실사용 9아이콘을 우선순위 후보로만 관리, 사용자 새 제작 승인은 별도.
- [#125 TTS](https://github.com/HappyPaul812/Project-Limitless/issues/125#issuecomment-7086796755): Main24~29 83행+서브6 18행, 총 101행, 주연72·신규 NPC23·무음6, 후보 VoiceID 101 고유, 기존312ID 정적 충돌0, 실런타임 ID 확인 전 합성0. docs/tts final 46506bcc54b67431ad06f4d9fe8a5f1d53e45d59. **위 신규 사용자 음성정책 우선**.
- [#31 BGM 정정](https://github.com/HappyPaul812/Project-Limitless/issues/31#issuecomment-7087190126): `Where_the_Grass_Bends` 및 `Hearth_in_the_Low_Valley` 2026-10-20 사용자 기존 승인 확인. 원곡 재승인 불필요. 새 루프 A/B 비배포 후보4개 디코드 PASS, LOOP_EDIT_APPROVAL_PENDING, Unity NOT_APPLIED. docs/bgm latest 8722b340efda51bc546faa03d70378ecdec0f32c.

## 변경·검증 경계
이번 총괄 기획 작업은 docs/planning 및 담당 Issue의 **정책 기록**만. 다른 세션 작업을 자동 실행했다고 말하지 않음. Unity6000.6.5f1, 기존 #85 HOLD/Discord #66 HOLD, 사용자 개인 Save/UserData/승인 원화·음성, 이전 EOD 복구 ref 보존. 원격 main 변경/로컬PC 제품 적용/Google TTS 외부 호출0.
