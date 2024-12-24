using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float timeBetweenSpawns = 5f; // General spawn interval
    [SerializeField] private float timeForSpecialZombie = 10f; // Interval for 4th prefab zombie
    private float timeSinceLastSpawn;
    private float timeSinceLastSpecialSpawn;

    [SerializeField] private Zombie[] zombiePrefabs; // Array of zombie prefabs
    private IObjectPool<Zombie> zombiePool;

    void Awake()
    {
        zombiePool = new ObjectPool<Zombie>(CreateZombie, OnGet, OnRelease);
    }

    private void OnGet(Zombie zombie)
    {
        zombie.gameObject.SetActive(true);
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        zombie.transform.position = randomSpawnPoint.position;
    }

    private void OnRelease(Zombie zombie)
    {
        zombie.gameObject.SetActive(false);
    }

    private Zombie CreateZombie()
    {
        // Randomly select a zombie prefab from the array for normal spawns
        Zombie selectedPrefab = zombiePrefabs[Random.Range(0, zombiePrefabs.Length - 1)];
        Zombie zombie = Instantiate(selectedPrefab);
        zombie.SetPool(zombiePool);
        return zombie;
    }

    private Zombie CreateSpecialZombie()
    {
        // Select the 4th prefab explicitly
        Zombie specialZombiePrefab = zombiePrefabs[^1];
        Zombie zombie = Instantiate(specialZombiePrefab);
        zombie.SetPool(zombiePool);
        return zombie;
    }

    void Start()
    {
        timeSinceLastSpawn = Time.time;
        timeSinceLastSpecialSpawn = Time.time;
    }

    void Update()
    {
        // Spawn regular zombies at the default interval
        if (Time.time > timeSinceLastSpawn)
        {
            zombiePool.Get();
            timeSinceLastSpawn = Time.time + timeBetweenSpawns;
        }

        // Spawn the 4th prefab zombie at its specific interval
        if (Time.time > timeSinceLastSpecialSpawn)
        {
            Zombie specialZombie = CreateSpecialZombie();
            OnGet(specialZombie);
            timeSinceLastSpecialSpawn = Time.time + timeForSpecialZombie;
        }
    }
}
