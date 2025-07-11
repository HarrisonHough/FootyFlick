using System.Collections;
using UnityEngine;

public class KickStyleTutorialStep : TutorialStep
{
    [SerializeField]
    private BallLauncher ballLauncher;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private Transform tightAnglePosition;
    
    public override IEnumerator TutorialStepLoop()
    {
        gameObject.SetActive(true);
        ballKicked = false;
        goalScoreWithSnap = false;
        playerController.MoveToPosition(tightAnglePosition.position);
        SetText("Tap the ball to change kicking styles!");
        while (!goalScoreWithSnap)
        {
            if (ballKicked)
            {
                ballKicked = false;
                SetText("Tap the ball to change kicking styles!");
            }
            if (ballLauncher.currentKickStyle != KickStyle.DropPunt)
            {
                SetText("Kick a goal to continue!");
            }
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
