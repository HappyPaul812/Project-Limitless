#!/usr/bin/env python3
"""CSV 또는 JSON 대사 목록을 MeloTTS 한국어 WAV로 미리 생성합니다."""

from __future__ import annotations

import argparse
import csv
import json
import re
from dataclasses import dataclass
from pathlib import Path
from typing import Iterable


SAFE_ID = re.compile(r"^[A-Za-z0-9._-]+$")


@dataclass(frozen=True)
class VoiceLine:
    line_id: str
    text: str
    speaker: str
    speed: float


def parse_line(row: dict[str, object], source: Path, index: int) -> VoiceLine:
    line_id = str(row.get("id", "")).strip()
    text = str(row.get("text", "")).strip()
    speaker = str(row.get("speaker", "KR")).strip() or "KR"
    try:
        speed = float(row.get("speed", 1.0))
    except (TypeError, ValueError) as error:
        raise ValueError(f"{source}:{index}: speed는 숫자여야 합니다.") from error

    if not SAFE_ID.fullmatch(line_id):
        raise ValueError(f"{source}:{index}: id는 영문·숫자·점·밑줄·하이픈만 사용할 수 있습니다: {line_id!r}")
    if not text:
        raise ValueError(f"{source}:{index}: text가 비어 있습니다.")
    if speed <= 0:
        raise ValueError(f"{source}:{index}: speed는 0보다 커야 합니다.")
    return VoiceLine(line_id, text, speaker, speed)


def load_lines(path: Path) -> list[VoiceLine]:
    suffix = path.suffix.lower()
    if suffix == ".csv":
        with path.open("r", encoding="utf-8-sig", newline="") as stream:
            rows: Iterable[dict[str, object]] = csv.DictReader(stream)
            return [parse_line(row, path, index) for index, row in enumerate(rows, start=2)]
    if suffix == ".json":
        with path.open("r", encoding="utf-8-sig") as stream:
            payload = json.load(stream)
        rows = payload.get("lines") if isinstance(payload, dict) else payload
        if not isinstance(rows, list):
            raise ValueError(f"{path}: JSON은 배열 또는 lines 배열을 가진 객체여야 합니다.")
        return [parse_line(row, path, index) for index, row in enumerate(rows, start=1)]
    raise ValueError(f"지원하지 않는 입력 형식입니다: {path.suffix} (CSV 또는 JSON 필요)")


def main() -> None:
    parser = argparse.ArgumentParser(description="LIMITLESS용 MeloTTS 한국어 WAV 일괄 생성")
    parser.add_argument("input", type=Path, help="id,text,speaker,speed 열을 가진 CSV 또는 JSON")
    parser.add_argument("--output-dir", type=Path, required=True, help="생성 WAV 폴더(Unity Assets 밖 권장)")
    parser.add_argument("--device", default="cpu", help="MeloTTS 장치: cpu, cuda, cuda:0 등")
    args = parser.parse_args()

    lines = load_lines(args.input.resolve())
    if not lines:
        raise ValueError("생성할 대사가 없습니다.")

    from melo.api import TTS

    output_dir = args.output_dir.resolve()
    output_dir.mkdir(parents=True, exist_ok=True)
    model = TTS(language="KR", device=args.device)
    speaker_ids = model.hps.data.spk2id

    for line in lines:
        if line.speaker not in speaker_ids:
            available = ", ".join(sorted(speaker_ids.keys()))
            raise ValueError(f"지원하지 않는 speaker {line.speaker!r}; 사용 가능: {available}")
        output_path = output_dir / f"{line.line_id}.wav"
        model.tts_to_file(line.text, speaker_ids[line.speaker], str(output_path), speed=line.speed)
        print(f"생성: {output_path} (speaker={line.speaker}, speed={line.speed:g})")


if __name__ == "__main__":
    main()
