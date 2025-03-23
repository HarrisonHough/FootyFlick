using UnityEngine;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField]
    private GameOverPanel gameOverPanel;

    [SerializeField]
    private GameObject optionsPanel;

    [SerializeField] private GameObject homePanel;

    private void Start()
    {
        GameController.OnGameOver += OnGameOver;
        GameController.OnGameStart += OnGameStart;
        gameOverPanel.gameObject.SetActive(false);
        optionsPanel.SetActive(false);
        homePanel.SetActive(true);
    }
    
    private void OnDestroy()
    {
        GameController.OnGameOver -= OnGameOver;
        GameController.OnGameStart -= OnGameStart;
    }
    
    private void OnGameOver()
    {
        gameOverPanel.gameObject.SetActive(true);
    }
    
    private void OnGameStart()
    {
        gameOverPanel.gameObject.SetActive(false);
        homePanel.SetActive(false);
    }
}
