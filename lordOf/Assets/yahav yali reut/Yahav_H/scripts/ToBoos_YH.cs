using UnityEngine;
using UnityEngine.SceneManagement;

public class ToBoos_YH : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LoadBossScene();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LoadBossScene();
        }
    }

    void LoadBossScene()
    {
        SceneManager.LoadScene("Boss_YH");
    }
}
