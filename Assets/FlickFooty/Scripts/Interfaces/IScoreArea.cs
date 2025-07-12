using System;

[Serializable]
public enum KickResult
{
    None,
    OutOfBounds,
    Goal,
    Point,
    HitGoalPost,
    HitPointPost,
    BinGoal
}

public interface IScoreArea
{
    public KickResult KickResult { get; }
}
