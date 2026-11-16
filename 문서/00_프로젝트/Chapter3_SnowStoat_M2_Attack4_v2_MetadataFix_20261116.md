# LIMITLESS / Chapter3 M2 설원족제비 Attack4 v2 메타데이터 단독 정정 — 2026-11-16

**담당:** 메인 신규 아트 `docs/art-new`  
**이슈:** [#100](https://github.com/HappyPaul812/Project-Limitless/issues/100)  
**기술 근거:** [Codex 실제 로컬 검수](https://github.com/HappyPaul812/Project-Limitless/issues/100#issuecomment-6911980162) · [기획 표적 수정 계약](https://github.com/HappyPaul812/Project-Limitless/issues/100#issuecomment-6912055109)

**최종 판정:** `METADATA_ONLY_REPACK_COMPLETE / MANIFEST_9_OF_9_PASS / SUMS_10_OF_10_PASS / ZIP_CRC_PASS / ATTACK4_V2_PNG_BYTE_KEEP / USER_V2_ART_APPROVAL_PENDING / UNITY_NOT_APPLIED`.

## 1. 오류와 수정 범위

이전 `Stoat_Attack4_MouthFix.zip` (852,864 bytes, SHA256 `76e27b0c8d4fcfab09a11f857235755ce759a97572ae96bbb8641ea990388cb4`)의 ZIP CRC 및 실제 `SHA256SUMS.txt` 10/10은 PASS였으나 **Manifest 내 2개 자체/상호 참조 레코드가 실제 파일과 불일치**:
1. `Manifest.json` 자기 자신의 이전 크기 3347 bytes / 이전 해시를 기록했으나 최종 길이는 3644 bytes.
2. `SHA256SUMS.txt` 항목의 해시가 실제 SUMS 파일과 달랐음.

원인 코드를 추가로 단정하지 않고 **비순환 해시 의존관계**로 재작성:
- `Manifest.json.files`에는 **실물 payload 9개만** 크기와 SHA256을 기록. Manifest 자신 및 SHA256SUMS 자체를 목록에서 제외.
- `SHA256SUMS.txt`에는 **payload 9개 + 최종 확정된 Manifest 1개** 합계 10개를 기록. SHA256SUMS 자기 자신을 제외.
- 기존 `Docs/SourceDeclaration_and_QA.md`, Sprite 시트/개별2, Preview5의 압축 해제된 원본 **9/9 byte-exact KEEP**.
- 실제 변경 파일은 ZIP 내 **`Manifest.json`, `SHA256SUMS.txt`만 2개**. 다른 원본 그림·애니메이션·문서는 재작업하지 않음.

## 2. 최종 재패키징 실측

| 항목 | 최종 값 |
| --- | --- |
| ZIP | `SnowStoat_Attack4_MetadataFix_20261116.zip` |
| ZIP 크기 | **852,900 bytes** |
| ZIP SHA256 | `24ce0b771d4cf68e9758828fb892d9043b20eb51bd27bf826e07dedda0d60dc8` |
| ZIP entries | **11** = payload9 + Manifest + SUMS |
| Manifest | **3,653 bytes**, SHA256 `6de8db70af3c3839607dc3cc8bb96c37151186ac6665dbe62ae92a3004d46bff` |
| SHA256SUMS | **1,091 bytes**, SHA256 `aaa7ffcc6a52886a94d1b6424bccc5661c749b15a79ca1e024220a296fa2318c` |
| ZIP CRC | **PASS** (`zipfile.testzip() == None`) |
| Manifest 파일/크기/해시 | **9/9 PASS**, 참조 순환 없음 |
| SHA256SUMS 파일/해시 | **10/10 PASS**, SUMS 자기참조 없음 |
| 원본 payload | **9/9 바이트 동일** |

**Attack4 v2 실제 스프라이트 SHA256 재검증:**
- `Sprite/SnowStoat_M2_Attack4_MouthFix_v2_2048x512_RGBA.png` = **`5175483e486889736f4f89402ec8a04595491c5c587e51a3f6bbd1365391364a`**.
- 이전 Codex 로컬 SHA, 이전 ZIP 내 시트 SHA, 새 ZIP 내 시트 SHA **완전 동일**.
- 기존 **KeyPose** SHA `fd25ef50528a5f630e44382e3fc40a3df607a267176f758706962a2cbc41e778` 및 **Idle4** SHA `ee6951b0333ba6586b4009ccf51cfd3a397d69f839552fe88a709f9c34bf0633`는 보존된 실제 로컬 파일에서 독립적으로 재검증 PASS.

## 3. 산출물과 보호

- **제출 파일은 ZIP 1개 및 수정된 `Manifest.json`, `SHA256SUMS.txt`만.** 이번 메타데이터 정정 과정에서 새 이미지/프리뷰/게임용 시트 제작0. 원본 payload를 재사용했을 뿐임.
- 파일 위치(이번 ChatGPT 대화 산출물): `/mnt/data/SnowStoat_Attack4_MetadataFix_20261116.zip` · `/mnt/data/SnowStoat_Attack4_MetadataFix_20261116/Manifest.json` · `/mnt/data/SnowStoat_Attack4_MetadataFix_20261116/SHA256SUMS.txt`.
- 사용자 PC의 `F:\Downloads`를 자동 수정/복사하지 않았고, GitHub에 PNG/ZIP 바이너리 업로드했다고 주장하지 않음. 현재 GitHub에는 **기술 결과 Markdown만** 갱신.
- 코덱스 시각 판정은 **`PNG_KEEP_RECOMMENDED`**, 바울 대표의 **Attack4 v2 최종 미술 승인과는 다름**. 1포즈·Idle4 승인 원본 유지. 승인 전 Hit/Defeat 제작·Unity 적용 금지.
- 승인 서리들쥐·서리올빼미·환경·보스, Unity **6000.6.5f1**, Issue #85 HOLD, Save/UserData, Discord, 기존 Unity Scene/Prefab/GUID/meta 변경0. 새 프로젝트/Unity QA/원격 main 직접 commit/Push/Merge0.
