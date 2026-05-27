using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyMissile : MonoBehaviour
{
    public float damage = 15f;          // נזק לשחקן
    public float fallSpeed = 5f;        // מהירות נפילה

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // הפעלה של נפילה קבועה
        rb.linearVelocity = Vector2.down * fallSpeed;

        // אם רוצים Gravity אמיתית, אפשר לבטל את השורה למעלה
        rb.gravityScale = 0f;

        // חשוב: זה עובד עם Trigger
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // אם זה השחקן
        YZ_Player player = other.GetComponent<YZ_Player>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }

        // אם פוגע בכל דבר אחר (רצפה, מכשול)
        Destroy(gameObject);
    }
}