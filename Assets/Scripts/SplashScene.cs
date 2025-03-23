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
        SceneManager.LoadScene(1);
    }
}
