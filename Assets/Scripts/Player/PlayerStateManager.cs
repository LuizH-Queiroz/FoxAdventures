using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public float playerSpeed;
    public float jumpForce;
    public SpriteRenderer sprite;
    public Animator animator;

    PlayerBaseState currentState;
    public PlayerBaseState IdleState = new PlayerIdleState();
    public PlayerBaseState WalkState = new PlayerWalkState();
    public PlayerBaseState JumpState = new PlayerJumpState();
    public PlayerBaseState FallState = new PlayerFallState();

    public enum STATES
    {
        IDLE,
        WALK,
        JUMP,
        FALL
    }


    // Start is called before the first frame update
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

    public void ChangeState(PlayerBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void SetSpriteDirection(float moveDirection)
    {
        if (moveDirection == -1)
        {
            sprite.flipX = true;
        }
        else if (moveDirection == 1)
        {
            sprite.flipX = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy") == false)
        {
            return;
        }

        if (currentState.GetType() == typeof(PlayerFallState))
        {
            ChangeState(JumpState);

            if (collision.gameObject.GetComponent<Eagle>() != null)
            {
                StartCoroutine(collision.gameObject.GetComponent<Eagle>().Die());
            }
            else if (collision.gameObject.GetComponent<FrogStateManager>() != null)
            {
                StartCoroutine(collision.gameObject.GetComponent<FrogStateManager>().Die());
            }
            else
            {
                StartCoroutine(collision.gameObject.GetComponent<Opossum>().Die());
            }
        }
    }
}
