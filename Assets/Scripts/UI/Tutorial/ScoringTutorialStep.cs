using System.Collections;
using UnityEngine;

public class ScoringTutorialStep : TutorialStep
{
    public override IEnumerator TutorialStepLoop()
    {
        ballKicked = false;
        goalScored = false;
        pointScored = false;
        goalScoreWithSnap = false;

        pointScored = false;
        var timeElapsed = 0f;
        while (timeElapsed < 5f && !goalScored && !pointScored)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        SetText("Kick a goal!");
        while (!goalScored)
        {
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
