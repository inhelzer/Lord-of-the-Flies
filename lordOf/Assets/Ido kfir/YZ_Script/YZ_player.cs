using UnityEngine;
using UnityEngine.InputSystem;

public class YZ_Player : MonoBehaviour, Controls.IGmaeControlsActions
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

        // Auto-find FirePoint by name
        if (firePoint == null)
            firePoint = transform.Find("FirePoint");

        // Auto-load bullet prefab from Resources/Bullet.prefab
        if (bulletPrefab == null)
            bulletPrefab = Resources.Load<GameObject>("Bullet");
    }



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        controls.GmaeControls.Enable();

        if (body != null)
        {
            anim = body.GetComponent<Animator>();
            ChangeAnimationState(idle);
        }

        yLocalScale = transform.localScale.y;
    }

    private void Update()
    {
        if (moveInput > 0)
            transform.localScale = new Vector3(1, yLocalScale, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, yLocalScale, 1);

        if (spark != null)
        {
            if (isGrounded && moveInput != 0 &&
                (Time.timeSinceLevelLoad - sparkTime >= Random.Range(delay * 0.6f, delay * 1.3f)))
            {
                sparkTime = Time.timeSinceLevelLoad;
                CreateSpark();
            }
        }
    }

    private void FixedUpdate()
    {
        // �� ���� linearVelocity ���� ���� ���� ������ - ����.
        // �� ������� �����:
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    public void OnMoveHorizontal(InputAction.CallbackContext context)
    {
        // ���� ����, ����� ��������
        if (context.canceled)
        {
            moveInput = 0;
            if (!isJump) ChangeAnimationState(idle);
            return;
        }

        moveInput = context.ReadValue<float>();
        if (!isJump)
        {
            if (moveInput != 0) ChangeAnimationState(run);
            else ChangeAnimationState(idle);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // ������� ����� ��� ��� ����� ���: ����� �� maxJumps �� ������
        if (jumpCount >= maxJumps) return;

        isJump = true;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpCount++;
        ChangeAnimationState(jump);
    }

    public void OnShot(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (Time.time < lastFireTime + fireCooldown) return;

        lastFireTime = Time.time;

        if (bulletPrefab == null || firePoint == null) return;

        float dir = transform.localScale.x >= 0 ? 1f : -1f;

        GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D brb = b.GetComponent<Rigidbody2D>();
        if (brb != null)
            brb.linearVelocity = new Vector2(dir * bulletSpeed, 0f);
    }


    public void CreateSpark()
    {
        GameObject currentSpark = Instantiate(
            spark,
            transform.position + new Vector3(moveInput * -0.5f, yShift, 0),
            Quaternion.identity
        );

        SpriteRenderer sr = currentSpark.GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = sparkColor;

        Rigidbody2D srb = currentSpark.GetComponent<Rigidbody2D>();
        if (srb != null)
        {
            srb.linearVelocity = new Vector2(moveInput * -1, Random.Range(0.7f, 4f));
        }

        Destroy(currentSpark, 1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("Rock"))
        {
            if (isJump)
            {
                isJump = false;
                if (moveInput != 0) ChangeAnimationState(run);
                else ChangeAnimationState(idle);
            }

            isGrounded = true;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("Rock"))
        {
            isGrounded = false;
        }
    }

    public void ChangeAnimationState(string newAnimation)
    {
        if (anim == null) return;
        if (currentAnimation == newAnimation) return;

        anim.Play(newAnimation);
        currentAnimation = newAnimation;
    }

    private void OnDestroy()
    {
        if (controls != null) controls.GmaeControls.Disable();
    }
}
