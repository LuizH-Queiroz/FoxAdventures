using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.animator.SetInteger("State", (int) PlayerStateManager.STATES.IDLE);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            player.ChangeState(player.WalkState);
        }
        else if (Input.GetButtonDown("Jump"))
        {
            player.ChangeState(player.JumpState);
        }
    }
}
