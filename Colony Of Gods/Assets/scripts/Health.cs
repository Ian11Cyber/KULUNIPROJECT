using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Events")]
    public UnityEvent<int,int> onHealthChanged; // (current, max)
    public UnityEvent onDeath;
    public UnityEvent<int> onDamaged; // damage amount

    public bool IsDead => currentHealth <= 0;

    void Awake()
    {
        currentHealth = Mathf.Clamp(currentHealth <= 0 ? maxHealth : currentHealth, 0, maxHealth);
        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (IsDead) return;
        currentHealth = Mathf.Max(0, currentHealth - Mathf.Max(0, amount));
        onDamaged?.Invoke(amount);
        onHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (IsDead) return;
        currentHealth = Mathf.Min(maxHealth, currentHealth + Mathf.Max(0, amount));
        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Die()
    {
        onDeath?.Invoke();
        // Optional: disable collider/AI immediately
        var col = GetComponent<Collider2D>(); if (col) col.enabled = false;
        var rb = GetComponent<Rigidbody2D>(); if (rb) rb.simulated = false;
        // Destroy by default if you don't have a death animation:
        Destroy(gameObject, 0.05f);
    }
}
