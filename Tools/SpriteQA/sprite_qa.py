# coding: utf-8
"""원본 PNG를 읽기만 하는 Player Sprite QA. 시각 검수 근거 없이 READY를 내지 않는다."""
import argparse
import json
from pathlib import Path

import numpy as np
from PIL import Image
from scipy.ndimage import distance_transform_edt, find_objects, label

BLOCKING = {"head_crop", "body_crop", "foot_crop", "wheelchair_crop", "wheel_crop", "weapon_crop", "staff_crop", "cloak_crop", "visible_fragment",
            "neighbor_intrusion", "character_swap", "direction_error", "equipment_loss",
            "wheelchair_loss", "silhouette_damage"}


def analyze_frame(alpha):
    """Alpha>0의 8방향 성분과 실제 본체까지의 거리를 기록한다."""
    labels, count = label(alpha > 0, np.ones((3, 3)))
    if not count:
        return {"empty": True, "edge_crop_candidate": False, "components": []}
    sizes = np.bincount(labels.ravel()); sizes[0] = 0
    body = int(sizes.argmax())
    distance = distance_transform_edt(labels != body)
    components = []
    for index, region in enumerate(find_objects(labels), 1):
        if index == body:
            continue
        ys, xs = region; mask = labels[region] == index
        bbox = [xs.start, ys.start, xs.stop, ys.stop]
        maximum = int(alpha[region][mask].max()); pixels = int(mask.sum())
        large_or_long = pixels > 64 or max(xs.stop-xs.start, ys.stop-ys.start) > 24
        components.append({"maxAlpha": maximum, "pixelCount": pixels, "bbox": bbox,
                           "distanceToBody": float(distance[region][mask].min()),
                           "touchesCellEdge": xs.start == 0 or ys.start == 0 or xs.stop == alpha.shape[1] or ys.stop == alpha.shape[0],
                           "shapeReviewRequired": large_or_long,
                           "classification": "REVIEW_REQUIRED" if maximum > 16 or large_or_long else "LOW_ALPHA_NOISE"})
    visible = alpha >= 32
    # 셀 경계의 불투명 파츠는 Crop 후보이며, 셀 내부 절단은 시각 검수가 필요합니다.
    edge_crop = any(int(np.count_nonzero(edge)) >= 8 for edge in [visible[0], visible[-1], visible[:, 0], visible[:, -1]])
    return {"empty": False, "edge_crop_candidate": edge_crop, "components": components}


def analyze_sheet(path):
    image = Image.open(path)
    if image.mode != "RGBA" or image.size != (512, 512):
        raise ValueError("512x512 RGBA required; no conversion performed")
    alpha = np.asarray(image)[:, :, 3]
    frames = [analyze_frame(alpha[f//4*128:f//4*128+128, f%4*128:f%4*128+128]) for f in range(16)]
    # 동일 좌표의 반복은 자동 허용하지 않고 실제 파츠/선인지 별도로 확인합니다.
    for f, frame in enumerate(frames):
        frame["frame"] = f
        for component in frame["components"]:
            consecutive = 1
            for previous in range(f-1, -1, -1):
                if not any(c["bbox"] == component["bbox"] for c in frames[previous]["components"]):
                    break
                consecutive += 1
            component["repeatedPatternReviewRequired"] = consecutive >= 4
            if consecutive >= 4:
                component["classification"] = "REVIEW_REQUIRED"
    return frames


def decide(frames, review):
    """명확한 오류를 우선하고 낮은 Alpha 값만으로 PASS하지 않습니다."""
    if any(frame["empty"] for frame in frames) or BLOCKING.intersection(review.get("blocking_errors", [])):
        return "BLOCKED_ART"
    if review.get("blocking_errors"):
        return "REVIEW_REQUIRED"
    if any(frame["edge_crop_candidate"] for frame in frames):
        return "BLOCKED_ART" if review.get("edge_crop_confirmed") else "REVIEW_REQUIRED"
    if not review.get("all_frames_reviewed") or not review.get("silhouette_equipment_direction_preserved"):
        return "REVIEW_REQUIRED"
    candidates = [c for f in frames for c in f["components"] if c["classification"] == "REVIEW_REQUIRED"]
    if candidates and not review.get("review_candidates_confirmed_nonblocking"):
        return "REVIEW_REQUIRED"
    return "READY"


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("png"); parser.add_argument("--review", required=True)
    args = parser.parse_args()
    frames = analyze_sheet(Path(args.png)); review = json.loads(Path(args.review).read_text(encoding="utf-8"))
    print(json.dumps({"status": decide(frames, review), "frames": frames}, ensure_ascii=False, indent=2))


if __name__ == "__main__":
    main()
