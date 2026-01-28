using System.Collections;
using UnityEngine;

public class AttackObject2 : MonoBehaviour
{
    [SerializeField] private int Health;

    Rigidbody2D rb2d;

    private GameObject Player;
    public int damage;

    [Header("Must be positive")]
    public float approachX;
    public float approachY;
    public float timeBetweenApproaches;
    public float moveDuration;

    private bool hasHitPlayer = false;
    private Coroutine approachRoutine;
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        approachRoutine = StartCoroutine(ApproachCoroutine());
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            hasHitPlayer = true;
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
    private IEnumerator ApproachCoroutine()
    {
        while (!hasHitPlayer)
        {
            Vector2 direction = (Player.transform.position - transform.position).normalized;
            rb2d.linearVelocity = new Vector2(
                direction.x * approachX,
                direction.y * approachY
            );
            yield return new WaitForSeconds(moveDuration);
            rb2d.linearVelocity = Vector2.zero;
            yield return new WaitForSeconds(timeBetweenApproaches);
        }
    }
}
