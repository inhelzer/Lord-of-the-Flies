using UnityEngine;

public class CircleRotation : MonoBehaviour
{
    public float rotationSpeed = 100f;

    void Update()
    {
        // Vector3.forward מייצג את ציר ה-Z
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}