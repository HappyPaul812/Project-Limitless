# 2026-10-19 EOD 승인 기능 통합 및 원본 동기화 마감

## 현재 단계

통합 기준 main `675988c9f2c5c507d9a696b0378be27c0bd3309b`. `codex/eod-integration-20261019`에서 승인 기능8커밋을 선택 통합했습니다. PR 병합과 원본 HEAD/index 동기화는 후속 확정 증거를 따르며 이 준비 시점에는 완료로 선언하지 않습니다.

## 승인 통합과 QA

|범위|공식 내용|기존 QA 재사용|
|---|---|---|
|Main09|청취 승인27개, 원음 보존/새GUID 연결, 폴 재회 후 안내|550+261 PASS|
|Main10|청취 승인18개, 조사점 입력 우선/치유사 유지|132+164 PASS|
|Field06|v1.1 B42 환경375, Ground49 숨김 보호, 실제 Collider 겹침 Spawn 안전 보정|349 및 공동124 PASS|
|Field07|v1 B42 환경384, 기존 투명먼지 그대로 복사해 Ash7 사각형 제거|324 성공·기존FAIL 해소, 공동124 PASS|
|Field08|공식9 PNG 변경0, 연결 확인 기존 결과 유지|NO_NEW_ART_REQUIRED|

이번 통합 Delta: **정적8 + 실제 격리 Unity10 =18 PASS/0 FAIL**. 승인 게임파일168개가 현재 원본 반영본과 byte-identical, 승인 WAV45/meta/PCM SHA와 Catalog GUID 일치, Catalog290/나머지245 보호, 환경375/384 및 Sprite·Script 참조 검사/컴파일 PASS. 검사 도구의 출력경로/자동생성 NPC 집계 오류는 도구에서 수정했으며 게임 오류로 합산하지 않았습니다. 기존 수백 QA 재주행0, 신규TTS/API0. [검증 원본](Evidence/EOD20261019/IntegrationQA.json).

원본 Unity Editor/Play/Refresh/포커스 조작0. 원본 자연Import/손플레이 및 Main15·16 전주행 NOT_VERIFIED, 기존 Search/URP 환경 OPEN. 격리 QA를 사용자 손플레이 PASS로 표시하지 않습니다.

## 보호·문서 의미 병합

원본 tracked dirty317/untracked707/index staged0 실측, UserData·Settings 포함 **1,059파일/203,125,963bytes** SHA256 백업. `Temp/EOD20261019/BackupManifest.json`, Git index/refs/39 worktree 상태 보존. 기존 전체 보호파일4,339 SHA 불일치0. UserData/Save/Settings는 원본 그대로 유지합니다.

기획/리뉴얼/TTS는 branch 전체 merge 없이 파일별 공통조상·main·담당 최신 비교. 충돌 문서는 main 고유 문단과 담당 고유 문단을 HISTORY로 보존하고 각 최신 인계 최상단에 현재 승인/제외 상태를 명시했습니다. Chapter3는 문서 제안만 보존/구현0. 기존 로컬 문서2커밋 ff022e63/3a37aa2e는 main 조상으로 유지합니다. 원본 문서를 한쪽으로 통째로 대체하지 않습니다. [선택/의미 병합 목록](Evidence/EOD20261019/DocumentMergeInventory.json).

기획의 Main09 최종SHA6e1a93c는 복구감사 당시 커밋으로, 실제 승인연결 최종은 `424b19122221d1555fef14d46c459fa174ac07a2`. Main10 SHA 오기는 실제 `6de449abc16d09bbf20db003ecc64d101eca18f0`로 대조했습니다. [기능커밋 대응](Evidence/EOD20261019/FeatureCommitMap.csv).

## 공식 통합 제외와 다음날

- Field09: 재포장 성공/기술40+Main18delta92 PASS와 별개로 **미술 FAIL**. 지면 회색 반복띠·6소품 흰 가이드선 잔존. 실패 기록만 보존하며 후보PNG/Scene/원래·재포장ZIP 모두 main 제외, 원본 공식10 PNG/meta/Scene 보호. 아트 담당이 동일 결함을 수정한 신규 납품 후 별도 승인/검수 필요.
- 독사/늑대 v1 반려, v2 HOLD, v3 sandbox 미승인: 공식 이미지/시트/Runtime 적용0.
- Main20·21 TTS22 미등록: 후보17 중 음색 불합격9·나머지8 승인미확인, 미존재5. 기존 정확한 CSV 및 TTS 담당 큐 유지, 청취 승인 전 적용0.
- Chapter3 Main22+: 기획/ID/아트 승인 게이트 후 구현. 오늘 신규콘텐츠0.
- 다음 재개: 최신 main/로컬 보호 증거 확인 → 대표 평소 원본 자연Import·Main09/10·Field06/07 화면 확인 → TTS/Field09 담당 승인 납품 대기. 완료 기능 재구현·기존 전체 QA 반복 금지.

## 원본 동기화 판정

168게임파일은 기존 승인 원본과 정확히 일치하여 다시 쓰지 않습니다. 문서 변경은 clean-old 또는 부재인 경우만 적용하고, 별도 격리 리허설 후 비파괴 fast-forward를 검토합니다. 실제 root HEAD=origin/main=GitHub main, index=HEAD, 허용 dirty/Save SHA 전후 검증을 모두 마친 증거만 SYNC_COMPLETE입니다. 실패 시 SYNC_SAFE_BLOCKED/원본 보존이 원칙입니다.

격리 동기화 리허설 PASS: 기존 승인168파일 index만 선반영 후 일반 `git merge --ff-only` 성공, staged0/백업1,059 SHA 불일치0. 원본에 같은 절차를 실행하기 전 실제 파일·index SHA를 다시 확인합니다.
