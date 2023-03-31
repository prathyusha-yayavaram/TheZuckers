using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StartingAreaMapGenerator : MonoBehaviour
{
    private int[,] mapGrid;
    private MapPainter mapPainter;

    private void Start() {

        MapPainter mapPainter = transform.GetChild(0).GetComponent<MapPainter>();
        this.mapPainter = mapPainter;
        
        Generate();

        mapPainter.map = mapGrid;
        mapPainter.GenerateWallTiles();
    }

    private void Generate()
    {
        mapGrid = new int[11, 5];
        
        FillWithWalls();

        EmptyArea();

        GenerateExitArea();
        
        GenerateGoArea();
        
    }

    private void GenerateExitArea()
    {
        mapGrid[0, 2] = 0;
        mapPainter.PaintSpecialExitTile(0, 2);
    }
    
    private void GenerateGoArea()
    {
        int x = mapGrid.GetLength(0) - 1;
        int y = 2;
        mapGrid[x, y] = 0;
        mapPainter.PaintSpecialGoTile(x, y);
    }

    private void EmptyArea()
    {
        for (int x = 1; x < mapGrid.GetLength(0) - 1; x++)
        {
            for (int y = 1; y < mapGrid.GetLength(1) - 1; y++)
            {
                mapGrid[x, y] = 0;
            }
        }
    }

    private void FillWithWalls()
    {
        for (int x = 0; x < mapGrid.GetLength(0); x++)
        {
            for (int y = 0; y < mapGrid.GetLength(1); y++)
            {
                mapGrid[x, y] = 1;
            }
        }
    }
    
}
