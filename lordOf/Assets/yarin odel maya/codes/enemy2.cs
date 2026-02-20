using UnityEngine;
using UnityEngine.SceneManagement;
public class enemy2 : MonoBehaviour
{
 
    public float speed = 100f;
    public float height = 0.0f;
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
          
            // טעינת השלב מחדש
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}