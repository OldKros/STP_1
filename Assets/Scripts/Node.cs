using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STP
{
    public class Node
    {
        public Vector3Int Origin { get; protected set; }
        public Vector3Int CellSize { get; protected set; }

        public bool IsWalkable { get; protected set; }

        public Node(Vector3Int origin, Vector3Int cellSize, bool isWalkable)
        {
            Origin = origin;
            CellSize = cellSize;
            IsWalkable = isWalkable;
        }

        public override string ToString()
        {
            return $"({Origin.x}, {Origin.y})";
        }
    }
}
