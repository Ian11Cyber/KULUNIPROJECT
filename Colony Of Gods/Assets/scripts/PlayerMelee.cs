using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [Header("Attack")]
    public int damage = 15;
    public float attackRate = 3f;          // swings per second
    public float attackRadius = 0.6f;      // circle size
    public LayerMask enemyLayers;          // set to Enemy
    public Transform attackPoint;          // empty child transform in front of player
    public float knockbackForce = 4f;

    float _nextAttackTime;

    void Update()
    {
        bool pressed = Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0);
        if (!pressed) return;
        if (Time.time < _nextAttackTime) return;

        DoAttack();
        _nextAttackTime = Time.time + 1f / attackRate;
    }

    void DoAttack()
    {
        Vector2 center = (attackPoint ? (Vector2)attackPoint.position : (Vector2)transform.position);

        // OverlapCircleAll expects a Vector2 center
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, attackRadius, enemyLayers);

        foreach (var h in hits)
        {
            var hp = h.GetComponentInParent<Health>();
            if (hp != null && !hp.IsDead)
            {
                hp.TakeDamage(damage);

                // knockback (optional)
                var rb = h.attachedRigidbody;
                if (rb)
                {
                    Vector2 targetCenter = (Vector2)h.bounds.center;
                    Vector2 dir = (targetCenter - center).normalized;
                    rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    // Draw the attack circle in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 p3 = attackPoint ? attackPoint.position : transform.position; // Gizmos uses Vector3
        Gizmos.DrawWireSphere(p3, attackRadius);
    }
}
