# LIMITLESS — 2026-10-21 Main22 적용 준비 완료 / Wolf Run6 후보 미술 FAIL / Main23 병렬 작업중

기획 브랜치 docs/planning. 바울 대표 제공 Codex 보고서 두 장과 “둘 다 시켰어.” 사용자 발언을 근거로 실제 진행 상태를 분리 관리한다. “둘 다”는 직전 **Main23 서릿길 마을 환경 art-new 발주 + Codex Main23 Village 기술 사전점검**을 이미 전달했다는 뜻. **두 작업 재발주 금지**.

## A. Main22 원본 반영 준비 — READY, 실제 원본/merge는 미실행
- Branch: `codex/chapter3-main22-apply-preparation-20261021`, commit `2631e8750df8d98fb298754f3c82c5ca651c2fda` (사용자 공유 검수).
- 총 **583개 파일**(기존 13 + 신규 40 + 기준 수정 5로 화면 기록; 단순 합과 차이가 나므로 중복/합산 범주 실파일 Allowlist 우선)의 기존/추가/영향을 조사, 적용예상 Scene/Asset 17충돌 여지 확인, 실제 **원본 고유 설정 보존·Scene 항목만 추가**하도록 안전 병합 계약 제시.
- 기존 격리 Main22 136PASS/F1 64PASS/MouseBattle 40PASS는 **재검사하지 않고 사용**. 정식 Script/TTS·밸런스·퀘스트 보상은 임시 상태. `MAIN22_APPLY_PLAN_READY / ORIGINAL_LOCAL_NOT_APPLIED / MAIN_NOT_MERGED`; Main23 Quest/Town을 이 PR에 섞지 않는다.
- 사용자 원본 Unity/Save/UserData/Editor, 4,351파일 보호. main 직접 commit/Push/merge0.

## B. Wolf Run6 후보 — 기술 PASS, 미술 FAIL
- Branch `codex/wolf-run6-candidate-qa-20261021`, 최종 commit `c47d7ba17c6332b28379b7c2ae315ed1ad30ec1e`.
- ZIP `Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtPilot_v2_CANDIDATE.zip`, SHA256 `c69ca17e8ebe2b8591c4dc77ebc6531b60dbe8a44ee3475d39d4cc6159f8f2ad`; 29파일 CRC/Manifest SHA/PNG6·Strip·Atlas·GIF/APNG·128/40 Preview 파일기술 PASS. 비교 자료는 오프라인이며 **실제 Unity Import/실시간 모션 검증 미실행**.
- 실제 Codex 미술 **FAIL**: Run frame **03,04 꼬리 윤곽 절단**, 중간 프레임의 몸체 비율·발 접지 연결이 일관되지 않고 05→00 루프 및 실제 달리는 동작을 최종 미술 승인할 수 없음. 이 결과는 기존 Composite 포즈 추출 시 늑대들이 겹쳤던 소스의 한계와도 정합. **형식 PASS를 Unity production-ready로 표기 금지**.
- **정적 Wolf v5 승인 원화는 바이트 불변** SHA256 `b770c73c36b9035a2bb0e70b8c126090b5fa248d27db542af077094d12ee2ea1`. 이번 Run6 candidate는 별도 미술 재수정 범위. v5 원화를 다른 포즈로 임의 덮어쓰지 않는다.
- 리뉴얼 `docs/art-renewal`에 **Run6 모션만 개선** 발주: 현재 여섯 포즈 전체 재작업 대신 적어도 프레임03·04 꼬리 형태를 온전하게 그려 복원하고, 다른 프레임과 함께 몸통 길이/늑대 가슴/눈·털·다리 관절/발 접지/프레임05→00 loop의 통일성 확보. 심한 포즈 겹침·꼬리 분리 한계를 '투명 Alpha만 패치'해 해결할 수 없다면 **각 포즈를 새로 그려 별도 소스6 PNG로 확실히 분리한 후** 시트 패킹 (타겟이 승인 v5 계열 오리지널 성견 늑대이되 동일 소스 그림을 왜곡한 단순 가로 이동/리깅 사칭 금지).
- 6 frames LEFT / 12FPS / 각 source 현재 512×384 RGBA, Strip 3072×384 + 3×2 Atlas 1536×768 / 128px·40px Preview 기술 후보를 유지하되 Codex 최종 PPU/WorldUnit/Actor Scale 확인 전 정식 runtime 64×40 강제축소 금지. Frame별 alpha bbox/foot anchor, clip0, 털 디테일, 몸통 유효길이/귀·꼬리 속도·contact 순서, loop 안정성/개별 Source PNG, strip/atlas GIF/APNG·Manifest·ZIP/PNG SHA/CRC와 **동일배율 전후 모션 비교** 보고. **사용자 Wolf Run 미술 승인/실제 Unity Import/기존 Wolf 교체는 아직 없음**.
- Bear/Fox/뱀 모션 및 Chapter3 설빙 보스, main 직접 push/merge0.

## C. 이미 진행 중인 Main23 별개 선행 업무
- 바울 대표는 직전 Main23 작업 2건을 모두 각 담당에 보냈다고 확인. (1) `docs/art-new` 서릿길 마을 신규 환경 PNG Pilot v2, (2) Codex Main23 Village Scene/Quest SafeZone/포탈OFF **읽기전용 기술 preflight**. 기획 [Issue #45](https://github.com/HappyPaul812/Project-Limitless/issues/45) 및 `20261021_Chapter3_Main23_Frostway_Village_Art_And_Tech_Parallel_Kickoff.md` 확인. 동일 발주 재전달/겹친 codex branch 생성 금지.
- 다음 완료 응답을 받아 Village 미술/기술 계약 후 격리 실제 Main23 구현; **Main22 본편 동기화와 제품 merge는 별도 사용자 명시 승인 시점에만**.
- 아이콘·스킬/소모템·잡템 누락 목록은 기획 내부대장 유지, 북부 마을/새 스킬 ID·가격/공식 TTS 임의 확정금지.
