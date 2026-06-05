using UnityEngine;
using System.Collections;

public class SideStepTimer : MonoBehaviour
{
    public float moveAmount = -0.4f;
    public float interval = 3.5f;
    public float moveDuration = 0.25f;

    private float timer;
    private bool isMoving;
    private bool started;   // NEW

    private Vector3 startPos;
    private Vector3 targetPos;
    private float moveT;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.applyRootMotion = false;

        StartCoroutine(StartWithDelay()); // NEW
    }

    IEnumerator StartWithDelay()
    {
        yield return new WaitForSeconds(0.25f);
        started = true;
    }

    void Update()
    {
        if (!started) return; // NEW: blocks everything for first 0.5s

        if (!isMoving)
        {
            timer += Time.deltaTime;

            if (timer >= interval)
            {
                timer = 0f;
                StartMove();
            }
        }
        else
        {
            SmoothMove();
        }
    }

    void StartMove()
    {
        isMoving = true;
        moveT = 0f;

        startPos = transform.position;
        targetPos = startPos + new Vector3(moveAmount, 0f, 0f);
    }

    void SmoothMove()
    {
        moveT += Time.deltaTime;
        float progress = moveT / moveDuration;

        transform.position = Vector3.Lerp(startPos, targetPos, progress);

        if (progress >= 1f)
        {
            transform.position = targetPos;
            isMoving = false;
        }
    }
}