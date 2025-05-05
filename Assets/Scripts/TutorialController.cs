using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private TutorialStep exitPracticeTutorial;
    [SerializeField]
    private BallLauncher ballLauncher;
    
    [SerializeField]
    private TutorialStep[] tutorialSteps;
    private TutorialStep currentStep;
    
    private void Start()
    { 
        for(var i = 1; i < tutorialSteps.Length; i++)
        { 
            tutorialSteps[i].gameObject.SetActive(false);
        }
        tutorialSteps[0].gameObject.SetActive(true);
        StartCoroutine(TutorialLoop());
        
        Ball.OnKickComplete += OnKickComplete;
    }
    
    private void OnDestroy()
    {
        Ball.OnKickComplete -= OnKickComplete;
    }
    
    private void OnKickComplete(KickData kickData)
    {
        currentStep.OnKickComplete(kickData);
        if (kickData.Result == KickResult.Goal && currentStep is KickStyleTutorialStep) return;
        StartCoroutine( DelayNextBallSpawn(1f));
    }

    
    private IEnumerator TutorialLoop()
    {
        ballLauncher.KickReady();
        foreach (var tutorialStep in tutorialSteps)
        {
             currentStep = tutorialStep;
             currentStep.gameObject.SetActive(true);
             yield return currentStep.TutorialStepLoop();
             currentStep.gameObject.SetActive(false);
        }
        exitPracticeTutorial.gameObject.SetActive(true); 
        exitPracticeTutorial.SetText("Tap the home icon to exit the tutorial");
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
