using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
  private NavMeshAgent agent;
  private float health = 10f;

  void Start()
  {
    agent = GetComponent<NavMeshAgent>();
  }

  void Update()
  {
    
  }

  public void TakeDamage(float damage)
  {
    health -= damage;
    Debug.Log("Player's health: ");
    Debug.Log(health);
  }
}