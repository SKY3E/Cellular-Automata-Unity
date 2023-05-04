using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int width = 40;
    public int height = 40;
    public float fillPercent = 40f;
    public int seed;
    public int smoothCount = 3;

    private int[,] map;

    private void Start()
    {
        seed = Random.Range(0, 100000);
        map = GenerateMap();
    }

    public int[,] GenerateMap()
    {
        map = new int[width, height];
        System.Random random = new System.Random(seed);

        // Randomly fill the map
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    map[x, y] = 1;
                else
                    map[x, y] = (random.Next(0, 100) < fillPercent) ? 1 : 0;
            }
        }

        // Smooth the map
        for (int i = 0; i < smoothCount; i++)
        {
            SmoothMap();
        }

        return map;
    }

    private void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighboringWalls = GetSurroundingWallCount(x, y);

                if (neighboringWalls > 4)
                    map[x, y] = 1;
                else if (neighboringWalls < 4)
                    map[x, y] = 0;
            }
        }
    }

    private int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;

        for (int neighborX = gridX - 1; neighborX <= gridX + 1; neighborX++)
        {
            for (int neighborY = gridY - 1; neighborY <= gridY + 1; neighborY++)
            {
                if (neighborX >= 0 && neighborX < width && neighborY >= 0 && neighborY < height)
                {
                    if (neighborX != gridX || neighborY != gridY)
                    {
                        wallCount += map[neighborX, neighborY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }
}
