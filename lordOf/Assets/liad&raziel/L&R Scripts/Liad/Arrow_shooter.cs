using UnityEngine;

public class Arrow_shooter : MonoBehaviour
{
    [SerializeField]GameObject shoot;
    [SerializeField] GameObject player;
    float delay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        delay = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x - gameObject.transform.position.x < 20f && player.transform.position.x - gameObject.transform.position.x > -20f && delay < Time.timeSinceLevelLoad)
        {
            Instantiate(shoot, transform.position - new Vector3(1,0,0), Quaternion.Euler(0,0,90));
            delay = Time.timeSinceLevelLoad + 1.5f;
        }
    }
}
