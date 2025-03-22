using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private RandomPositionInSector randomPositionInSector;
    
    private void Start()
    {
        Ball.OnBallScoreComplete += OnBallScoreComplete;
    }
    
    public void OnBallScoreComplete(BallScoreData ballScoreData)
    {
        player.MoveToPosition(randomPositionInSector.GetRandomPositionInSector());
        WindControl.Instance.RandomizeWindStrength();
        if(ballScoreData.scoreType == ScoreType.Goal)
        {
            
        }
    }

    IEnumerator GameLoop()
    {
        yield return null;
    }
}

