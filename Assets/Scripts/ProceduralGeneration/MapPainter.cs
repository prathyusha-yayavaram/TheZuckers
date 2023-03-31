using System;
using UnityEngine;using UnityEngine.Tilemaps;

public class MapPainter : MonoBehaviour
{

    [SerializeField] private Tilemap wallMap, floorMap, exitMap;
    [SerializeField] private Tilemap specialGoMap, specialExitMap;
    public int[,] map;
    [SerializeField] private TileBase wallTile, floorTile;
    

    public void GenerateWallTiles()
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == 1)
                {
                    wallMap.SetTile(new Vector3Int(i, j, 0), wallTile);
                }
                else
                {
                    floorMap.SetTile(new Vector3Int(i, j, 0), floorTile);
                }
            }
        }
    }

    public void RefreshAllTiles()
    {
        wallMap.ClearAllTiles();
        floorMap.ClearAllTiles();
        exitMap.ClearAllTiles();
    }

    public void PaintRegularExitTile(int x, int y)
    {
        exitMap.SetTile(new Vector3Int(x, y), floorTile);
    }

    public void PaintSpecialGoTile(int x, int y)
    {
        specialGoMap.SetTile(new Vector3Int(x, y), floorTile);
    }

    public void PaintSpecialExitTile(int x, int y)
    {
        specialExitMap.SetTile(new Vector3Int(x, y), floorTile);
    }
}