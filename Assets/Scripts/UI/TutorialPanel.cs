using System;
using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    
    public Action OnTutorialComplete;

    public void TutorialComplete()
    {
        OnTutorialComplete?.Invoke();
    }
}
