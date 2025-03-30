using UnityEngine;

public class GoalOrNothingMode : IGameMode
{
    private GameManager game;
    private int score = 0;
    private bool gameOver = false;

    public GoalOrNothingMode(GameManager gm) => game = gm;

    public void StartMode()
    {
        score = 0;
        game.MovePlayerToRandomPosition();
        WindControl.Instance.RandomizeWindStrength();
    }

    public void OnKickResult(KickResult result)
    {
        if (result == KickResult.Goal)
            score += 6;
        else if (result == KickResult.Point)
            score += 1;
        else
            gameOver = true;

        if (!gameOver)
        {
            game.MovePlayerToRandomPosition();
            WindControl.Instance.RandomizeWindStrength();
        }
    }

    public void Update(float deltaTime) { }

    public bool IsGameOver => gameOver;

    public string GetStatusText() => $"Score: {score}";

    public void EndMode() { }
}
