using System.Collections;
using System.Collections.Generic;
using STP.Pathfinding;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] STP.Grid.GridManager gridManager;
    [SerializeField] float moveSpeed = 5f;
    Camera mainCamera;
    SpriteRenderer spriteRenderer;
    // NavMeshAgent navMeshAgent;
    Animator animator;

    Vector3 m_destination;
    Pathfinder pathfinder;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        pathfinder = new Pathfinder(gridManager.GameGrid);
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
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            gridManager.GameGrid.GetXY(mouseWorldPos, out int endX, out int endY);
            gridManager.GameGrid.GetXY(transform.position, out int startX, out int startY);
            List<PathNode> path = pathfinder.FindPath(startX, startY, endX, endY);

            if (path != null)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    Debug.DrawLine(path[i].Origin, path[i + 1].Origin, Color.red, 2f);
                }
            }

            // m_destination = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            // if (m_destination.x < transform.position.x) spriteRenderer.flipX = true;
            // else spriteRenderer.flipX = false;
            // navMeshAgent.destination = destination;
            // navMeshAgent.isStopped = false;
        }

        // if (navMeshAgent.isStopped) animator.SetBool("isIdle", true);
        float movementThisFrame = moveSpeed * Time.deltaTime;
        // transform.position = Vector2.MoveTowards(transform.position, m_destination, movementThisFrame);
    }

    private void UpdateAnimator()
    {
        // Vector3 velocity = navMeshAgent.velocity;
        // Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        // float speed = localVelocity.x + localVelocity.y;
        // Debug.Log(speed);
        // animator.SetFloat("movementSpeed", Mathf.Abs(speed));
    }


}
