using UnityEngine;

public class Raziel_Enemy1 : MonoBehaviour
{
    [SerializeField] float speed;
    Vector2 direction;
    Rigidbody2D rb2d;
    float yRotation;
    bool canMove;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        direction = new Vector2 (1, 0);
        yRotation = 0;
        canMove = false;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground") && !canMove)
        {
            canMove = true;
        }
        
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("ground"))
        {
            SwitchDirection();
        }
        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            SwitchDirection();
        }
    }

    void Move()
    {
        rb2d.MovePosition(rb2d.position + direction * speed * Time.fixedDeltaTime);
    }
    
    void SwitchDirection()
    {
        direction = -direction;
            yRotation += 180f;
            if (yRotation == 360f)
                yRotation = 0;
            transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    
}
