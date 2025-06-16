using UnityEngine;

public class SkullChaseState : State
{
    public SkullExplodeState explodeState;
    public bool closeToPlayer;
    
    public override State RunCurrentState()
    {
        //Do the chase logic here
        Debug.Log("In Chase State");
        return closeToPlayer ? explodeState : this;
    }
}
