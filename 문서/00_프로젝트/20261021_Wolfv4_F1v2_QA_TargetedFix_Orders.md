# LIMITLESS — 2026-10-21 Wolf v4 / NorthF1 v2 QA: 결함만 국소 보완

기획 branch: docs/planning. 사용자 제공 Codex QA 캡처 기준. 격리 QA branch codex/art-review-wolfv4-northf1v2-20261021, 최종 commit 2471b1f2e98413dfaf0a705fc63cfdd7c8688fb5. 모든 비교 이미지는 오프라인 합성이고 Unity GameView가 아님.
두 ZIP 모두 SHA256/CRC/PNG 기술 PASS. 원본 Wolf v3와 F1 원본6 모두 보존, 사용자 원본 4,351파일/Git/Editor 보존. 아직 게임 적용/Push/main 병합 없음. 최종 미술 승인 별도.

## 1. art-renewal — Wolf v4 HOLD: v5 최소 보정
- 입력 F:\Downloads\Limitless_Wolf_KeyArt_v3_TargetedAlphaFix_v4.zip; ZIP SHA e241535f1c505bf50f4de09317e74dbc3780be722139ff0baa02c908223765c6.
- 입력 Wolf_KeyArt_v3_AlphaFix_v4.png; PNG SHA 62f72e97525f0266cbb6132da8b21571594f7c0deb3b1e1749cce749c9bf1c0a. v4 자체는 RGB 변경0, Alpha 감소12088px의 표적 보정.
- 미술 HOLD: 가슴 **위쪽** 회색 배경 조각, 다리 주변 미세 배경 잔여, **앞발 밑 바닥 조각**이 여전히 남음. 이전의 가슴 아래 매트 문제가 해결됐다 해도 통과 아님.
- 기존 v3 원본과 v4 모두 SHA 그대로 보존. 새 Wolf_KeyArt_v3_AlphaFix_v5.png (952×1306 RGBA)를 기존 v4 PNG를 직접 소스로 사용해 딱 세 부위 Alpha만 정리. 픽셀 RGB 채널/색/포즈/실루엣/회색 털·흰 털·발톱 변경 금지. Alpha 경계의 실제 털을 무작정 지우지 말 것.
- v4→v5 변경좌표 CSV와 2배 확대(가슴 위·다리·앞발 바닥), 실크기 128px 동일배율 전후비교, CRC/PNG SHA/Manifest/ZIP SHA+실물 납품 필수. Snake v3는 정적1포즈 Codex 미술 PASS **권고**, 사용자 최종 승인대기. Snake 원화 및 Wolf Run·Bear/Fox 등 모션 금지.

## 2. art-new — F1 6종 중 3종만 v3 수정
- 입력 F:\Downloads\Limitless_Chapter3_NorthF1_Environment_6_ArtImprovement_v2.zip; ZIP SHA 8158f501763b525022b8b9fc1645fe2b0a5c0bc8bf9f73aabcb02024c3a3235c.
- 이미 미술 PASS한 아래 3종 **동일 바이트 유지**: 흙 지면 SHA ccf2c22e90bee292ecf2d0d030dad53e0032457272ebb94fccdf6491fabcf8b9; 흙길 SHA e78b41d48707dfce6a79dd4e826f864c5c2608b45a0f457af414ee9768a20175; 서리 관목 SHA 6c83c88881e7f89245ed580d50424b2671629ad9491da93f247aab99bfe4746e.
- 수정 1: 얇은 서리 HOLD — 3×3에서 밝은 서리 패치가 반복해서 보임. 흙 우세를 유지하면서 밝은 서리 모티프 반복·표면 재질과 테두리 패턴 완화, seamless edge 보호.
- 수정 2: 흙↔서리 전환부 FAIL — 진한 흰 서리가 다음 타일에서 갑자기 끊김. 1방향 구조를 유지하며 알파·밀도·형상과 토양/서리 재질 경사가 **옆 실제 타일로 자연스럽게 연결**되도록 처리. 검은/불투명 덧판으로 결함 감추기 금지.
- 수정 3: 길 표지판 HOLD — 양끝이 대칭적으로 보여 방향 지시가 불분명. 한쪽 화살촉/꼬리 대비·표면 양각/외곽 명암으로 실제 방향을 분명하게 표시, 존재하지 않는 퀘스트 목적지나 텍스트 임의 기입 금지.
- 256×256 RGBA 유지, 원본 Pilot6 및 v2 후보6 무수정. 새 개선 3 PNG만 v3 이름으로 제작하고 PASS v2 3종 동일SHA와 합쳐 **F1 6종 통합 QA ZIP v3** 제출. 2×2/3×3 반복, 우측 전환의 전/후 연속 타일, 128px 길/방향 식별, Chapter2 원본과 질감 비교. 오프라인 렌더=Unity GameView 주장 금지.
- 기술 ZIP/PNG PASS는 기존 결과이고 F1 전체 미술은 3 PASS/2 HOLD/1 FAIL이므로 HOLD. PPU/TileGrid/Collider/휠체어 실제 플레이 미확인. 통합 QA 뒤 사용자 미술 최종 승인 필요.

## 3. 다음 순서/안전
- 각 담당은 본인 docs/art-renewal 또는 docs/art-new 브랜치에 문서화. 실제 PNG와 ZIP 납품·SHA/Manifest·수정 범위/후속 승인 명시. main 직접 커밋/Push/병합 금지.
- 다음 실물 ZIP 두 개가 나온 뒤 Codex가 **Wolf 잔여3 및 F1 수정3 + KEEP3 동일 SHA**만 격리 Delta 검증, 128px 미술 비교. 이전 수십~수백 QA 전체 재실행 금지.
- Snake v3 사용자 최종 미술승인 대기, F1 Main22 Unity 게임 구현은 F1 미술 통과 이후. 승인된 서리들쥐 Combat/Idle/보스 설빙의 폭군 Idle, Field09 B 파생 Glow, 다른 Chapter1·2 공식 자산, Save/UserData/원본 Editor 보호.
- 내부 자산 누락 추적(신규 스킬·강화 아이콘/한기 소모템·따끈 어묵탕/몬스터 잡템 아이콘)은 다음 별도 카탈로그 확정/일정에서 계속 관리하되 지금 미술 수정 범위에 섞지 않음.
