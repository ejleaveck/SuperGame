using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;

public class PlayerMovement : MonoBehaviour
{
    // Movement variables
    float moveDirection = 0;
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float runSpeed = 20f;
    [SerializeField] private float accelerationTime = 1f;

    private float currentSpeed;
    private float targetSpeed;
    private float speedSmoothing;

    private bool isRunning;

    // Jump variables
    bool canJump = false;
    bool isJumping = false;
    [SerializeField] private float jumpForce = 30f;
    [SerializeField] private float fallForceMultiplier = 10f;

    public LayerMask jumpableLayer;
    public Transform groundCheck;
    public float groundCheckWidth = .5f;
    public float groundCheckHeight = .5f;

    // Player state variables


    // Input variables


    // References to other components or objects
    private Rigidbody2D activePlayerRB;
    private PlayableCharacter playerCharacterChoice;
    private SpriteRenderer characterSpriteRenderer;


    void Start()
    {
        // Instantiate references to other components or objects
        activePlayerRB = GetComponent<Rigidbody2D>();
        Spawner spawner = FindObjectOfType<Spawner>();
        playerCharacterChoice = spawner.playerChoice;
        characterSpriteRenderer = GetComponent<SpriteRenderer>();

        if (activePlayerRB == null || spawner == null)
        {
            Debug.Log($"{nameof(PlayerMovement)} > {nameof(Update)} > activePlayerRB=\"{activePlayerRB.IsUnityNull().ToString()}\" > spawner=\"{spawner.IsUnityNull().ToString()}\".");
        }
        // ...


    }


    void Update()
    {
        // Get horizontal input
        moveDirection = Input.GetAxisRaw("Horizontal");
        FlipSpriteBasedOnMovement(moveDirection);

        isRunning = Input.GetButton("Fire1");
        targetSpeed = !Mathf.Approximately(moveDirection, 0) ? (isRunning ? runSpeed : walkSpeed) : 0f;
        speedSmoothing = !Mathf.Approximately(moveDirection, 0) ? walkSpeed / accelerationTime : walkSpeed / (accelerationTime / 2);


        if (Input.GetButtonDown("Jump") && canJump)
        {
            isJumping = true;
        }



    }

    void FixedUpdate()
    {
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, speedSmoothing * Time.deltaTime);
        //Apply the movment
        activePlayerRB.velocity = new Vector2(moveDirection * currentSpeed, activePlayerRB.velocity.y);

        // ground check
        Collider2D[] colliders = Physics2D.OverlapBoxAll(groundCheck.position, new Vector2(groundCheckWidth, groundCheckHeight), 0f, jumpableLayer);
        canJump = colliders.Length > 0;

        if (isJumping && canJump)
        {
            activePlayerRB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumping = false;
        }
    }

    void FlipSpriteBasedOnMovement(float horizontalDirection)
    {
        if (horizontalDirection != 0)
        {
            characterSpriteRenderer.flipX = horizontalDirection < 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Jumpable"))
        {
            canJump = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & jumpableLayer) != 0)
        {
            canJump = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, new Vector3(groundCheckWidth, groundCheckHeight, 1f));
    }
}

