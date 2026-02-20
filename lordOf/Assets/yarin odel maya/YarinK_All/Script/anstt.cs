using UnityEngine;
using UnityEngine.EventSystems;

public class anstt : MonoBehaviour
{
    public GameObject cloudPrefab;
    public float spawnInterval = 3f;
    public float cloudSpeed = 2f;
    public Vector2 moveDirection = Vector2.right;

    private BoxCollider2D box;

    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        InvokeRepeating(nameof(SpawnCloud), 0f, spawnInterval);
    }

    void SpawnCloud()
    {
        if (cloudPrefab == null || box == null) return;

        Bounds bounds = box.bounds;

        float spawnX = bounds.min.x;
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        Vector2 spawnPosition = new Vector2(spawnX, randomY);

        GameObject cloud = Instantiate(cloudPrefab, spawnPosition, Quaternion.identity);

        Rigidbody2D rb = cloud.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = moveDirection.normalized * cloudSpeed;
        }
    }
}
