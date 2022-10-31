using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStateManager : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    public float playerSpeed;
    public float jumpForce;

    public Rigidbody2D rigidbody;
    public SpriteRenderer sprite;
    public Animator animator;

    public enum STATES
    {
        IDLE,
        WALK,
        JUMP,
        FALL,
        ON_HIT
    }

    PlayerBaseState currentState;
    public PlayerBaseState IdleState = new PlayerIdleState();
    public PlayerBaseState WalkState = new PlayerWalkState();
    public PlayerBaseState JumpState = new PlayerJumpState();
    public PlayerBaseState FallState = new PlayerFallState();
    public PlayerBaseState OnHitState = new PlayerOnHitState();



    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

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

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth > 0)
        {
            ChangeState(OnHitState);
        }
        else
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
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
            else
            {
                TakeDamage(10);
            }
        }
    }
}
