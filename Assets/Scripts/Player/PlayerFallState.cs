using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    float playerSpeed;
    Rigidbody2D rigidbody;

    float moveDirection;

    public override void EnterState(PlayerStateManager player)
    {
        playerSpeed = player.playerSpeed;
        rigidbody = player.GetComponent<Rigidbody2D>();
    }

    public override void UpdateState(PlayerStateManager player)
    {
        moveDirection = Input.GetAxisRaw("Horizontal");

        player.transform.Translate(moveDirection * playerSpeed * Time.deltaTime, 0, 0);

        if (rigidbody.velocity.y == 0)
        {
            if (moveDirection != 0)
            {
                player.ChangeState(player.WalkState);
            }
            else
            {
                player.ChangeState(player.IdleState);
            }
        }
    }
}
