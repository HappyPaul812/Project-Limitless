# Main08 기존 WAV 복구 후보 납품 — 2026-10-13

**복구 후보 준비 완료6개 / 현재 재제작 확정0개 / 전부 사용자 청취 대기.** 브랜치 `codex/main08-voice-recovery-candidates-20261013`, 이전 감사9024eda기준. GitHub 최신기획 ca1b290을 읽고 기존 PCM와 자동발화를 다시 검증한 결과입니다. 후보는 `문서/00_프로젝트/Evidence/Main08_RecoveryCandidates_20261013/Candidates`에만 저장했습니다. 원본 WAV·GUID/meta·Catalog·Unity/Save/설정 변경0, TTS/API0, 화면/음향 출력 조작0, Push0.

## 실제 청취용 파일

[청취 페이지: 여섯 개별 재생기](Evidence/Main08_RecoveryCandidates_20261013/Listen_Main08_Candidates.html) · [정확한 ID별 목록](Evidence/Main08_RecoveryCandidates_20261013/Main08_Recovery_Candidates.csv). 자동 재생이 없는 HTML이며 각 WAV는 아래 링크에서 직접 받을 수도 있습니다. 파일명은 **대상 StableID**이고 원본의 잘못된 ID를 그대로 옮겨 적지 않았습니다. 외부 페이지나 음성 업로드는 필요 없습니다.

| ID | 정확한 원문(후보 ASR 일치) | 길이(초) | RMS/Peak dBFS | 분리 |
|---|---|---:|---:|---|
| `main08_taeon_supp_002` | 여기부터 흔적이 거의 없습니다. | 2.410000 | -25.35 / -6.01 | 아니요 |
| `main08_miel_supp_001` | 몬스터가 적다는 건 좋은 일 아닌가요? | 3.730000 | -18.10 / -1.76 | 아니요 |
| `main08_taeon_supp_003` | 평소라면 그렇겠죠. | 3.130000 | -26.60 / -4.08 | 아니요 |
| `main08_taeon_supp_004` | 그런데 조금 전까지 이쪽으로 이어지던 흔적들이 전부 방향을 바꾸고 있습니다. | 6.890000 | -17.93 / -1.65 | 아니요 |
| `main08_miel_supp_002` | 없어진 게 아니네요. | 2.057542 | -18.85 / -3.92 | 예 |
| `main08_miel_supp_003` | 다들 이쪽을 피해서 지나가고 있어요. | 3.062458 | -19.72 / -3.32 | 예 |

- [main08_taeon_supp_002.wav](Evidence/Main08_RecoveryCandidates_20261013/Candidates/main08_taeon_supp_002.wav)
- [main08_miel_supp_001.wav](Evidence/Main08_RecoveryCandidates_20261013/Candidates/main08_miel_supp_001.wav)
- [main08_taeon_supp_003.wav](Evidence/Main08_RecoveryCandidates_20261013/Candidates/main08_taeon_supp_003.wav)
- [main08_taeon_supp_004.wav](Evidence/Main08_RecoveryCandidates_20261013/Candidates/main08_taeon_supp_004.wav)
- [main08_miel_supp_002.wav](Evidence/Main08_RecoveryCandidates_20261013/Candidates/main08_miel_supp_002.wav)
- [main08_miel_supp_003.wav](Evidence/Main08_RecoveryCandidates_20261013/Candidates/main08_miel_supp_003.wav)

## 단순 재사용4개와 긴 대사 검색

- 태온002←기존태온003, 태온003←기존태온004, 미엘001←기존미엘002: 원본 파일과 **WAV 전체 바이트/SHA 동일** 복사입니다. PCM/음량/호흡/앞뒤 여백을 편집하지 않았습니다.
- 태온004의 긴 원문 “그런데 조금 전까지 이쪽으로 이어지던 흔적들이 전부 방향을 바꾸고 있습니다.”는 **기존태온005에서 발견**, 제작원본 폴더의 같은 파일도 동일 SHA입니다. 게임Voice/Google TTS/보관Main18·19의 WAV 657개를 기존 확인된 PCM 해시로 검색하여 같은내용의 원본2경로를 찾았습니다. 파일명/CSV 순서만으로 맞다고 판정한 것이 아니라 태온005의 새 로컬 ASR 발화가 긴 정본과 일치함도 확인했습니다. 다른 전체WAV를 새로 자동 전사하지 않았습니다.
- 따라서 태온004를 현재 재제작 후보로 확정할 필요가 없습니다. 후보는태온005 바이트 그대로 복사했고 기존태온005 연결은 수정하지 않았습니다. 사용자 청취가 부적합이면 그때해당ID만 재제작 후보로 다시분류합니다.

## 미엘 합본 분리2개

