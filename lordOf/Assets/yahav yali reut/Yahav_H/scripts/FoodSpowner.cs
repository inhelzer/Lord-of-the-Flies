using UnityEngine;

public class FoodSpowner : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private float minSpawnX = -7.5f;
    [SerializeField] private float maxSpawnX = 7.5f;
    [SerializeField] private float spawnY = -1.5f;
    [SerializeField] private float spawnInterval = 4f;
    [SerializeField] private int maxFoodOnScene = 3;

    private float nextSpawnTime;

    void Start()
    {
        nextSpawnTime = Time.timeSinceLevelLoad + spawnInterval;
    }

    void Update()
    {
        if (foodPrefab == null)
        {
            return;
        }

        if (Time.timeSinceLevelLoad < nextSpawnTime)
        {
            return;
        }

        if (CountExistingFood() >= maxFoodOnScene)
        {
            nextSpawnTime = Time.timeSinceLevelLoad + spawnInterval;
            return;
        }

        SpawnFood();
        nextSpawnTime = Time.timeSinceLevelLoad + spawnInterval;
    }

    void SpawnFood()
    {
        float randomX = Random.Range(minSpawnX, maxSpawnX);
        Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f);
        Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
    }

    int CountExistingFood()
    {
        GameObject[] allFood = GameObject.FindGameObjectsWithTag("Peror");
        return allFood.Length;
    }
}
