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
        AudioController.Instance.PlayMusic(AudioId.MusicLoop);
        if (GamePrefs.GetBool(GamePrefs.TUTORIAL_COMPLETE_PREFS))
        {
            SceneManager.LoadScene(2);
            yield break;
        }
        
        SceneManager.LoadScene(1);
    }
}
