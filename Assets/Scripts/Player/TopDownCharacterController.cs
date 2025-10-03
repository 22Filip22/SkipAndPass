using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class TopDownCharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    float movementSpeedScale = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalize diagonal movement
        movement = movement.normalized;

        if (movement.x > 0)
        {
            spriteRenderer.flipX = false;

        } else if (movement.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        animator.SetFloat("xMovement", movement.x);
        animator.SetFloat("yMovement", movement.y);
    }



    void FixedUpdate()
    {
        // Movement
        rb.MovePosition(rb.position + movement * (moveSpeed * movementSpeedScale) * Time.fixedDeltaTime);
    }
}
