using System;

[Serializable]
public enum ScoreType
{
    None,
    OutOfBounds,
    Goal,
    Point
}

public interface IScoreArea
{
    public ScoreType ScoreType { get; }
}
