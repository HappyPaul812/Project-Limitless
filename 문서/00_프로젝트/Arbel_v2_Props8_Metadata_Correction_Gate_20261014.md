# LIMITLESS — Arbel v2 QA 후속: 소품 원본 크기 8종 정정 (2026-10-14)

**현재: NEEDS_MINOR_FIX / META_8_CORRECTIONS_PENDING / UNITY_NOT_APPLIED / USER_ART_APPROVAL_PENDING.**

바울 대표가 Codex 읽기전용 재검수 결과를 전달했다. 기획 담당이 사용자의 로컬 F: ZIP을 직접 열거나 Unity를 실행했다는 뜻은 아니다.

## 합격 및 보존
- 검사대상: F:/Downloads/Limitless_Arbel_Renewal_Art_v2.zip
- SHA256: 17625d35c93599bdd74a4b8280c0416240c402be279815c8966199410bca97b7 (Codex 검사 보고와 정확 일치).
- 총 55파일·PNG43(실사용34, Preview8, QA1), CRC·decode·Manifest·해상도 PASS.
- v1 대비 PNG31 동일, 도로2+지붕1만 변경. 도로 좌우/상하 RGB·Alpha mismatch 0, 북서 지붕 십자205px 제거/Alpha 보존.
- Arbel SpriteRenderer 284/284 이름·source GUID/local fileID·PPU/Pivot/Sorting 정상.

## 아직 남은 정확한 문제
- 소품 9종 중 8종의 **원본 Single Sprite.rect 크기 기록**이 부정확함. 기존 복합/시트 크기 또는 fallback 값 혼용. 9개 PNG를 모두 다시 그리거나 크기 변경하는 문제 아님.
- 사례: Medium Tree 잘못된 125×146 → 실제 등록 128×156; Barrel 잘못된 64×82 → 실제 등록 128×128. 나머지 6종은 **Codex가 작성한 정정 근거 CSV의 실제 값만** 사용; 임의 숫자 금지.
- 원본 Sprite.rect 크기, Texture canvas, visible Alpha foot/bbox, PPU, Pivot 및 실제 world grounding은 별개 열로 관리. ZIP 문서 수정으로 Unity 접지 PASS를 주장하지 않는다.

## 적용 승인 경계
- 교체 후보23종 중 바닥·도로·벽12종 기술 후보 PASS, 지붕2종은 NPC/출입구/카메라 가림 HOLD, 소품9종은 8개 기록 정정 및 원본 실제 Alpha foot·Unity 접지 비교 HOLD.
- 선택 장식11종은 별도 사용자 승인 전 미배치. PortalAnchor(-7,-4)와 TreeSouthWest(-7,-4.2) 겹침/선택 Portal 장식 HOLD.
- 원본 Unity Scene/Save/Editor·Play/포커스/GUID/meta·Field04/승인Main08 음성·BGM/원본 PNG·기존 회복 자산 수정 금지.

## Codex 작업 지시(기술 메타데이터만)
1. 기존 읽기전용 감사의 정정 근거 CSV에서 8종의 원래 등록 Single Sprite 크기를 검증해 9종 대응표의 원본 Rect 필드만 바로잡는다. 잘못된 fallback이 남은 Manifest/README/QA 기록/매핑 주석도 필요 범위만 정정한다.
2. **v1/v2 ZIP 원본 byte와 SHA 그대로 보존**. 새 파일 Limitless_Arbel_Renewal_Art_v3.zip(문서정정판)으로 따로 저장하되 **실사용 PNG34종은 각각 v2와 바이트·SHA 완전 동일**해야 한다. 이미지 보정/리사이즈 금지.
3. v3 새 ZIP SHA, 문서 변경분, 8종 Before/After rect 표와 원래 Codex 근거 CSV의 대응 열·근거를 보고한다. 284/284 매핑/Seam 0/북서 십자 제거/모든 PNG/CRC·Manifest 해시·GUID 메타 검사를 다시 통과시킨다.
4. 원본 Alpha foot의 실제 접지, 두 지붕의 가림, Portal 충돌은 기술 미해결 HOLD로 분리하며 PNG 수정이나 Unity 원본 적용을 하지 않는다.
5. 리뉴얼 담당 docs/art-renewal가 납품본과 인계 문서를 갱신하도록 결과를 넘긴다. 사용자 미술 승인 뒤에만 Codex의 별도 worktree Unity 적용/Runtime QA와 작업 브랜치 Push가 가능하다. main 직접 commit/Push·병합 금지.

