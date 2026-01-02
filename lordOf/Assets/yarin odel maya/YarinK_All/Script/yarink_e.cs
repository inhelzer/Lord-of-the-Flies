using UnityEngine;

public class yarink_e : MonoBehaviour
{

    [SerializeField] private GameObject elevator;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float delay = 2f;

    [SerializeField] private Camera mainCamera;
    private Color originalColor;

    [Header("Explosion Settings")]
    [SerializeField] private GameObject smallRectanglePrefab; // המלבן הקטן שאתה שם
    [SerializeField] private int explosionCount = 20;         // כמה מלבנים יתפוצצו
    [SerializeField] private float explosionForce = 6f;       // עוצמת ההתפזרות
    [SerializeField] private float explosionSpread = 1.5f;    // כמה הם רחוקים מנקודת הפיצוץ

    private bool isPressed = false;
    private bool startMoving = false;

    private void Start()
    {
        if (mainCamera != null)
            originalColor = mainCamera.backgroundColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isPressed && other.CompareTag("Player"))
        {
            isPressed = true;

            // הצגת רקע לבן
            ChangeBackgroundColor(Color.white);

            // פיצוץ של 20 מלבנים
            ExplodeRectangles();

            // התחלת תזוזת מעלית
            Invoke("StartElevator", delay);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeBackgroundColor(originalColor);
        }
    }

    private void StartElevator()
    {
        startMoving = true;
    }

    private void Update()
    {
        if (startMoving && elevator != null)
        {
            elevator.transform.position = Vector3.MoveTowards(
                elevator.transform.position,
                targetPosition,
                speed * Time.deltaTime
            );

            if (elevator.transform.position == targetPosition)
            {
                startMoving = false;
            }
        }
    }

    private void ChangeBackgroundColor(Color color)
    {
        if (mainCamera != null)
            mainCamera.backgroundColor = color;
    }

    private void ExplodeRectangles()
    {
        if (smallRectanglePrefab == null) return;

        for (int i = 0; i < explosionCount; i++)
        {
            Vector3 spawnPos = transform.position + (Vector3)(Random.insideUnitCircle * explosionSpread);

            GameObject rect = Instantiate(smallRectanglePrefab, spawnPos, Quaternion.identity);

            Rigidbody2D rb = rect.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 randomDir = Random.insideUnitCircle.normalized;
                rb.AddForce(randomDir * explosionForce, ForceMode2D.Impulse);
            }
        }
    }
}