# BGM_LATEST — 2026-10-20

## 현재 역할
Chapter3 BGM 담당. 담당 브랜치 `docs/bgm`. main 직접 commit/Push/merge 금지.

## 최신 발주
Issue #31: F1 북풍·서릿길 마을 2곡 샘플.

## 현재 완료
- docs/planning Chapter3 병렬 제작 킥오프 확인.
- Main22 F1 / Main23~24 서릿길 마을 최신 기획 확인.
- 2곡의 제작 프롬프트와 QA 계약 확정.
- 제작 브리프: `문서/00_프로젝트/Chapter3_BGM_Pilot_Production_Brief_20261020.md`

## 상태
- F1 데모: PROMPT_LOCKED / AUDIO_GENERATION_PENDING
- 서릿길 마을 데모: PROMPT_LOCKED / AUDIO_GENERATION_PENDING
- 사용자 청취: PENDING
- SHA/decode/loop QA: PENDING
- Unity 적용: NOT AUTHORIZED / 0
- 설빙의 폭군 전체 보스곡: NOT AUTHORIZED / 0

## 보호
기존 Chapter1·2 공식 BGM, TTS, Unity Audio/Mixer/Scene/Save 원본 변경0.

## 다음 순서
1. 위 프롬프트로 각 1차 데모 생성(45~90초 권장).
2. MP3/WAV 수령 즉시 SHA256·decode·길이·루프·TTS 공간 검수.
3. 사용자 청취 승인.
4. 승인 후에만 Unity 적용용 별도 Handoff 작성.
