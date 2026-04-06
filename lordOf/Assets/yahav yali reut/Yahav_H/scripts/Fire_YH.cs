using UnityEngine;

public class Fire_YH : MonoBehaviour
{
    public float speed = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Time.timeSinceLevelLoad * speed;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
    }
}
