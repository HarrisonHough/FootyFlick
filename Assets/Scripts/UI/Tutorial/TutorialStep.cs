using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialStep : MonoBehaviour, ITutorialStep
{
    [SerializeField]
    private TMP_Text text;

    [SerializeField] public GameObject[] objectsToEnable;
    protected bool ballKicked;
    protected bool goalScored;
    protected bool pointScored;
    protected bool goalScoreWithSnap;

    
    protected virtual void OnEnable()
    {
        if (objectsToEnable == null) return;
        foreach (var obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
    }

    private void OnDisable()
    {
        if (objectsToEnable == null) return;
        foreach (var obj in objectsToEnable)
        {
            obj.SetActive(false);
        }
    }

    public void SetText(string newText)
    {
        text.text = newText;
    }

    public virtual void OnKickComplete(KickData kickData)
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
    }

    public virtual IEnumerator TutorialStepLoop()
    {
        yield return null;
    }
}
