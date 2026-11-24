# 2026-11-24 / LIMITLESS 리뉴얼 / Issue #124 수호자 스킬3종 표적 보완 v2

- [Codex 검수](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7127093770), 로컬 보고 0c97c18dc11f763f1e4239fd4e28419b3c002dd5 확인. v1 파일·Unity 표시·스킬/상태 공유 계약 PASS 유지. 철벽/기본 방어 작은 HUD 구분 및 맹세 내부 아군 관계 PARTIAL만 보완. 기존 PASS 재실행0.
- 철벽 v2: 기존 금속/금빛/청색 화풍과 요새+방패 구성을 유지하면서 요새의 비중·흉벽·벽돌을 강조하고 중앙 방패 축소. 생성된 표적 구성 수정본을 128 RGBA로 정규화한 것으로, 변경이 픽셀 국소에만 한정되었다고 주장하지 않음. 기존 v1 바이트 보존.
- 맹세 v2: v1 자체의 좌우 아군 주변 RGB 대비만 국소 보완. 1835 픽셀 변경, 전체 변경영역 bbox [15,71,113,110] (exclusive), 전체 alpha 일치. 새 생성 시안으로 원본 전체를 교체하지 않음. 작은 HUD 내부 관계 개선 정도는 추가 미술 평가 필요.
- 도발 v1 KEEP: SHA256 960bd0a3b87eadae637a1e2243eb3eab848b28963651ba328b01e699dba5687b. 기존 v1 3종 및 승인 기본 명령 공격/방어/스킬 참조 원본 SHA 일치.
- Art/Guardian_IronWall_v2_128_RGBA.png SHA256 6bebeac7b03c6906b6f23adb822f6a05531c3fe49ea48b8452d44cf76ae5f08f; Art/Guardian_CoverAllies_v2_128_RGBA.png SHA256 3b66adf62bff00d86056f29564e7a885d0987b58812624c4bb8de03ba6e49977; Art/Guardian_Taunt_v1_KEEP_128_RGBA.png SHA256 960bd0a3b87eadae637a1e2243eb3eab848b28963651ba328b01e699dba5687b. 모두 128x128 RGBA, alpha 0..255, 네 모서리 alpha0.
- Review/IronWall_vs_Defend_128_40_30_24_20.png, Guardian_v1_v2_128_40_30_24_20.png, HUD_20_24px_4x_Zoom.png 및 실제 축소 개별 PNG. 오프라인 비교에서 요새 우선 실루엣 확인. v2 실제 Unity HUD 검수 NOT_VERIFIED.
- ZIP LIMITLESS_Issue124_GuardianSkill3_v2_Targeted_20261124.zip, 2204204 bytes, SHA256 9abaa07de47ee869f35570a7647aad70189eb660400a77c5332acdbb0d9dfb18; ZIP CRC41/41, Manifest39/39 SHA+size, SUMS40/40 PASS. 대화 다운로드 파일, Windows F: 자동 복사 및 GitHub 바이너리 업로드 아님.
- 기존 승인 자산, 기존 skill/status 공유/기능/수치, 코드/GUID/meta/Scene/Prefab/Save/UserData/#85/#131/Discord/main 변경0. Unity 실행·적용, main 병합/Push, Codex 검수 요청0.
- 상태: TAUNT_V1_KEEP / IRON_WALL_V2_TARGETED / COVER_ALLIES_V2_LOCAL_CONTRAST / FILE_QA_PASS / USER_ART_APPROVAL_PENDING / V2_GAMEVIEW_NOT_VERIFIED / NOT_UNITY_APPLIED.
