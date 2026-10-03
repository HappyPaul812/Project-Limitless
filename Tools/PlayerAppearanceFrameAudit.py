"""검증본 PNG를 수정하지 않고 셀별 Alpha 경계와 원본 해시를 기록합니다."""
import hashlib
import json
from pathlib import Path
from PIL import Image, ImageDraw

ROOT = Path(__file__).resolve().parents[1]
INVENTORY = ROOT / "Unity/Client/Assets/_Project/Resources/PlayerAppearances/Validated50.json"
OUTPUT = ROOT / "문서/11_UI/Player_Appearance_Frame_Bounds.json"


def run():
    inventory = json.loads(INVENTORY.read_text(encoding="utf-8-sig"))
    results = []
    contacts = 0
    for entry in inventory["entries"]:
        path = ROOT / "Unity/Client" / entry["assetPath"]
        digest = hashlib.sha256(path.read_bytes()).hexdigest()
        assert digest == entry["sha256"], f"원본 PNG 변경: {path}"
        with Image.open(path) as sheet:
            assert sheet.mode == "RGBA" and sheet.size == (512, 512)
            cells = []
            for frame in range(16):
                x, y = frame % 4 * 128, frame // 4 * 128
                cell = sheet.crop((x, y, x + 128, y + 128))
                alpha = cell.getchannel("A")
                bbox = alpha.getbbox()
                assert bbox is not None, f"빈 셀: {path}/{frame}"
                edges = {}
                for side, box in {
                    "top": (0, 0, 128, 1), "bottom": (0, 127, 128, 128),
                    "left": (0, 0, 1, 128), "right": (127, 0, 128, 128),
                }.items():
                    values = list(alpha.crop(box).getdata())
                    if max(values) > 0:
                        edges[side] = {"pixelsAlphaNonzero": sum(v > 0 for v in values),
                                       "pixelsAlphaAtLeast128": sum(v >= 128 for v in values),
                                       "maxAlpha": max(values)}
                if edges:
                    contacts += 1
                cells.append({"frame": frame, "direction": ["Down", "Left", "Right", "Up"][frame // 4],
                              "directionFrame": frame % 4, "bbox": bbox, "edges": edges})
        results.append({"appearanceId": entry["appearanceId"], "assetPath": entry["assetPath"],
                        "sha256MatchesOriginal": True, "readyForSelection": entry["readyForSelection"],
                        "reviewReason": entry["reviewReason"], "cells": cells})
    report = {"appearanceCount": len(results), "frameCount": len(results) * 16,
              "edgeWarningSheets": sum(any(c["edges"] for c in r["cells"]) for r in results),
              "edgeWarningCells": contacts, "ready": sum(r["readyForSelection"] for r in results),
              "blocked": sum(not r["readyForSelection"] for r in results),
              "note": "Alpha 경계 접촉은 자동 검사 결과이며 잘림의 확정 판정이 아닙니다.", "entries": results}
    OUTPUT.write_text(json.dumps(report, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    # 검수용 Contact Sheet는 Temp에만 만들며 게임 Asset에는 편입하지 않습니다.
    temporary = ROOT / "Temp/AppearanceQA"
    temporary.mkdir(parents=True, exist_ok=True)
    for theme in sorted({e["theme"] for e in inventory["entries"]}):
        entries = [e for e in inventory["entries"] if e["theme"] == theme]
        canvas = Image.new("RGB", (2560, 1064), "#36404e")
        draw = ImageDraw.Draw(canvas)
        for i, entry in enumerate(entries):
            with Image.open(ROOT / "Unity/Client" / entry["assetPath"]) as sheet:
                canvas.paste(sheet, (i % 5 * 512, i // 5 * 532), sheet)
            draw.text((i % 5 * 512, i // 5 * 532 + 512), entry["jobTheme"] + " " + entry["gender"], fill="white")
        canvas.save(temporary / (theme + "Full.png"))
    print({k: v for k, v in report.items() if k != "entries"})


if __name__ == "__main__":
    run()
