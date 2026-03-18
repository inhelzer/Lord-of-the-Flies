using UnityEngine;

public class spawner : MonoBehaviour
{
    [SerializeField] GameObject objToSpawn;
    [SerializeField] GameObject deathEffect; // Added death effect reference
    float counter;



    [SerializeField] float yMin;
    [SerializeField] float yMax;
    [SerializeField] float xMin;
    [SerializeField] float xMax;

    void Update()
    {
        if (Time.time >= counter)
        {
            // Spawn the object at a random position
            GameObject spawnedObj = Instantiate(
                objToSpawn,
                new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 0),
                Quaternion.identity);

            // Destroy the spawned object after 7 seconds
            Destroy(spawnedObj, 7f);

            // Update counter to spawn the next object after 1 second
            counter = Time.time + 5f;
        }

    }
}
