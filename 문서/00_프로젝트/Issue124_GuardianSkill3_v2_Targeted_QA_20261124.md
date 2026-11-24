# Issue #124 · GuardianSkill3 v2 표적 재검수

2026-11-24 · **PARTIAL** · 철벽 보완 PASS, 수호의 맹세 작은 HUD 관계 판독 PARTIAL.

[#124 기획 기준](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7126855514) · [v1 검수](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7127093770) · [v2 납품](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7127728672)

## PASS · 변경점과 파일 검증

- **철벽**은 v1의 큰 중앙 방패를 줄이고 성벽 상단과 넓은 기단을 외곽에 배치했습니다. 승인 기본 방어의 뾰족한 방패와 30/40px에서 명확히 구분됩니다. 24/20px에서도 성벽 상단과 받침이 우선 읽힙니다. 기존 공격·방어·스킬을 나란히 놓았을 때 금속·금테·청색 계열이 이어집니다.
- **도발**은 v1 원화와 SHA256 및 바이트가 같습니다. 붉은 표식과 금속 외곽은 30/40px에서 유지되며, 20/24px에서도 흰색 `pawn_right` 상태와 구분됩니다.
- ZIP 2,204,204 B · SHA256 `9abaa07de47ee869f35570a7647aad70189eb660400a77c5332acdbb0d9dfb18`. ZIP CRC 41/41, PNG 청크 CRC 및 디코딩 37/37, Manifest 경로·크기·해시 39/39, SHA256SUMS 40/40. 승인 기본 버튼 3종과 v1 참조 3종은 기존 실물과 바이트가 일치합니다. 후보 3종은 128×128 RGBA, 알파 0–255 및 투명 모서리를 확인했습니다.
- 철벽 `6bebeac7b03c6906b6f23adb822f6a05531c3fe49ea48b8452d44cf76ae5f08f`, 맹세 `3b66adf62bff00d86056f29564e7a885d0987b58812624c4bb8de03ba6e49977`, 도발 `960bd0a3b87eadae637a1e2243eb3eab848b28963651ba328b01e699dba5687b`.

## PASS · Unity 표적 표시 및 공유

- Unity 6000.6.5f1 격리 Battle 배치 Play Mode에서 실제 스킬 버튼 생성과 상태 배지 갱신 경로를 호출했습니다. 철벽과 맹세는 각각 스킬과 상태가 동일한 v2 Sprite 인스턴스를 참조했고, 도발 스킬은 KEEP Sprite, 도발 상태는 기존 `pawn_right`를 유지했습니다. 승인 공격·방어·스킬 Sprite도 표시되었습니다. 상태는 검수용 입력으로 구성했으며 스킬 시전은 검수 범위에서 제외했습니다.
- 동일 배율의 v1/v2 Unity 캡처를 잘라 `Temp/i124-guardian-v2-qa/Guardian_Unity_v1_v2_Crops.png`에 보존했습니다. 스킬 약 30px/HUD 20px이며 원본 캡처도 같은 폴더에 있습니다. 격리용 Sprite 및 매핑을 원복했고 배치는 종료 코드 0으로 끝났습니다.

## PARTIAL · 수호의 맹세

- 동료 주변 RGB 1,835픽셀이 바뀌었고 전체 알파는 같습니다. 넓은 청색 보호막은 철벽의 수직 성벽과 기본 방어의 방패 외곽과 구분됩니다. 30/40px에서는 좌우 동료가 조금 더 드러납니다. 24/20px에서는 대비 보완이 작고, 보호 관계를 아이콘만으로 읽기에는 라벨의 도움이 필요합니다.

## HOLD · 본편 반영

Unity 6000.6.5f1 유지. 승인 자산, 원본 main `65cceb2d6c3283d18abec6e27d8a6f1296411f17`, #85 `c5d9ed202a445ce3ef68c72f0b54cc47c1719b9c`, #131 `9d155ca635143b217a9cdd68a0c3e8be54106cb4`, Save/UserData 보호. 본편 반영, 전수 QA 재실행, main 병합, 원격 Push 없이 보고 문서만 로컬로 커밋합니다. `LOCAL_COMMITTED / REMOTE_NOT_PUSHED`.

실제 40/30/24/20px 비교판 `Temp/i124-guardian-v2-qa/Guardian_v1_v2_ActualPixels.png` · SHA256 `334dce7cb1b8c28dc77ce0ac83cd08ed6d0e350537342da09b60116051c1b24c`.
