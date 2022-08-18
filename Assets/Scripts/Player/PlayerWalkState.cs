using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    float playerSpeed;
    Rigidbody2D rigidbody;

    float moveDirection;

    public override void EnterState(PlayerStateManager player)
    {
        playerSpeed = player.playerSpeed;
        rigidbody = player.GetComponent<Rigidbody2D>();

        player.animator.SetInteger("State", (int) PlayerStateManager.STATES.WALK);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        moveDirection = Input.GetAxisRaw("Horizontal");
        player.SetSpriteDirection(moveDirection);

        if (rigidbody.velocity.y < 0)
        {
            player.ChangeState(player.FallState);
        }
        else if (moveDirection == 0)
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