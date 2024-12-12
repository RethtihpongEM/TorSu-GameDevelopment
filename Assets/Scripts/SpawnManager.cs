using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public string zombieTag;

    // Define spawn range
    public float spawnRangeX = 10f; // Range for X-axis
    public float spawnRangeZ = 10f; // Range for Z-axis
    public float spawnY = 0f;       // Fixed Y-axis position (ground level)

    void Start()
    {
        // Call SpawnZombie repeatedly every 5 seconds after a 2-second delay
        InvokeRepeating(nameof(SpawnZombie), 2f, 5f);
    }

    void SpawnZombie()
    {
        // Generate a random position within the defined range
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        float randomZ = Random.Range(-spawnRangeZ, spawnRangeZ);
        Vector3 spawnPosition = new Vector3(randomX, spawnY, randomZ);

        // Spawn the zombie from the object pool
        ObjectPooler.Instance.SpawnFromPool(zombieTag, spawnPosition, Quaternion.identity);
    }
}
