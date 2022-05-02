using UnityEngine;
using UnityEngine.Tilemaps;
using UGrid = UnityEngine.Grid;

using STP.Grid;
using System.Collections.Generic;
using System;

namespace STP.Pathfinding
{

    public class Pathfinder
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGANOL_COST = 14;

        private readonly Vector3Int[] MOVE_DIRECTIONS = {   new Vector3Int(1,1,0),
                                                            new Vector3Int(1,-1,0),
                                                            new Vector3Int(-1,1,0),
                                                            new Vector3Int(-1,-1,0),
                                                        Vector3Int.right, Vector3Int.left,
                                                        Vector3Int.up, Vector3Int.down };


        private GameGrid gameGrid;

        private Dictionary<Vector3Int, PathNode> gridLookup;

        private List<PathNode> openList;
        private List<PathNode> closedList;


        // public Pathfinder(UGrid grid, Tilemap groundTilemap)
        // {
        //     gameGrid = new GameGrid(grid, groundTilemap);
        // }

        public Pathfinder(GameGrid gameGrid)
        {
            this.gameGrid = gameGrid;
            gridLookup = new Dictionary<Vector3Int, PathNode>();

        }

        public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
        {
            //PathNode startNode = new PathNode(gameGrid.GetGridNodeAt(startX, startY));
            //PathNode endNode = new PathNode(gameGrid.GetGridNodeAt(endX, endY));

            Debug.Log(gameGrid.GridLookup.Count);


            if (gridLookup.Count == 0)
            {
                foreach (var keyValuePair in gameGrid.GridLookup)
                {
                    Debug.Log(keyValuePair);
                    gridLookup.Add(
                        keyValuePair.Key,
                        new PathNode(keyValuePair.Value)
                    );
                }
            }

            PathNode startNode = gridLookup[new Vector3Int(startX, startY)];
            PathNode endNode = gridLookup[new Vector3Int(endX, endY)];


            Debug.Log("Grid Size: " + gridLookup.Count);

            openList = new List<PathNode> { startNode };
            closedList = new List<PathNode>();

            Vector3Int origin = gameGrid.GetOriginPoint();

            for (int x = 0; x < gameGrid.Width; x++)
            {
                for (int y = 0; y < gameGrid.Height; y++)
                {
                    Vector3Int coordsToCheck = new Vector3Int(x, y) + origin;
                    Debug.Log(gridLookup.ContainsKey(coordsToCheck));
                    PathNode pathNode = gridLookup[(coordsToCheck)];
                    Debug.Log("Checking node: " + pathNode + " at " + (coordsToCheck));
                    pathNode.gCost = int.MaxValue;
                    pathNode.CalculateFCost();
                    pathNode.connectedNode = null;
                }
            }

            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();

            while (openList.Count > 0)
            {
                PathNode currentNode = getLowestFCostNode(openList);
                if (currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (var neighbour in GetNeighbourList(currentNode))
                {
                    if (!neighbour.IsWalkable) closedList.Add(neighbour);
                    if (closedList.Contains(neighbour)) continue;


                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbour);
                    if (tentativeGCost < neighbour.gCost)
                    {
                        neighbour.connectedNode = currentNode;
                        neighbour.gCost = tentativeGCost;
                        neighbour.hCost = CalculateDistanceCost(neighbour, endNode);
                        neighbour.CalculateFCost();

                        if (!openList.Contains(neighbour))
                            openList.Add(neighbour);
                    }
                }
            }

            // Out of nodes on openList
            return null;
        }

        private List<PathNode> GetNeighbourList(PathNode node)
        {
            List<PathNode> neighbors = new List<PathNode>(); ;
            PathNode neighbor;
            //System.Random random = new System.Random();

            foreach (Vector3Int direction in MOVE_DIRECTIONS)
            {
                Vector3Int neighborCoords = node.Origin + direction;

                if (gridLookup.ContainsKey(neighborCoords))
                {
                    neighbor = gridLookup[neighborCoords];
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }

        private List<PathNode> CalculatePath(PathNode endNode)
        {
            List<PathNode> path = new List<PathNode>();
            path.Add(endNode);
            PathNode currentNode = endNode;
            while (currentNode.connectedNode != null)
            {
                path.Add(currentNode.connectedNode);
                currentNode = currentNode.connectedNode;
            }
            path.Reverse();
            return path;
        }

        private int CalculateDistanceCost(PathNode a, PathNode b)
        {
            int xDistance = Mathf.Abs(a.Origin.x - b.Origin.x);
            int yDistance = Mathf.Abs(a.Origin.y - b.Origin.y);
            int remaining = Mathf.Abs(xDistance - yDistance);
            return MOVE_DIAGANOL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        private PathNode getLowestFCostNode(List<PathNode> pathNodes)
        {
            PathNode lowestFCostNode = pathNodes[0];
            for (int i = 1; i < pathNodes.Count; i++)
            {
                if (pathNodes[i].fCost < lowestFCostNode.fCost)
                {
                    lowestFCostNode = pathNodes[i];
                }
            }
            return lowestFCostNode;
        }
    }
}