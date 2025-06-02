using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelUpPanelController : MonoBehaviour
{
    public GameObject panelRoot;        // LevelUpPanelCanvas
    public GameObject darkBackground;   // Arka plan karartma Image objesi
    public GameObject joystickUI;       // Joystick’in root GameObject’i
    public PlayerStats playerStats;     // Player’daki PlayerStats
    public TMP_Text titleText;              // Panelde “Seviye X!” yazacak Text

    private Action onCloseCallback;

    public void ShowPanel(int newLevel, Action onClose)
    {
        Time.timeScale = 0f;
        if (darkBackground != null)
            darkBackground.SetActive(true);
        if (panelRoot != null)
            panelRoot.SetActive(true);
        if (joystickUI != null)
            joystickUI.SetActive(false);
        if (titleText != null)
            titleText.text = $"{newLevel}!";
        onCloseCallback = onClose;
    }

    public void ApplyAndClose(int perkIndex)
    {
        if (playerStats != null)
            playerStats.ApplyPerk(perkIndex);
        Time.timeScale = 1f;
        if (darkBackground != null)
            darkBackground.SetActive(false);
        if (panelRoot != null)
            panelRoot.SetActive(false);
        if (joystickUI != null)
            joystickUI.SetActive(true);
        onCloseCallback?.Invoke();
    }
}
