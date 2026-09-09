using UnityEngine;
using SpiderGame.Core;

namespace SpiderGame.Movement
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputReader))]
    [RequireComponent(typeof(PlayerStateMachine))]
    public class CharacterMover : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float walkSpeed = 4f;
        [SerializeField] private float sprintSpeed = 7f;
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float deceleration = 10f;
        [SerializeField] private float rotationSpeed = 10f;

        [Header("Jump / Gravity")]
        [SerializeField] private float jumpHeight = 1.5f;
        [SerializeField] private float gravity = -20f;

        private CharacterController controller;
        private PlayerInputReader input;
        private PlayerStateMachine state;

        private Vector3 velocity;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            input = GetComponent<PlayerInputReader>();
            state = GetComponent<PlayerStateMachine>();
        }

        private void Update()
        {
            HandleMovement();
            HandleJump();
            HandleGravity();
            ApplyMovement();
        }

        private void HandleMovement()
        {
            Vector2 inputVector = input.MoveInput;

            Vector3 inputDirection = new Vector3(
                inputVector.x,
                0f,
                inputVector.y
            );

            Vector3 desiredDirection = Vector3.zero;

            if (inputDirection.sqrMagnitude > 0.001f)
            {
                desiredDirection = transform.TransformDirection(inputDirection).normalized;
            }

            float targetSpeed = input.SprintHeld
                ? sprintSpeed
                : walkSpeed;

            Vector3 targetVelocity = desiredDirection * targetSpeed;

            Vector3 horizontalVelocity = new Vector3(
                velocity.x,
                0f,
                velocity.z
            );

            float speedChange = desiredDirection.sqrMagnitude > 0.001f
                ? acceleration
                : deceleration;

            horizontalVelocity = Vector3.MoveTowards(
                horizontalVelocity,
                targetVelocity,
                speedChange * Time.deltaTime
            );

            velocity.x = horizontalVelocity.x;
            velocity.z = horizontalVelocity.z;

            if (desiredDirection.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(
                    desiredDirection,
                    Vector3.up
                );

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }

        private void HandleJump()
        {
            if (state.IsGrounded && input.ConsumeJumpPressed())
            {
                velocity.y = Mathf.Sqrt(
                    jumpHeight * -2f * gravity
                );
            }
        }

        private void HandleGravity()
        {
            if (state.IsGrounded && velocity.y < 0f)
            {
                velocity.y = -2f;
            }

            velocity.y += gravity * Time.deltaTime;
        }

        private void ApplyMovement()
        {
            controller.Move(velocity * Time.deltaTime);
        }
    }
}