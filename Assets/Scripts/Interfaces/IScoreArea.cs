using System;

[Serializable]
public enum KickResult
{
    None,
    OutOfBounds,
    Goal,
    Point
}

public interface IScoreArea
{
    public KickResult KickResult { get; }
}
