using System;
using UnityEngine;

public class GameOverPanelBase : MonoBehaviour
{
    public Action OnRetryButtonClicked;
    public Action OnHomeButtonClicked;

    public void Retry()
    {
        OnRetryButtonClicked?.Invoke();
    }
    
    public void Home()
    {
        OnHomeButtonClicked?.Invoke();
    }
}
