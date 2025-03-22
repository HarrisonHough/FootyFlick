using UnityEngine;
using UnityEngine.Serialization;


public class ScoreArea : MonoBehaviour, IScoreArea
{
    [FormerlySerializedAs("scoringType")] [FormerlySerializedAs("scoringAreaType")] [SerializeField]
    private ScoreType scoreType;

    public ScoreType ScoreType
    {
        get => scoreType;
        private set => scoreType = value;
    }
}
