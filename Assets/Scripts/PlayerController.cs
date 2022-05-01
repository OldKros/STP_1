using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float moveSpeed = 5f;
    Camera mainCamera;
    Tilemap walkableTileMap;
    SpriteRenderer spriteRenderer;
    // NavMeshAgent navMeshAgent;
    Animator animator;


    Vector3 m_destination;


    private void Awake()
    {
        mainCamera = Camera.main;
        walkableTileMap = GameObject.FindGameObjectWithTag("WalkableTileMap").GetComponent<Tilemap>();

    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        // navMeshAgent = GetComponent<NavMeshAgent>();
        // navMeshAgent.updateRotation = false;
        // navMeshAgent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimator();
        ProcessMovement();
    }

    private void ProcessMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_destination = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (m_destination.x < transform.position.x) spriteRenderer.flipX = true;
            else spriteRenderer.flipX = false;
            // navMeshAgent.destination = destination;
            // navMeshAgent.isStopped = false;
        }

        // if (navMeshAgent.isStopped) animator.SetBool("isIdle", true);
        float movementThisFrame = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, m_destination, movementThisFrame);
    }

    private void UpdateAnimator()
    {
        // Vector3 velocity = navMeshAgent.velocity;
        // Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        // float speed = localVelocity.x + localVelocity.y;
        // Debug.Log(speed);
        // animator.SetFloat("movementSpeed", Mathf.Abs(speed));
    }

    private void FindFastestPathOnGrid(Vector3 destination)
    {
        Vector3Int currentGridCell = walkableTileMap.WorldToCell(transform.position);
        Vector3Int destinationGridCell = walkableTileMap.WorldToCell(destination);

        Vector3Int distanceInGridCell = destinationGridCell - currentGridCell;
        distanceInGridCell.z = currentGridCell.z;

        Vector3Int tileLocationToTest;

        Queue<Vector2Int> cellMovePositions = new Queue<Vector2Int>();

        MovementDirection movementDirection = MovementDirection.Right;

        while (distanceInGridCell.x != 0 || distanceInGridCell.y != 0)
        {
            //Which Directions do we want to move?
            int xCellsToMove = Mathf.Abs(distanceInGridCell.x);
            int yCellsToMove = Mathf.Abs(distanceInGridCell.y);

            // if we have to move diagonally?
            if (xCellsToMove == yCellsToMove)
            {
                if (distanceInGridCell.x < 0 && distanceInGridCell.y < 0)
                    movementDirection = MovementDirection.DownLeft;
                else if (distanceInGridCell.x < 0 && distanceInGridCell.y > 0)
                    movementDirection = MovementDirection.UpLeft;
                else if (distanceInGridCell.x > 0 && distanceInGridCell.y < 0)
                    movementDirection = MovementDirection.DownRight;
                else if (distanceInGridCell.x > 0 && distanceInGridCell.y > 0)
                    movementDirection = MovementDirection.UpRight;
            }
            // If we have to move in the X direction
            else if (xCellsToMove > yCellsToMove)
            {
                if (distanceInGridCell.x > 0)
                    movementDirection = MovementDirection.Right;
                else
                    movementDirection = MovementDirection.Left;
            }
            // If we have to move in the Y direction.
            else if (xCellsToMove < yCellsToMove)
            {
                if (distanceInGridCell.y > 0)
                    movementDirection = MovementDirection.Up;
                else
                    movementDirection = MovementDirection.Down;
                distanceInGridCell.y--;
            }


            switch (movementDirection)
            {
                case MovementDirection.Up:
                    tileLocationToTest = currentGridCell + Vector3Int.up;
                    break;
                case MovementDirection.Right:
                    tileLocationToTest = currentGridCell + Vector3Int.right;
                    break;
                case MovementDirection.Down:
                    tileLocationToTest = currentGridCell + Vector3Int.down;
                    break;
                case MovementDirection.Left:
                    tileLocationToTest = currentGridCell + Vector3Int.left;
                    break;
                case MovementDirection.UpRight:
                    tileLocationToTest = currentGridCell + Vector3Int.up + Vector3Int.right;
                    break;
                case MovementDirection.DownRight:
                    tileLocationToTest = currentGridCell + Vector3Int.down + Vector3Int.right;
                    break;
                case MovementDirection.UpLeft:
                    tileLocationToTest = currentGridCell + Vector3Int.up + Vector3Int.left;
                    break;
                case MovementDirection.DownLeft:
                    tileLocationToTest = currentGridCell + Vector3Int.down + Vector3Int.left;
                    break;
                default:
                    tileLocationToTest = currentGridCell;
                    break;
            }


            if (walkableTileMap.GetTile(tileLocationToTest) != null)
            {
                m_destination = tileLocationToTest;
            }
            else
            {

            }



        }


    }


    private enum MovementDirection
    {
        Up,
        Right,
        Down,
        Left,
        UpRight,
        DownRight,
        UpLeft,
        DownLeft
    }
}
