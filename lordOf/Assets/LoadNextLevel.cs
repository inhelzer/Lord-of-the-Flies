using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{
    [SerializeField] string scene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("player"))
        {
            SceneManager.LoadScene(scene);
        }
    }
}
