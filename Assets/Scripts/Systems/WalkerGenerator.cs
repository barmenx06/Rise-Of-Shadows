using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WalkerGenerator : MonoBehaviour
{
    public enum GridType { FLOOR, WALL, EMPTY }

    public GridType[,] gridHandler;
    public List<WalkerObject> Walkers;
    public Tilemap tileMap;
    public Tile Floor;
    public Tile Wall;
    public int MapWidth = 30;
    public int MapHeight = 30;

    public int MaximumWalkers = 25;
    public int TileCount = 0;
    public float FillPercentage = 0.85f;
    public float WaitTime = 0.01f;

    [Header("Optional: Add player spawn")]
    public GameObject playerPrefab;

    private Vector3Int mapOffset;

    void Start()
    {
        mapOffset = new Vector3Int(-MapWidth / 2, -MapHeight / 2, 0);
        InitializeGrid();
    }

    void InitializeGrid()
    {
        gridHandler = new GridType[MapWidth, MapHeight];
        for (int x = 0; x < MapWidth; x++)
        {
            for (int y = 0; y < MapHeight; y++)
                gridHandler[x, y] = GridType.EMPTY;
        }

        Walkers = new List<WalkerObject>();
        Vector3Int start = new Vector3Int(MapWidth / 2, MapHeight / 2, 0);

        WalkerObject walker = new WalkerObject(new Vector2(start.x, start.y), GetDirection(), 0.2f);
        gridHandler[start.x, start.y] = GridType.FLOOR;
        tileMap.SetTile(start + mapOffset, Floor);
        Walkers.Add(walker);
        TileCount++;

        StartCoroutine(CreateFloors());
    }

    Vector2 GetDirection()
    {
        switch (Random.Range(0, 4))
        {
            case 0: return Vector2.down;
            case 1: return Vector2.left;
            case 2: return Vector2.up;
            case 3: return Vector2.right;
            default: return Vector2.zero;
        }
    }

    IEnumerator CreateFloors()
    {
        while ((float)TileCount / gridHandler.Length < FillPercentage)
        {
            bool addedTile = false;
            foreach (var walker in Walkers)
            {
                Vector3Int pos = new Vector3Int((int)walker.Position.x, (int)walker.Position.y, 0);
                if (gridHandler[pos.x, pos.y] != GridType.FLOOR)
                {
                    gridHandler[pos.x, pos.y] = GridType.FLOOR;
                    tileMap.SetTile(pos + mapOffset, Floor);
                    TileCount++;
                    addedTile = true;
                }
            }

            ChanceToRemove();
            ChanceToRedirect();
            ChanceToCreate();
            UpdatePositions();

            if (addedTile)
                yield return new WaitForSeconds(WaitTime);
        }

        StartCoroutine(CreateWalls());
    }

    void ChanceToRemove()
    {
        for (int i = Walkers.Count - 1; i >= 0; i--)
        {
            if (Random.value < Walkers[i].ChanceToChange && Walkers.Count > 1)
            {
                Walkers.RemoveAt(i);
                break;
            }
        }
    }

    void ChanceToRedirect()
    {
        for (int i = 0; i < Walkers.Count; i++)
        {
            if (Random.value < Walkers[i].ChanceToChange)
            {
                var w = Walkers[i];
                w.Direction = GetDirection();
                Walkers[i] = w;
            }
        }
    }

    void ChanceToCreate()
    {
        int count = Walkers.Count;
        for (int i = 0; i < count; i++)
        {
            if (Random.value < Walkers[i].ChanceToChange && Walkers.Count < MaximumWalkers)
            {
                var newWalker = new WalkerObject(Walkers[i].Position, GetDirection(), 0.2f);
                Walkers.Add(newWalker);
            }
        }
    }

    void UpdatePositions()
    {
        for (int i = 0; i < Walkers.Count; i++)
        {
            var w = Walkers[i];
            w.Position += w.Direction;
            w.Position.x = Mathf.Clamp(w.Position.x, 1, MapWidth - 2);
            w.Position.y = Mathf.Clamp(w.Position.y, 1, MapHeight - 2);
            Walkers[i] = w;
        }
    }

    IEnumerator CreateWalls()
    {
        for (int x = 1; x < MapWidth - 1; x++)
        {
            for (int y = 1; y < MapHeight - 1; y++)
            {
                if (gridHandler[x, y] == GridType.FLOOR)
                {
                    TryCreateWall(x + 1, y);
                    TryCreateWall(x - 1, y);
                    TryCreateWall(x, y + 1);
                    TryCreateWall(x, y - 1);
                }
            }
        }

        yield return null;

        SpawnPlayer();
    }

    void TryCreateWall(int x, int y)
    {
        if (gridHandler[x, y] == GridType.EMPTY)
        {
            tileMap.SetTile(new Vector3Int(x, y, 0) + mapOffset, Wall);
            gridHandler[x, y] = GridType.WALL;
        }
    }

    void SpawnPlayer()
    {
        if (playerPrefab == null) return;

        for (int x = 1; x < MapWidth - 1; x++)
        {
            for (int y = 1; y < MapHeight - 1; y++)
            {
                if (gridHandler[x, y] == GridType.FLOOR)
                {
                    Vector3 pos = new Vector3(x + 0.5f, y + 0.5f, 0) + (Vector3)mapOffset;
                    Instantiate(playerPrefab, pos, Quaternion.identity);
                    return;
                }
            }
        }
    }
}
