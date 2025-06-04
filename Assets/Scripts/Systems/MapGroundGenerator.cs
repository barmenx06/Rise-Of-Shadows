using UnityEngine;

public class MapGroundGenerator : MonoBehaviour
{
    void Start()
    {
        if (MapManager.Instance == null || MapManager.Instance.currentMap == null)
        {
            Debug.LogError("MapGroundGenerator: currentMap is null! MapSelector eksik olabilir.");
            return;
        }

        GenerateGround();
    }

    void GenerateGround()
    {
        MapProfile map = MapManager.Instance.currentMap;

        for (int x = Mathf.FloorToInt(-map.mapSize.x / 2); x < Mathf.CeilToInt(map.mapSize.x / 2); x++)
        {
            for (int y = Mathf.FloorToInt(-map.mapSize.y / 2); y < Mathf.CeilToInt(map.mapSize.y / 2); y++)
            {
                Vector2 pos = new Vector2(x, y);
                Instantiate(map.groundPrefab, pos, Quaternion.identity, transform);
            }
        }
    }
}