- 원본 `main08_miel_supp_003.wav` 5.12초에는 “없어진 게 아니네요.”와 “다들 이쪽을 피해서 지나가고 있어요.”가 함께 검출됩니다. 원문후보의 단어 타임스탬프와10ms peak/rms 파형을 대조했습니다. 타임스탬프는 자동인식 추정값이며 사람이 확정한 경계는 아닙니다.
- 첫 문장 ASR종료1.64초 / 다음 “다들” ASR시작2.28초 사이에 **peak≤−45dBFS 구간1.83~2.28초(450ms)**가 있어 문장 사이의 낮은신호 간격으로 판단했습니다. 따라서 발화 도중 임의 절단하지 않았습니다.
- 실제절단은 **49381번째 프레임 / 2.057541667초 / 샘플값0**. 앞샘플5/32768, 절단전후40ms peak -49.72dBFS였습니다. 미엘002는원본 `[0,49381)`, 미엘003은 `[49381,122880)` PCM을 사용했습니다. WAV header의 프레임수만 각 후보에 맞게 작성했고 채널/24kHz/16bit는 유지했습니다.
- **두 후보의 PCM을 순서대로 이어붙이면 원본 PCM과 완전히 동일**, 삭제/중복0프레임. Fade·증폭·Normalize·노이즈제거·리샘플0. 발화 샘플을 고치지 않고 자연 간격 중간에서 나눴습니다. 절단 클릭/끝음잘림/앞뒤 여백의 최종 자연스러움은 사용자 청취로 확인해야 하며 PASS로 가장하지 않습니다.
- 분리된 두파일을 각각 새로 자동인식해 “없어진 게 아니네요.” / “다들 이쪽을 피해서 지나가고 있어요.”가 단독으로검출됨을 확인했습니다. 앞 문장 혼입 검출0입니다. 미엘003 문장 자체를 잘못된 대사로 판단하거나 바꾸지 않았습니다.

## 원본 출처와 SHA256

- `main08_taeon_supp_003` / `Unity\Client\Assets\_Project\Audio\Voice\Story\Main08\main08_taeon_supp_003.wav`
  - 원본 WAV SHA256: `7075ffb191443725ecc01680d92a4cabb258d1b7f4766808b4ad6cc3bc777e2c`
- `main08_miel_supp_002` / `Unity\Client\Assets\_Project\Audio\Voice\Story\Main08\main08_miel_supp_002.wav`
  - 원본 WAV SHA256: `d06d4fc4453ebdb96a43f6847b58fe617e901fb3eb54451226bbaa4d74db34d8`
- `main08_taeon_supp_004` / `Unity\Client\Assets\_Project\Audio\Voice\Story\Main08\main08_taeon_supp_004.wav`
  - 원본 WAV SHA256: `4162175dbbc3570ed5ad1531818a9bb2939263216ec6895241bcb9885f79dbce`
- `main08_taeon_supp_005` / `Unity\Client\Assets\_Project\Audio\Voice\Story\Main08\main08_taeon_supp_005.wav`
  - 원본 WAV SHA256: `195b136a6d44acd305a6a9d426841770d5f5206d38d71e2a8d15b9141c5a5779`
- `main08_miel_supp_003` / `Unity\Client\Assets\_Project\Audio\Voice\Story\Main08\main08_miel_supp_003.wav`
  - 원본 WAV SHA256: `21d143ebd34e6ec8038d9335897494d6f4bc3fc0c30c5d54cbe3e90c28e37b23`

## 검증·승인 상태

- WAV6개 RIFF/PCM decode PASS, 복사4개의전체바이트 동일, 분리2개의원본샘플동일/합산원본복원 PASS. 새 로컬ASR6개 모두 정본본문과일치(띄어쓰기/문장부호만제외), 자동인식이므로 인간청취승인아님. 태온Gacrux·미엘Sulafat의기존원음재사용이며새음색/스타일생성0.
- 기준 원본 보호 **5539파일 SHA**·기존미커밋목록·HEAD 불변. 지난196추가/7갱신 문서와CURRENT_STATUS 보존병합도 변경하지 않았습니다. 정식 게임에적용하지 않았으며Unity는실행하지 않았습니다. 이번WAVdecode/ASR를 새Quest/Battle/Save Runtime QA라고 보고하지 않습니다.
- **복구 후보 준비 완료:** 위6개. **재제작 후보:** 현재확정0개; 사용자청취에서복사본발화/품질·분리본경계가부적합한ID만추가분류. **사용자청취대기:**6/6. 모든후보는미승인으로유지합니다.
- 청취후 바울 대표가 ID별 승인하신 항목만 별도 적용계획을 세웁니다. 기존GUID/meta를보존한파일단위수정과해당페이지·앞뒤·Quest/Save 경계QA가필요하며이번엔어떤연결도변경하지않았습니다. 승인전새TTS와음성교체0을유지합니다.
- [후보 Manifest](Evidence/Main08_RecoveryCandidates_20261013/candidate_manifest.json), [경계·무손실검증](Evidence/Main08_RecoveryCandidates_20261013/boundary_validation.json), [후보별ASR](Evidence/Main08_RecoveryCandidates_20261013/candidate_asr_validation.json), [긴대사원본검색](Evidence/Main08_RecoveryCandidates_20261013/long_sentence_source_search.json), [원본보호](Evidence/Main08_RecoveryCandidates_20261013/protection.json). 로컬기존Whisper모델을오프라인으로사용했고이번모델/API/TTS 네트워크호출0입니다.
