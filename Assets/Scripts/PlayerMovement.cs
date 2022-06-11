using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private float horizontalMovementInput;
    private SpriteRenderer spriteRenderer;

    //* Jump
    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpTime;
    private float jumpTimeCounter;
    private bool isJumping;

    //* Coyote Jumping
    [Header("Coyote Jumping")]
    [SerializeField] private float coyoteJumpTime;
    private float coyoteJumpTimeCounter;
    //* Ground Check
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayerMask;
    private bool isGrounded;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.red, 0f);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayerMask);
        horizontalMovementInput = Input.GetAxisRaw("Horizontal");

        HandleCoyoteJump();
        HandleSpriteDirection();
        HandleJump();
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalMovementInput * speed, rb.velocity.y);
    }

    private void HandleSpriteDirection()
    {
        if (horizontalMovementInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalMovementInput < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && coyoteJumpTimeCounter > 0)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                jumpTimeCounter -= Time.deltaTime;
                rb.velocity = Vector2.up * jumpForce;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            coyoteJumpTimeCounter = 0;
        }
    }

    private void HandleCoyoteJump()
    {
        if (isGrounded)
        {
            coyoteJumpTimeCounter = coyoteJumpTime;
        }
        else
        {
            coyoteJumpTimeCounter -= Time.deltaTime;
        }
    }

    // Draw Gizmos-Sphere
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
