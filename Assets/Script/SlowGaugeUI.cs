using UnityEngine;
using UnityEngine.UI;

public class SlowGaugeUI : MonoBehaviour
{
    public SlowMotionManager slowManager;
    public Image fillImage;

    void Update()
    {
        if (slowManager == null) return;

        fillImage.fillAmount = slowManager.GetSlowGaugePercent();
    }
}
