using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private TutorialStep kickTutorial;
    [SerializeField]
    private TutorialStep behindTutorial;
    [SerializeField]
    private TutorialStep goalTutorial;
    [SerializeField]
    private TutorialStep windTutorial;
    [SerializeField]
    private TutorialStep kickStyleTutorial;
    [SerializeField]
    private TutorialStep exitPracticeTutorial;
    [SerializeField]
    private WindControl windControl;
    [SerializeField]
    private WindPanel windPanel;
    [SerializeField]
    private Transform tightAnglePosition;
    [SerializeField]
    private BallLauncher ballLauncher;
    [SerializeField]
    private PlayerController playerController;
    
    private bool ballKicked;
    private bool goalScored;
    private bool pointScored;
    private bool goalScoreWithSnap;
    
    private void Start()
    {
        kickTutorial.gameObject.SetActive(true);
        exitPracticeTutorial.gameObject.SetActive(false);
        behindTutorial.gameObject.SetActive(false);
        goalTutorial.gameObject.SetActive(false);
        windTutorial.gameObject.SetActive(false);
        kickStyleTutorial.gameObject.SetActive(false);
        windPanel.gameObject.SetActive(false);
        StartCoroutine(TutorialLoop());
        
        Ball.OnKickComplete += OnKickComplete;
    }
    
    private void OnDestroy()
    {
        Ball.OnKickComplete -= OnKickComplete;
    }
    
    private void OnKickComplete(KickData kickData)
    {
        ballKicked = true;
        if( kickData.Result == KickResult.Goal)
        {
            goalScored = true;
        }
        else if (kickData.Result == KickResult.Point)
        {
            pointScored = true;
        }
        
        goalScoreWithSnap = kickData.Style is KickStyle.SnapLeft or KickStyle.SnapRight && kickData.Result == KickResult.Goal;
        StartCoroutine( DelayNextBallSpawn(1f));
    }

    
    private IEnumerator TutorialLoop()
    {
        yield return KickTutorial();
        yield return BehindTutorial();
        yield return GoalTutorial();
        yield return WindTutorial();
        yield return KickStyleTutorial();
        exitPracticeTutorial.gameObject.SetActive(true);
        exitPracticeTutorial.SetText("Tap the home icon to exit the tutorial");
    }
    
    private IEnumerator KickTutorial()
    {
        ballKicked = false;
        ballLauncher.KickReady();
        while (!ballKicked)
        {
            yield return null;
        }
        kickTutorial.gameObject.SetActive(false);
    }

    private IEnumerator BehindTutorial()
    {
        behindTutorial.gameObject.SetActive(true);
        pointScored = false;
        var timeElapsed = 0f;
        while (timeElapsed < 3f && !goalScored && !pointScored)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        behindTutorial.SetText("Kick the ball between the outer posts to score 1 point!");
        while (!pointScored)
        {
            yield return null;
        }
        behindTutorial.gameObject.SetActive(false);
    }
    
    private IEnumerator GoalTutorial()
    {
        behindTutorial.gameObject.SetActive(true);
        goalScored = false;
        pointScored = false;
        behindTutorial.SetText("Kick the ball between the big posts to score 6 points!");
        while (!goalScored)
        {
            yield return null;
        }
        behindTutorial.gameObject.SetActive(false);
    }

    private IEnumerator WindTutorial()
    {
        windPanel.gameObject.SetActive(true);
        windTutorial.gameObject.SetActive(true);
        windControl.SetWindStrength(3f);
        windTutorial.SetText("Adjust your kick to account for the wind!");
        goalScored = false;
        while (!goalScored)
        {
            yield return null;
        }
        windTutorial.gameObject.SetActive(false);
        windControl.SetWindStrength(0);
        windPanel.gameObject.SetActive(false);
    }


    private IEnumerator KickStyleTutorial()
    {
        kickStyleTutorial.gameObject.SetActive(true);
        ballKicked = false;
        playerController.MoveToPosition(tightAnglePosition.position);
        kickStyleTutorial.SetText("Tap the ball to change kicking styles!");
        while (!goalScoreWithSnap)
        {
            if (ballKicked)
            {
                ballKicked = false;
                kickStyleTutorial.SetText("Tap the ball to change kicking styles!");
            }
            if (ballLauncher.currentKickStyle != KickStyle.DropPunt)
            {
                kickStyleTutorial.SetText("Kick a goal to continue!");
            }
            yield return null;
        }
        kickStyleTutorial.gameObject.SetActive(false);
    }

    private IEnumerator DelayNextBallSpawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        ballLauncher.KickReady();
    }

    public void GoToGameScene()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(2);
    }
}
