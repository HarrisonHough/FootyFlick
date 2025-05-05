using TMPro;
using UnityEngine;
using System.Collections;

public interface ITutorialStep
{
    void SetText(string newText);
    void OnKickComplete(KickData kickData);
    IEnumerator TutorialStepLoop();
}
