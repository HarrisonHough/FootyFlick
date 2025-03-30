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
        GameManager.OnGameOver += OnGameOver;
        GameManager.OnGameStart += OnGameStart;
        gameOverPanel.gameObject.SetActive(false);
        optionsPanel.SetActive(false);
        homePanel.SetActive(true);
    }
    
    private void OnDestroy()
    {
        GameManager.OnGameOver -= OnGameOver;
        GameManager.OnGameStart -= OnGameStart;
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
