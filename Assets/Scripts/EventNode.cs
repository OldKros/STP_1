using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STP.Grid;
using STP.Pathfinding;

namespace STP.Pathfinding
{

    public class EventNode : PathNode
    {
        GridTileEvent tileEvent;
        public GridTileEvent TileEvent { get => tileEvent; set => tileEvent = value; }

        public EventNode(Node node, GridTileEvent tileEvent) : base(node)
        {
            this.TileEvent = tileEvent;
        }


    }
}