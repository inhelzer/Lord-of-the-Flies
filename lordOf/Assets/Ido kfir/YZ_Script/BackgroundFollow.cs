using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    [SerializeField] private Transform cam;        // גרור Main Camera
    [Range(0f, 1f)] public float followX = 0.5f;   // 0=לא זז, 1=זז כמו מצלמה

    private float startCamX;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;   // איפה שמיקמת את האבא בסצנה
        startCamX = cam.position.x;      // איפה שהמצלמה התחילה
    }

    void LateUpdate()
    {
        float camDeltaX = cam.position.x - startCamX;          // כמה המצלמה זזה מאז ההתחלה
        transform.position = startPos + new Vector3(camDeltaX * followX, 0f, 0f);
    }
}