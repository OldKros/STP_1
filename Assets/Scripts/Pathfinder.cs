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

        private Dictionary<Vector3Int, PathNode> openNodes;
        private Dictionary<Vector3Int, PathNode> checkedNodes;


        // public Pathfinder(UGrid grid, Tilemap groundTilemap)
        // {
        //     gameGrid = new GameGrid(grid, groundTilemap);
        // }

        public Pathfinder(GameGrid gameGrid)
        {
            this.gameGrid = gameGrid;
            gridLookup = new Dictionary<Vector3Int, PathNode>();
            openNodes = new();
            checkedNodes = new();
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
            Vector3Int endPosition = new Vector3Int(endX, endY);
            Vector3Int startPosition = new Vector3Int(startX, startY);

            int maxAttempts = 50;
            // Try to find the closest walkable tile to the one clicked on.
            for (int i = 0; i < maxAttempts; i++)
            {
                if (gridLookup.ContainsKey(endPosition) && gridLookup[endPosition].IsWalkable)
                { break; }

                Vector3 difference = endPosition - startPosition;

                Vector3Int cellSize = gameGrid.CellSize;

                if (difference.x == 0)
                {
                    endPosition.y = difference.y > 0 ? endPosition.y -= cellSize.y : endPosition.y += cellSize.y;
                }
                else if (difference.y == 0)
                {
                    endPosition.x = difference.x > 0 ? endPosition.x -= cellSize.x : endPosition.x += cellSize.x;
                }
                else if (difference.x == difference.y)
                {
                    endPosition.x = difference.x > 0 ? endPosition.x -= cellSize.x : endPosition.x += cellSize.x;
                }
                else 
                {
                    if (Math.Abs(difference.x) < Math.Abs(difference.y))
                    {
                        endPosition.y = difference.y > 0 ? endPosition.y -= cellSize.y : endPosition.y += cellSize.y;
                    }
                    else
                    {
                        endPosition.x = difference.x > 0 ? endPosition.x -= cellSize.x : endPosition.x += cellSize.x;
                    }
                }
                    
            }

            // If we still havent found a path then the we will ignore the click
            if (!gridLookup.ContainsKey(endPosition) || !gridLookup.ContainsKey(startPosition)) return null;

            PathNode startNode = gridLookup[startPosition];
            PathNode endNode = gridLookup[endPosition];


            Debug.Log("Grid Size: " + gridLookup.Count);

            openNodes.Clear();
            checkedNodes.Clear();

            openNodes.Add(startNode.Origin,startNode);
            //closedList.Add(startNode.Origin, startNode);

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

            //openList.Add(currentNode.Origin);
            //closedList.Add(startNode.Origin, startNode);

            while (openNodes.Count > 0)
            {
                PathNode currentNode = getLowestFCostNode(openNodes);
                if (currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }

                openNodes.Remove(currentNode.Origin);
                checkedNodes.Add(currentNode.Origin, currentNode);

                foreach (var neighbour in GetNeighbourList(currentNode))
                {
                    if (!neighbour.IsWalkable) checkedNodes.Add(neighbour.Origin, neighbour);
                    if (checkedNodes.ContainsKey(neighbour.Origin)) continue;


                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbour);
                    if (tentativeGCost < neighbour.gCost)
                    {
                        neighbour.connectedNode = currentNode;
                        neighbour.gCost = tentativeGCost;
                        neighbour.hCost = CalculateDistanceCost(neighbour, endNode);
                        neighbour.CalculateFCost();

                        if (!openNodes.ContainsKey(neighbour.Origin))
                            openNodes.Add(neighbour.Origin, neighbour);
                    }
                }
            }

            // Out of nodes on openList
            return null;
        }

        private List<PathNode> GetNeighbourList(PathNode node)
        {
            List<PathNode> viableNeighbors = new List<PathNode>(); ;
            PathNode neighbor;
            //System.Random random = new System.Random();

            foreach (Vector3Int direction in MOVE_DIRECTIONS)
            {
                Vector3Int neighborCoords = node.Origin + direction;

                if (direction.x !=0 && direction.y != 0) 
                {
                    Vector3Int neighborLeftRight = new Vector3Int
                    {
                        x = node.Origin.x + direction.x,
                        y = node.Origin.y,
                        z = node.Origin.z
                    };
                    Vector3Int neighborAboveBelow = new Vector3Int
                    {
                        x = node.Origin.x,
                        y = node.Origin.y + direction.y,
                        z = node.Origin.z
                    };

                    if (!gridLookup.ContainsKey(neighborAboveBelow) || !gridLookup.ContainsKey(neighborLeftRight)
                        || !gridLookup[neighborAboveBelow].IsWalkable || !gridLookup[neighborLeftRight].IsWalkable)
                    {
                        continue;
                    }
                }
                
                if (gridLookup.ContainsKey(neighborCoords))
                {
                    neighbor = gridLookup[neighborCoords];
                    viableNeighbors.Add(neighbor);
                }
            }

            return viableNeighbors;
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

        private PathNode getLowestFCostNode(Dictionary<Vector3Int, PathNode> pathNodes)
        {
            PathNode lowestFCostNode = null;

            foreach(var node in pathNodes)
            {
                if (lowestFCostNode == null) { lowestFCostNode = node.Value; }

                if (node.Value.fCost < lowestFCostNode.fCost)
                {
                    lowestFCostNode = node.Value;
                }
            }
            return lowestFCostNode;
        }
    }
}