using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement; // נוסף בשביל מעבר סצנות

public class GameManeger : MonoBehaviour
{
    public static GameManeger Instance;

    public int score = 0;
    public bool isPaused = false;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Scene Management")]
    [Tooltip("הכנס כאן את שם הסצנה שאליה תרצה לעבור")]
    [SerializeField] private string winSceneName; // משתנה לבחירת שם הסצנה

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

        if (score % 10 == 0 && score < 50) // שונה ל- < כדי שלא יפעיל פאוז ב-50
        {
            StartCoroutine(PauseGame());
        }

        if (score >= 50)
        {
            Debug.Log("YOU WIN!");
            SceneManager.LoadScene(6); // מעבר לסצנה שבחרת
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
