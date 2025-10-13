
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Good: Prevents player from falling if gravity is not needed
        rb.gravityScale = 0f; 
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Read input from the keyboard (WASD or Arrows)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized; // Keep movement speed consistent diagonally

        bool isMoving = movement != Vector2.zero;
        if (animator != null) animator.SetBool("isMoving", isMoving);
    }

    void FixedUpdate()
    {
        // Move the Rigidbody using physics (best practice for movement)
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        
        // Handle rotation to face the direction of movement
        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }
    }
}