using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogStateManager : MonoBehaviour
{
    public enum STATES
    {
        IDLE,
        JUMP,
        FALL
    }

    FrogBaseState currentState;
    public FrogBaseState IdleState = new FrogIdleState();
    public FrogBaseState JumpState = new FrogJumpState();
    public FrogBaseState FallState = new FrogFallState();

    public SpriteRenderer sprite;
    public Animator animator;
    public Rigidbody2D rigidbody;
    public float jumpForceUp;
    public float jumpForceLateral;


    void Start()
    {
        currentState = IdleState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void ChangeState(FrogBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void SetSpriteDirection(float moveDirection)
    {
        if (moveDirection == -1)
        {
            sprite.flipX = false;
        }
        else if (moveDirection == 1)
        {
            sprite.flipX = true;
        }
    }

    public IEnumerator Die()
    {
        foreach (var collider in gameObject.GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }
        rigidbody.Sleep();
        ChangeState(IdleState); // for the death animation not to move
        animator.SetTrigger("Death");

        yield return new WaitForSeconds(1);

        Destroy(gameObject);
    }
}
