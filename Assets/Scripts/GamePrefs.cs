using UnityEngine;

public static class GamePrefs 
{
    private const string GOAL_OR_NOTHING_BEST_SCORE = "OnlySnags_BestScore";
    private const string GOAL_OR_NOTHING_TUTORIAL_COMPLETE = "OnlySnags_Tutorial_Complete";
    private const string TIME_ATTACK_BEST_SCORE = "TimeAttack_Tutorial_Complete";
    private const string TIME_ATTACK_TUTORIAL_COMPLETE = "TimeAttack_Tutorial_Complete";
    private const string PRACTICE_TUTORIAL_COMPLETE = "Practice_Tutorial_Complete";

    
    public static int GetBestScore(GameModeEnum gameMode)
    {
        switch (gameMode)
        {
            case GameModeEnum.GoalOrNothing:
                return PlayerPrefs.GetInt(GOAL_OR_NOTHING_BEST_SCORE, 0);
            case GameModeEnum.TimeAttack:
                return PlayerPrefs.GetInt(TIME_ATTACK_BEST_SCORE, 0);
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
            case GameModeEnum.TimeAttack:
                PlayerPrefs.SetInt(TIME_ATTACK_BEST_SCORE, score);
                break;
        }
    }
    
    public static bool GetTutorialComplete(GameModeEnum gameMode)
    {
        switch (gameMode)
        {
            case GameModeEnum.Practice:
                return PlayerPrefs.GetInt(PRACTICE_TUTORIAL_COMPLETE, 0) == 1;
                break;
            case GameModeEnum.GoalOrNothing:
                return PlayerPrefs.GetInt(GOAL_OR_NOTHING_TUTORIAL_COMPLETE, 0) == 1;
                break;
            case GameModeEnum.TimeAttack:
                return PlayerPrefs.GetInt(TIME_ATTACK_TUTORIAL_COMPLETE, 0) == 1;
                break;
            default:
                Debug.LogError($"Game mode {gameMode} not found.");
                return false;
                break;
        }
    }

    public static void SetTutorialComplete(GameModeEnum gameMode)
    {
        
        switch (gameMode)
        {
            case GameModeEnum.Practice:
                PlayerPrefs.SetInt(PRACTICE_TUTORIAL_COMPLETE, 1);
                break;
            case GameModeEnum.GoalOrNothing:
                PlayerPrefs.SetInt(GOAL_OR_NOTHING_TUTORIAL_COMPLETE, 1);
                break;
            case GameModeEnum.TimeAttack:
                PlayerPrefs.SetInt(TIME_ATTACK_TUTORIAL_COMPLETE, 1);
                break;
        }
    }
}
