
using UnityEngine;

public abstract class GameModeBase : MonoBehaviour
{
    protected GameManager gameManager;
    
    [SerializeField]
    protected TutorialCarousel tutorialPanel;

    [SerializeField] protected GameObject gameCanvasObject;
    
    [SerializeField]
    protected GameModeEnum gameMode;
    

    public abstract void StartMode();
    public abstract void OnKickResult(KickData kickData);
    public virtual void EndMode() { }
    
    public virtual void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
        gameCanvasObject.SetActive(false);
        tutorialPanel.SetGameMode(gameMode);
        var hasCompletedTutorial = GamePrefs.GetTutorialComplete(gameMode);
        tutorialPanel.gameObject.SetActive(!hasCompletedTutorial);
        if (hasCompletedTutorial) return;
        SetPaused(true);
    }
    
    public virtual void SetPaused(bool paused)
    {
        gameManager.SetSwipeDisabled(paused);
    }
}
