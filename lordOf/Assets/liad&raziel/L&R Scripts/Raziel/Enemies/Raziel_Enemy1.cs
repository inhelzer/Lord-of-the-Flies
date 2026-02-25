using UnityEngine;

public class Raziel_Enemy1 : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] protected float direction;
    Rigidbody2D rb2d;
    protected float yRotation;
    bool canMove;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        direction = 1;
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("ground") &&
            !collision.gameObject.CompareTag("LaserWarning") && !collision.gameObject.CompareTag("Laser") &&
            !collision.gameObject.CompareTag("Wind"))
        {
            SwitchDirection();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            SwitchDirection();
        }
    }

    void Move()
    {
        rb2d.linearVelocityX = direction * speed;
    }
    
    protected virtual void SwitchDirection()
    {
        direction = -1 * direction;
            yRotation += 180f;
            if (yRotation == 360f)
                yRotation = 0;
            transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    
}
