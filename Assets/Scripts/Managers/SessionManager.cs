// Assets/Scripts/Managers/SessionManager.cs

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;  // Eğer TextMeshPro kullanıyorsan: using TMPro;
using TMPro;

public class SessionManager : MonoBehaviour
{
    [Header("Süre Ayarları")]
    [Tooltip("Oturum süresi (saniye cinsinden). 300 = 5 dakika")]
    public int sessionDurationSeconds = 300;

    private int remainingSeconds;

    [Header("UI Referansları")]
    [Tooltip("Geri sayımı gösterecek UI Text (veya TMP_Text)")]
    public TMP_Text timerText;

    /// <summary>
    /// Süre dolduğunda diğer sistemlerin dinleyebileceği event
    /// </summary>
    public event Action OnSessionEnd;

    private void Start()
    {
        // Sahne yüklendiğinde otomatik başlat
        StartSession();
    }

    /// <summary>
    /// Geri sayımı başlatır.
    /// </summary>
    public void StartSession()
    {
        remainingSeconds = sessionDurationSeconds;
        timerText.text = FormatTime(remainingSeconds);
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        while (remainingSeconds > 0)
        {
            yield return new WaitForSeconds(1f);
            remainingSeconds--;
            timerText.text = FormatTime(remainingSeconds);
        }

        // Süre doldu
        timerText.text = "00:00";
        OnSessionEnd?.Invoke();
    }

    private string FormatTime(int seconds)
    {
        int minutes = seconds / 60;
        int secs = seconds % 60;
        return $"{minutes:00}:{secs:00}";
    }
}
