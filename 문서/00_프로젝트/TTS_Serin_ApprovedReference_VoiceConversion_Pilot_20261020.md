# LIMITLESS / 바울 대표 승인 세린 WAV 기준 음색 변환 1개 시범 (2026-10-20)

**PYTHON_PACKAGE_READY / USER_AUDIO_NOT_ACCESSIBLE_IN_CHAT / REAL_VOICE_CONVERSION_NOT_EXECUTED / API0 / UNITY0 / USER_LISTENING_PENDING**

## 사용자 요청 / 범위

바울 대표: “내가 OK한 세린 파형으로 니가 바꿔볼래?”

원래 같은 보이스 ID Schedar 생성 음성이 바뀌는 문제로 Main20/21 세린8개가 남성 목소리처럼 들림. 기존 피치·포먼트 DSP 6개 미리듣기는 “외계인 목소리”로 전량 불합격했고 Gemini 재시도 1회에서도 남성으로 들림. 따라서 임의 음높이 조절/새 Gemini 호출 없이 **정식 청취 승인 WAV의 실제 화자 음색을 참조하는 오프라인 AI voice conversion 1개**만 시험.

## 확정 참조 / 대상

- 사용자 승인 실제 기준: `F:\Downloads\Voice\Limitless_TTS_Serin10_Schedar_v2\sample_candidate\main18_ground_serin_01.wav`; 기존 승인 문서 SHA256 `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e`.
- 고칠 원본 1개: `F:\Downloads\Voice\Limitless_TTS_Today20_400Diagnostic_20261020\main20_confront_serin_01_DIAGNOSTIC.wav`, 오늘 Google TTS 단발 제작에 성공했으나 사용자 음색 판정 FAIL. 원본17·진단 로그·HOLD/lock은 변경하지 않는다.
- 새 결과: `F:\Downloads\Voice\Limitless_TTS_Serin_ReferenceVC_20261020\main20_confront_serin_01_APPROVED_VOICE_VC_PREVIEW.wav`.
- 독립 QA `PREVIEW_AUDIT.json` SHA/샘플레이트/길이 및 소스·참조 원본 해시 기록. user listening 전 Unity 절대 적용 금지.

## 직접 만든 Python 실행 패키지

`LIMITLESS_Serin_ApprovedVoice_VC_20261020.zip` (6,711 bytes, SHA256 `77293681c01e82f7be00fb2029f73ca287c0522dadbb338c8086097c7f567f06`).

포함: `LIMITLESS_Serin_ReferenceVC.py` (10,528 bytes, SHA256 `7035e4f3b209d53b8b35c2eeddff089402e2b1a190929f7991ce52da0b1651c7`), `README_KO.txt`.

- Python 표준 라이브러리만 사용. Windows CPU용 `jingangdidi/voice_clone` GitHub v0.1.2 OpenVoice 기반 실행 파일과 `rsxdalv/OpenVoiceV2` HuggingFace pretrained model을 별도 명시적 `setup` 단계에서만 다운.
- Windows 바이너리 GitHub Release SHA256 `cc2392d1612d18317dbb49635b7350e9fcfc9a59ed9d6ccb53b05b819a3edaf3`. OpenVoice V2 converter checkpoint SHA256 `9652c27e92b6b2a91632590ac9962ef7ae2b712e5c5b7f4c34ec55ee2b37ab9e`; 본문 ZIP에 exe/model 원본 포함하지 않음.
- `check`: 원본/참조 PCM16 mono 형식·실제 승인 WAV SHA를 읽기 전용 확인. API0/파일쓰기0.
- `setup`: 별도 공개 OSS 실행파일 약 15MB, 모델 약 131MB를 허가 후 다운로드/해시 검사. 무료 인터넷 다운로드, **Gemini TTS API0**, 엔진이 외부 바이너리임을 명시. 기존 Python conda py3_12 유지.
- `preview`: 정확히 1개 소스와 승인 참조만 staging temp로 바이트 복사. Windows 드라이브 콜론을 voice_clone 인수 separator로 해석하는 문제 회피를 위해 실행기에는 `source.wav`와 `reference.wav` 상대경로만 전달. OpenVoice converter 결과를 새 별도 WAV로 저장. 유효한 SHA 확인·중복 결과 덮어쓰기 차단·원본 SHA 불변 재확인. API0.
- **실제 사용자 F: WAV 및 제3자 엔진/model을 이 ChatGPT 런타임에서 실행하지 못했음.** Python compile PASS, 가상 PCM16 입력/승인 hash, 사전검사, command 인수 조립, 샘플 결과·QA audit, 재실행 차단, 잘못된 승인 SHA 거절 모의 PASS. 이는 실음성 변환 품질 검증이 아님.

## 실행 (Anaconda PowerShell)

```powershell
conda activate py3_12
cd F:\Downloads\Voice\LIMITLESS_Serin_ApprovedVoice_VC_20261020
python .\LIMITLESS_Serin_ReferenceVC.py check
python .\LIMITLESS_Serin_ReferenceVC.py setup
python .\LIMITLESS_Serin_ReferenceVC.py preview
```

ZIP은 위 폴더에 압축 해제. `check`가 실패하면 임의 WAV를 대체해서 진행하지 않고 보고. 모델 다운로드에는 일반 인터넷 연결/로컬 디스크 공간이 필요하며 다른 사람 음성 원격 업로드는 하지 않는 로컬 변환. 결과는 청취/대사/왜곡 QA 후 승인 여부 결정, 이전 8개 일괄 작업/Unity 변경 금지.

외부 OSS 및 모델 링크:
- https://github.com/jingangdidi/voice_clone
- https://github.com/jingangdidi/voice_clone/releases/tag/v0.1.2
- https://huggingface.co/rsxdalv/OpenVoiceV2/tree/main/checkpoints_v2/converter

## 운영

TTS 세션이 Python 제작 및 사용자 청취 피드백 관리. Codex는 사용자 최종 승인 WAV Unity 통합만. 이번 문서화는 docs/tts 브랜치만, main 직접 커밋/Push/merge 없음.
