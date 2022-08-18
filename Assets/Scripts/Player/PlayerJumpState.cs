using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    float jumpForce;
    float playerSpeed;

    Rigidbody2D rigidbody;
    float moveDirection;

    public override void EnterState(PlayerStateManager player)
    {
        jumpForce = player.jumpForce;
        playerSpeed = player.playerSpeed;
        rigidbody = player.GetComponent<Rigidbody2D>();

        rigidbody.velocity = new Vector2(0, jumpForce);

        player.animator.SetInteger("State", (int) PlayerStateManager.STATES.JUMP);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        moveDirection = Input.GetAxisRaw("Horizontal");
        player.SetSpriteDirection(moveDirection);
        player.transform.Translate(moveDirection * playerSpeed * Time.deltaTime, 0, 0);

        if (rigidbody.velocity.y < 0)
        {
            player.ChangeState(player.FallState);
        }
    }
}
