using UnityEngine;

[CreateAssetMenu(fileName = "New Map Profile", menuName = "Map System/Map Profile")]
public class MapProfile: ScriptableObject
{
    [Header("Zemin")]
    public GameObject groundPrefab;

    [Header("Objeler")]
    public GameObject[] flowerPrefabs;
    public GameObject[] bushPrefab;
    public GameObject[] mushroomPrefab;
    public GameObject[] rockPrefab;
    public GameObject[] chestPrefab;

    [Header("Sayýsal Ayarlar")]
    public int flowerCount = 5;
    public int bushCount = 5;
    public int mushroomCount = 5;
    public int rockCount = 5;
    public int chestCount=5;

    public Vector2 mapSize = new Vector2(200, 200);
    public float minObjectDistance = 2.5f;
}
