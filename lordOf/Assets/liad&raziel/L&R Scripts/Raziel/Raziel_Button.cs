using UnityEngine;

public class Raziel_Button : MonoBehaviour
{
    [SerializeField] GameObject door;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (door != null)
        door.GetComponent<Raziel_Door>().OpenDoor();
    }
}
