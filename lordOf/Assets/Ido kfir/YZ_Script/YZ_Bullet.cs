using UnityEngine;

public class YZ_Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player")) return;
        Destroy(gameObject);
    }

    // אם אתה משתמש ב-Trigger במקום Collision, תכבה את הפונקציה למעלה ותדליק את זאת:
    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player")) return;
    //     Destroy(gameObject);
    // }
}
