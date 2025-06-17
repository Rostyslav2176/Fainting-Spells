using UnityEngine;

public class SkullChaseState : State
{
    [Header("State References")]
    public SkullExplodeState explodeState;

    [Header("Chase Settings")]
    public Transform player;
    public Transform skullBody;
    public float initialChaseSpeed = 2f;
    public float maxChaseSpeed = 6f;
    public float accelerationRate = 0.5f;
    public float stoppingDistance = 1.5f;

    [Header("Floating Animation")]
    public float verticalAmplitude = 0.5f;
    public float verticalSpeed = 2f;

    private float currentChaseSpeed;
    private Vector3 startLocalPos;
    public bool closeToPlayer;

    private void Start()
    {
        if (skullBody == null || player == null)
        {
            Debug.LogError("Missing SkullBody or Player reference!");
            return;
        }

        startLocalPos = skullBody.localPosition;
        currentChaseSpeed = initialChaseSpeed;
    }

    public override State RunCurrentState()
    {
        Debug.Log("In Chase State");

        if (player == null || skullBody == null)
            return this;

        // Accelerate chase speed
        currentChaseSpeed = Mathf.Min(currentChaseSpeed + accelerationRate * Time.deltaTime, maxChaseSpeed);

        // Move horizontally toward the player
        Vector3 horizontalTarget = new Vector3(player.position.x, skullBody.position.y, player.position.z);
        skullBody.position = Vector3.MoveTowards(skullBody.position, horizontalTarget, currentChaseSpeed * Time.deltaTime);

        // Smoothly float to player's height
        float floatOffset = Mathf.Sin(Time.time * verticalSpeed) * verticalAmplitude;
        float desiredY = player.position.y + floatOffset;
        float smoothY = Mathf.Lerp(skullBody.position.y, desiredY, Time.deltaTime * 5f);
        skullBody.position = new Vector3(skullBody.position.x, smoothY, skullBody.position.z);

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
