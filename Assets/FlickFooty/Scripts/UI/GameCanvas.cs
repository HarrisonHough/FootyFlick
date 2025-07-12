using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    [SerializeField]
    private WindPanel windPanel;

    private void Start()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameStateEnum gameState)
    {
        switch (gameState)
        {
            case GameStateEnum.GameStarted:
                OnGameStart();
                break;
            case GameStateEnum.GameOver:
                OnGameOver();
                break;
            default:
                break;
        }
    }

    public void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }
    private void OnGameOver()
    {
        SetUIVisibility(false);
    }

    private void SetUIVisibility(bool isVisible)
    {
        windPanel.gameObject.SetActive(isVisible);
    }
    
    public void HideWindPanel()
    {
        windPanel.gameObject.SetActive(false);
    }
    
    public void ShowWindPanel()
    {
        windPanel.gameObject.SetActive(true);
    }

    private void OnGameStart()
    {
        ResetUI();
    }
    
    private void ResetUI()
    {
        Debug.Log("Resetting UI");
        SetUIVisibility(true);
    }
}
