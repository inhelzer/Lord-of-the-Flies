using UnityEngine;
using UnityEngine.SceneManagement;

public class yarindie : MonoBehaviour
{

    public Transform playerStartPoint; // נקודת התחלה
    public float deathDelay = 2f;      // זמן לפני ההחזרה
    private bool isResetting = false;  // מונע הפעלה כפולה

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isResetting)
        {
            isResetting = true;
            Invoke("ResetPlayer", deathDelay);
        }
    }

    private void ResetPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // מחזירים את השחקן למיקום ההתחלה
            player.transform.position = playerStartPoint.position;
            player.transform.rotation = playerStartPoint.rotation;

            // אם יש Rigidbody, איפוס המהירות
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;   // אם הוא Kinematic, נבטל תנועות רגעיות
                rb.isKinematic = false;  // ומחזירים למצב רגיל
            }

            // אם יש CharacterController, נבטל תנועה זמנית
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false;
                cc.enabled = true;
            }
        }

        isResetting = false;
        Debug.Log("Player reset to start point!");
    }
}