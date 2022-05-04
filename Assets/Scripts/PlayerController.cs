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
    [SerializeField] Vector3 spriteOffset;
    Camera mainCamera;
    SpriteRenderer spriteRenderer;
    // NavMeshAgent navMeshAgent;
    Animator animator;

    Vector3 m_destination;
    Pathfinder pathfinder;
    List<PathNode> path;
    private int currentPathIndex;
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
            Debug.Log(transform.position);
            gridManager.GameGrid.GetXY(transform.position - spriteOffset, out int startX, out int startY);
            path = pathfinder.FindPath(startX, startY, endX, endY);
            currentPathIndex = 0;
            if (path != null)
            {
                //Debug.Log("Path is not null: " + path.Count);
                for (int i = 0; i < path.Count-1; i++)
                {
                    Debug.DrawLine(path[i].Origin + Vector3.one * 0.5f, path[i + 1].Origin + Vector3.one * 0.5f, Color.red, 2f);
                }
            }

            // navMeshAgent.destination = destination;
            // navMeshAgent.isStopped = false;
        }
        if (path != null)
        {
            
            if (Vector3.Distance(transform.position, path[currentPathIndex].Origin + spriteOffset) < 0.01)
            {
                currentPathIndex++;
            }

            if (currentPathIndex < path.Count)
            {
                float movementThisFrame = moveSpeed * Time.deltaTime;
                var transformSnapShot = transform.position;
                transform.position = Vector3.MoveTowards(transform.position -spriteOffset, (Vector3)path[currentPathIndex].Origin, movementThisFrame) + spriteOffset;
                spriteRenderer.flipX = path[currentPathIndex].Origin.x < transformSnapShot.x - spriteOffset.x;
            }
            else
            {
                path = null;
            }
        }
    }

    private void UpdateAnimator()
    {
        // Vector3 velocity = navMeshAgent.velocity;
        //Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        // float speed = localVelocity.x + localVelocity.y;
        // Debug.Log(speed);
        if (path != null && currentPathIndex < path.Count)
        {
            animator.SetBool("running", true);
            //Vector3 localVelocity = transform.TransformDirection(path[currentPathIndex].Origin);
            //var animatorSpeed = localVelocity.x + localVelocity.y;
            //animator.SetFloat("movementSpeed", Mathf.Abs(animatorSpeed));
        }
        animator.SetBool("running", false);
    }


}
