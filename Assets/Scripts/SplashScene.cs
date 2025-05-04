using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScene : MonoBehaviour
{
    private float displayTime = 2f;

    private void Start()
    {
        StartCoroutine( DelayLoadScene() );
    }

    IEnumerator DelayLoadScene()
    {
        yield return new WaitForSeconds(displayTime);
        // TODO maybe make a separte bool for into tutorial
        if (GamePrefs.GetTutorialComplete(GameModeEnum.Practice))
        {
            SceneManager.LoadScene(1);
            yield break;
        }
        SceneManager.LoadScene(2);
    }
}
