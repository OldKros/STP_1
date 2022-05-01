using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace STP.Grid
{

    public class GridManager : MonoBehaviour
    {
        [SerializeField] Tilemap groundTileMap;
        [SerializeField] Tilemap interactableTileMap;
        [SerializeField] Tilemap colliderTileMap;

        [SerializeField] Dictionary<Tile, GameObject> tileReplacementMap;

        private Grid grid;
        // Start is called before the first frame update
        void Start()
        {
            grid = new Grid(groundTileMap);
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}
