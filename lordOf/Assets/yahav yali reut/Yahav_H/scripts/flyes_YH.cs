using UnityEngine;
using System.Collections;

public class flyes_YH : MonoBehaviour
{//את קטע זה בגלל לחץ זמן ניסיתי עם AI יכלתי לעשות את זה בעצמי במהירות ובעילות יותר לדעתי
    [SerializeField] private float fallSpeed = 8f;
    [SerializeField] private float squashTime = 0.2f;
    [SerializeField] private Vector3 squashScale = new Vector3(1.4f, 0.2f, 1f);

    private Rigidbody2D rb;
    private Collider2D[] colliders;
    private SpriteRenderer[] renderers;
    private Vector3 startScale;
    private bool isDead;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        colliders = GetComponentsInChildren<Collider2D>();
        renderers = GetComponentsInChildren<SpriteRenderer>();
        startScale = transform.localScale;

        foreach (SpriteRenderer sr in renderers)
        {
            sr.enabled = true;
            Color c = sr.color;
            if (c.a <= 0f)
            {
                c.a = 1f;
                sr.color = c;
            }
        }

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;
            rb.useFullKinematicContacts = true;
        }
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        rb.MovePosition(rb.position + Vector2.down * fallSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            StartCoroutine(SquashAndDisappear());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            StartCoroutine(SquashAndDisappear());
        }
    }

    private IEnumerator SquashAndDisappear()
    {
        if (isDead)
        {
            yield break;
        }

        isDead = true;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        foreach (Collider2D currentCollider in colliders)
        {
            currentCollider.enabled = false;
        }

        float timer = 0f;
        Vector3 targetScale = new Vector3(
            startScale.x * squashScale.x,
            startScale.y * squashScale.y,
            startScale.z * squashScale.z);

        while (timer < squashTime)
        {
            float t = timer / squashTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            foreach (SpriteRenderer sr in renderers)
            {
                Color c = sr.color;
                c.a = 1f - t;
                sr.color = c;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
