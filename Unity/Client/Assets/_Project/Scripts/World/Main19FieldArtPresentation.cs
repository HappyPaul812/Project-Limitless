using System.Collections;
using ProjectLimitless.Core;
using UnityEngine;

namespace ProjectLimitless.World
{
    /// <summary>Field10의 조사 맥동과 먼 목격 배경만 담당합니다. Quest·대화·카메라·저장을 변경하지 않습니다.</summary>
    public sealed class Main19FieldArtPresentation : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer silhouette;
        [SerializeField] private SpriteRenderer groundPulse;
        [SerializeField] private SpriteRenderer distantFlare;
        private Coroutine groundRoutine;
        private bool witnessStarted;
        public bool SilhouetteVisible => silhouette != null && silhouette.enabled;
        public int GroundPulseCount { get; private set; }
        public int DistantFlareCount { get; private set; }
        public float GroundPulseAlpha => groundPulse != null ? groundPulse.color.a : 0;
        public float DistantFlareAlpha => distantFlare != null ? distantFlare.color.a : 0;

        /// <summary>전용 배치 도구가 Scene 안의 세 Renderer를 연결합니다. 원본 PNG와 공용 Material은 수정하지 않습니다.</summary>
        public void Configure(SpriteRenderer distant, SpriteRenderer ground, SpriteRenderer flare)
        {
            silhouette = distant; groundPulse = ground; distantFlare = flare;
            silhouette.enabled = false;
            SetAlpha(groundPulse, 0); SetAlpha(distantFlare, 0);
        }
        private void Start()
        {
            QuestService.Changed += RefreshVisibility;
            SetAlpha(groundPulse, 0); SetAlpha(distantFlare, 0);
            RefreshVisibility();
        }
        private void OnDestroy()
        {
            QuestService.Changed -= RefreshVisibility;
            // 두 VFX는 Scene에 고정된 Renderer입니다. Scene 종료 시 Coroutine과 함께 사라져 누적되지 않습니다.
        }
        private void RefreshVisibility()
        {
            // Continue에서 목격 전(목표10 포함)은 숨기고, 목격 대화 완료 후 목표11/완료 기록으로 복원합니다.
            // 시작 중의 표시만 메모리에 보관하므로 새 Save 필드나 저장 버전을 만들지 않습니다.
            bool witnessed = witnessStarted || Chapter2Main19Flow.IsCurrent(11) ||
                QuestService.GetState(Chapter2Main19Flow.QuestId) == QuestState.Completed;
            if (silhouette != null) silhouette.enabled = witnessed;
        }
        /// <summary>기존 Site가 실제 대화를 시작할 때만 호출합니다. 잘못된 순서·중복 입력은 Site의 기존 Gate가 막습니다.</summary>
        public void BeginInvestigation(int index)
        {
            if (index == 5)
            {
                if (groundRoutine != null) StopCoroutine(groundRoutine);
                GroundPulseCount++;
                groundRoutine = StartCoroutine(Pulse(groundPulse, 2.4f, .55f));
            }
            if (index != 10 || witnessStarted) return;
            witnessStarted = true; RefreshVisibility();
            DistantFlareCount++;
            StartCoroutine(Pulse(distantFlare, 2.8f, .42f));
        }
        private static void SetAlpha(SpriteRenderer renderer, float alpha)
        {
            if (renderer == null) return;
            var color = renderer.color; color.a = alpha; renderer.color = color;
        }
        private IEnumerator Pulse(SpriteRenderer renderer, float seconds, float peak)
        {
            // 단일 PNG를 완만하게 한 번 밝히고 되돌립니다. 흰 Flash·반복 Strobe·추가 프레임 생성은 없습니다.
            float elapsed = 0;
            while (elapsed < seconds)
            {
                float envelope = Mathf.Sin(Mathf.PI * elapsed / seconds);
                SetAlpha(renderer, peak * envelope * envelope);
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }
            SetAlpha(renderer, 0);
        }
    }
}
