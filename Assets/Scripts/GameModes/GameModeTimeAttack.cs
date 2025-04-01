using UnityEngine;

public class GameModeTimeAttack : GameModeBase
{
    //[SerializeField] private GameUI_TimeAttack ui;
    [SerializeField] private float timeLimit = 60f;

    private float timer;
    private int score;
    private bool gameOver;

    public override void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public override void StartMode()
    {
        timer = timeLimit;
        score = 0;
        gameOver = false;
        //ui.ShowStart();
        gameManager.MovePlayerToRandomPosition();
        WindControl.Instance.RandomizeWindStrength();
    }

    private void Update()
    {
        if (gameOver) return;

        timer -= Time.deltaTime;
        //ui.UpdateTimer(timer);

        if (timer <= 0f)
        {
            gameOver = true;
            //ui.ShowGameOver(score);
        }
    }

    public override void OnKickResult(KickData kickData)
    {
        if (kickData.Result == KickResult.Goal)
            score += 6;
        else if (kickData.Result == KickResult.Point)
            score += 1;

        gameManager.MovePlayerToRandomPosition();
    }
}