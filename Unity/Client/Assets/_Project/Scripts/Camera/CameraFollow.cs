using UnityEngine;

namespace ProjectLimitless.CameraSystem
{
    /// <summary>
    /// 2D 카메라가 지정된 대상을 부드럽게 따라가게 합니다.
    /// Main Camera GameObject에 붙이며, 보통 플레이어의 Transform(위치·회전·크기 정보)을 대상으로 지정합니다.
    /// </summary>
    public sealed class CameraFollow : MonoBehaviour
    {
        // 카메라가 따라갈 대상입니다. Unity Inspector에서 플레이어를 연결할 수 있습니다.
        [SerializeField] private Transform target;
        // 값이 클수록 카메라가 대상 위치에 더 빠르게 가까워집니다.
        [SerializeField, Min(0.1f)] private float followSpeed = 8f;

        private Camera attachedCamera;
        private Bounds movementBounds;
        private float defaultOrthographicSize;
        private bool useMovementBounds;

        /// <summary>Scene 생성 도구 등이 카메라의 추적 대상을 지정할 때 사용합니다.</summary>
        public void SetTarget(Transform followTarget) => target = followTarget;

        /// <summary>카메라 화면 전체가 지정 영역 안에 머물도록 이동 범위를 설정합니다.</summary>
        public void SetMovementBounds(Bounds bounds)
        {
            movementBounds = bounds;
            attachedCamera = GetComponent<Camera>();
            defaultOrthographicSize = attachedCamera != null ? attachedCamera.orthographicSize : 0f;
            useMovementBounds = true;
        }

        /// <summary>
        /// 다른 오브젝트의 이동이 끝난 뒤 호출되어 카메라를 움직입니다.
        /// 플레이어보다 나중에 이동함으로써 화면 떨림을 줄입니다.
        /// </summary>
        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            // 2D 평면의 X, Y만 따라가고 카메라의 앞뒤 거리인 Z는 그대로 유지합니다.
            Vector3 destination = new Vector3(target.position.x, target.position.y, transform.position.z);
            if (useMovementBounds && attachedCamera != null && attachedCamera.orthographic)
            {
                FitCameraInsideBounds();
                float halfHeight = attachedCamera.orthographicSize;
                float halfWidth = halfHeight * attachedCamera.aspect;
                destination.x = ClampToViewport(destination.x, movementBounds.min.x, movementBounds.max.x, halfWidth);
                destination.y = ClampToViewport(destination.y, movementBounds.min.y, movementBounds.max.y, halfHeight);
            }

            // Lerp는 현재 위치와 목표 위치 사이를 조금씩 보간하여 부드럽게 이동시킵니다.
            transform.position = Vector3.Lerp(transform.position, destination, followSpeed * Time.deltaTime);
        }

        private void FitCameraInsideBounds()
        {
            float maximumSize = Mathf.Min(movementBounds.extents.y, movementBounds.extents.x / attachedCamera.aspect);
            attachedCamera.orthographicSize = Mathf.Min(defaultOrthographicSize, maximumSize);
        }

        private static float ClampToViewport(float value, float minimum, float maximum, float viewportExtent)
        {
            float clampedMinimum = minimum + viewportExtent;
            float clampedMaximum = maximum - viewportExtent;
            return clampedMinimum <= clampedMaximum
                ? Mathf.Clamp(value, clampedMinimum, clampedMaximum)
                : (minimum + maximum) * .5f;
        }
    }
}
