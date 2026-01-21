using UnityEngine;

public class yarink_ana : MonoBehaviour
{
    public float amplitude = 0.2f;   // כמה האובייקט זז למעלה ולמטה
    public float frequency = 1f;     // כמה מהר האובייקט מתנדנד

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // שומרים את המיקום ההתחלתי
    }

    void Update()
    {
        // תנועה קטנה למעלה ולמטה
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPos.x, startPos.y + yOffset, startPos.z);
    }
}

