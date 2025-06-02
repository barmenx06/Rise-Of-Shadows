using UnityEngine;
using TMPro;

public class WaveUIController : MonoBehaviour
{
    public TMP_Text waveInfoText;
    [Header("Countdown Thresholds")]
    public int firstThreshold = 30;
    public int secondThreshold = 20;
    public int finalThreshold = 10;

    private int prevSeconds = -1;

    /// <summary>
    /// Dalga metnini göstermek veya gizlemek için çaðrýlýr.
    /// </summary>
    public void ShowWaveCountdown(string waveName, int secondsLeft)
    {
        if (secondsLeft == prevSeconds) return; // Ayný saniyeyi tekrar göstermemek için
        prevSeconds = secondsLeft; // Geçerli saniyeyi güncelle

        if (waveInfoText == null)
            return;

        // Eþik deðerleri kontrol
        if (secondsLeft == firstThreshold || secondsLeft == secondThreshold)
        {
            waveInfoText.text = $"{waveName}, {secondsLeft} sn içinde baþlayacak";
            waveInfoText.gameObject.SetActive(true);
        }
        else if (secondsLeft <= finalThreshold && secondsLeft > 0)
        {
            waveInfoText.text = $"{secondsLeft}";
            waveInfoText.gameObject.SetActive(true);
        }
        else // diðer durumlarda gizle
        {
            waveInfoText.gameObject.SetActive(false);
        }
    }

    public void HideWaveText()
    {
        if (waveInfoText == null)
            return;
        waveInfoText.gameObject.SetActive(false);
    }
}