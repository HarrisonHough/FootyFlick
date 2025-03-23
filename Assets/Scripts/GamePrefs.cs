using UnityEngine;

public enum GameModeEnum
{
    GoalOrNothing
}

public static class GamePrefs 
{
    private const string GOAL_OR_NOTHING_BEST_SCORE = "GoalOrNothing_BestScore";
    
    public static int GetBestScore(GameModeEnum gameMode)
    {
        switch (gameMode)
        {
            case GameModeEnum.GoalOrNothing:
                return PlayerPrefs.GetInt(GOAL_OR_NOTHING_BEST_SCORE, 0);;
            default:
                return 0;
        }
    }
    
    public static void SetBestScore(GameModeEnum gameMode, int score)
    {
        switch (gameMode)
        {
            case GameModeEnum.GoalOrNothing:
                PlayerPrefs.SetInt(GOAL_OR_NOTHING_BEST_SCORE, score);
                break;
        }
    }
}
