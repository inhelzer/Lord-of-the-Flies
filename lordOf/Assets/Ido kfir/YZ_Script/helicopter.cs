using UnityEngine;

public class helicopter : MonoBehaviour
{
    [Header("Drop Settings")]
    public GameObject bulletPrefab;     // ה-Bullet Prefab
    public Transform dropPoint;         // מתחת למסוק
    public float dropRate = 1.5f;       // כל כמה שניות נופל טיל
    public float bulletSpeed = 5f;      // מהירות נפילה

    private float timer = 0f;

    void Update()
    {
        // סופר זמן
        timer += Time.deltaTime;

        if (timer >= dropRate)
        {
            DropBullet();
            timer = 0f; // איפוס הטיימר כדי לירות שוב
        }
    }

    void DropBullet()
    {
        if (bulletPrefab == null || dropPoint == null) return;

        // יוצרים את הטיל
        GameObject bullet = Instantiate(bulletPrefab, dropPoint.position, Quaternion.identity);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.down * bulletSpeed; // נפילה למטה
        }

        // מוודא שה-Bullet יש לו Tag נכון
        bullet.tag = "EnemyBullet";
    }
}