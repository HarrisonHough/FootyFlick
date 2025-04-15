using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "GameModePrefabs", menuName = "ScriptableObjects/GameModePrefabs", order = 1)]
public class GameModePrefabs : ScriptableObject
{
    [FormerlySerializedAs("trainingMode"),FormerlySerializedAs("TutorialMode")] public PracticeMode practiceMode;
    public GoalOrNothingMode GoalOrNothingMode;
    public GameModeTimeAttack TimeAttackMode;

    public GameModeBase GetGameMode(GameModeEnum gameModeEnum)
    {
        switch (gameModeEnum)
        {
            case GameModeEnum.Practice:
                return practiceMode;
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
