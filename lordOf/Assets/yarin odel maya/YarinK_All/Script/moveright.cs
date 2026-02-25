using UnityEngine;

public class moveright : MonoBehaviour
{
    public GameObject cloudPrefab;

    public float spawnX = -12f;
    public float yPosition = 4f;

    public float spacing = 1.5f;
    public float spawnDelay = 0.5f;

    private float nextSpawnX;

    void Start()
    {
        nextSpawnX = spawnX;
        InvokeRepeating(nameof(SpawnCloud), 0f, spawnDelay);
    }

    void SpawnCloud()
    {
        Vector3 pos = new Vector3(nextSpawnX, yPosition, 0f);
        Instantiate(cloudPrefab, pos, Quaternion.identity);

        nextSpawnX += spacing;
    }

}
