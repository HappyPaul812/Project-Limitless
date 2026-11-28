# Discord V2 F 시작 실패 인계

- 상태는 NO_GO / MANUAL / EXECUTION_HOLD. 기존 전용 Task와 Worker 모두 비활성 복귀 확인. Task 삭제·재등록·추가 시작 없음.
- 마지막 제품 기준 5d93b885a0a564ac2ef06b8cbea909e2dc7ff486. Probe와 DeploymentAdmin 및 기존 Task 정의 재사용. C 보호 교체·복구 계약 및 P 실제 배치 토큰 검증 PASS.
- 첫 F는 서명 만료 오프셋 오류로 시작 입력 설치 전에 거부됐고 대기 파일 세 개는 보호 보관. UTC 직렬화 검사 후 재발급한 두 번째 F는 시작 입력 설치 PASS.
- 두 번째 F의 시작 의도와 소비 기록은 보호 보존 대상. Task 활성 상태가 관측됐으나 인스턴스 영수증·최종 Evidence·잔류 제품 프로세스 없음. 기존 승인에 포함된 독립 원복으로 Task와 Worker 비활성을 확인. 실제 Supervisor/Probe 실행 및 Job drain 미검증.
- 20:18:34 KST PowerShellCore 4100 레코드 12402는 HOLD_START_NO_RETRY만 포함. 20:16~20:20 KST TaskScheduler/Application/선택 Security 이벤트 일치 없음. HRESULT와 상세 실패 단계 UNKNOWN.
- 래퍼 실행 순서는 승인 설치 → 기존 Worker 활성화 및 읽기 확인 → Supervisor 관리자 시작 → finally에서 Worker 비활성 복귀. Task 활성화가 관측되어 최초 Worker 비활성 상태만으로 이번 실패를 설명할 근거 없음. COM Run 직전 승인·계정·Task XML/SDDL fingerprint 재검증, COM Run 반환, 인스턴스 반환/저장 및 안전 비활성 검증을 구분할 진단이 필요. 소스상 안전 비활성에도 fingerprint 검사가 있으므로 그 경로의 예외가 선행 오류를 가릴 수 있음. 실제 원인으로 확정되지 않음.
- 재시작 금지. 실패한 시작 승인/시작 의도/원장/모든 증거 보존. 후속은 진단 소스와 외부 비활성 복귀 경계를 격리 검증하고 실제 재개 범위를 별도 합의.
- 관리자 보조 스크립트, 보관본, 권한 값, 서명 자산은 원래 로컬 위치에 보존. GitHub에는 검토 소스와 이 인계만 반영하며 호스트 식별값·비밀·운영 바이너리 제외.
