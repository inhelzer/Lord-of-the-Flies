using System.Collections;
using UnityEngine;

public class Raziel_Enemy2 : MonoBehaviour
{
    Transform playerTransform;
    [SerializeField] float aggroRange;
    [SerializeField] float attackRange;
    [SerializeField] float aggroSpeed;
    float deltaFromPlayer;
    [SerializeField] bool aggroed;
    [SerializeField] bool attacking;
    [SerializeField] bool mayRoam;
    [SerializeField] float attackDuration;

    [Header("Animation")]
    Animator animator;
    string currentAnimation;
    public string walk;
    public string startAttack;
    public string attack;

    /*
    [SerializeField] GameObject weapon;
    [SerializeField] float weaponDeltaY;
    [SerializeField] float weaponDeltaX;
    Vector3 weaponDestination;
    */

    [SerializeField] float speed;
    float direction;
    Rigidbody2D rb2d;
    float yRotation;
    bool canMove;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        direction = 1;
        yRotation = 0;

        canMove = false;
        mayRoam = true;
        aggroed = false;
        attacking = false;

        //weaponDestination = new Vector3();
    }

    private void Start()
    {
        playerTransform = GameObject.Find("Raziel_BasePlayer Variant").transform;

        animator = GetComponent<Animator>();
        ChangeAnimationState(walk);
    }
    private void Update()
    {
        //AttractWeapon();

        if (!attacking)
        {
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

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider.CompareTag("Weapon"))
            return;

        if (collision.gameObject.CompareTag("ground") && !canMove)
        {
            canMove = true;
        }
        else if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("ground"))
        {
            SwitchDirection();
        }
    }
    */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("ground") && !canMove)
        {
            canMove = true;
        }
        
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Wind") && !collision.gameObject.CompareTag("ground") &&
            !collision.gameObject.CompareTag("LaserWarning") && !collision.gameObject.CompareTag("Laser"))
        {
            SwitchDirection();
        }
    }

    /*
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            SwitchDirection();
        }
    }
    */

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            SwitchDirection();
        }
    }

    void Move()
    {
        rb2d.linearVelocityX = direction * speed;
    }

    void Move(float speed)
    {
        rb2d.linearVelocityX = direction * speed;
    }

    void MoveTowardsPlayer()
    {
        if (playerTransform.position.x < transform.position.x && direction == 1 || playerTransform.position.x > transform.position.x && direction == -1)
        {
            //direction = -direction;
            SwitchDirection();
        }

        Move(aggroSpeed);
    }

    private IEnumerator Attack()
    {
        attacking = true;
        //weapon.SetActive(true);

        /*
        ChangeAnimationState(startAttack);
        yield return new WaitUntil(() =>
        animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f &&
        !animator.IsInTransition(0));
        */


        ChangeAnimationState(attack);

        yield return new WaitForSeconds(attackDuration);

        //weapon.SetActive(false);
        ChangeAnimationState(walk);
        attacking = false;
    }

    /*
    void AttractWeapon()
    {
        if (direction.x == 1)
            weaponDestination = new Vector3(transform.position.x + weaponDeltaX, transform.position.y + weaponDeltaY, 0);
        else
            weaponDestination = new Vector3(transform.position.x - weaponDeltaX, transform.position.y + weaponDeltaY, 0);

        weapon.transform.position = weaponDestination;
    }
    */

    void SwitchDirection()
    {
        direction = -direction;
        yRotation += 180f;
        if (yRotation == 360f)
            yRotation = 0;
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;
        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }
}
