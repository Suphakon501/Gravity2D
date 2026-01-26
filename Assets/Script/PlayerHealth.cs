using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 4;
    private int currentHealth;

    [Header("Flicker Settings")]
    public float flashDuration = 0.1f;
    public int flashCount = 6;
    private SpriteRenderer spriteRenderer;
    private bool isInvincible = false;

    [Header("Air Timer Settings")]
    public float maxAirTime = 5f;
    [SerializeField] private float airTimer = 0f;

    private PlayerController playerController;
    private Collider2D playerCollider;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
        playerCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        HandleAirTimer();
    }

    void HandleAirTimer()
    {
        if (playerController != null && !playerController.GetGroundedStatus())
        {
            airTimer += Time.deltaTime;
            if (airTimer >= maxAirTime)
            {
                Die();
            }
        }
        else
        {
            airTimer = 0f;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(DamageFlicker());
        }
    }
    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    IEnumerator DamageFlicker()
    {
        isInvincible = true;
        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(flashDuration);
        }
        isInvincible = false;
    }

    public DeathPopupUI deathPopupUI;
    public ScoreManager scoreManager;


    void Die()
    {
        if (scoreManager != null)
        {
            scoreManager.isAlive = false;
        }
        deathPopupUI.ShowDeathPopup();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("obstruction"))
        {
            TakeDamage(1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("SideObstruction"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.x < -0.7f)
                {
                    TakeDamage(1);
                    StartCoroutine(DisableCollisionTemporarily(collision.collider));
                    break;
                }
            }
        }
    }

    IEnumerator DisableCollisionTemporarily(Collider2D otherCollider)
    {
        if (playerCollider != null && otherCollider != null)
        {
            Physics2D.IgnoreCollision(playerCollider, otherCollider, true);

            yield return new WaitForSeconds(flashDuration * flashCount);

            if (otherCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, otherCollider, false);
            }
        }
    }
}