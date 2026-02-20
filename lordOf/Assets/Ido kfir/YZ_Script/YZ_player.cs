using UnityEngine;
using UnityEngine.InputSystem;

public class YZ_Player : MonoBehaviour, Controls.IGmaeControlsActions
{
    Controls controls;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    private float moveInput;
    private Rigidbody2D rb;
    private float yLocalScale;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 16f;
    [SerializeField] private int maxJumps = 3;
    private int jumpCount;
    private bool isJump;

    [Header("Jump Reset")]
    [SerializeField] private string resetJumpTag = "toJump";

    [Header("Animation")]
    [SerializeField] private GameObject body;
    [SerializeField] private string idle;
    [SerializeField] private string run;
    [SerializeField] private string jump;
    private Animator anim;
    private string currentAnimation;

    [Header("Shooting")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 14f;
    [SerializeField] private float fireCooldown = 0.15f;
    private float lastFireTime;

    private void Awake()
    {
        controls = new Controls();
        controls.GmaeControls.SetCallbacks(this);

        rb = GetComponent<Rigidbody2D>();

        yLocalScale = transform.localScale.y;

        if (body != null)
            anim = body.GetComponent<Animator>();

        if (firePoint == null)
            firePoint = transform.Find("FirePoint");

        if (bulletPrefab == null)
            bulletPrefab = Resources.Load<GameObject>("Bullet");
    }

    private void OnEnable()
    {
        controls.GmaeControls.Enable();
    }

    private void OnDisable()
    {
        controls.GmaeControls.Disable();
    }

    private void Update()
    {
        if (moveInput > 0)
            transform.localScale = new Vector3(1, yLocalScale, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, yLocalScale, 1);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    public void OnMoveHorizontal(InputAction.CallbackContext context)
    {
        if (context.canceled)
            moveInput = 0f;
        else
            moveInput = context.ReadValue<float>();

        UpdateMoveAnim();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (jumpCount >= maxJumps) return;

        isJump = true;
        jumpCount++;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        ChangeAnimationState(jump);
    }

    public void OnShot(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (Time.time < lastFireTime + fireCooldown) return;
        if (bulletPrefab == null) return;
        if (firePoint == null) return;

        lastFireTime = Time.time;

        float dir = 1f;
        if (transform.localScale.x < 0)
            dir = -1f;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D brb = bullet.GetComponent<Rigidbody2D>();

        if (brb != null)
            brb.linearVelocity = new Vector2(dir * bulletSpeed, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == null) return;
        if (!collision.collider.CompareTag(resetJumpTag)) return;

        ResetJumpsAndAnim();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;
        if (!other.CompareTag(resetJumpTag)) return;

        ResetJumpsAndAnim();
    }

    private void ResetJumpsAndAnim()
    {
        jumpCount = 0;
        isJump = false;

        UpdateMoveAnim();
    }

    private void UpdateMoveAnim()
    {
        if (isJump) return;

        if (moveInput != 0)
            ChangeAnimationState(run);
        else
            ChangeAnimationState(idle);
    }

    private void ChangeAnimationState(string newAnimation)
    {
        if (anim == null) return;
        if (string.IsNullOrEmpty(newAnimation)) return;
        if (currentAnimation == newAnimation) return;

        anim.Play(newAnimation);
        currentAnimation = newAnimation;
    }
}