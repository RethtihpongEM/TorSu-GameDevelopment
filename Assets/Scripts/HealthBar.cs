using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Image _healthbarSprite;

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        if (_healthbarSprite == null)
        {
            Debug.LogError("Healthbar sprite is not assigned!");
            return;
        }

        _healthbarSprite.fillAmount = currentHealth / maxHealth;
    }
}
