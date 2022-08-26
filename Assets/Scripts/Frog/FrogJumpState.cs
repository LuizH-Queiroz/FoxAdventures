using UnityEngine;

public class FrogJumpState : FrogBaseState
{

    public override void EnterState(FrogStateManager frog)
    {
        // making frog jump, remembering to make lateral force negative when needed
        frog.rigidbody.velocity = new Vector2(frog.jumpForceLateral * (frog.sprite.flipX? 1 : -1), frog.jumpForceUp);

        frog.animator.SetInteger("State", (int) FrogStateManager.STATES.JUMP);
    }

    public override void UpdateState(FrogStateManager frog)
    {
        if (frog.rigidbody.velocity.y <= 0)
        {
            frog.ChangeState(frog.FallState);
        }
    }
}
