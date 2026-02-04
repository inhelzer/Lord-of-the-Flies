using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMove_YH : MonoBehaviour, Controls.IGmaeControlsActions
{
    Controls controls;

    [Header("Movement")]
    public float moveSpeed = 8f;
    public float moveInput;
    private Rigidbody2D rb;

    //?????
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip eat;
    [SerializeField] private AudioClip loseClip;

    float yLocalScale;
    float xLocalScale;
    bool lastdirection;//????? ?????? ???? ????? ?????
    float scaletime;
    public float PerorPower = 0.35f;
    public float jumpPower = 3;

    [SerializeField] private GameObject effectPrefab;//effect
    bool lostTriggered = false;
    float losttimer;

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
    public string non;
    bool isJump = false;

    [Header("walkEffect")]
    [SerializeField] GameObject spark;
    [SerializeField] float delay;
    float sparkTime;
    public Color sparkColor;
    public float yShift = -2f;
    private Vector3 baseScale;

    private void Awake()
    {
        controls = new Controls();
        controls.GmaeControls.SetCallbacks(this);
    }

    private void Start()
    {
        baseScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        controls.GmaeControls.Enable();
        anim = body.GetComponent<Animator>();
        ChangeAnimationState(idle);
        yLocalScale = transform.localScale.y;
        xLocalScale = transform.localScale.x;
    }



    private void Update()
    {
        if (scaletime + 0.1f < Time.timeSinceLevelLoad)
        {
            xLocalScale = xLocalScale * 0.99f;
            yLocalScale = yLocalScale * 0.99f;
            scaletime = Time.timeSinceLevelLoad;
        }
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(xLocalScale, yLocalScale, 1);  // Facing right
            lastdirection = true;
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-xLocalScale, yLocalScale, 1);  // Facing left
            lastdirection = false;
        }
        else if (moveInput == 0)
        {
            if (lastdirection == true)
            {
                transform.localScale = new Vector3(xLocalScale, yLocalScale, 1);  // Facing right
            }
            else
            {
                transform.localScale = new Vector3(-xLocalScale, yLocalScale, 1);  // Facing left
            }
        }

        if (!lostTriggered && yLocalScale < 0.1f)
        {
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
            losttimer = Time.timeSinceLevelLoad;
            lostTriggered = true;
            audioSource.PlayOneShot(loseClip);
            ChangeAnimationState(non);
        }

        if (lostTriggered && Time.timeSinceLevelLoad > losttimer + 1f)
        {
            SceneManager.LoadScene("Yahav_H");
        }


        if (spark != null)
        {
            if ((isGrounded) && (moveInput != 0) &&
                (Time.timeSinceLevelLoad - sparkTime >= Random.Range(delay * 0.6f, delay * 1.3f)))
            {
                sparkTime = Time.timeSinceLevelLoad;
                CreateSpark();
            }
        }

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

    public void CreateSpark()
    {
        GameObject currentSpark =
        Instantiate(spark, transform.position + new Vector3(moveInput * -0.5f, yShift, 0), Quaternion.identity) as GameObject;
        currentSpark.GetComponent<SpriteRenderer>().color = sparkColor;
        currentSpark.GetComponent<Rigidbody2D>().linearVelocity =
            new Vector2(moveInput * -1, Random.Range(0.7f, 4f));
        Destroy(currentSpark, 1f);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Peror")
        {
            xLocalScale = xLocalScale + PerorPower;
            yLocalScale = yLocalScale + PerorPower;
            Destroy(other.gameObject);
            audioSource.PlayOneShot(eat);

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

        if (collision.gameObject.CompareTag("Jump"))
        {
            Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
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
}
