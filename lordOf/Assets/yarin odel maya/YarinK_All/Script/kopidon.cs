using UnityEngine;

public class kopidon : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 2f;
    public float changeTargetDistance = 0.2f;

    [Header("Bounds (YOU SET)")]
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private Vector2 target;

    void Start()
    {
        PickNewTarget();
    }

    void Update()
    {
        MoveToTarget();
    }

    void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, target) < changeTargetDistance)
        {
            PickNewTarget();
        }
    }

    void PickNewTarget()
    {
        target = new Vector2(
            Random.Range(minBounds.x, maxBounds.x),
            Random.Range(minBounds.y, maxBounds.y)
        );
    }
}

