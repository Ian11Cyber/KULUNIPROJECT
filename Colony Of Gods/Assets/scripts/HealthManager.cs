using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [Header("UI Reference")]
    public Image fillImage;

    [Header("Target Reference")]
    public Health target; // kahit sino, basta may Health script

    void Update()
    {
        if (target == null || fillImage == null) return;

        float healthPercent = (float)target.currentHealth / target.maxHealth;
        fillImage.fillAmount = healthPercent;
    }
}
