using System;
using UnityEngine;

public struct PlayerScoreData
{
    public int Score;
    public int KickCount;
    public float Accuracy;
    public int GoalCount;
    public int PointCount;
    public int OutOfBoundsCount;
    public int HitGoalPostCount;
    public int HitPointPostCount;
    public int BinGoalCount;
    public int DropPuntKickCount;
    public int BananaKickCount;
}

public class PlayerScore : MonoBehaviour
{
    private PlayerScoreData playerScoreData;
    
    public static Action<PlayerScoreData> OnScoreUpdated;
    
    private void Start()
    {
        playerScoreData = new PlayerScoreData();
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameStateEnum gameState)
    {
        if (gameState == GameStateEnum.GameStarted)
        {
            ResetScore();
        }
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }
    
    public void AddKickData(KickData kickData)
    {
        playerScoreData.KickCount++;
        switch (kickData.Result)
        {
            case KickResult.Goal:
                playerScoreData.GoalCount++;
                AddToScore(6);
                break;
            case KickResult.Point:
                playerScoreData.PointCount++;
                AddToScore(1);
                break;
            case KickResult.OutOfBounds:
                playerScoreData.OutOfBoundsCount++;
                break;
            case KickResult.HitGoalPost:
                playerScoreData.HitGoalPostCount++;
                break;
            case KickResult.HitPointPost:
                playerScoreData.HitPointPostCount++;
                break;
            case KickResult.None:
                break;
        }
        playerScoreData.Accuracy = (float)playerScoreData.GoalCount / playerScoreData.KickCount * 100f;

        switch (kickData.Style)
        {
            case KickStyle.DropPunt:
                playerScoreData.DropPuntKickCount++;
                break;
            case KickStyle.SnapLeft:case KickStyle.SnapRight:
                playerScoreData.BananaKickCount++;
                break;
            default:
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
        playerScoreData.Score += score;
        OnScoreUpdated?.Invoke(GetScoreData);
    }

    public void ResetScore()
    {
        GetScoreData = new PlayerScoreData();
        OnScoreUpdated?.Invoke(GetScoreData);
    }
}
