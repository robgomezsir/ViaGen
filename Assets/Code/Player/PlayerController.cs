using UnityEngine;
using UnityEngine.InputSystem;
using ViaGen.Core;
using ViaGen.Ship;

namespace ViaGen.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(SurvivalStats))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float walkSpeed = 4f;
        [SerializeField] private float runSpeed = 7f;
        [SerializeField] private float gravity = -20f;
        [SerializeField] private float jumpHeight = 1.2f;

        [Header("Third Person")]
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private Transform visualRoot;

        [Header("Jetpack")]
        [SerializeField] private float jetpackForce = 8f;
        [SerializeField] private float jetpackEnergyPerSecond = 15f;

        private CharacterController _controller;
        private SurvivalStats _survival;
        private PlayerVisualProcedural _visual;
        private ThirdPersonCameraController _cameraController;
        private Vector3 _velocity;
        private bool _jetpackUnlocked;

        public bool CanMove { get; set; } = true;
        public bool IsGrounded => _controller.isGrounded;

        private void Awake()
        {
            gameObject.tag = "Player";
            _controller = GetComponent<CharacterController>();
            _survival = GetComponent<SurvivalStats>();
            _visual = GetComponent<PlayerVisualProcedural>();
            if (_visual == null) _visual = gameObject.AddComponent<PlayerVisualProcedural>();

            if (cameraTarget == null)
            {
                var t = transform.Find("CinemachineTarget");
                if (t == null)
                {
                    var go = new GameObject("CinemachineTarget");
                    go.transform.SetParent(transform);
                    go.transform.localPosition = new Vector3(0f, 1.6f, 0f);
                    t = go.transform;
                }
                cameraTarget = t;
            }

            if (visualRoot == null)
            {
                var v = transform.Find("Visual");
                if (v != null) visualRoot = v;
            }

            _visual.SetVisualRoot(visualRoot);
            SetupCamera();
        }

        private void SetupCamera()
        {
            var cam = Camera.main;
            if (cam == null)
            {
                var camGo = new GameObject("Main Camera");
                cam = camGo.AddComponent<Camera>();
                cam.tag = "MainCamera";
                camGo.AddComponent<AudioListener>();
            }

            _cameraController = cam.GetComponent<ThirdPersonCameraController>();
            if (_cameraController == null)
                _cameraController = cam.gameObject.AddComponent<ThirdPersonCameraController>();
            _cameraController.Bind(cameraTarget);

            cam.transform.SetParent(null);
            cam.transform.position = transform.position + new Vector3(0f, 2f, -4f);
        }

        private void OnEnable()
        {
            GameEvents.OnGameStateChanged += OnGameStateChanged;
            GameEvents.OnTechUnlocked += OnTechUnlocked;
        }

        private void OnDisable()
        {
            GameEvents.OnGameStateChanged -= OnGameStateChanged;
            GameEvents.OnTechUnlocked -= OnTechUnlocked;
        }

        private void OnGameStateChanged(GameState prev, GameState current)
        {
            CanMove = current == GameState.Exploring || current == GameState.InShip;
            Cursor.lockState = CanMove ? CursorLockMode.Locked : CursorLockMode.None;
        }

        private void OnTechUnlocked(string techId)
        {
            if (techId == "jetpack") _jetpackUnlocked = true;
        }

        private void Start()
        {
            if (ShipManager.Instance != null)
                _jetpackUnlocked = ShipManager.Instance.HasTech("jetpack");
        }

        private void Update()
        {
            if (!CanMove) return;
            var keyboard = Keyboard.current;
            if (keyboard == null) return;
            HandleMovement(keyboard);
        }

        private void HandleMovement(Keyboard keyboard)
        {
            var input = Vector2.zero;
            if (keyboard.wKey.isPressed) input.y += 1;
            if (keyboard.sKey.isPressed) input.y -= 1;
            if (keyboard.aKey.isPressed) input.x -= 1;
            if (keyboard.dKey.isPressed) input.x += 1;

            var forward = cameraTarget != null ? cameraTarget.forward : transform.forward;
            forward.y = 0f;
            forward.Normalize();
            var right = Vector3.Cross(Vector3.up, forward);
            var move = (right * input.x + forward * input.y).normalized;

            var speed = keyboard.leftShiftKey.isPressed ? runSpeed : walkSpeed;
            if (keyboard.leftShiftKey.isPressed)
                _survival.DrainEnergy(8f * Time.deltaTime);

            _controller.Move(move * (speed * Time.deltaTime));

            if (_controller.isGrounded && _velocity.y < 0)
                _velocity.y = -2f;

            if (keyboard.spaceKey.wasPressedThisFrame && _controller.isGrounded)
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (_jetpackUnlocked && !_controller.isGrounded && keyboard.spaceKey.isPressed)
            {
                if (_survival.TrySpendEnergy(jetpackEnergyPerSecond * Time.deltaTime))
                    _velocity.y = jetpackForce;
            }

            _velocity.y += gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }
    }
}
