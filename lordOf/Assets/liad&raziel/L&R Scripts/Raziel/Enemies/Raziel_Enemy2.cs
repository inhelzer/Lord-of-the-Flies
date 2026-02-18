using System.Collections;
using UnityEngine;

public class Raziel_Enemy2 : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] float aggroRange;
    [SerializeField] float attackRange;
    [SerializeField] float aggroSpeed;
    float deltaFromPlayer;
    bool aggroed;
    bool attacking;
    bool mayRoam;
    [SerializeField] float attackDuration;

    [SerializeField] GameObject weapon;
    [SerializeField] float weaponDeltaY;
    [SerializeField] float weaponDeltaX;
    Vector3 weaponDestination;

    [SerializeField] float speed;
    Vector2 direction;
    Rigidbody2D rb2d;
    float yRotation;
    bool canMove;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        direction = new Vector2(1, 0);
        yRotation = 0;

        canMove = false;
        mayRoam = true;
        aggroed = false;
        attacking = false;

        weaponDestination = new Vector3();
    }

    private void Update()
    {
        AttractWeapon();

        deltaFromPlayer = Mathf.Abs(transform.position.x - playerTransform.position.x);
        if (deltaFromPlayer < aggroRange && playerTransform.position.y >= transform.position.y && playerTransform.position.y < transform.position.y + aggroRange)
        {
            mayRoam = false;
            if (deltaFromPlayer < attackRange)
            {
                aggroed = false;
            }
            else
            {
                aggroed = true;
            }
        }
        else if (!mayRoam)
        {
            mayRoam = true;
        }
    }

    private void FixedUpdate()
    {
        if (!attacking)
        {
            if (canMove && mayRoam)
            {
                Move();
            }
            else if (aggroed)
            {
                MoveTowardsPlayer();
            }
            else
            {
                StartCoroutine(Attack());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground") && !canMove)
        {
            canMove = true;
        }
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("ground"))
        {
            SwitchDirection();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            SwitchDirection();
        }
    }

    void Move()
    {
        rb2d.MovePosition(rb2d.position + direction * speed * Time.fixedDeltaTime);
    }

    void MoveTowardsPlayer()
    {
        if (playerTransform.position.x < transform.position.x && direction.x == 1 || playerTransform.position.x > transform.position.x && direction.x == -1)
        {
            direction = -direction;
        }
        
        rb2d.MovePosition(rb2d.position + direction * aggroSpeed * Time.fixedDeltaTime);
    }

    private IEnumerator Attack()
    {
        attacking = true;
        weapon.SetActive(true);

        yield return new WaitForSeconds(attackDuration);

        weapon.SetActive(false);
        attacking = false;
    }

    void AttractWeapon()
    {
        if (direction.x == 1)
            weaponDestination = new Vector3(transform.position.x + weaponDeltaX, transform.position.y + weaponDeltaY, 0);
        else
            weaponDestination = new Vector3(transform.position.x - weaponDeltaX, transform.position.y + weaponDeltaY, 0);

        weapon.transform.position = weaponDestination;
    }

    void SwitchDirection()
    {
        direction = -direction;
        yRotation += 180f;
        if (yRotation == 360f)
            yRotation = 0;
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
