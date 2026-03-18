using UnityEngine;

public class Aroow : MonoBehaviour
{
    float DI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DI = Time.time + 5f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 15f * Time.deltaTime, 0);
        if (DI <= Time.time)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (!hit.gameObject.CompareTag("Bad"))
        {
            Destroy(gameObject);
        }
    }
}
