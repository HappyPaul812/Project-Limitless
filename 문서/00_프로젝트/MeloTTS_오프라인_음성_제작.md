# MeloTTS 오프라인 음성 제작

## 용도와 경계

MeloTTS는 개발자가 한국어 대사를 WAV로 미리 만드는 외부 제작 도구로만 사용한다. MeloTTS 코드·모델·Conda 환경·다운로드 캐시는 Unity 프로젝트와 Git 저장소에 넣지 않는다. 음질과 사용 여부를 확인한 최종 WAV만 향후 별도 작업에서 Unity `Assets`에 추가한다. Unity 런타임과 NPC 대화 시스템은 이번 시험에 포함하지 않는다.

## 공식 기준

- 공식 저장소: https://github.com/myshell-ai/MeloTTS
- 시험 소스: `main` commit `209145371cff8fc3bd60d7be902ea69cbdb7965a`
- 패키지 버전: `0.1.2`
- 공식 개발·테스트 환경: Ubuntu 20.04, Python 3.9
- 한국어 API: `TTS(language='KR')`, speaker `KR`
- Windows 공식 권장 방식: Docker

로컬 시험은 기존 `py3_12`를 변경하지 않고 전용 Conda 환경 `melotts`(Python 3.9)를 사용한다. Windows에서 `g2pkk`가 자동 설치하는 `eunjeon`은 C++ 빌드 도구가 필요해 실패하므로, 전용 환경에서 일본어용 `mecab-python3`를 제거하고 Windows wheel이 있는 `python-mecab-ko`를 설치한다. `eunjeon.Mecab` 이름을 `mecab.MeCab`에 연결하는 최소 shim은 전용 환경에만 둔다.

## 로컬 생성

공식 소스와 모델 캐시는 `F:/study/tts/MeloTTS`, 시험 WAV는 `F:/study/tts/MeloTTS/output/limitless`에서 관리한다. 저장소의 `Tools/TTS/melotts_batch.py`는 UTF-8 CSV 또는 JSON을 읽고 모델을 한 번만 로드한 뒤 `id.wav`를 만든다. OpeningIntro 음질 확인 결과는 `intro_test_speed100.wav`를 기준으로 확정했으며, 제작 기준은 MeloTTS `0.1.2`, Python `3.9.25`, language/speaker `KR`, speed `1.00`, CPU다.

CSV 열은 `id,text,speaker,speed`이며 `speaker` 기본값은 `KR`, `speed` 기본값은 `1.0`이다. ID에는 영문·숫자·점·밑줄·하이픈만 허용해 출력 폴더 밖으로 파일이 생성되지 않게 한다.

```powershell
conda run --no-capture-output -n melotts python Tools/TTS/melotts_batch.py Tools/TTS/intro_test.csv --output-dir F:/study/tts/MeloTTS/output/limitless --device cpu
```

JSON은 레코드 배열 또는 `{ "lines": [...] }` 구조를 지원한다. 수백 개 대사도 같은 모델 인스턴스를 재사용하며 각 레코드를 `id.wav`로 출력한다.

OpeningIntro 전체 대사는 `Tools/TTS/opening_narration.csv`에서 관리하고 외부 작업 결과는 `F:/study/tts/MeloTTS/output/limitless/opening`에 생성한다. WAV의 문장·샘플레이트·채널·비트 깊이·무음 여부를 확인한 뒤에만 `Unity/Client/Assets/_Project/Audio/Voice/Opening`으로 복사한다. 현재 18개 파일은 44.1kHz, 모노, 16비트이며 최종 `LIMITLESS` 제목 장면에는 내레이션을 사용하지 않는다.

```powershell
conda run --no-capture-output -n melotts python Tools/TTS/melotts_batch.py Tools/TTS/opening_narration.csv --output-dir F:/study/tts/MeloTTS/output/limitless/opening --device cpu
```

## Google Colab

`Tools/TTS/MeloTTS_LIMITLESS_Colab.ipynb`를 사용한다. 2026-09-09 기준 Google의 공개 Runtime Version 목록에서 최신 계열은 Python 3.12이며 MeloTTS 공식 시험 버전 Python 3.9와 차이가 있다. Notebook은 Colab 시스템 Python을 교체하지 않고 `uv`로 `/content/melotts-venv`에 Python 3.9 환경을 만든다. Linux에서는 대소문자를 구분하므로 일본어 import용 `MeCab`과 한국어용 `mecab` 패키지를 함께 둘 수 있다. Google Drive mount 셀은 주석 상태이며 사용자가 선택해서 실행할 때만 연결한다.

Colab 런타임과 외부 패키지는 바뀔 수 있으므로 설치·한국어 모델 로드·WAV 생성 셀을 순서대로 실행해 매번 확인한다. Python 버전을 억지로 바꾸거나 기존 Colab 패키지를 대량 downgrade하지 않는다.

## 라이선스

공식 `LICENSE`는 Copyright (c) 2024 MyShell.ai의 MIT License다. 사용·복제·수정·배포·재라이선스·판매를 허용하므로 상업 및 비상업 사용이 가능하다. 소프트웨어 또는 그 상당 부분을 배포할 때 저작권 고지와 허가문을 포함해야 한다. 모델 카드와 실제 생성 음성에 적용될 수 있는 별도 조건은 정식 배포 전에 다시 확인한다.

LIMITLESS에서는 MeloTTS 자체를 Unity 런타임에 배포하지 않고 개발 중 사전 음성 생성에만 사용한다.
