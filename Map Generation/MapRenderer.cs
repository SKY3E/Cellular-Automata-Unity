using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapRenderer : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile wallTile;
    public Tile floorTile;
    public MapGenerator mapGenerator;

    private void Start()
    {
        RenderMap(mapGenerator.GenerateMap());
    }

    private void RenderMap(int[,] map)
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);

                if (map[x, y] == 1)
                    tilemap.SetTile(position, wallTile);
                else
                    tilemap.SetTile(position, floorTile);
            }
        }
    }
}
