using System.Collections;
using UnityEngine;

public class WindTutorialStep : TutorialStep
{
    [SerializeField]
    private WindControl windControl;
    [SerializeField]
    private WindPanel windPanel;
    
    public override IEnumerator TutorialStepLoop()
    {
        windPanel.gameObject.SetActive(true);
        windControl.SetWindStrength(3.5f);
        SetText("Adjust your kick to account for the wind!");
        goalScored = false;
        while (!goalScored)
        {
            yield return null;
        }
        
        windControl.SetWindStrength(0);
        windPanel.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
