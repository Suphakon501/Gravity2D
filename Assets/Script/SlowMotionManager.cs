using UnityEngine;
using UnityEngine.UI;

public class SlowMotionManager : MonoBehaviour
{
    public static SlowMotionManager Instance;
    public bool isGameOver;

    [Header("Time Scale")]
    [Range(0.05f, 1f)]
    public float slowTimeScale = 0.4f;
    public float normalTimeScale = 1f;

    [Header("Slow Gauge")]
    public float maxSlowEnergy = 100f; 
    public float drainRate = 10f;      
    public float regenRate = 15f;
    public float regenDelay = 1f;

    [Header("Cooldown")]
    public float slowCooldown = 1f;
    public float unlockThreshold = 1f;

    [Header("UI")]
    public Slider slowSlider;

    float currentEnergy;
    float regenTimer;
    float cooldownTimer;
    float originalFixedDeltaTime;

    bool isSlowing;
    bool slowLocked;
    bool canStartSlow = true;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        originalFixedDeltaTime = Time.fixedDeltaTime;
        currentEnergy = maxSlowEnergy;
        UpdateUI();
    }

    void Update()
    {
        if (isGameOver) return;

        HandleCooldown();
        HandleInput();
        HandleDrain();
        HandleRegen();
    }

    // ================= INPUT =================
    void HandleInput()
    {
        if (!ScoreManager.isAlive)
            return;

       
        if (Input.GetMouseButtonUp(0) && isSlowing)
        {
            StopSlow();
            cooldownTimer = slowCooldown;
            return;
        }

        if (slowLocked || !canStartSlow || cooldownTimer > 0f)
            return;

        if (Input.GetMouseButtonDown(0) && currentEnergy > 0f)
        {
            StartSlow();
        }

    }

    // ================= START =================
    void StartSlow()
    {
        if (isSlowing) return;

        isSlowing = true;
        canStartSlow = false;
        regenTimer = 0f;

        Time.timeScale = slowTimeScale;
        Time.fixedDeltaTime = originalFixedDeltaTime * slowTimeScale;

        AudioManager.Instance?.PlaySlowStart();
    }
    void HandleDrain()
    {
        if (!isSlowing) return;

        currentEnergy -= drainRate * Time.unscaledDeltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxSlowEnergy);

        if (currentEnergy <= 0f)
        {
            currentEnergy = 0f;
            StopSlow();
            slowLocked = true;
            cooldownTimer = slowCooldown;
        }

        UpdateUI();
    }

    // ================= STOP =================
    void StopSlow()
    {
        if (!isSlowing) return;

        isSlowing = false;

        Time.timeScale = normalTimeScale;
        Time.fixedDeltaTime = originalFixedDeltaTime;

        AudioManager.Instance?.PlaySlowEnd();
    }

    // ================= COOLDOWN =================
    void HandleCooldown()
    {
        if (cooldownTimer <= 0f) return;

        cooldownTimer -= Time.unscaledDeltaTime;

        if (cooldownTimer <= 0f)
        {
            cooldownTimer = 0f;
            canStartSlow = true;
        }
    }

    // ================= REGEN =================
    void HandleRegen()
    {
        if (isSlowing || currentEnergy >= maxSlowEnergy) return;

        regenTimer += Time.unscaledDeltaTime;
        if (regenTimer < regenDelay) return;

        currentEnergy += regenRate * Time.unscaledDeltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxSlowEnergy);

        if (slowLocked && currentEnergy >= unlockThreshold)
        {
            slowLocked = false;
        }

        UpdateUI();
    }

    public void ForceStop()
    {
        isSlowing = false;
        Time.timeScale = normalTimeScale;
        Time.fixedDeltaTime = originalFixedDeltaTime;
    }
    public void OnGameOver()
    {
        isGameOver = true;
        StopSlow();
    }

    // ================= UI =================
    public float GetSlowGaugePercent()
    {
        return Mathf.Clamp01(currentEnergy / maxSlowEnergy);
    }
    void UpdateUI()
    {
        if (slowSlider != null)
        {
            slowSlider.value = currentEnergy / maxSlowEnergy;
        }
    }
}
