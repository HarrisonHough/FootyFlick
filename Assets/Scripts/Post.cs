using UnityEngine;
public enum GoalPostCollisionType
{
    None,
    Goal,
    Point
}

public class GoalPost : MonoBehaviour
{
    [SerializeField]
    private GoalPostCollisionType goalPostCollisionType = GoalPostCollisionType.Goal;
    
    public GoalPostCollisionType CollisionType => goalPostCollisionType;
    
}
