using UnityEngine;
using static UnityEngine.GridBrushBase;

public class Kinfe_YH : MonoBehaviour
{
    public Vector3 rotationDirection;
    float r_speed = 300;
    int direction = 4;
    float timer = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, direction * r_speed * Time.deltaTime);

        if (timer + 4 > Time.timeSinceLevelLoad)
        {
            direction = -4;
        }
        else if (timer + 4.4 > Time.timeSinceLevelLoad)
        {
            direction = 40;
        }
        else { timer = Time.timeSinceLevelLoad; }



    }
}
