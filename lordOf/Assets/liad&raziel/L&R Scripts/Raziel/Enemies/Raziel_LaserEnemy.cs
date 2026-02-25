using UnityEngine;
using System.Collections;

public class Raziel_LaserEnemy : Raziel_Enemy1
{
    [SerializeField] Transform playerTransform;
    [SerializeField] float attackRange;
    float deltaFromPlayer;
    bool isAttacking;
    [SerializeField] float warningDuration;
    [SerializeField] float delayDuration;
    [SerializeField] float attackDuration;

    GameObject laserWarning;
    GameObject laser;

    [Header("Animations")]
    [SerializeField] string playerSpotted;
    [SerializeField] string playerLost;
    Animator animator;
    string currentAnimation;
    bool spotted;

    private void Start()
    {
        isAttacking = false;
        laserWarning = transform.Find("LaserWarning").gameObject;
        laser = transform.Find("Laser").gameObject;

        laserWarning.SetActive(false);
        laser.SetActive(false);

        animator = GetComponent<Animator>();
        spotted = false;
    }


    private void Update()
    {
        if (!isAttacking)
        {
            deltaFromPlayer = Mathf.Abs(transform.position.x - playerTransform.position.x);
            if (deltaFromPlayer < attackRange && playerTransform.position.y >= transform.position.y && playerTransform.position.y < transform.position.y + attackRange)
            {
                StartCoroutine(Attack());
            }
        }
    }

    public IEnumerator Attack()
    {
        isAttacking = true;


        ChangeAnimationState(playerSpotted);
        laserWarning.SetActive(true);
        yield return new WaitForSeconds(warningDuration);

        laserWarning.SetActive(false);
        yield return new WaitForSeconds(delayDuration);

        laser.SetActive(true);
        laser.transform.rotation = Quaternion.Euler(0f, 0f, laserWarning.transform.rotation.eulerAngles.z);
        yield return new WaitForSeconds(attackDuration);

        laser.SetActive(false);
        ChangeAnimationState(playerLost);
        

        isAttacking = false;
    }

    protected override void SwitchDirection()
    {
        direction = -1 * direction;

        yRotation += 180f;
        if (yRotation == 360f)
            yRotation = 0;
        transform.rotation = Quaternion.Euler(0, yRotation, 0);

        // Override
        laser.transform.rotation = Quaternion.Euler(0f, 0f, laserWarning.transform.rotation.eulerAngles.z);
    }

    public void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;
        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }
}
