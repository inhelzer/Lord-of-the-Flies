using UnityEngine;

public class ball_attack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        /*
        if(other.gameObject.GetComponent(tag).name == "player")
        {

        }
        if(other.gameObject.GetComponent(tag).name != "enemy")
        {
            Destroy(gameObject);
        }
        */
    }
}
