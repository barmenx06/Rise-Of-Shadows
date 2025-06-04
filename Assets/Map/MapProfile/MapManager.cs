using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    [Header("Aktif Map Profili")]
    public MapProfile currentMap;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentMap = null; 
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
