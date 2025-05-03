using UnityEngine;
using UnityEngine.Events;

public class GameModePanel : CarouselElement
{
    [SerializeField] private GameModeEnum gameMode;
    [SerializeField] 
    private GameObject unlockedPanel;
    [SerializeField] 
    private GameObject lockedPanel;
    public UnityEvent<GameModeEnum> OnGameModeSelected;

    private void OnEnable()
    {
        if (GamePrefs.IsGameModeUnlocked(gameMode))
        {
            unlockedPanel.SetActive(true);
            lockedPanel.SetActive(false);
        }
        else
        {
            unlockedPanel.SetActive(false);
            lockedPanel.SetActive(true);
        }
    }

    public void SelectGameMode()
    {
        OnGameModeSelected?.Invoke(gameMode);
    }
}
