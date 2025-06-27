using UnityEngine;

public class TutorialPortalSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Vector3 spawnOffset = Vector3.zero;

    public void Spawn()
    {
        if (objectToSpawn != null)
        {
            Instantiate(objectToSpawn, transform.position + spawnOffset, Quaternion.identity);
        }
    }
}
