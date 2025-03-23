using System;
using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private RandomPositionInSector randomPositionInSector;

    public static Action OnKickReady;
    public static Action OnGameOver;
    public static Action OnGameStart;
    
    private void Start()
    {
        Ball.OnBallScoreComplete += OnBallScoreComplete;
        StartGame();
    }

    private void OnDestroy()
    {
        Ball.OnBallScoreComplete -= OnBallScoreComplete;
    }

    private void StartGame()
    {
        OnGameStart?.Invoke();
        MovePlayer();
    }

    public void OnBallScoreComplete(BallScoreData ballScoreData)
    {
        if (ballScoreData.scoreType == ScoreType.Goal)
        {
            StartCoroutine(DelayMovePlayer(1f));
            return;
        }
        OnGameOver?.Invoke();
    }
    
    public void RestartGame()
    {
        StartGame();
    }

    private IEnumerator DelayMovePlayer(float delay)
    {
        yield return new WaitForSeconds(delay);
        MovePlayer();
    }

    private void MovePlayer()
    {
        player.MoveToPosition(randomPositionInSector.GetRandomPositionInSector());
        WindControl.Instance.RandomizeWindStrength();
        OnKickReady?.Invoke();
    }
}

