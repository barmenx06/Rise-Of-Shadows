using UnityEngine;
using System.Collections.Generic;

public class MapObjectSpawner : MonoBehaviour
{
    private List<Vector2> usedPositions = new List<Vector2>();

    void Start()
    {
        SpawnAllObjects();
    }

    void SpawnAllObjects()
    {
        MapProfile map = MapManager.Instance.currentMap;

        SpawnObjects(map.flowerPrefabs, map.flowerCount);     
        SpawnObjects( map.bushPrefab , map.bushCount);
        SpawnObjects(map.mushroomPrefab, map.mushroomCount);
        SpawnObjects( map.rockPrefab , map.rockCount);
        SpawnObjects( map.chestPrefab, map.chestCount);
    }

    void SpawnObjects(GameObject[] prefabs, int count)
    {
        MapProfile map = MapManager.Instance.currentMap;
        int placed = 0;
        int tries = 0;

        while (placed < count && tries < count * 10)
        {
            Vector2 randomPos = new Vector2(
                Mathf.Round(Random.Range(-map.mapSize.x / 2f, map.mapSize.x / 2f)),
                Mathf.Round(Random.Range(-map.mapSize.y / 2f, map.mapSize.y / 2f))
            );

            bool tooClose = false;
            foreach (Vector2 pos in usedPositions)
            {
                if (Vector2.Distance(pos, randomPos) < map.minObjectDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
            {
                int index = Random.Range(0, prefabs.Length);
                Instantiate(prefabs[index], randomPos, Quaternion.identity, transform);
                usedPositions.Add(randomPos);
                placed++;
            }

            tries++;
        }
    }
}
