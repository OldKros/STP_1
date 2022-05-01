using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using STP.Grid;
using Grid = STP.Grid.Grid;

public class Testing : MonoBehaviour
{
    [SerializeField] Tilemap tileMap;

    private Grid grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid(tileMap);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.SetValue(worldPos, 56);
        }
    }

}
