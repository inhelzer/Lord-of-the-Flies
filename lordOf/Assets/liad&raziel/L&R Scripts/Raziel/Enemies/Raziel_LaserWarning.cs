using UnityEngine;

public class Raziel_LaserWarning : MonoBehaviour
{
    Transform playerTransform;
    Vector3 direction;

    private void Start()
    {
        playerTransform = GameObject.Find("Raziel_BasePlayer Variant").transform;
    }

    private void Update()
    {
        direction = playerTransform.position - transform.position;
        transform.right = direction;
    }
}
