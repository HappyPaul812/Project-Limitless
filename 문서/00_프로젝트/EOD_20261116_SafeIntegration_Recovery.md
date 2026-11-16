# 2026-11-16 승인 제품 복구 체크포인트

**RECOVERY_CHECKPOINT_ONLY / NOT_RELEASE_READY / ISSUE85_IMPORT_GUID_HOLD / NON_GIT_OFFSITE_INCOMPLETE**.

기존 공개전송 감사 통과 Clean 계보61fdc690에 오늘 승인된 NPC4종 PNG/meta와 카탈로그, 실제 QA/기획/아트/영상 Markdown만 선별 보관합니다. 원본 local main의 제외아카이브 포함72커밋 이력과 모든 사용자 미커밋/미추적은 이 브랜치에 들어있지 않습니다. 기존 이력/refs 재작성0. 복구본 구성 후 실제 Push·재조회 결과는 Issue85 EOD 댓글에서 확인해야 합니다. 이 문서 자체는 Push 성공을 미리 주장하지 않습니다.

## 포함과 보류

- 기존Clean 승인제품235파일/승인삭제21 재사용, 기존main에서 오늘 승인 NPC02/17·10/07v2 제품9경로 선별. 원본 커밋7b84fa76/81f8a45d에 대응하는 **파일 blob을 동일하게** 보관하며 부모이력을 공개전송하지 않습니다. TTS162 및 기존 승인몬스터/환경/코드는 Clean 계보의 검증선별본으로 유지합니다.
- Unity버전6000.6.5f1, 하지만 Clean 계보의 기존 Animation/PSD Importer 호환성 문제·2D Tooling Import 예외·BattleProjectiles GUID6중복은 **해결하지 않았습니다**. 기존 #85 격리 수정 후보598ce8ea는 원본에 적용하지 않았으며 이 복구본의package2파일도 임의교체0. **새 PC에서 바로 정상빌드/실행되는 릴리스가 아닙니다.**
- KeyPose/Idle4/Attack4v2/Hit4는 모두 미술승인·Unity미적용. 해당 F: ZIP/PNG는 이 체크포인트에 추가하지 않았습니다. Defeat4미제작/HOLD작업 보호, 오늘 신규기능 착수0.
- 비Git 아트ZIP/영상/TTS원본/Save/Discord/인증/사용자dirty·untracked 및 다른 미Push브랜치 전부는 여기서 복원되지 않습니다. 비공개외부저장소·암호키·다른기기복원 확인 미확보. **PC고장 전체복구 완료가 아닙니다**. 동일F:에만남긴 보존본은 오프사이트가 아닙니다.

## 복구 방법

새기기에서 GitHub 저장소를 clone한 뒤 backup/2026-11-16-safe-checkpoint를 별도 작업브랜치로 checkout합니다. 이 브랜치의 승인제품을 보존하고 Issue85의 패키지/Import/GUID 승인·표적검증부터 재개해야 합니다. 기존Dirty/Save/Discord를 이 브랜치에서 되살릴 수 있다고 오해하지 마세요. 원본main을 reset/force로 이 브랜치에 정렬하지 않습니다.

로컬보고: EOD_20261116_SafeIntegration_Recovery.md. 최종상태는 GitHub Issue85 EOD 댓글을 확인합니다. 오늘개발종료, 추가작업자동시작금지.
