# Issue96 물약 상점 한정 Delta QA

Unity6000.6.5f1의 기존 격리 캐시 `Temp/Issue97/QA/Client`를 재사용합니다. 원본 Editor와 별개이며 이전 Snake/F2 QA는 호출하지 않습니다. `PotionProbe.cs`는 이 격리 프로젝트에만 복사하고 제품 Assets에는 넣지 않습니다.

- `-batchmode -executeMethod PotionBatch.Baseline`: 원본과 같은 바이트의 기존 ShopPresenter로 실제 교체 전960×720/1280×720 GameView 촬영.
- `PotionBatch.After`: 제품5파일을 격리 Assets에 복사한 뒤 신규 상점 표시·거래·입력·범위 검사46건.
- `PotionBatch.PostApply`: 원본에 반영된 실제5파일을 읽어 동일 바이트로 연결한 후 재실행.

검증 결과는 `Temp/Issue96Original/QA/Before`, `After`, `PostApply`에 분리합니다. Save/UserSettings도 단계별 Synthetic 디렉터리로 우회합니다. 기존 사용자 Save를 읽거나 수정하지 않습니다.
Input System 가상 Mouse/Keyboard를 생성해 UI 모듈에 실제 입력 이벤트를 넣습니다. OS 포커스·실제 커서·사용자 키보드를 조작하지 않습니다. 카메라 합성 대신 UnityEditor.GameView.RenderView를 캡처하며 크기 변경 후 warm-up을 거칩니다. D3D 저장 방향만 정규화합니다.

첫 검증기 컴파일의 사용하지 않는 namespace를 제거했습니다. 첫 After 실행은 격리 환경의 Define 변경 재컴파일/도메인 재로드가 중간 상태를 초기화해 완료되지 않았습니다. 이후 짧은 Play 검증 중에만 격리 Editor의 assembly reload를 잠그고 종료/timeout 때 해제하도록 보완해46/46 PASS를 확보했습니다. 제품 코드와 원본 Editor의 재로드 설정은 변경하지 않았습니다. 기존 SearchDatabase/Burst 참조 초기화 로그는 Runtime 오류와 구분합니다.
