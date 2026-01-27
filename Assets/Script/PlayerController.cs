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

    private Animator animator;

    // ================= SLOW TIME =================
    [Header("Slow Motion")]
    public float slowMaxGauge = 5f;
    public float slowGauge = 5f;
    public float slowDrainSpeed = 1.5f;
    public float slowRecoverSpeed = 0.6f;
    public float slowScale = 0.4f;
    public float unlockThreshold = 1.0f; // 🔓 ต้องฟื้นถึงเท่านี้ถึงใช้ได้

    bool isSlowing = false;
    bool slowLocked = false; // 🔒 ตัวนี้สำคัญ



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentRunSpeed = maxRunSpeed * startSpeedPercent;
        rb.gravityScale = gravityScale;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.linearVelocity = Vector2.zero;

        slowGauge = slowMaxGauge;
    }

    void Update()
    {
        HandleMovement();
        HandleGroundCheck();
        HandleJump();
        HandleSlowTime();
    }

    // ================= MOVEMENT =================
    void HandleMovement()
    {
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        currentRunSpeed = Mathf.MoveTowards(
            currentRunSpeed,
            maxRunSpeed,
            acceleration * Time.deltaTime
        );

        rb.linearVelocity = new Vector2(currentRunSpeed, rb.linearVelocity.y);
    }

    // ================= GROUND =================
    void HandleGroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            checkRadius,
            whatIsGround
        );

        animator.SetBool("IsGrounded", isGrounded);
    }

    // ================= JUMP =================
    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            AudioManager.Instance.PlayJumpSFX();
            animator.SetTrigger("Jump");
            FlipGravity();
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

    // ================= SLOW TIME =================
    void HandleSlowTime()
    {
        // =========================
        // 🔒 ถ้าถูกล็อก
        // =========================
        if (slowLocked)
        {
            slowGauge += slowRecoverSpeed * Time.unscaledDeltaTime;
            slowGauge = Mathf.Clamp(slowGauge, 0f, slowMaxGauge);

            // 🔓 ปลดล็อกเมื่อฟื้นพอ
            if (slowGauge >= unlockThreshold)
            {
                slowLocked = false;
            }

            return; // ❌ ห้ามอ่าน Input ใด ๆ
        }

        // =========================
        // ⏳ กดค้างเพื่อสโลว์
        // =========================
        if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                AudioManager.Instance.PlaySlowStart();
            }

            if (!isSlowing)
            {
                Time.timeScale = slowScale;
                Time.fixedDeltaTime = 0.02f * slowScale;
                animator.speed = slowScale;
                isSlowing = true;
            }

            slowGauge -= slowDrainSpeed * Time.unscaledDeltaTime;

            // 🔥 เกจหมด → ตัดทันที + ล็อก
            if (slowGauge <= 0f)
            {
                slowGauge = 0f;
                ForceStopSlow();
                slowLocked = true;
            }
        }
        else
        {
            // =========================
            // ⏱ ปล่อยปุ่ม
            // =========================
            if (isSlowing)
            {
                ForceStopSlow();
            }

            slowGauge += slowRecoverSpeed * Time.unscaledDeltaTime;
        }

        slowGauge = Mathf.Clamp(slowGauge, 0f, slowMaxGauge);
    }
    void ForceStopSlow()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        animator.speed = 1f;
        isSlowing = false;
    }




    // ================= UI =================
    public float GetSlowGaugePercent()
    {
        return slowGauge / slowMaxGauge;
    }

    public bool GetGroundedStatus()
    {
        return isGrounded;
    }
}
