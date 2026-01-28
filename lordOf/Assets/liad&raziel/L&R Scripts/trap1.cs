using UnityEngine;

public class trap1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float delay;
    bool trap_on = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (trap_on)
        {
            if (delay < Time.timeSinceLevelLoad)
            {
                Destroy(gameObject);
            }
            // משהו שמתריאה את השחקן
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        delay = Time.timeSinceLevelLoad+1f;
        trap_on = true;
    }
}
