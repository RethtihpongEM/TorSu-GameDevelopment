using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class Zombie : MonoBehaviour
{
  private NavMeshAgent agent;
  private Transform player;
  private Animator animator;
  [SerializeField] int damage = 25;
  [SerializeField] float attackDistance = 2f;

  private IObjectPool<Zombie> zombiePool;

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
}