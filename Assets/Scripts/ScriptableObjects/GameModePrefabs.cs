using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "GameModePrefabs", menuName = "ScriptableObjects/GameModePrefabs", order = 1)]
public class GameModePrefabs : ScriptableObject
{
    [FormerlySerializedAs("practiceMode")] [FormerlySerializedAs("trainingMode"),FormerlySerializedAs("TutorialMode")] public PracticeGameMode practiceGameMode;
    public GoalOrNothingMode GoalOrNothingMode;
    public GameModeTimeAttack TimeAttackMode;

    public GameModeBase GetGameMode(GameModeEnum gameModeEnum)
    {
        switch (gameModeEnum)
        {
            case GameModeEnum.Practice:
                return practiceGameMode;
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
