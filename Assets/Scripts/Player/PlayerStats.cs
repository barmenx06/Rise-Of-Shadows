using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Level & EXP")]
    public int level = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;

    public float attackPower;
    public int maxHealth, currentHealth;
    public float moveSpeed;

    [Header("LevelUp Panel")]
    public LevelUpPanelController panelCtrl; // Inspector’dan ata

    [Header("UI")]
    public ExpBarUI expBarUI;    // Inspector’dan referans

    private Queue<int> pendingLevelUps = new Queue<int>();
    private bool isLevelUpPanelOpen = false;

    void Start()
    {
        level = 1;
        currentExp = 0;
        expToNextLevel = 100;
        UpdateUI();
    }

    public void ApplyPerk(int idx)
    {
        switch (idx)
        {
            case 0:
                attackPower += 2;
                break;
            case 1:
                maxHealth += 20;
                currentHealth = maxHealth;
                break;
            case 2:
                moveSpeed *= 1.1f;
                break;
        }
        UpdateUI();
    }

    public void GainExp(int amount)
    {
        currentExp += amount;
        Debug.Log($"Gained {amount} EXP, total: {currentExp}");

        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            level++;
            expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.2f);
            UpdateUI();
            pendingLevelUps.Enqueue(level);
        }

        float fill = expToNextLevel > 0 ? (float)currentExp / expToNextLevel : 0f;
        expBarUI.fillImage.fillAmount = Mathf.Clamp01(fill);
        expBarUI.levelText.text = $"LVL {level}";

        if (!isLevelUpPanelOpen && pendingLevelUps.Count > 0)
        {
            ShowNextLevelUpPanel();
        }
    }

    private void ShowNextLevelUpPanel()
    {
        if (pendingLevelUps.Count == 0)
            return;

        int nextLevelToShow = pendingLevelUps.Dequeue();
        isLevelUpPanelOpen = true;

        if (panelCtrl != null)
        {
            panelCtrl.ShowPanel(nextLevelToShow, OnLevelUpPanelClosed);
        }
        else
        {
            Debug.LogWarning("Panel Bulunamadi");
            isLevelUpPanelOpen = false;
            ShowNextLevelUpPanel();
        }
    }

    private void OnLevelUpPanelClosed()
    {
        isLevelUpPanelOpen = false;
        if (pendingLevelUps.Count > 0)
        {
            ShowNextLevelUpPanel();
        }
    }

    private void UpdateUI()
    {
        if (expBarUI != null)
            expBarUI.SetExpBar(currentExp, expToNextLevel, level);
    }
}
