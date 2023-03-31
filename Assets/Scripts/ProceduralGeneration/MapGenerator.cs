using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour {
    
    private int[,] mapGrid;
    private int[,] overlayGrid;
    private MapPainter mapPainter;
    private Pathfinding pathfinding;
    public Vector2 playerSpawn;
    [NonSerialized] public List<Vector2Int> spawns = new List<Vector2Int>();
    private List<GameObject> enemies = new List<GameObject>();

    [SerializeField] private GameObject enemy;
    
    
    public void StartUpMapGenerator() {
        mapPainter = transform.GetChild(0).GetComponent<MapPainter>();
        Generate();
        mapPainter.map = mapGrid;
        mapPainter.GenerateWallTiles();
        pathfinding = new Pathfinding(this);
    }

    public void ClearMapGenerator()
    {
        DestroyEnemies();
        mapPainter.RefreshAllTiles();
        mapGrid = null;
        spawns.Clear();
        enemies.Clear();
        overlayGrid = null;
        pathfinding = null;
    }

    private void DestroyEnemies()
    {
        foreach(GameObject obj in enemies)
        {
            Destroy(obj.gameObject);
        }
    }

    private void Generate()
    {
        mapGrid = new int[50, 50];
        GenerateRooms();
    }

    private void GenerateRooms()
    {

        MakeAllCellsWalls();

        RectInt firstRoom = GenerateFirstRoom(10, 20, 3, 3);

        GenerateMultiRooms(10, 20, firstRoom);

        GenerateObstacles();

        CopyIntoNewArray();

        GenerateEnemySpawns();

        CutOutExit();

        FindEntrance();

        SpawnEnemies();

    }

    private void SpawnEnemies()
    {
        foreach (Vector2Int spawnPos in spawns)
        {
            GameObject enemyObj = Instantiate(enemy, new Vector3(spawnPos.x + 0.5f, spawnPos.y + 0.5f), Quaternion.identity);
            // enemyObj.GetComponent<Enemy>()
            int type = Random.Range(0, 4);
            EnemyController enemyController = enemyObj.GetComponent<EnemyController>();
            if (type == 0) {
                enemyController.enemyLike = EnemyLikes.SAT;
            }else if (type == 1) {
                enemyController.enemyLike = EnemyLikes.BEN_SHAPIRO;
            }else if (type == 2) {
                enemyController.enemyLike = EnemyLikes.CD;
            }else if (type == 3) {
                enemyController.enemyLike = EnemyLikes.DOG_FOOD;
            }
            enemyObj.GetComponent<EnemyController>().ChangePopup();
            enemies.Add(enemyObj);
            
        }
    }

    private void FindEntrance()
    {

        for (int x = 0; x < mapGrid.GetLength(0); x++)
        {
            for (int y = 0; y < mapGrid.GetLength(1); y++)
            {
                if (mapGrid[x, y] == 0)
                {
                    playerSpawn = new Vector2(x, y);
                    return;
                }
            }
        }
    }

    private void CutOutExit()
    {
        //cut it out of the most right tiles basically
        int width = 3;

        int furthestRightX = -1;
        int furthestRightY = -1;


        for (int x = 0; x < mapGrid.GetLength(0); x++)
        {
            for (int y = 0; y < mapGrid.GetLength(1); y++)
            {
                if (mapGrid[x, y] == 0)
                {
                    if (furthestRightX <= x)
                    {
                        furthestRightX = x;
                        furthestRightY = y;
                    }
                }
            }
        }

        for (int x = 0; x < 25; x++)
        {
            for (int y = furthestRightY; y < furthestRightY + width; y++)
            {
                if (x == 0)
                {
                    mapPainter.PaintRegularExitTile(mapGrid.GetLength(0) - 1 - x, y);
                }
                mapGrid[mapGrid.GetLength(0) - 1 - x, y] = 0;
            }
        }

    }

    private void GenerateEnemySpawns()
    {
        for (int x = 0; x < overlayGrid.GetLength(0); x++)
        {
            for (int y = 0; y < overlayGrid.GetLength(1); y++)
            {
                if (overlayGrid[x, y] == 0)
                {
                    if (Random.value < 0.02f)
                    {
                        spawns.Add(new Vector2Int(x, y));
                        overlayGrid[x, y] = 2;
                    }
                }
            }
        }

    }

    private void CopyIntoNewArray()
    {
        overlayGrid = new int[mapGrid.GetLength(0), mapGrid.GetLength(1)];

        for (int x = 0; x < mapGrid.GetLength(0); x++)
        {
            for (int y = 0; y < mapGrid.GetLength(1); y++)
            {
                overlayGrid[x, y] = mapGrid[x, y];
            }
        }
    }

    private void GenerateObstacles() {
        for (int x = 3; x < mapGrid.GetLength(0) - 3; x++)
        {
            for (int y = 3; y < mapGrid.GetLength(1) - 3; y++)
            {
                if(mapGrid[x, y] == 1) continue;
                if (Random.value < 0.04f)
                {
                    int obstacleType = Random.Range(0, 11);
                    switch (obstacleType)
                    {
                        case 0:
                            mapGrid[x, y] = 1;
                            break;
                        case 1:
                            mapGrid[x, y] = 1;
                            mapGrid[x + 1, y] = 1;
                            break;
                        case 2:
                            mapGrid[x, y] = 1;
                            mapGrid[x, y + 1] = 1;
                            break;
                        case 3:
                            mapGrid[x, y] = 1;
                            mapGrid[x, y + 1] = 1;
                            mapGrid[x + 1, y] = 1;
                            mapGrid[x + 1, y + 1] = 1;
                            break;
                        case 4:
                            mapGrid[x, y] = 1;
                            mapGrid[x, y + 1] = 1;
                            mapGrid[x, y - 1] = 1;
                            break;
                        case 5:
                            mapGrid[x, y] = 1;
                            mapGrid[x - 1, y] = 1;
                            mapGrid[x + 1, y] = 1;
                            break;
                        case 6:
                            mapGrid[x, y] = 1;
                            mapGrid[x, y + 1] = 1;
                            mapGrid[x, y - 1] = 1;
                            mapGrid[x + 1, y] = 1;
                            mapGrid[x + 1, y + 1] = 1;
                            mapGrid[x + 1, y - 1] = 1;
                            break;
                        case 7:
                            mapGrid[x, y] = 1;
                            mapGrid[x - 1, y] = 1;
                            mapGrid[x + 1, y] = 1;
                            mapGrid[x, y + 1] = 1;
                            mapGrid[x - 1, y + 1] = 1;
                            mapGrid[x + 1, y + 1] = 1;
                            break;
                        case 8:
                            mapGrid[x, y] = 1;
                            mapGrid[x, y + 1] = 1;
                            mapGrid[x, y - 1] = 1;
                            mapGrid[x, y - 2] = 1;
                            break;
                        case 9:
                            mapGrid[x, y] = 1;
                            mapGrid[x + 1, y] = 1;
                            mapGrid[x - 1, y] = 1;
                            mapGrid[x - 2, y] = 1;
                            break;
                        case 10:
                            mapGrid[x, y] = 1;
                            mapGrid[x - 1, y] = 1;
                            mapGrid[x + 1, y] = 1;
                            mapGrid[x, y - 1] = 1;
                            mapGrid[x, y + 1] = 1;
                            mapGrid[x + 1, y + 1] = 1;
                            mapGrid[x + 1, y - 1] = 1;
                            mapGrid[x - 1, y + 1] = 1;
                            mapGrid[x - 1, y - 1] = 1;
                            break;
                    }
                }
            }
        }
    }

    private void GenerateMultiRooms(int minSize, int maxSize, RectInt rect) {
        //top side
        int topWidth = Random.Range(minSize, maxSize);
        int topHeight = Random.Range(minSize, maxSize);
        
        int topPosMinX = Random.Range(rect.xMin + 4, rect.xMax - 4);
        int topPosMinY = rect.yMax;
        if (topPosMinX + topWidth < mapGrid.GetLength(0) - 4 && 
            topPosMinY + topHeight < mapGrid.GetLength(1) - 4) {
            RectInt topRect = new RectInt(topPosMinX, topPosMinY, topWidth, topHeight);
            DrawRectangle(topRect);
            GenerateMultiRooms(minSize, maxSize, topRect);
        }

        //right side
        int rightWidth = Random.Range(minSize, maxSize);
        int rightHeight = Random.Range(minSize, maxSize);
        
        int rightPosMinX = rect.xMax;
        int rightPosMinY = Random.Range(rect.yMin + 4, rect.yMax - 4);
        if (rightPosMinX + rightWidth < mapGrid.GetLength(0) - 4 &&
            rightPosMinY + rightHeight < mapGrid.GetLength(1) - 4) {
            RectInt rightRect = new RectInt(rightPosMinX, rightPosMinY, rightWidth, rightHeight);
            DrawRectangle(rightRect);
            GenerateMultiRooms(minSize, maxSize, rightRect);
        }

    }
    

    private RectInt GenerateFirstRoom(int smallestSize, int biggestSize, int minX, int minY) {
        int width = Random.Range(smallestSize, biggestSize);
        int height = Random.Range(smallestSize, biggestSize);
        
        RectInt rect = new RectInt(minX, minY, width, height);

        DrawRectangle(rect);

        return rect;
    }


    private void DrawRectangle(RectInt rect)
    {
        for (int x = rect.xMin; x < rect.xMax; x++)
        {
            for (int y = rect.yMin; y < rect.yMax; y++)
            {
                mapGrid[x, y] = 0;
            }
        }
    }


    private int GetNeighborsCellCount(int x, int y) {
        int neighborCount = 0;
        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                neighborCount += mapGrid[x + i, y + j];
            }
        }
        return neighborCount - mapGrid[x,y];
    }

    private void MakeAllCellsWalls() {
        for (int i = 0; i < mapGrid.GetLength(0); i++) {
            for (int j = 0; j < mapGrid.GetLength(1); j++) {
                mapGrid[i, j] = 1;
            }
        }
    }
    public int[,] GetGrid() {
        return mapGrid;
    }

    public Pathfinding GetPathfinder()
    {
        return pathfinding;
    }
}
