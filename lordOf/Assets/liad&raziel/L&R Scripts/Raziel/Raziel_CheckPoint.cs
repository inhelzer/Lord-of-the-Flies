using UnityEngine;

public class Raziel_CheckPoint : MonoBehaviour
{
    private Vector3 myPosition;
    Liad_player playerCode;
    [SerializeField] private GameObject player;
    private void Start()
    {
        myPosition = transform.position;
        playerCode = player.GetComponent<Liad_player>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
            playerCode.SetRespawnPosition(myPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            playerCode.SetRespawnPosition(myPosition);
    }
}
