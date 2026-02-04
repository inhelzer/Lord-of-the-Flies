using UnityEngine;

public class Raziel_Button : MonoBehaviour
{
    [SerializeField] GameObject door;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        door.GetComponent<Raziel_Door>().OpenDoor();
    }
}
