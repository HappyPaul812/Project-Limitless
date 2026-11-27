# Issue #30 설빙의 폭군 Defeat4 v1 표적 검수

2026-11-27 · [기획 접수](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-7209113312)

**336px 외형·자세·접지 및 비반복 계약 PASS. 납품 일치성 PARTIAL, 승인·본편 반영 HOLD.**

- Issue #30 및 원격 `docs/art-new`의 최신 `SPRITE_ART_LATEST.md` 재조회 결과, 독립된 Defeat4 납품 보고와 기대 ZIP SHA는 미등록입니다. 로컬 ZIP 실측 SHA256 `0c7af59131557383359a819b4f5ccf738d767f47e416fbcb5512a7846101d1a4`, CRC 18/18, SUMS 기재 16/16, Manifest 명시 SHA 9/9 일치. Manifest.md는 SUMS 목록에서 빠져 있습니다. 시트 SHA256 `9ee9f07f3c7cba7b391934fbfe9efc98eff955fcccaa680939ff9afb9d99fb40`.
- Attack4 v3·Hit4 v1 참조는 승인본과 바이트 동일. KeyPose·Idle4 참조는 승인본과 디코딩 RGBA 픽셀 동일, PNG SHA는 각각 `0a5eb38d56e8c7dd5b4b5e6f1565504e9301062884d59c26db88142709f4cbd9`, `bde6ee20e03f30d97c03d7887fd782106f23ba1680e3da49c2d6a65d497cf2ab`로 기존 승인 SHA와 다릅니다. 승인 원본을 교체하지 않았습니다.
- 2048×512 시트와 개별 512셀은 모든 가시 픽셀이 일치합니다. 셀 01/02/03의 126/175/186 픽셀은 완전 투명 영역 RGB만 다릅니다. 하단 152행의 RGB와 가시 영역 마스크는 대응 Idle과 같지만 알파가 최대 64 높아지는 픽셀이 있어 RGBA 전체 동일은 성립하지 않습니다. 가시 접지 y490 exclusive, 셀 경계 알파 0, 잘림·뚜렷한 배경 매트 없음.
- 동일 Unity 배율에서 머리와 어깨가 점차 낮아지고 눈과 서리 광채가 어두워집니다. 뿔·흉곽·발톱 실루엣과 발 위치는 유지됩니다. 상단 y25→45의 변화는 336px에서 약 13px입니다. 전체적으로 주저앉기보다 상체가 가라앉는 연출입니다.
- 납품 APNG는 1회 재생에 마지막 추가 1000ms 홀드를 담습니다. 디코딩 재생에서 02·03 및 홀드의 경계 알파가 시트와 다릅니다. GIF의 loop 확장값 1은 재반복으로 해석하는 뷰어가 있어 비반복 근거로 사용하지 않았습니다.
- Unity 6000.6.5f1 격리에서 PPU512/하단 중앙 Pivot/Scale3/336px 실제 SpriteRenderer 8장을 캡처했습니다. 기존 `MonsterSpriteSheetAnimation.PlayDefeat`를 임시 정의로 호출하고 실제 Update의 8FPS 시점 샘플이 `0,1,2,3,3,3,3`인지 확인했습니다. 2초 이후 및 정상 Update 재개에도 마지막 Sprite 유지, 재검사 종료 0. 최초 실시간 샘플은 렌더 직후 긴 프레임으로 `0,3`만 관측됐습니다. 실제 보스 전투 종료 트리거·퇴장은 미검증입니다.

로컬 `Temp/i30-defeat4-i124-mage-v2-qa/`의 `Defeat_Unity_336px.png` SHA256 `3d04f96a3d863f0de56c5b3065e30f1f195a8277dab04a05f2c0e9b7b2773389`, `Defeat_Unity_OneShot.png` SHA256 `899506485f33c82d9af42568314e38f6a3ae156ca6ecff974feb70545a470eb3`. 후자는 실제 Unity 캡처를 조합한 1회 재생 APNG입니다.

승인 KeyPose·Idle4·Attack4 v3·Hit4 v1 및 #85/#131/#132·Save/UserData 보호. 본편 적용·main 병합·Push 없음.
