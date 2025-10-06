using System.Collections;
using UnityEngine;

public class GoalFeedback : MonoBehaviour
{
    [SerializeField]
    private GameObject goalIndicator;
    [SerializeField]
    private GameObject pointLeft;
    [SerializeField]
    private GameObject pointRight;
    [SerializeField]
    private float indicatorDuration = 1.5f;
    private void Start()
    {
        Ball.OnKickComplete += OnKickComplete;
        HideIndicators();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        Ball.OnKickComplete -= OnKickComplete;
    }

    private void OnKickComplete(KickData kickData)
    {
        switch (kickData.Result)
        {
            case KickResult.Goal:
                goalIndicator.SetActive(true);
                break;
            case KickResult.Point when kickData.CollisionPoint.x < 0:
                pointLeft.SetActive(true);
                break;
            case KickResult.Point:
                pointRight.SetActive(true);
                break;
            default:
                return;
        }

        StartCoroutine(DelayHideIndicators());
    }
    
    private void HideIndicators()
    {
        goalIndicator.SetActive(false);
        pointLeft.SetActive(false);
        pointRight.SetActive(false);
    }

    private IEnumerator DelayHideIndicators()
    {    
        yield return new WaitForSeconds(indicatorDuration);
        HideIndicators();
    }
}
