using UnityEngine;

public class MapSelector : MonoBehaviour
{
    public MapProfile[] availableMaps;

    void Awake()
    {
        if (MapManager.Instance != null && MapManager.Instance.currentMap == null)
        {
            int index = Random.Range(0, availableMaps.Length);
            MapManager.Instance.currentMap = availableMaps[index];
        }
    }
}
