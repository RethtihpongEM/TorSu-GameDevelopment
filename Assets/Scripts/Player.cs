using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
  private NavMeshAgent agent;

  public int maxHealth = 100;
  public int currentHealth;
  [SerializeField] private Animator _animator;

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
    if (currentHealth <= 0)
    {
      Die();
    } 
  }

  private void Die()
  {
    _animator.SetBool("IsDeath", true);
    GameManager.Instance.TriggerLose();
  }

  // private void Alive()
  // {
  //   _animator.SetBool("IsDeath", false);
  //   GameManager.Instance.TriggerTimeUp();
  // }
}