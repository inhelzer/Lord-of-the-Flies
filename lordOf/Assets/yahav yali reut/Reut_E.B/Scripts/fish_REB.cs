using UnityEngine;
using System.Collections;

public class fish_REB : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveTime = 0.2f;
    [SerializeField] private float interval = 0.75f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);

            yield return new WaitForSeconds(moveTime);

            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
    }
}