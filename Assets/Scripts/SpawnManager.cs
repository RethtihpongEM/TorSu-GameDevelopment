using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public string zombieTag;

    // Define spawn range
    public float spawnRangeX = 10f; // Range for X-axis
    public float spawnRangeZ = 10f; // Range for Z-axis
    public float spawnY = 0f;       // Fixed Y-axis position (ground level)

    public int maxZombies = 10;       // Maximum number of zombies allowed
    private int currentZombieCount = 0; // Current number of zombies
    private List<GameObject> activeZombies = new List<GameObject>(); // Track active zombies

    void Start()
    {
        // Spawn initial zombies to reach the max count
        for (int i = 0; i < maxZombies; i++)
        {
            SpawnZombie();
        }
    }

    void SpawnZombie()
    {
        if (currentZombieCount < maxZombies)
        {
            // Generate a random position within the defined range
            float randomX = Random.Range(-spawnRangeX, spawnRangeX);
            float randomZ = Random.Range(-spawnRangeZ, spawnRangeZ);
            Vector3 spawnPosition = new Vector3(randomX, spawnY, randomZ);

            // Spawn the zombie and add it to the active list
            GameObject zombie = ObjectPooler.Instance.SpawnFromPool(zombieTag, spawnPosition, Quaternion.identity);
            activeZombies.Add(zombie);
            currentZombieCount++;
        }
    }

    void HandleZombieDeath(GameObject zombie)
    {
        // Remove the zombie from the active list and decrease the count
        if (activeZombies.Contains(zombie))
        {
            activeZombies.Remove(zombie);
            currentZombieCount--;
        }

        // Spawn a new zombie to maintain the count
        SpawnZombie();
    }
}
