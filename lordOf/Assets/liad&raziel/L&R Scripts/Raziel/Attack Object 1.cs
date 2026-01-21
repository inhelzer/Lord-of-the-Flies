using UnityEngine;

public class AttackObject1 : MonoBehaviour
{
    [SerializeField] private int Health;

    private GameObject Player;
    public int damage;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Player.GetComponent<Player>().DealDamage(damage);
            SelfDestruct();
        }
    }
    void SelfDestruct()
    {
        Destroy(gameObject);
    }
    public void SetPlayerToTrack(GameObject Player)
    {
        this.Player = Player;
    }
}
