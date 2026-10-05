# Player Sprite QA helper

기존 `Temp/MobilityV2NineAudit/audit.py`의 Alpha>0/8방향 연결성분 분석을 재사용 가능한 도구로 정리했다. Game Runtime/Unity C#와 독립적인 QA 전용 Python 도구이며 PNG를 읽기만 한다. Python의 Pillow/NumPy/SciPy를 사용한다.

회귀 실행:

```powershell
python -m unittest discover -s Tools/SpriteQA -p 'test_*.py' -v
```

단일 원본 검사:

```powershell
python Tools/SpriteQA/sprite_qa.py ORIGINAL.png --review visual-review.json
```

review JSON 예시:

```json
{
  "all_frames_reviewed": true,
  "silhouette_equipment_direction_preserved": true,
  "blocking_errors": [],
  "review_candidates_confirmed_nonblocking": false,
  "evidence": "16프레임 머리/바퀴/장비/실루엣/방향 시각검수 근거"
}
```

실제 오류는 `blocking_errors`에 `head_crop`, `visible_fragment`, `neighbor_intrusion`, `wheelchair_loss` 등의 근거를 전달한다. Helper가 Crop/Character Swap/방향을 의미론적으로 자동 검출한다고 가정하지 않는다. 불투명 셀 경계, 빈 프레임, 고립성분 크기/길이/반복은 자동 후보 검출이며 셀 내부 머리절단 및 장비/방향/실루엣은 시각검수가 담당한다. 시각검수 없는 결과는 REVIEW_REQUIRED이고 낮은Alpha만으로 READY가 되지 않는다. 후보를 비차단으로 확인할 때에는 이유를 evidence에 기록한다. 알 수 없는 blocking_errors도 자동 PASS하지 않는다.

정식 기준은 `문서/11_UI/Player_Sprite_QA_Policy.md`, Mobility 정책 재판정 및 성분별거리/좌표는 `문서/11_UI/Mobility_LowAlphaPolicy_QA.json`을 따른다.
