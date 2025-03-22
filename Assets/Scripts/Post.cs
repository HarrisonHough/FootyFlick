using UnityEngine;

public class GoalPost : MonoBehaviour, IGoalPost
{
    [SerializeField]
    private GoalPostType goalPostType;

    public GoalPostType GoalPostType => goalPostType;
}
