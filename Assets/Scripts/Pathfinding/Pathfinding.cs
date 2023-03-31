using System.Collections.Generic;
using UnityEngine;

public class Pathfinding {

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAG_COST = 14;
    
    private Node[,] grid;
    private List<Node> openList;
    private List<Node> closedList;
    
    public Pathfinding(MapGenerator generator) {
        
        int[,] basicGrid = generator.GetGrid();
        ConvertToNodeGrid(basicGrid);
    }

    private void ConvertToNodeGrid(int[,] basicGrid) {
        grid = new Node[basicGrid.GetLength(0), basicGrid.GetLength(1)];
        for (int x = 0; x < basicGrid.GetLength(0); x++) {
            for (int y = 0; y < basicGrid.GetLength(1); y++) {
                Node node = new Node(x, y);
                node.value = basicGrid[x, y];
                grid[x, y] = node;
            }
        }
    }

    public List<Vector2> FindPath(Vector2 startPos, Vector2 endPos)
    {
        List<Node> findPath = FindPath((int) startPos.x, (int) startPos.y, (int) endPos.x, (int) endPos.y);
        List<Vector2> positions = new List<Vector2>();

        foreach (Node node in findPath)
        {
            Vector2 vector2 = new Vector2(node.x + 0.5f, node.y + 0.5f);
            positions.Add(vector2);
        }


        return positions;
    }
    
    public List<Node> FindPath(int startX, int startY, int endX, int endY) {
        Node startNode = grid[startX, startY];
        Node endNode = grid[endX, endY];

        openList = new List<Node>() {startNode};
        closedList = new List<Node>();

        for (int x = 0; x < grid.GetLength(0); x++) {
            for (int y = 0; y < grid.GetLength(1); y++) {
                Node node = grid[x, y];
                node.gCost = int.MaxValue;
                node.CalculateFCost();
                node.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0) {
            Node currentNode = GetLowestFCost();
            if (currentNode == endNode) {
                //recursively loopback to start
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (Node neighborNode in GetNeighbors(currentNode)) {
                if (closedList.Contains(neighborNode)) continue;

                int tentativeGCost = currentNode.gCost + CalculateDistCost(currentNode, neighborNode);
                if (tentativeGCost < neighborNode.gCost) {
                    neighborNode.cameFromNode = currentNode;
                    neighborNode.gCost = tentativeGCost;
                    neighborNode.hCost = CalculateDistCost(neighborNode, endNode);
                    neighborNode.CalculateFCost();

                    if (!openList.Contains(neighborNode)) {
                        openList.Add(neighborNode);
                    }

                }

            }
            

        }
        
        //Out of nodes on the openList
        Debug.Log("Unable to find path to target");
        return null;
    }

    private List<Node> GetNeighbors(Node node) {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {

                if (x == 0 && y == 0) {
                    continue;
                }
                
                int nodeX = node.x + x;
                int nodeY = node.y + y;
                
                if (nodeX < 0 || nodeX >= grid.GetLength(0) || nodeY < 0 || nodeY >= grid.GetLength(1)) {
                    continue;
                }

                if (grid[nodeX, nodeY].value == 0) {
                    neighbors.Add(grid[nodeX, nodeY]);
                }
            }
        }
        
        return neighbors;
    }

    private List<Node> CalculatePath(Node endNode) {
        List<Node> path = new List<Node>();
        path.Add(endNode);
        Node currentNode = endNode;
        while (currentNode.cameFromNode != null) {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistCost(Node a, Node b) {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int dist = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAG_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * dist;
    }

    private Node GetLowestFCost() {
        Node tempNode = openList[0];
        for (int i = 1; i < openList.Count; i++) {
            if (openList[i].fCost < tempNode.fCost) {
                tempNode = openList[i];
            }
        }
        return tempNode;
    }

}