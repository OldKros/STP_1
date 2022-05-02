using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STP.Grid
{


    public class TileLabeller : MonoBehaviour
    {

        [SerializeField] GridManager gridManager;
        [SerializeField] GameObject gridTextParent;

        private TextMesh[,] debugTextArray;


        // Start is called before the first frame update
        void Start()
        {

        }


        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                ToggleGridLabels();
            }
        }

        public void ToggleGridLabels()
        {
            gridTextParent.SetActive(!gridTextParent.activeInHierarchy);
        }


        private void SetUpGridText()
        {
            // for(int x =0; x < gridManager.GameGrid.Width; x++)
            // {
            //     for (int y=0; y < gridManager.GameGrid.Height; y++)
            //     {
            //         debugTextArray[x, y] = CreateWorldText(m_gridTiles[x, y].ToString(), GetWorldPosition(x, y) + m_cellSize * 0.5f, Color.white);
            //     }
            // }
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