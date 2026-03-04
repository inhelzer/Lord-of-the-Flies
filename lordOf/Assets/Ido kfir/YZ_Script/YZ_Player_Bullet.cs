using UnityEngine;

public class YZ_Player_Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 6.7f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.CompareTag("Player")) return;
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }

}
