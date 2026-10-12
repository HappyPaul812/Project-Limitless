# Field03 Rock B 기술 보완 요청 — 2026-10-12

미술 사용자 승인은 USER_ART_APPROVED입니다. 아래 문제 때문에 Rock B 한 종류만 TECH_HOLD로 기존 Sprite를 유지합니다. 다른11종/375참조는 정식 연결하며 원본PNG는 수정하지 않습니다.

- 대상: `Props/LL_C1_F03_Rock_Moss_B_128_v1.png` (원본128×128, PPU128/중앙 Pivot)
- 원본 alpha 상단에 본체와 분리된14픽셀: bbox(64,0)~(70,3). 의도 없는 잔여픽셀이면 제작 원본에서 제거해 주세요.
- 본체3537px bbox(12,69)~(116,125): 중앙보다 낮습니다. Scene RockNorth(4.5,4.4), Collider size(.4,.22)/offset(0,-.22)는 보호 대상입니다.
- 기존 Collider 하단Y4.07에 비해 신규 본체 하단Y3.92344로0.14656world 낮습니다. 기존85×54 Slice/PPU100에서 보이던 접지 관계를 유지하도록 **PNG 내부 위치/접지**를 제작 담당이 보완해 주세요. Scene 좌표·Collider 임의 변경은 하지 않았습니다.
- 수정 납품은 별도버전/Manifest/hash로 제공하고, 원본ZIP/v1PNG는 보존해 주세요. 이후 격리 확대검사와 이1개 참조만 선택적 연결합니다.

[원본 alpha 확대](../Field03Renewal_v1_20261012/rock_b_alpha_artifact_zoom.png), [실제 Unity 비교](rock_b_decision_comparison.png). Overlay/설명은 검수자료이며 게임 자산 수정이 아닙니다.
