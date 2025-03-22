using System;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField]
    private int score;
    
    public static Action<int> OnScoreUpdated;

    private void Start()
    {
        Ball.OnScore += OnScore;
    }

    private void OnScore(BallScoreData ballScoreData)
    {
        if(ballScoreData.scoreType == ScoreType.Point)
        {
            AddScore(1);
        }
        else if (ballScoreData.scoreType == ScoreType.Goal)
        {
            AddScore(6);
        }
    }

    public int Score
    {
        get => score;
        set => score = value;
    }

    public void AddScore(int score)
    {
        Score += score;
        OnScoreUpdated.Invoke(Score);
    }

    public void ResetScore()
    {
        Score = 0;
        OnScoreUpdated.Invoke(Score);
    }
    
    
}
