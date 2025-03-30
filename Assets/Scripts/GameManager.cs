using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private RandomPositionInSector randomPositionInSector;

    public static Action OnKickReady;
    public static Action OnGameOver;
    public static Action OnGameStart;
    
    private IGameMode activeMode;
    
    private void Start()
    {
        Ball.OnBallScoreComplete += OnBallScoreComplete;
    }

    private void OnDestroy()
    {
        Ball.OnBallScoreComplete -= OnBallScoreComplete;
    }
    
    private void Update()
    {
        activeMode?.Update(Time.deltaTime);

        if (activeMode != null && activeMode.IsGameOver)
        {
            ShowGameOverScreen();
        }
    }
    
    private void SetupGameMode(GameModeType modeType)
    {
        switch (modeType)
        {
            case GameModeType.GoalOrNothing:
                activeMode = new GoalOrNothingMode(this);
                break;
            case GameModeType.Training:
                activeMode = new TrainingMode(this);
                break;
            case GameModeType.TimeAttack:
                activeMode = new TimeAttackMode(this);
                break;
            case GameModeType.AroundTheWorld:
                activeMode = new AroundTheWorldMode(this);
                break;
        }

        activeMode.StartMode();
    }

    public void StartGame()
    {
        OnGameStart?.Invoke();
        OnKickReady?.Invoke();
        WindControl.Instance.RandomizeWindStrength();
    }

    public void OnBallScoreComplete(BallScoreData ballScoreData)
    {
        if (ballScoreData.kickResult == KickResult.Goal)
        {
            StartCoroutine(DelayMovePlayer(1f));
            return;
        }
        StartCoroutine(DelayGameOver(1f));
    }
    
    public void RestartGame()
    {
        StartGame();
        activeMode?.EndMode();
        SetupGameMode(settings.SelectedMode);
    }

    private IEnumerator DelayMovePlayer(float delay)
    {
        yield return new WaitForSeconds(delay);
        MovePlayerToRandomPosition();
    }

    public void MovePlayerToRandomPosition()
    {
        player.MoveToPosition(randomPositionInSector.GetRandomPositionInSector());
        WindControl.Instance.RandomizeWindStrength();
        OnKickReady?.Invoke();
    }

    private IEnumerator DelayGameOver(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnGameOver?.Invoke();
    }
}

