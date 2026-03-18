using UnityEngine;

public class Arrow_shooter : MonoBehaviour
{
    GameObject player;
    [SerializeField]GameObject shoot;
    [SerializeField] string playerName;
    float delay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find(playerName);
        delay = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x - gameObject.transform.position.x < 20f && player.transform.position.x - gameObject.transform.position.x > -20f && delay < Time.time)
        {
            Instantiate(shoot, transform.position - new Vector3(1,0,0), Quaternion.Euler(0,0,90));
            delay = Time.time + 1.5f;
        }
    }
}
