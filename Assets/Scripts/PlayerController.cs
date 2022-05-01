using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float moveSpeed = 5f;
    Camera mainCamera;
    Tilemap tileMap;


    Vector3 destination;


    private void Awake()
    {
        mainCamera = Camera.main;
        tileMap = GameObject.FindGameObjectWithTag("WalkableTileMap").GetComponent<Tilemap>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            destination = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            destination.z = transform.position.z;
        }
        float movementThisFrame = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, destination, movementThisFrame);
    }
}
