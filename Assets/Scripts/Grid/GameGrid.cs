using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using UGrid = UnityEngine.Grid;

namespace STP.Grid
{
    public class GameGrid
    {
        UGrid m_grid;
        Tilemap m_WalkableTileMap;

        Dictionary<Vector3Int, Node> m_gridLookup = new Dictionary<Vector3Int, Node>();
        public Dictionary<Vector3Int, Node> GridLookup { get { return m_gridLookup; } }

        private int m_width;
        public int Width { get { return m_width; } }
        private int m_height;
        public int Height { get { return m_height; } }
        private Vector3Int m_cellSize;
        public Vector3Int CellSize { get { return m_cellSize; } }
        private Vector3Int m_originPosition;

        private TextMesh[,] debugTextArray;



        private GameObject gridTextParent;

        public GameGrid(UGrid grid, Tilemap groundTilemap)
        {
            m_grid = grid;
            m_WalkableTileMap = groundTilemap;
            m_cellSize = new Vector3Int
            {
                x = Mathf.RoundToInt(m_grid.cellSize.x),
                y = Mathf.RoundToInt(m_grid.cellSize.y),
                z = Mathf.RoundToInt(m_grid.cellSize.z)

            };
            SetUpGrid();
        }

        private void SetUpGrid()
        {
            SetUpGridBoundaries();
            debugTextArray = new TextMesh[m_width, m_height];


            Debug.Log("Creating new Grid of size: " + m_width + " by " + m_height);
            Debug.Log("Creating new Grid with a cell size of " + m_cellSize.x + ", " + m_cellSize.y);


            gridTextParent = new GameObject("Grid Text Parent");
            Transform transform = gridTextParent.transform;
            transform.SetParent(null, false);
            transform.localPosition = new Vector3(0, 0, 0);

            // Set up the new grid with the size
            // Once we have the grid size we can set it up, starting at the origin position.
            for (int x = 0; x < m_width; x++)
            {
                for (int y = 0; y < m_height; y++)
                {
                    bool isWalkable = m_WalkableTileMap.HasTile(GetWorldPosition(x, y));
                    Vector3Int gridNodeOrigin = new Vector3Int(x * m_cellSize.x, y * m_cellSize.y) + m_originPosition;
                    m_gridLookup.Add(gridNodeOrigin,
                                    new Node(gridNodeOrigin, m_cellSize, isWalkable));


                    Color labelColour = isWalkable ? Color.white : Color.red;
                    debugTextArray[x, y] = CreateWorldText(gridNodeOrigin.ToString(), CenterOfTile(x, y), labelColour);

                    Debug.Log("Creating GridTile at " + gridNodeOrigin);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                    // Debug.Log("Creating GridTile at " + x + ", " + y);
                    // Debug.DrawLine(new Vector3(x, y), new Vector3(x, y + m_cellSize.y), labelColour, 100f);
                    // Debug.DrawLine(new Vector3(x, y), new Vector3(x + m_cellSize.x, y), labelColour, 100f);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, m_height), GetWorldPosition(m_width, m_height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(m_width, 0), GetWorldPosition(m_width, m_height), Color.white, 100f);
        }

        private Vector3 CenterOfTile(Node tile)
        {
            return GetWorldPosition(tile.Origin.x, tile.Origin.y) + (Vector3)tile.CellSize * 0.5f;
        }

        private Vector3 CenterOfTile(int x, int y)
        {
            return GetWorldPosition((float)x, (float)y) + (Vector3)m_cellSize * 0.5f;
        }

        private void SetUpGridBoundaries()
        {
            Vector3Int startingOrigin = new Vector3Int(0, 0, 0);
            Vector3Int startingSize = new Vector3Int(0, 0, 0);

            Tilemap[] currentMap = m_grid.GetComponentsInChildren<Tilemap>();
            foreach (var map in currentMap)
            {
                if (map.origin.x < startingOrigin.x)
                    startingOrigin.x = map.origin.x;
                if (map.origin.y < startingOrigin.y)
                    startingOrigin.y = map.origin.y;

                // new size of the grid
                Vector3Int newSize = startingOrigin + startingSize;
                // size of the map we are checking
                Vector3Int mapSize = startingOrigin + map.size;

                if (mapSize.x > newSize.x)
                    startingSize.x = map.size.x;
                if (mapSize.y > newSize.y)
                    startingSize.y = map.size.y;
            }
            m_width = startingSize.x;
            m_height = startingSize.y;
            m_originPosition = startingOrigin;
        }

        public Vector3 GetWorldPosition(float x, float y)
        {
            return new Vector3(x * m_cellSize.x, y * m_cellSize.y) + m_originPosition;
        }

        public Vector3Int GetWorldPosition(int x, int y)
        {
            return new Vector3Int(x * m_cellSize.x, y * m_cellSize.y) + m_originPosition;
        }

        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt(worldPosition.x / m_cellSize.x);
            y = Mathf.FloorToInt(worldPosition.y / m_cellSize.y);
        }

        public Vector2Int GetXY(Vector3 worldPosition)
        {
            return new Vector2Int
            {
                x = Mathf.FloorToInt(worldPosition.x / m_cellSize.x),
                y = Mathf.FloorToInt(worldPosition.y / m_cellSize.y)
            };
        }

        public Node GetGridNodeAt(int x, int y)
        {
            if (m_gridLookup.ContainsKey(new Vector3Int(x, y)))
                return m_gridLookup[new Vector3Int(x, y)];

            return null;
        }

        public Vector3Int GetOriginPoint()
        {
            return m_originPosition;
        }

        // private void SetValue(int x, int y, int value)
        // {
        //     if (x >= m_originPosition.x && y >= m_originPosition.y && x < m_width && y < m_height)
        //     {
        //         // m_grid[x, y] = value;

        //         debugTextArray[x, y].text = m_gridTiles[x, y].ToString();
        //     }
        // }

        // public void SetValue(Vector3 worldPosition, int value)
        // {
        //     int x, y;
        //     GetXY(worldPosition, out x, out y);
        //     SetValue(x, y, value);
        // }

        // public int GetValue(int x, int y)
        // {
        //     // if (x >= 0 && y >= 0 && x < m_width && y < m_height)
        //     //     return m_grid[x, y];
        //     return -1;
        // }

        // public int GetValue(Vector3 worldPosition)
        // {
        //     int x, y;
        //     GetXY(worldPosition, out x, out y);
        //     return GetValue(x, y);
        // }


        private TextMesh CreateWorldText(string text, Vector3 localPosition, Color color)
        {
            GameObject newText = new GameObject("WorldText", typeof(TextMesh));
            Transform transform = newText.transform;
            transform.SetParent(gridTextParent.transform, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = newText.GetComponent<TextMesh>();
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            textMesh.text = text;
            textMesh.fontSize = 30;
            textMesh.characterSize = 0.1f;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = 5000;
            return textMesh;
        }

    }
}