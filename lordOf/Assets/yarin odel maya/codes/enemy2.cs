using UnityEngine;
using UnityEngine.SceneManagement;
public class enemy2 : MonoBehaviour
{
 
    public float speed = 0.3f;
    public float height = 0.2f;
    private float offset;

    void Start()
    {
        offset = Random.Range(0f, 100f);
        Destroy(gameObject, 30f);
    }

    void Update()
    {
        float y = Mathf.Sin((Time.time + offset) * speed) * height;
        transform.position += new Vector3(0.7f * Time.deltaTime, y * Time.deltaTime, 0);
    }

    // הקוד החדש שמוסיף את הפגיעה בשחקן

    private void OnTriggerEnter2D(Collider2D other)
    {
        // בדיקה אם האובייקט שנגענו בו הוא השחקן
        if (other.CompareTag("Player"))
        {
            // הדפסה למסך הבדיקה (Console) כדי לראות שזה עובד
            Debug.Log("The enemy killed the player!");

            // טעינת השלב מחדש
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}