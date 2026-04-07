using UnityEngine;

public class Spider_YH : MonoBehaviour
{
    [Header("anim")]
    public GameObject Head;
    private Animator anim;
    private string currentAnimation;

    public string Wake = "Wakeing";
    public string Blink = "Blink";
    public string Angry = "Anger";

    [Header("time")]
    public float wakeTime = 5f;
    public float blinkMinTime = 2f;
    public float blinkMaxTime = 5f;
    public float angryTime = 3f;

    [Header("movement")]
    public GameObject hand1;
    public GameObject hand2;
    public GameObject hand3;
    public float moveDistanceX = 1.5f;
    public float startMoveSpeed = 0.7f;
    public float fastMoveSpeed = 1.8f;
    public float speedRampDelay = 6f;
    public float speedRampDuration = 5f;
    
    [Header("hand strike")]
    public float handUpAngle = 20f;
    public float handDownAngle = -70f;
    public float handRaiseTime = 0.9f;
    public float handDropTime = 0.18f;
    public float hand1Direction = 1f;
    public float hand2Direction = -1f;
    public float hand3Direction = -1f;

    private float wakeTimer;
    private float blinkTimer;
    private float angryTimer;

    private Vector3 startBossPosition;
    private Quaternion hand1StartRot;
    private Quaternion hand2StartRot;
    private Quaternion hand3StartRot;

    private bool wakeFinished = false;
    private bool isAngry = false;
    private bool savedStartPositions = false;
    private float movementStartTime;

    void Start()
    {
        if (Head != null)
        {
            anim = Head.GetComponent<Animator>();

            if (anim == null)
            {
                anim = Head.GetComponentInChildren<Animator>();
            }
        }

        if (anim == null)
        {
            Debug.LogError("Spider_YH: Head Animator is missing");
            return;
        }

        FindHands();
        SetHandsCollidersAsTriggers();
        ChangeAnimationState(Wake);
        wakeTimer = Time.timeSinceLevelLoad + wakeTime;
    }

    void Update()
    {
        if (anim == null)
        {
            return;
        }

        if (!wakeFinished)
        {
            if (Time.timeSinceLevelLoad >= wakeTimer)
            {
                wakeFinished = true;
                SaveStartPositions();
                movementStartTime = Time.timeSinceLevelLoad;
                currentAnimation = "";
                ChangeAnimationState(Blink);
                ResetBlinkTimer();
            }

            return;
        }

        if (isAngry)
        {
            ReturnToStartPosition();

            if (Time.timeSinceLevelLoad >= angryTimer)
            {
                isAngry = false;
                currentAnimation = "";
                ChangeAnimationState(Blink);
                ResetBlinkTimer();
            }

            return;
        }

        MoveBossAndHands();

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(Blink) && stateInfo.normalizedTime >= 1f)
        {
            currentAnimation = "";
        }

