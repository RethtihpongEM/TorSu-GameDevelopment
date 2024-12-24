using UnityEngine;

public class Bullet : MonoBehaviour
{
    private PlayerController _playerController;

    private void Start()
    {
        // Find the PlayerController in the scene
        _playerController = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            Zombie zombie = other.GetComponent<Zombie>();

            if (zombie != null)
            {
                Debug.Log($"Bullet hit zombie with ID: {zombie.ID}");
                zombie.TakeDamage(1);
            }

            // Return the bullet to the pool
            if (_playerController != null)
            {
                _playerController.ReturnBulletToPool(gameObject);
            }
        }
    }

    private void OnBecameInvisible()
    {
        // Return the bullet to the pool when it goes off-screen
        if (_playerController != null)
        {
            _playerController.ReturnBulletToPool(gameObject);
        }
    }
}
