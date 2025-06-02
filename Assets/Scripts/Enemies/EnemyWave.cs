using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tek bir dalgaya (wave) ait düþman tipleri listesini ve dalga adýný tutar.
/// </summary>
[System.Serializable]
public class EnemyWave
{
    [Tooltip("Dalga adý, UI'da gösterilir")]
    public string waveName;

    [Tooltip("Bu dalgadaki tüm düþman tipi bilgileri")]
    public List<EnemySpawnInfo> enemiesInWave;
}
