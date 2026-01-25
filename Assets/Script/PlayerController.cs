using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxRunSpeed = 10f;

    [Range(0f, 1f)]
    public float startSpeedPercent = 0.6f; 

    public float acceleration = 4f;
    private float currentRunSpeed;

    [Header("Gravity Settings")]
    public float gravityScale = 8f;
    private Rigidbody2D rb;
    private bool gravityDown = true;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.3f;
    public LayerMask whatIsGround;
    public bool isGrounded;

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentRunSpeed = maxRunSpeed * startSpeedPercent;

        rb.gravityScale = gravityScale;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        currentRunSpeed = Mathf.MoveTowards(
            currentRunSpeed,
            maxRunSpeed,
            acceleration * Time.deltaTime
        );

        rb.linearVelocity = new Vector2(currentRunSpeed, rb.linearVelocity.y);

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            checkRadius,
            whatIsGround
        );

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            animator.SetTrigger("Jump");
            FlipGravity();
        }
    }

    void FlipGravity()
    {
        gravityDown = !gravityDown;
        rb.gravityScale = gravityDown ? gravityScale : -gravityScale;

        transform.Rotate(180f, 0f, 0f);

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        float pushDirection = gravityDown ? -1f : 1f;

        rb.AddForce(Vector2.up * pushDirection * 2f, ForceMode2D.Impulse);
    }

    public bool GetGroundedStatus()
    {
        return isGrounded;
    }
}
