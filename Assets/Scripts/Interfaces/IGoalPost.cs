public enum GoalPostType
{
    None,
    Goal,
    Point
}

public interface IGoalPost 
{
    public GoalPostType GoalPostType { get; }
}
