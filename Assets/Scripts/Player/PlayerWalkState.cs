using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    float playerSpeed;

    float moveDirection;

    public override void EnterState(PlayerStateManager player)
    {
        playerSpeed = player.playerSpeed;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        moveDirection = Input.GetAxisRaw("Horizontal");
        if (moveDirection == 0)
        {
            player.ChangeState(player.IdleState);
        }
        else if (Input.GetButtonDown("Jump"))
        {
            player.ChangeState(player.JumpState);
        }

        player.transform.Translate(moveDirection * playerSpeed * Time.deltaTime, 0, 0);
    }
}