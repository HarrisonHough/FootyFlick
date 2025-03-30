using System;
using UnityEngine.Serialization;

[Serializable]
public struct BallScoreData
{
    [FormerlySerializedAs("scoreType")] public KickResult kickResult;
    public GoalPostType goalPostCollisionType;
}
