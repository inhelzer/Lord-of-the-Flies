using UnityEngine;

public class elevator2 : MonoBehaviour
{
    public float xMove;
    public float yMove;
    public float speed;
    public bool isPressed = false;

    private float startY;   // נשמור את ה-Y ההתחלתי
    private bool goingUp = true; // בודק אם עולה או יורד

    void Start()
    {
        // שומרים את המיקום ההתחלתי בציר Y
        startY = transform.position.y;
    }

    void Update()
    {
        if (isPressed)
        {
            // נחשב את המטרה: או למעלה או חזרה למטה
            float targetY = goingUp ? startY + yMove : startY;

            // זזים לכיוון היעד
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(transform.position.x + xMove * Time.deltaTime * speed, targetY, transform.position.z),
                speed * Time.deltaTime
            );

            // אם הגענו ליעד -> מחליפים כיוון
            if (Mathf.Abs(transform.position.y - targetY) < 0.01f)
            {
                goingUp = !goingUp;
            }
        }
    }
}
