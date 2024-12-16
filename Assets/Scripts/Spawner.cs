using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float timeBetweenSpwawns =5f;
    private float timeSinceLastSpawn; 
    [SerializeField] private Zombie zombiePrefab;

    private IObjectPool<Zombie> zombiePool;

    void Awake()
    {
        zombiePool = new ObjectPool<Zombie>(CreateZombie, OnGet, OnRelease);
    }

    private void OnGet(Zombie zombie)
    {
        zombie.gameObject.SetActive(true);
        Transform randomSpawnPoints = spawnPoints[Random.Range(0,spawnPoints.Length)];
        zombie.transform.position= randomSpawnPoints.position;
    }

    private void OnRelease(Zombie zombie)
    {
        zombie.gameObject.SetActive(false);
    }

    private Zombie CreateZombie()
    {
        Zombie zombie = Instantiate(zombiePrefab);
        zombie.SetPool(zombiePool);
        return zombie;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > timeSinceLastSpawn)
        {
            zombiePool.Get();
            timeSinceLastSpawn = Time.time + timeBetweenSpwawns;
        }
    }
}
