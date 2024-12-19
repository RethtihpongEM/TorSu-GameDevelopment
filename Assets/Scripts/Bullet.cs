using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to a Zombie
        if (other.CompareTag("Zombie"))
        {
            // Get the Zombie component
            Zombie zombie = other.GetComponent<Zombie>();

            if (zombie != null)
            {
                // Log the hit with a unique ID or name of the zombie
                Debug.Log($"Bullet hit zombie with ID: {zombie.ID}");

                // Optionally, call a method on the zombie to handle damage
                zombie.TakeDamage(1);  // Example: passing a damage value of 1

                // Destroy the bullet after it hits a zombie
                Destroy(gameObject);  // This destroys the bullet object
            }
        }
    }
}
