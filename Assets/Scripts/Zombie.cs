using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class Zombie : MonoBehaviour
{
  private NavMeshAgent agent;
  private Transform player;
  private Animator animator;
  [SerializeField] float damage = 2f;
  [SerializeField] float attackDistance = 2f;

  private IObjectPool<Zombie> zombiePool;
  public int ID { get; private set; }
  private static int nextID = 1;

  private int health = 10;
  private int hitCount = 0;  // Track how many times the zombie has been hit
  public static int killedZombieCount = 0; // Static counter for the number of killed zombies

  public void SetPool(IObjectPool<Zombie> pool)
  {
    zombiePool = pool;
  }

  void Start()
  {
    agent = GetComponent<NavMeshAgent>();
    animator = GetComponent<Animator>();
    GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
    if (playerObject != null)
    {
      player = playerObject.transform;
    }
    else
    {
      Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
    }
  }

  void Update()
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
      agent.SetDestination(player.position);

      animator.SetBool("IsAttacking", false);
      animator.SetBool("IsWalking", true);
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

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      Debug.Log("Zombie reached the player!");
    }
  }

  private void Awake()
  {
    // Assign a unique ID to each zombie
    ID = nextID;
    nextID++;
  }

  public void TakeDamage(int damage)
  {
    // Decrease health by the damage value
    health -= damage;
    hitCount++;  // Increment the hit counter

    // Log the damage and the total number of hits
    Debug.Log($"Zombie ID: {ID} took {damage} damage. Remaining health: {health}. Total hits: {hitCount}");

    // Check if the zombie is dead (health <= 0)
    if (health <= 0)
    {
      // Increment the killed zombie counter
      killedZombieCount++;

      // Log the death of the zombie
      Debug.Log($"Zombie ID: {ID} has been killed!, Total zombies killed: {killedZombieCount}");

      // Destroy the zombie game object
      Destroy(gameObject);
    }
  }
}