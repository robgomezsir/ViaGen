using UnityEngine;
using UnityEngine.InputSystem;

namespace ViaGen.Player
{
    public class ThirdPersonCameraController : MonoBehaviour
    {
        [SerializeField] private Transform followTarget;
        [SerializeField] private Vector3 offset = new(0f, 2f, -4f);
        [SerializeField] private float lookSensitivity = 1.5f;
        [SerializeField] private float minPitch = -25f;
        [SerializeField] private float maxPitch = 55f;
        [SerializeField] private float smooth = 12f;

        private float _yaw;
        private float _pitch = 15f;

        public void Bind(Transform target) => followTarget = target;

        private void LateUpdate()
        {
            if (followTarget == null) return;
            var mouse = Mouse.current;
            if (mouse != null)
            {
                var look = mouse.delta.ReadValue() * lookSensitivity * 0.05f;
                _yaw += look.x;
                _pitch -= look.y;
                _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);
            }

            followTarget.rotation = Quaternion.Euler(0f, _yaw, 0f);
            var rot = Quaternion.Euler(_pitch, _yaw, 0f);
            var desiredPos = followTarget.position + rot * offset;
            transform.position = Vector3.Lerp(transform.position, desiredPos, smooth * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, smooth * Time.deltaTime);
        }
    }
}
