using UnityEngine;

public class rotate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float switchTime = 1f;   // how often to switch
    private float timer;
    private bool rotated = false;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= switchTime)
        {
            timer = 0f;

            rotated = !rotated;

            if (rotated)
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            else
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}
