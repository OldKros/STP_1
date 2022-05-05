using STP.Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace STP.Grid
{

    public class GridManager : MonoBehaviour
    {
        [SerializeField] UnityEngine.Grid grid;

        [SerializeField] GridTile[] eventTiles;

        private GameGrid m_gameGrid;
        public GameGrid GameGrid { get { return m_gameGrid; } }
        
        // Start is called before the first frame update
        void Awake()
        {
            m_gameGrid = new GameGrid(grid);
            m_gameGrid.SetEventTiles(eventTiles);
        }

        private void Start()
        {

        }


        // Update is called once per frame
        void Update()
        {

        }


        public int GetGridHeight()
        {
            return m_gameGrid.Height;
        }

        public int GetGridWidth()
        {
            return m_gameGrid.Width;
        }

        public bool HasEventTileAtPosition(Vector3Int position)
        {
            return m_gameGrid.HasEventTileAt(position);
        }

    }

    [System.Serializable]
    public struct GridTile
    {
        public Tile tile;
        public GridTileEvent gridTileEvent;

    }
}
