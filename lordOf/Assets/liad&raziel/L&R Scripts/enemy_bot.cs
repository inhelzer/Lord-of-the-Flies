using Unity.Mathematics;
using UnityEngine;

public class enemy_bot : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject ball;
    [SerializeField] float enemy_speed;
    float direction = 0;
    float delay = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x - gameObject.transform.position.x < 10f && player.transform.position.x - gameObject.transform.position.x > -10f)
        {
            direction = math.sqrt(math.pow(player.transform.position.x - gameObject.transform.position.x, 2)) / (player.transform.position.x - gameObject.transform.position.x); // caculating direction from the player
            transform.Translate(direction * (enemy_speed * Time.deltaTime), 0, 0);
        }
        if (player.transform.position.x - gameObject.transform.position.x < 8f && player.transform.position.x - gameObject.transform.position.x > -8f)
        {
            if (delay < Time.timeSinceLevelLoad)
            {
                Instantiate(ball, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
                delay = Time.timeSinceLevelLoad + 1f;
            }
        }
    }
}
