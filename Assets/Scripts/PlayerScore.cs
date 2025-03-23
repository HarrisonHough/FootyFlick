using System;
using UnityEngine;

public struct PlayerScoreData
{
    public int score;
    public int kickCount;
    public float accuracy;
    public int goalCount;
    public int pointCount;
    public int outOfBoundsCount;
}

public class PlayerScore : MonoBehaviour
{
    private PlayerScoreData playerScoreData;
    
    public static Action<PlayerScoreData> OnScoreUpdated;
    
    private void Start()
    {
        playerScoreData = new PlayerScoreData();
        Ball.OnBallScoreComplete += OnScore;
        BallLauncher.OnBallLaunched += OnKick;
        GameController.OnGameStart += ResetScore;
    }
    
    private void OnDestroy()
    {
        Ball.OnBallScoreComplete -= OnScore;
        BallLauncher.OnBallLaunched -= OnKick;
        GameController.OnGameStart -= ResetScore;
    }
    
    private void OnKick()
    {
        playerScoreData.kickCount++;
    }

    private void OnScore(BallScoreData ballScoreData)
    {
        switch (ballScoreData.scoreType)
        {
            case ScoreType.Goal:
                playerScoreData.goalCount++;
                AddToScore(6);
                break;
            case ScoreType.Point:
                playerScoreData.pointCount++;
                AddToScore(1);
                break;
            case ScoreType.OutOfBounds:
                playerScoreData.outOfBoundsCount++;
                break;
            case ScoreType.None:
                break;
        }
    }

    public PlayerScoreData GetScoreData
    {
        get => playerScoreData;
        set => playerScoreData = value;
    }

    public void AddToScore(int score)
    {
        playerScoreData.score += score;
        OnScoreUpdated.Invoke(GetScoreData);
    }

    public void ResetScore()
    {
        GetScoreData = new PlayerScoreData();
        OnScoreUpdated.Invoke(GetScoreData);
    }
}
