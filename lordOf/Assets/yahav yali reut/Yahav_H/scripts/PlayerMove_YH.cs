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
    [SerializeField] private float butterDuration = 5f;
    [SerializeField] private float butterSpeedMultiplier = 1.5f;
    [SerializeField] private float butterAccelerationControl = 10f;
    [SerializeField] private float butterDecelerationControl = 1f;
    [Header("Slope Slip")]
    [SerializeField] private float minSlopeAngleForBoost = 20f;
    [SerializeField] private float maxSlopeAngle = 60f;
    [SerializeField] private float slopeGravityMultiplier = 3.5f;
    [SerializeField] private float uphillBrakeStrength = 0.35f;
    [SerializeField] private float maxSlopeSpeedMultiplier = 3.5f;
    private bool isSlipping;
    private float slipEndTime;
    private float normalMoveSpeed;
    private Vector2 groundNormal = Vector2.up;

    //?????
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip eat;
    [SerializeField] private AudioClip jily;
    [SerializeField] private AudioClip loseClip;

    float yLocalScale;
    float xLocalScale;
    bool lastdirection;//????? ?????? ???? ????? ?????
    float scaletime;
    public float PerorPower = 0.35f;
    public float jumpPower = 3;

    public ParticleSystem dust;
    [SerializeField] private GameObject effectPrefab;//effect
    [SerializeField] private GameObject blood;//effect
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
        normalMoveSpeed = moveSpeed;
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
            moveInput = 0f;
            rb.linearVelocity = Vector2.zero;
            HideBody();
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
                (Time.timeSinceLevelLoad - sparkTime >= Random.Range(delay * 0.1f, delay * 0.2f)))
            {
                sparkTime = Time.timeSinceLevelLoad;
                CreateSpark();
            }
        }

        if (isSlipping && Time.time >= slipEndTime)
        {
            isSlipping = false;
            moveSpeed = normalMoveSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (lostTriggered)
        {
            StopDust();
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (isSlipping)
        {
            float slopeAngle = Vector2.Angle(groundNormal, Vector2.up);
            if (slopeAngle < minSlopeAngleForBoost)
            {
                float targetSpeed = moveInput * moveSpeed;
                float control = Mathf.Abs(moveInput) > 0.01f ? butterAccelerationControl : butterDecelerationControl;
                float newVelocityX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, control * moveSpeed * Time.fixedDeltaTime);

                if (Mathf.Abs(newVelocityX) > 0.1f)
                {
                    CreateDust();
                }
                else
                {
                    StopDust();
                }

                rb.linearVelocity = new Vector2(newVelocityX, rb.linearVelocity.y);
                return;
            }

            if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
            {
                //Debug.Log("DUST PLAY");
                CreateDust();
            }
            else
            {
                StopDust();
            }
            float slopeT = Mathf.InverseLerp(minSlopeAngleForBoost, maxSlopeAngle, slopeAngle);

            Vector2 downhill = new Vector2(groundNormal.y, -groundNormal.x);
            if (Vector2.Dot(downhill, Vector2.down) < 0f)
            {
                downhill *= -1f;
            }

            float slopeGravityForce = rb.gravityScale * Physics2D.gravity.magnitude * slopeGravityMultiplier * slopeT;
            rb.AddForce(downhill.normalized * slopeGravityForce, ForceMode2D.Force);

            float dynamicMaxSpeed = moveSpeed * Mathf.Lerp(1f, maxSlopeSpeedMultiplier, slopeT);
            float downhillVelocity = Vector2.Dot(rb.linearVelocity, downhill.normalized);
            float downhillInput = Vector2.Dot(new Vector2(moveInput, 0f), downhill.normalized);

            if (downhillInput < 0f)
            {
                downhillVelocity += downhillInput * moveSpeed * uphillBrakeStrength * Time.fixedDeltaTime;
                downhillVelocity = Mathf.Max(0f, downhillVelocity);
            }
            else if (downhillInput > 0f)
            {
                downhillVelocity += downhillInput * moveSpeed * butterAccelerationControl * Time.fixedDeltaTime;
            }

            downhillVelocity = Mathf.Clamp(downhillVelocity, 0f, dynamicMaxSpeed);
            Vector2 slopeVelocity = downhill.normalized * downhillVelocity;
            rb.linearVelocity = new Vector2(slopeVelocity.x, rb.linearVelocity.y);
            return;
        }

        StopDust();
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
        if (!lostTriggered && (other.gameObject.CompareTag("enemy") || IsFireObject(other.gameObject)))
        {
            TriggerDeath();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("Butter"))
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

        if (collision.gameObject.CompareTag("Butter"))
        {
            isSlipping = true;
            slipEndTime = Time.time + butterDuration;
            moveSpeed = normalMoveSpeed * butterSpeedMultiplier;
        }
        if (collision.gameObject.CompareTag("Jump"))
        {
            Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, 25);
        }
        if (collision.gameObject.CompareTag("Jily"))
        {
            audioSource.PlayOneShot(jily);
            Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, 35);
        }
        if (!lostTriggered && (collision.gameObject.CompareTag("enemy") || IsFireObject(collision.gameObject)))
        {
            TriggerDeath();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("Butter"))
        {
            isGrounded = false;

        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("Butter")) && collision.contactCount > 0)
        {
            groundNormal = collision.GetContact(0).normal;
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

    public void OnBend(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    void CreateDust()
    {
        if (dust != null && !dust.isPlaying)
        {
            dust.Play();
        }
    }

    void StopDust()
    {
        if (dust != null && dust.isPlaying)
        {
            dust.Stop();
        }
    }

    void HideBody()
    {
        if (body == null)
        {
            return;
        }

        SpriteRenderer[] renderers = body.GetComponentsInChildren<SpriteRenderer>(true);
        foreach (SpriteRenderer sr in renderers)
        {
            Color c = sr.color;
            c.a = 0f;
            sr.color = c;
        }
    }

    void TriggerDeath()
    {
        if (blood != null)
        {
            Instantiate(blood, transform.position, Quaternion.identity);
        }

        losttimer = Time.timeSinceLevelLoad;
        lostTriggered = true;
        moveInput = 0f;
        rb.linearVelocity = Vector2.zero;
        HideBody();
        audioSource.PlayOneShot(loseClip);
        ChangeAnimationState(non);
    }

    bool IsFireObject(GameObject obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj.CompareTag("Fire"))
        {
            return true;
        }

        Transform current = obj.transform.parent;
        while (current != null)
        {
            if (current.CompareTag("Fire"))
            {
                return true;
            }

            current = current.parent;
        }

        return obj.GetComponentInParent<Fire_YH>() != null;
    }
}



