using UnityEngine;

public class SkullIdleState : State
{
    public SkullChaseState chaseState;
    public bool canSeePlayer;
    
    public override State RunCurrentState()
    {
        if (canSeePlayer)
        {
            return chaseState;
        }
        else
        {
            return this;   
        }
    }
}
