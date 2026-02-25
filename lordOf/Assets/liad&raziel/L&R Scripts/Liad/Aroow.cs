using UnityEngine;

public class Aroow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-15f * Time.deltaTime, 0, 0);
    }
    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (!(hit.gameObject.tag == "Bad"))
        {
            Destroy(gameObject);
        }
    }
}
