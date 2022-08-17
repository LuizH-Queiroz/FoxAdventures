using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum STATES {
        IDLE,
        WALKING,
        JUMPING,
        FALLING
    };

    STATES currentState;

    public float playerSpeed;
    public float jumpForce;
    [SerializeField] Rigidbody2D playerRigidBody;
    float moveDirection;
    bool jumped;

    // Start is called before the first frame update
    void Start()
    {
        currentState = STATES.IDLE;
        jumped = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case STATES.IDLE:
                if (Input.GetAxisRaw("Horizontal") != 0)
                {
                    currentState = STATES.WALKING;
                }
                else if (Input.GetButtonDown("Jump"))
                {
                    currentState = STATES.JUMPING;
                }
                break;

            case STATES.WALKING:
                moveDirection = Input.GetAxisRaw("Horizontal");

                if (moveDirection != 0)
                {
                    transform.Translate(new Vector3(moveDirection * playerSpeed * Time.fixedDeltaTime, 0, 0));
                }
                else if (Input.GetButtonDown("Jump"))
                {
                    currentState = STATES.JUMPING;
                }
                break;

            case STATES.JUMPING:
                if (!jumped)
                {
                    playerRigidBody.velocity = new Vector2(0, jumpForce);
                    jumped = true;
                }
                else if (playerRigidBody.velocity.y <= 0)
                {
                    currentState = STATES.IDLE;
                    jumped = false;
                }
                break;
        }
    }
}
