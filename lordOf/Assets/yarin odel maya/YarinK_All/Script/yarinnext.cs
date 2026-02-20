using UnityEngine;
using UnityEngine.SceneManagement;

public class yarinnext : MonoBehaviour
{
    public string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            LoadNextLevel();
        }
    }

    void LoadNextLevel()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(current + 1);
    }
}

