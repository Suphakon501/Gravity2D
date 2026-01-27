using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxRunSpeed = 10f;
    [Range(0f, 1f)] public float startSpeedPercent = 0.6f;
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

    [Header("State")]
    public bool canMove = true;

    [Header("Slow Motion")]
    public float slowTimeScale = 0.3f;
    private bool isSlow = false;

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        canMove = true;
        gravityDown = true;

        currentRunSpeed = maxRunSpeed * startSpeedPercent;

        rb.gravityScale = gravityScale;
        rb.linearVelocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // กัน TimeScale ค้างจากรอบก่อน
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    void Update()
    {
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // ===== MOVE =====
        currentRunSpeed = Mathf.MoveTowards(
            currentRunSpeed,
            maxRunSpeed,
            acceleration * Time.deltaTime
        );

        rb.linearVelocity = new Vector2(currentRunSpeed, rb.linearVelocity.y);

        // ===== GROUND CHECK =====
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            checkRadius,
            whatIsGround
        );

        // ===== JUMP =====
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            AudioManager.Instance.PlayJumpSFX();
            animator.ResetTrigger("Jump");
            animator.SetTrigger("Jump");
            FlipGravity();
        }

        // ===== SLOW MOTION (คลิกซ้าย) =====
        if (Input.GetMouseButtonDown(0))
        {
            ToggleSlowMotion();
        }
    }

    void ToggleSlowMotion()
    {
        isSlow = !isSlow;

        if (isSlow)
        {
            Time.timeScale = slowTimeScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            AudioManager.Instance.PlaySlowStart();
        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;

            AudioManager.Instance.PlaySlowEnd();
        }
    }

    void FlipGravity()
    {
        gravityDown = !gravityDown;

        rb.gravityScale = gravityDown ? gravityScale : -gravityScale;

        transform.rotation = gravityDown
            ? Quaternion.identity
            : Quaternion.Euler(180f, 0f, 0f);

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        float pushDirection = gravityDown ? -1f : 1f;
        rb.AddForce(Vector2.up * pushDirection * 2f, ForceMode2D.Impulse);
    }

    public bool GetGroundedStatus()
    {
        return isGrounded;
    }
}
