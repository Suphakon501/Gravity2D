using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image heartImage;

    public Color fullHealthColor = Color.red;
    public Color lowHealthColor = Color.black;

    void OnEnable()
    {
        PlayerHealth.OnHealthChanged += UpdateHeartColor;
    }

    void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= UpdateHeartColor;
    }

    void UpdateHeartColor(int current, int max)
    {
        float t = 1f - ((float)current / max);
        heartImage.color = Color.Lerp(fullHealthColor, lowHealthColor, t);
    }
}
