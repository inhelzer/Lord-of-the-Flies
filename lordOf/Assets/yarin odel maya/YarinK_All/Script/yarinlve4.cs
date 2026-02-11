using UnityEngine;

public class yarinlve4 : MonoBehaviour
{
    private float timer = 0f;
    private bool playerOnPlatform = false;

    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (playerOnPlatform)
        {
            timer += Time.deltaTime;

            if (timer >= 1f)
            {
                Disappear();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = false;
            timer = 0f; // מאפס אם הוא ירד לפני 3 שניות
        }
    }

    void Disappear()
    {
        boxCollider.enabled = false;
        spriteRenderer.enabled = false;
    }

}
