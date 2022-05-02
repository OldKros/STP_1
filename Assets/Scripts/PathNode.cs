using System;
using System.Collections;
using System.Collections.Generic;
using STP.Grid;
using UnityEngine;

namespace STP.Pathfinding
{

    public class PathNode : Node
    {
        public PathNode connectedNode;

        public int gCost;
        public int hCost;
        public int fCost;

        public PathNode(Vector3Int origin, Vector3Int cellSize, bool isWalkable)
        : base(origin, cellSize, isWalkable)
        {
        }


        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }

    }

}
