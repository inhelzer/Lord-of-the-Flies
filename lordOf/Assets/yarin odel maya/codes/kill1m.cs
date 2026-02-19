using UnityEngine;
using UnityEngine.SceneManagement; // השורה הזו הייתה חסרה!

public class kill1m : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        // בודק אם האובייקט שנגע בי הוא השחקן
        if (other.gameObject.CompareTag("Player"))
        {
            // טוען מחדש את השלב הנוכחי
            SceneManager.LoadScene("maya1");

            // אם אתה רוצה גם להעלים את השחקן באותו רגע:
            Destroy(other.gameObject);
        }
    }
}