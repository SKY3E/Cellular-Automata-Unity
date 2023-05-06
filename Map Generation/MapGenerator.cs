using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    // Define map size and fill probability
    private int[,] newMap;
    // Default : 0.48f
    private readonly float fillProb = 0.48f;
    // Default : 40, 40
    private readonly Vector2Int shape = new Vector2Int(40, 40);
    // Default : 5
    private readonly int SMOOTH_ITERATIONS = 5;

    public Tilemap tilemap;
    public TileBase[] tiles;

    void Start () {
        GenerateCave();
    }

    public void RegenerateCave() {
        GenerateCave();
    }

    void GenerateCave() {
        newMap = new int[shape.x, shape.y];
        for (int i = 0; i < shape.x; i++) {
            for (int j = 0; j < shape.y; j++) {
                float choice = Random.Range(0f, 1f);
                newMap[i, j] = choice < fillProb ? 1 : 0;
            }
        }

        // Perform a smoothing algorithm to remove isolated cells
        for (int i = 0; i < SMOOTH_ITERATIONS; i++) {
            SmoothMap();
        }

        DisplayCave();
    }

    private void SmoothMap() {
        for (int i = 0; i < shape.x; i++) {
            for (int j = 0; j < shape.y; j++) {
                int wallCount = CountWalls(newMap, i, j);
                if (wallCount > 4) {
                    newMap[i, j] = 1;
                } else if (wallCount < 4) {
                    newMap[i, j] = 0;
                }
            }
        }
    }

    private int CountWalls(int[,] map, int i, int j) {
        int wallCount = 0;
        for (int x = i - 1; x <= i + 1; x++) {
            for (int y = j - 1; y <= j + 1; y++) {
                if (x >= 0 && x < shape.x && y >= 0 && y < shape.y && (x != i || y != j)) {
                    wallCount += map[x, y];
                }
            }
        }
        return wallCount;
    }

    TileBase ChooseTile(int tileValue)
    {
        if (tileValue == 0){ 
            return tiles[0];
        }
        else{
            return tiles[1];
        }
    }

    private void DisplayCave() {
        for (int x = 0; x < shape.x; x++) {
            for (int y = 0; y < shape.y; y++) {
                TileBase tile = ChooseTile(newMap[x, y]);
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }
}
