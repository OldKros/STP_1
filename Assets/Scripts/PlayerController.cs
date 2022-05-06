using System;
using System.Collections;
using System.Collections.Generic;
using STP.Grid;
using STP.Pathfinding;
using STP.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

namespace STP
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 5f;
        [field:SerializeField] public Vector3 SpriteOffset { get; private set; }
        


        GridManager gridManager;
        Camera mainCamera;
        SpriteRenderer spriteRenderer;
        // NavMeshAgent navMeshAgent;
        
        Animator animator;
        bool canUpdateAnimator = true;
        Vector3 m_destination;
        Pathfinder pathfinder;
        List<PathNode> path;
        private int currentPathIndex;

        PlayerState playerState = PlayerState.Idle;
        GridTileEvent triggeredTileEvent = GridTileEvent.None;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (gridManager is null)
                gridManager = FindObjectOfType<GridManager>();
            pathfinder = new Pathfinder(gridManager.GameGrid);
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            animator = GetComponentInChildren<Animator>();
            // navMeshAgent = GetComponent<NavMeshAgent>();
            // navMeshAgent.updateRotation = false;
            // navMeshAgent.updateUpAxis = false;
        }

        // Update is called once per frame
        void Update()
        {
            ProcessMovement();
        }

        private void UpdateAnimatorBasedOnState()
        {
            if (!canUpdateAnimator) return;
            switch(playerState)
            {
                case PlayerState.Idle:
                    animator.SetBool("running", false);
                    break;
                case PlayerState.Falling:
                    animator.SetBool("falling", true);
                    break;
                case PlayerState.Moving:
                    animator.SetBool("running", true);
                    break;
                default:
                    animator.SetBool("running", false);
                    break;
            }
        }

        private void ProcessMovement()
        {
            if (playerState != PlayerState.Idle && playerState != PlayerState.Moving) return;

            if (Input.GetMouseButtonDown(0))
            {
                GetNewMovementPath();
            }

            if (path == null) return;

            if (Vector3.Distance(transform.position, path[currentPathIndex].Origin) < 0.01)
            {
                if (path[currentPathIndex] is EventNode)
                {
                    triggeredTileEvent = (path[currentPathIndex] as EventNode).TileEvent;
                }
                currentPathIndex++;
            }

            if (currentPathIndex < path.Count)
            {
                //Trigger tile event if it is an event node
                
                float movementThisFrame = moveSpeed * Time.deltaTime;
                var transformSnapShot = transform.position;
                transform.position = Vector3.MoveTowards(transform.position, (Vector3)path[currentPathIndex].Origin, movementThisFrame);
                spriteRenderer.flipX = path[currentPathIndex].Origin.x < transformSnapShot.x ;
            }
            else
            {
                path = null;
            }

        }

        private void GetNewMovementPath()
        {
            gridManager.GameGrid.GetXY(mainCamera.ScreenToWorldPoint(Input.mousePosition), out int endX, out int endY);

            Vector3 movementFromPosition = path == null ? transform.position : path[currentPathIndex].Origin;
            gridManager.GameGrid.GetXY(movementFromPosition, out int startX, out int startY);

            path = pathfinder.FindPath(startX, startY, endX, endY);
            currentPathIndex = 0;

            if (path != null)
            {
                //Debug.Log("Path is not null: " + path.Count);
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(path[i].Origin + Vector3.one * 0.5f, path[i + 1].Origin + Vector3.one * 0.5f, Color.red, 2f);
                }
            }
        }

        private void UpdateState()
        {
            if (triggeredTileEvent != GridTileEvent.None)
            {
                switch (triggeredTileEvent)
                {
                    case GridTileEvent.Fall:
                        playerState = PlayerState.Falling;
                        break;
                    case GridTileEvent.Interactible:
                        playerState = PlayerState.Interacting;
                        break;
                    default:
                        break;
                }
            }
            else if (path != null && currentPathIndex < path.Count)
            {
                playerState = PlayerState.Moving;
            }
            else
            {
                playerState = PlayerState.Idle;
            }
            UpdateAnimatorBasedOnState();
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Portal"))
            {
                playerState = PlayerState.Falling;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Portal"))
            {
                playerState = PlayerState.Idle;
            }
        }


        private void OnDisable()
        {
            path = null;
        }


        private enum PlayerState
        {
            Idle,
            Moving,
            Falling,
            Interacting
        }
    }
}