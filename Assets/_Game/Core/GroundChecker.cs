using UnityEngine;

namespace SpiderGame.Core
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerStateMachine))]
    public class GroundChecker : MonoBehaviour
    {
        [Header("Ground Check")]
        [SerializeField] private float groundCheckDistance = 0.1f;
        [SerializeField] private float sphereRadiusMultiplier = 0.9f;
        [SerializeField] private LayerMask groundMask = ~0;

        private CharacterController controller;
        private PlayerStateMachine stateMachine;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            stateMachine = GetComponent<PlayerStateMachine>();
        }

        private void Update()
        {
            bool grounded = controller.isGrounded;

            if (grounded)
            {
                grounded = HasGroundContact();
            }

            stateMachine.SetGrounded(grounded);
        }

        private bool HasGroundContact()
        {
            Vector3 origin = transform.position + Vector3.up * 0.1f;

            float radius = controller.radius * sphereRadiusMultiplier;
            float distance = groundCheckDistance + 0.1f;

            return Physics.SphereCast(
                origin,
                radius,
                Vector3.down,
                out _,
                distance,
                groundMask,
                QueryTriggerInteraction.Ignore
            );
        }
    }
}