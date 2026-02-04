using System.Collections;
using UnityEngine;

public class PlankSpawner : MonoBehaviour
{
    [SerializeField] GameObject plank;
    [SerializeField] float delay;
    [SerializeField] float death;

    void Start()
    {
        StartCoroutine(SpawnPlank());
    }

    IEnumerator SpawnPlank()
    {
        while (true)
        {
            GameObject spawned = Instantiate(plank, transform.position, Quaternion.identity);
            Destroy(spawned, death);
            yield return new WaitForSeconds(delay);
        }
    }

}
