using TMPro;
using UnityEngine;

public class GameModeButton : MonoBehaviour
{
    [SerializeField]
    private TMP_Text buttonText;

    private void Start()
    {
        buttonText.text = "GAMEMODE: PRACTICE";
        GameManager.OnGameModeChanged += UpdateButtonText;
    }

    private void UpdateButtonText(GameModeEnum gameModeEnum)
    {
        switch (gameModeEnum)
        {
            case GameModeEnum.Practice:
                buttonText.text = "GAMEMODE: PRACTICE";
                break;
            case GameModeEnum.TimeAttack:
                buttonText.text = "GAMEMODE: TIME ATTACK";
                break;
            case GameModeEnum.GoalOrNothing:
                buttonText.text = "GAMEMODE: ONLY SNAGS";
                break;
            case GameModeEnum.InTheBin:
                buttonText.text = "GAMEMODE: IN THE BIN";
                break;
            case GameModeEnum.RoundTheWorld:
                buttonText.text = "GAMEMODE: ROUND THE WORLD";
                break;
            default:
                buttonText.text = "Unknown Mode";
                break;
        }
    }

    private void OnDestroy()
    {
        GameManager.OnGameModeChanged -= UpdateButtonText;
    }
}
