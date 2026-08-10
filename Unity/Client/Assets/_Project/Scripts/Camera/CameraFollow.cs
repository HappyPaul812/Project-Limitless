using UnityEngine;

namespace ProjectLimitless.CameraSystem
{
    /// <summary>2D 카메라가 지정된 대상을 부드럽게 따라가게 한다.</summary>
    public sealed class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField, Min(0.1f)] private float followSpeed = 8f;

        public void SetTarget(Transform followTarget) => target = followTarget;

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            Vector3 destination = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, destination, followSpeed * Time.deltaTime);
        }
    }
}
