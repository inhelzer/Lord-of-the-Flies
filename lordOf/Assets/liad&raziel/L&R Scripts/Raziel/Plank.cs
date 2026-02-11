using UnityEngine;

public class Plank : MonoBehaviour
{
    [SerializeField] private int speed = 3;
    Rigidbody2D rb2d;
    Vector2 down = Vector2.down;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + speed * down * Time.fixedDeltaTime);
    }
}
