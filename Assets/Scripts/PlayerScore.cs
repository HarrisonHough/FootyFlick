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
        GameManager.OnGameStart += ResetScore;
    }
    
    private void OnDestroy()
    {
        Ball.OnBallScoreComplete -= OnScore;
        BallLauncher.OnBallLaunched -= OnKick;
        GameManager.OnGameStart -= ResetScore;
    }
    
    private void OnKick()
    {
        playerScoreData.kickCount++;
    }

    private void OnScore(BallScoreData ballScoreData)
    {
        switch (ballScoreData.kickResult)
        {
            case KickResult.Goal:
                playerScoreData.goalCount++;
                AddToScore(6);
                break;
            case KickResult.Point:
                playerScoreData.pointCount++;
                AddToScore(1);
                break;
            case KickResult.OutOfBounds:
                playerScoreData.outOfBoundsCount++;
                break;
            case KickResult.None:
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
