using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectLimitless.World
{
    /// <summary>현재 Scene의 stable ID와 실제 Transform을 연결하는 등록소입니다.</summary>
    public static class QuestNavigationTargetRegistry
    {
        public readonly struct Target
        {
            public Target(Transform transform, string label) { Transform = transform; Label = label; }
            public Transform Transform { get; }
            public string Label { get; }
        }

        private static readonly Dictionary<string, Target> Targets = new Dictionary<string, Target>(StringComparer.Ordinal);
        public static event Action Changed;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset() { Targets.Clear(); Changed = null; }

        internal static void Register(string id, Transform transform, string label)
        {
            if (string.IsNullOrWhiteSpace(id) || transform == null) return;
            Targets[id] = new Target(transform, string.IsNullOrWhiteSpace(label) ? "목표" : label);
            Changed?.Invoke();
        }

        internal static void Unregister(string id, Transform transform)
        {
            if (!Targets.TryGetValue(id ?? string.Empty, out Target target) || target.Transform != transform) return;
            Targets.Remove(id);
            Changed?.Invoke();
        }

        public static bool TryGet(string id, out Target target)
        {
            if (Targets.TryGetValue(id ?? string.Empty, out target) && target.Transform != null) return true;
            if (!string.IsNullOrEmpty(id)) Targets.Remove(id);
            target = default;
            return false;
        }
    }

    /// <summary>Scene Object가 stable ID 위치를 등록하고 비활성화 시 안전하게 해제합니다.</summary>
    public sealed class QuestNavigationTarget : MonoBehaviour
    {
        [SerializeField] private string targetId = string.Empty;
        [SerializeField] private string navigationLabel = "목표";

        public static QuestNavigationTarget Attach(GameObject owner, string id, string label, Vector3 localOffset = default)
        {
            if (owner == null || string.IsNullOrWhiteSpace(id)) return null;
            string childName = "QuestNavigation_" + id;
            Transform child = owner.transform.Find(childName);
            GameObject anchor = child != null ? child.gameObject : new GameObject(childName);
            anchor.transform.SetParent(owner.transform, false);
            anchor.transform.localPosition = localOffset;
            QuestNavigationTarget target = anchor.GetComponent<QuestNavigationTarget>() ?? anchor.AddComponent<QuestNavigationTarget>();
            target.Configure(id, label);
            return target;
        }

        public void Configure(string id, string label)
        {
            if (isActiveAndEnabled) QuestNavigationTargetRegistry.Unregister(targetId, transform);
            targetId = id ?? string.Empty;
            navigationLabel = string.IsNullOrWhiteSpace(label) ? "목표" : label;
            if (isActiveAndEnabled) QuestNavigationTargetRegistry.Register(targetId, transform, navigationLabel);
        }

        private void OnEnable() => QuestNavigationTargetRegistry.Register(targetId, transform, navigationLabel);
        private void OnDisable() => QuestNavigationTargetRegistry.Unregister(targetId, transform);
    }
}
