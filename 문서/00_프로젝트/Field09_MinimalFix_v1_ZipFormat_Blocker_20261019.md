# Field09 Minimal Fix v1 — 표준 ZIP 형식 불합격·Unity 적용 중단 (2026-10-19)

**ZIP_SHA_MATCH / ZIP_FORMAT_FAIL / ORIGINAL8_SHA_MATCH / FORENSIC_PAYLOAD13_CRC_MATCH / FIXED8_MANIFEST_SHA_MATCH / UNITY_QA_NOT_RUN / ORIGINAL_APPLY_BLOCKED**.

독립 브랜치 `codex/field09-minimal-fix-unity-integration-20261019`, main `675988c9f2c5c507d9a696b0378be27c0bd3309b` 기반. 최신 기획 `Field09_MinimalFix_v1_IsolatedQA_And_OfficialApply_Authorized_20261019.md`와 납품 `Field09_ObsidianScar_OfficialArt_MinimalFix_v1_Delivery_20261019.md` 확인. Main10/Field06/07/몬스터·늑대 작업에 변경0.

## 실물 결과

- 로컬 `F:/Downloads/Limitless_Field09_ObsidianScar_Minimal_Fix_v1.zip`, 크기 **2,995,422 bytes**. SHA256 **`7dfa2417a8135e29e3e72c7d7921d3ade76fbedfdb376f764c09b02892296139`**로 납품값과 일치합니다.
- GitHub `origin/docs/art-renewal:Art_Deliveries/Limitless_Field09_ObsidianScar_Minimal_Fix_v1.zip` Git blob내용과 로컬은 **바이트 동일**합니다. 로컬 다운로드 손상/누락이 아니라 납품된 컨테이너 자체의 형식 문제입니다.
- Python zipfile: `BadZipFile: File is not a zip file`. Windows .NET ZipFile: `End of Central Directory record could not be found.` **표준 ZIP 파서2종 모두 실패**하므로 납품문서의 ZIP CRC PASS 기록과 실물 결과가 일치하지 않습니다.

| 구조 | 정상 ZIP 바이트 | 납품 실제 바이트 |
|---|---|---|
| Local file header | 50 4B 03 04 | 04 03 4B 50 |
| Central directory | 50 4B 01 02 | 02 01 4B 50 |
| End of central directory | 50 4B 05 06 | 06 05 4B 50 |

진단상 **32비트 서명/CRC/길이/offset은 Big Endian, 16비트 필드는 Little Endian**으로 섞였습니다. 표준 ZIP의32비트 필드는 Little Endian이어야 합니다. 파일명을 바꾸거나 다시 다운로드해서 해결되는 문제가 아닙니다.

## 내부 데이터 읽기 전용 확인 — Unity 적용 허가로 확대하지 않음

원인 확인에 한해 혼합 바이트 순서를 읽어 메모리에서13개 payload를 분리했습니다. payload13 CRC32 일치, PNG8은 전부512×512 RGBA decode, 내부Manifest의 fixedSHA8 전부 일치. 원본Source8 SHA256와Git blob도 현재공식PNG와 일치하고 metaSHA를 기록했습니다. 따라서 그림 데이터 손상보다 **ZIP 포장 코드 오류**로 좁혀집니다. 이 결과는 표준 ZIP CRC 검수 PASS가 아니며 형식 불합격을 숨기지 않습니다. 후보PNG 파일을 추출·재포장하거나 Unity Asset으로 추가하지 않았습니다.

## 재납품에 필요한 최소 수정

1. **기존 payload13개(그림8·문서/Manifest5)를 바이트 그대로 유지**하고 일반 Python zipfile 또는 표준 ZIP 도구로 재포장합니다. PNG를 재생성/리터칭할 필요가 있다는 근거는 없습니다.
2. 새ZIP을 Python/.NET으로 열어13 entry·CRC를 검사하고 새컨테이너 SHA256을 납품문서와 함께 갱신합니다. 기존7dfa ZIP은 보존하며 덮어쓰지 않습니다.
3. PNG8의 fixed SHA는 [SourceCandidate8.csv](Evidence/Field09_MinimalFix_ZipFailure_20261019/SourceCandidate8.csv) 값 그대로 유지해야 합니다.
4. 정상 ZIP과 새납품SHA가 오면 이 worktree를 그대로 사용해 무결성검사→새Asset8+Scene8참조→실제Unity3해상도/이종지면접합/가이드6→Main18Delta→충돌·백업후원본적용 순서로 재개합니다. 현행중단은 “불일치/불합격 시 적용하지말고 보고” 지시에 따른 것이며 임의우회재포장·공식적용은 하지 않았습니다.

## 보호·미검증

원본 **4339파일 SHA 불일치0**, UserData23·Settings 백업, Git status raw byte동일/index변경0. 원본UnityEditor/Play/포커스조작0, Unity Assets/Scene/GUID 변경0, Main10완료/Main09승인27/Field06·07 B42/Field08·09 공식원본 보존. GitPush/PR/main병합0, local/origin/main HEAD유지.

새 before/after Unity화면·Main18기능Delta·원본적용은 **NOT_RUN / BLOCKED**입니다. 기존원본시각검수와Main18역사500PASS를 새후보검증으로 계수하지 않았습니다. [형식진단](Evidence/Field09_MinimalFix_ZipFailure_20261019/ArchiveForensics.json), [물리파일비교](Evidence/Field09_MinimalFix_ZipFailure_20261019/ZIP_Physical_Failure.json), [보호SHA](Evidence/Field09_MinimalFix_ZipFailure_20261019/ProtectionFinal.json).
