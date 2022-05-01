using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace STP.Grid
{
    public class Grid
    {
        Tilemap m_TileMap;

        private int m_width;
        private int m_height;
        private Vector3 m_cellSize;
        private Vector3 m_originPosition;
        // private int[,] m_grid;
        private GridTile[,] m_grid;

        private TextMesh[,] debugTextArray;
        private GameObject gridTextParent;

        public Grid(Tilemap tileMap)
        {
            m_TileMap = tileMap;

            m_width = m_TileMap.size.x;
            m_height = m_TileMap.size.y;

            m_cellSize = m_TileMap.layoutGrid.cellSize;
            m_originPosition = m_TileMap.origin;
            SetUpGrid();
        }


        public Grid(int width, int height, float cellSize, Vector3 originPosition)
        {
            m_width = width;
            m_height = height;
            m_cellSize = new Vector3(cellSize, cellSize);
            m_originPosition = originPosition;

            SetUpGrid();
        }

        private void SetUpGrid()
        {
            m_grid = new GridTile[m_width, m_height];
            debugTextArray = new TextMesh[m_width, m_height];
            Debug.Log("Creating new Grid of size: " + m_width + " by " + m_height);

            gridTextParent = new GameObject("Grid Text");
            Transform transform = gridTextParent.transform;
            transform.SetParent(null, false);
            transform.localPosition = new Vector3(0, 0, 0);


            for (int x = 0; x < m_grid.GetLength(0); x++)
            {
                for (int y = 0; y < m_grid.GetLength(1); y++)
                {
                    m_grid[x, y] = new GridTile(new Vector3(x * m_cellSize.x, y * m_cellSize.y), m_cellSize, true);
                    Debug.Log("Creating GridTile at " + x * m_cellSize.x + ", " + y * m_cellSize.y);

                    debugTextArray[x, y] = CreateWorldText(m_grid[x, y].ToString(), GetWorldPosition(x, y) + m_cellSize * 0.5f, Color.white);

                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, m_height), GetWorldPosition(m_width, m_height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(m_width, 0), GetWorldPosition(m_width, m_height), Color.white, 100f);
        }

        private Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x * m_cellSize.x, y * m_cellSize.y) + m_originPosition;
        }

        private void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - m_originPosition).x / m_cellSize.x);
            y = Mathf.FloorToInt((worldPosition - m_originPosition).y / m_cellSize.y);
        }

        private void SetValue(int x, int y, int value)
        {
            if (x >= m_originPosition.x && y >= m_originPosition.y && x < m_width && y < m_height)
            {
                // m_grid[x, y] = value;

                debugTextArray[x, y].text = m_grid[x, y].ToString();
            }
        }

        public void SetValue(Vector3 worldPosition, int value)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            SetValue(x, y, value);
        }

        public int GetValue(int x, int y)
        {
            // if (x >= 0 && y >= 0 && x < m_width && y < m_height)
            //     return m_grid[x, y];
            return -1;
        }

        public int GetValue(Vector3 worldPosition)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            return GetValue(x, y);
        }

        public void ToggleGridLabels()
        {
            gridTextParent.SetActive(!gridTextParent.activeInHierarchy);
        }


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