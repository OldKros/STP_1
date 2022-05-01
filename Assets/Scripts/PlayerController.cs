using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float moveSpeed = 5f;
    Camera mainCamera;
    Tilemap tileMap;
    SpriteRenderer spriteRenderer;
    NavMeshAgent navMeshAgent;
    Animator animator;


    Vector3 destination;


    private void Awake()
    {
        mainCamera = Camera.main;
        tileMap = GameObject.FindGameObjectWithTag("WalkableTileMap").GetComponent<Tilemap>();

    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimator();
        if (Input.GetMouseButtonDown(0))
        {
            destination = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (destination.x < transform.position.x) spriteRenderer.flipX = true;
            else spriteRenderer.flipX = false;
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }

        if (navMeshAgent.isStopped) animator.SetBool("isIdle", true);
        // float movementThisFrame = moveSpeed * Time.deltaTime;
        // transform.position = Vector2.MoveTowards(transform.position, destination, movementThisFrame);
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.x + localVelocity.y;
        Debug.Log(speed);
        animator.SetFloat("movementSpeed", Mathf.Abs(speed));
    }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (other.gameObject.tag == "Static")
    //     { destination = transform.position; }
    // }
}
