using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Jily_YH : MonoBehaviour
{
    Vector3 originalScale;
    bool hit = false;
    float hit_timer;
    int did1 = 1;
    int did2 = 1;
    int did3 = 1;
    int did4 = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

        if (hit && hit_timer +0.1 >= Time.timeSinceLevelLoad )
        {
            if (did1 == 1)
            {
                transform.localScale = new Vector3(originalScale.x, originalScale.y - 1, originalScale.z);
                did1 = 0;
            }
        }
        else if (hit && hit_timer + 0.2 >= Time.timeSinceLevelLoad)
        {
            if (did2 == 1)
            {
                transform.localScale = new Vector3(originalScale.x, originalScale.y - 2, originalScale.z);
                did2 = 0;
            }
        }
        else if (hit && hit_timer + 0.3 >= Time.timeSinceLevelLoad)
        {
            if (did3 == 1)
            {
                transform.localScale = new Vector3(originalScale.x, originalScale.y + 2, originalScale.z);
                did3 = 0;
            }
        }
        else if (hit && hit_timer + 0.4 >= Time.timeSinceLevelLoad)
        {
            if (did4 == 1)
            {
                transform.localScale = new Vector3(originalScale.x, originalScale.y + 1, originalScale.z);
                did4 = 0;
            }
        }
        else if (hit && hit_timer + 0.5 >= Time.timeSinceLevelLoad)
        {
            did1 = 1;
            did2 = 1;
            did3 = 1;
            did4 = 1;
            hit = false;
        }

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            hit = true;
            hit_timer = Time.timeSinceLevelLoad;
        }
    }
}
