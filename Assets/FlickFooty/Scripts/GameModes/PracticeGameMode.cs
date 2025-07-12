using System.Collections;
using UnityEngine;

public class PracticeGameMode : GameModeBase
{
    [SerializeField]
    private PracticeGameOverPanel gameOverPanel;
    private PlayerScore playerScore;
    
    public override void Initialize(GameManager gameManager)
    {        
        base.Initialize(gameManager);
        playerScore = this.gameManager.GetPlayerScore();
        Ball.OnKickComplete += OnKickResult;
        gameOverPanel.OnHomeButtonClicked += OnHomeButtonClicked;
        WindControl.Instance.RandomizeWindStrength();
    }

    private void OnHomeButtonClicked()
    {
        gameOverPanel.gameObject.SetActive(false);
        GameManager.SetGameState(GameStateEnum.Home);
    }

    public override void StartMode()
    {
        gameCanvasObject.SetActive(true);
        GameManager.SetGameState(GameStateEnum.GameKicking);
    }

    public override void OnKickResult(KickData kickData)
    {
        playerScore.AddKickData(kickData);
        StartCoroutine(StartNextKick(1f));
    }

    private IEnumerator StartNextKick(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameManager.MovePlayerToRandomPosition();
        WindControl.Instance.RandomizeWindStrength();
        GameManager.SetGameState(GameStateEnum.GameKicking);
    }

    public override void EndMode()
    {
        Ball.OnKickComplete -= OnKickResult;
    }
    
    public void GameOver()
    {
        gameOverPanel.gameObject.SetActive(true);
        GameManager.SetGameState(GameStateEnum.GameOver);
        gameOverPanel.UpdateText(playerScore.GetScoreData);
    }
}
