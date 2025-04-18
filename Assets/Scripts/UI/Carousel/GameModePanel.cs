using UnityEngine;
using UnityEngine.Events;

public class GameModePanel : CarouselElement
{
    [SerializeField] private GameModeEnum gameMode;
    
    public UnityEvent<GameModeEnum> OnGameModeSelected;
    
    public void SelectGameMode()
    {
        OnGameModeSelected?.Invoke(gameMode);
    }
}
