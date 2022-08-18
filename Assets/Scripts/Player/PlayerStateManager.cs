using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public float playerSpeed;
    public float jumpForce;

    PlayerBaseState currentState;
    public PlayerBaseState IdleState = new PlayerIdleState();
    public PlayerBaseState WalkState = new PlayerWalkState();
    public PlayerBaseState JumpState = new PlayerJumpState();
    public PlayerBaseState FallState = new PlayerFallState();

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
}
