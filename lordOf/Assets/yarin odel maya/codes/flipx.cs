using UnityEngine;

public class flipx : MonoBehaviour
{

    [SerializeField] private GameObject target;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 0.1f)
        {
            Vector3 scale = target.transform.localScale;
            scale.x *= -1;
            target.transform.localScale = scale;

            timer = 0f;
        }

    }
}
