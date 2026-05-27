using UnityEngine;

public class BasicSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject prefab;

    [Header("Spawn Time")]
    [SerializeField] private float minTime = 1f;
    [SerializeField] private float maxTime = 2f;

    [Header("Spawn X Range")]
    [SerializeField] private float minX = -5f;
    [SerializeField] private float maxX = 5f;

    [Header("Spawn Y")]
    [SerializeField] private float spawnY = -4f;

    [Header("Smart Spawn")]
    [SerializeField] private float minDistanceFromLastSpawn = 2f;
    [SerializeField] private float minDistanceFromOthers = 1.5f;
    [SerializeField] private int maxSpawnTries = 10;
    [SerializeField] private LayerMask checkLayer;

    private float timer;
    private Vector3 lastSpawnPos;
    private bool hasLastSpawn;

    private void Start()
    {
        SetRandomTime();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Spawn();
            SetRandomTime();
        }
    }

    private void Spawn()
    {
        for (int i = 0; i < maxSpawnTries; i++)
        {
            float randomX = Random.Range(minX, maxX);
            Vector3 spawnPos = new Vector3(randomX, spawnY, 0f);

            if (!IsFarEnoughFromLastSpawn(spawnPos))
                continue;

            if (!IsAreaFree(spawnPos))
                continue;

            Instantiate(prefab, spawnPos, Quaternion.identity);

            lastSpawnPos = spawnPos;
            hasLastSpawn = true;
            return;
        }

        // אם לא מצא מקום טוב אחרי כמה ניסיונות, פשוט מדלג על הספאון הזה
    }

    private bool IsFarEnoughFromLastSpawn(Vector3 spawnPos)
    {
        if (!hasLastSpawn)
            return true;

        return Vector3.Distance(spawnPos, lastSpawnPos) >= minDistanceFromLastSpawn;
    }

    private bool IsAreaFree(Vector3 spawnPos)
    {
        Collider2D hit = Physics2D.OverlapCircle(spawnPos, minDistanceFromOthers, checkLayer);
        return hit == null;
    }

    private void SetRandomTime()
    {
        timer = Random.Range(minTime, maxTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(minX, spawnY, 0f), new Vector3(maxX, spawnY, 0f));
    }
}