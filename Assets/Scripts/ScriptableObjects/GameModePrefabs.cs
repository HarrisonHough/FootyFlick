using UnityEngine;

[CreateAssetMenu(fileName = "GameModePrefabs", menuName = "ScriptableObjects/GameModePrefabs", order = 1)]
public class GameModePrefabs : ScriptableObject
{
    public TutorialMode TutorialMode;
    public GoalOrNothingMode GoalOrNothingMode;
    public GameModeTimeAttack TimeAttackMode;

    public GameModeBase GetGameMode(GameModeEnum gameModeEnum)
    {
        switch (gameModeEnum)
        {
            case GameModeEnum.Tutorial:
                return TutorialMode;
            case GameModeEnum.GoalOrNothing:
                return GoalOrNothingMode;
            case GameModeEnum.TimeAttack:
                return TimeAttackMode;
            case GameModeEnum.RoundTheWorld:
            case GameModeEnum.InTheBin:
            default:
                Debug.LogError($"Game mode {gameModeEnum} not found.");
                return null;
        }
    }
}
