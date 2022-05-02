using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using STP.Grid;

public class Testing : MonoBehaviour
{
    [SerializeField] UnityEngine.Grid grid;
    [SerializeField] Tilemap tileMap;

    private GameGrid gameGrid;
    // Start is called before the first frame update
    void Start()
    {
        gameGrid = new GameGrid(grid, tileMap);
    }

    // Update is called once per frame
    void Update()
    {

    }

}
