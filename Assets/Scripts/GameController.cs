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
    }

    private void OnDestroy()
    {
        Ball.OnBallScoreComplete -= OnBallScoreComplete;
    }

    public void StartGame()
    {
        OnGameStart?.Invoke();
        OnKickReady?.Invoke();
        WindControl.Instance.RandomizeWindStrength();
    }

    public void OnBallScoreComplete(BallScoreData ballScoreData)
    {
        if (ballScoreData.scoreType == ScoreType.Goal)
        {
            StartCoroutine(DelayMovePlayer(1f));
            return;
        }
        StartCoroutine(DelayGameOver(1f));
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

    private IEnumerator DelayGameOver(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnGameOver?.Invoke();
    }
}

