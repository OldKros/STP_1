using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using STP.Grid;

public class Testing : MonoBehaviour
{
    [SerializeField] Tilemap tileMap;

    private GameGrid grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = new GameGrid(tileMap);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.SetValue(worldPos, 56);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            grid.ToggleGridLabels();
        }
    }

}
