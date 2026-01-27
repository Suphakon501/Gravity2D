using UnityEngine;

public class SlowMotionManager : MonoBehaviour
{
    [Header("Slow Motion Settings")]
    [Range(0.05f, 1f)]
    public float slowTimeScale = 0.25f;   // ความช้าตอนสโล
    public float normalTimeScale = 1f;

    private float originalFixedDeltaTime;

    void Awake()
    {
        originalFixedDeltaTime = Time.fixedDeltaTime;
    }

    void Update()
    {
        // 🖱 กดคลิกซ้ายค้าง = สโล
        if (Input.GetMouseButton(0))
        {
            EnableSlowMotion();
        }
        else
        {
            DisableSlowMotion();
        }
    }

    void EnableSlowMotion()
    {
        Time.timeScale = slowTimeScale;
        Time.fixedDeltaTime = originalFixedDeltaTime * slowTimeScale;
    }

    void DisableSlowMotion()
    {
        Time.timeScale = normalTimeScale;
        Time.fixedDeltaTime = originalFixedDeltaTime;
    }
}
