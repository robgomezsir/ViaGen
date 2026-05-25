using UnityEngine;

namespace ViaGen.Player
{
    public class PlayerVisualProcedural : MonoBehaviour
    {
        [SerializeField] private Transform visualRoot;
        [SerializeField] private float walkBobSpeed = 8f;
        [SerializeField] private float walkBobAmount = 0.04f;
        [SerializeField] private float runLeanAngle = 5f;
        [SerializeField] private float rotationSpeed = 12f;

        private Vector3 _baseLocalPos;
        private Quaternion _baseLocalRot;
        private float _bobPhase;
        private bool _wasGrounded = true;

        public void SetVisualRoot(Transform root)
        {
            visualRoot = root;
            if (visualRoot != null)
            {
                _baseLocalPos = visualRoot.localPosition;
                _baseLocalRot = visualRoot.localRotation;
            }
        }

        private void LateUpdate()
        {
            if (visualRoot == null) return;
            var controller = GetComponentInParent<CharacterController>();
            if (controller == null) return;

            var planarVel = new Vector3(controller.velocity.x, 0f, controller.velocity.z);
            var speed = planarVel.magnitude;
            var isMoving = speed > 0.1f;
            var isRunning = speed > 5f;

            if (isMoving)
            {
                var targetRot = Quaternion.LookRotation(planarVel.normalized, Vector3.up);
                visualRoot.rotation = Quaternion.Slerp(visualRoot.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }

            _bobPhase += Time.deltaTime * (isRunning ? walkBobSpeed * 1.4f : walkBobSpeed);
            var bob = isMoving ? Mathf.Sin(_bobPhase) * walkBobAmount : Mathf.Sin(Time.time * 2f) * walkBobAmount * 0.25f;
            visualRoot.localPosition = _baseLocalPos + Vector3.up * bob;

            var lean = isRunning ? Quaternion.Euler(runLeanAngle, 0f, 0f) : Quaternion.identity;
            visualRoot.localRotation = _baseLocalRot * lean;

            if (controller.isGrounded && !_wasGrounded)
                StartCoroutine(LandingSquash());
            _wasGrounded = controller.isGrounded;
        }

        private System.Collections.IEnumerator LandingSquash()
        {
            var t = 0f;
            while (t < 0.15f)
            {
                t += Time.deltaTime;
                var s = Mathf.Lerp(0.95f, 1f, t / 0.15f);
                visualRoot.localScale = new Vector3(1f, s, 1f);
                yield return null;
            }
            visualRoot.localScale = Vector3.one;
        }
    }
}
