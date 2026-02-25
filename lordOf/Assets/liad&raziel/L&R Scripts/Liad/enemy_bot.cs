using Unity.Mathematics;
using UnityEngine;

public class enemy_bot : MonoBehaviour
{
    GameObject player;
    [SerializeField] string playerName;
    [SerializeField] GameObject ball;
    [SerializeField] float max_speed;
    float accelerator = 0;
    float direction = 0;
    //float delay = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find(playerName);
    }

    // Update is called once per frame
    void Update()
    {
        if (math.round(player.transform.position.x) == math.round(gameObject.transform.position.x))
        {
            accelerator = accelerator - (accelerator / 2.0f);
        }
        else if (player.transform.position.x - gameObject.transform.position.x < 20f && player.transform.position.x - gameObject.transform.position.x > -20f)
        {
            direction = math.sqrt(math.pow(player.transform.position.x - gameObject.transform.position.x, 2)) / (player.transform.position.x - gameObject.transform.position.x); // caculating direction from the player
            transform.Translate(direction * Time.deltaTime * accelerator, 0, 0);
            if (accelerator < max_speed)
            {
                accelerator = accelerator + 0.05f;
            }
        }
        if (direction > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        // ball attack
        /*
        if (player.transform.position.x - gameObject.transform.position.x < 8f && player.transform.position.x - gameObject.transform.position.x > -8f)
        {
            if (delay < Time.timeSinceLevelLoad)
            {
                Instantiate(ball, transform.position + new Vector3(player.transform.position.x - gameObject.transform.position.x, 7, 0), Quaternion.identity);
                delay = Time.timeSinceLevelLoad + 1f;
            }
        }
        */ 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != player)
        {
            Destroy(gameObject);
        }
    }
}
