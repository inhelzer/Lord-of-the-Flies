using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Raziel_BasePlayer : MonoBehaviour, Controls.IGmaeControlsActions
{
    [Header("Raziel")]
    Vector3 respawnPosition; // Default in start

    [SerializeField] float respawn_ground;

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
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        controls.GmaeControls.Enable();
        anim = body.GetComponent<Animator>();
        ChangeAnimationState(idle);
        yLocalScale = transform.localScale.y;

        // Raziel added
        respawnPosition = transform.localPosition;
    }



    private void Update()
    {
        // Raziel added
        if (Input.GetKeyDown(KeyCode.R))
        {
            SendToCheckpoint();
        }

        if (moveInput > 0)
            transform.localScale = new Vector3(1, yLocalScale, 1);  // Facing right
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, yLocalScale, 1);  // Facing left

    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
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
        if (context.performed)
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
        if (context.performed)
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
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        /*
        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = false;
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Raziel - Added quicksand
        if (collision.gameObject.CompareTag("quicksand"))
        {
            //Debug.Log(0);
            EnterQucicksand();
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
    }

    public void OnShot(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
    // Raziel
    public void EnterQucicksand()
    {
        moveSpeed = 1f;
        jumpForce = 1f;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.005f;
        isGrounded = true;
        jumpCount = -9999;
        //isJump = false;
    }

    // Raziel
    public void ExitQuicksand()
    {
        moveSpeed = 8f;
        jumpForce = 20f;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 4f;
        jumpCount = 0;

    }

    // Raziel
    public void SetRespawnPosition(Vector3 respawnPosition)
    {
        this.respawnPosition = respawnPosition;
    }

    // Raziel
    public void SendToCheckpoint()
    {
        transform.position = respawnPosition;
    }

    public void OnBend(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }
}
