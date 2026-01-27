using UnityEngine;
using UnityEngine.UI;

public class SlowGaugeUI : MonoBehaviour
{
    public PlayerController player;
    public Image fillImage;

    void Update()
    {
        if (player == null) return;

        fillImage.fillAmount = player.GetSlowGaugePercent();
    }
}
