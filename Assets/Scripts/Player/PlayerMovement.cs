using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement variables
    float moveDirection = 0;
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float runSpeed = 20f;
    [SerializeField] private float accelerationTime = 1.5f;

    private float currentSpeed;
    private float targetSpeed;
    private float speedSmoothing;

    private bool isRunning;

    private bool leftButtonPressed = false;
    private bool rightButtonPressed = false;


    // Jump variables
    bool isJumping = false;
    private bool jumpButtonReleasedEarly = false;
    [SerializeField] private float jumpForce = 30f;
    [SerializeField] private float fallForceMultiplier = 10f;

    public LayerMask jumpableLayer;
    public Transform groundCheck;
    public float groundCheckWidth = .5f;
    public float groundCheckHeight = .5f;

    // Player state variables


    // Input variables
    private const int maxCollidersInTheFrame = 30;
    private Collider2D[] collidersInTheFrame = new Collider2D[maxCollidersInTheFrame];


    // References to other components or objects
    private Rigidbody2D activePlayerRB;
    private PlayableCharacter playerCharacterChoice;
    private SpriteRenderer characterSpriteRenderer;

    private OutOfBoundsChecker outOfBoundsChecker;

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, new Vector3(groundCheckWidth, groundCheckHeight, 1f));
        }
    }

    void Start()
    {
        // Instantiate references to other components or objects
        activePlayerRB = GetComponent<Rigidbody2D>();
        Spawner spawner = FindObjectOfType<Spawner>();
        playerCharacterChoice = spawner.playerChoice;
        characterSpriteRenderer = GetComponent<SpriteRenderer>();

        if (activePlayerRB == null || spawner == null)
        {
            Debug.LogError($"{nameof(PlayerMovement)} > {nameof(Update)} > activePlayerRB=\"{activePlayerRB.IsUnityNull().ToString()}\" > spawner=\"{spawner.IsUnityNull().ToString()}\".");
        }

        outOfBoundsChecker = GetComponent<OutOfBoundsChecker>();
        // ...
    }


    void Update()
    {
        // Get horizontal input
        SetPlayerMoveDirection();


        isRunning = Input.GetButton("Fire1");
        targetSpeed = !Mathf.Approximately(moveDirection, 0) ? (isRunning ? runSpeed : walkSpeed) : 0f;
        speedSmoothing = !Mathf.Approximately(moveDirection, 0) ? walkSpeed / accelerationTime : walkSpeed / (accelerationTime / 2);


        if (Input.GetButtonDown("Fire2") && CanPlayerJump())
        {
            isJumping = true;
            jumpButtonReleasedEarly = false;
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            jumpButtonReleasedEarly = true;
        }

    }

    void FixedUpdate()
    { 
        CheckOutOfBounds();
        
        MovePlayer(moveDirection);

        if (isJumping)
        {
            JumpThePlayer(1);
        }

        AcceleratePlayerFall();

       
    }

    void CheckOutOfBounds()
    {
        if (outOfBoundsChecker.IsOutOfBoundsLeft && moveDirection < 0)
        {
            moveDirection = 0;
                    }
        else if (outOfBoundsChecker.IsOutOfBoundsRight && moveDirection > 0)
        {
            moveDirection = 0;
        }
    }

    void SetPlayerMoveDirection()
    {
        bool leftButtonPressedThisFrame = Input.GetKeyDown(KeyCode.LeftArrow);
        bool rightButtonPressedThisFrame = Input.GetKeyDown(KeyCode.RightArrow);
        bool leftButtonReleasedThisFrame = Input.GetKeyUp(KeyCode.LeftArrow);
        bool rightButtonReleasedThisFrame = Input.GetKeyUp(KeyCode.RightArrow);

        if (leftButtonPressedThisFrame)
        {
            moveDirection = -1;
            leftButtonPressed = true;
            rightButtonPressed = false;
        }

        if (leftButtonReleasedThisFrame)
        {
            if (rightButtonPressed == false)
            {
                moveDirection = 0;
            }
            leftButtonPressed = false;
        }

        if (rightButtonPressedThisFrame)
        {
            moveDirection = 1;
            rightButtonPressed = true;
            leftButtonPressed = false;
        }

        if (rightButtonReleasedThisFrame)
        {
            if (leftButtonPressed == false)
            {
                moveDirection = 0;
            }
            rightButtonPressed = false;
        }

        //joystick control
        if (!leftButtonPressed && !rightButtonPressed)
        {
            moveDirection = Input.GetAxisRaw("Horizontal");
        }
    }

    void FlipSpriteBasedOnMovement(float horizontalDirection)
    {
        if (horizontalDirection != 0)
        {
            characterSpriteRenderer.flipX = horizontalDirection < 0;
        }
    }

    void MovePlayer(float direction)
    {
        FlipSpriteBasedOnMovement(direction);

              currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, speedSmoothing * Time.deltaTime);
        activePlayerRB.velocity = new Vector2(direction * currentSpeed, activePlayerRB.velocity.y);

    }

    void JumpThePlayer(float jumpForceMultiplier = 1f)
    {
        activePlayerRB.AddForce(Vector2.up * (jumpForce * jumpForceMultiplier), ForceMode2D.Impulse);
        isJumping = false;
    }

    void AcceleratePlayerFall()
    {
        if (jumpButtonReleasedEarly && activePlayerRB.velocity.y > 0)
        {
            activePlayerRB.velocity += Vector2.up * Physics2D.gravity.y * (fallForceMultiplier - 1) * Time.deltaTime;
        }
        if (activePlayerRB.velocity.y < 0)
        {
            activePlayerRB.velocity += Vector2.up * Physics2D.gravity.y * (fallForceMultiplier - 1) * Time.deltaTime;
        }
    }

    bool CanPlayerJump()
    {
        // ground check
        int colliderCount = Physics2D.OverlapBoxNonAlloc(groundCheck.position, new Vector2(groundCheckWidth, groundCheckHeight), 0f, collidersInTheFrame, jumpableLayer);

        if (colliderCount >= maxCollidersInTheFrame)
        {
            Debug.LogWarning($"{nameof(PlayerMovement)} > {nameof(CanPlayerJump)} > colliderCount=\"{colliderCount}\" > maxCollidersInTheFrame=\"{maxCollidersInTheFrame}\".");
        }

        return colliderCount > 0;
    }
}

