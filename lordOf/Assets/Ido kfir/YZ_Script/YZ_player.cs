using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class YZ_Player : MonoBehaviour, Controls.IGmaeControlsActions
{
    Controls controls;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    private float moveInput;
    private Rigidbody2D rb;

    private float facingX = 1f; // 1 ימינה, -1 שמאלה

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

    [Header("Weapon Pickup")]
    [SerializeField] private GameObject playerWeapon;
    [SerializeField] private GameObject weaponPickup;
    private bool hasWeapon;

    [Header("Bend (simple + not OP)")]
    [SerializeField, Range(0.3f, 1f)] private float bendYScale = 0.65f; // 0.6-0.7
    [SerializeField] private float bendSmooth = 12f;                   // כמה חלק
    [SerializeField] private float maxBendTime = 1.5f;                 // אחרי כמה זמן משתחרר לבד
    [SerializeField] private float bendCooldown = 1.0f;                // זמן המתנה בין Bend

    private bool bendHeld;
    private bool isBending;
    private float bendTimer;
    private float nextBendAllowedTime;

    // Bend על הגוף בלבד (ויזואלי)
    private Transform bodyT;
    private Vector3 bodyNormalScale;

    // מד חיים
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float damage = 5f;
    [SerializeField] private HealthBar healthBarUI;

    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthBarUI != null)
            healthBarUI.SetHealthInstant(currentHealth, maxHealth);
    }

    private void Awake()
    {
        controls = new Controls();
        controls.GmaeControls.SetCallbacks(this);

        rb = GetComponent<Rigidbody2D>();

        facingX = Mathf.Sign(transform.localScale.x);

        bodyT = (body != null) ? body.transform : transform;   // אם אין body, ניפול על transform (פחות מומלץ)
        bodyNormalScale = bodyT.localScale;

        if (body != null) anim = body.GetComponent<Animator>();
        if (firePoint == null) firePoint = transform.Find("FirePoint");
        if (bulletPrefab == null) bulletPrefab = Resources.Load<GameObject>("Bullet");

        hasWeapon = (playerWeapon != null && playerWeapon.activeSelf);
    }

    private void OnEnable()
    {
        if (controls != null)
            controls.GmaeControls.Enable();
    }

    private void OnDisable()
    {
        if (controls != null)
            controls.GmaeControls.Disable();
    }

    private void Update()
    {
        // כיוון לפי תנועה (פליפ על השחקן עצמו)
        if (moveInput > 0) facingX = 1f;
        else if (moveInput < 0) facingX = -1f;

        transform.localScale = new Vector3(facingX, transform.localScale.y, 1f);

        HandleBendVisualOnly();

        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    public void OnMoveHorizontal(InputAction.CallbackContext context)
    {
        moveInput = context.canceled ? 0f : context.ReadValue<float>();
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
        if (!hasWeapon) return;

        if (Time.time < lastFireTime + fireCooldown) return;
        if (bulletPrefab == null || firePoint == null) return;

        lastFireTime = Time.time;

        float dir = (facingX < 0) ? -1f : 1f;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D brb = bullet.GetComponent<Rigidbody2D>();
        if (brb != null) brb.linearVelocity = new Vector2(dir * bulletSpeed, 0f);
    }

    // ---- BEND ----
    public void OnBend(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            bendHeld = true;

            // מתחיל רק אם קולדאון עבר
            if (Time.time >= nextBendAllowedTime)
            {
                isBending = true;
                bendTimer = 0f;
            }
        }

        if (context.canceled)
        {
            bendHeld = false;
            StopBend();
        }
    }

    private void StopBend()
    {
        if (!isBending) return;

        isBending = false;
        nextBendAllowedTime = Time.time + bendCooldown;
    }

    private void HandleBendVisualOnly()
    {
        // אם כפוף יותר מדי זמן - משתחרר לבד
        if (isBending)
        {
            bendTimer += Time.deltaTime;
            if (bendTimer >= maxBendTime)
                StopBend();
        }

        // יעד הסקייל על הגוף בלבד
        float targetY = isBending ? bendYScale : bodyNormalScale.y;

        float newY = (bendSmooth <= 0f)
            ? targetY
            : Mathf.Lerp(bodyT.localScale.y, targetY, Time.deltaTime * bendSmooth);

        bodyT.localScale = new Vector3(bodyNormalScale.x, newY, bodyNormalScale.z);
    }

    // ---- WEAPON ----
    public void EnableWeapon()
    {
        hasWeapon = true;
        if (playerWeapon != null) playerWeapon.SetActive(true);
    }

    // ---- COLLISIONS ----
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == null) return;
        if (!collision.collider.CompareTag(resetJumpTag)) return;
        ResetJumpsAndAnim();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;

        // איסוף נשק
        if (other.gameObject == weaponPickup)
        {
            EnableWeapon();
            Destroy(weaponPickup);
            return;
        }

        // איפוס קפיצות
        if (other.CompareTag(resetJumpTag))
        {
            ResetJumpsAndAnim();
            return;
        }

        // פגיעה מכדור אויב
        if (other.CompareTag("EnemyBullet"))
        {
            TakeDamage(damage);
            Destroy(other.gameObject);
        }
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
        ChangeAnimationState(moveInput != 0 ? run : idle);
    }

    private void ChangeAnimationState(string newAnimation)
    {
        if (anim == null) return;
        if (string.IsNullOrEmpty(newAnimation)) return;
        if (currentAnimation == newAnimation) return;

        anim.Play(newAnimation);
        currentAnimation = newAnimation;
    }

    // הורדת חיים
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (healthBarUI != null)
            healthBarUI.SetHealth(currentHealth, maxHealth);

        if (currentHealth <= 0f)
            Die();
    }

    // הוספת חיים
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (healthBarUI != null)
            healthBarUI.SetHealth(currentHealth, maxHealth);
    }

    // שחקן מת
    private void Die()
    {
        //Debug.Log("Player died");
        Destroy(gameObject);
    }

}

