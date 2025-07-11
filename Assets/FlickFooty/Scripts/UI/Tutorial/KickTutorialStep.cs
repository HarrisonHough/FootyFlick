using System.Collections;

public class KickTutorialStep : TutorialStep
{
    public override IEnumerator TutorialStepLoop()
    {
        ballKicked = false;
        while (!ballKicked)
        {
            yield return null;
        }
        gameObject.SetActive(false);
    }
    
}
