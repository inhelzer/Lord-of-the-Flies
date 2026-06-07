using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player_REB : MonoBehaviour, Controls.IGmaeControlsActions
{
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

    [Header("walkEffect")]
    [SerializeField] GameObject spark;
    [SerializeField] float delay;
    float sparkTime;
    public Color sparkColor;
    public float yShift = -2f;

    private void Awake()
    {
        controls = new Controls();
        controls.GmaeControls.SetCallbacks(this);
    }
    
    [SerializeField] private AudioClip munch;
    [SerializeField] private AudioClip eat;
    [SerializeField] private AudioClip chip;
    public GameObject REB_BasePlayer;

    AudioClip[] aud;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        controls.GmaeControls.Enable();
        anim = body.GetComponent<Animator>();
        ChangeAnimationState(idle);
        yLocalScale = transform.localScale.y;

        aud = new AudioClip[3] { munch, eat, chip };
    }



    private void Update()
    {
        if (moveInput > 0)
        {
            //transform.position = new Vector3(transform.position.x, transform.position.y, 1);  // Facing right
            body.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (moveInput < 0)
        {
            //transform.position = new Vector3(transform.position.x, transform.position.y, 1);
            body.GetComponent<SpriteRenderer>().flipX = true;
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
            
            if (isGrounded) 
            {
                Debug.Log("is grounded");
                if (jumpCount < maxJumps - 1)
                {
                    Debug.Log("maxjumps");
                    isJump = true;
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                    jumpCount++;
                    ChangeAnimationState(jump);
                }
                
            }
        }
    }

    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Check if a next scene actually exists to avoid errors
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels! Returning to Main Menu.");
            SceneManager.LoadScene(0); // Load first scene (usually menu)
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        
        if (collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("Rock"))
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
            Debug.Log("grounded");
            jumpCount = 0;  // Reset jump count when touching ground
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = false;
            Debug.Log("not grounded");
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

   
    int counter = 0;
    public void OnTriggerEnter2D(Collider2D other)
    {
        int foods = 10;

        if (other.gameObject.CompareTag("food"))
        {
            if (GetComponent<Rigidbody2D>().gravityScale > 0.3)
            {
                GetComponent<Rigidbody2D>().gravityScale = GetComponent<Rigidbody2D>().gravityScale - 0.30f;
                Debug.Log("Gravity Scale: " + GetComponent<Rigidbody2D>().gravityScale);
            }
            

            foods--;

            if (foods == 0)
            {
                LoadNextLevel();
            }
            counter = counter + 2;

            transform.localScale = new Vector3(transform.localScale.x + 0.25f,
                transform.localScale.y + 0.25f, transform.localScale.z);

            Destroy(other.gameObject);

            AudioClip clipToPlay = aud[Random.Range(0, aud.Length)];
            body.GetComponent<AudioSource>().PlayOneShot(clipToPlay);

            GetComponent<CinemachineCamera>().Lens.OrthographicSize = 50f * counter;

            //REB_BasePlayer.GetComponent<Rigidbody2D>().gravityScale = 5.7f + counter;

        }

        if(other.gameObject.tag == "SpaceBorder")
        {
            SceneManager.LoadScene("V1");
        }
    }

    void Controls.IGmaeControlsActions.OnBend(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }



    public Sprite eiffel;
    public Sprite spaceship;
    public Sprite plane;
    public Sprite building;
    public Sprite spiral;
    public Sprite sparkle;
    public Sprite building2;
    public Sprite bigben;
}
