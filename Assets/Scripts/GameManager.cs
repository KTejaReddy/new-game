using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGameActive { get; private set; }
    public float CurrentSpeed { get; private set; }
    public float Score { get; private set; }

    [Header("Game Settings")]
    public float baseSpeed = 20f;
    public float speedMultiplier = 0.5f;
    public float maxSpeed = 100f;

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

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        IsGameActive = true;
        Score = 0f;
        CurrentSpeed = baseSpeed;
        UIManager.Instance.UpdateScore(Score);
        UIManager.Instance.ToggleGameOverScreen(false);
    }

    private void Update()
    {
        if (!IsGameActive) return;

        // Increase score based on distance
        Score += Time.deltaTime * (CurrentSpeed / 5f);
        
        // Increase speed over time for difficulty
        if (CurrentSpeed < maxSpeed)
        {
            CurrentSpeed += Time.deltaTime * speedMultiplier;
        }

        UIManager.Instance.UpdateScore(Score);
    }

    public void AddScore(float amount)
    {
        if (!IsGameActive) return;
        Score += amount;
        UIManager.Instance.UpdateScore(Score);
    }

    public void GameOver()
    {
        IsGameActive = false;
        CurrentSpeed = 0f;
        UIManager.Instance.ToggleGameOverScreen(true);
    }

    public void RestartGame()
    {
        // Simple restart by reloading scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
