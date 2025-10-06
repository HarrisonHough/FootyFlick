using UnityEngine;

public static class GamePrefs 
{
    private const string GOAL_OR_NOTHING_BEST_SCORE = "OnlySnags_BestScore";
    private const string GOAL_OR_NOTHING_TUTORIAL_COMPLETE = "OnlySnags_Tutorial_Complete";
    private const string TIME_ATTACK_BEST_SCORE = "TimeAttack_Tutorial_BestScore";
    private const string TIME_ATTACK_TUTORIAL_COMPLETE = "TimeAttack_Tutorial_Complete";
    private const string PRACTICE_TUTORIAL_COMPLETE = "Practice_Tutorial_Complete";
    private const string PRACTICE_BEST_SCORE = "Practice_BestScore";
    
    public const string TUTORIAL_COMPLETE_PREFS = "TutorialComplete";
    public static int GetBestScore(GameModeEnum gameMode)
    {
        switch (gameMode)
        {
            case GameModeEnum.GoalOrNothing:
                return PlayerPrefs.GetInt(GOAL_OR_NOTHING_BEST_SCORE, 0);
            case GameModeEnum.TimeAttack:
                return PlayerPrefs.GetInt(TIME_ATTACK_BEST_SCORE, 0);
            case GameModeEnum.Practice:
                return PlayerPrefs.GetInt(PRACTICE_BEST_SCORE, 0);
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
            case GameModeEnum.Practice:
                PlayerPrefs.SetInt(PRACTICE_BEST_SCORE, score);
                break;
        }
        PlayerPrefs.Save();
    }
    
    public static bool GetTutorialComplete(GameModeEnum gameMode)
    {
        switch (gameMode)
        {
            case GameModeEnum.Practice:
                return PlayerPrefs.GetInt(PRACTICE_TUTORIAL_COMPLETE, 0) == 1;
            case GameModeEnum.GoalOrNothing:
                return PlayerPrefs.GetInt(GOAL_OR_NOTHING_TUTORIAL_COMPLETE, 0) == 1;
            case GameModeEnum.TimeAttack:
                return PlayerPrefs.GetInt(TIME_ATTACK_TUTORIAL_COMPLETE, 0) == 1;
            default:
                Debug.LogError($"Game mode {gameMode} not found.");
                return false;
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
        PlayerPrefs.Save();
    }
    
    public static bool IsGameModeUnlocked(GameModeEnum gameMode)
    {
        return gameMode switch
        {
            GameModeEnum.Practice => true,
            GameModeEnum.TimeAttack => GetBestScore(GameModeEnum.Practice) >= 100,
            GameModeEnum.GoalOrNothing => GetBestScore(GameModeEnum.TimeAttack) > 66,
            _ => false
        };
    }
    
    public static bool GetBool(string key)
    {
        return PlayerPrefs.GetInt(key, 0) == 1;
    }
    
    public static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
        PlayerPrefs.Save();
    }
}
