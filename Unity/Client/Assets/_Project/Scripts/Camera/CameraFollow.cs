using UnityEngine;
using ProjectLimitless.World;

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

        // 같은 GameObject의 Camera Component를 보관해 매 프레임 다시 찾지 않게 합니다.
        private Camera attachedCamera;
        // 값이 있으면 카메라 화면 전체가 이 월드 영역 안에 머물도록 제한합니다. 없는 Scene에서는 기존 추적만 수행합니다.
        [SerializeField] private WorldBounds2D worldBounds;
        // 화면 비율에 맞추느라 카메라를 축소한 뒤에도 원래 확대 크기보다 커지지 않도록 최초 값을 보관합니다.
        private float defaultOrthographicSize;

        /// <summary>Scene 생성 도구 등이 카메라의 추적 대상을 지정할 때 사용합니다.</summary>
        public void SetTarget(Transform followTarget) => target = followTarget;

        // GameObject가 준비될 때 Camera와 원래 Orthographic Size를 한 번 저장합니다.
        private void Awake() => CacheCamera();

        /// <summary>같은 GameObject의 Camera와 Scene에 설정된 기본 화면 크기를 저장합니다.</summary>
        private void CacheCamera()
        {
            attachedCamera = GetComponent<Camera>();
            defaultOrthographicSize = attachedCamera != null ? attachedCamera.orthographicSize : 0f;
        }

        /// <summary>카메라 화면 전체가 지정 영역 안에 머물도록 이동 범위를 설정합니다.</summary>
        public void SetWorldBounds(WorldBounds2D bounds)
        {
            worldBounds = bounds;
            if (attachedCamera == null) CacheCamera();
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
            if (worldBounds != null && attachedCamera != null && attachedCamera.orthographic)
            {
                Bounds movementBounds = worldBounds.Bounds;
                FitCameraInsideBounds();
                // Orthographic Size는 화면의 세로 절반 길이입니다. 여기에 현재 종횡비를 곱하면
                // 해상도나 창 크기가 달라져도 실제 화면의 가로 절반 길이를 얻을 수 있습니다.
                float halfHeight = attachedCamera.orthographicSize;
                float halfWidth = halfHeight * attachedCamera.aspect;
                // 카메라 중심이 맵 끝까지 가면 화면의 절반이 맵 밖으로 나갑니다.
                // 따라서 각 방향에서 화면 절반 크기를 제외한 안쪽 범위로 카메라 중심을 제한합니다.
                destination.x = ClampToViewport(destination.x, movementBounds.min.x, movementBounds.max.x, halfWidth);
                destination.y = ClampToViewport(destination.y, movementBounds.min.y, movementBounds.max.y, halfHeight);
            }

            // Lerp는 현재 위치와 목표 위치 사이를 조금씩 보간하여 부드럽게 이동시킵니다.
            transform.position = Vector3.Lerp(transform.position, destination, followSpeed * Time.deltaTime);
        }

        /// <summary>
        /// 창이 매우 넓거나 맵이 작은 경우 화면 자체가 Bounds보다 커지지 않도록 Orthographic Size를 줄입니다.
        /// 일반 화면에서는 Scene에 설정된 원래 크기를 그대로 유지합니다.
        /// </summary>
        private void FitCameraInsideBounds()
        {
            Bounds movementBounds = worldBounds.Bounds;
            float maximumSize = Mathf.Min(movementBounds.extents.y, movementBounds.extents.x / attachedCamera.aspect);
            attachedCamera.orthographicSize = Mathf.Min(defaultOrthographicSize, maximumSize);
        }

        /// <summary>화면 반경을 제외하고도 남는 구간 안으로 카메라 중심 좌표 하나를 제한합니다.</summary>
        private static float ClampToViewport(float value, float minimum, float maximum, float viewportExtent)
        {
            float clampedMinimum = minimum + viewportExtent;
            float clampedMaximum = maximum - viewportExtent;
            // 화면이 Bounds보다 큰 예외 상황에서는 유효한 이동 구간이 없으므로 중앙을 사용합니다.
            return clampedMinimum <= clampedMaximum
                ? Mathf.Clamp(value, clampedMinimum, clampedMaximum)
                : (minimum + maximum) * .5f;
        }
    }
}
