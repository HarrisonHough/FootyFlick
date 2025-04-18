
public class TutorialCarousel : CarouselBase
{
    private GameModeEnum gameMode;

    public void SetGameMode(GameModeEnum gameMode)
    {
        this.gameMode = gameMode;
    }
    
    public void Close()
    {
        GamePrefs.SetTutorialComplete(gameMode);
        gameObject.SetActive(false);
    }
}
