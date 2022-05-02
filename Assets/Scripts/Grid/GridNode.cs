using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STP.Grid
{

    public class GridNode : Node
    {

        public GridNode(Vector3Int origin, Vector3Int cellSize, bool isWalkable)
        : base(origin, cellSize, isWalkable)
        {

        }



    }
}
