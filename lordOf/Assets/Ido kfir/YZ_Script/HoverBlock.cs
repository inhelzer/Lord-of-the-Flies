using UnityEngine;
using System.Collections;

public class HoverBlock : MonoBehaviour
{
    [SerializeField] float downAmount = 0.5f;   // כמה הבלוק יורד
    [SerializeField] float moveTime = 0.2f;       // כמה מהר
    private Vector3 startPos;
    private bool isMoving;

    void Start()
    {
        startPos = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isMoving)
        {
            StartCoroutine(HoverEffect());
        }
    }

    IEnumerator HoverEffect()
    {
        isMoving = true;

        Vector3 downPos = startPos + Vector3.down * downAmount;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / moveTime;
            transform.position = Vector3.Lerp(startPos, downPos, t);
            yield return null;
        }

        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / moveTime;
            transform.position = Vector3.Lerp(downPos, startPos, t);
            yield return null;
        }

        isMoving = false;
    }
}