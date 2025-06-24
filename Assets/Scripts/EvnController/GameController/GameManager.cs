using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int score = 0; // Sửa lỗi chính tả "scrore" -> "score"

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject GameOverUI;

    private bool isGameOver = false;

    void Start()
    {
        UpdateScore();
        GameOverUI.SetActive(false);
    }

    void Update()
    {
        // Có thể kiểm tra game over tại đây nếu cần
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreText.text = score.ToString(); // Sửa gán sai
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        GameOverUI.SetActive(true);
    }
}
