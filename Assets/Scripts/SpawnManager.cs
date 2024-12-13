using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public string zombieTag;

    // Define spawn range
    public float spawnRangeX = 10f; // Range for X-axis
    public float spawnRangeZ = 10f; // Range for Z-axis
    public float spawnY = 5f;       // Fixed Y-axis position (ground level)

    public int maxZombies = 10;       // Maximum number of zombies allowed
    private int currentZombieCount = 0; // Current number of zombies
    private List<GameObject> activeZombies = new List<GameObject>(); // Track active zombies

    void Start()
    {
        StartCoroutine(SpawnZombiesWithDelay());
    }

    IEnumerator SpawnZombiesWithDelay()
    {
        while (currentZombieCount < maxZombies)
        {
            // Spawn 2 zombies in each iteration
            for (int i = 0; i < 2 && currentZombieCount < maxZombies; i++)
            {
                SpawnZombie();
            }

            // Wait for 0.5 seconds before the next iteration
            yield return new WaitForSeconds(0.5f);
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

            // Define a rotation of 180 degrees on the Y-axis
            Quaternion spawnRotation = Quaternion.Euler(0, 180, 0);

            // Spawn the zombie with the specified rotation and add it to the active list
            GameObject zombie = ObjectPooler.Instance.SpawnFromPool(zombieTag, spawnPosition, spawnRotation);
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

        // Ensure zombies are maintained by restarting the coroutine
        if (currentZombieCount < maxZombies)
        {
            StartCoroutine(SpawnZombiesWithDelay());
        }
    }
}
