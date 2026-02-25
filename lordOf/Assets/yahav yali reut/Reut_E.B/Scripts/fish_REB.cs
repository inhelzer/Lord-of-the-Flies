using UnityEngine;

public class fish_REB : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    float interval = 2.3f;  
    float timer = 0f;

    float moveDuration = 0.2f;  
    float moveTimer = 0f;

    bool movingLeft = false;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;
            movingLeft = true;
            moveTimer = moveDuration;
        }
    }

    private Rigidbody2D rb;

    void FixedUpdate()
    {
        if (movingLeft)
        {
            rb.linearVelocity = new Vector2(-1f, rb.linearVelocity.y);

            moveTimer -= Time.fixedDeltaTime;

            if (moveTimer <= 0f)
            {
                movingLeft = false;
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            }
        }
    }

}
