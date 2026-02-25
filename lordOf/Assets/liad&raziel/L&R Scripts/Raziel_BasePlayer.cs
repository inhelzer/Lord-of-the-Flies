using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Raziel_BasePlayer : MonoBehaviour, Controls.IGmaeControlsActions, Controls.IRazielControlsActions
{
    [Header("Liad")]
    [SerializeField] int health;
    int full_health;
    float delay;

    [Header("Raziel")]
    Vector3 respawnPosition; // Default in start
    [SerializeField] float respawn_ground;
    float windSpeed;
    [SerializeField] float enteredWindSpeed;
    bool isDashing;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    float dashDirection;
    int dashesLeft;

    bool playerControlling;
    Controls controls;

    [Header("Movement")]
    public float moveSpeed = 8f;
    public float moveInput;
    private Rigidbody2D rb;

    float yLocalScale;

    [Header("Jumping")]
    public float jumpForce = 16f;
    public int maxJumps = 2;
    private int jumpCount;
    public bool isGrounded;

    [Header("anim")]
    public GameObject body;
    Animator anim;
    string currentAnimation;
    public string idle;
    public string run;
    public string jump;
    bool isJump = false;



    private void Awake()
    {
        controls = new Controls();
        controls.GmaeControls.SetCallbacks(this);
        controls.RazielControls.SetCallbacks(this);

        playerControlling = true;
    }

    private void Start()
    {
        // Liad
        full_health = health; 
        delay = Time.timeSinceLevelLoad-1f;

        rb = GetComponent<Rigidbody2D>();
        controls.GmaeControls.Enable();
        controls.RazielControls.Enable();
        anim = body.GetComponent<Animator>();
        ChangeAnimationState(idle);
        yLocalScale = transform.localScale.y;

        // Raziel added
        respawnPosition = transform.localPosition;
        isDashing = false;
        dashesLeft = 1;
    }
    private void Update()
    {
        // Liad
        if (body.GetComponent<SpriteRenderer>().color == Color.red && delay < Time.timeSinceLevelLoad)
        {
            body.GetComponent<SpriteRenderer>().color = Color.white;
        }

        // Raziel added
        if (Input.GetKeyDown(KeyCode.R))
        {
            SendToPosition();
        }


        if (moveInput > 0)
            transform.localScale = new Vector3(1, yLocalScale, 1);  // Facing right
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, yLocalScale, 1);  // Facing left

    }

    private void FixedUpdate()
    {
        if (!isDashing)
            rb.linearVelocity = new Vector2(moveInput * moveSpeed + windSpeed, rb.linearVelocity.y);
    }

    public void OnMoveHorizontal(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            moveInput = 0;
            if (!isJump)
            {
                ChangeAnimationState(idle);
            }

        }
        if (context.performed && playerControlling)
        {
            moveInput = context.ReadValue<float>();
            if (!isJump)
            {
                ChangeAnimationState(run);
            }
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && playerControlling)
        {
            if (isGrounded && jumpCount < maxJumps - 1)
            {
                isJump = true;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpCount++;
                ChangeAnimationState(jump);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
    private void OnCollisionExit2D(Collision2D collision)
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Raziel - Added quicksand
        /*
        if (collision.gameObject.CompareTag("quicksand"))
        {
            //Debug.Log(0);
            EnterQucicksand();
        }

        */
        // Liad
        if (collision.gameObject.CompareTag("Bad"))
        {
            if (delay < Time.timeSinceLevelLoad)
            {
                health = health - 1;
                if (health <= 0)
                {
                    SendToPosition();
                    health = full_health;
                }
                else
                {
                    body.GetComponent<SpriteRenderer>().color = Color.red;
                    delay = Time.timeSinceLevelLoad + 1f;
                }
            }
        }
        if (collision.gameObject.CompareTag("ground"))
        {
            if (isJump)
            {
                if (moveInput != 0)
                {
                    isJump = false;
                    ChangeAnimationState(run);
                }
                else
                {
                    isJump = false;
                    ChangeAnimationState(idle);
                }
            }
            isGrounded = true;
            jumpCount = 0;  // Reset jump count when touching ground
            dashesLeft = 1;
        }

        // Wind checks
        if (collision.gameObject.CompareTag("Wind"))
        {
            windSpeed = enteredWindSpeed;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Raziel - Added quicksand
        if (collision.gameObject.CompareTag("quicksand"))
        {
            //Debug.Log(1);
            ExitQuicksand();
        }

        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = false;
        }
        // Wind Checks
        if (collision.gameObject.CompareTag("Wind"))
        {
            windSpeed = 0;
        }
    }

    public void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;
        anim.Play(newAnimation);
        currentAnimation = newAnimation;
    }

    private void OnDestroy()
    {
        controls.GmaeControls.Disable();
        controls.RazielControls.Disable();
    }

    public void OnShot(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }
    // Raziel -
    public void EnterQucicksand()
    {
        moveSpeed = 1f;
        jumpForce = 1f;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.005f;
        isGrounded = true;
        jumpCount = -9999;
        //isJump = false;
    }

    public void ExitQuicksand()
    {
        moveSpeed = 8f;
        jumpForce = 20f;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 4f;
        jumpCount = 0;

    }

    public void SetRespawnPosition(Vector3 respawnPosition)
    {
        this.respawnPosition = respawnPosition;
    }

    public void SendToPosition()
    {
        transform.position = respawnPosition;
    }

    public void SendToPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (playerControlling)
        {
            if (context.performed && !isDashing && dashesLeft >= 1)
            {
                dashDirection = transform.localScale.x;
                StartCoroutine(Dash());
            }
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;

        dashesLeft--;
        float elapsed = 0f;

        while (elapsed < dashTime)
        {
            rb.linearVelocity = new Vector2(dashDirection * dashSpeed, rb.linearVelocity.y);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (isGrounded)
            dashesLeft = 1;

        isDashing = false;
    }

    public void GiveControls(bool controls)
    {
        playerControlling = controls;
    }

    public void CheckPlayerConstraints()
    {
        if (playerControlling)
            rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        else
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    }
}
