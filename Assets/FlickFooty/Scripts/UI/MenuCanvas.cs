using UnityEngine;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject homePanel;
    [SerializeField] private GameObject SelectGameModePanel;
    [SerializeField] private GameObject playButton;
    
    private const string HAS_SHOWN_GAME_MODE_TUTORIAL = "HasShownGameModeTutorial";
    
    private void Start()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
        optionsPanel.SetActive(false);
        homePanel.SetActive(true);
        SelectGameModePanel.SetActive(false);
        if (!GamePrefs.GetBool(HAS_SHOWN_GAME_MODE_TUTORIAL))
        {
            playButton.SetActive(false);
            SelectGameModePanel.SetActive(true);
            GamePrefs.SetBool(HAS_SHOWN_GAME_MODE_TUTORIAL, true);
        }
    }

    private void OnGameStateChanged(GameStateEnum gameState)
    {
        switch (gameState)
        {
            case GameStateEnum.GameStarted:
                optionsPanel.SetActive(false);
                homePanel.SetActive(false);
                break;
            case GameStateEnum.Home:
                optionsPanel.SetActive(false);
                homePanel.SetActive(true);
                break;
            default:
                break;
        }
        playButton.SetActive(true);
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }
    
    private void OnGameStart()
    {
        homePanel.SetActive(false);
    }
}
