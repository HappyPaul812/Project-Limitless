# Player Sprite QA 정책

LOCAL 프로젝트와 사용자 PNG가 정본이다. 정책 적용 순서는 문서화→구현→검증이며 PNG 재저장/수정 없이 판정한다.

## 차단 오류

`BLOCKING_ART_ERROR`는 머리/발/바퀴/무기/장비 Crop, 가시적인 분리 조각, 인접 셀 침범, Character Swap, 방향 오류, Empty Frame, 장비/휠체어 손실, 실제 실루엣을 훼손하는 Alpha 파츠다. 하나라도 확인되면 `BLOCKED_ART`다. 머리 절단처럼 투명 여백 안에서 발생하는 오류는 픽셀 경계 검사만으로 검출할 수 없으므로 전체 프레임 시각 검수가 필요하다.

## 비차단 경고

`LOW_ALPHA_NOISE / NON_BLOCKING_WARNING`은 최대Alpha1–16의 작은 고립 성분 중 육안으로 사실상 보이지 않고 실루엣/방향/장비/휠체어/인접 셀에 영향을 주지 않는 경우다. 단순 non-zero Alpha만으로 BLOCKED하지 않는다. 경고의 단위는 Alpha>0의 8방향 연결성분 하나다.

Alpha<=16만으로 자동 PASS하지 않는다. 큰 면적, 긴 선, 반복 패턴, 실제 파츠 형태, 셀 침범은 별도로 시각 판단한다. Helper의 보수적인 검토 후보 기준은 64px 초과 면적, bbox 가로/세로24px 초과, 동일 bbox가 연속4Frame 이상 반복이다. 기준 이하라도 가시적 오류이면 차단한다. 후보 기준 초과는 자동 미술 실패가 아니라 `REVIEW_REQUIRED`이며 검수 근거를 남겨 판정한다.

Helper는 MaxAlpha/PixelCount/BoundingBox/본체까지최소유클리드거리/경계접촉/반복 후보를 기록한다. 본체는 Alpha>0 연결성분 중 가장 큰 성분이다. 전체 시각 검수 완료와 오류 없음의 명시적인 근거 없이 READY를 반환하지 않는다. 정상 AA 윤곽과 별개 고립 노이즈를 구분하며 명확한 Crop/분리조각/침범/손실/방향 오류 검수를 대체하지 않는다.

## 이번 적용 계획

기존41READY는 재검사/재판정하지 않는다. Mobility v2 redownload9종만 최신144Frame 시각검수 근거와 현재PNG 연결성분을 재평가한다. HealerFemale READY/모든50PNG/meta/import/StableID/Save/fallback는 유지한다. 이미 수행한85PASS Runtime을 반복하지 않고 READY 전환에 필요한 기존Entry의8Clip 연결·프레임/Mapping/Preview/World/Battle/Save 정본 전달만 최소 회귀 확인한다. PNG 수정/Stage 및 GitHubPush 금지.
