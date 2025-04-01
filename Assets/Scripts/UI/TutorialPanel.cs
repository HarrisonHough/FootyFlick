using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private GameObject kickTutorial;
    [SerializeField] private GameObject windTutorial;
    [SerializeField] private GameObject kickStyleTutorial;
    [SerializeField] private GameObject tutorialComplete;
    
    public void ShowKickTutorial()
    {
        tutorialComplete.SetActive(false);
        kickTutorial.SetActive(true);
        windTutorial.SetActive(false);
        kickStyleTutorial.SetActive(false);
    }
    
    public void ShowWindTutorial()
    {
        tutorialComplete.SetActive(false);
        kickTutorial.SetActive(false);
        windTutorial.SetActive(true);
        kickStyleTutorial.SetActive(false);
    }
    
    public void ShowKickStyleTutorial()
    {
        tutorialComplete.SetActive(false);
        kickTutorial.SetActive(false);
        windTutorial.SetActive(false);
        kickStyleTutorial.SetActive(true);
    }
    
    public void ShowTutorialComplete()
    {
        kickTutorial.SetActive(false);
        windTutorial.SetActive(false);
        kickStyleTutorial.SetActive(false);
        tutorialComplete.SetActive(true);
    }
    
    public void HideAllTutorials()
    {
        kickTutorial.SetActive(false);
        windTutorial.SetActive(false);
        kickStyleTutorial.SetActive(false);
        tutorialComplete.SetActive(false);
    }
}
