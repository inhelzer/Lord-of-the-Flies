using UnityEngine;
using System.Collections;
using TMPro;

public class GameManeger : MonoBehaviour
{
    public static GameManeger Instance;

    public int score = 0;
    public bool isPaused = false;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore()
    {
        score++;

        UpdateScoreUI();

        if (score % 10 == 0 && score <= 50)
        {
            StartCoroutine(PauseGame());
        }

        if (score >= 50)
        {
            Debug.Log("YOU WIN!");
            Time.timeScale = 0f;
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    IEnumerator PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1f;
        isPaused = false;
    }
}