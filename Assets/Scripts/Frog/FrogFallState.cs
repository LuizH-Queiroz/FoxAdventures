using UnityEngine;

public class FrogFallState : FrogBaseState
{
    float timer = 0;

    public override void EnterState(FrogStateManager frog)
    {
        timer = 0;

        frog.animator.SetInteger("State", (int) FrogStateManager.STATES.FALL);
    }

    public override void UpdateState(FrogStateManager frog)
    {
        if (frog.rigidbody.velocity.y == 0)
        {
            frog.animator.SetInteger("State", 0); // although we're not in IdleState, we want
                                                  // the Idle animation to be played
            timer += Time.deltaTime;

            if (timer >= 2)
            {
                frog.ChangeState(frog.IdleState);
            }
        }
    }
}
