using UnityEngine;
using UnityEngine.SceneManagement;

public class restart_yarink : MonoBehaviour
{
    [Header("Prefabs & Settings")]
    public GameObject trianglePrefab; // פריפאב של משולש
    public GameObject squarePrefab;   // פריפאב של ריבוע
    public int shapesCount = 20;
    public float spawnRadius = 0.5f;
    public float fallForce = 5f;
    public float sideForce = 2f;
    public float restartDelay = 1f;

    private bool isDead = false;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    private Animator animator;
    private Rigidbody2D rb;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Spike"))
            Die();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Spike"))
            Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // השבתת הדמות
        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (animator != null) animator.enabled = false;
        if (col != null) col.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        // יצירת חלקים שנופלים
        for (int i = 0; i < shapesCount; i++)
        {
            // רנדום בין משולש לריבוע
            GameObject prefabToSpawn = (Random.value > 0.5f) ? trianglePrefab : squarePrefab;

            Vector2 spawnPos = (Vector2)transform.position +
                new Vector2(Random.Range(-spawnRadius, spawnRadius),
                            Random.Range(-spawnRadius, spawnRadius));

            GameObject shape = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

            Rigidbody2D shapeRb = shape.GetComponent<Rigidbody2D>();
            if (shapeRb != null)
            {
                shapeRb.gravityScale = 1f; // חשוב – גורם לנפילה אמיתית!
                shapeRb.freezeRotation = false;

                // כוח נפילה כלפי מטה עם טיפה פיזור לצדדים
                float randomSide = Random.Range(-sideForce, sideForce);
                float randomFall = Random.Range(-fallForce, -2f);
                shapeRb.linearVelocity = new Vector2(randomSide, randomFall);
            }

            Destroy(shape, 2f);
        }

        Invoke(nameof(RestartLevel), restartDelay);
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

