using UnityEngine;

public class SkullChaseState : State
{
    [Header("State References")]
    public SkullExplodeState explodeState;

    [Header("Chase Settings")]
    public Transform skullBody;
    public float initialChaseSpeed = 2f;
    public float maxChaseSpeed = 6f;
    public float accelerationRate = 0.5f;
    public float stoppingDistance = 1.5f;
    public float verticalFollowRange = 10f;

    [Header("Vertical Tracking")]
    public float verticalLerpSpeed = 5f;

    private float currentChaseSpeed;
    public bool closeToPlayer;

    private Transform player;

    private void Start()
    {
        if (skullBody == null)
        {
            Debug.LogError("Missing SkullBody reference!");
            return;
        }

        currentChaseSpeed = initialChaseSpeed;

        // Find player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogError("Player with tag 'Player' not found!");
    }

    public override State RunCurrentState()
    {
        if (player == null || skullBody == null)
            return this;

        float distanceToPlayer = Vector3.Distance(skullBody.position, player.position);
        closeToPlayer = distanceToPlayer <= stoppingDistance;

        // Accelerate
        currentChaseSpeed = Mathf.Min(currentChaseSpeed + accelerationRate * Time.deltaTime, maxChaseSpeed);

        // Determine desired Y (same level by default, match player Y if within range)
        float targetY = skullBody.position.y;
        if (distanceToPlayer <= verticalFollowRange)
        {
            targetY = player.position.y;
        }

        // Smoothly move to desired Y
        float smoothY = Mathf.Lerp(skullBody.position.y, targetY, Time.deltaTime * verticalLerpSpeed);

        // Final target position
        Vector3 targetPosition = new Vector3(player.position.x, smoothY, player.position.z);
        skullBody.position = Vector3.MoveTowards(skullBody.position, targetPosition, currentChaseSpeed * Time.deltaTime);

        // Rotate to face player
        Vector3 directionToPlayer = player.position - skullBody.position;
        directionToPlayer.y = 0f;
        if (directionToPlayer.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            skullBody.rotation = Quaternion.Slerp(skullBody.rotation, targetRotation, Time.deltaTime * 5f);
        }

        return closeToPlayer ? explodeState : this;
    }
}
