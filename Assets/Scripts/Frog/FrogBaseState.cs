using UnityEngine;

public abstract class FrogBaseState
{
    public abstract void EnterState(FrogStateManager frog);

    public abstract void UpdateState(FrogStateManager frog);
}