최종 상태는 **NOT YET ART APPROVED / v2 NEEDS_MINOR_FIX / v3 TARGETED_METADATA_FIX_REQUESTED** 이다.
## 2026-10-14 Codex v3 문서 정정 완료 보고·리뉴얼 담당 확인 지시

바울 대표가 **Codex의 v3 완료 보고**를 전달함. 실제 Arbel 리뉴얼 담당 `docs/art-renewal`의 `WORLD_ART_RENEWAL_LATEST.md`를 기획 담당이 조회한 결과 **v2 납품까지만 기록되어 있어 v3 검토/접수 여부는 확인되지 않음**. 따라서 `Codex가 리뉴얼 담당 채팅에 인계했다고 보고`한 사실을 `리뉴얼 담당이 읽고 승인/회신 완료`로 간주하지 않는다.

- 새 로컬 ZIP: `F:/Downloads/Limitless_Arbel_Renewal_Art_v3.zip`, SHA256 `84e9707df7f0409e0238781f0860499dd4344b79ec88b49964a7ce7d8e10633b` (Codex 보고 기준).
- 원본 Sprite.rect 오기 8종 정정: Barrel64×82→128×128, Bush Dry99×76→128×128, Bush Olive100×84→128×128, Campfire88×75→128×128, Chest84×73→128×128, DryMossRock102×62→128×128, Big Tree205×247→255×256, Medium Tree125×146→128×156; Fence 128×128 유지.
- QA 보고: 실사용 PNG34/34 v2와 바이트/SHA 동일, Renderer284/284, 도로 seam0, 북서 십자 수정, ZIP CRC/Manifest 및 크기 오기0, 독립 검사2455·패키지191 PASS. Unity 원본 Scene·Play·Push·main 병합0, 원본 5221파일 SHA·v1/v2 ZIP 보호.
- 미해결 HOLD: 지붕 가림, 소품 실제 Unity 접지, 선택 장식11, Portal↔나무 겹침. 미술 최종 승인·Unity 통합/Runtime 승인 아님.

**리뉴얼 담당에게 명시적으로 요청할 것:** `docs/planning`의 이 문서와 Codex v3 완료 보고를 **직접 열어 읽고**, 자신이 가진 v2 납품 정본과 대조하여 v3 문서 8종의 정확성·PNG34 동일 바이트·사용 가능한 Preview/Manifest/284 대응·HOLD를 **확인했다고 회신**. 접수 후 `docs/art-renewal`의 `Arbel_Renewal_Art_v2_MinorFix_Delivery_20261014.md`에 v3 후속 이력(원본 v2를 덮지 않음)을 연결하고 `WORLD_ART_RENEWAL_LATEST.md`를 v3 현재 상태로 갱신. 담당 브랜치 커밋 SHA, 미술 검토 여부, 바울 대표의 승인 필요 항목을 보고.

리뉴얼 담당은 로컬 F: ZIP에 직접 접근할 수 없을 경우 **접근 가능한 GitHub QA·Codex 보고만으로 확인한 범위**를 명확히 밝히고, 파일 자체를 열어보았다고 주장하지 않는다. 필요하면 Codex에게 스크린샷/프리뷰/비교 증거를 요청한다. 본 시점의 사용자 지시에는 Unity 적용·Push 승인이 포함되지 않는다.

**상태:** `CODEX_V3_METADATA_QA_REPORTED_PASS / ART_LEAD_EXPLICIT_REVIEW_PENDING / USER_ART_APPROVAL_PENDING / UNITY_NOT_APPLIED`.