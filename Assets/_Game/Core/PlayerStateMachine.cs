using UnityEngine;

namespace SpiderGame.Core
{
    public enum PlayerState
    {
        Grounded,
        Airborne
    }

    public class PlayerStateMachine : MonoBehaviour
    {
        public PlayerState CurrentState { get; private set; } = PlayerState.Grounded;

        public bool IsGrounded => CurrentState == PlayerState.Grounded;
        public bool IsAirborne => CurrentState == PlayerState.Airborne;

        public void SetGrounded(bool grounded)
        {
            var newState = grounded ? PlayerState.Grounded : PlayerState.Airborne;
            if (newState != CurrentState)
            {
                CurrentState = newState;
            }
        }
    }
}
