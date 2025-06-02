using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpBarUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Image fillImage;
    public TextMeshProUGUI levelText;
   

    public void SetExpBar(int currentExp, int expToNext, int level)
    {
        float fill = expToNext > 0 ? (float)currentExp / expToNext : 0f;
        if (fillImage != null)
            fillImage.fillAmount = Mathf.Clamp01(fill);

        if (levelText != null)
            levelText.text =level.ToString();

        
    }
}
