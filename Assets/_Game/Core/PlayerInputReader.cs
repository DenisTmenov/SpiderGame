using UnityEngine;
using UnityEngine.InputSystem;

namespace SpiderGame.Core
{
    public class PlayerInputReader : MonoBehaviour
    {
        [Header("Input Actions")]
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference jumpAction;
        [SerializeField] private InputActionReference sprintAction;

        public Vector2 MoveInput { get; private set; }
        public bool JumpPressed { get; private set; }
        public bool SprintHeld { get; private set; }

        private void OnEnable()
        {
            if (!ValidateInputActions())
                return;

            moveAction.action.Enable();
            jumpAction.action.Enable();
            sprintAction.action.Enable();

            moveAction.action.performed += OnMove;
            moveAction.action.canceled += OnMove;

            jumpAction.action.performed += OnJump;

            sprintAction.action.performed += OnSprint;
            sprintAction.action.canceled += OnSprint;
        }

        private void OnDisable()
        {
            if (moveAction != null && moveAction.action != null)
            {
                moveAction.action.performed -= OnMove;
                moveAction.action.canceled -= OnMove;
                moveAction.action.Disable();
            }

            if (jumpAction != null && jumpAction.action != null)
            {
                jumpAction.action.performed -= OnJump;
                jumpAction.action.Disable();
            }

            if (sprintAction != null && sprintAction.action != null)
            {
                sprintAction.action.performed -= OnSprint;
                sprintAction.action.canceled -= OnSprint;
                sprintAction.action.Disable();
            }

            MoveInput = Vector2.zero;
            JumpPressed = false;
            SprintHeld = false;
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            MoveInput = ctx.ReadValue<Vector2>();
        }

        private void OnJump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                JumpPressed = true;
            }
        }

        private void OnSprint(InputAction.CallbackContext ctx)
        {
            SprintHeld = ctx.ReadValueAsButton();
        }

        public bool ConsumeJumpPressed()
        {
            if (!JumpPressed)
                return false;

            JumpPressed = false;
            return true;
        }

        private bool ValidateInputActions()
        {
            if (moveAction == null || moveAction.action == null)
            {
                Debug.LogError(
                    $"{nameof(PlayerInputReader)}: Move action is not assigned.",
                    this
                );

                return false;
            }

            if (jumpAction == null || jumpAction.action == null)
            {
                Debug.LogError(
                    $"{nameof(PlayerInputReader)}: Jump action is not assigned.",
                    this
                );

                return false;
            }

            if (sprintAction == null || sprintAction.action == null)
            {
                Debug.LogError(
                    $"{nameof(PlayerInputReader)}: Sprint action is not assigned.",
                    this
                );

                return false;
            }

            return true;
        }
    }
}