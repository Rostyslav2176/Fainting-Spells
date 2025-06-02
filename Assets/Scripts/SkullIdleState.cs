using UnityEngine;

public class SkullIdleState : State
{
    public SkullChaseState chaseState;
    public bool canSeePlayer;
    
    public float moveSpeed = 1.0f;
    public float amplitude = 1.0f;
    
    private Vector3 startPos;
    private Transform enemyBody;
    
    void Start()
    {
        enemyBody  = transform.parent.parent.Find("SkullBody");
        
        if (enemyBody == null)
        {
            Debug.LogError("SkullBody not found! Check hierarchy.");
            return;
        }
        
        startPos = enemyBody.localPosition;
    }

    void Update()
    {
        if (enemyBody == null) return;

        float verticalMovement = Mathf.Sin(Time.time * moveSpeed) * amplitude;
        Vector3 newLocalPos = startPos + Vector3.up * verticalMovement;
        
       enemyBody.localPosition = newLocalPos; 
    }

    
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
