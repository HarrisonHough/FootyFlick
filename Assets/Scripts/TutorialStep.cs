using TMPro;
using UnityEngine;

public class TutorialStep : MonoBehaviour
{ 
    [SerializeField]
    private TMP_Text text;

    [SerializeField] private GameObject[] objectsToEnable;

    private void OnEnable()
    {
        if (objectsToEnable == null) return;
        foreach (var obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
    }

    public void SetText(string newText)
    {
        text.text = newText;
    }
}
