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
        while (timeElapsed < 3f && !goalScored && !pointScored)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        SetText("Goals are worth 6 points!");
        while (!goalScored)
        {
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
