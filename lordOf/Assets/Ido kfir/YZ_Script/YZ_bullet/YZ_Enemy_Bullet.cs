using UnityEngine;

public class YZ_Enemy_Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 6.7f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.CompareTag("Enemy")) return;
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

}
