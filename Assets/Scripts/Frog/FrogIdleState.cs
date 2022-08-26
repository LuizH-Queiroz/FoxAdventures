using UnityEngine;

public class FrogIdleState : FrogBaseState
{
    float idleTime; // amount of time the frog will stay idle
    float timer;

    public override void EnterState(FrogStateManager frog)
    {
        idleTime = Random.Range(2, 7);
        timer = 0;

        frog.SetSpriteDirection(Random.Range(-1, 2) <= 0 ? -1 : 1);

        frog.animator.SetInteger("State", (int) FrogStateManager.STATES.IDLE);
    }

    public override void UpdateState(FrogStateManager frog)
    {
        timer += Time.deltaTime;

        if (timer >= idleTime)
        {
            frog.ChangeState(frog.JumpState);
        }
    }
}
