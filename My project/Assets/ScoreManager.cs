using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int player1Score = 0;
    public int player2Score = 0;
    public TextMesh player1ScoreText; 
    public TextMesh player2ScoreText; 
    void Start()
    {
        UpdateScoreUI();
    }
    public void AddScore(int player, int points)
    {
        if (player == 1)
        {
            player1Score += points;
        }
        else if (player == 2)
        {
            player2Score += points;
        }
        UpdateScoreUI();
    }
    void UpdateScoreUI()
    {
        if (player1ScoreText != null)
        {
            player1ScoreText.text = "Blue: " + player1Score;
        }
        if (player2ScoreText != null)
        {
            player2ScoreText.text = "Red: " + player2Score;
        }
    }
}