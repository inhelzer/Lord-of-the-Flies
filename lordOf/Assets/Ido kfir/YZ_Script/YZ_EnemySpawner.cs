using UnityEngine;

public class BasicSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject prefab;

    [Header("Spawn Time")]
    [SerializeField] private float minTime = 1f;
    [SerializeField] private float maxTime = 3f;

    [Header("Spawn X Range")]
    [SerializeField] private float minX = -5f;
    [SerializeField] private float maxX = 5f;

    [Header("Spawn Y")]
    [SerializeField] private float spawnY = -4f;

    private float timer;

    void Start()
    {
        SetRandomTime();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Spawn();
            SetRandomTime();
        }
    }

    void Spawn()
    {
        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPos = new Vector3(randomX, spawnY, 0f);

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }

    void SetRandomTime()
    {
        timer = Random.Range(minTime, maxTime);
    }
}