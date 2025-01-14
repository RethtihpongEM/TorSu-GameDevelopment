using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float timeBetweenSpawnsType1 = 5f; // Spawn interval for zombie type 1
    [SerializeField] private float timeBetweenSpawnsType2 = 10f; // Spawn interval for zombie type 2
    private float timeSinceLastSpawnType1;
    private float timeSinceLastSpawnType2;

    [SerializeField] private Zombie zombiePrefabType1; // Prefab for zombie type 1
    [SerializeField] private Zombie zombiePrefabType2; // Prefab for zombie type 2

    private IObjectPool<Zombie> zombiePoolType1;
    private IObjectPool<Zombie> zombiePoolType2;

    void Awake()
    {
        zombiePoolType1 = new ObjectPool<Zombie>(() => CreateZombie(zombiePrefabType1), OnGet, OnRelease);
        zombiePoolType2 = new ObjectPool<Zombie>(() => CreateZombie(zombiePrefabType2), OnGet, OnRelease);
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

    private Zombie CreateZombie(Zombie prefab)
    {
        Zombie zombie = Instantiate(prefab);
        zombie.SetPool(zombiePoolType1); // Set the pool
        return zombie;
    }

    void Update()
    {
        // Spawn zombie type 1
        if (Time.time >= timeSinceLastSpawnType1)
        {
            zombiePoolType1.Get();
            timeSinceLastSpawnType1 = Time.time + timeBetweenSpawnsType1;
        }

        // Spawn zombie type 2
        if (Time.time >= timeSinceLastSpawnType2)
        {
            zombiePoolType2.Get();
            timeSinceLastSpawnType2 = Time.time + timeBetweenSpawnsType2;
        }
    }
}
