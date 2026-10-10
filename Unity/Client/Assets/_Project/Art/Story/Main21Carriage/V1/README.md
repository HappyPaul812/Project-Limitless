# Main21 접근 가능한 귀환 마차 v1 — 독립 아트 등록

원본 PNG 바이트 보존. 열린/닫힌 마차만 Single Sprite, 말/부품 시트는 미분할 참고 Texture다. Scene/Prefab/Controller/Quest에 연결하지 않았다.

PPU128은 현 프로젝트 기준의 **등록용 값**이며 최종 월드 크기/탑승 치수/정차 Pivot/Anchor 계약이 아니다. 1254px 캔버스는 9.796875 world unit이므로 무보정 월드 배치를 하지 않는다. Point, Clamp, Mipmap Off, NPOT None, 무압축, MaxSize2048. 중앙 Pivot은 임시 등록 기준이며 열린/닫힌 상태 교환 시 차체가 자동 정렬된다고 가정하지 않는다.

말 3등분 자동 Slice 금지: x835/836 부근에 비투명 픽셀이 있어 418px 균등 분할은 포즈 일부를 자른다. 정식 프레임 Rect·공통 접지점·재생 순서/속도·견인 Anchor를 후속 확정한다. Parts Sheet는 참조 모음이며 실제 분리 레이어/Animation이 아니다.

휠체어 2대와 벤치가 그려졌지만 탑승5명/휠체어 실측 수용은 USER_ART_REVIEW_PENDING. 빈 휠체어가 본체에 그려져 있어 기존 폴/플레이어 휠체어 Sprite를 중복 합성하지 않도록 후속 레이어 계약을 정한다. 휠체어를 제거하여 강제 보행시키지 않는다.

검수 정본: 문서/00_프로젝트/Main21_Accessible_Wagon_Art_v1_Inspection_20261010.md.