        if (Time.timeSinceLevelLoad >= blinkTimer)
        {
            ChangeAnimationState(Blink);
            ResetBlinkTimer();
        }
    }

    public void ChangeAnimationState(string newAnimation)
    {
        if (anim == null) return;
        if (currentAnimation == newAnimation) return;

        anim.Play(newAnimation);
        currentAnimation = newAnimation;
    }

    public void StartAngry()
    {
        if (anim == null || !wakeFinished)
        {
            return;
        }

        isAngry = true;
        angryTimer = Time.timeSinceLevelLoad + angryTime;
        currentAnimation = "";
        ChangeAnimationState(Angry);
    }

    public void StopAngry()
    {
        if (anim == null)
        {
            return;
        }

        isAngry = false;
        currentAnimation = "";
        ChangeAnimationState(Blink);
        ResetBlinkTimer();
    }

    void ResetBlinkTimer()
    {
        blinkTimer = Time.timeSinceLevelLoad + Random.Range(blinkMinTime, blinkMaxTime);
    }

    void FindHands()
    {
        if (hand1 == null)
        {
            Transform t = transform.Find("hand1");
            if (t != null) hand1 = t.gameObject;
        }

        if (hand2 == null)
        {
            Transform t = transform.Find("hand2");
            if (t != null) hand2 = t.gameObject;
        }

        if (hand3 == null)
        {
            Transform t = transform.Find("hand3");
            if (t != null) hand3 = t.gameObject;
        }
    }

    void SetHandsCollidersAsTriggers()
    {
        SetHandCollidersAsTrigger(hand1);
        SetHandCollidersAsTrigger(hand2);
        SetHandCollidersAsTrigger(hand3);
    }

    void SetHandCollidersAsTrigger(GameObject hand)
    {
        if (hand == null) return;

        Collider2D[] colliders = hand.GetComponentsInChildren<Collider2D>(true);
        foreach (Collider2D handCollider in colliders)
        {
            handCollider.isTrigger = true;
        }
    }

    void SaveStartPositions()
    {
        if (savedStartPositions) return;

        startBossPosition = transform.position;

        if (hand1 != null) hand1StartRot = hand1.transform.localRotation;
        if (hand2 != null) hand2StartRot = hand2.transform.localRotation;
        if (hand3 != null) hand3StartRot = hand3.transform.localRotation;

        savedStartPositions = true;
    }

    void MoveBossAndHands()
    {
        if (!savedStartPositions) return;

        float currentMoveSpeed = GetCurrentMoveSpeed();
        float moveX = Mathf.Sin(Time.timeSinceLevelLoad * currentMoveSpeed) * moveDistanceX;
        transform.position = new Vector3(startBossPosition.x + moveX, startBossPosition.y, startBossPosition.z);

        MoveHand(hand1, hand1StartRot, 0f, hand1Direction);
        MoveHand(hand2, hand2StartRot, 0.35f, hand2Direction);
        MoveHand(hand3, hand3StartRot, 0.7f, hand3Direction);
    }

    void MoveHand(GameObject hand, Quaternion startRot, float timeOffset, float direction)
    {
        if (hand == null) return;

        float cycleTime = handRaiseTime + handDropTime;
        if (cycleTime <= 0.01f) return;

        float timeInCycle = (Time.timeSinceLevelLoad + timeOffset) % cycleTime;
        float angle;

        if (timeInCycle < handRaiseTime)
        {
            float t = timeInCycle / handRaiseTime;
            angle = Mathf.Lerp(handDownAngle, handUpAngle, t);
        }
        else
        {
            float t = (timeInCycle - handRaiseTime) / handDropTime;
            angle = Mathf.Lerp(handUpAngle, handDownAngle, t);
        }

        hand.transform.localRotation = startRot * Quaternion.Euler(0f, 0f, angle * direction);
    }

    void ReturnToStartPosition()
    {
        if (!savedStartPositions) return;

        transform.position = Vector3.Lerp(transform.position, startBossPosition, Time.deltaTime * 3f);

        if (hand1 != null)
        {
            hand1.transform.localRotation = Quaternion.Lerp(hand1.transform.localRotation, hand1StartRot, Time.deltaTime * 4f);
        }

        if (hand2 != null)
        {
            hand2.transform.localRotation = Quaternion.Lerp(hand2.transform.localRotation, hand2StartRot, Time.deltaTime * 4f);
        }

        if (hand3 != null)
        {
            hand3.transform.localRotation = Quaternion.Lerp(hand3.transform.localRotation, hand3StartRot, Time.deltaTime * 4f);
        }
    }

    float GetCurrentMoveSpeed()
    {
        float elapsedMovementTime = Time.timeSinceLevelLoad - movementStartTime;
        if (elapsedMovementTime <= speedRampDelay)
        {
            return startMoveSpeed;
        }

        if (speedRampDuration <= 0f)
        {
            return fastMoveSpeed;
        }

        float rampT = Mathf.InverseLerp(speedRampDelay, speedRampDelay + speedRampDuration, elapsedMovementTime);
        return Mathf.Lerp(startMoveSpeed, fastMoveSpeed, rampT);
    }
}
