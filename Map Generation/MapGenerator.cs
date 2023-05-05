using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public int mapWidth = 50;
    public int mapHeight = 50;
    public float noiseScale = 1000f;

    public Tilemap tilemap;
    public TileBase[] tiles;

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        float[,] noiseMap = GenerateNoiseMap(mapWidth, mapHeight, noiseScale);

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                float xCoord = ((float)x / mapWidth * noiseScale) + Random.Range(-10000f, 10000f);
                float yCoord = ((float)y / mapHeight * noiseScale) + Random.Range(-10000f, 10000f);

                float height = Mathf.PerlinNoise(xCoord, yCoord);
                TileBase tile = ChooseTile(height);
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    float[,] GenerateNoiseMap(int width, int height, float scale)
    {
        float[,] noiseMap = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = ((float)x / width * scale) + Random.Range(-10000f, 10000f);
                float yCoord = ((float)y / height * scale) + Random.Range(-10000f, 10000f);

                float noise = Mathf.PerlinNoise(xCoord, yCoord);
                noiseMap[x, y] = noise;
            }
        }

        // Apply a blur filter
        int kernelSize = 9;
        float[,] kernel = new float[kernelSize, kernelSize];
        for (int x = 0; x < kernelSize; x++)
        {
            for (int y = 0; y < kernelSize; y++)
            {
                kernel[x, y] = 1f / (float)(kernelSize * kernelSize);
            }
        }
        noiseMap = ApplyConvolutionFilter(noiseMap, kernel);

        return noiseMap;
    }

    float[,] ApplyConvolutionFilter(float[,] input, float[,] kernel)
    {
        int kernelSize = kernel.GetLength(0);
        int kernelRadius = kernelSize / 2;

        int width = input.GetLength(0);
        int height = input.GetLength(1);

        float[,] output = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float sum = 0f;

                for (int i = 0; i < kernelSize; i++)
                {
                    for (int j = 0; j < kernelSize; j++)
                    {
                        int dx = x + i - kernelRadius;
                        int dy = y + j - kernelRadius;

                        if (dx >= 0 && dx < width && dy >= 0 && dy < height)
                        {
                            sum += input[dx, dy] * kernel[i, j];
                        }
                    }
                }

                output[x, y] = sum;
            }
        }

        return output;
    }


    TileBase ChooseTile(float height)
    {
        if (height < 0.1f)
        {
            return tiles[0];
        }
        else if (height < 0.3f)
        {
            if (Random.Range(0f, 1f) < 0.2f) // randomly select tile 0 with a 20% probability
            {
                return tiles[0];
            }
            else
            {
                return tiles[1];
            }
        }
        else if (height < 0.5f)
        {
            bool tile0 = false;
            int adjacent1 = 0;
            int adjacent2 = 0;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;

                    float nx = ((float)x / mapWidth * noiseScale) + Random.Range(-10000f, 10000f);
                    float ny = ((float)y / mapHeight * noiseScale) + Random.Range(-10000f, 10000f);

                    float h = Mathf.PerlinNoise(nx, ny);
                    if (h < 0.4f) tile0 = true;
                    else if (h < 0.5f) adjacent1++;
                    else if (h < 0.7f) adjacent2++;
                }
            }

            if (tile0 || adjacent1 >= 3) return tiles[0];
            else if (adjacent2 >= 5) return tiles[2];
            else return tiles[1];
        }
        else
        {
            return tiles[3];
        }
    }
}
