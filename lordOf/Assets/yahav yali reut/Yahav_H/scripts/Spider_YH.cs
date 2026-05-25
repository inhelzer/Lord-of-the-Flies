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
    public string Dead = "Dead_YH";

    [Header("time")]
    public float wakeTime = 5f;
    public float blinkMinTime = 2f;
    public float blinkMaxTime = 5f;
    public float normalPhaseDuration = 25f;
    public float angryPhaseDuration = 20f;
    public float returnToNormalDuration = 1f;
    public int phaseCycles = 2;

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
    public float handsMoveSmooth = 3f;
    public float hand1AngryAngle = -90f;
    public float hand2AngryAngle = 0f;
    public float hand3AngryAngle = 90f;

    private float wakeTimer;
    private float blinkTimer;
    private float phaseTimer;
    private float returnToNormalTimer;

    private Vector3 startBossPosition;
    private Quaternion hand1StartRot;
    private Quaternion hand2StartRot;
    private Quaternion hand3StartRot;
    private bool hand1WasActiveAtStart;
    private bool hand2WasActiveAtStart;
    private bool hand3WasActiveAtStart;

    private bool wakeFinished = false;
    private bool isAngry = false;
    private bool isDead = false;
    private bool isReturningToNormal = false;
    private bool savedStartPositions = false;
    private float movementStartTime;
    private int completedCycles = 0;

    public bool IsAngry => isAngry;
    public bool IsDead => isDead;

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
        SaveHandsInitialActiveState();
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

        if (isDead)
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
                StartNormalPhase();
            }

            return;
        }

        if (isReturningToNormal)
        {
            MoveBodyToStartPosition();
            ResetHandsToStartRotation();

            if (Time.timeSinceLevelLoad >= returnToNormalTimer)
            {
                StartNormalPhase();
            }

            return;
        }

        if (isAngry)
        {
            ReturnToStartPosition();

            if (Time.timeSinceLevelLoad >= phaseTimer)
            {
                completedCycles++;
                if (completedCycles >= phaseCycles)
                {
                    StartDeadPhase();
                }
                else
                {
                    StartReturnToNormalPhase();
                }
            }

            return;
        }

        MoveBossAndHands();

        if (completedCycles < phaseCycles && Time.timeSinceLevelLoad >= phaseTimer)
        {
            StartAngryPhase();
            return;
        }

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

    void StartNormalPhase()
    {
        isAngry = false;
        isReturningToNormal = false;
        movementStartTime = Time.timeSinceLevelLoad;
        ResetHandsToStartRotation();
        SetHandsVisible(true);
        phaseTimer = Time.timeSinceLevelLoad + normalPhaseDuration;
        currentAnimation = "";
        ChangeAnimationState(Blink);
        ResetBlinkTimer();
    }

    void StartAngryPhase()
    {
        isAngry = true;
        isReturningToNormal = false;
        phaseTimer = Time.timeSinceLevelLoad + angryPhaseDuration;
        ResetHandsToStartRotation();
        SetHandsVisible(true);
        currentAnimation = "";
        ChangeAnimationState(Angry);
    }

    void StartReturnToNormalPhase()
    {
        isAngry = false;
        isReturningToNormal = true;
        returnToNormalTimer = Time.timeSinceLevelLoad + returnToNormalDuration;
        phaseTimer = float.PositiveInfinity;
        ResetHandsToStartRotation();
        SetHandsVisible(false);
        currentAnimation = "";
        ChangeAnimationState(Blink);
        ResetBlinkTimer();
    }

    void StartDeadPhase()
    {
        isDead = true;
        isAngry = false;
        isReturningToNormal = false;
        phaseTimer = float.PositiveInfinity;
        ReturnToStartPosition();
        SetHandsVisible(false);
        currentAnimation = "";
        ChangeAnimationState(Dead);
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

    void SaveHandsInitialActiveState()
    {
        hand1WasActiveAtStart = hand1 != null && hand1.activeSelf;
        hand2WasActiveAtStart = hand2 != null && hand2.activeSelf;
        hand3WasActiveAtStart = hand3 != null && hand3.activeSelf;
    }

    void SetHandsVisible(bool isVisible)
    {
        SetHandVisible(hand1, isVisible, hand1WasActiveAtStart);
        SetHandVisible(hand2, isVisible, hand2WasActiveAtStart);
        SetHandVisible(hand3, isVisible, hand3WasActiveAtStart);
    }

    void SetHandVisible(GameObject hand, bool isVisible, bool wasActiveAtStart)
    {
        if (hand == null) return;
        hand.SetActive(isVisible && wasActiveAtStart);
    }

    void SaveStartPositions()
    {
        if (savedStartPositions) return;

        startBossPosition = transform.position;

        if (hand1 != null)
        {
            hand1StartRot = hand1.transform.localRotation;
        }
        if (hand2 != null)
        {
            hand2StartRot = hand2.transform.localRotation;
        }
        if (hand3 != null)
        {
            hand3StartRot = hand3.transform.localRotation;
        }

        savedStartPositions = true;
    }

    void MoveBossAndHands()
    {
        if (!savedStartPositions) return;

        float currentMoveSpeed = GetCurrentMoveSpeed();
        float elapsedMovementTime = Time.timeSinceLevelLoad - movementStartTime;
        float moveX = Mathf.Sin(elapsedMovementTime * currentMoveSpeed) * moveDistanceX;
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

        float handMotionTime = Mathf.Max(0f, Time.timeSinceLevelLoad - movementStartTime + timeOffset);
        float timeInCycle = handMotionTime % cycleTime;
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

    void ResetHandsToStartRotation()
    {
        if (hand1 != null)
        {
            hand1.transform.localRotation = hand1StartRot;
        }

        if (hand2 != null)
        {
            hand2.transform.localRotation = hand2StartRot;
        }

        if (hand3 != null)
        {
            hand3.transform.localRotation = hand3StartRot;
        }
    }

    void ReturnToStartPosition()
    {
        if (!savedStartPositions) return;

        MoveBodyToStartPosition();
        MoveHandToAngryRotation(hand1, hand1StartRot, hand1AngryAngle, hand1WasActiveAtStart);
        MoveHandToAngryRotation(hand2, hand2StartRot, hand2AngryAngle, hand2WasActiveAtStart);
        MoveHandToAngryRotation(hand3, hand3StartRot, hand3AngryAngle, hand3WasActiveAtStart);
    }

    void MoveBodyToStartPosition()
    {
        if (!savedStartPositions) return;

        transform.position = Vector3.Lerp(transform.position, startBossPosition, Time.deltaTime * 3f);
    }

    void MoveHandToAngryRotation(GameObject hand, Quaternion startRot, float angryAngle, bool wasActiveAtStart)
    {
        if (hand == null || !wasActiveAtStart) return;

        Quaternion targetRotation = startRot * Quaternion.Euler(0f, 0f, angryAngle);
        hand.transform.localRotation = Quaternion.Lerp(
            hand.transform.localRotation,
            targetRotation,
            Time.deltaTime * handsMoveSmooth
        );
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
