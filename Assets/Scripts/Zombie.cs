using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class Zombie : MonoBehaviour
{
  private NavMeshAgent agent;
  private Transform player;
  private Animator animator;
  [SerializeField] int damage = 5;
  [SerializeField] float attackDistance = 2f;
  [SerializeField] private GameObject healthBarPrefab; // Reference to the health bar prefab
  [SerializeField] private GameObject coinPrefab;
  private IObjectPool<Zombie> zombiePool;
  private HealthBar healthBar; // Reference to the health bar script

  public int maxHealth = 10;
  private int _currentHealth;
  private int hitCount = 0; // Track how many times the zombie has been hit

  public int ID { get; private set; }
  private static int nextID = 1;

  // [SerializeField] int health = 10;

  public void SetPool(IObjectPool<Zombie> pool)
  {
    zombiePool = pool;
  }

  private void Awake()
  {
    // Assign a unique ID to each zombie
    ID = nextID;
    nextID++;
  }

  void Start()
  {
    agent = GetComponent<NavMeshAgent>();
    animator = GetComponent<Animator>();

    // Initialize current health
    _currentHealth = maxHealth;

    // Find the player by tag
    GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
    if (playerObject != null)
    {
      player = playerObject.transform;
    }
    else
    {
      Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
    }

    // Instantiate health bar
    Vector3 healthBarPosition = transform.position + Vector3.up * 2; // Position above the zombie
    GameObject healthBarObject = Instantiate(healthBarPrefab, healthBarPosition, Quaternion.identity, transform);

    healthBar = healthBarObject.GetComponent<HealthBar>();
    if (healthBar == null)
    {
      Debug.LogError("Healthbar script not found on the instantiated prefab! Make sure the prefab has the Healthbar script attached.");
    }

    // Set the initial health in the health bar
    healthBar?.UpdateHealthBar(maxHealth, _currentHealth);
  }

  void Update()
  {
    // Update zombie movement
    if (player != null)
    {
      agent.SetDestination(player.position);
      float distanceToPlayer = Vector3.Distance(transform.position, player.position);

      if (distanceToPlayer <= attackDistance)
      {
        // Stop moving and play attack animation
        agent.isStopped = true;
        animator.SetBool("IsAttacking", true);
      }
      else
      {
        // Move towards the player and play walking animation
        agent.isStopped = false;
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsWalking", true);
      }
    }

    // Keep the health bar facing the camera
    if (healthBar != null)
    {
      Vector3 directionToCamera = Camera.main.transform.position - healthBar.transform.position;
      directionToCamera.y = 0; // Prevent rotation on the Y-axis
      healthBar.transform.rotation = Quaternion.LookRotation(directionToCamera);
    }
  }

  public void DealDamage() // Called by the animation event
  {
    if (player != null)
    {
      Player playerComponent = player.GetComponent<Player>();
      if (playerComponent != null)
      {
        playerComponent.TakeDamage(damage);
        Debug.Log("Player damaged!");
      }
    }
  }

  public void TakeDamage(int damage)
  {
    _currentHealth -= damage;
    hitCount++;

    // Update health bar
    if (healthBar != null)
    {
      healthBar.UpdateHealthBar(maxHealth, _currentHealth);
    }

    if (_currentHealth <= 0)
    {
      // Destroy the zombie game object
      Destroy(gameObject);
      SpawnCoin();
      GameManager.Instance.IncrementZombieKillCount();

    }
  }

  private void SpawnCoin()
  {
    if (coinPrefab != null)
    {
      // Adjust the position to raise the coin along the Y-axis
      Vector3 spawnPosition = transform.position;
      spawnPosition.y += 1.0f; // Adjust Y-axis offset if needed

      // Set a rotation of 90 degrees on the X-axis
      Quaternion spawnRotation = Quaternion.Euler(90f, 0f, 0f);

      // Instantiate the coin with the adjusted position and rotation
      Instantiate(coinPrefab, spawnPosition, spawnRotation);
    }
    else
    {
      Debug.LogWarning("Coin prefab is not assigned!");

    }
  }
}
