using UnityEngine;

public class Raziel_LaserWarning : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    Vector3 direction;

    private void Update()
    {
        direction = playerTransform.position - transform.position;
        transform.right = direction;
    }
}
