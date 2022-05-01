using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STP.Grid
{

    public class GridTile
    {
        public Vector3 Origin { get; private set; }
        public Vector3 CellSize { get; private set; }

        public bool IsWalkable { get; private set; }
        public bool IsBlocked { get; private set; }

        public GridTile(Vector3 origin, Vector3 cellSize, bool isWalkable)
        {
            Origin = origin;
            CellSize = cellSize;
            IsWalkable = isWalkable;
            IsBlocked = false;
        }

        public override string ToString()
        {
            return $"({Origin.x}, {Origin.y})";
        }
    }

}
