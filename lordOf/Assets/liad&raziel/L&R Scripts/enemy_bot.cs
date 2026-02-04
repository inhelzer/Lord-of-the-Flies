using Unity.Mathematics;
using UnityEngine;

public class enemy_bot : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject ball;
    [SerializeField] float max_speed;
    float accelerator = 0;
    float direction = 0;
    //float delay = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (math.round(player.transform.position.x) == math.round(gameObject.transform.position.x))
        {
            accelerator = accelerator - (accelerator / 2.0f);
        }
        else if (player.transform.position.x - gameObject.transform.position.x < 10f && player.transform.position.x - gameObject.transform.position.x > -10f)
        {
            direction = math.sqrt(math.pow(player.transform.position.x - gameObject.transform.position.x, 2)) / (player.transform.position.x - gameObject.transform.position.x); // caculating direction from the player
            transform.Translate(direction * Time.deltaTime * accelerator, 0, 0);
            if (accelerator < max_speed)
            {
                accelerator = accelerator + 0.05f;
            }
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
}
