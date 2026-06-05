using UnityEngine;

public class YZ_Enemy : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] Transform player;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;

    [Header("Move")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float patrolDistance = 2f;

    [Header("Detect & Shoot")]
    [SerializeField] float detectDistance = 8f;
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] float minShootDelay = 1f;
    [SerializeField] float maxShootDelay = 2f;

    float startX;
    int dir = -1;
    float nextShootTime;

    void Awake()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        startX = rb.position.x;
        nextShootTime = Time.time + Random.Range(minShootDelay, maxShootDelay);

        SetFacing(dir);
    }

    void Update()
    {
        if (GameManeger.Instance != null && GameManeger.Instance.isPaused)
            return;

        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= detectDistance)
        {
            int aimDir = (player.position.x >= transform.position.x) ? 1 : -1;
            SetFacing(aimDir);
            TryShoot(aimDir);
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        float left = startX - patrolDistance;
        float right = startX + patrolDistance;

        if (rb.position.x <= left) dir = 1;
        else if (rb.position.x >= right) dir = -1;

        SetFacing(dir);

        Vector2 nextPos = rb.position + Vector2.right * dir * moveSpeed * Time.deltaTime;
        rb.MovePosition(nextPos);
    }

    void TryShoot(int aimDir)
    {
        if (Time.time < nextShootTime) return;

        nextShootTime = Time.time + Random.Range(minShootDelay, maxShootDelay);

        GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D brb = b.GetComponent<Rigidbody2D>();
        if (brb != null) brb.linearVelocity = new Vector2(aimDir * bulletSpeed, 0f);
    }

    void SetFacing(int d)
    {
        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * (d == 1 ? -1f : 1f);
        transform.localScale = s;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("pBullet"))
        {
            if (GameManeger.Instance != null)
                GameManeger.Instance.AddScore();

            Destroy(gameObject);
        }
    }
}