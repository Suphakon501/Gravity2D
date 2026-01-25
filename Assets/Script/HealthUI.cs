using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Slider healthSlider;
    public PlayerHealth playerHealth;
    public float smoothSpeed = 5f;

    void Start()
    {
        healthSlider.maxValue = playerHealth.maxHealth;
        healthSlider.value = playerHealth.maxHealth;
    }

    void Update()
    {
        healthSlider.value = Mathf.Lerp(
            healthSlider.value,
            playerHealth.CurrentHealth,
            Time.deltaTime * smoothSpeed
        );
    }
}
