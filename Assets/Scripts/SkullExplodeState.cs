using UnityEngine;

public class SkullExplodeState : State
{
    public override State RunCurrentState()
    {
        Debug.Log("Exploded");
        return this;
    }
}
