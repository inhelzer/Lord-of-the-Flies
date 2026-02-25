using UnityEngine;
using System.Collections;

public class RealisticAxeSlam : MonoBehaviour
{
    [Header("הגדרות זווית")]
    public float startAngle = 0f;
    public float fallenAngle = -90f;

    [Header("זמנים")]
    public float waitTimeUp = 0.5f;
    public float fallDuration = 0.2f; // תוך כמה זמן הוא מסיים את הנפילה (קצר = מהיר)
    public float returnDuration = 0.6f; // חזרה איטית יותר נראית טוב יותר

    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, startAngle);
        StartCoroutine(SlamRoutine());
    }

    IEnumerator SlamRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTimeUp);

            // נפילה עם האצה (נראה כמו כוח משיכה)
            yield return RotateSmooth(startAngle, fallenAngle, fallDuration, true);

            // עצירה קטנה למטה להדגשת המכה
            yield return new WaitForSeconds(0.15f);

            // חזרה למעלה (תנועה חלקה)
            yield return RotateSmooth(fallenAngle, startAngle, returnDuration, false);
        }
    }

    IEnumerator RotateSmooth(float fromAngle, float toAngle, float duration, bool isFalling)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;

            // כאן קורה הקסם:
            // אם זו נפילה - נשתמש ב-InQuart (מתחיל לאט ומאיץ מאוד)
            // אם זו חזרה - נשתמש ב-SmoothStep (תנועה נעימה)
            float curve = isFalling ? percent * percent * percent : Mathf.SmoothStep(0, 1, percent);

            float currentAngle = Mathf.LerpAngle(fromAngle, toAngle, curve);
            transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, 0, toAngle);
    }
}