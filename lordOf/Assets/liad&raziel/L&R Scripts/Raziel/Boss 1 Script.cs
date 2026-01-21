using System.Collections;
using UnityEngine;
public class Boss1Script : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Transform playerTransform;

    [Header("Choose Attack")]
    public int amountOfAttacks;
    private float chooseAttackTimer;
    private float lastAttack = 0;
    public float deltaAttack;
    private bool isAttacking = false;

    [Header("Attack 0")]
    [SerializeField] Vector3 attack0SpawnPosition;
    [SerializeField] GameObject attack0Object;
    public float object0Velocity;
    public int amountToSpawn;
    public float object0Life;
    public float deltaSpawn0;

    [Header("Attack 1")]
    [SerializeField] GameObject attack1Object;
    public float object1Velocity;
    public float object1Life;
    public float deltaSpawnPosition1;
   

    void Update()
    {
        ChooseAttack();
    }
    private void ChooseAttack()
    {
        chooseAttackTimer = Time.timeSinceLevelLoad - lastAttack;
        if ((chooseAttackTimer >= deltaAttack) && !isAttacking)
        {
            isAttacking = true;
            lastAttack = Time.timeSinceLevelLoad;
            switch (Random.Range(0, amountOfAttacks))
            {
                case 0:
                    Debug.Log("Attack " + 0);
                    Attack0();
                    break;

                case 1:
                    Debug.Log("Attack " + 1);
                    Attack1();
                    break;

                case 2:
                    Debug.Log("Attack " + 2);
                    isAttacking = false; // Remove when an attack is added
                    break;

                case 3:
                    Debug.Log("Attack " + 3);
                    isAttacking = false; // Remove when an attack is added
                    break;

                case 4:
                    Debug.Log("Attack " + 4);
                    isAttacking = false; // Remove when an attack is added
                    break;

                default:
                    Debug.Log("Entered default state.");
                    isAttacking = false;
                    break;
            }
        }
    }
    private void Attack0()
    {
        StartCoroutine(Attack0Coroutine());
        isAttacking = false;
    }
    private IEnumerator Attack0Coroutine()
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
            GameObject thisAttackObject = Instantiate(attack0Object, attack0SpawnPosition, Quaternion.identity);
            thisAttackObject.GetComponent<Rigidbody2D>().linearVelocityX = object0Velocity;
            Destroy(thisAttackObject, object0Life);
            yield return new WaitForSeconds(deltaSpawn0);
        }
    }
    private void Attack1()
    {
        GameObject thisAttackObject = Instantiate(attack1Object,
            new Vector3(playerTransform.position.x, playerTransform.position.y + deltaSpawnPosition1, 0), Quaternion.identity);
        thisAttackObject.GetComponent<Rigidbody2D>().linearVelocityY = object1Velocity;
        Destroy(thisAttackObject, object1Life);
        isAttacking = false;
    }
}
