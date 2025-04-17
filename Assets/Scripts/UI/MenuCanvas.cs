using UnityEngine;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField]
    private GameObject optionsPanel;

    [SerializeField] private GameObject homePanel;

    private void Start()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
        optionsPanel.SetActive(false);
        homePanel.SetActive(true);
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
