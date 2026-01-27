using UnityEngine;
using UnityEngine.UI;

public class SlowMotionManager : MonoBehaviour
{
    [Header("Time Scale")]
    [Range(0.05f, 1f)]
    public float slowTimeScale = 0.4f;
    public float normalTimeScale = 1f;

    [Header("Slow Gauge")]
    public float maxSlowEnergy = 5f;
    public float drainRate = 1.5f;
    public float regenRate = 0.5f;
    public float regenDelay = 1f;

    [Header("Cooldown")]
    public float slowCooldown = 1.0f;     // ⏱ คูลดาวน์
    public float unlockThreshold = 1.0f;  // 🔓 ต้องฟื้นถึงเท่านี้ถึงใช้ได้

    [Header("UI")]
    public Slider slowSlider;

    private float currentEnergy;
    private float originalFixedDeltaTime;
    private float regenTimer;
    private float cooldownTimer;

    private bool isSlowing;
    private bool slowLocked; // 🔒 ล็อกเมื่อพลังหมด

    private bool canStartSlow = true; // 🚪 ประตูเริ่ม Slow


    void Awake()
    {
        originalFixedDeltaTime = Time.fixedDeltaTime;
        currentEnergy = maxSlowEnergy;
        UpdateUI();
    }

    void Update()
    {
        HandleCooldown();
        HandleSlowInput();
        HandleRegen();
    }

    // ================= COOLDOWN =================
    void HandleCooldown()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.unscaledDeltaTime;
        }
    }

    // ================= INPUT =================
    void HandleSlowInput()
    {
        // ⏱ ลด cooldown
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.unscaledDeltaTime;
            return;
        }

        // 🔒 พลังหมด = ห้ามเริ่ม
        if (slowLocked)
            return;

        // ▶️ เริ่ม Slow (ได้ครั้งเดียว)
        if (Input.GetMouseButtonDown(0) && canStartSlow && currentEnergy > 0f)
        {
            EnableSlow();
            canStartSlow = false; // 🔒 ล็อกทันที
        }

        // ⏳ ระหว่าง Slow
        if (isSlowing)
        {
            currentEnergy -= drainRate * Time.unscaledDeltaTime;
            currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxSlowEnergy);

            if (currentEnergy <= 0f)
            {
                currentEnergy = 0f;
                DisableSlow();
                slowLocked = true;
                cooldownTimer = slowCooldown;
            }

            UpdateUI();
        }

        // ⏹ ปล่อยปุ่ม
        if (Input.GetMouseButtonUp(0) && isSlowing)
        {
            DisableSlow();
            cooldownTimer = slowCooldown;
        }
    }



    // ================= ENABLE =================
    void EnableSlow()
    {
        if (!isSlowing)
        {
            isSlowing = true;
            regenTimer = 0f;

            Time.timeScale = slowTimeScale;
            Time.fixedDeltaTime = originalFixedDeltaTime * slowTimeScale;
        }

        currentEnergy -= drainRate * Time.unscaledDeltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxSlowEnergy);

        // 🔥 พลังหมด → ตัด + ล็อก + CD
        if (currentEnergy <= 0f)
        {
            currentEnergy = 0f;
            DisableSlow();
            slowLocked = true;
            cooldownTimer = slowCooldown;
        }

        UpdateUI();
    }

    // ================= DISABLE =================
    void DisableSlow()
    {
        if (!isSlowing) return;

        isSlowing = false;
        Time.timeScale = normalTimeScale;
        Time.fixedDeltaTime = originalFixedDeltaTime;

        canStartSlow = true; // 🔓 เปิดประตูให้เริ่มใหม่ (หลัง CD)
    }


    // ================= REGEN =================
    void HandleRegen()
    {
        if (isSlowing) return;
        if (currentEnergy >= maxSlowEnergy) return;

        regenTimer += Time.unscaledDeltaTime;
        if (regenTimer < regenDelay) return;

        currentEnergy += regenRate * Time.unscaledDeltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxSlowEnergy);

        // 🔓 ปลดล็อกเมื่อฟื้นพอ
        if (slowLocked && currentEnergy >= unlockThreshold)
        {
            slowLocked = false;
        }

        UpdateUI();
    }

    // ================= UI =================
    void UpdateUI()
    {
        if (slowSlider != null)
        {
            slowSlider.value = currentEnergy / maxSlowEnergy;
        }
    }
}
