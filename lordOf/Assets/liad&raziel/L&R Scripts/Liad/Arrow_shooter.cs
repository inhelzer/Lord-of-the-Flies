using UnityEngine;

public class Arrow_shooter : MonoBehaviour
{
    [SerializeField]GameObject shoot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            Instantiate(shoot, transform.position + new Vector3(1, 0, 0), Quaternion.identity);
        }
    }
}
