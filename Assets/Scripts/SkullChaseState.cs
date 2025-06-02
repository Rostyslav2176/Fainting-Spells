using UnityEngine;

public class SkullChaseState : State
{
    public SkullExplodeState explodeState;
    public bool closeToPlayer;
    
    public override State RunCurrentState()
    {
        Debug.Log("Skull chase state");
        if (closeToPlayer)
        {
            return explodeState;
        }
        else
        {
            return this;   
        }
    }
}
