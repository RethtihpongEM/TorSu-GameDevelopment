using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
  private NavMeshAgent agent;

  public int maxHealth = 100;
  public int currentHealth;

  void Start()
  {
    currentHealth = maxHealth;
    NotifyHealthChange();
    agent = GetComponent<NavMeshAgent>();
  }

  void Update()
  {

  }

  private void NotifyHealthChange()
  {
    GameManager.Instance.UpdateHealthUI(currentHealth, maxHealth); // Notify GameManager of health changes
  }

  public void TakeDamage(int damage)
  {
    currentHealth -= damage;
    NotifyHealthChange();
    Debug.Log("Player's health: ");
    Debug.Log(currentHealth);
    if (currentHealth <= 0)
    {
      Die();
    }
  }

  private void Die()
  {
    GameManager.Instance.EndGameWhenPlayerDie();
  }
}