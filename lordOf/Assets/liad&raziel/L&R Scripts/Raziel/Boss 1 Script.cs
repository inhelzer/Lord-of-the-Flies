using System.Collections;
using UnityEngine;
public class Boss1Script : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] GameObject Player;

    [Header("Choose Attack")]
    public int amountOfAttacks;
    private float chooseAttackTimer;
    private float lastAttack = 0;
    public float deltaAttack;
    private bool isAttacking = false;
    public float SecondsBeforeNextAttack;

    [Header("Attack 0")]
    [SerializeField] Vector3 attack0SpawnPosition;
    [SerializeField] GameObject attack0Object;
    public float object0Velocity;
    public int amountToSpawn0;
    public float object0Life;
    public float deltaSpawn0;

    [Header("Attack 1")]
    [SerializeField] GameObject attack1Object;
    public float object1Velocity;
    public float object1Life;
    public float deltaSpawnPosition1;

    [Header("Attack 2")]
    [SerializeField] GameObject attack2Object;
    [SerializeField] Vector3 attack2SpawnPosition;
    public int amountToSpawn2;
    public float object2Life;
    public float deltaSpawn2;

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
                    Attack2();
                    break;

                default:
                    Debug.Log("Entered default state.");
                    isAttacking = false;
                    break;
            }
            WaitBeforeNextAttack();
        }
    }
    private IEnumerator WaitBeforeNextAttack()
    {
        yield return new WaitForSeconds(SecondsBeforeNextAttack);
    }
    private void Attack0()
    {
        StartCoroutine(Attack0Coroutine());
        isAttacking = false;
    }
    private IEnumerator Attack0Coroutine()
    {
        for (int i = 1; i <= amountToSpawn0; i++)
        {
            GameObject thisAttackObject = Instantiate(attack0Object, attack0SpawnPosition, Quaternion.identity);
            thisAttackObject.GetComponent<AttackObject1>().SetPlayerToTrack(Player);
            thisAttackObject.GetComponent<Rigidbody2D>().linearVelocityX = object0Velocity;
            Destroy(thisAttackObject, object0Life);
            yield return new WaitForSeconds(deltaSpawn0);
        }
    }
    private void Attack1()
    {
        GameObject thisAttackObject = Instantiate(attack1Object,
            new Vector3(Player.transform.position.x, Player.transform.position.y + deltaSpawnPosition1, 0), Quaternion.identity);
        thisAttackObject.GetComponent<AttackObject1>().SetPlayerToTrack(Player);
        thisAttackObject.GetComponent<Rigidbody2D>().linearVelocityY = object1Velocity;
        Destroy(thisAttackObject, object1Life);
        isAttacking = false;
    }
    private void Attack2()
    {
        StartCoroutine(Attack2Coroutine());
        isAttacking = false;
    }
    private IEnumerator Attack2Coroutine()
    {
        for (int i = 1; i <= amountToSpawn2; i++)
        {
            GameObject thisAttackObject = Instantiate(attack2Object, attack2SpawnPosition, Quaternion.identity);
            thisAttackObject.GetComponent<AttackObject2>().SetPlayerToTrack(Player);
            Destroy(thisAttackObject, object2Life);
            yield return new WaitForSeconds(deltaSpawn2);
        }
    }
    
}
