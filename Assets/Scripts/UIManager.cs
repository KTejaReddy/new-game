using UnityEngine;
using TMPro; // Make sure you have TextMeshPro imported

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateScore(float score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {Mathf.FloorToInt(score)}";
        }
    }

    public void ToggleGameOverScreen(bool isGameOver)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(isGameOver);
            if (isGameOver && finalScoreText != null)
            {
                finalScoreText.text = $"Final Score: {Mathf.FloorToInt(GameManager.Instance.Score)}";
            }
        }
    }

    public void OnRestartButtonClicked()
    {
        GameManager.Instance.RestartGame();
    }
}
