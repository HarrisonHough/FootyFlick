using UnityEngine;
using UnityEngine.Serialization;


public class ScoreArea : MonoBehaviour, IScoreArea
{
    public KickResult kickResult;
    public KickResult KickResult
    {
        get => kickResult;
        private set => kickResult = value;
    }

    
}
