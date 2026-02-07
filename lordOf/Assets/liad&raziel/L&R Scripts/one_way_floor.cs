using UnityEngine;

public class one_way_floor : MonoBehaviour
{
    [SerializeField] GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y-1.8f >= gameObject.transform.position.y)
        {
            GetComponent<Collider2D>().enabled = true;
        }
        else
        {
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
