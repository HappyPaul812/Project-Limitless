# coding: utf-8
"""합성 Alpha 배열과 시각 검수 근거로 실제 오류 통과와 노이즈 오탐을 방지합니다."""
import unittest
import numpy as np
from sprite_qa import analyze_frame, analyze_sheet, decide
from unittest.mock import patch
from PIL import Image

REVIEW = {"all_frames_reviewed": True, "silhouette_equipment_direction_preserved": True, "blocking_errors": []}


class SpriteQaTests(unittest.TestCase):
    def frame(self):
        alpha = np.zeros((128, 128), dtype=np.uint8); alpha[30:100, 40:90] = 255
        return alpha

    def test_head_crop(self):
        alpha = self.frame(); alpha[:30, 40:90] = 255
        self.assertEqual(decide([analyze_frame(alpha)], dict(REVIEW, edge_crop_confirmed=True)), "BLOCKED_ART")

    def test_crop_inside_transparent_margin(self):
        self.assertEqual(decide([analyze_frame(self.frame())], dict(REVIEW, blocking_errors=["head_crop"])), "BLOCKED_ART")

    def test_visible_fragment(self):
        alpha = self.frame(); alpha[110:115, 10:15] = 255
        frame = analyze_frame(alpha)
        self.assertEqual(frame["components"][0]["classification"], "REVIEW_REQUIRED")
        self.assertEqual(decide([frame], dict(REVIEW, blocking_errors=["visible_fragment"])), "BLOCKED_ART")

    def test_neighbor_equipment(self):
        alpha = self.frame(); alpha[110:120, 0:4] = 200
        self.assertTrue(analyze_frame(alpha)["components"][0]["touchesCellEdge"])
        self.assertEqual(decide([analyze_frame(alpha)], dict(REVIEW, blocking_errors=["neighbor_intrusion"])), "BLOCKED_ART")

    def test_small_noise_all_alpha_1_to_16(self):
        for value in range(1, 17):
            alpha = self.frame(); alpha[110, 10] = value
            frame = analyze_frame(alpha); component = frame["components"][0]
            self.assertEqual(component["classification"], "LOW_ALPHA_NOISE")
            self.assertEqual(component["pixelCount"], 1); self.assertGreater(component["distanceToBody"], 0)
            self.assertEqual(decide([frame], REVIEW), "READY")

    def test_normal(self):
        self.assertEqual(decide([analyze_frame(self.frame())], REVIEW), "READY")

    def test_no_visual_review_no_auto_pass(self):
        self.assertEqual(decide([analyze_frame(self.frame())], {}), "REVIEW_REQUIRED")

    def test_low_alpha_large_area_and_long_line_require_review(self):
        for region in [(slice(105, 115), slice(5, 15)), (slice(110, 111), slice(1, 31))]:
            alpha = self.frame(); alpha[region] = 16
            self.assertEqual(decide([analyze_frame(alpha)], REVIEW), "REVIEW_REQUIRED")

    def test_low_alpha_visible_damage_still_blocks(self):
        alpha = self.frame(); alpha[110, 10] = 16
        self.assertEqual(decide([analyze_frame(alpha)], dict(REVIEW, blocking_errors=["silhouette_damage"])), "BLOCKED_ART")

    def test_empty(self):
        self.assertEqual(decide([analyze_frame(np.zeros((128, 128), dtype=np.uint8))], REVIEW), "BLOCKED_ART")

    def test_repeated_low_alpha_pattern_requires_review(self):
        rgba = np.zeros((512, 512, 4), dtype=np.uint8)
        for f in range(16):
            rgba[f//4*128:f//4*128+128, f%4*128:f%4*128+128, 3] = self.frame()
            rgba[f//4*128+110, f%4*128+10, 3] = 8
        with patch("sprite_qa.Image.open", return_value=Image.fromarray(rgba)):
            frames = analyze_sheet("synthetic-memory-only")
        self.assertTrue(frames[3]["components"][0]["repeatedPatternReviewRequired"])
        self.assertEqual(decide(frames, REVIEW), "REVIEW_REQUIRED")


if __name__ == "__main__":
    unittest.main()
