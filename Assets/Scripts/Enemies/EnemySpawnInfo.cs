using UnityEngine;

/// <summary>
/// Bir dalgadaki tek bir düþman tipi için spawn ayarlarýný tutar.
/// </summary>
[System.Serializable]
public class EnemySpawnInfo
{
    [Tooltip("Spawn edilecek düþman prefab'ý")]
    public GameObject enemyPrefab;

    [Tooltip("Bu prefab'tan kaç tane spawn edilecek")]
    public int amount;
}
