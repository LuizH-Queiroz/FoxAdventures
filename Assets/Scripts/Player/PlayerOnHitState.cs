using UnityEngine;

public class PlayerOnHitState : PlayerBaseState
{
    float startTime;

    public override void EnterState(PlayerStateManager player)
    {
        startTime = Time.time;
        Bump(player);

        player.animator.SetInteger("State", (int) PlayerStateManager.STATES.ON_HIT);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if (Time.time - startTime >= 0.8)
        {
            player.ChangeState(player.IdleState);
        }
    }

    void Bump(PlayerStateManager player)
    {
        player.rigidbody.velocity = new Vector2(6 * (player.sprite.flipX ? 1 : -1), 12);
    }
}
