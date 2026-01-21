using UnityEngine;

public class IH_Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] objToSpawn;
    [SerializeField] float delay;
    [SerializeField] float disX;
    [SerializeField] float disY;
    float lastSpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeSinceLevelLoad - lastSpawn >= Random.Range(delay*0.7f, delay*2))
        {
            CreateObj();
        }
    }

    private void CreateObj()
    {
        Instantiate(objToSpawn[Random.Range(0, objToSpawn.Length)],
            transform.position + new Vector3(Random.Range(-disX, disX), Random.Range(-disY, disY), 0),
            Quaternion.identity);
        lastSpawn = Time.timeSinceLevelLoad;
    }
}
