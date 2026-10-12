using ProjectLimitless.Battle;
using UnityEngine;

namespace ProjectLimitless.World
{
    /// <summary>
    /// Main21 하렌 첫 만남 지점에 공식 필드 외형만 표시합니다.
    /// Character ID·능력치·Companion·파티를 만들지 않으며 목표와 상호작용은 기존 Main21Site가 처리합니다.
    /// </summary>
    public sealed class Main21HarenVisual : MonoBehaviour
    {
        private GameObject visual;

        private void Start()
        {
            Sprite idle = System.Array.Find(Resources.LoadAll<Sprite>(
                "StoryCharacters/Haren/Haren_Field_Walk_4Dir"), sprite => sprite.name == "Haren_Idle_Down_00");
            RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>(
                "StoryCharacters/Haren/Animations/Haren_Field");
            // 콘텐츠 누락이 발생해도 퀘스트 지점과 대화는 유지해 진행을 막지 않습니다.
            if (idle == null || controller == null)
            {
                Debug.LogError("Main21 하렌 공식 필드 Sprite 또는 Animator가 없습니다.");
                return;
            }
            visual = new GameObject("HarenOfficialVisual", typeof(SpriteRenderer), typeof(Animator));
            visual.transform.SetParent(transform, false);
            SpriteRenderer renderer = visual.GetComponent<SpriteRenderer>();
            renderer.sprite = idle;
            renderer.sortingOrder = 8;
            visual.GetComponent<Animator>().runtimeAnimatorController = controller;
            Refresh();
        }

        private void Update() => Refresh();

        private void Refresh()
        {
            // 첫 등장 목표에서만 보입니다. 이후 등장·합류 시점은 별도 기획으로 결정합니다.
            if (visual != null) visual.SetActive(Chapter2Main21Flow.Current(11));
        }
    }
}
